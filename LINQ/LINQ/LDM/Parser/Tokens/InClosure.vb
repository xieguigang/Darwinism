Imports Microsoft.VisualBasic.LINQ.Framework
Imports Microsoft.VisualBasic.LINQ.Framework.Provider

Namespace LDM.Statements.Tokens

    ''' <summary>
    ''' 从字符串引用
    ''' </summary>
    Public Class UriRef : Inherits InClosure

        Public Property URI As String

        Public Overrides ReadOnly Property Type As SourceTypes
            Get
                Return SourceTypes.FileURI
            End Get
        End Property

        Sub New(tokens As ClosureTokens, parent As LINQStatement)
            Call MyBase.New(tokens, parent)
            URI = tokens.Tokens.First.TokenValue.GetString
        End Sub

        Public Overrides Function ToString() As String
            Return $"[In] uri:={URI}"
        End Function
    End Class

    Public Class Reference : Inherits InClosure

        Public Overrides ReadOnly Property Type As SourceTypes
            Get
                Return SourceTypes.Reference
            End Get
        End Property

        Sub New(tokens As ClosureTokens, parent As LINQStatement)
            Call MyBase.New(tokens, parent)
        End Sub

        Public Overrides Function ToString() As String
            Return Source.ToString
        End Function
    End Class

    Public Enum SourceTypes
        ''' <summary>
        ''' 目标集合类型为一个数据文件
        ''' </summary>
        ''' <remarks>只有一个元素，并且为字符串类型</remarks>
        FileURI
        ''' <summary>
        ''' 目标集合类型为一个内存对象的引用
        ''' </summary>
        ''' <remarks>为表达式，含有多个Token</remarks>
        Reference
    End Enum

    ''' <summary>
    ''' 表示目标对象的数据集合的文件路径或者内存对象的引用
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class InClosure : Inherits Closure

        Public MustOverride ReadOnly Property Type As SourceTypes

        Sub New(token As ClosureTokens, parent As LINQStatement)
            Call MyBase.New(token, parent)
        End Sub

        Public Shared Function CreateObject(tokens As ClosureTokens(), parent As LINQStatement) As InClosure
            Dim source As ClosureTokens = Closure.GetTokens(TokenIcer.Tokens.In, tokens)
            If source.Tokens.Length = 1 AndAlso
               source.Tokens(Scan0).TokenName = TokenIcer.Tokens.String Then
                Return New UriRef(source, parent)
            Else
                Return New Reference(source, parent)
            End If
        End Function
    End Class
End Namespace