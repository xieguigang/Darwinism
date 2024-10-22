Imports System.IO
Imports Microsoft.VisualBasic.CommandLine
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

    Private Function InspectBson(file As String, args As CommandLine) As Integer
        Using s As Stream = file.Open(FileMode.Open, doClear:=False, [readOnly]:=True)
            Dim doc = BSONFormat.Load(s)

        End Using
    End Function

End Module
