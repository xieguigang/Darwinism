#Region "Microsoft.VisualBasic::94d652ff8946a2259397f9ffe71ba4c1, src\data\LINQ\RQL\SearchIndex\MongoDBIndexer.vb"

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

    '   Total Lines: 76
    '    Code Lines: 57 (75.00%)
    ' Comment Lines: 4 (5.26%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 15 (19.74%)
    '     File Size: 2.56 KB


    ' Class MongoDBIndexer
    ' 
    '     Function: requestJSON
    ' 
    '     Sub: CreateDocumentIndex
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports LINQ
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.MIME.application.json.BSON
Imports Microsoft.VisualBasic.MIME.application.json.Javascript

Public Class MongoDBIndexer : Inherits DocumentIndexer

    Dim mongoDB As Decoder
    Dim offset As Long
    Dim documentLen As Long

    Public Overrides Sub CreateDocumentIndex(document As Stream)
        Dim id As Integer = 0

        documentLen = document.Length
        mongoDB = New Decoder(document)

        For Each json As JsonObject In TqdmWrapper.WrapStreamReader(document.Length, AddressOf requestJSON)
            If json Is Nothing Then
                If offset >= documentLen - 1 Then
                    Exit For
                Else
                    Continue For
                End If
            End If

            ' make hash index
            For Each field As String In hashIndex.Keys
                If Not json.HasObjectKey(field) Then
                    Continue For
                End If

                Dim val As JsonElement = json(field)
                Dim index As TermHashIndex = hashIndex(field)

                If TypeOf val Is JsonValue Then
                    ' is scalar string 
                    Dim str As String = DirectCast(val, JsonValue).GetStripString(decodeMetachar:=False, null:="")
                    Call index.Indexing(str, id)
                ElseIf TypeOf val Is JsonArray Then
                    ' is string array?
                    Dim strs As String() = DirectCast(val, JsonArray) _
                        .Select(Function(si) DirectCast(si, JsonValue) _
                        .GetStripString(decodeMetachar:=False, null:="")) _
                        .ToArray

                    For Each str As String In strs
                        Call index.Indexing(str, id)
                    Next
                Else
                    ' do nothing
                End If
            Next

            Call offsets.Add(id, offset)

            id += 1

            If offset >= documentLen - 1 Then
                Exit For
            End If
        Next
    End Sub

    Private Function requestJSON(ByRef getOffset As Long, bar As ProgressBar) As JsonObject
        getOffset = mongoDB.getDocumentOffset
        offset = getOffset

        If offset >= documentLen - 1 Then
            Return Nothing
        End If

        Return mongoDB.decodeDocument
    End Function
End Class
