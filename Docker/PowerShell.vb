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
