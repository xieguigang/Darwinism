Imports System.CodeDom
Imports System.Reflection
Imports System.Text
Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic.LINQ.Framework.DynamicCode
Imports Microsoft.VisualBasic.LINQ.Framework.DynamicCode.VBC

Namespace LDM.Expression

    ''' <summary>
    ''' 测试的是一个对象
    ''' </summary>
    ''' 
    Public Class WhereClosure : Inherits Closure

        Friend Expression As CodeExpression
        Friend TestMethod As MethodInfo

        Sub New(source As Statements.Tokens.WhereClosure)
            Call MyBase.New(source)

        End Sub

        Protected Overrides Function __parsing() As CodeExpression
            Throw New NotImplementedException()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="obj">
        ''' From x In $source let y =func(x) Where test(x,y) Select ctor(x,y)
        ''' Where条件所测试的实际上是由Where前面的x和y通过vbc所构成的一个新的临时匿名类型的对象
        ''' </param>
        ''' <returns></returns>
        Public Function WhereTest(obj As Object) As Boolean

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="type">X的类型</param>
        ''' <param name="expr">Where逻辑表达式</param>
        ''' <returns></returns>
        Public Shared Function CreateLinqWhere(type As Type, expr As String) As WhereClosure

        End Function
    End Class
End Namespace