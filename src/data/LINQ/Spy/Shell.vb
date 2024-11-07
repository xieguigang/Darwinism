Imports LINQ.Interpreter
Imports LINQ.Interpreter.Query
Imports LINQ.Language
Imports LINQ.Runtime
Imports LINQ.Script
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

    ' # file driver is detected via the file extension name suffix
    ' let x = load "/path/to/file"
    ' let y = from xi in x where xi.score > 80 select xi.name

    Private Sub ExecLinq(script As String)
        Dim tokens As Token() = LINQ.Language.GetTokens(script).ToArray
        Dim query As ProjectionExpression = tokens.PopulateQueryExpression
        Dim context As New ExecutableContext With {
            .env = env,
            .throwError = True
        }
    End Sub

End Module
