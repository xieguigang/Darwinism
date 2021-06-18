#Region "Microsoft.VisualBasic::ab6301c864f94a35ede0a5a03a60263e, LINQ\LINQ\Runtime\GlobalEnvironment.vb"

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

    '     Class GlobalEnvironment
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: GetDriverByCode
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Runtime.Drivers
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel

Namespace Runtime

    Public Class GlobalEnvironment : Inherits Environment

        Protected Friend ReadOnly registry As Registry

        Sub New(registry As Registry, ParamArray values As NamedValue(Of Object)())
            Call MyBase.New(Nothing)

            Dim typeCode As String
            Dim symbol As Symbol

            For Each item In values
                typeCode = registry.GetTypeCodeName(item.Value.GetType)
                symbol = AddSymbol(item.Name, typeCode)
                symbol.value = item.Value
            Next

            Me.registry = registry
        End Sub

        Public Function GetDriverByCode(code As String, arguments As String()) As DataSourceDriver
            Return registry.GetReader(code, arguments)
        End Function
    End Class
End Namespace
