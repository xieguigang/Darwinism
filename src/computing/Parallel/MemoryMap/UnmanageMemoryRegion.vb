#Region "Microsoft.VisualBasic::7d77c80b602c38f45007952c7caa3edf, G:/GCModeller/src/runtime/Darwinism/src/computing/Parallel//MemoryMap/UnmanageMemoryRegion.vb"

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

    '   Total Lines: 33
    '    Code Lines: 22
    ' Comment Lines: 4
    '   Blank Lines: 7
    '     File Size: 977 B


    ' Class UnmanageMemoryRegion
    ' 
    '     Properties: isError, memoryFile, size
    ' 
    '     Function: GetMemoryMap, ToString
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices

Public Class UnmanageMemoryRegion

    Public Property memoryFile As String
    Public Property size As Integer

    ''' <summary>
    ''' It is an error memory mapping region?
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property isError As Boolean
        Get
            Return size < 0
        End Get
    End Property

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function GetMemoryMap() As MapObject
        Return MapObject.FromPointer(Me)
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Overrides Function ToString() As String
        Return $"{memoryFile} ({StringFormats.Lanudry(size)})"
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Narrowing Operator CType(map As UnmanageMemoryRegion) As MapObject
        Return MapObject.FromPointer(map)
    End Operator

End Class
