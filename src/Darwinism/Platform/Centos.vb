
Imports Darwinism.Centos
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
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
    Public Function netstat_func(Optional x As String = "-tulnp",
                                 Optional verbose As Boolean = False,
                                 Optional env As Environment = Nothing) As Object
        If x.StringEmpty Then
            ' netstat command do nothing
            Return Nothing
        ElseIf Strings.Trim(x).Contains(" ") Then
            ' is output text?
            Return netstat.tulnp(x).ToArray
        Else
            ' shell command
            Dim text = RunLinuxHelper("netstat", x, verbose:=verbose, env:=env)

            If text Like GetType(Message) Then
                Return text.TryCast(Of Message)
            Else
                Return netstat.tulnp(text.TryCast(Of String)).ToArray
            End If
        End If
    End Function

    Private Function RunLinuxHelper(command As String, args As String, verbose As Boolean, env As Environment) As [Variant](Of String, Message)
        If Interaction.isUnix Then
            Return Interaction.Shell(command, args, verbose:=verbose)
        Else
            Return Internal.debug.stop("only works on linux system!", env)
        End If
    End Function

End Module
