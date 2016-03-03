Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.ComputingServices.TaskHost
Imports Microsoft.VisualBasic.Net
Imports SMRUCC.HTTPInternal.AppEngine
Imports SMRUCC.HTTPInternal.AppEngine.APIMethods
Imports SMRUCC.HTTPInternal.Platform

''' <summary>
''' 对外部提供Linq查询服务的WebApp
''' </summary>
Public Class LinqAPI : Inherits LinqPool

    Public ReadOnly Property Repository As Repository

    Sub New(repo As Repository)
        Me.Repository = repo
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="url">数据源的引用位置</param>
    ''' <param name="args">查询参数</param>
    ''' <returns></returns>
    Public Overloads Function OpenQuery(url As String, args As String) As LinqEntry
        Dim source As IEnumerable = Repository.GetRepository(url, args) ' expr为空的话，则没有where测试，则返回所有数据
        Dim type As Type = Repository.GetType(url)  ' 得到元素的类型信息
        Dim linq As IPEndPoint = OpenQuery(source, type)

    End Function

    Public Function MoveNext(args As Dictionary(Of String, String)) As String

    End Function

    ''' <summary>
    ''' 释放掉一个Linq查询的资源
    ''' </summary>
    ''' <param name="args"></param>
    ''' <returns></returns>
    Public Overloads Function Free(args As Dictionary(Of String, String)) As String

    End Function
End Class
