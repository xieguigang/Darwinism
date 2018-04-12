Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.InteropService

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
        Return RunProgram("ls").ShellExec.ParseBuckets
    End Function

    ''' <summary>
    ''' 无论如何这个命令都会返回文件夹之中的所有递归的内容的，不仅仅限于当前的文件夹
    ''' </summary>
    ''' <param name="uri"></param>
    ''' <returns></returns>
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function ListObjects(uri As String) As IEnumerable(Of [Object])
        Return RunProgram($"ls {uri}").ShellExec.ParseObjects
    End Function

    Public Sub Copy(from$, to$)
        Call RunProgram($"cp {from.CLIToken} {[to].CLIToken}").Run()
    End Sub
End Class
