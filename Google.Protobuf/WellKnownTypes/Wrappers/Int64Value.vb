Namespace Google.Protobuf.WellKnownTypes

    ''' <summary>
    '''  Wrapper message for `int64`.
    '''
    '''  The JSON representation for `Int64Value` is JSON string.
    ''' </summary>
    Partial Public NotInheritable Class Int64Value
        Implements IMessageType(Of Int64Value)

        Private Shared ReadOnly _parser As MessageParserType(Of Int64Value) = New MessageParserType(Of Int64Value)(Function() New Int64Value())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of Int64Value)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As pbr.MessageDescriptor
            Get
                Return Global.Google.Protobuf.WellKnownTypes.WrappersReflection.Descriptor.MessageTypes(2)
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
        Public Sub New(other As Int64Value)
            Me.New()
            value_ = other.value_
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As Int64Value Implements IDeepCloneable(Of Int64Value).Clone
            Return New Int64Value(Me)
        End Function

        ''' <summary>Field number for the "value" field.</summary>
        Public Const ValueFieldNumber As Integer = 1
        Private value_ As Long
        ''' <summary>
        '''  The int64 value.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Value As Long
            Get
                Return value_
            End Get
            Set(value As Long)
                value_ = value
            End Set
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, Int64Value))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As Int64Value) As Boolean Implements IEquatable(Of Int64Value).Equals
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
            If Value <> 0L Then hash = hash Xor Value.GetHashCode()
            Return hash
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function ToString() As String
            Return JsonFormatter.ToDiagnosticString(Me)
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
            If Value <> 0L Then
                output.WriteRawTag(8)
                output.WriteInt64(Value)
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0

            If Value <> 0L Then
                size += 1 + CodedOutputStream.ComputeInt64Size(Value)
            End If

            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As Int64Value) Implements IMessageType(Of Int64Value).MergeFrom
            If other Is Nothing Then
                Return
            End If

            If other.Value <> 0L Then
                Value = other.Value
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
            Dim tag As New Value(Of UInteger)

            While ((tag = input.ReadTag())) <> 0

                Select Case tag.Value
                    Case 8
                        Value = input.ReadInt64()

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub
    End Class

End Namespace