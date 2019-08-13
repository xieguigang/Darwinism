Imports System.IO
Imports System.IO.Compression
Imports Microsoft.VisualBasic.ApplicationServices.Zip
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.MachineLearning.Darwinism.GAF
Imports Microsoft.VisualBasic.MachineLearning.Darwinism.NonlinearGridTopology

''' <summary>
''' 使用zip压缩的形式，将population保存为临时文件
''' </summary>
Public Class PopulationZip : Inherits PopulationCollection(Of Genome)

    ReadOnly target$
    ReadOnly index As VBInteger = Scan0
    ReadOnly chunkSize%

    ReadOnly mutationRate As Double, truncate As Double

    ''' <summary>
    ''' The target zip file
    ''' </summary>
    ''' <param name="target$"></param>
    Sub New(target$, Optional chunkSize% = 20480)
        Me.target = target
        Me.chunkSize = chunkSize
    End Sub

    Public Overrides ReadOnly Property Count As Integer

    Default Public Overrides ReadOnly Property Item(index As Integer) As Genome
        Get
            Return New Genome(GetIndividual(index), mutationRate, truncate)
        End Get
    End Property

    Public Overloads Sub Add(genome As GridSystem)
        Dim temp = App.GetAppSysTempFile($".grid/{++index}", App.PID, "population_")

        Using file As FileStream = temp.Open
            Call genome.Serialize(file, chunkSize:=chunkSize)
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

    Public Overrides Sub Add(chr As Genome)
        Call Add(chr.chromosome)
    End Sub

    Public Overrides Sub Trim(capacitySize As Integer)
        Throw New NotImplementedException()
    End Sub

    Public Overrides Sub OrderBy(fitness As Func(Of Genome, Double))
        Throw New NotImplementedException()
    End Sub

    Public Function GetIndividual(i As Integer) As GridSystem
        Dim buffer As MemoryStream = ZipStreamReader.GetZipSubStream(target, CStr(i))

        If buffer Is Nothing Then
            Throw New MissingMemberException(i)
        Else
            Using buffer
                Return buffer.LoadGridSystem
            End Using
        End If
    End Function
End Class
