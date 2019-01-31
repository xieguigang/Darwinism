#Region "Microsoft.VisualBasic::b24a8c5cdacd2244bca9326de2226f35, RQL\StorageTek\LinqSource.vb"

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

    '     Module LinqSource
    ' 
    '         Function: Source
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Reflection
Imports sciBASIC.ComputingServices.Linq.Framework.Provider

Namespace StorageTek

    Public Module LinqSource

        Const NO_LINQ As String = "The target function pointer handle have not defined any Linq source entry yet..."

        ''' <summary>
        ''' 生成Linq数据源
        ''' </summary>
        ''' <param name="res"></param>
        ''' <param name="handle"></param>
        ''' <returns></returns>
        Public Function Source(res As String, handle As GetLinqResource) As EntityProvider
            Dim mINFO As MethodInfo = handle.Method
            Dim Linq As TypeEntry = TypeRegistry.ParsingEntry(mINFO)
            If Linq Is Nothing Then
                Dim ex As New Exception(NO_LINQ)
                ex = New DataException(mINFO.GetFullName(True), ex)
                Throw ex
            End If
            Return New EntityProvider(Linq, res)
        End Function
    End Module
End Namespace
