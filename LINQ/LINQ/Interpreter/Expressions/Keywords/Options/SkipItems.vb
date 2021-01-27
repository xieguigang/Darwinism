#Region "Microsoft.VisualBasic::a47858251c0d09202512517202fe5de6, LINQ\LINQ\Interpreter\Expressions\Keywords\Options\SkipItems.vb"

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

    '     Class SkipItems
    ' 
    '         Properties: keyword
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: (+2 Overloads) Exec
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.My.JavaScript

Namespace Interpreter.Expressions

    Public Class SkipItems : Inherits PipelineKeyword

        Public Overrides ReadOnly Property keyword As String
            Get
                Return "Skip"
            End Get
        End Property

        Dim n As Expression

        Sub New(n As Expression)
            Me.n = n
        End Sub

        Public Overrides Function Exec(result As IEnumerable(Of JavaScriptObject), context As ExecutableContext) As IEnumerable(Of JavaScriptObject)
            Return result.Skip(count:=CInt(Exec(context)))
        End Function

        Public Overrides Function Exec(context As ExecutableContext) As Object
            Return n.Exec(context)
        End Function
    End Class
End Namespace
