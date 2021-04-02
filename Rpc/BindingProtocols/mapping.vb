#Region "Microsoft.VisualBasic::0fe15f75d20e135d78954f95827c63fa, Rpc\BindingProtocols\mapping.vb"

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

    '     Structure mapping
    ' 
    '         Function: ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Data.IO.Xdr

Namespace Rpc.BindingProtocols
    ''' <summary>
    ''' A mapping of (program, version, protocol) to port number
    ''' http://tools.ietf.org/html/rfc1833#section-3.1
    ''' </summary>
    Public Structure mapping
        ''' <summary>
        ''' program number
        ''' </summary>
        <Order(0)>
        Public prog As UInteger
        ''' <summary>
        ''' version number
        ''' </summary>
        <Order(1)>
        Public vers As UInteger
        ''' <summary>
        ''' protocol
        ''' </summary>
        <Order(2)>
        Public prot As Protocol
        ''' <summary>
        ''' port
        ''' </summary>
        <Order(3)>
        Public port As UInteger

        ''' <summary>
        ''' Returns a <seecref="System.String"/> that represents the current <seecref="Rpc.BindingProtocols.mapping"/>.
        ''' </summary>
        ''' <returns>
        ''' A <seecref="System.String"/> that represents the current <seecref="Rpc.BindingProtocols.mapping"/>.
        ''' </returns>
        Public Overrides Function ToString() As String
            Return String.Format("port:{0} prog:{1} prot:{2} vers:{3}", port, prog, prot, vers)
        End Function
    End Structure
End Namespace

