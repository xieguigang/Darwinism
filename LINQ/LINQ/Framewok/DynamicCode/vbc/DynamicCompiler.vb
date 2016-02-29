Imports System.Text
Imports System.CodeDom.Compiler
Imports System.CodeDom
Imports Microsoft.VisualBasic.Linq.Statements
Imports Microsoft.VisualBasic.CodeDOM_VBC
Imports Microsoft.VisualBasic.Linq.Framework.Provider
Imports Microsoft.VisualBasic.Linq.Framework.Provider.ImportsAPI
Imports System.Reflection

Namespace Framework.DynamicCode.VBC

    ''' <summary>
    ''' 编译整个LINQ语句的动态代码编译器
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DynamicCompiler : Implements IDisposable

        Public ReadOnly Property EntityProvider As TypeRegistry
        Public ReadOnly Property ApiProvider As APIProvider

        Sub New(entity As TypeRegistry, api As APIProvider)
            ApiProvider = api
            EntityProvider = entity
        End Sub

        Sub New()
            Call Me.New(TypeRegistry.LoadDefault, APIProvider.LoadDefault)
        End Sub

        Public ReadOnly Property ImportsNamespace As List(Of String) = New List(Of String)
        Public ReadOnly Property ReferenceList As New List(Of String)

        Public Sub [Imports](ns As String)
            Dim types As Type() = ApiProvider.GetType(ns)
            For Each nsDef As Type In types
                Dim name As String = nsDef.FullName
                If Not ImportsNamespace.Contains(name) Then
                    Call ImportsNamespace.Add(name)
                End If
                Dim assm As String = nsDef.Assembly.Location
                If Not ReferenceList.Contains(assm) Then
                    Call ReferenceList.Add(assm)
                End If
            Next
        End Sub

        Public Function Compile([declare] As CodeTypeDeclaration) As Type
            Dim assmUnit As CodeCompileUnit = DeclareAssembly()
            Dim ns As CodeNamespace = assmUnit.Namespaces.Item(0)
            Call ns.Types.Add([declare])
            Call ns.Imports.AddRange(Me.ImportsNamespace.ImportsNamespace)
            Dim assm As Assembly = CompileDll(assmUnit, ReferenceList, EntityProvider.SDK)
            Dim types As Type() = assm.GetTypes
            Dim name As String = [declare].Name
            Dim LQuery = (From x As Type In types
                          Where String.Equals(x.Name, name)
                          Select x).FirstOrDefault
            Return LQuery
        End Function

        Public Shared Function DeclareAssembly() As CodeCompileUnit
            Dim Assembly As CodeDom.CodeCompileUnit = New CodeDom.CodeCompileUnit
            Dim DynamicCodeNameSpace As CodeDom.CodeNamespace = New CodeDom.CodeNamespace("LINQDynamicCodeCompiled")
            Assembly.Namespaces.Add(DynamicCodeNameSpace)
            Return Assembly
        End Function

        Private Function DeclareType() As CodeDom.CodeTypeDeclaration
            Dim [Module] = DynamicCode.VBC.TokenCompiler.DeclareType(ModuleName, LINQStatement.var, LINQStatement.PreDeclare)
            Call [Module].Members.Add(New DynamicCode.VBC.WhereConditionTestCompiler(LINQStatement).Compile)
            Call [Module].Members.Add(DeclareSetObject)
            Call [Module].Members.Add(New DynamicCode.VBC.SelectConstructCompiler(LINQStatement).Compile)

            Return [Module]
        End Function

        Private Function DeclareSetObject() As CodeDom.CodeMemberMethod
            Dim StatementCollection As CodeDom.CodeStatementCollection = New CodeDom.CodeStatementCollection
            Call StatementCollection.Add(New CodeDom.CodeAssignStatement(New CodeDom.CodeFieldReferenceExpression(New CodeDom.CodeThisReferenceExpression(), LINQStatement.var.Name), New CodeDom.CodeArgumentReferenceExpression("p")))
            For Each ReadOnlyObject In LINQStatement.PreDeclare
                Call StatementCollection.Add(New DynamicCode.VBC.ReadOnlyObjectCompiler(ReadOnlyObject).Compile)
            Next

            Dim SetObject As CodeDom.CodeMemberMethod = DeclareFunction(SetObjectName, "System.Boolean", StatementCollection)
            SetObject.Parameters.Add(New CodeDom.CodeParameterDeclarationExpression(LINQStatement.var.TypeId, "p"))
            SetObject.Attributes = CodeDom.MemberAttributes.Public
            Return SetObject
        End Function

        Public Function Compile(ReferenceAssemblys As String()) As System.Reflection.Assembly
            Dim Code = DeclareAssembly()
            Dim Assembly = CodeDOMExtension.Compile(Code, ReferenceAssemblys, DotNETReferenceAssembliesDir)
            Return Assembly
        End Function

        ''' <summary>
        ''' Declare a function with a specific function name and return type. please notice that in this newly 
        ''' declare function there is always a local variable name rval using for return the value.
        ''' (申明一个方法，返回指定类型的数据并且具有一个特定的函数名，请注意，在这个新申明的函数之中，
        ''' 固定含有一个rval的局部变量用于返回数据)
        ''' </summary>
        ''' <param name="Name">Function name.(函数名)</param>
        ''' <param name="Type">Function return value type.(该函数的返回值类型)</param>
        ''' <returns>A codeDOM object model of the target function.(一个函数的CodeDom对象模型)</returns>
        ''' <remarks></remarks>
        Public Shared Function DeclareFunction(Name As String, Type As String, Statements As CodeDom.CodeStatementCollection) As CodeDom.CodeMemberMethod
            Dim CodeMemberMethod As CodeDom.CodeMemberMethod = New CodeDom.CodeMemberMethod()
            CodeMemberMethod.Name = Name : CodeMemberMethod.ReturnType = New CodeDom.CodeTypeReference(Type) '创建一个名为“WhereTest”，返回值类型为Boolean的无参数的函数
            If String.Equals(Type, "System.Boolean", StringComparison.OrdinalIgnoreCase) Then
                CodeMemberMethod.Statements.Add(New CodeDom.CodeVariableDeclarationStatement(Type, "rval", New CodeDom.CodePrimitiveExpression(True)))   '创建一个用于返回值的局部变量，对于逻辑值，默认为真
            Else
                CodeMemberMethod.Statements.Add(New CodeDom.CodeVariableDeclarationStatement(Type, "rval"))   '创建一个用于返回值的局部变量
            End If

            If Not (Statements Is Nothing OrElse Statements.Count = 0) Then
                CodeMemberMethod.Statements.AddRange(Statements)
            End If
            CodeMemberMethod.Statements.Add(New CodeDom.CodeMethodReturnStatement(New CodeDom.CodeVariableReferenceExpression("rval")))  '引用返回值的局部变量

            Return CodeMemberMethod
        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 检测冗余的调用

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO:  释放托管状态(托管对象)。
                End If

                ' TODO:  释放非托管资源(非托管对象)并重写下面的 Finalize()。
                ' TODO:  将大型字段设置为 null。
            End If
            Me.disposedValue = True
        End Sub

        ' TODO:  仅当上面的 Dispose( disposing As Boolean)具有释放非托管资源的代码时重写 Finalize()。
        'Protected Overrides Sub Finalize()
        '    ' 不要更改此代码。    请将清理代码放入上面的 Dispose( disposing As Boolean)中。
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' Visual Basic 添加此代码是为了正确实现可处置模式。
        Public Sub Dispose() Implements IDisposable.Dispose
            ' 不要更改此代码。    请将清理代码放入上面的 Dispose (disposing As Boolean)中。
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class
End Namespace