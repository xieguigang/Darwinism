Imports LINQ.Runtime
Imports Microsoft.VisualBasic.ApplicationServices.Terminal
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.LineEdit
Imports Microsoft.VisualBasic.Language.UnixBash

''' <summary>
''' Linq shell
''' </summary>
Module LinqShell

    Dim env As New GlobalEnvironment(New Registry)
    Dim console As Shell

    ReadOnly ps1_ready As New PS1("> ")
    ReadOnly ps1_incomplete As New PS1("+ ")

    Public Function RunTerminal() As Integer
        Dim editor As New LineEditor("LINQ", 5000) With {
            .HeuristicsMode = True,
            .TabAtStartCompletes = False,
            .MaxWidth = 120
        }

        console = New Shell(ps1_ready, AddressOf ExecLinq, dev:=New LineReader(editor)) With {
            .Quite = "!.LINQ#::quit" & Rnd()
        }

        Return console.Run
    End Function

    Private Sub ExecLinq(script As String)

    End Sub

End Module
