Imports CDF.PInvoke
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData

<Package("CDF")>
Module CDF

    Sub New()
        Call VBDebugger.EchoLine("the netcdf module only works for windows platform currently!")
    End Sub

    ''' <summary>
    ''' open a connection to the netcdf file
    ''' </summary>
    ''' <param name="file"></param>
    ''' <returns>
    ''' the file stream id
    ''' </returns>
    <ExportAPI("open")>
    Public Function open(file As String) As DataReader
        Return New DataReader(file)
    End Function

    ''' <summary>
    ''' get variable data from given file
    ''' </summary>
    ''' <param name="nc"></param>
    ''' <param name="name"></param>
    ''' <returns></returns>
    <ExportAPI("var_data")>
    Public Function getData(nc As DataReader, name As String) As Object
        Return nc.GetData(name)
    End Function
End Module
