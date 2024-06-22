Imports LINQ

Module fulltext_demo

    Sub Main()
        Dim index = InMemoryDocuments.CreateEngine

        For Each path As String In {"G:\GCModeller\src\runtime\sciBASIC#\Data\TextRank\Beauty_and_the_Beast.txt",
"G:\GCModeller\src\runtime\sciBASIC#\Data\TextRank\Cinderalla.txt",
"G:\GCModeller\src\runtime\sciBASIC#\Data\TextRank\Rapunzel.txt"}

            Dim pars = path.ReadAllLines

            Call index.Indexing(pars)
        Next

        Dim search1 = index.Search("his wife").ToArray
        Dim search2 = index.Search("clock struck midnight").ToArray
        Dim search3 = index.Search("What is this").ToArray

        Pause()
    End Sub
End Module
