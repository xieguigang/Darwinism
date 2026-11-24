Imports System.Globalization
Imports LINQ.Interpreter
Imports LINQ.Interpreter.Query
Imports LINQ.Language
Imports LINQ.Runtime
Imports LINQ.Script
Imports Microsoft.VisualBasic.ApplicationServices.Debugging
Imports Microsoft.VisualBasic.Data.Framework.StorageProvider
Imports Microsoft.VisualBasic.My.JavaScript
Imports any = Microsoft.VisualBasic.Scripting

''' <summary>
''' LINQ 脚本查询引擎的索引查询与聚合查询测试程序。
''' 全部的测试结果都通过<c>DebugAudit</c>模块记录，并且导出为JSON格式的测试报告。
''' </summary>
Public Module Program

    ''' <summary>
    ''' 测试数据以及脚本文件所位于的文件夹
    ''' </summary>
    Dim baseDir As String
    Dim m_dataFrame As DataFrameResolver

    Public Function Main(args As String()) As Integer
        baseDir = If(args.Length > 0, args(0), AppContext.BaseDirectory)
        System.Environment.CurrentDirectory = baseDir

        If args.Length > 1 AndAlso args(1) = "--dump" Then
            Call Probe.DumpTokens(IO.File.ReadAllText($"{baseDir}/query_aggregate.linq"))

            Return 0
        End If

        Call DebugAudit.Snapshot(
            ("baseDir", baseDir),
            ("dataRows", GetDataFrame().Nrows),
            ("columns", GetDataFrame().HeadTitles.Length)
        )

        Call Invoke("script-parsing", AddressOf TestScriptParsing)
        Call Invoke("select-scan", AddressOf TestSelectScan)
        Call Invoke("select-index", AddressOf TestSelectIndex)
        Call Invoke("select-equivalence", AddressOf TestSelectEquivalence)
        Call Invoke("where-logic", AddressOf TestWhereLogic)
        Call Invoke("order-by-pushdown", AddressOf TestOrderByPushdown)
        Call Invoke("aggregate", AddressOf TestAggregate)
        Call Invoke("aggregate-index", AddressOf TestAggregateIndex)
        Call Invoke("aggregate-group", AddressOf TestAggregateGroup)
        Call Invoke("aggregate-equivalence", AddressOf TestAggregateEquivalence)

        Dim report As String = $"{baseDir}/indexed_query_test_report.json"

        Call PrintGroupAggregate()
        Call DebugAudit.SaveReport(report)
        Call Console.WriteLine(DebugAudit.Dump(includeRecords:=True))
        Call Console.WriteLine($"test report: {report}")

        Return If(DebugAudit.FailureCount = 0, 0, 1)
    End Function

    ''' <summary>
    ''' 将分组聚合查询的结果打印到控制台，方便人工检查聚合计算的数值
    ''' </summary>
    Private Sub PrintGroupAggregate()
        Dim q As QueryExpression = Nothing
        Dim rows As JavaScriptObject() = RunFile("query_aggregate_group.linq", q)

        Call Console.WriteLine()
        Call Console.WriteLine("GROUP BY Class (WHERE PeakQuality >= 0.9)")
        Call Console.WriteLine("Class	n	avgQuality	maxMz	sumArea")

        For Each row As JavaScriptObject In rows
            Call Console.WriteLine(
                String.Join(vbTab, {
                    Text(row("Class")),
                    Text(row("n")),
                    Text(row("avgQuality")),
                    Text(row("maxMz")),
                    Text(row("sumArea"))
                })
            )
        Next

        Call Console.WriteLine()
    End Sub

    Private Sub Invoke(name As String, test As Action)
        Call DebugAudit.Mark($"{name} begin")

        Try
            Call test()
        Catch ex As Exception
            Call DebugAudit.Check(False, $"{name} throws {ex.GetType.Name}: {ex.Message}")
        End Try

        Call DebugAudit.Mark($"{name} end")
    End Sub

