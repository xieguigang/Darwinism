Imports System.Reflection
Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.Serialization

Namespace Framework.Provider.ImportsAPI

    ''' <summary>
    ''' 只需要记住命名空间和对应的Assembly的引用就行了
    ''' </summary>
    Public Class APIProvider : Implements ISaveHandle
        Implements IDisposable

        Public Property Packages As ImportsNs()
            Get
                Return __nsList.ToArray
            End Get
            Set(value As ImportsNs())
                If value Is Nothing Then
                    __nsList = New List(Of ImportsNs)
                Else
                    __nsList = value.ToList
                End If
            End Set
        End Property

        Dim __nsList As List(Of ImportsNs)

        Public Shared ReadOnly Property DefaultFile As String =
            App.ProductSharedDIR & "/API.Imports.json"

        Public Function Register(assm As Assembly) As Boolean

        End Function

        Public Function Save(Optional Path As String = "", Optional encoding As Encoding = Nothing) As Boolean Implements ISaveHandle.Save
            Return Me.GetJson.SaveTo(If(String.IsNullOrEmpty(Path), DefaultFile, Path), encoding)
        End Function

        Public Function Save(Optional Path As String = "", Optional encoding As Encodings = Encodings.UTF8) As Boolean Implements ISaveHandle.Save
            Return Save(Path, encoding.GetEncodings)
        End Function

        Sub Save()
            Call Save(DefaultFile, Encodings.ASCII)
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                    Call Save()
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            ' TODO: uncomment the following line if Finalize() is overridden above.
            ' GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class
End Namespace