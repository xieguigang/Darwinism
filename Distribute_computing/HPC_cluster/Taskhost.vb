#Region "Microsoft.VisualBasic::52a28aab44d5e28b6d80ea6111871c5e, Distribute_computing\HPC_cluster\Taskhost.vb"

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

    ' Class Taskhost_d
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: FromEnvironment
    ' 
    ' 
    ' /********************************************************************************/

#End Region

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
''' /parallel --master &lt;port&gt; [--host &lt;localhost, default=localhost&gt; --socket &lt;tempdir&gt; --imageName &lt;docker_imageName&gt;]
''' ```
''' Run task parallel
''' </summary>
'''
''' <param name="host"> The host location of the master node, default is localhost, means parallel computing on one host node, different host ip means cluster computing.
''' </param>
''' <param name="master"> The tcp port of the master node that opened to current parallel slave node.
''' </param>
''' <param name="socket"> A data location on shared storage of your cluster nodes if run parallel in cluster computing mode.
''' </param>
''' <param name="imageName"> The docker image name if your cluster application is deployed via docker.
''' </param>
Public Function Parallel(master As String, Optional host As String = "localhost", Optional socket As String = "", Optional imagename As String = "") As Integer
Dim cli = GetParallelCommandLine(master:=master, host:=host, socket:=socket, imagename:=imagename, internal_pipelineMode:=True)
    Dim proc As IIORedirectAbstract = RunDotNetApp(cli)
    Return proc.Run()
End Function
Public Function GetParallelCommandLine(master As String, Optional host As String = "localhost", Optional socket As String = "", Optional imagename As String = "", Optional internal_pipelineMode As Boolean = True) As String
    Dim CLI As New StringBuilder("/parallel")
    Call CLI.Append(" ")
    Call CLI.Append("--master " & """" & master & """ ")
    If Not host.StringEmpty Then
            Call CLI.Append("--host " & """" & host & """ ")
    End If
    If Not socket.StringEmpty Then
            Call CLI.Append("--socket " & """" & socket & """ ")
    End If
    If Not imagename.StringEmpty Then
            Call CLI.Append("--imagename " & """" & imagename & """ ")
    End If
     Call CLI.Append($"/@set --internal_pipeline={internal_pipelineMode.ToString.ToUpper()} ")


Return CLI.ToString()
End Function
End Class
End Namespace




