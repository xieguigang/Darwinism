#Region "Microsoft.VisualBasic::dc720bf5ac14ba41d1a66c96563518fb, G:/GCModeller/src/runtime/Darwinism/src/data/LINQ/LINQ//Interpreter/Expressions/ArrayExpression.vb"

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

    '   Total Lines: 27
    '    Code Lines: 19
    ' Comment Lines: 0
    '   Blank Lines: 8
    '     File Size: 693 B


    '     Class ArrayExpression
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: Exec, ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Runtime

Namespace Interpreter.Expressions

    Public Class ArrayExpression : Inherits Expression

        Dim seq As Expression()

        Sub New(seq As IEnumerable(Of Expression))
            Me.seq = seq.ToArray
        End Sub

        Public Overrides Function Exec(context As ExecutableContext) As Object
            Dim list As New List(Of Object)

            For Each item In seq
                list.Add(item.Exec(context))
            Next

            Return list.ToArray
        End Function

        Public Overrides Function ToString() As String
            Return $"[{seq.JoinBy(", ")}]"
        End Function
    End Class
End Namespace
