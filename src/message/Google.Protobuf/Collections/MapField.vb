#Region "Microsoft.VisualBasic::091d6ba17dea23eab9d67309ce161389, src\message\Google.Protobuf\Collections\MapField.vb"

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

    '   Total Lines: 771
    '    Code Lines: 439
    ' Comment Lines: 221
    '   Blank Lines: 111
    '     File Size: 33.58 KB


    '     Class MapField
    ' 
    '         Properties: Count, IsFixedSize, IsReadOnly, IsSynchronized, Item1
    '                     Keys, KeysProp, SyncRoot, Values, ValuesProp
    ' 
    '         Function: CalculateSize, Clone, Contains1, Contains2, ContainsKey
    '                   ContainsValue, (+2 Overloads) Equals, GetEnumerator, GetEnumerator1, GetEnumerator2
    '                   GetHashCode, Remove, Remove1, ToString, TryGetValue
    ' 
    '         Sub: (+2 Overloads) Add, Add1, Add2, AddEntriesFrom, Clear
    '              CopyTo1, CopyTo2, Remove2, WriteTo
    '         Class DictionaryEnumerator
    ' 
    '             Properties: Current, Entry, Key, Value
    ' 
    '             Constructor: (+1 Overloads) Sub New
    ' 
    '             Function: MoveNext
    ' 
    '             Sub: Reset
    ' 
    '         Class Codec
    ' 
    '             Properties: MapTag
    ' 
    '             Constructor: (+1 Overloads) Sub New
    '             Class MessageAdapter
    ' 
    '                 Properties: Descriptor, Key, Value
    ' 
    '                 Constructor: (+1 Overloads) Sub New
    ' 
    '                 Function: CalculateSize
    ' 
    '                 Sub: MergeFrom, Reset, WriteTo
    ' 
    ' 
    ' 
    '         Class MapView
    ' 
    '             Properties: Count, IsReadOnly, IsSynchronized, SyncRoot
    ' 
    '             Constructor: (+1 Overloads) Sub New
    ' 
    '             Function: Contains, GetEnumerator, GetEnumerator3, Remove
    ' 
    '             Sub: Add, Clear, (+2 Overloads) CopyTo
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

#Region "Copyright notice and license"
' Protocol Buffers - Google's data interchange format
' Copyright 2015 Google Inc.  All rights reserved.
' https://developers.google.com/protocol-buffers/
'
' Redistribution and use in source and binary forms, with or without
' modification, are permitted provided that the following conditions are
' met:
'
'     * Redistributions of source code must retain the above copyright
' notice, this list of conditions and the following disclaimer.
'     * Redistributions in binary form must reproduce the above
' copyright notice, this list of conditions and the following disclaimer
' in the documentation and/or other materials provided with the
' distribution.
'     * Neither the name of Google Inc. nor the names of its
' contributors may be used to endorse or promote products derived from
' this software without specific prior written permission.
'
' THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
' "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
' LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
' A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
' OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
' SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
' LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
' DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
' THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
' (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
' OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#End Region

Imports Google.Protobuf.Reflection
Imports Microsoft.VisualBasic.Language
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Runtime.InteropServices

