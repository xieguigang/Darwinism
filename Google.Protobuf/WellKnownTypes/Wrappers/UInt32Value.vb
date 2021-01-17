Namespace Google.Protobuf.WellKnownTypes

    ''' <summary>
    '''  Wrapper message for `uint32`.
    '''
    '''  The JSON representation for `UInt32Value` is JSON number.
    ''' </summary>
    Partial Public NotInheritable Class UInt32Value
        Implements IMessageType(Of UInt32Value)

        Private Shared ReadOnly _parser As MessageParserType(Of UInt32Value) = New MessageParserType(Of UInt32Value)(Function() New UInt32Value())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of UInt32Value)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As pbr.MessageDescriptor
            Get
                Return Global.Google.Protobuf.WellKnownTypes.WrappersReflection.Descriptor.MessageTypes(5)
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
        Public Sub New(other As UInt32Value)
            Me.New()
            value_ = other.value_
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As UInt32Value Implements IDeepCloneable(Of UInt32Value).Clone
            Return New UInt32Value(Me)
        End Function

        ''' <summary>Field number for the "value" field.</summary>
        Public Const ValueFieldNumber As Integer = 1
        Private value_ As UInteger
        ''' <summary>
        '''  The uint32 value.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Value As UInteger
            Get
                Return value_
            End Get
            Set(value As UInteger)
                value_ = value
            End Set
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, UInt32Value))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As UInt32Value) As Boolean Implements IEquatable(Of UInt32Value).Equals
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
            If Value <> 0 Then hash = hash Xor Value.GetHashCode()
            Return hash
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function ToString() As String
            Return JsonFormatter.ToDiagnosticString(Me)
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
            If Value <> 0 Then
                output.WriteRawTag(8)
                output.WriteUInt32(Value)
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0

            If Value <> 0 Then
                size += 1 + CodedOutputStream.ComputeUInt32Size(Value)
            End If

            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As UInt32Value) Implements IMessageType(Of UInt32Value).MergeFrom
            If other Is Nothing Then
                Return
            End If

            If other.Value <> 0 Then
                Value = other.Value
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
            Dim tag As New Value(Of UInteger)

            While ((tag = input.ReadTag())) <> 0

                Select Case tag.Value
                    Case 8
                        Value = input.ReadUInt32()

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub
    End Class

End Namespace