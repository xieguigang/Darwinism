Imports System.Reflection
Imports Microsoft.VisualBasic.LINQ.Framework.ObjectModel
Imports Microsoft.VisualBasic.LINQ.Framework.Provider
Imports Microsoft.VisualBasic.LINQ.LDM.Expression
Imports Microsoft.VisualBasic.LINQ.Script
Imports Microsoft.VisualBasic.Linq.LDM.Statements

Namespace Framework

    Public Class LQueryFramework : Implements System.IDisposable

        ''' <summary>
        ''' LINQ查询框架的默认注册表文件的文件名
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DefaultFile As String = "./Settings/LinqRegistry.xml"

        Public Property Registry As TypeRegistry
        Public Property Runtime As DynamicsRuntime

        ''' <summary>
        ''' 本模块的完整的文件路径
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property ReferenceAssembly As String = App.ExecutablePath

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="library">Registry library xml file path</param>
        Sub New(Optional library As String = DefaultFile)
            Registry = TypeRegistry.Load(library)
        End Sub

        Const TypeEntryMissingException As String =
            "There is a type missing error in this linq statement, where could not found any type registry information for type id ""{0}"""
        Const LinqInterfaceMissingException As String =
            "There is a type missing error while trying to load the {0} ILINQ interface type definition."

        ''' <summary>
        ''' 加载外部模块，并查询出目标类型的ILINQCollection接口类型信息
        ''' </summary>
        ''' <param name="typeId">目标对象的类型标识符，即RegistryItem对象中的Name属性</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function [GetType](typeId As String) As Type
            Dim typeDef As TypeEntry = Registry.Find(typeId)

            If typeDef Is Nothing Then
                Throw New TypeMissingExzception(TypeEntryMissingException, typeId)
            End If

            Dim assm As Assembly = typeDef.LoadAssembly
            Dim type As Type = assm.GetType(typeDef.TypeId, False, False)

            If type Is Nothing Then
                Throw New TypeMissingExzception(LinqInterfaceMissingException, typeId)
            Else
                Return type
            End If
        End Function

        ''' <summary>
        ''' 查找出目标模块之中的含有指定的自定义属性的所有类型
        ''' </summary>
        ''' <param name="assembly"></param>
        ''' <param name="findEntry">目标自定义属性的类型</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function LoadAssembly(assembly As Assembly, findEntry As Type) As TypeInfo()
            Dim LQuery = From [mod] As TypeInfo
                         In assembly.DefinedTypes
                         Let attrs As Object() = [mod].GetCustomAttributes(findEntry, inherit:=False)
                         Where Not attrs Is Nothing AndAlso attrs.Length = 1
                         Select [mod] '
            Return LQuery.ToArray
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="name"></param>
        ''' <param name="target"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetValue(name As String, target As Object) As Object
            Dim Type As Type = target.GetType
            Dim Properties As PropertyInfo() = Type.GetProperties
            Dim LQuery = From [Property] As PropertyInfo
                         In Properties
                         Where String.Equals([Property].Name, name, StringComparison.OrdinalIgnoreCase)
                         Select [Property] '
            Dim result As PropertyInfo = LQuery.FirstOrDefault
            If Not result Is Nothing Then
                Return result.GetValue(target)
            Else
                Dim ex As String = String.Format(PropertyNotFound, name)
                Throw New DataException(ex)
            End If
        End Function

        Public Const PropertyNotFound As String = "No such a property named '{0}' in the target type information."

        ''' <summary>
        ''' Exception for [We could not found any registered type information from the type registry.]
        ''' </summary>
        ''' <remarks></remarks>
        Public Class TypeMissingExzception : Inherits Exception

            Public Overrides ReadOnly Property Message As String

            Sub New(msg As String, ParamArray arg As String())
                Me.Message = String.Format(msg, arg)
            End Sub

            Public Overrides Function ToString() As String
                Return Message
            End Function
        End Class

        ''' <summary>
        ''' Execute a compiled LINQ statement object model to query a object-orientale database.
        ''' </summary>
        ''' <param name="statement"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Dim List As List(Of Object) = New List(Of Object)
        ''' 
        ''' For Each [Object] In LINQ.GetCollection(Statement)
        '''    Call SetObject([Object])
        '''    If True = Test() Then
        '''        List.Add(SelectConstruct())
        '''    End If
        ''' Next
        ''' Return List.ToArray
        ''' </remarks>
        Public Function EXEC(statement As LINQStatement) As IEnumerable
            Using obj As ObjectModel.Linq = __createObject(statement)
                Return obj.EXEC
            End Using
        End Function

        ''' <summary>
        ''' 创建一个LINQ查询表达式的对象句柄
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function __createObject(statement As LINQStatement) As ObjectModel.Linq
            Dim expr As Expression = New Expression(statement, Registry)

            If expr.source.IsParallel Then
                '     Return New ParallelLINQ(Statement:=statement, FrameworkRuntime:=Me.Runtime)
            Else
                '      Return New ObjectModel.LINQ(Statement:=statement, Runtime:=Me.Runtime)
            End If
        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 检测冗余的调用

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    Call Registry.Dispose()
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