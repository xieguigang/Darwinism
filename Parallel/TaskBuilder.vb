Imports System.Reflection
Imports Microsoft.VisualBasic.ComponentModel

Public Class TaskBuilder : Implements ITaskDriver

    ReadOnly masterPort As Integer

    Sub New(port As Integer)
        masterPort = port
    End Sub

    Public Function Run() As Integer Implements ITaskDriver.Run
        Dim task As IDelegate = GetMethod()
        Dim n As Integer
        Dim args As New List(Of Object)

        For i As Integer = 0 To n - 1
            args.Add(GetArgumentValue(i))
        Next

        Dim api As MethodInfo = task.GetMethod
        Dim params As ParameterInfo() = api.GetParameters

        For i As Integer = n To params.Length - 1
            If Not params(i).IsOptional Then
                Return PostError(New Exception($"missing parameter value for [{i}]{params(i).Name}!"))
            Else
                args.Add(params(i).DefaultValue)
            End If
        Next

        Call PostFinished(api.Invoke(Nothing, args.ToArray))

        Return 0
    End Function

    Private Function GetMethod() As IDelegate

    End Function

    Private Function GetArgumentValue(i As Integer) As Object

    End Function

    Private Function PostError(err As Exception) As Integer


        Return 500
    End Function

    Private Sub PostFinished(result As Object)

    End Sub
End Class
