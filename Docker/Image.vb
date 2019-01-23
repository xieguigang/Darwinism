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
    Public Shared Widening Operator CType(repo As String) As Image
        Return ParseEntry(repo)
    End Operator

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Narrowing Operator CType(img As Image) As String
        Return img.ToString
    End Operator
End Class

''' <summary>
''' 共享文件夹
''' </summary>
Public Class Mount

    ''' <summary>
    ''' 宿主机内的本地文件夹全路径
    ''' </summary>
    Public Property local As String
    ''' <summary>
    ''' 虚拟机内的文件路径,共享文件夹将会在虚拟机内被挂载到这个路径上面
    ''' </summary>
    Public Property virtual As String

    ''' <summary>
    ''' ``local:virtual``
    ''' </summary>
    ''' <returns></returns>
    Public Overrides Function ToString() As String
        Return $"{local}:{virtual}"
    End Function
End Class