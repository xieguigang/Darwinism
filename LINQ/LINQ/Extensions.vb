Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Linq.Framework.DynamicCode.VBC
Imports Microsoft.VisualBasic.Linq.Framework.Provider
Imports Microsoft.VisualBasic.Linq.Framework.Provider.ImportsAPI
Imports Microsoft.VisualBasic.Linq.LDM.Expression

Public Module Extensions

    <Extension> Public Function Compile(where As WhereClosure, types As TypeRegistry, api As APIProvider) As Type
        Dim compiler As New DynamicCompiler(types, api)
        Return compiler.Compile(where.BuildModule)
    End Function
End Module
