Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Text.Parser

Namespace ProtocolBuffers.Model

    ''' <summary>
    ''' Definition parser of ProtocolBuffers
    ''' </summary>
    Public Class Parser

        ReadOnly chars As CharPtr

        Private Sub New(defText As String)
            chars = New CharPtr(defText)
        End Sub

        Private Function doParse() As BufferModel

        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Function TryParse(defText As String) As BufferModel
            Return New Parser(defText).doParse
        End Function
    End Class
End Namespace