#Region "Microsoft.VisualBasic::797ab599e90383ade6a8b54d23e136b1, G:/GCModeller/src/runtime/Darwinism/src/data/LINQ/LINQ//test/resourceTest.vb"

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

    '   Total Lines: 35
    '    Code Lines: 26
    ' Comment Lines: 0
    '   Blank Lines: 9
    '     File Size: 1.66 KB


    ' Module resourceTest
    ' 
    '     Sub: Main, read, write
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.ComponentModel.Ranges
Imports Microsoft.VisualBasic.DataStorage.HDSPack.FileSystem
Imports RQL

Module resourceTest

    Const filepath As String = "./resource.db"

    Sub Main()
        Call write()
        Call read()
        Call Pause()
    End Sub

    Sub write()
        Dim res As New Resource(StreamPack.CreateNewStream(filepath))

        Call res.Add("Although the molecules of water", "the physical and chemical properties of the compound are extraordinarily complicated, and they are not typical of most substances found on Earth.")
        Call res.Add("Water occurs as a liquid", "Water occurs as a liquid on the surface of Earth under normal conditions, which makes it invaluable for transportation, for recreation, and as a habitat for a myriad of plants and animals. The fact that water is readily changed to a vapour (gas) allows it to be transported through the atmosphere from the oceans to inland areas where it condenses and, as rain, nourishes plant and animal life. (See hydrosphere: The hydrologic cycle for a description of the cycle by which water is transferred over Earth.)")

        Call res.Dispose()
    End Sub

    Sub read()
        Dim res As New Resource(New StreamPack(filepath.Open(IO.FileMode.Open, doClear:=False, [readOnly]:=True)))
        Dim maps As New List(Of NumericTagged(Of String))

        maps.AddRange(res.Get("the molecules"))
        maps.AddRange(res.Get("occurs as a liquid"))

        For Each key As NumericTagged(Of String) In maps
            Call Console.WriteLine(res.ReadString(key.value))
        Next
    End Sub
End Module

