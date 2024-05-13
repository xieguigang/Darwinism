#Region "Microsoft.VisualBasic::e15b7a9dbc3ca96bce9c6062ff5f6b36, src\computing\Parallel\IpcParallel\Stream\IPCError.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.



    ' /********************************************************************************/

    ' Summaries:


    ' Code Statistics:

    '   Total Lines: 55
    '    Code Lines: 42
    ' Comment Lines: 0
    '   Blank Lines: 13
    '     File Size: 1.72 KB


    '     Class IPCError
    ' 
    '         Properties: exceptionName, inner, message, stackTrace
    ' 
    '         Constructor: (+2 Overloads) Sub New
    '         Function: CreateError, GetAllErrorMessages, GetSourceTrace, ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

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

            Return New IPCException(messages, trace)
        End Function

    End Class
End Namespace
