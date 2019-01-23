Imports System.Runtime.CompilerServices

''' <summary>
''' Docker image name
''' </summary>
Public Class Image

    Public Property Publisher As String
    Public Property Package As String

    Public Shared Function ParseEntry(text As String) As Image
        With text.Trim.Split("/"c)
            Dim user$, name$

            If .Length = 1 Then
                user = Nothing
                name = .ElementAt(0)
            Else
                user = .ElementAt(0)
                name = .ElementAt(1)
            End If

            Return New Image With {
                .Package = name,
                .Publisher = user
            }
        End With
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Overrides Function ToString() As String
        If Publisher.StringEmpty Then
            Return Package
        Else
            Return $"{Publisher}/{Package}"
        End If
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Narrowing Operator CType(img As Image) As String
        Return img.ToString
    End Operator
End Class
