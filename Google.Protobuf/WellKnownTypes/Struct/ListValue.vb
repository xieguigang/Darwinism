Imports Microsoft.VisualBasic.Language
Imports pbc = Google.Protobuf.Collections
Imports pbr = Google.Protobuf.Reflection

Namespace Google.Protobuf.WellKnownTypes

    ''' <summary>
    '''  `ListValue` is a wrapper around a repeated field of values.
    '''
    '''  The JSON representation for `ListValue` is JSON array.
    ''' </summary>
    Partial Public NotInheritable Class ListValue
        Implements IMessageType(Of ListValue)

        Private Shared ReadOnly _parser As MessageParserType(Of ListValue) = New MessageParserType(Of ListValue)(Function() New ListValue())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of ListValue)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As pbr.MessageDescriptor
            Get
                Return Global.Google.Protobuf.WellKnownTypes.StructReflection.Descriptor.MessageTypes(2)
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
        Public Sub New(other As ListValue)
            Me.New()
            values_ = other.values_.Clone()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As ListValue Implements IDeepCloneable(Of ListValue).Clone
            Return New ListValue(Me)
        End Function

        ''' <summary>Field number for the "values" field.</summary>
        Public Const ValuesFieldNumber As Integer = 1
        Private Shared ReadOnly _repeated_values_codec As FieldCodecType(Of Global.Google.Protobuf.WellKnownTypes.Value) = FieldCodec.ForMessage(10, Global.Google.Protobuf.WellKnownTypes.Value.Parser)
        Friend values_ As pbc.RepeatedField(Of Global.Google.Protobuf.WellKnownTypes.Value) = New pbc.RepeatedField(Of Global.Google.Protobuf.WellKnownTypes.Value)()
        ''' <summary>
        '''  Repeated field of dynamically typed values.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property Values As pbc.RepeatedField(Of Global.Google.Protobuf.WellKnownTypes.Value)
            Get
                Return values_
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, ListValue))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As ListValue) As Boolean Implements IEquatable(Of ListValue).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If

            If ReferenceEquals(other, Me) Then
                Return True
            End If

            If Not values_.Equals(other.values_) Then Return False
            Return True
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function GetHashCode() As Integer
            Dim hash = 1
            hash = hash Xor values_.GetHashCode()
            Return hash
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function ToString() As String
            Return JsonFormatter.ToDiagnosticString(Me)
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
            values_.WriteTo(output, _repeated_values_codec)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0
            size += values_.CalculateSize(_repeated_values_codec)
            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As ListValue) Implements IMessageType(Of ListValue).MergeFrom
            If other Is Nothing Then
                Return
            End If

            values_.Add(other.values_)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
            Dim tag As New Value(Of UInteger)

            While ((tag = input.ReadTag())) <> 0

                Select Case tag.Value
                    Case 10
                        values_.AddEntriesFrom(input, _repeated_values_codec)

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub
    End Class

End Namespace