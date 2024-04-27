#Region "Microsoft.VisualBasic::a16040febc3f2d131e86271d7a5e67cb, G:/GCModeller/src/runtime/Darwinism/src/CloudKit/Docker//Arguments/Image.vb"

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

    '   Total Lines: 61
    '    Code Lines: 39
    ' Comment Lines: 13
    '   Blank Lines: 9
    '     File Size: 1.82 KB


    '     Class Image
    ' 
    '         Properties: Package, Publisher
    ' 
    '         Function: ParseEntry, ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices

Namespace Arguments

    ''' <summary>
    ''' Docker image name
    ''' </summary>
    Public Class Image

        Public Property Publisher As String
        Public Property Package As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="text">
        ''' accept image name reference in format like:
        ''' 
        ''' + ``imageName:tag``
        ''' + ``user/imageName:tag``
        ''' </param>
        ''' <returns></returns>
        Public Shared Function ParseEntry(text As String) As Image
            With text.Trim.Split("/"c)
                Dim user$, name$

                If .Length = 1 Then
                    user = Nothing
                    name = .ElementAt(0)
                Else
                    user = .ElementAt(0)
                    name = .ElementAt(1)
                End If

                Return New Image With {
                    .Package = name,
                    .Publisher = user
                }
            End With
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Overrides Function ToString() As String
            If Publisher.StringEmpty Then
                Return Package
            Else
                Return $"{Publisher}/{Package}"
            End If
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Widening Operator CType(repo As String) As Image
            Return ParseEntry(repo)
        End Operator

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Narrowing Operator CType(img As Image) As String
            Return img.ToString
        End Operator
    End Class
End Namespace
