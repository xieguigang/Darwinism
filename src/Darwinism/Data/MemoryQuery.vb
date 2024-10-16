#Region "Microsoft.VisualBasic::f8433b5a8445793a60719c3e28a1dcd3, src\Darwinism\Data\MemoryQuery.vb"

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

    '   Total Lines: 312
    '    Code Lines: 215 (68.91%)
    ' Comment Lines: 57 (18.27%)
    '    - Xml Docs: 94.74%
    ' 
    '   Blank Lines: 40 (12.82%)
    '     File Size: 11.56 KB


    ' Module MemoryQuery
    ' 
    '     Function: [select], between, BuildQuery, fulltext, hashindex
    '               load, match_against, valueindex
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports LINQ
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine.ExpressionSymbols.Closure
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine.ExpressionSymbols.DataSets
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine.ExpressionSymbols.Operators
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Interop
Imports SMRUCC.Rsharp.Runtime.Vectorization
Imports dataframe = Microsoft.VisualBasic.Data.csv.IO.DataFrame
Imports rdataframe = SMRUCC.Rsharp.Runtime.Internal.Object.dataframe
Imports renv = SMRUCC.Rsharp.Runtime
Imports RInternal = SMRUCC.Rsharp.Runtime.Internal
Imports VB = Microsoft.VisualBasic.Language.Runtime

''' <summary>
''' package tools for run in-memory query
''' </summary>
<Package("memory_query")>
Module MemoryQuery

    ''' <summary>
    ''' load in-memory table 
    ''' </summary>
    ''' <param name="x">a dataframe object, a clr object array, or the file resource to a csv dataframe file.</param>
    ''' <param name="nested_field">
    ''' use the nested property for make the clr object array index, this property name parameter value not working 
    ''' for the dataframe or the table value, only works for the generic clr array object index.
    ''' </param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("load")>
    <RApiReturn(GetType(MemoryTable))>
    Public Function load(<RRawVectorArgument>
                         x As Object,
                         Optional nested_field As String = Nothing,
                         Optional env As Environment = Nothing) As Object
        Dim df As dataframe

        If x Is Nothing Then
            Return RInternal.debug.stop("the required input data source `x` should not be nothing!", env)
        End If

        If TypeOf x Is rdataframe Then
            Dim rdf As rdataframe = x
            Dim fields As New List(Of ArgumentReference)

            With New VB
                For Each name As String In rdf.colnames
                    Call fields.Add(.Argument(name) = CLRVector.asCharacter(rdf.getColumnVector(name)))
                Next
            End With

            df = New dataframe(fields.ToArray)
        ElseIf TypeOf x Is String OrElse TypeOf x Is Stream Then
            Dim file = SMRUCC.Rsharp.GetFileStream(x, FileAccess.Read, env)

            If file Like GetType(Message) Then
                Return file.TryCast(Of Message)
            End If

            df = dataframe.Load(file.TryCast(Of Stream))
        ElseIf x.GetType.IsArray OrElse TypeOf x Is vector Then
            Dim genericArray As Array = renv.UnsafeTryCastGenericArray(CLRVector.asObject(x))
            Dim index As New MemoryPool(genericArray, [property]:=nested_field)

            Return index
        Else
            Return Message.InCompatibleType(GetType(dataframe), x.GetType, env)
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
    Public Function fulltext(x As MemoryIndex, fields As String()) As MemoryIndex
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
    Public Function hashindex(x As MemoryIndex, fields As String()) As MemoryIndex
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
    Public Function valueindex(x As MemoryIndex,
                               <RListObjectArgument>
                               fields As list,
                               Optional env As Environment = Nothing) As MemoryIndex

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

        Dim vec As Array = renv.TryCastGenericArray(renv.asVector(Of Object)(range), env)
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

    Private Function BuildQuery(query As list, env As Environment) As [Variant](Of List(Of Query), Message)
        Dim filter As New List(Of Query)

        For Each name As String In query.slotKeys
            Dim q As Object = query.getByName(name)

            If TypeOf q Is Literal OrElse TypeOf q Is FunctionInvoke Then
                q = DirectCast(q, Expression).Evaluate(env)
            ElseIf TypeOf q Is BinaryExpression Then
                Dim bin As BinaryExpression = q

                If bin.operator = ">" Then
                    q = New Query With {
                        .field = ValueAssignExpression.GetSymbol(bin.left),
                        .search = LINQ.Query.Type.ValueRangeGreaterThan,
                        .value = bin.right.Evaluate(env)
                    }
                ElseIf bin.operator = "<" Then
                    q = New Query With {
                        .field = ValueAssignExpression.GetSymbol(bin.left),
                        .search = LINQ.Query.Type.ValueRangeLessThan,
                        .value = bin.right.Evaluate(env)
                    }
                Else
                    Throw New NotImplementedException
                End If
            ElseIf TypeOf q Is Expression Then
                q = DirectCast(q, Expression).Evaluate(env)
            End If

            If TypeOf q Is Message Then
                Return q
            End If
            ' get scalar value from r# evaluation
            If TypeOf q Is vector Then
                q = DirectCast(q, vector).data.GetValueOrDefault(0)
            End If

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
                        Throw New NotImplementedException(q.GetType.FullName)
                End Select
            End If

            Call filter.Add(q)
        Next

        Return filter
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
    Public Function [select](x As MemoryIndex,
                             <RListObjectArgument>
                             <RLazyExpression>
                             query As list,
                             Optional env As Environment = Nothing) As Object

        Dim filter = BuildQuery(query, env)

        If filter Like GetType(Message) Then
            Return filter.TryCast(Of Message)
        End If

        If TypeOf x Is MemoryTable Then
            Dim tbl As MemoryTable = DirectCast(x, MemoryTable)
            Dim df As dataframe = tbl.Query(filter)
            Dim result As rdataframe

            If df Is Nothing Then
                Return Nothing
            Else
                result = New rdataframe With {
                    .columns = New Dictionary(Of String, Array)
                }
            End If

            For Each name As String In df.HeadTitles
                Call result.add(name, df.Column(name))
            Next

            Return result
        Else
            Throw New NotImplementedException
        End If
    End Function

End Module
