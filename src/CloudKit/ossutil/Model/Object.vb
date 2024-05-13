#Region "Microsoft.VisualBasic::543abaabe407846fc1a9295ce3c0549c, src\CloudKit\ossutil\Model\Object.vb"

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

    '   Total Lines: 42
    '    Code Lines: 32
    ' Comment Lines: 3
    '   Blank Lines: 7
    '     File Size: 1.03 KB


    '     Class [Object]
    ' 
    '         Properties: ETAG, LastModifiedTime, ObjectName, Size, StorageClass
    ' 
    '         Function: ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace Model

    ''' <summary>
    ''' File/Directory object
    ''' </summary>
    Public Class [Object] : Inherits MetaData

        Public ReadOnly Property LastModifiedTime As String
            Get
                Return getValue()
            End Get
        End Property

        Public ReadOnly Property Size As Long
            Get
                Return getValue()
            End Get
        End Property

        Public ReadOnly Property StorageClass As String
            Get
                Return getValue()
            End Get
        End Property

        Public ReadOnly Property ETAG As String
            Get
                Return getValue()
            End Get
        End Property

        Public ReadOnly Property ObjectName As String
            Get
                Return getValue()
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return ObjectName
        End Function
    End Class
End Namespace
