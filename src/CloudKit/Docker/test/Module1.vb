#Region "Microsoft.VisualBasic::a754bf01330f73d1889d37788fb98aec, src\CloudKit\Docker\test\Module1.vb"

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

    '   Total Lines: 39
    '    Code Lines: 22
    ' Comment Lines: 3
    '   Blank Lines: 14
    '     File Size: 1.18 KB


    ' Module Module1
    ' 
    '     Sub: Main, testPuTTY
    ' 
    ' /********************************************************************************/

#End Region

Imports Darwinism
Imports Darwinism.Centos
Imports Darwinism.Docker.Arguments
Imports Microsoft.VisualBasic.Serialization.JSON

Module Module1

    Sub Main()

        Call testPuTTY()

        Dim ps As New Docker.PowerShell

        ' Call Console.WriteLine(ps.RunScript("docker ps"))

        '   Call Console.WriteLine(Docker.PS.ToArray.GetJson)

        '    Call Console.WriteLine(Docker.Search("centos").ToArray.GetJson)


        Call Console.WriteLine(Docker.Run("centos", "echo ""hello world"""))
        Call Console.WriteLine(Docker.Run("centos", "ls -l /mnt/ntfs", mounts:={New Mount With {.local = "D:\test", .virtual = "/mnt/ntfs"}}))

        For Each line In Docker.CommandHistory
            Call Console.WriteLine(line)
        Next

        Pause()
    End Sub

    Sub testPuTTY()
        Dim ssh As New PuTTY("xcms", "12345", "192.168.1.253")

        Call Console.WriteLine(ssh.Shell("ls", "-l /mnt/smb/tmp/test_watcher/p+n/neg/mzML"))
        Call Console.WriteLine(ssh.Shell("Rscript", "-e \""biodeep::run.Deconvolution(raw = '/mnt/smb/tmp/test_watcher/p+n/neg/mzML');\"""))

        Pause()
    End Sub
End Module
