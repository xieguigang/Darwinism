#Region "Microsoft.VisualBasic::260611612766d6688b161f2575f97984, src\data\CDF.PInvoke\PInvoke\FillValues.vb"

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

    '   Total Lines: 35
    '    Code Lines: 15 (42.86%)
    ' Comment Lines: 18 (51.43%)
    '    - Xml Docs: 33.33%
    ' 
    '   Blank Lines: 2 (5.71%)
    '     File Size: 1.62 KB


    ' Class FillValues
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

' #define NC_FILL_BYTE    ((signed char)-127)
' #define NC_FILL_CHAR    ((char)0)
' #define NC_FILL_SHORT   ((short)-32767)
' #define NC_FILL_INT     (-2147483647)
' #define NC_FILL_FLOAT   (9.9692099683868690e+36f) /* near 15 * 2^119 
' #define NC_FILL_DOUBLE  (9.9692099683868690e+36)
' #define NC_FILL_UBYTE   (255)
' #define NC_FILL_USHORT  (65535)
' #define NC_FILL_UINT    (4294967295U)
' #define NC_FILL_INT64   ((long long)-9223372036854775806LL)
' #define NC_FILL_UINT64  ((unsigned long long)18446744073709551614ULL)
' #define NC_FILL_STRING  ((char *)"")

Imports Microsoft.VisualBasic.Text

''' <summary>
''' Default fill values, used unless _FillValue attribute is set.
''' These values are stuffed into newly allocated space as appropriate.
''' The hope is that one might use these to notice that a particular datum
''' has not been set.
''' </summary>
Public NotInheritable Class FillValues
    Public Const NC_FILL_BYTE As SByte = -127
    Public Const NC_FILL_CHAR As Char = ASCII.NUL
    Public Const NC_FILL_SHORT As Short = -32767
    Public Const NC_FILL_INT As Integer = -2147483647
    Public Const NC_FILL_FLOAT As Single = 9.96921E+36F    ' near 15 * 2^119 
    Public Const NC_FILL_DOUBLE As Double = 9.969209968386869E+36
    Public Const NC_FILL_UBYTE As Byte = 255
    Public Const NC_FILL_USHORT As UShort = 65535
    Public Const NC_FILL_UINT As UInteger = 4294967295UI
    Public Const NC_FILL_INT64 As Long = -9223372036854775806L
    Public Const NC_FILL_UINT64 As ULong = 18446744073709551614UL
    Public Const NC_FILL_STRING As String = ""
End Class
