#Region "Microsoft.VisualBasic::667cf461f26c7a4952024bc8ad44b4ee, Distribute_computing\HPC_cluster\Cluster.vb"

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

    ' Class Cluster
    ' 
    '     Constructor: (+2 Overloads) Sub New
    '     Function: RunTask
    ' 
    ' /********************************************************************************/

#End Region

Imports Darwinism.Centos
Imports Darwinism.Docker
Imports HPC_cluster.CLI
Imports Microsoft.VisualBasic.Net

''' <summary>
''' 高性能计算集群，高性能计算集群采用将计算任务分配到集群的不同计算节点儿提高计算能力，
''' 因而主要应用在科学计算领域。比较流行的HPC采用Linux操作系统和其它一些免费软件来完成并行运算。
''' 
''' 这一集群配置通常被称为Beowulf集群。这类集群通常运行特定的程序以发挥HPC cluster的并行能力。
''' 这类程序一般应用特定的运行库, 比如专为科学计算设计的MPI库。HPC集群特别适合于在计算中各计算节点
''' 之间发生大量数据通讯的计算作业，比如一个节点的中间结果或影响到其它节点计算结果的情况。
''' </summary>
''' <remarks>
''' This module only works on Linux server.
''' 
''' 在这里假设每一个集群节点中的环境都是一致的
''' </remarks>
Public Class Cluster

    ReadOnly ssh As SSH
    ReadOnly docker As Environment
    ReadOnly localhost As String

    ''' <summary>
    ''' create a helper module for deploy environment via ``ssh`` and ``cluster node share storage``.
    ''' </summary>
    ''' <param name="remote"></param>
    ''' <param name="userName"></param>
    ''' <param name="imageName"></param>
    Sub New(remote As IPEndPoint, userName As String, Optional imageName As String = Nothing)
        Me.ssh = New SSH(userName, Nothing, remote.ipAddress, remote.port)
        Me.localhost = WebServiceUtils.LocalIPAddress

        If Not imageName.StringEmpty Then
            Me.docker = New Environment(imageName)
        End If
    End Sub

    Sub New(ssh As SSH, docker As Environment)
        Me.localhost = WebServiceUtils.LocalIPAddress
        Me.ssh = ssh
        Me.docker = docker
    End Sub

    Public Function RunTask(master As Integer) As String
        Dim taskHost As Taskhost_d = Taskhost_d.FromEnvironment(App.HOME)
        Dim socketStream As String = App.GetVariable("sockets")

        If socketStream.StringEmpty Then
            Throw New InvalidOperationException($"you should set variable 'sockets' to a location on your clusters' share storage at first!")
        End If

        Dim cli As String = taskHost.GetParallelCommandLine(
            master:=master,
            host:=localhost,
            socket:=socketStream,
            imagename:=docker?.container
        )

        If docker Is Nothing Then
            ' run on physical machine
            Return ssh.Run($"{taskHost.Path} {cli}")
        Else
            Return ssh.Run(docker.CreateDockerCommand($"{taskHost.Path} {cli}"))
        End If
    End Function
End Class

