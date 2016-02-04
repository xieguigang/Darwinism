Imports System.IO
Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Protocol
Imports Microsoft.VisualBasic.Net.Protocol.Reflection
Imports Microsoft.VisualBasic.Serialization

Namespace FileSystem.Protocols

    Public Module NetTransfer

        ''' <summary>
        ''' 从本地上传文件
        ''' </summary>
        ''' <param name="local"></param>
        ''' <param name="destination"></param>
        ''' <param name="remote"></param>
        Public Sub Upload(local As String, destination As String, remote As IPEndPoint)
            Using file As New IO.FileStream(destination, FileMode.OpenOrCreate, remote)
                Dim localFile As New FileStream(local, FileMode.Open)
                Call Transfer(localFile, file, 1024 * 1024)
            End Using
        End Sub

        Public Sub Transfer(source As Stream, target As Stream, bufLen As Integer)
            Dim buffer As Byte() = New Byte(bufLen - 1) {}

            Do While source.Position < source.Length
                Dim d As Integer = (source.Length - source.Position) - buffer.Length

                If d < 0 Then ' 注意：d 是负值的
                    buffer = New Byte(-d - 1) {}
                End If

                Call source.Read(buffer, Scan0, buffer.Length)
                Call target.Write(buffer, Scan0, buffer.Length)
                Call target.Flush()
            Loop
        End Sub

        Public Sub Download(destination As String, local As String, remote As IPEndPoint)
            Using file As New IO.FileStream(destination, FileMode.OpenOrCreate, remote)
                Dim localFile As New FileStream(local, FileMode.OpenOrCreate)
                Call Transfer(file, localFile, 1024 * 1024)
            End Using
        End Sub
    End Module
End Namespace