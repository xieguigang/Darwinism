#Region "Microsoft.VisualBasic::d1d63509484407ff2856ed8eda58fb3f, LINQ\LINQ\Runtime\Registry.vb"

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

    '     Class Registry
    ' 
    '         Function: GetReader, GetTypeCodeName
    ' 
    '         Sub: Register
    ' 
    '     Delegate Function
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Reflection
Imports LINQ.Runtime.Drivers
Imports Microsoft.VisualBasic.Language

Namespace Runtime

    Public Class Registry

        ReadOnly drivers As New Dictionary(Of String, IDriverLoader)

        Public Sub Register(driverDll As String)
            Dim assembly As Assembly = Assembly.LoadFrom(driverDll)

            For Each type As Type In From m As Type
                                     In assembly.GetTypes
                                     Let flag As DriverFlagAttribute = m.GetCustomAttribute(Of DriverFlagAttribute)
                                     Where Not flag Is Nothing
                                     Select m

                Dim flag As DriverFlagAttribute = type.GetCustomAttribute(Of DriverFlagAttribute)()

                drivers(flag.type) = Function(args)
                                         Return Activator.CreateInstance(type, {args})
                                     End Function
            Next
        End Sub

        Public Function GetTypeCodeName(type As Type) As String
            Select Case type
                Case GetType(Integer) : Return "i32"
                Case GetType(Long) : Return "i64"
                Case Else
                    Throw New MissingPrimaryKeyException
            End Select
        End Function

        Public Function GetReader(type As String, arguments As String()) As DataSourceDriver
            If type = "row" Then
                Return New DataFrameDriver
            ElseIf drivers.ContainsKey(type) Then
                Return drivers(type)(arguments)
            Else
                Throw New MissingPrimaryKeyException(type)
            End If
        End Function
    End Class

    Public Delegate Function IDriverLoader(arguments As String()) As DataSourceDriver

End Namespace
