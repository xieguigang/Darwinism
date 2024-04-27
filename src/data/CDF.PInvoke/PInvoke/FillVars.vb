#Region "Microsoft.VisualBasic::25980b66ca7f52b03f055893b4c20314, G:/GCModeller/src/runtime/Darwinism/src/data/CDF.PInvoke//PInvoke/FillVars.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.



    ' /********************************************************************************/

    ' Summaries:


    ' Code Statistics:

    '   Total Lines: 19
    '    Code Lines: 14
    ' Comment Lines: 5
    '   Blank Lines: 0
    '     File Size: 1.27 KB


    ' Class FillVars
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

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
