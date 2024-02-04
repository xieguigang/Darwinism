' 
'   Use this file, rather than CsNetCDF.cs, for customised methods, such as multidimensional array
'   support, or other helper methods.
' 
Imports System.Runtime.InteropServices
Imports System.Text
Imports Microsoft.VisualBasic.Math
Imports nc_type = Microsoft.VisualBasic.DataStorage.netCDF.Data.CDFDataTypes

Partial Public Module NetCDF
#Region "C# Friendly methods"
#Region "Wrapping the IntPtr calls"
#Region "Misc and attribute methods"
    Public Function nc_inq_path(ncid As Integer, <Out> ByRef pathlen As Integer, path As StringBuilder) As Integer
        Dim len As IntPtr = Nothing
        Dim status = nc_inq_path(ncid, len, path)
        pathlen = CInt(len)
        Return status
    End Function
    Public Function nc_inq_att(ncid As Integer, varid As Integer, name As String, <Out> ByRef xtypep As nc_type, <Out> ByRef len As Integer) As Integer
        Dim lenp As IntPtr = Nothing
        Dim status = nc_inq_att(ncid, varid, name, xtypep, lenp)
        len = CInt(lenp)
        Return status
    End Function
    Public Function nc_inq_attlen(ncid As Integer, varid As Integer, name As String, <Out> ByRef len As Integer) As Integer
        Dim lenp As IntPtr = Nothing
        Dim status = nc_inq_attlen(ncid, varid, name, lenp)
        len = CInt(lenp)
        Return status
    End Function
    Public Function nc_put_att_text(ncid As Integer, varid As Integer, name As String, len As Integer, value As String) As Integer
        Return nc_put_att_text(ncid, varid, name, CType(len, IntPtr), value)
    End Function
    Public Function nc_put_att_schar(ncid As Integer, varid As Integer, name As String, type As nc_type, len As Integer, value As SByte()) As Integer
        Return nc_put_att_schar(ncid, varid, name, type, CType(len, IntPtr), value)
    End Function
    Public Function nc_put_att_uchar(ncid As Integer, varid As Integer, name As String, type As nc_type, len As Integer, value As Byte()) As Integer
        Return nc_put_att_uchar(ncid, varid, name, type, CType(len, IntPtr), value)
    End Function
    Public Function nc_put_att_short(ncid As Integer, varid As Integer, name As String, type As nc_type, len As Integer, value As Short()) As Integer
        Return nc_put_att_short(ncid, varid, name, type, CType(len, IntPtr), value)
    End Function
    Public Function nc_put_att_int(ncid As Integer, varid As Integer, name As String, type As nc_type, len As Integer, value As Integer()) As Integer
        Return nc_put_att_int(ncid, varid, name, type, CType(len, IntPtr), value)
    End Function
    Public Function nc_put_att_long(ncid As Integer, varid As Integer, name As String, type As nc_type, len As Integer, value As Long()) As Integer
        Return nc_put_att_long(ncid, varid, name, type, CType(len, IntPtr), value)
    End Function
    Public Function nc_put_att_float(ncid As Integer, varid As Integer, name As String, type As nc_type, len As Integer, value As Single()) As Integer
        Return nc_put_att_float(ncid, varid, name, type, CType(len, IntPtr), value)
    End Function
    Public Function nc_put_att_double(ncid As Integer, varid As Integer, name As String, type As nc_type, len As Integer, value As Double()) As Integer
        Return nc_put_att_double(ncid, varid, name, type, CType(len, IntPtr), value)
    End Function
    Public Function nc_put_att_ubyte(ncid As Integer, varid As Integer, name As String, type As nc_type, len As Integer, value As Byte()) As Integer
        Return nc_put_att_ubyte(ncid, varid, name, type, CType(len, IntPtr), value)
    End Function
    Public Function nc_put_att_ushort(ncid As Integer, varid As Integer, name As String, type As nc_type, len As Integer, value As UShort()) As Integer
        Return nc_put_att_ushort(ncid, varid, name, type, CType(len, IntPtr), value)
    End Function
    Public Function nc_put_att_uint(ncid As Integer, varid As Integer, name As String, type As nc_type, len As Integer, value As UInteger()) As Integer
        Return nc_put_att_uint(ncid, varid, name, type, CType(len, IntPtr), value)
    End Function
    Public Function nc_put_att_ulonglong(ncid As Integer, varid As Integer, name As String, type As nc_type, len As Integer, value As ULong()) As Integer
        Return nc_put_att_ulonglong(ncid, varid, name, type, CType(len, IntPtr), value)
    End Function
    Public Function nc_inq_grpname_full(ncid As Integer, <Out> ByRef len As Integer, full_name As StringBuilder) As Integer
        Dim lenp As IntPtr = Nothing
        Dim status = nc_inq_grpname_full(ncid, lenp, full_name)
        len = CInt(lenp)
        Return status
    End Function
    Public Function nc_inq_grpname_len(ncid As Integer, <Out> ByRef len As Integer) As Integer
        Dim lenp As IntPtr = Nothing
        Dim status = nc_inq_grpname_len(ncid, lenp)
        len = CInt(lenp)
        Return status
    End Function
    Public Function nc_get_vlen_element(ncid As Integer, typeid1 As Integer, vlen_element As Object, <Out> ByRef len As Integer, <Out> ByRef data As Object) As Integer
        Dim lenp As IntPtr = Nothing
        Dim status = nc_get_vlen_element(ncid, typeid1, vlen_element, lenp, data)
        len = CInt(lenp)
        Return status
    End Function
    Public Function nc_def_dim(ncid As Integer, name As String, len As Integer, <Out> ByRef dimidp As Integer) As Integer
        Return nc_def_dim(ncid, name, CType(len, IntPtr), dimidp)
    End Function
    Public Function nc_inq_dim(ncid As Integer, dimid As Integer, name As StringBuilder, <Out> ByRef len As Integer) As Integer
        Dim lenp As IntPtr = Nothing
        Dim status = nc_inq_dim(ncid, dimid, name, lenp)
        len = CInt(lenp)
        Return status
    End Function
    Public Function nc_inq_dimlen(ncid As Integer, dimid As Integer, <Out> ByRef len As Integer) As Integer
        Dim lenp As IntPtr = Nothing
        Dim status = nc_inq_dimlen(ncid, dimid, lenp)
        len = CInt(lenp)
        Return status
    End Function
#End Region

    ' Wrap all the var1, vara and vars methods so that the calling program can pass the index and count arrays as ints
