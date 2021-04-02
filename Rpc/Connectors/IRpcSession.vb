#Region "Microsoft.VisualBasic::7704e207ae0ce5c71aa683fccd1ff0a8, Rpc\Connectors\IRpcSession.vb"

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

    '     Interface IRpcSession
    ' 
    '         Sub: AsyncSend, Close
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System

Namespace Rpc.Connectors
    Friend Interface IRpcSession
        Inherits ITicketOwner

        Sub AsyncSend(ByVal ticket As ITicket)
        Sub Close(ByVal ex As Exception)
        Event OnExcepted As Action(Of IRpcSession, Exception)
        Event OnSended As Action(Of IRpcSession)
    End Interface
End Namespace

