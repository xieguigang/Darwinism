
Imports Microsoft.VisualBasic.Language
Imports pbc = Google.Protobuf.Collections
Imports pbr = Google.Protobuf.Reflection

Namespace Google.Protobuf.WellKnownTypes

    ''' <summary>Holder for reflection information generated from google/protobuf/field_mask.proto</summary>
    Partial Public Module FieldMaskReflection

#Region "Descriptor"
        ''' <summary>File descriptor for google/protobuf/field_mask.proto</summary>
        Public ReadOnly Property Descriptor As pbr.FileDescriptor
            Get
                Return descriptorField
            End Get
        End Property

        Private descriptorField As pbr.FileDescriptor

        Sub New()
            Dim descriptorData As Byte() = Global.System.Convert.FromBase64String(String.Concat("CiBnb29nbGUvcHJvdG9idWYvZmllbGRfbWFzay5wcm90bxIPZ29vZ2xlLnBy", "b3RvYnVmIhoKCUZpZWxkTWFzaxINCgVwYXRocxgBIAMoCUJRChNjb20uZ29v", "Z2xlLnByb3RvYnVmQg5GaWVsZE1hc2tQcm90b1ABoAEBogIDR1BCqgIeR29v", "Z2xlLlByb3RvYnVmLldlbGxLbm93blR5cGVzYgZwcm90bzM="))
            descriptorField = pbr.FileDescriptor.FromGeneratedCode(descriptorData, New pbr.FileDescriptor() {}, New pbr.GeneratedClrTypeInfo(Nothing, New pbr.GeneratedClrTypeInfo() {New pbr.GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.WellKnownTypes.FieldMask), Global.Google.Protobuf.WellKnownTypes.FieldMask.Parser, {"Paths"}, Nothing, Nothing, Nothing)}))
        End Sub
#End Region

    End Module
End Namespace