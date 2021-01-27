#Region "Microsoft.VisualBasic::5579761566c0d0dff8160051ad86f1db, Google.Protobuf\WellKnownTypes\Struct\StructReflection.vb"

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

    '     Module StructReflection
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

    ''' <summary>Holder for reflection information generated from google/protobuf/struct.proto</summary>
    Partial Public Module StructReflection

#Region "Descriptor"
        ''' <summary>File descriptor for google/protobuf/struct.proto</summary>
        Public ReadOnly Property Descriptor As pbr.FileDescriptor
            Get
                Return descriptorField
            End Get
        End Property

        Private descriptorField As pbr.FileDescriptor

        Sub New()
            Dim descriptorData As Byte() = Global.System.Convert.FromBase64String(String.Concat("Chxnb29nbGUvcHJvdG9idWYvc3RydWN0LnByb3RvEg9nb29nbGUucHJvdG9i", "dWYihAEKBlN0cnVjdBIzCgZmaWVsZHMYASADKAsyIy5nb29nbGUucHJvdG9i", "dWYuU3RydWN0LkZpZWxkc0VudHJ5GkUKC0ZpZWxkc0VudHJ5EgsKA2tleRgB", "IAEoCRIlCgV2YWx1ZRgCIAEoCzIWLmdvb2dsZS5wcm90b2J1Zi5WYWx1ZToC", "OAEi6gEKBVZhbHVlEjAKCm51bGxfdmFsdWUYASABKA4yGi5nb29nbGUucHJv", "dG9idWYuTnVsbFZhbHVlSAASFgoMbnVtYmVyX3ZhbHVlGAIgASgBSAASFgoM", "c3RyaW5nX3ZhbHVlGAMgASgJSAASFAoKYm9vbF92YWx1ZRgEIAEoCEgAEi8K", "DHN0cnVjdF92YWx1ZRgFIAEoCzIXLmdvb2dsZS5wcm90b2J1Zi5TdHJ1Y3RI", "ABIwCgpsaXN0X3ZhbHVlGAYgASgLMhouZ29vZ2xlLnByb3RvYnVmLkxpc3RW", "YWx1ZUgAQgYKBGtpbmQiMwoJTGlzdFZhbHVlEiYKBnZhbHVlcxgBIAMoCzIW", "Lmdvb2dsZS5wcm90b2J1Zi5WYWx1ZSobCglOdWxsVmFsdWUSDgoKTlVMTF9W", "QUxVRRAAQoEBChNjb20uZ29vZ2xlLnByb3RvYnVmQgtTdHJ1Y3RQcm90b1AB", "WjFnaXRodWIuY29tL2dvbGFuZy9wcm90b2J1Zi9wdHlwZXMvc3RydWN0O3N0", "cnVjdHBioAEBogIDR1BCqgIeR29vZ2xlLlByb3RvYnVmLldlbGxLbm93blR5", "cGVzYgZwcm90bzM="))
            descriptorField = pbr.FileDescriptor.FromGeneratedCode(descriptorData, New pbr.FileDescriptor() {}, New pbr.GeneratedClrTypeInfo({GetType(Global.Google.Protobuf.WellKnownTypes.NullValue)}, New pbr.GeneratedClrTypeInfo() {New pbr.GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.WellKnownTypes.Struct), Global.Google.Protobuf.WellKnownTypes.Struct.Parser, {"Fields"}, Nothing, Nothing, New pbr.GeneratedClrTypeInfo() {Nothing}), New pbr.GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.WellKnownTypes.Value), Global.Google.Protobuf.WellKnownTypes.Value.Parser, {"NullValue", "NumberValue", "StringValue", "BoolValue", "StructValue", "ListValue"}, {"Kind"}, Nothing, Nothing), New pbr.GeneratedClrTypeInfo(GetType(Global.Google.Protobuf.WellKnownTypes.ListValue), Global.Google.Protobuf.WellKnownTypes.ListValue.Parser, {"Values"}, Nothing, Nothing, Nothing)}))
        End Sub
#End Region

    End Module
End Namespace
