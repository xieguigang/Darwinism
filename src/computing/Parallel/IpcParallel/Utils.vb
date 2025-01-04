#Region "Microsoft.VisualBasic::4fa03b8069e703b0bad4c85b2f61f65c, src\computing\Parallel\IpcParallel\Utils.vb"

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

    '   Total Lines: 57
    '    Code Lines: 40 (70.18%)
    ' Comment Lines: 6 (10.53%)
    '    - Xml Docs: 83.33%
    ' 
    '   Blank Lines: 11 (19.30%)
    '     File Size: 1.50 KB


    ' Module BackgroundTaskUtils
    ' 
    '     Sub: BindToMaster, checkMasterHeartbeat
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Threading

Public Module BackgroundTaskUtils

    ''' <summary>
    ''' if parent is exists then kill current web server 
    ''' </summary>
    ''' <param name="parentId"></param>
    ''' <param name="kill"></param>
    Public Sub BindToMaster(parentId As String, kill As IDisposable)
        ' not specific the parent process id
        If parentId.StringEmpty OrElse Val(parentId) <= 0 Then
            Return
        Else
#If WINDOWS Then
            Dim task As New ThreadStart(
                Sub()
                    BackgroundTaskUtils.checkMasterHeartbeat(Integer.Parse(parentId), kill)
                End Sub)

            Call New Thread(task).Start()
#End If
        End If
    End Sub

    Private Sub checkMasterHeartbeat(parentId As Integer, kill As IDisposable)
        Dim parent As Process

        Try
            parent = Process.GetProcessById(parentId)
        Catch ex As Exception
            Call kill.Dispose()
            Return
        End Try

        Do While App.Running
            Try
                If parent.HasExited Then
                    Call kill.Dispose()
                    Call App.Exit()

                    Exit Do
                End If
            Catch ex As Exception
                Call kill.Dispose()
                Call App.Exit()

                Exit Do
            End Try

            Call Thread.Sleep(100)
        Loop

        Call App.Exit()
    End Sub

End Module
