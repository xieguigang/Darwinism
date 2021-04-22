#Region "Microsoft.VisualBasic::0cd572f89794247aa7a60e608fbb89f5, LINQ\LINQ\Interpreter\Query\Options.vb"

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

    '     Class Options
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: RunOptionPipeline
    ' 
    ' 
    ' /********************************************************************************/

#End Region


Imports LINQ.Interpreter.Expressions
Imports LINQ.Runtime
Imports Microsoft.VisualBasic.My.JavaScript

Namespace Interpreter.Query

    Public Class Options

        Dim pipeline As PipelineKeyword()

        Sub New(pipeline As IEnumerable(Of Expression))
            Me.pipeline = pipeline _
                .Select(Function(l) DirectCast(l, PipelineKeyword)) _
                .ToArray
        End Sub

        Public Function RunOptionPipeline(output As IEnumerable(Of JavaScriptObject), context As ExecutableContext) As IEnumerable(Of JavaScriptObject)
            Dim raw As JavaScriptObject() = output.ToArray
            Dim allNames As String() = raw(Scan0).GetNames
            Dim env As Environment = context.env

            For Each name As String In allNames
                If Not env.HasSymbol(name) Then
                    Call env.AddSymbol(name, "any")
                End If
            Next

            For Each line As PipelineKeyword In pipeline
                raw = line.Exec(raw, context).ToArray
            Next

            Return raw
        End Function

    End Class
End Namespace
