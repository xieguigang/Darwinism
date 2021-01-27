#Region "Microsoft.VisualBasic::655f57f7e586e30dfa74a1335af88f29, LINQ\LINQ\Interpreter\Expressions\FuncEval.vb"

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

    '     Class FuncEval
    ' 
    '         Properties: func, parameters
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: Exec, ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Runtime

Namespace Interpreter.Expressions

    Public Class FuncEval : Inherits Expression

        Public Property func As Expression
        Public Property parameters As Expression()

        Sub New(func As Expression, parameters As IEnumerable(Of Expression))
            Me.func = func
            Me.parameters = parameters.ToArray
        End Sub

        Public Overrides Function Exec(context As ExecutableContext) As Object
            Dim invoke As Object = func.Exec(New ExecutableContext With {.env = context.env, .throwError = False})

            If invoke Is Nothing Then
                If TypeOf func Is Literals Then
                    invoke = context.env.FindInvoke(func.Exec(Nothing))
                ElseIf TypeOf func Is SymbolReference Then
                    invoke = context.env.FindInvoke(DirectCast(func, SymbolReference).symbolName)
                End If
            Else
                Throw New NotImplementedException
            End If

            Dim args As New List(Of Object)

            For Each item In parameters
                args.Add(item.Exec(context))
            Next

            Dim result As Object = DirectCast(invoke, Callable).Evaluate(args.ToArray)

            Return result
        End Function

        Public Overrides Function ToString() As String
            Return $"{func}({parameters.JoinBy(", ")})"
        End Function
    End Class
End Namespace
