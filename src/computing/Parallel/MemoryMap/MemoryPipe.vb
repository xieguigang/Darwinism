#Region "Microsoft.VisualBasic::799078c734e6bc1244faca4d88ba3ef5, src\computing\Parallel\MemoryMap\MemoryPipe.vb"

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

    '   Total Lines: 82
    '    Code Lines: 50 (60.98%)
    ' Comment Lines: 12 (14.63%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 20 (24.39%)
    '     File Size: 2.16 KB


    ' Class MemoryPipe
    ' 
    '     Constructor: (+1 Overloads) Sub New
    ' 
    '     Function: LoadImage, LoadStream, ToString
    ' 
    '     Sub: (+2 Overloads) WriteBuffer
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports System.Runtime.CompilerServices

#If NET48 Then
Imports Image = System.Drawing.Image
#Else
Imports Image = Microsoft.VisualBasic.Imaging.Image
#End If

Public Class MemoryPipe

    Dim channel As MapObject

    Sub New(channel As MapObject)
        Me.channel = channel

        If Not MapObject.Exists(channel) Then
            Call MapObject.Allocate(channel)
        End If
    End Sub

    ''' <summary>
    ''' read data from memory
    ''' </summary>
    ''' <returns></returns>
    Public Function LoadStream() As Byte()
        Dim file As Stream = channel.OpenFile
        Dim bytes As Byte() = New Byte(4 - 1) {}

        Call file.Seek(Scan0, SeekOrigin.Begin)
        Call file.Read(bytes, Scan0, bytes.Length)

        bytes = New Byte(BitConverter.ToInt32(bytes, Scan0) - 1) {}

        Call file.Read(bytes, Scan0, bytes.Length)

        Return bytes
    End Function

#Disable Warning

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function LoadImage() As Image
        Return Image.FromStream(New MemoryStream(LoadStream))
    End Function

#Enable Warning

    ''' <summary>
    ''' save data into memory
    ''' </summary>
    ''' <param name="data"></param>
    Public Sub WriteBuffer(data As MemoryStream)
        Dim buf As Stream = channel.OpenFile

        Call buf.Seek(Scan0, SeekOrigin.Begin)
        Call buf.Write(BitConverter.GetBytes(data.Length), 0, 4)
        Call buf.Write(data.ToArray, Scan0, data.Length)
        Call buf.Flush()

        data.Dispose()
    End Sub

    ''' <summary>
    ''' save data into memory
    ''' </summary>
    ''' <param name="data"></param>
    Public Sub WriteBuffer(ByRef data As Byte())
        Dim buf As Stream = channel.OpenFile

        Call buf.Seek(Scan0, SeekOrigin.Begin)
        Call buf.Write(BitConverter.GetBytes(data.Length), 0, 4)
        Call buf.Write(data, Scan0, data.Length)
        Call buf.Flush()

        Erase data
    End Sub

    Public Overrides Function ToString() As String
        Return channel.ToString
    End Function
End Class
