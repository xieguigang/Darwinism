Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports SMRUCC.HTTPInternal.AppEngine
Imports SMRUCC.HTTPInternal.AppEngine.APIMethods
Imports SMRUCC.HTTPInternal.Platform

''' <summary>
''' 对外部提供Linq查询服务的WebApp
''' </summary>
<[Namespace]("Linq")>
Public Class LinqAPI : Inherits WebApp

    Sub New(main As PlatformEngine)
        Call MyBase.New(main)
    End Sub

    ''' <summary>
    ''' 通过这个默认的API复写方法来执行linq查询的创建
    ''' </summary>
    ''' <param name="uri"></param>
    ''' <param name="args"></param>
    ''' <param name="out"></param>
    ''' <returns></returns>
    Public Function ExecLinq(uri As String, args As String, ByRef out As String) As Boolean

    End Function

    <ExportAPI("/linq/move_next.vb",
               Info:="Execute linq iterator move next and then returns a data set. If the n number parameter is default 1, then a single object will returns, or a array of object will be returns.",
               Usage:="/linq/move_next.vb?uid=&lt;hashCode>&n=&lt;numbers,default is 1>")>
    <[GET](GetType(String))>
    Public Function MoveNext(args As String) As String

    End Function

    Public Overrides Function Page404() As String
        Return PlatformEngine.Page404
    End Function
End Class
