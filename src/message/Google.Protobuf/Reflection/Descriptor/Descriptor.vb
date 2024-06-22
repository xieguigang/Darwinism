#Region "Microsoft.VisualBasic::fb6cde20e9c66219cb837069d7f425dc, src\message\Google.Protobuf\Reflection\Descriptor\Descriptor.vb"

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

    '   Total Lines: 6675
    '    Code Lines: 4845 (72.58%)
    ' Comment Lines: 714 (10.70%)
    '    - Xml Docs: 96.22%
    ' 
    '   Blank Lines: 1116 (16.72%)
    '     File Size: 295.12 KB


    '     Module DescriptorReflection
    ' 
    '         Properties: Descriptor
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    '     Class FileDescriptorSet
    ' 
    '         Properties: Descriptor, DescriptorProp, File, Parser
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '         Function: CalculateSize, Clone, (+2 Overloads) Equals, GetHashCode, ToString
    ' 
    '         Sub: (+2 Overloads) MergeFrom, OnConstruction, WriteTo
    ' 
    '     Class FileDescriptorProto
    ' 
    '         Properties: Dependency, Descriptor, DescriptorProp, EnumType, Extension
    '                     MessageType, Name, Options, Package, Parser
    '                     PublicDependency, Service, SourceCodeInfo, Syntax, WeakDependency
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '         Function: CalculateSize, Clone, (+2 Overloads) Equals, GetHashCode, ToString
    ' 
    '         Sub: (+2 Overloads) MergeFrom, OnConstruction, WriteTo
    ' 
    '     Class DescriptorProto
    ' 
    '         Properties: Descriptor, DescriptorProp, EnumType, Extension, ExtensionRange
    '                     Field, Name, NestedType, OneofDecl, Options
    '                     Parser, ReservedName, ReservedRange
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '         Function: CalculateSize, Clone, (+2 Overloads) Equals, GetHashCode, ToString
    ' 
    '         Sub: (+2 Overloads) MergeFrom, OnConstruction, WriteTo
    '         Class Types
    ' 
    ' 
    '             Class ExtensionRange
    ' 
    '                 Properties: [End], Descriptor, DescriptorProp, Parser, Start
    ' 
    '                 Constructor: (+2 Overloads) Sub New
    ' 
    '                 Function: CalculateSize, Clone, (+2 Overloads) Equals, GetHashCode, ToString
    ' 
    '                 Sub: (+2 Overloads) MergeFrom, OnConstruction, WriteTo
    ' 
    '             Class ReservedRange
    ' 
    '                 Properties: [End], Descriptor, DescriptorProp, Parser, Start
    ' 
    '                 Constructor: (+2 Overloads) Sub New
    ' 
    '                 Function: CalculateSize, Clone, (+2 Overloads) Equals, GetHashCode, ToString
    ' 
    '                 Sub: (+2 Overloads) MergeFrom, OnConstruction, WriteTo
    ' 
    ' 
    ' 
    ' 
    ' 
    '     Class FieldDescriptorProto
    ' 
    '         Properties: DefaultValue, Descriptor, DescriptorProp, Extendee, JsonName
    '                     Label, Name, Number, OneofIndex, Options
    '                     Parser, Type, TypeName
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '         Function: CalculateSize, Clone, (+2 Overloads) Equals, GetHashCode, ToString
    ' 
    '         Sub: (+2 Overloads) MergeFrom, OnConstruction, WriteTo
    '         Class Types
    ' 
    ' 
    '             Enum Type
    ' 
    '                 (+6 Overloads) Get, Return
    ' 
    ' 
    ' 
    '             Enum Label
    ' 
    ' 
    ' 
    ' 
    '  
    ' 
    ' 
    ' 
    '  
    ' 
    ' 
    ' 
    '     Class OneofDescriptorProto
    ' 
    '         Properties: Descriptor, DescriptorProp, Name, Options, Parser
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '         Function: CalculateSize, Clone, (+2 Overloads) Equals, GetHashCode, ToString
    ' 
    '         Sub: (+2 Overloads) MergeFrom, OnConstruction, WriteTo
    ' 
    '     Class EnumDescriptorProto
    ' 
    '         Properties: Descriptor, DescriptorProp, Name, Options, Parser
    '                     Value
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '         Function: CalculateSize, Clone, (+2 Overloads) Equals, GetHashCode, ToString
    ' 
    '         Sub: (+2 Overloads) MergeFrom, OnConstruction, WriteTo
    ' 
    '     Class EnumValueDescriptorProto
    ' 
    '         Properties: Descriptor, DescriptorProp, Name, Number, Options
    '                     Parser
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '         Function: CalculateSize, Clone, (+2 Overloads) Equals, GetHashCode, ToString
    ' 
    '         Sub: (+2 Overloads) MergeFrom, OnConstruction, WriteTo
    ' 
    '     Class ServiceDescriptorProto
    ' 
    '         Properties: Descriptor, DescriptorProp, Method, Name, Options
    '                     Parser
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '         Function: CalculateSize, Clone, (+2 Overloads) Equals, GetHashCode, ToString
    ' 
    '         Sub: (+2 Overloads) MergeFrom, OnConstruction, WriteTo
    ' 
    '     Class MethodDescriptorProto
    ' 
    '         Properties: ClientStreaming, Descriptor, DescriptorProp, InputType, Name
    '                     Options, OutputType, Parser, ServerStreaming
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '         Function: CalculateSize, Clone, (+2 Overloads) Equals, GetHashCode, ToString
    ' 
    '         Sub: (+2 Overloads) MergeFrom, OnConstruction, WriteTo
    ' 
    '     Class FileOptions
    ' 
    '         Properties: CcEnableArenas, CcGenericServices, CsharpNamespace, Deprecated, Descriptor
    '                     DescriptorProp, GoPackage, JavaGenerateEqualsAndHash, JavaGenericServices, JavaMultipleFiles
    '                     JavaOuterClassname, JavaPackage, JavaStringCheckUtf8, ObjcClassPrefix, OptimizeFor
    '                     Parser, PyGenericServices, UninterpretedOption
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '         Function: CalculateSize, Clone, (+2 Overloads) Equals, GetHashCode, ToString
    ' 
    '         Sub: (+2 Overloads) MergeFrom, OnConstruction, WriteTo
    '         Class Types
    ' 
    ' 
    '             Enum OptimizeMode
    ' 
    ' 
    ' 
    ' 
    ' 
    ' 
    '  
    ' 
    ' 
    ' 
    '     Class MessageOptions
    ' 
    '         Properties: Deprecated, Descriptor, DescriptorProp, MapEntry, MessageSetWireFormat
    '                     NoStandardDescriptorAccessor, Parser, UninterpretedOption
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '         Function: CalculateSize, Clone, (+2 Overloads) Equals, GetHashCode, ToString
    ' 
    '         Sub: (+2 Overloads) MergeFrom, OnConstruction, WriteTo
    ' 
    '     Class FieldOptions
    ' 
    '         Properties: [Ctype], Deprecated, Descriptor, DescriptorProp, Jstype
    '                     Lazy, Packed, Parser, UninterpretedOption, Weak
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '         Function: CalculateSize, Clone, (+2 Overloads) Equals, GetHashCode, ToString
    ' 
    '         Sub: (+2 Overloads) MergeFrom, OnConstruction, WriteTo
    '         Class Types
    ' 
    ' 
    '             Enum [CType]
    ' 
    '                 (+6 Overloads) Get, Return
    ' 
    ' 
    ' 
    '             Enum JSType
    ' 
    ' 
    ' 
    ' 
    '  
    ' 
    ' 
    ' 
    '  
    ' 
    ' 
    ' 
    '     Class OneofOptions
    ' 
    '         Properties: Descriptor, DescriptorProp, Parser, UninterpretedOption
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '         Function: CalculateSize, Clone, (+2 Overloads) Equals, GetHashCode, ToString
    ' 
    '         Sub: (+2 Overloads) MergeFrom, OnConstruction, WriteTo
    ' 
    '     Class EnumOptions
    ' 
    '         Properties: AllowAlias, Deprecated, Descriptor, DescriptorProp, Parser
    '                     UninterpretedOption
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '         Function: CalculateSize, Clone, (+2 Overloads) Equals, GetHashCode, ToString
    ' 
    '         Sub: (+2 Overloads) MergeFrom, OnConstruction, WriteTo
    ' 
    '     Class EnumValueOptions
    ' 
    '         Properties: Deprecated, Descriptor, DescriptorProp, Parser, UninterpretedOption
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '         Function: CalculateSize, Clone, (+2 Overloads) Equals, GetHashCode, ToString
    ' 
    '         Sub: (+2 Overloads) MergeFrom, OnConstruction, WriteTo
    ' 
    '     Class ServiceOptions
    ' 
    '         Properties: Deprecated, Descriptor, DescriptorProp, Parser, UninterpretedOption
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '         Function: CalculateSize, Clone, (+2 Overloads) Equals, GetHashCode, ToString
    ' 
    '         Sub: (+2 Overloads) MergeFrom, OnConstruction, WriteTo
    ' 
    '     Class MethodOptions
    ' 
    '         Properties: Deprecated, Descriptor, DescriptorProp, Parser, UninterpretedOption
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '         Function: CalculateSize, Clone, (+2 Overloads) Equals, GetHashCode, ToString
    ' 
    '         Sub: (+2 Overloads) MergeFrom, OnConstruction, WriteTo
    ' 
    '     Class UninterpretedOption
    ' 
    '         Properties: AggregateValue, Descriptor, DescriptorProp, DoubleValue, IdentifierValue
    '                     Name, NegativeIntValue, Parser, PositiveIntValue, StringValue
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '         Function: CalculateSize, Clone, (+2 Overloads) Equals, GetHashCode, ToString
    ' 
    '         Sub: (+2 Overloads) MergeFrom, OnConstruction, WriteTo
    '         Class Types
    ' 
    ' 
    '             Class NamePart
    ' 
    '                 Properties: Descriptor, DescriptorProp, IsExtension, NamePart_, Parser
    ' 
    '                 Constructor: (+2 Overloads) Sub New
    ' 
    '                 Function: CalculateSize, Clone, (+2 Overloads) Equals, GetHashCode, ToString
    ' 
    '                 Sub: (+2 Overloads) MergeFrom, OnConstruction, WriteTo
    ' 
    ' 
    ' 
    ' 
    ' 
    '     Class SourceCodeInfo
    ' 
    '         Properties: Descriptor, DescriptorProp, Location, Parser
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '         Function: CalculateSize, Clone, (+2 Overloads) Equals, GetHashCode, ToString
    ' 
    '         Sub: (+2 Overloads) MergeFrom, OnConstruction, WriteTo
    '         Class Types
    ' 
    ' 
    '             Class Location
    ' 
    '                 Properties: Descriptor, DescriptorProp, LeadingComments, LeadingDetachedComments, Parser
    '                             Path, Span, TrailingComments
    ' 
    '                 Constructor: (+2 Overloads) Sub New
    ' 
    '                 Function: CalculateSize, Clone, (+2 Overloads) Equals, GetHashCode, ToString
    ' 
    '                 Sub: (+2 Overloads) MergeFrom, OnConstruction, WriteTo
    ' 
    ' 
    ' 
    ' 
    ' 
    '     Class GeneratedCodeInfo
    ' 
    '         Properties: Annotation, Descriptor, DescriptorProp, Parser
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '         Function: CalculateSize, Clone, (+2 Overloads) Equals, GetHashCode, ToString
    ' 
    '         Sub: (+2 Overloads) MergeFrom, OnConstruction, WriteTo
    '         Class Types
    ' 
    ' 
    '             Class Annotation
    ' 
    '                 Properties: [End], Begin, Descriptor, DescriptorProp, Parser
    '                             Path, SourceFile
    ' 
    '                 Constructor: (+2 Overloads) Sub New
    ' 
    '                 Function: CalculateSize, Clone, (+2 Overloads) Equals, GetHashCode, ToString
    ' 
    '                 Sub: (+2 Overloads) MergeFrom, OnConstruction, WriteTo
    ' 
    ' 
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

' Generated by the protocol buffer compiler.  DO NOT EDIT!
' source: google/protobuf/descriptor.proto
#Region "Designer generated code"

Imports Microsoft.VisualBasic.Language
Imports pbc = Google.Protobuf.Collections

Namespace Google.Protobuf.Reflection

    ''' <summary>Holder for reflection information generated from google/protobuf/descriptor.proto</summary>
    Partial Friend Module DescriptorReflection

#Region "Descriptor"
        ''' <summary>File descriptor for google/protobuf/descriptor.proto</summary>
        Public ReadOnly Property Descriptor As FileDescriptor
            Get
                Return descriptorField
            End Get
        End Property

        Private descriptorField As FileDescriptor

        Sub New()
            Dim descriptorData As Byte() = Global.System.Convert.FromBase64String(String.Concat("CiBnb29nbGUvcHJvdG9idWYvZGVzY3JpcHRvci5wcm90bxIPZ29vZ2xlLnBy", "b3RvYnVmIkcKEUZpbGVEZXNjcmlwdG9yU2V0EjIKBGZpbGUYASADKAsyJC5n", "b29nbGUucHJvdG9idWYuRmlsZURlc2NyaXB0b3JQcm90byLbAwoTRmlsZURl", "c2NyaXB0b3JQcm90bxIMCgRuYW1lGAEgASgJEg8KB3BhY2thZ2UYAiABKAkS", "EgoKZGVwZW5kZW5jeRgDIAMoCRIZChFwdWJsaWNfZGVwZW5kZW5jeRgKIAMo", "BRIXCg93ZWFrX2RlcGVuZGVuY3kYCyADKAUSNgoMbWVzc2FnZV90eXBlGAQg", "AygLMiAuZ29vZ2xlLnByb3RvYnVmLkRlc2NyaXB0b3JQcm90bxI3CgllbnVt", "X3R5cGUYBSADKAsyJC5nb29nbGUucHJvdG9idWYuRW51bURlc2NyaXB0b3JQ", "cm90bxI4CgdzZXJ2aWNlGAYgAygLMicuZ29vZ2xlLnByb3RvYnVmLlNlcnZp", "Y2VEZXNjcmlwdG9yUHJvdG8SOAoJZXh0ZW5zaW9uGAcgAygLMiUuZ29vZ2xl", "LnByb3RvYnVmLkZpZWxkRGVzY3JpcHRvclByb3RvEi0KB29wdGlvbnMYCCAB", "KAsyHC5nb29nbGUucHJvdG9idWYuRmlsZU9wdGlvbnMSOQoQc291cmNlX2Nv", "ZGVfaW5mbxgJIAEoCzIfLmdvb2dsZS5wcm90b2J1Zi5Tb3VyY2VDb2RlSW5m", "bxIOCgZzeW50YXgYDCABKAki8AQKD0Rlc2NyaXB0b3JQcm90bxIMCgRuYW1l", "GAEgASgJEjQKBWZpZWxkGAIgAygLMiUuZ29vZ2xlLnByb3RvYnVmLkZpZWxk", "RGVzY3JpcHRvclByb3RvEjgKCWV4dGVuc2lvbhgGIAMoCzIlLmdvb2dsZS5w", "cm90b2J1Zi5GaWVsZERlc2NyaXB0b3JQcm90bxI1CgtuZXN0ZWRfdHlwZRgD", "IAMoCzIgLmdvb2dsZS5wcm90b2J1Zi5EZXNjcmlwdG9yUHJvdG8SNwoJZW51", "bV90eXBlGAQgAygLMiQuZ29vZ2xlLnByb3RvYnVmLkVudW1EZXNjcmlwdG9y", "UHJvdG8SSAoPZXh0ZW5zaW9uX3JhbmdlGAUgAygLMi8uZ29vZ2xlLnByb3Rv", "YnVmLkRlc2NyaXB0b3JQcm90by5FeHRlbnNpb25SYW5nZRI5CgpvbmVvZl9k", "ZWNsGAggAygLMiUuZ29vZ2xlLnByb3RvYnVmLk9uZW9mRGVzY3JpcHRvclBy", "b3RvEjAKB29wdGlvbnMYByABKAsyHy5nb29nbGUucHJvdG9idWYuTWVzc2Fn", "ZU9wdGlvbnMSRgoOcmVzZXJ2ZWRfcmFuZ2UYCSADKAsyLi5nb29nbGUucHJv", "dG9idWYuRGVzY3JpcHRvclByb3RvLlJlc2VydmVkUmFuZ2USFQoNcmVzZXJ2", "ZWRfbmFtZRgKIAMoCRosCg5FeHRlbnNpb25SYW5nZRINCgVzdGFydBgBIAEo", "BRILCgNlbmQYAiABKAUaKwoNUmVzZXJ2ZWRSYW5nZRINCgVzdGFydBgBIAEo", "BRILCgNlbmQYAiABKAUivAUKFEZpZWxkRGVzY3JpcHRvclByb3RvEgwKBG5h", "bWUYASABKAkSDgoGbnVtYmVyGAMgASgFEjoKBWxhYmVsGAQgASgOMisuZ29v", "Z2xlLnByb3RvYnVmLkZpZWxkRGVzY3JpcHRvclByb3RvLkxhYmVsEjgKBHR5", "cGUYBSABKA4yKi5nb29nbGUucHJvdG9idWYuRmllbGREZXNjcmlwdG9yUHJv", "dG8uVHlwZRIRCgl0eXBlX25hbWUYBiABKAkSEAoIZXh0ZW5kZWUYAiABKAkS", "FQoNZGVmYXVsdF92YWx1ZRgHIAEoCRITCgtvbmVvZl9pbmRleBgJIAEoBRIR", "Cglqc29uX25hbWUYCiABKAkSLgoHb3B0aW9ucxgIIAEoCzIdLmdvb2dsZS5w", "cm90b2J1Zi5GaWVsZE9wdGlvbnMitgIKBFR5cGUSDwoLVFlQRV9ET1VCTEUQ", "ARIOCgpUWVBFX0ZMT0FUEAISDgoKVFlQRV9JTlQ2NBADEg8KC1RZUEVfVUlO", "VDY0EAQSDgoKVFlQRV9JTlQzMhAFEhAKDFRZUEVfRklYRUQ2NBAGEhAKDFRZ", "UEVfRklYRUQzMhAHEg0KCVRZUEVfQk9PTBAIEg8KC1RZUEVfU1RSSU5HEAkS", "DgoKVFlQRV9HUk9VUBAKEhAKDFRZUEVfTUVTU0FHRRALEg4KClRZUEVfQllU", "RVMQDBIPCgtUWVBFX1VJTlQzMhANEg0KCVRZUEVfRU5VTRAOEhEKDVRZUEVf", "U0ZJWEVEMzIQDxIRCg1UWVBFX1NGSVhFRDY0EBASDwoLVFlQRV9TSU5UMzIQ", "ERIPCgtUWVBFX1NJTlQ2NBASIkMKBUxhYmVsEhIKDkxBQkVMX09QVElPTkFM", "EAESEgoOTEFCRUxfUkVRVUlSRUQQAhISCg5MQUJFTF9SRVBFQVRFRBADIlQK", "FE9uZW9mRGVzY3JpcHRvclByb3RvEgwKBG5hbWUYASABKAkSLgoHb3B0aW9u", "cxgCIAEoCzIdLmdvb2dsZS5wcm90b2J1Zi5PbmVvZk9wdGlvbnMijAEKE0Vu", "dW1EZXNjcmlwdG9yUHJvdG8SDAoEbmFtZRgBIAEoCRI4CgV2YWx1ZRgCIAMo", "CzIpLmdvb2dsZS5wcm90b2J1Zi5FbnVtVmFsdWVEZXNjcmlwdG9yUHJvdG8S", "LQoHb3B0aW9ucxgDIAEoCzIcLmdvb2dsZS5wcm90b2J1Zi5FbnVtT3B0aW9u", "cyJsChhFbnVtVmFsdWVEZXNjcmlwdG9yUHJvdG8SDAoEbmFtZRgBIAEoCRIO", "CgZudW1iZXIYAiABKAUSMgoHb3B0aW9ucxgDIAEoCzIhLmdvb2dsZS5wcm90", "b2J1Zi5FbnVtVmFsdWVPcHRpb25zIpABChZTZXJ2aWNlRGVzY3JpcHRvclBy", "b3RvEgwKBG5hbWUYASABKAkSNgoGbWV0aG9kGAIgAygLMiYuZ29vZ2xlLnBy", "b3RvYnVmLk1ldGhvZERlc2NyaXB0b3JQcm90bxIwCgdvcHRpb25zGAMgASgL", "Mh8uZ29vZ2xlLnByb3RvYnVmLlNlcnZpY2VPcHRpb25zIsEBChVNZXRob2RE", "ZXNjcmlwdG9yUHJvdG8SDAoEbmFtZRgBIAEoCRISCgppbnB1dF90eXBlGAIg", "ASgJEhMKC291dHB1dF90eXBlGAMgASgJEi8KB29wdGlvbnMYBCABKAsyHi5n", "b29nbGUucHJvdG9idWYuTWV0aG9kT3B0aW9ucxIfChBjbGllbnRfc3RyZWFt", "aW5nGAUgASgIOgVmYWxzZRIfChBzZXJ2ZXJfc3RyZWFtaW5nGAYgASgIOgVm", "YWxzZSKHBQoLRmlsZU9wdGlvbnMSFAoMamF2YV9wYWNrYWdlGAEgASgJEhwK", "FGphdmFfb3V0ZXJfY2xhc3NuYW1lGAggASgJEiIKE2phdmFfbXVsdGlwbGVf", "ZmlsZXMYCiABKAg6BWZhbHNlEiwKHWphdmFfZ2VuZXJhdGVfZXF1YWxzX2Fu", "ZF9oYXNoGBQgASgIOgVmYWxzZRIlChZqYXZhX3N0cmluZ19jaGVja191dGY4", "GBsgASgIOgVmYWxzZRJGCgxvcHRpbWl6ZV9mb3IYCSABKA4yKS5nb29nbGUu", "cHJvdG9idWYuRmlsZU9wdGlvbnMuT3B0aW1pemVNb2RlOgVTUEVFRBISCgpn", "b19wYWNrYWdlGAsgASgJEiIKE2NjX2dlbmVyaWNfc2VydmljZXMYECABKAg6", "BWZhbHNlEiQKFWphdmFfZ2VuZXJpY19zZXJ2aWNlcxgRIAEoCDoFZmFsc2US", "IgoTcHlfZ2VuZXJpY19zZXJ2aWNlcxgSIAEoCDoFZmFsc2USGQoKZGVwcmVj", "YXRlZBgXIAEoCDoFZmFsc2USHwoQY2NfZW5hYmxlX2FyZW5hcxgfIAEoCDoF", "ZmFsc2USGQoRb2JqY19jbGFzc19wcmVmaXgYJCABKAkSGAoQY3NoYXJwX25h", "bWVzcGFjZRglIAEoCRJDChR1bmludGVycHJldGVkX29wdGlvbhjnByADKAsy", "JC5nb29nbGUucHJvdG9idWYuVW5pbnRlcnByZXRlZE9wdGlvbiI6CgxPcHRp", "bWl6ZU1vZGUSCQoFU1BFRUQQARINCglDT0RFX1NJWkUQAhIQCgxMSVRFX1JV", "TlRJTUUQAyoJCOgHEICAgIACSgQIJhAnIuYBCg5NZXNzYWdlT3B0aW9ucxIm", "ChdtZXNzYWdlX3NldF93aXJlX2Zvcm1hdBgBIAEoCDoFZmFsc2USLgofbm9f", "c3RhbmRhcmRfZGVzY3JpcHRvcl9hY2Nlc3NvchgCIAEoCDoFZmFsc2USGQoK", "ZGVwcmVjYXRlZBgDIAEoCDoFZmFsc2USEQoJbWFwX2VudHJ5GAcgASgIEkMK", "FHVuaW50ZXJwcmV0ZWRfb3B0aW9uGOcHIAMoCzIkLmdvb2dsZS5wcm90b2J1", "Zi5VbmludGVycHJldGVkT3B0aW9uKgkI6AcQgICAgAIimAMKDEZpZWxkT3B0", "aW9ucxI6CgVjdHlwZRgBIAEoDjIjLmdvb2dsZS5wcm90b2J1Zi5GaWVsZE9w", "dGlvbnMuQ1R5cGU6BlNUUklORxIOCgZwYWNrZWQYAiABKAgSPwoGanN0eXBl", "GAYgASgOMiQuZ29vZ2xlLnByb3RvYnVmLkZpZWxkT3B0aW9ucy5KU1R5cGU6", "CUpTX05PUk1BTBITCgRsYXp5GAUgASgIOgVmYWxzZRIZCgpkZXByZWNhdGVk", "GAMgASgIOgVmYWxzZRITCgR3ZWFrGAogASgIOgVmYWxzZRJDChR1bmludGVy", "cHJldGVkX29wdGlvbhjnByADKAsyJC5nb29nbGUucHJvdG9idWYuVW5pbnRl", "cnByZXRlZE9wdGlvbiIvCgVDVHlwZRIKCgZTVFJJTkcQABIICgRDT1JEEAES", "EAoMU1RSSU5HX1BJRUNFEAIiNQoGSlNUeXBlEg0KCUpTX05PUk1BTBAAEg0K", "CUpTX1NUUklORxABEg0KCUpTX05VTUJFUhACKgkI6AcQgICAgAIiXgoMT25l", "b2ZPcHRpb25zEkMKFHVuaW50ZXJwcmV0ZWRfb3B0aW9uGOcHIAMoCzIkLmdv", "b2dsZS5wcm90b2J1Zi5VbmludGVycHJldGVkT3B0aW9uKgkI6AcQgICAgAIi", "jQEKC0VudW1PcHRpb25zEhMKC2FsbG93X2FsaWFzGAIgASgIEhkKCmRlcHJl", "Y2F0ZWQYAyABKAg6BWZhbHNlEkMKFHVuaW50ZXJwcmV0ZWRfb3B0aW9uGOcH", "IAMoCzIkLmdvb2dsZS5wcm90b2J1Zi5VbmludGVycHJldGVkT3B0aW9uKgkI", "6AcQgICAgAIifQoQRW51bVZhbHVlT3B0aW9ucxIZCgpkZXByZWNhdGVkGAEg", "ASgIOgVmYWxzZRJDChR1bmludGVycHJldGVkX29wdGlvbhjnByADKAsyJC5n", "b29nbGUucHJvdG9idWYuVW5pbnRlcnByZXRlZE9wdGlvbioJCOgHEICAgIAC", "InsKDlNlcnZpY2VPcHRpb25zEhkKCmRlcHJlY2F0ZWQYISABKAg6BWZhbHNl", "EkMKFHVuaW50ZXJwcmV0ZWRfb3B0aW9uGOcHIAMoCzIkLmdvb2dsZS5wcm90", "b2J1Zi5VbmludGVycHJldGVkT3B0aW9uKgkI6AcQgICAgAIiegoNTWV0aG9k", "T3B0aW9ucxIZCgpkZXByZWNhdGVkGCEgASgIOgVmYWxzZRJDChR1bmludGVy", "cHJldGVkX29wdGlvbhjnByADKAsyJC5nb29nbGUucHJvdG9idWYuVW5pbnRl", "cnByZXRlZE9wdGlvbioJCOgHEICAgIACIp4CChNVbmludGVycHJldGVkT3B0", "aW9uEjsKBG5hbWUYAiADKAsyLS5nb29nbGUucHJvdG9idWYuVW5pbnRlcnBy", "ZXRlZE9wdGlvbi5OYW1lUGFydBIYChBpZGVudGlmaWVyX3ZhbHVlGAMgASgJ", "EhoKEnBvc2l0aXZlX2ludF92YWx1ZRgEIAEoBBIaChJuZWdhdGl2ZV9pbnRf", "dmFsdWUYBSABKAMSFAoMZG91YmxlX3ZhbHVlGAYgASgBEhQKDHN0cmluZ192", "YWx1ZRgHIAEoDBIXCg9hZ2dyZWdhdGVfdmFsdWUYCCABKAkaMwoITmFtZVBh", "cnQSEQoJbmFtZV9wYXJ0GAEgAigJEhQKDGlzX2V4dGVuc2lvbhgCIAIoCCLV", "AQoOU291cmNlQ29kZUluZm8SOgoIbG9jYXRpb24YASADKAsyKC5nb29nbGUu", "cHJvdG9idWYuU291cmNlQ29kZUluZm8uTG9jYXRpb24ahgEKCExvY2F0aW9u", "EhAKBHBhdGgYASADKAVCAhABEhAKBHNwYW4YAiADKAVCAhABEhgKEGxlYWRp", "bmdfY29tbWVudHMYAyABKAkSGQoRdHJhaWxpbmdfY29tbWVudHMYBCABKAkS", "IQoZbGVhZGluZ19kZXRhY2hlZF9jb21tZW50cxgGIAMoCSKnAQoRR2VuZXJh", "dGVkQ29kZUluZm8SQQoKYW5ub3RhdGlvbhgBIAMoCzItLmdvb2dsZS5wcm90", "b2J1Zi5HZW5lcmF0ZWRDb2RlSW5mby5Bbm5vdGF0aW9uGk8KCkFubm90YXRp", "b24SEAoEcGF0aBgBIAMoBUICEAESEwoLc291cmNlX2ZpbGUYAiABKAkSDQoF", "YmVnaW4YAyABKAUSCwoDZW5kGAQgASgFQlsKE2NvbS5nb29nbGUucHJvdG9i", "dWZCEERlc2NyaXB0b3JQcm90b3NIAVoKZGVzY3JpcHRvcqABAaICA0dQQqoC", "Gkdvb2dsZS5Qcm90b2J1Zi5SZWZsZWN0aW9u"))
            descriptorField = FileDescriptor.FromGeneratedCode(descriptorData, New FileDescriptor() {}, New GeneratedClrTypeInfo(Nothing, New GeneratedClrTypeInfo() {New GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.Reflection.FileDescriptorSet), Global.Google.Protobuf.Reflection.FileDescriptorSet.Parser, {"File"}, Nothing, Nothing, Nothing), New GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.Reflection.FileDescriptorProto), Global.Google.Protobuf.Reflection.FileDescriptorProto.Parser, {"Name", "Package", "Dependency", "PublicDependency", "WeakDependency", "MessageType", "EnumType", "Service", "Extension", "Options", "SourceCodeInfo", "Syntax"}, Nothing, Nothing, Nothing), New GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.Reflection.DescriptorProto), Global.Google.Protobuf.Reflection.DescriptorProto.Parser, {"Name", "Field", "Extension", "NestedType", "EnumType", "ExtensionRange", "OneofDecl", "Options", "ReservedRange", "ReservedName"}, Nothing, Nothing, New GeneratedClrTypeInfo() {New GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.Reflection.DescriptorProto.Types.ExtensionRange), Global.Google.Protobuf.Reflection.DescriptorProto.Types.ExtensionRange.Parser, {"Start", "End"}, Nothing, Nothing, Nothing), New GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.Reflection.DescriptorProto.Types.ReservedRange), Global.Google.Protobuf.Reflection.DescriptorProto.Types.ReservedRange.Parser, {"Start", "End"}, Nothing, Nothing, Nothing)}), New GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.Reflection.FieldDescriptorProto), Global.Google.Protobuf.Reflection.FieldDescriptorProto.Parser, {"Name", "Number", "Label", "Type", "TypeName", "Extendee", "DefaultValue", "OneofIndex", "JsonName", "Options"}, Nothing, {GetType(Global.Google.Protobuf.Reflection.FieldDescriptorProto.Types.Type), GetType(Global.Google.Protobuf.Reflection.FieldDescriptorProto.Types.Label)}, Nothing), New GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.Reflection.OneofDescriptorProto), Global.Google.Protobuf.Reflection.OneofDescriptorProto.Parser, {"Name", "Options"}, Nothing, Nothing, Nothing), New GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.Reflection.EnumDescriptorProto), Global.Google.Protobuf.Reflection.EnumDescriptorProto.Parser, {"Name", "Value", "Options"}, Nothing, Nothing, Nothing), New GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.Reflection.EnumValueDescriptorProto), Global.Google.Protobuf.Reflection.EnumValueDescriptorProto.Parser, {"Name", "Number", "Options"}, Nothing, Nothing, Nothing), New GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.Reflection.ServiceDescriptorProto), Global.Google.Protobuf.Reflection.ServiceDescriptorProto.Parser, {"Name", "Method", "Options"}, Nothing, Nothing, Nothing), New GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.Reflection.MethodDescriptorProto), Global.Google.Protobuf.Reflection.MethodDescriptorProto.Parser, {"Name", "InputType", "OutputType", "Options", "ClientStreaming", "ServerStreaming"}, Nothing, Nothing, Nothing), New GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.Reflection.FileOptions), Global.Google.Protobuf.Reflection.FileOptions.Parser, {"JavaPackage", "JavaOuterClassname", "JavaMultipleFiles", "JavaGenerateEqualsAndHash", "JavaStringCheckUtf8", "OptimizeFor", "GoPackage", "CcGenericServices", "JavaGenericServices", "PyGenericServices", "Deprecated", "CcEnableArenas", "ObjcClassPrefix", "CsharpNamespace", "UninterpretedOption"}, Nothing, {GetType(Global.Google.Protobuf.Reflection.FileOptions.Types.OptimizeMode)}, Nothing), New GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.Reflection.MessageOptions), Global.Google.Protobuf.Reflection.MessageOptions.Parser, {"MessageSetWireFormat", "NoStandardDescriptorAccessor", "Deprecated", "MapEntry", "UninterpretedOption"}, Nothing, Nothing, Nothing), New GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.Reflection.FieldOptions), Global.Google.Protobuf.Reflection.FieldOptions.Parser, {"Ctype", "Packed", "Jstype", "Lazy", "Deprecated", "Weak", "UninterpretedOption"}, Nothing, {GetType(Global.Google.Protobuf.Reflection.FieldOptions.Types.CType), GetType(Global.Google.Protobuf.Reflection.FieldOptions.Types.JSType)}, Nothing), New GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.Reflection.OneofOptions), Global.Google.Protobuf.Reflection.OneofOptions.Parser, {"UninterpretedOption"}, Nothing, Nothing, Nothing), New GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.Reflection.EnumOptions), Global.Google.Protobuf.Reflection.EnumOptions.Parser, {"AllowAlias", "Deprecated", "UninterpretedOption"}, Nothing, Nothing, Nothing), New GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.Reflection.EnumValueOptions), Global.Google.Protobuf.Reflection.EnumValueOptions.Parser, {"Deprecated", "UninterpretedOption"}, Nothing, Nothing, Nothing), New GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.Reflection.ServiceOptions), Global.Google.Protobuf.Reflection.ServiceOptions.Parser, {"Deprecated", "UninterpretedOption"}, Nothing, Nothing, Nothing), New GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.Reflection.MethodOptions), Global.Google.Protobuf.Reflection.MethodOptions.Parser, {"Deprecated", "UninterpretedOption"}, Nothing, Nothing, Nothing), New GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.Reflection.UninterpretedOption), Global.Google.Protobuf.Reflection.UninterpretedOption.Parser, {"Name", "IdentifierValue", "PositiveIntValue", "NegativeIntValue", "DoubleValue", "StringValue", "AggregateValue"}, Nothing, Nothing, New GeneratedClrTypeInfo() {New GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.Reflection.UninterpretedOption.Types.NamePart), Global.Google.Protobuf.Reflection.UninterpretedOption.Types.NamePart.Parser, {"NamePart_", "IsExtension"}, Nothing, Nothing, Nothing)}), New GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.Reflection.SourceCodeInfo), Global.Google.Protobuf.Reflection.SourceCodeInfo.Parser, {"Location"}, Nothing, Nothing, New GeneratedClrTypeInfo() {New GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.Reflection.SourceCodeInfo.Types.Location), Global.Google.Protobuf.Reflection.SourceCodeInfo.Types.Location.Parser, {"Path", "Span", "LeadingComments", "TrailingComments", "LeadingDetachedComments"}, Nothing, Nothing, Nothing)}), New GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.Reflection.GeneratedCodeInfo), Global.Google.Protobuf.Reflection.GeneratedCodeInfo.Parser, {"Annotation"}, Nothing, Nothing, New GeneratedClrTypeInfo() {New GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.Reflection.GeneratedCodeInfo.Types.Annotation), Global.Google.Protobuf.Reflection.GeneratedCodeInfo.Types.Annotation.Parser, {"Path", "SourceFile", "Begin", "End"}, Nothing, Nothing, Nothing)})}))
        End Sub
