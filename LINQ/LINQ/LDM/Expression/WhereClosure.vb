Imports System.CodeDom
Imports System.Reflection
Imports System.Text
Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic.Linq.Statements.TokenIcer
Imports Microsoft.VisualBasic.Linq.Framework.DynamicCode
Imports Microsoft.VisualBasic.Linq.Framework.DynamicCode.VBC
Imports Microsoft.VisualBasic.Scripting.TokenIcer
Imports Microsoft.VisualBasic.CodeDOM_VBC

Namespace LDM.Expression

    ''' <summary>
    ''' 测试的是一个对象
    ''' </summary>
    ''' <remarks>
    ''' Where 测试的一个对象类型，对象的属性则是前面的In和Let所生成的变量
    ''' Where 对象里面则通过一个逻辑值的函数来测试对象
    ''' 
    ''' Public Module Where
    ''' 
    '''     Public Function Test(x As objectType) As Boolean
    '''         Return (......) X ' 由where生成的测试语句
    '''     End Function
    ''' 
    ''' End Module
    ''' </remarks>
    Public Class WhereClosure : Inherits Closure

        ''' <summary>
        ''' 有where生成的模块里面的一个测试函数的函数方法信息
        ''' </summary>
        Dim TestMethod As MethodInfo
        ''' <summary>
        ''' 前面的语句所生成的匿名类型的类型信息
        ''' </summary>
        Dim _typeINFO As Type

        Sub New(source As Statements.Tokens.WhereClosure)
            Call MyBase.New(source)
        End Sub

        Sub New(expr As Func(Of Tokens), type As Type)
            Call MyBase.New(New Statements.Tokens.WhereClosure(expr))
        End Sub

        ''' <summary>
        ''' 在这个函数里面生成测试函数之中的表达式，然后再由vbc生成模块类型
        ''' 函数只有一个参数，并且参数名为obj
        ''' </summary>
        ''' <returns></returns>
        Protected Overrides Function __parsing() As CodeExpression
            Dim expr As Func(Of Tokens) =
                MyBase._source.Source.ParsingStack.Args.First  ' 只能够有一个测试语句
        End Function

        Private Function __buildFunc() As CodeMemberMethod
            Dim [Function] As CodeMemberMethod =
                DeclareFunc("Test", New Dictionary(Of String, Type) From {{"obj", Me._typeINFO}}, GetType(Boolean))
            Call [Function].Statements.Add(LocalsInit("rtvl", GetType(Boolean), init:=False))
            Call [Function].Statements.Add(ValueAssign(LocalVariable("obj"), __parsing))
            Call [Function].Statements.Add([Return]("obj"))

            Return [Function]
        End Function

        Sub New(expr As Token(Of Tokens)(), type As Type)
            Call MyBase.New(New Statements.Tokens.WhereClosure(expr))
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="obj">
        ''' From x In $source let y =func(x) Where test(x,y) Select ctor(x,y)
        ''' Where条件所测试的实际上是由Where前面的x和y通过vbc所构成的一个新的临时匿名类型的对象
        ''' </param>
        ''' <returns></returns>
        Public Function WhereTest(obj As Object) As Boolean
            Return DirectCast(TestMethod.Invoke(Nothing, {obj}), Boolean)
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
            Dim tokens = Statements.TokenIcer.GetTokens(expr).TrimWhiteSpace
            Return New WhereClosure(tokens, type)
        End Function
    End Class
End Namespace