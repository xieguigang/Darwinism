#Region "Microsoft.VisualBasic::f13ccaf2e2cec401b9f2c278b53e375f, LINQ\TestLINQEntity\ExampleXML.vb"

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

    ' Class ExampleXMLCollection
    ' 
    '     Properties: members
    ' 
    '     Function: GetCollection, GetEntityType
    ' 
    ' Class member
    ' 
    '     Properties: name, summary
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Extensions

<LINQ.Framework.Reflection.LINQEntity("member")>
<Xml.Serialization.XmlType("doc")> Public Class ExampleXMLCollection
    Implements LINQ.Framework.ILINQCollection

    Public Property members As List(Of member)

    Public Function GetCollection(Path As String) As Object() Implements LINQ.Framework.ILINQCollection.GetCollection
        Dim xml = Path.LoadXml(Of ExampleXMLCollection)()
        Me.members = xml.members
        Return members.ToArray
    End Function

    Public Function GetEntityType() As Type Implements LINQ.Framework.ILINQCollection.GetEntityType
        Return GetType(member)
    End Function
End Class

Public Class member
    <Xml.Serialization.XmlAttribute> Public Property name As String
    <Xml.Serialization.XmlElement> Public Property summary As String
End Class