#End Region

    End Module
#Region "Messages"
    ''' <summary>
    '''  The protocol compiler can output a FileDescriptorSet containing the .proto
    '''  files it parses.
    ''' </summary>
    Partial Friend NotInheritable Class FileDescriptorSet
        Implements IMessageType(Of FileDescriptorSet)

        Private Shared ReadOnly _parser As MessageParserType(Of FileDescriptorSet) = New MessageParserType(Of FileDescriptorSet)(Function() New FileDescriptorSet())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of FileDescriptorSet)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As MessageDescriptor
            Get
                Return Global.Google.Protobuf.Reflection.DescriptorReflection.Descriptor.MessageTypes(0)
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Private ReadOnly Property Descriptor As MessageDescriptor Implements IMessage.Descriptor
            Get
                Return DescriptorProp
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New()
            OnConstruction()
        End Sub

        Partial Private Sub OnConstruction()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New(other As FileDescriptorSet)
            Me.New()
            file_ = other.file_.Clone()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As FileDescriptorSet Implements IDeepCloneable(Of FileDescriptorSet).Clone
            Return New FileDescriptorSet(Me)
        End Function

        ''' <summary>Field number for the "file" field.</summary>
        Public Const FileFieldNumber As Integer = 1
        Private Shared ReadOnly _repeated_file_codec As FieldCodecType(Of Global.Google.Protobuf.Reflection.FileDescriptorProto) = FieldCodec.ForMessage(10, Global.Google.Protobuf.Reflection.FileDescriptorProto.Parser)
        Private ReadOnly file_ As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.FileDescriptorProto) = New pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.FileDescriptorProto)()

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property File As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.FileDescriptorProto)
            Get
                Return file_
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, FileDescriptorSet))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As FileDescriptorSet) As Boolean Implements IEquatable(Of FileDescriptorSet).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If

            If ReferenceEquals(other, Me) Then
                Return True
            End If

            If Not file_.Equals(other.file_) Then Return False
            Return True
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function GetHashCode() As Integer
            Dim hash = 1
            hash = hash Xor file_.GetHashCode()
            Return hash
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function ToString() As String
            Return JsonFormatter.ToDiagnosticString(Me)
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
            file_.WriteTo(output, _repeated_file_codec)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0
            size += file_.CalculateSize(_repeated_file_codec)
            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As FileDescriptorSet) Implements IMessageType(Of FileDescriptorSet).MergeFrom
            If other Is Nothing Then
                Return
            End If

            file_.Add(other.file_)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
            Dim tag As New Value(Of UInteger)

            While ((tag = input.ReadTag())) <> 0

                Select Case tag.Value
                    Case 10
                        file_.AddEntriesFrom(input, _repeated_file_codec)

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub
    End Class

    ''' <summary>
    '''  Describes a complete .proto file.
    ''' </summary>
    Partial Friend NotInheritable Class FileDescriptorProto
        Implements IMessageType(Of FileDescriptorProto)

        Private Shared ReadOnly _parser As MessageParserType(Of FileDescriptorProto) = New MessageParserType(Of FileDescriptorProto)(Function() New FileDescriptorProto())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of FileDescriptorProto)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As MessageDescriptor
            Get
                Return Global.Google.Protobuf.Reflection.DescriptorReflection.Descriptor.MessageTypes(1)
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Private ReadOnly Property Descriptor As MessageDescriptor Implements IMessage.Descriptor
            Get
                Return DescriptorProp
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New()
            OnConstruction()
        End Sub

        Partial Private Sub OnConstruction()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New(other As FileDescriptorProto)
            Me.New()
            name_ = other.name_
            package_ = other.package_
            dependency_ = other.dependency_.Clone()
            publicDependency_ = other.publicDependency_.Clone()
            weakDependency_ = other.weakDependency_.Clone()
            messageType_ = other.messageType_.Clone()
            enumType_ = other.enumType_.Clone()
            service_ = other.service_.Clone()
            extension_ = other.extension_.Clone()
            Options = If(other.options_ IsNot Nothing, other.Options.Clone(), Nothing)
            SourceCodeInfo = If(other.sourceCodeInfo_ IsNot Nothing, other.SourceCodeInfo.Clone(), Nothing)
            syntax_ = other.syntax_
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As FileDescriptorProto Implements IDeepCloneable(Of FileDescriptorProto).Clone
            Return New FileDescriptorProto(Me)
        End Function

        ''' <summary>Field number for the "name" field.</summary>
        Public Const NameFieldNumber As Integer = 1
        Private name_ As String = ""
        ''' <summary>
        '''  file name, relative to root of source tree
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Name As String
            Get
                Return name_
            End Get
            Set(value As String)
                name_ = CheckNotNull(value, "value")
            End Set
        End Property

        ''' <summary>Field number for the "package" field.</summary>
        Public Const PackageFieldNumber As Integer = 2
        Private package_ As String = ""
        ''' <summary>
        '''  e.g. "foo", "foo.bar", etc.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Package As String
            Get
                Return package_
            End Get
            Set(value As String)
                package_ = CheckNotNull(value, "value")
            End Set
        End Property

        ''' <summary>Field number for the "dependency" field.</summary>
        Public Const DependencyFieldNumber As Integer = 3
        Private Shared ReadOnly _repeated_dependency_codec As FieldCodecType(Of String) = ForString(26)
        Private ReadOnly dependency_ As pbc.RepeatedField(Of String) = New pbc.RepeatedField(Of String)()
        ''' <summary>
        '''  Names of files imported by this file.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property Dependency As pbc.RepeatedField(Of String)
            Get
                Return dependency_
            End Get
        End Property

        ''' <summary>Field number for the "public_dependency" field.</summary>
        Public Const PublicDependencyFieldNumber As Integer = 10
        Private Shared ReadOnly _repeated_publicDependency_codec As FieldCodecType(Of Integer) = ForInt32(80)
        Private ReadOnly publicDependency_ As pbc.RepeatedField(Of Integer) = New pbc.RepeatedField(Of Integer)()
        ''' <summary>
        '''  Indexes of the public imported files in the dependency list above.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property PublicDependency As pbc.RepeatedField(Of Integer)
            Get
                Return publicDependency_
            End Get
        End Property

        ''' <summary>Field number for the "weak_dependency" field.</summary>
        Public Const WeakDependencyFieldNumber As Integer = 11
        Private Shared ReadOnly _repeated_weakDependency_codec As FieldCodecType(Of Integer) = ForInt32(88)
        Private ReadOnly weakDependency_ As pbc.RepeatedField(Of Integer) = New pbc.RepeatedField(Of Integer)()
        ''' <summary>
        '''  Indexes of the weak imported files in the dependency list.
        '''  For Google-internal migration only. Do not use.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property WeakDependency As pbc.RepeatedField(Of Integer)
            Get
                Return weakDependency_
            End Get
        End Property

        ''' <summary>Field number for the "message_type" field.</summary>
        Public Const MessageTypeFieldNumber As Integer = 4
        Private Shared ReadOnly _repeated_messageType_codec As FieldCodecType(Of Global.Google.Protobuf.Reflection.DescriptorProto) = FieldCodec.ForMessage(34, Global.Google.Protobuf.Reflection.DescriptorProto.Parser)
        Private ReadOnly messageType_ As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.DescriptorProto) = New pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.DescriptorProto)()
        ''' <summary>
        '''  All top-level definitions in this file.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property MessageType As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.DescriptorProto)
            Get
                Return messageType_
            End Get
        End Property

        ''' <summary>Field number for the "enum_type" field.</summary>
        Public Const EnumTypeFieldNumber As Integer = 5
        Private Shared ReadOnly _repeated_enumType_codec As FieldCodecType(Of Global.Google.Protobuf.Reflection.EnumDescriptorProto) = FieldCodec.ForMessage(42, Global.Google.Protobuf.Reflection.EnumDescriptorProto.Parser)
        Private ReadOnly enumType_ As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.EnumDescriptorProto) = New pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.EnumDescriptorProto)()

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property EnumType As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.EnumDescriptorProto)
            Get
                Return enumType_
            End Get
        End Property

        ''' <summary>Field number for the "service" field.</summary>
        Public Const ServiceFieldNumber As Integer = 6
        Private Shared ReadOnly _repeated_service_codec As FieldCodecType(Of Global.Google.Protobuf.Reflection.ServiceDescriptorProto) = FieldCodec.ForMessage(50, Global.Google.Protobuf.Reflection.ServiceDescriptorProto.Parser)
        Private ReadOnly service_ As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.ServiceDescriptorProto) = New pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.ServiceDescriptorProto)()

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property Service As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.ServiceDescriptorProto)
            Get
                Return service_
            End Get
        End Property

        ''' <summary>Field number for the "extension" field.</summary>
        Public Const ExtensionFieldNumber As Integer = 7
        Private Shared ReadOnly _repeated_extension_codec As FieldCodecType(Of Global.Google.Protobuf.Reflection.FieldDescriptorProto) = FieldCodec.ForMessage(58, Global.Google.Protobuf.Reflection.FieldDescriptorProto.Parser)
        Private ReadOnly extension_ As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.FieldDescriptorProto) = New pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.FieldDescriptorProto)()

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property Extension As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.FieldDescriptorProto)
            Get
                Return extension_
            End Get
        End Property

        ''' <summary>Field number for the "options" field.</summary>
        Public Const OptionsFieldNumber As Integer = 8
        Private options_ As Global.Google.Protobuf.Reflection.FileOptions

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Options As Global.Google.Protobuf.Reflection.FileOptions
            Get
                Return options_
            End Get
            Set(value As Global.Google.Protobuf.Reflection.FileOptions)
                options_ = value
            End Set
        End Property

        ''' <summary>Field number for the "source_code_info" field.</summary>
        Public Const SourceCodeInfoFieldNumber As Integer = 9
        Private sourceCodeInfo_ As Global.Google.Protobuf.Reflection.SourceCodeInfo
        ''' <summary>
        '''  This field contains optional information about the original source code.
        '''  You may safely remove this entire field without harming runtime
        '''  functionality of the descriptors -- the information is needed only by
        '''  development tools.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property SourceCodeInfo As Global.Google.Protobuf.Reflection.SourceCodeInfo
            Get
                Return sourceCodeInfo_
            End Get
            Set(value As Global.Google.Protobuf.Reflection.SourceCodeInfo)
                sourceCodeInfo_ = value
            End Set
        End Property

        ''' <summary>Field number for the "syntax" field.</summary>
        Public Const SyntaxFieldNumber As Integer = 12
        Private syntax_ As String = ""
        ''' <summary>
        '''  The syntax of the proto file.
        '''  The supported values are "proto2" and "proto3".
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Syntax As String
            Get
                Return syntax_
            End Get
            Set(value As String)
                syntax_ = CheckNotNull(value, "value")
            End Set
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, FileDescriptorProto))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As FileDescriptorProto) As Boolean Implements IEquatable(Of FileDescriptorProto).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If

            If ReferenceEquals(other, Me) Then
                Return True
            End If

            If Not Equals(Name, other.Name) Then Return False
            If Not Equals(Package, other.Package) Then Return False
            If Not dependency_.Equals(other.dependency_) Then Return False
            If Not publicDependency_.Equals(other.publicDependency_) Then Return False
            If Not weakDependency_.Equals(other.weakDependency_) Then Return False
            If Not messageType_.Equals(other.messageType_) Then Return False
            If Not enumType_.Equals(other.enumType_) Then Return False
            If Not service_.Equals(other.service_) Then Return False
            If Not extension_.Equals(other.extension_) Then Return False
            If Not Object.Equals(Options, other.Options) Then Return False
            If Not Object.Equals(SourceCodeInfo, other.SourceCodeInfo) Then Return False
            If Not Equals(Syntax, other.Syntax) Then Return False
            Return True
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function GetHashCode() As Integer
            Dim hash = 1
            If Name.Length <> 0 Then hash = hash Xor Name.GetHashCode()
            If Package.Length <> 0 Then hash = hash Xor Package.GetHashCode()
            hash = hash Xor dependency_.GetHashCode()
            hash = hash Xor publicDependency_.GetHashCode()
            hash = hash Xor weakDependency_.GetHashCode()
            hash = hash Xor messageType_.GetHashCode()
            hash = hash Xor enumType_.GetHashCode()
            hash = hash Xor service_.GetHashCode()
            hash = hash Xor extension_.GetHashCode()
            If options_ IsNot Nothing Then hash = hash Xor Options.GetHashCode()
            If sourceCodeInfo_ IsNot Nothing Then hash = hash Xor SourceCodeInfo.GetHashCode()
            If Syntax.Length <> 0 Then hash = hash Xor Syntax.GetHashCode()
            Return hash
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function ToString() As String
            Return JsonFormatter.ToDiagnosticString(Me)
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
            If Name.Length <> 0 Then
                output.WriteRawTag(10)
                output.WriteString(Name)
            End If

            If Package.Length <> 0 Then
                output.WriteRawTag(18)
                output.WriteString(Package)
            End If

            dependency_.WriteTo(output, _repeated_dependency_codec)
            messageType_.WriteTo(output, _repeated_messageType_codec)
            enumType_.WriteTo(output, _repeated_enumType_codec)
            service_.WriteTo(output, _repeated_service_codec)
            extension_.WriteTo(output, _repeated_extension_codec)

            If options_ IsNot Nothing Then
                output.WriteRawTag(66)
                output.WriteMessage(Options)
            End If

            If sourceCodeInfo_ IsNot Nothing Then
                output.WriteRawTag(74)
                output.WriteMessage(SourceCodeInfo)
            End If

            publicDependency_.WriteTo(output, _repeated_publicDependency_codec)
            weakDependency_.WriteTo(output, _repeated_weakDependency_codec)

            If Syntax.Length <> 0 Then
                output.WriteRawTag(98)
                output.WriteString(Syntax)
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0

            If Name.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeStringSize(Name)
            End If

            If Package.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeStringSize(Package)
            End If

            size += dependency_.CalculateSize(_repeated_dependency_codec)
            size += publicDependency_.CalculateSize(_repeated_publicDependency_codec)
            size += weakDependency_.CalculateSize(_repeated_weakDependency_codec)
            size += messageType_.CalculateSize(_repeated_messageType_codec)
            size += enumType_.CalculateSize(_repeated_enumType_codec)
            size += service_.CalculateSize(_repeated_service_codec)
            size += extension_.CalculateSize(_repeated_extension_codec)

            If options_ IsNot Nothing Then
                size += 1 + CodedOutputStream.ComputeMessageSize(Options)
            End If

            If sourceCodeInfo_ IsNot Nothing Then
                size += 1 + CodedOutputStream.ComputeMessageSize(SourceCodeInfo)
            End If

            If Syntax.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeStringSize(Syntax)
            End If

            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As FileDescriptorProto) Implements IMessageType(Of FileDescriptorProto).MergeFrom
            If other Is Nothing Then
                Return
            End If

            If other.Name.Length <> 0 Then
                Name = other.Name
            End If

            If other.Package.Length <> 0 Then
                Package = other.Package
            End If

            dependency_.Add(other.dependency_)
            publicDependency_.Add(other.publicDependency_)
            weakDependency_.Add(other.weakDependency_)
            messageType_.Add(other.messageType_)
            enumType_.Add(other.enumType_)
            service_.Add(other.service_)
            extension_.Add(other.extension_)

            If other.options_ IsNot Nothing Then
                If options_ Is Nothing Then
                    options_ = New Global.Google.Protobuf.Reflection.FileOptions()
                End If

                Options.MergeFrom(other.Options)
            End If

            If other.sourceCodeInfo_ IsNot Nothing Then
                If sourceCodeInfo_ Is Nothing Then
                    sourceCodeInfo_ = New Global.Google.Protobuf.Reflection.SourceCodeInfo()
                End If

                SourceCodeInfo.MergeFrom(other.SourceCodeInfo)
            End If

            If other.Syntax.Length <> 0 Then
                Syntax = other.Syntax
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
            Dim tag As New Value(Of UInteger)

            While ((tag = input.ReadTag())) <> 0

                Select Case tag.Value
                    Case 10
                        Name = input.ReadString()

                    Case 18
                        Package = input.ReadString()

                    Case 26
                        dependency_.AddEntriesFrom(input, _repeated_dependency_codec)

                    Case 34
                        messageType_.AddEntriesFrom(input, _repeated_messageType_codec)

                    Case 42
                        enumType_.AddEntriesFrom(input, _repeated_enumType_codec)

                    Case 50
                        service_.AddEntriesFrom(input, _repeated_service_codec)

                    Case 58
                        extension_.AddEntriesFrom(input, _repeated_extension_codec)

                    Case 66

                        If options_ Is Nothing Then
                            options_ = New Global.Google.Protobuf.Reflection.FileOptions()
                        End If

                        input.ReadMessage(options_)

                    Case 74

                        If sourceCodeInfo_ Is Nothing Then
                            sourceCodeInfo_ = New Global.Google.Protobuf.Reflection.SourceCodeInfo()
                        End If

                        input.ReadMessage(sourceCodeInfo_)

                    Case 82, 80
                        publicDependency_.AddEntriesFrom(input, _repeated_publicDependency_codec)

                    Case 90, 88
                        weakDependency_.AddEntriesFrom(input, _repeated_weakDependency_codec)

                    Case 98
                        Syntax = input.ReadString()

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub
    End Class

    ''' <summary>
    '''  Describes a message type.
    ''' </summary>
    Partial Friend NotInheritable Class DescriptorProto
        Implements IMessageType(Of DescriptorProto)

        Private Shared ReadOnly _parser As MessageParserType(Of DescriptorProto) = New MessageParserType(Of DescriptorProto)(Function() New DescriptorProto())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of DescriptorProto)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As MessageDescriptor
            Get
                Return Global.Google.Protobuf.Reflection.DescriptorReflection.Descriptor.MessageTypes(2)
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Private ReadOnly Property Descriptor As MessageDescriptor Implements IMessage.Descriptor
            Get
                Return DescriptorProp
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New()
            OnConstruction()
        End Sub

        Partial Private Sub OnConstruction()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New(other As DescriptorProto)
            Me.New()
            name_ = other.name_
            field_ = other.field_.Clone()
            extension_ = other.extension_.Clone()
            nestedType_ = other.nestedType_.Clone()
            enumType_ = other.enumType_.Clone()
            extensionRange_ = other.extensionRange_.Clone()
            oneofDecl_ = other.oneofDecl_.Clone()
            Options = If(other.options_ IsNot Nothing, other.Options.Clone(), Nothing)
            reservedRange_ = other.reservedRange_.Clone()
            reservedName_ = other.reservedName_.Clone()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As DescriptorProto Implements IDeepCloneable(Of DescriptorProto).Clone
            Return New DescriptorProto(Me)
        End Function

        ''' <summary>Field number for the "name" field.</summary>
        Public Const NameFieldNumber As Integer = 1
        Private name_ As String = ""

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Name As String
            Get
                Return name_
            End Get
            Set(value As String)
                name_ = CheckNotNull(value, "value")
            End Set
        End Property

        ''' <summary>Field number for the "field" field.</summary>
        Public Const FieldFieldNumber As Integer = 2
        Private Shared ReadOnly _repeated_field_codec As FieldCodecType(Of Global.Google.Protobuf.Reflection.FieldDescriptorProto) = FieldCodec.ForMessage(18, Global.Google.Protobuf.Reflection.FieldDescriptorProto.Parser)
        Private ReadOnly field_ As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.FieldDescriptorProto) = New pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.FieldDescriptorProto)()

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property Field As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.FieldDescriptorProto)
            Get
                Return field_
            End Get
        End Property

        ''' <summary>Field number for the "extension" field.</summary>
        Public Const ExtensionFieldNumber As Integer = 6
        Private Shared ReadOnly _repeated_extension_codec As FieldCodecType(Of Global.Google.Protobuf.Reflection.FieldDescriptorProto) = FieldCodec.ForMessage(50, Global.Google.Protobuf.Reflection.FieldDescriptorProto.Parser)
        Private ReadOnly extension_ As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.FieldDescriptorProto) = New pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.FieldDescriptorProto)()

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property Extension As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.FieldDescriptorProto)
            Get
                Return extension_
            End Get
        End Property

        ''' <summary>Field number for the "nested_type" field.</summary>
        Public Const NestedTypeFieldNumber As Integer = 3
        Private Shared ReadOnly _repeated_nestedType_codec As FieldCodecType(Of Global.Google.Protobuf.Reflection.DescriptorProto) = FieldCodec.ForMessage(26, Global.Google.Protobuf.Reflection.DescriptorProto.Parser)
        Private ReadOnly nestedType_ As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.DescriptorProto) = New pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.DescriptorProto)()

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property NestedType As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.DescriptorProto)
            Get
                Return nestedType_
            End Get
        End Property

        ''' <summary>Field number for the "enum_type" field.</summary>
        Public Const EnumTypeFieldNumber As Integer = 4
        Private Shared ReadOnly _repeated_enumType_codec As FieldCodecType(Of Global.Google.Protobuf.Reflection.EnumDescriptorProto) = FieldCodec.ForMessage(34, Global.Google.Protobuf.Reflection.EnumDescriptorProto.Parser)
        Private ReadOnly enumType_ As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.EnumDescriptorProto) = New pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.EnumDescriptorProto)()

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property EnumType As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.EnumDescriptorProto)
            Get
                Return enumType_
            End Get
        End Property

        ''' <summary>Field number for the "extension_range" field.</summary>
        Public Const ExtensionRangeFieldNumber As Integer = 5
        Private Shared ReadOnly _repeated_extensionRange_codec As FieldCodecType(Of Global.Google.Protobuf.Reflection.DescriptorProto.Types.ExtensionRange) = FieldCodec.ForMessage(42, Global.Google.Protobuf.Reflection.DescriptorProto.Types.ExtensionRange.Parser)
        Private ReadOnly extensionRange_ As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.DescriptorProto.Types.ExtensionRange) = New pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.DescriptorProto.Types.ExtensionRange)()

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property ExtensionRange As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.DescriptorProto.Types.ExtensionRange)
            Get
                Return extensionRange_
            End Get
        End Property

        ''' <summary>Field number for the "oneof_decl" field.</summary>
        Public Const OneofDeclFieldNumber As Integer = 8
        Private Shared ReadOnly _repeated_oneofDecl_codec As FieldCodecType(Of Global.Google.Protobuf.Reflection.OneofDescriptorProto) = FieldCodec.ForMessage(66, Global.Google.Protobuf.Reflection.OneofDescriptorProto.Parser)
        Private ReadOnly oneofDecl_ As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.OneofDescriptorProto) = New pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.OneofDescriptorProto)()

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property OneofDecl As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.OneofDescriptorProto)
            Get
                Return oneofDecl_
            End Get
        End Property

        ''' <summary>Field number for the "options" field.</summary>
        Public Const OptionsFieldNumber As Integer = 7
        Private options_ As Global.Google.Protobuf.Reflection.MessageOptions

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Options As Global.Google.Protobuf.Reflection.MessageOptions
            Get
                Return options_
            End Get
            Set(value As Global.Google.Protobuf.Reflection.MessageOptions)
                options_ = value
            End Set
        End Property

        ''' <summary>Field number for the "reserved_range" field.</summary>
        Public Const ReservedRangeFieldNumber As Integer = 9
        Private Shared ReadOnly _repeated_reservedRange_codec As FieldCodecType(Of Global.Google.Protobuf.Reflection.DescriptorProto.Types.ReservedRange) = FieldCodec.ForMessage(74, Global.Google.Protobuf.Reflection.DescriptorProto.Types.ReservedRange.Parser)
        Private ReadOnly reservedRange_ As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.DescriptorProto.Types.ReservedRange) = New pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.DescriptorProto.Types.ReservedRange)()

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property ReservedRange As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.DescriptorProto.Types.ReservedRange)
            Get
                Return reservedRange_
            End Get
        End Property

        ''' <summary>Field number for the "reserved_name" field.</summary>
        Public Const ReservedNameFieldNumber As Integer = 10
        Private Shared ReadOnly _repeated_reservedName_codec As FieldCodecType(Of String) = ForString(82)
        Private ReadOnly reservedName_ As pbc.RepeatedField(Of String) = New pbc.RepeatedField(Of String)()
        ''' <summary>
        '''  Reserved field names, which may not be used by fields in the same message.
        '''  A given name may only be reserved once.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property ReservedName As pbc.RepeatedField(Of String)
            Get
                Return reservedName_
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, DescriptorProto))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As DescriptorProto) As Boolean Implements IEquatable(Of DescriptorProto).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If

            If ReferenceEquals(other, Me) Then
                Return True
            End If

            If Not Equals(Name, other.Name) Then Return False
            If Not field_.Equals(other.field_) Then Return False
            If Not extension_.Equals(other.extension_) Then Return False
            If Not nestedType_.Equals(other.nestedType_) Then Return False
            If Not enumType_.Equals(other.enumType_) Then Return False
            If Not extensionRange_.Equals(other.extensionRange_) Then Return False
            If Not oneofDecl_.Equals(other.oneofDecl_) Then Return False
            If Not Object.Equals(Options, other.Options) Then Return False
            If Not reservedRange_.Equals(other.reservedRange_) Then Return False
            If Not reservedName_.Equals(other.reservedName_) Then Return False
            Return True
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function GetHashCode() As Integer
            Dim hash = 1
            If Name.Length <> 0 Then hash = hash Xor Name.GetHashCode()
            hash = hash Xor field_.GetHashCode()
            hash = hash Xor extension_.GetHashCode()
            hash = hash Xor nestedType_.GetHashCode()
            hash = hash Xor enumType_.GetHashCode()
            hash = hash Xor extensionRange_.GetHashCode()
            hash = hash Xor oneofDecl_.GetHashCode()
            If options_ IsNot Nothing Then hash = hash Xor Options.GetHashCode()
            hash = hash Xor reservedRange_.GetHashCode()
            hash = hash Xor reservedName_.GetHashCode()
            Return hash
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function ToString() As String
            Return JsonFormatter.ToDiagnosticString(Me)
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
            If Name.Length <> 0 Then
                output.WriteRawTag(10)
                output.WriteString(Name)
            End If

            field_.WriteTo(output, _repeated_field_codec)
            nestedType_.WriteTo(output, _repeated_nestedType_codec)
            enumType_.WriteTo(output, _repeated_enumType_codec)
            extensionRange_.WriteTo(output, _repeated_extensionRange_codec)
            extension_.WriteTo(output, _repeated_extension_codec)

            If options_ IsNot Nothing Then
                output.WriteRawTag(58)
                output.WriteMessage(Options)
            End If

            oneofDecl_.WriteTo(output, _repeated_oneofDecl_codec)
            reservedRange_.WriteTo(output, _repeated_reservedRange_codec)
            reservedName_.WriteTo(output, _repeated_reservedName_codec)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0

            If Name.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeStringSize(Name)
            End If

            size += field_.CalculateSize(_repeated_field_codec)
            size += extension_.CalculateSize(_repeated_extension_codec)
            size += nestedType_.CalculateSize(_repeated_nestedType_codec)
            size += enumType_.CalculateSize(_repeated_enumType_codec)
            size += extensionRange_.CalculateSize(_repeated_extensionRange_codec)
            size += oneofDecl_.CalculateSize(_repeated_oneofDecl_codec)

            If options_ IsNot Nothing Then
                size += 1 + CodedOutputStream.ComputeMessageSize(Options)
            End If

            size += reservedRange_.CalculateSize(_repeated_reservedRange_codec)
            size += reservedName_.CalculateSize(_repeated_reservedName_codec)
            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As DescriptorProto) Implements IMessageType(Of DescriptorProto).MergeFrom
            If other Is Nothing Then
                Return
            End If

            If other.Name.Length <> 0 Then
                Name = other.Name
            End If

            field_.Add(other.field_)
            extension_.Add(other.extension_)
            nestedType_.Add(other.nestedType_)
            enumType_.Add(other.enumType_)
            extensionRange_.Add(other.extensionRange_)
            oneofDecl_.Add(other.oneofDecl_)

            If other.options_ IsNot Nothing Then
                If options_ Is Nothing Then
                    options_ = New Global.Google.Protobuf.Reflection.MessageOptions()
                End If

                Options.MergeFrom(other.Options)
            End If

            reservedRange_.Add(other.reservedRange_)
            reservedName_.Add(other.reservedName_)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
            Dim tag As New Value(Of UInteger)

            While ((tag = input.ReadTag())) <> 0

                Select Case tag.Value
                    Case 10
                        Name = input.ReadString()

                    Case 18
                        field_.AddEntriesFrom(input, _repeated_field_codec)

                    Case 26
                        nestedType_.AddEntriesFrom(input, _repeated_nestedType_codec)

                    Case 34
                        enumType_.AddEntriesFrom(input, _repeated_enumType_codec)

                    Case 42
                        extensionRange_.AddEntriesFrom(input, _repeated_extensionRange_codec)

                    Case 50
                        extension_.AddEntriesFrom(input, _repeated_extension_codec)

                    Case 58

                        If options_ Is Nothing Then
                            options_ = New Global.Google.Protobuf.Reflection.MessageOptions()
                        End If

                        input.ReadMessage(options_)

                    Case 66
                        oneofDecl_.AddEntriesFrom(input, _repeated_oneofDecl_codec)

                    Case 74
                        reservedRange_.AddEntriesFrom(input, _repeated_reservedRange_codec)

                    Case 82
                        reservedName_.AddEntriesFrom(input, _repeated_reservedName_codec)

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub

#Region "Nested types"
        ''' <summary>Container for nested types declared in the DescriptorProto message type.</summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Partial Public NotInheritable Class Types
            Partial Friend NotInheritable Class ExtensionRange
                Implements IMessageType(Of ExtensionRange)

                Private Shared ReadOnly _parser As MessageParserType(Of ExtensionRange) = New MessageParserType(Of ExtensionRange)(Function() New ExtensionRange())

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Shared ReadOnly Property Parser As MessageParserType(Of ExtensionRange)
                    Get
                        Return _parser
                    End Get
                End Property

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Shared ReadOnly Property DescriptorProp As MessageDescriptor
                    Get
                        Return Global.Google.Protobuf.Reflection.DescriptorProto.DescriptorProp.NestedTypes(0)
                    End Get
                End Property

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Private ReadOnly Property Descriptor As MessageDescriptor Implements IMessage.Descriptor
                    Get
                        Return DescriptorProp
                    End Get
                End Property

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Sub New()
                    OnConstruction()
                End Sub

                Partial Private Sub OnConstruction()
                End Sub

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Sub New(other As ExtensionRange)
                    Me.New()
                    start_ = other.start_
                    end_ = other.end_
                End Sub

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Function Clone() As ExtensionRange Implements IDeepCloneable(Of ExtensionRange).Clone
                    Return New ExtensionRange(Me)
                End Function

                ''' <summary>Field number for the "start" field.</summary>
                Public Const StartFieldNumber As Integer = 1
                Private start_ As Integer

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Property Start As Integer
                    Get
                        Return start_
                    End Get
                    Set(value As Integer)
                        start_ = value
                    End Set
                End Property

                ''' <summary>Field number for the "end" field.</summary>
                Public Const EndFieldNumber As Integer = 2
                Private end_ As Integer

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Property [End] As Integer
                    Get
                        Return end_
                    End Get
                    Set(value As Integer)
                        end_ = value
                    End Set
                End Property

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Overrides Function Equals(other As Object) As Boolean
                    Return Equals(TryCast(other, ExtensionRange))
                End Function

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Overloads Function Equals(other As ExtensionRange) As Boolean Implements IEquatable(Of ExtensionRange).Equals
                    If ReferenceEquals(other, Nothing) Then
                        Return False
                    End If

                    If ReferenceEquals(other, Me) Then
                        Return True
                    End If

                    If Start <> other.Start Then Return False
                    If [End] <> other.End Then Return False
                    Return True
                End Function

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Overrides Function GetHashCode() As Integer
                    Dim hash = 1
                    If Start <> 0 Then hash = hash Xor Start.GetHashCode()
                    If [End] <> 0 Then hash = hash Xor [End].GetHashCode()
                    Return hash
                End Function

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Overrides Function ToString() As String
                    Return JsonFormatter.ToDiagnosticString(Me)
                End Function

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
                    If Start <> 0 Then
                        output.WriteRawTag(8)
                        output.WriteInt32(Start)
                    End If

                    If [End] <> 0 Then
                        output.WriteRawTag(16)
                        output.WriteInt32([End])
                    End If
                End Sub

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
                    Dim size = 0

                    If Start <> 0 Then
                        size += 1 + CodedOutputStream.ComputeInt32Size(Start)
                    End If

                    If [End] <> 0 Then
                        size += 1 + CodedOutputStream.ComputeInt32Size([End])
                    End If

                    Return size
                End Function

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Sub MergeFrom(other As ExtensionRange) Implements IMessageType(Of ExtensionRange).MergeFrom
                    If other Is Nothing Then
                        Return
                    End If

                    If other.Start <> 0 Then
                        Start = other.Start
                    End If

                    If other.End <> 0 Then
                        [End] = other.End
                    End If
                End Sub

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
                    Dim tag As New Value(Of UInteger)

                    While ((tag = input.ReadTag())) <> 0

                        Select Case tag.Value
                            Case 8
                                Start = input.ReadInt32()

                            Case 16
                                [End] = input.ReadInt32()

                            Case Else
                                input.SkipLastField()
                        End Select
                    End While
                End Sub
            End Class

            ''' <summary>
            '''  Range of reserved tag numbers. Reserved tag numbers may not be used by
            '''  fields or extension ranges in the same message. Reserved ranges may
            '''  not overlap.
            ''' </summary>
            Partial Friend NotInheritable Class ReservedRange
                Implements IMessageType(Of ReservedRange)

                Private Shared ReadOnly _parser As MessageParserType(Of ReservedRange) = New MessageParserType(Of ReservedRange)(Function() New ReservedRange())

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Shared ReadOnly Property Parser As MessageParserType(Of ReservedRange)
                    Get
                        Return _parser
                    End Get
                End Property

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Shared ReadOnly Property DescriptorProp As MessageDescriptor
                    Get
                        Return Global.Google.Protobuf.Reflection.DescriptorProto.DescriptorProp.NestedTypes(1)
                    End Get
                End Property

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Private ReadOnly Property Descriptor As MessageDescriptor Implements IMessage.Descriptor
                    Get
                        Return DescriptorProp
                    End Get
                End Property

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Sub New()
                    OnConstruction()
                End Sub

                Partial Private Sub OnConstruction()
                End Sub

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Sub New(other As ReservedRange)
                    Me.New()
                    start_ = other.start_
                    end_ = other.end_
                End Sub

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Function Clone() As ReservedRange Implements IDeepCloneable(Of ReservedRange).Clone
                    Return New ReservedRange(Me)
                End Function

                ''' <summary>Field number for the "start" field.</summary>
                Public Const StartFieldNumber As Integer = 1
                Private start_ As Integer
                ''' <summary>
                '''  Inclusive.
                ''' </summary>
                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Property Start As Integer
                    Get
                        Return start_
                    End Get
                    Set(value As Integer)
                        start_ = value
                    End Set
                End Property

                ''' <summary>Field number for the "end" field.</summary>
                Public Const EndFieldNumber As Integer = 2
                Private end_ As Integer
                ''' <summary>
                '''  Exclusive.
                ''' </summary>
                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Property [End] As Integer
                    Get
                        Return end_
                    End Get
                    Set(value As Integer)
                        end_ = value
                    End Set
                End Property

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Overrides Function Equals(other As Object) As Boolean
                    Return Equals(TryCast(other, ReservedRange))
                End Function

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Overloads Function Equals(other As ReservedRange) As Boolean Implements IEquatable(Of ReservedRange).Equals
                    If ReferenceEquals(other, Nothing) Then
                        Return False
                    End If

                    If ReferenceEquals(other, Me) Then
                        Return True
                    End If

                    If Start <> other.Start Then Return False
                    If [End] <> other.End Then Return False
                    Return True
                End Function

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Overrides Function GetHashCode() As Integer
                    Dim hash = 1
                    If Start <> 0 Then hash = hash Xor Start.GetHashCode()
                    If [End] <> 0 Then hash = hash Xor [End].GetHashCode()
                    Return hash
                End Function

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Overrides Function ToString() As String
                    Return JsonFormatter.ToDiagnosticString(Me)
                End Function

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
                    If Start <> 0 Then
                        output.WriteRawTag(8)
                        output.WriteInt32(Start)
                    End If

                    If [End] <> 0 Then
                        output.WriteRawTag(16)
                        output.WriteInt32([End])
                    End If
                End Sub

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
                    Dim size = 0

                    If Start <> 0 Then
                        size += 1 + CodedOutputStream.ComputeInt32Size(Start)
                    End If

                    If [End] <> 0 Then
                        size += 1 + CodedOutputStream.ComputeInt32Size([End])
                    End If

                    Return size
                End Function

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Sub MergeFrom(other As ReservedRange) Implements IMessageType(Of ReservedRange).MergeFrom
                    If other Is Nothing Then
                        Return
                    End If

                    If other.Start <> 0 Then
                        Start = other.Start
                    End If

                    If other.End <> 0 Then
                        [End] = other.End
                    End If
                End Sub

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
                    Dim tag As New Value(Of UInteger)

                    While ((tag = input.ReadTag())) <> 0

                        Select Case tag.Value
                            Case 8
                                Start = input.ReadInt32()

                            Case 16
                                [End] = input.ReadInt32()

                            Case Else
                                input.SkipLastField()
                        End Select
                    End While
                End Sub
            End Class
        End Class
#End Region

    End Class

    ''' <summary>
    '''  Describes a field within a message.
    ''' </summary>
    Partial Friend NotInheritable Class FieldDescriptorProto
        Implements IMessageType(Of FieldDescriptorProto)

        Private Shared ReadOnly _parser As MessageParserType(Of FieldDescriptorProto) = New MessageParserType(Of FieldDescriptorProto)(Function() New FieldDescriptorProto())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of FieldDescriptorProto)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As MessageDescriptor
            Get
                Return Global.Google.Protobuf.Reflection.DescriptorReflection.Descriptor.MessageTypes(3)
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Private ReadOnly Property Descriptor As MessageDescriptor Implements IMessage.Descriptor
            Get
                Return DescriptorProp
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New()
            Me.OnConstruction()
        End Sub

        Partial Private Sub OnConstruction()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New(other As FieldDescriptorProto)
            Me.New()
            name_ = other.name_
            number_ = other.number_
            label_ = other.label_
            type_ = other.type_
            typeName_ = other.typeName_
            extendee_ = other.extendee_
            defaultValue_ = other.defaultValue_
            oneofIndex_ = other.oneofIndex_
            jsonName_ = other.jsonName_
            Options = If(other.options_ IsNot Nothing, other.Options.Clone(), Nothing)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As FieldDescriptorProto Implements IDeepCloneable(Of FieldDescriptorProto).Clone
            Return New FieldDescriptorProto(Me)
        End Function

        ''' <summary>Field number for the "name" field.</summary>
        Public Const NameFieldNumber As Integer = 1
        Private name_ As String = ""

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Name As String
            Get
                Return name_
            End Get
            Set(value As String)
                name_ = CheckNotNull(value, "value")
            End Set
        End Property

        ''' <summary>Field number for the "number" field.</summary>
        Public Const NumberFieldNumber As Integer = 3
        Private number_ As Integer

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Number As Integer
            Get
                Return number_
            End Get
            Set(value As Integer)
                number_ = value
            End Set
        End Property

        ''' <summary>Field number for the "label" field.</summary>
        Public Const LabelFieldNumber As Integer = 4
        Private label_ As Global.Google.Protobuf.Reflection.FieldDescriptorProto.Types.Label = 0

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Label As Global.Google.Protobuf.Reflection.FieldDescriptorProto.Types.Label
            Get
                Return label_
            End Get
            Set(value As Global.Google.Protobuf.Reflection.FieldDescriptorProto.Types.Label)
                label_ = value
            End Set
        End Property

        ''' <summary>Field number for the "type" field.</summary>
        Public Const TypeFieldNumber As Integer = 5
        Private type_ As Global.Google.Protobuf.Reflection.FieldDescriptorProto.Types.Type = 0
        ''' <summary>
        '''  If type_name is set, this need not be set.  If both this and type_name
        '''  are set, this must be one of TYPE_ENUM, TYPE_MESSAGE or TYPE_GROUP.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Type As Global.Google.Protobuf.Reflection.FieldDescriptorProto.Types.Type
            Get
                Return type_
            End Get
            Set(value As Global.Google.Protobuf.Reflection.FieldDescriptorProto.Types.Type)
                type_ = value
            End Set
        End Property

        ''' <summary>Field number for the "type_name" field.</summary>
        Public Const TypeNameFieldNumber As Integer = 6
        Private typeName_ As String = ""
        ''' <summary>
        '''  For message and enum types, this is the name of the type.  If the name
        '''  starts with a '.', it is fully-qualified.  Otherwise, C++-like scoping
        '''  rules are used to find the type (i.e. first the nested types within this
        '''  message are searched, then within the parent, on up to the root
        '''  namespace).
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property TypeName As String
            Get
                Return typeName_
            End Get
            Set(value As String)
                typeName_ = CheckNotNull(value, "value")
            End Set
        End Property

        ''' <summary>Field number for the "extendee" field.</summary>
        Public Const ExtendeeFieldNumber As Integer = 2
        Private extendee_ As String = ""
        ''' <summary>
        '''  For extensions, this is the name of the type being extended.  It is
        '''  resolved in the same manner as type_name.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Extendee As String
            Get
                Return extendee_
            End Get
            Set(value As String)
                extendee_ = CheckNotNull(value, "value")
            End Set
        End Property

        ''' <summary>Field number for the "default_value" field.</summary>
        Public Const DefaultValueFieldNumber As Integer = 7
        Private defaultValue_ As String = ""
        ''' <summary>
        '''  For numeric types, contains the original text representation of the value.
        '''  For booleans, "true" or "false".
        '''  For strings, contains the default text contents (not escaped in any way).
        '''  For bytes, contains the C escaped value.  All bytes >= 128 are escaped.
        '''  TODO(kenton):  Base-64 encode?
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property DefaultValue As String
            Get
                Return defaultValue_
            End Get
            Set(value As String)
                defaultValue_ = CheckNotNull(value, "value")
            End Set
        End Property

        ''' <summary>Field number for the "oneof_index" field.</summary>
        Public Const OneofIndexFieldNumber As Integer = 9
        Private oneofIndex_ As Integer
        ''' <summary>
        '''  If set, gives the index of a oneof in the containing type's oneof_decl
        '''  list.  This field is a member of that oneof.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property OneofIndex As Integer
            Get
                Return oneofIndex_
            End Get
            Set(value As Integer)
                oneofIndex_ = value
            End Set
        End Property

        ''' <summary>Field number for the "json_name" field.</summary>
        Public Const JsonNameFieldNumber As Integer = 10
        Private jsonName_ As String = ""
        ''' <summary>
        '''  JSON name of this field. The value is set by protocol compiler. If the
        '''  user has set a "json_name" option on this field, that option's value
        '''  will be used. Otherwise, it's deduced from the field's name by converting
        '''  it to camelCase.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property JsonName As String
            Get
                Return jsonName_
            End Get
            Set(value As String)
                jsonName_ = CheckNotNull(value, "value")
            End Set
        End Property

        ''' <summary>Field number for the "options" field.</summary>
        Public Const OptionsFieldNumber As Integer = 8
        Private options_ As Global.Google.Protobuf.Reflection.FieldOptions

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Options As Global.Google.Protobuf.Reflection.FieldOptions
            Get
                Return options_
            End Get
            Set(value As Global.Google.Protobuf.Reflection.FieldOptions)
                options_ = value
            End Set
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, FieldDescriptorProto))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As FieldDescriptorProto) As Boolean Implements IEquatable(Of FieldDescriptorProto).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If

            If ReferenceEquals(other, Me) Then
                Return True
            End If

            If Not Equals(Name, other.Name) Then Return False
            If Number <> other.Number Then Return False
            If Label <> other.Label Then Return False
            If Type <> other.Type Then Return False
            If Not Equals(TypeName, other.TypeName) Then Return False
            If Not Equals(Extendee, other.Extendee) Then Return False
            If Not Equals(DefaultValue, other.DefaultValue) Then Return False
            If OneofIndex <> other.OneofIndex Then Return False
            If Not Equals(JsonName, other.JsonName) Then Return False
            If Not Object.Equals(Options, other.Options) Then Return False
            Return True
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function GetHashCode() As Integer
            Dim hash = 1
            If Name.Length <> 0 Then hash = hash Xor Name.GetHashCode()
            If Number <> 0 Then hash = hash Xor Number.GetHashCode()
            If Label <> 0 Then hash = hash Xor Label.GetHashCode()
            If Type <> 0 Then hash = hash Xor Type.GetHashCode()
            If TypeName.Length <> 0 Then hash = hash Xor TypeName.GetHashCode()
            If Extendee.Length <> 0 Then hash = hash Xor Extendee.GetHashCode()
            If DefaultValue.Length <> 0 Then hash = hash Xor DefaultValue.GetHashCode()
            If OneofIndex <> 0 Then hash = hash Xor OneofIndex.GetHashCode()
            If JsonName.Length <> 0 Then hash = hash Xor JsonName.GetHashCode()
            If options_ IsNot Nothing Then hash = hash Xor Options.GetHashCode()
            Return hash
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function ToString() As String
            Return JsonFormatter.ToDiagnosticString(Me)
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
            If Name.Length <> 0 Then
                output.WriteRawTag(10)
                output.WriteString(Name)
            End If

            If Extendee.Length <> 0 Then
                output.WriteRawTag(18)
                output.WriteString(Extendee)
            End If

            If Number <> 0 Then
                output.WriteRawTag(24)
                output.WriteInt32(Number)
            End If

            If Label <> 0 Then
                output.WriteRawTag(32)
                output.WriteEnum(CInt(Label))
            End If

            If Type <> 0 Then
                output.WriteRawTag(40)
                output.WriteEnum(CInt(Type))
            End If

            If TypeName.Length <> 0 Then
                output.WriteRawTag(50)
                output.WriteString(TypeName)
            End If

            If DefaultValue.Length <> 0 Then
                output.WriteRawTag(58)
                output.WriteString(DefaultValue)
            End If

            If options_ IsNot Nothing Then
                output.WriteRawTag(66)
                output.WriteMessage(Options)
            End If

            If OneofIndex <> 0 Then
                output.WriteRawTag(72)
                output.WriteInt32(OneofIndex)
            End If

            If JsonName.Length <> 0 Then
                output.WriteRawTag(82)
                output.WriteString(JsonName)
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0

            If Name.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeStringSize(Name)
            End If

            If Number <> 0 Then
                size += 1 + CodedOutputStream.ComputeInt32Size(Number)
            End If

            If Label <> 0 Then
                size += 1 + CodedOutputStream.ComputeEnumSize(CInt(Label))
            End If

            If Type <> 0 Then
                size += 1 + CodedOutputStream.ComputeEnumSize(CInt(Type))
            End If

            If TypeName.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeStringSize(TypeName)
            End If

            If Extendee.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeStringSize(Extendee)
            End If

            If DefaultValue.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeStringSize(DefaultValue)
            End If

            If OneofIndex <> 0 Then
                size += 1 + CodedOutputStream.ComputeInt32Size(OneofIndex)
            End If

            If JsonName.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeStringSize(JsonName)
            End If

            If options_ IsNot Nothing Then
                size += 1 + CodedOutputStream.ComputeMessageSize(Options)
            End If

            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As FieldDescriptorProto) Implements IMessageType(Of FieldDescriptorProto).MergeFrom
            If other Is Nothing Then
                Return
            End If

            If other.Name.Length <> 0 Then
                Name = other.Name
            End If

            If other.Number <> 0 Then
                Number = other.Number
            End If

            If other.Label <> 0 Then
                Label = other.Label
            End If

            If other.Type <> 0 Then
                Type = other.Type
            End If

            If other.TypeName.Length <> 0 Then
                TypeName = other.TypeName
            End If

            If other.Extendee.Length <> 0 Then
                Extendee = other.Extendee
            End If

            If other.DefaultValue.Length <> 0 Then
                DefaultValue = other.DefaultValue
            End If

            If other.OneofIndex <> 0 Then
                OneofIndex = other.OneofIndex
            End If

            If other.JsonName.Length <> 0 Then
                JsonName = other.JsonName
            End If

            If other.options_ IsNot Nothing Then
                If options_ Is Nothing Then
                    options_ = New Global.Google.Protobuf.Reflection.FieldOptions()
                End If

                Options.MergeFrom(other.Options)
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
            Dim tag As New Value(Of UInteger)

            While ((tag = input.ReadTag())) <> 0

                Select Case tag.Value
                    Case 10
                        Name = input.ReadString()

                    Case 18
                        Extendee = input.ReadString()

                    Case 24
                        Number = input.ReadInt32()

                    Case 32
                        label_ = CType(input.ReadEnum(), Global.Google.Protobuf.Reflection.FieldDescriptorProto.Types.Label)

                    Case 40
                        type_ = CType(input.ReadEnum(), Global.Google.Protobuf.Reflection.FieldDescriptorProto.Types.Type)

                    Case 50
                        TypeName = input.ReadString()

                    Case 58
                        DefaultValue = input.ReadString()

                    Case 66

                        If options_ Is Nothing Then
                            options_ = New Global.Google.Protobuf.Reflection.FieldOptions()
                        End If

                        input.ReadMessage(options_)

                    Case 72
                        OneofIndex = input.ReadInt32()

                    Case 82
                        JsonName = input.ReadString()

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub

