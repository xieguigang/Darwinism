Imports System.IO
Imports System.IO.Compression
Imports Microsoft.VisualBasic.ApplicationServices.Zip
Imports Microsoft.VisualBasic.MachineLearning.Darwinism.NonlinearGridTopology

''' <summary>
''' 使用zip压缩的形式，将population保存为临时文件
''' </summary>
Public Class PopulationZip

    ReadOnly target$

    ''' <summary>
    ''' The target zip file
    ''' </summary>
    ''' <param name="target$"></param>
    Sub New(target$)
        Me.target = target
    End Sub

    Public Sub Add(genome As GridSystem)
        Dim temp = App.GetAppSysTempFile(".grid", App.PID, "population_")

        Using file As FileStream = temp.Open
            Call genome.Serialize(file, chunkSize:=2048)
        End Using

        Call ZipLib.AddToArchive(
            files:={temp},
            archiveFullName:=target,
            action:=ArchiveAction.Merge,
            fileOverwrite:=Overwrite.Always,
            compression:=CompressionLevel.Fastest
        )

        Call temp.DeleteFile
    End Sub
End Class
