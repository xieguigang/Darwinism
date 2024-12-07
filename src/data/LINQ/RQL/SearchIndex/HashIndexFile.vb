#Region "Microsoft.VisualBasic::66cca9ef6c6efdcb0993f2cd38f8f470, src\data\LINQ\RQL\SearchIndex\HashIndexFile.vb"

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

    '   Total Lines: 34
    '    Code Lines: 27 (79.41%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 7 (20.59%)
    '     File Size: 1.66 KB


    ' Class HashIndexFile
    ' 
    '     Function: LoadIndex
    ' 
    '     Sub: WriteIndex
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports LINQ
Imports Microsoft.VisualBasic.DataStorage.HDSPack
Imports Microsoft.VisualBasic.DataStorage.HDSPack.FileSystem
Imports Microsoft.VisualBasic.MIME.application.json
Imports Microsoft.VisualBasic.MIME.application.json.BSON
Imports Microsoft.VisualBasic.MIME.application.json.Javascript
Imports Microsoft.VisualBasic.Serialization.JSON

Public Class HashIndexFile

    Public Shared Sub WriteIndex(index As TermHashIndex, root As String, s As StreamPack)
        Call s.WriteText(index.GetDocumentMaps.GetJson(simpleDict:=True), $"{root}/documentMaps.json")

        Using buf As Stream = s.OpenFile($"{root}/hashMaps.bson", FileMode.Create, FileAccess.Write)
            Dim bytes As MemoryStream = BSONFormat.SafeGetBuffer(index.GetHashIndex.CreateJSONElement)

            Call bytes.Seek(Scan0, SeekOrigin.Begin)
            Call bytes.Flush()
            Call bytes.CopyTo(buf)
        End Using
    End Sub

    Public Shared Function LoadIndex(s As StreamPack, root As String) As TermHashIndex
        Dim json_str As String = s.ReadText($"{root}/documentMaps.json")
        Dim documentMaps As Dictionary(Of Integer, Integer) = json_str.LoadJSON(Of Dictionary(Of Integer, Integer))
        Dim buf As Stream = s.OpenFile($"{root}/hashMaps.bson", FileMode.Open, FileAccess.Read)
        Dim hashMaps As JsonObject = BSONFormat.Load(buf, leaveOpen:=True)
        Dim hashMapping As Dictionary(Of String, Integer()) = hashMaps.CreateObject(Of Dictionary(Of String, Integer()))

        Return New TermHashIndex(New InMemoryDocuments, documentMaps, hashMapping)
    End Function

End Class

