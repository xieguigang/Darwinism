#Region "Microsoft.VisualBasic::5efc7517b0bffb4b14886a39aaa44b71, src\CloudKit\ossutil\Model\TextParsers.vb"

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

    '   Total Lines: 67
    '    Code Lines: 50 (74.63%)
    ' Comment Lines: 6 (8.96%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 11 (16.42%)
    '     File Size: 2.98 KB


    '     Module TextParsers
    ' 
    '         Function: ParseBuckets, ParseObjects
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports r = System.Text.RegularExpressions.Regex

Namespace Model

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
End Namespace
