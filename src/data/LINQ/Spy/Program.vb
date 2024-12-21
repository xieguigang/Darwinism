#Region "Microsoft.VisualBasic::d4b1b5cbad0b77f68b98998808b06bf7, src\data\LINQ\Spy\Program.vb"

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

    '   Total Lines: 86
    '    Code Lines: 66 (76.74%)
    ' Comment Lines: 6 (6.98%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 14 (16.28%)
    '     File Size: 3.17 KB


    ' Module Program
    ' 
    '     Function: InspectBson, InspectFile, listen, Main, MakeIndex
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports Flute.Http
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.MIME.application.json
Imports Microsoft.VisualBasic.MIME.application.json.BSON
Imports RQL

Module Program

    Public Function Main() As Integer
        Return GetType(Program).RunCLI(
            args:=App.CommandLine,
            executeFile:=AddressOf InspectFile,
            executeEmpty:=AddressOf LinqShell.RunTerminal
        )
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
