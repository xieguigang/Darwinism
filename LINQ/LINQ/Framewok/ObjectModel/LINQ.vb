Imports Microsoft.VisualBasic.LINQ.LDM.Expression
Imports Microsoft.VisualBasic.LINQ.Script
Imports Microsoft.VisualBasic.Linq.LDM.Statements
Imports Microsoft.VisualBasic.Linq.LDM.Statements.Tokens

Namespace Framework.ObjectModel

    ''' <summary>
    ''' LINQ查询表达式的对象模型
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Linq : Implements System.IDisposable

        Protected Friend StatementInstance As Object
        Protected Friend Test As System.Func(Of Boolean)
        Protected Friend SetObject As System.Func(Of Object, Boolean)
        Protected Friend SelectConstruct As System.Func(Of Object)
        Protected Friend Statement As LINQStatement
        Protected Friend source As Object()
        Protected Friend FrameworkRuntime As DynamicsRuntime

        Sub New(expression As Expression, Runtime As DynamicsRuntime)
            Me.StatementInstance = Statement.CreateInstance  'Create a instance for the LINQ entity and intialzie the components
            '  Me.Test = Function() Statement.Where.TestMethod.Invoke(StatementInstance, Nothing) 'Construct the Lambda expression
            '  Me.SetObject = Function(p As Object) Statement.var.SetObject.Invoke(StatementInstance, {p})
            Me.SelectConstruct = Function() Statement.SelectClosure.SelectMethod.Invoke(StatementInstance, Nothing)
            Me.source = Linq.GetCollection(Statement, Runtime)
            Me.Statement = Statement
            Me.FrameworkRuntime = Runtime
        End Sub

        Protected Friend Shared Function GetCollection(Statement As LINQStatement, Runtime As DynamicsRuntime) As Object()
            If Statement.source.Type = SourceTypes.FileURI Then
                '    Return Statement.source.ILINQCollectin.GetResource(Statement.source.Value)
            Else
                '返回运行时环境中的对象集合
                '   Return Runtime.GetResource(Statement.source)
            End If
        End Function

        Public Overridable Function EXEC() As IEnumerable
            Dim LQuery = From [Object] As Object In source
                         Let f As Boolean = SetObject([Object])
                         Where True = Test()
                         Let t As Object = SelectConstruct()
                         Select t     'Build a LINQ query object model using the constructed elements
            Return LQuery 'Return the query result
        End Function

        Public Overrides Function ToString() As String
            Return Statement.ToString
        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 检测冗余的调用

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO:  释放托管状态(托管对象)。
                End If

                ' TODO:  释放非托管资源(非托管对象)并重写下面的 Finalize()。
                ' TODO:  将大型字段设置为 null。
            End If
            Me.disposedValue = True
        End Sub

        ' TODO:  仅当上面的 Dispose( disposing As Boolean)具有释放非托管资源的代码时重写 Finalize()。
        'Protected Overrides Sub Finalize()
        '    ' 不要更改此代码。    请将清理代码放入上面的 Dispose( disposing As Boolean)中。
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' Visual Basic 添加此代码是为了正确实现可处置模式。
        Public Sub Dispose() Implements IDisposable.Dispose
            ' 不要更改此代码。    请将清理代码放入上面的 Dispose (disposing As Boolean)中。
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class
End Namespace