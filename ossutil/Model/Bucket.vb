#Region "Microsoft.VisualBasic::bcd0e3eaa9ec60e7fef31497b3782439, ossutil\Model\Bucket.vb"

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

    '     Class Bucket
    ' 
    '         Properties: BucketName, CreationTime, Region, StorageClass
    ' 
    '         Function: ToString, URI
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace Model

    ''' <summary>
    ''' Bucket storage device meta data for cloud file system
    ''' </summary>
    Public Class Bucket : Inherits MetaData

        Public Const Protocol$ = "oss://"

        Public ReadOnly Property CreationTime As String
            Get
                Return getValue()
            End Get
        End Property

        Public ReadOnly Property Region As String
            Get
                Return getValue()
            End Get
        End Property

        Public ReadOnly Property StorageClass As String
            Get
                Return getValue()
            End Get
        End Property

        Public ReadOnly Property BucketName As String
            Get
                Return Mid(getValue(), Protocol.Length + 1)
            End Get
        End Property

        Public Function URI(path As String) As String
            Return $"oss://{BucketName}/{path}"
        End Function

        Public Overrides Function ToString() As String
            Return $"{BucketName} @ {Region}"
        End Function
    End Class
End Namespace
