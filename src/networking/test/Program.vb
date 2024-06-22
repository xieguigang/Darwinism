#Region "Microsoft.VisualBasic::5b7766f50bcf19438c2b4f1dc906a30c, src\networking\test\Program.vb"

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

    '   Total Lines: 28
    '    Code Lines: 22 (78.57%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 6 (21.43%)
    '     File Size: 660 B


    ' Module Program
    ' 
    '     Sub: Main, runClient, runServer
    ' 
    ' /********************************************************************************/

#End Region

Imports Darwinism.IPC.Networking.Tcp
Imports Microsoft.VisualBasic.Parallel

Module Program
    Sub Main(args As String())
        Dim cmdl = App.CommandLine

        If cmdl.Name = "-s" Then
            Call runServer()
        Else
            Call runClient()
        End If
    End Sub

    Dim listen As Integer = 11003

    Private Sub runClient()
        Dim socket As New TcpRequest("127.0.0.1", listen)
        Dim msg As New RequestStream(0, 0, "hello")

        socket.SendMessage(msg)
    End Sub

    Private Sub runServer()
        Dim socket As New TcpServicesSocket(listen)
        socket.Run()
    End Sub
End Module