#Region "Nested types"
        ''' <summary>Container for nested types declared in the FieldDescriptorProto message type.</summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Partial Public NotInheritable Class Types
            Friend Enum Type
                ''' <summary>
                '''  0 is reserved for errors.
                '''  Order is weird for historical reasons.
                ''' </summary>
                <OriginalName("TYPE_DOUBLE")>
                [Double] = 1
                <OriginalName("TYPE_FLOAT")>
                Float = 2
                ''' <summary>
                '''  Not ZigZag encoded.  Negative numbers take 10 bytes.  Use TYPE_SINT64 if
                '''  negative values are likely.
                ''' </summary>
                <OriginalName("TYPE_INT64")>
                Int64 = 3
                <OriginalName("TYPE_UINT64")>
                Uint64 = 4
                ''' <summary>
                '''  Not ZigZag encoded.  Negative numbers take 10 bytes.  Use TYPE_SINT32 if
                '''  negative values are likely.
                ''' </summary>
                <OriginalName("TYPE_INT32")>
                Int32 = 5
                <OriginalName("TYPE_FIXED64")>
                Fixed64 = 6
                <OriginalName("TYPE_FIXED32")>
                Fixed32 = 7
                <OriginalName("TYPE_BOOL")>
                Bool = 8
                <OriginalName("TYPE_STRING")>
                [String] = 9
                ''' <summary>
                '''  Tag-delimited aggregate.
                ''' </summary>
                <OriginalName("TYPE_GROUP")>
                Group = 10
                ''' <summary>
                '''  Length-delimited aggregate.
                ''' </summary>
                <OriginalName("TYPE_MESSAGE")>
                Message = 11
                ''' <summary>
                '''  New in version 2.
                ''' </summary>
                <OriginalName("TYPE_BYTES")>
                Bytes = 12
                <OriginalName("TYPE_UINT32")>
                Uint32 = 13
                <OriginalName("TYPE_ENUM")>
                [Enum] = 14
                <OriginalName("TYPE_SFIXED32")>
                Sfixed32 = 15
                <OriginalName("TYPE_SFIXED64")>
                Sfixed64 = 16
                ''' <summary>
                '''  Uses ZigZag encoding.
                ''' </summary>
                <OriginalName("TYPE_SINT32")>
                Sint32 = 17
                ''' <summary>
                '''  Uses ZigZag encoding.
                ''' </summary>
                <OriginalName("TYPE_SINT64")>
                Sint64 = 18
            End Enum

            Friend Enum Label
                ''' <summary>
                '''  0 is reserved for errors
                ''' </summary>
                <OriginalName("LABEL_OPTIONAL")>
                [Optional] = 1
                <OriginalName("LABEL_REQUIRED")>
                Required = 2
                ''' <summary>
                '''  TODO(sanjay): Should we add LABEL_MAP?
                ''' </summary>
                <OriginalName("LABEL_REPEATED")>
                Repeated = 3
            End Enum
        End Class
