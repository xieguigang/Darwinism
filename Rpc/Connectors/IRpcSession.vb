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
