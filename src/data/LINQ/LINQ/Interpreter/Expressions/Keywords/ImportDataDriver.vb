#Region "Microsoft.VisualBasic::273b6a67ee0361038aa1a31f09fbf0f2, src\data\LINQ\LINQ\Interpreter\Expressions\Keywords\ImportDataDriver.vb"

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

    '   Total Lines: 53
    '    Code Lines: 42
    ' Comment Lines: 0
    '   Blank Lines: 11
    '     File Size: 1.61 KB


    '     Class ImportDataDriver
    ' 
    '         Properties: dllName, keyword
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: Exec, FindDllFile, ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Language

Namespace Interpreter.Expressions

    Public Class ImportDataDriver : Inherits KeywordExpression

        Public ReadOnly Property dllName As String

        Public Overrides ReadOnly Property keyword As String
            Get
                Return "Imports"
            End Get
        End Property

        Sub New(dllName As String)
            Me.dllName = dllName
        End Sub

        Public Overrides Function Exec(context As ExecutableContext) As Object
            If dllName.FileExists Then
                Return dllName
            Else
                Return FindDllFile(context)
            End If
        End Function

        Private Function FindDllFile(context As ExecutableContext) As String
            Dim fileName As Value(Of String) = ""
            Dim driver As String = dllName

            If driver.ExtensionSuffix <> "dll" Then
                driver = $"{driver}.dll"
            End If

            For Each dir As String In {
                App.HOME,
                App.CurrentDirectory,
                $"{App.HOME}/Library",
                $"{App.HOME}/Drivers"
            }
                If (fileName = $"{dir}/{driver}").FileExists Then
                    Return fileName
                End If
            Next

            Throw New BadImageFormatException($"driver module '{driver}' not found!")
        End Function

        Public Overrides Function ToString() As String
            Return $"load driver: {dllName}"
        End Function
    End Class
End Namespace
