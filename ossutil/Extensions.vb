Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.GraphTheory
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
    ''' <param name="relativePath$">Relative path</param>
    ''' <param name="root">Root name.(必须是绝对路径)</param>
    ''' <returns></returns>
    ''' 
    <Extension>
    Public Function ChangeFileSystemContext(current As Tree(Of [Object]), relativePath$(), Optional root$ = "/") As Tree(Of [Object])
        Dim relWords As New List(Of String)

        For Each name As String In relativePath
            If name = "." Then
                ' No change
            ElseIf name = ".." Then
                ' Parent directory
                current = current.Parent
            Else
                current = current.Childs(name)
            End If
        Next

        ' 防止通过..操作进行越权
        If InStr(current.QualifyName, root) = 0 Then
            current = current.BacktrackingRoot
        End If

        Return current
    End Function
End Module
