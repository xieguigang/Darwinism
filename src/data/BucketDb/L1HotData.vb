Friend Class L1CacheHotData

    Public hashcode As UInteger
    Public bucket As UInteger

    ''' <summary>
    ''' 命中计数：通过 <see cref="Interlocked.Increment"/> 无锁原子递增，
    ''' 多线程并发读取时保持一致。
    ''' </summary>
    Public hits As Integer
    Public data As Byte()

    Public Overrides Function ToString() As String
        Return $"{hashcode}@bucket-{bucket}, {hits} hits - {StringFormats.Lanudry(data.TryCount)}"
    End Function

End Class