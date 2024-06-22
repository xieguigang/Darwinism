#Region "Microsoft.VisualBasic::f9108dc26f255452002a3fe9fc4a1c1b, src\message\Google.Protobuf\WellKnownTypes\FieldMaskReflection.vb"

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
    '     File Size: 1.45 KB


    '     Module FieldMaskReflection
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