#End Region

    End Class

    ''' <summary>
    '''  Describes a oneof.
    ''' </summary>
    Partial Friend NotInheritable Class OneofDescriptorProto
        Implements IMessageType(Of OneofDescriptorProto)

        Private Shared ReadOnly _parser As MessageParserType(Of OneofDescriptorProto) = New MessageParserType(Of OneofDescriptorProto)(Function() New OneofDescriptorProto())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of OneofDescriptorProto)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As MessageDescriptor
            Get
                Return Global.Google.Protobuf.Reflection.DescriptorReflection.Descriptor.MessageTypes(4)
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Private ReadOnly Property Descriptor As MessageDescriptor Implements IMessage.Descriptor
            Get
                Return DescriptorProp
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New()
            OnConstruction()
        End Sub

        Partial Private Sub OnConstruction()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New(other As OneofDescriptorProto)
            Me.New()
            name_ = other.name_
            Options = If(other.options_ IsNot Nothing, other.Options.Clone(), Nothing)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As OneofDescriptorProto Implements IDeepCloneable(Of OneofDescriptorProto).Clone
            Return New OneofDescriptorProto(Me)
        End Function

        ''' <summary>Field number for the "name" field.</summary>
        Public Const NameFieldNumber As Integer = 1
        Private name_ As String = ""

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Name As String
            Get
                Return name_
            End Get
            Set(value As String)
                name_ = CheckNotNull(value, "value")
            End Set
        End Property

        ''' <summary>Field number for the "options" field.</summary>
        Public Const OptionsFieldNumber As Integer = 2
        Private options_ As Global.Google.Protobuf.Reflection.OneofOptions

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Options As Global.Google.Protobuf.Reflection.OneofOptions
            Get
                Return options_
            End Get
            Set(value As Global.Google.Protobuf.Reflection.OneofOptions)
                options_ = value
            End Set
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, OneofDescriptorProto))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As OneofDescriptorProto) As Boolean Implements IEquatable(Of OneofDescriptorProto).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If

            If ReferenceEquals(other, Me) Then
                Return True
            End If

            If Not Equals(Name, other.Name) Then Return False
            If Not Object.Equals(Options, other.Options) Then Return False
            Return True
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function GetHashCode() As Integer
            Dim hash = 1
            If Name.Length <> 0 Then hash = hash Xor Name.GetHashCode()
            If options_ IsNot Nothing Then hash = hash Xor Options.GetHashCode()
            Return hash
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function ToString() As String
            Return JsonFormatter.ToDiagnosticString(Me)
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
            If Name.Length <> 0 Then
                output.WriteRawTag(10)
                output.WriteString(Name)
            End If

            If options_ IsNot Nothing Then
                output.WriteRawTag(18)
                output.WriteMessage(Options)
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0

            If Name.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeStringSize(Name)
            End If

            If options_ IsNot Nothing Then
                size += 1 + CodedOutputStream.ComputeMessageSize(Options)
            End If

            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As OneofDescriptorProto) Implements IMessageType(Of OneofDescriptorProto).MergeFrom
            If other Is Nothing Then
                Return
            End If

            If other.Name.Length <> 0 Then
                Name = other.Name
            End If

            If other.options_ IsNot Nothing Then
                If options_ Is Nothing Then
                    options_ = New Global.Google.Protobuf.Reflection.OneofOptions()
                End If

                Options.MergeFrom(other.Options)
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
            Dim tag As New Value(Of UInteger)

            While ((tag = input.ReadTag())) <> 0

                Select Case tag.Value
                    Case 10
                        Name = input.ReadString()

                    Case 18

                        If options_ Is Nothing Then
                            options_ = New Global.Google.Protobuf.Reflection.OneofOptions()
                        End If

                        input.ReadMessage(options_)

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub
    End Class

    ''' <summary>
    '''  Describes an enum type.
    ''' </summary>
    Partial Friend NotInheritable Class EnumDescriptorProto
        Implements IMessageType(Of EnumDescriptorProto)

        Private Shared ReadOnly _parser As MessageParserType(Of EnumDescriptorProto) = New MessageParserType(Of EnumDescriptorProto)(Function() New EnumDescriptorProto())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of EnumDescriptorProto)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As MessageDescriptor
            Get
                Return Global.Google.Protobuf.Reflection.DescriptorReflection.Descriptor.MessageTypes(5)
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Private ReadOnly Property Descriptor As MessageDescriptor Implements IMessage.Descriptor
            Get
                Return DescriptorProp
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New()
            OnConstruction()
        End Sub

        Partial Private Sub OnConstruction()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New(other As EnumDescriptorProto)
            Me.New()
            name_ = other.name_
            value_ = other.value_.Clone()
            Options = If(other.options_ IsNot Nothing, other.Options.Clone(), Nothing)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As EnumDescriptorProto Implements IDeepCloneable(Of EnumDescriptorProto).Clone
            Return New EnumDescriptorProto(Me)
        End Function

        ''' <summary>Field number for the "name" field.</summary>
        Public Const NameFieldNumber As Integer = 1
        Private name_ As String = ""

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Name As String
            Get
                Return name_
            End Get
            Set(value As String)
                name_ = CheckNotNull(value, "value")
            End Set
        End Property

        ''' <summary>Field number for the "value" field.</summary>
        Public Const ValueFieldNumber As Integer = 2
        Private Shared ReadOnly _repeated_value_codec As FieldCodecType(Of Global.Google.Protobuf.Reflection.EnumValueDescriptorProto) = FieldCodec.ForMessage(18, Global.Google.Protobuf.Reflection.EnumValueDescriptorProto.Parser)
        Private ReadOnly value_ As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.EnumValueDescriptorProto) = New pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.EnumValueDescriptorProto)()

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property Value As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.EnumValueDescriptorProto)
            Get
                Return value_
            End Get
        End Property

        ''' <summary>Field number for the "options" field.</summary>
        Public Const OptionsFieldNumber As Integer = 3
        Private options_ As Global.Google.Protobuf.Reflection.EnumOptions

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Options As Global.Google.Protobuf.Reflection.EnumOptions
            Get
                Return options_
            End Get
            Set(value As Global.Google.Protobuf.Reflection.EnumOptions)
                options_ = value
            End Set
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, EnumDescriptorProto))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As EnumDescriptorProto) As Boolean Implements IEquatable(Of EnumDescriptorProto).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If

            If ReferenceEquals(other, Me) Then
                Return True
            End If

            If Not Equals(Name, other.Name) Then Return False
            If Not value_.Equals(other.value_) Then Return False
            If Not Object.Equals(Options, other.Options) Then Return False
            Return True
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function GetHashCode() As Integer
            Dim hash = 1
            If Name.Length <> 0 Then hash = hash Xor Name.GetHashCode()
            hash = hash Xor value_.GetHashCode()
            If options_ IsNot Nothing Then hash = hash Xor Options.GetHashCode()
            Return hash
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function ToString() As String
            Return JsonFormatter.ToDiagnosticString(Me)
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
            If Name.Length <> 0 Then
                output.WriteRawTag(10)
                output.WriteString(Name)
            End If

            value_.WriteTo(output, _repeated_value_codec)

            If options_ IsNot Nothing Then
                output.WriteRawTag(26)
                output.WriteMessage(Options)
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0

            If Name.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeStringSize(Name)
            End If

            size += value_.CalculateSize(_repeated_value_codec)

            If options_ IsNot Nothing Then
                size += 1 + CodedOutputStream.ComputeMessageSize(Options)
            End If

            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As EnumDescriptorProto) Implements IMessageType(Of EnumDescriptorProto).MergeFrom
            If other Is Nothing Then
                Return
            End If

            If other.Name.Length <> 0 Then
                Name = other.Name
            End If

            value_.Add(other.value_)

            If other.options_ IsNot Nothing Then
                If options_ Is Nothing Then
                    options_ = New Global.Google.Protobuf.Reflection.EnumOptions()
                End If

                Options.MergeFrom(other.Options)
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
            Dim tag As New Value(Of UInteger)

            While ((tag = input.ReadTag())) <> 0

                Select Case tag.Value
                    Case 10
                        Name = input.ReadString()

                    Case 18
                        value_.AddEntriesFrom(input, _repeated_value_codec)

                    Case 26

                        If options_ Is Nothing Then
                            options_ = New Global.Google.Protobuf.Reflection.EnumOptions()
                        End If

                        input.ReadMessage(options_)

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub
    End Class

    ''' <summary>
    '''  Describes a value within an enum.
    ''' </summary>
    Partial Friend NotInheritable Class EnumValueDescriptorProto
        Implements IMessageType(Of EnumValueDescriptorProto)

        Private Shared ReadOnly _parser As MessageParserType(Of EnumValueDescriptorProto) = New MessageParserType(Of EnumValueDescriptorProto)(Function() New EnumValueDescriptorProto())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of EnumValueDescriptorProto)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As MessageDescriptor
            Get
                Return Global.Google.Protobuf.Reflection.DescriptorReflection.Descriptor.MessageTypes(6)
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Private ReadOnly Property Descriptor As MessageDescriptor Implements IMessage.Descriptor
            Get
                Return DescriptorProp
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New()
            OnConstruction()
        End Sub

        Partial Private Sub OnConstruction()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New(other As EnumValueDescriptorProto)
            Me.New()
            name_ = other.name_
            number_ = other.number_
            Options = If(other.options_ IsNot Nothing, other.Options.Clone(), Nothing)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As EnumValueDescriptorProto Implements IDeepCloneable(Of EnumValueDescriptorProto).Clone
            Return New EnumValueDescriptorProto(Me)
        End Function

        ''' <summary>Field number for the "name" field.</summary>
        Public Const NameFieldNumber As Integer = 1
        Private name_ As String = ""

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Name As String
            Get
                Return name_
            End Get
            Set(value As String)
                name_ = CheckNotNull(value, "value")
            End Set
        End Property

        ''' <summary>Field number for the "number" field.</summary>
        Public Const NumberFieldNumber As Integer = 2
        Private number_ As Integer

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Number As Integer
            Get
                Return number_
            End Get
            Set(value As Integer)
                number_ = value
            End Set
        End Property

        ''' <summary>Field number for the "options" field.</summary>
        Public Const OptionsFieldNumber As Integer = 3
        Private options_ As Global.Google.Protobuf.Reflection.EnumValueOptions

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Options As Global.Google.Protobuf.Reflection.EnumValueOptions
            Get
                Return options_
            End Get
            Set(value As Global.Google.Protobuf.Reflection.EnumValueOptions)
                options_ = value
            End Set
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, EnumValueDescriptorProto))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As EnumValueDescriptorProto) As Boolean Implements IEquatable(Of EnumValueDescriptorProto).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If

            If ReferenceEquals(other, Me) Then
                Return True
            End If

            If Not Equals(Name, other.Name) Then Return False
            If Number <> other.Number Then Return False
            If Not Object.Equals(Options, other.Options) Then Return False
            Return True
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function GetHashCode() As Integer
            Dim hash = 1
            If Name.Length <> 0 Then hash = hash Xor Name.GetHashCode()
            If Number <> 0 Then hash = hash Xor Number.GetHashCode()
            If options_ IsNot Nothing Then hash = hash Xor Options.GetHashCode()
            Return hash
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function ToString() As String
            Return JsonFormatter.ToDiagnosticString(Me)
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
            If Name.Length <> 0 Then
                output.WriteRawTag(10)
                output.WriteString(Name)
            End If

            If Number <> 0 Then
                output.WriteRawTag(16)
                output.WriteInt32(Number)
            End If

            If options_ IsNot Nothing Then
                output.WriteRawTag(26)
                output.WriteMessage(Options)
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0

            If Name.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeStringSize(Name)
            End If

            If Number <> 0 Then
                size += 1 + CodedOutputStream.ComputeInt32Size(Number)
            End If

            If options_ IsNot Nothing Then
                size += 1 + CodedOutputStream.ComputeMessageSize(Options)
            End If

            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As EnumValueDescriptorProto) Implements IMessageType(Of EnumValueDescriptorProto).MergeFrom
            If other Is Nothing Then
                Return
            End If

            If other.Name.Length <> 0 Then
                Name = other.Name
            End If

            If other.Number <> 0 Then
                Number = other.Number
            End If

            If other.options_ IsNot Nothing Then
                If options_ Is Nothing Then
                    options_ = New Global.Google.Protobuf.Reflection.EnumValueOptions()
                End If

                Options.MergeFrom(other.Options)
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
            Dim tag As New Value(Of UInteger)

            While ((tag = input.ReadTag())) <> 0

                Select Case tag.Value
                    Case 10
                        Name = input.ReadString()

                    Case 16
                        Number = input.ReadInt32()

                    Case 26

                        If options_ Is Nothing Then
                            options_ = New Global.Google.Protobuf.Reflection.EnumValueOptions()
                        End If

                        input.ReadMessage(options_)

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub
    End Class

    ''' <summary>
    '''  Describes a service.
    ''' </summary>
    Partial Friend NotInheritable Class ServiceDescriptorProto
        Implements IMessageType(Of ServiceDescriptorProto)

        Private Shared ReadOnly _parser As MessageParserType(Of ServiceDescriptorProto) = New MessageParserType(Of ServiceDescriptorProto)(Function() New ServiceDescriptorProto())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of ServiceDescriptorProto)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As MessageDescriptor
            Get
                Return Global.Google.Protobuf.Reflection.DescriptorReflection.Descriptor.MessageTypes(7)
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Private ReadOnly Property Descriptor As MessageDescriptor Implements IMessage.Descriptor
            Get
                Return DescriptorProp
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New()
            OnConstruction()
        End Sub

        Partial Private Sub OnConstruction()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New(other As ServiceDescriptorProto)
            Me.New()
            name_ = other.name_
            method_ = other.method_.Clone()
            Options = If(other.options_ IsNot Nothing, other.Options.Clone(), Nothing)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As ServiceDescriptorProto Implements IDeepCloneable(Of ServiceDescriptorProto).Clone
            Return New ServiceDescriptorProto(Me)
        End Function

        ''' <summary>Field number for the "name" field.</summary>
        Public Const NameFieldNumber As Integer = 1
        Private name_ As String = ""

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Name As String
            Get
                Return name_
            End Get
            Set(value As String)
                name_ = CheckNotNull(value, "value")
            End Set
        End Property

        ''' <summary>Field number for the "method" field.</summary>
        Public Const MethodFieldNumber As Integer = 2
        Private Shared ReadOnly _repeated_method_codec As FieldCodecType(Of Global.Google.Protobuf.Reflection.MethodDescriptorProto) = FieldCodec.ForMessage(18, Global.Google.Protobuf.Reflection.MethodDescriptorProto.Parser)
        Private ReadOnly method_ As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.MethodDescriptorProto) = New pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.MethodDescriptorProto)()

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property Method As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.MethodDescriptorProto)
            Get
                Return method_
            End Get
        End Property

        ''' <summary>Field number for the "options" field.</summary>
        Public Const OptionsFieldNumber As Integer = 3
        Private options_ As Global.Google.Protobuf.Reflection.ServiceOptions

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Options As Global.Google.Protobuf.Reflection.ServiceOptions
            Get
                Return options_
            End Get
            Set(value As Global.Google.Protobuf.Reflection.ServiceOptions)
                options_ = value
            End Set
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, ServiceDescriptorProto))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As ServiceDescriptorProto) As Boolean Implements IEquatable(Of ServiceDescriptorProto).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If

            If ReferenceEquals(other, Me) Then
                Return True
            End If

            If Not Equals(Name, other.Name) Then Return False
            If Not method_.Equals(other.method_) Then Return False
            If Not Object.Equals(Options, other.Options) Then Return False
            Return True
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function GetHashCode() As Integer
            Dim hash = 1
            If Name.Length <> 0 Then hash = hash Xor Name.GetHashCode()
            hash = hash Xor method_.GetHashCode()
            If options_ IsNot Nothing Then hash = hash Xor Options.GetHashCode()
            Return hash
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function ToString() As String
            Return JsonFormatter.ToDiagnosticString(Me)
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
            If Name.Length <> 0 Then
                output.WriteRawTag(10)
                output.WriteString(Name)
            End If

            method_.WriteTo(output, _repeated_method_codec)

            If options_ IsNot Nothing Then
                output.WriteRawTag(26)
                output.WriteMessage(Options)
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0

            If Name.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeStringSize(Name)
            End If

            size += method_.CalculateSize(_repeated_method_codec)

            If options_ IsNot Nothing Then
                size += 1 + CodedOutputStream.ComputeMessageSize(Options)
            End If

            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As ServiceDescriptorProto) Implements IMessageType(Of ServiceDescriptorProto).MergeFrom
            If other Is Nothing Then
                Return
            End If

            If other.Name.Length <> 0 Then
                Name = other.Name
            End If

            method_.Add(other.method_)

            If other.options_ IsNot Nothing Then
                If options_ Is Nothing Then
                    options_ = New Global.Google.Protobuf.Reflection.ServiceOptions()
                End If

                Options.MergeFrom(other.Options)
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
            Dim tag As New Value(Of UInteger)

            While ((tag = input.ReadTag())) <> 0

                Select Case tag
                    Case 10
                        Name = input.ReadString()

                    Case 18
                        method_.AddEntriesFrom(input, _repeated_method_codec)

                    Case 26

                        If options_ Is Nothing Then
                            options_ = New Global.Google.Protobuf.Reflection.ServiceOptions()
                        End If

                        input.ReadMessage(options_)

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub
    End Class

    ''' <summary>
    '''  Describes a method of a service.
    ''' </summary>
    Partial Friend NotInheritable Class MethodDescriptorProto
        Implements IMessageType(Of MethodDescriptorProto)

        Private Shared ReadOnly _parser As MessageParserType(Of MethodDescriptorProto) = New MessageParserType(Of MethodDescriptorProto)(Function() New MethodDescriptorProto())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of MethodDescriptorProto)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As MessageDescriptor
            Get
                Return Global.Google.Protobuf.Reflection.DescriptorReflection.Descriptor.MessageTypes(8)
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Private ReadOnly Property Descriptor As MessageDescriptor Implements IMessage.Descriptor
            Get
                Return DescriptorProp
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New()
            OnConstruction()
        End Sub

        Partial Private Sub OnConstruction()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New(other As MethodDescriptorProto)
            Me.New()
            name_ = other.name_
            inputType_ = other.inputType_
            outputType_ = other.outputType_
            Options = If(other.options_ IsNot Nothing, other.Options.Clone(), Nothing)
            clientStreaming_ = other.clientStreaming_
            serverStreaming_ = other.serverStreaming_
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As MethodDescriptorProto Implements IDeepCloneable(Of MethodDescriptorProto).Clone
            Return New MethodDescriptorProto(Me)
        End Function

        ''' <summary>Field number for the "name" field.</summary>
        Public Const NameFieldNumber As Integer = 1
        Private name_ As String = ""

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Name As String
            Get
                Return name_
            End Get
            Set(value As String)
                name_ = CheckNotNull(value, "value")
            End Set
        End Property

        ''' <summary>Field number for the "input_type" field.</summary>
        Public Const InputTypeFieldNumber As Integer = 2
        Private inputType_ As String = ""
        ''' <summary>
        '''  Input and output type names.  These are resolved in the same way as
        '''  FieldDescriptorProto.type_name, but must refer to a message type.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property InputType As String
            Get
                Return inputType_
            End Get
            Set(value As String)
                inputType_ = CheckNotNull(value, "value")
            End Set
        End Property

        ''' <summary>Field number for the "output_type" field.</summary>
        Public Const OutputTypeFieldNumber As Integer = 3
        Private outputType_ As String = ""

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property OutputType As String
            Get
                Return outputType_
            End Get
            Set(value As String)
                outputType_ = CheckNotNull(value, "value")
            End Set
        End Property

        ''' <summary>Field number for the "options" field.</summary>
        Public Const OptionsFieldNumber As Integer = 4
        Private options_ As Global.Google.Protobuf.Reflection.MethodOptions

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Options As Global.Google.Protobuf.Reflection.MethodOptions
            Get
                Return options_
            End Get
            Set(value As Global.Google.Protobuf.Reflection.MethodOptions)
                options_ = value
            End Set
        End Property

        ''' <summary>Field number for the "client_streaming" field.</summary>
        Public Const ClientStreamingFieldNumber As Integer = 5
        Private clientStreaming_ As Boolean
        ''' <summary>
        '''  Identifies if client streams multiple client messages
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property ClientStreaming As Boolean
            Get
                Return clientStreaming_
            End Get
            Set(value As Boolean)
                clientStreaming_ = value
            End Set
        End Property

        ''' <summary>Field number for the "server_streaming" field.</summary>
        Public Const ServerStreamingFieldNumber As Integer = 6
        Private serverStreaming_ As Boolean
        ''' <summary>
        '''  Identifies if server streams multiple server messages
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property ServerStreaming As Boolean
            Get
                Return serverStreaming_
            End Get
            Set(value As Boolean)
                serverStreaming_ = value
            End Set
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, MethodDescriptorProto))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As MethodDescriptorProto) As Boolean Implements IEquatable(Of MethodDescriptorProto).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If

            If ReferenceEquals(other, Me) Then
                Return True
            End If

            If Not Equals(Name, other.Name) Then Return False
            If Not Equals(InputType, other.InputType) Then Return False
            If Not Equals(OutputType, other.OutputType) Then Return False
            If Not Object.Equals(Options, other.Options) Then Return False
            If ClientStreaming <> other.ClientStreaming Then Return False
            If ServerStreaming <> other.ServerStreaming Then Return False
            Return True
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function GetHashCode() As Integer
            Dim hash = 1
            If Name.Length <> 0 Then hash = hash Xor Name.GetHashCode()
            If InputType.Length <> 0 Then hash = hash Xor InputType.GetHashCode()
            If OutputType.Length <> 0 Then hash = hash Xor OutputType.GetHashCode()
            If options_ IsNot Nothing Then hash = hash Xor Options.GetHashCode()
            If ClientStreaming <> False Then hash = hash Xor ClientStreaming.GetHashCode()
            If ServerStreaming <> False Then hash = hash Xor ServerStreaming.GetHashCode()
            Return hash
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function ToString() As String
            Return JsonFormatter.ToDiagnosticString(Me)
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
            If Name.Length <> 0 Then
                output.WriteRawTag(10)
                output.WriteString(Name)
            End If

            If InputType.Length <> 0 Then
                output.WriteRawTag(18)
                output.WriteString(InputType)
            End If

            If OutputType.Length <> 0 Then
                output.WriteRawTag(26)
                output.WriteString(OutputType)
            End If

            If options_ IsNot Nothing Then
                output.WriteRawTag(34)
                output.WriteMessage(Options)
            End If

            If ClientStreaming <> False Then
                output.WriteRawTag(40)
                output.WriteBool(ClientStreaming)
            End If

            If ServerStreaming <> False Then
                output.WriteRawTag(48)
                output.WriteBool(ServerStreaming)
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0

            If Name.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeStringSize(Name)
            End If

            If InputType.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeStringSize(InputType)
            End If

            If OutputType.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeStringSize(OutputType)
            End If

            If options_ IsNot Nothing Then
                size += 1 + CodedOutputStream.ComputeMessageSize(Options)
            End If

            If ClientStreaming <> False Then
                size += 1 + 1
            End If

            If ServerStreaming <> False Then
                size += 1 + 1
            End If

            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As MethodDescriptorProto) Implements IMessageType(Of MethodDescriptorProto).MergeFrom
            If other Is Nothing Then
                Return
            End If

            If other.Name.Length <> 0 Then
                Name = other.Name
            End If

            If other.InputType.Length <> 0 Then
                InputType = other.InputType
            End If

            If other.OutputType.Length <> 0 Then
                OutputType = other.OutputType
            End If

            If other.options_ IsNot Nothing Then
                If options_ Is Nothing Then
                    options_ = New Global.Google.Protobuf.Reflection.MethodOptions()
                End If

                Options.MergeFrom(other.Options)
            End If

            If other.ClientStreaming <> False Then
                ClientStreaming = other.ClientStreaming
            End If

            If other.ServerStreaming <> False Then
                ServerStreaming = other.ServerStreaming
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
            Dim tag As New Value(Of UInteger)

            While ((tag = input.ReadTag())) <> 0

                Select Case tag.Value
                    Case 10
                        Name = input.ReadString()

                    Case 18
                        InputType = input.ReadString()

                    Case 26
                        OutputType = input.ReadString()

                    Case 34

                        If options_ Is Nothing Then
                            options_ = New Global.Google.Protobuf.Reflection.MethodOptions()
                        End If

                        input.ReadMessage(options_)

                    Case 40
                        ClientStreaming = input.ReadBool()

                    Case 48
                        ServerStreaming = input.ReadBool()

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub
    End Class

    Partial Friend NotInheritable Class FileOptions
        Implements IMessageType(Of FileOptions)

        Private Shared ReadOnly _parser As MessageParserType(Of FileOptions) = New MessageParserType(Of FileOptions)(Function() New FileOptions())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of FileOptions)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As MessageDescriptor
            Get
                Return Global.Google.Protobuf.Reflection.DescriptorReflection.Descriptor.MessageTypes(9)
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Private ReadOnly Property Descriptor As MessageDescriptor Implements IMessage.Descriptor
            Get
                Return DescriptorProp
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New()
            OnConstruction()
        End Sub

        Partial Private Sub OnConstruction()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New(other As FileOptions)
            Me.New()
            javaPackage_ = other.javaPackage_
            javaOuterClassname_ = other.javaOuterClassname_
            javaMultipleFiles_ = other.javaMultipleFiles_
            javaGenerateEqualsAndHash_ = other.javaGenerateEqualsAndHash_
            javaStringCheckUtf8_ = other.javaStringCheckUtf8_
            optimizeFor_ = other.optimizeFor_
            goPackage_ = other.goPackage_
            ccGenericServices_ = other.ccGenericServices_
            javaGenericServices_ = other.javaGenericServices_
            pyGenericServices_ = other.pyGenericServices_
            deprecated_ = other.deprecated_
            ccEnableArenas_ = other.ccEnableArenas_
            objcClassPrefix_ = other.objcClassPrefix_
            csharpNamespace_ = other.csharpNamespace_
            uninterpretedOption_ = other.uninterpretedOption_.Clone()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As FileOptions Implements IDeepCloneable(Of FileOptions).Clone
            Return New FileOptions(Me)
        End Function

        ''' <summary>Field number for the "java_package" field.</summary>
        Public Const JavaPackageFieldNumber As Integer = 1
        Private javaPackage_ As String = ""
        ''' <summary>
        '''  Sets the Java package where classes generated from this .proto will be
        '''  placed.  By default, the proto package is used, but this is often
        '''  inappropriate because proto packages do not normally start with backwards
        '''  domain names.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property JavaPackage As String
            Get
                Return javaPackage_
            End Get
            Set(value As String)
                javaPackage_ = CheckNotNull(value, "value")
            End Set
        End Property

        ''' <summary>Field number for the "java_outer_classname" field.</summary>
        Public Const JavaOuterClassnameFieldNumber As Integer = 8
        Private javaOuterClassname_ As String = ""
        ''' <summary>
        '''  If set, all the classes from the .proto file are wrapped in a single
        '''  outer class with the given name.  This applies to both Proto1
        '''  (equivalent to the old "--one_java_file" option) and Proto2 (where
        '''  a .proto always translates to a single class, but you may want to
        '''  explicitly choose the class name).
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property JavaOuterClassname As String
            Get
                Return javaOuterClassname_
            End Get
            Set(value As String)
                javaOuterClassname_ = CheckNotNull(value, "value")
            End Set
        End Property

        ''' <summary>Field number for the "java_multiple_files" field.</summary>
        Public Const JavaMultipleFilesFieldNumber As Integer = 10
        Private javaMultipleFiles_ As Boolean
        ''' <summary>
        '''  If set true, then the Java code generator will generate a separate .java
        '''  file for each top-level message, enum, and service defined in the .proto
        '''  file.  Thus, these types will *not* be nested inside the outer class
        '''  named by java_outer_classname.  However, the outer class will still be
        '''  generated to contain the file's getDescriptor() method as well as any
        '''  top-level extensions defined in the file.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property JavaMultipleFiles As Boolean
            Get
                Return javaMultipleFiles_
            End Get
            Set(value As Boolean)
                javaMultipleFiles_ = value
            End Set
        End Property

        ''' <summary>Field number for the "java_generate_equals_and_hash" field.</summary>
        Public Const JavaGenerateEqualsAndHashFieldNumber As Integer = 20
        Private javaGenerateEqualsAndHash_ As Boolean
        ''' <summary>
        '''  If set true, then the Java code generator will generate equals() and
        '''  hashCode() methods for all messages defined in the .proto file.
        '''  This increases generated code size, potentially substantially for large
        '''  protos, which may harm a memory-constrained application.
        '''  - In the full runtime this is a speed optimization, as the
        '''  AbstractMessage base class includes reflection-based implementations of
        '''  these methods.
        '''  - In the lite runtime, setting this option changes the semantics of
        '''  equals() and hashCode() to more closely match those of the full runtime;
        '''  the generated methods compute their results based on field values rather
        '''  than object identity. (Implementations should not assume that hashcodes
        '''  will be consistent across runtimes or versions of the protocol compiler.)
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property JavaGenerateEqualsAndHash As Boolean
            Get
                Return javaGenerateEqualsAndHash_
            End Get
            Set(value As Boolean)
                javaGenerateEqualsAndHash_ = value
            End Set
        End Property

        ''' <summary>Field number for the "java_string_check_utf8" field.</summary>
        Public Const JavaStringCheckUtf8FieldNumber As Integer = 27
        Private javaStringCheckUtf8_ As Boolean
        ''' <summary>
        '''  If set true, then the Java2 code generator will generate code that
        '''  throws an exception whenever an attempt is made to assign a non-UTF-8
        '''  byte sequence to a string field.
        '''  Message reflection will do the same.
        '''  However, an extension field still accepts non-UTF-8 byte sequences.
        '''  This option has no effect on when used with the lite runtime.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property JavaStringCheckUtf8 As Boolean
            Get
                Return javaStringCheckUtf8_
            End Get
            Set(value As Boolean)
                javaStringCheckUtf8_ = value
            End Set
        End Property

        ''' <summary>Field number for the "optimize_for" field.</summary>
        Public Const OptimizeForFieldNumber As Integer = 9
        Private optimizeFor_ As Global.Google.Protobuf.Reflection.FileOptions.Types.OptimizeMode = 0

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property OptimizeFor As Global.Google.Protobuf.Reflection.FileOptions.Types.OptimizeMode
            Get
                Return optimizeFor_
            End Get
            Set(value As Global.Google.Protobuf.Reflection.FileOptions.Types.OptimizeMode)
                optimizeFor_ = value
            End Set
        End Property

        ''' <summary>Field number for the "go_package" field.</summary>
        Public Const GoPackageFieldNumber As Integer = 11
        Private goPackage_ As String = ""
        ''' <summary>
        '''  Sets the Go package where structs generated from this .proto will be
        '''  placed. If omitted, the Go package will be derived from the following:
        '''    - The basename of the package import path, if provided.
        '''    - Otherwise, the package statement in the .proto file, if present.
        '''    - Otherwise, the basename of the .proto file, without extension.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property GoPackage As String
            Get
                Return goPackage_
            End Get
            Set(value As String)
                goPackage_ = CheckNotNull(value, "value")
            End Set
        End Property

        ''' <summary>Field number for the "cc_generic_services" field.</summary>
        Public Const CcGenericServicesFieldNumber As Integer = 16
        Private ccGenericServices_ As Boolean
        ''' <summary>
        '''  Should generic services be generated in each language?  "Generic" services
        '''  are not specific to any particular RPC system.  They are generated by the
        '''  main code generators in each language (without additional plugins).
        '''  Generic services were the only kind of service generation supported by
        '''  early versions of google.protobuf.
        '''
        '''  Generic services are now considered deprecated in favor of using plugins
        '''  that generate code specific to your particular RPC system.  Therefore,
        '''  these default to false.  Old code which depends on generic services should
        '''  explicitly set them to true.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property CcGenericServices As Boolean
            Get
                Return ccGenericServices_
            End Get
            Set(value As Boolean)
                ccGenericServices_ = value
            End Set
        End Property

        ''' <summary>Field number for the "java_generic_services" field.</summary>
        Public Const JavaGenericServicesFieldNumber As Integer = 17
        Private javaGenericServices_ As Boolean

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property JavaGenericServices As Boolean
            Get
                Return javaGenericServices_
            End Get
            Set(value As Boolean)
                javaGenericServices_ = value
            End Set
        End Property

        ''' <summary>Field number for the "py_generic_services" field.</summary>
        Public Const PyGenericServicesFieldNumber As Integer = 18
        Private pyGenericServices_ As Boolean

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property PyGenericServices As Boolean
            Get
                Return pyGenericServices_
            End Get
            Set(value As Boolean)
                pyGenericServices_ = value
            End Set
        End Property

        ''' <summary>Field number for the "deprecated" field.</summary>
        Public Const DeprecatedFieldNumber As Integer = 23
        Private deprecated_ As Boolean
        ''' <summary>
        '''  Is this file deprecated?
        '''  Depending on the target platform, this can emit Deprecated annotations
        '''  for everything in the file, or it will be completely ignored; in the very
        '''  least, this is a formalization for deprecating files.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Deprecated As Boolean
            Get
                Return deprecated_
            End Get
            Set(value As Boolean)
                deprecated_ = value
            End Set
        End Property

        ''' <summary>Field number for the "cc_enable_arenas" field.</summary>
        Public Const CcEnableArenasFieldNumber As Integer = 31
        Private ccEnableArenas_ As Boolean
        ''' <summary>
        '''  Enables the use of arenas for the proto messages in this file. This applies
        '''  only to generated classes for C++.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property CcEnableArenas As Boolean
            Get
                Return ccEnableArenas_
            End Get
            Set(value As Boolean)
                ccEnableArenas_ = value
            End Set
        End Property

        ''' <summary>Field number for the "objc_class_prefix" field.</summary>
        Public Const ObjcClassPrefixFieldNumber As Integer = 36
        Private objcClassPrefix_ As String = ""
        ''' <summary>
        '''  Sets the objective c class prefix which is prepended to all objective c
        '''  generated classes from this .proto. There is no default.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property ObjcClassPrefix As String
            Get
                Return objcClassPrefix_
            End Get
            Set(value As String)
                objcClassPrefix_ = CheckNotNull(value, "value")
            End Set
        End Property

        ''' <summary>Field number for the "csharp_namespace" field.</summary>
        Public Const CsharpNamespaceFieldNumber As Integer = 37
        Private csharpNamespace_ As String = ""
        ''' <summary>
        '''  Namespace for generated classes; defaults to the package.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property CsharpNamespace As String
            Get
                Return csharpNamespace_
            End Get
            Set(value As String)
                csharpNamespace_ = CheckNotNull(value, "value")
            End Set
        End Property

        ''' <summary>Field number for the "uninterpreted_option" field.</summary>
        Public Const UninterpretedOptionFieldNumber As Integer = 999
        Private Shared ReadOnly _repeated_uninterpretedOption_codec As FieldCodecType(Of Global.Google.Protobuf.Reflection.UninterpretedOption) = FieldCodec.ForMessage(7994, Global.Google.Protobuf.Reflection.UninterpretedOption.Parser)
        Private ReadOnly uninterpretedOption_ As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.UninterpretedOption) = New pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.UninterpretedOption)()
        ''' <summary>
        '''  The parser stores options it doesn't recognize here. See above.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property UninterpretedOption As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.UninterpretedOption)
            Get
                Return uninterpretedOption_
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, FileOptions))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As FileOptions) As Boolean Implements IEquatable(Of FileOptions).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If

            If ReferenceEquals(other, Me) Then
                Return True
            End If

            If Not Equals(JavaPackage, other.JavaPackage) Then Return False
            If Not Equals(JavaOuterClassname, other.JavaOuterClassname) Then Return False
            If JavaMultipleFiles <> other.JavaMultipleFiles Then Return False
            If JavaGenerateEqualsAndHash <> other.JavaGenerateEqualsAndHash Then Return False
            If JavaStringCheckUtf8 <> other.JavaStringCheckUtf8 Then Return False
            If OptimizeFor <> other.OptimizeFor Then Return False
            If Not Equals(GoPackage, other.GoPackage) Then Return False
            If CcGenericServices <> other.CcGenericServices Then Return False
            If JavaGenericServices <> other.JavaGenericServices Then Return False
            If PyGenericServices <> other.PyGenericServices Then Return False
            If Deprecated <> other.Deprecated Then Return False
            If CcEnableArenas <> other.CcEnableArenas Then Return False
            If Not Equals(ObjcClassPrefix, other.ObjcClassPrefix) Then Return False
            If Not Equals(CsharpNamespace, other.CsharpNamespace) Then Return False
            If Not uninterpretedOption_.Equals(other.uninterpretedOption_) Then Return False
            Return True
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function GetHashCode() As Integer
            Dim hash = 1
            If JavaPackage.Length <> 0 Then hash = hash Xor JavaPackage.GetHashCode()
            If JavaOuterClassname.Length <> 0 Then hash = hash Xor JavaOuterClassname.GetHashCode()
            If JavaMultipleFiles <> False Then hash = hash Xor JavaMultipleFiles.GetHashCode()
            If JavaGenerateEqualsAndHash <> False Then hash = hash Xor JavaGenerateEqualsAndHash.GetHashCode()
            If JavaStringCheckUtf8 <> False Then hash = hash Xor JavaStringCheckUtf8.GetHashCode()
            If OptimizeFor <> 0 Then hash = hash Xor OptimizeFor.GetHashCode()
            If GoPackage.Length <> 0 Then hash = hash Xor GoPackage.GetHashCode()
            If CcGenericServices <> False Then hash = hash Xor CcGenericServices.GetHashCode()
            If JavaGenericServices <> False Then hash = hash Xor JavaGenericServices.GetHashCode()
            If PyGenericServices <> False Then hash = hash Xor PyGenericServices.GetHashCode()
            If Deprecated <> False Then hash = hash Xor Deprecated.GetHashCode()
            If CcEnableArenas <> False Then hash = hash Xor CcEnableArenas.GetHashCode()
            If ObjcClassPrefix.Length <> 0 Then hash = hash Xor ObjcClassPrefix.GetHashCode()
            If CsharpNamespace.Length <> 0 Then hash = hash Xor CsharpNamespace.GetHashCode()
            hash = hash Xor uninterpretedOption_.GetHashCode()
            Return hash
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function ToString() As String
            Return JsonFormatter.ToDiagnosticString(Me)
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
            If JavaPackage.Length <> 0 Then
                output.WriteRawTag(10)
                output.WriteString(JavaPackage)
            End If

            If JavaOuterClassname.Length <> 0 Then
                output.WriteRawTag(66)
                output.WriteString(JavaOuterClassname)
            End If

            If OptimizeFor <> 0 Then
                output.WriteRawTag(72)
                output.WriteEnum(CInt(OptimizeFor))
            End If

            If JavaMultipleFiles <> False Then
                output.WriteRawTag(80)
                output.WriteBool(JavaMultipleFiles)
            End If

            If GoPackage.Length <> 0 Then
                output.WriteRawTag(90)
                output.WriteString(GoPackage)
            End If

            If CcGenericServices <> False Then
                output.WriteRawTag(128, 1)
                output.WriteBool(CcGenericServices)
            End If

            If JavaGenericServices <> False Then
                output.WriteRawTag(136, 1)
                output.WriteBool(JavaGenericServices)
            End If

            If PyGenericServices <> False Then
                output.WriteRawTag(144, 1)
                output.WriteBool(PyGenericServices)
            End If

            If JavaGenerateEqualsAndHash <> False Then
                output.WriteRawTag(160, 1)
                output.WriteBool(JavaGenerateEqualsAndHash)
            End If

            If Deprecated <> False Then
                output.WriteRawTag(184, 1)
                output.WriteBool(Deprecated)
            End If

            If JavaStringCheckUtf8 <> False Then
                output.WriteRawTag(216, 1)
                output.WriteBool(JavaStringCheckUtf8)
            End If

            If CcEnableArenas <> False Then
                output.WriteRawTag(248, 1)
                output.WriteBool(CcEnableArenas)
            End If

            If ObjcClassPrefix.Length <> 0 Then
                output.WriteRawTag(162, 2)
                output.WriteString(ObjcClassPrefix)
            End If

            If CsharpNamespace.Length <> 0 Then
                output.WriteRawTag(170, 2)
                output.WriteString(CsharpNamespace)
            End If

            uninterpretedOption_.WriteTo(output, _repeated_uninterpretedOption_codec)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0

            If JavaPackage.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeStringSize(JavaPackage)
            End If

            If JavaOuterClassname.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeStringSize(JavaOuterClassname)
            End If

            If JavaMultipleFiles <> False Then
                size += 1 + 1
            End If

            If JavaGenerateEqualsAndHash <> False Then
                size += 2 + 1
            End If

            If JavaStringCheckUtf8 <> False Then
                size += 2 + 1
            End If

            If OptimizeFor <> 0 Then
                size += 1 + CodedOutputStream.ComputeEnumSize(CInt(OptimizeFor))
            End If

            If GoPackage.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeStringSize(GoPackage)
            End If

            If CcGenericServices <> False Then
                size += 2 + 1
            End If

            If JavaGenericServices <> False Then
                size += 2 + 1
            End If

            If PyGenericServices <> False Then
                size += 2 + 1
            End If

            If Deprecated <> False Then
                size += 2 + 1
            End If

            If CcEnableArenas <> False Then
                size += 2 + 1
            End If

            If ObjcClassPrefix.Length <> 0 Then
                size += 2 + CodedOutputStream.ComputeStringSize(ObjcClassPrefix)
            End If

            If CsharpNamespace.Length <> 0 Then
                size += 2 + CodedOutputStream.ComputeStringSize(CsharpNamespace)
            End If

            size += uninterpretedOption_.CalculateSize(_repeated_uninterpretedOption_codec)
            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As FileOptions) Implements IMessageType(Of FileOptions).MergeFrom
            If other Is Nothing Then
                Return
            End If

            If other.JavaPackage.Length <> 0 Then
                JavaPackage = other.JavaPackage
            End If

            If other.JavaOuterClassname.Length <> 0 Then
                JavaOuterClassname = other.JavaOuterClassname
            End If

            If other.JavaMultipleFiles <> False Then
                JavaMultipleFiles = other.JavaMultipleFiles
            End If

            If other.JavaGenerateEqualsAndHash <> False Then
                JavaGenerateEqualsAndHash = other.JavaGenerateEqualsAndHash
            End If

            If other.JavaStringCheckUtf8 <> False Then
                JavaStringCheckUtf8 = other.JavaStringCheckUtf8
            End If

            If other.OptimizeFor <> 0 Then
                OptimizeFor = other.OptimizeFor
            End If

            If other.GoPackage.Length <> 0 Then
                GoPackage = other.GoPackage
            End If

            If other.CcGenericServices <> False Then
                CcGenericServices = other.CcGenericServices
            End If

            If other.JavaGenericServices <> False Then
                JavaGenericServices = other.JavaGenericServices
            End If

            If other.PyGenericServices <> False Then
                PyGenericServices = other.PyGenericServices
            End If

            If other.Deprecated <> False Then
                Deprecated = other.Deprecated
            End If

            If other.CcEnableArenas <> False Then
                CcEnableArenas = other.CcEnableArenas
            End If

            If other.ObjcClassPrefix.Length <> 0 Then
                ObjcClassPrefix = other.ObjcClassPrefix
            End If

            If other.CsharpNamespace.Length <> 0 Then
                CsharpNamespace = other.CsharpNamespace
            End If

            uninterpretedOption_.Add(other.uninterpretedOption_)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
            Dim tag As New Value(Of UInteger)

            While ((tag = input.ReadTag())) <> 0

                Select Case tag.Value
                    Case 10
                        JavaPackage = input.ReadString()

                    Case 66
                        JavaOuterClassname = input.ReadString()

                    Case 72
                        optimizeFor_ = CType(input.ReadEnum(), Global.Google.Protobuf.Reflection.FileOptions.Types.OptimizeMode)

                    Case 80
                        JavaMultipleFiles = input.ReadBool()

                    Case 90
                        GoPackage = input.ReadString()

                    Case 128
                        CcGenericServices = input.ReadBool()

                    Case 136
                        JavaGenericServices = input.ReadBool()

                    Case 144
                        PyGenericServices = input.ReadBool()

                    Case 160
                        JavaGenerateEqualsAndHash = input.ReadBool()

                    Case 184
                        Deprecated = input.ReadBool()

                    Case 216
                        JavaStringCheckUtf8 = input.ReadBool()

                    Case 248
                        CcEnableArenas = input.ReadBool()

                    Case 290
                        ObjcClassPrefix = input.ReadString()

                    Case 298
                        CsharpNamespace = input.ReadString()

                    Case 7994
                        uninterpretedOption_.AddEntriesFrom(input, _repeated_uninterpretedOption_codec)

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub

