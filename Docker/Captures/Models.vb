Imports Darwinism.Docker.Arguments

Namespace Captures

    Public Structure Search
        Dim NAME As Image
        Dim DESCRIPTION As String
        Dim STARS As Integer
        Dim OFFICIAL As String
        Dim AUTOMATED As String
    End Structure

    Public Structure Container
        Dim CONTAINER_ID As String
        Dim IMAGE As Image
        Dim COMMAND As String
        Dim CREATED As String
        Dim STATUS As String
        Dim PORTS As String
        Dim NAMES As String
    End Structure
End Namespace