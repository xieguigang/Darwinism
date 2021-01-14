Imports Microsoft.VisualBasic.Serialization.JSON
Imports Parallel

Module Module1

    Sub Main()

        Console.WriteLine("1 for host and 2 for client")

        Dim type = Console.ReadLine

        If type = "1" Then
            Dim map As MapObject = MapObject.FromObject(New vec With {.v = {2, 2, 2, 3, 55555}})

            Call Console.WriteLine(CType(map, UnmanageMemoryRegion).GetJson)

            Pause()
        Else
            Console.WriteLine("pointer:")
            Dim p As String = Console.ReadLine
            Console.WriteLine("region size:")
            Dim size As Integer = Console.ReadLine
            Dim mem_p As New UnmanageMemoryRegion With {.memoryFile = p, .size = size}

            Dim obj As vec = MapObject.FromPointer(mem_p).GetObject(GetType(vec))

            Call Console.WriteLine(obj.GetJson)

            Pause()
        End If

    End Sub

End Module

Public Class vec

    Public Property v As Double()
End Class