#Region "Microsoft.VisualBasic::5baf554035b1d7d14d654c8b000ba616, G:/GCModeller/src/runtime/Darwinism/src/computing/batch//Host/Host.vb"

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

    '   Total Lines: 105
    '    Code Lines: 48
    ' Comment Lines: 42
    '   Blank Lines: 15
    '     File Size: 3.99 KB


    ' Module Host
    ' 
    '     Function: CreateSlave, GetCurrentThread, ParallelFor, SlaveTask, Solve
    ' 
    '     Sub: SetRange
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.CommandLine.InteropService
Imports Parallel
Imports Parallel.IpcStream

Public Module Host

    ''' <summary>
    ''' Create a slave task factory
    ''' </summary>
    ''' <returns></returns>
    Public Function CreateSlave(Optional debugPort As Integer? = Nothing,
                                Optional verbose As Boolean = False,
                                Optional ignoreError As Boolean = False,
                                Optional libpath As String = Nothing) As SlaveTask

        Return New SlaveTask(Host.GetCurrentThread(libpath), cli:=AddressOf Host.SlaveTask,
                             debugPort:=debugPort,
                             verbose:=verbose,
                             ignoreError:=ignoreError)
    End Function

    ''' <summary>
    ''' get ``batch.exe``
    ''' </summary>
    ''' <param name="libpath">
    ''' parameter for R# package: due to the reason of Rscript build R# package
    ''' will skip do file copy of some common modules, example like:
    ''' Microsoft.VisualBasic.Runtime.dll, so used the default location will
    ''' case the assembly file not found error, this parameter for config the
    ''' program location that contains the missing assembly file.
    ''' </param>
    ''' <returns></returns>
    Private Function GetCurrentThread(libpath As String) As InteropService
        Dim path As String = GetType(Host).Assembly.Location
        Dim assembly As String = path.BaseName & ".dll"
        Dim program As New InteropService(If(libpath.StringEmpty, path, $"{libpath}/{assembly}"))

#If NETCOREAPP Then
        Call program.SetDotNetCoreDll()
#End If

        Return program
    End Function

    ''' <summary>
    ''' create commandline argument for launch a slave task
    ''' </summary>
    ''' <param name="host"></param>
    ''' <param name="port"></param>
    ''' <returns></returns>
    Private Function SlaveTask(host As InteropService, port As Integer) As String
        Return $"/parallel --port {port} --master ""localhost"" /@set --internal_pipeline=true"
    End Function

    ''' <summary>
    ''' solve a parallel task
    ''' </summary>
    ''' <param name="master"></param>
    ''' <param name="port"></param>
    ''' <returns></returns>
    ''' 
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Friend Function Solve(master As String, port As Integer,
                          Optional timeout As Double = 15,
                          Optional verbose As Boolean = False) As Integer

        Return New TaskBuilder(port, master, timeout:=timeout, verbose:=verbose).Run
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <typeparam name="I"></typeparam>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="par"></param>
    ''' <param name="task"></param>
    ''' <param name="[loop]"></param>
    ''' <param name="args"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' this function required of ``netstat`` command has been installed when running on linux platform, 
    ''' or network error may be happends!
    ''' </remarks>
    <Extension>
    Public Function ParallelFor(Of I, T)(par As Argument, task As [Delegate], [loop] As I(), ParamArray args As SocketRef()) As IEnumerable(Of T)
        Dim foreach As New ParallelFor(Of T)(par)
        Dim loopVal As SocketRef() = [loop] _
            .Select(Function(obj) SocketRef.WriteBuffer(obj)) _
            .ToArray
        Dim run = batch.ParallelFor(Of T).CreateFunction(par, task, loopVal, args)

        Return foreach.GetResult(run)
    End Function

    <Extension>
    Friend Sub SetRange(ByRef args As SocketRef(), appendAfter As SocketRef(), offset As Integer)
        For i As Integer = 0 To appendAfter.Length - 1
            args(i + offset) = appendAfter(i)
        Next
    End Sub

End Module