#Region "query helpers"

    Private Function RunQuery(script As String, ByRef query As QueryExpression) As JavaScriptObject()
        Dim tokens As Token() = LINQ.Language.GetTokens(script).ToArray
        Dim env As New GlobalEnvironment(New Registry)
        Dim context As New ExecutableContext With {.env = env, .throwError = True}

        query = tokens.PopulateQueryExpression

        Return DirectCast(query.Exec(context), JavaScriptObject())
    End Function

    Private Function RunFile(name As String, ByRef query As QueryExpression) As JavaScriptObject()
        Return RunQuery(IO.File.ReadAllText($"{baseDir}/{name}"), query)
    End Function

    Private Function GetDataFrame() As DataFrameResolver
        If m_dataFrame Is Nothing Then
            m_dataFrame = DataFrameResolver.Load($"{baseDir}/data.csv").MeasureTypeSchema
        End If

        Return m_dataFrame
    End Function

    Private Function Column(name As String) As Object()
        Dim df As DataFrameResolver = GetDataFrame()
        Dim offset As Integer = df.SchemaOridinal(name)

        Return df _
            .EnumerateRowObjects() _
            .Select(Function(row) row(offset)) _
            .ToArray
    End Function

    Private Function Text(value As Object) As String
        Return any.ToString(value)
    End Function

    Private Function ToDouble(value As Object) As Double
        If value Is Nothing Then
            Return 0
        End If

        Dim d As Double

        If Double.TryParse(Text(value), NumberStyles.Float, CultureInfo.InvariantCulture, d) Then
            Return d
        Else
            Return 0
        End If
    End Function

    Private Sub AssertNear(expected As Double, actual As Double, name As String)
        Dim ok As Boolean = Math.Abs(expected - actual) <= Math.Max(0.000001, Math.Abs(expected) * 0.000000001)

        Call DebugAudit.Check(ok, $"{name}: expected {expected} but actually is {actual}")
    End Sub

#End Region

