Namespace Google.Protobuf.WellKnownTypes

    ''' <summary>
    '''  Wrapper message for `bool`.
    '''
    '''  The JSON representation for `BoolValue` is JSON `true` and `false`.
    ''' </summary>
    Partial Public NotInheritable Class BoolValue
        Implements IMessageType(Of BoolValue)

        Private Shared ReadOnly _parser As MessageParserType(Of BoolValue) = New MessageParserType(Of BoolValue)(Function() New BoolValue())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of BoolValue)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As pbr.MessageDescriptor
            Get
                Return Global.Google.Protobuf.WellKnownTypes.WrappersReflection.Descriptor.MessageTypes(6)
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
        Public Sub New(other As BoolValue)
            Me.New()
            value_ = other.value_
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As BoolValue Implements IDeepCloneable(Of BoolValue).Clone
            Return New BoolValue(Me)
        End Function

        ''' <summary>Field number for the "value" field.</summary>
        Public Const ValueFieldNumber As Integer = 1
        Private value_ As Boolean
        ''' <summary>
        '''  The bool value.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Value As Boolean
            Get
                Return value_
            End Get
            Set(value As Boolean)
                value_ = value
            End Set
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, BoolValue))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As BoolValue) As Boolean Implements IEquatable(Of BoolValue).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If

            If ReferenceEquals(other, Me) Then
                Return True
            End If

            If Value <> other.Value Then Return False
            Return True
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function GetHashCode() As Integer
            Dim hash = 1
            If Value <> False Then hash = hash Xor Value.GetHashCode()
            Return hash
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function ToString() As String
            Return JsonFormatter.ToDiagnosticString(Me)
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
            If Value <> False Then
                output.WriteRawTag(8)
                output.WriteBool(Value)
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0

            If Value <> False Then
                size += 1 + 1
            End If

            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As BoolValue) Implements IMessageType(Of BoolValue).MergeFrom
            If other Is Nothing Then
                Return
            End If

            If other.Value <> False Then
                Value = other.Value
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
            Dim tag As New Value(Of UInteger)

            While ((tag = input.ReadTag())) <> 0

                Select Case tag.Value
                    Case 8
                        Value = input.ReadBool()

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub
    End Class

End Namespace