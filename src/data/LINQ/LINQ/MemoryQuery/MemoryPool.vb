﻿#Region "Microsoft.VisualBasic::db0843e8b47d8234cebb46c6870c9ed6, src\data\LINQ\LINQ\MemoryQuery\MemoryPool.vb"

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

    '   Total Lines: 101
    '    Code Lines: 62 (61.39%)
    ' Comment Lines: 20 (19.80%)
    '    - Xml Docs: 45.00%
    ' 
    '   Blank Lines: 19 (18.81%)
    '     File Size: 3.42 KB


    ' Class MemoryPool
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: CheckScalar, GetData, Query
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Reflection
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Emit.Delegates
Imports Microsoft.VisualBasic.Scripting

''' <summary>
''' index of the general clr object array.
''' </summary>
Public Class MemoryPool : Inherits MemoryIndex

    ReadOnly vector As DataObjectVector
    ReadOnly sub_vector As New Dictionary(Of String, DataObjectVector)

    Sub New(data As Array, Optional [property] As String = Nothing)
        vector = New DataObjectVector(data)

        If Not [property].StringEmpty() Then
            ' index via the nested data property
            ' not the raw vector
            ' the query returns result use the raw vector
            vector = New DataObjectVector(DirectCast(vector(name:=[property]), Array))
        End If
    End Sub

    Protected Overrides Function CheckScalar(field As String) As Boolean
        Dim prop As PropertyInfo

        If field.Contains("."c) Then
            ' a.b.c get from the clr object which
            ' comes from the property value.
            ' a is root
            ' b is path
            ' c is the data property
            Dim path As String() = field.Split("."c)
            Dim subvec As DataObjectVector = sub_vector _
                .ComputeIfAbsent(path(0), lazyValue:=Function(name) vector.GetSubVector(name))
            Dim visit As New List(Of String) From {path(0)}

            ' -2 means we needs to ignores of the data property
            For i As Integer = 1 To path.Length - 2
                Dim name As String = path(i)

                visit.Add(name)
                subvec = sub_vector _
                    .ComputeIfAbsent(visit.JoinBy("."),
                                     lazyValue:=Function()
                                                    Return subvec.GetSubVector(name)
                                                End Function)
            Next

            ' get property for read the data 
            prop = subvec.GetProperty(path.Last)
        Else
            prop = vector.GetProperty(field)
        End If

        Dim is_scalar As Boolean = Not prop.PropertyType.IsArray
        Return is_scalar
    End Function

    Protected Overrides Function GetData(Of V)(field As String) As V()
        Dim prop As PropertyInfo
        Dim vector As DataObjectVector = Me.vector

        If field.Contains("."c) Then
            Dim t = field.Split("."c)
            Dim path As String = t.Take(t.Length - 1).JoinBy(".")

            vector = sub_vector(path)
            field = t.Last
        End If

        prop = vector.GetProperty(field)

        If prop.PropertyType Is GetType(V) Then
            Return vector(field)
        Else
            Dim pull As Array = vector(field)
            Dim cast As V() = pull.CTypeDynamic(GetType(V))

            Return cast
        End If
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="filter"></param>
    ''' <returns>
    ''' this function will returns nothing if query filter has no result
    ''' </returns>
    Public Function Query(filter As IEnumerable(Of Query)) As Array
        Dim index As Integer() = GetIndex(filter)

        If index.IsNullOrEmpty Then
            Return Nothing
        End If

        Return vector(index)
    End Function
End Class
