#Region "Microsoft.VisualBasic::3ac155dedf409fba8717a27d3aeb7663, G:/GCModeller/src/runtime/Darwinism/src/data/CDF.PInvoke//PInvoke/netCDF.vb"

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

    '   Total Lines: 1313
    '    Code Lines: 842
    ' Comment Lines: 218
    '   Blank Lines: 253
    '     File Size: 72.22 KB


    ' Module NetCDF
    ' 
    '     Function: nc_abort, nc_close, nc_copy_att, nc_copy_var, nc_create
    '               nc_create_mem, nc_def_compound, nc_def_dim, nc_def_enum, nc_def_grp
    '               nc_def_var, nc_def_var_chunking, nc_def_var_deflate, nc_def_var_endian, nc_def_var_fill
    '               nc_def_var_filter, nc_def_var_fletcher32, nc_def_vlen, nc_del_att, nc_enddef
    '               nc_free_string, nc_get_att_double, nc_get_att_float, nc_get_att_int, nc_get_att_long
    '               nc_get_att_longlong, nc_get_att_schar, nc_get_att_short, nc_get_att_text, nc_get_att_ubyte
    '               nc_get_att_uchar, nc_get_att_uint, nc_get_att_ulonglong, nc_get_att_ushort, nc_get_chunk_cache
    '               nc_get_var_chunk_cache, nc_get_var_double, nc_get_var_float, nc_get_var_int, nc_get_var_long
    '               nc_get_var_longlong, nc_get_var_schar, nc_get_var_short, nc_get_var_string, nc_get_var_text
    '               nc_get_var_ubyte, nc_get_var_uchar, nc_get_var_uint, nc_get_var_ulonglong, nc_get_var_ushort
    '               nc_get_var1_double, nc_get_var1_float, nc_get_var1_int, nc_get_var1_long, nc_get_var1_longlong
    '               nc_get_var1_schar, nc_get_var1_short, nc_get_var1_string, nc_get_var1_text, nc_get_var1_ubyte
    '               nc_get_var1_uchar, nc_get_var1_uint, nc_get_var1_ulonglong, nc_get_var1_ushort, nc_get_vara_double
    '               nc_get_vara_float, nc_get_vara_int, nc_get_vara_long, nc_get_vara_longlong, nc_get_vara_schar
    '               nc_get_vara_short, nc_get_vara_string, nc_get_vara_text, nc_get_vara_ubyte, nc_get_vara_uchar
    '               nc_get_vara_uint, nc_get_vara_ulonglong, nc_get_vara_ushort, nc_get_vars_double, nc_get_vars_float
    '               nc_get_vars_int, nc_get_vars_long, nc_get_vars_longlong, nc_get_vars_schar, nc_get_vars_short
    '               nc_get_vars_string, nc_get_vars_text, nc_get_vars_uchar, nc_get_vars_uint, nc_get_vars_ulonglong
    '               nc_get_vars_ushort, nc_get_vlen_element, nc_inq, (+2 Overloads) nc_inq_att, nc_inq_attid
    '               (+2 Overloads) nc_inq_attlen, nc_inq_attname, nc_inq_atttype, nc_inq_compound, nc_inq_compound_field
    '               nc_inq_compound_fielddim_sizes, nc_inq_compound_fieldindex, nc_inq_compound_fieldname, nc_inq_compound_fieldndims, nc_inq_compound_fieldoffset
    '               nc_inq_compound_fieldtype, nc_inq_compound_name, nc_inq_compound_nfields, nc_inq_compound_size, nc_inq_dim
    '               nc_inq_dimid, nc_inq_dimids, nc_inq_dimlen, nc_inq_dimname, nc_inq_enum
    '               nc_inq_enum_ident, nc_inq_enum_member, nc_inq_format, nc_inq_format_extended, nc_inq_grp_full_ncid
    '               nc_inq_grp_ncid, nc_inq_grp_parent, nc_inq_grpname, nc_inq_grpname_full, nc_inq_grpname_len
    '               nc_inq_grps, nc_inq_libvers, nc_inq_natts, nc_inq_ncid, nc_inq_ndims
    '               nc_inq_nvars, nc_inq_path, (+2 Overloads) nc_inq_type, nc_inq_type_equal, nc_inq_typeid
    '               (+2 Overloads) nc_inq_typeids, nc_inq_unlimdim, nc_inq_unlimdims, nc_inq_user_type, nc_inq_var
    '               nc_inq_var_chunking, nc_inq_var_deflate, nc_inq_var_endian, nc_inq_var_fill, nc_inq_var_filter
    '               nc_inq_var_fletcher32, nc_inq_var_szip, nc_inq_vardimid, nc_inq_varid, nc_inq_varids
    '               nc_inq_varname, nc_inq_varnatts, nc_inq_varndims, nc_inq_vartype, nc_inq_vlen
    '               nc_insert_array_compound, nc_insert_compound, nc_insert_enum, nc_open, nc_put_att_double
    '               nc_put_att_float, nc_put_att_int, nc_put_att_long, nc_put_att_longlong, nc_put_att_schar
    '               nc_put_att_short, nc_put_att_text, nc_put_att_ubyte, nc_put_att_uchar, nc_put_att_uint
    '               nc_put_att_ulonglong, nc_put_att_ushort, nc_put_var_double, nc_put_var_float, nc_put_var_int
    '               nc_put_var_long, nc_put_var_longlong, nc_put_var_schar, nc_put_var_short, nc_put_var_string
    '               nc_put_var_text, nc_put_var_ubyte, nc_put_var_uchar, nc_put_var_uint, nc_put_var_ulonglong
    '               nc_put_var_ushort, nc_put_var1_double, nc_put_var1_float, nc_put_var1_int, nc_put_var1_long
    '               nc_put_var1_longlong, nc_put_var1_schar, nc_put_var1_short, nc_put_var1_string, nc_put_var1_text
    '               nc_put_var1_ubyte, nc_put_var1_uchar, nc_put_var1_uint, nc_put_var1_ulonglong, nc_put_var1_ushort
    '               nc_put_vara_double, nc_put_vara_float, nc_put_vara_int, nc_put_vara_long, nc_put_vara_longlong
    '               nc_put_vara_schar, nc_put_vara_short, nc_put_vara_string, nc_put_vara_text, nc_put_vara_ubyte
    '               nc_put_vara_uchar, nc_put_vara_uint, nc_put_vara_ulonglong, nc_put_vara_ushort, nc_put_vars_double
    '               nc_put_vars_float, nc_put_vars_int, nc_put_vars_long, nc_put_vars_longlong, nc_put_vars_schar
    '               nc_put_vars_short, nc_put_vars_string, nc_put_vars_text, nc_put_vars_uchar, nc_put_vars_uint
    '               nc_put_vars_ulonglong, nc_put_vars_ushort, nc_put_vlen_element, nc_redef, nc_rename_att
    '               nc_rename_dim, nc_rename_grp, nc_rename_var, nc_set_chunk_cache, nc_set_default_format
    '               nc_set_fill, nc_set_var_chunk_cache, nc_set_var_szip, nc_show_metadata, nc_strerror
    '               nc_sync
    '     Class ConstCharPtrMarshaler
    ' 
    '         Function: GetInstance, GetNativeDataSize, MarshalManagedToNative, MarshalNativeToManaged
    ' 
    '         Sub: CleanUpManagedData, CleanUpNativeData
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

