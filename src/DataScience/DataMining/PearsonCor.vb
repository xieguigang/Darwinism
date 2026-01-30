Imports Microsoft.VisualBasic.ApplicationServices
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

        ' 保存小矩阵
        Dim m2_file As String = TempFileSystem.GetAppSysTempFile(".dat", App.PID, prefix:="df_")

        Call m2.wr

    End Function

End Module
