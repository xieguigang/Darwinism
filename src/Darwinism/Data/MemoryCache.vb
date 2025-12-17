Imports Darwinism.Repository.BucketDb
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime.Interop

<Package("memory_cache")>
<RTypeExport("memory_db", GetType(Buckets))>
Public Module MemoryCache

    <ExportAPI("new")>
    Public Function newDb(dir As String) As Buckets
        Return New Buckets(dir)
    End Function

End Module
