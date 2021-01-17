Namespace Google.Protobuf.WellKnownTypes

    ''' <summary>
    '''  Wrapper message for `float`.
    '''
    '''  The JSON representation for `FloatValue` is JSON number.
    ''' </summary>
    Partial Public NotInheritable Class FloatValue
        Implements IMessageType(Of FloatValue)

        Private Shared ReadOnly _parser As MessageParserType(Of FloatValue) = New MessageParserType(Of FloatValue)(Function() New FloatValue())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of FloatValue)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As pbr.MessageDescriptor
            Get
                Return Global.Google.Protobuf.WellKnownTypes.WrappersReflection.Descriptor.MessageTypes(1)
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
        Public Sub New(other As FloatValue)
            Me.New()
            value_ = other.value_
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As FloatValue Implements IDeepCloneable(Of FloatValue).Clone
            Return New FloatValue(Me)
        End Function

        ''' <summary>Field number for the "value" field.</summary>
        Public Const ValueFieldNumber As Integer = 1
        Private value_ As Single
        ''' <summary>
        '''  The float value.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Value As Single
            Get
                Return value_
            End Get
            Set(value As Single)
                value_ = value
            End Set
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, FloatValue))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As FloatValue) As Boolean Implements IEquatable(Of FloatValue).Equals
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
            If Value <> 0F Then hash = hash Xor Value.GetHashCode()
            Return hash
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function ToString() As String
            Return JsonFormatter.ToDiagnosticString(Me)
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
            If Value <> 0F Then
                output.WriteRawTag(13)
                output.WriteFloat(Value)
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0

            If Value <> 0F Then
                size += 1 + 4
            End If

            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As FloatValue) Implements IMessageType(Of FloatValue).MergeFrom
            If other Is Nothing Then
                Return
            End If

            If other.Value <> 0F Then
                Value = other.Value
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
            Dim tag As New Value(Of UInteger)

            While ((tag = input.ReadTag())) <> 0

                Select Case tag.Value
                    Case 13
                        Value = input.ReadFloat()

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub
    End Class

End Namespace