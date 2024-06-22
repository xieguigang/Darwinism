#Region "Microsoft.VisualBasic::739c6de893bf0913b18c7371463b73b9, src\computing\Parallel\IpcParallel\Stream\IPCException.vb"

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

    '   Total Lines: 32
    '    Code Lines: 17 (53.12%)
    ' Comment Lines: 9 (28.12%)
    '    - Xml Docs: 77.78%
    ' 
    '   Blank Lines: 6 (18.75%)
    '     File Size: 1.04 KB


    '     Class IPCException
    ' 
    '         Properties: StackTrace
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.ApplicationServices.Debugging.Diagnostics

Namespace IpcStream

    Public Class IPCException : Inherits Exception

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' # https://stackoverflow.com/questions/912420/throw-exceptions-with-custom-stack-trace
        ''' 
        ''' The StackTrace property is virtual - create your own derived Exception class and have the property return whatever you want.
        ''' </remarks>
        Public Overrides ReadOnly Property StackTrace As String
            Get
                Return _stackTrace
            End Get
        End Property

        ReadOnly _stackTrace As String

        Sub New(messages As String(), stackTrace As StackFrame())
            Call MyBase.New(messages.JoinBy(" -> "))

            _stackTrace = stackTrace _
                .Select(Function(a) a.ToString) _
                .JoinBy(vbCrLf)
        End Sub
    End Class
End Namespace