' 
'  A C# interface to the UniData NetCDF dll.
'  Nick Humphries   April 2021
'      
'  This C# interface supports the functions provided by the Unidata netcdf.dll (https://www.unidata.ucar.edu/software/netcdf/) 
'  currently up to 4.8.0 (2021-03-31), although not all functions are supported (e.g. I have omitted the deprecated *varm* functions)
'  
'  netCDF: doi:10.5065/D6H70CW6 https://doi.org/10.5065/D6H70CW6
'  
'  This file supports both x86 and x64 versions of the dlls, by defining the index[] start[] and count[] arrays for get_vara, get_var1 and get_vars methods as IntPtr.
'  Wrappers for these methods have been provided so that these arrays can always be defined in the calling program as int[].
'  Note that this also applies to lengths, such as returned from nc_inq_dimlen, where again the lengths are defined as IntPtr but wrappers exists to allow int to be used.
'  
'  A collection of C# friendly methods have been provided to simplify calls to functions returning string variables and also for 
'  getting and putting attributes. Thanks to https://stackoverflow.com/questions/6300093/why-cant-i-return-a-char-string-from-c-to-c-sharp-in-a-release-build 
'  (https://stackoverflow.com/users/1164966/benoit-blanchon) for the custom marshaller.
' 
'  Also, I have provided a couple of examples of how to get or put multidimensional arrays without reformatting. 
'  Any of the functions can be copied and modified to provide direct access to multidimensional arrays simply by changing the method from: 
'  
'           nc_put_var_float(int ncid, int varid, float[] op);
'  
'           to nc_put_var_float(int ncid, int varid, float[,] op);
'           or nc_put_var_float(int ncid, int varid, float[,,] op);
'                 
'  Data types
'  Some of the data types supported by the netCDF dll do not map exactly to C# data types
'  The following netCDF data types are defined:
'              
'       NC_BYTE     C# sbyte
'       NC_CHAR     C# byte
'       NC_SHORT    C# short
'       NC_INT      C# int
'       NC_FLOAT    C# float
'       NC_DOUBLE   C# double
'       NC_UBYTE    C# byte
'       NC_USHORT   C# ushort
'       NC_UINT     C# uint
'       NC_INT64    C# long
'       NC_UINT64   C# ulong
'       NC_STRING   C# string
'  
'  Additionally, the following *_var* functions do not have an exact netCDF data type
'  
'       text
'       schar   
'       uchar
'       
'  NC_CHAR and NC_BYTE do not have an exact set of *_var* functions
'   
' 

Imports System.Runtime.InteropServices
Imports System.Text
Imports nc_type = Microsoft.VisualBasic.DataStorage.netCDF.Data.CDFDataTypes

