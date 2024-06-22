#Region "Microsoft.VisualBasic::e95b64c8e9d125580b915d23c2e125c0, src\data\LINQ\LINQ\Runtime\Indexing\RangeIndex.vb"

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

    '   Total Lines: 145
    '    Code Lines: 116 (80.00%)
    ' Comment Lines: 4 (2.76%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 25 (17.24%)
    '     File Size: 4.63 KB


    ' Class RangeIndex
    ' 
    '     Properties: UnderlyingType
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: IndexData, (+2 Overloads) Search, SearchGreaterThan, SearchLessThan
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.ComponentModel.Algorithm
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math

''' <summary>
''' in-memory index of the numeric value
''' </summary>
''' <typeparam name="T"></typeparam>
Public Class RangeIndex(Of T) : Inherits ValueIndex

    Dim index As BlockSearchFunction(Of SeqValue(Of T))
    Dim eval As Func(Of T, Double)
    Dim tolerance As Double
    Dim doubles As Double()

    Public Overrides ReadOnly Property UnderlyingType As Type
        Get
            Return GetType(T)
        End Get
    End Property

    Sub New(eval As Func(Of T, Double))
        Me.eval = eval
    End Sub

    Public Function IndexData(data As IEnumerable(Of T)) As RangeIndex(Of T)
        Dim pool = data.Select(Function(xi, i) New SeqValue(Of T)(i, xi)).ToArray
        Dim x As Double() = (From xi As SeqValue(Of T) In pool Select eval(xi.value)).ToArray
        Dim diff As Double() = x.OrderBy(Function(a) a) _
            .Split(x.Length / 1000) _
            .AsParallel _
            .Select(Function(a) a.Max - a.Min) _
            .ToArray
        Dim win_size As Double = diff.Average * 1.125

        tolerance = win_size
        doubles = x
        index = New BlockSearchFunction(Of SeqValue(Of T))(
            data:=pool,
            eval:=Function(i) eval(i.value),
            tolerance:=win_size,
            fuzzy:=True
        )

        Return Me
    End Function

    Public Iterator Function SearchLessThan(x As T) As IEnumerable(Of IAddressOf)
        Dim right_d As Double = eval(x)
        Dim right = index.GetOffset(New SeqValue(Of T)(x))

        If right < 0 Then
            Return
        End If

        For i As Integer = 0 To right
            If i = right Then
                For Each item As SeqValue(Of T) In index.GetBlock(i)
                    Dim xi As Double = eval(item.value)

                    If xi <= right_d Then
                        Yield item
                    End If
                Next
            Else
                For Each item As SeqValue(Of T) In index.GetBlock(i)
                    Yield item
                Next
            End If
        Next
    End Function

    Public Iterator Function SearchGreaterThan(x As T) As IEnumerable(Of IAddressOf)
        Dim left_d As Double = eval(x)
        Dim left = index.GetOffset(New SeqValue(Of T)(x))

        If left < 0 Then
            left = 0
        End If

        For i As Integer = left To index.numBlocks - 1
            If i = left Then
                For Each item As SeqValue(Of T) In index.GetBlock(i)
                    Dim xi As Double = eval(item.value)

                    If xi >= left_d Then
                        Yield item
                    End If
                Next
            Else
                For Each item As SeqValue(Of T) In index.GetBlock(i)
                    Yield item
                Next
            End If
        Next
    End Function

    Public Iterator Function Search(min As T, max As T) As IEnumerable(Of IAddressOf)
        Dim max_d As Double = eval(max)
        Dim min_d As Double = eval(min)
        Dim window As Double = max_d - min_d

        If window < 0 Then
            Return
        End If

        Dim left = index.GetOffset(New SeqValue(Of T)(min))
        Dim right = index.GetOffset(New SeqValue(Of T)(max))

        If left < 0 Then left = 0
        If right < 0 Then right = index.numBlocks - 1

        For i As Integer = left To right
            Dim checkBoundary As Boolean = i = left OrElse i = right

            If checkBoundary Then
                For Each item As SeqValue(Of T) In index.GetBlock(i)
                    Dim xi As Double = eval(item.value)

                    If xi >= min_d AndAlso xi <= max_d Then
                        Yield item
                    End If
                Next
            Else
                For Each item As SeqValue(Of T) In index.GetBlock(i)
                    Yield item
                Next
            End If
        Next
    End Function

    Public Iterator Function Search(x As T) As IEnumerable(Of IAddressOf)
        Dim q = index.Search(New SeqValue(Of T)(x), tolerance)
        Dim target As Double = eval(x)

        For Each item As SeqValue(Of T) In q
            If target = doubles(item.i) Then
                Yield item
            End If
        Next
    End Function

End Class
