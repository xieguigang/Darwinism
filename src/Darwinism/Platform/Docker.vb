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
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Interop

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
    Public Function image_reference(name As String, Optional publisher As String = Nothing) As Image
        Return New Image With {
            .Package = name,
            .Publisher = publisher
        }
    End Function

    ''' <summary>
    ''' Run a command in a new container.(这个函数会捕捉到命令的标准输出然后以字符串的形式返回)
    ''' </summary>
    ''' <param name="command"></param>
    ''' <remarks>
    ''' 这个方法能够自定义的参数比较有限,如果需要更加复杂的使用方法,可以使用<see cref="Environment"/>对象
    ''' </remarks>
    ''' 
    <RApiReturn(TypeCodes.string)>
    <ExportAPI("run")>
    Public Function Run(container As Object, command$,
                        Optional workdir As String = Nothing,
                        Optional mounts As Mount() = Nothing,
                        Optional portForward As PortForward = Nothing,
                        Optional env As Environment = Nothing) As Object

        If Not TypeOf container Is Image Then
            If TypeOf container Is String Then
                container = New Image(CStr(container))
            Else
                Return Message.InCompatibleType(GetType(Image), container.GetType, env)
            End If
        End If

        Dim cli As String = New Docker.Environment(container) _
            .Mount(mounts) _
            .CreateDockerCommand(command, workdir, portForward)

        Return ShellCommand.Run("docker", cli)
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