#Region "get_var1"
    Public Function nc_get_var1_text(ncid As Integer, varid As Integer, index As Integer(), <Out> ByRef ip As Byte) As Integer
        Return nc_get_var1_text(ncid, varid, ConvertToIntPtr(index), ip)
    End Function
    Public Function nc_get_var1_schar(ncid As Integer, varid As Integer, index As Integer(), <Out> ByRef ip As SByte) As Integer
        Return nc_get_var1_schar(ncid, varid, ConvertToIntPtr(index), ip)
    End Function
    Public Function nc_get_var1_uchar(ncid As Integer, varid As Integer, index As Integer(), <Out> ByRef ip As Byte) As Integer
        Return nc_get_var1_uchar(ncid, varid, ConvertToIntPtr(index), ip)
    End Function
    Public Function nc_get_var1_short(ncid As Integer, varid As Integer, index As Integer(), <Out> ByRef ip As Short) As Integer
        Return nc_get_var1_short(ncid, varid, ConvertToIntPtr(index), ip)
    End Function
    Public Function nc_get_var1_int(ncid As Integer, varid As Integer, index As Integer(), <Out> ByRef ip As Integer) As Integer
        Return nc_get_var1_int(ncid, varid, ConvertToIntPtr(index), ip)
    End Function
    Public Function nc_get_var1_long(ncid As Integer, varid As Integer, index As Integer(), <Out> ByRef ip As Long) As Integer
        Return nc_get_var1_long(ncid, varid, ConvertToIntPtr(index), ip)
    End Function
    Public Function nc_get_var1_float(ncid As Integer, varid As Integer, index As Integer(), <Out> ByRef ip As Single) As Integer
        Return nc_get_var1_float(ncid, varid, ConvertToIntPtr(index), ip)
    End Function
    Public Function nc_get_var1_double(ncid As Integer, varid As Integer, index As Integer(), <Out> ByRef ip As Double) As Integer
        Return nc_get_var1_double(ncid, varid, ConvertToIntPtr(index), ip)
    End Function
    Public Function nc_get_var1_ubyte(ncid As Integer, varid As Integer, index As Integer(), <Out> ByRef ip As Byte) As Integer
        Return nc_get_var1_ubyte(ncid, varid, ConvertToIntPtr(index), ip)
    End Function
    Public Function nc_get_var1_ushort(ncid As Integer, varid As Integer, index As Integer(), <Out> ByRef ip As UShort) As Integer
        Return nc_get_var1_ushort(ncid, varid, ConvertToIntPtr(index), ip)
    End Function
    Public Function nc_get_var1_uint(ncid As Integer, varid As Integer, index As Integer(), <Out> ByRef ip As UInteger) As Integer
        Return nc_get_var1_uint(ncid, varid, ConvertToIntPtr(index), ip)
    End Function
    Public Function nc_get_var1_longlong(ncid As Integer, varid As Integer, index As Integer(), <Out> ByRef ip As Long) As Integer
        Return nc_get_var1_longlong(ncid, varid, ConvertToIntPtr(index), ip)
    End Function
    Public Function nc_get_var1_ulonglong(ncid As Integer, varid As Integer, index As Integer(), <Out> ByRef ip As ULong) As Integer
        Return nc_get_var1_ulonglong(ncid, varid, ConvertToIntPtr(index), ip)
    End Function
    Public Function nc_get_var1_string(ncid As Integer, varid As Integer, index As Integer(), ip As IntPtr()) As Integer
        Return nc_get_var1_string(ncid, varid, ConvertToIntPtr(index), ip)
    End Function
#End Region

#Region "get_vara"
    Public Function nc_get_vara_text(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), ip As Byte()) As Integer
        Return nc_get_vara_text(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip)
    End Function
    Public Function nc_get_vara_schar(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), ip As SByte()) As Integer
        Return nc_get_vara_schar(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip)
    End Function
    Public Function nc_get_vara_uchar(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), ip As Byte()) As Integer
        Return nc_get_vara_uchar(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip)
    End Function
    Public Function nc_get_vara_short(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), ip As Short()) As Integer
        Return nc_get_vara_short(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip)
    End Function
    Public Function nc_get_vara_int(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), ip As Integer()) As Integer
        Return nc_get_vara_int(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip)
    End Function
    Public Function nc_get_vara_long(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), ip As Long()) As Integer
        Return nc_get_vara_long(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip)
    End Function
    Public Function nc_get_vara_float(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), ip As Single()) As Integer
        Return nc_get_vara_float(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip)
    End Function
    Public Function nc_get_vara_double(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), ip As Double()) As Integer
        Return nc_get_vara_double(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip)
    End Function
    Public Function nc_get_vara_ubyte(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), ip As Byte()) As Integer
        Return nc_get_vara_ubyte(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip)
    End Function
    Public Function nc_get_vara_ushort(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), ip As UShort()) As Integer
        Return nc_get_vara_ushort(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip)
    End Function
    Public Function nc_get_vara_uint(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), ip As UInteger()) As Integer
        Return nc_get_vara_uint(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip)
    End Function
    Public Function nc_get_vara_longlong(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), ip As Long()) As Integer
        Return nc_get_vara_longlong(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip)
    End Function
    Public Function nc_get_vara_ulonglong(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), ip As ULong()) As Integer
        Return nc_get_vara_ulonglong(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip)
    End Function
    Public Function nc_get_vara_string(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), ip As IntPtr()) As Integer
        Return nc_get_vara_string(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip)
    End Function

    ' Multidimensional array support
    Public Function nc_get_vara_short(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), ip As Short(,)) As Integer
        Return nc_get_vara_short(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip)
    End Function
    Public Function nc_get_vara_int(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), ip As Integer(,)) As Integer
        Return nc_get_vara_int(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip)
    End Function
    Public Function nc_get_vara_long(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), ip As Long(,)) As Integer
        Return nc_get_vara_long(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip)
    End Function
    Public Function nc_get_vara_float(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), ip As Single(,)) As Integer
        Return nc_get_vara_float(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip)
    End Function
    Public Function nc_get_vara_double(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), ip As Double(,)) As Integer
        Return nc_get_vara_double(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip)
    End Function

    Public Function nc_get_vara_short(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), ip As Short(,,)) As Integer
        Return nc_get_vara_short(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip)
    End Function
    Public Function nc_get_vara_int(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), ip As Integer(,,)) As Integer
        Return nc_get_vara_int(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip)
    End Function
    Public Function nc_get_vara_long(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), ip As Long(,,)) As Integer
        Return nc_get_vara_long(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip)
    End Function
    Public Function nc_get_vara_float(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), ip As Single(,,)) As Integer
        Return nc_get_vara_float(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip)
    End Function
    Public Function nc_get_vara_double(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), ip As Double(,,)) As Integer
        Return nc_get_vara_double(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), ip)
    End Function

#End Region

#Region "get_vars"
    Public Function nc_get_vars_text(ncid As Integer, varid As Integer, startp As Integer(), countp As Integer(), stridep As Integer(), ip As Byte()) As Integer
        Return nc_get_vars_text(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), ip)
    End Function
    Public Function nc_get_vars_uchar(ncid As Integer, varid As Integer, startp As Integer(), countp As Integer(), stridep As Integer(), ip As Byte()) As Integer
        Return nc_get_vars_uchar(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), ip)
    End Function
    Public Function nc_get_vars_schar(ncid As Integer, varid As Integer, startp As Integer(), countp As Integer(), stridep As Integer(), ip As SByte()) As Integer
        Return nc_get_vars_schar(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), ip)
    End Function
    Public Function nc_get_vars_short(ncid As Integer, varid As Integer, startp As Integer(), countp As Integer(), stridep As Integer(), ip As Short()) As Integer
        Return nc_get_vars_short(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), ip)
    End Function
    Public Function nc_get_vars_int(ncid As Integer, varid As Integer, startp As Integer(), countp As Integer(), stridep As Integer(), ip As Integer()) As Integer
        Return nc_get_vars_int(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), ip)
    End Function
    Public Function nc_get_vars_long(ncid As Integer, varid As Integer, startp As Integer(), countp As Integer(), stridep As Integer(), ip As Long()) As Integer
        Return nc_get_vars_long(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), ip)
    End Function
    Public Function nc_get_vars_float(ncid As Integer, varid As Integer, startp As Integer(), countp As Integer(), stridep As Integer(), ip As Single()) As Integer
        Return nc_get_vars_float(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), ip)
    End Function
    Public Function nc_get_vars_double(ncid As Integer, varid As Integer, startp As Integer(), countp As Integer(), stridep As Integer(), ip As Double()) As Integer
        Return nc_get_vars_double(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), ip)
    End Function
    Public Function nc_get_vars_ushort(ncid As Integer, varid As Integer, startp As Integer(), countp As Integer(), stridep As Integer(), ip As UShort()) As Integer
        Return nc_get_vars_ushort(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), ip)
    End Function
    Public Function nc_get_vars_uint(ncid As Integer, varid As Integer, startp As Integer(), countp As Integer(), stridep As Integer(), ip As UInteger()) As Integer
        Return nc_get_vars_uint(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), ip)
    End Function
    Public Function nc_get_vars_longlong(ncid As Integer, varid As Integer, startp As Integer(), countp As Integer(), stridep As Integer(), ip As Long()) As Integer
        Return nc_get_vars_longlong(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), ip)
    End Function
    Public Function nc_get_vars_ulonglong(ncid As Integer, varid As Integer, startp As Integer(), countp As Integer(), stridep As Integer(), ip As ULong()) As Integer
        Return nc_get_vars_ulonglong(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), ip)
    End Function
    Public Function nc_get_vars_string(ncid As Integer, varid As Integer, startp As Integer(), countp As Integer(), stridep As Integer(), ip As IntPtr()) As Integer
        Return nc_get_vars_string(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), ip)
    End Function
