#Region "Microsoft.VisualBasic::92b2e11020a3ca0f304c481c28383c48, src\data\LINQ\Spy\HttpQuery.vb"

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

    '   Total Lines: 71
    '    Code Lines: 60 (84.51%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 11 (15.49%)
    '     File Size: 2.48 KB


    ' Class HttpQuery
    ' 
    '     Constructor: (+1 Overloads) Sub New
    ' 
    '     Function: ParseUrlQuery, readBson
    ' 
    '     Sub: HandleQuery, MakeQuery
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports System.Runtime.CompilerServices
Imports Flute.Http.Core
Imports Flute.Http.Core.Message
Imports LINQ
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.MIME.application.json
Imports Microsoft.VisualBasic.MIME.application.json.BSON
Imports Microsoft.VisualBasic.MIME.application.json.Javascript
Imports Microsoft.VisualBasic.Net.Http
Imports RQL

Public Class HttpQuery : Implements IAppHandler

    ReadOnly type As String
    ReadOnly document As DocumentIndex
    ReadOnly queryIndex As QueryIndex
    ReadOnly reader As Func(Of UInteger, JsonObject)

    Sub New(file As String)
        type = file.ExtensionSuffix
        document = DocumentIndexer.LoadDocument(file)
        queryIndex = New QueryIndex(file.ChangeSuffix("rqi").Open(FileMode.Open, doClear:=False, [readOnly]:=True))

        Select Case type
            Case "bson" : reader = AddressOf readBson
            Case Else
                Throw New NotImplementedException
        End Select
    End Sub

    Public Sub HandleQuery(request As HttpRequest, response As HttpResponse) Implements IAppHandler.AppHandler
        Dim url As URL = request.URL

        Select Case url.path
            Case "/hash_keys"
                Call response.WriteJSON(queryIndex.hashKeys)
            Case Else
                Call MakeQuery(url, response)
        End Select
    End Sub

    Private Sub MakeQuery(url As URL, response As HttpResponse)
        Dim q As Query() = ParseUrlQuery(url).ToArray
        Dim index = queryIndex.GetIndex(q)
        Dim data As New List(Of JsonObject)

        For Each id As Integer In index.SafeQuery
            Call data.Add(reader(id))
        Next

        Call response.AddCustomHttpHeader("Content-Type", "application/json")
        Call response.WriteLine(data.CreateArray.BuildJsonString)
        Call response.Flush()
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Private Function readBson(id As UInteger) As JsonObject
        Return BSONFormat.Load(document.GetSubBuffer(id), leaveOpen:=True)
    End Function

    Private Iterator Function ParseUrlQuery(url As URL) As IEnumerable(Of Query)
        For Each qi In url.query
            Yield New Query With {
                .field = qi.Key,
                .search = Query.Type.HashTerm,
                .value = qi.Value(0)
            }
        Next
    End Function
End Class

