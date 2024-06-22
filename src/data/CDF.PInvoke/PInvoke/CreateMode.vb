#Region "Microsoft.VisualBasic::8a03be19cf3c7487e466684c0d9ce4bb, src\data\CDF.PInvoke\PInvoke\CreateMode.vb"

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

    '   Total Lines: 34
    '    Code Lines: 12 (35.29%)
    ' Comment Lines: 22 (64.71%)
    '    - Xml Docs: 45.45%
    ' 
    '   Blank Lines: 0 (0.00%)
    '     File Size: 1.85 KB


    ' Enum CreateMode
    ' 
    ' 
    '  
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

' 
'  cmode	The creation mode flag. The following flags are available: 
'  NC_CLOBBER (overwrite existing file), 
'  NC_NOCLOBBER (do not overwrite existing file), 
'  NC_SHARE (limit write caching - netcdf classic files only), 
'  NC_64BIT_OFFSET (create 64-bit offset file), 
'  NC_64BIT_DATA (alias NC_CDF5) (create CDF-5 file), 
'  NC_NETCDF4 (create netCDF-4/HDF5 file), 
'  NC_CLASSIC_MODEL (enforce netCDF classic mode on netCDF-4/HDF5 files), 
'  NC_DISKLESS (store data in memory), and NC_PERSIST (force the NC_DISKLESS data from memory to a file), 
'  NC_MMAP (use MMAP for NC_DISKLESS instead of NC_INMEMORY – deprecated). 
' 
Public Enum CreateMode As Integer
    ''' <summary>Overwrite existing file. Mode flag for nc_create()</summary>
    NC_CLOBBER = &H0
    ''' <summary>Don't destroy existing file. Mode flag for nc_create()</summary>
    NC_NOCLOBBER = &H4
    ''' <summary>Use diskless file. Mode flag for nc_open() or nc_create()</summary>
    NC_DISKLESS = &H8
    ''' <summary>deprecated Use diskless file with mmap. Mode flag for nc_open() or nc_create()</summary>
    NC_MMAP = &H10
    ''' <summary>CDF-5 format: classic model but 64 bit dimensions and sizes</summary>
    NC_64BIT_DATA = &H20
    ''' <summary>Enforce classic model on netCDF-4. Mode flag for nc_create()</summary>
    NC_CLASSIC_MODEL = &H100
    ''' <summary>Use large (64-bit) file offsets. Mode flag for nc_create()</summary>
    NC_64BIT_OFFSET = &H200
    ''' <summary>Share updates, limit caching. Use this in mode flags for both nc_create() and nc_open()</summary>
    NC_SHARE = &H800
    ''' <summary>se netCDF-4/HDF5 format. Mode flag for nc_create()</summary>
    NC_NETCDF4 = &H1000
    ''' <summary>Save diskless contents to disk. Mode flag for nc_open() or nc_create()</summary>
    NC_PERSIST = &H4000
End Enum
