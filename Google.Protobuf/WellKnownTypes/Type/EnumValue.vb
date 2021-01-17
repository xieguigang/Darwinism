Imports Microsoft.VisualBasic.Language
Imports pbc = Google.Protobuf.Collections
Imports pbr = Google.Protobuf.Reflection

Namespace Google.Protobuf.WellKnownTypes

    ''' <summary>
    '''  Enum value definition.
    ''' </summary>
    Partial Public NotInheritable Class EnumValue
        Implements IMessageType(Of EnumValue)

        Private Shared ReadOnly _parser As MessageParserType(Of EnumValue) = New MessageParserType(Of EnumValue)(Function() New EnumValue())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of EnumValue)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As pbr.MessageDescriptor
            Get
                Return Global.Google.Protobuf.WellKnownTypes.TypeReflection.Descriptor.MessageTypes(3)
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
        Public Sub New(other As EnumValue)
            Me.New()
            name_ = other.name_
            number_ = other.number_
            options_ = other.options_.Clone()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As EnumValue Implements IDeepCloneable(Of EnumValue).Clone
            Return New EnumValue(Me)
        End Function

        ''' <summary>Field number for the "name" field.</summary>
        Public Const NameFieldNumber As Integer = 1
        Private name_ As String = ""
        ''' <summary>
        '''  Enum value name.
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

        ''' <summary>Field number for the "number" field.</summary>
        Public Const NumberFieldNumber As Integer = 2
        Private number_ As Integer
        ''' <summary>
        '''  Enum value number.
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

        ''' <summary>Field number for the "options" field.</summary>
        Public Const OptionsFieldNumber As Integer = 3
        Private Shared ReadOnly _repeated_options_codec As FieldCodecType(Of Global.Google.Protobuf.WellKnownTypes.Option) = FieldCodec.ForMessage(26, Global.Google.Protobuf.WellKnownTypes.Option.Parser)
        Private ReadOnly options_ As pbc.RepeatedField(Of Global.Google.Protobuf.WellKnownTypes.Option) = New pbc.RepeatedField(Of Global.Google.Protobuf.WellKnownTypes.Option)()
        ''' <summary>
        '''  Protocol buffer options.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property Options As pbc.RepeatedField(Of Global.Google.Protobuf.WellKnownTypes.Option)
            Get
                Return options_
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, EnumValue))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As EnumValue) As Boolean Implements IEquatable(Of EnumValue).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If

            If ReferenceEquals(other, Me) Then
                Return True
            End If

            If Not Equals(Name, other.Name) Then Return False
            If Number <> other.Number Then Return False
            If Not options_.Equals(other.options_) Then Return False
            Return True
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function GetHashCode() As Integer
            Dim hash = 1
            If Name.Length <> 0 Then hash = hash Xor Name.GetHashCode()
            If Number <> 0 Then hash = hash Xor Number.GetHashCode()
            hash = hash Xor options_.GetHashCode()
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

            options_.WriteTo(output, _repeated_options_codec)
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

            size += options_.CalculateSize(_repeated_options_codec)
            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As EnumValue) Implements IMessageType(Of EnumValue).MergeFrom
            If other Is Nothing Then
                Return
            End If

            If other.Name.Length <> 0 Then
                Name = other.Name
            End If

            If other.Number <> 0 Then
                Number = other.Number
            End If

            options_.Add(other.options_)
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
                        options_.AddEntriesFrom(input, _repeated_options_codec)

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub
    End Class
End Namespace