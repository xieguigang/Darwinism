Imports System
Imports Darwinism.OSSUtil

Module Program
    Sub Main(args As String())
        downloadData()
        Console.WriteLine("Hello World!")
    End Sub

    Sub downloadData()
        Dim downloader As New IDMBatchDownloader("D:\Tools\Internet Download Manager\IDMan.exe", defaultDownloadPath:="D:\datapool\ncbi_genbank")
        Dim list As String() = "D:\datapool\ncbi_genbank\http.txt".ReadAllLines
        Dim downloadList = list.Select(Function(path) New DownloadItem(path)).ToArray

        Call downloader.BatchDownload(downloadList)
        Call downloader.StartAllDownloads()
    End Sub
End Module
