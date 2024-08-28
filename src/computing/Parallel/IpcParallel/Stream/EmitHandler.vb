#Region "Microsoft.VisualBasic::38cc0b4027fc15a25f20e2288132d8dd, src\computing\Parallel\IpcParallel\Stream\EmitHandler.vb"

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

    '   Total Lines: 81
    '    Code Lines: 59 (72.84%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 22 (27.16%)
    '     File Size: 3.95 KB


    '     Module EmitHandler
    ' 
    ' 
    '         Delegate Function
    ' 
    ' 
    '         Delegate Function
    ' 
    '             Constructor: (+1 Overloads) Sub New
    '             Function: PopulatePrimitiveHandles, PopulatePrimitiveParsers, readAllBytes, (+2 Overloads) stringBuffer
    ' 
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.ValueTypes

Namespace IpcStream

    Module EmitHandler

        ''' <summary>
        ''' serialize target object as memory stream bytes
        ''' </summary>
        ''' <param name="obj"></param>
        ''' <returns></returns>
        Public Delegate Function toBuffer(obj As Object) As Stream
        ''' <summary>
        ''' parse target object from a given memory stream bytes
        ''' </summary>
        ''' <param name="buf"></param>
        ''' <returns></returns>
        Public Delegate Function loadBuffer(buf As Stream) As Object

        Sub New()
            Call RegisterDiagnoseBuffer()
        End Sub

        Public Iterator Function PopulatePrimitiveHandles() As IEnumerable(Of (target As Type, emit As toBuffer))
            Yield (GetType(Integer), Function(i) New MemoryStream(BitConverter.GetBytes(DirectCast(i, Integer))))
            Yield (GetType(UInteger), Function(i) New MemoryStream(BitConverter.GetBytes(DirectCast(i, UInteger))))

            Yield (GetType(Long), Function(l) New MemoryStream(BitConverter.GetBytes(DirectCast(l, Long))))
            Yield (GetType(ULong), Function(l) New MemoryStream(BitConverter.GetBytes(DirectCast(l, ULong))))

            Yield (GetType(Single), Function(f) New MemoryStream(BitConverter.GetBytes(DirectCast(f, Single))))
            Yield (GetType(Double), Function(d) New MemoryStream(BitConverter.GetBytes(DirectCast(d, Double))))

            Yield (GetType(UShort), Function(s) New MemoryStream(BitConverter.GetBytes(DirectCast(s, UShort))))
            Yield (GetType(Boolean), Function(b) New MemoryStream(New Byte() {If(DirectCast(b, Boolean), 1, 0)}))
            Yield (GetType(String), AddressOf stringBuffer)
            Yield (GetType(Char), Function(c) New MemoryStream(BitConverter.GetBytes(AscW(DirectCast(c, Char)))))

            Yield (GetType(Byte), Function(b) New MemoryStream({DirectCast(b, Byte)}))
            Yield (GetType(Date), Function(d) New MemoryStream(BitConverter.GetBytes(DirectCast(d, Date).UnixTimeStamp)))

            ' special handler for data
            Yield (GetType(NamedValue(Of Double)), Function(d) saveTagNumber(d))
            Yield (GetType(NamedValue(Of Double)()), Function(d) saveTagVector(d))
        End Function

        Private Function saveTagVector(vec As NamedValue(Of Double)()) As MemoryStream
            Dim s As New MemoryStream()
            Dim bin As New BinaryWriter(s, Encoding.UTF8)
            bin.Write(vec.Length)

            For Each d As NamedValue(Of Double) In vec
                bin.Write(If(d.Name, ""))
                bin.Write(d.Value)
                bin.Write(If(d.Description, ""))
            Next

            bin.Flush()
            s.Seek(Scan0, SeekOrigin.Begin)
            Return s
        End Function

        Private Function saveTagNumber(d As NamedValue(Of Double)) As MemoryStream
            Dim s As New MemoryStream()
            Dim bin As New BinaryWriter(s, Encoding.UTF8)
            bin.Write(If(d.Name, ""))
            bin.Write(d.Value)
            bin.Write(If(d.Description, ""))
            bin.Flush()
            s.Seek(Scan0, SeekOrigin.Begin)
            Return s
        End Function

        Private Function readTagVector(s As Stream) As NamedValue(Of Double)()
            Dim bin As New BinaryReader(s, Encoding.UTF8)
            Dim n As Integer = bin.ReadInt32
            Dim vec As NamedValue(Of Double)() = New NamedValue(Of Double)(n - 1) {}

            For i As Integer = 0 To n - 1
                Dim name = bin.ReadString
                Dim val = bin.ReadDouble
                Dim desc = bin.ReadString

                vec(i) = New NamedValue(Of Double)(name, val, desc)
            Next

            Return vec
        End Function

        Private Function readTagNumber(s As Stream) As NamedValue(Of Double)
            Dim bin As New BinaryReader(s, Encoding.UTF8)
            Dim name = bin.ReadString
            Dim val = bin.ReadDouble
            Dim desc = bin.ReadString

            Return New NamedValue(Of Double)(name, val, desc)
        End Function

        Private Function stringBuffer(s As Object) As Stream
            Dim bytes As Byte() = Encoding.UTF8.GetBytes(DirectCast(s, String))
            Dim ms As New MemoryStream(bytes)

            Return ms
        End Function

        Private Function stringBuffer(s As Stream) As Object
            Dim bytes As Byte() = s.readAllBytes
            Dim str As String = Encoding.UTF8.GetString(bytes)

            Return str
        End Function

        Public Iterator Function PopulatePrimitiveParsers() As IEnumerable(Of (target As Type, emit As loadBuffer))
            Yield (GetType(Integer), Function(i) BitConverter.ToInt32(i.readAllBytes, Scan0))
            Yield (GetType(UInteger), Function(i) BitConverter.ToUInt32(i.readAllBytes, Scan0))

            Yield (GetType(Long), Function(l) BitConverter.ToInt64(l.readAllBytes, Scan0))
            Yield (GetType(ULong), Function(l) BitConverter.ToUInt64(l.readAllBytes, Scan0))

            Yield (GetType(Single), Function(f) BitConverter.ToSingle(f.readAllBytes, Scan0))
            Yield (GetType(Double), Function(d) BitConverter.ToDouble(d.readAllBytes, Scan0))

            Yield (GetType(Short), Function(s) BitConverter.ToInt16(s.readAllBytes, Scan0))
            Yield (GetType(UShort), Function(s) BitConverter.ToUInt16(s.readAllBytes, Scan0))

            Yield (GetType(Boolean), Function(b) b.readAllBytes()(Scan0) <> 0)
            Yield (GetType(String), AddressOf stringBuffer)
            Yield (GetType(Char), Function(c) ChrW(BitConverter.ToInt32(c.readAllBytes, Scan0)))

            Yield (GetType(Byte), Function(b) b.readAllBytes()(Scan0))
            Yield (GetType(Date), Function(d) FromUnixTimeStamp(BitConverter.ToDouble(d.readAllBytes, Scan0)))

            ' special handler for data
            Yield (GetType(NamedValue(Of Double)), Function(s) readTagNumber(s))
            Yield (GetType(NamedValue(Of Double)()), Function(s) readTagVector(s))
        End Function

        <Extension>
        Private Function readAllBytes(buf As Stream) As Byte()
            Dim sizeOf As Integer = buf.Length
            Dim bytes As Byte() = New Byte(sizeOf - 1) {}

            Call buf.Read(bytes, Scan0, sizeOf)

            Return bytes
        End Function
    End Module
End Namespace
