Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.MIME.application.json
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Parallel

Public Class Argument

    Public Property debugPort As Integer? = Nothing
    Public Property verbose As Boolean = False
    Public Property ignoreError As Boolean = False
    Public Property n_threads As Integer = 32
    Public Property thread_interval As Integer = 300
    Public Property libpath As String = Nothing

    Sub New()
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Sub New(n_threads As Integer)
        Me.n_threads = n_threads
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Overrides Function ToString() As String
        Return Me.GetJson(simpleDict:=True)
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function Copy() As Argument
        Return ToString.LoadJSON(Of Argument)
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function CreateHost() As SlaveTask
        Return Host.CreateSlave(debugPort,
                                verbose:=verbose,
                                ignoreError:=ignoreError,
                                libpath:=libpath)
    End Function

End Class
