#Region "Microsoft.VisualBasic::590ee32efdb20bd12cd3fb5f317173d5, ComputingServices\Cluster\Master.vb"

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

    '     Class Master
    ' 
    '         Properties: Nodes
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    '         Function: [Select], GetPreferNode, Invoke
    ' 
    '         Sub: Scan, ScanTask
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Http
Imports Microsoft.VisualBasic.Net.Protocols
Imports Microsoft.VisualBasic.Parallel
Imports sciBASIC.ComputingServices.TaskHost

Namespace Cluster

    ''' <summary>
    ''' Client
    ''' </summary>
    Public Class Master

        ''' <summary>
        ''' Online avaliable nodes in this server cluster.
        ''' </summary>
        Dim _nodes As New Dictionary(Of TaskRemote)
        Dim node_port%
        Dim net$

        ''' <summary>
        ''' 返回节点的数量
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Nodes As Integer
            Get
                Return _nodes.Count
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="net$"><see cref="EnumerateAddress"/></param>
        ''' <param name="node_port%"></param>
        Sub New(net$, node_port%)
            Me.node_port = node_port
            Me.net = net
        End Sub

        Public Sub Scan()
            Dim request As New RequestStream(Protocols.ProtocolEntry, TaskProtocols.Handshake)

            For Each IP$ In EnumerateAddress(Me.net)
                Dim response = New AsynInvoke(IP, node_port) _
                    .SendMessage(request, 200)

                SyncLock _nodes

                    If response.Protocol = HTTP_RFC.RFC_OK Then
                        If Not _nodes.ContainsKey(IP) Then
                            Call _nodes.Add(New TaskRemote(IP, node_port))
                            Call $"Add new node: {IP}".__DEBUG_ECHO
                        End If
                    Else
                        If _nodes.ContainsKey(IP) Then
                            Call _nodes.Remove(IP)
                            Call $"Removes offline node: {IP}".__DEBUG_ECHO
                        End If
                    End If
                End SyncLock
            Next
        End Sub

        ''' <summary>
        ''' 返回负载量最低的节点
        ''' </summary>
        ''' <returns></returns>
        Public Function GetPreferNode() As TaskRemote
            SyncLock _nodes

                Return LinqAPI.DefaultFirst(Of TaskRemote) <=
 _
                    From node As TaskRemote
                    In _nodes.Values.AsParallel
                    Select node,
                        node.Load
                    Order By Load Ascending

            End SyncLock
        End Function

        ''' <summary>
        ''' 自动分配空闲的计算节点
        ''' </summary>
        ''' <param name="target"></param>
        ''' <param name="args"></param>
        Public Function Invoke(target As [Delegate], ParamArray args As Object()) As Object
            Dim node As TaskRemote = GetPreferNode()
            Dim out As Object = node.Invoke(target, args)
            Return out
        End Function

        Public Iterator Function [Select](Of T, Tout)(source As IEnumerable(Of T), task As [Delegate], args As Object()) As IEnumerable(Of Tout)
            Dim node As TaskRemote = GetPreferNode()
            Dim out As ILinq(Of Tout) = node.Select(Of T, Tout)(task, source.ToArray, args)

            For Each x As Tout In out
                Yield x
            Next
        End Function

        Public Sub ScanTask()
            Call RunTask(AddressOf Scan)
        End Sub
    End Class
End Namespace
