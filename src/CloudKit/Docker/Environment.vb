#Region "Microsoft.VisualBasic::5b98b2872aca2d0fe99c7cc3638bb61c, src\CloudKit\Docker\Environment.vb"

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

    '   Total Lines: 269
    '    Code Lines: 72 (26.77%)
    ' Comment Lines: 174 (64.68%)
    '    - Xml Docs: 16.67%
    ' 
    '   Blank Lines: 23 (8.55%)
    '     File Size: 14.19 KB


    ' Class Environment
    ' 
    '     Properties: [Shared], container, environments, tty, workspace
    ' 
    '     Constructor: (+2 Overloads) Sub New
    '     Function: CreateDockerCommand, (+2 Overloads) Mount, SetImage, SetWorkdir
    ' 
    ' Class DockerAppDriver
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: Shell
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports System.Text
Imports Darwinism.Docker.Arguments
Imports Microsoft.VisualBasic.Linq

''' <summary>
''' The container environment module for wrap ``docker run ...``
''' </summary>
Public Class Environment

    ' Run a command in a new container
    '
    ' Options:
    '       --add-host list                  Add a custom host-to-IP mapping
    '                                        (host:ip)
    '   -a, --attach list                    Attach to STDIN, STDOUT or STDERR
    '       --blkio-weight uint16            Block IO (relative weight),
    '                                        between 10 and 1000, or 0 to
    '                                        disable (default 0)
    '       --blkio-weight-device list       Block IO weight (relative device
    '                                        weight) (default [])
    '       --cap-add list                   Add Linux capabilities
    '       --cap-drop list                  Drop Linux capabilities
    '       --cgroup-parent string           Optional parent cgroup for the
    '                                        container
    '       --cidfile string                 Write the container ID to the file
    '       --cpu-period int                 Limit CPU CFS (Completely Fair
    '                                        Scheduler) period
    '       --cpu-quota int                  Limit CPU CFS (Completely Fair
    '                                        Scheduler) quota
    '       --cpu-rt-period int              Limit CPU real-time period in
    '                                        microseconds
    '       --cpu-rt-runtime int             Limit CPU real-time runtime in
    '                                        microseconds
    '   -c, --cpu-shares int                 CPU shares (relative weight)
    '       --cpus decimal                   Number of CPUs
    '       --cpuset-cpus string             CPUs in which to allow execution
    '                                        (0-3, 0,1)
    '       --cpuset-mems string             MEMs in which to allow execution
    '                                        (0-3, 0,1)
    '   -d, --detach                         Run container in background and
    '                                        print container ID
    '       --detach-keys string             Override the key sequence for
    '                                        detaching a container
    '       --device list                    Add a host device to the container
    '       --device-cgroup-rule list        Add a rule to the cgroup allowed
    '                                        devices list
    '       --device-read-bps list           Limit read rate (bytes per second)
    '                                        from a device (default [])
    '       --device-read-iops list          Limit read rate (IO per second)
    '                                        from a device (default [])
    '       --device-write-bps list          Limit write rate (bytes per
    '                                        second) to a device (default [])
    '       --device-write-iops list         Limit write rate (IO per second)
    '                                        to a device (default [])
    '       --disable-content-trust          Skip image verification (default true)
    '       --dns list                       Set custom DNS servers
    '       --dns-option list                Set DNS options
    '       --dns-search list                Set custom DNS search domains
    '       --entrypoint string              Overwrite the default ENTRYPOINT
    '                                        of the image
    '   -e, --env list                       Set environment variables
    '       --env-file list                  Read in a file of environment variables
    '       --expose list                    Expose a port or a range of ports
    '       --group-add list                 Add additional groups to join
    '       --health-cmd string              Command to run to check health
    '       --health-interval duration       Time between running the check
    '                                        (ms|s|m|h) (default 0s)
    '       --health-retries int             Consecutive failures needed to
    '                                        report unhealthy
    '       --health-start-period duration   Start period for the container to
    '                                        initialize before starting
    '                                        health-retries countdown
    '                                        (ms|s|m|h) (default 0s)
    '       --health-timeout duration        Maximum time to allow one check to
    '                                        run (ms|s|m|h) (default 0s)
    '       --help                           Print usage
    '   -h, --hostname string                Container host name
    '       --init                           Run an init inside the container
    '                                        that forwards signals and reaps
    '                                        processes
    '   -i, --interactive                    Keep STDIN open even if not attached
    '       --ip string                      IPv4 address (e.g., 172.30.100.104)
    '       --ip6 string                     IPv6 address (e.g., 2001:db8::33)
    '       --ipc string                     IPC mode to use
    '       --isolation string               Container isolation technology
    '       --kernel-memory bytes            Kernel memory limit
    '   -l, --label list                     Set meta data on a container
    '       --label-file list                Read in a line delimited file of labels
    '       --link list                      Add link to another container
    '       --link-local-ip list             Container IPv4/IPv6 link-local
    '                                        addresses
    '       --log-driver string              Logging driver for the container
    '       --log-opt list                   Log driver options
    '       --mac-address string             Container MAC address (e.g.,
    '                                        92:d0:c6:0a:29:33)
    '   -m, --memory bytes                   Memory limit
    '       --memory-reservation bytes       Memory soft limit
    '       --memory-swap bytes              Swap limit equal to memory plus
    '                                        swap: '-1' to enable unlimited swap
    '       --memory-swappiness int          Tune container memory swappiness
    '                                        (0 to 100) (default -1)
    '       --mount mount                    Attach a filesystem mount to the
    '                                        container
    '       --name string                    Assign a name to the container
    '       --network string                 Connect a container to a network
    '                                        (default "default")
    '       --network-alias list             Add network-scoped alias for the
    '                                        container
    '       --no-healthcheck                 Disable any container-specified
    '                                        HEALTHCHECK
    '       --oom-kill-disable               Disable OOM Killer
    '       --oom-score-adj int              Tune host's OOM preferences (-1000
    '                                        to 1000)
    '       --pid string                     PID namespace to use
    '       --pids-limit int                 Tune container pids limit (set -1
    '                                        for unlimited)
    '       --privileged                     Give extended privileges to this
    '                                        container
    '   -p, --publish list                   Publish a container's port(s) to
    '                                        the host
    '   -P, --publish-all                    Publish all exposed ports to
    '                                        random ports
    '       --read-only                      Mount the container's root
    '                                        filesystem as read only
    '       --restart string                 Restart policy to apply when a
    '                                        container exits (default "no")
    '       --rm                             Automatically remove the container
    '                                        when it exits
    '       --runtime string                 Runtime to use for this container
    '       --security-opt list              Security Options
    '       --shm-size bytes                 Size of /dev/shm
    '       --sig-proxy                      Proxy received signals to the
    '                                        process (default true)
    '       --stop-signal string             Signal to stop a container
    '                                        (default "15")
    '       --stop-timeout int               Timeout (in seconds) to stop a
    '                                        container
    '       --storage-opt list               Storage driver options for the
    '                                        container
    '       --sysctl map                     Sysctl options (default map[])
    '       --tmpfs list                     Mount a tmpfs directory
    '   -t, --tty                            Allocate a pseudo-TTY
    '       --ulimit ulimit                  Ulimit options (default [])
    '   -u, --user string                    Username or UID (format:
    '                                        <name|uid>[:<group|gid>])
    '       --userns string                  User namespace to use
    '       --uts string                     UTS namespace to use
    '   -v, --volume list                    Bind mount a volume
    '       --volume-driver string           Optional volume driver for the
    '                                        container
    '       --volumes-from list              Mount volumes from the specified
    '                                        container(s)
    '   -w, --workdir string                 Working directory inside the container

    Public ReadOnly Property [Shared] As Mount()
    Public ReadOnly Property container As Image

    ''' <summary>
    ''' the environment variable
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property environments As New Dictionary(Of String, String)
    ''' <summary>
    ''' set the current workdir
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property workspace As String

    Public Property tty As Boolean

    Sub New(container As Image)
        Me.container = container
    End Sub

    Sub New()
    End Sub

    Public Function Mount(local$, virtual$) As Environment
        _Shared = _Shared.JoinIterates(New Mount With {.local = local, .virtual = virtual}).ToArray
        Return Me
    End Function

    Public Function Mount(ParamArray [shared] As Mount()) As Environment
        _Shared = _Shared.JoinIterates([shared]).ToArray
        Return Me
    End Function

    Public Function SetWorkdir(dir As String) As Environment
        _workspace = dir
        Return Me
    End Function

    Public Function SetImage(img As Image) As Environment
        _container = img
        Return Me
    End Function

    Const InvalidMount$ = "Shared Drive argument is presented, but value is invalid, -v option will be ignored!"

    ''' <summary>
    ''' Create a docker run command for running command ``app arguments``
    ''' </summary>
    ''' <param name="command">``app arguments``</param>
    ''' <param name="workdir">Working directory inside the container</param>
    ''' <param name="portForward">Publish a container's port(s) to the host</param>
    ''' <returns>
    ''' docker run xxx
    ''' </returns>
    Public Function CreateDockerCommand(command$, Optional workdir$ = Nothing, Optional portForward As PortForward = Nothing) As String
        Dim options As New StringBuilder

        workdir = If(workdir, workspace)

        If tty Then
            options.AppendLine("-it")
        End If

        For Each env In environments
            Call options.AppendLine($"--env ""{env.Key}={env.Value}""")
        Next

        If Not [Shared] Is Nothing Then
            For Each map As Mount In [Shared]
                If Not map.IsValid Then
                    Call InvalidMount.Warning
                End If

                Call options.AppendLine($"-v {map}")
            Next
        End If
        If Not workdir.StringEmpty Then
            Call options.AppendLine($"--workdir=""{workdir}""")
        End If
        If Not portForward Is Nothing Then
            Call options.AppendLine($"-p {portForward}")
        End If

        Return $"run {options.ToString.TrimNewLine(" ")} {container} {command}"
    End Function
End Class

''' <summary>
''' 通过这个模块来运行某一个Docker container之中的命令行命令
''' </summary>
Public Class DockerAppDriver

    ReadOnly docker As Environment
    ReadOnly appHome$
    ReadOnly appName$

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="container">Docker容器的``image ID``</param>
    ''' <param name="app">可执行文件的文件名</param>
    ''' <param name="mount"></param>
    ''' <param name="home">应用程序的文件夹目录路径</param>
    Sub New(container As Image, app$, Optional mount As Mount = Nothing, Optional home$ = Nothing)
        docker = New Environment(container).Mount(mount)
        appHome = home
        appName = app
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function Shell(args$, Optional workdir$ = Nothing) As String
        Return ShellCommand.Run("docker", docker.CreateDockerCommand($"{appHome}/{appName} {args}", workdir:=workdir))
    End Function
End Class
