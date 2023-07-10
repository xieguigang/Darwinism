#Region "Microsoft.VisualBasic::915d0087daf37e7e27f031dfbcfd2694, LINQ\LINQ\Interpreter\Query\DataSoruce\SymbolIterator.vb"

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

    '     Class SymbolIterator
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: PopulatesData
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Runtime
Imports Microsoft.VisualBasic.Emit.Delegates

Namespace Interpreter.Query

    Public Class SymbolIterator : Inherits DataSet

        ReadOnly objVal As Object

        Sub New(query As QueryExpression, obj As Object, env As Environment)
            MyBase.New(query.symbol, env)
            Me.objVal = obj
        End Sub

        Public Overrides Iterator Function PopulatesData() As IEnumerable(Of Object)
            If objVal Is Nothing Then
                Throw New NullReferenceException
            End If

            If objVal.GetType.IsArray Then
                With DirectCast(objVal, Array)
                    For i As Integer = 0 To .Length - 1
                        Yield .GetValue(i)
                    Next
                End With
            ElseIf objVal.GetType.ImplementInterface(GetType(IEnumerable)) Then
                For Each item As Object In DirectCast(objVal, IEnumerable)
                    Yield item
                Next
            Else
                Throw New InvalidCastException
            End If
        End Function
    End Class

End Namespace
