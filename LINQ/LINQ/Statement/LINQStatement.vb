Imports System.Text.RegularExpressions
Imports System.Text
Imports Microsoft.VisualBasic.LINQ.Framework.DynamicCode.VBC
Imports Microsoft.VisualBasic.LINQ.Statements.Tokens
Imports Microsoft.VisualBasic.LINQ.Framework.DynamicCode
Imports Microsoft.VisualBasic.LINQ.Framework

Namespace Statements

    ''' <summary>
    ''' A linq statement object model.
    ''' </summary>
    ''' <remarks>
    ''' From [Object [As TypeId]] 
    ''' In [Collection] 
    ''' Let [Declaration1, Declaration2, ...]
    ''' Where [Condition Test] 
    ''' Select [Object/Object Constrctor] 
    ''' [Distinct] 
    ''' [Order Statement]</remarks>
    Public Class LINQStatement

        ''' <summary>
        ''' An object element in the target query collection.(目标待查询集合之中的一个元素)
        ''' </summary>
        ''' <remarks></remarks>
        Public Property [Object] As LINQ.Statements.Tokens.ObjectDeclaration
        ''' <summary>
        ''' Where test condition for the query.(查询所使用的Where条件测试语句)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConditionTest As LINQ.Statements.Tokens.WhereCondition
        ''' <summary>
        ''' Target query collection expression, this can be a file path or a database connection string.
        ''' (目标待查询集合，值可以为一个文件路径或者数据库连接字符串)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Collection As LINQ.Statements.Tokens.ObjectCollection
        ''' <summary>
        ''' A read only object collection which were construct by the LET statement token in the LINQ statement.
        ''' (使用Let语句所构造出来的只读对象类型的对象申明集合)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ReadOnlyObjects As LINQ.Statements.Tokens.ReadOnlyObject()
        ''' <summary>
        ''' A expression for return the query result.(用于生成查询数据返回的语句)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SelectConstruct As LINQ.Statements.Tokens.SelectConstruct

        Friend _Tokens As String()
        Friend TypeRegistry As LINQ.Framework.TypeRegistry
        ''' <summary>
        ''' 本LINQ脚本对象所编译出来的临时模块
        ''' </summary>
        ''' <remarks></remarks>
        Friend ILINQProgram As System.Type

        Public ReadOnly Property CompiledCode As String

        ''' <summary>
        ''' 获取目标LINQCollection待查询集合中的元素对象的类型标识符，以进行外部模块的动态加载
        ''' 与RegistryItem中的Name属性值相对应
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property TypeId As String
            Get
                Return [Object].TypeId
            End Get
        End Property

        Public ReadOnly Property IsParallel As Boolean
            Get
                Return Me.Collection.IsParallel
            End Get
        End Property

        Public ReadOnly Property Original As String

        ''' <summary>
        ''' Create a instance for the compiled LINQ statement object model.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateInstance() As Object
            Return Activator.CreateInstance(ILINQProgram)
        End Function

        Public Overrides Function ToString() As String
            Return Original
        End Function

        ''' <summary>
        ''' Try to parsing a linq query script into a statement object model and compile the model into a assembly dynamic.
        ''' (尝试着从所输入的命令语句中解析出一个LINQ查询命令对象，并完成动态编译过程)
        ''' </summary>
        ''' <param name="StatementText"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function TryParse(StatementText As String, registry As TypeRegistry) As LINQStatement
            Dim Statement As LINQStatement = New LINQStatement With {
                ._Original = StatementText,
                ._Tokens = GetTokens(StatementText),
                .TypeRegistry = registry
            }
            Statement.Collection = New ObjectCollection(Statement)
            Statement.Object = New ObjectDeclaration(Statement)
            Statement.ReadOnlyObjects = GetReadOnlyObjects(Statement)
            Statement.ConditionTest = New WhereCondition(Statement)
            Statement.SelectConstruct = New SelectConstruct(Statement)

            Using Compiler As DynamicCompiler = New DynamicCompiler(Statement, SDK_PATH.AvaliableSDK) 'Dynamic code compiling.(动态编译代码)
                Dim LINQEntityLibFile As String = Statement.Object.RegistryType.AssemblyFullPath '

                If Not String.Equals(FileIO.FileSystem.GetParentPath(LINQEntityLibFile), System.Windows.Forms.Application.StartupPath) Then
                    LINQEntityLibFile = String.Format("{0}\TEMP_LINQ.Entity.lib", System.Windows.Forms.Application.StartupPath)

                    If FileIO.FileSystem.FileExists(LINQEntityLibFile) Then
                        Call FileIO.FileSystem.DeleteFile(LINQEntityLibFile, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                    End If
                    Call FileIO.FileSystem.CopyFile(Statement.Object.RegistryType.AssemblyFullPath, LINQEntityLibFile)
                End If

                Dim ReferenceAssemblys As String() = New String() {LQueryFramework.ReferenceAssembly, LINQEntityLibFile}
                Dim CompiledAssembly = Compiler.Compile(ReferenceAssemblys)
                Statement.ILINQProgram = DynamicInvoke.GetType(CompiledAssembly, Framework.DynamicCode.VBC.DynamicCompiler.ModuleName).First
                Statement._CompiledCode = Compiler.CompiledCode
            End Using

            Return Statement.Initialzie
        End Function

        Private Function Initialzie() As LINQStatement
            Call [Object].Initialize()
            Call ConditionTest.Initialize()
            Call SelectConstruct.Initialzie()
            Return Me
        End Function
    End Class
End Namespace