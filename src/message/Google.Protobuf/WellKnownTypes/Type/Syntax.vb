﻿#Region "Microsoft.VisualBasic::60cffc183a67e717684bf937ced8ba84, src\message\Google.Protobuf\WellKnownTypes\Type\Syntax.vb"

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

    '   Total Lines: 25
    '    Code Lines: 13 (52.00%)
    ' Comment Lines: 9 (36.00%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 3 (12.00%)
    '     File Size: 647 B


    '     Enum Syntax
    ' 
    ' 
    '  
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Language
Imports pbc = Google.Protobuf.Collections
Imports pbr = Google.Protobuf.Reflection

Namespace Google.Protobuf.WellKnownTypes

#Region "Enums"
    ''' <summary>
    '''  The syntax in which a protocol buffer element is defined.
    ''' </summary>
    Public Enum Syntax
        ''' <summary>
        '''  Syntax `proto2`.
        ''' </summary>
        <pbr.OriginalName("SYNTAX_PROTO2")>
        Proto2 = 0
        ''' <summary>
        '''  Syntax `proto3`.
        ''' </summary>
        <pbr.OriginalName("SYNTAX_PROTO3")>
        Proto3 = 1
    End Enum

#End Region
End Namespace
