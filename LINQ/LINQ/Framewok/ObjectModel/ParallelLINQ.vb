Imports Microsoft.VisualBasic.LINQ.LDM.Expression
Imports Microsoft.VisualBasic.LINQ.Script

Namespace Framework.ObjectModel

    ''' <summary>
    ''' 并行LINQ查询表达式的对象模型
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ParallelLINQ : Inherits LINQ

        Sub New(expression As Expression, FrameworkRuntime As DynamicsRuntime)
            Call MyBase.New(expression, Runtime:=FrameworkRuntime)
        End Sub

        Public Overrides Function EXEC() As IEnumerable
            Dim LQuery = From [Object] As Object In source.AsParallel
                         Let f As Boolean = SetObject([Object])
                         Where True = Test()
                         Let t As Object = SelectConstruct()
                         Select t     'Build a LINQ query object model using the constructed elements
            Return LQuery.ToArray 'Return the query result
        End Function
    End Class
End Namespace