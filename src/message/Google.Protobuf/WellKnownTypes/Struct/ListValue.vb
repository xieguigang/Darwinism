#Region "Microsoft.VisualBasic::4f05150d3088e3174ba217091a4470e6, src\message\Google.Protobuf\WellKnownTypes\Struct\ListValue.vb"

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

    '   Total Lines: 140
    '    Code Lines: 105
    ' Comment Lines: 9
    '   Blank Lines: 26
    '     File Size: 5.50 KB


    '     Class ListValue
    ' 
    '         Properties: Descriptor, DescriptorProp, Parser, Values
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
