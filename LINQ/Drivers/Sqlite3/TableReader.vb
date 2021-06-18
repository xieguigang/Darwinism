#Region "Microsoft.VisualBasic::fe7c1b7eeec229a2c9042fe003b3cb67, LINQ\Drivers\Sqlite3\TableReader.vb"

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

    ' Class TableReader
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: ReadFromUri
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Runtime.Drivers
Imports Microsoft.VisualBasic.Data.IO.ManagedSqlite.Core
Imports Microsoft.VisualBasic.Data.IO.ManagedSqlite.Core.SQLSchema
Imports Microsoft.VisualBasic.Data.IO.ManagedSqlite.Core.Tables
Imports Microsoft.VisualBasic.My.JavaScript

<DriverFlag("table")>
Public Class TableReader : Inherits DataSourceDriver

    Public Sub New(arguments() As String)
        MyBase.New(arguments)
    End Sub

    Public Overrides Iterator Function ReadFromUri(uri As String) As IEnumerable(Of Object)
        Dim tableName As String = arguments(Scan0)
        Dim sqlite As Sqlite3Database = Sqlite3Database.OpenFile(dbFile:=uri)
        Dim rawRef As Sqlite3Table = sqlite.GetTable(tableName)
        Dim schema As Schema = rawRef.SchemaDefinition.ParseSchema
        Dim colnames As String() = schema.columns.Select(Function(c) c.Name).ToArray

        For Each row As Sqlite3Row In rawRef.EnumerateRows
            Dim jsObj As New JavaScriptObject

            For i As Integer = 0 To colnames.Length - 1
                jsObj(colnames(i)) = row.Item(i)
            Next

            Yield jsObj
        Next
    End Function
End Class
