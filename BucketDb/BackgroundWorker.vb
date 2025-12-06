Imports System.Threading

Public Class BackgroundWorker

    ReadOnly buckets As Buckets

    Sub New(engine As Buckets)
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
End Class
