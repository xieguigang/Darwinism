#Region "Microsoft.VisualBasic::4fc2d17ac235975135d69b4a521ce747, src\CloudKit\Docker\Captures\ParserHelpers.vb"

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

    '   Total Lines: 25
    '    Code Lines: 20
    ' Comment Lines: 0
    '   Blank Lines: 5
    '     File Size: 852 B


    '     Module ParserHelpers
    ' 
    '         Function: ParseTable
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Text.Parser
Imports r = System.Text.RegularExpressions.Regex

Namespace Captures

    <HideModuleName>
    Public Module ParserHelpers

        <Extension>
        Public Iterator Function ParseTable(Of T)(text$, creator As Func(Of String(), T)) As IEnumerable(Of T)
            Dim summary$() = text.Trim.LineTokens
            Dim header = r.Matches(summary(Scan0), "(\S+\s+)|(\S+)").ToArray
            Dim fieldLength%() = header.Select(AddressOf Len).ToArray

            For Each line As String In summary.Skip(1)
                Dim tokens$() = FormattedParser _
                    .FieldParser(line, fieldLength) _
                    .ToArray

                Yield creator(tokens)
            Next
        End Function
    End Module
End Namespace