Partial Public Module NetCDF
#Region "Constants"
    ''' <summary>'size' argument to ncdimdef for an unlimited dimension</summary>
    Public Const NC_UNLIMITED As Integer = 0
    ''' <summary>attribute id to put/get a global attribute</summary>
    Public Const NC_GLOBAL As Integer = -1

    ' * In HDF5 files you can set the endianness of variables with nc_def_var_endian(). This define is used there. 

    Public Const NC_ENDIAN_NATIVE As Integer = 0
    Public Const NC_ENDIAN_LITTLE As Integer = 1
    Public Const NC_ENDIAN_BIG As Integer = 2

    ' * In HDF5 files you can set storage for each variable to be either contiguous or chunked, with nc_def_var_chunking().  This define is
    '  used there. 

    Public Const NC_CHUNKED As Integer = 0
    Public Const NC_CONTIGUOUS As Integer = 1

    ' In HDF5 files you can set check-summing for each variable. Currently the only checksum available is Fletcher-32, which can be set
    ' with the function nc_def_var_fletcher32.  These defines are used there.
    ' 
    Public Const NC_NOCHECKSUM As Integer = 0
    Public Const NC_FLETCHER32 As Integer = 1

    ' Control the HDF5 shuffle filter. In HDF5 files you can specify that a shuffle filter should be used on each chunk of a variable to
    '  improve compression for that variable. This per-variable shuffle property can be set with the function nc_def_var_deflate().
    ' 
    Public Const NC_NOSHUFFLE As Integer = 0
    Public Const NC_SHUFFLE As Integer = 1

    ' Control the compression
    ' 
    Public Const NC_NODEFLATE As Integer = 0
    Public Const NC_DEFLATE As Integer = 1

    ''' <summary>Minimum deflate level.</summary>
    Public Const NC_MIN_DEFLATE_LEVEL As Integer = 0
    ''' <summary>Maximum deflate level.</summary>
    Public Const NC_MAX_DEFLATE_LEVEL As Integer = 9

    ' Format specifier for nc_set_default_format() and returned by nc_inq_format. This returns the format as provided by
    '   the API. See nc_inq_format_extended to see the true file format. Starting with version 3.6, there are different format netCDF files.
    '   4.0 introduces the third one. \see netcdf_format
    ' 
    Public Const NC_FORMAT_CLASSIC As Integer = 1
    ' After adding CDF5 support, the NC_FORMAT_64BIT flag is somewhat confusing. So, it is renamed.
    ' Note that the name in the contributed code NC_FORMAT_64BIT was renamed to NC_FORMAT_CDF2
    ' 
    Public Const NC_FORMAT_64BIT_OFFSET As Integer = 2
    ''' <summary>deprecated Saved for compatibility.  Use NC_FORMAT_64BIT_OFFSET or NC_FORMAT_64BIT_DATA, from netCDF 4.4.0 onwards</summary>
    Public Const NC_FORMAT_64BIT As Integer = NC_FORMAT_64BIT_OFFSET
    Public Const NC_FORMAT_NETCDF4 As Integer = 3
    Public Const NC_FORMAT_NETCDF4_CLASSIC As Integer = 4
    Public Const NC_FORMAT_64BIT_DATA As Integer = 5

    ' Alias 
    Public Const NC_FORMAT_CDF5 As Integer = NC_FORMAT_64BIT_DATA

#End Region

#Region "Methods returning const char * that require the custom Marshaller"
    '
    ' Methods returning const char * require the custom Marshaller
    '
    ''' <summary>
    ''' Return the library version string
    ''' </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_libvers() As <MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef:=GetType(ConstCharPtrMarshaler))> String
    End Function

    ''' <summary>
    ''' Return the error message
    ''' </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_strerror(ncerr As Integer) As <MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef:=GetType(ConstCharPtrMarshaler))> String
    End Function

#Region "CustomMarshaller"
    ' From https://stackoverflow.com/questions/6300093/why-cant-i-return-a-char-string-from-c-to-c-sharp-in-a-release-build
    Friend Class ConstCharPtrMarshaler
        Implements ICustomMarshaler
        Public Function MarshalNativeToManaged(pNativeData As IntPtr) As Object Implements ICustomMarshaler.MarshalNativeToManaged
            Return Marshal.PtrToStringAnsi(pNativeData)
        End Function
        Public Function MarshalManagedToNative(ManagedObj As Object) As IntPtr Implements ICustomMarshaler.MarshalManagedToNative
            Return IntPtr.Zero
        End Function
        Public Sub CleanUpNativeData(pNativeData As IntPtr) Implements ICustomMarshaler.CleanUpNativeData
        End Sub
        Public Sub CleanUpManagedData(ManagedObj As Object) Implements ICustomMarshaler.CleanUpManagedData
        End Sub
        Public Function GetNativeDataSize() As Integer Implements ICustomMarshaler.GetNativeDataSize
            Return IntPtr.Size
        End Function
        Private Shared ReadOnly instance As ConstCharPtrMarshaler = New ConstCharPtrMarshaler()
        Public Shared Function GetInstance(cookie As String) As ICustomMarshaler
            Return instance
        End Function
    End Class
#End Region
#End Region

#Region "File and Data IO"
    '
    '  Some funtions are omitted here:
    '  nc_close_memio
    '  nc_create_par
    '  nc_create_par_fortran
    '  nc_def_user_format
    '  nc_int_user_format
    '  nc_open_mem
    '  nc_open_memio
    '  nc_open_par
    '  nc_open_par_fortran
    '  nc_var_par_access
    '
    ''' <summary>Provided fpr completeness - No longer necessary for user to invoke manually.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_abort(ncid As Integer) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_close(ncid As Integer) As Integer
    End Function

    ''' <summary>Create a new netCDF file.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_create(path As String, mode As CreateMode, <Out> ByRef ncidp As Integer) As Integer
    End Function

    ''' <summary>Create a netCDF file with the contents stored in memory.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_create_mem(path As String, mode As CreateMode, initialsize As Integer, <Out> ByRef ncidp As Integer) As Integer
    End Function

    ''' <summary>Leave define mode</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_enddef(ncidp As Integer) As Integer
    End Function

    ''' <summary>Inquire about a file or group.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq(ncid As Integer, <Out> ByRef ndims As Integer, <Out> ByRef nvars As Integer, <Out> ByRef ngatts As Integer, <Out> ByRef unlimdimid As Integer) As Integer
    End Function

    ''' <summary>Inquire about the binary format of a netCDF file as presented by the API.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_format(ncid As Integer, <Out> ByRef format As Integer) As Integer
    End Function

    ''' <summary>Obtain more detailed (vis-a-vis nc_inq_format) format information about an open dataset.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_format_extended(ncid As Integer, <Out> ByRef format As Integer, <Out> ByRef mode As Integer) As Integer
    End Function

    ''' <summary>Learn the path used to open/create the file. 
    ''' Use nc_inq_path(ncid) instead, otherwise a correctly sized StringBuilder is required</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_path(ncid As Integer, <Out> ByRef pathlen As IntPtr, path As StringBuilder) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_type(ncid As Integer, <Out> ByRef type As nc_type, name As StringBuilder, <Out> ByRef size As Integer) As Integer
    End Function

    ''' <summary>Open an existing netCDF file.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_open(path As String, mode As OpenMode, <Out> ByRef ncidp As Integer) As Integer
    End Function

    ''' <summary>Put open netcdf dataset into define mode</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_redef(ncid As Integer) As Integer
    End Function

    ''' <summary>Set the fill mode (classic or 64-bit offset files only).</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_set_fill(ncid As Integer, fillmode As Integer, <Out> ByRef old_modep As Integer) As Integer
    End Function

    ''' <summary>Synchronize an open netcdf dataset to disk</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_sync(ncid As Integer) As Integer
    End Function
#End Region

