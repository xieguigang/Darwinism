Imports System.IO
Imports System.Text

Module EmitHandler

    Public Iterator Function PopulatePrimitiveHandles() As IEnumerable(Of (target As Type, emit As Func(Of Object, Stream)))
        Yield (GetType(Integer), Function(i) New MemoryStream(BitConverter.GetBytes(DirectCast(i, Integer))))
        Yield (GetType(Long), Function(l) New MemoryStream(BitConverter.GetBytes(DirectCast(l, Long))))
        Yield (GetType(Single), Function(f) New MemoryStream(BitConverter.GetBytes(DirectCast(f, Single))))
        Yield (GetType(Double), Function(d) New MemoryStream(BitConverter.GetBytes(DirectCast(d, Double))))
        Yield (GetType(Short), Function(s) New MemoryStream(BitConverter.GetBytes(DirectCast(s, Short))))
        Yield (GetType(Boolean), Function(b) New MemoryStream(New Byte() {If(DirectCast(b, Boolean), 1, 0)}))
        Yield (GetType(String), Function(s) New MemoryStream(Encoding.UTF8.GetBytes(DirectCast(s, String))))
    End Function
End Module
