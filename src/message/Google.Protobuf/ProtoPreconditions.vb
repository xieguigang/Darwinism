#Region "Microsoft.VisualBasic::2a9330c5393e2ce58116b7eaed5f8593, src\message\Google.Protobuf\ProtoPreconditions.vb"

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

    '   Total Lines: 74
    '    Code Lines: 18 (24.32%)
    ' Comment Lines: 51 (68.92%)
    '    - Xml Docs: 43.14%
    ' 
    '   Blank Lines: 5 (6.76%)
    '     File Size: 3.47 KB


    '     Module ProtoPreconditions
    ' 
    '         Function: CheckNotNull, CheckNotNullUnconstrained
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


Namespace Google.Protobuf
    ''' <summary>
    ''' Helper methods for throwing exceptions when preconditions are not met.
    ''' </summary>
    ''' <remarks>
    ''' This class is used internally and by generated code; it is not particularly
    ''' expected to be used from application code, although nothing prevents it
    ''' from being used that way.
    ''' </remarks>
    Public Module ProtoPreconditions
        ''' <summary>
        ''' Throws an ArgumentNullException if the given value is null, otherwise
        ''' return the value to the caller.
        ''' </summary>
        Public Function CheckNotNull(Of T As Class)(value As T, name As String) As T
            If value Is Nothing Then
                Throw New ArgumentNullException(name)
            End If

            Return value
        End Function

        ''' <summary>
        ''' Throws an ArgumentNullException if the given value is null, otherwise
        ''' return the value to the caller.
        ''' </summary>
        ''' <remarks>
        ''' This is equivalent to <see cref="CheckNotNull(OfT)(T,String)"/> but without the type parameter
        ''' constraint. In most cases, the constraint is useful to prevent you from calling CheckNotNull
        ''' with a value type - but it gets in the way if either you want to use it with a nullable
        ''' value type, or you want to use it with an unconstrained type parameter.
        ''' </remarks>
        Friend Function CheckNotNullUnconstrained(Of T)(value As T, name As String) As T
            If value Is Nothing Then
                Throw New ArgumentNullException(name)
            End If

            Return value
        End Function
    End Module
End Namespace
