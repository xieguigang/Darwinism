Imports LINQ.Interpreter.Expressions
Imports LINQ.Runtime
Imports Microsoft.VisualBasic.Emit.Delegates

Namespace Interpreter.Query

    Public Class Options
        Public Property OrderBy As OrderBy
        Public Property Distinct As Boolean

    End Class

    ''' <summary>
    ''' from ... select ...
    ''' </summary>
    Public Class ProjectionExpression : Inherits QueryExpression

        Dim sequence As Expression
        Dim symbol As SymbolDeclare
        Dim executeQueue As Expression()
        Dim opt As Options

        Sub New(symbol As SymbolDeclare, sequence As Expression, exec As IEnumerable(Of Expression), opt As Options)
            Me.executeQueue = exec.ToArray
            Me.symbol = symbol
            Me.opt = opt
            Me.sequence = sequence
        End Sub

        Public Overrides Function Exec(env As Environment) As Object
            Dim projections As New List(Of Object)
            Dim closure As New Environment(parent:=env)
            Dim seqList As Object = sequence.Exec(env)

            If seqList Is Nothing Then
                Throw New NullReferenceException
            Else
                Dim type As Type = seqList.GetType

                If Not type.IsArray AndAlso Not type.ImplementInterface(GetType(IEnumerable)) Then
                    Throw New InvalidExpressionException("target object value is not a sequence!")
                End If
            End If

            Call closure.AddSymbol(symbol.symbolName, symbol.type)

            For Each item As Object In DirectCast(seqList, IEnumerable)
                closure.FindSymbol(symbol.name).value = item

                For Each line As Expression In executeQueue
                    If TypeOf line Is WhereFilter Then
                        Dim result As Boolean = line.Exec(closure)

                        If Not result Then
                            Exit For
                        End If
                    End If
                Next
            Next

            Return projections.ToArray
        End Function
    End Class
End Namespace