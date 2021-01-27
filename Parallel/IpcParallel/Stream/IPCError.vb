Imports Microsoft.VisualBasic.ApplicationServices.Debugging.Diagnostics

Namespace IpcStream

    Public Class IPCError

        Public Property message As String
        Public Property stackTrace As StackFrame()
        Public Property inner As IPCError
        Public Property exceptionName As String

        Sub New()
        End Sub

        Sub New(ex As Exception)
            exceptionName = ex.GetType.Name
            message = ex.Message
            stackTrace = ExceptionData.ParseStackTrace(ex.StackTrace)

            If Not ex.InnerException Is Nothing Then
                inner = New IPCError(ex.InnerException)
            End If
        End Sub

        Public Iterator Function GetAllErrorMessages() As IEnumerable(Of String)
            If Not inner Is Nothing Then
                For Each msg As String In inner.GetAllErrorMessages
                    Yield msg
                Next
            End If

            Yield $"{exceptionName}: {message}"
        End Function

        Public Function GetSourceTrace() As StackFrame()
            If Not inner Is Nothing Then
                Return inner.GetSourceTrace
            Else
                Return stackTrace
            End If
        End Function

        Public Overrides Function ToString() As String
            Return $"{exceptionName}: {message}"
        End Function

        Public Shared Function CreateError(err As IPCError) As Exception
            Dim messages As String() = err.GetAllErrorMessages.ToArray
            Dim trace As StackFrame() = err.GetSourceTrace

        End Function

    End Class
End Namespace