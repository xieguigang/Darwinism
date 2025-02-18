﻿#Region "Microsoft.VisualBasic::05cb51bae504b30cad47203c7ea05987, src\data\LINQ\RQL\ResourceQuery\NodeMap.vb"

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

    '   Total Lines: 33
    '    Code Lines: 21 (63.64%)
    ' Comment Lines: 7 (21.21%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 5 (15.15%)
    '     File Size: 842 B


    ' Class NodeMap
    ' 
    '     Properties: resources, size
    ' 
    '     Function: ToString
    ' 
    '     Sub: add
    ' 
    ' /********************************************************************************/

#End Region

''' <summary>
''' A descriptor of the specific resource file
''' </summary>
Public Class NodeMap

    Public Property resources As List(Of String)
    Public ReadOnly Property size As Integer
        Get
            If resources Is Nothing Then
                Return 0
            End If

            Return resources.Count
        End Get
    End Property

    ''' <summary>
    ''' add resource with duplicated removes
    ''' </summary>
    ''' <param name="resource"></param>
    Public Sub add(resource As String)
        If resource Is Nothing Then
            Return
        ElseIf resources.IndexOf(resource) = -1 Then
            Call resources.Add(resource)
        End If
    End Sub

    Public Overrides Function ToString() As String
        Return $"link_size:{size}"
    End Function

End Class
