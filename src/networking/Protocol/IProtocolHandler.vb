﻿#Region "Microsoft.VisualBasic::7ddd8d8a9dbee1b01dfd5957f850ef66, src\networking\IProtocolHandler.vb"

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

    '   Total Lines: 78
    '    Code Lines: 45 (57.69%)
    ' Comment Lines: 14 (17.95%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 19 (24.36%)
    '     File Size: 2.57 KB


    ' Class IProtocolHandler
    ' 
    ' 
    ' 
    ' Delegate Function
    ' 
    ' 
    ' Delegate Sub
    ' 
    ' 
    ' Delegate Sub
    ' 
    ' 
    ' Module Extensions
    ' 
    '     Function: (+2 Overloads) Ping
    ' 
    '     Sub: (+2 Overloads) SendMessage
    ' 
    ' 
    ' 
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Darwinism.IPC.Networking.Tcp
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Http
Imports Microsoft.VisualBasic.Parallel

Namespace Protocols

    <HideModuleName>
    Public Module Extensions

        ''' <summary>
        ''' -1标识Ping不通
        ''' </summary>
        ''' <param name="operationTimeOut">ms</param>
        ''' <returns></returns>
        Public Function Ping(ep As System.Net.IPEndPoint, Optional operationTimeOut As Integer = 3 * 1000) As Double
            Return New TcpRequest(ep).Ping(operationTimeOut)
        End Function

        ''' <summary>
        ''' -1 ping failure
        ''' </summary>
        ''' <param name="invoke"></param>
        ''' <param name="timeout"></param>
        ''' <returns></returns>
        <Extension>
        Public Function Ping(invoke As TcpRequest, Optional timeout As Integer = 3 * 1000) As Double
            Dim sw As Stopwatch = Stopwatch.StartNew
            Dim request As RequestStream = RequestStream.SystemProtocol(RequestStream.Protocols.Ping, PING_REQUEST)
            Dim response As RequestStream = invoke _
            .SetTimeOut(TimeSpan.FromMilliseconds(timeout)) _
            .SendMessage(request)

            If HTTP_RFC.RFC_REQUEST_TIMEOUT = response.Protocol Then
                Return -1
            End If

            Return sw.ElapsedMilliseconds
        End Function

        Public Const PING_REQUEST As String = "PING/TTL-78973"

#Region ""

        <Extension>
        Public Sub SendMessage(host As System.Net.IPEndPoint, request As String, Callback As Action(Of String))
            Dim client As New TcpRequest(host)
            Call New Threading.Thread(Sub() Callback(client.SendMessage(request))).Start()
        End Sub

        <Extension>
        Public Sub SendMessage(host As IPEndPoint, request As String, Callback As Action(Of String))
            Call host.GetIPEndPoint.SendMessage(request, Callback)
        End Sub

#End Region
    End Module
End Namespace