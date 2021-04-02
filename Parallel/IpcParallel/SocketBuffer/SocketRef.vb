Imports System.IO
Imports Microsoft.VisualBasic.MIME.application.json.BSON

Namespace IpcStream

    Public Class SocketRef

        Public Property address As String

        ''' <summary>
        ''' buffered object is <see cref="ObjectStream"/>
        ''' </summary>
        ''' <param name="target"></param>
        ''' <param name="emit"></param>
        ''' <returns></returns>
        Public Shared Function WriteBuffer(target As Object, Optional emit As StreamEmit = Nothing) As SocketRef
            Dim stream As ObjectStream
            Dim ref As SocketRef = CreateReference()

            If emit Is Nothing Then
                stream = New StreamEmit().handleSerialize(target)
            Else
                stream = emit.handleSerialize(target)
            End If

            Using file As Stream = ref.address.Open(FileMode.OpenOrCreate, doClear:=True, [readOnly]:=False)
                Call stream.Serialize(file)
                Call stream.Dispose()
            End Using

            Return ref
        End Function

        Public Function Open() As ObjectStream
            Using file As Stream = address.Open(FileMode.Open, doClear:=False, [readOnly]:=True)
                Return New ObjectStream(file)
            End Using
        End Function

        Public Overrides Function ToString() As String
            Return address
        End Function

        Public Shared Function GetSocket(stream As ObjectStream) As SocketRef
            Using file As Stream = stream.openMemoryBuffer
                Return BSONFormat.Load(file).CreateObject(GetType(SocketRef))
            End Using
        End Function

        Public Shared Function CreateReference() As SocketRef
            Return App.GetAppSysTempFile(".sock", App.PID.ToHexString, prefix:="Parallel")
        End Function

        Public Shared Widening Operator CType(ref As String) As SocketRef
            Return New SocketRef With {.address = ref}
        End Operator
    End Class
End Namespace