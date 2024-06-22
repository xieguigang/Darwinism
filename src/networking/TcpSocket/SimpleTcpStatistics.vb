#Region "Microsoft.VisualBasic::d190ad6bb24d3cd8456f08bfc0b828ce, src\networking\TcpSocket\SimpleTcpStatistics.vb"

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

    '   Total Lines: 59
    '    Code Lines: 26 (44.07%)
    ' Comment Lines: 25 (42.37%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 8 (13.56%)
    '     File Size: 1.95 KB


    '     Class SimpleTcpStatistics
    ' 
    '         Properties: ReceivedBytes, SentBytes, StartTime, UpTime
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    '         Function: ToString
    ' 
    '         Sub: Reset
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace TcpSocket

    ''' <summary>
    ''' SimpleTcp statistics.
    ''' </summary>
    Public Class SimpleTcpStatistics

        ''' <summary>
        ''' The time at which the client or server was started.
        ''' </summary>
        Public ReadOnly Property StartTime As Date = Date.Now.ToUniversalTime()

        ''' <summary>
        ''' The amount of time which the client or server has been up.
        ''' </summary>
        Public ReadOnly Property UpTime As TimeSpan
            Get
                Return Date.Now.ToUniversalTime() - _startTime
            End Get
        End Property

        ''' <summary>
        ''' The number of bytes received.
        ''' </summary>
        Public Property ReceivedBytes As Long

        ''' <summary>
        ''' The number of bytes sent.
        ''' </summary>
        Public Property SentBytes As Long

        ''' <summary>
        ''' Initialize the statistics object.
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Return human-readable version of the object.
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            Dim ret As String = "--- Statistics ---" & Environment.NewLine &
                "    Started        : " & _startTime.ToString() & Environment.NewLine &
                "    Uptime         : " & UpTime.ToString() & Environment.NewLine &
                "    Received bytes : " & ReceivedBytes.ToString() & Environment.NewLine &
                "    Sent bytes     : " & SentBytes.ToString() & Environment.NewLine
            Return ret
        End Function

        ''' <summary>
        ''' Reset statistics other than StartTime and UpTime.
        ''' </summary>
        Public Sub Reset()
            _receivedBytes = 0
            _sentBytes = 0
        End Sub
    End Class
End Namespace
