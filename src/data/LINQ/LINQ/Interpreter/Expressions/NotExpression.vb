#Region "Microsoft.VisualBasic::c0a5a1a4d0a3f7d3a1b6f4c2f0e1d9a7, src\data\LINQ\LINQ\Interpreter\Expressions\NotExpression.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed

    ' Summaries:

    '     Class NotExpression
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: Exec, ToString
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Runtime

Namespace Interpreter.Expressions

    ''' <summary>
    ''' 逻辑取反运算: ``NOT &lt;expression>``
    ''' </summary>
    Public Class NotExpression : Inherits Expression

        Friend ReadOnly value As Expression

        Sub New(value As Expression)
            Me.value = value
        End Sub

        ''' <summary>
        ''' <c>Nothing</c>值在逻辑运算之中被视为<c>False</c>，所以``not Nothing``的结果为<c>True</c>
        ''' </summary>
        ''' <param name="context"></param>
        ''' <returns></returns>
        Public Overrides Function Exec(context As ExecutableContext) As Object
            Return Not BinaryExpression.CBoolValue(value.Exec(context))
        End Function

        Public Overrides Function ToString() As String
            Return $"not {value}"
        End Function
    End Class
End Namespace
