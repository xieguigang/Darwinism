#Region "Microsoft.VisualBasic::a78b9aee88e59ef13dc1dadf8bf6b109, LINQ\LINQ\Language\Token.vb"

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

    '     Class Token
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Operators: <>, =
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Scripting.TokenIcer

Namespace Language

    Public Class Token : Inherits CodeToken(Of Tokens)

        Sub New(name As Tokens, Optional text As String = Nothing)
            Call MyBase.New(name, text)
        End Sub

        Public Overloads Shared Operator =(t As Token, c As (name As Tokens, text As String)) As Boolean
            Return t.name = c.name AndAlso t.text = c.text
        End Operator

        Public Overloads Shared Operator <>(t As Token, c As (name As Tokens, text As String)) As Boolean
            Return Not t = c
        End Operator

    End Class
End Namespace
