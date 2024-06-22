#Region "Microsoft.VisualBasic::d0236fa546c5bac748ce7ae6d87fa037, src\message\Google.Protobuf\Collections\ReadOnlyDictionary.vb"

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

    '   Total Lines: 138
    '    Code Lines: 82 (59.42%)
    ' Comment Lines: 32 (23.19%)
    '    - Xml Docs: 9.38%
    ' 
    '   Blank Lines: 24 (17.39%)
    '     File Size: 5.95 KB


    '     Class ReadOnlyDictionary
    ' 
    '         Properties: Count, IsReadOnly, KeysProp, ValuesProp
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    '         Function: Contains, ContainsKey, Equals, GetEnumerator, GetEnumerator1
    '                   GetHashCode, (+2 Overloads) Remove, ToString, TryGetValue
    ' 
    '         Sub: (+2 Overloads) Add, Clear, CopyTo
    ' 
    ' 
    ' /********************************************************************************/

#End Region

#Region "Copyright notice and license"
' Protocol Buffers - Google's data interchange format
' Copyright 2008 Google Inc.  All rights reserved.
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

Imports System.Runtime.InteropServices

Namespace Google.Protobuf.Collections

    ''' <summary>
    ''' Read-only wrapper around another dictionary.
    ''' </summary>
    Friend NotInheritable Class ReadOnlyDictionary(Of TKey, TValue)
        Implements IDictionary(Of TKey, TValue)

        Private ReadOnly wrapped As IDictionary(Of TKey, TValue)

        Public Sub New(wrapped As IDictionary(Of TKey, TValue))
            Me.wrapped = wrapped
        End Sub

        Public Sub Add(key As TKey, value As TValue) Implements IDictionary(Of TKey, TValue).Add
            Throw New InvalidOperationException()
        End Sub

        Public Function ContainsKey(key As TKey) As Boolean Implements IDictionary(Of TKey, TValue).ContainsKey
            Return wrapped.ContainsKey(key)
        End Function

        Public ReadOnly Property KeysProp As ICollection(Of TKey) Implements IDictionary(Of TKey, TValue).Keys
            Get
                Return wrapped.Keys
            End Get
        End Property

        Public Function Remove(key As TKey) As Boolean Implements IDictionary(Of TKey, TValue).Remove
            Throw New InvalidOperationException()
        End Function

        Public Function TryGetValue(key As TKey, <Out> ByRef value As TValue) As Boolean Implements IDictionary(Of TKey, TValue).TryGetValue
            Return wrapped.TryGetValue(key, value)
        End Function

        Public ReadOnly Property ValuesProp As ICollection(Of TValue) Implements IDictionary(Of TKey, TValue).Values
            Get
                Return wrapped.Values
            End Get
        End Property

        Default Public Property Item(key As TKey) As TValue Implements IDictionary(Of TKey, TValue).Item
            Get
                Return wrapped(key)
            End Get
            Set(value As TValue)
                Throw New InvalidOperationException()
            End Set
        End Property

        Public Sub Add(item As KeyValuePair(Of TKey, TValue)) Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Add
            Throw New InvalidOperationException()
        End Sub

        Public Sub Clear() Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Clear
            Throw New InvalidOperationException()
        End Sub

        Public Function Contains(item As KeyValuePair(Of TKey, TValue)) As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Contains
            Return wrapped.Contains(item)
        End Function

        Public Sub CopyTo(array As KeyValuePair(Of TKey, TValue)(), arrayIndex As Integer) Implements ICollection(Of KeyValuePair(Of TKey, TValue)).CopyTo
            wrapped.CopyTo(array, arrayIndex)
        End Sub

        Public ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Count
            Get
                Return wrapped.Count
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).IsReadOnly
            Get
                Return True
            End Get
        End Property

        Public Function Remove(item As KeyValuePair(Of TKey, TValue)) As Boolean Implements ICollection(Of KeyValuePair(Of TKey, TValue)).Remove
            Throw New InvalidOperationException()
        End Function

        Public Function GetEnumerator() As IEnumerator(Of KeyValuePair(Of TKey, TValue)) Implements IEnumerable(Of KeyValuePair(Of TKey, TValue)).GetEnumerator
            Return wrapped.GetEnumerator()
        End Function

        Private Function GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
            Return CType(wrapped, IEnumerable).GetEnumerator()
        End Function

        Public Overrides Function Equals(obj As Object) As Boolean
            Return wrapped.Equals(obj)
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return wrapped.GetHashCode()
        End Function

        Public Overrides Function ToString() As String
            Return wrapped.ToString()
        End Function
    End Class
End Namespace
