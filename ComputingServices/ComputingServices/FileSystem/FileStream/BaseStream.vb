Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Text
Imports Microsoft.VisualBasic.ComputingServices.FileSystem.Protocols
Imports Microsoft.VisualBasic.FileIO
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Protocol

Namespace FileSystem.IO

    ''' <summary>
    ''' <see cref="System.IO.FileStream"/><see cref="System.IO.StreamWriter"/>
    ''' </summary>
    Public MustInherit Class BaseStream : Inherits Stream
        Implements IDisposable

        ''' <summary>
        ''' 远端机器上面的文件系统的访问入口点
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property FileSystem As IPEndPoint

        Sub New(remote As FileSystem)
            FileSystem = remote.Portal
        End Sub

        Sub New(remote As IPEndPoint)
            FileSystem = remote
        End Sub

        Sub New(remote As String, port As Integer)
            Call Me.New(New IPEndPoint(remote, port))
        End Sub

        Sub New(remote As System.Net.IPEndPoint)
            Call Me.New(New IPEndPoint(remote))
        End Sub

        Public Overrides Function ToString() As String
            Return FileSystem.ToString
        End Function
    End Class
End Namespace