Imports System.Runtime.CompilerServices
Imports System.Text
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.InteropService
Imports Microsoft.VisualBasic.ApplicationServices

' Microsoft VisualBasic CommandLine Code AutoGenerator
' assembly: ..\build\Think.exe

' 
'  // 
'  // 
'  // 
'  // VERSION:   1.0.0.0
'  // COPYRIGHT: Copyright ©  2019
'  // GUID:      122d95f7-d875-4ee8-92b2-2fdbfdaa2261
'  // 
' 
' 
'  < thinking.CLI >
' 
' 
' SYNOPSIS
' Think command [/argument argument-value...] [/@set environment-variable=value...]
' 
' All of the command that available in this program has been list below:
' 
'  /slave:     Program running in slave mode, apply for the multiple-process parallel.
' 
' 
' ----------------------------------------------------------------------------------------------------
' 
'    1. You can using "Think ??<commandName>" for getting more details command help.
'    2. Using command "Think /CLI.dev [---echo]" for CLI pipeline development.
'    3. Using command "Think /i" for enter interactive console mode.

Namespace CLI


''' <summary>
''' thinking.CLI
''' </summary>
'''
Public Class Think : Inherits InteropService

    Public Const App$ = "Think.exe"

    Sub New(App$)
        MyBase._executableAssembly = App$
    End Sub

     <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Function FromEnvironment(directory As String) As Think
          Return New Think(App:=directory & "/" & Think.App)
     End Function

''' <summary>
''' ```
''' /slave /application &lt;invokeinfo/json_base64> /out &lt;memory_mapfile>
''' ```
''' Program running in slave mode, apply for the multiple-process parallel.
''' </summary>
'''
Public Function Slave(application As String, out As String) As Integer
    Dim CLI As New StringBuilder("/slave")
    Call CLI.Append(" ")
    Call CLI.Append("/application " & """" & application & """ ")
    Call CLI.Append("/out " & """" & out & """ ")
     Call CLI.Append("/@set --internal_pipeline=TRUE ")


    Dim proc As IIORedirectAbstract = RunDotNetApp(CLI.ToString())
    Return proc.Run()
End Function
End Class
End Namespace
