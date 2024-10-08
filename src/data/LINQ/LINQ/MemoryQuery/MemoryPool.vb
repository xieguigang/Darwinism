﻿#Region "Microsoft.VisualBasic::ec2461772164362b3eb3be10317cf150, src\data\LINQ\LINQ\MemoryQuery\MemoryPool.vb"

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

    '   Total Lines: 116
    '    Code Lines: 69 (59.48%)
    ' Comment Lines: 26 (22.41%)
    '    - Xml Docs: 80.77%
    ' 
    '   Blank Lines: 21 (18.10%)
    '     File Size: 3.46 KB


    ' Class MemoryPool
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: CheckScalar, GetData, Query
    ' 
    ' Class MemoryPool
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: CheckScalar, GetData, Query
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Reflection
Imports Microsoft.VisualBasic.Emit.Delegates
Imports Microsoft.VisualBasic.Scripting

''' <summary>
''' index of the general clr object array.
''' </summary>
Public Class MemoryPool : Inherits MemoryIndex

    ReadOnly vector As DataObjectVector
    ''' <summary>
    ''' the raw input pool data
    ''' </summary>
    ReadOnly pool As Array

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
        Dim prop As PropertyInfo = vector.GetProperty(field)
        Dim is_scalar As Boolean = Not prop.PropertyType.IsArray
        Return is_scalar
    End Function

    Protected Overrides Function GetData(Of V)(field As String) As V()
        Dim prop As PropertyInfo = vector.GetProperty(field)

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
    Public Function Query(filter As IEnumerable(Of Query)) As Array()
        Dim index As Integer() = GetIndex(filter)

        If index.IsNullOrEmpty Then
            Return Nothing
        End If

        Return index _
            .Select(Function(i) pool(i)) _
            .ToArray
    End Function
End Class

''' <summary>
''' Index of the generic clr object
''' </summary>
Public Class MemoryPool(Of T) : Inherits MemoryIndex

    ReadOnly pool As T()
    ReadOnly vector As DataValue(Of T)

    Sub New(data As T())
        pool = data
        vector = New DataValue(Of T)(data)
    End Sub

    Protected Overrides Function GetData(Of V)(field As String) As V()
        Dim prop As PropertyInfo = vector.GetProperty(field)

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
    Public Function Query(filter As IEnumerable(Of Query)) As T()
        Dim index As Integer() = GetIndex(filter)

        If index.IsNullOrEmpty Then
            Return Nothing
        End If

        Return index _
            .Select(Function(i) pool(i)) _
            .ToArray
    End Function

    Protected Overrides Function CheckScalar(field As String) As Boolean
        Dim prop As PropertyInfo = vector.GetProperty(field)
        Dim is_scalar As Boolean = Not prop.PropertyType.IsArray
        Return is_scalar
    End Function
End Class

