#Region "Microsoft.VisualBasic::fd0d2ea986c56be5e700779bea76b7d9, src\DataScience\DataMining\DarwinismEnvironment.vb"

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

    '   Total Lines: 46
    '    Code Lines: 28 (60.87%)
    ' Comment Lines: 12 (26.09%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 6 (13.04%)
    '     File Size: 1.26 KB


    ' Module DarwinismEnvironment
    ' 
    '     Function: GetEnvironmentArguments
    ' 
    '     Sub: SetEnvironment, SetLibPath, SetThreads
    ' 
    ' /********************************************************************************/

#End Region

Imports batch
Imports Darwinism.HPC.Parallel.IpcStream

Public Module DarwinismEnvironment

    ''' <summary>
    ''' a global parameters for the parallel computing
    ''' </summary>
    Dim par As Argument

    ''' <summary>
    ''' get a copy of the current parallel runtime environment arguments
    ''' </summary>
    ''' <returns></returns>
    Public Function GetEnvironmentArguments(Optional emit As StreamEmit = Nothing) As Argument
        If par Is Nothing Then
            Return Nothing
        Else
            Dim clone As Argument = par.Copy
            clone.emit = emit
            Return clone
        End If
    End Function

    Public Sub SetEnvironment(par As Argument)
        DarwinismEnvironment.par = par
    End Sub

    Public Sub SetThreads(n_threads As Integer)
        If par Is Nothing Then
            par = New Argument(n_threads)
        Else
            par.n_threads = n_threads
        End If
    End Sub

    ''' <summary>
    ''' set a directory path that contains the scibasic.net framework 
    ''' runtime of the current parallel environment.
    ''' </summary>
    ''' <param name="libpath"></param>
    Public Sub SetLibPath(libpath As String)
        If par Is Nothing Then
            par = New Argument(8) With {.libpath = libpath}
        Else
            par.libpath = libpath
        End If
    End Sub
End Module