Namespace Google.Protobuf.Collections
    ''' <summary>
    ''' Representation of a map field in a Protocol Buffer message.
    ''' </summary>
    ''' <typeparam name="TKey">Key type in the map. Must be a type supported by Protocol Buffer map keys.</typeparam>
    ''' <typeparam name="TValue">Value type in the map. Must be a type supported by Protocol Buffers.</typeparam>
    ''' <remarks>
    ''' <para>
    ''' For string keys, the equality comparison is provided by <see cref="StringComparer.Ordinal"/>.
    ''' </para>
    ''' <para>
    ''' Null values are not permitted in the map, either for wrapper types or regular messages.
    ''' If a map is deserialized from a data stream and the value is missing from an entry, a default value
    ''' is created instead. For primitive types, that is the regular default value (0, the empty string and so
    ''' on); for message types, an empty instance of the message is created, as if the map entry contained a 0-length
    ''' encoded value for the field.
    ''' </para>
    ''' <para>
    ''' This implementation does not generally prohibit the use of key/value types which are not
    ''' supported by Protocol Buffers (e.g. using a key type of <code>byte</code>) but nor does it guarantee
    ''' that all operations will work in such cases.
    ''' </para>
    ''' <para>
    ''' The order in which entries are returned when iterating over this object is undefined, and may change
    ''' in future versions.
    ''' </para>
    ''' </remarks>
    Public NotInheritable Class MapField(Of TKey, TValue)
        Implements IDeepCloneable(Of MapField(Of TKey, TValue)), IDictionary(Of TKey, TValue), IEquatable(Of MapField(Of TKey, TValue)), IDictionary
        ' TODO: Don't create the map/list until we have an entry. (Assume many maps will be empty.)
        Private ReadOnly map As Dictionary(Of TKey, LinkedListNode(Of KeyValuePair(Of TKey, TValue))) = New Dictionary(Of TKey, LinkedListNode(Of KeyValuePair(Of TKey, TValue)))()
        Private ReadOnly list As LinkedList(Of KeyValuePair(Of TKey, TValue)) = New LinkedList(Of KeyValuePair(Of TKey, TValue))()

        ''' <summary>
        ''' Creates a deep clone of this object.
        ''' </summary>
        ''' <returns>
        ''' A deep clone of this object.
        ''' </returns>
        Public Function Clone() As MapField(Of TKey, TValue) Implements IDeepCloneable(Of MapField(Of TKey, TValue)).Clone
            Dim lClone = New MapField(Of TKey, TValue)()
            ' Keys are never cloneable. Values might be.
            If GetType(IDeepCloneable(Of TValue)).IsAssignableFrom(GetType(TValue)) Then
                For Each pair In list
                    lClone.Add(pair.Key, CType(pair.Value, IDeepCloneable(Of TValue)).Clone())
                Next
            Else
                ' Nothing is cloneable, so we don't need to worry.
                lClone.Add(Me)
            End If

            Return lClone
        End Function

        ''' <summary>
        ''' Adds the specified key/value pair to the map.
        ''' </summary>
        ''' <remarks>
        ''' This operation fails if the key already exists in the map. To replace an existing entry, use the indexer.
        ''' </remarks>
        ''' <param name="key">The key to add</param>
        ''' <param name="value">The value to add.</param>
        ''' <exception cref="System.ArgumentException">The given key already exists in map.</exception>
        Public Sub Add(key As TKey, value As TValue) Implements IDictionary(Of TKey, TValue).Add
            ' Validation of arguments happens in ContainsKey and the indexer
            If ContainsKey(key) Then
                Throw New ArgumentException("Key already exists in map", NameOf(key))
            End If

            Me(key) = value
        End Sub

        ''' <summary>
        ''' Determines whether the specified key is present in the map.
        ''' </summary>
        ''' <param name="key">The key to check.</param>
        ''' <returns><c>true</c> if the map contains the given key; <c>false</c> otherwise.</returns>
        Public Function ContainsKey(key As TKey) As Boolean Implements IDictionary(Of TKey, TValue).ContainsKey
            CheckNotNullUnconstrained(key, NameOf(key))
            Return map.ContainsKey(key)
        End Function

        Private Function ContainsValue(value As TValue) As Boolean
            Dim comparer = EqualityComparer(Of TValue).Default
            Return list.Any(Function(pair) comparer.Equals(pair.Value, value))
        End Function

        ''' <summary>
        ''' Removes the entry identified by the given key from the map.
        ''' </summary>
        ''' <param name="key">The key indicating the entry to remove from the map.</param>
        ''' <returns><c>true</c> if the map contained the given key before the entry was removed; <c>false</c> otherwise.</returns>
        Public Function Remove(key As TKey) As Boolean Implements IDictionary(Of TKey, TValue).Remove
            CheckNotNullUnconstrained(key, NameOf(key))
            Dim node As LinkedListNode(Of KeyValuePair(Of TKey, TValue))

            If map.TryGetValue(key, node) Then
                map.Remove(key)
                node.List.Remove(node)
                Return True
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' Gets the value associated with the specified key.
        ''' </summary>
        ''' <param name="key">The key whose value to get.</param>
        ''' <param name="value">When this method returns, the value associated with the specified key, if the key is found;
        ''' otherwise, the default value for the type of the <paramrefname="value"/> parameter.
        ''' This parameter is passed uninitialized.</param>
        ''' <returns><c>true</c> if the map contains an element with the specified key; otherwise, <c>false</c>.</returns>
        Public Function TryGetValue(key As TKey, <Out> ByRef value As TValue) As Boolean Implements IDictionary(Of TKey, TValue).TryGetValue
            Dim node As LinkedListNode(Of KeyValuePair(Of TKey, TValue))

            If map.TryGetValue(key, node) Then
                value = node.Value.Value
                Return True
            Else
                value = Nothing
                Return False
            End If
        End Function

        ''' <summary>
        ''' Gets or sets the value associated with the specified key.
        ''' </summary>
        ''' <param name="key">The key of the value to get or set.</param>
        ''' <exception cref="KeyNotFoundException">The property is retrieved and key does not exist in the collection.</exception>
        ''' <returns>The value associated with the specified key. If the specified key is not found,
        ''' a get operation throws a <see cref="KeyNotFoundException"/>, and a set operation creates a new element with the specified key.</returns>
        Default Public Property Item(key As TKey) As TValue Implements IDictionary(Of TKey, TValue).Item
            Get
                CheckNotNullUnconstrained(key, NameOf(key))
                Dim value As TValue

                If TryGetValue(key, value) Then
                    Return value
                End If

                Throw New KeyNotFoundException()
            End Get
            Set(value As TValue)
                CheckNotNullUnconstrained(key, NameOf(key))
                ' value == null check here is redundant, but avoids boxing.
                If value Is Nothing Then
                    CheckNotNullUnconstrained(value, NameOf(value))
                End If

                Dim node As LinkedListNode(Of KeyValuePair(Of TKey, TValue))
                Dim pair = New KeyValuePair(Of TKey, TValue)(key, value)

                If map.TryGetValue(key, node) Then
                    node.Value = pair
                Else
                    node = list.AddLast(pair)
                    map(key) = node
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets a collection containing the keys in the map.
        ''' </summary>
        Public ReadOnly Property KeysProp As ICollection(Of TKey) Implements IDictionary(Of TKey, TValue).Keys
            Get
                Return New MapView(Of TKey)(Me, Function(pair) pair.Key, AddressOf ContainsKey)
            End Get
        End Property

        ''' <summary>
        ''' Gets a collection containing the values in the map.
        ''' </summary>
        Public ReadOnly Property ValuesProp As ICollection(Of TValue) Implements IDictionary(Of TKey, TValue).Values
            Get
                Return New MapView(Of TValue)(Me, Function(pair) pair.Value, AddressOf ContainsValue)
            End Get
        End Property

        ''' <summary>
        ''' Adds the specified entries to the map. The keys and values are not automatically cloned.
        ''' </summary>
        ''' <param name="entries">The entries to add to the map.</param>
        Public Sub Add(entries As IDictionary(Of TKey, TValue))
            CheckNotNull(entries, NameOf(entries))

            For Each pair In entries
                Add(pair.Key, pair.Value)
            Next
        End Sub

        ''' <summary>
        ''' Returns an enumerator that iterates through the collection.
        ''' </summary>
        ''' <returns>
        ''' An enumerator that can be used to iterate through the collection.
        ''' </returns>
        Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, TValue)) Implements IEnumerable(Of KeyValuePair(Of TKey, TValue)).GetEnumerator
            Return list.GetEnumerator()
        End Function

        ''' <summary>
        ''' Returns an enumerator that iterates through a collection.
        ''' </summary>
        ''' <returns>
        ''' An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        ''' </returns>
        Private Function GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
            Return GetEnumerator()
        End Function

        ''' <summary>
        ''' Adds the specified item to the map.
        ''' </summary>
        ''' <param name="item">The item to add to the map.</param>
        Private Sub Add1(item As KeyValuePair(Of TKey, TValue)) Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Add
            Add(item.Key, item.Value)
        End Sub

        ''' <summary>
        ''' Removes all items from the map.
        ''' </summary>
        Public Sub Clear() Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Clear, IDictionary.Clear
            list.Clear()
            map.Clear()
        End Sub

        ''' <summary>
        ''' Determines whether map contains an entry equivalent to the given key/value pair.
        ''' </summary>
        ''' <param name="item">The key/value pair to find.</param>
        ''' <returns></returns>
        Private Function Contains1(item As KeyValuePair(Of TKey, TValue)) As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Contains
            Dim value As TValue
            Return TryGetValue(item.Key, value) AndAlso EqualityComparer(Of TValue).Default.Equals(item.Value, value)
        End Function

        ''' <summary>
        ''' Copies the key/value pairs in this map to an array.
        ''' </summary>
        ''' <param name="array">The array to copy the entries into.</param>
        ''' <param name="arrayIndex">The index of the array at which to start copying values.</param>
        Private Sub CopyTo1(array As KeyValuePair(Of TKey, TValue)(), arrayIndex As Integer) Implements ICollection(Of KeyValuePair(Of TKey, TValue)).CopyTo
            list.CopyTo(array, arrayIndex)
        End Sub

        ''' <summary>
        ''' Removes the specified key/value pair from the map.
        ''' </summary>
        ''' <remarks>Both the key and the value must be found for the entry to be removed.</remarks>
        ''' <param name="item">The key/value pair to remove.</param>
        ''' <returns><c>true</c> if the key/value pair was found and removed; <c>false</c> otherwise.</returns>
        Private Function Remove1(item As KeyValuePair(Of TKey, TValue)) As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Remove
            If item.Key Is Nothing Then
                Throw New ArgumentException("Key is null", NameOf(item))
            End If

            Dim node As LinkedListNode(Of KeyValuePair(Of TKey, TValue))

            If map.TryGetValue(item.Key, node) AndAlso EqualityComparer(Of TValue).Default.Equals(item.Value, node.Value.Value) Then
                map.Remove(item.Key)
                node.List.Remove(node)
                Return True
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' Gets the number of elements contained in the map.
        ''' </summary>
        Public ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Count, ICollection.Count
            Get
                Return list.Count
            End Get
        End Property

        ''' <summary>
        ''' Gets a value indicating whether the map is read-only.
        ''' </summary>
        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).IsReadOnly, IDictionary.IsReadOnly
            Get
                Return False
            End Get
        End Property

        ''' <summary>
        ''' Determines whether the specified <see cref="System.Object"/>, is equal to this instance.
        ''' </summary>
        ''' <param name="other">The <see cref="System.Object"/> to compare with this instance.</param>
        ''' <returns>
        '''   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        ''' </returns>
        Public Overrides Function Equals(other As Object) As Boolean
            Return Equals(TryCast(other, MapField(Of TKey, TValue)))
        End Function

        ''' <summary>
        ''' Returns a hash code for this instance.
        ''' </summary>
        ''' <returns>
        ''' A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        ''' </returns>
        Public Overrides Function GetHashCode() As Integer
            Dim valueComparer = EqualityComparer(Of TValue).Default
            Dim hash = 0

            For Each pair In list
                hash = hash Xor pair.Key.GetHashCode() * 31 + valueComparer.GetHashCode(pair.Value)
            Next

            Return hash
        End Function

        ''' <summary>
        ''' Compares this map with another for equality.
        ''' </summary>
        ''' <remarks>
        ''' The order of the key/value pairs in the maps is not deemed significant in this comparison.
        ''' </remarks>
        ''' <param name="other">The map to compare this with.</param>
        ''' <returns><c>true</c> if <paramrefname="other"/> refers to an equal map; <c>false</c> otherwise.</returns>
        Public Overloads Function Equals(other As MapField(Of TKey, TValue)) As Boolean Implements IEquatable(Of MapField(Of TKey, TValue)).Equals
            If other Is Nothing Then
                Return False
            End If

            If other Is Me Then
                Return True
            End If

            If other.Count <> Count Then
                Return False
            End If

            Dim valueComparer = EqualityComparer(Of TValue).Default

            For Each pair In Me
                Dim value As TValue

                If Not other.TryGetValue(pair.Key, value) Then
                    Return False
                End If

                If Not valueComparer.Equals(value, pair.Value) Then
                    Return False
                End If
            Next

            Return True
        End Function

        ''' <summary>
        ''' Adds entries to the map from the given stream.
        ''' </summary>
        ''' <remarks>
        ''' It is assumed that the stream is initially positioned after the tag specified by the codec.
        ''' This method will continue reading entries from the stream until the end is reached, or
        ''' a different tag is encountered.
        ''' </remarks>
        ''' <param name="input">Stream to read from</param>
        ''' <param name="codec">Codec describing how the key/value pairs are encoded</param>
        Public Sub AddEntriesFrom(input As CodedInputStream, codec As Codec)
            Dim adapter = New Codec.MessageAdapter(codec)

            Do
                adapter.Reset()
                input.ReadMessage(adapter)
                Me(adapter.Key) = adapter.Value
            Loop While input.MaybeConsumeTag(codec.MapTag)
        End Sub

        ''' <summary>
        ''' Writes the contents of this map to the given coded output stream, using the specified codec
        ''' to encode each entry.
        ''' </summary>
        ''' <param name="output">The output stream to write to.</param>
        ''' <param name="codec">The codec to use for each entry.</param>
        Public Sub WriteTo(output As CodedOutputStream, codec As Codec)
            Dim message = New Codec.MessageAdapter(codec)

            For Each entry In list
                message.Key = entry.Key
                message.Value = entry.Value
                output.WriteTag(codec.MapTag)
                output.WriteMessage(message)
            Next
        End Sub

        ''' <summary>
        ''' Calculates the size of this map based on the given entry codec.
        ''' </summary>
        ''' <param name="codec">The codec to use to encode each entry.</param>
        ''' <returns></returns>
        Public Function CalculateSize(codec As Codec) As Integer
            If Count = 0 Then
                Return 0
            End If

            Dim message = New Codec.MessageAdapter(codec)
            Dim size = 0

            For Each entry In list
                message.Key = entry.Key
                message.Value = entry.Value
                size += CodedOutputStream.ComputeRawVarint32Size(codec.MapTag)
                size += CodedOutputStream.ComputeMessageSize(message)
            Next

            Return size
        End Function

        ''' <summary>
        ''' Returns a string representation of this repeated field, in the same
        ''' way as it would be represented by the default JSON formatter.
        ''' </summary>
        Public Overrides Function ToString() As String
            Dim writer = New StringWriter()
            JsonFormatter.Default.WriteDictionary(writer, Me)
            Return writer.ToString()
        End Function

#Region "IDictionary explicit interface implementation"
        Private Sub Add2(key As Object, value As Object) Implements IDictionary.Add
            Add(key, value)
        End Sub

        Private Function Contains2(key As Object) As Boolean Implements IDictionary.Contains
            If Not (TypeOf key Is TKey) Then
                Return False
            End If

            Return ContainsKey(key)
        End Function

        Private Function GetEnumerator2() As IDictionaryEnumerator Implements IDictionary.GetEnumerator
            Return New DictionaryEnumerator(GetEnumerator())
        End Function

        Private Sub Remove2(key As Object) Implements IDictionary.Remove
            CheckNotNull(key, NameOf(key))

            If Not (TypeOf key Is TKey) Then
                Return
            End If

            Remove(key)
        End Sub

        Private Sub CopyTo2(array As Array, index As Integer) Implements ICollection.CopyTo
            ' This is ugly and slow as heck, but with any luck it will never be used anyway.
            Dim temp As ICollection = [Select](Function(pair) New DictionaryEntry(pair.Key, pair.Value)).ToList()
            temp.CopyTo(array, index)
        End Sub

        Private ReadOnly Property IsFixedSize As Boolean Implements IDictionary.IsFixedSize
            Get
                Return False
            End Get
        End Property

        Private ReadOnly Property Keys As ICollection Implements IDictionary.Keys
            Get
                Return CType(KeysProp, ICollection)
            End Get
        End Property

        Private ReadOnly Property Values As ICollection Implements IDictionary.Values
            Get
                Return CType(ValuesProp, ICollection)
            End Get
        End Property

        Private ReadOnly Property IsSynchronized As Boolean Implements ICollection.IsSynchronized
            Get
                Return False
            End Get
        End Property

        Private ReadOnly Property SyncRoot As Object Implements ICollection.SyncRoot
            Get
                Return Me
            End Get
        End Property

        Private Property Item1(key As Object) As Object Implements IDictionary.Item
            Get
                CheckNotNull(key, NameOf(key))

                If Not (TypeOf key Is TKey) Then
                    Return Nothing
                End If

                Dim value As TValue
                TryGetValue(key, value)
                Return value
            End Get
            Set(value As Object)
                Me(key) = CType(value, TValue)
            End Set
        End Property
#End Region

        Private Class DictionaryEnumerator
            Implements IDictionaryEnumerator

            Private ReadOnly enumerator As IEnumerator(Of KeyValuePair(Of TKey, TValue))

            Friend Sub New(enumerator As IEnumerator(Of KeyValuePair(Of TKey, TValue)))
                Me.enumerator = enumerator
            End Sub

            Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
                Return enumerator.MoveNext()
            End Function

            Public Sub Reset() Implements IEnumerator.Reset
                enumerator.Reset()
            End Sub

            Public ReadOnly Property Current As Object Implements IEnumerator.Current
                Get
                    Return Entry
                End Get
            End Property

            Public ReadOnly Property Entry As DictionaryEntry Implements IDictionaryEnumerator.Entry
                Get
                    Return New DictionaryEntry(Key, Value)
                End Get
            End Property

            Public ReadOnly Property Key As Object Implements IDictionaryEnumerator.Key
                Get
                    Return enumerator.Current.Key
                End Get
            End Property

            Public ReadOnly Property Value As Object Implements IDictionaryEnumerator.Value
                Get
                    Return enumerator.Current.Value
                End Get
            End Property
        End Class

        ''' <summary>
        ''' A codec for a specific map field. This contains all the information required to encode and
        ''' decode the nested messages.
        ''' </summary>
        Public NotInheritable Class Codec
            Private ReadOnly keyCodec As FieldCodecType(Of TKey)
            Private ReadOnly valueCodec As FieldCodecType(Of TValue)
            Private ReadOnly mapTagField As UInteger

            ''' <summary>
            ''' Creates a new entry codec based on a separate key codec and value codec,
            ''' and the tag to use for each map entry.
            ''' </summary>
            ''' <param name="keyCodec">The key codec.</param>
            ''' <param name="valueCodec">The value codec.</param>
            ''' <param name="mapTag">The map tag to use to introduce each map entry.</param>
            Public Sub New(keyCodec As FieldCodecType(Of TKey), valueCodec As FieldCodecType(Of TValue), mapTag As UInteger)
                Me.keyCodec = keyCodec
                Me.valueCodec = valueCodec
                mapTagField = mapTag
            End Sub

            ''' <summary>
            ''' The tag used in the enclosing message to indicate map entries.
            ''' </summary>
            Friend ReadOnly Property MapTag As UInteger
                Get
                    Return mapTagField
                End Get
            End Property

            ''' <summary>
            ''' A mutable message class, used for parsing and serializing. This
            ''' delegates the work to a codec, but implements the <see cref="IMessage"/> interface
            ''' for interop with <see cref="CodedInputStream"/> and <see cref="CodedOutputStream"/>.
            ''' This is nested inside Codec as it's tightly coupled to the associated codec,
            ''' and it's simpler if it has direct access to all its fields.
            ''' </summary>
            Friend Class MessageAdapter
                Implements IMessage

                Private Shared ReadOnly ZeroLengthMessageStreamData As Byte() = New Byte() {0}
                Private ReadOnly codec As Codec
                Friend Property Key As TKey
                Friend Property Value As TValue

                Friend Sub New(codec As Codec)
                    Me.codec = codec
                End Sub

                Friend Sub Reset()
                    Key = codec.keyCodec.DefaultValue
                    Value = codec.valueCodec.DefaultValue
                End Sub

                Public Sub MergeFrom(input As CodedInputStream) Implements IMessage.MergeFrom
                    Dim tag As New Value(Of UInteger)

                    While ((tag = input.ReadTag())) <> 0

                        If tag.Value = codec.keyCodec.Tag Then
                            Key = codec.keyCodec.Read(input)
                        ElseIf tag.Value = codec.valueCodec.Tag Then
                            Value = codec.valueCodec.Read(input)
                        Else
                            input.SkipLastField()
                        End If
                    End While

                    ' Corner case: a map entry with a key but no value, where the value type is a message.
                    ' Read it as if we'd seen an input stream with no data (i.e. create a "default" message).
                    If Value Is Nothing Then
                        Value = codec.valueCodec.Read(New CodedInputStream(ZeroLengthMessageStreamData))
                    End If
                End Sub

                Public Sub WriteTo(output As CodedOutputStream) Implements IMessage.WriteTo
                    codec.keyCodec.WriteTagAndValue(output, Key)
                    codec.valueCodec.WriteTagAndValue(output, Value)
                End Sub

                Public Function CalculateSize() As Integer Implements IMessage.CalculateSize
                    Return codec.keyCodec.CalculateSizeWithTag(Key) + codec.valueCodec.CalculateSizeWithTag(Value)
                End Function

                Private ReadOnly Property Descriptor As MessageDescriptor Implements IMessage.Descriptor
                    Get
                        Return Nothing
                    End Get
                End Property
            End Class
        End Class

        Private Class MapView(Of T)
            Implements ICollection(Of T), ICollection

            Private ReadOnly parent As MapField(Of TKey, TValue)
            Private ReadOnly projection As Func(Of KeyValuePair(Of TKey, TValue), T)
            Private ReadOnly containsCheck As Func(Of T, Boolean)

            Friend Sub New(parent As MapField(Of TKey, TValue), projection As Func(Of KeyValuePair(Of TKey, TValue), T), containsCheck As Func(Of T, Boolean))
                Me.parent = parent
                Me.projection = projection
                Me.containsCheck = containsCheck
            End Sub

            Public ReadOnly Property Count As Integer Implements ICollection(Of T).Count, ICollection.Count
                Get
                    Return parent.Count
                End Get
            End Property

            Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of T).IsReadOnly
                Get
                    Return True
                End Get
            End Property

            Public ReadOnly Property IsSynchronized As Boolean Implements ICollection.IsSynchronized
                Get
                    Return False
                End Get
            End Property

            Public ReadOnly Property SyncRoot As Object Implements ICollection.SyncRoot
                Get
                    Return parent
                End Get
            End Property

            Public Sub Add(item As T) Implements ICollection(Of T).Add
                Throw New NotSupportedException()
            End Sub

            Public Sub Clear() Implements ICollection(Of T).Clear
                Throw New NotSupportedException()
            End Sub

            Public Function Contains(item As T) As Boolean Implements ICollection(Of T).Contains
                Return containsCheck(item)
            End Function

            Public Sub CopyTo(array As T(), arrayIndex As Integer) Implements ICollection(Of T).CopyTo
                If arrayIndex < 0 Then
                    Throw New ArgumentOutOfRangeException(NameOf(arrayIndex))
                End If

                If arrayIndex + Count >= array.Length Then
                    Throw New ArgumentException("Not enough space in the array", NameOf(array))
                End If

                For Each item As T In Me
                    array(Math.Min(Threading.Interlocked.Increment(arrayIndex), arrayIndex - 1)) = item
                Next
            End Sub

            Public Function GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
                Return Enumerable.Select(parent.list, projection).GetEnumerator()
            End Function

            Public Function Remove(item As T) As Boolean Implements ICollection(Of T).Remove
                Throw New NotSupportedException()
            End Function

            Private Function GetEnumerator3() As IEnumerator Implements IEnumerable.GetEnumerator
                Return GetEnumerator()
            End Function

            Public Sub CopyTo(array As Array, index As Integer) Implements ICollection.CopyTo
                If index < 0 Then
                    Throw New ArgumentOutOfRangeException(NameOf(index))
                End If

                If index + Count >= array.Length Then
                    Throw New ArgumentException("Not enough space in the array", NameOf(array))
                End If

                For Each item As T In Me
                    array.SetValue(item, Math.Min(Threading.Interlocked.Increment(index), index - 1))
                Next
            End Sub
        End Class
    End Class
End Namespace
