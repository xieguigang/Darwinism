﻿#Region "Microsoft.VisualBasic::feb839fe206c8518a09ae7577064ddbf, Parallel\IpcParallel\SocketBuffer\SocketRef.vb"

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

    '     Class SocketRef
    ' 
    '         Properties: address
    ' 
    '         Function: CreateReference, GetSocket, Open, ToString, WriteBuffer
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports System.Runtime.CompilerServices
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

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Overrides Function ToString() As String
            Return address
        End Function

        Public Shared Function GetSocket(stream As ObjectStream) As SocketRef
            Using file As Stream = stream.openMemoryBuffer
                Return BSONFormat.Load(file).CreateObject(GetType(SocketRef))
            End Using
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Function CreateReference() As SocketRef
            Return App.GetAppSysTempFile(".sock", App.PID.ToHexString, prefix:="Parallel")
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(ref As String) As SocketRef
            Return New SocketRef With {.address = ref}
        End Operator

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Narrowing Operator CType(socket As SocketRef) As String
            Return socket.address
        End Operator
    End Class
End Namespace