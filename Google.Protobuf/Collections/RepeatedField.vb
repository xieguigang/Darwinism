#Region "Microsoft.VisualBasic::f54ea1eef27595edd2b2401217762ae6, Google.Protobuf\Collections\RepeatedField.vb"

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

    '     Class RepeatedField
    ' 
    '         Properties: Count, IsFixedSize, IsReadOnly, IsSynchronized, Item1
    '                     SyncRoot
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '         Function: Add1, CalculatePackedDataSize, CalculateSize, Clone, Contains
    '                   Contains1, (+2 Overloads) Equals, GetEnumerator, GetEnumerator1, GetHashCode
    '                   IndexOf, IndexOf1, Remove, ToString
    ' 
    '         Sub: (+2 Overloads) Add, AddEntriesFrom, AddRange, Clear, CopyTo
    '              CopyTo1, EnsureSize, Insert, Insert1, Remove1
    '              RemoveAt, WriteTo
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

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.IO

Namespace Google.Protobuf.Collections
    ''' <summary>
    ''' The contents of a repeated field: essentially, a collection with some extra
    ''' restrictions (no null values) and capabilities (deep cloning).
    ''' </summary>
    ''' <remarks>
    ''' This implementation does not generally prohibit the use of types which are not
    ''' supported by Protocol Buffers but nor does it guarantee that all operations will work in such cases.
    ''' </remarks>
    ''' <typeparam name="T">The element type of the repeated field.</typeparam>
    Public NotInheritable Class RepeatedField(Of T)
        Implements IList(Of T), IList, IDeepCloneable(Of RepeatedField(Of T)), IEquatable(Of RepeatedField(Of T))

        Private Shared ReadOnly EmptyArray As T() = New T(-1) {}
        Private Const MinArraySize As Integer = 8
        Private array As T() = EmptyArray
        Private countField As Integer = 0

        Public Sub New()

        End Sub

        Sub New(data As IEnumerable(Of T))
            For Each item As T In data
                Call Add(item)
            Next
        End Sub

        ''' <summary>
        ''' Creates a deep clone of this repeated field.
        ''' </summary>
        ''' <remarks>
        ''' If the field type is
        ''' a message type, each element is also cloned; otherwise, it is
        ''' assumed that the field type is primitive (including string and
        ''' bytes, both of which are immutable) and so a simple copy is
        ''' equivalent to a deep clone.
        ''' </remarks>
        ''' <returns>A deep clone of this repeated field.</returns>
        Public Function Clone() As RepeatedField(Of T) Implements IDeepCloneable(Of RepeatedField(Of T)).Clone
            Dim lClone As RepeatedField(Of T) = New RepeatedField(Of T)()

            If array IsNot EmptyArray Then
                lClone.array = CType(array.Clone(), T())
                Dim cloneableArray As IDeepCloneable(Of T)() = TryCast(lClone.array, IDeepCloneable(Of T)())

                If cloneableArray IsNot Nothing Then
                    For i = 0 To countField - 1
                        lClone.array(i) = cloneableArray(i).Clone()
                    Next
                End If
            End If

            lClone.countField = countField
            Return lClone
        End Function

        ''' <summary>
        ''' Adds the entries from the given input stream, decoding them with the specified codec.
        ''' </summary>
        ''' <param name="input">The input stream to read from.</param>
        ''' <param name="codec">The codec to use in order to read each entry.</param>
        Public Sub AddEntriesFrom(input As CodedInputStream, codec As FieldCodecType(Of T))
            ' TODO: Inline some of the Add code, so we can avoid checking the size on every
            ' iteration.
            Dim tag = input.LastTag
            Dim reader = codec.ValueReader
            ' Non-nullable value types can be packed or not.
            If FieldCodecType(Of T).IsPackedRepeatedField(tag) Then
                Dim length As Integer = input.ReadLength()

                If length > 0 Then
                    Dim oldLimit = input.PushLimit(length)

                    While Not input.ReachedLimit
                        Add(reader(input))
                    End While

                    input.PopLimit(oldLimit)
                    ' Empty packed field. Odd, but valid - just ignore.
                End If
            Else
                ' Not packed... (possibly not packable)
                Do
                    Add(reader(input))
                Loop While input.MaybeConsumeTag(tag)
            End If
        End Sub

        ''' <summary>
        ''' Calculates the size of this collection based on the given codec.
        ''' </summary>
        ''' <param name="codec">The codec to use when encoding each field.</param>
        ''' <returns>The number of bytes that would be written to a <see cref="CodedOutputStream"/> by <see cref="WriteTo"/>,
        ''' using the same codec.</returns>
        Public Function CalculateSize(codec As FieldCodecType(Of T)) As Integer
            If countField = 0 Then
                Return 0
            End If

            Dim tag = codec.Tag

            If codec.PackedRepeatedField Then
                Dim dataSize = CalculatePackedDataSize(codec)
                Return CodedOutputStream.ComputeRawVarint32Size(tag) + CodedOutputStream.ComputeLengthSize(dataSize) + dataSize
            Else
                Dim sizeCalculator = codec.ValueSizeCalculator
                Dim size = countField * CodedOutputStream.ComputeRawVarint32Size(tag)

                For i = 0 To countField - 1
                    size += sizeCalculator(array(i))
                Next

                Return size
            End If
        End Function

        Private Function CalculatePackedDataSize(codec As FieldCodecType(Of T)) As Integer
            Dim fixedSize = codec.FixedSize

            If fixedSize = 0 Then
                Dim calculator = codec.ValueSizeCalculator
                Dim tmp = 0

                For i = 0 To countField - 1
                    tmp += calculator(array(i))
                Next

                Return tmp
            Else
                Return fixedSize * Count
            End If
        End Function

        ''' <summary>
        ''' Writes the contents of this collection to the given <see cref="CodedOutputStream"/>,
        ''' encoding each value using the specified codec.
        ''' </summary>
        ''' <param name="output">The output stream to write to.</param>
        ''' <param name="codec">The codec to use when encoding each value.</param>
        Public Sub WriteTo(output As CodedOutputStream, codec As FieldCodecType(Of T))
            If countField = 0 Then
                Return
            End If

            Dim writer = codec.ValueWriter
            Dim tag = codec.Tag

            If codec.PackedRepeatedField Then
                ' Packed primitive type
                Dim size As UInteger = CalculatePackedDataSize(codec)
                output.WriteTag(tag)
                output.WriteRawVarint32(size)

                For i = 0 To countField - 1
                    writer(output, array(i))
                Next
            Else
                ' Not packed: a simple tag/value pair for each value.
                ' Can't use codec.WriteTagAndValue, as that omits default values.
                For i = 0 To countField - 1
                    output.WriteTag(tag)
                    writer(output, array(i))
                Next
            End If
        End Sub

        Private Sub EnsureSize(size As Integer)
            If array.Length < size Then
                size = Math.Max(size, MinArraySize)
                Dim newSize = Math.Max(array.Length * 2, size)
                Dim tmp = New T(newSize - 1) {}
                System.Array.Copy(array, 0, tmp, 0, array.Length)
                array = tmp
            End If
        End Sub

        ''' <summary>
        ''' Adds the specified item to the collection.
        ''' </summary>
        ''' <param name="item">The item to add.</param>
        Public Sub Add(item As T) Implements ICollection(Of T).Add
            CheckNotNullUnconstrained(item, NameOf(item))
            EnsureSize(countField + 1)
            array(Math.Min(Threading.Interlocked.Increment(countField), countField - 1)) = item
        End Sub

        ''' <summary>
        ''' Removes all items from the collection.
        ''' </summary>
        Public Sub Clear() Implements ICollection(Of T).Clear, IList.Clear
            array = EmptyArray
            countField = 0
        End Sub

        ''' <summary>
        ''' Determines whether this collection contains the given item.
        ''' </summary>
        ''' <param name="item">The item to find.</param>
        ''' <returns><c>true</c> if this collection contains the given item; <c>false</c> otherwise.</returns>
        Public Function Contains(item As T) As Boolean Implements ICollection(Of T).Contains
            Return IndexOf(item) <> -1
        End Function

        ''' <summary>
        ''' Copies this collection to the given array.
        ''' </summary>
        ''' <param name="array">The array to copy to.</param>
        ''' <param name="arrayIndex">The first index of the array to copy to.</param>
        Public Sub CopyTo(array As T(), arrayIndex As Integer) Implements ICollection(Of T).CopyTo
            System.Array.Copy(Me.array, 0, array, arrayIndex, countField)
        End Sub

        ''' <summary>
        ''' Removes the specified item from the collection
        ''' </summary>
        ''' <param name="item">The item to remove.</param>
        ''' <returns><c>true</c> if the item was found and removed; <c>false</c> otherwise.</returns>
        Public Function Remove(item As T) As Boolean Implements ICollection(Of T).Remove
            Dim index = IndexOf(item)

            If index = -1 Then
                Return False
            End If

            System.Array.Copy(array, index + 1, array, index, countField - index - 1)
            countField -= 1
            array(countField) = Nothing
            Return True
        End Function

        ''' <summary>
        ''' Gets the number of elements contained in the collection.
        ''' </summary>
        Public ReadOnly Property Count As Integer Implements ICollection(Of T).Count, ICollection.Count
            Get
                Return countField
            End Get
        End Property

        ''' <summary>
        ''' Gets a value indicating whether the collection is read-only.
        ''' </summary>
        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of T).IsReadOnly, IList.IsReadOnly
            Get
                Return False
            End Get
        End Property

        ''' <summary>
        ''' Adds all of the specified values into this collection.
        ''' </summary>
        ''' <param name="values">The values to add to this collection.</param>
        Public Sub AddRange(values As IEnumerable(Of T))
            CheckNotNull(values, NameOf(values))

            ' Optimization 1: If the collection we're adding is already a RepeatedField<T>,
            ' we know the values are valid.
            Dim otherRepeatedField = TryCast(values, RepeatedField(Of T))

            If otherRepeatedField IsNot Nothing Then
                EnsureSize(countField + otherRepeatedField.countField)
                System.Array.Copy(otherRepeatedField.array, 0, array, countField, otherRepeatedField.countField)
                countField += otherRepeatedField.countField
                Return
            End If

            ' Optimization 2: The collection is an ICollection, so we can expand
            ' just once and ask the collection to copy itself into the array.
            Dim collection = TryCast(values, ICollection)

            If collection IsNot Nothing Then
                Dim extraCount = collection.Count
                ' For reference types and nullable value types, we need to check that there are no nulls
                ' present. (This isn't a thread-safe approach, but we don't advertise this is thread-safe.)
                ' We expect the JITter to optimize this test to true/false, so it's effectively conditional
                ' specialization.
                If Nothing Is Nothing Then
                    ' TODO: Measure whether iterating once to check and then letting the collection copy
                    ' itself is faster or slower than iterating and adding as we go. For large
                    ' collections this will not be great in terms of cache usage... but the optimized
                    ' copy may be significantly faster than doing it one at a time.
                    For Each item As T In collection

                        If item Is Nothing Then
                            Throw New ArgumentException("Sequence contained null element", NameOf(values))
                        End If
                    Next
                End If

                EnsureSize(countField + extraCount)
                collection.CopyTo(array, countField)
                countField += extraCount
                Return
            End If

            ' We *could* check for ICollection<T> as well, but very very few collections implement
            ' ICollection<T> but not ICollection. (HashSet<T> does, for one...)

            ' Fall back to a slower path of adding items one at a time.
            For Each item As T In values
                Add(item)
            Next
        End Sub

        ''' <summary>
        ''' Adds all of the specified values into this collection. This method is present to
        ''' allow repeated fields to be constructed from queries within collection initializers.
        ''' Within non-collection-initializer code, consider using the equivalent <see cref="AddRange"/>
        ''' method instead for clarity.
        ''' </summary>
        ''' <param name="values">The values to add to this collection.</param>
        Public Sub Add(values As IEnumerable(Of T))
            AddRange(values)
        End Sub

        ''' <summary>
        ''' Returns an enumerator that iterates through the collection.
        ''' </summary>
        ''' <returns>
        ''' An enumerator that can be used to iterate through the collection.
        ''' </returns>
        Public Iterator Function GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
            For i = 0 To countField - 1
                Yield array(i)
            Next
        End Function

        ''' <summary>
        ''' Determines whether the specified <see cref="System.Object"/>, is equal to this instance.
        ''' </summary>
        ''' <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        ''' <returns>
        '''   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        ''' </returns>
        Public Overrides Function Equals(obj As Object) As Boolean
            Return Equals(TryCast(obj, RepeatedField(Of T)))
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
        ''' Returns a hash code for this instance.
        ''' </summary>
        ''' <returns>
        ''' A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        ''' </returns>
        Public Overrides Function GetHashCode() As Integer
            Dim hash = 0

            For i = 0 To countField - 1
                hash = hash * 31 + array(i).GetHashCode()
            Next

            Return hash
        End Function

        ''' <summary>
        ''' Compares this repeated field with another for equality.
        ''' </summary>
        ''' <param name="other">The repeated field to compare this with.</param>
        ''' <returns><c>true</c> if <paramrefname="other"/> refers to an equal repeated field; <c>false</c> otherwise.</returns>
        Public Overloads Function Equals(other As RepeatedField(Of T)) As Boolean Implements IEquatable(Of RepeatedField(Of T)).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If

            If ReferenceEquals(other, Me) Then
                Return True
            End If

            If other.Count <> Count Then
                Return False
            End If

            Dim comparer = EqualityComparer(Of T).Default

            For i = 0 To countField - 1

                If Not comparer.Equals(array(i), other.array(i)) Then
                    Return False
                End If
            Next

            Return True
        End Function

        ''' <summary>
        ''' Returns the index of the given item within the collection, or -1 if the item is not
        ''' present.
        ''' </summary>
        ''' <param name="item">The item to find in the collection.</param>
        ''' <returns>The zero-based index of the item, or -1 if it is not found.</returns>
        Public Function IndexOf(item As T) As Integer Implements IList(Of T).IndexOf
            CheckNotNullUnconstrained(item, NameOf(item))
            Dim comparer = EqualityComparer(Of T).Default

            For i = 0 To countField - 1

                If comparer.Equals(array(i), item) Then
                    Return i
                End If
            Next

            Return -1
        End Function

        ''' <summary>
        ''' Inserts the given item at the specified index.
        ''' </summary>
        ''' <param name="index">The index at which to insert the item.</param>
        ''' <param name="item">The item to insert.</param>
        Public Sub Insert(index As Integer, item As T) Implements IList(Of T).Insert
            CheckNotNullUnconstrained(item, NameOf(item))

            If index < 0 OrElse index > countField Then
                Throw New ArgumentOutOfRangeException(NameOf(index))
            End If

            EnsureSize(countField + 1)
            System.Array.Copy(array, index, array, index + 1, countField - index)
            array(index) = item
            countField += 1
        End Sub

        ''' <summary>
        ''' Removes the item at the given index.
        ''' </summary>
        ''' <param name="index">The zero-based index of the item to remove.</param>
        Public Sub RemoveAt(index As Integer) Implements IList(Of T).RemoveAt, IList.RemoveAt
            If index < 0 OrElse index >= countField Then
                Throw New ArgumentOutOfRangeException(NameOf(index))
            End If

            System.Array.Copy(array, index + 1, array, index, countField - index - 1)
            countField -= 1
            array(countField) = Nothing
        End Sub

        ''' <summary>
        ''' Returns a string representation of this repeated field, in the same
        ''' way as it would be represented by the default JSON formatter.
        ''' </summary>
        Public Overrides Function ToString() As String
            Dim writer = New StringWriter()
            JsonFormatter.Default.WriteList(writer, Me)
            Return writer.ToString()
        End Function

        ''' <summary>
        ''' Gets or sets the item at the specified index.
        ''' </summary>
        ''' <value>
        ''' The element at the specified index.
        ''' </value>
        ''' <param name="index">The zero-based index of the element to get or set.</param>
        ''' <returns>The item at the specified index.</returns>
        Default Public Overloads Property Item(index As Integer) As T Implements IList(Of T).Item
            Get

                If index < 0 OrElse index >= countField Then
                    Throw New ArgumentOutOfRangeException(NameOf(index))
                End If

                Return array(index)
            End Get
            Set(value As T)

                If index < 0 OrElse index >= countField Then
                    Throw New ArgumentOutOfRangeException(NameOf(index))
                End If

                CheckNotNullUnconstrained(value, NameOf(value))
                array(index) = value
            End Set
        End Property

#Region "Explicit interface implementation for IList and ICollection."
        Private ReadOnly Property IsFixedSize As Boolean Implements IList.IsFixedSize
            Get
                Return False
            End Get
        End Property

        Private Sub CopyTo1(array As Array, index As Integer) Implements ICollection.CopyTo
            Array.Copy(Me.array, 0, array, index, countField)
        End Sub

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

        Private Property Item1(index As Integer) As Object Implements IList.Item
            Get
                Return Me(index)
            End Get
            Set(value As Object)
                Me(index) = CType(value, T)
            End Set
        End Property

        Private Function Add1(value As Object) As Integer Implements IList.Add
            Add(CType(value, T))
            Return countField - 1
        End Function

        Private Function Contains1(value As Object) As Boolean Implements IList.Contains
            Return TypeOf value Is T AndAlso Contains(value)
        End Function

        Private Function IndexOf1(value As Object) As Integer Implements IList.IndexOf
            If Not (TypeOf value Is T) Then
                Return -1
            End If

            Return IndexOf(value)
        End Function

        Private Sub Insert1(index As Integer, value As Object) Implements IList.Insert
            Insert(index, value)
        End Sub

        Private Sub Remove1(value As Object) Implements IList.Remove
            If Not (TypeOf value Is T) Then
                Return
            End If

            Remove(value)
        End Sub
#End Region
    End Class
End Namespace

