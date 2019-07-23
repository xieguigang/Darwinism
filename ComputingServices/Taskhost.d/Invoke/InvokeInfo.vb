#Region "Microsoft.VisualBasic::a23ede7ec89a19fecb80b31cf5db5ba6, ComputingServices\Taskhost.d\Invoke\InvokeInfo.vb"

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

    '     Class Argv
    ' 
    '         Properties: Type, value
    ' 
    '         Constructor: (+2 Overloads) Sub New
    '         Function: GetValue, ToString
    ' 
    '     Class InvokeInfo
    ' 
    '         Properties: Name, Parameters
    ' 
    '         Function: CreateObject, GetMethod, GetParameters, ToString
    ' 
    '         Sub: SetArgs
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Reflection
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Scripting
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace TaskHost

    ''' <summary>
    ''' Json value of the function parameter, and the type information is also included in this property.
    ''' </summary>
    Public Class Argv

        ''' <summary>
        ''' <see cref="System.Type.FullName"/>
        ''' </summary>
        ''' <returns><see cref="System.Type.FullName"/></returns>
        Public Property Type As String
        ''' <summary>
        ''' Json string
        ''' </summary>
        ''' <returns></returns>
        Public Property value As String

        Sub New()
        End Sub

        ''' <summary>
        ''' Creates the function remote calls one of its parameter value. 
        ''' </summary>
        ''' <param name="o"></param>
        Sub New(o As Object)
            Dim type As Type = o.GetType

            Me.Type = type.FullName
            Me.value = JsonContract.GetObjectJson(o, type)
        End Sub

        Public Overrides Function ToString() As String
            Return $"[{Type.Split("."c).Last}]  " & value
        End Function

        Public Function GetValue() As Object
            Dim type As Type = Type.GetType(Me.Type, True, False)
            Dim o As Object = JsonContract.LoadObject(value, type)
            Return o
        End Function
    End Class

    ''' <summary>
    ''' 分布式计算框架之中的远程调用的参数信息
    ''' </summary>
    Public Class InvokeInfo : Inherits MetaData.TypeInfo

        ''' <summary>
        ''' 函数名
        ''' </summary>
        ''' <returns></returns>
        Public Property Name As String
        ''' <summary>
        ''' json value.(函数参数)
        ''' </summary>
        ''' <returns></returns>
        Public Property Parameters As Argv()

        Public Function GetMethod() As MethodInfo
            Dim type As Type = [GetType]()
            Dim func As MethodInfo = type.GetMethod(Name, BindingFlags.Public Or BindingFlags.Static)
            Return func
        End Function

        Public Overrides Function ToString() As String
            Return $"{assm}!{FullIdentity}::{Name}"
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="method"></param>
        ''' <param name="args">json</param>
        ''' <returns></returns>
        Public Shared Function GetParameters(method As MethodInfo, args As String()) As Object()
            Dim params As Type() = method.GetParameters.Select(Function(x) x.ParameterType).ToArray
            Dim values As Object() = args.Select(Function(x, idx) JsonContract.LoadObject(x, params(idx))).ToArray
            Return values
        End Function

        Public Sub SetArgs(ParamArray args As Object())
            Me.Parameters = args.Select(Function(x) New Argv(x)).ToArray
        End Sub

        Public Shared Function CreateObject(func As [Delegate], args As Object()) As InvokeInfo
            Dim type As Type = func.Method.DeclaringType
            Dim assm As Assembly = type.Assembly
            Dim name As String = func.Method.Name
            Dim params As Argv() = args.Select(Function(x) New Argv(x)).ToArray   ' 由于函数调用的参数的类型可能是基类，所以json序列化操作会存在问题，在这里使用这个新的参数构建模块来避免这个问题

            Return New InvokeInfo With {
                .assm = FileIO.FileSystem.GetFileInfo(assm.Location).Name,
                .Name = name,
                .Parameters = params,
                .FullIdentity = type.FullName
            }
        End Function
    End Class
End Namespace
