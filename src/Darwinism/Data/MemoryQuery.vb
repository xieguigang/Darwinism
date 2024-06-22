#Region "Microsoft.VisualBasic::8770e9d95a75208d1641b127867a6a53, src\Darwinism\Data\MemoryQuery.vb"

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

    '   Total Lines: 225
    '    Code Lines: 144 (64.00%)
    ' Comment Lines: 52 (23.11%)
    '    - Xml Docs: 96.15%
    ' 
    '   Blank Lines: 29 (12.89%)
    '     File Size: 7.83 KB


    ' Module MemoryQuery
    ' 
    '     Function: [select], between, fulltext, hashindex, load
    '               match_against, valueindex
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports LINQ
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Interop
Imports SMRUCC.Rsharp.Runtime.Vectorization
Imports dataframe = Microsoft.VisualBasic.Data.csv.IO.DataFrame
Imports rdataframe = SMRUCC.Rsharp.Runtime.Internal.Object.dataframe
Imports renv = SMRUCC.Rsharp.Runtime
Imports VB = Microsoft.VisualBasic.Language.Runtime

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
        Dim df As dataframe

        If TypeOf x Is rdataframe Then
            Dim rdf As rdataframe = x
            Dim fields As New List(Of ArgumentReference)

            With New VB
                For Each name As String In rdf.colnames
                    Call fields.Add(.Argument(name) = CLRVector.asCharacter(rdf.getColumnVector(name)))
                Next
            End With

            df = New dataframe(fields)
        Else
            Dim file = SMRUCC.Rsharp.GetFileStream(x, FileAccess.Read, env)

            If file Like GetType(Message) Then
                Return file.TryCast(Of Message)
            End If

            df = dataframe.Load(file.TryCast(Of Stream))
        End If

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

    ''' <summary>
    ''' create a full text search filter
    ''' </summary>
    ''' <param name="name"></param>
    ''' <param name="text"></param>
    ''' <returns></returns>
    <ExportAPI("match_against")>
    Public Function match_against(name As String, text As String) As Query
        Return New Query With {
            .field = name,
            .search = Query.Type.FullText,
            .value = text
        }
    End Function

    <ExportAPI("between")>
    Public Function between(name As String,
                            <RRawVectorArgument>
                            range As Object,
                            Optional env As Environment = Nothing) As Query

        Dim vec As Array = renv.TryCastGenericArray(range, env)
        Dim mode = RType.TypeOf(vec)

        Select Case mode.mode
            Case TypeCodes.double : vec = CLRVector.asNumeric(vec)
            Case TypeCodes.integer : vec = CLRVector.asInteger(vec)
            Case Else
                Select Case mode.GetRawElementType
                    Case GetType(Date) : vec = CLRVector.asDate(vec)
                    Case Else
                        Throw New NotImplementedException
                End Select
        End Select

        Return New Query With {
            .field = name,
            .search = Query.Type.ValueRange,
            .value = vec
        }
    End Function

    ''' <summary>
    ''' make dataframe query
    ''' </summary>
    ''' <param name="x"></param>
    ''' <param name="query"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    ''' <example>
    ''' x 
    ''' |> select(match_against("field1", "full text value"), between("field2", [min, max]), field3 = "xxxxx")
    ''' |> print()
    ''' ;
    ''' </example>
    <ExportAPI("select")>
    Public Function [select](x As MemoryTable, <RListObjectArgument> query As list, Optional env As Environment = Nothing) As Object
        Dim filter As New List(Of Query)

        For Each name As String In query.slotKeys
            Dim q As Object = query.getByName(name)

            If TypeOf q Is Query Then
                ' do nothing
            Else
                ' a = b filter expression 
                Select Case q.GetType
                    Case GetType(String)
                        q = New Query With {
                            .field = name,
                            .search = LINQ.Query.Type.HashTerm,
                            .value = q
                        }
                    Case GetType(Double), GetType(Single)
                        q = New Query With {
                            .field = name,
                            .search = LINQ.Query.Type.ValueMatch,
                            .value = CDbl(q)
                        }
                    Case GetType(Integer), GetType(Long)
                        q = New Query With {
                            .field = name,
                            .search = LINQ.Query.Type.ValueMatch,
                            .value = CInt(q)
                        }
                    Case GetType(Date)
                        q = New Query With {
                            .field = name,
                            .search = LINQ.Query.Type.ValueMatch,
                            .value = CDate(q)
                        }
                    Case Else
                        Throw New NotImplementedException
                End Select
            End If

            Call filter.Add(q)
        Next

        Dim df As dataframe = x.Query(filter)
        Dim result As New rdataframe

        For Each name As String In df.HeadTitles
            Call result.add(name, df.Column(name))
        Next

        Return result
    End Function

End Module
