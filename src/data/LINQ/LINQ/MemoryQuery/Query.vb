#Region "Microsoft.VisualBasic::65b91dd8de769a87a64f8fa8e49f3459, src\data\LINQ\LINQ\MemoryQuery\Query.vb"

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

    '   Total Lines: 14
    '    Code Lines: 11 (78.57%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 3 (21.43%)
    '     File Size: 259 B


    ' Class Query
    ' 
    ' 
    '     Enum Type
    ' 
    '         FullText, HashTerm, ValueMatch, ValueRange
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
        FullText
        HashTerm
        ValueRange
        ValueMatch
    End Enum

    Public Property search As Type
    Public Property field As String
    Public Property value As Object

End Class

