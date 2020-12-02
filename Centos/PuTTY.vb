
''' <summary>
''' PuTTY automation combine with the hyper-V virtual machine for windows server
''' </summary>
Public Class PuTTY

    ReadOnly user As String
    ReadOnly password As String
    ReadOnly endpoint As String
    ReadOnly plink As String

    Sub New(user$, password$, Optional endpoint$ = "127.0.0.1:22", Optional plink As String = "plink")
        Me.user = user
        Me.password = password
        Me.endpoint = endpoint
        Me.plink = plink
    End Sub

    Public Overrides Function ToString() As String
        Return $"ssh {user}@{endpoint}"
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
    Public Function Shell(bash As String) As String
        Dim cli As String = $"{endpoint} -l {user}L -pw {password}L -batch bash ""{bash}"""
        Dim std_out As String = CommandLine.Call(plink, cli)

        Return std_out
    End Function
End Class
