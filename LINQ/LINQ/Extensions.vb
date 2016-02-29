Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Linq.Framework.DynamicCode.VBC
Imports Microsoft.VisualBasic.Linq.Framework.Provider
Imports Microsoft.VisualBasic.Linq.Framework.Provider.ImportsAPI
Imports Microsoft.VisualBasic.Linq.LDM.Expression

Public Module Extensions

    <Extension>
    Public Function Compile(where As WhereClosure, types As TypeRegistry, api As APIProvider) As Type
        Dim compiler As New DynamicCompiler(types, api)
        Return compiler.Compile(where.BuildModule)
    End Function

    <Extension>
    Public Function CompileTest(where As WhereClosure, types As TypeRegistry, api As APIProvider) As ITest
        Dim dynamicsType As Type = where.Compile(types, api) ' 得到动态编译出来的类型
        Return AddressOf New __where(dynamicsType).Test
    End Function
End Module

Public Delegate Function ITest(obj As Object) As Boolean

Friend Class __where

    ReadOnly __type As Type
    ReadOnly __test As MethodInfo

    Sub New(type As Type)
        __type = type
        __test.DeclaringType.GetMethod(WhereClosure.TestMethod)
    End Sub

    Public Function Test(obj As Object) As Boolean
        Return DirectCast(__test.Invoke(Nothing, {obj}), Boolean)
    End Function
End Class