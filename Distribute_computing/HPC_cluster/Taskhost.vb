Imports System.Runtime.CompilerServices
Imports System.Text
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.InteropService
Imports Microsoft.VisualBasic.ApplicationServices

' Microsoft VisualBasic CommandLine Code AutoGenerator
' assembly: ..\build\Taskhost.d.exe

' 
'  // 
'  // Taskhost.d Daemon Process
'  // 
'  // VERSION:   1.0.0.0
'  // ASSEMBLY:  Taskhost.d, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
'  // COPYRIGHT: Copyright (c) I@xieguigang.me 2016
'  // GUID:      9de7a49c-d3fe-4993-9014-7f5f000e5314
'  // BUILT:     1/1/2000 12:00:00 AM
'  // 
' 
' 
'  < sciBASIC.ComputingServices.Taskhost.Daemon.Program >
' 
' 
' SYNOPSIS
' Taskhost.d command [/argument argument-value...] [/@set environment-variable=value...]
' 
' All of the command that available in this program has been list below:
' 
'  /deploy:       
'  /parallel:     Run task parallel
' 
' 
' ----------------------------------------------------------------------------------------------------
' 
'    1. You can using "Taskhost.d ??<commandName>" for getting more details command help.
'    2. Using command "Taskhost.d /CLI.dev [---echo]" for CLI pipeline development.
'    3. Using command "Taskhost.d /i" for enter interactive console mode.

Namespace CLI


''' <summary>
''' sciBASIC.ComputingServices.Taskhost.Daemon.Program
''' </summary>
'''
Public Class Taskhost_d : Inherits InteropService

    Public Const App$ = "Taskhost.d.exe"

    Sub New(App$)
        MyBase._executableAssembly = App$
    End Sub
        
''' <summary>
''' Create an internal CLI pipeline invoker from a given environment path. 
''' </summary>
''' <param name="directory">A directory path that contains the target application</param>
''' <returns></returns>
     <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Function FromEnvironment(directory As String) As Taskhost_d
          Return New Taskhost_d(App:=directory & "/" & Taskhost_d.App)
     End Function

''' <summary>
''' ```bash
''' /deploy
''' ```
''' </summary>
'''

Public Function Deployed() As Integer
Dim cli = GetDeployedCommandLine(internal_pipelineMode:=True)
    Dim proc As IIORedirectAbstract = RunDotNetApp(cli)
    Return proc.Run()
End Function
Public Function GetDeployedCommandLine(Optional internal_pipelineMode As Boolean = True) As String
    Dim CLI As New StringBuilder("/deploy")
    Call CLI.Append(" ")
     Call CLI.Append($"/@set --internal_pipeline={internal_pipelineMode.ToString.ToUpper()} ")


Return CLI.ToString()
End Function

''' <summary>
''' ```bash
''' /parallel
''' ```
''' Run task parallel
''' </summary>
'''

Public Function Parallel() As Integer
Dim cli = GetParallelCommandLine(internal_pipelineMode:=True)
    Dim proc As IIORedirectAbstract = RunDotNetApp(cli)
    Return proc.Run()
End Function
Public Function GetParallelCommandLine(Optional internal_pipelineMode As Boolean = True) As String
    Dim CLI As New StringBuilder("/parallel")
    Call CLI.Append(" ")
     Call CLI.Append($"/@set --internal_pipeline={internal_pipelineMode.ToString.ToUpper()} ")


Return CLI.ToString()
End Function
End Class
End Namespace



