
Imports Darwinism.Centos
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime.Interop

''' <summary>
''' 
''' </summary>
''' <remarks>
''' this module only works on linux system
''' </remarks>
<Package("centos")>
<RTypeExport(GetType(netstat))>
Public Module CentosTools

    <ExportAPI("netstat")>
    Public Function netstat_func(Optional x As String = "-tulnp", Optional verbose As Boolean = False) As Object
        If x.StringEmpty Then
            ' netstat command do nothing
            Return Nothing
        ElseIf Strings.Trim(x).Contains(" ") Then
            ' is output text?
            Return netstat.tulnp(x).ToArray
        Else
            ' shell command
            Dim text As String = Interaction.Shell("netstat", "-tulnp", verbose:=verbose)
            Dim ls As netstat() = netstat.tulnp(text).ToArray

            Return ls
        End If
    End Function

End Module
