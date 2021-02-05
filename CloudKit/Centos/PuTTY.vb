#Region "Microsoft.VisualBasic::f37c65196baf41425b3bb74a2d5fc733, Centos\PuTTY.vb"

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

' Class PuTTY
' 
'     Constructor: (+1 Overloads) Sub New
'     Function: Run, Shell, ToString
' 
' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.CommandLine

''' <summary>
''' PuTTY automation combine with the hyper-V virtual machine for windows server
''' </summary>
Public Class PuTTY

    ReadOnly user As String
    ReadOnly password As String
    ReadOnly endpoint As String
    ReadOnly port As Integer
    ReadOnly plink As String
    ReadOnly debug As Boolean

    Sub New(user$, password$,
            Optional endpoint$ = "127.0.0.1",
            Optional port% = 22,
            Optional plink As String = "plink",
            Optional debug As Boolean = False)

        Me.user = user
        Me.password = password
        Me.endpoint = endpoint
        Me.plink = plink
        Me.port = port
        Me.debug = debug
    End Sub

    Public Overrides Function ToString() As String
        Return $"ssh {user}@{endpoint}:{port}"
    End Function

    ''' <summary>
    ''' Run a bash script in target virtual machine
    ''' </summary>
    ''' <param name="bash">the file path of the bash script in the hyper-v virtual machine.</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Hyper-V PowerShell - Run a bash command in Linux VM and get output.
    ''' 
    ''' https://stackoverflow.com/questions/57778301/hyper-v-powershell-run-a-bash-command-in-linux-vm-and-get-output
    ''' </remarks>
    Public Function Run(bash As String) As String
        Dim cli As String = $"{endpoint} -P {port} -l {user} -pw ""{password}"" -batch /bin/bash ""{bash}"""
        Dim std_out As String = callPipeline(cli)

        Return std_out
    End Function

    Private Function callPipeline(cli As String) As String
        If debug Then
            Call $"{plink} {cli}".__DEBUG_ECHO
        End If

        Return PipelineProcess.[Call](plink, cli)
    End Function

    Public Function Shell(command As String, Optional arguments As String = Nothing) As String
        Dim cmdl As String = If(arguments.StringEmpty, command, $"{command} {arguments}")
        Dim cli As String = $"{user}@{endpoint} -P {port} -pw ""{password}"" -batch {cmdl}"
        Dim std_out As String = callPipeline(cli)

        Return std_out
    End Function
End Class

