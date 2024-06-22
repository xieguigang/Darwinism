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
