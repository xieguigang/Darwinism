Friend Class L1CacheHotData

    Public hashcode As UInteger
    Public bucket As UInteger
    Public hits As Integer
    Public data As Byte()

    Public Overrides Function ToString() As String
        Return $"{hashcode}@bucket-{bucket}, {hits} hits - {StringFormats.Lanudry(data.TryCount)}"
    End Function

End Class