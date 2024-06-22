#Region "Microsoft.VisualBasic::84c589878c3cc8350584f74bf48df6a1, src\CloudKit\Centos\SSH\PuTTY.vb"

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

    '   Total Lines: 75
    '    Code Lines: 37 (49.33%)
    ' Comment Lines: 26 (34.67%)
    '    - Xml Docs: 92.31%
    ' 
    '   Blank Lines: 12 (16.00%)
    '     File Size: 3.00 KB


    ' Class PuTTY
    ' 
    '     Constructor: (+1 Overloads) Sub New
    ' 
    '     Function: callPipeline, Run, Shell
    ' 
    '     Sub: cacheServerKey
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.CommandLine

''' <summary>
''' PuTTY automation combine with the hyper-V virtual machine for windows server
''' </summary>
Public Class PuTTY : Inherits SSH

    ReadOnly plink As String
    ReadOnly session As String

    Sub New(user$, password$,
            Optional endpoint$ = "127.0.0.1",
            Optional port% = 22,
            Optional plink As String = "plink",
            Optional debug As Boolean = False)

        Call MyBase.New(user, password, endpoint, port, debug)

        Me.plink = plink
        Me.cacheServerKey()
        Me.session = TempFileSystem.GetAppSysTempFile(".txt", sessionID:=App.PID.ToHexString, prefix:="bash_session")
    End Sub

    ''' <summary>
    ''' try to fix problem of 
    ''' 
    ''' ```
    ''' D:\web\RWeb>plink pipeline_submit@192.168.0.254 -P 22 -pw "pipeline_submit123" -batch qsub -cwd -e "/mnt/smb3/tmp/GCMS_autoTest/error" -o "/mnt/smb3/tmp/GCMS_autoTest/stdout" /mnt/smb2/.auto//GC-MS/auto_f1803b4671326cc1491b2cd8b7b9e7d3.sh
    ''' The server's host key is not cached in the registry. You
    ''' have no guarantee that the server Is the computer you
    ''' think it Is.
    ''' The server 's ssh-ed25519 key fingerprint is:
    ''' ssh-ed25519 255 58:66:53:3d:3f:6c:a9:fb:8c:ad:af:bd:8c:dc:57:16
    ''' Connection abandoned.
    ''' ```
    ''' </summary>
    Private Sub cacheServerKey()
        Call Console.WriteLine(PipelineProcess.Call(plink, $"-ssh {user}@{endpoint} -P {port} -pw ""{password}"" -batch exit"))
    End Sub

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
    Public Overrides Function Run(bash As String) As String
        Dim cli As String = $"{endpoint} -P {port} -l {user} -pw ""{password}"" -batch /bin/bash ""{bash}"""
        Dim std_out As String = callPipeline(cli)

        Return std_out
    End Function

    Private Function callPipeline(cli As String) As String
        If debug Then
            Call $"{plink} {cli}".__DEBUG_ECHO
        End If

        Dim std_out As String = PipelineProcess.[Call](plink, cli)
        Return std_out
    End Function

    Public Function Shell(command As String, Optional arguments As String = Nothing) As String
        Dim cmdl As String = If(arguments.StringEmpty, command, $"{command} {arguments}")
        Dim cli As String = $"{user}@{endpoint} -P {port} -pw ""{password}"" -batch {cmdl}"
        Dim std_out As String = callPipeline(cli)

        Return std_out
    End Function
End Class
