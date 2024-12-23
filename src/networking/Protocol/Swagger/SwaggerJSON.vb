#Region "Microsoft.VisualBasic::d505b863fff5fd1528ea7869b7f4085a, src\networking\Protocol\Swagger\SwaggerJSON.vb"

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

    '   Total Lines: 72
    '    Code Lines: 45 (62.50%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 27 (37.50%)
    '     File Size: 1.69 KB


    '     Class SwaggerJSON
    ' 
    '         Properties: basePath, definitions, host, info, paths
    '                     schemes, swagger
    ' 
    '     Class definition
    ' 
    '         Properties: properties, title, type
    ' 
    '     Class [property]
    ' 
    '         Properties: example, type
    ' 
    '     Class info
    ' 
    '         Properties: title, version
    ' 
    '     Class path
    ' 
    '         Properties: [get]
    ' 
    '     Class handler
    ' 
    '         Properties: parameters, produces, responses
    ' 
    '     Class response
    ' 
    '         Properties: description, schema
    ' 
    '     Class schema
    ' 
    '         Properties: schema
    ' 
    '     Class parameter
    ' 
    '         Properties: [default], [in], name, type
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace Protocols.Swagger

    Public Class SwaggerJSON

        Public Property swagger As String
        Public Property info As info
        Public Property host As String
        Public Property basePath As String
        Public Property schemes As String()
        Public Property paths As Dictionary(Of String, path)
        Public Property definitions As Dictionary(Of String, definition)

    End Class
End Namespace
