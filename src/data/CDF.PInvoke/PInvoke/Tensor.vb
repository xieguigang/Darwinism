Imports System.Runtime.InteropServices

''' <summary>
''' Multi-dimensional array support
''' </summary>
Module Tensor

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
