Imports System.Runtime.CompilerServices

Namespace TaskHost

    Public Module Extensions

        Public ReadOnly Property PublicShared As System.Reflection.BindingFlags =
            System.Reflection.BindingFlags.Public Or
            System.Reflection.BindingFlags.Static

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="type"></param>
        ''' <param name="name">NameOf</param>
        ''' <returns></returns>
        <Extension> Public Function [AddressOf](type As Type, name As String) As InvokeInfo
            Dim method = type.GetMethod(name, bindingAttr:=PublicShared)
            Dim info As New InvokeInfo With {
                .Assembly = FileIO.FileSystem.GetFileInfo(type.Assembly.Location).Name,
                .Name = method.Name,
                .Type = type.FullName
            }
            Return info
        End Function

        <Extension> Public Function Invoke(info As InvokeInfo, host As TaskHost) As Object
            If host Is Nothing Then
                Return TaskInvoke.TryInvoke(info)
            Else
                Dim rtvl = host.Invoke(info)
                Dim entry = info.GetMethod
                Return rtvl.GetValue(entry.ReturnType)
            End If
        End Function

        ''' <summary>
        ''' DirectCast
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="info"></param>
        ''' <param name="host"></param>
        ''' <returns></returns>
        <Extension> Public Function Invoke(Of T)(info As InvokeInfo, host As TaskHost) As T
            Return DirectCast(info.Invoke(host), T)
        End Function
    End Module
End Namespace