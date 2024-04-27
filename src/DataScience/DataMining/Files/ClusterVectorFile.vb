#Region "Microsoft.VisualBasic::0cdb364d62fd7b693474fbe15816cf75, G:/GCModeller/src/runtime/Darwinism/src/DataScience/DataMining//Files/ClusterVectorFile.vb"

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

    '   Total Lines: 32
    '    Code Lines: 24
    ' Comment Lines: 3
    '   Blank Lines: 5
    '     File Size: 1.21 KB


    ' Class ClusterVectorFile
    ' 
    '     Function: BufferInMemory, ReadBuffer, (+2 Overloads) WriteBuffer
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports Microsoft.VisualBasic.DataMining.ComponentModel.Serialization
Imports Microsoft.VisualBasic.DataMining.KMeans
Imports Parallel

''' <summary>
''' file stream handler for <see cref="ClusterEntity()"/>.
''' </summary>
Public Class ClusterVectorFile : Implements IEmitStream

    Public Function BufferInMemory(obj As Object) As Boolean Implements IEmitStream.BufferInMemory
        Return True
    End Function

    Public Function WriteBuffer(obj As Object, file As Stream) As Boolean Implements IEmitStream.WriteBuffer
        Call EntityVectorFile.SaveVector(DirectCast(obj, ClusterEntity()), file)
        Call file.Flush()
        Return True
    End Function

    Public Function WriteBuffer(obj As Object) As Stream Implements IEmitStream.WriteBuffer
        Dim ms As New MemoryStream
        Call EntityVectorFile.SaveVector(DirectCast(obj, ClusterEntity()), ms)
        Call ms.Flush()
        Call ms.Seek(0, SeekOrigin.Begin)
        Return ms
    End Function

    Public Function ReadBuffer(file As Stream) As Object Implements IEmitStream.ReadBuffer
        Return EntityVectorFile.LoadVectors(file).ToArray
    End Function
End Class

