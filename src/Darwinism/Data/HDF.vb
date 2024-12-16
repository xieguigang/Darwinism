
Imports HDF.PInvoke
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData

<Package("HDF")>
Module HDF

    <ExportAPI("open")>
    Public Function open(file As String) As Long
        Dim fileId As Long = H5F.open(file, H5F.ACC_RDONLY)
        Return fileId
    End Function



End Module
