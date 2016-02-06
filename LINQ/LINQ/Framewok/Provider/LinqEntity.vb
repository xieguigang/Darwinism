Imports System.Text.RegularExpressions

Namespace Framework.Provider

    ''' <summary>
    ''' LINQ entity type
    ''' </summary>
    ''' <remarks></remarks>
    <AttributeUsage(AttributeTargets.Class Or AttributeTargets.Struct, AllowMultiple:=False, Inherited:=True)>
    Public Class LinqEntity : Inherits System.Attribute

        Public ReadOnly Property Type As String

        Public Shared ReadOnly Property ILinqEntity As Type = GetType(LinqEntity)

        Sub New(type As String)
            Me.Type = type
        End Sub

        Public Overrides Function ToString() As String
            Return Type
        End Function

        ''' <summary>
        ''' 获取目标类型上的自定义属性中的LINQEntity类型对象中的EntityType属性值
        ''' </summary>
        ''' <param name="type"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetEntityType(type As Type) As String
            Dim attr As Object() = type.GetCustomAttributes(ILinqEntity, True)
            If attr.IsNullOrEmpty Then
                Return ""
            Else
                Return DirectCast(attr(Scan0), LinqEntity).Type
            End If
        End Function
    End Class
End Namespace