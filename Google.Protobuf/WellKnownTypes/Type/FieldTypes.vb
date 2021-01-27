#Region "Microsoft.VisualBasic::543fc2c23ed6a07c445950edf7d55b0c, Google.Protobuf\WellKnownTypes\Type\FieldTypes.vb"

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

    '     Class FieldTypes
    ' 
    ' 
    '         Enum Kind
    ' 
    ' 
    ' 
    ' 
    '         Enum Cardinality
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
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Language
Imports pbc = Google.Protobuf.Collections
Imports pbr = Google.Protobuf.Reflection

Namespace Google.Protobuf.WellKnownTypes
#Region "Nested types"
    ''' <summary>Container for nested types declared in the Field message type.</summary>
    <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
    Partial Public NotInheritable Class FieldTypes
        ''' <summary>
        '''  Basic field types.
        ''' </summary>
        Public Enum Kind
            ''' <summary>
            '''  Field type unknown.
            ''' </summary>
            <pbr.OriginalName("TYPE_UNKNOWN")>
            TypeUnknown = 0
            ''' <summary>
            '''  Field type double.
            ''' </summary>
            <pbr.OriginalName("TYPE_DOUBLE")>
            TypeDouble = 1
            ''' <summary>
            '''  Field type float.
            ''' </summary>
            <pbr.OriginalName("TYPE_FLOAT")>
            TypeFloat = 2
            ''' <summary>
            '''  Field type int64.
            ''' </summary>
            <pbr.OriginalName("TYPE_INT64")>
            TypeInt64 = 3
            ''' <summary>
            '''  Field type uint64.
            ''' </summary>
            <pbr.OriginalName("TYPE_UINT64")>
            TypeUint64 = 4
            ''' <summary>
            '''  Field type int32.
            ''' </summary>
            <pbr.OriginalName("TYPE_INT32")>
            TypeInt32 = 5
            ''' <summary>
            '''  Field type fixed64.
            ''' </summary>
            <pbr.OriginalName("TYPE_FIXED64")>
            TypeFixed64 = 6
            ''' <summary>
            '''  Field type fixed32.
            ''' </summary>
            <pbr.OriginalName("TYPE_FIXED32")>
            TypeFixed32 = 7
            ''' <summary>
            '''  Field type bool.
            ''' </summary>
            <pbr.OriginalName("TYPE_BOOL")>
            TypeBool = 8
            ''' <summary>
            '''  Field type string.
            ''' </summary>
            <pbr.OriginalName("TYPE_STRING")>
            TypeString = 9
            ''' <summary>
            '''  Field type group. Proto2 syntax only, and deprecated.
            ''' </summary>
            <pbr.OriginalName("TYPE_GROUP")>
            TypeGroup = 10
            ''' <summary>
            '''  Field type message.
            ''' </summary>
            <pbr.OriginalName("TYPE_MESSAGE")>
            TypeMessage = 11
            ''' <summary>
            '''  Field type bytes.
            ''' </summary>
            <pbr.OriginalName("TYPE_BYTES")>
            TypeBytes = 12
            ''' <summary>
            '''  Field type uint32.
            ''' </summary>
            <pbr.OriginalName("TYPE_UINT32")>
            TypeUint32 = 13
            ''' <summary>
            '''  Field type enum.
            ''' </summary>
            <pbr.OriginalName("TYPE_ENUM")>
            TypeEnum = 14
            ''' <summary>
            '''  Field type sfixed32.
            ''' </summary>
            <pbr.OriginalName("TYPE_SFIXED32")>
            TypeSfixed32 = 15
            ''' <summary>
            '''  Field type sfixed64.
            ''' </summary>
            <pbr.OriginalName("TYPE_SFIXED64")>
            TypeSfixed64 = 16
            ''' <summary>
            '''  Field type sint32.
            ''' </summary>
            <pbr.OriginalName("TYPE_SINT32")>
            TypeSint32 = 17
            ''' <summary>
            '''  Field type sint64.
            ''' </summary>
            <pbr.OriginalName("TYPE_SINT64")>
            TypeSint64 = 18
        End Enum

        ''' <summary>
        '''  Whether a field is optional, required, or repeated.
        ''' </summary>
        Public Enum Cardinality
            ''' <summary>
            '''  For fields with unknown cardinality.
            ''' </summary>
            <pbr.OriginalName("CARDINALITY_UNKNOWN")>
            Unknown = 0
            ''' <summary>
            '''  For optional fields.
            ''' </summary>
            <pbr.OriginalName("CARDINALITY_OPTIONAL")>
            [Optional] = 1
            ''' <summary>
            '''  For required fields. Proto2 syntax only.
            ''' </summary>
            <pbr.OriginalName("CARDINALITY_REQUIRED")>
            Required = 2
            ''' <summary>
            '''  For repeated fields.
            ''' </summary>
            <pbr.OriginalName("CARDINALITY_REPEATED")>
            Repeated = 3
        End Enum
    End Class
#End Region
End Namespace
