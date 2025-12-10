Imports System.Xml.Serialization

<XmlRoot("ListBucketResult", [Namespace]:="http://s3.amazonaws.com/doc/2006-03-01/")>
Public Class ListBucketResult

    Public Property Name As String
    Public Property Prefix As String
    Public Property Marker As String
    Public Property MaxKeys As Integer
    Public Property IsTruncated As Boolean
    <XmlElement("Contents")> Public Property Contents As Content()

End Class
