#Region "Microsoft.VisualBasic::5ca3eb51ca553c6b02999585c339fb67, src\data\LINQ\LINQ\MemoryQuery\MemoryTable.vb"

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

    '   Total Lines: 54
    '    Code Lines: 30 (55.56%)
    ' Comment Lines: 15 (27.78%)
    '    - Xml Docs: 93.33%
    ' 
    '   Blank Lines: 9 (16.67%)
    '     File Size: 1.74 KB


    ' Class MemoryTable
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: CheckScalar, GetData, Query
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.Data.Framework.IO
Imports Microsoft.VisualBasic.Scripting.Runtime

''' <summary>
''' an in-memory data table with search index supports
''' </summary>
Public Class MemoryTable : Inherits MemoryIndex

    ReadOnly df As DataFrame

    Sub New(df As DataFrame)
        Me.df = df
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="filter"></param>
    ''' <returns>
    ''' this function will returns nothing if query filter has no result
    ''' </returns>
    Public Function Query(filter As IEnumerable(Of Query)) As DataFrame
        Dim index As Integer() = GetIndex(filter)

        If index.IsNullOrEmpty Then
            Return Nothing
        End If

        Return df.Slice(index)
    End Function

    Protected Overrides Function GetData(Of T)(field As String) As T()
        Select Case GetType(T)
            Case GetType(String) : Return CObj(df.Column(field))
            Case GetType(Integer) : Return CObj(df.Column(field).AsInteger)
            Case GetType(Double) : Return CObj(df.Column(field).AsDouble)
            Case GetType(Date) : Return CObj(df.Column(field).AsDateTime)
            Case GetType(Boolean) : Return CObj(df.Column(field).AsBoolean)

            Case Else
                Throw New NotImplementedException(GetType(T).FullName)
        End Select
    End Function

    ''' <summary>
    ''' data fields element type in dataframe always scalar
    ''' </summary>
    ''' <param name="field"></param>
    ''' <returns></returns>
    Protected Overrides Function CheckScalar(field As String) As Boolean
        Return True
    End Function
End Class
