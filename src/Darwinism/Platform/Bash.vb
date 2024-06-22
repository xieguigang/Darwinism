#Region "Microsoft.VisualBasic::410153f0e591303c55e5321410882adf, src\Darwinism\Platform\Bash.vb"

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

    '   Total Lines: 65
    '    Code Lines: 30 (46.15%)
    ' Comment Lines: 28 (43.08%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 7 (10.77%)
    '     File Size: 2.32 KB


    ' Module Bash
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: exec, run, ssh
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Darwinism.Centos
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime

''' <summary>
''' An automation pipeline toolkit build for cloud computing
''' </summary>
''' <remarks>
''' working on windows for connect remote linux server via putty
''' </remarks>
<Package("bash", Category:=APICategories.UtilityTools)>
Public Module Bash

    Sub New()
        Call Internal.ConsolePrinter.AttachConsoleFormatter(Of PuTTY)(Function(ssh) ssh.ToString)
    End Sub

    ''' <summary>
    ''' Create a new remote linux ssh session
    ''' </summary>
    ''' <param name="user$"></param>
    ''' <param name="password$"></param>
    ''' <param name="endpoint$"></param>
    ''' <param name="port%"></param>
    ''' <param name="plink"></param>
    ''' <returns></returns>
    <ExportAPI("ssh")>
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function ssh(user$, password$,
                        Optional endpoint$ = "127.0.0.1",
                        Optional port% = 22,
                        Optional plink As String = "plink",
                        Optional debug As Boolean = False) As PuTTY

        Return New PuTTY(user, password, endpoint, port, plink, debug)
    End Function

    ''' <summary>
    ''' Run a script on remote linux server session.
    ''' </summary>
    ''' <param name="ssh">remote linux ssh session</param>
    ''' <param name="script"></param>
    ''' <returns></returns>
    <ExportAPI("run")>
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function run(ssh As PuTTY, script As String) As String
        Return ssh.Run(script)
    End Function

    ''' <summary>
    ''' Execute a command on remote linux server session.
    ''' </summary>
    ''' <param name="ssh">remote linux ssh session</param>
    ''' <param name="command"></param>
    ''' <param name="arguments"></param>
    ''' <returns></returns>
    <ExportAPI("exec")>
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function exec(ssh As PuTTY, command As String, Optional arguments As String = Nothing) As String
        Return ssh.Shell(command, arguments)
    End Function

End Module
