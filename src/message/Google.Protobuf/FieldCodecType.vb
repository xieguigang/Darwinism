#Region "Microsoft.VisualBasic::9785d2b1cbe3a5bcc33b5f027227da6a, src\message\Google.Protobuf\FieldCodecType.vb"

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

    '   Total Lines: 130
    '    Code Lines: 56 (43.08%)
    ' Comment Lines: 57 (43.85%)
    '    - Xml Docs: 94.74%
    ' 
    '   Blank Lines: 17 (13.08%)
    '     File Size: 5.72 KB


    '     Class FieldCodecType
    ' 
    '         Properties: DefaultValue, FixedSize, PackedRepeatedField, Tag, ValueReader
    '                     ValueSizeCalculator, ValueWriter
    ' 
    '         Constructor: (+4 Overloads) Sub New
    ' 
    '         Function: CalculateSizeWithTag, IsDefault, IsPackedRepeatedField, Read
    ' 
    '         Sub: WriteTagAndValue
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace Google.Protobuf

    ''' <summary>
    ''' <para>
    ''' An encode/decode pair for a single field. This effectively encapsulates
    ''' all the information needed to read or write the field value from/to a coded
    ''' stream.
    ''' </para>
    ''' <para>
    ''' This class is public and has to be as it is used by generated code, but its public
    ''' API is very limited - just what the generated code needs to call directly.
    ''' </para>
    ''' </summary>
    ''' <remarks>
    ''' This never writes default values to the stream, and does not address "packedness"
    ''' in repeated fields itself, other than to know whether or not the field *should* be packed.
    ''' </remarks>
    Public NotInheritable Class FieldCodecType(Of T)
        Private Shared ReadOnly DefaultDefault As T
        ' Only non-nullable value types support packing. This is the simplest way of detecting that.
        Private Shared ReadOnly TypeSupportsPacking As Boolean = Nothing IsNot Nothing

        Shared Sub New()
            If GetType(T) Is GetType(String) Then
                DefaultDefault = CType(CObj(""), T)
            ElseIf GetType(T) Is GetType(ByteString) Then
                DefaultDefault = CType(CObj(ByteString.Empty), T)
            End If
            ' Otherwise it's the default value of the CLR type
        End Sub

        Friend Shared Function IsPackedRepeatedField(tag As UInteger) As Boolean
            Return TypeSupportsPacking AndAlso GetTagWireType(tag) = WireType.LengthDelimited
        End Function

        Friend ReadOnly Property PackedRepeatedField As Boolean

        ''' <summary>
        ''' Returns a delegate to write a value (unconditionally) to a coded output stream.
        ''' </summary>
        Friend ReadOnly Property ValueWriter As Action(Of CodedOutputStream, T)

        ''' <summary>
        ''' Returns the size calculator for just a value.
        ''' </summary>
        Friend ReadOnly Property ValueSizeCalculator As Func(Of T, Integer)

        ''' <summary>
        ''' Returns a delegate to read a value from a coded input stream. It is assumed that
        ''' the stream is already positioned on the appropriate tag.
        ''' </summary>
        Friend ReadOnly Property ValueReader As Func(Of CodedInputStream, T)

        ''' <summary>
        ''' Returns the fixed size for an entry, or 0 if sizes vary.
        ''' </summary>
        Friend ReadOnly Property FixedSize As Integer

        ''' <summary>
        ''' Gets the tag of the codec.
        ''' </summary>
        ''' <value>
        ''' The tag of the codec.
        ''' </value>
        Friend ReadOnly Property Tag As UInteger

        ''' <summary>
        ''' Default value for this codec. Usually the same for every instance of the same type, but
        ''' for string/ByteString wrapper fields the codec's default value is null, whereas for
        ''' other string/ByteString fields it's "" or ByteString.Empty.
        ''' </summary>
        ''' <value>
        ''' The default value of the codec's type.
        ''' </value>
        Friend ReadOnly Property DefaultValue As T
        Private ReadOnly tagSize As Integer

        Friend Sub New(reader As Func(Of CodedInputStream, T), writer As Action(Of CodedOutputStream, T), fixedSize As Integer, tag As UInteger)
            Me.New(reader, writer, Function(__) fixedSize, tag)
            Me.FixedSize = fixedSize
        End Sub

        Friend Sub New(reader As Func(Of CodedInputStream, T), writer As Action(Of CodedOutputStream, T), sizeCalculator As Func(Of T, Integer), tag As UInteger)
            Me.New(reader, writer, sizeCalculator, tag, DefaultDefault)
        End Sub

        Friend Sub New(reader As Func(Of CodedInputStream, T), writer As Action(Of CodedOutputStream, T), sizeCalculator As Func(Of T, Integer), tag As UInteger, defaultValue As T)
            ValueReader = reader
            ValueWriter = writer
            ValueSizeCalculator = sizeCalculator
            FixedSize = 0
            Me.Tag = tag
            Me.DefaultValue = defaultValue
            tagSize = CodedOutputStream.ComputeRawVarint32Size(tag)
            ' Detect packed-ness once, so we can check for it within RepeatedField<T>.
            PackedRepeatedField = IsPackedRepeatedField(tag)
        End Sub

        ''' <summary>
        ''' Write a tag and the given value, *if* the value is not the default.
        ''' </summary>
        Public Sub WriteTagAndValue(output As CodedOutputStream, value As T)
            If Not IsDefault(value) Then
                output.WriteTag(Tag)
                Me.ValueWriter()(output, value)
            End If
        End Sub

        ''' <summary>
        ''' Reads a value of the codec type from the given <see cref="CodedInputStream"/>.
        ''' </summary>
        ''' <param name="input">The input stream to read from.</param>
        ''' <returns>The value read from the stream.</returns>
        Public Function Read(input As CodedInputStream) As T
            Return ValueReader(input)
        End Function

        ''' <summary>
        ''' Calculates the size required to write the given value, with a tag,
        ''' if the value is not the default.
        ''' </summary>
        Public Function CalculateSizeWithTag(value As T) As Integer
            Return If(IsDefault(value), 0, ValueSizeCalculator(value) + tagSize)
        End Function

        Private Function IsDefault(value As T) As Boolean
            Return EqualityComparer(Of T).Default.Equals(value, DefaultValue)
        End Function
    End Class
End Namespace
