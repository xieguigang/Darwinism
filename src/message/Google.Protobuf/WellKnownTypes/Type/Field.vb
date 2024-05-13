#Region "Microsoft.VisualBasic::ed7a61661976304c8979fe2fd385c887, src\message\Google.Protobuf\WellKnownTypes\Type\Field.vb"

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

    '   Total Lines: 456
    '    Code Lines: 339
    ' Comment Lines: 45
    '   Blank Lines: 72
    '     File Size: 16.69 KB


    '     Class Field
    ' 
    '         Properties: Cardinality, DefaultValue, Descriptor, DescriptorProp, JsonName
    '                     Kind, Name, Number, OneofIndex, Options
    '                     Packed, Parser, TypeUrl
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '         Function: CalculateSize, Clone, (+2 Overloads) Equals, GetHashCode, ToString
    ' 
    '         Sub: (+2 Overloads) MergeFrom, OnConstruction, WriteTo
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Language
Imports pbc = Google.Protobuf.Collections
Imports pbr = Google.Protobuf.Reflection

Namespace Google.Protobuf.WellKnownTypes
    ''' <summary>
    '''  A single field of a message type.
    ''' </summary>
    Partial Public NotInheritable Class Field
        Implements IMessageType(Of Field)

        Private Shared ReadOnly _parser As MessageParserType(Of Field) = New MessageParserType(Of Field)(Function() New Field())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of Field)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As pbr.MessageDescriptor
            Get
                Return Global.Google.Protobuf.WellKnownTypes.TypeReflection.Descriptor.MessageTypes(1)
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Private ReadOnly Property Descriptor As pbr.MessageDescriptor Implements IMessage.Descriptor
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
        Public Sub New(other As Field)
            Me.New()
            kind_ = other.kind_
            cardinality_ = other.cardinality_
            number_ = other.number_
            name_ = other.name_
            typeUrl_ = other.typeUrl_
            oneofIndex_ = other.oneofIndex_
            packed_ = other.packed_
            options_ = other.options_.Clone()
            jsonName_ = other.jsonName_
            defaultValue_ = other.defaultValue_
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As Field Implements IDeepCloneable(Of Field).Clone
            Return New Field(Me)
        End Function

        ''' <summary>Field number for the "kind" field.</summary>
        Public Const KindFieldNumber As Integer = 1
        Private kind_ As FieldTypes.Kind = 0
        ''' <summary>
        '''  The field type.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Kind As FieldTypes.Kind
            Get
                Return kind_
            End Get
            Set(value As FieldTypes.Kind)
                kind_ = value
            End Set
        End Property

        ''' <summary>Field number for the "cardinality" field.</summary>
        Public Const CardinalityFieldNumber As Integer = 2
        Private cardinality_ As FieldTypes.Cardinality = 0
        ''' <summary>
        '''  The field cardinality.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Cardinality As FieldTypes.Cardinality
            Get
                Return cardinality_
            End Get
            Set(value As FieldTypes.Cardinality)
                cardinality_ = value
            End Set
        End Property

        ''' <summary>Field number for the "number" field.</summary>
        Public Const NumberFieldNumber As Integer = 3
        Private number_ As Integer
        ''' <summary>
        '''  The field number.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Number As Integer
            Get
                Return number_
            End Get
            Set(value As Integer)
                number_ = value
            End Set
        End Property

        ''' <summary>Field number for the "name" field.</summary>
        Public Const NameFieldNumber As Integer = 4
        Private name_ As String = ""
        ''' <summary>
        '''  The field name.
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

        ''' <summary>Field number for the "type_url" field.</summary>
        Public Const TypeUrlFieldNumber As Integer = 6
        Private typeUrl_ As String = ""
        ''' <summary>
        '''  The field type URL, without the scheme, for message or enumeration
        '''  types. Example: `"type.googleapis.com/google.protobuf.Timestamp"`.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property TypeUrl As String
            Get
                Return typeUrl_
            End Get
            Set(value As String)
                typeUrl_ = CheckNotNull(value, "value")
            End Set
        End Property

        ''' <summary>Field number for the "oneof_index" field.</summary>
        Public Const OneofIndexFieldNumber As Integer = 7
        Private oneofIndex_ As Integer
        ''' <summary>
        '''  The index of the field type in `Type.oneofs`, for message or enumeration
        '''  types. The first type has index 1; zero means the type is not in the list.
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

        ''' <summary>Field number for the "packed" field.</summary>
        Public Const PackedFieldNumber As Integer = 8
        Private packed_ As Boolean
        ''' <summary>
        '''  Whether to use alternative packed wire representation.
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

        ''' <summary>Field number for the "options" field.</summary>
        Public Const OptionsFieldNumber As Integer = 9
        Private Shared ReadOnly _repeated_options_codec As FieldCodecType(Of Global.Google.Protobuf.WellKnownTypes.Option) = FieldCodec.ForMessage(74, Global.Google.Protobuf.WellKnownTypes.Option.Parser)
        Private ReadOnly options_ As pbc.RepeatedField(Of Global.Google.Protobuf.WellKnownTypes.Option) = New pbc.RepeatedField(Of Global.Google.Protobuf.WellKnownTypes.Option)()
        ''' <summary>
        '''  The protocol buffer options.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property Options As pbc.RepeatedField(Of Global.Google.Protobuf.WellKnownTypes.Option)
            Get
                Return options_
            End Get
        End Property

        ''' <summary>Field number for the "json_name" field.</summary>
        Public Const JsonNameFieldNumber As Integer = 10
        Private jsonName_ As String = ""
        ''' <summary>
        '''  The field JSON name.
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

        ''' <summary>Field number for the "default_value" field.</summary>
        Public Const DefaultValueFieldNumber As Integer = 11
        Private defaultValue_ As String = ""
        ''' <summary>
        '''  The string value of the default value of this field. Proto2 syntax only.
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

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, Field))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As Field) As Boolean Implements IEquatable(Of Field).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If

            If ReferenceEquals(other, Me) Then
                Return True
            End If

            If Kind <> other.Kind Then Return False
            If Cardinality <> other.Cardinality Then Return False
            If Number <> other.Number Then Return False
            If Not Equals(Name, other.Name) Then Return False
            If Not Equals(TypeUrl, other.TypeUrl) Then Return False
            If OneofIndex <> other.OneofIndex Then Return False
            If Packed <> other.Packed Then Return False
            If Not options_.Equals(other.options_) Then Return False
            If Not Equals(JsonName, other.JsonName) Then Return False
            If Not Equals(DefaultValue, other.DefaultValue) Then Return False
            Return True
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function GetHashCode() As Integer
            Dim hash = 1
            If Kind <> 0 Then hash = hash Xor Kind.GetHashCode()
            If Cardinality <> 0 Then hash = hash Xor Cardinality.GetHashCode()
            If Number <> 0 Then hash = hash Xor Number.GetHashCode()
            If Name.Length <> 0 Then hash = hash Xor Name.GetHashCode()
            If TypeUrl.Length <> 0 Then hash = hash Xor TypeUrl.GetHashCode()
            If OneofIndex <> 0 Then hash = hash Xor OneofIndex.GetHashCode()
            If Packed <> False Then hash = hash Xor Packed.GetHashCode()
            hash = hash Xor options_.GetHashCode()
            If JsonName.Length <> 0 Then hash = hash Xor JsonName.GetHashCode()
            If DefaultValue.Length <> 0 Then hash = hash Xor DefaultValue.GetHashCode()
            Return hash
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function ToString() As String
            Return JsonFormatter.ToDiagnosticString(Me)
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
            If Kind <> 0 Then
                output.WriteRawTag(8)
                output.WriteEnum(CInt(Kind))
            End If

            If Cardinality <> 0 Then
                output.WriteRawTag(16)
                output.WriteEnum(CInt(Cardinality))
            End If

            If Number <> 0 Then
                output.WriteRawTag(24)
                output.WriteInt32(Number)
            End If

            If Name.Length <> 0 Then
                output.WriteRawTag(34)
                output.WriteString(Name)
            End If

            If TypeUrl.Length <> 0 Then
                output.WriteRawTag(50)
                output.WriteString(TypeUrl)
            End If

            If OneofIndex <> 0 Then
                output.WriteRawTag(56)
                output.WriteInt32(OneofIndex)
            End If

            If Packed <> False Then
                output.WriteRawTag(64)
                output.WriteBool(Packed)
            End If

            options_.WriteTo(output, _repeated_options_codec)

            If JsonName.Length <> 0 Then
                output.WriteRawTag(82)
                output.WriteString(JsonName)
            End If

            If DefaultValue.Length <> 0 Then
                output.WriteRawTag(90)
                output.WriteString(DefaultValue)
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0

            If Kind <> 0 Then
                size += 1 + CodedOutputStream.ComputeEnumSize(CInt(Kind))
            End If

            If Cardinality <> 0 Then
                size += 1 + CodedOutputStream.ComputeEnumSize(CInt(Cardinality))
            End If

            If Number <> 0 Then
                size += 1 + CodedOutputStream.ComputeInt32Size(Number)
            End If

            If Name.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeStringSize(Name)
            End If

            If TypeUrl.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeStringSize(TypeUrl)
            End If

            If OneofIndex <> 0 Then
                size += 1 + CodedOutputStream.ComputeInt32Size(OneofIndex)
            End If

            If Packed <> False Then
                size += 1 + 1
            End If

            size += options_.CalculateSize(_repeated_options_codec)

            If JsonName.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeStringSize(JsonName)
            End If

            If DefaultValue.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeStringSize(DefaultValue)
            End If

            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As Field) Implements IMessageType(Of Field).MergeFrom
            If other Is Nothing Then
                Return
            End If

            If other.Kind <> 0 Then
                Kind = other.Kind
            End If

            If other.Cardinality <> 0 Then
                Cardinality = other.Cardinality
            End If

            If other.Number <> 0 Then
                Number = other.Number
            End If

            If other.Name.Length <> 0 Then
                Name = other.Name
            End If

            If other.TypeUrl.Length <> 0 Then
                TypeUrl = other.TypeUrl
            End If

            If other.OneofIndex <> 0 Then
                OneofIndex = other.OneofIndex
            End If

            If other.Packed <> False Then
                Packed = other.Packed
            End If

            options_.Add(other.options_)

            If other.JsonName.Length <> 0 Then
                JsonName = other.JsonName
            End If

            If other.DefaultValue.Length <> 0 Then
                DefaultValue = other.DefaultValue
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
            Dim tag As New Value(Of UInteger)

            While ((tag = input.ReadTag())) <> 0

                Select Case tag.Value
                    Case 8
                        kind_ = CType(input.ReadEnum(), FieldTypes.Kind)

                    Case 16
                        cardinality_ = CType(input.ReadEnum(), FieldTypes.Cardinality)

                    Case 24
                        Number = input.ReadInt32()

                    Case 34
                        Name = input.ReadString()

                    Case 50
                        TypeUrl = input.ReadString()

                    Case 56
                        OneofIndex = input.ReadInt32()

                    Case 64
                        Packed = input.ReadBool()

                    Case 74
                        options_.AddEntriesFrom(input, _repeated_options_codec)

                    Case 82
                        JsonName = input.ReadString()

                    Case 90
                        DefaultValue = input.ReadString()

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub
    End Class

End Namespace
