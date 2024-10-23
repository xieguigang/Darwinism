Imports System.IO
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.MIME.application.json
Imports Microsoft.VisualBasic.MIME.application.json.BSON
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
                      index As Stream = file.Open(FileMode.OpenOrCreate, doClear:=True, [readOnly]:=False)

                    Call mongoDB.CreateDocumentIndex(s)
                    Call mongoDB.Save(index)
                End Using
            Case Else
                Throw New NotImplementedException
        End Select

        Return 0
    End Function

End Module
