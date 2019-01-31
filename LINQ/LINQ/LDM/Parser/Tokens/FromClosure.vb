#Region "Microsoft.VisualBasic::4a4845aad3415e9723db3d6fecf21094, LINQ\LINQ\LDM\Parser\Tokens\FromClosure.vb"

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

    '     Class FromClosure
    ' 
    '         Properties: Name, TypeId
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: [GetType], GetEntityRepository, ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports sciBASIC.ComputingServices.Linq.Framework
Imports sciBASIC.ComputingServices.Linq.Framework.DynamicCode
Imports sciBASIC.ComputingServices.Linq.Framework.Provider

Namespace LDM.Statements.Tokens

    ''' <summary>
    ''' The init variable.
    ''' </summary>
    Public Class FromClosure : Inherits Closure

        ''' <summary>
        ''' 变量的名称
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Name As String
        ''' <summary>
        ''' 变量的类型标识符
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TypeId As String

        Sub New(tokens As ClosureTokens(), parent As LinqStatement)
            Call MyBase.New(TokenIcer.Tokens.From, tokens, parent)

            Name = Source.Tokens(Scan0).Value
            TypeId = Source.Tokens(2).Value
        End Sub

        Public Overloads Function [GetType](defs As TypeRegistry) As Type
            Dim value = defs.Find(TypeId)
            If value Is Nothing Then
                Return Scripting.GetType(TypeId)
            Else
                Dim type As Type = value.GetType

                If Not type Is Nothing Then
                    Return type
                Else
                    Return value.GetType
                End If
            End If
        End Function

        Public Function GetEntityRepository(defs As TypeRegistry) As Provider.GetLinqResource
            Dim handle As GetLinqResource = defs.GetHandle(TypeId)
            If handle Is Nothing Then
                Throw New Exception
            Else
                Return handle
            End If
        End Function

        Public Overrides Function ToString() As String
            Return String.Format("Dim {0} As {1}", Name, TypeId)
        End Function
    End Class
End Namespace
