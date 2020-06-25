Namespace Language

    ''' <summary>
    ''' 
    ''' </summary>
    Public Enum Tokens
        Invalid

        keyword

        Open
        Close

        [Operator]

        Symbol
        Reference

        Interpolation
        Literal

        Number
        [Integer]
        [Boolean]

        Comma
        ''' <summary>
        ''' 与VB语言类似的，是以换行作为语句结束
        ''' </summary>
        Terminator

        Comment
    End Enum
End Namespace