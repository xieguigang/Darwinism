Imports System.IO

Namespace IpcStream

    Public Class SocketRef

        Public Property address As String

        Public Shared Function WriteBuffer(target As Object, emit As StreamEmit) As SocketRef
            Dim stream As ObjectStream = emit.handleSerialize(target)
            Dim ref As SocketRef = CreateReference()

            Using file As Stream = ref.address.Open(FileMode.OpenOrCreate, doClear:=True, [readOnly]:=False)

            End Using

            Return ref
        End Function

        Public Overrides Function ToString() As String
            Return address
        End Function

        Public Shared Function CreateReference() As SocketRef
            Return App.GetAppSysTempFile(".sock", App.PID.ToHexString, prefix:="Parallel")
        End Function

        Public Shared Widening Operator CType(ref As String) As SocketRef
            Return New SocketRef With {.address = ref}
        End Operator
    End Class
End Namespace