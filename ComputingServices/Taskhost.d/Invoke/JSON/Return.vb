#Region "Microsoft.VisualBasic::a16b41f17fe4b9dd0c69d4d469297120, ComputingServices\Taskhost.d\Invoke\Return.vb"

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

'     Class Rtvl
' 
'         Properties: errCode, ex, value
' 
'         Constructor: (+3 Overloads) Sub New
'         Function: CreateObject, (+2 Overloads) GetValue
' 
' 
' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.ApplicationServices.Debugging.Diagnostics
Imports Microsoft.VisualBasic.Net.Http
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace TaskHost

    ''' <summary>
    ''' The returns value.(远端调用的函数返回)
    ''' </summary>
    ''' <remarks>
    ''' 因为在远端是一个通用的计算结果,所以没有办法使用泛型
    ''' </remarks>
    Public Class Rtvl

        ''' <summary>
        ''' 200 OK
        ''' 500 Error
        ''' </summary>
        ''' <returns></returns>
        Public Property errCode As Integer

        ''' <summary>
        ''' The exception message
        ''' </summary>
        ''' <returns></returns>
        Public Property message As ExceptionData

        ''' <summary>
        ''' The result value in json string format 
        ''' </summary>
        ''' <returns></returns>
        Public Property info As Argument

        Sub New()
        End Sub

        Sub New(ex As Exception)
            errCode = HTTP_RFC.RFC_INTERNAL_SERVER_ERROR
            message = ExceptionData.CreateFromObject(ex)
        End Sub

        Sub New(value As Object, type As Type)
            errCode = HTTP_RFC.RFC_OK
            info = New Argument(value, type)
        End Sub

        ''' <summary>
        ''' If the remote execute raising a exception, 
        ''' then a exception will be throw from this function.
        ''' </summary>
        ''' <returns></returns>
        Public Function GetValue() As Object
            If errCode <> HTTP_RFC.RFC_OK Then
                Throw message.Exception
            Else
                Return info.GetValue()
            End If
        End Function

        Public Shared Function CreateObject(Of T)(x As T) As Rtvl
            Return New Rtvl(x, GetType(T))
        End Function
    End Class
End Namespace
