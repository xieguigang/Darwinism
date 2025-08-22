
Imports Darwinism.OSSUtil
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData

<Package("ossutil")>
Module FileSystemTool

    <ExportAPI("aspera")>
    Public Function Aspera(server As String, Optional bin As String = "ascp") As Aspera
        Return New Aspera(server, bin)
    End Function

End Module
