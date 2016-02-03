Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Text
Imports Microsoft.VisualBasic.ComputingServices.FileSystem.Protocols
Imports Microsoft.VisualBasic.FileIO
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Protocol

Namespace FileSystem

    Public MustInherit Class BaseStream

        Public ReadOnly Property FileSystem As FileSystem
        Public Property FileName As String
            Get
                Return _file
            End Get
            Protected Set(value As String)
                _file = value
            End Set
        End Property

        Dim _file As String

        Sub New(remote As FileSystem)
            FileSystem = remote
        End Sub

        Sub New(remote As IPEndPoint)
            Call Me.New(New FileSystem(remote))
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