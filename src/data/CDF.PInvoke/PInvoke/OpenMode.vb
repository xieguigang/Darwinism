#Region "Microsoft.VisualBasic::c1819f0085e2e3fd31c23eef859d25f0, src\data\CDF.PInvoke\PInvoke\OpenMode.vb"

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

    '   Total Lines: 13
    '    Code Lines: 7 (53.85%)
    ' Comment Lines: 6 (46.15%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 0 (0.00%)
    '     File Size: 612 B


    ' Enum OpenMode
    ' 
    ' 
    '  
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

''' <summary>The open mode flags</summary>
Public Enum OpenMode As Integer
    ''' <summary>Set read-only access for nc_open()</summary>
    NC_NOWRITE = &H0
    ''' <summary>Set read-write access for nc_open()</summary>
    NC_WRITE = &H1
    ''' <summary>Use diskless file. Mode flag for nc_open() or nc_create()</summary>
    NC_DISKLESS = &H8
    ''' <summary>Share updates, limit caching. Use this in mode flags for both nc_create() and nc_open()</summary>
    NC_SHARE = &H800
    ''' <summary>Read from memory. Mode flag for nc_open() or nc_create()</summary>
    NC_INMEMORY = &H8000
End Enum
