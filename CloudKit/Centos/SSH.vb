#Region "Microsoft.VisualBasic::ac056aef1c0d20b0d837d18075abe8aa, CloudKit\Centos\SSH.vb"

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

    ' Class SSH
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: Run, ToString
    ' 
    ' /********************************************************************************/

#End Region

''' <summary>
''' this works on linux system via ssh command from bash
''' </summary>
Public Class SSH

    Protected ReadOnly user As String
    Protected ReadOnly password As String
    Protected ReadOnly endpoint As String
    Protected ReadOnly port As Integer
    Protected ReadOnly debug As Boolean

    Sub New(user$, password$,
            Optional endpoint$ = "127.0.0.1",
            Optional port% = 22,
            Optional debug As Boolean = False)

        Me.user = user
        Me.password = password
        Me.endpoint = endpoint
        Me.port = port
        Me.debug = debug
    End Sub

    Public Overridable Function Run(command As String) As String
        Return CommandLine.Call("ssh", $"-o StrictHostKeyChecking=no {user}@{endpoint}:{port} {command}")
    End Function

    Public Overrides Function ToString() As String
        Return $"ssh {user}@{endpoint}:{port}"
    End Function
End Class