#Region "script parsing test"

    ''' <summary>
    ''' 现有的查询脚本都应当可以被正确地解析为查询表达式对象
    ''' </summary>
    Private Sub TestScriptParsing()
        For Each name As String In {
            "cdf.linq",
            "sqlite.linq",
            "sqlite_cli.linq",
            "query.linq",
            "query_index.linq",
            "query_aggregate.linq",
            "query_aggregate_index.linq",
            "query_aggregate_group.linq"
        }
            Dim tokens As Token() = LINQ.Language.GetTokens(IO.File.ReadAllText($"{baseDir}/{name}")).ToArray
            Dim query As QueryExpression = Nothing

            Try
                query = tokens.PopulateQueryExpression
            Catch ex As Exception
                Call DebugAudit.Check(False, $"failed to parse '{name}': {ex.Message}")
                Continue For
            End Try

            Call DebugAudit.Check(query IsNot Nothing, $"the script '{name}' should be parsed")
        Next
    End Sub

#End Region

#Region "SELECT query test"

    Private Sub TestSelectScan()
        Dim q As QueryExpression = Nothing
        Dim rows As JavaScriptObject() = RunFile("query.linq", q)

        Call DebugAudit.AreEqual(15, rows.Length, "query.linq should returns 15 rows")
        Call DebugAudit.Check(q.LastIndex Is Nothing, "query.linq should not use any index")

        For Each row As JavaScriptObject In rows
            Call DebugAudit.Check(ToDouble(row("PeakQuality")) >= 0.999, "the peak quality filter was not applied!")
        Next
    End Sub

    Private Sub TestSelectIndex()
        Dim q As QueryExpression = Nothing
        Dim rows As JavaScriptObject() = RunFile("query_index.linq", q)

        Call DebugAudit.AreEqual(15, rows.Length, "query_index.linq should returns 15 rows")
        Call DebugAudit.Check(q.LastIndex IsNot Nothing AndAlso q.LastIndex.Used, "query_index.linq should use the data source index!")
        Call DebugAudit.Check(q.LastIndex.Addresses.Length < GetDataFrame().Nrows, "the index should reduce the candidate rows")

        Call DebugAudit.Capture("indexCandidates", q.LastIndex.Addresses.Length)

        For Each row As JavaScriptObject In rows
            Call DebugAudit.Check(ToDouble(row("PeakQuality")) >= 0.999, "the peak quality filter was not applied!")
        Next
    End Sub

    Private Sub TestSelectEquivalence()
        Dim scanQuery As QueryExpression = Nothing
        Dim indexQuery As QueryExpression = Nothing
        Dim scan As JavaScriptObject() = RunFile("query.linq", scanQuery)
        Dim index As JavaScriptObject() = RunFile("query_index.linq", indexQuery)

        Call DebugAudit.AreEqual(scan.Length, index.Length, "the indexed query should returns the same row count")

        For i As Integer = 0 To scan.Length - 1
            For Each name As String In scan(i).GetNames
                Call DebugAudit.AreEqual(
                    Text(scan(i)(name)),
                    Text(index(i)(name)),
                    $"row {i} field '{name}' value mismatch between the scan query and the indexed query"
                )
            Next
        Next
    End Sub

#End Region

#Region "WHERE logic test"

    Private Sub TestWhereLogic()
        Dim q As QueryExpression = Nothing
        Dim rows As JavaScriptObject() = RunQuery(
            String.Join(vbLf, {
                "from x as row",
                "in ""data.csv""",
                "where x.Class = ""PC"" and x.PeakQuality >= 0.9",
                "select x.Class, x.PeakQuality, x.CalcMz"
            }), q)

        Dim classes As Object() = Column("Class")
        Dim qualities As Object() = Column("PeakQuality")
        Dim expectAnd As Integer = 0

        For i As Integer = 0 To classes.Length - 1
            If String.Equals(Text(classes(i)), "PC", StringComparison.Ordinal) AndAlso ToDouble(qualities(i)) >= 0.9 Then
                expectAnd += 1
            End If
        Next

        Call DebugAudit.AreEqual(expectAnd, rows.Length, "the 'and' logic query result")

        Dim q2 As QueryExpression = Nothing
        Dim rows2 As JavaScriptObject() = RunQuery(
            String.Join(vbLf, {
                "from x as row",
                "in ""data.csv""",
                "where not (x.Class = ""PC"") and x.CalcMz > 700",
                "select x.Class, x.CalcMz"
            }), q2)
        Dim mz As Object() = Column("CalcMz")
        Dim expectNot As Integer = 0

        For i As Integer = 0 To classes.Length - 1
            If Not String.Equals(Text(classes(i)), "PC", StringComparison.Ordinal) AndAlso ToDouble(mz(i)) > 700 Then
                expectNot += 1
            End If
        Next

        Call DebugAudit.AreEqual(expectNot, rows2.Length, "the 'not' logic query result")
    End Sub

#End Region

#Region "order by pushdown test"

    ''' <summary>
    ''' 排序字段建立了数值索引的时候，ORDER BY操作可以直接按照索引键的顺序取出数据行
    ''' </summary>
    Private Sub TestOrderByPushdown()
        Dim indexScript As String = String.Join(vbLf, {
            "from x as row",
            "in index(""data.csv"", range = [""CalcMz""])",
            "where x.CalcMz > 700",
            "select x.LipidIon, x.CalcMz",
            "order by CalcMz ascending",
            "take 20"
        })
        Dim scanScript As String = indexScript.Replace("index(""data.csv"", range = [""CalcMz""])", """data.csv""")
        Dim indexQuery As QueryExpression = Nothing
        Dim scanQuery As QueryExpression = Nothing
        Dim indexRows As JavaScriptObject() = RunQuery(indexScript, indexQuery)
        Dim scanRows As JavaScriptObject() = RunQuery(scanScript, scanQuery)

        Call DebugAudit.Check(indexQuery.LastIndex IsNot Nothing AndAlso indexQuery.LastIndex.Used, "the order by query should use the data source index")
        Call DebugAudit.AreEqual(20, indexRows.Length, "the TAKE 20 clause of the order by query")
        Call DebugAudit.AreEqual(scanRows.Length, indexRows.Length, "the order by query should returns the same row count")

        For i As Integer = 1 To indexRows.Length - 1
            Call DebugAudit.Check(ToDouble(indexRows(i - 1)("CalcMz")) <= ToDouble(indexRows(i)("CalcMz")), "the CalcMz column was not ordered ascending")
        Next

        For i As Integer = 0 To indexRows.Length - 1
            Call DebugAudit.AreEqual(
                Text(scanRows(i)("LipidIon")),
                Text(indexRows(i)("LipidIon")),
                $"row {i} of the order by query was mismatch between the scan query and the indexed query"
            )
        Next

        ' 排序键为数据源字段的成员引用的时候，排序操作会直接下推为按照索引键的顺序来读取数据行
        Dim descQuery As QueryExpression = Nothing
        Dim descRows As JavaScriptObject() = RunQuery(
            String.Join(vbLf, {
                "from x as row",
                "in index(""data.csv"", range = [""CalcMz""])",
                "where x.CalcMz > 700",
                "select x.LipidIon, x.CalcMz",
                "order by x.CalcMz descending",
                "take 10"
            }), descQuery)

        Call DebugAudit.AreEqual(10, descRows.Length, "the TAKE 10 clause of the descending order by query")

        For i As Integer = 1 To descRows.Length - 1
            Call DebugAudit.Check(ToDouble(descRows(i - 1)("CalcMz")) >= ToDouble(descRows(i)("CalcMz")), "the CalcMz column was not ordered descending")
        Next
    End Sub

