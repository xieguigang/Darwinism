#Region "Microsoft.VisualBasic::3c21de972f7dbe539786d772e666e80e, G:/GCModeller/src/runtime/Darwinism/src/data/LINQ/LINQ//Interpreter/Expressions/Keywords/Options/PipelineKeyword.vb"

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

    '   Total Lines: 33
    '    Code Lines: 21
    ' Comment Lines: 5
    '   Blank Lines: 7
    '     File Size: 1.30 KB


    '     Class PipelineKeyword
    ' 
    '         Function: FixLiteral
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.My.JavaScript
Imports LINQ.Runtime

Namespace Interpreter.Expressions

    Public MustInherit Class PipelineKeyword : Inherits KeywordExpression

        Public MustOverride Overloads Function Exec(result As IEnumerable(Of JavaScriptObject), context As ExecutableContext) As IEnumerable(Of JavaScriptObject)

        ''' <summary>
        ''' 将字符串常量表示转换为变量引用
        ''' </summary>
        ''' <param name="expr"></param>
        ''' <returns></returns>
        Protected Shared Function FixLiteral(expr As Expression) As Expression
            If TypeOf expr Is BinaryExpression Then
                Dim bin As BinaryExpression = DirectCast(expr, BinaryExpression)

                bin.left = FixLiteral(bin.left)
                bin.right = FixLiteral(bin.right)
            ElseIf TypeOf expr Is Literals Then
                expr = New SymbolReference(DirectCast(expr, Literals).value)
            ElseIf TypeOf expr Is FuncEval Then
                DirectCast(expr, FuncEval).parameters = DirectCast(expr, FuncEval).parameters _
                    .Select(AddressOf FixLiteral) _
                    .ToArray
            End If

            Return expr
        End Function

    End Class
End Namespace
