#Region "Microsoft.VisualBasic::b1e0de08bab89ad299202674cf88028c, src\data\LINQ\LINQ\Runtime\Symbol.vb"

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

    '   Total Lines: 27
    '    Code Lines: 21
    ' Comment Lines: 0
    '   Blank Lines: 6
    '     File Size: 860 B


    '     Class Symbol
    ' 
    '         Properties: SymbolKey, type, value, valueType
    ' 
    '         Function: ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.Repository

Namespace Runtime

    Public Class Symbol : Implements INamedValue

        Public Property SymbolKey As String Implements IKeyedEntity(Of String).Key
        Public Property type As String
        Public Property value As Object

        Public ReadOnly Property valueType As Type
            Get
                If value Is Nothing Then
                    Return GetType(Void)
                Else
                    Return value.GetType
                End If
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return $"let {SymbolKey} as {type} = ({valueType.Name.ToLower}) {value}"
        End Function

    End Class
End Namespace
