#Region "Microsoft.VisualBasic::e3f7f2d77b87af096307aea3e6c90ced, src\data\LINQ\LINQ\Runtime\Indexing\FullTextSearch\FTSEngine.vb"

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

    '   Total Lines: 81
    '    Code Lines: 48 (59.26%)
    ' Comment Lines: 18 (22.22%)
    '    - Xml Docs: 38.89%
    ' 
    '   Blank Lines: 15 (18.52%)
    '     File Size: 2.51 KB


    ' Class FTSEngine
    ' 
    '     Constructor: (+1 Overloads) Sub New
    ' 
    '     Function: Search
    ' 
    '     Sub: (+2 Overloads) Dispose, (+2 Overloads) Indexing, IndexingOneDocument
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Linq

Public Class FTSEngine : Inherits SearchIndex
    Implements IDisposable

    ReadOnly index As InvertedIndex

    Private disposedValue As Boolean

    Sub New(pool As DocumentPool)
        Call MyBase.New(pool)
        ' read index from file
        index = pool.GetIndex
    End Sub

    ''' <summary>
    ''' add index of a single document
    ''' </summary>
    ''' <param name="doc"></param>
    ''' <remarks>
    ''' thread unsafe
    ''' </remarks>
    Public Overrides Sub Indexing(doc As String)
        If index.Add(doc) Then
            Call documents.Save(doc)
        ElseIf doc Is Nothing Then
            ' 20241018 for avoid the incorrect data offset
            ' when there are some missing content in the
            ' data source
            Call documents.Save("")
        End If
    End Sub

    Public Overrides Sub Indexing(doc As String, id As Integer)
        If index.Add(doc, id) Then
            Call documents.Save(doc)
        End If
    End Sub

    Protected Overrides Sub IndexingOneDocument(data() As String)
        Dim id As Integer = index.MoveNext

        For Each doc As String In data.SafeQuery
            Call Indexing(doc, id)
        Next
    End Sub

    Public Iterator Function Search(text As String) As IEnumerable(Of SeqValue(Of String))
        Dim ids = index.Search(text)

        If ids Is Nothing Then
            Return
        End If

        For Each id As Integer In ids
            Yield New SeqValue(Of String)(id, documents.GetDocument(id))
        Next
    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: 释放托管状态(托管对象)
                Call documents.WriteIndex(index)
                Call documents.Dispose()
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
