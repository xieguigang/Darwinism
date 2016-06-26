Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Microsoft.VisualBasic.Serialization
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace SharedMemory

    ''' <summary>
    ''' The shared variable on the remote machine.
    ''' </summary>
    Public Class HashValue : Implements sIdEnumerable

        ''' <summary>
        ''' The variable name
        ''' </summary>
        ''' <returns></returns>
        Public Property Identifier As String Implements sIdEnumerable.Identifier
        ''' <summary>
        ''' variable value
        ''' </summary>
        ''' <returns></returns>
        Public Property value As Object
        ''' <summary>
        ''' Simple type information
        ''' </summary>
        ''' <returns></returns>
        Public Property Type As TypeInfo

        Sub New(name As String, x As Object)
            Identifier = name
            value = x
            Type = New TypeInfo(x.GetType)
        End Sub

        ''' <summary>
        ''' Json serialization for the network transfer.
        ''' </summary>
        ''' <returns></returns>
        Public Function GetValueJson() As String
            Return JsonContract.GetJson(value, Type.GetType(True))
        End Function

        Public Overrides Function ToString() As String
            Return $"Dim {Identifier} As {Type.ToString} = {JsonContract.GetJson(value, Type.GetType(True))}"
        End Function
    End Class

    ''' <summary>
    ''' Variable value for the network transfer
    ''' </summary>
    Public Structure Argv : Implements sIdEnumerable

        ''' <summary>
        ''' The variable name
        ''' </summary>
        ''' <returns></returns>
        Public Property Identifier As String Implements sIdEnumerable.Identifier
        ''' <summary>
        ''' Json value, and the type information is also included in this property.
        ''' </summary>
        ''' <returns></returns>
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