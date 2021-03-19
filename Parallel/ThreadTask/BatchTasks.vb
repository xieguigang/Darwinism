#Region "Microsoft.VisualBasic::897cad592566b84641453e627667509c, Parallel\ThreadTask\BatchTasks.vb"

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

    ' Module BatchTasks
    ' 
    '     Function: SelfFolks
    ' 
    '     Sub: Invoke
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.Language

Namespace ThreadTask

    Public Module BatchTasks

        ''' <summary>
        ''' Folk this program itself for the large amount data batch processing.
        ''' </summary>
        ''' <param name="CLI">Self folk processing commandline collection.</param>
        ''' <param name="parallel">If this parameter value less than 1, then will be a single 
        ''' thread task. Any positive value that greater than 1 will be parallel task.
        ''' (小于等于零表示非并行化，单线程任务)
        ''' </param>
        ''' <returns>
        ''' Returns the total executation time for running this task collection.
        ''' (返回任务的执行的总时长)
        ''' </returns>
        Public Function SelfFolks(CLI As IEnumerable(Of String), Optional parallel% = 0) As Long
            Dim sw As Stopwatch = Stopwatch.StartNew

            If parallel <= 0 Then
                For Each args As String In CLI
                    Call App.SelfFolk(args).Run()
                Next
            Else
                Dim Tasks As Func(Of Integer)() = LinqAPI.Exec(Of Func(Of Integer)) <=
 _
                    From args As String
                    In CLI
                    Let io As IIORedirectAbstract = App.SelfFolk(args)
                    Let task As Func(Of Integer) = AddressOf io.Run
                    Select task

                Call New ThreadTask(Of Integer)(Tasks) _
                    .WithDegreeOfParallelism(parallel) _
                    .RunParallel _
                    .ToArray
            End If

            Return sw.ElapsedMilliseconds
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="tasks"></param>
        ''' <param name="numOfThreads">同时执行的句柄的数目</param>
        ''' <remarks></remarks>
        <Extension>
        Public Sub Invoke(tasks As Action(), numOfThreads As Integer)
            Dim getTask As Func(Of Action, Func(Of Integer)) =
                Function(task)
                    Return AddressOf New TaskInvokeHelper With {
                        .task = task
                    }.RunTask
                End Function
            Dim invokes = From action As Action In tasks Select getTask(action)

            Call New ThreadTask(Of Integer)(invokes) _
                .WithDegreeOfParallelism(numOfThreads) _
                .RunParallel() _
                .ToArray
        End Sub
    End Module
End Namespace