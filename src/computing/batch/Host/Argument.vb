#Region "Microsoft.VisualBasic::cb19198829a8994b13d21e8681cdbe5c, src\computing\batch\Host\Argument.vb"

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

    '   Total Lines: 51
    '    Code Lines: 36 (70.59%)
    ' Comment Lines: 7 (13.73%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 8 (15.69%)
    '     File Size: 1.73 KB


    ' Class Argument
    ' 
    '     Properties: debugPort, emit, ignoreError, libpath, n_threads
    '                 thread_interval, verbose
    ' 
    '     Constructor: (+2 Overloads) Sub New
    '     Function: Copy, CreateHost, ToString
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Darwinism.HPC.Parallel
Imports Darwinism.HPC.Parallel.IpcStream
Imports Microsoft.VisualBasic.MIME.application.json
Imports Microsoft.VisualBasic.Serialization.JSON
Imports options = Darwinism.HPC.Parallel.Extensions

''' <summary>
''' the configuration of the Darwinism IPC parallel environment
''' </summary>
Public Class Argument

    Public Property debugPort As Integer? = Nothing
    ''' <summary>
    ''' apply the verbose option to <see cref="options.SetVerbose(Boolean)"/>
    ''' </summary>
    ''' <returns></returns>
    Public Property verbose As Boolean = False
    Public Property ignoreError As Boolean = False
    Public Property n_threads As Integer = 32
    Public Property thread_interval As Integer = 300
    Public Property libpath As String = Nothing
    Public Property emit As StreamEmit = Nothing

    Sub New()
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Sub New(n_threads As Integer)
        Me.n_threads = n_threads
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Overrides Function ToString() As String
        Return Me.GetJson(simpleDict:=True)
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function Copy() As Argument
        Return ToString.LoadJSON(Of Argument)
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function CreateHost() As SlaveTask
        Call options.SetVerbose(verbose)
        Return Host.CreateSlave(debugPort,
                                ignoreError:=ignoreError,
                                libpath:=libpath)
    End Function

End Class
