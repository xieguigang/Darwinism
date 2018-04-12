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

    Public Function GetBucketStorageDeviceList()

    End Function
End Class
