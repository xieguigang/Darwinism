#Region "Microsoft.VisualBasic::66127713c8990a997526dd27702d17c4, src\data\LINQ\LINQ\Runtime\Indexing\FullTextSearch\DocumentPool.vb"

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

    '   Total Lines: 28
    '    Code Lines: 7 (25.00%)
    ' Comment Lines: 17 (60.71%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 4 (14.29%)
    '     File Size: 861 B


    ' Class DocumentPool
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Public MustInherit Class DocumentPool

    ''' <summary>
    ''' save the document text to file
    ''' </summary>
    ''' <param name="doc"></param>
    ''' <remarks>
    ''' this function usually be used for save the document text string
    ''' to file and then returns the offset value of the document.
    ''' </remarks>
    Public MustOverride Function Save(doc As String) As Integer

    ''' <summary>
    ''' usually used for load index from file
    ''' </summary>
    ''' <returns></returns>
    Public MustOverride Function GetIndex() As InvertedIndex
    ''' <summary>
    ''' read document text from file via a given index
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Public MustOverride Function GetDocument(id As Integer) As String
    ''' <summary>
    ''' apply for save index to file
    ''' </summary>
    ''' <param name="index"></param>
    Public MustOverride Sub WriteIndex(index As InvertedIndex)

    Public MustOverride Sub Dispose()

End Class
