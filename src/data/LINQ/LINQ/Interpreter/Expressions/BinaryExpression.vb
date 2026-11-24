#Region "Microsoft.VisualBasic::9fc91ac66b882315f704f910c2ab57c2, src\data\LINQ\LINQ\Interpreter\Expressions\BinaryExpression.vb"

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

    '   Total Lines: 55
    '    Code Lines: 44 (80.00%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 11 (20.00%)
    '     File Size: 1.66 KB


    '     Class BinaryExpression
    ' 
    '         Properties: LikeValueAssign
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: Exec, ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Runtime

Namespace Interpreter.Expressions

    Public Class BinaryExpression : Inherits Expression

        Friend left, right As Expression
        Dim op As String

        ''' <summary>
        ''' the operator token of the binary expression, example as: ``+``, ``&gt;=``, ``and``
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property [Operator] As String
            Get
                Return op
            End Get
        End Property

        ''' <summary>
        ''' 左值是否为一个变量引用，即当前表达式是否为一个赋值表达式
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property LikeValueAssign As Boolean
            Get
                If op <> "=" Then
                    Return False
                Else
                    Return TypeOf left Is SymbolReference
                End If
            End Get
        End Property

        Sub New(left As Expression, right As Expression, op As String)
            Me.left = left
            Me.right = right
            Me.op = If(op, "").ToLower
        End Sub

        Public Overrides Function Exec(context As ExecutableContext) As Object
            Dim x As Object
            Dim y As Object

            Select Case op
                Case "and"
                    x = left.Exec(context)

                    If Not CBoolValue(x) Then
                        Return False
                    Else
                        Return CBoolValue(right.Exec(context))
                    End If
                Case "or"
                    x = left.Exec(context)

                    If CBoolValue(x) Then
                        Return True
                    Else
                        Return CBoolValue(right.Exec(context))
                    End If
                Case Else
                    x = left.Exec(context)
                    y = right.Exec(context)
            End Select

            Select Case op
                Case "+" : Return x + y
                Case "-" : Return x - y
                Case "*" : Return x * y
                Case "/" : Return x / y
                Case "^" : Return x ^ y
                Case ">" : Return x > y
                Case "<" : Return x < y
                Case "=" : Return x = y

                Case "&" : Return x & y

                Case ">=" : Return x >= y
                Case "<=" : Return x <= y
                Case "<>" : Return x <> y

                Case Else
                    Throw New NotImplementedException
            End Select
        End Function

        ''' <summary>
        ''' 将逻辑运算的中间值转换为布尔值，<c>Nothing</c>值在逻辑运算之中被视为<c>False</c>
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        Friend Shared Function CBoolValue(value As Object) As Boolean
            If value Is Nothing Then
                Return False
            ElseIf TypeOf value Is Boolean Then
                Return DirectCast(value, Boolean)
            Else
                Return CBool(value)
            End If
        End Function

        Public Overrides Function ToString() As String
            Return $"({left} {op} {right})"
        End Function
    End Class
End Namespace
