#Region "Microsoft.VisualBasic::372d05b8a555446612c0ffe7bc8736ef, src\data\LINQ\LINQ\Interpreter\QueryInterpreter.vb"

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

    '   Total Lines: 27
    '    Code Lines: 18 (66.67%)
    ' Comment Lines: 3 (11.11%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 6 (22.22%)
    '     File Size: 860 B


    '     Class QueryInterpreter
    ' 
    '         Properties: env
    ' 
    '         Function: Exec
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Interpreter.Query
Imports LINQ.Language
Imports LINQ.Runtime
Imports LINQ.Script
Imports Microsoft.VisualBasic.My.JavaScript

Namespace Interpreter

    ''' <summary>
    ''' interpreter of the linq query expression
    ''' </summary>
    Public Class QueryInterpreter

        Public Property env As Environment

        Public Function Exec(query_str As String) As Object
            Dim tokens As Token() = LINQ.Language.GetTokens(query_str).ToArray
            Dim query As ProjectionExpression = tokens.PopulateQueryExpression
            Dim env As New GlobalEnvironment(New Registry)
            Dim context As New ExecutableContext With {.env = env, .throwError = True}
            Dim result As JavaScriptObject() = query.Exec(context)

            Return result
        End Function

    End Class
End Namespace
