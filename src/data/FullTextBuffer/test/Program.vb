Imports System.Globalization
Imports System.IO
Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.Repository

Namespace Test

    Friend Module Program

        Private Const SourceRoot As String = "E:\codebuddy\GCModeller\src\runtime"
        Private Const DefaultMaxFiles As Integer = 200
        Private ReadOnly Queries As String() = {"qgramfulltext", "fulltextbuffer", "invertedindex", "binarydata", "componentmodel"}

        Sub Main(args As String())
            Try
                Call Run(args)
                Call Console.WriteLine("ALL TESTS PASSED")
            Catch ex As Exception
                Call Console.Error.WriteLine(ex.ToString)
                Environment.ExitCode = 1
            End Try
        End Sub

        Private Sub Run(args As String())
            Dim maxFiles As Integer = DefaultMaxFiles

            If args.Length > 0 AndAlso Integer.TryParse(args(0), maxFiles) Then
                If maxFiles < 1 Then
                    maxFiles = DefaultMaxFiles
                End If
            End If

            Dim files As String() = System.IO.Directory.EnumerateFiles(SourceRoot, "*.vb", SearchOption.AllDirectories) _
                .Where(Function(path) Not path.Contains("\obj\", StringComparison.OrdinalIgnoreCase)) _
                .Where(Function(path) Not path.Contains("\bin\", StringComparison.OrdinalIgnoreCase)) _
                .OrderBy(Function(path) path, StringComparer.Ordinal) _
                .Take(maxFiles) _
                .ToArray()

            Assert(files.Length > 0, "no source files were found for the test corpus")

            Dim index As New QGramFullText(3)
            Dim indexed As Integer = 0

            For Each source As String In files
                Dim text As String = System.IO.File.ReadAllText(source, Encoding.UTF8)

                If Not String.IsNullOrWhiteSpace(text) Then
                    Call index.Add(text)
                    indexed += 1
                End If
            Next

            Assert(indexed > 0, "no non-empty source files were indexed")

            Dim baseline As String() = QuerySignature(index)
            Dim cacheFile As String = Path.Combine(AppContext.BaseDirectory, "qgram-fulltext-cache.bin")

            Assert(QGramFullTextCache.Save(index, cacheFile), "failed to save cache")
            Assert(System.IO.File.Exists(cacheFile), "cache file was not created")

            Dim restored As QGramFullText = QGramFullTextCache.Load(cacheFile)
            Dim signature As String() = QuerySignature(restored)

            Assert(baseline.SequenceEqual(signature, StringComparer.Ordinal), "restored index query results differ from the original index")

            Dim badFile As String = cacheFile & ".corrupt"
            Dim data As Byte() = System.IO.File.ReadAllBytes(cacheFile)

            Assert(data.Length > 64, "cache file is unexpectedly small")
            data(40) = CByte(data(40) Xor &HFFUI)
            System.IO.File.WriteAllBytes(badFile, data)

            Dim ignored As QGramFullText = Nothing
            Dim accepted As Boolean = QGramFullTextCache.TryLoad(badFile, ignored)

            Assert(Not accepted, "corrupted cache was accepted")
            Assert(ignored Is Nothing, "corrupted cache produced a non-null index")

            Call System.IO.File.Delete(badFile)

            Call Console.WriteLine($"corpus files: {files.Length}; indexed: {indexed}")
            Call Console.WriteLine($"cache size: {New FileInfo(cacheFile).Length} bytes")
            Call Console.WriteLine($"query hits: {String.Join(", ", baseline.Select(Function(item) item.Split("|"c)(1)))}")
            Call Console.WriteLine("checksum failure: rejected")
        End Sub

        Private Function QuerySignature(index As QGramFullText) As String()
            Dim signature As New List(Of String)

            For Each query As String In Queries
                Dim hits = index.Search(query, top:=5, threshold:=0).ToArray()
                Dim parts As String() = hits _
                    .Select(Function(hit) $"{hit.similarity.ToString("F6", CultureInfo.InvariantCulture)}@{hit.text.Length}:{hit.text.GetHashCode}") _
                    .ToArray()

                Call signature.Add($"{query}|{hits.Length}|{String.Join(",", parts)}")
            Next

            Return signature.ToArray()
        End Function

        Private Sub Assert(condition As Boolean, message As String)
            If Not condition Then
                Throw New InvalidOperationException("ASSERT FAILED: " & message)
            End If
        End Sub

    End Module

End Namespace
