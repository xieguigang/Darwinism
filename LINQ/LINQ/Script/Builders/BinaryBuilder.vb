Imports System.Runtime.CompilerServices
Imports LINQ.Interpreter.Expressions
Imports LINQ.Language

Module BinaryBuilder

    <Extension>
    Public Function ParseBinary(tokenList As Token()) As Expression
        Dim blocks = tokenList.SplitOperators.ToArray

    End Function
End Module
