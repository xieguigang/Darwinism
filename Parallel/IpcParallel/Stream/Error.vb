Imports Microsoft.VisualBasic.ApplicationServices.Debugging.Diagnostics

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
        stackTrace = {StackFrame.Parser(ex.StackTrace)}

        If Not ex.InnerException Is Nothing Then
            inner = New IPCError(ex.InnerException)
        End If
    End Sub

End Class