#End Region

#Region "put_var1"
    Public Function nc_put_var1_text(ncid As Integer, varid As Integer, index As Integer(), op As Byte) As Integer
        Return nc_put_var1_text(ncid, varid, ConvertToIntPtr(index), op)
    End Function
    Public Function nc_put_var1_schar(ncid As Integer, varid As Integer, index As Integer(), op As SByte) As Integer
        Return nc_put_var1_schar(ncid, varid, ConvertToIntPtr(index), op)
    End Function
    Public Function nc_put_var1_uchar(ncid As Integer, varid As Integer, index As Integer(), op As Byte) As Integer
        Return nc_put_var1_uchar(ncid, varid, ConvertToIntPtr(index), op)
    End Function
    Public Function nc_put_var1_short(ncid As Integer, varid As Integer, index As Integer(), op As Short) As Integer
        Return nc_put_var1_short(ncid, varid, ConvertToIntPtr(index), op)
    End Function
    Public Function nc_put_var1_int(ncid As Integer, varid As Integer, index As Integer(), op As Integer) As Integer
        Return nc_put_var1_int(ncid, varid, ConvertToIntPtr(index), op)
    End Function
    Public Function nc_put_var1_long(ncid As Integer, varid As Integer, index As Integer(), op As Long) As Integer
        Return nc_put_var1_long(ncid, varid, ConvertToIntPtr(index), op)
    End Function
    Public Function nc_put_var1_float(ncid As Integer, varid As Integer, index As Integer(), op As Single) As Integer
        Return nc_put_var1_float(ncid, varid, ConvertToIntPtr(index), op)
    End Function
    Public Function nc_put_var1_double(ncid As Integer, varid As Integer, index As Integer(), op As Double) As Integer
        Return nc_put_var1_double(ncid, varid, ConvertToIntPtr(index), op)
    End Function
    Public Function nc_put_var1_ubyte(ncid As Integer, varid As Integer, index As Integer(), op As Byte) As Integer
        Return nc_put_var1_ubyte(ncid, varid, ConvertToIntPtr(index), op)
    End Function
    Public Function nc_put_var1_ushort(ncid As Integer, varid As Integer, index As Integer(), op As UShort) As Integer
        Return nc_put_var1_ushort(ncid, varid, ConvertToIntPtr(index), op)
    End Function
    Public Function nc_put_var1_uint(ncid As Integer, varid As Integer, index As Integer(), op As UInteger) As Integer
        Return nc_put_var1_uint(ncid, varid, ConvertToIntPtr(index), op)
    End Function
    Public Function nc_put_var1_longlong(ncid As Integer, varid As Integer, index As Integer(), op As Long) As Integer
        Return nc_put_var1_longlong(ncid, varid, ConvertToIntPtr(index), op)
    End Function
    Public Function nc_put_var1_ulonglong(ncid As Integer, varid As Integer, index As Integer(), op As ULong) As Integer
        Return nc_put_var1_ulonglong(ncid, varid, ConvertToIntPtr(index), op)
    End Function
    Public Function nc_put_var1_string(ncid As Integer, varid As Integer, index As Integer(), op As String) As Integer
        Return nc_put_var1_string(ncid, varid, ConvertToIntPtr(index), op)
    End Function
#End Region

#Region "put_vara"
    Public Function nc_put_vara_text(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), op As Byte()) As Integer
        Return nc_put_vara_text(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op)
    End Function
    Public Function nc_put_vara_schar(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), op As SByte()) As Integer
        Return nc_put_vara_schar(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op)
    End Function
    Public Function nc_put_vara_uchar(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), op As Byte()) As Integer
        Return nc_put_vara_uchar(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op)
    End Function
    Public Function nc_put_vara_short(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), op As Short()) As Integer
        Return nc_put_vara_short(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op)
    End Function
    Public Function nc_put_vara_int(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), op As Integer()) As Integer
        Return nc_put_vara_int(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op)
    End Function
    Public Function nc_put_vara_long(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), op As Long()) As Integer
        Return nc_put_vara_long(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op)
    End Function
    Public Function nc_put_vara_float(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), op As Single()) As Integer
        Return nc_put_vara_float(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op)
    End Function
    Public Function nc_put_vara_double(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), op As Double()) As Integer
        Return nc_put_vara_double(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op)
    End Function
    Public Function nc_put_vara_ubyte(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), op As Byte()) As Integer
        Return nc_put_vara_ubyte(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op)
    End Function
    Public Function nc_put_vara_ushort(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), op As UShort()) As Integer
        Return nc_put_vara_ushort(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op)
    End Function
    Public Function nc_put_vara_uint(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), op As UInteger()) As Integer
        Return nc_put_vara_uint(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op)
    End Function
    Public Function nc_put_vara_longlong(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), op As Long()) As Integer
        Return nc_put_vara_longlong(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op)
    End Function
    Public Function nc_put_vara_ulonglong(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), op As ULong()) As Integer
        Return nc_put_vara_ulonglong(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op)
    End Function
    Public Function nc_put_vara_string(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), op As String()) As Integer
        Return nc_put_vara_string(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op)
    End Function

    ' Multidimensional array support
    Public Function nc_put_vara_short(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), op As Short(,)) As Integer
        Return nc_put_vara_short(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op)
    End Function
    Public Function nc_put_vara_int(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), op As Integer(,)) As Integer
        Return nc_put_vara_int(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op)
    End Function
    Public Function nc_put_vara_long(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), op As Long(,)) As Integer
        Return nc_put_vara_long(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op)
    End Function
    Public Function nc_put_vara_float(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), op As Single(,)) As Integer
        Return nc_put_vara_float(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op)
    End Function
    Public Function nc_put_vara_double(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), op As Double(,)) As Integer
        Return nc_put_vara_double(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op)
    End Function

    Public Function nc_put_vara_short(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), op As Short(,,)) As Integer
        Return nc_put_vara_short(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op)
    End Function
    Public Function nc_put_vara_int(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), op As Integer(,,)) As Integer
        Return nc_put_vara_int(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op)
    End Function
    Public Function nc_put_vara_long(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), op As Long(,,)) As Integer
        Return nc_put_vara_long(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op)
    End Function
    Public Function nc_put_vara_float(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), op As Single(,,)) As Integer
        Return nc_put_vara_float(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op)
    End Function
    Public Function nc_put_vara_double(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), op As Double(,,)) As Integer
        Return nc_put_vara_double(ncid, varid, ConvertToIntPtr(start), ConvertToIntPtr(count), op)
    End Function
#End Region

