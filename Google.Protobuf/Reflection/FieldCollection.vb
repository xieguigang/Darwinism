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