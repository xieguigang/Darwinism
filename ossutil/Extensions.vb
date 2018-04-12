Imports System.Runtime.CompilerServices

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

    End Function
End Module
