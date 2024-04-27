#Region "Microsoft.VisualBasic::a52f301e7081bac6caec080fe641752c, G:/GCModeller/src/runtime/Darwinism/src/message/Google.Protobuf//WellKnownTypes/Struct/Value.vb"

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

    '   Total Lines: 363
    '    Code Lines: 274
    ' Comment Lines: 32
    '   Blank Lines: 57
    '     File Size: 14.69 KB


    '     Class Value
    ' 
    '         Properties: BoolValue, Descriptor, DescriptorProp, KindCase, ListValue
    '                     NullValue, NumberValue, Parser, StringValue, StructValue
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '         Function: CalculateSize, Clone, (+2 Overloads) Equals, GetHashCode, ToString
    ' 
    '         Sub: ClearKind, (+2 Overloads) MergeFrom, OnConstruction, WriteTo
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Language
Imports pbc = Google.Protobuf.Collections
Imports pbr = Google.Protobuf.Reflection

Namespace Google.Protobuf.WellKnownTypes

    ''' <summary>
    '''  `Value` represents a dynamically typed value which can be either
    '''  null, a number, a string, a boolean, a recursive struct value, or a
    '''  list of values. A producer of value is expected to set one of that
    '''  variants, absence of any variant indicates an error.
    '''
    '''  The JSON representation for `Value` is JSON value.
    ''' </summary>
    Partial Public NotInheritable Class Value
        Implements IMessageType(Of Value)

        Private Shared ReadOnly _parser As MessageParserType(Of Value) = New MessageParserType(Of Value)(Function() New Value())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of Value)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As pbr.MessageDescriptor
            Get
                Return Global.Google.Protobuf.WellKnownTypes.StructReflection.Descriptor.MessageTypes(1)
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
        Public Sub New(other As Value)
            Me.New()

            Select Case other.KindCase
                Case KindOneofCase.NullValue
                    NullValue = other.NullValue
                Case KindOneofCase.NumberValue
                    NumberValue = other.NumberValue
                Case KindOneofCase.StringValue
                    StringValue = other.StringValue
                Case KindOneofCase.BoolValue
                    BoolValue = other.BoolValue
                Case KindOneofCase.StructValue
                    StructValue = other.StructValue.Clone()
                Case KindOneofCase.ListValue
                    ListValue = other.ListValue.Clone()
            End Select
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As Value Implements IDeepCloneable(Of Value).Clone
            Return New Value(Me)
        End Function

        ''' <summary>Field number for the "null_value" field.</summary>
        Public Const NullValueFieldNumber As Integer = 1
        ''' <summary>
        '''  Represents a null value.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property NullValue As Global.Google.Protobuf.WellKnownTypes.NullValue
            Get
                Return If(kindCase_ = KindOneofCase.NullValue, CType(kind_, Global.Google.Protobuf.WellKnownTypes.NullValue), 0)
            End Get
            Set(value As Global.Google.Protobuf.WellKnownTypes.NullValue)
                kind_ = value
                kindCase_ = KindOneofCase.NullValue
            End Set
        End Property

        ''' <summary>Field number for the "number_value" field.</summary>
        Public Const NumberValueFieldNumber As Integer = 2
        ''' <summary>
        '''  Represents a double value.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property NumberValue As Double
            Get
                Return If(kindCase_ = KindOneofCase.NumberValue, CDbl(kind_), 0R)
            End Get
            Set(value As Double)
                kind_ = value
                kindCase_ = KindOneofCase.NumberValue
            End Set
        End Property

        ''' <summary>Field number for the "string_value" field.</summary>
        Public Const StringValueFieldNumber As Integer = 3
        ''' <summary>
        '''  Represents a string value.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property StringValue As String
            Get
                Return If(kindCase_ = KindOneofCase.StringValue, CStr(kind_), "")
            End Get
            Set(value As String)
                kind_ = CheckNotNull(value, "value")
                kindCase_ = KindOneofCase.StringValue
            End Set
        End Property

        ''' <summary>Field number for the "bool_value" field.</summary>
        Public Const BoolValueFieldNumber As Integer = 4
        ''' <summary>
        '''  Represents a boolean value.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property BoolValue As Boolean
            Get
                Return If(kindCase_ = KindOneofCase.BoolValue, CBool(kind_), False)
            End Get
            Set(value As Boolean)
                kind_ = value
                kindCase_ = KindOneofCase.BoolValue
            End Set
        End Property

        ''' <summary>Field number for the "struct_value" field.</summary>
        Public Const StructValueFieldNumber As Integer = 5
        ''' <summary>
        '''  Represents a structured value.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property StructValue As Global.Google.Protobuf.WellKnownTypes.Struct
            Get
                Return If(kindCase_ = KindOneofCase.StructValue, CType(kind_, Global.Google.Protobuf.WellKnownTypes.Struct), Nothing)
            End Get
            Set(value As Global.Google.Protobuf.WellKnownTypes.Struct)
                kind_ = value
                kindCase_ = If(value Is Nothing, KindOneofCase.None, KindOneofCase.StructValue)
            End Set
        End Property

        ''' <summary>Field number for the "list_value" field.</summary>
        Public Const ListValueFieldNumber As Integer = 6
        ''' <summary>
        '''  Represents a repeated `Value`.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property ListValue As Global.Google.Protobuf.WellKnownTypes.ListValue
            Get
                Return If(kindCase_ = KindOneofCase.ListValue, CType(kind_, Global.Google.Protobuf.WellKnownTypes.ListValue), Nothing)
            End Get
            Set(value As Global.Google.Protobuf.WellKnownTypes.ListValue)
                kind_ = value
                kindCase_ = If(value Is Nothing, KindOneofCase.None, KindOneofCase.ListValue)
            End Set
        End Property

        Private kind_ As Object

        Private kindCase_ As KindOneofCase = KindOneofCase.None

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property KindCase As KindOneofCase
            Get
                Return kindCase_
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub ClearKind()
            kindCase_ = KindOneofCase.None
            kind_ = Nothing
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, Value))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As Value) As Boolean Implements IEquatable(Of Value).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If

            If ReferenceEquals(other, Me) Then
                Return True
            End If

            If NullValue <> other.NullValue Then Return False
            If NumberValue <> other.NumberValue Then Return False
            If Not Equals(StringValue, other.StringValue) Then Return False
            If BoolValue <> other.BoolValue Then Return False
            If Not Object.Equals(StructValue, other.StructValue) Then Return False
            If Not Object.Equals(ListValue, other.ListValue) Then Return False
            If KindCase <> other.KindCase Then Return False
            Return True
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function GetHashCode() As Integer
            Dim hash = 1
            If kindCase_ = KindOneofCase.NullValue Then hash = hash Xor NullValue.GetHashCode()
            If kindCase_ = KindOneofCase.NumberValue Then hash = hash Xor NumberValue.GetHashCode()
            If kindCase_ = KindOneofCase.StringValue Then hash = hash Xor StringValue.GetHashCode()
            If kindCase_ = KindOneofCase.BoolValue Then hash = hash Xor BoolValue.GetHashCode()
            If kindCase_ = KindOneofCase.StructValue Then hash = hash Xor StructValue.GetHashCode()
            If kindCase_ = KindOneofCase.ListValue Then hash = hash Xor ListValue.GetHashCode()
            hash = hash Xor kindCase_
            Return hash
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function ToString() As String
            Return JsonFormatter.ToDiagnosticString(Me)
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
            If kindCase_ = KindOneofCase.NullValue Then
                output.WriteRawTag(8)
                output.WriteEnum(CInt(NullValue))
            End If

            If kindCase_ = KindOneofCase.NumberValue Then
                output.WriteRawTag(17)
                output.WriteDouble(NumberValue)
            End If

            If kindCase_ = KindOneofCase.StringValue Then
                output.WriteRawTag(26)
                output.WriteString(StringValue)
            End If

            If kindCase_ = KindOneofCase.BoolValue Then
                output.WriteRawTag(32)
                output.WriteBool(BoolValue)
            End If

            If kindCase_ = KindOneofCase.StructValue Then
                output.WriteRawTag(42)
                output.WriteMessage(StructValue)
            End If

            If kindCase_ = KindOneofCase.ListValue Then
                output.WriteRawTag(50)
                output.WriteMessage(ListValue)
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0

            If kindCase_ = KindOneofCase.NullValue Then
                size += 1 + CodedOutputStream.ComputeEnumSize(CInt(NullValue))
            End If

            If kindCase_ = KindOneofCase.NumberValue Then
                size += 1 + 8
            End If

            If kindCase_ = KindOneofCase.StringValue Then
                size += 1 + CodedOutputStream.ComputeStringSize(StringValue)
            End If

            If kindCase_ = KindOneofCase.BoolValue Then
                size += 1 + 1
            End If

            If kindCase_ = KindOneofCase.StructValue Then
                size += 1 + CodedOutputStream.ComputeMessageSize(StructValue)
            End If

            If kindCase_ = KindOneofCase.ListValue Then
                size += 1 + CodedOutputStream.ComputeMessageSize(ListValue)
            End If

            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As Value) Implements IMessageType(Of Value).MergeFrom
            If other Is Nothing Then
                Return
            End If

            Select Case other.KindCase
                Case KindOneofCase.NullValue
                    NullValue = other.NullValue
                Case KindOneofCase.NumberValue
                    NumberValue = other.NumberValue
                Case KindOneofCase.StringValue
                    StringValue = other.StringValue
                Case KindOneofCase.BoolValue
                    BoolValue = other.BoolValue
                Case KindOneofCase.StructValue
                    StructValue = other.StructValue
                Case KindOneofCase.ListValue
                    ListValue = other.ListValue
            End Select
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
            Dim tag As New Value(Of UInteger)

            While ((tag = input.ReadTag())) <> 0

                Select Case tag.Value
                    Case 8
                        kind_ = input.ReadEnum()
                        kindCase_ = KindOneofCase.NullValue

                    Case 17
                        NumberValue = input.ReadDouble()

                    Case 26
                        StringValue = input.ReadString()

                    Case 32
                        BoolValue = input.ReadBool()

                    Case 42
                        Dim subBuilder As Global.Google.Protobuf.WellKnownTypes.Struct = New Global.Google.Protobuf.WellKnownTypes.Struct()

                        If kindCase_ = KindOneofCase.StructValue Then
                            subBuilder.MergeFrom(StructValue)
                        End If

                        input.ReadMessage(subBuilder)
                        StructValue = subBuilder

                    Case 50
                        Dim subBuilder As Global.Google.Protobuf.WellKnownTypes.ListValue = New Global.Google.Protobuf.WellKnownTypes.ListValue()

                        If kindCase_ = KindOneofCase.ListValue Then
                            subBuilder.MergeFrom(ListValue)
                        End If

                        input.ReadMessage(subBuilder)
                        ListValue = subBuilder

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub
    End Class

End Namespace
