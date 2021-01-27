#Region "Microsoft.VisualBasic::e018351fed63ad4d82247c482f3a151a, LINQ\LINQ\Interpreter\Query\DataSoruce.vb"

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

    '     Class DataSet
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: CreateDataSet
    ' 
    '     Class SymbolIterator
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: PopulatesData
    ' 
    '     Class URIIteratorDriver
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: PopulatesData
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Interpreter.Expressions
Imports LINQ.Runtime
Imports Microsoft.VisualBasic.Emit.Delegates

Namespace Interpreter.Query

    Public MustInherit Class DataSet

        Protected ReadOnly env As Environment
        Protected ReadOnly symbolDeclare As SymbolDeclare

        Sub New(symbolDeclare As SymbolDeclare, env As Environment)
            Me.env = env
            Me.symbolDeclare = symbolDeclare
        End Sub

        Public MustOverride Function PopulatesData() As IEnumerable(Of Object)

        Public Shared Function CreateDataSet(query As QueryExpression, context As ExecutableContext) As DataSet
            Dim env As Environment = context.env

            If query.IsURISource Then
                Return New URIIteratorDriver(query, query.GetSeqValue(Nothing), env)
            Else
                Dim seqVal As Object = query.GetSeqValue(context)

                If TypeOf seqVal Is String Then
                    Return New URIIteratorDriver(query, seqVal, env)
                Else
                    Return New SymbolIterator(query, seqVal, env)
                End If
            End If
        End Function

    End Class

    Public Class SymbolIterator : Inherits DataSet

        ReadOnly objVal As Object

        Sub New(query As QueryExpression, obj As Object, env As Environment)
            MyBase.New(query.symbol, env)
            Me.objVal = obj
        End Sub

        Public Overrides Iterator Function PopulatesData() As IEnumerable(Of Object)
            If objVal Is Nothing Then
                Throw New NullReferenceException
            End If

            If objVal.GetType.IsArray Then
                With DirectCast(objVal, Array)
                    For i As Integer = 0 To .Length - 1
                        Yield .GetValue(i)
                    Next
                End With
            ElseIf objVal.GetType.ImplementInterface(GetType(IEnumerable)) Then
                For Each item As Object In DirectCast(objVal, IEnumerable)
                    Yield item
                Next
            Else
                Throw New InvalidCastException
            End If
        End Function
    End Class

    Public Class URIIteratorDriver : Inherits DataSet

        ReadOnly uri As String

        Sub New(query As QueryExpression, uri As String, env As Environment)
            MyBase.New(query.symbol, env)
            Me.uri = uri
        End Sub

        Public Overrides Iterator Function PopulatesData() As IEnumerable(Of Object)
            Dim driver As DataSourceDriver = env.GlobalEnvir.GetDriverByCode(symbolDeclare.type)

            For Each item As Object In driver.ReadFromUri(uri)
                Yield item
            Next
        End Function
    End Class
End Namespace
