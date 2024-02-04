
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
                Dim v As Single() = NetCDF.Get_float(handle, name)
                Return CType(v, floats)
            Case CDFDataTypes.NC_DOUBLE
                Dim v As Double() = NetCDF.Get_double(handle, name)
                Return CType(v, doubles)
            Case CDFDataTypes.NC_CHAR
            Case CDFDataTypes.NC_INT
                Return CType(NetCDF.Get_int(handle, name), integers)
            Case CDFDataTypes.NC_SHORT
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
