Imports System.IO

<AttributeUsage(AttributeTargets.Class, AllowMultiple:=False, Inherited:=False)>
Public Class EmitStreamAttribute : Inherits Attribute

    Public ReadOnly Property Handler As Type

    Sub New(handler As Type)
        Me.Handler = handler
    End Sub

End Class

Public Interface IEmitStream

    ''' <summary>
    ''' serialize into a memory stream buffer?
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    Function BufferInMemory(obj As Object) As Boolean
    Function WriteBuffer(obj As Object, file As Stream) As Boolean
    Function WriteBuffer(obj As Object) As Stream
    Function ReadBuffer(file As Stream) As Object

End Interface