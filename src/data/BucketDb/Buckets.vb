Imports System.Buffers
Imports System.Buffers.Binary
Imports System.Collections.Concurrent
Imports System.IO
Imports System.IO.Compression
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.Repository
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Unit
Imports Microsoft.VisualBasic.Data.IO
Imports Microsoft.VisualBasic.Data.Repository
Imports Microsoft.Win32.SafeHandles

''' <summary>
''' A hashcode bucketed in-memory key-value database with persistence and optimized performance.
''' </summary>
''' <remarks>
''' 使用追加写入日志、内存索引和后台任务实现高性能持久化。
''' </remarks>
Public Class Buckets : Inherits InMemoryDb
    Implements IDisposable

    ReadOnly partitions As Integer
    ReadOnly is_readonly As Boolean = False

    Friend ReadOnly database_dir As String

    ''' <summary>
    ''' L1 Cache: 存储最近访问的数据
    ''' </summary>
    ''' <remarks>
    ''' 使用 <see cref="ConcurrentDictionary"/> 实现读路径零锁：
    ''' 读取时仅 TryGetValue + <see cref="Interlocked.Increment"/>，无需读写锁，
    ''' 极大降低高并发读取下的锁争用开销。
    ''' </remarks>
    Friend ReadOnly hotCache As New ConcurrentDictionary(Of UInteger, L1CacheHotData)

    ''' <summary>
    ''' L2 Index: 内存中的文件索引，key: bucketId, value: 该桶的索引
    ''' </summary>
    Friend ReadOnly fileIndexes As New Dictionary(Of UInteger, Index)

    ''' <summary>
    ''' 每个桶数据文件的只读句柄：配合 <see cref="RandomAccess"/> 按偏移量读取。
    ''' 显式偏移量读取不共享 Stream.Position，因此同一个桶可以被多线程并发读取，
    ''' 无需任何锁（取代旧版的 SyncLock BaseStream 串行化读）。
    ''' </summary>
    ReadOnly bucketHandles As New Dictionary(Of Integer, SafeFileHandle)()

    ' 用于写入数据文件的流
    Friend ReadOnly bucketWriters As New Dictionary(Of Integer, BinaryDataWriter)()

    ''' <summary>
    ''' 细粒度锁：为每个桶提供一个独立的锁，取代全局锁，极大提升并发写入性能。
    ''' </summary>
    Friend ReadOnly bucketLocks As Object()

    ReadOnly worker As BackgroundWorker

    ' --- 后台任务相关 ---

    ''' <summary>
    ''' 用于标记哪些桶的索引是“脏”的，需要被后台任务持久化。
    ''' </summary>
    Friend ReadOnly dirtyIndexes As New HashSet(Of Integer)()

    ' --- 配置参数 ---
    Friend cacheLimitSize As Integer
    Friend cacheClearRatio As Single = 0.5F ' 清理50%的冷数据

    ''' <summary>
    ''' 大于 1KB 的数据写入时的 Brotli 压缩级别。
    ''' </summary>
    ''' <remarks>
    ''' 默认 <see cref="CompressionLevel.Fastest"/>：压缩写入吞吐比 Optimal 高数倍，
    ''' 代价是文件体积略增（存储换速度的权衡），可通过构造函数参数回退到 Optimal。
    ''' </remarks>
    Friend compressionLevel As CompressionLevel = CompressionLevel.Fastest

    Dim enableImmediateFlush As Boolean = False ' 是否在每次写入后立即Flush到磁盘

    Private disposedValue As Boolean

    ''' <summary>
    ''' 初始化数据库
    ''' </summary>
    ''' <param name="database_dir">数据库文件存储目录</param>
    ''' <param name="buckets">桶的数量，默认为64</param>
    Sub New(database_dir As String,
            Optional cacheSize As Integer = 100000,
            Optional buckets As Integer? = Nothing,
            Optional [readonly] As Boolean = False,
            Optional in_memory As Boolean = False,
            Optional compression As CompressionLevel = CompressionLevel.Fastest)

        Dim bucketFiles = database_dir.EnumerateFiles("*.db").Count

        buckets = If(buckets Is Nothing, bucketFiles, CInt(buckets))
        buckets = If(buckets Is Nothing OrElse CInt(buckets) = 0, 64, buckets)

        Me.partitions = CInt(buckets)
        Me.database_dir = database_dir
        Me.bucketLocks = New Object(buckets) {}
        Me.cacheLimitSize = cacheSize
        Me.compressionLevel = compression
        Me.worker = BackgroundWorker.Start(Me)
        Me.is_readonly = [readonly]

        For i As Integer = 0 To bucketLocks.Length - 1
            bucketLocks(i) = New Object
        Next

        Call database_dir.MakeDir
        Call InitEngine([readonly], in_memory)
    End Sub

    Private Sub InitEngine([readonly] As Boolean, in_memory As Boolean)
        ' 初始化每个桶的读写器和索引
        For i As Integer = 1 To partitions
            Dim dataFilePath = Path.Combine(database_dir, $"bucket{i}.db")
            Dim indexFilePath = Path.Combine(database_dir, $"bucket{i}.index")
            Dim bucketId As UInteger = i

            If Not dataFilePath.FileExists Then
                ' create an empty file is not existsed
                Call New Byte() {}.FlushStream(dataFilePath)
            End If

            ' 1. 为每个桶打开一个只读句柄：
            '    配合 RandomAccess 按显式偏移量读取，不共享 Stream.Position，
            '    因此同桶多线程并发读取无需加锁（取代旧版 Shared Reader + SyncLock）
            bucketHandles(i) = File.OpenHandle(
                dataFilePath, FileMode.Open, FileAccess.Read,
                FileShare.ReadWrite, FileOptions.RandomAccess)

            If Not [readonly] Then
                ' FileAccess.Write: 写入器只需要写权限
                ' FileShare.Read: 允许其他流同时读取，这对我们的 reader 很重要
                Dim writerStream As New FileStream(dataFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read)
                bucketWriters(i) = New BinaryDataWriter(writerStream)
            End If

            ' 3. 初始化延迟加载的内存索引
            fileIndexes(i) = New Index(indexFilePath)
        Next
    End Sub

    ''' <summary>
    ''' 枚举数据库中所有的键。
    ''' 此操作会遍历所有数据文件，可能比较耗时，建议在需要时调用。
    ''' </summary>
    ''' <returns>返回一个包含所有键的字符串集合。</returns>
    Public Overrides Iterator Function EnumerateAllKeys() As IEnumerable(Of Byte())
        For i As Integer = 1 To partitions
            Dim bucketId = i
            Dim dataFilePath = Path.Combine(database_dir, $"bucket{bucketId}.db")

            If Not File.Exists(dataFilePath) Then
                Continue For
            End If

            Using readerStream As New FileStream(dataFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                Using reader As New BinaryDataReader(readerStream)
                    Dim index = fileIndexes(bucketId).IndexValue

                    For Each entry In index.Values
                        reader.Position = entry.position

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

    Public Function GetString(key As String) As String
        Return Encoding.UTF8.GetString([Get](Encoding.UTF8.GetBytes(key)))
    End Function

    ''' <summary>
    ''' 从 <paramref name="handle"/> 的 <paramref name="fileOffset"/> 处读取数据填满
    ''' <paramref name="buffer"/>。常规文件一般一次读满，循环兜底处理短读。
    ''' </summary>
    Private Shared Sub ReadFully(handle As SafeFileHandle, buffer() As Byte, bufferOffset As Integer, count As Integer, fileOffset As Long)
        Dim total As Integer = 0

        While total < count
            ' RandomAccess.Read(handle, buffer, fileOffset)：从 fileOffset 处读取填满 buffer 切片
            Dim n As Integer = RandomAccess.Read(handle, buffer.AsSpan(bufferOffset + total, count - total), fileOffset + total)

            If n <= 0 Then
                Throw New EndOfStreamException($"unexpected end of data file at offset {fileOffset + total}")
            End If

            total += n
        End While
    End Sub

    Public Overrides Function [Get](keydata As Byte()) As Byte()
        Dim hashcode As UInteger
        Dim bucketId As UInteger

        Call HashKey(keydata, hashcode, bucketId)

        ' 1. 检查热缓存（ConcurrentDictionary 无锁读，允许多线程并发）
        Dim data As L1CacheHotData = Nothing

        If hotCache.TryGetValue(hashcode, data) Then
            Call Interlocked.Increment(data.hits) ' 无锁原子递增，修复旧版读锁内修改的竞态
            Return data.data
        End If

        ' 2. 检查内存索引
        Dim index = fileIndexes(bucketId).IndexValue
        Dim entry As BufferRegion = Nothing

        If index.TryGetValue(hashcode, entry) Then
            ' 3. 使用 RandomAccess 按偏移量读取数据文件：
            '    显式偏移读取不共享 Stream.Position，无需加锁，
            '    同一个桶可以被多个线程并发读取
            Dim offset As Long = entry.position
            Dim handle As SafeFileHandle = bucketHandles(CInt(bucketId))
            Dim header(3) As Byte

            Call ReadFully(handle, header, 0, 4, offset)

            Dim valueLength As Integer = BitConverter.ToInt32(header, 0)
            Dim record As Byte() = ArrayPool(Of Byte).Shared.Rent(valueLength + 1)

            Try
                ' 记录格式: [valueLen(4)][valueData][compress(1)]
                ' 一次读取数据体和压缩标志字节
                Call ReadFully(handle, record, 0, valueLength + 1, offset + 4)

                Dim compress As Boolean = record(valueLength) <> 0
                Dim dataBytes As Byte()

                If compress Then
#If NET48 Then
                    Throw New NotSupportedException("decompression of brotli stream is not supported in .net 4.8 runtime!")
#Else
                    Using compressedStream As New MemoryStream(record, 0, valueLength)
                        Using brotliStream As New BrotliStream(compressedStream, CompressionMode.Decompress)
                            Using resultStream As New MemoryStream()
                                Call brotliStream.CopyTo(resultStream)
                                dataBytes = resultStream.ToArray() ' 就是解压后的原始数据
                            End Using
                        End Using
                    End Using
#End If
                Else
                    dataBytes = New Byte(valueLength - 1) {}
                    Buffer.BlockCopy(record, 0, dataBytes, 0, valueLength)
                End If

                ' 4. 更新热缓存（TryAdd: 可能在等待期间已被其他线程添加，幂等）
                Call hotCache.TryAdd(hashcode, New L1CacheHotData With {
                    .bucket = bucketId,
                    .data = dataBytes,
                    .hashcode = hashcode,
                    .hits = 1
                })

                ' 5. 异步触发缓存清理（去重排队，避免任务风暴），不阻塞读取
                If hotCache.Count > cacheLimitSize Then
                    Call worker.RequestClearColdData()
                End If

                Return dataBytes
            Finally
                Call ArrayPool(Of Byte).Shared.Return(record)
            End Try
        End If

        ' 如果缓存和索引都没有找到，说明key不存在
        Return Nothing
    End Function

    Public Overloads Sub Put(key As String, data As String)
        Call Put(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(data))
    End Sub

    Public Overrides Sub Put(keybuf As Byte(), data As Byte())
        Dim hashcode As UInteger
        Dim bucketId As UInteger

        Call HashKey(keybuf, hashcode, bucketId)
        Dim bucketIdInt = CInt(bucketId)

        ' 使用细粒度锁，只锁定当前操作的桶
        SyncLock bucketLocks(bucketIdInt)
            ' 1. 更新热缓存
            Dim L1data As L1CacheHotData = Nothing

            If hotCache.TryGetValue(hashcode, L1data) Then
                L1data.data = data
            End If

            ' skip of write data file in readonly mode
            If is_readonly Then
                Return
            End If

            ' 2. 准备写入数据文件
            Dim bucketWriter As BinaryDataWriter = bucketWriters(bucketIdInt)
            Dim offset As Long = bucketWriter.BaseStream.Length
            Dim compress As Byte = 0

#If NETCOREAPP Then
            If data.Length > ByteSize.KB Then
                compress = 1

                Using originalStream As New MemoryStream(data)
                    Using compressedStream As New MemoryStream()
                        Using brotliStream As New BrotliStream(compressedStream, compressionLevel)
                            Call originalStream.CopyTo(brotliStream)
                        End Using

                        data = compressedStream.ToArray()
                    End Using
                End Using
            End If
#End If

            ' 3. 在桶锁内组包后单次写入：
            '    记录格式: [valueLen(4)][valueData][compress(1)][keyLen(4)][keyData]
            '    单次 Write 取代 5 次小写入，减少 IO 系统调用次数并缩短桶锁持有时间
            Dim recordSize As Integer = 4 + data.Length + 1 + 4 + keybuf.Length
            Dim record As Byte() = ArrayPool(Of Byte).Shared.Rent(recordSize)

            Try
                Call BinaryPrimitives.WriteInt32LittleEndian(record.AsSpan(0, 4), data.Length)
                Call Buffer.BlockCopy(data, 0, record, 4, data.Length)
                record(4 + data.Length) = compress
                Call BinaryPrimitives.WriteInt32LittleEndian(record.AsSpan(5 + data.Length, 4), keybuf.Length)
                Call Buffer.BlockCopy(keybuf, 0, record, 9 + data.Length, keybuf.Length)

                ' 将写入器指针移动到文件末尾
                Call bucketWriter.Seek(0, SeekOrigin.End)
                Call bucketWriter.Write(record, 0, recordSize)

                If enableImmediateFlush Then
                    Call bucketWriter.Flush() ' 性能影响大，但数据安全性最高
                End If
            Finally
                Call ArrayPool(Of Byte).Shared.Return(record)
            End Try

            ' 4. 更新内存索引，size是整个记录的大小
            Dim index = fileIndexes(bucketIdInt).IndexValue
            index(hashcode) = New BufferRegion(offset, recordSize)

            ' 5. 标记索引为“脏”，通知后台任务需要持久化
            SyncLock worker.backgroundSyncLock
                dirtyIndexes.Add(bucketIdInt)
            End SyncLock
        End SyncLock
    End Sub

    Private Sub HashKey(ByRef key As Byte(), <Out> ByRef hashcode As UInteger, <Out> ByRef bucket As UInteger)
        hashcode = MurmurHash.MurmurHashCode3_x86_32(key, &HFFFFFFFFUI)
        bucket = (hashcode Mod CUInt(partitions)) + 1 ' bucket id start from 1
    End Sub

    Public Sub Flush()
        If is_readonly Then
            Return
        End If

        ' 1. 取消后台任务
        Call worker.Cancel()
        Call worker.Wait()
        ' 等待后台任务完成最后一次循环并退出
        ' 这里可以加一个超时，例如 Task.Delay(1000).Wait()
        ' 但为了简单，我们假设它会很快退出

        ' 3. Flush并释放所有文件流
        For Each writer In bucketWriters.Values
            Call writer.Flush()
        Next

        ' 保存所有索引（包括脏的和干净的，确保最终状态一致）
        For i As Integer = 1 To partitions
            Call BackgroundWorker.SaveIndex(i, fileIndexes(i).IndexValue, database_dir)
        Next
    End Sub

    Public Overrides Function HasKey(keydata() As Byte) As Boolean
        Dim hashcode As UInteger
        Dim bucketId As UInteger

        Call HashKey(keydata, hashcode, bucketId)

        ' 2. 检查内存索引
        Dim index = fileIndexes(bucketId).IndexValue

        Return index.ContainsKey(hashcode)
    End Function

    Protected Overrides Sub Close()
        If Not is_readonly Then
            Call Console.WriteLine("Saving indexes before disposing...")
            Call Flush()
        End If

        ' 3. Flush并释放所有文件流与读取句柄
        For Each writer In bucketWriters.Values
            writer.BaseStream.Dispose()
        Next
        For Each handle In bucketHandles.Values
            handle.Dispose()
        Next

        ' 4. 清理集合
        bucketHandles.Clear()
        bucketWriters.Clear()
        fileIndexes.Clear()
        hotCache.Clear()
    End Sub
End Class
