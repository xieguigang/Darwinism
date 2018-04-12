Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Serialization.JSON

Public MustInherit Class MetaData

    Friend meta As Dictionary(Of String, String)

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Protected Function getValue(<CallerMemberName> Optional key$ = Nothing) As String
        Return If(meta.ContainsKey(key), meta(key), Nothing)
    End Function

    Public Overrides Function ToString() As String
        Return meta.GetJson
    End Function
End Class
