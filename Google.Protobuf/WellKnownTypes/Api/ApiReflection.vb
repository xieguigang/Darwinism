#Region "Microsoft.VisualBasic::d14f9b3835d0e6e63fd2a3250d71eb16, Google.Protobuf\WellKnownTypes\Api\ApiReflection.vb"

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

    '     Module ApiReflection
    ' 
    '         Properties: Descriptor
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Language
Imports pbc = Google.Protobuf.Collections
Imports pbr = Google.Protobuf.Reflection

Namespace Google.Protobuf.WellKnownTypes

    ''' <summary>Holder for reflection information generated from google/protobuf/api.proto</summary>
    Partial Public Module ApiReflection

#Region "Descriptor"
        ''' <summary>File descriptor for google/protobuf/api.proto</summary>
        Public ReadOnly Property Descriptor As pbr.FileDescriptor
            Get
                Return descriptorField
            End Get
        End Property

        Private descriptorField As pbr.FileDescriptor

        Sub New()
            Dim descriptorData As Byte() = Global.System.Convert.FromBase64String(String.Concat("Chlnb29nbGUvcHJvdG9idWYvYXBpLnByb3RvEg9nb29nbGUucHJvdG9idWYa", "JGdvb2dsZS9wcm90b2J1Zi9zb3VyY2VfY29udGV4dC5wcm90bxoaZ29vZ2xl", "L3Byb3RvYnVmL3R5cGUucHJvdG8igQIKA0FwaRIMCgRuYW1lGAEgASgJEigK", "B21ldGhvZHMYAiADKAsyFy5nb29nbGUucHJvdG9idWYuTWV0aG9kEigKB29w", "dGlvbnMYAyADKAsyFy5nb29nbGUucHJvdG9idWYuT3B0aW9uEg8KB3ZlcnNp", "b24YBCABKAkSNgoOc291cmNlX2NvbnRleHQYBSABKAsyHi5nb29nbGUucHJv", "dG9idWYuU291cmNlQ29udGV4dBImCgZtaXhpbnMYBiADKAsyFi5nb29nbGUu", "cHJvdG9idWYuTWl4aW4SJwoGc3ludGF4GAcgASgOMhcuZ29vZ2xlLnByb3Rv", "YnVmLlN5bnRheCLVAQoGTWV0aG9kEgwKBG5hbWUYASABKAkSGAoQcmVxdWVz", "dF90eXBlX3VybBgCIAEoCRIZChFyZXF1ZXN0X3N0cmVhbWluZxgDIAEoCBIZ", "ChFyZXNwb25zZV90eXBlX3VybBgEIAEoCRIaChJyZXNwb25zZV9zdHJlYW1p", "bmcYBSABKAgSKAoHb3B0aW9ucxgGIAMoCzIXLmdvb2dsZS5wcm90b2J1Zi5P", "cHRpb24SJwoGc3ludGF4GAcgASgOMhcuZ29vZ2xlLnByb3RvYnVmLlN5bnRh", "eCIjCgVNaXhpbhIMCgRuYW1lGAEgASgJEgwKBHJvb3QYAiABKAlCSwoTY29t", "Lmdvb2dsZS5wcm90b2J1ZkIIQXBpUHJvdG9QAaABAaICA0dQQqoCHkdvb2ds", "ZS5Qcm90b2J1Zi5XZWxsS25vd25UeXBlc2IGcHJvdG8z"))
            descriptorField = pbr.FileDescriptor.FromGeneratedCode(descriptorData, New pbr.FileDescriptor() {Global.Google.Protobuf.WellKnownTypes.SourceContextReflection.Descriptor, Global.Google.Protobuf.WellKnownTypes.TypeReflection.Descriptor}, New pbr.GeneratedClrTypeInfo(Nothing, New pbr.GeneratedClrTypeInfo() {New pbr.GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.WellKnownTypes.Api), Global.Google.Protobuf.WellKnownTypes.Api.Parser, {"Name", "Methods", "Options", "Version", "SourceContext", "Mixins", "Syntax"}, Nothing, Nothing, Nothing), New pbr.GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.WellKnownTypes.Method), Global.Google.Protobuf.WellKnownTypes.Method.Parser, {"Name", "RequestTypeUrl", "RequestStreaming", "ResponseTypeUrl", "ResponseStreaming", "Options", "Syntax"}, Nothing, Nothing, Nothing), New pbr.GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.WellKnownTypes.Mixin), Global.Google.Protobuf.WellKnownTypes.Mixin.Parser, {"Name", "Root"}, Nothing, Nothing, Nothing)}))
        End Sub
#End Region

    End Module
End Namespace
