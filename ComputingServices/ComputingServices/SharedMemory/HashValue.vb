Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Microsoft.VisualBasic.Serialization

Namespace SharedMemory

    Public Class HashValue : Implements sIdEnumerable

        Public Property Identifier As String Implements sIdEnumerable.Identifier
        Public Property value As Object
        Public Property Type As TypeInfo

        Sub New(name As String, x As Object)
            Identifier = name
            value = x
            Type = New TypeInfo(x.GetType)
        End Sub

        Public Function GetValueJson() As String
            Return JsonContract.GetJson(value, Type.GetType)
        End Function

        Public Overrides Function ToString() As String
            Return $"Dim {Identifier} As {Type.ToString} = {JsonContract.GetJson(value, Type.GetType(True))}"
        End Function
    End Class

    Public Structure Argv : Implements sIdEnumerable

        Public Property Identifier As String Implements sIdEnumerable.Identifier
        Public Property value As TaskHost.Argv

        Sub New(name As String, x As Object)
            Identifier = name
            value = New TaskHost.Argv(x)
        End Sub

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function
    End Structure
End Namespace