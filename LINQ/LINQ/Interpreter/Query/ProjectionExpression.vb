Imports LINQ.Interpreter.Expressions
Imports LINQ.Runtime
Imports Microsoft.VisualBasic.Emit.Delegates
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

    ''' <summary>
    ''' from ... select ...
    ''' </summary>
    Public Class ProjectionExpression : Inherits QueryExpression

        Dim opt As Options
        Dim project As OutputProjection

        Sub New(symbol As SymbolDeclare, sequence As Expression, exec As IEnumerable(Of Expression), proj As OutputProjection, opt As Options)
            Call MyBase.New(symbol, sequence, exec)

            Me.opt = opt
            Me.project = proj
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="context"></param>
        ''' <returns>
        ''' array of <see cref="JavaScriptObject"/>
        ''' </returns>
        Public Overrides Function Exec(context As ExecutableContext) As Object
            Dim projections As New List(Of JavaScriptObject)
            Dim env As Environment = context.env
            Dim closure As New ExecutableContext With {
                .env = New Environment(parent:=env),
                .throwError = context.throwError
            }
            Dim skipVal As Boolean
            Dim dataset As DataSet = GetDataSet(context)

            Call closure.env.AddSymbol(symbol.symbolName, symbol.type)

            For Each item As Object In dataset.PopulatesData()
                closure.env.FindSymbol(symbol.symbolName).value = item

                For Each line As Expression In executeQueue
                    If TypeOf line Is WhereFilter Then
                        skipVal = Not DirectCast(line.Exec(closure), Boolean)

                        If skipVal Then
                            Exit For
                        End If
                    End If
                Next

                If Not skipVal Then
                    projections.Add(project.Exec(closure))
                End If
            Next

            Return opt.RunOptionPipeline(projections, context).ToArray
        End Function
    End Class
End Namespace