#Region "Nested types"
        ''' <summary>Container for nested types declared in the FileOptions message type.</summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Partial Public NotInheritable Class Types
            ''' <summary>
            '''  Generated classes can be optimized for speed or code size.
            ''' </summary>
            Friend Enum OptimizeMode
                ''' <summary>
                '''  Generate complete code for parsing, serialization,
                ''' </summary>
                <OriginalName("SPEED")>
                Speed = 1
                ''' <summary>
                '''  etc.
                ''' </summary>
                <OriginalName("CODE_SIZE")>
                CodeSize = 2
                ''' <summary>
                '''  Generate code using MessageLite and the lite runtime.
                ''' </summary>
                <OriginalName("LITE_RUNTIME")>
                LiteRuntime = 3
            End Enum
        End Class
#End Region

    End Class

    Partial Friend NotInheritable Class MessageOptions
        Implements IMessageType(Of MessageOptions)

        Private Shared ReadOnly _parser As MessageParserType(Of MessageOptions) = New MessageParserType(Of MessageOptions)(Function() New MessageOptions())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of MessageOptions)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As MessageDescriptor
            Get
                Return Global.Google.Protobuf.Reflection.DescriptorReflection.Descriptor.MessageTypes(10)
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Private ReadOnly Property Descriptor As MessageDescriptor Implements IMessage.Descriptor
            Get
                Return DescriptorProp
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New()
            OnConstruction()
        End Sub

        Partial Private Sub OnConstruction()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New(other As MessageOptions)
            Me.New()
            messageSetWireFormat_ = other.messageSetWireFormat_
            noStandardDescriptorAccessor_ = other.noStandardDescriptorAccessor_
            deprecated_ = other.deprecated_
            mapEntry_ = other.mapEntry_
            uninterpretedOption_ = other.uninterpretedOption_.Clone()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As MessageOptions Implements IDeepCloneable(Of MessageOptions).Clone
            Return New MessageOptions(Me)
        End Function

        ''' <summary>Field number for the "message_set_wire_format" field.</summary>
        Public Const MessageSetWireFormatFieldNumber As Integer = 1
        Private messageSetWireFormat_ As Boolean
        ''' <summary>
        '''  Set true to use the old proto1 MessageSet wire format for extensions.
        '''  This is provided for backwards-compatibility with the MessageSet wire
        '''  format.  You should not use this for any other reason:  It's less
        '''  efficient, has fewer features, and is more complicated.
        '''
        '''  The message must be defined exactly as follows:
        '''    message Foo {
        '''      option message_set_wire_format = true;
        '''      extensions 4 to max;
        '''    }
        '''  Note that the message cannot have any defined fields; MessageSets only
        '''  have extensions.
        '''
        '''  All extensions of your type must be singular messages; e.g. they cannot
        '''  be int32s, enums, or repeated messages.
        '''
        '''  Because this is an option, the above two restrictions are not enforced by
        '''  the protocol compiler.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property MessageSetWireFormat As Boolean
            Get
                Return messageSetWireFormat_
            End Get
            Set(value As Boolean)
                messageSetWireFormat_ = value
            End Set
        End Property

        ''' <summary>Field number for the "no_standard_descriptor_accessor" field.</summary>
        Public Const NoStandardDescriptorAccessorFieldNumber As Integer = 2
        Private noStandardDescriptorAccessor_ As Boolean
        ''' <summary>
        '''  Disables the generation of the standard "descriptor()" accessor, which can
        '''  conflict with a field of the same name.  This is meant to make migration
        '''  from proto1 easier; new code should avoid fields named "descriptor".
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property NoStandardDescriptorAccessor As Boolean
            Get
                Return noStandardDescriptorAccessor_
            End Get
            Set(value As Boolean)
                noStandardDescriptorAccessor_ = value
            End Set
        End Property

        ''' <summary>Field number for the "deprecated" field.</summary>
        Public Const DeprecatedFieldNumber As Integer = 3
        Private deprecated_ As Boolean
        ''' <summary>
        '''  Is this message deprecated?
        '''  Depending on the target platform, this can emit Deprecated annotations
        '''  for the message, or it will be completely ignored; in the very least,
        '''  this is a formalization for deprecating messages.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Deprecated As Boolean
            Get
                Return deprecated_
            End Get
            Set(value As Boolean)
                deprecated_ = value
            End Set
        End Property

        ''' <summary>Field number for the "map_entry" field.</summary>
        Public Const MapEntryFieldNumber As Integer = 7
        Private mapEntry_ As Boolean
        ''' <summary>
        '''  Whether the message is an automatically generated map entry type for the
        '''  maps field.
        '''
        '''  For maps fields:
        '''      map&lt;KeyType, ValueType> map_field = 1;
        '''  The parsed descriptor looks like:
        '''      message MapFieldEntry {
        '''          option map_entry = true;
        '''          optional KeyType key = 1;
        '''          optional ValueType value = 2;
        '''      }
        '''      repeated MapFieldEntry map_field = 1;
        '''
        '''  Implementations may choose not to generate the map_entry=true message, but
        '''  use a native map in the target language to hold the keys and values.
        '''  The reflection APIs in such implementions still need to work as
        '''  if the field is a repeated message field.
        '''
        '''  NOTE: Do not set the option in .proto files. Always use the maps syntax
        '''  instead. The option should only be implicitly set by the proto compiler
        '''  parser.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property MapEntry As Boolean
            Get
                Return mapEntry_
            End Get
            Set(value As Boolean)
                mapEntry_ = value
            End Set
        End Property

        ''' <summary>Field number for the "uninterpreted_option" field.</summary>
        Public Const UninterpretedOptionFieldNumber As Integer = 999
        Private Shared ReadOnly _repeated_uninterpretedOption_codec As FieldCodecType(Of Global.Google.Protobuf.Reflection.UninterpretedOption) = FieldCodec.ForMessage(7994, Global.Google.Protobuf.Reflection.UninterpretedOption.Parser)
        Private ReadOnly uninterpretedOption_ As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.UninterpretedOption) = New pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.UninterpretedOption)()
        ''' <summary>
        '''  The parser stores options it doesn't recognize here. See above.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property UninterpretedOption As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.UninterpretedOption)
            Get
                Return uninterpretedOption_
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, MessageOptions))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As MessageOptions) As Boolean Implements IEquatable(Of MessageOptions).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If

            If ReferenceEquals(other, Me) Then
                Return True
            End If

            If MessageSetWireFormat <> other.MessageSetWireFormat Then Return False
            If NoStandardDescriptorAccessor <> other.NoStandardDescriptorAccessor Then Return False
            If Deprecated <> other.Deprecated Then Return False
            If MapEntry <> other.MapEntry Then Return False
            If Not uninterpretedOption_.Equals(other.uninterpretedOption_) Then Return False
            Return True
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function GetHashCode() As Integer
            Dim hash = 1
            If MessageSetWireFormat <> False Then hash = hash Xor MessageSetWireFormat.GetHashCode()
            If NoStandardDescriptorAccessor <> False Then hash = hash Xor NoStandardDescriptorAccessor.GetHashCode()
            If Deprecated <> False Then hash = hash Xor Deprecated.GetHashCode()
            If MapEntry <> False Then hash = hash Xor MapEntry.GetHashCode()
            hash = hash Xor uninterpretedOption_.GetHashCode()
            Return hash
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function ToString() As String
            Return JsonFormatter.ToDiagnosticString(Me)
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
            If MessageSetWireFormat <> False Then
                output.WriteRawTag(8)
                output.WriteBool(MessageSetWireFormat)
            End If

            If NoStandardDescriptorAccessor <> False Then
                output.WriteRawTag(16)
                output.WriteBool(NoStandardDescriptorAccessor)
            End If

            If Deprecated <> False Then
                output.WriteRawTag(24)
                output.WriteBool(Deprecated)
            End If

            If MapEntry <> False Then
                output.WriteRawTag(56)
                output.WriteBool(MapEntry)
            End If

            uninterpretedOption_.WriteTo(output, _repeated_uninterpretedOption_codec)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0

            If MessageSetWireFormat <> False Then
                size += 1 + 1
            End If

            If NoStandardDescriptorAccessor <> False Then
                size += 1 + 1
            End If

            If Deprecated <> False Then
                size += 1 + 1
            End If

            If MapEntry <> False Then
                size += 1 + 1
            End If

            size += uninterpretedOption_.CalculateSize(_repeated_uninterpretedOption_codec)
            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As MessageOptions) Implements IMessageType(Of MessageOptions).MergeFrom
            If other Is Nothing Then
                Return
            End If

            If other.MessageSetWireFormat <> False Then
                MessageSetWireFormat = other.MessageSetWireFormat
            End If

            If other.NoStandardDescriptorAccessor <> False Then
                NoStandardDescriptorAccessor = other.NoStandardDescriptorAccessor
            End If

            If other.Deprecated <> False Then
                Deprecated = other.Deprecated
            End If

            If other.MapEntry <> False Then
                MapEntry = other.MapEntry
            End If

            uninterpretedOption_.Add(other.uninterpretedOption_)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
            Dim tag As New Value(Of UInteger)

            While ((tag = input.ReadTag())) <> 0

                Select Case tag.Value
                    Case 8
                        MessageSetWireFormat = input.ReadBool()

                    Case 16
                        NoStandardDescriptorAccessor = input.ReadBool()

                    Case 24
                        Deprecated = input.ReadBool()

                    Case 56
                        MapEntry = input.ReadBool()

                    Case 7994
                        uninterpretedOption_.AddEntriesFrom(input, _repeated_uninterpretedOption_codec)

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub
    End Class

    Partial Friend NotInheritable Class FieldOptions
        Implements IMessageType(Of FieldOptions)

        Private Shared ReadOnly _parser As MessageParserType(Of FieldOptions) = New MessageParserType(Of FieldOptions)(Function() New FieldOptions())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of FieldOptions)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As MessageDescriptor
            Get
                Return Global.Google.Protobuf.Reflection.DescriptorReflection.Descriptor.MessageTypes(11)
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Private ReadOnly Property Descriptor As MessageDescriptor Implements IMessage.Descriptor
            Get
                Return DescriptorProp
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New()
            Me.OnConstruction()
        End Sub

        Partial Private Sub OnConstruction()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New(other As FieldOptions)
            Me.New()
            ctype_ = other.ctype_
            packed_ = other.packed_
            jstype_ = other.jstype_
            lazy_ = other.lazy_
            deprecated_ = other.deprecated_
            weak_ = other.weak_
            uninterpretedOption_ = other.uninterpretedOption_.Clone()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As FieldOptions Implements IDeepCloneable(Of FieldOptions).Clone
            Return New FieldOptions(Me)
        End Function

        ''' <summary>Field number for the "ctype" field.</summary>
        Public Const CtypeFieldNumber As Integer = 1
        Private ctype_ As Global.Google.Protobuf.Reflection.FieldOptions.Types.CType = 0
        ''' <summary>
        '''  The ctype option instructs the C++ code generator to use a different
        '''  representation of the field than it normally would.  See the specific
        '''  options below.  This option is not yet implemented in the open source
        '''  release -- sorry, we'll try to include it in a future version!
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property [Ctype] As Global.Google.Protobuf.Reflection.FieldOptions.Types.CType
            Get
                Return ctype_
            End Get
            Set(value As Global.Google.Protobuf.Reflection.FieldOptions.Types.CType)
                ctype_ = value
            End Set
        End Property

        ''' <summary>Field number for the "packed" field.</summary>
        Public Const PackedFieldNumber As Integer = 2
        Private packed_ As Boolean
        ''' <summary>
        '''  The packed option can be enabled for repeated primitive fields to enable
        '''  a more efficient representation on the wire. Rather than repeatedly
        '''  writing the tag and type for each element, the entire array is encoded as
        '''  a single length-delimited blob. In proto3, only explicit setting it to
        '''  false will avoid using packed encoding.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Packed As Boolean
            Get
                Return packed_
            End Get
            Set(value As Boolean)
                packed_ = value
            End Set
        End Property

        ''' <summary>Field number for the "jstype" field.</summary>
        Public Const JstypeFieldNumber As Integer = 6
        Private jstype_ As Global.Google.Protobuf.Reflection.FieldOptions.Types.JSType = 0
        ''' <summary>
        '''  The jstype option determines the JavaScript type used for values of the
        '''  field.  The option is permitted only for 64 bit integral and fixed types
        '''  (int64, uint64, sint64, fixed64, sfixed64).  By default these types are
        '''  represented as JavaScript strings.  This avoids loss of precision that can
        '''  happen when a large value is converted to a floating point JavaScript
        '''  numbers.  Specifying JS_NUMBER for the jstype causes the generated
        '''  JavaScript code to use the JavaScript "number" type instead of strings.
        '''  This option is an enum to permit additional types to be added,
        '''  e.g. goog.math.Integer.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Jstype As Global.Google.Protobuf.Reflection.FieldOptions.Types.JSType
            Get
                Return jstype_
            End Get
            Set(value As Global.Google.Protobuf.Reflection.FieldOptions.Types.JSType)
                jstype_ = value
            End Set
        End Property

        ''' <summary>Field number for the "lazy" field.</summary>
        Public Const LazyFieldNumber As Integer = 5
        Private lazy_ As Boolean
        ''' <summary>
        '''  Should this field be parsed lazily?  Lazy applies only to message-type
        '''  fields.  It means that when the outer message is initially parsed, the
        '''  inner message's contents will not be parsed but instead stored in encoded
        '''  form.  The inner message will actually be parsed when it is first accessed.
        '''
        '''  This is only a hint.  Implementations are free to choose whether to use
        '''  eager or lazy parsing regardless of the value of this option.  However,
        '''  setting this option true suggests that the protocol author believes that
        '''  using lazy parsing on this field is worth the additional bookkeeping
        '''  overhead typically needed to implement it.
        '''
        '''  This option does not affect the public interface of any generated code;
        '''  all method signatures remain the same.  Furthermore, thread-safety of the
        '''  interface is not affected by this option; const methods remain safe to
        '''  call from multiple threads concurrently, while non-const methods continue
        '''  to require exclusive access.
        '''
        '''  Note that implementations may choose not to check required fields within
        '''  a lazy sub-message.  That is, calling IsInitialized() on the outher message
        '''  may return true even if the inner message has missing required fields.
        '''  This is necessary because otherwise the inner message would have to be
        '''  parsed in order to perform the check, defeating the purpose of lazy
        '''  parsing.  An implementation which chooses not to check required fields
        '''  must be consistent about it.  That is, for any particular sub-message, the
        '''  implementation must either *always* check its required fields, or *never*
        '''  check its required fields, regardless of whether or not the message has
        '''  been parsed.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Lazy As Boolean
            Get
                Return lazy_
            End Get
            Set(value As Boolean)
                lazy_ = value
            End Set
        End Property

        ''' <summary>Field number for the "deprecated" field.</summary>
        Public Const DeprecatedFieldNumber As Integer = 3
        Private deprecated_ As Boolean
        ''' <summary>
        '''  Is this field deprecated?
        '''  Depending on the target platform, this can emit Deprecated annotations
        '''  for accessors, or it will be completely ignored; in the very least, this
        '''  is a formalization for deprecating fields.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Deprecated As Boolean
            Get
                Return deprecated_
            End Get
            Set(value As Boolean)
                deprecated_ = value
            End Set
        End Property

        ''' <summary>Field number for the "weak" field.</summary>
        Public Const WeakFieldNumber As Integer = 10
        Private weak_ As Boolean
        ''' <summary>
        '''  For Google-internal migration only. Do not use.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Weak As Boolean
            Get
                Return weak_
            End Get
            Set(value As Boolean)
                weak_ = value
            End Set
        End Property

        ''' <summary>Field number for the "uninterpreted_option" field.</summary>
        Public Const UninterpretedOptionFieldNumber As Integer = 999
        Private Shared ReadOnly _repeated_uninterpretedOption_codec As FieldCodecType(Of Global.Google.Protobuf.Reflection.UninterpretedOption) = FieldCodec.ForMessage(7994, Global.Google.Protobuf.Reflection.UninterpretedOption.Parser)
        Private ReadOnly uninterpretedOption_ As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.UninterpretedOption) = New pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.UninterpretedOption)()
        ''' <summary>
        '''  The parser stores options it doesn't recognize here. See above.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property UninterpretedOption As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.UninterpretedOption)
            Get
                Return uninterpretedOption_
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, FieldOptions))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As FieldOptions) As Boolean Implements IEquatable(Of FieldOptions).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If

            If ReferenceEquals(other, Me) Then
                Return True
            End If

            If [Ctype] <> other.Ctype Then Return False
            If Packed <> other.Packed Then Return False
            If Jstype <> other.Jstype Then Return False
            If Lazy <> other.Lazy Then Return False
            If Deprecated <> other.Deprecated Then Return False
            If Weak <> other.Weak Then Return False
            If Not uninterpretedOption_.Equals(other.uninterpretedOption_) Then Return False
            Return True
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function GetHashCode() As Integer
            Dim hash = 1
            If [Ctype] <> 0 Then hash = hash Xor [Ctype].GetHashCode()
            If Packed <> False Then hash = hash Xor Packed.GetHashCode()
            If Jstype <> 0 Then hash = hash Xor Jstype.GetHashCode()
            If Lazy <> False Then hash = hash Xor Lazy.GetHashCode()
            If Deprecated <> False Then hash = hash Xor Deprecated.GetHashCode()
            If Weak <> False Then hash = hash Xor Weak.GetHashCode()
            hash = hash Xor uninterpretedOption_.GetHashCode()
            Return hash
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function ToString() As String
            Return JsonFormatter.ToDiagnosticString(Me)
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
            If [Ctype] <> 0 Then
                output.WriteRawTag(8)
                output.WriteEnum(CInt([Ctype]))
            End If

            If Packed <> False Then
                output.WriteRawTag(16)
                output.WriteBool(Packed)
            End If

            If Deprecated <> False Then
                output.WriteRawTag(24)
                output.WriteBool(Deprecated)
            End If

            If Lazy <> False Then
                output.WriteRawTag(40)
                output.WriteBool(Lazy)
            End If

            If Jstype <> 0 Then
                output.WriteRawTag(48)
                output.WriteEnum(CInt(Jstype))
            End If

            If Weak <> False Then
                output.WriteRawTag(80)
                output.WriteBool(Weak)
            End If

            uninterpretedOption_.WriteTo(output, _repeated_uninterpretedOption_codec)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0

            If [Ctype] <> 0 Then
                size += 1 + CodedOutputStream.ComputeEnumSize(CInt([Ctype]))
            End If

            If Packed <> False Then
                size += 1 + 1
            End If

            If Jstype <> 0 Then
                size += 1 + CodedOutputStream.ComputeEnumSize(CInt(Jstype))
            End If

            If Lazy <> False Then
                size += 1 + 1
            End If

            If Deprecated <> False Then
                size += 1 + 1
            End If

            If Weak <> False Then
                size += 1 + 1
            End If

            size += uninterpretedOption_.CalculateSize(_repeated_uninterpretedOption_codec)
            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As FieldOptions) Implements IMessageType(Of FieldOptions).MergeFrom
            If other Is Nothing Then
                Return
            End If

            If other.Ctype <> 0 Then
                [Ctype] = other.Ctype
            End If

            If other.Packed <> False Then
                Packed = other.Packed
            End If

            If other.Jstype <> 0 Then
                Jstype = other.Jstype
            End If

            If other.Lazy <> False Then
                Lazy = other.Lazy
            End If

            If other.Deprecated <> False Then
                Deprecated = other.Deprecated
            End If

            If other.Weak <> False Then
                Weak = other.Weak
            End If

            uninterpretedOption_.Add(other.uninterpretedOption_)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
            Dim tag As New Value(Of UInteger)

            While ((tag = input.ReadTag())) <> 0

                Select Case tag.Value
                    Case 8
                        ctype_ = CType(input.ReadEnum(), Global.Google.Protobuf.Reflection.FieldOptions.Types.CType)

                    Case 16
                        Packed = input.ReadBool()

                    Case 24
                        Deprecated = input.ReadBool()

                    Case 40
                        Lazy = input.ReadBool()

                    Case 48
                        jstype_ = CType(input.ReadEnum(), Global.Google.Protobuf.Reflection.FieldOptions.Types.JSType)

                    Case 80
                        Weak = input.ReadBool()

                    Case 7994
                        uninterpretedOption_.AddEntriesFrom(input, _repeated_uninterpretedOption_codec)

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub

#Region "Nested types"
        ''' <summary>Container for nested types declared in the FieldOptions message type.</summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Partial Public NotInheritable Class Types
            Friend Enum [CType]
                ''' <summary>
                '''  Default mode.
                ''' </summary>
                <OriginalName("STRING")>
                [String] = 0
                <OriginalName("CORD")>
                Cord = 1
                <OriginalName("STRING_PIECE")>
                StringPiece = 2
            End Enum

            Friend Enum JSType
                ''' <summary>
                '''  Use the default type.
                ''' </summary>
                <OriginalName("JS_NORMAL")>
                JsNormal = 0
                ''' <summary>
                '''  Use JavaScript strings.
                ''' </summary>
                <OriginalName("JS_STRING")>
                JsString = 1
                ''' <summary>
                '''  Use JavaScript numbers.
                ''' </summary>
                <OriginalName("JS_NUMBER")>
                JsNumber = 2
            End Enum
        End Class
