Namespace Google.Protobuf

    ''' <summary>
    ''' Wire types within protobuf encoding.
    ''' </summary>
    Public Enum WireType As UInteger
        ''' <summary>
        ''' Variable-length integer.
        ''' </summary>
        Varint = 0
        ''' <summary>
        ''' A fixed-length 64-bit value.
        ''' </summary>
        Fixed64 = 1
        ''' <summary>
        ''' A length-delimited value, i.e. a length followed by that many bytes of data.
        ''' </summary>
        LengthDelimited = 2
        ''' <summary>
        ''' A "start group" value - not supported by this implementation.
        ''' </summary>
        StartGroup = 3
        ''' <summary>
        ''' An "end group" value - not supported by this implementation.
        ''' </summary>
        EndGroup = 4
        ''' <summary>
        ''' A fixed-length 32-bit value.
        ''' </summary>
        Fixed32 = 5
    End Enum
End Namespace