Imports System.IO
Imports Microsoft.VisualBasic.MIME.application.json
Imports Microsoft.VisualBasic.MIME.application.json.BSON
Imports Microsoft.VisualBasic.Scripting.MetaData

Namespace IpcStream

    Public Class StreamEmit

        ReadOnly toBuffers As New Dictionary(Of Type, toBuffer)
        ReadOnly loadBuffers As New Dictionary(Of Type, loadBuffer)

        Sub New()
            For Each [handle] In EmitHandler.PopulatePrimitiveHandles
                toBuffers(handle.target) = handle.emit
            Next
            For Each [handle] In EmitHandler.PopulatePrimitiveParsers
                loadBuffers(handle.target) = handle.emit
            Next
        End Sub

        Public Function Emit(Of T)(streamAs As Func(Of T, Stream)) As StreamEmit
            toBuffers(GetType(T)) = Function(obj) streamAs(obj)
            Return Me
        End Function

        Public Function Emit(Of T)(fromStream As Func(Of Stream, T)) As StreamEmit
            loadBuffers(GetType(T)) = Function(buf) fromStream(buf)
            Return Me
        End Function

        Public Function handleCreate(buf As Stream, type As Type, emit As StreamMethods) As Object
            If emit = StreamMethods.Auto Then
                If loadBuffers.ContainsKey(type) Then
                    Return loadBuffers(type)(buf)
                Else
                    Return BSONFormat.Load(buf).CreateObject(type)
                End If
            ElseIf emit = StreamMethods.BSON Then
                Return BSONFormat.Load(buf).CreateObject(type)
            ElseIf loadBuffers.ContainsKey(type) Then
                Return loadBuffers(type)(buf)
            Else
                Throw New NotImplementedException
            End If
        End Function

        Public Function handleSerialize(param As Object) As ObjectStream
            Dim type As Type = param.GetType

            If toBuffers.ContainsKey(type) Then
                Return New ObjectStream(New TypeInfo(type, fullpath:=True), StreamMethods.Emit, toBuffers(type)(param))
            Else
                Dim element = type.GetJsonElement(param, New JSONSerializerOptions)
                Dim buf As Stream = BSONFormat.SafeGetBuffer(element)

                Return New ObjectStream(New TypeInfo(type, fullpath:=True), StreamMethods.BSON, buf)
            End If
        End Function
    End Class
End Namespace