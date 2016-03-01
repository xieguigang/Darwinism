Imports Microsoft.VisualBasic.Linq.Framework.Provider
Imports Microsoft.VisualBasic.Linq.Framework.Provider.ImportsAPI
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel

Module Module1

    Sub Main()


        Dim source = {1, 2, 3, 4, 5, 6, 7}
        Dim itt As New Iterator(source)

        Do While itt.MoveNext
            Call __DEBUG_ECHO(Scripting.ToString(itt.Current) & " --> " & itt.ReadDone)
        Loop
        Call Scripting.ToString(itt.Current).__DEBUG_ECHO

        Dim s As String = "instr($s, cstr( $s->length), 8)"
        Dim typew = GetType(String)
        Dim www = Microsoft.VisualBasic.Linq.LDM.Expression.WhereClosure.CreateLinqWhere(typew, s)
        Dim types As TypeRegistry = TypeRegistry.LoadDefault
        Dim api As APIProvider = APIProvider.LoadDefault

        Call www.Compile(types, api)


        Dim p As New Parser.Parser
        Dim n = p.ParseExpression("$($(test2 pp $rt) -> test_func par1 $ffjhg par2 $dee) -> test3 p3 $($(test5 de) -> test4 ppp $gr)")
    End Sub
End Module
