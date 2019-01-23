Namespace Arguments

    ''' <summary>
    ''' 共享文件夹
    ''' </summary>
    Public Class Mount

        ''' <summary>
        ''' 宿主机内的本地文件夹全路径
        ''' </summary>
        Public Property local As String
        ''' <summary>
        ''' 虚拟机内的文件路径,共享文件夹将会在虚拟机内被挂载到这个路径上面
        ''' </summary>
        Public Property virtual As String

        ''' <summary>
        ''' ``local:virtual``
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            Return $"{local.GetDirectoryFullPath}:{virtual}"
        End Function
    End Class
End Namespace