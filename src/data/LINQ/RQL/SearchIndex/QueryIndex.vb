#Region "Microsoft.VisualBasic::36ee5637c9859fce12928c4eb8cffeb0, src\data\LINQ\RQL\SearchIndex\QueryIndex.vb"

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

    '   Total Lines: 35
    '    Code Lines: 23 (65.71%)
    ' Comment Lines: 6 (17.14%)
    '    - Xml Docs: 83.33%
    ' 
    '   Blank Lines: 6 (17.14%)
    '     File Size: 1.03 KB


    ' Class QueryIndex
    ' 
    '     Properties: hashKeys
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Sub: loadIndex
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports LINQ
Imports Microsoft.VisualBasic.DataStorage.HDSPack.FileSystem

Public Class QueryIndex : Inherits MemoryQuery

    Public ReadOnly Property hashKeys As String()
        Get
            Return m_hashindex.Keys.ToArray
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="rqi">
    ''' the data stream to the resource query index file
    ''' </param>
    Sub New(rqi As Stream)
        Call loadIndex(New StreamPack(rqi, [readonly]:=True))
    End Sub

    Private Sub loadIndex(rqi As StreamPack)
        Dim hash As StreamGroup = rqi.GetObject("/hash/")

        If Not hash Is Nothing Then
            For Each field As StreamGroup In hash.files.OfType(Of StreamGroup)
                Dim dir As String = $"/hash/{field.fileName}/"
                Dim index As TermHashIndex = HashIndexFile.LoadIndex(rqi, dir)

                m_hashindex(field.fileName) = index
            Next
        End If
    End Sub
End Class

