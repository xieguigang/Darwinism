Imports Microsoft.VisualBasic.Data.csv.IO
Imports Microsoft.VisualBasic.Scripting.Runtime

''' <summary>
''' an in-memory data table with search index supports
''' </summary>
Public Class MemoryTable

    ReadOnly df As DataFrame

    ReadOnly m_fulltext As New Dictionary(Of String, FTSEngine)
    ReadOnly m_hashindex As New Dictionary(Of String, TermHashIndex)
    ReadOnly m_valueindex As New Dictionary(Of String, ValueIndex)

    Sub New(df As DataFrame)
        Me.df = df
    End Sub

    Public Function FullText(field As String) As MemoryTable
        Dim col As String() = df.Column(field)
        Dim fts As FTSEngine = InMemoryDocuments.CreateFullTextSearch
        fts.Indexing(col)
        m_fulltext(field) = fts
        Return Me
    End Function

    Public Function HashIndex(field As String) As MemoryTable
        Dim col As String() = df.Column(field)
        Dim hash As TermHashIndex = InMemoryDocuments.CreateHashSearch
        hash.Indexing(col)
        m_hashindex(field) = hash
        Return Me
    End Function

    Public Function ValueRange(field As String, asType As Type) As MemoryTable
        Dim col As String() = df.Column(field)
        Dim index As ValueIndex

        Select Case asType
            Case GetType(Integer) : index = ValueIndex.IntegerIndex.IndexData(col.AsInteger)
            Case GetType(Double) : index = ValueIndex.DoubleIndex.IndexData(col.AsDouble)
            Case GetType(Date) : index = ValueIndex.DateIndex.IndexData(col.AsDateTime)
            Case Else
                Throw New NotImplementedException(asType.FullName)
        End Select

        m_valueindex(field) = index

        Return Me
    End Function

End Class
