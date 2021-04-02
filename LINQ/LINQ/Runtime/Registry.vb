#Region "Microsoft.VisualBasic::a9a98461d77d59c3c0013190f451e6ad, LINQ\LINQ\Runtime\Registry.vb"

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

    '     Class Registry
    ' 
    '         Function: GetReader, GetTypeCodeName
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace Runtime

    Public Class Registry

        Public Function GetTypeCodeName(type As Type) As String
            Select Case type
                Case GetType(Integer) : Return "i32"
                Case GetType(Long) : Return "i64"
                Case Else
                    Throw New MissingPrimaryKeyException
            End Select
        End Function

        Public Function GetReader(type As String) As DataSourceDriver
            If type = "row" Then
                Return New DataFrameDriver
            Else
                Throw New MissingPrimaryKeyException
            End If
        End Function
    End Class
End Namespace
