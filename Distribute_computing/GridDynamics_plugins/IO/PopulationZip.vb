#Region "Microsoft.VisualBasic::2a8d2c4ab4b6fd68150161b33a111f44, Distribute_computing\GridDynamics_plugins\IO\PopulationZip.vb"

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

    ' Class PopulationZip
    ' 
    '     Properties: Count
    ' 
    '     Constructor: (+1 Overloads) Sub New
    ' 
    '     Function: GetIndividual
    ' 
    '     Sub: (+2 Overloads) Add, OrderBy, Trim
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports System.IO.Compression
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices.Zip
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.MachineLearning.Darwinism.GAF
Imports Microsoft.VisualBasic.MachineLearning.Darwinism.NonlinearGridTopology

''' <summary>
''' 使用zip压缩的形式，将population保存为临时文件
''' </summary>
Public Class PopulationZip : Inherits PopulationCollection(Of Genome)

    ReadOnly target$
    ReadOnly chunkSize%

    ReadOnly mutationRate As Double, truncate As Double

    Dim index As VBInteger = Scan0
    ''' <summary>
    ''' [index => md5]
    ''' </summary>
    Dim indexHashMaps As New Dictionary(Of String, String)

    Public Overrides ReadOnly Property Count As Integer
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Get
            Return index
        End Get
    End Property

    Default Public Overrides ReadOnly Property Item(index As Integer) As Genome
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Get
            Dim genome As New Genome(GetIndividual(index), mutationRate, truncate)
            indexHashMaps(index.ToString) = genome.ToString
            Return genome
        End Get
    End Property

    ''' <summary>
    ''' The target zip file
    ''' </summary>
    ''' <param name="target$"></param>
    Sub New(target$, mutationRate As Double, truncate As Double, Optional chunkSize% = 20480)
        Me.target = target
        Me.chunkSize = chunkSize
        Me.mutationRate = mutationRate
        Me.truncate = truncate

        Call target.DeleteFile
    End Sub

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

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Overrides Sub Add(chr As Genome)
        Call Add(chr.chromosome)
    End Sub

    Public Overrides Sub Trim(capacitySize As Integer)
        If capacitySize = Count Then
            Return
        End If

        ' 将capacitysize后面的序号的genome全部删除
        Dim names = (Count - capacitySize).Sequence _
            .Select(Function(i) i + capacitySize) _
            .Select(Function(i) CStr(i)) _
            .ToArray

        index = capacitySize

        Using zip As ZipArchive = ZipFile.Open(target, ZipArchiveMode.Update)
            Call zip.DeleteItems(names)
        End Using
    End Sub

    Public Overrides Sub OrderBy(fitness As Func(Of String, Double))
        Dim tempZip As String = App.GetAppSysTempFile(".zip", App.PID)

        Using zip As ZipArchive = ZipFile.Open(target, ZipArchiveMode.Read)
            Dim orderEntries = zip.Entries.OrderBy(Function(e) fitness(indexHashMaps(e.Name))).ToArray
            Dim i As VBInteger = Scan0

            Using temporder As ZipArchive = ZipFile.Open(tempZip, ZipArchiveMode.Create)
                For Each entry In orderEntries
                    Dim newEntry = temporder.CreateEntry(++i, CompressionLevel.Fastest)

                    Using a = entry.Open, b = newEntry.Open
                        Call a.CopyTo(b)
                    End Using
                Next
            End Using
        End Using

        Call target.DeleteFile
        Call tempZip.FileMove(target)
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
