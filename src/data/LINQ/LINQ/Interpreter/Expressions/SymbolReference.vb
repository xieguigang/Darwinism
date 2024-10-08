﻿#Region "Microsoft.VisualBasic::2e93db5b60e5268ea8b284f474e9e320, src\data\LINQ\LINQ\Interpreter\Expressions\SymbolReference.vb"

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

    '   Total Lines: 32
    '    Code Lines: 25 (78.12%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 7 (21.88%)
    '     File Size: 904 B


    '     Class SymbolReference
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: Exec, ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Data
Imports LINQ.Runtime

Namespace Interpreter.Expressions

    Public Class SymbolReference : Inherits Expression

        Friend ReadOnly symbolName As String

        Sub New(name As String)
            Me.symbolName = name
        End Sub

        Public Overrides Function Exec(context As ExecutableContext) As Object
            Dim symbol As Symbol = context.env.FindSymbol(symbolName)

            If symbol Is Nothing Then
                If context.throwError Then
                    Throw New MissingPrimaryKeyException(symbolName)
                Else
                    Return Nothing
                End If
            Else
                Return symbol.value
            End If
        End Function

        Public Overrides Function ToString() As String
            Return $"&{symbolName}"
        End Function
    End Class
End Namespace
