﻿#Region "Microsoft.VisualBasic::d9fc60e97f8ac77fdc9bf9e8bdec53c4, Rpc\Toolkit.vb"

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

'     Module Toolkit
' 
'         Constructor: (+1 Overloads) Sub New
' 
'         Function: CreateReader, CreateWriter, DumpToLog, ToDisplay
' 
'         Sub: ReplyMessageValidate
' 
' 
' /********************************************************************************/

#End Region

Imports System
Imports System.IO.XDR.Reading
Imports System.IO.XDR.Writing
Imports System.Runtime.CompilerServices
Imports System.Text
Imports Microsoft.VisualBasic.Data.IO
Imports Rpc.MessageProtocol

Namespace Rpc
    ''' <summary>
    ''' set of tools to work with RPC messages
    ''' </summary>
    Public Module Toolkit
        Private ReadOnly _wb As WriteBuilder
        Private ReadOnly _rb As ReadBuilder

        Sub New()
            _wb = New WriteBuilder()
            _rb = New ReadBuilder()
        End Sub

        ''' <summary>
        ''' create writer configured for RPC protocol
        ''' </summary>
        ''' <param name="writer"></param>
        ''' <returns></returns>
        Public Function CreateWriter(writer As IByteWriter) As Writer
            Return _wb.Create(writer)
        End Function

        ''' <summary>
        ''' create reader configured for RPC protocol
        ''' </summary>
        ''' <param name="reader"></param>
        ''' <returns></returns>
        Public Function CreateReader(reader As IByteReader) As Reader
            Return _rb.Create(reader)
        End Function

        ''' <summary>
        ''' To create a delegate output byte array to the log
        ''' </summary>
        Friend Function DumpToLog(frm As String, buffer As Byte()) As String
            Return String.Format(frm, buffer.ToDisplay())
        End Function

        ''' <summary>
        ''' convert byte array to text
        ''' </summary>
        ''' <param name="buffer"></param>
        ''' <returns></returns>
        <Extension()>
        Public Function ToDisplay(buffer As Byte()) As String
            ' example:
            ' 12345678-12345678-12345678-12345678-12345678-12345678-12345678-12345678 12345678-1234...

            Dim sb As StringBuilder = New StringBuilder()

            For i = 0 To buffer.Length - 1

                If i Mod 4 = 0 Then
                    If i Mod 32 = 0 Then
                        sb.AppendLine()
                    Else
                        sb.Append(" "c)
                    End If
                End If

                sb.Append(buffer(i).ToString("X2"))
            Next

            Return sb.ToString()
        End Function

        ''' <summary>
        ''' returns the description of the RPC message
        ''' </summary>
        ''' <param name="msg"></param>
        ''' <return></return>
        Public Sub ReplyMessageValidate(msg As rpc_msg)
            Try
                If msg.body.mtype <> msg_type.REPLY Then Throw UnexpectedMessageType(msg.body.mtype)
                Dim replyBody = msg.body.rbody

                If replyBody.stat = reply_stat.MSG_ACCEPTED Then
                    Dim du = replyBody.areply.reply_data

                    Select Case du.stat
                        Case accept_stat.GARBAGE_ARGS
                            Throw GarbageArgs()
                        Case accept_stat.PROC_UNAVAIL
                            Throw ProcedureUnavalible(replyBody)
                        Case accept_stat.PROG_MISMATCH
                            Throw ProgramMismatch(replyBody, du.mismatch_info)
                        Case accept_stat.PROG_UNAVAIL
                            Throw ProgramUnavalible(replyBody)
                        Case accept_stat.SUCCESS
                            Return
                        Case accept_stat.SYSTEM_ERR
                            Throw SystemError(replyBody)
                        Case Else
                            Throw NoRFC5531("msg")
                    End Select
                End If

                If replyBody.stat = reply_stat.MSG_DENIED Then
                    If replyBody.rreply.rstat = reject_stat.AUTH_ERROR Then
                        Throw AuthError(replyBody, replyBody.rreply.astat)
                    ElseIf replyBody.rreply.rstat = reject_stat.RPC_MISMATCH Then
                        Throw RpcVersionError(replyBody, replyBody.rreply.mismatch_info)
                    Else
                        Throw NoRFC5531("msg")
                    End If
                End If

                Throw NoRFC5531("msg")
            Catch __unusedNullReferenceException1__ As NullReferenceException
                Throw NoRFC5531("msg")
            End Try
        End Sub
    End Module
End Namespace
