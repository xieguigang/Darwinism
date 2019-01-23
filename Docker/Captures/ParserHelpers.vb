Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Text
Imports r = System.Text.RegularExpressions.Regex

Namespace Captures

    Module ParserHelpers

        <Extension>
        Public Iterator Function ParseTable(Of T)(text$, creator As Func(Of String(), T)) As IEnumerable(Of T)
            Dim summary$() = text.Trim.LineTokens
            Dim header = r.Matches(summary(Scan0), "(\S+\s+)|(\S+)").ToArray
            Dim fieldLength%() = header.Select(AddressOf Len).ToArray

            For Each line As String In summary.Skip(1)
                Dim tokens$() = FormattedParser _
                    .FieldParser(line, fieldLength) _
                    .ToArray

                Yield creator(tokens)
            Next
        End Function
    End Module
End Namespace


