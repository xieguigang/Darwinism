#Region "Microsoft.VisualBasic::cb7af299126dc94d32b1c8d78f89795d, G:/GCModeller/src/runtime/Darwinism/src/computing/Parallel//IpcParallel/Stream/EmitStreamAttribute.vb"

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

    '   Total Lines: 60
    '    Code Lines: 34
    ' Comment Lines: 13
    '   Blank Lines: 13
    '     File Size: 1.82 KB


    ' Class EmitStreamAttribute
    ' 
    '     Properties: Handler, Target
    ' 
    '     Constructor: (+1 Overloads) Sub New
    ' 
    ' Interface IEmitStream
    ' 
    '     Function: BufferInMemory, ReadBuffer, (+2 Overloads) WriteBuffer
    ' 
    ' Module CustomStreamFile
    ' 
    '     Function: DefaultWriteMemory, GetReader, GetWriter
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports System.Runtime.CompilerServices

<AttributeUsage(AttributeTargets.Class Or AttributeTargets.Method, AllowMultiple:=True, Inherited:=False)>
Public Class EmitStreamAttribute : Inherits Attribute

    Public ReadOnly Property Handler As Type

    ''' <summary>
    ''' the target object type for create file i/o via the functions inside <see cref="Handler"/>
    ''' </summary>
    ''' <returns></returns>
    Public Property Target As Type

    ''' <summary>
    ''' the handler type should implements the interface <see cref="IEmitStream"/>
    ''' </summary>
    ''' <param name="handler"></param>
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

Public Module CustomStreamFile

    <Extension>
    Public Function GetWriter(Of T)(file As IEmitStream) As Func(Of T, Stream)
        Return Function(a) file.WriteBuffer(a)
    End Function

    <Extension>
    Public Function GetReader(Of T)(file As IEmitStream) As Func(Of Stream, T)
        Return Function(s) file.ReadBuffer(s)
    End Function

    <Extension>
    Public Function DefaultWriteMemory(s As IEmitStream, obj As Object) As Stream
        Dim ms As Stream = New MemoryStream
        Call s.WriteBuffer(obj, ms)
        Call ms.Flush()
        Call ms.Seek(0, SeekOrigin.Begin)
        Return ms
    End Function

End Module
