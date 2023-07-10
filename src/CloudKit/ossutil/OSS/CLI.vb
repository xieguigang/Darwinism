#Region "Microsoft.VisualBasic::8da48e3a99777ec1990b88a3bb42d879, CloudKit\ossutil\OSS\CLI.vb"

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

    ' Class CLI
    ' 
    '     Constructor: (+1 Overloads) Sub New
    ' 
    '     Function: GetBucketStorageDeviceList, ListObjects
    ' 
    '     Sub: Copy
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.InteropService
Imports ThinkVB.FileSystem.OSS.Model

''' <summary>
''' ``ossutil`` CLI driver
''' </summary>
Public Class CLI : Inherits InteropService

    ReadOnly config$

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="bin$">
    ''' The file path for one of the cli program
    ''' 
    ''' + ossutil32.exe
    ''' + ossutil64.exe
    ''' 
    ''' > https://github.com/aliyun/ossutil
    ''' </param>
    ''' <param name="config$"></param>
    Sub New(bin$, Optional config$ = Nothing)
        Call MyBase.New(app:=bin)

        Me.config = config
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function GetBucketStorageDeviceList() As IEnumerable(Of Bucket)
        Return RunProgram("ls", stdin:="y").ShellExec.ParseBuckets
    End Function

    ''' <summary>
    ''' 无论如何这个命令都会返回文件夹之中的所有递归的内容的，不仅仅限于当前的文件夹
    ''' </summary>
    ''' <param name="uri"></param>
    ''' <returns></returns>
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function ListObjects(uri As String) As IEnumerable(Of [Object])
        Return RunProgram($"ls {uri}", stdin:="y").ShellExec.ParseObjects
    End Function

    ''' <summary>
    ''' File copy operation between the OSS and local file system. FileUpload and FileDownload
    ''' </summary>
    ''' <param name="from$"></param>
    ''' <param name="to$"></param>
    Public Sub Copy(from$, to$)
        Dim stdout$ = RunProgram($"cp {from.CLIToken} {[to].CLIToken}", stdin:="y").ShellExec

        ' FinishWithError: Scanned 1 objects. Error num:  1. OK num: 0, Transfer size: 0.
        ' Error: oss: service returned without a response body (404 Not Found), Bucket=bionovogene-xcms, Object=mz.biodeep.cn/data/upload/rawfiles/225/181/1761/T201710170947282738.mzXML!
        If InStr(stdout, "FinishWithError:") > 0 Then
#Disable Warning
            Throw New ExecutionEngineException(stdout)
#Enable Warning
        End If
    End Sub
End Class
