#Region "Microsoft.VisualBasic::e147992baf842713336a08ed275e0487, Parallel\IpcParallel\Stream\StreamEmit.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.



    ' /********************************************************************************/

    ' Summaries:

    '     Class StreamEmit
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: (+2 Overloads) Emit, (+2 Overloads) handleCreate, handleSerialize
    ' 
    ' 
    ' /********************************************************************************/

#End Region

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

        Public Function handleCreate(stream As ObjectStream) As Object
            Using buf As Stream = stream.openMemoryBuffer
                Return handleCreate(buf, stream.GetUnderlyingType, stream.method)
            End Using
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
