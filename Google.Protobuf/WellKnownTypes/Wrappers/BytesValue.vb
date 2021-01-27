#Region "Microsoft.VisualBasic::4baca87440aed1dee11388d92f86e62c, Google.Protobuf\WellKnownTypes\Wrappers\BytesValue.vb"

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

    '     Class BytesValue
    ' 
    '         Properties: Descriptor, DescriptorProp, Parser, Value
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
Imports pbr = Google.Protobuf.Reflection

Namespace Google.Protobuf.WellKnownTypes
#Region "Messages"

    ''' <summary>
    '''  Wrapper message for `bytes`.
    '''
    '''  The JSON representation for `BytesValue` is JSON string.
    ''' </summary>
    Partial Public NotInheritable Class BytesValue
        Implements IMessageType(Of BytesValue)

        Private Shared ReadOnly _parser As MessageParserType(Of BytesValue) = New MessageParserType(Of BytesValue)(Function() New BytesValue())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of BytesValue)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As pbr.MessageDescriptor
            Get
                Return Global.Google.Protobuf.WellKnownTypes.WrappersReflection.Descriptor.MessageTypes(8)
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
        Public Sub New(other As BytesValue)
            Me.New()
            value_ = other.value_
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As BytesValue Implements IDeepCloneable(Of BytesValue).Clone
            Return New BytesValue(Me)
        End Function

        ''' <summary>Field number for the "value" field.</summary>
        Public Const ValueFieldNumber As Integer = 1
        Private value_ As ByteString = ByteString.Empty
        ''' <summary>
        '''  The bytes value.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Property Value As ByteString
            Get
                Return value_
            End Get
            Set(value As ByteString)
                value_ = CheckNotNull(value, "value")
            End Set
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, BytesValue))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As BytesValue) As Boolean Implements IEquatable(Of BytesValue).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If

            If ReferenceEquals(other, Me) Then
                Return True
            End If

            If Value IsNot other.Value Then Return False
            Return True
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function GetHashCode() As Integer
            Dim hash = 1
            If Value.Length <> 0 Then hash = hash Xor Value.GetHashCode()
            Return hash
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function ToString() As String
            Return JsonFormatter.ToDiagnosticString(Me)
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
            If Value.Length <> 0 Then
                output.WriteRawTag(10)
                output.WriteBytes(Value)
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0

            If Value.Length <> 0 Then
                size += 1 + CodedOutputStream.ComputeBytesSize(Value)
            End If

            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As BytesValue) Implements IMessageType(Of BytesValue).MergeFrom
            If other Is Nothing Then
                Return
            End If

            If other.Value.Length <> 0 Then
                Value = other.Value
            End If
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
            Dim tag As New Value(Of UInteger)

            While ((tag = input.ReadTag())) <> 0

                Select Case tag.Value
                    Case 10
                        Value = input.ReadBytes()

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub
    End Class

#End Region

End Namespace
