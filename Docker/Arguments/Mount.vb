Imports System.Runtime.CompilerServices

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
        ''' 任何一个路径是空字符串就会被判断为无效参数, 对于<see cref="local"/>本地文件夹路径
        ''' 还需要其存在于本地文件系统之上
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property IsValid As Boolean
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return Not local.StringEmpty AndAlso Not virtual.StringEmpty
            End Get
        End Property

        ''' <summary>
        ''' ``local:virtual``
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            Return $"{local.GetDirectoryFullPath}:{virtual}"
        End Function
    End Class
End Namespace