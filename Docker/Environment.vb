#Region "Microsoft.VisualBasic::f5ecc954d263574a6d558fd24cd8933f, Docker\Environment.vb"

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

    ' Class Environment
    ' 
    '     Properties: [Shared], container
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: GetDockerCommand, (+2 Overloads) Mount
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Text
Imports Darwinism.Docker.Arguments

''' <summary>
''' The container environment module
''' </summary>
Public Class Environment

    Public ReadOnly Property [Shared] As Mount
    Public ReadOnly Property container As Image

    Sub New(container As Image)
        Me.container = container
    End Sub

    Public Function Mount(local$, virtual$) As Environment
        _Shared = New Mount With {.local = local, .virtual = virtual}
        Return Me
    End Function

    Public Function Mount([shared] As Mount) As Environment
        _Shared = [shared]
        Return Me
    End Function

    Const InvalidMount$ = "Shared Drive argument is presented, but value is invalid, -v option will be ignored!"

    Public Function CreateDockerCommand(command As String) As String
        Dim options As New StringBuilder

        If Not [Shared] Is Nothing Then
            If [Shared].IsValid Then
                Call options.AppendLine($"-v {[Shared]}")
            Else
                Call InvalidMount.Warning
            End If
        End If

        Return $"docker run {options} {container} {command}"
    End Function
End Class

