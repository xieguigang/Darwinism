#Region "Microsoft.VisualBasic::e3ecf2143a05f268f224d1006073aa3f, CloudKit\ossutil\Aspera.vb"

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

    ' Class Aspera
    ' 
    '     Properties: AdaptiveRate, Encryption
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: Download, getCLI, IsAsperaProtocol, Speed, Upload
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports System.Text
Imports Microsoft.VisualBasic.CommandLine.InteropService

''' <summary>
''' aspera cli 
''' </summary>
Public Class Aspera : Inherits InteropService

    Dim maxSpeed$
    Dim minSpeed$

    ReadOnly server$

    Public Property AdaptiveRate As Boolean
    Public Property Encryption As Boolean

    ''' <summary>
    ''' ``&lt;username>@server_name``
    ''' </summary>
    ''' <param name="server">``&lt;username>@server_name``</param>
    ''' <param name="bin">可执行文件路径</param>
    Sub New(server$, Optional bin$ = "ascp")
        Call MyBase.New(bin)
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Function IsAsperaProtocol(url As String) As Boolean
        Return InStr(url, "fasp://", CompareMethod.Text) = 1
    End Function

    ''' <summary>
    ''' 单位都是MB
    ''' </summary>
    ''' <param name="min%"></param>
    ''' <param name="max%"></param>
    ''' <returns></returns>
    Public Function Speed(Optional min% = -1, Optional max% = -1) As Aspera
        If min > 0 Then
            minSpeed = min & "M"
        Else
            minSpeed = Nothing
        End If
        If max > 0 Then
            maxSpeed = max & "M"
        Else
            maxSpeed = Nothing
        End If

        Return Me
    End Function

    ''' <summary>
    ''' CLI for file transfer from ``a => b``
    ''' </summary>
    ''' <param name="a$"></param>
    ''' <param name="b$"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' + l - Allows for the user to set a maximum download speed that aspera should attempt to stay 
    '''       at or below for the duration of the transfer. A speed in Megabits must be provided 
    '''       with this flag.
    ''' + m - Allows for the user to set a minimum download sped that aspera should attempt to stay 
    '''       at Or above for the duration of the transfer. A speed in Megabits must be provided with 
    '''       this flag.
    ''' + Q - Turns adaptive rate on. Adaptive rate controls the speed of aspera with a goal of Not 
    '''       dominating the bandwidth available. Very useful on busy networks that may have other 
    '''       transfers ongoing.
    ''' + T - Turns encryption off. Turning encryption off will allow for a maximum throughput transfer 
    '''       but should Not be provided if data being uploaded Is sensitive.
    ''' </remarks>
    Private Function getCLI(a$, b$) As String
        Dim cli As New StringBuilder

        If Not maxSpeed.StringEmpty Then
            Call cli.AppendLine($"-l {maxSpeed}")
        End If
        If Not minSpeed.StringEmpty Then
            Call cli.AppendLine($"-m {minSpeed}")
        End If
        If AdaptiveRate Then
            Call cli.AppendLine("-Q")
        End If
        If Not Encryption Then
            Call cli.AppendLine("-T")
        End If

        Return cli.ToString
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="local">是本地的一个文件</param>
    ''' <param name="remote">是服务器上面的一个文件夹</param>
    ''' <returns></returns>
    Public Function Upload(local$, remote$) As String
        Return MyBase.RunProgram(getCLI(local, server & remote)).Run
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="remote">应该是服务器上面的一个文件或者文件夹</param>
    ''' <param name="local">应该是一个本地文件夹，不存在的话会自动创建</param>
    ''' <returns></returns>
    Public Function Download(remote$, local$) As String
        Return MyBase.RunProgram(getCLI(server & remote, local)).Run
    End Function
End Class
