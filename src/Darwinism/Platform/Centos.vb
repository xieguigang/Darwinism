
Imports Darwinism.Centos
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
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

    Sub Main()
        Call Internal.Object.Converts.makeDataframe.addHandler(GetType(netstat()), AddressOf netstat_table)
    End Sub

    Private Function netstat_table(netstat As netstat(), args As list, env As Environment) As dataframe
        Dim df As New dataframe With {.columns = New Dictionary(Of String, Array)}

        Call df.add("Proto", netstat.Select(Function(a) a.Proto))
        Call df.add("Recv-Q", netstat.Select(Function(a) a.RecvQ))
        Call df.add("Send-Q", netstat.Select(Function(a) a.SendQ))
        Call df.add("Local Address", netstat.Select(Function(a) a.LocalAddress))
        Call df.add("Foreign Address", netstat.Select(Function(a) a.ForeignAddress))
        Call df.add("State", netstat.Select(Function(a) a.State))
        Call df.add("PID/Program name", netstat.Select(Function(a) a.Program))
        Call df.add("listened_port", netstat.Select(Function(a) a.LocalListenPort))

        Return df
    End Function

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
