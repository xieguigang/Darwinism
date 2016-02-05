Imports System.CodeDom
Imports System.Reflection
Imports System.Text
Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic.LINQ.Framework.DynamicCode
Imports Microsoft.VisualBasic.LINQ.Framework.DynamicCode.VBC

Namespace LDM.Expression

    Public Class WhereClosure : Inherits Closure

        Friend Expression As CodeExpression
        Friend TestMethod As MethodInfo
        Sub New(source As Statements.Tokens.FromClosure)
            Call MyBase.New(source)


        End Sub

        Protected Overrides Function __parsing() As CodeExpression
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace