#Region "Microsoft.VisualBasic::9b2c4685b77e9337d48ce9613ad8a319, G:/GCModeller/src/runtime/Darwinism/src/message/Google.Protobuf//Message/IMessageType.vb"

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

    '   Total Lines: 18
    '    Code Lines: 6
    ' Comment Lines: 11
    '   Blank Lines: 1
    '     File Size: 776 B


    '     Interface IMessageType
    ' 
    '         Sub: MergeFrom
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace Google.Protobuf

    ''' <summary>
    ''' Generic interface for a Protocol Buffers message,
    ''' where the type parameter is expected to be the same type as
    ''' the implementation class.
    ''' </summary>
    ''' <typeparam name="T">The message type.</typeparam>
    Public Interface IMessageType(Of T As IMessageType(Of T))
        Inherits IMessage, IEquatable(Of T), IDeepCloneable(Of T)
        ''' <summary>
        ''' Merges the given message into this one.
        ''' </summary>
        ''' <remarks>See the user guide for precise merge semantics.</remarks>
        ''' <param name="message">The message to merge with this one. Must not be null.</param>
        Overloads Sub MergeFrom(message As T)
    End Interface
End Namespace
