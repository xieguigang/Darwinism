#Region "Microsoft.VisualBasic::2a61ed4f3c47c31936614827970aa61e, src\data\CDF.PInvoke\PInvoke\Tensor.vb"

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

    '   Total Lines: 179
    '    Code Lines: 126
    ' Comment Lines: 5
    '   Blank Lines: 48
    '     File Size: 8.99 KB


    ' Module NetCDF
    ' 
    '     Function: (+2 Overloads) nc_get_var_double, (+2 Overloads) nc_get_var_float, (+2 Overloads) nc_get_var_int, (+2 Overloads) nc_get_var_long, (+2 Overloads) nc_get_var_short
    '               nc_get_var_ubyte, (+2 Overloads) nc_get_vara_double, (+2 Overloads) nc_get_vara_float, (+2 Overloads) nc_get_vara_int, (+2 Overloads) nc_get_vara_long
    '               (+2 Overloads) nc_get_vara_short, (+2 Overloads) nc_put_var_double, (+2 Overloads) nc_put_var_float, (+2 Overloads) nc_put_var_int, (+2 Overloads) nc_put_var_long
    '               (+2 Overloads) nc_put_var_short, (+2 Overloads) nc_put_vara_double, (+2 Overloads) nc_put_vara_float, (+2 Overloads) nc_put_vara_int, (+2 Overloads) nc_put_vara_long
    '               (+2 Overloads) nc_put_vara_short
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.InteropServices

''' <summary>
''' Multi-dimensional array support
''' </summary>
Partial Module NetCDF

    ' Get methods
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var_ubyte(ncid As Integer, varid As Integer, ip As Byte(,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var_short(ncid As Integer, varid As Integer, ip As Short(,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var_int(ncid As Integer, varid As Integer, ip As Integer(,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var_long(ncid As Integer, varid As Integer, ip As Long(,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var_float(ncid As Integer, varid As Integer, ip As Single(,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var_double(ncid As Integer, varid As Integer, ip As Double(,)) As Integer
    End Function


    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var_short(ncid As Integer, varid As Integer, ip As Short(,,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var_int(ncid As Integer, varid As Integer, ip As Integer(,,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var_long(ncid As Integer, varid As Integer, ip As Long(,,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var_float(ncid As Integer, varid As Integer, ip As Single(,,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var_double(ncid As Integer, varid As Integer, ip As Double(,,)) As Integer
    End Function


    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vara_short(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), ip As Short(,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vara_int(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), ip As Integer(,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vara_long(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), ip As Long(,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vara_float(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), ip As Single(,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vara_double(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), ip As Double(,)) As Integer
    End Function


    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vara_short(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), ip As Short(,,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vara_int(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), ip As Integer(,,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vara_long(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), ip As Long(,,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vara_float(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), ip As Single(,,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vara_double(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), ip As Double(,,)) As Integer
    End Function


    ' Put methods
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var_short(ncid As Integer, varid As Integer, op As Short(,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var_int(ncid As Integer, varid As Integer, op As Integer(,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var_long(ncid As Integer, varid As Integer, op As Long(,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var_float(ncid As Integer, varid As Integer, op As Single(,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var_double(ncid As Integer, varid As Integer, op As Double(,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var_short(ncid As Integer, varid As Integer, op As Short(,,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var_int(ncid As Integer, varid As Integer, op As Integer(,,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var_long(ncid As Integer, varid As Integer, op As Long(,,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var_float(ncid As Integer, varid As Integer, op As Single(,,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var_double(ncid As Integer, varid As Integer, op As Double(,,)) As Integer
    End Function


    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vara_short(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), op As Short(,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vara_int(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), op As Integer(,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vara_long(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), op As Long(,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vara_float(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), op As Single(,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vara_double(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), op As Double(,)) As Integer
    End Function


    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vara_short(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), op As Short(,,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vara_int(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), op As Integer(,,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vara_long(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), op As Long(,,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vara_float(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), op As Single(,,)) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vara_double(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), op As Double(,,)) As Integer
    End Function
End Module
