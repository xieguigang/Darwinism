Imports Microsoft.VisualBasic.ValueTypes

Public Module IndexExtensions

    Public Function IntegerIndex() As RangeIndex(Of Integer)
        Return New RangeIndex(Of Integer)(Function(i) CDbl(i))
    End Function

    Public Function DoubleIndex() As RangeIndex(Of Double)
        Return New RangeIndex(Of Double)(Function(x) x)
    End Function

    Public Function DateIndex() As RangeIndex(Of Date)
        Return New RangeIndex(Of Date)(Function(x) x.UnixTimeStamp)
    End Function

End Module