#Region "put_vars"
    Public Function nc_put_vars_text(ncid As Integer, varid As Integer, startp As Integer(), countp As Integer(), stridep As Integer(), op As Byte()) As Integer
        Return nc_put_vars_text(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), op)
    End Function
    Public Function nc_put_vars_uchar(ncid As Integer, varid As Integer, startp As Integer(), countp As Integer(), stridep As Integer(), op As Byte()) As Integer
        Return nc_put_vars_uchar(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), op)
    End Function
    Public Function nc_put_vars_schar(ncid As Integer, varid As Integer, startp As Integer(), countp As Integer(), stridep As Integer(), op As SByte()) As Integer
        Return nc_put_vars_schar(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), op)
    End Function
    Public Function nc_put_vars_short(ncid As Integer, varid As Integer, startp As Integer(), countp As Integer(), stridep As Integer(), op As Short()) As Integer
        Return nc_put_vars_short(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), op)
    End Function
    Public Function nc_put_vars_int(ncid As Integer, varid As Integer, startp As Integer(), countp As Integer(), stridep As Integer(), op As Integer()) As Integer
        Return nc_put_vars_int(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), op)
    End Function
    Public Function nc_put_vars_long(ncid As Integer, varid As Integer, startp As Integer(), countp As Integer(), stridep As Integer(), op As Long()) As Integer
        Return nc_put_vars_long(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), op)
    End Function
    Public Function nc_put_vars_float(ncid As Integer, varid As Integer, startp As Integer(), countp As Integer(), stridep As Integer(), op As Single()) As Integer
        Return nc_put_vars_float(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), op)
    End Function
    Public Function nc_put_vars_double(ncid As Integer, varid As Integer, startp As Integer(), countp As Integer(), stridep As Integer(), op As Double()) As Integer
        Return nc_put_vars_double(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), op)
    End Function
    Public Function nc_put_vars_ushort(ncid As Integer, varid As Integer, startp As Integer(), countp As Integer(), stridep As Integer(), op As UShort()) As Integer
        Return nc_put_vars_ushort(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), op)
    End Function
    Public Function nc_put_vars_uint(ncid As Integer, varid As Integer, startp As Integer(), countp As Integer(), stridep As Integer(), op As UInteger()) As Integer
        Return nc_put_vars_uint(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), op)
    End Function
    Public Function nc_put_vars_longlong(ncid As Integer, varid As Integer, startp As Integer(), countp As Integer(), stridep As Integer(), op As Long()) As Integer
        Return nc_put_vars_longlong(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), op)
    End Function
    Public Function nc_put_vars_ulonglong(ncid As Integer, varid As Integer, startp As Integer(), countp As Integer(), stridep As Integer(), op As ULong()) As Integer
        Return nc_put_vars_ulonglong(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), op)
    End Function
    Public Function nc_put_vars_string(ncid As Integer, varid As Integer, startp As Integer(), countp As Integer(), stridep As Integer(), op As String) As Integer
        Return nc_put_vars_string(ncid, varid, ConvertToIntPtr(startp), ConvertToIntPtr(countp), ConvertToIntPtr(stridep), op)
    End Function
#End Region

    Private Function ConvertToIntPtr(indexp As Integer()) As IntPtr()
        Dim index = New IntPtr(indexp.Length - 1) {}
        For i = 0 To index.Length - 1
            index(i) = CType(indexp(i), IntPtr)
        Next
        Return index
    End Function
