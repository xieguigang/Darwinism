Imports System.Dynamic
Imports Microsoft.VisualBasic.Linq.LDM.Statements
Imports Microsoft.VisualBasic.Linq.Framework
Imports Microsoft.VisualBasic.Linq.Framework.Provider
Imports Microsoft.VisualBasic.Linq.Framework.ObjectModel
Imports Microsoft.VisualBasic.Linq.Framework.Provider.ImportsAPI

Namespace Script

    ''' <summary>
    ''' LINQ脚本数据源查询运行时环境
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DynamicsRuntime : Inherits DynamicObject
        Implements System.IDisposable

        ReadOnly _varsHash As Dictionary(Of String, Variable) = New Dictionary(Of String, Variable)

        Public ReadOnly Property Types As TypeRegistry
        Public ReadOnly Property Compiler As DynamicCode.DynamicCompiler
        Public ReadOnly Property API As APIProvider

        Sub New(entity As TypeRegistry, api As APIProvider)
            Me.API = api
            Me.Types = entity
            Me.Compiler = New DynamicCode.DynamicCompiler(entity, api)
        End Sub

        Sub New()
            Call Me.New(TypeRegistry.LoadDefault, APIProvider.LoadDefault)
        End Sub


        Public Function Evaluate(script As String) As IEnumerable

        End Function

        Public Function SetVariable(var As String, value As Object) As Boolean

        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose( disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose( disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

        Public Function GetResource(source As String) As IEnumerable
            If _varsHash.ContainsKey(source.ToLower.ShadowCopy(source)) Then
                Return _varsHash(source).Data
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="name"></param>
        ''' <param name="source"></param>
        ''' <remarks></remarks>
        Public Function SetObject(name As String, source As IEnumerable) As Boolean
            If _varsHash.ContainsKey(name.ToLower.ShadowCopy(name)) Then
                Call _varsHash.Remove(name)
            End If
            Call _varsHash.Add(name, New Variable With {.Name = name, .Data = source})
            Return True
        End Function

        Public Overrides Function ToString() As String
            Return String.Format("{0} variables in the LINQ runtime.", _varsHash.Count)
        End Function

        ''' <summary>
        ''' 执行一个LINQ查询脚本文件
        ''' </summary>
        ''' <param name="FilePath">LINQ脚本文件的文件路径</param>
        ''' <returns></returns>
        ''' <remarks>
        ''' 脚本要求：
        ''' 
        ''' </remarks>
        Public Function Source(FilePath As String) As Boolean

        End Function

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
        Public Function EXEC(statement As LinqStatement) As IEnumerable
            Using linq As ObjectModel.Linq = __new(statement)
                Return linq.EXEC
            End Using
        End Function

        Private Function __new(statement As LinqStatement) As ObjectModel.Linq
            Return If(statement.source.IsParallel, New ParallelLinq(statement, Me), New ObjectModel.Linq(statement, Me))
        End Function
    End Class
End Namespace