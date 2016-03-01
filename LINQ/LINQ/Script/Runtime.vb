Imports System.Dynamic
Imports Microsoft.VisualBasic.LINQ.Framework
Imports Microsoft.VisualBasic.LINQ.Framework.Provider

Namespace Script

    ''' <summary>
    ''' LINQ脚本查询环境
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DynamicsRuntime : Inherits DynamicObject
        Implements System.IDisposable

        Dim _vars As Dictionary(Of String, Variable) = New Dictionary(Of String, Variable)
        Dim _registry As TypeRegistry

        Public Function Evaluate(script As String) As IEnumerable

        End Function

        Public Function Initialize() As Boolean
            Return True
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
            If _vars.ContainsKey(source.ToLower.ShadowCopy(source)) Then
                Return _vars(source).Data
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
            If _vars.ContainsKey(name.ToLower.ShadowCopy(name)) Then
                Call _vars.Remove(name)
            End If
            Call _vars.Add(name, New Variable With {.Name = name, .Data = source})
            Return True
        End Function

        Public Overrides Function ToString() As String
            Return String.Format("{0} variables in the LINQ runtime.", _vars.Count)
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
    End Class
End Namespace