#End Region

    '
    ' C# Friendly methods for returning strings
    '  These methods return the status as a parameter
    '
    ''' <summary>Friendly method for nc_inq_path</summary>
    Public Function nc_inq_path(ncid As Integer, <Out> ByRef status As Integer) As String
        Dim len As IntPtr = Nothing
        status = nc_inq_path(ncid, len, Nothing)
        If status <> 0 Then Return String.Empty
        Dim sb As StringBuilder = New StringBuilder(CInt(len))
        status = nc_inq_path(ncid, len, sb)
        If status <> 0 Then Return String.Empty
        Return sb.ToString()
    End Function

    ''' <summary>Friendly method for nc_inq_grpname_full</summary>
    Public Function nc_inq_grpname_full(ncid As Integer, <Out> ByRef status As Integer) As String
        Dim len As IntPtr = Nothing
        status = nc_inq_grpname_full(ncid, len, Nothing)
        If status <> 0 Then Return String.Empty
        Dim sb As StringBuilder = New StringBuilder(CInt(len))
        status = nc_inq_grpname_full(ncid, len, sb)
        If status <> 0 Then Return String.Empty
        Return sb.ToString()
    End Function

    '
    ' Friendly methods for reading attributes
    '
    Public Function nc_get_att_text(ncid As Integer, varid As Integer, name As String, <Out> ByRef status As Integer) As String
        Dim len As IntPtr = Nothing
        status = nc_inq_attlen(ncid, varid, name, len)
        If status <> 0 Then Return String.Empty
        Dim sb As StringBuilder = New StringBuilder(CInt(len))
        status = nc_get_att_text(ncid, varid, name, sb)
        If status <> 0 Then Return String.Empty
        Return sb.ToString()
    End Function
    Public Function nc_get_att_schar(ncid As Integer, varid As Integer, name As String, <Out> ByRef status As Integer) As SByte()
        Dim len As IntPtr = Nothing
        status = nc_inq_attlen(ncid, varid, name, len)
        If status <> 0 Then Return Nothing
        Dim value = New SByte(CInt(len) - 1) {}
        status = nc_get_att_schar(ncid, varid, name, value)
        If status <> 0 Then Return Nothing
        Return value
    End Function
    Public Function nc_get_att_uchar(ncid As Integer, varid As Integer, name As String, <Out> ByRef status As Integer) As Byte()
        Dim len As IntPtr = Nothing
        status = nc_inq_attlen(ncid, varid, name, len)
        If status <> 0 Then Return Nothing
        Dim value = New Byte(CInt(len) - 1) {}
        status = nc_get_att_uchar(ncid, varid, name, value)
        If status <> 0 Then Return Nothing
        Return value
    End Function
    Public Function nc_get_att_short(ncid As Integer, varid As Integer, name As String, <Out> ByRef status As Integer) As Short()
        Dim len As IntPtr = Nothing
        status = nc_inq_attlen(ncid, varid, name, len)
        If status <> 0 Then Return Nothing
        Dim value = New Short(CInt(len) - 1) {}
        status = nc_get_att_short(ncid, varid, name, value)
        If status <> 0 Then Return Nothing
        Return value
    End Function
    Public Function nc_get_att_int(ncid As Integer, varid As Integer, name As String, <Out> ByRef status As Integer) As Integer()
        Dim len As IntPtr = Nothing
        status = nc_inq_attlen(ncid, varid, name, len)
        If status <> 0 Then Return Nothing
        Dim value = New Integer(CInt(len) - 1) {}
        status = nc_get_att_int(ncid, varid, name, value)
        If status <> 0 Then Return Nothing
        Return value
    End Function
    Public Function nc_get_att_long(ncid As Integer, varid As Integer, name As String, <Out> ByRef status As Integer) As Long()
        Dim len As IntPtr = Nothing
        status = nc_inq_attlen(ncid, varid, name, len)
        If status <> 0 Then Return Nothing
        Dim value = New Long(CInt(len) - 1) {}
        status = nc_get_att_long(ncid, varid, name, value)
        If status <> 0 Then Return Nothing
        Return value
    End Function
    Public Function nc_get_att_float(ncid As Integer, varid As Integer, name As String, <Out> ByRef status As Integer) As Single()
        Dim len As IntPtr = Nothing
        status = nc_inq_attlen(ncid, varid, name, len)
        If status <> 0 Then Return Nothing
        Dim value = New Single(CInt(len) - 1) {}
        status = nc_get_att_float(ncid, varid, name, value)
        If status <> 0 Then Return Nothing
        Return value
    End Function
    Public Function nc_get_att_double(ncid As Integer, varid As Integer, name As String, <Out> ByRef status As Integer) As Double()
        Dim len As IntPtr = Nothing
        status = nc_inq_attlen(ncid, varid, name, len)
        If status <> 0 Then Return Nothing
        Dim value = New Double(CInt(len) - 1) {}
        status = nc_get_att_double(ncid, varid, name, value)
        If status <> 0 Then Return Nothing
        Return value
    End Function
    Public Function nc_get_att_ubyte(ncid As Integer, varid As Integer, name As String, <Out> ByRef status As Integer) As Byte()
        Dim nctype As nc_type = Nothing, len As IntPtr = Nothing
        status = nc_inq_att(ncid, varid, name, nctype, len)
        If status <> 0 Then Return Nothing
        Dim value = New Byte(CInt(len) - 1) {}
        status = nc_get_att_ubyte(ncid, varid, name, value)
        If status <> 0 Then Return Nothing
        Return value
    End Function
    Public Function nc_get_att_ushort(ncid As Integer, varid As Integer, name As String, <Out> ByRef status As Integer) As UShort()
        Dim len As IntPtr = Nothing
        status = nc_inq_attlen(ncid, varid, name, len)
        If status <> 0 Then Return Nothing
        Dim value = New UShort(CInt(len) - 1) {}
        status = nc_get_att_ushort(ncid, varid, name, value)
        If status <> 0 Then Return Nothing
        Return value
    End Function
    Public Function nc_get_att_uint(ncid As Integer, varid As Integer, name As String, <Out> ByRef status As Integer) As UInteger()
        Dim len As IntPtr = Nothing
        status = nc_inq_attlen(ncid, varid, name, len)
        If status <> 0 Then Return Nothing
        Dim value = New UInteger(CInt(len) - 1) {}
        status = nc_get_att_uint(ncid, varid, name, value)
        If status <> 0 Then Return Nothing
        Return value
    End Function
    Public Function nc_get_att_longlong(ncid As Integer, varid As Integer, name As String, <Out> ByRef status As Integer) As Long()
        Dim len As IntPtr = Nothing
        status = nc_inq_attlen(ncid, varid, name, len)
        If status <> 0 Then Return Nothing
        Dim value = New Long(CInt(len) - 1) {}
        status = nc_get_att_longlong(ncid, varid, name, value)
        If status <> 0 Then Return Nothing
        Return value
    End Function
    Public Function nc_get_att_ulonglong(ncid As Integer, varid As Integer, name As String, <Out> ByRef status As Integer) As ULong()
        Dim len As IntPtr = Nothing
        status = nc_inq_attlen(ncid, varid, name, len)
        If status <> 0 Then Return Nothing
        Dim value = New ULong(CInt(len) - 1) {}
        status = nc_get_att_ulonglong(ncid, varid, name, value)
        If status <> 0 Then Return Nothing
        Return value
    End Function

    '
    ' Friendly methods for writing attributes
    '
    Public Function nc_put_att_text(ncid As Integer, varid As Integer, name As String, value As String) As Integer
        Return nc_put_att_text(ncid, varid, name, value.Length, value)
    End Function
    Public Function nc_put_att_double(ncid As Integer, varid As Integer, name As String, value As Double()) As Integer
        Return nc_put_att_double(ncid, varid, name, nc_type.NC_DOUBLE, value.Length, value)
    End Function
    ''' <summary>Write a single attribute value</summary>
    Public Function nc_put_att_double(ncid As Integer, varid As Integer, name As String, value As Double) As Integer
        Dim v = New Double(0) {}
        v(0) = value
        Return nc_put_att_double(ncid, varid, name, v)
    End Function
    Public Function nc_put_att_float(ncid As Integer, varid As Integer, name As String, value As Single()) As Integer
        Return nc_put_att_float(ncid, varid, name, nc_type.NC_FLOAT, value.Length, value)
    End Function

    ''' <summary>Write a single attribute value</summary>
    Public Function nc_put_att_float(ncid As Integer, varid As Integer, name As String, value As Single) As Integer
        Dim v = New Single(0) {}
        v(0) = value
        Return nc_put_att_float(ncid, varid, name, v)
    End Function
    Public Function nc_put_att_int(ncid As Integer, varid As Integer, name As String, value As Integer()) As Integer
        Return nc_put_att_int(ncid, varid, name, nc_type.NC_INT, value.Length, value)
    End Function
    ''' <summary>Write a single attribute value</summary>
    Public Function nc_put_att_int(ncid As Integer, varid As Integer, name As String, value As Integer) As Integer
        Dim v = New Integer(0) {}
        v(0) = value
        Return nc_put_att_int(ncid, varid, name, v)
    End Function
    Public Function nc_put_att_long(ncid As Integer, varid As Integer, name As String, value As Long()) As Integer
        Return nc_put_att_long(ncid, varid, name, nc_type.NC_INT64, value.Length, value)
    End Function

    ''' <summary>Write a single attribute value</summary>
    Public Function nc_put_att_long(ncid As Integer, varid As Integer, name As String, value As Long) As Integer
        Dim v = New Long(0) {}
        v(0) = value
        Return nc_put_att_long(ncid, varid, name, v)
    End Function
    Public Function nc_put_att_longlong(ncid As Integer, varid As Integer, name As String, value As Long()) As Integer
        Return nc_put_att_longlong(ncid, varid, name, nc_type.NC_INT64, CType(value.Length, IntPtr), value)
    End Function

    ''' <summary>Write a single attribute value</summary>
    Public Function nc_put_att_longlong(ncid As Integer, varid As Integer, name As String, value As Long) As Integer
        Dim v = New Long(0) {}
        v(0) = value
        Return nc_put_att_long(ncid, varid, name, v)
    End Function
    Public Function nc_put_att_schar(ncid As Integer, varid As Integer, name As String, value As SByte()) As Integer
        Return nc_put_att_schar(ncid, varid, name, nc_type.NC_BYTE, CType(value.Length, IntPtr), value)
    End Function

    ''' <summary>Write a single attribute value</summary>
    Public Function nc_put_att_schar(ncid As Integer, varid As Integer, name As String, value As SByte) As Integer
        Dim v = New SByte(0) {}
        v(0) = value
        Return nc_put_att_schar(ncid, varid, name, v)
    End Function
    Public Function nc_put_att_short(ncid As Integer, varid As Integer, name As String, value As Short()) As Integer
        Return nc_put_att_short(ncid, varid, name, nc_type.NC_SHORT, CType(value.Length, IntPtr), value)
    End Function
    ''' <summary>Write a single attribute value</summary>
    Public Function nc_put_att_short(ncid As Integer, varid As Integer, name As String, value As Short) As Integer
        Dim v = New Short(0) {}
        v(0) = value
        Return nc_put_att_short(ncid, varid, name, v)
    End Function

    Public Function nc_put_att_ubyte(ncid As Integer, varid As Integer, name As String, value As Byte()) As Integer
        Return nc_put_att_ubyte(ncid, varid, name, nc_type.NC_UBYTE, CType(value.Length, IntPtr), value)
    End Function

    ''' <summary>Write a single attribute value</summary>
    Public Function nc_put_att_ubyte(ncid As Integer, varid As Integer, name As String, value As Byte) As Integer
        Dim v = New Byte(0) {}
        v(0) = value
        Return nc_put_att_ubyte(ncid, varid, name, v)
    End Function
    Public Function nc_put_att_uchar(ncid As Integer, varid As Integer, name As String, value As Byte()) As Integer
        Return nc_put_att_uchar(ncid, varid, name, nc_type.NC_CHAR, CType(value.Length, IntPtr), value)
    End Function
    ''' <summary>Write a single attribute value</summary>
    Public Function nc_put_att_uchar(ncid As Integer, varid As Integer, name As String, value As Byte) As Integer
        Dim v = New Byte(0) {}
        v(0) = value
        Return nc_put_att_uchar(ncid, varid, name, v)
    End Function
    Public Function nc_put_att_uint(ncid As Integer, varid As Integer, name As String, value As UInteger()) As Integer
        Return nc_put_att_uint(ncid, varid, name, nc_type.NC_UINT, CType(value.Length, IntPtr), value)
    End Function

    ''' <summary>Write a single attribute value</summary>
    Public Function nc_put_att_uint(ncid As Integer, varid As Integer, name As String, value As UInteger) As Integer
        Dim v = New UInteger(0) {}
        v(0) = value
        Return nc_put_att_uint(ncid, varid, name, v)
    End Function

    Public Function nc_put_att_ulonglong(ncid As Integer, varid As Integer, name As String, value As ULong()) As Integer
        Return nc_put_att_ulonglong(ncid, varid, name, nc_type.NC_UINT64, CType(value.Length, IntPtr), value)
    End Function

    ''' <summary>Write a single attribute value</summary>
    Public Function nc_put_att_ulonglong(ncid As Integer, varid As Integer, name As String, value As ULong) As Integer
        Dim v = New ULong(0) {}
        v(0) = value
        Return nc_put_att_ulonglong(ncid, varid, name, v)
    End Function

    Public Function nc_put_att_ushort(ncid As Integer, varid As Integer, name As String, value As UShort()) As Integer
        Return nc_put_att_ushort(ncid, varid, name, nc_type.NC_USHORT, CType(value.Length, IntPtr), value)
    End Function

    ''' <summary>Write a single attribute value</summary>
    Public Function nc_put_att_ushort(ncid As Integer, varid As Integer, name As String, value As UShort) As Integer
        Dim v = New UShort(0) {}
        v(0) = value
        Return nc_put_att_ushort(ncid, varid, name, v)
    End Function

    '
    '  Friendly methods for dealing with strings and other awkward data types
    '
