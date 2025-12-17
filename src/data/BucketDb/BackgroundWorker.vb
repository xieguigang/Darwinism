Imports System.IO
Imports System.Threading
Imports Microsoft.VisualBasic.Data.IO

Public Class BackgroundWorker

    ReadOnly buckets As Buckets
    ''' <summary>
    ''' 后台任务取消令牌，用于优雅关闭
    ''' </summary>
    ReadOnly cts As New CancellationTokenSource

    Dim backgroundSyncIntervalMs As Integer = 5000 ' 后台同步间隔5秒

    ''' <summary>
    ''' 后台任务用于同步的锁
    ''' </summary>
    Friend ReadOnly backgroundSyncLock As New Object()

    Private Sub New(engine As Buckets)
        buckets = engine
    End Sub

    Public Sub ClearColdDataAsync()
        Dim hotCache = buckets.hotCache

        ' 防止多个清理任务同时运行
        If Monitor.TryEnter(buckets.hotCacheLock) Then
            Try
                If hotCache.Count > buckets.cacheLimitSize Then
                    Dim top = CInt(buckets.cacheLimitSize * buckets.cacheClearRatio)
                    ' 注意：OrderBy会创建快照，所以在锁内操作是安全的
                    Dim coldHashset = hotCache.Values _
                        .OrderBy(Function(a) a.hits) _
                        .Take(top) _
                        .Select(Function(a) a.hashcode) _
                        .ToArray()

                    For Each hashcode As UInteger In coldHashset
                        Call hotCache.Remove(hashcode)
                    Next
                End If
            Finally
                Monitor.Exit(buckets.hotCacheLock)
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
        Call cts.Cancel()
        Call Task.Run(AddressOf BackgroundSyncWorker, cts.Token)
    End Sub

    Public Sub Cancel()
        Call cts.Cancel()
    End Sub

    ''' <summary>
    ''' 后台同步工作线程：定期将脏的索引写入磁盘，并Flush数据文件。
    ''' </summary>
    Private Sub BackgroundSyncWorker()
        Do While Not cts.Token.IsCancellationRequested
            Try
                ' 等待指定间隔或取消信号
                Task.Delay(backgroundSyncIntervalMs, cts.Token).Wait()

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

            Catch ex As OperationCanceledException
                ' 正常退出，无需处理
                Exit Do
            Catch ex As Exception
                ' 记录日志，但保持后台任务运行
                ' Console.WriteLine($"Background sync error: {ex.Message}")
            End Try
        Loop
    End Sub

    ''' <summary>
    ''' 保存单个桶的索引到文件
    ''' </summary>
    Public Shared Sub SaveIndex(bucketId As Integer, ByRef index As Dictionary(Of UInteger, BufferRegion), database_dir As String)
        Dim indexFilePath As String = Path.Combine(database_dir, $"bucket{bucketId}.index")
        Dim tempPath As String = indexFilePath & ".tmp"

        Using indexStream As New FileStream(tempPath, FileMode.Create, FileAccess.Write)
            Using indexWriter As New BinaryDataWriter(indexStream)
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
        End Using

        If File.Exists(indexFilePath) Then
            File.Delete(indexFilePath)
        End If

        File.Move(tempPath, indexFilePath)
    End Sub
End Class
