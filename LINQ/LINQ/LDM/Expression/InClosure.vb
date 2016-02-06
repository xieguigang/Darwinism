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

        ''' <summary>
        ''' ILINQCollection对象的实例
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ILINQCollection As ILinqProvider

        Public ReadOnly Property IsParallel As Boolean
            Get
                Return False
            End Get
        End Property

        ''' <summary>
        ''' The file io object url or a object collection reference in the LINQ Frameowrk runtime. 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Value As String
            Get
                '       Return _original
            End Get
        End Property

        Sub New(source As Statements.Tokens.InClosure)
            Call MyBase.New(source)


        End Sub

        Public Overrides Function ToString() As String
            'If Type = CollectionTypes.File Then
            '    Return String.Format("(File) {0}", Me._original)
            'Else
            '    Return Type.ToString
            'End If
        End Function

        Protected Overrides Function __parsing() As CodeExpression

        End Function
    End Class
End Namespace