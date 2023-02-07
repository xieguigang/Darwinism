#Region "Microsoft.VisualBasic::58e4deac6a48b0d96f6db35d0d715b02, Parallel\MemoryMap\UnmanageMemoryRegion.vb"

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

    ' Class UnmanageMemoryRegion
    ' 
    '     Properties: memoryFile, size
    ' 
    ' /********************************************************************************/

#End Region

Public Class UnmanageMemoryRegion

    Public Property memoryFile As String
    Public Property size As Integer

    Public Function GetMemoryMap() As MapObject
        Return MapObject.FromPointer(Me)
    End Function

    Public Overrides Function ToString() As String
        Return $"{memoryFile} ({StringFormats.Lanudry(size)})"
    End Function

End Class
