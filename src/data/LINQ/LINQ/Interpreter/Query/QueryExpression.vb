#Region "Microsoft.VisualBasic::31b1d517569d8bac88bfda23087c33ad, src\data\LINQ\LINQ\Interpreter\Query\QueryExpression.vb"

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

    '   Total Lines: 48
    '    Code Lines: 37 (77.08%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 11 (22.92%)
    '     File Size: 1.74 KB


    '     Class QueryExpression
    ' 
    '         Properties: IsURISource
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    '         Function: AddAttachDrivers, GetDataSet, GetSeqValue
    ' 
    '         Sub: LoadDrivers
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Interpreter.Expressions
Imports LINQ.Runtime

Namespace Interpreter.Query

    Public MustInherit Class QueryExpression : Inherits Expression

        ReadOnly sequence As Expression
        ReadOnly attaches As New List(Of ImportDataDriver)

        Friend ReadOnly symbol As SymbolDeclare
        Friend ReadOnly executeQueue As Expression()

        Public ReadOnly Property IsURISource As Boolean
            Get
                Return TypeOf sequence Is Literals AndAlso DirectCast(sequence, Literals).type = GetType(String)
            End Get
        End Property

        Sub New(symbol As SymbolDeclare, sequence As Expression, execQueue As IEnumerable(Of Expression))
            Me.symbol = symbol
            Me.sequence = sequence
            Me.executeQueue = execQueue.ToArray
        End Sub

        Protected Sub LoadDrivers(context As ExecutableContext)
            Dim globalEnv As GlobalEnvironment = context.env.GlobalEnvir
            Dim registry As Registry = globalEnv.registry

            For Each name As ImportDataDriver In attaches
                Call registry.Register(driverDll:=name.Exec(context))
            Next
        End Sub

        Public Function AddAttachDrivers(drivers As IEnumerable(Of ImportDataDriver)) As QueryExpression
            attaches.AddRange(drivers)
            Return Me
        End Function

        Public Function GetSeqValue(context As ExecutableContext) As Object
            Return sequence.Exec(context)
        End Function

        Protected Function GetDataSet(context As ExecutableContext) As DataSet
            Return DataSet.CreateDataSet(Me, context)
        End Function

        ''' <summary>
        ''' 最近一次执行查询的时候，索引的使用情况（用于测试以及性能诊断）
        ''' </summary>
        ''' <returns></returns>
        Public Property LastIndex As IndexQueryResult

        ''' <summary>
        ''' 将当前查询之中的全部``WHERE``条件翻译为索引查询条件
        ''' </summary>
        ''' <returns>
        ''' 当存在无法使用索引的查询条件的时候，函数返回<c>Nothing</c>，此时应当回退为全表扫描。
        ''' </returns>
        Protected Function GetIndexQueries() As LINQ.Query()
            Dim queries As New List(Of LINQ.Query)

            For Each line As Expression In executeQueue
                If TypeOf line Is WhereFilter Then
                    Dim q As LINQ.Query() = DirectCast(line, WhereFilter).GetIndexQuery(symbol.symbolName)

                    If q Is Nothing Then
                        Return Nothing
                    Else
                        queries.AddRange(q)
                    End If
                End If
            Next

            If queries.Count = 0 Then
                Return Nothing
            Else
                Return queries.ToArray
            End If
        End Function
    End Class
End Namespace
