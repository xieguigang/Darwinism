#Region "Microsoft.VisualBasic::f4c326bbc4f28a74d340afa1e16b46f6, Centos\Bash.vb"

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

    ' Module Bash
    ' 
    '     Function: exec, run, ssh
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData

<Package("bash")>
Public Module Bash

    <ExportAPI("ssh")>
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function ssh(user$, password$, Optional endpoint$ = "127.0.0.1", Optional port% = 22, Optional plink As String = "plink") As PuTTY
        Return New PuTTY(user, password, endpoint, port, plink)
    End Function

    <ExportAPI("run")>
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function run(ssh As PuTTY, script As String) As String
        Return ssh.Run(script)
    End Function

    <ExportAPI("exec")>
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function exec(ssh As PuTTY, command As String, Optional arguments As String = Nothing) As String
        Return ssh.Shell(command, arguments)
    End Function

End Module
