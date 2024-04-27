#Region "Microsoft.VisualBasic::9bd23076d203de1d2865c07c374dbe5a, G:/GCModeller/src/runtime/Darwinism/src/CloudKit/ossutil//Model/MetaData.vb"

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

    '   Total Lines: 19
    '    Code Lines: 14
    ' Comment Lines: 0
    '   Blank Lines: 5
    '     File Size: 589 B


    '     Class MetaData
    ' 
    '         Function: getValue, ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace Model

    Public MustInherit Class MetaData

        Friend meta As Dictionary(Of String, String)

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Protected Function getValue(<CallerMemberName> Optional key$ = Nothing) As String
            Return If(meta.ContainsKey(key), meta(key), Nothing)
        End Function

        Public Overrides Function ToString() As String
            Return meta.GetJson
        End Function
    End Class
End Namespace
