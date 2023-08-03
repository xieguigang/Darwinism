
''' <summary>
''' define the application host information for the IPC parallel
''' </summary>
''' 
Public Class AppHost : Inherits Attribute

    ''' <summary>
    ''' executable name of the application host
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property AppName As String
    ''' <summary>
    ''' the commandline argument string that could be used as the protocol handler of this IPC parallel
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Protocol As String

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="app"></param>
    ''' <param name="args">
    ''' the commandline arguments template string
    ''' </param>
    Sub New(app As String, args As String)
        AppName = app
        Protocol = args
    End Sub

End Class