#End Region

    End Class

    Partial Friend NotInheritable Class OneofOptions
        Implements IMessageType(Of OneofOptions)

        Private Shared ReadOnly _parser As MessageParserType(Of OneofOptions) = New MessageParserType(Of OneofOptions)(Function() New OneofOptions())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of OneofOptions)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As MessageDescriptor
            Get
                Return Global.Google.Protobuf.Reflection.DescriptorReflection.Descriptor.MessageTypes(12)
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Private ReadOnly Property Descriptor As MessageDescriptor Implements IMessage.Descriptor
            Get
                Return DescriptorProp
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New()
            OnConstruction()
        End Sub

        Partial Private Sub OnConstruction()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New(other As OneofOptions)
            Me.New()
            uninterpretedOption_ = other.uninterpretedOption_.Clone()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As OneofOptions Implements IDeepCloneable(Of OneofOptions).Clone
            Return New OneofOptions(Me)
        End Function

        ''' <summary>Field number for the "uninterpreted_option" field.</summary>
        Public Const UninterpretedOptionFieldNumber As Integer = 999
        Private Shared ReadOnly _repeated_uninterpretedOption_codec As FieldCodecType(Of Global.Google.Protobuf.Reflection.UninterpretedOption) = FieldCodec.ForMessage(7994, Global.Google.Protobuf.Reflection.UninterpretedOption.Parser)
        Private ReadOnly uninterpretedOption_ As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.UninterpretedOption) = New pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.UninterpretedOption)()
        ''' <summary>
        '''  The parser stores options it doesn't recognize here. See above.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property UninterpretedOption As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.UninterpretedOption)
            Get
                Return uninterpretedOption_
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, OneofOptions))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As OneofOptions) As Boolean Implements IEquatable(Of OneofOptions).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If

            If ReferenceEquals(other, Me) Then
                Return True
            End If

            If Not uninterpretedOption_.Equals(other.uninterpretedOption_) Then Return False
            Return True
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function GetHashCode() As Integer
            Dim hash = 1
            hash = hash Xor uninterpretedOption_.GetHashCode()
            Return hash
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function ToString() As String
            Return JsonFormatter.ToDiagnosticString(Me)
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
            uninterpretedOption_.WriteTo(output, _repeated_uninterpretedOption_codec)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0
            size += uninterpretedOption_.CalculateSize(_repeated_uninterpretedOption_codec)
            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As OneofOptions) Implements IMessageType(Of OneofOptions).MergeFrom
            If other Is Nothing Then
                Return
            End If

            uninterpretedOption_.Add(other.uninterpretedOption_)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
            Dim tag As New Value(Of UInteger)

            While ((tag = input.ReadTag())) <> 0

                Select Case tag.Value
                    Case 7994
                        uninterpretedOption_.AddEntriesFrom(input, _repeated_uninterpretedOption_codec)

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub
    End Class

    Partial Friend NotInheritable Class EnumOptions
        Implements IMessageType(Of EnumOptions)

        Private Shared ReadOnly _parser As MessageParserType(Of EnumOptions) = New MessageParserType(Of EnumOptions)(Function() New EnumOptions())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of EnumOptions)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As MessageDescriptor
            Get
                Return Global.Google.Protobuf.Reflection.DescriptorReflection.Descriptor.MessageTypes(13)
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Private ReadOnly Property Descriptor As MessageDescriptor Implements IMessage.Descriptor
            Get
                Return DescriptorProp
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New()
            OnConstruction()
        End Sub

        Partial Private Sub OnConstruction()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New(other As EnumOptions)
            Me.New()
            allowAlias_ = other.allowAlias_
            deprecated_ = other.deprecated_
            uninterpretedOption_ = other.uninterpretedOption_.Clone()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As EnumOptions Implements IDeepCloneable(Of EnumOptions).Clone
            Return New EnumOptions(Me)
        End Function

        ''' <summary>Field number for the "allow_alias" field.</summary>
        Public Const AllowAliasFieldNumber As Integer = 2
        Private allowAlias_ As Boolean
        ''' <summary>
        '''  Set this option to true to allow mapping different tag names to the same
        '''  value.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property AllowAlias As Boolean
            Get
                Return allowAlias_
            End Get
            Set(value As Boolean)
                allowAlias_ = value
            End Set
        End Property

        ''' <summary>Field number for the "deprecated" field.</summary>
        Public Const DeprecatedFieldNumber As Integer = 3
        Private deprecated_ As Boolean
        ''' <summary>
        '''  Is this enum deprecated?
        '''  Depending on the target platform, this can emit Deprecated annotations
        '''  for the enum, or it will be completely ignored; in the very least, this
        '''  is a formalization for deprecating enums.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Deprecated As Boolean
            Get
                Return deprecated_
            End Get
            Set(value As Boolean)
                deprecated_ = value
            End Set
        End Property

        ''' <summary>Field number for the "uninterpreted_option" field.</summary>
        Public Const UninterpretedOptionFieldNumber As Integer = 999
        Private Shared ReadOnly _repeated_uninterpretedOption_codec As FieldCodecType(Of Global.Google.Protobuf.Reflection.UninterpretedOption) = FieldCodec.ForMessage(7994, Global.Google.Protobuf.Reflection.UninterpretedOption.Parser)
        Private ReadOnly uninterpretedOption_ As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.UninterpretedOption) = New pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.UninterpretedOption)()
        ''' <summary>
        '''  The parser stores options it doesn't recognize here. See above.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property UninterpretedOption As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.UninterpretedOption)
            Get
                Return uninterpretedOption_
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, EnumOptions))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As EnumOptions) As Boolean Implements IEquatable(Of EnumOptions).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If

            If ReferenceEquals(other, Me) Then
                Return True
            End If

            If AllowAlias <> other.AllowAlias Then Return False
            If Deprecated <> other.Deprecated Then Return False
            If Not uninterpretedOption_.Equals(other.uninterpretedOption_) Then Return False
            Return True
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function GetHashCode() As Integer
            Dim hash = 1
            If AllowAlias <> False Then hash = hash Xor AllowAlias.GetHashCode()
            If Deprecated <> False Then hash = hash Xor Deprecated.GetHashCode()
            hash = hash Xor uninterpretedOption_.GetHashCode()
            Return hash
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function ToString() As String
            Return JsonFormatter.ToDiagnosticString(Me)
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
            If AllowAlias <> False Then
                output.WriteRawTag(16)
                output.WriteBool(AllowAlias)
            End If

            If Deprecated <> False Then
                output.WriteRawTag(24)
                output.WriteBool(Deprecated)
            End If

            uninterpretedOption_.WriteTo(output, _repeated_uninterpretedOption_codec)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0

            If AllowAlias <> False Then
                size += 1 + 1
            End If

            If Deprecated <> False Then
                size += 1 + 1
            End If

            size += uninterpretedOption_.CalculateSize(_repeated_uninterpretedOption_codec)
            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As EnumOptions) Implements IMessageType(Of EnumOptions).MergeFrom
            If other Is Nothing Then
                Return
            End If

            If other.AllowAlias <> False Then
                AllowAlias = other.AllowAlias
            End If

            If other.Deprecated <> False Then
                Deprecated = other.Deprecated
            End If

            uninterpretedOption_.Add(other.uninterpretedOption_)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
            Dim tag As New Value(Of UInteger)

            While ((tag = input.ReadTag())) <> 0

                Select Case tag.Value
                    Case 16
                        AllowAlias = input.ReadBool()

                    Case 24
                        Deprecated = input.ReadBool()

                    Case 7994
                        uninterpretedOption_.AddEntriesFrom(input, _repeated_uninterpretedOption_codec)

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub
    End Class

    Partial Friend NotInheritable Class EnumValueOptions
        Implements IMessageType(Of EnumValueOptions)

        Private Shared ReadOnly _parser As MessageParserType(Of EnumValueOptions) = New MessageParserType(Of EnumValueOptions)(Function() New EnumValueOptions())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of EnumValueOptions)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As MessageDescriptor
            Get
                Return Global.Google.Protobuf.Reflection.DescriptorReflection.Descriptor.MessageTypes(14)
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Private ReadOnly Property Descriptor As MessageDescriptor Implements IMessage.Descriptor
            Get
                Return DescriptorProp
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New()
            OnConstruction()
        End Sub

        Partial Private Sub OnConstruction()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New(other As EnumValueOptions)
            Me.New()
            deprecated_ = other.deprecated_
            uninterpretedOption_ = other.uninterpretedOption_.Clone()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As EnumValueOptions Implements IDeepCloneable(Of EnumValueOptions).Clone
            Return New EnumValueOptions(Me)
        End Function

        ''' <summary>Field number for the "deprecated" field.</summary>
        Public Const DeprecatedFieldNumber As Integer = 1
        Private deprecated_ As Boolean
        ''' <summary>
        '''  Is this enum value deprecated?
        '''  Depending on the target platform, this can emit Deprecated annotations
        '''  for the enum value, or it will be completely ignored; in the very least,
        '''  this is a formalization for deprecating enum values.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Deprecated As Boolean
            Get
                Return deprecated_
            End Get
            Set(value As Boolean)
                deprecated_ = value
            End Set
        End Property

        ''' <summary>Field number for the "uninterpreted_option" field.</summary>
        Public Const UninterpretedOptionFieldNumber As Integer = 999
        Private Shared ReadOnly _repeated_uninterpretedOption_codec As FieldCodecType(Of Global.Google.Protobuf.Reflection.UninterpretedOption) = FieldCodec.ForMessage(7994, Global.Google.Protobuf.Reflection.UninterpretedOption.Parser)
        Private ReadOnly uninterpretedOption_ As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.UninterpretedOption) = New pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.UninterpretedOption)()
        ''' <summary>
        '''  The parser stores options it doesn't recognize here. See above.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property UninterpretedOption As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.UninterpretedOption)
            Get
                Return uninterpretedOption_
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, EnumValueOptions))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As EnumValueOptions) As Boolean Implements IEquatable(Of EnumValueOptions).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If

            If ReferenceEquals(other, Me) Then
                Return True
            End If

            If Deprecated <> other.Deprecated Then Return False
            If Not uninterpretedOption_.Equals(other.uninterpretedOption_) Then Return False
            Return True
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function GetHashCode() As Integer
            Dim hash = 1
            If Deprecated <> False Then hash = hash Xor Deprecated.GetHashCode()
            hash = hash Xor uninterpretedOption_.GetHashCode()
            Return hash
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function ToString() As String
            Return JsonFormatter.ToDiagnosticString(Me)
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
            If Deprecated <> False Then
                output.WriteRawTag(8)
                output.WriteBool(Deprecated)
            End If

            uninterpretedOption_.WriteTo(output, _repeated_uninterpretedOption_codec)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0

            If Deprecated <> False Then
                size += 1 + 1
            End If

            size += uninterpretedOption_.CalculateSize(_repeated_uninterpretedOption_codec)
            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As EnumValueOptions) Implements IMessageType(Of EnumValueOptions).MergeFrom
            If other Is Nothing Then
                Return
            End If

            If other.Deprecated <> False Then
                Deprecated = other.Deprecated
            End If

            uninterpretedOption_.Add(other.uninterpretedOption_)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
            Dim tag As New Value(Of UInteger)

            While ((tag = input.ReadTag())) <> 0

                Select Case tag.Value
                    Case 8
                        Deprecated = input.ReadBool()

                    Case 7994
                        uninterpretedOption_.AddEntriesFrom(input, _repeated_uninterpretedOption_codec)

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub
    End Class

    Partial Friend NotInheritable Class ServiceOptions
        Implements IMessageType(Of ServiceOptions)

        Private Shared ReadOnly _parser As MessageParserType(Of ServiceOptions) = New MessageParserType(Of ServiceOptions)(Function() New ServiceOptions())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of ServiceOptions)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As MessageDescriptor
            Get
                Return Global.Google.Protobuf.Reflection.DescriptorReflection.Descriptor.MessageTypes(15)
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Private ReadOnly Property Descriptor As MessageDescriptor Implements IMessage.Descriptor
            Get
                Return DescriptorProp
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New()
            OnConstruction()
        End Sub

        Partial Private Sub OnConstruction()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New(other As ServiceOptions)
            Me.New()
            deprecated_ = other.deprecated_
            uninterpretedOption_ = other.uninterpretedOption_.Clone()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As ServiceOptions Implements IDeepCloneable(Of ServiceOptions).Clone
            Return New ServiceOptions(Me)
        End Function

        ''' <summary>Field number for the "deprecated" field.</summary>
        Public Const DeprecatedFieldNumber As Integer = 33
        Private deprecated_ As Boolean
        ''' <summary>
        '''  Is this service deprecated?
        '''  Depending on the target platform, this can emit Deprecated annotations
        '''  for the service, or it will be completely ignored; in the very least,
        '''  this is a formalization for deprecating services.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Deprecated As Boolean
            Get
                Return deprecated_
            End Get
            Set(value As Boolean)
                deprecated_ = value
            End Set
        End Property

        ''' <summary>Field number for the "uninterpreted_option" field.</summary>
        Public Const UninterpretedOptionFieldNumber As Integer = 999
        Private Shared ReadOnly _repeated_uninterpretedOption_codec As FieldCodecType(Of Global.Google.Protobuf.Reflection.UninterpretedOption) = FieldCodec.ForMessage(7994, Global.Google.Protobuf.Reflection.UninterpretedOption.Parser)
        Private ReadOnly uninterpretedOption_ As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.UninterpretedOption) = New pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.UninterpretedOption)()
        ''' <summary>
        '''  The parser stores options it doesn't recognize here. See above.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property UninterpretedOption As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.UninterpretedOption)
            Get
                Return uninterpretedOption_
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, ServiceOptions))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As ServiceOptions) As Boolean Implements IEquatable(Of ServiceOptions).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If

            If ReferenceEquals(other, Me) Then
                Return True
            End If

            If Deprecated <> other.Deprecated Then Return False
            If Not uninterpretedOption_.Equals(other.uninterpretedOption_) Then Return False
            Return True
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function GetHashCode() As Integer
            Dim hash = 1
            If Deprecated <> False Then hash = hash Xor Deprecated.GetHashCode()
            hash = hash Xor uninterpretedOption_.GetHashCode()
            Return hash
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function ToString() As String
            Return JsonFormatter.ToDiagnosticString(Me)
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
            If Deprecated <> False Then
                output.WriteRawTag(136, 2)
                output.WriteBool(Deprecated)
            End If

            uninterpretedOption_.WriteTo(output, _repeated_uninterpretedOption_codec)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0

            If Deprecated <> False Then
                size += 2 + 1
            End If

            size += uninterpretedOption_.CalculateSize(_repeated_uninterpretedOption_codec)
            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As ServiceOptions) Implements IMessageType(Of ServiceOptions).MergeFrom
            If other Is Nothing Then
                Return
            End If

            If other.Deprecated <> False Then
                Deprecated = other.Deprecated
            End If

            uninterpretedOption_.Add(other.uninterpretedOption_)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
            Dim tag As New Value(Of UInteger)

            While ((tag = input.ReadTag())) <> 0

                Select Case tag.Value
                    Case 264
                        Deprecated = input.ReadBool()

                    Case 7994
                        uninterpretedOption_.AddEntriesFrom(input, _repeated_uninterpretedOption_codec)

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub
    End Class

    Partial Friend NotInheritable Class MethodOptions
        Implements IMessageType(Of MethodOptions)

        Private Shared ReadOnly _parser As MessageParserType(Of MethodOptions) = New MessageParserType(Of MethodOptions)(Function() New MethodOptions())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of MethodOptions)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As MessageDescriptor
            Get
                Return Global.Google.Protobuf.Reflection.DescriptorReflection.Descriptor.MessageTypes(16)
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Private ReadOnly Property Descriptor As MessageDescriptor Implements IMessage.Descriptor
            Get
                Return DescriptorProp
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New()
            OnConstruction()
        End Sub

        Partial Private Sub OnConstruction()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New(other As MethodOptions)
            Me.New()
            deprecated_ = other.deprecated_
            uninterpretedOption_ = other.uninterpretedOption_.Clone()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As MethodOptions Implements IDeepCloneable(Of MethodOptions).Clone
            Return New MethodOptions(Me)
        End Function

        ''' <summary>Field number for the "deprecated" field.</summary>
        Public Const DeprecatedFieldNumber As Integer = 33
        Private deprecated_ As Boolean
        ''' <summary>
        '''  Is this method deprecated?
        '''  Depending on the target platform, this can emit Deprecated annotations
        '''  for the method, or it will be completely ignored; in the very least,
        '''  this is a formalization for deprecating methods.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Deprecated As Boolean
            Get
                Return deprecated_
            End Get
            Set(value As Boolean)
                deprecated_ = value
            End Set
        End Property

        ''' <summary>Field number for the "uninterpreted_option" field.</summary>
        Public Const UninterpretedOptionFieldNumber As Integer = 999
        Private Shared ReadOnly _repeated_uninterpretedOption_codec As FieldCodecType(Of Global.Google.Protobuf.Reflection.UninterpretedOption) = FieldCodec.ForMessage(7994, Global.Google.Protobuf.Reflection.UninterpretedOption.Parser)
        Private ReadOnly uninterpretedOption_ As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.UninterpretedOption) = New pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.UninterpretedOption)()
        ''' <summary>
        '''  The parser stores options it doesn't recognize here. See above.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property UninterpretedOption As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.UninterpretedOption)
            Get
                Return uninterpretedOption_
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, MethodOptions))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As MethodOptions) As Boolean Implements IEquatable(Of MethodOptions).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If

            If ReferenceEquals(other, Me) Then
                Return True
            End If

            If Deprecated <> other.Deprecated Then Return False
            If Not uninterpretedOption_.Equals(other.uninterpretedOption_) Then Return False
            Return True
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function GetHashCode() As Integer
            Dim hash = 1
            If Deprecated <> False Then hash = hash Xor Deprecated.GetHashCode()
            hash = hash Xor uninterpretedOption_.GetHashCode()
            Return hash
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function ToString() As String
            Return JsonFormatter.ToDiagnosticString(Me)
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
            If Deprecated <> False Then
                output.WriteRawTag(136, 2)
                output.WriteBool(Deprecated)
            End If

            uninterpretedOption_.WriteTo(output, _repeated_uninterpretedOption_codec)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0

            If Deprecated <> False Then
                size += 2 + 1
            End If

            size += uninterpretedOption_.CalculateSize(_repeated_uninterpretedOption_codec)
            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As MethodOptions) Implements IMessageType(Of MethodOptions).MergeFrom
            If other Is Nothing Then
                Return
            End If

            If other.Deprecated <> False Then
                Deprecated = other.Deprecated
            End If

            uninterpretedOption_.Add(other.uninterpretedOption_)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
            Dim tag As New Value(Of UInteger)

            While ((tag = input.ReadTag())) <> 0

                Select Case tag.Value
                    Case 264
                        Deprecated = input.ReadBool()

                    Case 7994
                        uninterpretedOption_.AddEntriesFrom(input, _repeated_uninterpretedOption_codec)

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub
    End Class

    ''' <summary>
    '''  A message representing a option the parser does not recognize. This only
    '''  appears in options protos created by the compiler::Parser class.
    '''  DescriptorPool resolves these when building Descriptor objects. Therefore,
    '''  options protos in descriptor objects (e.g. returned by Descriptor::options(),
    '''  or produced by Descriptor::CopyTo()) will never have UninterpretedOptions
    '''  in them.
    ''' </summary>
    Partial Friend NotInheritable Class UninterpretedOption
        Implements IMessageType(Of UninterpretedOption)

        Private Shared ReadOnly _parser As MessageParserType(Of UninterpretedOption) = New MessageParserType(Of UninterpretedOption)(Function() New UninterpretedOption())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of UninterpretedOption)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As MessageDescriptor
            Get
                Return Global.Google.Protobuf.Reflection.DescriptorReflection.Descriptor.MessageTypes(17)
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Private ReadOnly Property Descriptor As MessageDescriptor Implements IMessage.Descriptor
            Get
                Return DescriptorProp
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New()
            OnConstruction()
        End Sub

        Partial Private Sub OnConstruction()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New(other As UninterpretedOption)
            Me.New()
            name_ = other.name_.Clone()
            identifierValue_ = other.identifierValue_
            positiveIntValue_ = other.positiveIntValue_
            negativeIntValue_ = other.negativeIntValue_
            doubleValue_ = other.doubleValue_
            stringValue_ = other.stringValue_
            aggregateValue_ = other.aggregateValue_
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As UninterpretedOption Implements IDeepCloneable(Of UninterpretedOption).Clone
            Return New UninterpretedOption(Me)
        End Function

        ''' <summary>Field number for the "name" field.</summary>
        Public Const NameFieldNumber As Integer = 2
        Private Shared ReadOnly _repeated_name_codec As FieldCodecType(Of Global.Google.Protobuf.Reflection.UninterpretedOption.Types.NamePart) = FieldCodec.ForMessage(18, Global.Google.Protobuf.Reflection.UninterpretedOption.Types.NamePart.Parser)
        Private ReadOnly name_ As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.UninterpretedOption.Types.NamePart) = New pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.UninterpretedOption.Types.NamePart)()

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property Name As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.UninterpretedOption.Types.NamePart)
            Get
                Return name_
            End Get
        End Property

        ''' <summary>Field number for the "identifier_value" field.</summary>
        Public Const IdentifierValueFieldNumber As Integer = 3
        Private identifierValue_ As String = ""
        ''' <summary>
        '''  The value of the uninterpreted option, in whatever type the tokenizer
        '''  identified it as during parsing. Exactly one of these should be set.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property IdentifierValue As String
            Get
                Return identifierValue_
            End Get
            Set(value As String)
                identifierValue_ = CheckNotNull(value, "value")
            End Set
        End Property

        ''' <summary>Field number for the "positive_int_value" field.</summary>
        Public Const PositiveIntValueFieldNumber As Integer = 4
        Private positiveIntValue_ As ULong

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property PositiveIntValue As ULong
            Get
                Return positiveIntValue_
            End Get
            Set(value As ULong)
                positiveIntValue_ = value
            End Set
        End Property

        ''' <summary>Field number for the "negative_int_value" field.</summary>
        Public Const NegativeIntValueFieldNumber As Integer = 5
        Private negativeIntValue_ As Long

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property NegativeIntValue As Long
            Get
                Return negativeIntValue_
            End Get
            Set(value As Long)
                negativeIntValue_ = value
            End Set
        End Property

        ''' <summary>Field number for the "double_value" field.</summary>
        Public Const DoubleValueFieldNumber As Integer = 6
        Private doubleValue_ As Double

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property DoubleValue As Double
            Get
                Return doubleValue_
            End Get
            Set(value As Double)
                doubleValue_ = value
            End Set
        End Property

        ''' <summary>Field number for the "string_value" field.</summary>
        Public Const StringValueFieldNumber As Integer = 7
        Private stringValue_ As ByteString = ByteString.Empty

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property StringValue As ByteString
            Get
                Return stringValue_
            End Get
            Set(value As ByteString)
                stringValue_ = CheckNotNull(value, "value")
            End Set
        End Property

        ''' <summary>Field number for the "aggregate_value" field.</summary>
        Public Const AggregateValueFieldNumber As Integer = 8
        Private aggregateValue_ As String = ""

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property AggregateValue As String
            Get
                Return aggregateValue_
            End Get
            Set(value As String)
                aggregateValue_ = CheckNotNull(value, "value")
            End Set
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, UninterpretedOption))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As UninterpretedOption) As Boolean Implements IEquatable(Of UninterpretedOption).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If

            If ReferenceEquals(other, Me) Then
                Return True
            End If

            If Not name_.Equals(other.name_) Then Return False
            If Not Equals(IdentifierValue, other.IdentifierValue) Then Return False
            If PositiveIntValue <> other.PositiveIntValue Then Return False
            If NegativeIntValue <> other.NegativeIntValue Then Return False
            If DoubleValue <> other.DoubleValue Then Return False
            If StringValue IsNot other.StringValue Then Return False
            If Not Equals(AggregateValue, other.AggregateValue) Then Return False
            Return True
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function GetHashCode() As Integer
            Dim hash = 1
            hash = hash Xor name_.GetHashCode()
            If IdentifierValue.Length <> 0 Then hash = hash Xor IdentifierValue.GetHashCode()
            If PositiveIntValue <> 0UL Then hash = hash Xor PositiveIntValue.GetHashCode()
            If NegativeIntValue <> 0L Then hash = hash Xor NegativeIntValue.GetHashCode()
            If DoubleValue <> 0R Then hash = hash Xor DoubleValue.GetHashCode()
            If StringValue.Length <> 0 Then hash = hash Xor StringValue.GetHashCode()
            If AggregateValue.Length <> 0 Then hash = hash Xor AggregateValue.GetHashCode()
            Return hash
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function ToString() As String
            Return JsonFormatter.ToDiagnosticString(Me)
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
            name_.WriteTo(output, _repeated_name_codec)

            If IdentifierValue.Length <> 0 Then
                output.WriteRawTag(26)
                output.WriteString(IdentifierValue)
            End If

            If PositiveIntValue <> 0UL Then
                output.WriteRawTag(32)
                output.WriteUInt64(PositiveIntValue)
            End If

            If NegativeIntValue <> 0L Then
                output.WriteRawTag(40)
                output.WriteInt64(NegativeIntValue)
            End If

            If DoubleValue <> 0R Then
                output.WriteRawTag(49)
                output.WriteDouble(DoubleValue)
            End If

            If StringValue.Length <> 0 Then
                output.WriteRawTag(58)
                output.WriteBytes(StringValue)
            End If

            If AggregateValue.Length <> 0 Then
                output.WriteRawTag(66)
                output.WriteString(AggregateValue)
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0
            size += name_.CalculateSize(_repeated_name_codec)

            If IdentifierValue.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeStringSize(IdentifierValue)
            End If

            If PositiveIntValue <> 0UL Then
                size += 1 + CodedOutputStream.ComputeUInt64Size(PositiveIntValue)
            End If

            If NegativeIntValue <> 0L Then
                size += 1 + CodedOutputStream.ComputeInt64Size(NegativeIntValue)
            End If

            If DoubleValue <> 0R Then
                size += 1 + 8
            End If

            If StringValue.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeBytesSize(StringValue)
            End If

            If AggregateValue.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeStringSize(AggregateValue)
            End If

            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As UninterpretedOption) Implements IMessageType(Of UninterpretedOption).MergeFrom
            If other Is Nothing Then
                Return
            End If

            name_.Add(other.name_)

            If other.IdentifierValue.Length <> 0 Then
                IdentifierValue = other.IdentifierValue
            End If

            If other.PositiveIntValue <> 0UL Then
                PositiveIntValue = other.PositiveIntValue
            End If

            If other.NegativeIntValue <> 0L Then
                NegativeIntValue = other.NegativeIntValue
            End If

            If other.DoubleValue <> 0R Then
                DoubleValue = other.DoubleValue
            End If

            If other.StringValue.Length <> 0 Then
                StringValue = other.StringValue
            End If

            If other.AggregateValue.Length <> 0 Then
                AggregateValue = other.AggregateValue
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
            Dim tag As New Value(Of UInteger)

            While ((tag = input.ReadTag())) <> 0

                Select Case tag.Value
                    Case 18
                        name_.AddEntriesFrom(input, _repeated_name_codec)

                    Case 26
                        IdentifierValue = input.ReadString()

                    Case 32
                        PositiveIntValue = input.ReadUInt64()

                    Case 40
                        NegativeIntValue = input.ReadInt64()

                    Case 49
                        DoubleValue = input.ReadDouble()

                    Case 58
                        StringValue = input.ReadBytes()

                    Case 66
                        AggregateValue = input.ReadString()

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub

#Region "Nested types"
        ''' <summary>Container for nested types declared in the UninterpretedOption message type.</summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Partial Public NotInheritable Class Types
            ''' <summary>
            '''  The name of the uninterpreted option.  Each string represents a segment in
            '''  a dot-separated name.  is_extension is true iff a segment represents an
            '''  extension (denoted with parentheses in options specs in .proto files).
            '''  E.g.,{ ["foo", false], ["bar.baz", true], ["qux", false] } represents
            '''  "foo.(bar.baz).qux".
            ''' </summary>
            Partial Friend NotInheritable Class NamePart
                Implements IMessageType(Of NamePart)

                Private Shared ReadOnly _parser As MessageParserType(Of NamePart) = New MessageParserType(Of NamePart)(Function() New NamePart())

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Shared ReadOnly Property Parser As MessageParserType(Of NamePart)
                    Get
                        Return _parser
                    End Get
                End Property

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Shared ReadOnly Property DescriptorProp As MessageDescriptor
                    Get
                        Return Global.Google.Protobuf.Reflection.UninterpretedOption.DescriptorProp.NestedTypes(0)
                    End Get
                End Property

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Private ReadOnly Property Descriptor As MessageDescriptor Implements IMessage.Descriptor
                    Get
                        Return DescriptorProp
                    End Get
                End Property

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Sub New()
                    OnConstruction()
                End Sub

                Partial Private Sub OnConstruction()
                End Sub

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Sub New(other As NamePart)
                    Me.New()
                    namePart_Field = other.namePart_Field
                    isExtension_ = other.isExtension_
                End Sub

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Function Clone() As NamePart Implements IDeepCloneable(Of NamePart).Clone
                    Return New NamePart(Me)
                End Function

                ''' <summary>Field number for the "name_part" field.</summary>
                Public Const NamePart_FieldNumber As Integer = 1
                Private namePart_Field As String = ""

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Property NamePart_ As String
                    Get
                        Return namePart_Field
                    End Get
                    Set(value As String)
                        namePart_Field = CheckNotNull(value, "value")
                    End Set
                End Property

                ''' <summary>Field number for the "is_extension" field.</summary>
                Public Const IsExtensionFieldNumber As Integer = 2
                Private isExtension_ As Boolean

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Property IsExtension As Boolean
                    Get
                        Return isExtension_
                    End Get
                    Set(value As Boolean)
                        isExtension_ = value
                    End Set
                End Property

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Overrides Function Equals(other As Object) As Boolean
                    Return Equals(TryCast(other, NamePart))
                End Function

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Overloads Function Equals(other As NamePart) As Boolean Implements IEquatable(Of NamePart).Equals
                    If ReferenceEquals(other, Nothing) Then
                        Return False
                    End If

                    If ReferenceEquals(other, Me) Then
                        Return True
                    End If

                    If Not Equals(NamePart_, other.NamePart_) Then Return False
                    If IsExtension <> other.IsExtension Then Return False
                    Return True
                End Function

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Overrides Function GetHashCode() As Integer
                    Dim hash = 1
                    If NamePart_.Length <> 0 Then hash = hash Xor NamePart_.GetHashCode()
                    If IsExtension <> False Then hash = hash Xor IsExtension.GetHashCode()
                    Return hash
                End Function

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Overrides Function ToString() As String
                    Return JsonFormatter.ToDiagnosticString(Me)
                End Function

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
                    If NamePart_.Length <> 0 Then
                        output.WriteRawTag(10)
                        output.WriteString(NamePart_)
                    End If

                    If IsExtension <> False Then
                        output.WriteRawTag(16)
                        output.WriteBool(IsExtension)
                    End If
                End Sub

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
                    Dim size = 0

                    If NamePart_.Length <> 0 Then
                        size += 1 + CodedOutputStream.ComputeStringSize(NamePart_)
                    End If

                    If IsExtension <> False Then
                        size += 1 + 1
                    End If

                    Return size
                End Function

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Sub MergeFrom(other As NamePart) Implements IMessageType(Of NamePart).MergeFrom
                    If other Is Nothing Then
                        Return
                    End If

                    If other.NamePart_.Length <> 0 Then
                        NamePart_ = other.NamePart_
                    End If

                    If other.IsExtension <> False Then
                        IsExtension = other.IsExtension
                    End If
                End Sub

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
                    Dim tag As New Value(Of UInteger)

                    While ((tag = input.ReadTag())) <> 0

                        Select Case tag.Value
                            Case 10
                                NamePart_ = input.ReadString()

                            Case 16
                                IsExtension = input.ReadBool()

                            Case Else
                                input.SkipLastField()
                        End Select
                    End While
                End Sub
            End Class
        End Class
#End Region

    End Class

    ''' <summary>
    '''  Encapsulates information about the original source file from which a
    '''  FileDescriptorProto was generated.
    ''' </summary>
    Partial Friend NotInheritable Class SourceCodeInfo
        Implements IMessageType(Of SourceCodeInfo)

        Private Shared ReadOnly _parser As MessageParserType(Of SourceCodeInfo) = New MessageParserType(Of SourceCodeInfo)(Function() New SourceCodeInfo())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of SourceCodeInfo)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As MessageDescriptor
            Get
                Return Global.Google.Protobuf.Reflection.DescriptorReflection.Descriptor.MessageTypes(18)
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Private ReadOnly Property Descriptor As MessageDescriptor Implements IMessage.Descriptor
            Get
                Return DescriptorProp
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New()
            OnConstruction()
        End Sub

        Partial Private Sub OnConstruction()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New(other As SourceCodeInfo)
            Me.New()
            location_ = other.location_.Clone()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As SourceCodeInfo Implements IDeepCloneable(Of SourceCodeInfo).Clone
            Return New SourceCodeInfo(Me)
        End Function

        ''' <summary>Field number for the "location" field.</summary>
        Public Const LocationFieldNumber As Integer = 1
        Private Shared ReadOnly _repeated_location_codec As FieldCodecType(Of Global.Google.Protobuf.Reflection.SourceCodeInfo.Types.Location) = FieldCodec.ForMessage(10, Global.Google.Protobuf.Reflection.SourceCodeInfo.Types.Location.Parser)
        Private ReadOnly location_ As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.SourceCodeInfo.Types.Location) = New pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.SourceCodeInfo.Types.Location)()
        ''' <summary>
        '''  A Location identifies a piece of source code in a .proto file which
        '''  corresponds to a particular definition.  This information is intended
        '''  to be useful to IDEs, code indexers, documentation generators, and similar
        '''  tools.
        '''
        '''  For example, say we have a file like:
        '''    message Foo {
        '''      optional string foo = 1;
        '''    }
        '''  Let's look at just the field definition:
        '''    optional string foo = 1;
        '''    ^       ^^     ^^  ^  ^^^
        '''    a       bc     de  f  ghi
        '''  We have the following locations:
        '''    span   path               represents
        '''    [a,i)  [ 4, 0, 2, 0 ]     The whole field definition.
        '''    [a,b)  [ 4, 0, 2, 0, 4 ]  The label (optional).
        '''    [c,d)  [ 4, 0, 2, 0, 5 ]  The type (string).
        '''    [e,f)  [ 4, 0, 2, 0, 1 ]  The name (foo).
        '''    [g,h)  [ 4, 0, 2, 0, 3 ]  The number (1).
        '''
        '''  Notes:
        '''  - A location may refer to a repeated field itself (i.e. not to any
        '''    particular index within it).  This is used whenever a set of elements are
        '''    logically enclosed in a single code segment.  For example, an entire
        '''    extend block (possibly containing multiple extension definitions) will
        '''    have an outer location whose path refers to the "extensions" repeated
        '''    field without an index.
        '''  - Multiple locations may have the same path.  This happens when a single
        '''    logical declaration is spread out across multiple places.  The most
        '''    obvious example is the "extend" block again -- there may be multiple
        '''    extend blocks in the same scope, each of which will have the same path.
        '''  - A location's span is not always a subset of its parent's span.  For
        '''    example, the "extendee" of an extension declaration appears at the
        '''    beginning of the "extend" block and is shared by all extensions within
        '''    the block.
        '''  - Just because a location's span is a subset of some other location's span
        '''    does not mean that it is a descendent.  For example, a "group" defines
        '''    both a type and a field in a single declaration.  Thus, the locations
        '''    corresponding to the type and field and their components will overlap.
        '''  - Code which tries to interpret locations should probably be designed to
        '''    ignore those that it doesn't understand, as more types of locations could
        '''    be recorded in the future.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property Location As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.SourceCodeInfo.Types.Location)
            Get
                Return location_
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, SourceCodeInfo))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As SourceCodeInfo) As Boolean Implements IEquatable(Of SourceCodeInfo).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If

            If ReferenceEquals(other, Me) Then
                Return True
            End If

            If Not location_.Equals(other.location_) Then Return False
            Return True
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function GetHashCode() As Integer
            Dim hash = 1
            hash = hash Xor location_.GetHashCode()
            Return hash
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function ToString() As String
            Return JsonFormatter.ToDiagnosticString(Me)
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
            location_.WriteTo(output, _repeated_location_codec)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0
            size += location_.CalculateSize(_repeated_location_codec)
            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As SourceCodeInfo) Implements IMessageType(Of SourceCodeInfo).MergeFrom
            If other Is Nothing Then
                Return
            End If

            location_.Add(other.location_)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
            Dim tag As New Value(Of UInteger)

            While ((tag = input.ReadTag())) <> 0

                Select Case tag.Value
                    Case 10
                        location_.AddEntriesFrom(input, _repeated_location_codec)

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub

