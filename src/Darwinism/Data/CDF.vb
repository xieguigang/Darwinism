Imports CDF.PInvoke
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.DataStorage.netCDF.Data
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
    Public Function open(file As String) As Integer
        Dim intptr As Integer
        NetCDF.nc_open(file, OpenMode.NC_NOWRITE, intptr)
        Return intptr
    End Function

    <ExportAPI("dim_len")>
    Public Function dimLen(nc As Integer, name As String) As Integer
        Dim dimid As Integer = 0
        Dim dimSize As Integer
        NetCDF.nc_inq_dimid(nc, name, dimid)
        NetCDF.nc_inq_dimlen(nc, dimid, dimSize)
        Return dimSize
    End Function

    ''' <summary>
    ''' get variable data from given file
    ''' </summary>
    ''' <param name="nc"></param>
    ''' <param name="name"></param>
    ''' <returns></returns>
    <ExportAPI("var_data")>
    Public Function getData(nc As Integer, name As String) As Object
        Dim var_id As Integer
        Dim dimid As Integer
        Dim attrs As Integer
        Dim type As CDFDataTypes
        NetCDF.nc_inq_varid(nc, name, var_id)
        NetCDF.nc_inq_varndims(nc, var_id, dimid)
        NetCDF.nc_inq_varnatts(nc, var_id, attrs)
        NetCDF.nc_inq_vartype(nc, var_id, type)

    End Function
End Module
