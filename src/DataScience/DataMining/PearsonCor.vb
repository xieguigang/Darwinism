Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.Framework
Imports Microsoft.VisualBasic.Math.Correlations

Public Module PearsonCor

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="m1"></param>
    ''' <param name="m2"></param>
    ''' <param name="prefilter_cor"></param>
    ''' <param name="prefilter_pval"></param>
    ''' <param name="n_threads"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 由于相关性是没有方向的，在这里会自动交换矩阵顺序，将大的矩阵进行切分为并行任务
    ''' </remarks>
    Public Iterator Function Correlation(m1 As DataFrame, m2 As DataFrame,
                                         Optional prefilter_cor As Double = 0.3,
                                         Optional prefilter_pval As Double = 0.05,
                                         Optional n_threads As Integer = 8) As IEnumerable(Of CorrelationNetwork)
        If m1.nsamples < m2.nsamples Then
            Call m1.Swap(m2)
        End If

        Dim cols As String() = m1.featureNames
        Dim spans = m1.SplitSpans(size:=m1.nsamples \ n_threads).ToArray


    End Function

    <Extension>
    Private Iterator Function SplitSpans(x As DataFrame, size As Integer) As IEnumerable(Of DataFrame)
        Dim cols As String() = x.featureNames

        For Each span As NamedCollection(Of Object)() In x.foreachRow.Split(size)
            Dim nums As NamedCollection(Of Double)() = span _
                .Select(Function(r)
                            Dim vec As IEnumerable(Of Double) = r.value.Select(Function(xi) CDbl(xi))
                            Dim row As New NamedCollection(Of Double)(r.name, vec)

                            Return row
                        End Function) _
                .ToArray
            Dim block As DataFrame = DataFrame.FromRows(nums, cols)

            Yield block
        Next
    End Function

End Module