#End Region

#Region "aggregate query test"

    Private Function ComputeAggregate() As (n As Integer, avgQuality As Double, minMz As Double, maxMz As Double, sumArea As Double, maxScore As Double)
        Dim qualities As Object() = Column("PeakQuality")
        Dim mz As Object() = Column("CalcMz")
        Dim area As Object() = Column("Area")
        Dim score As Object() = Column("AreaScore")
        Dim n As Integer = 0
        Dim sumQuality As Double = 0
        Dim minMz As Double = Double.MaxValue
        Dim maxMz As Double = Double.MinValue
        Dim sumArea As Double = 0
        Dim maxScore As Double = Double.MinValue

        For i As Integer = 0 To qualities.Length - 1
            If ToDouble(qualities(i)) < 0.999 Then
                Continue For
            End If

            n += 1
            sumQuality += ToDouble(qualities(i))

            If mz(i) IsNot Nothing Then
                minMz = Math.Min(minMz, ToDouble(mz(i)))
                maxMz = Math.Max(maxMz, ToDouble(mz(i)))
            End If
            If area(i) IsNot Nothing Then
                sumArea += ToDouble(area(i))
            End If
            If score(i) IsNot Nothing Then
                maxScore = Math.Max(maxScore, ToDouble(score(i)))
            End If
        Next

        Return (n, sumQuality / n, minMz, maxMz, sumArea, maxScore)
    End Function

    Private Sub TestAggregate()
        Dim q As QueryExpression = Nothing
        Dim rows As JavaScriptObject() = RunFile("query_aggregate.linq", q)

        Call DebugAudit.AreEqual(1, rows.Length, "the global aggregate query should returns a single row")
        Call DebugAudit.Check(q.LastIndex Is Nothing, "the scan aggregate query should not use any index")

        Dim expected = ComputeAggregate()
        Dim row As JavaScriptObject = rows(0)

        Call DebugAudit.AreEqual(expected.n, CInt(row("n")), "aggregate count(x)")
        Call AssertNear(expected.avgQuality, ToDouble(row("avgQuality")), "aggregate avg(x.PeakQuality)")
        Call AssertNear(expected.minMz, ToDouble(row("minMz")), "aggregate min(x.CalcMz)")
        Call AssertNear(expected.maxMz, ToDouble(row("maxMz")), "aggregate max(x.CalcMz)")
        Call AssertNear(expected.sumArea, ToDouble(row("sumArea")), "aggregate sum(x.Area)")
        Call AssertNear(expected.maxScore, ToDouble(row("maxScore")), "aggregate max(x.AreaScore)")
    End Sub

    Private Sub TestAggregateIndex()
        Dim q As QueryExpression = Nothing
        Dim rows As JavaScriptObject() = RunFile("query_aggregate_index.linq", q)

        Call DebugAudit.AreEqual(1, rows.Length, "the indexed aggregate query should returns a single row")
        Call DebugAudit.Check(q.LastIndex IsNot Nothing AndAlso q.LastIndex.Used, "the indexed aggregate query should use the data source index!")
        Call DebugAudit.Check(q.LastIndex.Addresses.Length < GetDataFrame().Nrows, "the index should reduce the candidate rows of the aggregate query")

        Call DebugAudit.Capture("aggregateIndexCandidates", q.LastIndex.Addresses.Length)

        Dim expected = ComputeAggregate()
        Dim row As JavaScriptObject = rows(0)

        Call DebugAudit.AreEqual(expected.n, CInt(row("n")), "indexed aggregate count(x)")
        Call AssertNear(expected.avgQuality, ToDouble(row("avgQuality")), "indexed aggregate avg(x.PeakQuality)")
        Call AssertNear(expected.sumArea, ToDouble(row("sumArea")), "indexed aggregate sum(x.Area)")
    End Sub

    Private Sub TestAggregateGroup()
        Dim q As QueryExpression = Nothing
        Dim rows As JavaScriptObject() = RunFile("query_aggregate_group.linq", q)

        Call DebugAudit.Check(rows.Length > 0, "the group aggregate query should returns some rows")
        Call DebugAudit.Check(rows.Length <= 10, "the TAKE 10 clause should limit the output rows")
        Call DebugAudit.Check(q.LastIndex IsNot Nothing AndAlso q.LastIndex.Used, "the group aggregate query should use the data source index!")

        For i As Integer = 1 To rows.Length - 1
            Call DebugAudit.Check(CInt(rows(i - 1)("n")) >= CInt(rows(i)("n")), "the aggregate result was not ordered by n descending")
        Next

        Dim classes As Object() = Column("Class")
        Dim qualities As Object() = Column("PeakQuality")
        Dim counts As New Dictionary(Of String, Integer)(StringComparer.Ordinal)

        For i As Integer = 0 To classes.Length - 1
            If ToDouble(qualities(i)) < 0.9 Then
                Continue For
            End If

            Dim key As String = Text(classes(i))

            counts(key) = If(counts.ContainsKey(key), counts(key) + 1, 1)
        Next

        For Each row As JavaScriptObject In rows
            Dim key As String = Text(row("Class"))

            Call DebugAudit.Check(counts.ContainsKey(key), $"the group '{key}' was not found in the raw data")
            Call DebugAudit.AreEqual(counts(key), CInt(row("n")), $"group '{key}' count(x)")
            Call DebugAudit.Check(CInt(row("n")) > 0, $"group '{key}' should contains at least one row")
        Next

        ' 聚合查询的结果可以直接转换为表格数据输出(CLI的表格/CSV输出路径)
        Dim table As DataFrameResolver = rows.CreateTableDataSet

        Call DebugAudit.AreEqual(rows.Length, table.Nrows, "the aggregate result should be converted into a table")
        Call DebugAudit.Check(table.HeadTitles.Contains("Class"), "the group key should be a column of the aggregate table")
    End Sub

    Private Sub TestAggregateEquivalence()
        Dim scanQuery As QueryExpression = Nothing
        Dim indexQuery As QueryExpression = Nothing
        Dim scan As JavaScriptObject() = RunFile("query_aggregate.linq", scanQuery)
        Dim index As JavaScriptObject() = RunFile("query_aggregate_index.linq", indexQuery)

        Call DebugAudit.AreEqual(scan.Length, index.Length, "the indexed aggregate query should returns the same row count")
        Call DebugAudit.AreEqual(CInt(scan(0)("n")), CInt(index(0)("n")), "the indexed aggregate count should be the same as the scan result")
        Call AssertNear(ToDouble(scan(0)("avgQuality")), ToDouble(index(0)("avgQuality")), "aggregate avg quality equivalence")
        Call AssertNear(ToDouble(scan(0)("minMz")), ToDouble(index(0)("minMz")), "aggregate min mz equivalence")
        Call AssertNear(ToDouble(scan(0)("maxMz")), ToDouble(index(0)("maxMz")), "aggregate max mz equivalence")
        Call AssertNear(ToDouble(scan(0)("sumArea")), ToDouble(index(0)("sumArea")), "aggregate sum area equivalence")
        Call AssertNear(ToDouble(scan(0)("maxScore")), ToDouble(index(0)("maxScore")), "aggregate max score equivalence")
    End Sub

#End Region
End Module
