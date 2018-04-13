#Region "Microsoft.VisualBasic::816e393c806226d625e716542a09081b, LINQ\LINQ\Framewok\Provider\DefaultEntity.vb"

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

    '     Module DefaultEntity
    ' 
    '         Function: GetDouble, GetInt32, GetInt64
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Scripting.Runtime

Namespace Framework.Provider

    Public Module DefaultEntity

        <LinqEntity("integer", GetType(Integer))>
        Public Function GetInt32(uri As String) As IEnumerable
            Dim lines As String() = IO.File.ReadAllLines(uri)
            Dim LQuery = (From line As String In lines Select CastInteger(line))
            Return LQuery
        End Function

        <LinqEntity("long", GetType(Long))>
        Public Function GetInt64(uri As String) As IEnumerable
            Dim lines As String() = IO.File.ReadAllLines(uri)
            Dim LQuery = (From line As String In lines Select CastLong(line))
            Return LQuery
        End Function

        <LinqEntity("double", GetType(Double))>
        Public Function GetDouble(uri As String) As IEnumerable
            Dim lines As String() = IO.File.ReadAllLines(uri)
            Dim LQuery = lines.Select(AddressOf Val)
            Return LQuery
        End Function
    End Module
End Namespace
