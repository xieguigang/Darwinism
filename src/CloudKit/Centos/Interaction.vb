#Region "Microsoft.VisualBasic::0cf9299d7956d2c3299068153e02ee2b, src\CloudKit\Centos\Interaction.vb"

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

    '   Total Lines: 119
    '    Code Lines: 56 (47.06%)
    ' Comment Lines: 45 (37.82%)
    '    - Xml Docs: 53.33%
    ' 
    '   Blank Lines: 18 (15.13%)
    '     File Size: 4.70 KB


    ' Class Interaction
    ' 
    '     Properties: isUnix, release, version
    ' 
    '     Function: cat, HasCommand, Shell
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices

''' <summary>
''' linux command interaction shell module
''' </summary>
Public Class Interaction

    Public Shared ReadOnly Property version As String
        Get
            Return cat("/proc/version", verbose:=False)
        End Get
    End Property

    Public Shared ReadOnly Property release As String
        Get
            If "/etc/centos-release".FileExists Then
                Return cat("/etc/centos-release", verbose:=False)
            ElseIf "/etc/redhat-release".FileExists Then
                Return cat("/etc/redhat-release", verbose:=False)
            Else
                Return Shell("lsb_release", "-d", verbose:=False) _
                    .GetTagValue(":", trim:=True) _
                    .Value
            End If
        End Get
    End Property

    Public Shared ReadOnly Property isUnix As Boolean
        Get
            Dim platform = Environment.OSVersion.Platform

#If NET48 Then
            Return platform = PlatformID.Unix OrElse platform = PlatformID.MacOSX
#Else
            Return platform = PlatformID.Unix
#End If
        End Get
    End Property

    ''' <summary>
    ''' does the required command is installed in the centos system?
    ''' </summary>
    ''' <param name="command"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' use the ``command -v`` for check the command is existsed or not, example as 
    ''' check docker command is existed ``command -v docker``.
    ''' </remarks>
    Public Shared Function HasCommand(command As String, Optional verbose As Boolean = False) As Boolean
        Return Not Interaction.Shell("command", $"-v {command}", verbose:=verbose).StringEmpty
    End Function

    ' 20201023
    '
    ' ./linux_run.R --ignore-missing-startup-packages

    ' Loading required package: base
    ' Loading required package: utils
    ' Loading required package: grDevices
    ' Loading required package: stats

    ' Error in <globalEnvironment> -> InitializeEnvironment -> str -> str -> "iostat" -> iostat
    ' 1. TargetInvocationException: Exception has been thrown by the target of an invocation.
    ' 2. Win32Exception: ApplicationName='iostat', CommandLine='', CurrentDirectory='', Native error= Cannot find the specified file
    ' 3. stackFrames: 
    ' at System.Reflection.MonoMethod.Invoke (System.Object obj, System.Reflection.BindingFlags invokeAttr, System.Reflection.Binder binder, System.Object[] parameters, System.Globalization.CultureInfo culture) [0x00083] in <6b8bdf3aa3e64e7f91da81ece11b0637>:0 
    ' at System.Reflection.MethodBase.Invoke (System.Object obj, System.Object[] parameters) [0x00000] in <6b8bdf3aa3e64e7f91da81ece11b0637>:0 
    ' at SMRUCC.Rsharp.Runtime.Interop.RMethodInfo.Invoke (System.Object[] parameters, SMRUCC.Rsharp.Runtime.Environment env) [0x000b3] in <b355be0c7d6e431380a05a956aec0fbf>:0 

    ' R# source: Call "str"(Call "iostat"())

    ' linux.R#_interop::.iostat at Profiler.dll:line <unknown>
    ' SMRUCC/R#.call_function."iostat" at linux_run.R:line 3
    ' base.R#_interop::.str at REnv.dll:line <unknown>
    ' SMRUCC/R#.call_function.str at linux_run.R:line n/a
    ' SMRUCC/R#.n/a.InitializeEnvironment at linux_run.R:line 0
    ' SMRUCC/R#.global.<globalEnvironment> at <globalEnvironment>:line n/a

    ''' <summary>
    ''' run a linux command
    ''' </summary>
    ''' <param name="command">the command name or its executative file path.</param>
    ''' <param name="args">command line arguments</param>
    ''' <param name="verbose">debug option</param>
    ''' <returns>
    ''' the std_output of the specific command.
    ''' </returns>
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Function Shell(command As String, args As String, verbose As Boolean) As String
        Dim cmdl As String

        If args.StringEmpty Then
            cmdl = command
        Else
            cmdl = $"{command} {args}"
        End If

        If verbose Then
            Call Console.WriteLine("run commandline:")
            Call Console.WriteLine($"/bin/bash -c ""{cmdl}""")
        End If

        Dim stdout As String = CommandLine.Call("/bin/bash", $"-c ""{cmdl}""", debug:=verbose)

        If verbose Then
            Call Console.WriteLine("std_output:")
            Call Console.WriteLine(stdout)
        End If

        Return stdout
    End Function

    ''' <summary>
    ''' read the file content of a specific <paramref name="file"/>
    ''' </summary>
    ''' <param name="file">the file path of the target text file.</param>
    ''' <param name="verbose"></param>
    ''' <returns></returns>
    Public Shared Function cat(file As String, verbose As Boolean) As String
        Return Shell("cat", args:=file, verbose)
    End Function
End Class
