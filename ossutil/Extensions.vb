#Region "Microsoft.VisualBasic::50f4de2be262a991c69e0e03c6aecf58, ossutil\Extensions.vb"

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

    ' Module Extensions
    ' 
    '     Function: ChangeFileSystemContext, IsDirectory
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.GraphTheory
Imports ThinkVB.FileSystem.OSS.Model

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
