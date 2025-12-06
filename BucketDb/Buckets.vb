Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading
Imports Microsoft.VisualBasic.Data.IO
Imports Microsoft.VisualBasic.Data.Repository

''' <summary>
''' A hashcode bucketed in-memory key-value database with persistence and optimized performance.
''' </summary>
''' <remarks>
''' 使用追加写入日志、内存索引和后台任务实现高性能持久化。
''' </remarks>
Public Class Buckets : Implements IDisposable

    ReadOnly partitions As Integer
    Friend ReadOnly database_dir As String

    ''' <summary>
    ''' L1 Cache: 存储最近访问的数据
    ''' </summary>
    Friend ReadOnly hotCache As New Dictionary(Of UInteger, L1CacheHotData)
    ' 用于同步 hotCache 的访问
    Friend ReadOnly hotCacheLock As New ReaderWriterLockSlim()

    ' L2 Index: 内存中的文件索引，key: bucketId, value: 该桶的索引
    ' 索引项: key: hashcode, value: (offset, size)
    Friend ReadOnly fileIndexes As New Dictionary(Of Integer, Lazy(Of Dictionary(Of UInteger, (offset As Long, size As Integer))))()

    ' 用于读取数据文件的流
    ReadOnly bucketReaders As New Dictionary(Of Integer, BinaryDataReader)()
    ' 用于写入数据文件的流
    Friend ReadOnly bucketWriters As New Dictionary(Of Integer, BinaryDataWriter)()

    ''' <summary>
    ''' 细粒度锁：为每个桶提供一个独立的锁，取代全局锁，极大提升并发写入性能。
    ''' </summary>
    ReadOnly bucketLocks As Object()

    ReadOnly worker As BackgroundWorker

    ' --- 后台任务相关 ---

    ''' <summary>
    ''' 用于标记哪些桶的索引是“脏”的，需要被后台任务持久化。
    ''' </summary>
    Friend ReadOnly dirtyIndexes As New HashSet(Of Integer)()

    ' --- 配置参数 ---
    Friend cacheLimitSize As Integer
    Friend cacheClearRatio As Single = 0.5F ' 清理50%的冷数据

    Dim enableImmediateFlush As Boolean = False ' 是否在每次写入后立即Flush到磁盘

    Private disposedValue As Boolean

    ''' <summary>
    ''' 初始化数据库
    ''' </summary>
    ''' <param name="database_dir">数据库文件存储目录</param>
    ''' <param name="partitions">桶的数量，默认为64</param>
    Sub New(database_dir As String, Optional partitions As Integer = 64, Optional cacheSize As Integer = 100000)
        Me.partitions = partitions
        Me.database_dir = database_dir
        Me.bucketLocks = New Object(partitions) {}
        Me.cacheLimitSize = cacheSize
        Me.worker = New BackgroundWorker(Me)

        For i As Integer = 0 To bucketLocks.Length - 1
            bucketLocks(i) = New Object
        Next

        Call database_dir.MakeDir

        ' 初始化每个桶的读写器和索引
        For i As Integer = 1 To partitions
            Dim dataFilePath = Path.Combine(database_dir, $"bucket{i}.db")
            Dim indexFilePath = Path.Combine(database_dir, $"bucket{i}.index")

            ' 1. 初始化数据文件读写器
            ' FileMode.OpenOrCreate: 文件存在则打开，不存在则创建
            ' FileAccess.Read: 读取器只需要读权限
            Dim readerStream As New FileStream(dataFilePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite)
            bucketReaders(i) = New BinaryDataReader(readerStream)

            ' FileAccess.Write: 写入器只需要写权限
            ' FileShare.Read: 允许其他流同时读取，这对我们的 reader 很重要
            Dim writerStream As New FileStream(dataFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read)
            bucketWriters(i) = New BinaryDataWriter(writerStream)

            ' 3. 初始化延迟加载的内存索引
            ' Lazy(Of T) 确保索引只被加载一次，且在首次访问时才加载
            Dim bucketId = i
            fileIndexes(i) = New Lazy(Of Dictionary(Of UInteger, (offset As Long, size As Integer)))(
                Function()
                    Dim index As New Dictionary(Of UInteger, (offset As Long, size As Integer))()
                    LoadIndex(bucketId, indexFilePath, index)
                    Return index
                End Function,
                LazyThreadSafetyMode.ExecutionAndPublication ' 确保线程安全
            )
        Next
    End Sub

    ''' <summary>
    ''' 枚举数据库中所有的键。
    ''' 此操作会遍历所有数据文件，可能比较耗时，建议在需要时调用。
    ''' </summary>
    ''' <returns>返回一个包含所有键的字符串集合。</returns>
    Public Iterator Function EnumerateAllKeys() As IEnumerable(Of Byte())
        For i As Integer = 1 To partitions
            Dim bucketId = i
            Dim dataFilePath = Path.Combine(database_dir, $"bucket{bucketId}.db")

            If Not File.Exists(dataFilePath) Then
                Continue For
            End If

            Using readerStream As New FileStream(dataFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                Using reader As New BinaryDataReader(readerStream)
                    Dim index = fileIndexes(bucketId).Value

                    For Each entry In index.Values
                        reader.Position = entry.offset

                        ' 数据块格式: [ValueLen][ValueData][KeyLen][KeyData]
                        ' 1. 读取并跳过Value部分
                        Dim valueLength As Integer = reader.ReadInt32()
                        reader.BaseStream.Seek(valueLength, SeekOrigin.Current) ' 跳过ValueData

                        ' 2. 读取Key部分
                        Dim keyLength As Integer = reader.ReadInt32()
                        Dim keyBytes = reader.ReadBytes(keyLength)

                        Yield keyBytes
                    Next
                End Using
            End Using
        Next
    End Function

    ''' <summary>
    ''' 从索引文件加载索引到内存
    ''' </summary>
    Private Shared Sub LoadIndex(bucketId As Integer, indexFilePath As String, ByRef index As Dictionary(Of UInteger, (offset As Long, size As Integer)))
        If Not File.Exists(indexFilePath) OrElse New FileInfo(indexFilePath).Length = 0 Then
            Return
        End If

        Using indexStream As New FileStream(indexFilePath, FileMode.Open, FileAccess.Read)
            Using indexReader As New BinaryDataReader(indexStream)
                Dim count As Integer = indexReader.ReadInt32()
                For j As Integer = 0 To count - 1
                    Dim hashcode As UInteger = indexReader.ReadUInt32()
                    Dim offset As Long = indexReader.ReadInt64()
                    Dim size As Integer = indexReader.ReadInt32()
                    index(hashcode) = (offset, size)
                Next
            End Using
        End Using
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function [Get](key As String) As Byte()
        Return [Get](Encoding.UTF8.GetBytes(key))
    End Function

    Public Function [Get](keydata As Byte()) As Byte()
        Dim hashcode As UInteger
        Dim bucketId As UInteger

        Call HashKey(keydata, hashcode, bucketId)

        ' 1. 检查热缓存 (使用读写锁，允许多个读并发)
        hotCacheLock.EnterReadLock()
        Try
            Dim data As L1CacheHotData = Nothing
            If hotCache.TryGetValue(hashcode, data) Then
                data.hits += 1
                Return data.data
            End If
        Finally
            hotCacheLock.ExitReadLock()
        End Try

        ' 2. 检查内存索引
        Dim index = fileIndexes(bucketId).Value
        Dim entry As (offset As Long, size As Integer)

        If index.TryGetValue(hashcode, entry) Then
            ' 从索引中找到偏移量和大小
            Dim offset As Long = entry.offset
            ' 3. 从数据文件读取
            Dim bucketReader As BinaryDataReader = bucketReaders(CInt(bucketId))

            SyncLock bucketReader.BaseStream ' 对单个流进行同步，防止并发读取时Position混乱
                bucketReader.Position = offset

                Dim valueLength As Integer = bucketReader.ReadInt32()
                Dim dataBytes = bucketReader.ReadBytes(valueLength)

                ' 4. 更新热缓存
                hotCacheLock.EnterWriteLock()
                Try
                    ' 再次检查，可能在等待锁的过程中已被其他线程添加
                    If Not hotCache.ContainsKey(hashcode) Then
                        hotCache(hashcode) = New L1CacheHotData With {
                            .bucket = bucketId,
                            .data = dataBytes,
                            .hashcode = hashcode,
                            .hits = 1
                        }
                    End If
                Finally
                    hotCacheLock.ExitWriteLock()
                End Try

                ' 5. 异步触发缓存清理，避免阻塞读取
                If hotCache.Count > cacheLimitSize Then
                    Task.Run(AddressOf worker.ClearColdDataAsync)
                End If

                Return dataBytes
            End SyncLock
        End If

        ' 如果缓存和索引都没有找到，说明key不存在
        Return Nothing
    End Function

    Public Sub Put(keybuf As Byte(), data As Byte())
        Dim hashcode As UInteger
        Dim bucketId As UInteger

        Call HashKey(keybuf, hashcode, bucketId)
        Dim bucketIdInt = CInt(bucketId)

        ' 使用细粒度锁，只锁定当前操作的桶
        SyncLock bucketLocks(bucketIdInt)
            ' 1. 更新热缓存
            hotCacheLock.EnterWriteLock()
            Try
                Dim L1data As L1CacheHotData = Nothing
                If hotCache.TryGetValue(hashcode, L1data) Then
                    L1data.data = data
                End If
            Finally
                hotCacheLock.ExitWriteLock()
            End Try

            ' 2. 准备写入数据文件
            Dim bucketWriter As BinaryDataWriter = bucketWriters(bucketIdInt)
            Dim offset As Long = bucketWriter.BaseStream.Length

            ' 将写入器指针移动到文件末尾
            bucketWriter.Seek(0, SeekOrigin.End)

            ' 3. 写入数据 (格式: [数据长度(4字节)][数据内容(N字节)])
            bucketWriter.Write(data.Length)
            bucketWriter.Write(data)
            bucketWriter.Write(keybuf.Length)
            bucketWriter.Write(keybuf)

            If enableImmediateFlush Then
                bucketWriter.Flush() ' 性能影响大，但数据安全性最高
            End If

            ' 3. 更新内存索引，size是整个记录的大小
            Dim recordSize = 4 + data.Length + 4 + keybuf.Length
            Dim index = fileIndexes(bucketIdInt).Value
            index(hashcode) = (offset, recordSize)

            ' 4. 标记索引为“脏”，通知后台任务需要持久化
            SyncLock worker.backgroundSyncLock
                dirtyIndexes.Add(bucketIdInt)
            End SyncLock
        End SyncLock
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Sub Put(key As String, data As Byte())
        Call Put(Encoding.UTF8.GetBytes(key), data)
    End Sub

    Private Sub HashKey(ByRef key As Byte(), <Out> ByRef hashcode As UInteger, <Out> ByRef bucket As UInteger)
        hashcode = MurmurHash.MurmurHashCode3_x86_32(key, &HFFFFFFFFUI)
        bucket = (hashcode Mod CUInt(partitions)) + 1 ' bucket id start from 1
    End Sub

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects)
                ' 释放托管资源
                ' 在释放前，最重要的一步是保存索引！
                Console.WriteLine("Saving indexes before disposing...")
                ' 1. 取消后台任务
                worker.Cancel()
                ' 等待后台任务完成最后一次循环并退出
                ' 这里可以加一个超时，例如 Task.Delay(1000).Wait()
                ' 但为了简单，我们假设它会很快退出

                ' 2. 强制执行最后一次完整的同步
                ' 获取所有脏索引
                Dim allDirtyIndexes As Integer()
                SyncLock worker.backgroundSyncLock
                    allDirtyIndexes = dirtyIndexes.ToArray()
                End SyncLock

                ' 保存所有索引（包括脏的和干净的，确保最终状态一致）
                For i As Integer = 1 To partitions
                    BackgroundWorker.SaveIndex(i, fileIndexes(i).Value, database_dir)
                Next

                ' 3. Flush并释放所有文件流
                For Each writer In bucketWriters.Values
                    writer.Flush()
                    writer.BaseStream.Dispose()
                Next
                For Each reader In bucketReaders.Values
                    reader.BaseStream.Dispose()
                Next

                ' 4. 释放锁
                hotCacheLock.Dispose()

                ' 5. 清理集合
                bucketReaders.Clear()
                bucketWriters.Clear()
                fileIndexes.Clear()
                hotCache.Clear()
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override finalizer
            ' TODO: set large fields to null
            disposedValue = True
        End If
    End Sub

    ' ' TODO: override finalizer only if 'Dispose(disposing As Boolean)' has code to free unmanaged resources
    ' Protected Overrides Sub Finalize()
    '     ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
    '     Dispose(disposing:=False)
    '     MyBase.Finalize()
    ' End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub
End Class
