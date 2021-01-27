#Region "Microsoft.VisualBasic::bfd2bf7e327f6cfa9b21f60d2c670e4f, Docker\Arguments\Mount.vb"

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

    '     Class Mount
    ' 
    '         Properties: IsValid, local, virtual
    ' 
    '         Function: ToString
    ' 
    '     Class PortForward
    ' 
    '         Properties: local, virtual
    ' 
    '         Function: ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Net

Namespace Arguments

    ''' <summary>
    ''' 共享文件夹
    ''' </summary>
    Public Class Mount

        ''' <summary>
        ''' 宿主机内的本地文件夹全路径
        ''' </summary>
        Public Property local As String
        ''' <summary>
        ''' 虚拟机内的文件路径,共享文件夹将会在虚拟机内被挂载到这个路径上面
        ''' </summary>
        Public Property virtual As String

        ''' <summary>
        ''' 任何一个路径是空字符串就会被判断为无效参数, 对于<see cref="local"/>本地文件夹路径
        ''' 还需要其存在于本地文件系统之上
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property IsValid As Boolean
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return (local.FileExists OrElse local.DirectoryExists) AndAlso Not virtual.StringEmpty
            End Get
        End Property

        ''' <summary>
        ''' ``local:virtual``
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            Return $"{local.GetDirectoryFullPath}:{virtual}"
        End Function
    End Class

    ' -p 127.0.0.1:80:8080/tcp
    Public Class PortForward

        Public Property virtual As Integer
        Public Property local As IPEndPoint

        Public Overrides Function ToString() As String
            Return $"{local.IPAddress}:{local.Port}:{virtual}/tcp"
        End Function

    End Class

End Namespace
