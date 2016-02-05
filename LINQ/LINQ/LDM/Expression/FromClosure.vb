Imports System.CodeDom
Imports Microsoft.VisualBasic.LINQ.Framework
Imports Microsoft.VisualBasic.LINQ.Framework.DynamicCode
Imports Microsoft.VisualBasic.LINQ.Framework.DynamicCode.VBC
Imports Microsoft.VisualBasic.LINQ.Framework.LQueryFramework

Namespace LDM.Expression

    ''' <summary>
    ''' The init variable.
    ''' </summary>
    Public Class FromClosure : Inherits Closure

        ''' <summary>
        ''' 变量的名称
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Name As String
        ''' <summary>
        ''' 变量的类型标识符
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TypeId As String

        Public Property RegistryType As RegistryItem

        Friend SetObject As System.Reflection.MethodInfo

        Sub New(source As Statements.Tokens.FromClosure)
            Call MyBase.New(source)


        End Sub

        Public Overrides Function ToString() As String
            Return String.Format("Dim {0} As {1}", Name, TypeId)
        End Function

        Protected Overrides Function __parsing() As CodeExpression

        End Function
    End Class
End Namespace