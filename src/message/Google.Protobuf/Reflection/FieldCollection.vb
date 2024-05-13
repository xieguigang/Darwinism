#Region "Microsoft.VisualBasic::7a6673f4ff508ea3a66ea8fae7ffcb1c, src\message\Google.Protobuf\Reflection\FieldCollection.vb"

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

    '   Total Lines: 81
    '    Code Lines: 35
    ' Comment Lines: 34
    '   Blank Lines: 12
    '     File Size: 3.71 KB


    '     Class FieldCollection
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: ByJsonName, InDeclarationOrder, InFieldNumberOrder
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace Google.Protobuf.Reflection

    ''' <summary>
    ''' A collection to simplify retrieving the field accessor for a particular field.
    ''' </summary>
    Public NotInheritable Class FieldCollection
        Private ReadOnly messageDescriptor As MessageDescriptor

        Friend Sub New(messageDescriptor As MessageDescriptor)
            Me.messageDescriptor = messageDescriptor
        End Sub

        ''' <value>
        ''' Returns the fields in the message as an immutable list, in the order in which they
        ''' are declared in the source .proto file.
        ''' </value>
        Public Function InDeclarationOrder() As IList(Of FieldDescriptor)
            Return messageDescriptor.fieldsInDeclarationOrder
        End Function

        ''' <value>
        ''' Returns the fields in the message as an immutable list, in ascending field number
        ''' order. Field numbers need not be contiguous, so there is no direct mapping from the
        ''' index in the list to the field number; to retrieve a field by field number, it is better
        ''' to use the <see cref="FieldCollection"/> indexer.
        ''' </value>
        Public Function InFieldNumberOrder() As IList(Of FieldDescriptor)
            Return messageDescriptor.fieldsInNumberOrder
        End Function

        ' TODO: consider making this public in the future. (Being conservative for now...)

        ''' <value>
        ''' Returns a read-only dictionary mapping the field names in this message as they're available
        ''' in the JSON representation to the field descriptors. For example, a field <c>foo_bar</c>
        ''' in the message would result two entries, one with a key <c>fooBar</c> and one with a key
        ''' <c>foo_bar</c>, both referring to the same field.
        ''' </value>
        Friend Function ByJsonName() As IDictionary(Of String, FieldDescriptor)
            Return messageDescriptor.jsonFieldMap
        End Function

        ''' <summary>
        ''' Retrieves the descriptor for the field with the given number.
        ''' </summary>
        ''' <param name="number">Number of the field to retrieve the descriptor for</param>
        ''' <returns>The accessor for the given field</returns>
        ''' <exception cref="KeyNotFoundException">The message descriptor does not contain a field
        ''' with the given number</exception>
        Default Public ReadOnly Property Item(number As Integer) As FieldDescriptor
            Get
                Dim fieldDescriptor = messageDescriptor.FindFieldByNumber(number)

                If fieldDescriptor Is Nothing Then
                    Throw New KeyNotFoundException("No such field number")
                End If

                Return fieldDescriptor
            End Get
        End Property

        ''' <summary>
        ''' Retrieves the descriptor for the field with the given name.
        ''' </summary>
        ''' <param name="name">Name of the field to retrieve the descriptor for</param>
        ''' <returns>The descriptor for the given field</returns>
        ''' <exception cref="KeyNotFoundException">The message descriptor does not contain a field
        ''' with the given name</exception>
        Default Public ReadOnly Property Item(name As String) As FieldDescriptor
            Get
                Dim fieldDescriptor = messageDescriptor.FindFieldByName(name)

                If fieldDescriptor Is Nothing Then
                    Throw New KeyNotFoundException("No such field name")
                End If

                Return fieldDescriptor
            End Get
        End Property
    End Class
End Namespace
