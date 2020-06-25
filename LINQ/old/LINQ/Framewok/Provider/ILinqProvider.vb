#Region "Microsoft.VisualBasic::9acfa44be9588becd290eab8e587a850, LINQ\LINQ\Framewok\Provider\ILinqProvider.vb"

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

    '     Delegate Function
    ' 
    ' 
    '     Module RegistryReader
    ' 
    '         Function: GetHandle, GetResource
    ' 
    '     Class DelegateProvider
    ' 
    '         Properties: MethodInfo
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: GetLinqResource, ToString
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace Framework.Provider

    ''' <summary>
    ''' Get a Collection of the target LINQ entity from file object.(从文件对象获取目标LINQ实体对象的集合)
    ''' </summary>
    ''' <param name="uri">File path or resource from url</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Delegate Function GetLinqResource(uri As String) As IEnumerable

    Public Module RegistryReader

        ''' <summary>
        ''' 数据源的示例函数
        ''' </summary>
        ''' <param name="uri"></param>
        ''' <returns></returns>
        <LinqEntity("typeDef", GetType(TypeEntry))>
        Public Function GetResource(uri As String) As IEnumerable
            Dim registry As TypeRegistry = TypeRegistry.Load(uri)
            Return registry.typeDefs
        End Function

        ''' <summary>
        ''' 获取数据源的读取接口
        ''' </summary>
        ''' <param name="provider"></param>
        ''' <returns></returns>
        <Extension>
        Public Function GetHandle(provider As TypeEntry) As GetLinqResource
            Try
                Dim type As Type = provider.Repository.GetType
                Dim method As MethodInfo = type.GetMethod(provider.Func, types:={GetType(String)})
                Dim [delegate] As New DelegateProvider(method)
                Dim handle As GetLinqResource = AddressOf [delegate].GetLinqResource
                Return handle
            Catch ex As Exception
                ex = New Exception(provider.name, ex)
                ex = New Exception(provider.GetJson, ex)
                Call App.LogException(ex)
                Return Nothing
            End Try
        End Function
    End Module

    Public Class DelegateProvider

        Public ReadOnly Property MethodInfo As MethodInfo

        ''' <summary>
        ''' 函数句柄
        ''' </summary>
        ''' <param name="h"></param>
        Sub New(h As MethodInfo)
            MethodInfo = h
        End Sub

        ''' <summary>
        ''' 实现数据源的读取接口
        ''' </summary>
        ''' <param name="uri"></param>
        ''' <returns></returns>
        Public Function GetLinqResource(uri As String) As IEnumerable
            Dim value As Object = MethodInfo.Invoke(Nothing, {uri})
            Return DirectCast(value, IEnumerable)
        End Function

        Public Overrides Function ToString() As String
            Return MethodInfo.FullName(True)
        End Function
    End Class
End Namespace
