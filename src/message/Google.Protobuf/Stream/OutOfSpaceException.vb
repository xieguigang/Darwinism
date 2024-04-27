#Region "Microsoft.VisualBasic::0c530499643defc43599f5dc9f9e4b0b, G:/GCModeller/src/runtime/Darwinism/src/message/Google.Protobuf//Stream/OutOfSpaceException.vb"

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

    '   Total Lines: 16
    '    Code Lines: 9
    ' Comment Lines: 4
    '   Blank Lines: 3
    '     File Size: 443 B


    '     Class OutOfSpaceException
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO

Namespace Google.Protobuf

    ''' <summary>
    ''' Indicates that a CodedOutputStream wrapping a flat byte array
    ''' ran out of space.
    ''' </summary>
    Public NotInheritable Class OutOfSpaceException
        Inherits IOException

        Friend Sub New()
            MyBase.New("CodedOutputStream was writing to a flat byte array and ran out of space.")
        End Sub
    End Class
End Namespace
