﻿#Region "Microsoft.VisualBasic::c13cff8741090c38069104bbac531de7, src\networking\Protocol\Reflection\ProtocolHandler.vb"

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

'   Total Lines: 171
'    Code Lines: 113 (66.08%)
' Comment Lines: 33 (19.30%)
'    - Xml Docs: 90.91%
' 
'   Blank Lines: 25 (14.62%)
'     File Size: 7.75 KB


'     Class ProtocolHandler
' 
'         Properties: DeclaringType, ProtocolEntry
' 
'         Constructor: (+1 Overloads) Sub New
'         Function: GetMethod, HandlePush, HandleRequest, method1, method2
'                   (+2 Overloads) SafelyCreateObject, ToString
' 
' 
' /********************************************************************************/

#End Region

Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports Darwinism.IPC.Networking.HTTP
Imports Microsoft.VisualBasic.Parallel
Imports TcpEndPoint = System.Net.IPEndPoint

Namespace Protocols.Reflection

    ''' <summary>
    ''' 这个模块只处理<see cref="DataRequestHandler"/>类型的接口
    ''' 
    ''' ```vbnet
    ''' <see cref="System.Delegate"/> Function(request As <see cref="RequestStream"/>, RemoteAddress As <see cref="TcpEndPoint"/>) As <see cref="RequestStream"/>
    ''' ```
    ''' </summary>
    Public Class ProtocolHandler : Inherits IProtocolHandler

        Protected Protocols As Dictionary(Of Long, DataRequestHandler)
        Protected debug As Boolean

        ''' <summary>
        ''' target object that current handler hooks on
        ''' </summary>
        Protected ReadOnly host As Object

        ''' <summary>
        ''' 这个类型建议一般为某种枚举类型
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property DeclaringType As Type
        Public Overrides ReadOnly Property ProtocolEntry As Long

        Public Overrides Function ToString() As String
            Return $"*{ProtocolEntry}   ---> {DeclaringType.FullName}  //{Protocols.Count} TCP Protocols."
        End Function

        Const AllInstanceMethod As BindingFlags =
            BindingFlags.Public Or
            BindingFlags.NonPublic Or
            BindingFlags.Instance

        ''' <summary>
        ''' 请注意，假若没有在目标的类型定义之中查找出入口点的定义，则这个构造函数会报错，
        ''' 假若需要安全的创建对象，可以使用<see cref="ProtocolHandler.SafelyCreateObject(Of T)(T)"/>函数
        ''' </summary>
        ''' <param name="target">protocol的实例</param>
        Sub New(target As Object, Optional debug As Boolean = False)
            Dim type As Type = target.GetType
            Dim entry As ProtocolAttribute = ProtocolAttribute.GetProtocolCategory(type)

            Me.DeclaringType = entry?.DeclaringType
            Me.ProtocolEntry = entry?.EntryPoint
            Me.debug = debug
            Me.host = target
            Me.Protocols = FindMethods(type, target, debug) _
                .ToDictionary(Function(element)
                                  Return element.protocol.EntryPoint
                              End Function,
                              Function(element)
                                  Return element.method
                              End Function)
        End Sub

        Private Shared Function FindMethods(type As Type, target As Object, debug As Boolean) As IEnumerable(Of (protocol As ProtocolAttribute, entryPoint As MethodInfo, method As DataRequestHandler))
            ' 解析出所有符合 WrapperClassTools.Net.DataRequestHandler 接口类型的函数方法
            Dim methods = type.GetMethods(bindingAttr:=AllInstanceMethod)

            Return From entryPoint As MethodInfo
                   In methods
                   Let Protocol As ProtocolAttribute = ProtocolAttribute.GetEntryPoint(entryPoint)
                   Let method As DataRequestHandler = GetMethod(target, entryPoint, debug:=debug)
                   Where Not (Protocol Is Nothing) AndAlso Not method Is Nothing
                   Select (Protocol, entryPoint, method)
        End Function

        ''' <summary>
        ''' 失败会返回空值
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="App"></param>
        ''' <returns></returns>
        Public Shared Function SafelyCreateObject(Of T As Class)(App As T) As ProtocolHandler
            Try
                Return New ProtocolHandler(App)
            Catch ex As Exception
                Call Microsoft.VisualBasic.App.LogException(ex)
                Return Nothing
            End Try
        End Function

        Public Shared Function SafelyCreateObject(App As Object) As ProtocolHandler
            Try
                Return New ProtocolHandler(App)
            Catch ex As Exception
                Call Microsoft.VisualBasic.App.LogException(ex)
                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Handle the data request from the client for socket events: ResponseHandler.
        ''' </summary>
        ''' <param name="request">The request stream object which contains the commands from the client</param>
        ''' <param name="remoteDevcie">The IPAddress of the target incoming client data request.</param>
        ''' <returns></returns>
        Public Overrides Function HandleRequest(request As RequestStream, remoteDevcie As TcpEndPoint) As BufferPipe
            If request.ProtocolCategory <> Me.ProtocolEntry Then
#If DEBUG Then
                Call $"Protocol_entry:={request.ProtocolCategory} was not found!".__DEBUG_ECHO
#End If
                Return New DataPipe(NetResponse.RFC_VERSION_NOT_SUPPORTED)
            End If

            If Not Me.Protocols.ContainsKey(request.Protocol) Then
#If DEBUG Then
                Call $"Protocol:={request.Protocol} was not found!".__DEBUG_ECHO
#End If
                ' 没有找到相对应的协议处理逻辑，则没有实现相对应的数据协议处理方法
                Return New DataPipe(NetResponse.RFC_NOT_IMPLEMENTED)
            End If

            Dim process As DataRequestHandler = Me.Protocols(request.Protocol)
            Dim value As BufferPipe = process(request, remoteDevcie)

            Return value
        End Function

        Private Shared Function GetMethod(obj As Object, entryPoint As MethodInfo, Optional debug As Boolean = False) As DataRequestHandler
            Dim parameters As ParameterInfo() = entryPoint.GetParameters

            If Not entryPoint.ReturnType.IsInheritsFrom(GetType(BufferPipe), strict:=False) Then
                Return Nothing
            ElseIf parameters.Length > 2 Then
                Return Nothing
            End If

            If parameters.Length = 0 Then
                Return AddressOf New ProtocolInvoker(obj, entryPoint, debug).InvokeProtocol0
            ElseIf parameters.Length = 1 Then
                Return method1(obj, entryPoint, parameters, debug)
            ElseIf parameters.Length = 2 Then
                Return method2(obj, entryPoint, parameters, debug)
            End If

            Return Nothing
        End Function

        Private Shared Function method2(obj As Object, entryPoint As MethodInfo, parameters As ParameterInfo(), debug As Boolean) As DataRequestHandler
            If parameters.First.ParameterType IsNot GetType(RequestStream) OrElse
                parameters.Last.ParameterType IsNot GetType(TcpEndPoint) Then
                Return Nothing
            Else
                Return AddressOf New ProtocolInvoker(obj, entryPoint, debug).InvokeProtocol2
            End If
        End Function

        Private Shared Function method1(obj As Object, entryPoint As MethodInfo, parameters As ParameterInfo(), debug As Boolean) As DataRequestHandler
            If parameters.First.ParameterType IsNot GetType(RequestStream) Then
                Return Nothing
            Else
                Return AddressOf New ProtocolInvoker(obj, entryPoint, debug).InvokeProtocol1
            End If
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Narrowing Operator CType(handler As ProtocolHandler) As DataRequestHandler
            Return AddressOf handler.HandleRequest
        End Operator
    End Class
End Namespace
