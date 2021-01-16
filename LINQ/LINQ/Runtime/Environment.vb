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
                Throw New MissingPrimaryKeyException(name)
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