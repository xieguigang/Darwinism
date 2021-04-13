#Region "Microsoft.VisualBasic::2ee8556607679bf916d41894466cce0b, Distribute_computing\GridDynamics_plugins\IO\VectorIOExtensions.vb"

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

    ' Module VectorIOExtensions
    ' 
    '     Function: LoadVector
    ' 
    '     Sub: Serialize
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.IO
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math.LinearAlgebra

Public Module VectorIOExtensions

    <Extension>
    Public Sub Serialize(vec As Vector, save As Stream, Optional chunkSize% = 1024)
        Dim chunks = vec.Array.Split(chunkSize)
        Dim bytes As Byte()
        Dim ms As MemoryStream
        Dim buffers As New List(Of MemoryStream)

        ' chunksize int (校验用)
        ' chunks int
        ' index1 long length1 long
        ' index2 long length2 long
        ' ...
        ' gzipchunk1 bytes
        ' gzipchunk2 bytes
        ' ...

        For Each chunk As Double() In chunks
            bytes = chunk _
                .Select(AddressOf BitConverter.GetBytes) _
                .IteratesALL _
                .ToArray
            ' 对一个chunk做gzip压缩
            ms = New MemoryStream(bytes) '.GZipStream
            buffers += ms
        Next

        Dim writer As New BinaryDataWriter(save)
        writer.Write(chunkSize)
        writer.Write(buffers.Count)

        Dim index As New List(Of (offset&, size&))
        Dim offset As Long = 8 + (8 + 8) * buffers.Count

        For Each chunk As MemoryStream In buffers
            index += (offset, chunk.Length)
            offset += chunk.Length
        Next

        For Each offsetIndex In index
            Call writer.Write(offsetIndex.offset)
            Call writer.Write(offsetIndex.size)
        Next

        For Each chunk As MemoryStream In buffers
            Call writer.Write(chunk)
        Next

        Call writer.Flush()
    End Sub

    <Extension>
    Public Function LoadVector(reads As Stream) As Vector
        Dim data As New List(Of Double)

        Using reader As New BinaryDataReader(reads)
            Dim chunkSize = reader.ReadInt32
            Dim chunks = reader.ReadInt32
            Dim offset, size As Long
            Dim buffer As Byte()

            For i As Integer = 0 To chunks - 1
                offset = reader.ReadInt64
                size = reader.ReadInt64

                reader.Mark()
                ' goback to original
                reader.Seek(-(4 + 4 + 16 * (i + 1)), SeekOrigin.Current)
                ' then goto data offset
                reader.Seek(offset, SeekOrigin.Current)
                ' read gzip data chunk and then ungzip
                buffer = reader.ReadBytes(size) '.UnGzipStream
                ' back to doubles
                data += buffer.ToArray _
                    .Split(8) _
                    .Select(Function(bytes)
                                Return BitConverter.ToDouble(bytes, Scan0)
                            End Function)

                reader.Reset()
            Next
        End Using

        Return New Vector(data)
    End Function
End Module
