﻿#Region "Microsoft.VisualBasic::e0b334b5fa745a3c5f60e991a4da0df2, src\Darwinism\Platform\Tcp.vb"

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

'   Total Lines: 3
'    Code Lines: 2 (66.67%)
' Comment Lines: 0 (0.00%)
'    - Xml Docs: 0.00%
' 
'   Blank Lines: 1 (33.33%)
'     File Size: 24 B


' Module Tcp
' 
' 
' 
' /********************************************************************************/

#End Region

Imports Darwinism.Centos
Imports Darwinism.HPC.Parallel
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Tcp
Imports Microsoft.VisualBasic.Scripting.MetaData

<Package("tcp")>
Module Tcp

    ''' <summary>
    ''' get a list of tcp port in used
    ''' </summary>
    ''' <returns></returns>
    ''' 
    <ExportAPI("port_in_used")>
    Public Function portInUsed(Optional verbose As Boolean = True) As Integer()
        If App.Platform = PlatformID.Unix Then
            If verbose Then
                Call VBDebugger.EchoLine("detects used of tcp ports for unix platform")
            End If
            Return IPCSocket.PortIsUsed(verbose)
        Else
            If verbose Then
                Call VBDebugger.EchoLine("detects used of tcp ports via windows api")
            End If
            Return TCPExtensions.PortIsUsed
        End If
    End Function

    <ExportAPI("local_address")>
    Public Function local_address(tcp As proc.net.tcp()) As IPEndPoint()
        Return tcp _
            .Select(Function(p) p.GetLocalAddress) _
            .ToArray
    End Function
End Module
