Imports System.IO
Imports System.Runtime.CompilerServices
Imports batch
Imports Darwinism.HPC.Parallel
Imports Darwinism.HPC.Parallel.IpcStream
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.Framework
Imports Microsoft.VisualBasic.Data.IO
Imports Microsoft.VisualBasic.Math.Correlations
Imports std = System.Math

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
        Dim task As New Func(Of DataFrame, DataFrame, Double, Double, CorrelationNetwork())(AddressOf PearsonTask)
        Dim env As Argument = DarwinismEnvironment.GetEnvironmentArguments
        Dim m2mat As SocketRef = SocketRef.WriteBuffer(m2, StreamEmit.Custom(Of DataFrame)(New DataFrameFile))

        For Each block As CorrelationNetwork() In Host.ParallelFor(Of DataFrame, CorrelationNetwork())(env, task, spans, m2mat, prefilter_cor, prefilter_pval)
            For Each edge As CorrelationNetwork In block
                Yield edge
            Next
        Next
    End Function

    <Extension>
    Private Iterator Function SplitSpans(x As DataFrame, size As Integer) As IEnumerable(Of DataFrame)
        Dim cols As String() = x.featureNames

        For Each span As NamedCollection(Of Object)() In x.foreachRow.Split(size)
            Dim nums As NamedCollection(Of Double)() = span.CastRowVectors.ToArray
            Dim block As DataFrame = DataFrame.FromRows(nums, cols)

            Yield block
        Next
    End Function

    <Extension>
    Private Iterator Function CastRowVectors(span As IEnumerable(Of NamedCollection(Of Object))) As IEnumerable(Of NamedCollection(Of Double))
        For Each r As NamedCollection(Of Object) In span
            Dim vec As IEnumerable(Of Double) = r.value.Select(Function(xi) CDbl(xi))
            Dim row As New NamedCollection(Of Double)(r.name, vec)

            Yield row
        Next
    End Function

    <EmitStream(GetType(DataFrameFile), Target:=GetType(DataFrame))>
    Private Function PearsonTask(m1 As DataFrame, m2 As DataFrame, prefilter_cor As Double, prefilter_pval As Double) As CorrelationNetwork()
        Dim v1 As NamedCollection(Of Double)() = m1.foreachRow.CastRowVectors.ToArray
        Dim v2 As NamedCollection(Of Double)() = m2.foreachRow.CastRowVectors.ToArray
        Dim result As New List(Of CorrelationNetwork)

        For Each u As NamedCollection(Of Double) In v1
            For Each v As NamedCollection(Of Double) In v2
                Dim pval As Double = 1
                Dim z As Double = 1
                Dim cor As Double = Correlations.GetPearson(u, v, pval, z:=z, throwMaxIterError:=False)

                If std.Abs(cor) > prefilter_cor AndAlso pval < prefilter_pval Then
                    Call result.Add(New CorrelationNetwork With {
                        .cor = cor,
                        .pvalue = pval,
                        .u = u.name,
                        .v = v.name,
                        .z = z
                    })
                End If
            Next
        Next

        Return result.ToArray
    End Function

End Module

''' <summary>
''' file stream handler for the data file of <see cref="DataFrame"/>.
''' </summary>
Public Class DataFrameFile : Implements IEmitStream

    Public Function BufferInMemory(obj As Object) As Boolean Implements IEmitStream.BufferInMemory
        Return True
    End Function

    Public Function WriteBuffer(obj As Object, file As Stream) As Boolean Implements IEmitStream.WriteBuffer
        Return FrameWriter.WriteFrame(DirectCast(obj, DataFrame), file)
    End Function

    Public Function WriteBuffer(obj As Object) As Stream Implements IEmitStream.WriteBuffer
        Dim s As New MemoryStream
        Call WriteBuffer(obj, s)
        Return s
    End Function

    Public Function ReadBuffer(file As Stream) As Object Implements IEmitStream.ReadBuffer
        Return FrameReader.ReadFrame(file)
    End Function
End Class