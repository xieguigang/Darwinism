Imports System.IO
Imports System.IO.Compression
Imports System.Threading
Imports Microsoft.VisualBasic.Data.IO

Public Class BackgroundWorker

    ReadOnly buckets As Buckets

    ''' <summary>
    ''' 后台任务取消令牌，用于优雅关闭
    ''' </summary>
    Dim cts As New CancellationTokenSource
    Dim backgroundSyncIntervalMs As Integer = 5000 ' 后台同步间隔5秒
    Dim running As Boolean = False

    ''' <summary>
    ''' 后台任务用于同步的锁
    ''' </summary>
    Friend ReadOnly backgroundSyncLock As New Object()

    ''' <summary>
    ''' 缓存清理任务防重入标志: 0 = 空闲, 1 = 清理中
    ''' </summary>
    Dim cacheClearing As Integer = 0

    ''' <summary>
    ''' 清理请求去重标志: 0 = 无请求, 1 = 已排队
    ''' </summary>
    Dim clearRequested As Integer = 0

    Private Sub New(engine As Buckets)
        buckets = engine
    End Sub

    ''' <summary>
    ''' 请求执行一次冷数据清理：通过 <see cref="Interlocked.CompareExchange"/>
    ''' 去重排队，避免缓存超限期间每次读取都触发 Task.Run 的任务风暴。
    ''' </summary>
    Public Sub RequestClearColdData()
        If Interlocked.CompareExchange(clearRequested, 1, 0) = 0 Then
            Call Task.Run(Sub()
                              Try
                                  Call ClearColdDataAsync()
                              Finally
                                  Call Interlocked.Exchange(clearRequested, 0)
                              End Try
                          End Sub)
        End If
    End Sub

    Public Sub ClearColdDataAsync()
        Dim hotCache = buckets.hotCache

        ' 防止多个清理任务同时运行
        If Interlocked.CompareExchange(cacheClearing, 1, 0) = 0 Then
            Try
                If hotCache.Count > buckets.cacheLimitSize Then
                    ' 快照后在副本上按 hits 就地排序：
                    ' 相比 OrderBy+Take 的 LINQ 管线（多次中间集合分配），
                    ' 就地排序无额外分配，且快照隔离了并发修改
                    Dim snapshot As L1CacheHotData() = hotCache.Values.ToArray()
                    Call Array.Sort(snapshot, Function(a, b) a.hits.CompareTo(b.hits))

                    Dim top As Integer = CInt(buckets.cacheLimitSize * buckets.cacheClearRatio)

                    If top > snapshot.Length Then
                        top = snapshot.Length
                    End If

                    For i As Integer = 0 To top - 1
                        Dim removed As L1CacheHotData = Nothing
                        Call hotCache.TryRemove(snapshot(i).hashcode, removed)
                    Next
                End If
            Finally
                Call Interlocked.Exchange(cacheClearing, 0)
            End Try
        End If
    End Sub

    Public Shared Function Start(engine As Buckets) As BackgroundWorker
        Dim worker As New BackgroundWorker(engine)
        worker.Start()
        Return worker
    End Function

    ''' <summary>
    ''' 启动后台任务
    ''' </summary>
    Public Sub Start()
        If Not running Then
            cts = New CancellationTokenSource
            Task.Run(AddressOf BackgroundSyncWorker, cts.Token)
        End If
    End Sub

    Public Sub Cancel()
        Call cts.Cancel()
    End Sub

    Public Sub Wait()
        Do While running AndAlso App.Running
            Call Thread.Sleep(100)
        Loop
    End Sub

    ''' <summary>
    ''' 后台同步工作线程：定期将脏的索引写入磁盘，并Flush数据文件。
    ''' </summary>
    Private Sub BackgroundSyncWorker()
        running = True

        Do While Not cts.Token.IsCancellationRequested
            Try
                ' 等待指定间隔或取消信号
                Call Task.Delay(backgroundSyncIntervalMs, cts.Token).Wait()
                Call Sync()
            Catch ex As OperationCanceledException
                ' 正常退出，无需处理
                Exit Do
            Catch ex As Exception
                ' 记录日志，但保持后台任务运行
                ' Console.WriteLine($"Background sync error: {ex.Message}")
            End Try
        Loop

        running = False
    End Sub

    Private Sub Sync()
        Dim indexesToSave As Integer()
        Dim database_dir As String = buckets.database_dir
        Dim bucketWriters = buckets.bucketWriters

        ' 获取需要保存的索引列表，并清空脏标记
        SyncLock backgroundSyncLock
            indexesToSave = buckets.dirtyIndexes.ToArray()
            buckets.dirtyIndexes.Clear()
        End SyncLock

        If indexesToSave.Length > 0 Then
            Dim fileIndexes = buckets.fileIndexes

            ' 并行保存多个索引文件
            Parallel.ForEach(indexesToSave,
                 Sub(bucketId)
                     Dim index As Index = fileIndexes(bucketId)
                     Dim indexData = index.IndexValue

                     SyncLock buckets.bucketLocks(bucketId)
                         Call SaveIndex(bucketId, indexData, database_dir)
                     End SyncLock
                 End Sub)

            ' 强制Flush所有数据文件，确保数据落盘
            For Each writer In bucketWriters.Values
                Call writer.Flush()
            Next
        End If
    End Sub

    ''' <summary>
    ''' 保存单个桶的索引到文件
    ''' </summary>
    Public Shared Sub SaveIndex(bucketId As Integer, ByRef index As Dictionary(Of UInteger, BufferRegion), database_dir As String)
        Dim indexFilePath As String = Path.Combine(database_dir, $"bucket{bucketId}.index")
        Dim tempPath As String = indexFilePath & ".tmp"

        Using indexStream As New MemoryStream
            Using indexWriter As New BinaryDataWriter(indexStream, leaveOpen:=True) With {.ByteOrder = ByteOrder.LittleEndian}
                Dim lockBuffer As List(Of KeyValuePair(Of UInteger, BufferRegion))

                SyncLock index
                    lockBuffer = index.ToList
                End SyncLock

                Call indexWriter.Write(lockBuffer.Count)

                For Each entry As KeyValuePair(Of UInteger, BufferRegion) In lockBuffer
                    indexWriter.Write(entry.Key) ' hashcode
                    indexWriter.Write(entry.Value.position)
                    indexWriter.Write(entry.Value.size)
                Next

                Call indexWriter.Flush()
            End Using

#If NETCOREAPP Then
            Using compressedStream As New FileStream(tempPath, FileMode.Create, FileAccess.Write)
                ' Fastest 级别：索引保存频率高（后台定期同步），压缩吞吐优先于压缩率
                Using compressor As New BrotliStream(compressedStream, CompressionLevel.Fastest)
                    indexStream.Position = 0
                    indexStream.CopyTo(compressor) ' 压缩后写入文件
                End Using
            End Using
#Else
            Using compressedStream As New FileStream(tempPath, FileMode.Create, FileAccess.Write)
                indexStream.CopyTo(compressedStream)
            End Using
#End If
        End Using

        ' 原子替换：免去先删除旧文件的窗口期，崩溃时旧索引仍然完整可用
        Call File.Move(tempPath, indexFilePath, overwrite:=True)
    End Sub
End Class
