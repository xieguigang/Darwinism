#Region "Microsoft.VisualBasic::0f1ea7574a0e9e6ca915812e2c418ec7, src\message\Google.Protobuf\WellKnownTypes\Type\Enum.vb"

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

    '   Total Lines: 277
    '    Code Lines: 206 (74.37%)
    ' Comment Lines: 23 (8.30%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 48 (17.33%)
    '     File Size: 11.14 KB


    '     Class [Enum]
    ' 
    '         Properties: Descriptor, DescriptorProp, Enumvalue, Name, Options
    '                     Parser, SourceContext, Syntax
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
    '''  Enum type definition.
    ''' </summary>
    Partial Public NotInheritable Class [Enum]
        Implements IMessageType(Of [Enum])

        Private Shared ReadOnly _parser As MessageParserType(Of [Enum]) = New MessageParserType(Of [Enum])(Function() New [Enum]())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of [Enum])
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As pbr.MessageDescriptor
            Get
                Return Global.Google.Protobuf.WellKnownTypes.TypeReflection.Descriptor.MessageTypes(2)
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
        Public Sub New(other As [Enum])
            Me.New()
            name_ = other.name_
            enumvalue_ = other.enumvalue_.Clone()
            options_ = other.options_.Clone()
            SourceContext = If(other.sourceContext_ IsNot Nothing, other.SourceContext.Clone(), Nothing)
            syntax_ = other.syntax_
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As [Enum] Implements IDeepCloneable(Of [Enum]).Clone
            Return New [Enum](Me)
        End Function

        ''' <summary>Field number for the "name" field.</summary>
        Public Const NameFieldNumber As Integer = 1
        Private name_ As String = ""
        ''' <summary>
        '''  Enum type name.
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

        ''' <summary>Field number for the "enumvalue" field.</summary>
        Public Const EnumvalueFieldNumber As Integer = 2
        Private Shared ReadOnly _repeated_enumvalue_codec As FieldCodecType(Of Global.Google.Protobuf.WellKnownTypes.EnumValue) = FieldCodec.ForMessage(18, Global.Google.Protobuf.WellKnownTypes.EnumValue.Parser)
        Private ReadOnly enumvalue_ As pbc.RepeatedField(Of Global.Google.Protobuf.WellKnownTypes.EnumValue) = New pbc.RepeatedField(Of Global.Google.Protobuf.WellKnownTypes.EnumValue)()
        ''' <summary>
        '''  Enum value definitions.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property Enumvalue As pbc.RepeatedField(Of Global.Google.Protobuf.WellKnownTypes.EnumValue)
            Get
                Return enumvalue_
            End Get
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

        ''' <summary>Field number for the "source_context" field.</summary>
        Public Const SourceContextFieldNumber As Integer = 4
        Private sourceContext_ As Global.Google.Protobuf.WellKnownTypes.SourceContext
        ''' <summary>
        '''  The source context.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property SourceContext As Global.Google.Protobuf.WellKnownTypes.SourceContext
            Get
                Return sourceContext_
            End Get
            Set(value As Global.Google.Protobuf.WellKnownTypes.SourceContext)
                sourceContext_ = value
            End Set
        End Property

        ''' <summary>Field number for the "syntax" field.</summary>
        Public Const SyntaxFieldNumber As Integer = 5
        Private syntax_ As Global.Google.Protobuf.WellKnownTypes.Syntax = 0
        ''' <summary>
        '''  The source syntax.
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
            Return Equals(TryCast(other, [Enum]))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As [Enum]) As Boolean Implements IEquatable(Of [Enum]).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If

            If ReferenceEquals(other, Me) Then
                Return True
            End If

            If Not Equals(Name, other.Name) Then Return False
            If Not enumvalue_.Equals(other.enumvalue_) Then Return False
            If Not options_.Equals(other.options_) Then Return False
            If Not Object.Equals(SourceContext, other.SourceContext) Then Return False
            If Syntax <> other.Syntax Then Return False
            Return True
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function GetHashCode() As Integer
            Dim hash = 1
            If Name.Length <> 0 Then hash = hash Xor Name.GetHashCode()
            hash = hash Xor enumvalue_.GetHashCode()
            hash = hash Xor options_.GetHashCode()
            If sourceContext_ IsNot Nothing Then hash = hash Xor SourceContext.GetHashCode()
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

            enumvalue_.WriteTo(output, _repeated_enumvalue_codec)
            options_.WriteTo(output, _repeated_options_codec)

            If sourceContext_ IsNot Nothing Then
                output.WriteRawTag(34)
                output.WriteMessage(SourceContext)
            End If

            If Syntax <> 0 Then
                output.WriteRawTag(40)
                output.WriteEnum(CInt(Syntax))
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0

            If Name.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeStringSize(Name)
            End If

            size += enumvalue_.CalculateSize(_repeated_enumvalue_codec)
            size += options_.CalculateSize(_repeated_options_codec)

            If sourceContext_ IsNot Nothing Then
                size += 1 + CodedOutputStream.ComputeMessageSize(SourceContext)
            End If

            If Syntax <> 0 Then
                size += 1 + CodedOutputStream.ComputeEnumSize(CInt(Syntax))
            End If

            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As [Enum]) Implements IMessageType(Of [Enum]).MergeFrom
            If other Is Nothing Then
                Return
            End If

            If other.Name.Length <> 0 Then
                Name = other.Name
            End If

            enumvalue_.Add(other.enumvalue_)
            options_.Add(other.options_)

            If other.sourceContext_ IsNot Nothing Then
                If sourceContext_ Is Nothing Then
                    sourceContext_ = New Global.Google.Protobuf.WellKnownTypes.SourceContext()
                End If

                SourceContext.MergeFrom(other.SourceContext)
            End If

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
                        enumvalue_.AddEntriesFrom(input, _repeated_enumvalue_codec)

                    Case 26
                        options_.AddEntriesFrom(input, _repeated_options_codec)

                    Case 34

                        If sourceContext_ Is Nothing Then
                            sourceContext_ = New Global.Google.Protobuf.WellKnownTypes.SourceContext()
                        End If

                        input.ReadMessage(sourceContext_)

                    Case 40
                        syntax_ = CType(input.ReadEnum(), Global.Google.Protobuf.WellKnownTypes.Syntax)

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub
    End Class

End Namespace
