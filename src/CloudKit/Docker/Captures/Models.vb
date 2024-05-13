#Region "Microsoft.VisualBasic::dea7be9c9d31d3f899a0e5e3c1cf4b45, src\CloudKit\Docker\Captures\Models.vb"

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

    '   Total Lines: 22
    '    Code Lines: 19
    ' Comment Lines: 0
    '   Blank Lines: 3
    '     File Size: 545 B


    '     Structure Search
    ' 
    ' 
    ' 
    '     Structure Container
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Darwinism.Docker.Arguments

Namespace Captures

    Public Structure Search
        Dim NAME As Image
        Dim DESCRIPTION As String
        Dim STARS As Integer
        Dim OFFICIAL As String
        Dim AUTOMATED As String
    End Structure

    Public Structure Container
        Dim CONTAINER_ID As String
        Dim IMAGE As Image
        Dim COMMAND As String
        Dim CREATED As String
        Dim STATUS As String
        Dim PORTS As String
        Dim NAMES As String
    End Structure
End Namespace
