#Region "Microsoft.VisualBasic::d06aecce1fe79dc45199d43a8ee36352, src\data\LINQ\LINQ\Runtime\Indexing\ValueIndex.vb"

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

    '   Total Lines: 17
    '    Code Lines: 12 (70.59%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 5 (29.41%)
    '     File Size: 541 B


    ' Class ValueIndex
    ' 
    '     Function: DateIndex, DoubleIndex, IntegerIndex
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.ValueTypes

Public MustInherit Class ValueIndex

    Public MustOverride ReadOnly Property UnderlyingType As Type

    Public Shared Function IntegerIndex() As RangeIndex(Of Integer)
        Return New RangeIndex(Of Integer)(Function(i) CDbl(i))
    End Function

    Public Shared Function DoubleIndex() As RangeIndex(Of Double)
        Return New RangeIndex(Of Double)(Function(x) x)
    End Function

    Public Shared Function DateIndex() As RangeIndex(Of Date)
        Return New RangeIndex(Of Date)(Function(x) x.UnixTimeStamp)
    End Function

End Class
