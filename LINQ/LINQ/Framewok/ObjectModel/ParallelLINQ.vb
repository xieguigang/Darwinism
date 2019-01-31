#Region "Microsoft.VisualBasic::b5ed01fb96f3c5ce71119ef496ea8f97, LINQ\LINQ\Framewok\ObjectModel\ParallelLINQ.vb"

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

    '     Class ParallelLinq
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: EXEC
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports sciBASIC.ComputingServices.Linq.Framework.DynamicCode
Imports sciBASIC.ComputingServices.Linq.LDM.Statements
Imports sciBASIC.ComputingServices.Linq.Script

Namespace Framework.ObjectModel

    ''' <summary>
    ''' 并行LINQ查询表达式的对象模型
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ParallelLinq : Inherits Linq

        Sub New(Expr As LinqStatement, FrameworkRuntime As DynamicsRuntime)
            Call MyBase.New(Expr, Runtime:=FrameworkRuntime)
        End Sub

        Public Overrides Function EXEC() As IEnumerable
            Dim Linq = (From x As Object In __getSource.AsParallel
                        Let value As LinqValue = __project(x)
                        Where value.IsTrue
                        Select value.Projects)
            Return Linq
        End Function
    End Class
End Namespace
