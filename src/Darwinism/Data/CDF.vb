#Region "Microsoft.VisualBasic::5aec493da6f01aa78c8dfc82ce350eea, G:/GCModeller/src/runtime/Darwinism/src/Darwinism//Data/CDF.vb"

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

    '   Total Lines: 39
    '    Code Lines: 21
    ' Comment Lines: 13
    '   Blank Lines: 5
    '     File Size: 1.09 KB


    ' Module CDF
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: getData, open, vars
    ' 
    ' /********************************************************************************/

#End Region

Imports CDF.PInvoke
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData

<Package("CDF")>
Module CDF

    Sub New()
        Call VBDebugger.EchoLine("the netcdf module only works for windows platform currently!")
    End Sub

    ''' <summary>
    ''' open a connection to the netcdf file
    ''' </summary>
    ''' <param name="file"></param>
    ''' <returns>
    ''' the file stream id
    ''' </returns>
    <ExportAPI("open")>
    Public Function open(file As String) As DataReader
        Return New DataReader(file)
    End Function

    <ExportAPI("vars")>
    Public Function vars(nc As DataReader) As String()
        Return nc.vars
    End Function

    ''' <summary>
    ''' get variable data from given file
    ''' </summary>
    ''' <param name="nc"></param>
    ''' <param name="name"></param>
    ''' <returns></returns>
    <ExportAPI("var_data")>
    Public Function getData(nc As DataReader, name As String) As Object
        Return nc.GetData(name)
    End Function
End Module

