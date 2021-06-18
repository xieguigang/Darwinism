#Region "Microsoft.VisualBasic::550ac42be283838badad0e16ae7212fe, LINQ\LINQ\Interpreter\Query\DataSoruce\URIIteratorDriver.vb"

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

    '     Class URIIteratorDriver
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: PopulatesData
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Interpreter.Expressions
Imports LINQ.Runtime
Imports LINQ.Runtime.Drivers

Namespace Interpreter.Query

    Public Class URIIteratorDriver : Inherits DataSet

        ReadOnly uri As String

        Sub New(query As QueryExpression, uri As String, env As Environment)
            MyBase.New(query.symbol, env)
            Me.uri = uri
        End Sub

        Public Overrides Iterator Function PopulatesData() As IEnumerable(Of Object)
            Dim driver As DataSourceDriver = env.GlobalEnvir.GetDriverByCode(
                code:=symbolDeclare.type,
                arguments:=symbolDeclare.arguments
            )

            For Each item As Object In driver.ReadFromUri(uri)
                Yield item
            Next
        End Function
    End Class
End Namespace
