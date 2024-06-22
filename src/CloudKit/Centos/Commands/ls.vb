#Region "Microsoft.VisualBasic::e9f933b108621ac52a00063279c9878e, src\CloudKit\Centos\Commands\ls.vb"

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

    '   Total Lines: 31
    '    Code Lines: 24 (77.42%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 7 (22.58%)
    '     File Size: 945 B


    ' Class ls
    ' 
    '     Properties: [date], file, group, link, number1
    '                 permission, size, user
    ' 
    '     Function: Parse, ParseLine, ToString
    ' 
    ' /********************************************************************************/

#End Region

Public Class ls

    Public Property permission As String
    Public Property number1 As Integer
    Public Property user As String
    Public Property group As String
    Public Property size As String
    Public Property [date] As String
    Public Property file As String
    Public Property link As String

    Public Overrides Function ToString() As String
        Dim fileName As String = If(link.StringEmpty, file, $"{file} -> {link}")
        Dim line As String = New String() {
            permission, number1, user, group, size, [date], fileName
        }.JoinBy(vbTab)

        Return line
    End Function

    Public Shared Iterator Function Parse(stdout As String) As IEnumerable(Of ls)
        For Each line As String In stdout.LineTokens.Skip(1)
            Yield ParseLine(line)
        Next
    End Function

    Private Shared Function ParseLine(line As String) As ls

    End Function

End Class