#Region "Nested types"
        ''' <summary>Container for nested types declared in the SourceCodeInfo message type.</summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Partial Public NotInheritable Class Types
            Partial Friend NotInheritable Class Location
                Implements IMessageType(Of Location)

                Private Shared ReadOnly _parser As MessageParserType(Of Location) = New MessageParserType(Of Location)(Function() New Location())

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Shared ReadOnly Property Parser As MessageParserType(Of Location)
                    Get
                        Return _parser
                    End Get
                End Property

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Shared ReadOnly Property DescriptorProp As MessageDescriptor
                    Get
                        Return Global.Google.Protobuf.Reflection.SourceCodeInfo.DescriptorProp.NestedTypes(0)
                    End Get
                End Property

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Private ReadOnly Property Descriptor As MessageDescriptor Implements IMessage.Descriptor
                    Get
                        Return DescriptorProp
                    End Get
                End Property

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Sub New()
                    OnConstruction()
                End Sub

                Partial Private Sub OnConstruction()
                End Sub

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Sub New(other As Location)
                    Me.New()
                    path_ = other.path_.Clone()
                    span_ = other.span_.Clone()
                    leadingComments_ = other.leadingComments_
                    trailingComments_ = other.trailingComments_
                    leadingDetachedComments_ = other.leadingDetachedComments_.Clone()
                End Sub

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Function Clone() As Location Implements IDeepCloneable(Of Location).Clone
                    Return New Location(Me)
                End Function

                ''' <summary>Field number for the "path" field.</summary>
                Public Const PathFieldNumber As Integer = 1
                Private Shared ReadOnly _repeated_path_codec As FieldCodecType(Of Integer) = ForInt32(10)
                Private ReadOnly path_ As pbc.RepeatedField(Of Integer) = New pbc.RepeatedField(Of Integer)()
                ''' <summary>
                '''  Identifies which part of the FileDescriptorProto was defined at this
                '''  location.
                '''
                '''  Each element is a field number or an index.  They form a path from
                '''  the root FileDescriptorProto to the place where the definition.  For
                '''  example, this path:
                '''    [ 4, 3, 2, 7, 1 ]
                '''  refers to:
                '''    file.message_type(3)  // 4, 3
                '''        .field(7)         // 2, 7
                '''        .name()           // 1
                '''  This is because FileDescriptorProto.message_type has field number 4:
                '''    repeated DescriptorProto message_type = 4;
                '''  and DescriptorProto.field has field number 2:
                '''    repeated FieldDescriptorProto field = 2;
                '''  and FieldDescriptorProto.name has field number 1:
                '''    optional string name = 1;
                '''
                '''  Thus, the above path gives the location of a field name.  If we removed
                '''  the last element:
                '''    [ 4, 3, 2, 7 ]
                '''  this path refers to the whole field declaration (from the beginning
                '''  of the label to the terminating semicolon).
                ''' </summary>
                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public ReadOnly Property Path As pbc.RepeatedField(Of Integer)
                    Get
                        Return path_
                    End Get
                End Property

                ''' <summary>Field number for the "span" field.</summary>
                Public Const SpanFieldNumber As Integer = 2
                Private Shared ReadOnly _repeated_span_codec As FieldCodecType(Of Integer) = ForInt32(18)
                Private ReadOnly span_ As pbc.RepeatedField(Of Integer) = New pbc.RepeatedField(Of Integer)()
                ''' <summary>
                '''  Always has exactly three or four elements: start line, start column,
                '''  end line (optional, otherwise assumed same as start line), end column.
                '''  These are packed into a single field for efficiency.  Note that line
                '''  and column numbers are zero-based -- typically you will want to add
                '''  1 to each before displaying to a user.
                ''' </summary>
                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public ReadOnly Property Span As pbc.RepeatedField(Of Integer)
                    Get
                        Return span_
                    End Get
                End Property

                ''' <summary>Field number for the "leading_comments" field.</summary>
                Public Const LeadingCommentsFieldNumber As Integer = 3
                Private leadingComments_ As String = ""
                ''' <summary>
                '''  If this SourceCodeInfo represents a complete declaration, these are any
                '''  comments appearing before and after the declaration which appear to be
                '''  attached to the declaration.
                '''
                '''  A series of line comments appearing on consecutive lines, with no other
                '''  tokens appearing on those lines, will be treated as a single comment.
                '''
                '''  leading_detached_comments will keep paragraphs of comments that appear
                '''  before (but not connected to) the current element. Each paragraph,
                '''  separated by empty lines, will be one comment element in the repeated
                '''  field.
                '''
                '''  Only the comment content is provided; comment markers (e.g. //) are
                '''  stripped out.  For block comments, leading whitespace and an asterisk
                '''  will be stripped from the beginning of each line other than the first.
                '''  Newlines are included in the output.
                '''
                '''  Examples:
                '''
                '''    optional int32 foo = 1;  // Comment attached to foo.
                '''    // Comment attached to bar.
                '''    optional int32 bar = 2;
                '''
                '''    optional string baz = 3;
                '''    // Comment attached to baz.
                '''    // Another line attached to baz.
                '''
                '''    // Comment attached to qux.
                '''    //
                '''    // Another line attached to qux.
                '''    optional double qux = 4;
                '''
                '''    // Detached comment for corge. This is not leading or trailing comments
                '''    // to qux or corge because there are blank lines separating it from
                '''    // both.
                '''
                '''    // Detached comment for corge paragraph 2.
                '''
                '''    optional string corge = 5;
                '''    /* Block comment attached
                '''     * to corge.  Leading asterisks
                '''     * will be removed. */
                '''    /* Block comment attached to
                '''     * grault. */
                '''    optional int32 grault = 6;
                '''
                '''    // ignored detached comments.
                ''' </summary>
                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Property LeadingComments As String
                    Get
                        Return leadingComments_
                    End Get
                    Set(value As String)
                        leadingComments_ = CheckNotNull(value, "value")
                    End Set
                End Property

                ''' <summary>Field number for the "trailing_comments" field.</summary>
                Public Const TrailingCommentsFieldNumber As Integer = 4
                Private trailingComments_ As String = ""

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Property TrailingComments As String
                    Get
                        Return trailingComments_
                    End Get
                    Set(value As String)
                        trailingComments_ = CheckNotNull(value, "value")
                    End Set
                End Property

                ''' <summary>Field number for the "leading_detached_comments" field.</summary>
                Public Const LeadingDetachedCommentsFieldNumber As Integer = 6
                Private Shared ReadOnly _repeated_leadingDetachedComments_codec As FieldCodecType(Of String) = ForString(50)
                Private ReadOnly leadingDetachedComments_ As pbc.RepeatedField(Of String) = New pbc.RepeatedField(Of String)()

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public ReadOnly Property LeadingDetachedComments As pbc.RepeatedField(Of String)
                    Get
                        Return leadingDetachedComments_
                    End Get
                End Property

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Overrides Function Equals(other As Object) As Boolean
                    Return Equals(TryCast(other, Location))
                End Function

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Overloads Function Equals(other As Location) As Boolean Implements IEquatable(Of Location).Equals
                    If ReferenceEquals(other, Nothing) Then
                        Return False
                    End If

                    If ReferenceEquals(other, Me) Then
                        Return True
                    End If

                    If Not path_.Equals(other.path_) Then Return False
                    If Not span_.Equals(other.span_) Then Return False
                    If Not Equals(LeadingComments, other.LeadingComments) Then Return False
                    If Not Equals(TrailingComments, other.TrailingComments) Then Return False
                    If Not leadingDetachedComments_.Equals(other.leadingDetachedComments_) Then Return False
                    Return True
                End Function

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Overrides Function GetHashCode() As Integer
                    Dim hash = 1
                    hash = hash Xor path_.GetHashCode()
                    hash = hash Xor span_.GetHashCode()
                    If LeadingComments.Length <> 0 Then hash = hash Xor LeadingComments.GetHashCode()
                    If TrailingComments.Length <> 0 Then hash = hash Xor TrailingComments.GetHashCode()
                    hash = hash Xor leadingDetachedComments_.GetHashCode()
                    Return hash
                End Function

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Overrides Function ToString() As String
                    Return JsonFormatter.ToDiagnosticString(Me)
                End Function

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
                    path_.WriteTo(output, _repeated_path_codec)
                    span_.WriteTo(output, _repeated_span_codec)

                    If LeadingComments.Length <> 0 Then
                        output.WriteRawTag(26)
                        output.WriteString(LeadingComments)
                    End If

                    If TrailingComments.Length <> 0 Then
                        output.WriteRawTag(34)
                        output.WriteString(TrailingComments)
                    End If

                    leadingDetachedComments_.WriteTo(output, _repeated_leadingDetachedComments_codec)
                End Sub

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
                    Dim size = 0
                    size += path_.CalculateSize(_repeated_path_codec)
                    size += span_.CalculateSize(_repeated_span_codec)

                    If LeadingComments.Length <> 0 Then
                        size += 1 + CodedOutputStream.ComputeStringSize(LeadingComments)
                    End If

                    If TrailingComments.Length <> 0 Then
                        size += 1 + CodedOutputStream.ComputeStringSize(TrailingComments)
                    End If

                    size += leadingDetachedComments_.CalculateSize(_repeated_leadingDetachedComments_codec)
                    Return size
                End Function

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Sub MergeFrom(other As Location) Implements IMessageType(Of Location).MergeFrom
                    If other Is Nothing Then
                        Return
                    End If

                    path_.Add(other.path_)
                    span_.Add(other.span_)

                    If other.LeadingComments.Length <> 0 Then
                        LeadingComments = other.LeadingComments
                    End If

                    If other.TrailingComments.Length <> 0 Then
                        TrailingComments = other.TrailingComments
                    End If

                    leadingDetachedComments_.Add(other.leadingDetachedComments_)
                End Sub

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
                    Dim tag As New Value(Of UInteger)

                    While ((tag = input.ReadTag())) <> 0

                        Select Case tag
                            Case 10, 8
                                path_.AddEntriesFrom(input, _repeated_path_codec)

                            Case 18, 16
                                span_.AddEntriesFrom(input, _repeated_span_codec)

                            Case 26
                                LeadingComments = input.ReadString()

                            Case 34
                                TrailingComments = input.ReadString()

                            Case 50
                                leadingDetachedComments_.AddEntriesFrom(input, _repeated_leadingDetachedComments_codec)

                            Case Else
                                input.SkipLastField()
                        End Select
                    End While
                End Sub
            End Class
        End Class
#End Region

    End Class

    ''' <summary>
    '''  Describes the relationship between generated code and its original source
    '''  file. A GeneratedCodeInfo message is associated with only one generated
    '''  source file, but may contain references to different source .proto files.
    ''' </summary>
    Partial Friend NotInheritable Class GeneratedCodeInfo
        Implements IMessageType(Of GeneratedCodeInfo)

        Private Shared ReadOnly _parser As MessageParserType(Of GeneratedCodeInfo) = New MessageParserType(Of GeneratedCodeInfo)(Function() New GeneratedCodeInfo())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of GeneratedCodeInfo)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As MessageDescriptor
            Get
                Return Global.Google.Protobuf.Reflection.DescriptorReflection.Descriptor.MessageTypes(19)
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Private ReadOnly Property Descriptor As MessageDescriptor Implements IMessage.Descriptor
            Get
                Return DescriptorProp
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New()
            OnConstruction()
        End Sub

        Partial Private Sub OnConstruction()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub New(other As GeneratedCodeInfo)
            Me.New()
            annotation_ = other.annotation_.Clone()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As GeneratedCodeInfo Implements IDeepCloneable(Of GeneratedCodeInfo).Clone
            Return New GeneratedCodeInfo(Me)
        End Function

        ''' <summary>Field number for the "annotation" field.</summary>
        Public Const AnnotationFieldNumber As Integer = 1
        Private Shared ReadOnly _repeated_annotation_codec As FieldCodecType(Of Global.Google.Protobuf.Reflection.GeneratedCodeInfo.Types.Annotation) = FieldCodec.ForMessage(10, Global.Google.Protobuf.Reflection.GeneratedCodeInfo.Types.Annotation.Parser)
        Private ReadOnly annotation_ As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.GeneratedCodeInfo.Types.Annotation) = New pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.GeneratedCodeInfo.Types.Annotation)()
        ''' <summary>
        '''  An Annotation connects some span of text in generated code to an element
        '''  of its generating .proto file.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property Annotation As pbc.RepeatedField(Of Global.Google.Protobuf.Reflection.GeneratedCodeInfo.Types.Annotation)
            Get
                Return annotation_
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, GeneratedCodeInfo))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As GeneratedCodeInfo) As Boolean Implements IEquatable(Of GeneratedCodeInfo).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If

            If ReferenceEquals(other, Me) Then
                Return True
            End If

            If Not annotation_.Equals(other.annotation_) Then Return False
            Return True
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function GetHashCode() As Integer
            Dim hash = 1
            hash = hash Xor annotation_.GetHashCode()
            Return hash
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function ToString() As String
            Return JsonFormatter.ToDiagnosticString(Me)
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
            annotation_.WriteTo(output, _repeated_annotation_codec)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0
            size += annotation_.CalculateSize(_repeated_annotation_codec)
            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As GeneratedCodeInfo) Implements IMessageType(Of GeneratedCodeInfo).MergeFrom
            If other Is Nothing Then
                Return
            End If

            annotation_.Add(other.annotation_)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
            Dim tag As New Value(Of UInteger)

            While ((tag = input.ReadTag())) <> 0

                Select Case tag.Value
                    Case 10
                        annotation_.AddEntriesFrom(input, _repeated_annotation_codec)

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub

#Region "Nested types"
        ''' <summary>Container for nested types declared in the GeneratedCodeInfo message type.</summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Partial Public NotInheritable Class Types
            Partial Friend NotInheritable Class Annotation
                Implements IMessageType(Of Annotation)

                Private Shared ReadOnly _parser As MessageParserType(Of Annotation) = New MessageParserType(Of Annotation)(Function() New Annotation())

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Shared ReadOnly Property Parser As MessageParserType(Of Annotation)
                    Get
                        Return _parser
                    End Get
                End Property

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Shared ReadOnly Property DescriptorProp As MessageDescriptor
                    Get
                        Return Global.Google.Protobuf.Reflection.GeneratedCodeInfo.DescriptorProp.NestedTypes(0)
                    End Get
                End Property

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Private ReadOnly Property Descriptor As MessageDescriptor Implements IMessage.Descriptor
                    Get
                        Return DescriptorProp
                    End Get
                End Property

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Sub New()
                    OnConstruction()
                End Sub

                Partial Private Sub OnConstruction()
                End Sub

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Sub New(other As Annotation)
                    Me.New()
                    path_ = other.path_.Clone()
                    sourceFile_ = other.sourceFile_
                    begin_ = other.begin_
                    end_ = other.end_
                End Sub

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Function Clone() As Annotation Implements IDeepCloneable(Of Annotation).Clone
                    Return New Annotation(Me)
                End Function

                ''' <summary>Field number for the "path" field.</summary>
                Public Const PathFieldNumber As Integer = 1
                Private Shared ReadOnly _repeated_path_codec As FieldCodecType(Of Integer) = ForInt32(10)
                Private ReadOnly path_ As pbc.RepeatedField(Of Integer) = New pbc.RepeatedField(Of Integer)()
                ''' <summary>
                '''  Identifies the element in the original source .proto file. This field
                '''  is formatted the same as SourceCodeInfo.Location.path.
                ''' </summary>
                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public ReadOnly Property Path As pbc.RepeatedField(Of Integer)
                    Get
                        Return path_
                    End Get
                End Property

                ''' <summary>Field number for the "source_file" field.</summary>
                Public Const SourceFileFieldNumber As Integer = 2
                Private sourceFile_ As String = ""
                ''' <summary>
                '''  Identifies the filesystem path to the original source .proto.
                ''' </summary>
                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Property SourceFile As String
                    Get
                        Return sourceFile_
                    End Get
                    Set(value As String)
                        sourceFile_ = CheckNotNull(value, "value")
                    End Set
                End Property

                ''' <summary>Field number for the "begin" field.</summary>
                Public Const BeginFieldNumber As Integer = 3
                Private begin_ As Integer
                ''' <summary>
                '''  Identifies the starting offset in bytes in the generated code
                '''  that relates to the identified object.
                ''' </summary>
                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Property Begin As Integer
                    Get
                        Return begin_
                    End Get
                    Set(value As Integer)
                        begin_ = value
                    End Set
                End Property

                ''' <summary>Field number for the "end" field.</summary>
                Public Const EndFieldNumber As Integer = 4
                Private end_ As Integer
                ''' <summary>
                '''  Identifies the ending offset in bytes in the generated code that
                '''  relates to the identified offset. The end offset should be one past
                '''  the last relevant byte (so the length of the text = end - begin).
                ''' </summary>
                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Property [End] As Integer
                    Get
                        Return end_
                    End Get
                    Set(value As Integer)
                        end_ = value
                    End Set
                End Property

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Overrides Function Equals(other As Object) As Boolean
                    Return Equals(TryCast(other, Annotation))
                End Function

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Overloads Function Equals(other As Annotation) As Boolean Implements IEquatable(Of Annotation).Equals
                    If ReferenceEquals(other, Nothing) Then
                        Return False
                    End If

                    If ReferenceEquals(other, Me) Then
                        Return True
                    End If

                    If Not path_.Equals(other.path_) Then Return False
                    If Not Equals(SourceFile, other.SourceFile) Then Return False
                    If Begin <> other.Begin Then Return False
                    If [End] <> other.End Then Return False
                    Return True
                End Function

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Overrides Function GetHashCode() As Integer
                    Dim hash = 1
                    hash = hash Xor path_.GetHashCode()
                    If SourceFile.Length <> 0 Then hash = hash Xor SourceFile.GetHashCode()
                    If Begin <> 0 Then hash = hash Xor Begin.GetHashCode()
                    If [End] <> 0 Then hash = hash Xor [End].GetHashCode()
                    Return hash
                End Function

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Overrides Function ToString() As String
                    Return JsonFormatter.ToDiagnosticString(Me)
                End Function

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
                    path_.WriteTo(output, _repeated_path_codec)

                    If SourceFile.Length <> 0 Then
                        output.WriteRawTag(18)
                        output.WriteString(SourceFile)
                    End If

                    If Begin <> 0 Then
                        output.WriteRawTag(24)
                        output.WriteInt32(Begin)
                    End If

                    If [End] <> 0 Then
                        output.WriteRawTag(32)
                        output.WriteInt32([End])
                    End If
                End Sub

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
                    Dim size = 0
                    size += path_.CalculateSize(_repeated_path_codec)

                    If SourceFile.Length <> 0 Then
                        size += 1 + CodedOutputStream.ComputeStringSize(SourceFile)
                    End If

                    If Begin <> 0 Then
                        size += 1 + CodedOutputStream.ComputeInt32Size(Begin)
                    End If

                    If [End] <> 0 Then
                        size += 1 + CodedOutputStream.ComputeInt32Size([End])
                    End If

                    Return size
                End Function

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Sub MergeFrom(other As Annotation) Implements IMessageType(Of Annotation).MergeFrom
                    If other Is Nothing Then
                        Return
                    End If

                    path_.Add(other.path_)

                    If other.SourceFile.Length <> 0 Then
                        SourceFile = other.SourceFile
                    End If

                    If other.Begin <> 0 Then
                        Begin = other.Begin
                    End If

                    If other.End <> 0 Then
                        [End] = other.End
                    End If
                End Sub

                <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
                Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
                    Dim tag As New Value(Of UInteger)

                    While ((tag = input.ReadTag())) <> 0

                        Select Case tag.Value
                            Case 10, 8
                                path_.AddEntriesFrom(input, _repeated_path_codec)

                            Case 18
                                SourceFile = input.ReadString()

                            Case 24
                                Begin = input.ReadInt32()

                            Case 32
                                [End] = input.ReadInt32()

                            Case Else
                                input.SkipLastField()
                        End Select
                    End While
                End Sub
            End Class
        End Class
#End Region

    End Class

#End Region

End Namespace
#End Region
