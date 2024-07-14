#Region "Microsoft.VisualBasic::70fb010cc99a24b6d632b1b4ac537898, mzkit\mzblender\MSIRender\MemoryPipe.vb"

' Author:
' 
'       xieguigang (gg.xie@bionovogene.com, BioNovoGene Co., LTD.)
' 
' Copyright (c) 2018 gg.xie@bionovogene.com, BioNovoGene Co., LTD.
' 
' 
' MIT License
' 
' 
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
' 
' The above copyright notice and this permission notice shall be included in all
' copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
' SOFTWARE.



' /********************************************************************************/

' Summaries:


' Code Statistics:

'   Total Lines: 66
'    Code Lines: 48 (72.73%)
' Comment Lines: 0 (0.00%)
'    - Xml Docs: 0.00%
' 
'   Blank Lines: 18 (27.27%)
'     File Size: 1.88 KB


' Class MemoryPipe
' 
'     Constructor: (+1 Overloads) Sub New
' 
'     Function: LoadImage, LoadStream, ToString
' 
'     Sub: (+3 Overloads) WriteBuffer
' 
' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.IO
Imports System.Runtime.CompilerServices

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
