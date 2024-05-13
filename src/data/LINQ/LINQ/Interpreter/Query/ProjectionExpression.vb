#Region "Microsoft.VisualBasic::e964dd6164ddb0aa2e34cf054fdfb15b, src\data\LINQ\LINQ\Interpreter\Query\ProjectionExpression.vb"

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

    '   Total Lines: 71
    '    Code Lines: 45
    ' Comment Lines: 12
    '   Blank Lines: 14
    '     File Size: 2.47 KB


    '     Class ProjectionExpression
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: Exec
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Interpreter.Expressions
Imports LINQ.Runtime
Imports Microsoft.VisualBasic.My.JavaScript

Namespace Interpreter.Query

    ''' <summary>
    ''' from ... select ...
    ''' </summary>
    Public Class ProjectionExpression : Inherits QueryExpression

        Dim opt As Options
        Dim project As OutputProjection

        Sub New(symbol As SymbolDeclare, sequence As Expression, exec As IEnumerable(Of Expression), proj As OutputProjection, opt As Options)
            Call MyBase.New(symbol, sequence, exec)

            Me.opt = opt
            Me.project = proj
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="context"></param>
        ''' <returns>
        ''' array of <see cref="JavaScriptObject"/>
        ''' </returns>
        Public Overrides Function Exec(context As ExecutableContext) As Object
            Dim projections As New List(Of JavaScriptObject)
            Dim env As Environment = context.env
            Dim closure As New ExecutableContext With {
                .env = New Environment(parent:=env),
                .throwError = context.throwError
            }

            Call LoadDrivers(context)

            Dim skipVal As Boolean
            Dim dataset As DataSet = GetDataSet(context)

            Call closure.env.AddSymbol(symbol.symbolName, symbol.type)

            For Each item As Object In dataset.PopulatesData()
                closure.env.FindSymbol(symbol.symbolName).value = item

                For Each line As Expression In executeQueue
                    If TypeOf line Is WhereFilter Then
                        skipVal = Not DirectCast(line.Exec(closure), Boolean)

                        If skipVal Then
                            Exit For
                        End If
                    End If
                Next

                If Not skipVal Then
                    If project Is Nothing Then
                        ' 当SELECT投影操作为空的时候，默认不进行投影操作
                        ' 即返回最原始的变量值
                        projections.Add(item)
                    Else
                        projections.Add(project.Exec(closure))
                    End If
                End If
            Next

            Return opt.RunOptionPipeline(projections, context).ToArray
        End Function
    End Class
End Namespace
