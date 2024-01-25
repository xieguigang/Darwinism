#Region "Microsoft.VisualBasic::5d76115216b9dae9ba847582caf5aa8c, Parallel\IpcParallel\IDelegate.vb"

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

    ' Class IDelegate
    ' 
    '     Properties: filepath, name, type
    ' 
    '     Constructor: (+3 Overloads) Sub New
    '     Function: GetMethod, GetMethodTarget, provideType, ToString
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Reflection
Imports Microsoft.VisualBasic.Scripting.SymbolBuilder
Imports TypeInfo = Microsoft.VisualBasic.Scripting.MetaData.TypeInfo

#If NETCOREAPP Then
Imports Microsoft.VisualBasic.ApplicationServices.Development.NetCoreApp
#End If

''' <summary>
''' remote method handler
''' </summary>
Public Class IDelegate

    ''' <summary>
    ''' the function name
    ''' </summary>
    ''' <returns></returns>
    Public Property name As String
    ''' <summary>
    ''' the declared container type of the target method <see cref="name"/>
    ''' </summary>
    ''' <returns></returns>
    Public Property type As TypeInfo
    ''' <summary>
    ''' the absolute path of the assembly module file that contains target delegate
    ''' </summary>
    ''' <returns></returns>
    Public Property filepath As String

    Sub New()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="target">
    ''' The name of this target method should be unique!
    ''' </param>
    Sub New(target As MethodInfo)
        type = New TypeInfo(target.DeclaringType)
        name = target.Name
        filepath = target.DeclaringType.Assembly.Location
    End Sub

    Sub New(target As [Delegate])
        Call Me.New(target.Method)
    End Sub

    ''' <summary>
    ''' is static or instance method?
    ''' </summary>
    ''' <returns></returns>
    Public Function GetMethodTarget() As Object
        Dim type As Type = provideType()

        If VBLanguage.IsValidVBSymbolName(name) Then
            Return Nothing
        Else
            Return Activator.CreateInstance(type)
        End If
    End Function

    Private Function provideType() As Type
        Return type.GetType(knownFirst:=True, searchPath:={filepath})
    End Function

    ''' <summary>
    ''' load method data from the delegate json data
    ''' </summary>
    ''' <returns></returns>
    Public Function GetMethod() As MethodInfo
        Dim type As Type = provideType()

#If NETCOREAPP Then
        Call deps.TryHandleNetCore5AssemblyBugs(package:=type)
#End If

        If Not VBLanguage.IsValidVBSymbolName(name) Then
            For Each method As MethodInfo In CType(type, System.Reflection.TypeInfo).DeclaredMethods
                If (Not method.IsStatic) AndAlso method.Name = name Then
                    Return method
                End If
            Next
        Else
            For Each method As MethodInfo In CType(type, System.Reflection.TypeInfo).DeclaredMethods
                If method.IsStatic AndAlso method.Name = name Then
                    Return method
                End If
            Next
        End If

        Return Nothing
    End Function

    Public Overrides Function ToString() As String
        Return $"{type}::{name}"
    End Function
End Class
