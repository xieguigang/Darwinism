Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Language

Public Module Extensions

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <Extension>
    Public Function IsDirectory(obj As [Object]) As Boolean
        Return obj.ObjectName.Last = "/"c AndAlso obj.Size = 0
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="current$">Current path</param>
    ''' <param name="rel$">Relative path</param>
    ''' <param name="root">Root name</param>
    ''' <returns></returns>
    ''' 
    <Extension>
    Public Function ChangeFileSystemContext(current$, root$, rel$) As String
        Dim currentTokens = current.SplitPath.AsList
        Dim relTokens$() = rel.SplitPath
        Dim relWords As New List(Of String)

        For Each word In relWords
            If word = "." Then
                ' No change
            ElseIf word = ".." Then
                ' Parent directory
                currentTokens.Pop()
            Else
                relWords += word
            End If
        Next

        current = currentTokens.JoinBy("/")
        rel = relWords.JoinBy("/")

        ' 防止通过..操作进行越权
        If InStr(current, root) = 0 Then
            current = root & "/" & current
        End If

        Dim path$ = (current & "/" & rel).StringReplace("[/]{2,}", "/")
        Return path
    End Function
End Module
