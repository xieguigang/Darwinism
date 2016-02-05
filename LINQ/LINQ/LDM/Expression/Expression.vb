Imports Microsoft.VisualBasic.LINQ.Framework
Imports Microsoft.VisualBasic.LINQ.Statements

Namespace LDM.Expression

    Public Class Expression

        ''' <summary>
        ''' An object element in the target query collection.(目标待查询集合之中的一个元素)
        ''' </summary>
        ''' <remarks></remarks>
        Public Property var As FromClosure
        ''' <summary>
        ''' Target query collection expression, this can be a file path or a database connection string.
        ''' (目标待查询集合，值可以为一个文件路径或者数据库连接字符串)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property source As InClosure
        ''' <summary>
        ''' A read only object collection which were construct by the LET statement token in the LINQ statement.
        ''' (使用Let语句所构造出来的只读对象类型的对象申明集合)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PreDeclare As LetClosure()
        ''' <summary>
        ''' Where test condition for the query.(查询所使用的Where条件测试语句)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Where As WhereClosure
        Public Property AfterDeclare As LetClosure()
        ''' <summary>
        ''' A expression for return the query result.(用于生成查询数据返回的语句)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SelectClosure As SelectClosure

        Sub New(statement As LINQStatement, registry As TypeRegistry)
            var = New FromClosure(statement.var, registry)
            source = New InClosure(statement.source)
            PreDeclare = statement.PreDeclare.ToArray(Function(x) New LetClosure(x))
            Where = New WhereClosure(statement.Where)
            AfterDeclare = statement.AfterDeclare.ToArray(Function(x) New LetClosure(x))
            SelectClosure = New SelectClosure(statement.SelectClosure)
        End Sub
    End Class
End Namespace