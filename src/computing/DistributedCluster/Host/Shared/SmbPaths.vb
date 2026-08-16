Imports System.IO

Namespace ClusterShared

    ''' <summary>
    ''' SMB 共享文件系统的路径约定工具。
    ''' 布局：{smbRoot}/jobs/{jobId}/{blocks,results,logs}/{guid}
    ''' </summary>
    Public Class SmbPaths

        ''' <summary>
        ''' SMB 挂载根目录（例如 /mnt/cluster 或 \\\\host\share）。
        ''' </summary>
        Public ReadOnly Property Root As String

        Sub New(smbRoot As String)
            Me.Root = smbRoot.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
        End Sub

        ''' <summary>
        ''' 任务根目录：{root}/jobs/{jobId}
        ''' </summary>
        Public Function JobRoot(jobId As String) As String
            Return Path.Combine(Root, "jobs", jobId)
        End Function

        ''' <summary>
        ''' 数据块目录：{root}/jobs/{jobId}/blocks
        ''' </summary>
        Public Function BlocksDir(jobId As String) As String
            Return Path.Combine(JobRoot(jobId), "blocks")
        End Function

        ''' <summary>
        ''' 结果目录：{root}/jobs/{jobId}/results
        ''' </summary>
        Public Function ResultsDir(jobId As String) As String
            Return Path.Combine(JobRoot(jobId), "results")
        End Function

        ''' <summary>
        ''' 日志目录：{root}/jobs/{jobId}/logs
        ''' </summary>
        Public Function LogsDir(jobId As String) As String
            Return Path.Combine(JobRoot(jobId), "logs")
        End Function

        ''' <summary>
        ''' 数据块文件路径：{root}/jobs/{jobId}/blocks/{guid}
        ''' </summary>
        Public Function BlockPath(jobId As String, guid As String) As String
            Return Path.Combine(BlocksDir(jobId), guid)
        End Function

        ''' <summary>
        ''' 结果文件路径：{root}/jobs/{jobId}/results/{guid}
        ''' </summary>
        Public Function ResultPath(jobId As String, guid As String) As String
            Return Path.Combine(ResultsDir(jobId), guid)
        End Function

        ''' <summary>
        ''' 日志文件路径：{root}/jobs/{jobId}/logs/{guid}.log
        ''' </summary>
        Public Function LogPath(jobId As String, guid As String) As String
            Return Path.Combine(LogsDir(jobId), guid & ".log")
        End Function

        ''' <summary>
        ''' 确保任务相关的所有子目录都存在。
        ''' </summary>
        Public Sub EnsureJobDirs(jobId As String)
            For Each jobDir In {BlocksDir(jobId), ResultsDir(jobId), LogsDir(jobId)}
                If Not System.IO.Directory.Exists(jobDir) Then
                    Call System.IO.Directory.CreateDirectory(jobDir)
                End If
            Next
        End Sub
    End Class
End Namespace
