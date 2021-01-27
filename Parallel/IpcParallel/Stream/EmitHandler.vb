Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text
Imports Microsoft.VisualBasic.ValueTypes

Namespace IpcStream

    Module EmitHandler

        Public Delegate Function toBuffer(obj As Object) As Stream
        Public Delegate Function loadBuffer(buf As Stream) As Object

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