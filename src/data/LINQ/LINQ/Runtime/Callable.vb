#Region "Microsoft.VisualBasic::a9f1ea206fbb3b8cc0b9967f6b41945c, src\data\LINQ\LINQ\Runtime\Callable.vb"

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
    '    Code Lines: 38
    ' Comment Lines: 0
    '   Blank Lines: 10
    '     File Size: 1.40 KB


    '     Class Callable
    ' 
    '         Properties: name
    ' 
    '         Constructor: (+3 Overloads) Sub New
    '         Function: Evaluate
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Data
Imports System.Reflection

Namespace Runtime

    Public Class Callable

        Dim method As MethodInfo
        Dim parameters As ParameterInfo()

        Public ReadOnly Property name As String
            Get
                Return method.Name
            End Get
        End Property

        Sub New(method As MethodInfo)
            Me.method = method
            Me.parameters = method.GetParameters
        End Sub

        Sub New(math1 As Func(Of Double, Double))
            Call Me.New(math1.Method)
        End Sub

        Sub New(math2 As Func(Of Double, Double, Double))
            Call Me.New(math2.Method)
        End Sub

        Public Function Evaluate(params As Object()) As Object
            Dim args As New List(Of Object)

            For i As Integer = 0 To parameters.Length - 1
                If i >= params.Length Then
                    If parameters(i).IsOptional Then
                        args.Add(parameters(i).DefaultValue)
                    Else
                        Throw New InvalidExpressionException
                    End If
                Else
                    args.Add(CTypeDynamic(params(i), parameters(i).ParameterType))
                End If
            Next

            Return method.Invoke(Nothing, args.ToArray)
        End Function
    End Class
End Namespace
