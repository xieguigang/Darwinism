﻿#Region "Microsoft.VisualBasic::f985976eef0e3bdc7e0c453e42174ace, src\Darwinism\Platform\Centos.vb"

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

'   Total Lines: 90
'    Code Lines: 64 (71.11%)
' Comment Lines: 16 (17.78%)
'    - Xml Docs: 75.00%
' 
'   Blank Lines: 10 (11.11%)
'     File Size: 3.59 KB


' Module CentosTools
' 
'     Function: check_command_exists, netstat_func, netstat_table, RunLinuxHelper
' 
'     Sub: Main
' 
' /********************************************************************************/

#End Region

Imports System.IO
Imports Darwinism.Centos
Imports Darwinism.Centos.proc.net
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
Imports SMRUCC.Rsharp.Runtime.Interop
Imports RInternal = SMRUCC.Rsharp.Runtime.Internal

''' <summary>
''' 
''' </summary>
''' <remarks>
''' this module only works on linux system
''' </remarks>
<Package("centos")>
<RTypeExport(GetType(netstat))>
Public Module CentosTools

    Sub Main()
        Call RInternal.Object.Converts.makeDataframe.addHandler(GetType(netstat()), AddressOf netstat_table)
    End Sub

    <RGenericOverloads("as.data.frame")>
    Private Function netstat_table(netstat As netstat(), args As list, env As Environment) As dataframe
        Dim df As New dataframe With {.columns = New Dictionary(Of String, Array)}

        Call df.add("Proto", netstat.Select(Function(a) a.Proto))
        Call df.add("Recv-Q", netstat.Select(Function(a) a.RecvQ))
        Call df.add("Send-Q", netstat.Select(Function(a) a.SendQ))
        Call df.add("Local Address", netstat.Select(Function(a) a.LocalAddress))
        Call df.add("Foreign Address", netstat.Select(Function(a) a.ForeignAddress))
        Call df.add("State", netstat.Select(Function(a) a.State))
        Call df.add("PID/Program name", netstat.Select(Function(a) a.Program))
        Call df.add("listened_port", netstat.Select(Function(a) a.LocalListenPort))

        Return df
    End Function

    ''' <summary>
    ''' check command is existsed on linux system or not?
    ''' </summary>
    ''' <param name="command">
    ''' the command name
    ''' </param>
    ''' <returns></returns>
    <ExportAPI("check_command_exists")>
    <RApiReturn(TypeCodes.boolean)>
    Public Function check_command_exists(command As String,
                                         Optional verbose As Boolean? = Nothing,
                                         Optional env As Environment = Nothing) As Object
        If Interaction.isUnix Then
            Return Interaction.HasCommand(command, env.verboseOption(verbose))
        Else
            Return RInternal.debug.stop("only works on linux system!", env)
        End If
    End Function

    <ExportAPI("netstat")>
    Public Function netstat_func(Optional x As String = "-tulnp",
                                 Optional verbose As Boolean = False,
                                 Optional env As Environment = Nothing) As Object
        If x.StringEmpty Then
            ' netstat command do nothing
            Return Nothing
        ElseIf Strings.Trim(x).Contains(" ") Then
            ' is output text?
            Return netstat.tulnp(x).ToArray
        Else
            ' shell command
            Dim text = RunLinuxHelper("netstat", x, verbose:=verbose, env:=env)

            If text Like GetType(Message) Then
                Return text.TryCast(Of Message)
            Else
                Return netstat.tulnp(text.TryCast(Of String)).ToArray
            End If
        End If
    End Function

    Private Function RunLinuxHelper(command As String, args As String, verbose As Boolean, env As Environment) As [Variant](Of String, Message)
        If Interaction.isUnix Then
            Return Interaction.Shell(command, args, verbose:=verbose)
        Else
            Return RInternal.debug.stop("only works on linux system!", env)
        End If
    End Function

    ''' <summary>
    ''' Parse the file content of file ``/proc/net/tcp``
    ''' </summary>
    ''' <param name="text"></param>
    ''' <returns></returns>
    <ExportAPI("parse_tcp")>
    <RApiReturn(GetType(proc.net.tcp))>
    Public Function ParseTcpFile(text As String) As Object
        Return proc.net.tcp.Parse(New StringReader(text)).ToArray
    End Function

End Module
