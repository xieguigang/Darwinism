#Region "Microsoft.VisualBasic::04597e5b4cb6c9351875ed36102270a5, src\data\LINQ\LINQ\Interpreter\Expressions\Literals.vb"

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

    '   Total Lines: 40
    '    Code Lines: 33
    ' Comment Lines: 0
    '   Blank Lines: 7
    '     File Size: 1.25 KB


    '     Class Literals
    ' 
    '         Properties: type, value
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: Exec, ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Language
Imports LINQ.Runtime
Imports any = Microsoft.VisualBasic.Scripting

Namespace Interpreter.Expressions

    Public Class Literals : Inherits Expression

        Public Property value As Object

        Public ReadOnly Property type As Type
            Get
                If value Is Nothing Then
                    Return GetType(Void)
                Else
                    Return value.GetType
                End If
            End Get
        End Property

        Sub New(t As Token)
            Select Case t.name
                Case Tokens.Boolean : value = t.text.ParseBoolean
                Case Tokens.Integer : value = t.text.ParseInteger
                Case Tokens.Number : value = t.text.ParseDouble
                Case Tokens.Literal : value = t.text
                Case Else
                    Throw New InvalidCastException
            End Select
        End Sub

        Public Overrides Function Exec(context As ExecutableContext) As Object
            Return value
        End Function

        Public Overrides Function ToString() As String
            Return $"({type.Name.ToLower}) {any.ToString(value, "null")}"
        End Function
    End Class
End Namespace
