﻿#Region "Microsoft.VisualBasic::0b92678551b9497b309663816051ab7a, Google.Protobuf\WellKnownTypes\Struct\Struct.vb"

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

    '     Class Struct
    ' 
    '         Properties: Descriptor, DescriptorProp, Fields, Parser
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

' Generated by the protocol buffer compiler.  DO NOT EDIT!
' source: google/protobuf/struct.proto
#Region "Designer generated code"

Imports Microsoft.VisualBasic.Language
Imports pbc = Google.Protobuf.Collections
Imports pbr = Google.Protobuf.Reflection

Namespace Google.Protobuf.WellKnownTypes

#Region "Messages"
    ''' <summary>
    '''  `Struct` represents a structured data value, consisting of fields
    '''  which map to dynamically typed values. In some languages, `Struct`
    '''  might be supported by a native representation. For example, in
    '''  scripting languages like JS a struct is represented as an
    '''  object. The details of that representation are described together
    '''  with the proto support for the language.
    '''
    '''  The JSON representation for `Struct` is JSON object.
    ''' </summary>
    Public NotInheritable Partial Class Struct
        Implements IMessageType(Of Struct)

        Private Shared ReadOnly _parser As MessageParserType(Of Struct) = New MessageParserType(Of Struct)(Function() New Struct())

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property Parser As MessageParserType(Of Struct)
            Get
                Return _parser
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Shared ReadOnly Property DescriptorProp As pbr.MessageDescriptor
            Get
                Return Global.Google.Protobuf.WellKnownTypes.StructReflection.Descriptor.MessageTypes(0)
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
        Public Sub New(other As Struct)
            Me.New()
            fields_ = other.fields_.Clone()
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function Clone() As Struct Implements IDeepCloneable(Of Struct).Clone
            Return New Struct(Me)
        End Function

        ''' <summary>Field number for the "fields" field.</summary>
        Public Const FieldsFieldNumber As Integer = 1
        Private Shared ReadOnly _map_fields_codec As pbc.MapField(Of String, Global.Google.Protobuf.WellKnownTypes.Value).Codec = New pbc.MapField(Of String, Global.Google.Protobuf.WellKnownTypes.Value).Codec(ForString(10), FieldCodec.ForMessage(18, Global.Google.Protobuf.WellKnownTypes.Value.Parser), 10)
        Private ReadOnly fields_ As pbc.MapField(Of String, Global.Google.Protobuf.WellKnownTypes.Value) = New pbc.MapField(Of String, Global.Google.Protobuf.WellKnownTypes.Value)()
        ''' <summary>
        '''  Unordered map of dynamically typed values.
        ''' </summary>
        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public ReadOnly Property Fields As pbc.MapField(Of String, Global.Google.Protobuf.WellKnownTypes.Value)
            Get
                Return fields_
            End Get
        End Property

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, Struct))
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overloads Function Equals(other As Struct) As Boolean Implements IEquatable(Of Struct).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If

            If ReferenceEquals(other, Me) Then
                Return True
            End If

            If Not Fields.Equals(other.Fields) Then Return False
            Return True
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function GetHashCode() As Integer
            Dim hash = 1
            hash = hash Xor Fields.GetHashCode()
            Return hash
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Overrides Function ToString() As String
            Return JsonFormatter.ToDiagnosticString(Me)
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
            fields_.WriteTo(output, _map_fields_codec)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
            Dim size = 0
            size += fields_.CalculateSize(_map_fields_codec)
            Return size
        End Function

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(other As Struct) Implements IMessageType(Of Struct).MergeFrom
            If other Is Nothing Then
                Return
            End If

            fields_.Add(other.fields_)
        End Sub

        <Global.System.Diagnostics.DebuggerNonUserCodeAttribute>
        Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
            Dim tag As New Value(Of UInteger)

            While ((tag = input.ReadTag())) <> 0

                Select Case tag.Value
                    Case 10
                        fields_.AddEntriesFrom(input, _map_fields_codec)

                    Case Else
                        input.SkipLastField()
                End Select
            End While
        End Sub
    End Class


#End Region

End Namespace
#End Region