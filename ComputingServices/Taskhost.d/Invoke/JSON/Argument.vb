Imports System.Reflection
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace TaskHost
    ''' <summary>
    ''' Json value of the function parameter, and the type information is also included in this property.
    ''' </summary>
    ''' <remarks>
    ''' 不推荐使用泛型作为参数值
    ''' </remarks>
    Public Class Argument

        ''' <summary>
        ''' <see cref="TypeInfo.FullName"/>
        ''' </summary>
        ''' <returns><see cref="TypeInfo.FullName"/></returns>
        ''' <remarks>
        ''' 在这里设置这个属性的原因是为了可以直接调用<see cref="GetValue()"/>函数来完成反序列化操作
        ''' </remarks>
        Public Property type As String
        ''' <summary>
        ''' Json string
        ''' </summary>
        ''' <returns></returns>
        Public Property value As String

        Sub New()
        End Sub

        ''' <summary>
        ''' Creates the function remote calls one of its parameter value. 
        ''' </summary>
        ''' <param name="o"></param>
        Sub New(o As Object, Optional type As Type = Nothing)
            type = If(type, o.GetType)

            Me.type = type.FullName
            Me.value = JsonContract.GetObjectJson(o, type)
        End Sub

        Public Overrides Function ToString() As String
            Return $"[{type.Split("."c).Last}]  " & value
        End Function

        Public Function GetValue() As Object
            Dim type As Type = Type.GetType(Me.type, True, False)
            Dim obj As Object = JsonContract.LoadObject(value, type)
            Return obj
        End Function
    End Class
End Namespace