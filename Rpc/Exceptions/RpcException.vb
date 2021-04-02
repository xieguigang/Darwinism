#Region "Microsoft.VisualBasic::7c85e6230709bf64ec1bf2777d221f43, Rpc\Exceptions\RpcException.vb"

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

    '     Class RpcException
    ' 
    '         Constructor: (+3 Overloads) Sub New
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System

Namespace Rpc
    ''' <summary>
    ''' Error associated with work on RPC protocol
    ''' </summary>
    Public Class RpcException
        Inherits SystemException
        ''' <summary>
        ''' Error associated with work on RPC protocol
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Error associated with work on RPC protocol
        ''' </summary>
        ''' <paramname="message"></param>
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        ''' <summary>
        ''' Error associated with work on RPC protocol
        ''' </summary>
        ''' <paramname="message"></param>
        ''' <paramname="innerEx"></param>
        Public Sub New(ByVal message As String, ByVal innerEx As Exception)
            MyBase.New(message, innerEx)
        End Sub
    End Class
End Namespace

