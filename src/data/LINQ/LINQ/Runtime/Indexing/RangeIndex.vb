Imports Microsoft.VisualBasic.ComponentModel.Algorithm
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math

''' <summary>
''' in-memory index of the numeric value
''' </summary>
''' <typeparam name="T"></typeparam>
Public Class RangeIndex(Of T)

    Dim index As BlockSearchFunction(Of SeqValue(Of T))
    Dim eval As Func(Of T, Double)
    Dim tolerance As Double
    Dim doubles As Double()

    Sub New(eval As Func(Of T, Double))
        Me.eval = eval
    End Sub

    Public Function IndexData(data As IEnumerable(Of T)) As RangeIndex(Of T)
        Dim pool = data.Select(Function(xi, i) New SeqValue(Of T)(i, xi)).ToArray
        Dim x As Double() = (From xi As SeqValue(Of T) In pool Select eval(xi.value)).ToArray
        Dim diff As Double() = NumberGroups.diff(x:=x.OrderBy(Function(a) a).ToArray)
        Dim win_size As Double = diff.Average * 5

        tolerance = win_size
        doubles = x
        index = New BlockSearchFunction(Of SeqValue(Of T))(
            data:=pool,
            eval:=Function(i) eval(i.value),
            tolerance:=win_size,
            fuzzy:=True
        )

        Return Me
    End Function

    Public Iterator Function Search(min As T, max As T) As IEnumerable(Of SeqValue(Of T))
        Dim max_d As Double = eval(max)
        Dim min_d As Double = eval(min)
        Dim window As Double = max_d - min_d

        If window < 0 Then
            Return
        End If

        Dim left = index.Search(New SeqValue(Of T)(min), window)
        Dim right = index.Search(New SeqValue(Of T)(max), window)

        For Each item As SeqValue(Of T) In left.JoinIterates(right)
            Dim xi As Double = eval(item.value)

            If xi >= min_d AndAlso xi <= max_d Then
                Yield item
            End If
        Next
    End Function

    Public Iterator Function Search(x As T) As IEnumerable(Of SeqValue(Of T))
        Dim q = index.Search(New SeqValue(Of T)(x), tolerance)
        Dim target As Double = eval(x)

        For Each item As SeqValue(Of T) In q
            If target = doubles(item.i) Then
                Yield item
            End If
        Next
    End Function

End Class
