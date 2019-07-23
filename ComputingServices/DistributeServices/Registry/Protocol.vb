Imports System.Runtime.CompilerServices

Namespace DistributeServices

    <HideModuleName>
    Public Module Protocol

        <Extension>
        Friend Iterator Function GetIPAddressList(IPrange As String) As IEnumerable(Of String)

        End Function
    End Module
End Namespace