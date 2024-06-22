#Region "Microsoft.VisualBasic::6157735144b0e8129a3928cc26fb4090, src\data\LINQ\LINQ\test\fulltext_demo.vb"

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

    '   Total Lines: 23
    '    Code Lines: 16 (69.57%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 7 (30.43%)
    '     File Size: 691 B


    ' Module fulltext_demo
    ' 
    '     Sub: Main
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ

Module fulltext_demo

    Sub Main()
        Dim index = InMemoryDocuments.CreateFullTextSearch

        For Each path As String In {"G:\GCModeller\src\runtime\sciBASIC#\Data\TextRank\Beauty_and_the_Beast.txt",
"G:\GCModeller\src\runtime\sciBASIC#\Data\TextRank\Cinderalla.txt",
"G:\GCModeller\src\runtime\sciBASIC#\Data\TextRank\Rapunzel.txt"}

            Dim pars = path.ReadAllLines

            Call index.Indexing(pars)
        Next

        Dim search1 = index.Search("his wife").ToArray
        Dim search2 = index.Search("clock struck midnight").ToArray
        Dim search3 = index.Search("What is this").ToArray

        Pause()
    End Sub
End Module

