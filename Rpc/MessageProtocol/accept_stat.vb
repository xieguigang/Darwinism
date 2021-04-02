#Region "Microsoft.VisualBasic::cab73729bbb9b74d42df93cc021ef94e, Rpc\MessageProtocol\accept_stat.vb"

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

    '     Enum accept_stat
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
    ''' Given that a call message was accepted, the following is the status of an attempt to call a remote procedure.
    ''' http://tools.ietf.org/html/rfc5531#section-9
    ''' </summary>
    Public Enum accept_stat As Integer
        ''' <summary>
        ''' RPC executed successfully
        ''' </summary>
        SUCCESS = 0

        ''' <summary>
        ''' remote hasn't exported program
        ''' </summary>
        PROG_UNAVAIL = 1

        ''' <summary>
        ''' remote can't support version # 
        ''' </summary>
        PROG_MISMATCH = 2

        ''' <summary>
        ''' program can't support procedure
        ''' </summary>
        PROC_UNAVAIL = 3

        ''' <summary>
        ''' procedure can't decode params
        ''' </summary>
        GARBAGE_ARGS = 4

        ''' <summary>
        ''' e.g. memory allocation failure
        ''' </summary>
        SYSTEM_ERR = 5
    End Enum
End Namespace

