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
    ReadOnly database_dir As String

    ''' <summary>
    ''' L1 Cache: 存储最近访问的数据
    ''' </summary>
    ReadOnly hotCache As New Dictionary(Of UInteger, HotData)
    ' 用于同步 hotCache 的访问
    ReadOnly hotCacheLock As New ReaderWriterLockSlim()

    ' L2 Index: 内存中的文件索引，key: bucketId, value: 该桶的索引
    ' 索引项: key: hashcode, value: (offset, size)
    ReadOnly fileIndexes As New Dictionary(Of Integer, Lazy(Of Dictionary(Of UInteger, (offset As Long, size As Integer))))()

    ' 用于读取数据文件的流
    ReadOnly bucketReaders As New Dictionary(Of Integer, BinaryDataReader)()
    ' 用于写入数据文件的流
    ReadOnly bucketWriters As New Dictionary(Of Integer, BinaryDataWriter)()

    ''' <summary>
    ''' 细粒度锁：为每个桶提供一个独立的锁，取代全局锁，极大提升并发写入性能。
    ''' </summary>
    ReadOnly bucketLocks As Object()

    ' --- 后台任务相关 ---

    ''' <summary>
    ''' 用于标记哪些桶的索引是“脏”的，需要被后台任务持久化。
    ''' </summary>
    ReadOnly dirtyIndexes As New HashSet(Of Integer)()

    ''' <summary>
    ''' 后台任务用于同步的锁
    ''' </summary>
    ReadOnly backgroundSyncLock As New Object()

    ''' <summary>
    ''' 后台任务取消令牌，用于优雅关闭
    ''' </summary>
    ReadOnly cts As CancellationTokenSource

    ' --- 配置参数 ---
    Dim cacheLimitSize As Integer
    Dim cacheClearRatio As Single = 0.5F ' 清理50%的冷数据
    Dim backgroundSyncIntervalMs As Integer = 5000 ' 后台同步间隔5秒
    Dim enableImmediateFlush As Boolean = False ' 是否在每次写入后立即Flush到磁盘

    Private disposedValue As Boolean

    ''' <summary>
    ''' 初始化数据库
    ''' </summary>
    ''' <param name="database_dir">数据库文件存储目录</param>
    ''' <param name="partitions">桶的数量，默认为64</param>
    Sub New(database_dir As String, Optional partitions As Integer = 64, Optional cacheSize As Integer = 10000)
        Me.partitions = partitions
        Me.database_dir = database_dir
        Me.bucketLocks = New Object(partitions) {}
        Me.cacheLimitSize = cacheSize
        Me.cts = New CancellationTokenSource()

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

        ' 启动后台任务
        Task.Run(AddressOf BackgroundSyncWorker, cts.Token)
    End Sub

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

    ''' <summary>
    ''' 将内存中的索引保存到文件
    ''' </summary>
    Public Sub SaveAllIndexes()
        For i As Integer = 1 To partitions
            SaveIndex(i)
        Next
    End Sub

    ''' <summary>
    ''' 保存单个桶的索引到文件
    ''' </summary>
    Private Shared Sub SaveIndex(bucketId As Integer, index As Dictionary(Of UInteger, (offset As Long, size As Integer)), database_dir As String)
        Dim indexFilePath = Path.Combine(database_dir, $"bucket{bucketId}.index")
        Dim tempPath = indexFilePath & ".tmp"

        Using indexStream As New FileStream(tempPath, FileMode.Create, FileAccess.Write)
            Using indexWriter As New BinaryDataWriter(indexStream)
                indexWriter.Write(index.Count)
                For Each entry In index
                    indexWriter.Write(entry.Key) ' hashcode
                    indexWriter.Write(entry.Value.offset)
                    indexWriter.Write(entry.Value.size)
                Next
                indexWriter.Flush()
            End Using
        End Using

        If File.Exists(indexFilePath) Then File.Delete(indexFilePath)
        File.Move(tempPath, indexFilePath)
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
            Dim data As HotData = Nothing
            If hotCache.TryGetValue(hashcode, data) Then
                data.hits += 1
                Return data.data
            End If
        Finally
            hotCacheLock.ExitReadLock()
        End Try

        ' 2. 检查内存索引
        Dim index = fileIndexes(CInt(bucketId))
        Dim entry As (offset As Long, size As Integer)

        If index.TryGetValue(hashcode, entry) Then
            ' 从索引中找到偏移量和大小
            Dim offset As Long = entry.offset
            Dim bufSize As Integer = entry.size

            ' 3. 从数据文件读取
            Dim bucketReader As BinaryDataReader = bucketReaders(CInt(bucketId))
            bucketReader.Position = offset

            Dim data As New HotData With {
                .bucket = bucketId,
                .data = bucketReader.ReadBytes(bufSize),
                .hashcode = hashcode,
                .hits = 1
            }
            Call hotCache.Add(hashcode, data)

            If hotCache.Count > cacheLimitSize Then
                Call ClearColdData()
            End If

            Return data.data
        End If

        ' 如果缓存和索引都没有找到，说明key不存在
        Return Nothing
    End Function

    Private Sub ClearColdData()
        Dim top As Integer = CInt(cacheLimitSize * 0.5)
        Dim coldHashset = hotCache.Values.OrderBy(Function(a) a.hits).Take(top).ToArray

        SyncLock _syncLock
            For Each colddata As HotData In coldHashset
                Call hotCache.Remove(colddata.hashcode)
            Next
        End SyncLock
    End Sub

    Public Sub Put(keybuf As Byte(), data As Byte())
        Dim hashcode As UInteger
        Dim bucketId As UInteger

        Call HashKey(keybuf, hashcode, bucketId)
        Dim bucketIdInt = CInt(bucketId)

        SyncLock _syncLock
            Dim hotData As HotData = Nothing

            ' 1. 更新热缓存
            If hotCache.TryGetValue(hashcode, hotData) Then
                hotData.data = data
            End If

            ' 2. 准备写入数据文件
            Dim bucketWriter As BinaryDataWriter = bucketWriters(bucketIdInt)
            Dim offset As Long = bucketWriter.BaseStream.Length

            ' 将写入器指针移动到文件末尾
            bucketWriter.Seek(0, SeekOrigin.End)

            ' 3. 写入数据 (格式: [数据长度(4字节)][数据内容(N字节)])
            bucketWriter.Write(data.Length)
            bucketWriter.Write(data)
            ' bucketWriter.Flush() ' 确保数据写入磁盘

            ' 4. 更新内存索引
            fileIndexes(bucketIdInt)(hashcode) = (offset, data.Length)
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

    Public Class HotData

        Public hashcode As UInteger
        Public bucket As UInteger
        Public hits As Integer
        Public data As Byte()

    End Class

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects)
                ' 释放托管资源
                ' 在释放前，最重要的一步是保存索引！
                Console.WriteLine("Saving indexes before disposing...")
                SaveAllIndexes()

                For Each reader In bucketReaders.Values
                    reader.BaseStream.Dispose()
                Next
                For Each writer In bucketWriters.Values
                    writer.Flush()
                    writer.BaseStream.Dispose()
                Next
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
