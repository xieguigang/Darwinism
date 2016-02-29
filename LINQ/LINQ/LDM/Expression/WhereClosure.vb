Imports System.CodeDom
Imports System.Reflection
Imports System.Text
Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic.Linq.Statements.TokenIcer
Imports Microsoft.VisualBasic.Linq.Framework.DynamicCode
Imports Microsoft.VisualBasic.Linq.Framework.DynamicCode.VBC
Imports Microsoft.VisualBasic.Scripting.TokenIcer

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

        Sub New(expr As Func(Of Tokens), type As Type)
            Call MyBase.New(New Statements.Tokens.WhereClosure(expr))
        End Sub

        Protected Overrides Function __parsing() As CodeExpression

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
        ''' <param name="expr">
        ''' Where逻辑表达式，表达式内部的变量都看作为对前面的对象类型的属性的引用
        ''' $var看作为由前面的let和in语句所生成的匿名类型的对象实例的引用
        ''' </param>
        ''' <returns></returns>
        Public Shared Function CreateLinqWhere(type As Type, expr As String) As WhereClosure
            Dim tokens = Statements.TokenIcer.GetTokens(expr).TrimWhiteSpace.Parsing(stackT)
            Dim exp = tokens.Args.First ' Where里面只允许单条表达式语句
            Return New WhereClosure(exp, type)
        End Function
    End Class
End Namespace