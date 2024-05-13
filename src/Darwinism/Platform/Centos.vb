#Region "Microsoft.VisualBasic::e4c616a6a11bef7fa4bdf71f8980fd1c, src\Darwinism\Platform\Centos.vb"

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

    '   Total Lines: 85
    '    Code Lines: 61
    ' Comment Lines: 14
    '   Blank Lines: 10
    '     File Size: 3.35 KB


    ' Module CentosTools
    ' 
    '     Function: check_command_exists, netstat_func, netstat_table, RunLinuxHelper
    ' 
    '     Sub: Main
    ' 
    ' /********************************************************************************/

#End Region

Imports Darwinism.Centos
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
Imports SMRUCC.Rsharp.Runtime.Interop

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
        Call Internal.Object.Converts.makeDataframe.addHandler(GetType(netstat()), AddressOf netstat_table)
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
    ''' <param name="command"></param>
    ''' <returns></returns>
    <ExportAPI("check_command_exists")>
    <RApiReturn(TypeCodes.boolean)>
    Public Function check_command_exists(command As String, Optional env As Environment = Nothing) As Object
        If Interaction.isUnix Then
            Return Interaction.HasCommand(command)
        Else
            Return Internal.debug.stop("only works on linux system!", env)
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
            Return Internal.debug.stop("only works on linux system!", env)
        End If
    End Function

End Module
