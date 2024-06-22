#Region "Microsoft.VisualBasic::8af4711d352fde1006be4a159d0cbe40, src\data\LINQ\LINQ\Interpreter\Query\DataSoruce\DataSoruce.vb"

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

    '   Total Lines: 34
    '    Code Lines: 26 (76.47%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 8 (23.53%)
    '     File Size: 1.16 KB


    '     Class DataSet
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: CreateDataSet
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Interpreter.Expressions
Imports LINQ.Runtime

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
End Namespace
