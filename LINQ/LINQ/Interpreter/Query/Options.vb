
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