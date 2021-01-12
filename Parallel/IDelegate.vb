Imports System.Reflection
Imports TypeInfo = Microsoft.VisualBasic.Scripting.MetaData.TypeInfo

Public Class IDelegate

    Public Property name As String
    Public Property type As TypeInfo
    ''' <summary>
    ''' the absolute path of the assembly module file that contains target delegate
    ''' </summary>
    ''' <returns></returns>
    Public Property filepath As String

    Sub New()
    End Sub

    Sub New(target As MethodInfo)
        type = New TypeInfo(target.DeclaringType)
        name = target.Name
        filepath = target.DeclaringType.Assembly.Location
    End Sub

    Sub New(target As [Delegate])
        Call Me.New(target.Method)
    End Sub

    Public Function GetMethod() As MethodInfo
        Dim type As Type = Me.type.GetType(searchPath:={filepath})
        Dim methods As MethodInfo() = type.GetMethods.Where(Function(m) m.IsStatic AndAlso m.Name = name).ToArray

        If methods.IsNullOrEmpty Then
            Return Nothing
        Else
            Return methods(Scan0)
        End If
    End Function
End Class