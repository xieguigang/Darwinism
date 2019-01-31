#Region "Microsoft.VisualBasic::55f46f3ae0858ecd0ffcfe754b6b7f98, Docker\PowerShell.vb"

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

    ' Class PowerShell
    ' 
    '     Function: RunPSScript, RunScript
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Collections.ObjectModel
Imports System.Management.Automation
Imports System.Management.Automation.Runspaces
Imports System.Runtime.CompilerServices
Imports System.Text

''' <summary>
''' PowerShell interface to VB.NET
''' </summary>
Public Class PowerShell

    Friend ReadOnly logs As New List(Of String)

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="command">
    ''' 会自动将回车符替换为空格
    ''' </param>
    ''' <returns></returns>
    Default Public ReadOnly Property EVal(command As String) As String
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Get
            Return RunScript(scriptText:=command.TrimNewLine)
        End Get
    End Property

    ''' <summary>
    ''' Takes script text as input and runs it, then converts the results to a string to return to the user 
    ''' </summary>
    ''' <param name="scriptText"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' https://blogs.msdn.microsoft.com/zainnab/2008/07/26/calling-a-powershell-script-from-your-net-code/
    ''' </remarks>
    Public Function RunScript(scriptText As String) As String
        ' convert the script result into a single string 
        Dim out As New StringBuilder()
        Dim results As Collection(Of PSObject) = RunPSScript(scriptText)

        For Each obj As PSObject In results
            out.AppendLine(obj.ToString())
        Next

        Call logs.Add(scriptText)

        ' return the results of the script that has 
        ' now been converted to text 
        Return out.ToString()
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ps"></param>
    ''' <param name="outString">
    ''' add an extra command to transform the script output objects 
    ''' into nicely formatted strings remove this line to get the 
    ''' actual objects that the script returns. For example, the script 
    ''' ``Get-Process`` returns a collection of <see cref="Process"/>
    ''' instances.
    ''' </param>
    ''' <returns></returns>
    Public Function RunPSScript(ps$, Optional outString As Boolean = True) As Collection(Of PSObject)
        ' create Powershell runspace 
        Using myRunSpace As Runspace = RunspaceFactory.CreateRunspace()
            ' open it 
            Call myRunSpace.Open()

            With myRunSpace.CreatePipeline()
                ' create a pipeline and feed it the script text 
                Call .Commands.AddScript(ps)

                If outString Then
                    ' add an extra command to transform the script output objects 
                    ' into nicely formatted strings remove this line to get the 
                    ' actual objects that the script returns. For example, the script 
                    ' "Get-Process" returns a collection of System.Diagnostics.Process 
                    ' instances.
                    Call .Commands.Add("Out-String")
                End If

                ' execute the script 
                Return .Invoke()
            End With
        End Using
    End Function
End Class
