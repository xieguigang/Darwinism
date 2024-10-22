Imports System.IO
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.MIME.application.json
Imports Microsoft.VisualBasic.MIME.application.json.BSON

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

End Module
