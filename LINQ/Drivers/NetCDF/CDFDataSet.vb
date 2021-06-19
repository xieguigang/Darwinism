#Region "Microsoft.VisualBasic::6b9b62435842858d057df49f23e18377, LINQ\Drivers\NetCDF\CDFDataSet.vb"

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

' Class CDFDataSet
' 
'     Constructor: (+1 Overloads) Sub New
'     Function: ReadFromUri
' 
' /********************************************************************************/

#End Region

Imports LINQ.Runtime.Drivers
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.IO.netCDF
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.My.JavaScript

<DriverFlag("dataframe")>
Public Class CDFDataSet : Inherits DataSourceDriver

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="arguments">should be variable name</param>
    Public Sub New(arguments() As String)
        MyBase.New(arguments)
    End Sub

    Public Overrides Iterator Function ReadFromUri(uri As String) As IEnumerable(Of Object)
        Using cdf As New netCDFReader(uri)
            Dim vectors As New List(Of NamedValue(Of Func(Of Integer, Object)))
            Dim size As Integer

            For Each name As String In arguments
                vectors += New NamedValue(Of Func(Of Integer, Object)) With {
                    .Name = name,
                    .Value = vectorReader(cdf, name, size),
                    .Description = size
                }
            Next

            Dim maxSize As Integer = vectors _
                .Select(Function(v)
                            Return Integer.Parse(v.Description)
                        End Function) _
                .Max

            For i As Integer = 0 To maxSize - 1
                Dim obj As New JavaScriptObject

                For Each field In vectors
                    obj(field.Name) = field.Value(i)
                Next

                Yield obj
            Next
        End Using
    End Function

    Private Shared Function vectorReader(cdf As netCDFReader, name As String, ByRef size As Integer) As Func(Of Integer, Object)
        Dim vec As Array = cdf.getDataVariable(name).genericValue

        size = vec.Length

        If size = 1 Then
            Dim [single] As Object = vec.GetValue(Scan0)

            Return Function(i) [single]
        Else
            Return AddressOf vec.GetValue
        End If
    End Function
End Class

