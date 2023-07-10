﻿#Region "Microsoft.VisualBasic::4a78e6e2ba51853e82ea495ba548192c, LINQ\RQL\StorageTek\EntityProvider.vb"

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

    '     Class EntityProvider
    ' 
    '         Properties: MapFileIO, Tek
    ' 
    '         Constructor: (+2 Overloads) Sub New
    '         Function: __internalRepository, GetRepository, LinqWhere, ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports sciBASIC.ComputingServices.Linq.Framework.DynamicCode
Imports sciBASIC.ComputingServices.Linq.Framework.Provider
Imports sciBASIC.ComputingServices.Linq.LDM.Statements.Tokens
Imports sciBASIC.ComputingServices.Linq.LDM.Statements.Tokens.WhereClosure

Namespace StorageTek

    ''' <summary>
    ''' 实体对象，实际上这个模块最主要的功能就是提供数据源的读取方法
    ''' </summary>
    Public Class EntityProvider : Inherits TypeEntry

        ''' <summary>
        ''' 存储的方法
        ''' </summary>
        ''' <returns></returns>
        Public Property Tek As StorageTeks
        ''' <summary>
        ''' 映射的实际的存储位置
        ''' </summary>
        ''' <returns></returns>
        Public Property MapFileIO As String

        Sub New()
        End Sub

        Sub New(Linq As TypeEntry, res As String)
            Call MyBase.New(Linq)
            Tek = StorageTeks.Linq
            MapFileIO = res
        End Sub

        Public Function GetRepository() As IEnumerable
            If Tek = StorageTeks.Linq Then  ' 使用的是Linq数据源
                Dim hwnd As GetLinqResource = Me.GetHandle
                Return hwnd(MapFileIO)
            Else
                Return __internalRepository()
            End If
        End Function

        ''' <summary>
        ''' 系统的自有的数据源方法
        ''' </summary>
        ''' <returns></returns>
        Private Function __internalRepository() As IEnumerable
            Dim api As IRepository = StorageTek.API.InternalAPIs(Tek)  ' 在这里是系统的自有的数据源方法
            Dim type As Type = Me.TypeId.GetType  ' 得到元素类型的信息
            Dim source As IEnumerable = api(MapFileIO, type)
            Return source
        End Function

        Public Overrides Function ToString() As String
            Return $"[{Tek.ToString}] {MapFileIO}  //{MyBase.ToString}"
        End Function

        Public Function LinqWhere(where As String, compiler As DynamicCompiler) As IEnumerable
            Dim type As Type = Me.GetType
            Dim test As Predicate(Of Object) = WhereClosure.CreateLinqWhere(where, type, compiler)
            Dim LQuery = (From x As Object In GetRepository() Where True = test(x) Select x)
            Return LQuery
        End Function
    End Class
End Namespace
