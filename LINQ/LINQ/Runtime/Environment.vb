#Region "Microsoft.VisualBasic::a09e923bdeddc127b386c6f1ad992995, LINQ\LINQ\Runtime\Environment.vb"

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

    '     Class Environment
    ' 
    '         Properties: GlobalEnvir, IsGlobal
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: AddSymbol, FindInvoke, FindSymbol, HasSymbol
    ' 
    '     Class GlobalEnvironment
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: GetDriverByCode
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel

Namespace Runtime

    Public Class Environment

        Protected ReadOnly symbols As New Dictionary(Of String, Symbol)
        Protected ReadOnly parent As Environment

        Public ReadOnly Property IsGlobal As Boolean
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

        Sub New(parent As Environment)
            Me.parent = parent
        End Sub

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

    Public Class GlobalEnvironment : Inherits Environment

        Protected ReadOnly registry As Registry

        Sub New(registry As Registry, ParamArray values As NamedValue(Of Object)())
            Call MyBase.New(Nothing)

            Dim typeCode As String
            Dim symbol As Symbol

            For Each item In values
                typeCode = registry.GetTypeCodeName(item.Value.GetType)
                symbol = AddSymbol(item.Name, typeCode)
                symbol.value = item.Value
            Next

            Me.registry = registry
        End Sub

        Public Function GetDriverByCode(code As String) As DataSourceDriver
            Return registry.GetReader(code)
        End Function
    End Class
End Namespace
