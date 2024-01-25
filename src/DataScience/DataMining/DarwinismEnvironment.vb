Imports batch

Public Module DarwinismEnvironment

    ''' <summary>
    ''' a global parameters for the parallel computing
    ''' </summary>
    Dim par As Argument

    ''' <summary>
    ''' get a copy of the current parallel runtime environment arguments
    ''' </summary>
    ''' <returns></returns>
    Public Function GetEnvironmentArguments() As Argument
        If par Is Nothing Then
            Return Nothing
        Else
            Return par.Copy
        End If
    End Function

    Public Sub SetEnvironment(par As Argument)
        DarwinismEnvironment.par = par
    End Sub

    Public Sub SetThreads(n_threads As Integer)
        If par Is Nothing Then
            par = New Argument(n_threads)
        Else
            par.n_threads = n_threads
        End If
    End Sub

    ''' <summary>
    ''' set a directory path that contains the scibasic.net framework 
    ''' runtime of the current parallel environment.
    ''' </summary>
    ''' <param name="libpath"></param>
    Public Sub SetLibPath(libpath As String)
        If par Is Nothing Then
            par = New Argument(8) With {.libpath = libpath}
        Else
            par.libpath = libpath
        End If
    End Sub
End Module
