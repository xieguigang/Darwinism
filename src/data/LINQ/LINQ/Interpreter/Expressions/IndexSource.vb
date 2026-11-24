#Region "Microsoft.VisualBasic::f7e6d5c4b3a2918070605040302010f0, src\data\LINQ\LINQ\Interpreter\Expressions\IndexSource.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed

    ' Summaries:

    '     Class IndexSource
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: Exec, ToString
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Data
Imports LINQ.Interpreter.Query
Imports LINQ.Runtime
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Linq
Imports any = Microsoft.VisualBasic.Scripting

Namespace Interpreter.Expressions

    ''' <summary>
    ''' 带索引的数据源构造表达式：``index(&lt;uri>, terms = [...], range = [...], fulltext = [...])``
    ''' </summary>
    ''' <remarks>
    ''' 这个表达式无法通过<see cref="FuncEval"/>来实现，因为命名参数``terms = [...]``
    ''' 会被解析为等值比较表达式，所以需要在这里直接求值命名参数所对应的字段列表。
    ''' </remarks>
    Public Class IndexSource : Inherits Expression

        Friend ReadOnly path As Expression
        Friend ReadOnly specs As NamedValue(Of Expression)()

        Sub New(path As Expression, specs As IEnumerable(Of NamedValue(Of Expression)))
            Me.path = path
            Me.specs = specs.ToArray
        End Sub

        Public Overrides Function Exec(context As ExecutableContext) As Object
            Dim uri As String = any.ToString(path.Exec(context))
            Dim terms As String() = Nothing
            Dim ranges As String() = Nothing
            Dim fulltext As String() = Nothing
            Dim levenshtein As String() = Nothing

            If uri.StringEmpty Then
                Throw New InvalidExpressionException("missing data source file for the index function!")
            End If

            For Each spec As NamedValue(Of Expression) In specs
                Dim fields As String() = ReadFields(spec.Value, context)

                Select Case Strings.LCase(spec.Name)
                    Case "terms", "hash", "index"
                        terms = fields
                    Case "range", "ranges"
                        ranges = fields
                    Case "fulltext", "full_text"
                        fulltext = fields
                    Case "levenshtein", "similar"
                        levenshtein = fields
                    Case Else
                        Throw New SyntaxErrorException($"unknown index type '{spec.Name}'!")
                End Select
            Next

            Return New IndexTableSource(uri, terms, ranges, fulltext, levenshtein)
        End Function

        Private Shared Function ReadFields(fields As Expression, context As ExecutableContext) As String()
            Dim value As Object = fields.Exec(context)

            If value Is Nothing Then
                Return New String() {}
            ElseIf TypeOf value Is String Then
                Return New String() {DirectCast(value, String)}
            ElseIf TypeOf value Is IEnumerable Then
                Return DirectCast(value, IEnumerable).Cast(Of Object).Select(AddressOf any.ToString).ToArray
            Else
                Return New String() {any.ToString(value)}
            End If
        End Function

        Public Overrides Function ToString() As String
            Return $"index({path}, {specs.Select(Function(a) $"{a.Name}=[{a.Value}]").JoinBy(", ")})"
        End Function
    End Class
End Namespace
