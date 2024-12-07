#Region "Microsoft.VisualBasic::a1ddd0cb325ef27dc042fc74c8b5d7fc, src\data\LINQ\LINQ\Runtime\Environment.vb"

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

    '   Total Lines: 73
    '    Code Lines: 57 (78.08%)
    ' Comment Lines: 3 (4.11%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 13 (17.81%)
    '     File Size: 2.19 KB


    '     Class Environment
    ' 
    '         Properties: GlobalEnvir, IsGlobal
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: AddSymbol, FindInvoke, FindSymbol, HasSymbol
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports LINQ.Runtime.Internal

Namespace Runtime

    ''' <summary>
    ''' A linq runtime environment
    ''' </summary>
    Public Class Environment

        Protected ReadOnly symbols As New Dictionary(Of String, Symbol)
        Protected ReadOnly parent As Environment

        Public ReadOnly Property IsGlobal As Boolean
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return parent Is Nothing
            End Get
        End Property

        Public ReadOnly Property GlobalEnvir As GlobalEnvironment
            Get
                If parent Is Nothing Then
                    Return Me
                Else
                    Return parent.GlobalEnvir
                End If
            End Get
        End Property

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Sub New(parent As Environment)
            Me.parent = parent
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function FindInvoke(name As String) As Callable
            Return InternalInvoke.FindInvoke(name)
        End Function

        Public Function HasSymbol(name As String) As Boolean
            If symbols.ContainsKey(name) Then
                Return True
            ElseIf Not parent Is Nothing Then
                Return parent.HasSymbol(name)
            Else
                Return False
            End If
        End Function

        Public Function AddSymbol(name As String, type As String) As Symbol
            Dim newSymbol As New Symbol With {
                .SymbolKey = name,
                .type = type
            }

            Call symbols.Add(name, newSymbol)

            Return newSymbol
        End Function

        Public Function FindSymbol(name As String) As Symbol
            If symbols.ContainsKey(name) Then
                Return symbols(name)
            ElseIf Not parent Is Nothing Then
                Return parent.FindSymbol(name)
            Else
                Return Nothing
            End If
        End Function

    End Class
End Namespace
