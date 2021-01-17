Imports Microsoft.VisualBasic.Language
Imports pbc = Google.Protobuf.Collections
Imports pbr = Google.Protobuf.Reflection

Namespace Google.Protobuf.WellKnownTypes

    ''' <summary>
    '''  Method represents a method of an api.
    ''' </summary>
    Partial Public NotInheritable Class Method
        Implements IMessageType(Of Method)

        Private Shared ReadOnly _parser As MessageParserType(Of Method) = New MessageParserType(Of Method)(Function() New Method())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of Method)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As pbr.MessageDescriptor
            Get
                Return Global.Google.Protobuf.WellKnownTypes.ApiReflection.Descriptor.MessageTypes(1)
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
        Public Sub New(other As Method)
            Me.New()
            name_ = other.name_
            requestTypeUrl_ = other.requestTypeUrl_
            requestStreaming_ = other.requestStreaming_
            responseTypeUrl_ = other.responseTypeUrl_
            responseStreaming_ = other.responseStreaming_
            options_ = other.options_.Clone()
            syntax_ = other.syntax_
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As Method Implements IDeepCloneable(Of Method).Clone
            Return New Method(Me)
        End Function

        ''' <summary>Field number for the "name" field.</summary>
        Public Const NameFieldNumber As Integer = 1
        Private name_ As String = ""
        ''' <summary>
        '''  The simple name of this method.
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

        ''' <summary>Field number for the "request_type_url" field.</summary>
        Public Const RequestTypeUrlFieldNumber As Integer = 2
        Private requestTypeUrl_ As String = ""
        ''' <summary>
        '''  A URL of the input message type.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property RequestTypeUrl As String
            Get
                Return requestTypeUrl_
            End Get
            Set(value As String)
                requestTypeUrl_ = CheckNotNull(value, "value")
            End Set
        End Property

        ''' <summary>Field number for the "request_streaming" field.</summary>
        Public Const RequestStreamingFieldNumber As Integer = 3
        Private requestStreaming_ As Boolean
        ''' <summary>
        '''  If true, the request is streamed.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property RequestStreaming As Boolean
            Get
                Return requestStreaming_
            End Get
            Set(value As Boolean)
                requestStreaming_ = value
            End Set
        End Property

        ''' <summary>Field number for the "response_type_url" field.</summary>
        Public Const ResponseTypeUrlFieldNumber As Integer = 4
        Private responseTypeUrl_ As String = ""
        ''' <summary>
        '''  The URL of the output message type.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property ResponseTypeUrl As String
            Get
                Return responseTypeUrl_
            End Get
            Set(value As String)
                responseTypeUrl_ = CheckNotNull(value, "value")
            End Set
        End Property

        ''' <summary>Field number for the "response_streaming" field.</summary>
        Public Const ResponseStreamingFieldNumber As Integer = 5
        Private responseStreaming_ As Boolean
        ''' <summary>
        '''  If true, the response is streamed.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property ResponseStreaming As Boolean
            Get
                Return responseStreaming_
            End Get
            Set(value As Boolean)
                responseStreaming_ = value
            End Set
        End Property

        ''' <summary>Field number for the "options" field.</summary>
        Public Const OptionsFieldNumber As Integer = 6
        Private Shared ReadOnly _repeated_options_codec As FieldCodecType(Of Global.Google.Protobuf.WellKnownTypes.Option) = FieldCodec.ForMessage(50, Global.Google.Protobuf.WellKnownTypes.Option.Parser)
        Private ReadOnly options_ As pbc.RepeatedField(Of Global.Google.Protobuf.WellKnownTypes.Option) = New pbc.RepeatedField(Of Global.Google.Protobuf.WellKnownTypes.Option)()
        ''' <summary>
        '''  Any metadata attached to the method.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property Options As pbc.RepeatedField(Of Global.Google.Protobuf.WellKnownTypes.Option)
            Get
                Return options_
            End Get
        End Property

        ''' <summary>Field number for the "syntax" field.</summary>
        Public Const SyntaxFieldNumber As Integer = 7
        Private syntax_ As Global.Google.Protobuf.WellKnownTypes.Syntax = 0
        ''' <summary>
        '''  The source syntax of this method.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Syntax As Global.Google.Protobuf.WellKnownTypes.Syntax
            Get
                Return syntax_
            End Get
            Set(value As Global.Google.Protobuf.WellKnownTypes.Syntax)
                syntax_ = value
            End Set
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, Method))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As Method) As Boolean Implements IEquatable(Of Method).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If

            If ReferenceEquals(other, Me) Then
                Return True
            End If

            If Not Equals(Name, other.Name) Then Return False
            If Not Equals(RequestTypeUrl, other.RequestTypeUrl) Then Return False
            If RequestStreaming <> other.RequestStreaming Then Return False
            If Not Equals(ResponseTypeUrl, other.ResponseTypeUrl) Then Return False
            If ResponseStreaming <> other.ResponseStreaming Then Return False
            If Not options_.Equals(other.options_) Then Return False
            If Syntax <> other.Syntax Then Return False
            Return True
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function GetHashCode() As Integer
            Dim hash = 1
            If Name.Length <> 0 Then hash = hash Xor Name.GetHashCode()
            If RequestTypeUrl.Length <> 0 Then hash = hash Xor RequestTypeUrl.GetHashCode()
            If RequestStreaming <> False Then hash = hash Xor RequestStreaming.GetHashCode()
            If ResponseTypeUrl.Length <> 0 Then hash = hash Xor ResponseTypeUrl.GetHashCode()
            If ResponseStreaming <> False Then hash = hash Xor ResponseStreaming.GetHashCode()
            hash = hash Xor options_.GetHashCode()
            If Syntax <> 0 Then hash = hash Xor Syntax.GetHashCode()
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

            If RequestTypeUrl.Length <> 0 Then
                output.WriteRawTag(18)
                output.WriteString(RequestTypeUrl)
            End If

            If RequestStreaming <> False Then
                output.WriteRawTag(24)
                output.WriteBool(RequestStreaming)
            End If

            If ResponseTypeUrl.Length <> 0 Then
                output.WriteRawTag(34)
                output.WriteString(ResponseTypeUrl)
            End If

            If ResponseStreaming <> False Then
                output.WriteRawTag(40)
                output.WriteBool(ResponseStreaming)
            End If

            options_.WriteTo(output, _repeated_options_codec)

            If Syntax <> 0 Then
                output.WriteRawTag(56)
                output.WriteEnum(CInt(Syntax))
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0

            If Name.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeStringSize(Name)
            End If

            If RequestTypeUrl.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeStringSize(RequestTypeUrl)
            End If

            If RequestStreaming <> False Then
                size += 1 + 1
            End If

            If ResponseTypeUrl.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeStringSize(ResponseTypeUrl)
            End If

            If ResponseStreaming <> False Then
                size += 1 + 1
            End If

            size += options_.CalculateSize(_repeated_options_codec)

            If Syntax <> 0 Then
                size += 1 + CodedOutputStream.ComputeEnumSize(CInt(Syntax))
            End If

            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As Method) Implements IMessageType(Of Method).MergeFrom
            If other Is Nothing Then
                Return
            End If

            If other.Name.Length <> 0 Then
                Name = other.Name
            End If

            If other.RequestTypeUrl.Length <> 0 Then
                RequestTypeUrl = other.RequestTypeUrl
            End If

            If other.RequestStreaming <> False Then
                RequestStreaming = other.RequestStreaming
            End If

            If other.ResponseTypeUrl.Length <> 0 Then
                ResponseTypeUrl = other.ResponseTypeUrl
            End If

            If other.ResponseStreaming <> False Then
                ResponseStreaming = other.ResponseStreaming
            End If

            options_.Add(other.options_)

            If other.Syntax <> 0 Then
                Syntax = other.Syntax
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
            Dim tag As New Value(Of UInteger)

            While ((tag = input.ReadTag())) <> 0

                Select Case tag.Value
                    Case 10
                        Name = input.ReadString()

                    Case 18
                        RequestTypeUrl = input.ReadString()

                    Case 24
                        RequestStreaming = input.ReadBool()

                    Case 34
                        ResponseTypeUrl = input.ReadString()

                    Case 40
                        ResponseStreaming = input.ReadBool()

                    Case 50
                        options_.AddEntriesFrom(input, _repeated_options_codec)

                    Case 56
                        syntax_ = CType(input.ReadEnum(), Global.Google.Protobuf.WellKnownTypes.Syntax)

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub
    End Class

End Namespace