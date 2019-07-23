Imports System.Runtime.CompilerServices

Namespace DistributeServices

    <HideModuleName>
    Friend Module ProtocolExtensions

        <Extension>
        Friend Function GetIPAddressList(IPrange As String) As IEnumerable(Of String)
            Dim range = IPrange.StringSplit("\s*[-]\s*")

            If range.Length = 1 Then
                Return Populate(range(Scan0))
            Else
                Return Populate(range(Scan0), range(1))
            End If
        End Function

        Private Iterator Function Populate(range As String) As IEnumerable(Of String)

        End Function

        Private Iterator Function Populate(lower$, upper$) As IEnumerable(Of String)

        End Function
    End Module
End Namespace