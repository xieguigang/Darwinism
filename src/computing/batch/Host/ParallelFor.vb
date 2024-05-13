#Region "Microsoft.VisualBasic::b79d9d416604ef76d19abdcd290b0a4b, src\computing\batch\Host\ParallelFor.vb"

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

    '   Total Lines: 53
    '    Code Lines: 41
    ' Comment Lines: 0
    '   Blank Lines: 12
    '     File Size: 1.60 KB


    ' Class ParallelFor
    ' 
    '     Properties: debugMode
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: CreateFunction, GetResult, ToString
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.MIME.application.json
Imports Parallel
Imports Parallel.IpcStream
Imports Parallel.ThreadTask

Public Class ParallelFor(Of T)

    ReadOnly args As Argument

    Public ReadOnly Property debugMode As Boolean
        Get
            Return Not args.debugPort Is Nothing
        End Get
    End Property

    Sub New(args As Argument)
        Me.args = args
    End Sub

    Public Iterator Function GetResult(task As IEnumerable(Of Func(Of T))) As IEnumerable(Of T)
        Dim [for] As New ThreadTask(Of T)(task,
            debugMode:=debugMode,
            verbose:=args.verbose,
            taskInterval:=args.thread_interval)

        For Each yout As T In [for] _
            .WithDegreeOfParallelism(args.n_threads) _
            .RunParallel

            Yield yout
        Next
    End Function

    Public Overrides Function ToString() As String
        Return args.GetJson
    End Function

    Public Shared Iterator Function CreateFunction(par As Argument, task As [Delegate], [loop] As SocketRef(), ParamArray args As SocketRef()) As IEnumerable(Of Func(Of T))
        For Each xi As SocketRef In [loop]
            Dim host As SlaveTask = par.CreateHost
            Dim post As SocketRef() = New SocketRef(args.Length) {}

            post(0) = xi
            post.SetRange(args, offset:=1)

            Yield Function() As T
                      Dim result As T = host.RunTask(Of T)(task, post)
                      Return result
                  End Function
        Next
    End Function

End Class
