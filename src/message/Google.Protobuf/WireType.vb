#Region "Microsoft.VisualBasic::68309125d39eea5458145a8cf51938e5, src\message\Google.Protobuf\WireType.vb"

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

    '   Total Lines: 32
    '    Code Lines: 10 (31.25%)
    ' Comment Lines: 21 (65.62%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 1 (3.12%)
    '     File Size: 959 B


    '     Enum WireType
    ' 
    ' 
    '  
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace Google.Protobuf

    ''' <summary>
    ''' Wire types within protobuf encoding.
    ''' </summary>
    Public Enum WireType As UInteger
        ''' <summary>
        ''' Variable-length integer.
        ''' </summary>
        Varint = 0
        ''' <summary>
        ''' A fixed-length 64-bit value.
        ''' </summary>
        Fixed64 = 1
        ''' <summary>
        ''' A length-delimited value, i.e. a length followed by that many bytes of data.
        ''' </summary>
        LengthDelimited = 2
        ''' <summary>
        ''' A "start group" value - not supported by this implementation.
        ''' </summary>
        StartGroup = 3
        ''' <summary>
        ''' An "end group" value - not supported by this implementation.
        ''' </summary>
        EndGroup = 4
        ''' <summary>
        ''' A fixed-length 32-bit value.
        ''' </summary>
        Fixed32 = 5
    End Enum
End Namespace
