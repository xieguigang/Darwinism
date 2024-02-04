''' <summary>
''' Fill value arrays for use in the corresponding nc_put_att function e.g.
''' NetCDF.nc_put_att_float(ncid, DataVarid, "_FillValue", NetCDF.nc_type.NC_FLOAT, 1, NetCDF.FillVars.FILL_FLOAT);
''' To save having to define the array each time
''' </summary>
Public NotInheritable Class FillVars
    Public Shared ReadOnly FILL_BYTE As SByte() = {FillValues.NC_FILL_BYTE}
    Public Shared ReadOnly FILL_CHAR As Char() = {FillValues.NC_FILL_CHAR}
    Public Shared ReadOnly FILL_SHORT As Short() = {FillValues.NC_FILL_SHORT}
    Public Shared ReadOnly FILL_INT As Integer() = {FillValues.NC_FILL_INT}
    Public Shared ReadOnly FILL_FLOAT As Single() = {FillValues.NC_FILL_FLOAT}
    Public Shared ReadOnly FILL_DOUBLE As Double() = {FillValues.NC_FILL_DOUBLE}
    Public Shared ReadOnly FILL_UBYTE As Byte() = {FillValues.NC_FILL_UBYTE}
    Public Shared ReadOnly FILL_USHORT As UShort() = {FillValues.NC_FILL_USHORT}
    Public Shared ReadOnly FILL_UINT As UInteger() = {FillValues.NC_FILL_UINT}
    Public Shared ReadOnly FILL_INT64 As Long() = {FillValues.NC_FILL_INT64}
    Public Shared ReadOnly FILL_UINT64 As ULong() = {FillValues.NC_FILL_UINT64}
    Public Shared ReadOnly FILL_STRING As String() = {FillValues.NC_FILL_STRING}
End Class