Imports System.CodeDom
Imports Microsoft.VisualBasic.LINQ.Framework
Imports Microsoft.VisualBasic.LINQ.Framework.Provider
Imports Microsoft.VisualBasic.LINQ.Statements.Tokens

Namespace LDM.Expression

    ''' <summary>
    ''' 表示目标对象的数据集合的文件路径或者内存对象的引用
    ''' </summary>
    ''' <remarks></remarks>
    Public Class InClosure : Inherits Closure

        Public ReadOnly Property Type As SourceTypes

        Public ReadOnly Property IsParallel As Boolean

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="source">[in] var -> parallel</param>
        Sub New(source As Statements.Tokens.InClosure)
            Call MyBase.New(source)


        End Sub

        Protected Overrides Function __parsing() As CodeExpression

        End Function
    End Class
End Namespace