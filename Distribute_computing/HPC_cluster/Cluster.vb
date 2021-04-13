Imports System.Reflection
Imports Microsoft.VisualBasic.Net

''' <summary>
''' 高性能计算集群，高性能计算集群采用将计算任务分配到集群的不同计算节点儿提高计算能力，
''' 因而主要应用在科学计算领域。比较流行的HPC采用Linux操作系统和其它一些免费软件来完成并行运算。
''' 
''' 这一集群配置通常被称为Beowulf集群。这类集群通常运行特定的程序以发挥HPC cluster的并行能力。
''' 这类程序一般应用特定的运行库, 比如专为科学计算设计的MPI库。HPC集群特别适合于在计算中各计算节点
''' 之间发生大量数据通讯的计算作业，比如一个节点的中间结果或影响到其它节点计算结果的情况。
''' </summary>
''' <remarks>
''' This module only works on Linux server
''' </remarks>
Public Class Cluster

    ReadOnly remote As IPEndPoint
    ReadOnly userName As String
    ReadOnly imageName As String

    ''' <summary>
    ''' create a helper module for deploy environment via ``ssh`` and ``cluster node share storage``.
    ''' </summary>
    ''' <param name="remote"></param>
    ''' <param name="userName"></param>
    ''' <param name="imageName"></param>
    Sub New(remote As IPEndPoint, userName As String, Optional imageName As String = Nothing)
        Me.remote = remote
        Me.userName = userName
        Me.imageName = imageName
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Public Sub Deploy()
        Dim deployBase As String = App.GetVariable("sockets")
        Dim appBase As String = App.HOME.GetDirectoryFullPath

        If deployBase.StringEmpty Then
            Throw New InvalidOperationException($"you should set variable 'sockets' to a location on your clusters' share storage at first!")
        Else
            deployBase = $"{deployBase}/.deploy/"
        End If

        For Each assembly As Assembly In AppDomain.CurrentDomain.GetAssemblies
            Dim dllFile As String = assembly.Location
            Dim dllBase As String = dllFile.ParentPath.GetDirectoryFullPath
            Dim relative As String = dllBase.Replace(appBase, "") & "/" & dllFile.FileName

            If InStr(dllBase, appBase, CompareMethod.Text) > 0 Then
                Call dllFile.FileCopy($"{deployBase}/{relative}")
            End If
        Next
    End Sub
End Class
