#Region "Microsoft.VisualBasic::649d6beaad89002ea8a513be53a3967f, Parallel\IpcParallel\Stream\EmitStreamAttribute.vb"

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

    ' Class EmitStreamAttribute
    ' 
    '     Properties: Handler
    ' 
    '     Constructor: (+1 Overloads) Sub New
    ' 
    ' Interface IEmitStream
    ' 
    '     Function: BufferInMemory, ReadBuffer, (+2 Overloads) WriteBuffer
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO

<AttributeUsage(AttributeTargets.Class, AllowMultiple:=False, Inherited:=False)>
Public Class EmitStreamAttribute : Inherits Attribute

    Public ReadOnly Property Handler As Type

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
