#Region "Microsoft.VisualBasic::2c73f67e2ead98087074c71607b21e5e, src\data\LINQ\LINQ\Runtime\Internal\Math.vb"

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
'    Code Lines: 9 (69.23%)
' Comment Lines: 0 (0.00%)
'    - Xml Docs: 0.00%
' 
'   Blank Lines: 4 (30.77%)
'     File Size: 413 B


'     Class Math
' 
' 
' 
' 
' /********************************************************************************/

#End Region

Imports std = System.Math

Namespace Runtime.Internal

    Friend Class Math

        Public Shared ReadOnly abs As New Callable(AddressOf std.Abs)
        Public Shared ReadOnly min As New Callable(AddressOf std.Min)
        Public Shared ReadOnly max As New Callable(AddressOf std.Max)
        Public Shared ReadOnly pow As New Callable(AddressOf std.Pow)
        Public Shared ReadOnly exp As New Callable(AddressOf std.Exp)
        Public Shared ReadOnly log As New Callable(math2:=AddressOf std.Log)
        Public Shared ReadOnly log10 As New Callable(AddressOf std.Log10)
        Public Shared ReadOnly ln As New Callable(math1:=AddressOf std.Log)

    End Class
End Namespace
