#Region "Microsoft.VisualBasic::d6174f18955f94afc64848699e9ca436, src\computing\Parallel\test\memoryTest.vb"

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

    '   Total Lines: 39
    '    Code Lines: 25 (64.10%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 14 (35.90%)
    '     File Size: 1000 B


    ' Module memoryTest
    ' 
    '     Sub: Main1
    ' 
    ' Class vec
    ' 
    '     Properties: v
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Serialization.JSON
Imports Parallel

Module memoryTest

    Sub Main1()

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
