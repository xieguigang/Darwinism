#Region "Microsoft.VisualBasic::3f53582c20fd5306214a7fbc99a35f2a, src\data\LINQ\LINQ\MemoryQuery\Query.vb"

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

    '   Total Lines: 20
    '    Code Lines: 14 (70.00%)
    ' Comment Lines: 2 (10.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 4 (20.00%)
    '     File Size: 400 B


    ' Class Query
    ' 
    ' 
    '     Enum Type
    ' 
    '         FullText, HashTerm, Levenshtein, ValueMatch, ValueRange
    '         ValueRangeGreaterThan, ValueRangeLessThan
    ' 
    ' 
    ' 
    '  
    ' 
    '     Properties: field, search, value
    ' 
    ' /********************************************************************************/

#End Region

Public Class Query

    Public Enum Type
        ' text search index
        FullText
        HashTerm
        Levenshtein

        ' numeric value search
        ValueRange
        ValueRangeGreaterThan
        ValueRangeLessThan
        ValueMatch
    End Enum

    Public Property search As Type
    Public Property field As String
    Public Property value As Object

End Class
