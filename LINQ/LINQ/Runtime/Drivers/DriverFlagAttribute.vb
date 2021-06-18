Namespace Runtime.Drivers

    <AttributeUsage(AttributeTargets.Class, AllowMultiple:=False, Inherited:=True)>
    Public Class DriverFlagAttribute : Inherits Attribute

        Public ReadOnly Property type As String

        Sub New(name As String)
            type = name
        End Sub

        Public Overrides Function ToString() As String
            Return type
        End Function

    End Class
End Namespace