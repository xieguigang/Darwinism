Module Module1

    Sub Main()

        Dim s As String = "$s->length + 5"
        Dim typew = GetType(String)
        Dim www = Microsoft.VisualBasic.Linq.LDM.Expression.WhereClosure.CreateLinqWhere(typew, s)

        Dim p As New Parser.Parser
        Dim n = p.ParseExpression("$($(test2 pp $rt) -> test_func par1 $ffjhg par2 $dee) -> test3 p3 $($(test5 de) -> test4 ppp $gr)")
    End Sub
End Module
