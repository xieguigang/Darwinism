﻿#Region "Microsoft.VisualBasic::5f5b96635874f371d8472e0fdcbf0b01, src\computing\Parallel\IpcParallel\SocketBuffer\SocketRef.vb"

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

    '   Total Lines: 103
    '    Code Lines: 59 (57.28%)
    ' Comment Lines: 29 (28.16%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 15 (14.56%)
    '     File Size: 4.60 KB


    '     Class SocketRef
    ' 
    '         Properties: address
    ' 
    '         Function: CreateReference, GetSocket, Open, ToString, WriteBuffer
    ' 
    '         Sub: SetSocketPool
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.MIME.application.json.BSON
Imports options = Darwinism.HPC.Parallel.Extensions

Namespace IpcStream

    Public Class SocketRef

        Public Property address As String

        ''' <summary>
        ''' buffered object is <see cref="ObjectStream"/>
        ''' </summary>
        ''' <param name="target">target .net clr object to save to file, and then which 
        ''' could be load into memory in another process for run the parallel computing.
        ''' </param>
        ''' <param name="emit">the object file read/write helper, default bson serializer will be used if this parameter is missing.</param>
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

            If options.Verbose Then
                Call VBDebugger.EchoLine($"object {ref.address}: {StringFormats.Lanudry(bytes:=ref.address.FileLength)}")
            End If

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
                Return BSONFormat.Load(file).CreateObject(GetType(SocketRef), decodeMetachar:=False)
            End Using
        End Function

        ''' <summary>
        ''' Usually be used for create memory temp file reference
        ''' </summary>
        ''' <returns>
        ''' this function returns a temp file inside memory cache location: ``/dev/shm`` by default.
        ''' the default location could be modify via the sciBASIC framework variable: ``sockets``.
        ''' </returns>
        ''' <remarks>
        ''' the size of ``/dev/shm`` depends on the memory size of the host system, example as the 
        ''' memory size of linux host system is 1.5TB, then ``/dev/shm`` is allocated as 756GB.
        ''' but the docker contains ``/dev/shm`` has a very small size allocated by default: 64MB, 
        ''' you needs configs of the docker container startup parameter for adjust of the size via 
        ''' ``--shm-size``, example as: ``docker run -it --shm-size="512M" docker-image-id``.
        ''' </remarks>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Function CreateReference() As SocketRef
            Dim tempDevice As String = If(App.IsMicrosoftPlatform, App.AppSystemTemp, "/dev/shm")
            tempDevice = App.GetVariable("sockets", tempDevice)
            Return TempFileSystem.CreateTempFilePath(tempDevice, ".sock", App.PID.ToHexString, prefix:="Parallel")
        End Function

        ''' <summary>
        ''' set the directory for place the socket data that used for data exchange 
        ''' between the slave process and master node.
        ''' </summary>
        ''' <param name="handle">
        ''' usually be a directory path that in memory disk
        ''' </param>
        Public Shared Sub SetSocketPool(handle As String)
            Call App.JoinVariable("sockets", handle)
            Call handle.MakeDir
        End Sub

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
