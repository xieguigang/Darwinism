#Region "Microsoft.VisualBasic::2e43fcbbe187b0aabe07e3f31fb5b895, src\computing\Parallel\IpcParallel\Stream\ObjectStream.vb"

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

    '   Total Lines: 139
    '    Code Lines: 103 (74.10%)
    ' Comment Lines: 11 (7.91%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 25 (17.99%)
    '     File Size: 5.30 KB


    '     Class ObjectStream
    ' 
    '         Properties: IsNothing, isPrimitive, method, stream, type
    ' 
    '         Constructor: (+5 Overloads) Sub New
    ' 
    '         Function: GetUnderlyingType, openMemoryBuffer
    ' 
    '         Sub: (+2 Overloads) Dispose, LoadBuffer, Serialize
    '         Class TypeMsgPack
    ' 
    '             Function: GetObjectSchema
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.IO.MessagePack
Imports Microsoft.VisualBasic.Data.IO.MessagePack.Serialization
Imports Microsoft.VisualBasic.Parallel
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Microsoft.VisualBasic.Serialization
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace IpcStream

    Public Class ObjectStream : Inherits RawStream
        Implements IDisposable

        Dim disposedValue As Boolean

        <MessagePackMember(0)> Public Property method As StreamMethods
        <MessagePackMember(1)> Public Property stream As Byte()
        <MessagePackMember(2)> Public Property type As TypeInfo

        Public ReadOnly Property isPrimitive As Boolean
            Get
                Return DataFramework.IsPrimitive(GetUnderlyingType)
            End Get
        End Property

        Public ReadOnly Property IsNothing As Boolean
            Get
                Return type Is Nothing OrElse stream Is Nothing
            End Get
        End Property

        Sub New()
        End Sub

        Sub New(type As TypeInfo, method As StreamMethods, stream As Stream)
            Me.method = method
            Me.stream = New StreamPipe(stream).Read
            Me.type = type

            Call stream.Close()
            Call stream.Dispose()
        End Sub

        Sub New(raw As Byte())
            Using file As New MemoryStream(raw)
                Call LoadBuffer(file)
            End Using
        End Sub

        Sub New(raw As Stream)
            Call LoadBuffer(raw)
        End Sub

        Private Sub LoadBuffer(raw As Stream)
            Using read As New BinaryReader(raw)
                Dim methodi As Integer = read.ReadInt32
                Dim size As Integer = read.ReadInt32
                Dim chunk As Byte() = read.ReadBytes(size)
                Dim typeJson As String = chunk.UTF8String

                If chunk.IsNullOrEmpty AndAlso typeJson.StringEmpty Then
                    ' target object is nothing
                    type = Nothing
                    method = StreamMethods.Auto
                    stream = Nothing
                Else
                    size = read.ReadInt32
                    type = typeJson.LoadJSON(Of TypeInfo)
                    method = CType(methodi, StreamMethods)
                    stream = read.ReadBytes(size)
                End If
            End Using
        End Sub

        Shared Sub New()
            Call MsgPackSerializer.DefaultContext.RegisterSerializer(New TypeMsgPack)
        End Sub

        Private Class TypeMsgPack : Inherits SchemaProvider(Of TypeInfo)

            Protected Overrides Iterator Function GetObjectSchema() As IEnumerable(Of (obj As Type, schema As Dictionary(Of String, NilImplication)))
                Dim map As New Dictionary(Of String, NilImplication) From {
                    {NameOf(TypeInfo.assembly), NilImplication.MemberDefault},
                    {NameOf(TypeInfo.fullName), NilImplication.MemberDefault},
                    {NameOf(TypeInfo.reference), NilImplication.MemberDefault}
                }

                Yield (GetType(TypeInfo), map)
            End Function
        End Class

        Public Function GetUnderlyingType() As Type
            Return type.GetType(knownFirst:=True)
        End Function

        Public Function openMemoryBuffer() As MemoryStream
            Return New MemoryStream(stream)
        End Function

        Public Overrides Sub Serialize(MS As Stream)
            Dim json As Byte() = Encoding.UTF8.GetBytes(type.GetJson)

            Call MS.Write(BitConverter.GetBytes(method), Scan0, RawStream.INT32)
            Call MS.Write(BitConverter.GetBytes(json.Length), Scan0, RawStream.INT32)
            Call MS.Write(json, Scan0, json.Length)
            Call MS.Write(BitConverter.GetBytes(stream.Length), Scan0, RawStream.INT32)
            Call MS.Write(stream, Scan0, stream.Length)
            Call MS.Flush()
        End Sub

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: 释放托管状态(托管对象)
                    Erase stream
                End If

                ' TODO: 释放未托管的资源(未托管的对象)并替代终结器
                ' TODO: 将大型字段设置为 null
                disposedValue = True
            End If
        End Sub

        ' ' TODO: 仅当“Dispose(disposing As Boolean)”拥有用于释放未托管资源的代码时才替代终结器
        ' Protected Overrides Sub Finalize()
        '     ' 不要更改此代码。请将清理代码放入“Dispose(disposing As Boolean)”方法中
        '     Dispose(disposing:=False)
        '     MyBase.Finalize()
        ' End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            ' 不要更改此代码。请将清理代码放入“Dispose(disposing As Boolean)”方法中
            Dispose(disposing:=True)
            GC.SuppressFinalize(Me)
        End Sub
    End Class
End Namespace
