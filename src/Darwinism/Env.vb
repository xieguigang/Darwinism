
Imports Darwinism.DataScience.DataMining
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData

''' <summary>
''' the IPC parallel environment
''' </summary>
<Package("Environment")>
Module Env

    ''' <summary>
    ''' set the parallel batch threads
    ''' </summary>
    ''' <param name="n_threads"></param>
    ''' <returns></returns>
    <ExportAPI("set_threads")>
    Public Function Set_threads(n_threads As Integer) As Object
        Call VectorMath.SetThreads(n_threads)
        Return Nothing
    End Function

End Module