#Region "Dimensions"
    '
    ' Dimensions
    '
    ''' <summary>Define a new dimension.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_def_dim(ncid As Integer, name As String, len As IntPtr, <Out> ByRef dimidp As Integer) As Integer
    End Function

    ''' <summary>Find the name and length of a dimension.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_dim(ncid As Integer, dimid As Integer, name As StringBuilder, <Out> ByRef len As IntPtr) As Integer
    End Function

    ''' <summary>Find the ID of a dimension from the name.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_dimid(ncid As Integer, name As String, <Out> ByRef dimid As Integer) As Integer
    End Function

    ''' <summary>Find the length of a dimension.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_dimlen(ncid As Integer, dimid As Integer, <Out> ByRef len As IntPtr) As Integer
    End Function

    ''' <summary>Find out the name of a dimension.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_dimname(ncid As Integer, dimid As Integer, name As StringBuilder) As Integer
    End Function

    ''' <summary>Find the number of dimensions.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_ndims(ncid As Integer, <Out> ByRef ndims As Integer) As Integer
    End Function

    ''' <summary>Find the ID of the unlimited dimension.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_unlimdim(ncid As Integer, <Out> ByRef unlimdimid As Integer) As Integer
    End Function

    ''' <summary>Find the ID of the unlimited dimension.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_unlimdims(ncid As Integer, nunlimdimsp As Integer(), unlimdimidsp As Integer()) As Integer
    End Function

    ''' <summary>Rename a dimension.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_rename_dim(ncid As Integer, dimid As Integer, name As String, <Out> ByRef status As Integer) As Integer
    End Function
#End Region

#Region "Defining Variables"
    '
    ' Defining Variables
    ' Learning about Variables
    '
    ''' <summary>Define a variable</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_def_var(ncid As Integer, name As String, xtype As nc_type, ndims As Integer, dimids As Integer(), <Out> ByRef varidp As Integer) As Integer
    End Function

    ''' <summary>Define fill value behavior for a variable. This must be done after nc_def_var</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_def_var_fill(ncid As Integer, varid As Integer, no_fill As Integer, fill_value As Integer) As Integer
    End Function

    ''' <summary>Set compression settings for a variable. Lower is faster, higher is better.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_def_var_deflate(ncid As Integer, varid As Integer, shuffle As Integer, deflate As Integer, deflate_level As Integer) As Integer
    End Function

    ''' <summary>Set fletcher32 checksum for a var. This must be done after nc_def_var</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_def_var_fletcher32(ncid As Integer, varid As Integer, fletcher32 As Integer) As Integer
    End Function

    ''' <summary>Define chunking for a variable. This must be done after nc_def_var</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_def_var_chunking(ncid As Integer, varid As Integer, storage As Integer, <Out> ByRef chunksizesp As Integer) As Integer
    End Function

    ''' <summary>Define endianness of a variable.
    ''' NC_ENDIAN_NATIVE to select the native endianness of the platform (the default), NC_ENDIAN_LITTLE to use little-endian, NC_ENDIAN_BIG to use big-endian
    ''' </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_def_var_endian(ncid As Integer, varid As Integer, endian As Integer) As Integer
    End Function

    ''' <summary>Define a filter for a variable</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_def_var_filter(ncid As Integer, varid As Integer, id As UInteger, nparams As Integer, <Out> ByRef parms As UInteger) As Integer
    End Function

    ''' <summary>Set szip compression settings on a variable.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_set_var_szip(ncid As Integer, varid As Integer, options_maskp As Integer, pixels_per_blockp As Integer) As Integer
    End Function

    ''' <summary>Rename a variable.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_rename_var(ncid As Integer, varid As Integer, name As String) As Integer
    End Function

    ''' <summary>Use this function to free resources associated with NC_STRING data.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_free_string(len As IntPtr, data As IntPtr()) As Integer
    End Function

    ''' <summary>Set the per-variable cache size, nelems, and preemption policy. </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_set_var_chunk_cache(ncid As Integer, varid As Integer, size As Integer, nelems As Integer, preemption As Single) As Integer
    End Function

    ''' <summary>Get the per-variable cache size, nelems, and preemption policy.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var_chunk_cache(ncid As Integer, varid As Integer, <Out> ByRef sizep As Integer, <Out> ByRef nelemsp As Integer, <Out> ByRef preemptionp As Single) As Integer
    End Function
#End Region

#Region "Reading Data from Variables (x86 and x64 versions)"
#Region "nc_get_var*"
    '
    ' Reading values from variables
    '  Note that the generic functions have been omitted:
    '  nc_get_var
    '  nc_get_vara
    '  nc_get_vars
    '  and all deprecated nc_get_varm funcrions
    '
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var_text(ncid As Integer, varid As Integer, ip As Byte()) As Integer
    End Function
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var_schar(ncid As Integer, varid As Integer, ip As SByte()) As Integer
    End Function
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var_uchar(ncid As Integer, varid As Integer, ip As Byte()) As Integer
    End Function
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var_short(ncid As Integer, varid As Integer, ip As Short()) As Integer
    End Function
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var_int(ncid As Integer, varid As Integer, ip As Integer()) As Integer
    End Function
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var_long(ncid As Integer, varid As Integer, ip As Long()) As Integer
    End Function
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var_float(ncid As Integer, varid As Integer, ip As Single()) As Integer
    End Function
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var_double(ncid As Integer, varid As Integer, ip As Double()) As Integer
    End Function
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var_ubyte(ncid As Integer, varid As Integer, ip As Byte()) As Integer
    End Function
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var_ushort(ncid As Integer, varid As Integer, ip As UShort()) As Integer
    End Function
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var_uint(ncid As Integer, varid As Integer, ip As UInteger()) As Integer
    End Function
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var_longlong(ncid As Integer, varid As Integer, ip As Long()) As Integer
    End Function
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var_ulonglong(ncid As Integer, varid As Integer, ip As ULong()) As Integer
    End Function
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var_string(ncid As Integer, varid As Integer, ip As IntPtr()) As Integer
    End Function
#End Region

#Region "get_var1"
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var1_text(ncid As Integer, varid As Integer, index As IntPtr(), <Out> ByRef ip As Byte) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var1_schar(ncid As Integer, varid As Integer, index As IntPtr(), <Out> ByRef ip As SByte) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var1_uchar(ncid As Integer, varid As Integer, index As IntPtr(), <Out> ByRef ip As Byte) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var1_short(ncid As Integer, varid As Integer, index As IntPtr(), <Out> ByRef ip As Short) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var1_int(ncid As Integer, varid As Integer, index As IntPtr(), <Out> ByRef ip As Integer) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var1_long(ncid As Integer, varid As Integer, index As IntPtr(), <Out> ByRef ip As Long) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var1_float(ncid As Integer, varid As Integer, index As IntPtr(), <Out> ByRef ip As Single) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var1_double(ncid As Integer, varid As Integer, index As IntPtr(), <Out> ByRef ip As Double) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var1_ubyte(ncid As Integer, varid As Integer, index As IntPtr(), <Out> ByRef ip As Byte) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var1_ushort(ncid As Integer, varid As Integer, index As IntPtr(), <Out> ByRef ip As UShort) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var1_uint(ncid As Integer, varid As Integer, index As IntPtr(), <Out> ByRef ip As UInteger) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var1_longlong(ncid As Integer, varid As Integer, index As IntPtr(), <Out> ByRef ip As Long) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var1_ulonglong(ncid As Integer, varid As Integer, index As IntPtr(), <Out> ByRef ip As ULong) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_var1_string(ncid As Integer, varid As Integer, index As IntPtr(), ip As IntPtr()) As Integer
    End Function
#End Region

#Region "get_vara"
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vara_text(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), ip As Byte()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vara_schar(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), ip As SByte()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vara_uchar(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), ip As Byte()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vara_short(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), ip As Short()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vara_int(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), ip As Integer()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vara_long(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), ip As Long()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vara_float(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), ip As Single()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vara_double(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), ip As Double()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vara_ubyte(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), ip As Byte()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vara_ushort(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), ip As UShort()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vara_uint(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), ip As UInteger()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vara_longlong(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), ip As Long()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vara_ulonglong(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), ip As ULong()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vara_string(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), ip As IntPtr()) As Integer
    End Function
#End Region

#Region "get_vars"
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vars_text(ncid As Integer, varid As Integer, startp As IntPtr(), countp As IntPtr(), stridep As IntPtr(), ip As Byte()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vars_uchar(ncid As Integer, varid As Integer, startp As IntPtr(), countp As IntPtr(), stridep As IntPtr(), ip As Byte()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vars_schar(ncid As Integer, varid As Integer, startp As IntPtr(), countp As IntPtr(), stridep As IntPtr(), ip As SByte()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vars_short(ncid As Integer, varid As Integer, startp As IntPtr(), countp As IntPtr(), stridep As IntPtr(), ip As Short()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vars_int(ncid As Integer, varid As Integer, startp As IntPtr(), countp As IntPtr(), stridep As IntPtr(), ip As Integer()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vars_long(ncid As Integer, varid As Integer, startp As IntPtr(), countp As IntPtr(), stridep As IntPtr(), ip As Long()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vars_float(ncid As Integer, varid As Integer, startp As IntPtr(), countp As IntPtr(), stridep As IntPtr(), ip As Single()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vars_double(ncid As Integer, varid As Integer, startp As IntPtr(), countp As IntPtr(), stridep As IntPtr(), ip As Double()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vars_ushort(ncid As Integer, varid As Integer, startp As IntPtr(), countp As IntPtr(), stridep As IntPtr(), ip As UShort()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vars_uint(ncid As Integer, varid As Integer, startp As IntPtr(), countp As IntPtr(), stridep As IntPtr(), ip As UInteger()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vars_longlong(ncid As Integer, varid As Integer, startp As IntPtr(), countp As IntPtr(), stridep As IntPtr(), ip As Long()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vars_ulonglong(ncid As Integer, varid As Integer, startp As IntPtr(), countp As IntPtr(), stridep As IntPtr(), ip As ULong()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vars_string(ncid As Integer, varid As Integer, startp As IntPtr(), countp As IntPtr(), stridep As IntPtr(), ip As IntPtr()) As Integer
    End Function
#End Region

#End Region

#Region "Learning about Variables"
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_varid(ncid As Integer, name As String, <Out> ByRef varidp As Integer) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_var(ncid As Integer, varid As Integer, name As String, <Out> ByRef type As nc_type, <Out> ByRef ndims As Integer, dimids As Integer(), <Out> ByRef natts As Integer) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_varname(ncid As Integer, varid As Integer, name As StringBuilder) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_vartype(ncid As Integer, varid As Integer, <Out> ByRef xtypep As nc_type) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_varndims(ncid As Integer, varid As Integer, <Out> ByRef ndims As Integer) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_vardimid(ncid As Integer, varid As Integer, dimids As Integer()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_varnatts(ncid As Integer, varid As Integer, <Out> ByRef nattsp As Integer) As Integer
    End Function

    ''' <summary>Find out compression settings of a var.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_var_deflate(ncid As Integer, varid As Integer, <Out> ByRef shufflep As Integer, <Out> ByRef deflatep As Integer, <Out> ByRef deflate_levelp As Integer) As Integer
    End Function

    ''' <summary>Inquire about fletcher32 checksum for a var.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_var_fletcher32(ncid As Integer, varid As Integer, <Out> ByRef fletcher32p As Integer) As Integer
    End Function

    ''' <summary>Inq chunking stuff for a var.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_var_chunking(ncid As Integer, varid As Integer, <Out> ByRef storagep As Integer, <Out> ByRef chunksizesp As Integer) As Integer
    End Function

    ''' <summary>Inq fill value setting for a var.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_var_fill(ncid As Integer, varid As Integer, <Out> ByRef no_fill As Integer, <Out> ByRef fill_valuep As Integer) As Integer
    End Function

    ''' <summary>Learn about the endianness of a variable.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_var_endian(ncid As Integer, varid As Integer, <Out> ByRef endianp As Integer) As Integer
    End Function

    ''' <summary>Find out szip settings of a var.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_var_szip(ncid As Integer, varid As Integer, <Out> ByRef options_maskp As Integer, <Out> ByRef pixels_per_blockp As Integer) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_nvars(ncid As Integer, <Out> ByRef nvars As Integer) As Integer
    End Function

    ''' <summary>Learn about the filter on a variable</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_var_filter(ncid As Integer, varid As Integer, <Out> ByRef idp As UInteger, <Out> ByRef nparams As Integer, <Out> ByRef parms As UInteger) As Integer
    End Function
