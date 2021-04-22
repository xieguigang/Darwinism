#Region "Microsoft.VisualBasic::1e46c22f28005d12e009f4f12562bce1, Distribute_computing\Taskhost.d\Program.vb"

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

    ' Module Program
    ' 
    '     Function: Deployed, Parallel
    ' 
    '     Sub: Main
    ' 
    ' /********************************************************************************/

#End Region

Imports System.ComponentModel
Imports System.Net
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Parallel
Imports Parallel.IpcStream

''' <summary>
''' Running on the server cluster nodes
''' </summary>
Module Program

    Sub Main()
        Call GetType(Program).RunCLI(App.CommandLine)
    End Sub

    ''' <summary>
    ''' 将当前的环境自动化部署到目标计算节点之上
    ''' </summary>
    ''' <param name="args"></param>
    ''' <returns></returns>
    ''' 
    <ExportAPI("/deploy")>
    <Usage("/deploy")>
    Public Function Deployed(args As CommandLine) As Integer

    End Function

    ''' <summary>
    ''' run parallel
    ''' </summary>
    ''' <param name="args"></param>
    ''' <returns></returns>
    <ExportAPI("/parallel")>
    <Usage("/parallel --master <port> [--host <localhost, default=localhost> --socket <tempdir> --imageName <docker_imageName>]")>
    <Description("Run task parallel")>
    <Argument("--host", True, CLITypes.String, PipelineTypes.undefined,
              AcceptTypes:={GetType(IPAddress)},
              Description:="The host location of the master node, default is localhost, means parallel computing on one host node, different host ip means cluster computing.")>
    <Argument("--master", False, CLITypes.Integer, PipelineTypes.undefined,
              AcceptTypes:={GetType(Integer)},
              Description:="The tcp port of the master node that opened to current parallel slave node.")>
    <Argument("--socket", True, CLITypes.File, PipelineTypes.undefined,
              AcceptTypes:={GetType(String)},
              Description:="A data location on shared storage of your cluster nodes if run parallel in cluster computing mode.")>
    <Argument("--imageName", True, CLITypes.String, PipelineTypes.undefined,
              AcceptTypes:={GetType(String)},
              Description:="The docker image name if your cluster application is deployed via docker.")>
    Public Function Parallel(args As CommandLine) As Integer
        Dim master As Integer = args <= "--master"
        Dim host As String = args("--host") Or "localhost"
        Dim socket As String = args <= "--socket"
        Dim imageName As String = args <= "--imageName"

        If Not socket.StringEmpty Then
            Call SocketRef.SetSocketPool(handle:=socket)
        End If

        Return New TaskBuilder(port:=master, master:=host).Run
    End Function

End Module
