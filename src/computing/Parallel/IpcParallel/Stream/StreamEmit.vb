#Region "Microsoft.VisualBasic::286cdf7fa183c9664e466f806607c148, src\computing\Parallel\IpcParallel\Stream\StreamEmit.vb"

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


    ' Code Statistics:

    '   Total Lines: 171
    '    Code Lines: 122 (71.35%)
    ' Comment Lines: 28 (16.37%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 21 (12.28%)
    '     File Size: 6.86 KB


    '     Class StreamEmit
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: CreateHandler, Custom, (+5 Overloads) Emit, GetHandler, (+2 Overloads) handleCreate
    '                   handleSerialize
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Emit.Delegates
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.MIME.application.json
Imports Microsoft.VisualBasic.MIME.application.json.BSON
Imports TypeInfo = Microsoft.VisualBasic.Scripting.MetaData.TypeInfo

Namespace IpcStream

    Public Class StreamEmit

        ''' <summary>
        ''' convert clr object to file stream data
        ''' </summary>
        ReadOnly toBuffers As New Dictionary(Of Type, toBuffer)
        ''' <summary>
        ''' create clr object from a given file stream data
        ''' </summary>
        ReadOnly loadBuffers As New Dictionary(Of Type, loadBuffer)
        ReadOnly emitCache As New Dictionary(Of Type, IEmitStream)

        Sub New()
            For Each [handle] In EmitHandler.PopulatePrimitiveHandles
                toBuffers(handle.target) = handle.emit
            Next
            For Each [handle] In EmitHandler.PopulatePrimitiveParsers
                loadBuffers(handle.target) = handle.emit
            Next
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Function Custom(Of T)(file As IEmitStream) As StreamEmit
            Return New StreamEmit() _
                .Emit(file.GetWriter(Of T)) _
                .Emit(file.GetReader(Of T))
        End Function

        ''' <summary>
        ''' add handler for convert the clr object to file stream data
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="streamAs"></param>
        ''' <returns></returns>
        Public Function Emit(Of T)(streamAs As Func(Of T, Stream)) As StreamEmit
            toBuffers(GetType(T)) = Function(obj) streamAs(obj)
            Return Me
        End Function

        Public Function Emit(t As Type, streamAs As Func(Of Object, Stream)) As StreamEmit
            toBuffers(t) = New toBuffer(AddressOf streamAs.Invoke)
            Return Me
        End Function

        Public Function Emit(t As Type, fromStream As Func(Of Stream, Object)) As StreamEmit
            loadBuffers(t) = New loadBuffer(AddressOf fromStream.Invoke)
            Return Me
        End Function

        Public Function Emit(t As Type, file As IEmitStream) As StreamEmit
            toBuffers(t) = New toBuffer(AddressOf file.WriteBuffer)
            loadBuffers(t) = New loadBuffer(AddressOf file.ReadBuffer)
            Return Me
        End Function

        ''' <summary>
        ''' add handler for creates the clr object from the file stream data
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="fromStream"></param>
        ''' <returns></returns>
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
                ElseIf emitCache.ContainsKey(type) Then
                    Return emitCache(type).ReadBuffer(buf)
                Else
                    Dim handler As IEmitStream = GetHandler(type)

                    If Not handler Is Nothing Then
                        emitCache.Add(type, handler)
                        Return emitCache(type).ReadBuffer(buf)
                    Else
                        Return BSONFormat.Load(buf).CreateObject(type, decodeMetachar:=False)
                    End If
                End If
            ElseIf emit = StreamMethods.BSON Then
                Return BSONFormat.Load(buf).CreateObject(type, decodeMetachar:=False)
            ElseIf loadBuffers.ContainsKey(type) Then
                Return loadBuffers(type)(buf)
            Else
                Throw New NotImplementedException
            End If
        End Function

        ''' <summary>
        ''' try to get the stream data function
        ''' </summary>
        ''' <param name="type">target object type for save/read of a stream file.</param>
        ''' <returns></returns>
        Public Shared Function GetHandler(type As Type) As IEmitStream
            Dim attr As EmitStreamAttribute = CType(type, System.Reflection.TypeInfo) _
                .GetCustomAttributes(Of EmitStreamAttribute) _
                .FirstOrDefault

            If attr Is Nothing Then
                Return Nothing
            Else
                Return CreateHandler(attr)
            End If
        End Function

        Public Shared Function CreateHandler(attr As EmitStreamAttribute) As IEmitStream
            Dim type As Type = attr.Handler

            If Not Type.ImplementInterface(Of IEmitStream) Then
                Return Nothing
            Else
                Return Activator.CreateInstance(Type)
            End If
        End Function

        ''' <summary>
        ''' generates the object file binary data
        ''' </summary>
        ''' <param name="param"></param>
        ''' <returns></returns>
        Public Function handleSerialize(param As Object) As ObjectStream
            Dim type As Type = param.GetType
            Dim method As StreamMethods
            Dim typeinfo As New TypeInfo(type, fullpath:=True)
            Dim buf As Stream

            If toBuffers.ContainsKey(type) Then
                method = StreamMethods.Emit
                buf = toBuffers(type)(param)
            ElseIf emitCache.ContainsKey(type) Then
                method = StreamMethods.Auto
                buf = emitCache(type).WriteBuffer(param)
            Else
                Dim handler As IEmitStream = GetHandler(type)

                If Not handler Is Nothing Then
                    emitCache.Add(type, handler)
                    method = StreamMethods.Auto
                    buf = handler.WriteBuffer(param)
                Else
                    buf = type _
                        .GetJsonElement(param, New JSONSerializerOptions) _
                        .DoCall(AddressOf BSONFormat.SafeGetBuffer)
                    method = StreamMethods.BSON
                End If
            End If

            Return New ObjectStream(typeinfo, method, buf)
        End Function
    End Class
End Namespace
