Imports System.Runtime.CompilerServices
Imports r = System.Text.RegularExpressions.Regex

Module TextParsers

    Const DateTimePattern$ = "\d+[-]\d+[-]\d+ \d+[:]\d+[:]\d+ \+\d+ [A-Z]+"

    <Extension>
    Public Iterator Function ParseBuckets(stdout As String) As IEnumerable(Of Bucket)
        Dim lines = stdout.LineTokens _
                          .Skip(1) _
                          .TakeWhile(Function(line)
                                         Return InStr(line, NameOf(Bucket)) <= 0
                                     End Function) _
                          .ToArray

        For Each line As String In lines
            Dim time$ = r.Match(line, DateTimePattern, RegexICMul).Value
            Dim t = Mid(line, time.Length).StringSplit("\s+")

            Yield New Bucket With {
                .meta = New Dictionary(Of String, String) From {
                    {NameOf(Bucket.CreationTime), time},
                    {NameOf(Bucket.Region), t(0)},
                    {NameOf(Bucket.StorageClass), t(1)},
                    {NameOf(Bucket.BucketName), t(2)}
                }
            }
        Next
    End Function

    <Extension>
    Public Iterator Function ParseObjects(stdout As String) As IEnumerable(Of [Object])
        Dim lines = stdout.LineTokens _
                          .Skip(1) _
                          .TakeWhile(Function(line)
                                         Return InStr(line, NameOf([Object])) <= 0
                                     End Function) _
                          .ToArray

        For Each line As String In lines
            Dim time$ = r.Match(line, DateTimePattern, RegexICMul).Value
            ' 假设在文件的路径之中不会存在连续的多个空格
            Dim t = Mid(line, time.Length).StringSplit("\s{2,}")

            Yield New [Object] With {
                .meta = New Dictionary(Of String, String) From {
                    {NameOf([Object].LastModifiedTime), time},
                    {NameOf([Object].Size), t(0)},
                    {NameOf([Object].StorageClass), t(1)},
                    {NameOf([Object].ETAG), t(2)},
                    {NameOf([Object].ObjectName), t(3)}
                }
            }
        Next
    End Function
End Module
