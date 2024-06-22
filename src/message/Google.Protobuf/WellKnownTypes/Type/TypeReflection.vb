#Region "Microsoft.VisualBasic::eeb73aedf7a20d4ae29394a932da1f6b, src\message\Google.Protobuf\WellKnownTypes\Type\TypeReflection.vb"

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

    '   Total Lines: 27
    '    Code Lines: 19 (70.37%)
    ' Comment Lines: 2 (7.41%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 6 (22.22%)
    '     File Size: 4.56 KB


    '     Module TypeReflection
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

    ''' <summary>Holder for reflection information generated from google/protobuf/type.proto</summary>
    Partial Public Module TypeReflection

#Region "Descriptor"
        ''' <summary>File descriptor for google/protobuf/type.proto</summary>
        Public ReadOnly Property Descriptor As pbr.FileDescriptor
            Get
                Return descriptorField
            End Get
        End Property

        Private descriptorField As pbr.FileDescriptor

        Sub New()
            Dim descriptorData As Byte() = Global.System.Convert.FromBase64String(String.Concat("Chpnb29nbGUvcHJvdG9idWYvdHlwZS5wcm90bxIPZ29vZ2xlLnByb3RvYnVm", "Ghlnb29nbGUvcHJvdG9idWYvYW55LnByb3RvGiRnb29nbGUvcHJvdG9idWYv", "c291cmNlX2NvbnRleHQucHJvdG8i1wEKBFR5cGUSDAoEbmFtZRgBIAEoCRIm", "CgZmaWVsZHMYAiADKAsyFi5nb29nbGUucHJvdG9idWYuRmllbGQSDgoGb25l", "b2ZzGAMgAygJEigKB29wdGlvbnMYBCADKAsyFy5nb29nbGUucHJvdG9idWYu", "T3B0aW9uEjYKDnNvdXJjZV9jb250ZXh0GAUgASgLMh4uZ29vZ2xlLnByb3Rv", "YnVmLlNvdXJjZUNvbnRleHQSJwoGc3ludGF4GAYgASgOMhcuZ29vZ2xlLnBy", "b3RvYnVmLlN5bnRheCLVBQoFRmllbGQSKQoEa2luZBgBIAEoDjIbLmdvb2ds", "ZS5wcm90b2J1Zi5GaWVsZC5LaW5kEjcKC2NhcmRpbmFsaXR5GAIgASgOMiIu", "Z29vZ2xlLnByb3RvYnVmLkZpZWxkLkNhcmRpbmFsaXR5Eg4KBm51bWJlchgD", "IAEoBRIMCgRuYW1lGAQgASgJEhAKCHR5cGVfdXJsGAYgASgJEhMKC29uZW9m", "X2luZGV4GAcgASgFEg4KBnBhY2tlZBgIIAEoCBIoCgdvcHRpb25zGAkgAygL", "MhcuZ29vZ2xlLnByb3RvYnVmLk9wdGlvbhIRCglqc29uX25hbWUYCiABKAkS", "FQoNZGVmYXVsdF92YWx1ZRgLIAEoCSLIAgoES2luZBIQCgxUWVBFX1VOS05P", "V04QABIPCgtUWVBFX0RPVUJMRRABEg4KClRZUEVfRkxPQVQQAhIOCgpUWVBF", "X0lOVDY0EAMSDwoLVFlQRV9VSU5UNjQQBBIOCgpUWVBFX0lOVDMyEAUSEAoM", "VFlQRV9GSVhFRDY0EAYSEAoMVFlQRV9GSVhFRDMyEAcSDQoJVFlQRV9CT09M", "EAgSDwoLVFlQRV9TVFJJTkcQCRIOCgpUWVBFX0dST1VQEAoSEAoMVFlQRV9N", "RVNTQUdFEAsSDgoKVFlQRV9CWVRFUxAMEg8KC1RZUEVfVUlOVDMyEA0SDQoJ", "VFlQRV9FTlVNEA4SEQoNVFlQRV9TRklYRUQzMhAPEhEKDVRZUEVfU0ZJWEVE", "NjQQEBIPCgtUWVBFX1NJTlQzMhAREg8KC1RZUEVfU0lOVDY0EBIidAoLQ2Fy", "ZGluYWxpdHkSFwoTQ0FSRElOQUxJVFlfVU5LTk9XThAAEhgKFENBUkRJTkFM", "SVRZX09QVElPTkFMEAESGAoUQ0FSRElOQUxJVFlfUkVRVUlSRUQQAhIYChRD", "QVJESU5BTElUWV9SRVBFQVRFRBADIs4BCgRFbnVtEgwKBG5hbWUYASABKAkS", "LQoJZW51bXZhbHVlGAIgAygLMhouZ29vZ2xlLnByb3RvYnVmLkVudW1WYWx1", "ZRIoCgdvcHRpb25zGAMgAygLMhcuZ29vZ2xlLnByb3RvYnVmLk9wdGlvbhI2", "Cg5zb3VyY2VfY29udGV4dBgEIAEoCzIeLmdvb2dsZS5wcm90b2J1Zi5Tb3Vy", "Y2VDb250ZXh0EicKBnN5bnRheBgFIAEoDjIXLmdvb2dsZS5wcm90b2J1Zi5T", "eW50YXgiUwoJRW51bVZhbHVlEgwKBG5hbWUYASABKAkSDgoGbnVtYmVyGAIg", "ASgFEigKB29wdGlvbnMYAyADKAsyFy5nb29nbGUucHJvdG9idWYuT3B0aW9u", "IjsKBk9wdGlvbhIMCgRuYW1lGAEgASgJEiMKBXZhbHVlGAIgASgLMhQuZ29v", "Z2xlLnByb3RvYnVmLkFueSouCgZTeW50YXgSEQoNU1lOVEFYX1BST1RPMhAA", "EhEKDVNZTlRBWF9QUk9UTzMQAUJMChNjb20uZ29vZ2xlLnByb3RvYnVmQglU", "eXBlUHJvdG9QAaABAaICA0dQQqoCHkdvb2dsZS5Qcm90b2J1Zi5XZWxsS25v", "d25UeXBlc2IGcHJvdG8z"))
            descriptorField = pbr.FileDescriptor.FromGeneratedCode(descriptorData, New pbr.FileDescriptor() {Global.Google.Protobuf.WellKnownTypes.AnyReflection.Descriptor, Global.Google.Protobuf.WellKnownTypes.SourceContextReflection.Descriptor}, New pbr.GeneratedClrTypeInfo({GetType(Global.Google.Protobuf.WellKnownTypes.Syntax)}, New pbr.GeneratedClrTypeInfo() {New pbr.GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.WellKnownTypes.Type), Global.Google.Protobuf.WellKnownTypes.Type.Parser, {"Name", "Fields", "Oneofs", "Options", "SourceContext", "Syntax"}, Nothing, Nothing, Nothing), New pbr.GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.WellKnownTypes.Field), Global.Google.Protobuf.WellKnownTypes.Field.Parser, {"Kind", "Cardinality", "Number", "Name", "TypeUrl", "OneofIndex", "Packed", "Options", "JsonName", "DefaultValue"}, Nothing, {GetType(FieldTypes.Kind), GetType(FieldTypes.Cardinality)}, Nothing), New pbr.GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.WellKnownTypes.Enum), Global.Google.Protobuf.WellKnownTypes.Enum.Parser, {"Name", "Enumvalue", "Options", "SourceContext", "Syntax"}, Nothing, Nothing, Nothing), New pbr.GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.WellKnownTypes.EnumValue), Global.Google.Protobuf.WellKnownTypes.EnumValue.Parser, {"Name", "Number", "Options"}, Nothing, Nothing, Nothing), New pbr.GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.WellKnownTypes.Option), Global.Google.Protobuf.WellKnownTypes.Option.Parser, {"Name", "Value"}, Nothing, Nothing, Nothing)}))
        End Sub
#End Region

    End Module
End Namespace
