Imports System.CodeDom
Imports Microsoft.VisualBasic.LINQ.Framework
Imports Microsoft.VisualBasic.LINQ.Framework.DynamicCode
Imports Microsoft.VisualBasic.LINQ.Framework.DynamicCode.VBC
Imports Microsoft.VisualBasic.LINQ.Framework.LQueryFramework

Namespace LDM.Expression

    ''' <summary>
    ''' The init variable.
    ''' </summary>
    Public Class FromClosure : Inherits Closure

        ''' <summary>
        ''' 变量的名称
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Name As String
        ''' <summary>
        ''' 变量的类型标识符
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TypeId As String

        Public Property RegistryType As TypeEntry

        Friend SetObject As System.Reflection.MethodInfo

        Sub New(source As Statements.Tokens.FromClosure, registry As TypeRegistry)
            Call MyBase.New(source)

            'Me.RegistryType = Statement.TypeRegistry.Find(TypeId)
            'If RegistryType Is Nothing Then
            '    Throw New TypeMissingExzception("Could not found any information about the type {0}.", TypeId)
            'Else
            '    Dim ILINQCollection As System.Type = _tokens.ObjectCollection.LoadExternalModule(RegistryType)
            '    Statement.Collection.ILINQCollection = Activator.CreateInstance(ILINQCollection)
            '    Me.TypeId = Statement.Collection.ILINQCollection.GetEntityType.FullName
            'End If
        End Sub

        Public Sub Initialize()
            '  Me.SetObject = DynamicInvoke.GetMethod(_statement.ILINQProgram, DynamicCompiler.SetObjectName)
        End Sub

        Public Overrides Function ToString() As String
            Return String.Format("Dim {0} As {1}", Name, TypeId)
        End Function

        Protected Overrides Function __parsing() As CodeExpression

        End Function
    End Class
End Namespace