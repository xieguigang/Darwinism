Namespace Framework.DynamicCode

    Public Class AnonymousType

        <Xml.Serialization.XmlArray> Public Property Properties As [Property]()

        Default Public ReadOnly Property [Property](Name As String) As Object
            Get
                Dim LQuery = From p As [Property] In Me.Properties
                             Where String.Equals(Name, p.Name, StringComparison.OrdinalIgnoreCase)
                             Select p '
                Dim result As [Property] = LQuery.FirstOrDefault
                Return result
            End Get
        End Property
    End Class

    Public Class [Property]
        <Xml.Serialization.XmlAttribute> Public Property Name As String
        <Xml.Serialization.XmlElement> Public Property Value As Object

        Public Overrides Function ToString() As String
            Return String.Format("{0}:={1}", Name, Value.ToString)
        End Function
    End Class
End Namespace