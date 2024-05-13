#Region "Microsoft.VisualBasic::85a637df5f263ddb8e3a2f5ea65ac4da, src\message\Google.Protobuf\ICustomDiagnosticMessage.vb"

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

    '   Total Lines: 70
    '    Code Lines: 8
    ' Comment Lines: 59
    '   Blank Lines: 3
    '     File Size: 3.82 KB


    '     Interface ICustomDiagnosticMessage
    ' 
    '         Function: ToDiagnosticString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

#Region "Copyright notice and license"
' Protocol Buffers - Google's data interchange format
' Copyright 2016 Google Inc.  All rights reserved.
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

Namespace Google.Protobuf

    ''' <summary>
    ''' A message type that has a custom string format for diagnostic purposes.
    ''' </summary>
    ''' <remarks>
    ''' <para>
    ''' Calling <see cref="Object.ToString"/> on a generated message type normally
    ''' returns the JSON representation. If a message type implements this interface,
    ''' then the <see cref="ToDiagnosticString"/> method will be called instead of the regular
    ''' JSON formatting code, but only when <c>ToString()</c> is called either on the message itself
    ''' or on another message which contains it. This does not affect the normal JSON formatting of
    ''' the message.
    ''' </para>
    ''' <para>
    ''' For example, if you create a proto message representing a GUID, the internal
    ''' representation may be a <c>bytes</c> field or four <c>fixed32</c> fields. However, when debugging
    ''' it may be more convenient to see a result in the same format as <see cref="System.Guid"/> provides.
    ''' </para>
    ''' <para>This interface extends <see cref="IMessage"/> to avoid it accidentally being implemented
    ''' on types other than messages, where it would not be used by anything in the framework.</para>
    ''' </remarks>
    Public Interface ICustomDiagnosticMessage
        Inherits IMessage

        ''' <summary>
        ''' Returns a string representation of this object, for diagnostic purposes.
        ''' </summary>
        ''' <remarks>
        ''' This method is called when a message is formatted as part of a <see cref="Object.ToString"/>
        ''' call. It does not affect the JSON representation used by <see cref="JsonFormatter"/> other than
        ''' in calls to <see cref="JsonFormatter.ToDiagnosticString(IMessage)"/>. While it is recommended
        ''' that the result is valid JSON, this is never assumed by the Protobuf library.
        ''' </remarks>
        ''' <returns>A string representation of this object, for diagnostic purposes.</returns>
        Function ToDiagnosticString() As String
    End Interface
End Namespace
