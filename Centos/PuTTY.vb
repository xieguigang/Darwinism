
''' <summary>
''' PuTTY automation combine with the hyper-V virtual machine for windows server
''' </summary>
Public Class PuTTY

    ReadOnly user As String
    ReadOnly password As String
    ReadOnly endpoint As String
    ReadOnly port As Integer
    ReadOnly plink As String

    Sub New(user$, password$, Optional endpoint$ = "127.0.0.1", Optional port% = 22, Optional plink As String = "plink")
        Me.user = user
        Me.password = password
        Me.endpoint = endpoint
        Me.plink = plink
        Me.port = port
    End Sub

    Public Overrides Function ToString() As String
        Return $"ssh {user}@{endpoint}:{port}"
    End Function

    ''' <summary>
    ''' Run a bash script in target virtual machine
    ''' </summary>
    ''' <param name="bash">the file path of the bash script in the hyper-v virtual machine.</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Hyper-V PowerShell - Run a bash command in Linux VM and get output.
    ''' 
    ''' https://stackoverflow.com/questions/57778301/hyper-v-powershell-run-a-bash-command-in-linux-vm-and-get-output
    ''' </remarks>
    Public Function Run(bash As String) As String
        Dim cli As String = $"{endpoint} -P {port} -l {user} -pw ""{password}"" -batch /bin/bash ""{bash}"""
        Dim std_out As String = CommandLine.Call(plink, cli)

        Return std_out
    End Function

    Public Function Shell(command As String, Optional arguments As String = Nothing) As String
        Dim cmdl As String = If(arguments.StringEmpty, command, $"{command} {arguments}")
        Dim cli As String = $"{user}@{endpoint} -P {port} -pw ""{password}"" -batch {cmdl}"
        Dim std_out As String = CommandLine.Call(plink, cli)

        Return std_out
    End Function
End Class
