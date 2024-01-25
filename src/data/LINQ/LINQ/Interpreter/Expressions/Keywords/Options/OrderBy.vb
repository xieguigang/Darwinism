﻿#Region "Microsoft.VisualBasic::900f73fea2c7e819ef4250efb84e3790, LINQ\LINQ\Interpreter\Expressions\Keywords\Options\OrderBy.vb"

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

    '     Class OrderBy
    ' 
    '         Properties: keyword
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: DoOrder, (+2 Overloads) Exec, GetOrderKey, ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Runtime
Imports Microsoft.VisualBasic.My.JavaScript

Namespace Interpreter.Expressions

    Public Class OrderBy : Inherits PipelineKeyword

        Public Overrides ReadOnly Property keyword As String
            Get
                Return "Order By"
            End Get
        End Property

        Dim key As Expression
        Dim desc As Boolean

        Sub New(key As Expression, desc As Boolean)
            Me.key = FixLiteral(key)
            Me.desc = desc
        End Sub

        Public Overrides Function Exec(context As ExecutableContext) As Object
            Return key.Exec(context)
        End Function

        Private Function GetOrderKey(obj As JavaScriptObject, context As ExecutableContext) As Object
            Dim env As Environment = context.env

            For Each key As String In obj
                env.FindSymbol(key).value = obj(key)
            Next

            Return Exec(context)
        End Function

        Public Overrides Function Exec(result As IEnumerable(Of JavaScriptObject), context As ExecutableContext) As IEnumerable(Of JavaScriptObject)
            Dim raw As JavaScriptObject() = result.ToArray
            Dim keys As Object()
            Dim i As Integer()

            If TypeOf key Is Literals Then
                Dim keyName As String = key.Exec(Nothing)

                keys = raw.Select(Function(obj) obj(keyName)).ToArray
            Else
                keys = raw _
                    .Select(Function(obj)
                                Return GetOrderKey(obj, context)
                            End Function) _
                    .ToArray
            End If

            If keys.All(Function(xi) xi.GetType Is GetType(Double)) Then
                i = DoOrder(keys.Select(Function(k) DirectCast(k, Double)))
            ElseIf keys.All(Function(xi) xi.GetType Is GetType(Integer)) Then
                i = DoOrder(keys.Select(Function(k) DirectCast(k, Integer)))
            ElseIf keys.All(Function(xi) xi.GetType Is GetType(String)) Then
                i = DoOrder(keys.Select(Function(k) DirectCast(k, String)))
            Else
                Throw New NotImplementedException
            End If

            Return i.Select(Function(index) raw(index))
        End Function

        Private Function DoOrder(Of T As IComparable(Of T))(keys As IEnumerable(Of T)) As Integer()
            If desc Then
                Return keys.Select(Function(key, i) (key, i)).OrderByDescending(Function(ti) ti.key).Select(Function(ti) ti.i).ToArray
            Else
                Return keys.Select(Function(key, i) (key, i)).OrderBy(Function(ti) ti.key).Select(Function(ti) ti.i).ToArray
            End If
        End Function

        Public Overrides Function ToString() As String
            Return $"order by {key}"
        End Function
    End Class
End Namespace
