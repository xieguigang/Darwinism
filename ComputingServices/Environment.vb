#Region "Microsoft.VisualBasic::04003b3c910f342502b928b396e06897, ComputingServices\Environment.vb"

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

' Module Environment
' 
'     Function: AsDistributed
' 
'     Sub: Start
' 
' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports System.Threading
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Parallel.Linq
Imports Microsoft.VisualBasic.Parallel.Tasks
Imports sciBASIC.ComputingServices.DistributeServices
Imports sciBASIC.ComputingServices.TaskHost

''' <summary>
''' 分布式计算环境，因为这里是为了做高性能计算而构建的一个内部网络的计算集群，
''' 所以数据再网络传输的过程之中加密与否已经无所谓了
''' </summary>
''' <remarks>
''' ```vbnet
''' '
''' ' ------Dragon be here!------
''' '        ┏┓    ┏┓
''' '     ┏━━┛┻━━━━┛┻━━┓
''' '     ┃            ┃
''' '     ┃     ━━     ┃
''' '     ┃  ┳━┛  ┗━┳  ┃
''' '     ┃　　　 　　　┃
''' '     ┃     ━┻━    ┃
''' '     ┃            ┃
''' '     ┗━┓        ┏━┛
''' '       ┃        ┃ 神兽保佑
''' '       ┃        ┃ 代码无BUG
''' '       ┃        ┗━━┓
''' '       ┃           ┣┓
''' '       ┃           ┏┛
''' '       ┗━━┓┓┏━━┳┓┏━┛
''' '          ┃┫┫  ┃┫┫
''' '          ┗┻┛  ┗┻┛
''' ' ━━━━━━━━━━神兽出没━━━━━━━━━━
''' ```
''' </remarks>
Public Module Environment

    Dim cluster As CenterController

    ''' <summary>
    ''' 扫描局域网内所有可用的计算节点
    ''' </summary>
    ''' <param name="port">The default port of the <see cref="TaskInvoke"/> cluster nodes is **1234**</param>
    Public Sub Start(netRange$, Optional port% = 1234)
        cluster = New CenterController(netRange, port)
        cluster.ScanTask()

        Call Thread.Sleep(1000)
    End Sub

    ''' <summary>
    ''' Running this task sequence in distributed computing mode.(使用分布式的方式来执行这个任务集合序列)
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <typeparam name="Tout"></typeparam>
    ''' <param name="source"></param>
    ''' <param name="args">All of the task use the same additional parameter</param>
    ''' <returns></returns>
    <Extension>
    Public Iterator Function AsDistributed(Of T, Tout)(source As IEnumerable(Of T), task As [Delegate], ParamArray args As Object()) As IEnumerable(Of Tout)
        Dim partitions = TaskPartitions.SplitIterator(source, source.Count / cluster.NumberOfNodes)
        Dim tasks As New List(Of AsyncHandle(Of Tout()))

        ' Run task partitions in different grid node
        For Each part As T() In partitions
            tasks += New AsyncHandle(Of Tout())(Function() cluster.Select(Of T, Tout)(part, task, args).ToArray)
        Next

        For Each out As AsyncHandle(Of Tout()) In tasks
            For Each x As Tout In out.GetValue
                Yield x
            Next
        Next
    End Function
End Module
