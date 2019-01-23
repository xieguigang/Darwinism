#Region "Microsoft.VisualBasic::d8e9122dd87394e9d9150d2b25e9501c, ComputingServices\ComponentModel\IHostBase.vb"

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

    '     Class IHostBase
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '     Class IMasterBase
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Abstract
Imports Microsoft.VisualBasic.Net.Tcp

Namespace ComponentModel

    Public MustInherit Class IHostBase : Inherits IMasterBase(Of TcpServicesSocket)

        Sub New(portal As Integer)
            __host = New TcpServicesSocket(portal)
        End Sub

        Sub New()
        End Sub
    End Class

    Public MustInherit Class IMasterBase(Of TSocket As IServicesSocket)

        Public MustOverride ReadOnly Property Portal As IPEndPoint

        Protected Friend __host As TSocket

        Public Shared Narrowing Operator CType(master As IMasterBase(Of TSocket)) As IPEndPoint
            Return master.Portal
        End Operator

        Public Shared Narrowing Operator CType(master As IMasterBase(Of TSocket)) As System.Net.IPEndPoint
            Return master.Portal.GetIPEndPoint
        End Operator

    End Class
End Namespace
