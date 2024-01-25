
Imports Darwinism.DataScience.DataMining
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime.Interop

''' <summary>
''' the IPC parallel environment
''' </summary>
<Package("Environment")>
<RTypeExport("darwinism_argument", GetType(batch.Argument))>
Module Env

    ''' <summary>
    ''' set the parallel batch threads
    ''' </summary>
    ''' <param name="n_threads"></param>
    ''' <returns></returns>
    <ExportAPI("set_threads")>
    Public Function Set_threads(n_threads As Integer) As batch.Argument
        Call VectorMath.SetThreads(n_threads)
        Return VectorMath.GetEnvironmentArguments
    End Function

    <ExportAPI("set_libpath")>
    Public Function Set_libpath(libpath As String) As batch.Argument
        Call VectorMath.SetLibPath(libpath)
        Return VectorMath.GetEnvironmentArguments
    End Function

End Module
