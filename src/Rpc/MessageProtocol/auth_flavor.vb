#Region "Microsoft.VisualBasic::8ca02b98ec4cda01bbeb49377ba9de20, Rpc\MessageProtocol\auth_flavor.vb"

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

    '     Enum auth_flavor
    ' 
    ' 
    '  
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace Rpc.MessageProtocol
    ''' <summary>
    ''' Authentication flavor
    ''' http://tools.ietf.org/html/rfc5531#section-8.2
    ''' </summary>
    Public Enum auth_flavor As Integer
        ''' <summary>
        '''
        ''' </summary>
        AUTH_NONE = 0

        ''' <summary>
        ''' 
        ''' </summary>
        AUTH_SYS = 1

        ''' <summary>
        ''' 
        ''' </summary>
        AUTH_SHORT = 2

        ''' <summary>
        ''' 
        ''' </summary>
        AUTH_DH = 3

        ''' <summary>
        ''' 
        ''' </summary>
        RPCSEC_GSS = 6
    End Enum
End Namespace
