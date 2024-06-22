#Region "Microsoft.VisualBasic::e7e5773ab882cf0de8fea86eebb9afee, src\Darwinism\Data\MemoryQuery.vb"

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

    '   Total Lines: 101
    '    Code Lines: 56 (55.45%)
    ' Comment Lines: 31 (30.69%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 14 (13.86%)
    '     File Size: 3.47 KB


    ' Module MemoryQuery
    ' 
    '     Function: fulltext, hashindex, load, valueindex
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports LINQ
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Interop
Imports SMRUCC.Rsharp.Runtime.Vectorization
Imports dataframe = Microsoft.VisualBasic.Data.csv.IO.DataFrame
Imports rdataframe = SMRUCC.Rsharp.Runtime.Internal.Object.dataframe

''' <summary>
''' package tools for run in-memory query
''' </summary>
<Package("memory_query")>
Module MemoryQuery

    ''' <summary>
    ''' load in-memory table 
    ''' </summary>
    ''' <param name="x">a dataframe object, or the file resource to a csv dataframe file</param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("load")>
    <RApiReturn(GetType(MemoryTable))>
    Public Function load(<RRawVectorArgument> x As Object, Optional env As Environment = Nothing) As Object
        If TypeOf x Is rdataframe Then
            Throw New NotImplementedException
        End If

        Dim file = SMRUCC.Rsharp.GetFileStream(x, FileAccess.Read, env)

        If file Like GetType(Message) Then
            Return file.TryCast(Of Message)
        End If

        Dim df As dataframe = dataframe.Load(file.TryCast(Of Stream))
        Dim table As New MemoryTable(df)

        Return table
    End Function

    ''' <summary>
    ''' set full text search index on data fields
    ''' </summary>
    ''' <param name="x"></param>
    ''' <param name="fields"></param>
    ''' <returns></returns>
    <ExportAPI("fulltext")>
    Public Function fulltext(x As MemoryTable, fields As String()) As MemoryTable
        For Each name As String In fields
            x = x.FullText(name)
        Next

        Return x
    End Function

    ''' <summary>
    ''' set hash term search index on data fields
    ''' </summary>
    ''' <param name="x"></param>
    ''' <param name="fields"></param>
    ''' <returns></returns>
    <ExportAPI("hashindex")>
    Public Function hashindex(x As MemoryTable, fields As String()) As MemoryTable
        For Each name As String In fields
            x = x.HashIndex(name)
        Next

        Return x
    End Function

    ''' <summary>
    ''' set value range search index on data fields
    ''' </summary>
    ''' <param name="x"></param>
    ''' <param name="fields">
    ''' a collection of the key-value tuples for create index fields,
    ''' field = type, type should be one of the enum terms: number,integer,date
    ''' </param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("valueindex")>
    Public Function valueindex(x As MemoryTable, <RListObjectArgument> fields As list, Optional env As Environment = Nothing) As MemoryTable
        For Each name As String In fields.getNames
            Dim desc As String = CLRVector.asCharacter(fields.getByName(name)).FirstOrDefault

            Select Case Strings.LCase(desc)
                Case "number" : x.ValueRange(name, GetType(Double))
                Case "integer" : x.ValueRange(name, GetType(Integer))
                Case "date" : x.ValueRange(name, GetType(Date))
                Case Else
                    Throw New NotImplementedException(desc)
            End Select
        Next

        Return x
    End Function

End Module

