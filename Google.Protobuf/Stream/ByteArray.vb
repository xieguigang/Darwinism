#Region "Microsoft.VisualBasic::a4c0c3e042dda9daf9bcfea730effec9, Google.Protobuf\Stream\ByteArray.vb"

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

    '     Module ByteArray
    ' 
    '         Sub: Copy, Reverse
    ' 
    ' 
    ' /********************************************************************************/

#End Region

#Region "Copyright notice and license"
' Protocol Buffers - Google's data interchange format
' Copyright 2008 Google Inc.  All rights reserved.
' https://developers.google.com/protocol-buffers/
'
' Redistribution and use in source and binary forms, with or without
' modification, are permitted provided that the following conditions are
' met:
'
'     * Redistributions of source code must retain the above copyright
' notice, this list of conditions and the following disclaimer.
'     * Redistributions in binary form must reproduce the above
' copyright notice, this list of conditions and the following disclaimer
' in the documentation and/or other materials provided with the
' distribution.
'     * Neither the name of Google Inc. nor the names of its
' contributors may be used to endorse or promote products derived from
' this software without specific prior written permission.
'
' THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
' "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
' LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
' A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
' OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
' SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
' LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
' DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
' THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
' (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
' OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#End Region

Imports System

Namespace Google.Protobuf
    ''' <summary>
    ''' Provides a utility routine to copy small arrays much more quickly than Buffer.BlockCopy
    ''' </summary>
    Friend Module ByteArray
        ''' <summary>
        ''' The threshold above which you should use Buffer.BlockCopy rather than ByteArray.Copy
        ''' </summary>
        Private Const CopyThreshold As Integer = 12

        ''' <summary>
        ''' Determines which copy routine to use based on the number of bytes to be copied.
        ''' </summary>
        Friend Sub Copy(src As Byte(), srcOffset As Integer, dst As Byte(), dstOffset As Integer, count As Integer)
            If count > CopyThreshold Then
                Buffer.BlockCopy(src, srcOffset, dst, dstOffset, count)
            Else
                Dim [stop] = srcOffset + count

                For i = srcOffset To [stop] - 1
                    dst(Math.Min(Threading.Interlocked.Increment(dstOffset), dstOffset - 1)) = src(i)
                Next
            End If
        End Sub

        ''' <summary>
        ''' Reverses the order of bytes in the array
        ''' </summary>
        Friend Sub Reverse(bytes As Byte())
            Dim first = 0, last = bytes.Length - 1

            While first < last
                Dim temp = bytes(first)
                bytes(first) = bytes(last)
                bytes(last) = temp
                first += 1
                last -= 1
            End While
        End Sub
    End Module
End Namespace
