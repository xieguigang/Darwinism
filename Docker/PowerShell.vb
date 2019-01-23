Imports System.Collections.ObjectModel
Imports System.Management.Automation
Imports System.Management.Automation.Runspaces
Imports System.Text
Imports System.IO

Public Class PowerShell

    ' 
    ' 
    ''' <summary>
    ''' Takes script text as input and runs it, then converts 
    ''' the results to a string to return to the user 
    ''' </summary>
    ''' <param name="scriptText"></param>
    ''' <returns></returns>
    Public Function RunScript(scriptText As String) As String
        ' create Powershell runspace 
        Dim MyRunSpace As Runspace = RunspaceFactory.CreateRunspace()

        ' open it 
        MyRunSpace.Open()

        ' create a pipeline and feed it the script text 
        Dim MyPipeline As Pipeline = MyRunSpace.CreatePipeline()

        MyPipeline.Commands.AddScript(scriptText)

        ' add an extra command to transform the script output objects into nicely formatted strings 
        ' remove this line to get the actual objects that the script returns. For example, the script 
        ' "Get-Process" returns a collection of System.Diagnostics.Process instances. 
        MyPipeline.Commands.Add("Out-String")

        ' execute the script 
        Dim results As Collection(Of PSObject) = MyPipeline.Invoke()

        ' close the runspace 
        MyRunSpace.Close()

        ' convert the script result into a single string 
        Dim MyStringBuilder As New StringBuilder()

        For Each obj As PSObject In results
            MyStringBuilder.AppendLine(obj.ToString())
        Next

        ' return the results of the script that has 
        ' now been converted to text 
        Return MyStringBuilder.ToString()
    End Function
End Class
