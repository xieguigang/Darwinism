#Region "Microsoft.VisualBasic::96b6937c45f2c43af639592d184f868b, G:/GCModeller/src/runtime/Darwinism/src/data/CDF.PInvoke//DataReader.vb"

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

    '   Total Lines: 116
    '    Code Lines: 76
    ' Comment Lines: 17
    '   Blank Lines: 23
    '     File Size: 4.06 KB


    ' Class DataReader
    ' 
    '     Properties: attributes, error_no, vars
    ' 
    '     Constructor: (+1 Overloads) Sub New
    ' 
    '     Function: GetData
    ' 
    '     Sub: (+2 Overloads) Dispose
    ' 
    ' /********************************************************************************/

#End Region


Imports System.Text
Imports Microsoft.VisualBasic.DataStorage.netCDF.Components
Imports Microsoft.VisualBasic.DataStorage.netCDF.Data
Imports Microsoft.VisualBasic.DataStorage.netCDF.DataVector

''' <summary>
''' netcdf data reader model
''' </summary>
Public Class DataReader : Implements IDisposable

    Dim disposedValue As Boolean
    Dim handle As Integer
    Dim varnames As New Dictionary(Of String, Integer)

    Public ReadOnly Property error_no As Integer
    Public ReadOnly Property vars As String()
        Get
            Return varnames.Keys.ToArray
        End Get
    End Property

    ''' <summary>
    ''' the global attributes
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property attributes As Dictionary(Of String, String)

    Sub New(file As String)
        _error_no = NetCDF.nc_open(file, OpenMode.NC_NOWRITE, ncidp:=handle)

        If _error_no = 0 Then
            Dim varids As Integer() = New Integer(2048) {}
            Dim nvars As Integer
            Dim name As New StringBuilder

            _error_no = NetCDF.nc_inq_varids(handle, nvars, varids)

            For i As Integer = 0 To nvars - 1
                _error_no = NetCDF.nc_inq_varname(handle, varids(i), name)
                varnames(name.ToString) = varids(i)
                name.Clear()
            Next

            attributes = New Dictionary(Of String, String)

            Dim nattrs As Integer

            NetCDF.nc_inq_natts(handle, nattrs)

            For i As Integer = 0 To nattrs - 1
                NetCDF.nc_inq_attname(handle, NC_GLOBAL, i, name)
                attributes(name.ToString) = NetCDF.GetGlobalAttribute(handle, name.ToString)
                name.Clear()
            Next
        End If
    End Sub

    Public Function GetData(name As String) As ICDFDataVector
        Dim id As Integer = varnames.TryGetValue(name, [default]:=-1)

        If id < 0 Then
            Return Nothing
        End If

        Dim type As CDFDataTypes = CDFDataTypes.undefined
        Dim ndims As Integer
        Dim dims As Integer()

        _error_no = NetCDF.nc_inq_vartype(handle, id, type)
        _error_no = NetCDF.nc_inq_varndims(handle, id, ndims)

        dims = New Integer(ndims - 1) {}

        Select Case type
            Case CDFDataTypes.NC_FLOAT
                Return CType(NetCDF.Get_float(handle, name), floats)
            Case CDFDataTypes.NC_DOUBLE
                Return CType(NetCDF.Get_double(handle, name), doubles)
            Case CDFDataTypes.NC_CHAR
                Return CType(NetCDF.Get_char(handle, name), chars)
            Case CDFDataTypes.NC_INT
                Return CType(NetCDF.Get_int(handle, name), integers)
            Case CDFDataTypes.NC_SHORT
                Return CType(NetCDF.Get_short(handle, name), shorts)
            Case Else
                Throw New NotImplementedException(type.ToString)
        End Select
    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: 释放托管状态(托管对象)
                NetCDF.nc_close(handle)
            End If

            ' TODO: 释放未托管的资源(未托管的对象)并重写终结器
            ' TODO: 将大型字段设置为 null
            disposedValue = True
        End If
    End Sub

    ' ' TODO: 仅当“Dispose(disposing As Boolean)”拥有用于释放未托管资源的代码时才替代终结器
    ' Protected Overrides Sub Finalize()
    '     ' 不要更改此代码。请将清理代码放入“Dispose(disposing As Boolean)”方法中
    '     Dispose(disposing:=False)
    '     MyBase.Finalize()
    ' End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' 不要更改此代码。请将清理代码放入“Dispose(disposing As Boolean)”方法中
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub
End Class