#Region "Methods for strings"
    Public Function nc_get_var_string(ncid As Integer, varid As Integer, <Out> ByRef status As Integer) As String()
        Dim [dim] = New Integer(0) {}
        status = nc_inq_vardimid(ncid, varid, [dim])
        If status <> 0 Then Return Nothing
        Dim length As IntPtr = Nothing
        status = nc_inq_dimlen(ncid, [dim](0), length)
        If status <> 0 Then Return Nothing
        Dim ptrs = New IntPtr(CInt(length) - 1) {}
        status = nc_get_var_string(ncid, varid, ptrs)
        If status <> 0 Then Return Nothing
        Dim s = New String(ptrs.Length - 1) {}
        For i = 0 To ptrs.Length - 1
            s(i) = Marshal.PtrToStringAnsi(ptrs(i))
        Next
        status = nc_free_string(CType(ptrs.Length, IntPtr), ptrs)
        Return s
    End Function

    Public Function nc_get_var1_string(ncid As Integer, varid As Integer, index As Integer(), <Out> ByRef status As Integer) As String
        Dim ptrs = New IntPtr(0) {}
        status = nc_get_var1_string(ncid, varid, index, ptrs)
        If status <> 0 Then Return Nothing
        Dim s As String
        s = Marshal.PtrToStringAnsi(ptrs(0))
        status = nc_free_string(CType(ptrs.Length, IntPtr), ptrs)
        Return s
    End Function

    Public Function nc_get_vara_string(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), <Out> ByRef status As Integer) As String()
        Dim len = 0
        For i = 0 To count.Length - 1
            len += count(i)
        Next
        Dim ptrs = New IntPtr(len - 1) {}
        status = nc_get_vara_string(ncid, varid, start, count, ptrs)
        If status <> 0 Then Return Nothing
        Dim s = New String(len - 1) {}
        For i = 0 To ptrs.Length - 1
            s(i) = Marshal.PtrToStringAnsi(ptrs(i))
        Next
        status = nc_free_string(CType(ptrs.Length, IntPtr), ptrs)
        Return s
    End Function

    Public Function nc_get_vars_string(ncid As Integer, varid As Integer, start As Integer(), count As Integer(), stride As Integer(), <Out> ByRef status As Integer) As String()
        Dim len = 0
        For i = 0 To count.Length - 1
            len += count(i) / stride(i)
        Next
        Dim ptrs = New IntPtr(len - 1) {}
        status = nc_get_vars_string(ncid, varid, start, count, stride, ptrs)
        If status <> 0 Then Return Nothing
        Dim s = New String(len - 1) {}
        For i = 0 To ptrs.Length - 1
            s(i) = Marshal.PtrToStringAnsi(ptrs(i))
        Next
        status = nc_free_string(CType(ptrs.Length, IntPtr), ptrs)
        Return s
    End Function
#End Region
#End Region

