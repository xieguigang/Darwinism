Imports sciBASIC.ComputingServices.Linq.Framework.DynamicCode
Imports sciBASIC.ComputingServices.Linq.LDM.Statements
Imports sciBASIC.ComputingServices.Linq.Script

Namespace Framework.ObjectModel

    ''' <summary>
    ''' 并行LINQ查询表达式的对象模型
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ParallelLinq : Inherits Linq

        Sub New(Expr As LinqStatement, FrameworkRuntime As DynamicsRuntime)
            Call MyBase.New(Expr, Runtime:=FrameworkRuntime)
        End Sub

        Public Overrides Function EXEC() As IEnumerable
            Dim Linq = (From x As Object In __getSource.AsParallel
                        Let value As LinqValue = __project(x)
                        Where value.IsTrue
                        Select value.Projects)
            Return Linq
        End Function
    End Class
End Namespace