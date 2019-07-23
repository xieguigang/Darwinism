#Region "Microsoft.VisualBasic::a5c8a6ad26052cb2bd9928f1eb09ff41, ComputingServices\Taskhost.d\Invoke\TaskInvoke.vb"

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

    '     Class TaskInvoke
    ' 
    '         Properties: FileSystem, LinqProvider, Portal
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    '         Function: __invoke, Free, GetNodeLoad, Handshake, (+2 Overloads) Invoke
    '                   InvokeLinq, LinqSelect, Run, TryInvoke
    ' 
    '         Sub: (+2 Overloads) Dispose
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Reflection
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Http
Imports Microsoft.VisualBasic.Net.Protocols
Imports Microsoft.VisualBasic.Net.Protocols.Reflection
Imports Microsoft.VisualBasic.Net.Tcp
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Microsoft.VisualBasic.Win32

Namespace TaskHost

    ''' <summary>
    ''' Running on the server cluster nodes.
    ''' </summary>
    <Protocol(GetType(TaskProtocols))> Public Class TaskInvoke
        Implements IDisposable

        Dim host As TcpServicesSocket

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="port">You can suing function <see cref="GetFirstAvailablePort"/> to initialize this server object.</param>
        Sub New(Optional port As Integer = 1234)
            host = New TcpServicesSocket(port)
            host.ResponseHandler = AddressOf New ProtocolHandler(Me).HandleRequest
        End Sub

        Public Function Run() As Integer
            Return host.Run()
        End Function

        Public ReadOnly Property LinqProvider As LinqPool = New LinqPool

        <Protocol(TaskProtocols.Invoke)>
        Private Function Invoke(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream
            Dim params As InvokeInfo = JsonContract.LoadJSON(Of InvokeInfo)(args.GetUTF8String)
            Dim value As Rtvl = RemoteCall.Invoke(params)
            Return New RequestStream(value.GetJson)
        End Function

        <Protocol(TaskProtocols.Free)>
        Private Function Free(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream
            Dim uid As String = args.GetUTF8String
            Call LinqProvider.Free(uid)
            Return NetResponse.RFC_OK  ' HTTP/200
        End Function

        ''' <summary>
        ''' 执行远程Linq代码
        ''' </summary>
        ''' <param name="CA">SSL证书编号</param>
        ''' <param name="args"></param>
        ''' <param name="remote"></param>
        ''' <returns></returns>
        <Protocol(TaskProtocols.InvokeLinq)>
        Private Function InvokeLinq(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream
            Dim params As InvokeInfo = JsonContract.LoadJSON(Of InvokeInfo)(args.GetUTF8String) ' 得到远程函数指针信息
            Dim value As Object = RemoteCall.doCall(params)
            Dim source As IEnumerable = DirectCast(value, IEnumerable)
            Dim svr As String = LinqProvider.OpenQuery(source, value.GetType).GetJson   ' 返回数据源信息
            Return New RequestStream(svr)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="CA"></param>
        ''' <param name="args">``{source, args}``</param>
        ''' <param name="remote"></param>
        ''' <returns></returns>
        <Protocol(TaskProtocols.Select)>
        Private Function LinqSelect(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream
            Dim params As InvokeInfo = JsonContract.LoadJSON(Of InvokeInfo)(args.GetUTF8String) ' 得到远程函数指针信息
            Dim func As MethodInfo = params.GetMethod
            Dim paramsValue As Object() = params.parameters.Select(Function(arg) arg.GetValue).ToArray
            Dim source As IEnumerable = DirectCast(paramsValue(Scan0), IEnumerable)
            Dim type As Type = func.ReturnType

            source = From x As Object
                     In source.AsParallel
                     Let inputs As Object() =
                         {x}.Join(paramsValue.Skip(1)) _
                            .ToArray
                     Select func.Invoke(Nothing, inputs)

            Dim svr As String = LinqProvider _
                .OpenQuery(source, type) _
                .GetJson
            Return New RequestStream(svr) ' 返回数据源信息
        End Function

        ''' <summary>
        ''' This node is alive
        ''' </summary>
        ''' <param name="CA"></param>
        ''' <param name="args"></param>
        ''' <param name="remote"></param>
        ''' <returns></returns>
        <Protocol(TaskProtocols.Handshake)>
        Private Function Handshake(CA&, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream
            Return NetResponse.RFC_OK ' HTTP/200
        End Function

        <Protocol(TaskProtocols.NodeLoad)>
        Private Function GetNodeLoad(CA&, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream
            Return RequestStream.CreatePackage(TaskManager.ProcessUsage)
        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                    Call _LinqProvider.Free
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            ' TODO: uncomment the following line if Finalize() is overridden above.
            ' GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class
End Namespace
