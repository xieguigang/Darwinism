Imports System.Reflection
Imports Microsoft.VisualBasic.Scripting.SymbolBuilder
#If netcore5 = 1 Then
Imports Microsoft.VisualBasic.ApplicationServices.Development.NetCore5
#End If
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

    Public Function GetMethodTarget() As Object
        Dim type As Type = provideType()

        If VBLanguage.IsValidVBSymbolName(name) Then
            Return Nothing
        Else
            Return Activator.CreateInstance(type)
        End If
    End Function

    Private Function provideType() As Type
        Return type.GetType(knownFirst:=True, searchPath:={filepath})
    End Function

    Public Function GetMethod() As MethodInfo
        Dim type As Type = provideType()

#If netcore5 = 1 Then
        Call deps.TryHandleNetCore5AssemblyBugs(package:=type)
#End If

        If Not VBLanguage.IsValidVBSymbolName(name) Then
            For Each method As MethodInfo In CType(type, System.Reflection.TypeInfo).DeclaredMethods
                If (Not method.IsStatic) AndAlso method.Name = name Then
                    Return method
                End If
            Next
        Else
            For Each method As MethodInfo In CType(type, System.Reflection.TypeInfo).DeclaredMethods
                If method.IsStatic AndAlso method.Name = name Then
                    Return method
                End If
            Next
        End If

        Return Nothing
    End Function

    Public Overrides Function ToString() As String
        Return $"{type}::{name}"
    End Function
End Class