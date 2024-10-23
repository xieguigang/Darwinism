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
