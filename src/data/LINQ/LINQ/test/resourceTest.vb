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
