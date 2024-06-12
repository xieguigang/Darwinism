#Region "Microsoft.VisualBasic::e96aa65b2e6420e3b0d132ae7c146c3b, src\Darwinism\Platform\Docker.vb"

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

'   Total Lines: 192
'    Code Lines: 70
' Comment Lines: 104
'   Blank Lines: 18
'     File Size: 8.50 KB


' Module DockerTools
' 
'     Function: PS, rmi, Run, Search
' 
'     Sub: [Stop]
' 
' /********************************************************************************/

#End Region

Imports Darwinism.Docker
Imports Darwinism.Docker.Arguments
Imports Darwinism.Docker.Captures
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine.ExpressionSymbols.Closure
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine.ExpressionSymbols.Operators
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Interop
Imports SMRUCC.Rsharp.Runtime.Vectorization
Imports REnvironment = SMRUCC.Rsharp.Runtime.Environment

''' <summary>
''' Docker commands
''' </summary>
''' 
<Package("docker", Category:=APICategories.SoftwareTools, Publisher:="xie.guigang@live.com")>
<RTypeExport("image", GetType(Image))>
<RTypeExport("mount", GetType(Mount))>
Public Module DockerTools

    ' PS C:\Users\lipidsearch> docker

    ' Usage:  docker [OPTIONS] COMMAND

    '   A self-sufficient runtime for containers

    ' Options:
    '   --config string          Location of client config files (default "C:\\Users\\lipidsearch\\.docker")
    '   -D, --debug              Enable debug mode
    '   -H, --host list          Daemon socket(s) to connect to
    '   -l, --log-level string   Set the logging level ("debug"|"info"|"warn"|"error"|"fatal") (default "info")
    '   --tls                    Use TLS; implied by --tlsverify
    '   --tlscacert string       Trust certs signed only by this CA (default "C:\\Users\\lipidsearch\\.docker\\ca.pem")
    '   --tlscert string         Path to TLS certificate file (default "C:\\Users\\lipidsearch\\.docker\\cert.pem")
    '   --tlskey string          Path to TLS key file (default "C:\\Users\\lipidsearch\\.docker\\key.pem")
    '   --tlsverify              Use TLS and verify the remote
    '   -v, --version            Print version information and quit

    ' Management Commands:
    '   builder     Manage builds
    '   config      Manage Docker configs
    '   container   Manage containers
    '   image       Manage images
    '   network     Manage networks
    '   node        Manage Swarm nodes
    '   plugin      Manage plugins
    '   secret      Manage Docker secrets
    '   service     Manage services
    '   stack       Manage Docker stacks
    '   swarm       Manage Swarm
    '   system      Manage Docker
    '   trust       Manage trust on Docker images
    '   volume      Manage volumes

    ' Commands:
    '   attach      Attach local standard input, output, and error streams to a running container
    '   build       Build an image from a Dockerfile
    '   commit      Create a new image from a container's changes
    '   cp          Copy files/folders between a container and the local filesystem
    '   create      Create a new container
    '   diff        Inspect changes to files or directories on a container's filesystem
    '   events      Get real time events from the server
    '   exec        Run a command in a running container
    '   export      Export a container's filesystem as a tar archive
    '   history     Show the history of an image
    '   images      List images
    '   import      Import the contents from a tarball to create a filesystem image
    '   info        Display system-wide information
    '   inspect     Return low-level information on Docker objects
    '   kill        Kill one or more running containers
    '   load        Load an image from a tar archive or STDIN
    '   login       Log in to a Docker registry
    '   logout      Log out from a Docker registry
    '   logs        Fetch the logs of a container
    '   pause       Pause all processes within one or more containers
    '   port        List port mappings or a specific mapping for the container
    '   ps          List containers
    '   pull        Pull an image or a repository from a registry
    '   push        Push an image or a repository to a registry
    '   rename      Rename a container
    '   restart     Restart one or more containers
    '   rm          Remove one or more containers
    '   rmi         Remove one or more images
    '   run         Run a command in a new container
    '   save        Save one or more images to a tar archive (streamed to STDOUT by default)
    '   search      Search the Docker Hub for images
    '   start       Start one or more stopped containers
    '   stats       Display a live stream of container(s) resource usage statistics
    '   stop        Stop one or more running containers
    '   tag         Create a tag TARGET_IMAGE that refers to SOURCE_IMAGE
    '   top         Display the running processes of a container
    '   unpause     Unpause all processes within one or more containers
    '   update      Update configuration of one or more containers
    '   version     Show the Docker version information
    '   wait        Block until one or more containers stop, then print their exit codes

    ' Run 'docker COMMAND --help' for more information on a command.

    <ExportAPI("docker")>
    Public Function docker_env(Optional img As String = Nothing) As Docker.Environment
        If img.StringEmpty Then
            Return New Docker.Environment
        Else
            Return New Docker.Environment(New Image(img))
        End If
    End Function

    ''' <summary>
    ''' Search the Docker Hub for images
    ''' </summary>
    ''' <param name="term"></param>
    ''' <returns></returns>
    ''' 
    <ExportAPI("search")>
    Public Function Search(term As String) As IEnumerable(Of Search)
        Return ShellCommand.Run("docker", $"search {term}") _
            .ParseTable(Function(tokens)
                            Return New Search With {
                                .NAME = Image.ParseEntry(tokens(0)),
                                .DESCRIPTION = tokens(1).Trim,
                                .STARS = tokens(2).Trim,
                                .OFFICIAL = tokens(3).Trim,
                                .AUTOMATED = tokens(4).Trim
                            }
                        End Function)
    End Function

    ''' <summary>
    ''' List containers
    ''' </summary>
    ''' <returns></returns>
    ''' 
    <ExportAPI("ps")>
    Public Function PS() As IEnumerable(Of Container)
        Return ShellCommand.Run("docker", "ps") _
            .ParseTable(Function(tokens)
                            Return New Container With {
                                .CONTAINER_ID = tokens(0).Trim,
                                .IMAGE = Image.ParseEntry(tokens(1)),
                                .COMMAND = tokens(2).Trim,
                                .CREATED = tokens(3).Trim,
                                .STATUS = tokens(4).Trim,
                                .PORTS = tokens(5),
                                .NAMES = tokens(6).Trim
                            }
                        End Function)
    End Function

    ''' <summary>
    ''' Stop one or more running containers
    ''' </summary>
    ''' <param name="containers"></param>
    ''' 
    <ExportAPI("stop")>
    Public Sub [Stop](Optional containers As String() = Nothing)
        For Each id As String In containers
            Call ShellCommand.Run("docker", $"stop {id}")
        Next
    End Sub

    ''' <summary>
    ''' create docker image reference
    ''' </summary>
    ''' <returns></returns>
    ''' 
    <ExportAPI("image")>
    <RApiReturn(GetType(Docker.Environment), GetType(Image))>
    Public Function image_reference(x As Object,
                                    Optional name As String = Nothing,
                                    Optional publisher As String = Nothing,
                                    Optional env As REnvironment = Nothing) As Object

        If TypeOf x Is Docker.Environment Then
            ' add reference to a docker image in R#
            ' docker() |> image("...")
            Return DirectCast(x, Docker.Environment).SetImage(New Image(name))
        ElseIf TypeOf x Is RMethodInfo Then
            Dim f As RMethodInfo = x

            If f.parameters.Length <> 1 Then
                Return Internal.debug.stop("invalid docker command wrapper!", env)
            End If

            If f.name = "docker" AndAlso f.name = "docker" Then
                Dim docker As Docker.Environment = f.Invoke({Nothing}, env)
                docker.SetImage(New Image(name))
                Return docker
            Else
                Return Internal.debug.stop("invalid docker command wrapper!", env)
            End If
        Else
            ' create docker image reference
            name = CLRVector.asCharacter(x).FirstOrDefault

            If name.StringEmpty Then
                Return Internal.debug.stop("invalid docker image: empty image reference name!", env)
            End If
        End If

        Return New Image With {
            .Package = name,
            .Publisher = publisher
        }
    End Function

    ''' <summary>
    ''' set environment variable for the docker run
    ''' </summary>
    ''' <param name="docker"></param>
    ''' <param name="args"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("env")>
    Public Function setVariable(docker As Docker.Environment, <RListObjectArgument> args As list, Optional env As REnvironment = Nothing) As Object
        For Each tuple As KeyValuePair(Of String, Object) In args.slots
            Dim value As String() = CLRVector.asCharacter(tuple.Value)

            If value.IsNullOrEmpty Then
                docker.environments(tuple.Key) = ""
            Else
                docker.environments(tuple.Key) = value(0)
            End If
        Next

        Return docker
    End Function

    ''' <summary>
    ''' mount shared volumn between the host and the container
    ''' </summary>
    ''' <param name="docker"></param>
    ''' <param name="mount">
    ''' the folder path for shared, value of this parameter could be:
    ''' 
    ''' 1. just a single character vector for specific the path string
    ''' 2. a lambda expression for specific the different folder name
    '''
    ''' </param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    ''' <example>
    ''' # for just mount a folder
    ''' #
    ''' # -v "/path/to/shared/folder/:/path/to/shared/folder/"
    ''' #
    ''' docker 
    ''' |> mount("/path/to/shared/folder/");
    ''' 
    ''' # for mount a folder with different name
    ''' #
    ''' # -v "/foldername_in_host:/folder_name_in_container"
    ''' #
    ''' docker 
    ''' |> mount("/foldername_in_host" -> "/folder_name_in_container")
    ''' </example>
    <ExportAPI("mount")>
    Public Function mountVolumn(docker As Docker.Environment, mount As Object, Optional env As REnvironment = Nothing) As Object
        If TypeOf mount Is String Then
            Return docker.Mount(New Mount(CStr(mount)))
        ElseIf TypeOf mount Is DeclareLambdaFunction Then
            Dim lambda As DeclareLambdaFunction = mount
            Dim host As String = lambda.parameterNames(0)
            Dim virtual As String = ValueAssignExpression.GetSymbol(lambda.closure)

            Return docker.Mount(New Mount With {.local = host, .virtual = virtual})
        Else
            Return Message.InCompatibleType(GetType(String), mount.GetType, env)
        End If
    End Function

    <ExportAPI("workspace")>
    Public Function setWorkspace(docker As Docker.Environment, workdir As String) As Object
        Return docker.SetWorkdir(workdir)
    End Function

    ''' <summary>
    ''' enable interactive tty device
    ''' </summary>
    ''' <param name="docker"></param>
    ''' <returns></returns>
    <ExportAPI("tty")>
    Public Function tty(docker As Docker.Environment) As Object
        docker.tty = True
        Return docker
    End Function

    ''' <summary>
    ''' Run a command in a new container.(这个函数会捕捉到命令的标准输出然后以字符串的形式返回)
    ''' </summary>
    ''' <param name="command"></param>
    ''' <param name="shell_cmdl">
    ''' this debug parameter specific that just returns the commandline for 
    ''' run docker instead of run command and returns the std_output.
    ''' </param>
    ''' <remarks>
    ''' 这个方法能够自定义的参数比较有限,如果需要更加复杂的使用方法,可以使用<see cref="Environment"/>对象
    ''' </remarks>
    ''' 
    <RApiReturn(TypeCodes.string)>
    <ExportAPI("run")>
    Public Function Run(container As Object, command$,
                        Optional script As String = Nothing,
                        Optional workdir As String = Nothing,
                        Optional mounts As Mount() = Nothing,
                        Optional portForward As PortForward = Nothing,
                        <RListObjectArgument>
                        Optional args As list = Nothing,
                        Optional shell_cmdl As Boolean = False,
                        Optional env As REnvironment = Nothing) As Object

        If (Not TypeOf container Is Image) AndAlso (Not TypeOf container Is Docker.Environment) Then
            If TypeOf container Is String Then
                container = New Image(CStr(container))
            Else
                Return Message.InCompatibleType(GetType(Image), container.GetType, env)
            End If
        End If

        Dim host As Docker.Environment

        If TypeOf container Is Docker.Environment Then
            host = container
        Else
            host = New Docker.Environment(container)
        End If

        If Not script.StringEmpty Then
            command = command & " " & script.CLIPath
        End If

        If args.length > 0 Then
            Dim pars As String() = args.slots _
                .Select(Function(t)
                            Return $"{t.Key} {CLRVector.asCharacter(t.Value).JoinBy(",").CLIToken}"
                        End Function) _
                .ToArray

            command = command & " " & pars.JoinBy(" ")
        End If

        Dim cli As String = host _
            .Mount(mounts) _
            .CreateDockerCommand(command, workdir, portForward)

        If shell_cmdl Then
            Return $"docker {cli}"
        Else
            Return ShellCommand.Run("docker", cli)
        End If
    End Function

    ''' <summary>
    ''' delete docker images and related containers
    ''' </summary>
    ''' <returns></returns>
    ''' 
    <ExportAPI("rmImage")>
    Public Function rmi(imageId As String) As Boolean
        Dim stdout As Value(Of String) = ""
        Dim containerId As String

        Do While (stdout = CommandLine.Call("docker", $"rmi {imageId}")).Contains("image is being used by stopped container")
            containerId = Strings.Split(Strings.Trim(stdout)).Last

            Call ShellCommand.Run("docker", $"rm {containerId}")
            Call Console.WriteLine($"remove container {containerId}")
        Loop

        Return True
    End Function
End Module
