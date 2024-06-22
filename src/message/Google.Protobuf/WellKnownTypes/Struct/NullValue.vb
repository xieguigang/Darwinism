#Region "Microsoft.VisualBasic::1be883cf85c2fbde62b049541f558528, src\message\Google.Protobuf\WellKnownTypes\Struct\NullValue.vb"

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
    '    Code Lines: 11 (50.00%)
    ' Comment Lines: 9 (40.91%)
    '    - Xml Docs: 88.89%
    ' 
    '   Blank Lines: 2 (9.09%)
    '     File Size: 623 B


    '     Enum NullValue
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
    '''  `NullValue` is a singleton enumeration to represent the null value for the
    '''  `Value` type union.
    '''
    '''   The JSON representation for `NullValue` is JSON `null`.
    ''' </summary>
    Public Enum NullValue
        ''' <summary>
        '''  Null value.
        ''' </summary>
        <pbr.OriginalName("NULL_VALUE")>
        NullValue = 0
    End Enum

#End Region
End Namespace
