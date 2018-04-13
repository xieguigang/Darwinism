Imports System.Runtime.CompilerServices
Imports r = System.Text.RegularExpressions.Regex

Public Module TextParsers

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
            Dim t = Mid(line, time.Length + 1S).StringSplit("\s+")

            Yield New Bucket With {
                .meta = New Dictionary(Of String, String) From {
                    {NameOf(Bucket.CreationTime), time},
                    {NameOf(Bucket.Region), t(1)},
                    {NameOf(Bucket.StorageClass), t(2)},
                    {NameOf(Bucket.BucketName), t(3)}
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
            Dim t = Mid(line, time.Length + 1S).StringSplit("\s{2,}")

            Yield New [Object] With {
                .meta = New Dictionary(Of String, String) From {
                    {NameOf([Object].LastModifiedTime), time},
                    {NameOf([Object].Size), t(1)},
                    {NameOf([Object].StorageClass), t(2)},
                    {NameOf([Object].ETAG), t(3)},
                    {NameOf([Object].ObjectName), t(4)}
                }
            }
        Next
    End Function

    ' FinishWithError: Scanned 1 objects. Error num: 1. OK num: 0, Transfer size: 0.
    ' Error: oss: service returned without a response body (404 Not Found), Bucket=bionovogene-xcms, Object=mz.biodeep.cn/data/upload/rawfiles/225/181/1761/T201710170947282738.mzXML!
    '<Extension>
    'Public Iterator Function PopulateErrors(stdout As String) As IEnumerable(Of String)

    'End Function
End Module
