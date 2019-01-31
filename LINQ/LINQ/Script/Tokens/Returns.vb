#Region "Microsoft.VisualBasic::171052e0050ac84faf5ffac25f72716d, LINQ\LINQ\Script\Tokens\Returns.vb"

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

    '     Class Returns
    ' 
    '         Properties: Ref
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Scripting.TokenIcer
Imports sciBASIC.ComputingServices.Linq.LDM.Statements

Namespace Script.Tokens

    Public Class Returns : Inherits TokenBase

        Public ReadOnly Property Ref As String

        Sub New(source As IEnumerable(Of Token(Of TokenIcer.Tokens)))
            Call MyBase.New(source)

            For Each x In source.Skip(1)
                If Not x.Type = TokenIcer.Tokens.WhiteSpace Then
                    Ref = x.Text
                    Exit For
                End If
            Next
        End Sub

        Public Overrides Function ToString() As String
            Return "Return " & Ref
        End Function
    End Class
End Namespace
