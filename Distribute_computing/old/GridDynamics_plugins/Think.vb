#Region "Microsoft.VisualBasic::195ea346f5b749167e6e2474c5b72dba, Distribute_computing\GridDynamics_plugins\Think.vb"

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

    ' Class Think
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: FromEnvironment
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports System.Text
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.InteropService
Imports Microsoft.VisualBasic.ApplicationServices

' Microsoft VisualBasic CommandLine Code AutoGenerator
' assembly: ..\build\Think.exe

' 
'  // 
'  // 
'  // 
'  // VERSION:   1.0.0.0
'  // COPYRIGHT: Copyright ©  2019
'  // GUID:      122d95f7-d875-4ee8-92b2-2fdbfdaa2261
'  // 
' 
' 
'  < thinking.CLI >
' 
' 
' SYNOPSIS
' Think command [/argument argument-value...] [/@set environment-variable=value...]
' 
' All of the command that available in this program has been list below:
' 
'  /slave:     Program running in slave mode, apply for the multiple-process parallel.
' 
' 
' ----------------------------------------------------------------------------------------------------
' 
'    1. You can using "Think ??<commandName>" for getting more details command help.
'    2. Using command "Think /CLI.dev [---echo]" for CLI pipeline development.
'    3. Using command "Think /i" for enter interactive console mode.

Namespace CLI


''' <summary>
''' thinking.CLI
''' </summary>
'''
Public Class Think : Inherits InteropService

    Public Const App$ = "Think.exe"

    Sub New(App$)
        MyBase._executableAssembly = App$
    End Sub

     <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Function FromEnvironment(directory As String) As Think
          Return New Think(App:=directory & "/" & Think.App)
     End Function

''' <summary>
''' ```
''' /slave /application &lt;invokeinfo/json_base64> /out &lt;memory_mapfile>
''' ```
''' Program running in slave mode, apply for the multiple-process parallel.
''' </summary>
'''
Public Function Slave(application As String, out As String) As Integer
    Dim CLI As New StringBuilder("/slave")
    Call CLI.Append(" ")
    Call CLI.Append("/application " & """" & application & """ ")
    Call CLI.Append("/out " & """" & out & """ ")
     Call CLI.Append("/@set --internal_pipeline=TRUE ")


    Dim proc As IIORedirectAbstract = RunDotNetApp(CLI.ToString())
    Return proc.Run()
End Function
End Class
End Namespace
