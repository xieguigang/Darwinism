#Region "Microsoft.VisualBasic::6b9b62435842858d057df49f23e18377, LINQ\Drivers\NetCDF\CDFDataSet.vb"

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

    ' Class CDFDataSet
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: ReadFromUri
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Runtime.Drivers

<DriverFlag("dataframe")>
Public Class CDFDataSet : Inherits DataSourceDriver

    Public Sub New(arguments() As String)
        MyBase.New(arguments)
    End Sub

    Public Overrides Function ReadFromUri(uri As String) As IEnumerable(Of Object)
        Throw New NotImplementedException()
    End Function
End Class

