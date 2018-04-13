#Region "Microsoft.VisualBasic::9c265c0ee6f7456eee307ab41d5c6764, LINQ\LINQ\APP\Program.vb"

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

    ' Module Program
    ' 
    '     Function: __exeEmpty, Main
    ' 
    ' /********************************************************************************/

#End Region

Imports sciBASIC.ComputingServices.Linq.LDM
Imports sciBASIC.ComputingServices.Linq.Framework
Imports sciBASIC.ComputingServices.Linq.Framework.Provider
Imports sciBASIC.ComputingServices.Linq.Script

Module Program

    ''' <summary>
    ''' DO_NOTHING
    ''' </summary>
    ''' <remarks></remarks>
    Public Function Main() As Integer
        Return GetType(CLI).RunCLI(App.CommandLine, AddressOf __exeEmpty)
    End Function

    Private Function __exeEmpty() As Integer
        Call Console.WriteLine("{0}!{1}", GetType(Program).Assembly.Location, GetType(DynamicsRuntime).FullName)
        Return 0
    End Function
End Module