#Region "Higher level methods"
    ' These methods wrap up some NetCDF function calls that make them easier to use
    '  but less robust - they will be fine if we know we are using a good NetCDF file or we're happy to skip some error processing
    ' Get a global attribute
    Public Function GetGlobalAttribute(ncid As Integer, p_AttName As String) As String
        Dim type As nc_type = Nothing, len As IntPtr = Nothing
        Try
            If nc_inq_att(ncid, NC_GLOBAL, p_AttName, type, len) <> 0 Then Return String.Empty
            If type = nc_type.NC_INT Then
                Dim att = New Integer(CInt(len) - 1) {}
                If nc_get_att_int(ncid, NC_GLOBAL, p_AttName, att) = 0 Then Return att(0).ToString()
            End If
            If type = nc_type.NC_FLOAT Then
                Dim att = New Single(CInt(len) - 1) {}
                If nc_get_att_float(ncid, NC_GLOBAL, p_AttName, att) = 0 Then Return att(0).ToString()
            End If
            If type = nc_type.NC_DOUBLE Then
                Dim att = New Double(CInt(len) - 1) {}
                If nc_get_att_double(ncid, NC_GLOBAL, p_AttName, att) = 0 Then Return att(0).ToString()
            End If
            If type = nc_type.NC_INT64 Then
                Dim att = New Long(CInt(len) - 1) {}
                If nc_get_att_longlong(ncid, NC_GLOBAL, p_AttName, att) = 0 Then Return att(0).ToString()
            End If

            If type <> nc_type.NC_STRING AndAlso type <> nc_type.NC_CHAR Then Return String.Empty
            Dim sb As StringBuilder = New StringBuilder(CInt(len))
            If nc_get_att_text(ncid, NC_GLOBAL, p_AttName, sb) <> 0 Then Return String.Empty
            Return sb.ToString().Substring(0, CInt(len))
        Catch __unusedException1__ As Exception
            Return "GetGlobalAttribute for " & p_AttName & " failed"
        End Try
    End Function

    Public Function GetGlobalAttribute(ncid As Integer, p_AttName As String, <Out> ByRef value As Integer) As Boolean
        value = 0

        Dim type As nc_type = Nothing, len As IntPtr = Nothing
        If nc_inq_att(ncid, NC_GLOBAL, p_AttName, type, len) <> 0 Then Return False
        If type <> nc_type.NC_INT Then Return False

        Dim s = New Integer(CInt(len) - 1) {}

        If nc_get_att_int(ncid, NC_GLOBAL, p_AttName, s) <> 0 Then Return False
        value = s(0)
        Return True
    End Function

    Public Function GetGlobalDouble(ncid As Integer, p_AttName As String) As Double
        Dim type As nc_type = Nothing, len As IntPtr = Nothing
        If nc_inq_att(ncid, NC_GLOBAL, p_AttName, type, len) <> 0 Then Return 0
        If type <> nc_type.NC_DOUBLE Then Return 0

        Dim data = New Double(CInt(len) - 1) {}
        If nc_get_att_double(ncid, NC_GLOBAL, p_AttName, data) <> 0 Then Return 0
        Return data(0)
    End Function
    Public Function GetGlobalFloat(ncid As Integer, p_AttName As String) As Single
        Dim type As nc_type = Nothing, len As IntPtr = Nothing
        If nc_inq_att(ncid, NC_GLOBAL, p_AttName, type, len) <> 0 Then Return 0
        If type <> nc_type.NC_FLOAT Then Return 0

        Dim data = New Single(CInt(len) - 1) {}
        If nc_get_att_float(ncid, NC_GLOBAL, p_AttName, data) <> 0 Then Return 0
        Return data(0)
    End Function

    Public Function GetGlobalShort(ncid As Integer, p_AttName As String) As Short
        Dim type As nc_type = Nothing, len As IntPtr = Nothing
        If nc_inq_att(ncid, NC_GLOBAL, p_AttName, type, len) <> 0 Then Return 0
        If type <> nc_type.NC_SHORT Then Return 0

        Dim data = New Short(CInt(len) - 1) {}
        If nc_get_att_short(ncid, NC_GLOBAL, p_AttName, data) <> 0 Then Return 0
        Return data(0)
    End Function

    Public Function GetGlobalInt(ncid As Integer, p_AttName As String) As Integer
        Dim type As nc_type = Nothing, len As IntPtr = Nothing
        If nc_inq_att(ncid, NC_GLOBAL, p_AttName, type, len) <> 0 Then Return 0
        If type <> nc_type.NC_INT Then Return 0

        Dim data = New Integer(CInt(len) - 1) {}
        If nc_get_att_int(ncid, NC_GLOBAL, p_AttName, data) <> 0 Then Return 0
        Return data(0)
    End Function

    Public Function GetGlobalBool(ncid As Integer, p_AttName As String) As Boolean
        Dim type As nc_type = Nothing, len As IntPtr = Nothing
        If nc_inq_att(ncid, NC_GLOBAL, p_AttName, type, len) <> 0 Then Return False

        Dim sb As StringBuilder = New StringBuilder(CInt(len))
        If nc_get_att_text(ncid, NC_GLOBAL, p_AttName, sb) <> 0 Then Return False
        Dim result As Boolean = Nothing
        Call Boolean.TryParse(sb.ToString(), result)
        Return result
    End Function

    ' Dates are stored as strings in the metadata - more accessible
    Public Function GetGlobalDateTime(ncid As Integer, p_AttName As String) As Date
        Dim type As nc_type = Nothing, len As IntPtr = Nothing
        If nc_inq_att(ncid, NC_GLOBAL, p_AttName, type, len) <> 0 Then Return New DateTime()
        Dim sb As StringBuilder = New StringBuilder(CInt(len))
        If nc_get_att_text(ncid, NC_GLOBAL, p_AttName, sb) <> 0 Then Return New DateTime()

        Dim [date] As Date = Nothing
        Date.TryParse(sb.ToString(), [date])
        Return [date]
    End Function

    ' Get a variable attribute
    Public Function GetVarAttribute(ncid As Integer, VarName As String, p_AttName As String) As String
        Dim varid As Integer = Nothing
        If nc_inq_varid(ncid, VarName, varid) <> 0 Then Return String.Empty
        Return GetVarAttribute(ncid, varid, p_AttName)
    End Function

    Public Function GetVarAttribute(ncid As Integer, varid As Integer, p_AttName As String) As String
        Dim type As nc_type = Nothing, len As IntPtr = Nothing
        If nc_inq_att(ncid, varid, p_AttName, type, len) <> 0 Then Return String.Empty
        Dim sb As StringBuilder = New StringBuilder(CInt(len))
        If nc_get_att_text(ncid, varid, p_AttName, sb) <> 0 Then Return String.Empty
        Return sb.ToString().Substring(0, CInt(len))
    End Function
    Public Function GetVarAttribute(ncid As Integer, VarName As String, p_AttName As String, <Out> ByRef value As Short) As Boolean
        value = 0

        Dim varid As Integer = Nothing
        If nc_inq_varid(ncid, VarName, varid) <> 0 Then Return False
        Dim type As nc_type = Nothing, len As IntPtr = Nothing
        If nc_inq_att(ncid, varid, p_AttName, type, len) <> 0 Then Return False

        Dim s = New Short(CInt(len) - 1) {}

        If nc_get_att_short(ncid, varid, p_AttName, s) <> 0 Then Return False
        value = s(0)
        Return True
    End Function

    Public Function GetVarAttribute(ncid As Integer, varid As Integer, p_AttName As String, <Out> ByRef value As Short) As Boolean
        value = 0

        Dim type As nc_type = Nothing, len As IntPtr = Nothing
        If nc_inq_att(ncid, varid, p_AttName, type, len) <> 0 Then Return False

        Dim s = New Short(CInt(len) - 1) {}

        If nc_get_att_short(ncid, varid, p_AttName, s) <> 0 Then Return False
        value = s(0)
        Return True
    End Function

    Public Function GetVarAttribute(ncid As Integer, VarName As String, p_AttName As String, <Out> ByRef value As Integer) As Boolean
        value = 0

        Dim varid As Integer = Nothing
        If nc_inq_varid(ncid, VarName, varid) <> 0 Then Return False
        Dim type As nc_type = Nothing, len As IntPtr = Nothing
        If nc_inq_att(ncid, varid, p_AttName, type, len) <> 0 Then Return False

        Dim s = New Integer(CInt(len) - 1) {}

        If nc_get_att_int(ncid, varid, p_AttName, s) <> 0 Then Return False
        value = s(0)
        Return True
    End Function

    Public Function GetVarAttribute(ncid As Integer, VarName As String, p_AttName As String, <Out> ByRef value As Single) As Boolean
        value = 0

        Dim varid As Integer = Nothing
        If nc_inq_varid(ncid, VarName, varid) <> 0 Then Return False
        Dim type As nc_type = Nothing, len As IntPtr = Nothing
        If nc_inq_att(ncid, varid, p_AttName, type, len) <> 0 Then Return False

        Dim s = New Single(CInt(len) - 1) {}

        If nc_get_att_float(ncid, varid, p_AttName, s) <> 0 Then Return False
        value = s(0)
        Return True
    End Function

    Public Function GetVarAttribute(ncid As Integer, varid As Integer, p_AttName As String, <Out> ByRef value As Single) As Boolean
        value = 0

        Dim type As nc_type = Nothing, len As IntPtr = Nothing
        If nc_inq_att(ncid, varid, p_AttName, type, len) <> 0 Then Return False

        Dim s = New Single(CInt(len) - 1) {}

        If nc_get_att_float(ncid, varid, p_AttName, s) <> 0 Then Return False
        value = s(0)
        Return True
    End Function

    ' Check if a variable exists
    Public Function VarExists(ncid As Integer, VarName As String) As Boolean
        Return NetCDF.nc_inq_varid(ncid, VarName, Nothing) = 0
    End Function

    ' Get int data
    Public Sub Get_int(ncid As Integer, VarName As String, data As Integer())
        Dim varid As Integer = Nothing
        nc_inq_varid(ncid, VarName, varid)
        nc_get_var_int(ncid, varid, data)
    End Sub

    Public Function Get_int(ncid As Integer, VarName As String) As Integer()
        Dim varid As Integer = Nothing
        nc_inq_varid(ncid, VarName, varid)
        Dim dimid As Integer = Nothing
        nc_inq_dimid(ncid, VarName, dimid)
        Dim len As IntPtr = Nothing
        nc_inq_dimlen(ncid, dimid, len)
        Dim data = New Integer(CInt(len) - 1) {}
        nc_get_var_int(ncid, varid, data)
        Return data
    End Function

    ' Get float data
    Public Sub Get_float(ncid As Integer, VarName As String, data As Single())
        Dim varid As Integer = Nothing
        nc_inq_varid(ncid, VarName, varid)
        nc_get_var_float(ncid, varid, data)
    End Sub

    Public Function Get_float(ncid As Integer, VarName As String) As Single()
        Dim varid As Integer = Nothing
        nc_inq_varid(ncid, VarName, varid)
        Dim ndims As Integer
        nc_inq_varndims(ncid, varid, ndims)
        Dim dims As Integer() = New Integer(ndims - 1) {}
        nc_inq_vardimid(ncid, varid, dims)
        Dim len As Integer = dims.ProductALL
        Dim data = New Single(len - 1) {}
        nc_get_var_float(ncid, varid, data)
        Return data
    End Function

    ' Get float data
    Public Sub Get_float(ncid As Integer, VarName As String, data As Single(,,))
        Dim varid As Integer = Nothing
        nc_inq_varid(ncid, VarName, varid)
        nc_get_var_float(ncid, varid, data)
    End Sub

    ' Get double data
    Public Function Get_double(ncid As Integer, varName As String) As Double()
        Dim varid As Integer = Nothing
        nc_inq_varid(ncid, varName, varid)
        Dim dimid As Integer = Nothing
        nc_inq_dimid(ncid, varName, dimid)
        Dim len As IntPtr = Nothing
        nc_inq_dimlen(ncid, dimid, len)
        Dim data = New Double(CInt(len) - 1) {}
        nc_get_var_double(ncid, varid, data)
        Return data
    End Function

    Public Sub Get_double(ncid As Integer, VarName As String, data As Double())
        Dim varid As Integer = Nothing
        nc_inq_varid(ncid, VarName, varid)
        nc_get_var_double(ncid, varid, data)
    End Sub

    ' Get short data
    Public Sub Get_short(ncid As Integer, VarName As String, data As Short())
        Dim varid As Integer = Nothing
        nc_inq_varid(ncid, VarName, varid)
        nc_get_var_short(ncid, varid, data)
    End Sub

    Public Function Get_short(ncid As Integer, VarName As String) As Short()
        Dim varid As Integer = Nothing
        nc_inq_varid(ncid, VarName, varid)
        Dim dimid As Integer = Nothing
        nc_inq_dimid(ncid, VarName, dimid)
        Dim len As IntPtr = Nothing
        nc_inq_dimlen(ncid, dimid, len)
        Dim data = New Short(CInt(len) - 1) {}
        nc_get_var_short(ncid, varid, data)
        Return data
    End Function

    Public Sub Get_short(ncid As Integer, VarName As String, data As Short(,))
        Dim varid As Integer = Nothing
        nc_inq_varid(ncid, VarName, varid)
        nc_get_var_short(ncid, varid, data)
    End Sub

    ' Get long data
    Public Sub Get_long(ncid As Integer, VarName As String, data As Long())
        Dim varid As Integer = Nothing
        nc_inq_varid(ncid, VarName, varid)
        nc_get_var_longlong(ncid, varid, data)
    End Sub

    ' Get byte data
    Public Sub Get_byte(ncid As Integer, VarName As String, data As Byte())
        Dim varid As Integer = Nothing
        nc_inq_varid(ncid, VarName, varid)
        nc_get_var_ubyte(ncid, varid, data)
    End Sub

    Public Function Get_byte(ncid As Integer, VarName As String) As Byte()
        Dim varid As Integer = Nothing
        nc_inq_varid(ncid, VarName, varid)
        Dim dimid As Integer = Nothing
        nc_inq_dimid(ncid, VarName, dimid)
        Dim len As IntPtr = Nothing
        nc_inq_dimlen(ncid, dimid, len)
        Dim data = New Byte(CInt(len) - 1) {}
        nc_get_var_ubyte(ncid, varid, data)
        Return data
    End Function

    ' Methods to write attribute data
    Public Sub PutGlobalAttribute(ncid As Integer, AttName As String, AttValue As String)
        nc_put_att_text(ncid, NC_GLOBAL, AttName, AttValue.Length, AttValue)
    End Sub
    Public Sub PutGlobalAttribute(ncid As Integer, AttName As String, AttValue As Date)
        PutVarAttribute(ncid, NC_GLOBAL, AttName, AttValue)
    End Sub
    Public Sub PutGlobalAttribute(ncid As Integer, AttName As String, AttValue As Double)
        PutVarAttribute(ncid, NC_GLOBAL, AttName, AttValue)
    End Sub
    Public Sub PutGlobalAttribute(ncid As Integer, AttName As String, AttValue As Single)
        PutVarAttribute(ncid, NC_GLOBAL, AttName, AttValue)
    End Sub
    Public Sub PutGlobalAttribute(ncid As Integer, AttName As String, AttValue As Integer)
        PutVarAttribute(ncid, NC_GLOBAL, AttName, AttValue)
    End Sub
    Public Sub PutGlobalAttribute(ncid As Integer, AttName As String, AttValue As Short)
        PutVarAttribute(ncid, NC_GLOBAL, AttName, AttValue)
    End Sub
    Public Sub PutGlobalAttribute(ncid As Integer, AttName As String, AttValue As Boolean)
        PutGlobalAttribute(ncid, AttName, AttValue.ToString())
    End Sub
    Public Sub PutVarAttribute(ncid As Integer, varid As Integer, AttName As String, AttValue As String)
        nc_put_att_text(ncid, varid, AttName, AttValue.Length, AttValue)
    End Sub
    Public Sub PutVarAttribute(ncid As Integer, varid As Integer, AttName As String, AttValue As Date)
        Dim [date] = AttValue.ToString("o")
        nc_put_att_text(ncid, varid, AttName, [date].Length, [date])
    End Sub
    Public Sub PutVarAttribute(ncid As Integer, varid As Integer, AttName As String, AttValue As Double)
        Dim att = New Double(0) {}
        att(0) = AttValue
        nc_put_att_double(ncid, varid, AttName, nc_type.NC_DOUBLE, CType(att.Length, IntPtr), att)
    End Sub
    Public Sub PutVarAttribute(ncid As Integer, varid As Integer, AttName As String, AttValue As Single)
        Dim att = New Single(0) {}
        att(0) = AttValue
        nc_put_att_float(ncid, varid, AttName, nc_type.NC_FLOAT, CType(att.Length, IntPtr), att)
    End Sub
    Public Sub PutVarAttribute(ncid As Integer, varid As Integer, AttName As String, AttValue As Integer)
        Dim att = New Integer(0) {}
        att(0) = AttValue
        nc_put_att_int(ncid, varid, AttName, nc_type.NC_INT, CType(att.Length, IntPtr), att)
    End Sub
    Public Sub PutVarAttribute(ncid As Integer, varid As Integer, AttName As String, AttValue As Short)
        Dim att = New Short(0) {}
        att(0) = AttValue
        nc_put_att_short(ncid, varid, AttName, nc_type.NC_SHORT, CType(att.Length, IntPtr), att)
    End Sub
    Public Sub PutVarAttribute(ncid As Integer, varid As Integer, AttName As String, AttValue As Boolean)
        PutVarAttribute(ncid, varid, AttName, AttValue.ToString())
    End Sub

#End Region

End Module
