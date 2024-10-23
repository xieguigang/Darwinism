Imports System.IO
Imports Flute.Http
Imports Flute.Http.Core.HttpSocket
Imports LINQ
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.MIME.application.json
Imports Microsoft.VisualBasic.MIME.application.json.BSON
Imports Microsoft.VisualBasic.MIME.application.json.Javascript
Imports Microsoft.VisualBasic.Net.Http
Imports RQL

Module Program

    Public Function Main() As Integer
        Return GetType(Program).RunCLI(App.CommandLine, executeFile:=AddressOf InspectFile)
    End Function

    Public Function InspectFile(file As String, args As CommandLine) As Integer
        Select Case file.ExtensionSuffix
            Case "bson" : Return InspectBson(file, args)
            Case Else
                Throw New NotImplementedException
        End Select
    End Function

    ''' <summary>
    ''' inspect of the MongoDB bson list
    ''' </summary>
    ''' <param name="file"></param>
    ''' <param name="args"></param>
    ''' <returns></returns>
    Private Function InspectBson(file As String, args As CommandLine) As Integer
        Using s As Stream = file.Open(FileMode.Open, doClear:=False, [readOnly]:=True)
            For Each item In BSONFormat.LoadList(s)
                Call Console.WriteLine(item.BuildJsonString)
            Next
        End Using
    End Function

    <ExportAPI("/make_index")>
    <Usage("/make_index /file <data.file> [--hash <field1,field2,field3,...>]")>
    Public Function MakeIndex(args As CommandLine) As Integer
        Dim file As String = args("/file")
        Dim hash As String = args("--hash")
        Dim indexfile As String = file.ChangeSuffix(".rqi")

        Select Case file.ExtensionSuffix
            Case "bson"
                Dim mongoDB As New MongoDBIndexer

                Using s As Stream = file.Open(FileMode.Open, doClear:=False, [readOnly]:=True),
                      index As Stream = indexfile.Open(FileMode.OpenOrCreate, doClear:=True, [readOnly]:=False)

                    If Not hash.StringEmpty(, True) Then
                        Call hash.Split(","c).ForEach(AddressOf mongoDB.AddHashIndex)
                    End If

                    Call mongoDB.CreateDocumentIndex(s)
                    Call mongoDB.Save(index)
                End Using
            Case Else
                Throw New NotImplementedException
        End Select

        Return 0
    End Function

    <ExportAPI("/listen")>
    <Usage("/listen -i <input_file> [-p <http_port, default=80>]")>
    Public Function listen(args As CommandLine) As Integer
        Dim file As String = args("-i")
        Dim port As Integer = args("-p") Or 80
        Dim indexfile As String = file.ChangeSuffix(".rqi")

        If Not indexfile.FileExists(ZERO_Nonexists:=True) Then
            Throw New InvalidProgramException("No index for the database file, please make index via ``/make_index`` at first, and then try again!")
        End If

        Return New HttpDriver() _
            .HttpMethod("get", New HttpQuery(file)) _
            .GetSocket(port) _
            .Run()
    End Function

End Module
