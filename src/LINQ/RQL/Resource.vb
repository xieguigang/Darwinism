Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel.Ranges
Imports Microsoft.VisualBasic.Data.GraphTheory
Imports Microsoft.VisualBasic.Data.Repository
Imports Microsoft.VisualBasic.DataStorage.HDSPack.FileSystem
Imports Microsoft.VisualBasic.SecurityString

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

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function Add(key As String, str As String)
        Return Add(key, Encoding.UTF8.GetBytes(str))
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function ReadString(map As String) As String
        Return Encoding.UTF8.GetString(ReadBuffer(map))
    End Function

    Public Function ReadBuffer(map As String) As Byte()
        Dim path As String = URL(map)
        Dim file As Stream = buf.OpenFile(path, FileMode.Open, FileAccess.Read)
        Dim bytes As Byte() = New Byte(file.Length - 1) {}
        Call file.Read(bytes, Scan0, bytes.Length)
        Return bytes
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Private Shared Function URL(map As String) As String
        Return $"/pool/{map.Substring(4, 2)}/{map.Substring(16, 6)}/{map}"
    End Function

    Public Shared Function GetHashKey(data As Byte()) As String
        Static md5 As New Md5HashProvider

        ' all null/empty data point to ZERO location
        If data.IsNullOrEmpty Then
            Return New String("0"c, 32)
        End If

        ' combine two hash algorithm for avoid the hash confliction
        Dim key1 As String = md5.GetMd5Hash(data.ToArray)
        Dim firstByte = data(0).ToString
        Dim lastByte = data(data.Length - 1).ToString
        Dim middleByte = data((data.Length - 1) / 2).ToString
        Dim fnv = FNV1a.GetHashCode({firstByte, lastByte, middleByte}).ToHexString
        Dim hashcode As String = md5.GetMd5Hash(key1 & fnv)

        Return hashcode
    End Function

    ''' <summary>
    ''' Associates a resource <paramref name="data"/> with a given query <paramref name="key"/>.
    ''' </summary>
    ''' <param name="key">The query key, could be any text.(SHOULD NOT BE EMPTY!)</param>
    ''' <param name="data">the data for store in the database and associated with
    ''' given query text data <paramref name="key"/>, the unique reference key of
    ''' this resource data is generated via a specific hash algorithm based on 
    ''' this data payload.</param>
    ''' <returns></returns>
    Public Function Add(key As String, data As Byte()) As Boolean
        Dim tokens As String() = Strings.LCase(key).Split
        Dim map As String = GetHashKey(data)
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

    ''' <summary>
    ''' Query resources matches
    ''' </summary>
    ''' <param name="query">any query term</param>
    ''' <returns>
    ''' A collection of the query result key with score value,
    ''' the numeric tag in this collection is the query matches
    ''' score and the key string value could be used for read
    ''' resource data via the <see cref="ReadBuffer(String)"/> 
    ''' function.
    ''' </returns>
    ''' <remarks>
    ''' the result data of the query result has already been re-order
    ''' via the matches score desc
    ''' </remarks>
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
