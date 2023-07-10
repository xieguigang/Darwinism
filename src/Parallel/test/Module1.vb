#Region "Microsoft.VisualBasic::8fa0ec130da2dfd7e656c5bcf9ade941, Parallel\test\Module1.vb"

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

    ' Module Module1
    ' 
    '     Sub: Main
    ' 
    ' Class vec
    ' 
    '     Properties: v
    ' 
    ' /********************************************************************************/

#End Region

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
