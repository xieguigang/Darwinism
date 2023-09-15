Imports System.IO
Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel.Ranges
Imports Microsoft.VisualBasic.Data.GraphTheory
Imports Microsoft.VisualBasic.DataStorage.HDSPack.FileSystem

''' <summary>
''' Resource query language
''' </summary>
Public Class Resource : Implements IDisposable

    ReadOnly buf As StreamPack
    ReadOnly index As Trie(Of NodeMap)

    Private disposedValue As Boolean

    Sub New(res As StreamPack)
        Dim indexfile = res.OpenFile("/index.dat", FileMode.OpenOrCreate, FileAccess.Read)
        Dim parser As New IndexReader(indexfile)

        buf = res
        index = parser.Read
    End Sub

    Public Function Add(key As String, str As String)
        Return Add(key, Encoding.UTF8.GetBytes(str))
    End Function

    Public Function ReadString(map As String) As String
        Dim path As String = URL(map)
        Dim file As Stream = buf.OpenFile(path, FileMode.Open, FileAccess.Read)
        Dim bytes As Byte() = New Byte(file.Length - 1) {}
        Call file.Read(bytes, Scan0, bytes.Length)
        Return Encoding.UTF8.GetString(bytes)
    End Function

    Private Shared Function URL(map As String) As String
        Return $"/pool/{map.Substring(4, 2)}/{map.Substring(16, 6)}/{map}"
    End Function

    Public Function Add(key As String, data As Byte()) As Boolean
        Dim tokens As String() = Strings.LCase(key).Split
        Dim map As String = key.MD5
        Dim path As String = URL(map)

        For Each si As String In tokens
            Dim v = index.Add(si)
            Dim page As NodeMap = v.data

            If page Is Nothing Then
                v.data = New NodeMap With {.resources = New List(Of String)}
                page = v.data
            End If

            Call page.resources.Add(map)
        Next

        Dim file As Stream = buf.OpenFile(path, FileMode.OpenOrCreate, FileAccess.Write)
        Call file.Write(data, Scan0, data.Length)
        Call file.Flush()
        Call file.Dispose()

        Return True
    End Function

    Public Function [Get](query As String) As IEnumerable(Of NumericTagged(Of String))
        Dim tokens As String() = Strings.LCase(query).Split
        Dim maps As New Dictionary(Of String, Double)

        For Each si As String In tokens
            Dim v = index.Find(si)
            Dim f As Double = 0.5

            If v.success Then
                f = 1
            End If

            If v.child.data Is Nothing Then
                v.child.data = New NodeMap With {.resources = New List(Of String)}
            End If

            For Each map As String In v.child.data.resources
                If Not maps.ContainsKey(map) Then
                    Call maps.Add(map, 0)
                End If

                maps(map) += f
            Next
        Next

        Return From a
               In maps
               Select o = New NumericTagged(Of String)(a.Value, a.Key)
               Order By o.tag Descending
    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                Using ms As New MemoryStream
                    Call New IndexWriter(ms).Write(index)
                    Call ms.Flush()
                    Call ms.Seek(Scan0, SeekOrigin.Begin)

                    Dim file As Stream = buf.OpenFile("/index.dat", FileMode.OpenOrCreate, FileAccess.Write)

                    Call file.Write(ms.ToArray, Scan0, ms.Length)
                    Call file.Flush()
                    Call file.Dispose()
                End Using

                ' TODO: 释放托管状态(托管对象)
                Call buf.Dispose()
            End If

            ' TODO: 释放未托管的资源(未托管的对象)并重写终结器
            ' TODO: 将大型字段设置为 null
            disposedValue = True
        End If
    End Sub

    ' ' TODO: 仅当“Dispose(disposing As Boolean)”拥有用于释放未托管资源的代码时才替代终结器
    ' Protected Overrides Sub Finalize()
    '     ' 不要更改此代码。请将清理代码放入“Dispose(disposing As Boolean)”方法中
    '     Dispose(disposing:=False)
    '     MyBase.Finalize()
    ' End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' 不要更改此代码。请将清理代码放入“Dispose(disposing As Boolean)”方法中
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub
End Class
