Imports Microsoft.VisualBasic.ApplicationServices.Debugging.Diagnostics

Namespace IpcStream

    Public Class IPCException : Inherits Exception

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' # https://stackoverflow.com/questions/912420/throw-exceptions-with-custom-stack-trace
        ''' 
        ''' The StackTrace property is virtual - create your own derived Exception class and have the property return whatever you want.
        ''' </remarks>
        Public Overrides ReadOnly Property StackTrace As String
            Get
                Return _stackTrace
            End Get
        End Property

        ReadOnly _stackTrace As String

        Sub New(messages As String(), stackTrace As StackFrame())
            Call MyBase.New(messages.JoinBy(" -> "))

            _stackTrace = stackTrace _
                .Select(Function(a) a.ToString) _
                .JoinBy(vbCrLf)
        End Sub
    End Class
End Namespace