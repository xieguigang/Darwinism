Imports System.Text

Public Module LinqClosure

    Public Function BuildClosure(x As String, type As Type, preLetClosures As IEnumerable(Of String), afterLetClosures As IEnumerable(Of String), projects As IEnumerable(Of String), Optional where As String = "") As String
        Return BuildClosure(x, type.FullName, preLetClosures, afterLetClosures, projects, where)
    End Function

    Public Function BuildClosure(x As String, type As String, preLetClosures As IEnumerable(Of String), afterLetClosures As IEnumerable(Of String), projects As IEnumerable(Of String), Optional where As String = "") As String
        Dim lBuilder As New StringBuilder()

        Call lBuilder.AppendLine("Namespace ___linqClosure")
        Call lBuilder.AppendLine("Public Class Linq")

        Call lBuilder.AppendLine($"Public Shared Function Project({x} As {type}) As LinqValue")

        If Not preLetClosures Is Nothing Then
            For Each line As String In preLetClosures
                Call lBuilder.AppendLine("Dim " & line)
            Next
        End If

        If Not String.IsNullOrEmpty(where) Then
            Call lBuilder.AppendLine($"If Not {where} Then")
            Call lBuilder.AppendLine($"Return {GetType(LinqValue).FullName}.Unavailable()")
            Call lBuilder.AppendLine("End If")
        End If

        If Not afterLetClosures Is Nothing Then
            For Each line As String In afterLetClosures
                Call lBuilder.AppendLine("Dim " & line)
            Next
        End If

        Call lBuilder.AppendLine("Dim obj As Object = " & __getProjects(projects))
        Call lBuilder.AppendLine("Return obj")

        Call lBuilder.AppendLine("End Function")

        Call lBuilder.AppendLine("End Class")
        Call lBuilder.AppendLine("End Namespace")

        Return lBuilder.ToString
    End Function

    Private Function __getProjects(source As IEnumerable(Of String)) As String
        Dim projects As String() = source.ToArray

        If projects.Length = 1 Then
            Return projects(Scan0)
        Else
            Dim anym As StringBuilder = New StringBuilder("New With {" & vbCrLf)
            Call anym.AppendLine(String.Join(", ", projects.ToArray(Function(s) __property(s))))
            Call anym.AppendLine("}")

            Return anym.ToString
        End If
    End Function

    Private Function __property(prop As String) As String
        Dim pos As Integer = InStr(prop, "=")

        If pos = 0 Then
            Return $".{prop} = {prop}"
        Else
            Dim name As String = Mid(prop, 1, pos - 1).Trim
            Dim value As String = Mid(prop, pos + 1).Trim
            Return $".{name} = {value}"
        End If
    End Function

    Public Delegate Function IProject(x As Object) As LinqValue
End Module

Public NotInheritable Class Linq

    ''' <summary>
    ''' From X As &lt;Type> In $source Let a = &lt;Expression> let b = &lt;Expression> Where &lt;Expression> Let c = &lt;Expression> Select X,a,b,c
    ''' </summary>
    ''' <param name="x"></param>
    ''' <returns></returns>
    Public Shared Function Project(x As Object) As LinqValue
        Dim a '= <Expression>
        Dim b '= <Expression> 

        If Not True Then
            Return LinqValue.Unavailable()
        End If

        Dim c '= <Expression> 

#Disable Warning
        Dim obj As Object = New With {.x = x, .A = a, .b = b, .c = c}
#Enable Warning

        Return New LinqValue(obj)
    End Function
End Class