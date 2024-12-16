#Region "Microsoft.VisualBasic::8e7ea1fb545a9b5b56af72dd6233284a, src\data\LINQ\RQL\ResourceQuery\Resource.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.



    ' /********************************************************************************/

    ' Summaries:


    ' Code Statistics:

    '   Total Lines: 241
    '    Code Lines: 136 (56.43%)
    ' Comment Lines: 65 (26.97%)
    '    - Xml Docs: 81.54%
    ' 
    '   Blank Lines: 40 (16.60%)
    '     File Size: 8.66 KB


    ' Class Resource
    ' 
    '     Properties: Archive
    ' 
    '     Constructor: (+1 Overloads) Sub New
    ' 
    '     Function: [Get], (+2 Overloads) Add, GetHashKey, ReadBuffer, ReadString
    '               URL
    ' 
    '     Sub: (+2 Overloads) Dispose, saveTreeIndex
    ' 
    ' /********************************************************************************/

#End Region

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

    Public ReadOnly Property Archive As StreamPack
        Get
            Return buf
        End Get
    End Property

    ''' <summary>
    ''' the tree index
    ''' </summary>
    Const indexfile As String = "/index.dat"

    Sub New(res As StreamPack)
        Dim indexfile = res.OpenFile(Resource.indexfile, FileMode.OpenOrCreate, FileAccess.Read)
        Dim parser As New IndexReader(indexfile)

        buf = res
        index = parser.Read
    End Sub

    ''' <summary>
    ''' Add a string resource into current arhive file, 
    ''' and associated this string resource data with
    ''' a given <paramref name="key"/> value.
    ''' </summary>
    ''' <param name="key"></param>
    ''' <param name="str"></param>
    ''' <returns></returns>
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function Add(key As String, str As String)
        Return Add(key, Encoding.UTF8.GetBytes(str))
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function ReadString(map As String, Optional category As String = "") As String
        Return Encoding.UTF8.GetString(ReadBuffer(map, category))
    End Function

    ''' <summary>
    ''' Read the resource pack data from the archive via a given resource map key
    ''' </summary>
    ''' <param name="map">A resource key which is <see cref="Get(String)"/> from
    ''' the archive index via a given query text</param>
    ''' <param name="category"></param>
    ''' <returns>
    ''' this function just returns nothing if the given resource 
    ''' is not exists inside of current archive file.
    ''' </returns>
    Public Function ReadBuffer(map As String, Optional category As String = "") As Byte()
        Dim path As String = URL(map, category)

        If Not buf.FileExists(path) Then
            Return Nothing
        End If

        Dim file As Stream = buf.OpenFile(path, FileMode.Open, FileAccess.Read)
        Dim bytes As Byte() = New Byte(file.Length - 1) {}
        Call file.Read(bytes, Scan0, bytes.Length)
        Return bytes
    End Function

    ''' <summary>
    ''' generates the internal package resource reference url
    ''' </summary>
    ''' <param name="map"></param>
    ''' <returns></returns>
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Private Shared Function URL(map As String, category As String) As String
        Return $"/pool/{category}/{map.Substring(4, 2)}/{map.Substring(16, 6)}/{map}"
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
    Public Function Add(key As String, data As Byte(), Optional category As String = "") As Boolean
        If key.StringEmpty Then
            Return False
        End If

        Dim tokens As String() = Strings.LCase(key).Split
        Dim map As String = GetHashKey(data)
        Dim path As String = URL(map, category)

        For Each si As String In tokens
            For len As Integer = 1 To si.Length - 1
                Dim sij = si.Substring(0, len)
                Dim v = index.Add(sij)
                Dim page As NodeMap = v.data

                If page Is Nothing Then
                    v.data = New NodeMap With {.resources = New List(Of String)}
                    page = v.data
                End If

                Call page.add(map)
            Next
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

    Private Sub saveTreeIndex()
        Using ms As New MemoryStream
            Call New IndexWriter(ms).Write(index)
            Call ms.Flush()
            Call ms.Seek(Scan0, SeekOrigin.Begin)

            Call buf.Delete(indexfile)

            Dim file As Stream = buf.OpenFile(indexfile, FileMode.OpenOrCreate, FileAccess.Write)

            Call file.Write(ms.ToArray, Scan0, ms.Length)
            Call file.Flush()
            Call file.Dispose()
        End Using
    End Sub

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                If Not buf.is_readonly Then
                    Call saveTreeIndex()
                End If

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
