Imports LINQ.Interpreter.Expressions
Imports LINQ.Runtime
Imports Microsoft.VisualBasic.Emit.Delegates
Imports Microsoft.VisualBasic.My.JavaScript

Namespace Interpreter.Query

    Public Class Options
        Public Property OrderBy As OrderBy
        Public Property Distinct As Boolean

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
        ''' <param name="env"></param>
        ''' <returns>
        ''' array of <see cref="JavaScriptObject"/>
        ''' </returns>
        Public Overrides Function Exec(env As Environment) As Object
            Dim projections As New List(Of JavaScriptObject)
            Dim closure As New Environment(parent:=env)
            Dim skipVal As Boolean
            Dim dataset As DataSet = GetDataSet(env)

            Call closure.AddSymbol(symbol.symbolName, symbol.type)

            For Each item As Object In dataset.PopulatesData()
                closure.FindSymbol(symbol.symbolName).value = item

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

            If Not opt.OrderBy Is Nothing Then
                projections = opt.OrderBy.Sort(projections, closure).AsList
            End If

            Return projections.ToArray
        End Function
    End Class
End Namespace