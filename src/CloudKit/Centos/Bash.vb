Public Module Bash

    Public Function which(command As String) As String
        Return Strings.Trim(Interaction.Shell("which", command, verbose:=False))
    End Function
End Module
