#Region "Microsoft.VisualBasic::d85eb77c40b16ad4cf6b7648d688467d, src\Darwinism\Env.vb"

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

    '   Total Lines: 38
    '    Code Lines: 18 (47.37%)
    ' Comment Lines: 16 (42.11%)
    '    - Xml Docs: 93.75%
    ' 
    '   Blank Lines: 4 (10.53%)
    '     File Size: 1.29 KB


    ' Module Env
    ' 
    '     Function: Set_libpath, Set_threads
    ' 
    ' /********************************************************************************/

#End Region

Imports Darwinism.DataScience.DataMining
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime.Interop

''' <summary>
''' the IPC parallel environment
''' </summary>
<Package("Environment")>
<RTypeExport("darwinism_argument", GetType(batch.Argument))>
Module Env

    ''' <summary>
    ''' set the parallel batch threads
    ''' </summary>
    ''' <param name="n_threads"></param>
    ''' <returns></returns>
    <ExportAPI("set_threads")>
    Public Function Set_threads(n_threads As Integer) As batch.Argument
        Call DarwinismEnvironment.SetThreads(n_threads)
        Return DarwinismEnvironment.GetEnvironmentArguments
    End Function

    ''' <summary>
    ''' ### Set framework library directory path
    ''' 
    ''' set a directory path that contains the scibasic.net framework 
    ''' runtime of the current parallel environment.
    ''' </summary>
    ''' <param name="libpath"></param>
    ''' <returns></returns>
    <ExportAPI("set_libpath")>
    Public Function Set_libpath(libpath As String) As batch.Argument
        Call DarwinismEnvironment.SetLibPath(libpath)
        Return DarwinismEnvironment.GetEnvironmentArguments
    End Function

End Module
