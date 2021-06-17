#Region "Microsoft.VisualBasic::3e1bbd34965824c90713e8099928b0c1, LINQ\LINQ\Runtime\InternalInvoke.vb"

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

    '     Class InternalInvoke
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    '         Function: FindInvoke
    ' 
    '         Sub: loadInternal
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Reflection

Namespace Runtime.Internal

    Public Class InternalInvoke

        Shared ReadOnly invokes As New Dictionary(Of String, Callable)

        Shared Sub New()
            Call loadInternal(Of Math)()
        End Sub

        Private Shared Sub loadInternal(Of T As Class)()
            Dim type As TypeInfo = GetType(T)
            Dim fields As FieldInfo() = type.DeclaredFields _
                .Where(Function(m)
                           Return m.IsStatic AndAlso m.FieldType Is GetType(Callable)
                       End Function) _
                .ToArray

            For Each item As FieldInfo In fields
                invokes(item.Name) = item.GetValue(Nothing)
            Next
        End Sub

        Public Shared Function FindInvoke(name As String) As Callable
            Return invokes.TryGetValue(name)
        End Function
    End Class
End Namespace