#End Region

#Region "Writing variables"
#Region "nc_put_var"
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var_text(ncid As Integer, varid As Integer, op As Byte()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var_schar(ncid As Integer, varid As Integer, op As SByte()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var_uchar(ncid As Integer, varid As Integer, op As Byte()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var_short(ncid As Integer, varid As Integer, op As Short()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var_int(ncid As Integer, varid As Integer, op As Integer()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var_long(ncid As Integer, varid As Integer, op As Long()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var_float(ncid As Integer, varid As Integer, op As Single()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var_double(ncid As Integer, varid As Integer, op As Double()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var_ubyte(ncid As Integer, varid As Integer, op As Byte()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var_ushort(ncid As Integer, varid As Integer, op As UShort()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var_uint(ncid As Integer, varid As Integer, op As UInteger()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var_longlong(ncid As Integer, varid As Integer, op As Long()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var_ulonglong(ncid As Integer, varid As Integer, op As ULong()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var_string(ncid As Integer, varid As Integer, op As String()) As Integer
    End Function
#End Region

#Region "put_var1"
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var1_text(ncid As Integer, varid As Integer, index As IntPtr(), op As Byte) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var1_schar(ncid As Integer, varid As Integer, index As IntPtr(), op As SByte) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var1_uchar(ncid As Integer, varid As Integer, index As IntPtr(), op As Byte) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var1_short(ncid As Integer, varid As Integer, index As IntPtr(), op As Short) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var1_int(ncid As Integer, varid As Integer, index As IntPtr(), op As Integer) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var1_long(ncid As Integer, varid As Integer, index As IntPtr(), op As Long) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var1_float(ncid As Integer, varid As Integer, index As IntPtr(), op As Single) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var1_double(ncid As Integer, varid As Integer, index As IntPtr(), op As Double) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var1_ubyte(ncid As Integer, varid As Integer, index As IntPtr(), op As Byte) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var1_ushort(ncid As Integer, varid As Integer, index As IntPtr(), op As UShort) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var1_uint(ncid As Integer, varid As Integer, index As IntPtr(), op As UInteger) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var1_longlong(ncid As Integer, varid As Integer, index As IntPtr(), op As Long) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var1_ulonglong(ncid As Integer, varid As Integer, index As IntPtr(), op As ULong) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_var1_string(ncid As Integer, varid As Integer, index As IntPtr(), op As String) As Integer
    End Function
#End Region

#Region "put_vara"
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vara_text(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), op As Byte()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vara_schar(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), op As SByte()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vara_uchar(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), op As Byte()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vara_short(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), op As Short()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vara_int(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), op As Integer()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vara_long(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), op As Long()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vara_float(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), op As Single()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vara_double(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), op As Double()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vara_ubyte(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), op As Byte()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vara_ushort(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), op As UShort()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vara_uint(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), op As UInteger()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vara_longlong(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), op As Long()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vara_ulonglong(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), op As ULong()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vara_string(ncid As Integer, varid As Integer, start As IntPtr(), count As IntPtr(), op As String()) As Integer
    End Function
#End Region

#Region "put_vars"
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vars_text(ncid As Integer, varid As Integer, startp As IntPtr(), countp As IntPtr(), stridep As IntPtr(), op As Byte()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vars_uchar(ncid As Integer, varid As Integer, startp As IntPtr(), countp As IntPtr(), stridep As IntPtr(), op As Byte()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vars_schar(ncid As Integer, varid As Integer, startp As IntPtr(), countp As IntPtr(), stridep As IntPtr(), op As SByte()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vars_short(ncid As Integer, varid As Integer, startp As IntPtr(), countp As IntPtr(), stridep As IntPtr(), op As Short()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vars_int(ncid As Integer, varid As Integer, startp As IntPtr(), countp As IntPtr(), stridep As IntPtr(), op As Integer()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vars_long(ncid As Integer, varid As Integer, startp As IntPtr(), countp As IntPtr(), stridep As IntPtr(), op As Long()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vars_float(ncid As Integer, varid As Integer, startp As IntPtr(), countp As IntPtr(), stridep As IntPtr(), op As Single()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vars_double(ncid As Integer, varid As Integer, startp As IntPtr(), countp As IntPtr(), stridep As IntPtr(), op As Double()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vars_ushort(ncid As Integer, varid As Integer, startp As IntPtr(), countp As IntPtr(), stridep As IntPtr(), op As UShort()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vars_uint(ncid As Integer, varid As Integer, startp As IntPtr(), countp As IntPtr(), stridep As IntPtr(), op As UInteger()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vars_longlong(ncid As Integer, varid As Integer, startp As IntPtr(), countp As IntPtr(), stridep As IntPtr(), op As Long()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vars_ulonglong(ncid As Integer, varid As Integer, startp As IntPtr(), countp As IntPtr(), stridep As IntPtr(), op As ULong()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vars_string(ncid As Integer, varid As Integer, startp As IntPtr(), countp As IntPtr(), stridep As IntPtr(), op As String) As Integer
    End Function
#End Region

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_copy_var(ncid_in As Integer, varid As Integer, ncid_out As Integer) As Integer
    End Function
#End Region

#Region "Attributes "
#Region "Learning about Attributes"
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_att(ncid As Integer, varid As Integer, name As String, <Out> ByRef xtypep As nc_type, <Out> ByRef lenp As IntPtr) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_attid(ncid As Integer, varid As Integer, name As String, <Out> ByRef idp As Integer) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_attname(ncid As Integer, varid As Integer, attnum As Integer, name As StringBuilder) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_natts(ncid As Integer, <Out> ByRef ngatts As Integer) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_atttype(ncid As Integer, varid As Integer, name As String, <Out> ByRef xtypep As nc_type) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_attlen(ncid As Integer, varid As Integer, name As String, <Out> ByRef lenp As IntPtr) As Integer
    End Function

#Region "x64"
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_att(ncid As Integer, varid As Integer, name As String, <Out> ByRef xtypep As nc_type, <Out> ByRef lenp As Long) As Integer
    End Function
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_attlen(ncid As Integer, varid As Integer, name As String, <Out> ByRef lenp As Long) As Integer
    End Function
#End Region
#End Region

#Region "Getting Attributes"
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_att_text(ncid As Integer, varid As Integer, name As String, value As StringBuilder) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_att_schar(ncid As Integer, varid As Integer, name As String, value As SByte()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_att_uchar(ncid As Integer, varid As Integer, name As String, value As Byte()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_att_short(ncid As Integer, varid As Integer, name As String, value As Short()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_att_int(ncid As Integer, varid As Integer, name As String, value As Integer()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_att_long(ncid As Integer, varid As Integer, name As String, value As Long()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_att_float(ncid As Integer, varid As Integer, name As String, value As Single()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_att_double(ncid As Integer, varid As Integer, name As String, value As Double()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_att_ubyte(ncid As Integer, varid As Integer, name As String, value As Byte()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_att_ushort(ncid As Integer, varid As Integer, name As String, value As UShort()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_att_uint(ncid As Integer, varid As Integer, name As String, value As UInteger()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_att_longlong(ncid As Integer, varid As Integer, name As String, value As Long()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_att_ulonglong(ncid As Integer, varid As Integer, name As String, value As ULong()) As Integer
    End Function
#End Region

#Region "Writing Attributes"
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_att_text(ncid As Integer, varid As Integer, name As String, len As IntPtr, value As String) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_att_schar(ncid As Integer, varid As Integer, name As String, type As nc_type, len As IntPtr, value As SByte()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_att_uchar(ncid As Integer, varid As Integer, name As String, type As nc_type, len As IntPtr, value As Byte()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_att_short(ncid As Integer, varid As Integer, name As String, type As nc_type, len As IntPtr, value As Short()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_att_int(ncid As Integer, varid As Integer, name As String, type As nc_type, len As IntPtr, value As Integer()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_att_long(ncid As Integer, varid As Integer, name As String, type As nc_type, len As IntPtr, value As Long()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_att_float(ncid As Integer, varid As Integer, name As String, type As nc_type, len As IntPtr, value As Single()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_att_double(ncid As Integer, varid As Integer, name As String, type As nc_type, len As IntPtr, value As Double()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_att_ubyte(ncid As Integer, varid As Integer, name As String, type As nc_type, len As IntPtr, value As Byte()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_att_ushort(ncid As Integer, varid As Integer, name As String, type As nc_type, len As IntPtr, value As UShort()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_att_uint(ncid As Integer, varid As Integer, name As String, type As nc_type, len As IntPtr, value As UInteger()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_att_longlong(ncid As Integer, varid As Integer, name As String, type As nc_type, len As IntPtr, value As Long()) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_att_ulonglong(ncid As Integer, varid As Integer, name As String, type As nc_type, len As IntPtr, value As ULong()) As Integer
    End Function
#End Region

#Region "Copying, Deleting and Renaming Attributes"
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_copy_att(ncid_in As Integer, varid_in As Integer, name As String, ncid_out As Integer, varid_out As Integer) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_del_att(ncid_in As Integer, varid As Integer, name As String) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_rename_att(ncid As Integer, varid As Integer, name As String, newname As String) As Integer
    End Function
#End Region
#End Region

#Region "Groups"
    ''' <summary>Define a new group.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_def_grp(ncid As Integer, name As String, <Out> ByRef grp_ncid As Integer) As Integer
    End Function

    ''' <summary>Retrieve a list of dimension ids associated with a group</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_dimids(ncid As Integer, <Out> ByRef ndims As Integer, dimids As Integer(), include_parents As Integer) As Integer
    End Function

    ''' <summary>Given a full name and ncid, find group ncid.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_grp_full_ncid(ncid As Integer, full_name As String, <Out> ByRef grp_ncid As Integer) As Integer
    End Function

    ''' <summary>Given a name and parent ncid, find group ncid.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_grp_ncid(ncid As Integer, grp_name As String, <Out> ByRef grp_ncid As Integer) As Integer
    End Function

    ''' <summary>Given an ncid, find the ncid of its parent group.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_grp_parent(ncid As Integer, <Out> ByRef parent_ncid As Integer) As Integer
    End Function

    ''' <summary>Given locid, find name of group. (Root group is named "/".) </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_grpname(ncid As Integer, name As StringBuilder) As Integer
    End Function

    ''' <summary>
    ''' Given ncid, find full name and len of full name. (Root group is named "/", with length 1.) 
    ''' But use the C# friendlier nc_inq_grpname_full(ncid) instead
    ''' </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_grpname_full(ncid As Integer, <Out> ByRef lenp As IntPtr, full_name As StringBuilder) As Integer
    End Function

    ''' <summary>Given ncid, find len of full name. </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_grpname_len(ncid As Integer, <Out> ByRef lenp As IntPtr) As Integer
    End Function

    ''' <summary>Given a location id, return the number of groups it contains, and an array of their locids.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_grps(ncid As Integer, <Out> ByRef numgrps As Integer, <Out> ByRef ncids As Integer) As Integer
    End Function

    ''' <summary>Given an ncid and group name (NULL gets root group), return locid. </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_ncid(ncid As Integer, name As String, <Out> ByRef grp_ncid As Integer) As Integer
    End Function

    ''' <summary>Retrieve a list of types associated with a group.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_typeids(ncid As Integer, <Out> ByRef ntypes As Integer, typeids As Integer()) As Integer
    End Function

    ''' <summary>Get a list of varids associated with a group given a group ID.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_varids(ncid As Integer, <Out> ByRef nvars As Integer, varids As Integer()) As Integer
    End Function

    ''' <summary>Rename a group.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_rename_grp(ncid As Integer, name As String) As Integer
    End Function

    ''' <summary>Print the metadata for a file.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_show_metadata(ncid As Integer) As Integer
    End Function
#End Region


    ' NOTE User defined, Compound, Enum and VLen functions have not yet been tested
    '  and the functions required for VLen are incomplete. e.g. the VLen struct is not defined here
    '  There is also a macro defined for VLen, which we do not have : #define NC_COMPOUND_OFFSET(S,M)    (offsetof(S,M))

#Region "Untested functions"

#Region "User-Defined Types"
    ''' <summary> Get the name and size of a type. </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_type(ncid As Integer, xtype As nc_type, name As StringBuilder, <Out> ByRef size As IntPtr) As Integer
    End Function

    ''' <summary> Are two types equal? </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_type_equal(ncid1 As Integer, typeid1 As nc_type, ncid2 As Integer, typeid2 As nc_type, <Out> ByRef equal As Integer) As Integer
    End Function

    ''' <summary> Get the id of a type from the name. </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_typeid(ncid As Integer, name As String, <Out> ByRef typeidp As nc_type) As Integer
    End Function

    ''' <summary> Find all user-defined types for a location. This finds all user-defined types in a group. </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_typeids(ncid As Integer, <Out> ByRef ntypes As Integer, <Out> ByRef typeids As Integer) As Integer
    End Function

    ''' <summary> Find out about a user defined type. </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_user_type(ncid As Integer, xtype As nc_type, name As StringBuilder, <Out> ByRef size As IntPtr, <Out> ByRef base_nc_typep As nc_type, <Out> ByRef nfieldsp As Integer, <Out> ByRef classp As Integer) As Integer
    End Function
#End Region

#Region "Compound Types"
    ''' <summary> Here are functions for dealing with compound types.  Create a compound type. </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_def_compound(ncid As Integer, size As Integer, name As String, <Out> ByRef typeidp As nc_type) As Integer
    End Function

    ''' <summary> Insert a named field into a compound type. </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_insert_compound(ncid As Integer, xtype As nc_type, name As String, offset As Integer, field_typeid As nc_type) As Integer
    End Function

    ''' <summary> Insert a named array into a compound type. </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_insert_array_compound(ncid As Integer, xtype As nc_type, name As String, offset As Integer, field_typeid As nc_type, ndims As Integer, dim_sizes As Integer) As Integer
    End Function

    ''' <summary> Get the name, size, and number of fields in a compound type. </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_compound(ncid As Integer, xtype As nc_type, name As StringBuilder, <Out> ByRef sizep As IntPtr, <Out> ByRef nfieldsp As Integer) As Integer
    End Function

    ''' <summary> Get the name of a compound type. </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_compound_name(ncid As Integer, xtype As nc_type, name As StringBuilder) As Integer
    End Function

    ''' <summary> Get the size of a compound type. </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_compound_size(ncid As Integer, xtype As nc_type, <Out> ByRef sizep As IntPtr) As Integer
    End Function

    ''' <summary> Get the number of fields in this compound type. </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_compound_nfields(ncid As Integer, xtype As nc_type, <Out> ByRef nfieldsp As Integer) As Integer
    End Function

    ''' <summary> Given the xtype and the fieldid, get all info about it. </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_compound_field(ncid As Integer, xtype As nc_type, fieldid As Integer, name As StringBuilder, <Out> ByRef offsetp As Integer, <Out> ByRef field_typeidp As nc_type, <Out> ByRef ndimsp As Integer, <Out> ByRef dim_sizesp As Integer) As Integer
    End Function

    ''' <summary> Given the typeid and the fieldid, get the name. </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_compound_fieldname(ncid As Integer, xtype As nc_type, fieldid As Integer, name As StringBuilder) As Integer
    End Function

    ''' <summary> Given the xtype and the name, get the fieldid. </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_compound_fieldindex(ncid As Integer, xtype As nc_type, name As String, <Out> ByRef fieldidp As Integer) As Integer
    End Function

    ''' <summary> Given the xtype and fieldid, get the offset. </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_compound_fieldoffset(ncid As Integer, xtype As nc_type, fieldid As Integer, <Out> ByRef offsetp As Integer) As Integer
    End Function

    ''' <summary> Given the xtype and the fieldid, get the type of that field. </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_compound_fieldtype(ncid As Integer, xtype As nc_type, fieldid As Integer, <Out> ByRef field_typeidp As nc_type) As Integer
    End Function

    ''' <summary> Given the xtype and the fieldid, get the number of dimensions for that field (scalars are 0). </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_compound_fieldndims(ncid As Integer, xtype As nc_type, fieldid As Integer, <Out> ByRef ndimsp As Integer) As Integer
    End Function

    ''' <summary> Given the xtype and the fieldid, get the sizes of dimensions for that field. User must have allocated storage for the dim_sizes. </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_compound_fielddim_sizes(ncid As Integer, xtype As nc_type, fieldid As Integer, <Out> ByRef dim_sizes As Integer) As Integer
    End Function
#End Region

#Region "Enum types"
    ' Enum types
    ''' <summary>
    ''' Create an enum type. Provide a base type and a name. At the moment
    ''' only ints are accepted as base types. 
    ''' </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_def_enum(ncid As Integer, base_typeid As nc_type, name As String, <Out> ByRef typeidp As nc_type) As Integer
    End Function

    ''' <summary>Insert a named value into an enum type.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_insert_enum(ncid As Integer, xtype As nc_type, name As String, value As Object) As Integer
    End Function

    ''' <summary>Get information about an enum type: its name, base type and the number of members defined. </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_enum(ncid As Integer, xtype As nc_type, name As StringBuilder, <Out> ByRef base_nc_typep As nc_type, <Out> ByRef base_sizep As IntPtr, <Out> ByRef num_membersp As Integer) As Integer
    End Function

    ''' <summary>Get information about an enum member</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_enum_member(ncid As Integer, xtype As nc_type, idx As Integer, name As String, <Out> ByRef value As Object) As Integer
    End Function

    ''' <summary>Get enum name from enum value. Name size will be </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_enum_ident(ncid As Integer, xtype As nc_type, value As Long, identifier As StringBuilder) As Integer
    End Function
#End Region

#Region "Variable Length Array Types"
    ''' <summary>* This is the type of arrays of vlens. * Calculate an offset for creating a compound type. This calls a mysterious C macro which was found carved into one of the blocks of the Newgrange passage tomb in County Meath, Ireland. This code has been carbon dated to 3200 B.C.E.  Create a variable length type. </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_def_vlen(ncid As Integer, name As String, base_typeid As nc_type, <Out> ByRef xtypep As nc_type) As Integer
    End Function

    ''' <summary> Find out about a vlen. </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_inq_vlen(ncid As Integer, xtype As nc_type, name As StringBuilder, <Out> ByRef datum_sizep As IntPtr, <Out> ByRef base_nc_typep As nc_type) As Integer
    End Function

    ' When you read VLEN type the library will actually allocate the storage space for the data. This storage space must be freed, so pass the pointer back to this function, when you're done with the data, and it will free the vlen memory. </summary>
    ' [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
    ' public static extern int nc_free_vlen(nc_vlen_t* vl);

    ' [DllImport("netcdf.dll", CallingConvention = CallingConvention.Cdecl)]
    ' public static extern int nc_free_vlens(int len, nc_vlen_t vlens[]);

    ''' <summary> Put or get one element in a vlen array. </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_put_vlen_element(ncid As Integer, typeid1 As Integer, <Out> ByRef vlen_element As Object, len As IntPtr, data As Object) As Integer
    End Function

    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_vlen_element(ncid As Integer, typeid1 As Integer, vlen_element As Object, <Out> ByRef len As IntPtr, <Out> ByRef data As Object) As Integer
    End Function
#End Region
#End Region

#Region "Misc methods"
    ''' <summary>
    ''' Set the default nc_create format to NC_FORMAT_CLASSIC, NC_FORMAT_64BIT,
    ''' NC_FORMAT_CDF5, NC_FORMAT_NETCDF4, or NC_FORMAT_NETCDF4_CLASSIC 
    ''' </summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_set_default_format(format As Integer, <Out> ByRef old_formatp As Integer) As Integer
    End Function

    ''' <summary>Set the cache size, nelems, and preemption policy.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_set_chunk_cache(size As Integer, nelems As Integer, preemption As Single) As Integer
    End Function

    ''' <summary>Get the cache size, nelems, and preemption policy.</summary>
    <DllImport("netcdf.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function nc_get_chunk_cache(<Out> ByRef sizep As Integer, <Out> ByRef nelemsp As Integer, <Out> ByRef preemptionp As Single) As Integer
    End Function

#End Region

End Module

