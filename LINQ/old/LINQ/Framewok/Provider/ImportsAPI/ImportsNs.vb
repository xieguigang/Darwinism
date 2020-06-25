#Region "Microsoft.VisualBasic::851c9f276750fc960f4d2e9a94cbb422, LINQ\LINQ\Framewok\Provider\ImportsAPI\ImportsNs.vb"

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

    '     Class ImportsNs
    ' 
    '         Properties: Modules
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '         Function: Remove
    ' 
    '         Sub: Add
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Extensions
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Scripting.MetaData

Namespace Framework.Provider.ImportsAPI

    ''' <summary>
    ''' 导入的命名空间
    ''' </summary>
    Public Class ImportsNs : Inherits PackageAttribute

        ''' <summary>
        ''' {namespace, typeinfo}
        ''' </summary>
        ''' <returns></returns>
        Public Property Modules As TypeInfo()
            Get
                Return __list.ToArray
            End Get
            Set(value As TypeInfo())
                If value Is Nothing Then
                    __list = New List(Of TypeInfo)
                Else
                    __list = New List(Of TypeInfo)(value)
                End If
            End Set
        End Property

        Dim __list As New List(Of TypeInfo)

        Sub New()
        End Sub

        Sub New(base As PackageAttribute)
            Call MyBase.New(base)
        End Sub

        Public Sub Add(type As Type)
            Dim info As New TypeInfo(type)
            Call __list.Add(info)
        End Sub

        Public Function Remove(type As Type) As Boolean
            Dim LQuery = (From x In __list Where x = type Select x).ToArray
            For Each x In LQuery
                Call __list.Remove(x)
            Next

            Return Not LQuery.IsNullOrEmpty
        End Function
    End Class
End Namespace
