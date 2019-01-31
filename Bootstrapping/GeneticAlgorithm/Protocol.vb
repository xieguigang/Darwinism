#Region "Microsoft.VisualBasic::6d4b03ccf24e96c02a52658a2ba355b7, Bootstrapping\GeneticAlgorithm\Protocol.vb"

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

    ' Module Protocol
    ' 
    '     Function: GA_PLinq, GetFitness
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.Bootstrapping.Darwinism.GAF
Imports Microsoft.VisualBasic.DataMining.Darwinism.GAF
Imports Microsoft.VisualBasic.Mathematical.Calculus
Imports Microsoft.VisualBasic.Scripting.MetaData

Public Module Protocol

    <Extension>
    Public Function GetFitness(v As ParameterVector, model As TypeInfo, observation As ODEsOut, ynames$(), y0 As Dictionary(Of String, Double), n%, t0#, tt#) As Double
        Return model.GetType.GetFitness(v, observation, ynames, y0, n, t0, tt, False, Nothing)
    End Function

    Public Function GA_PLinq(GA As GeneticAlgorithm(Of ParameterVector), source As NamedValue(Of ParameterVector)()) As IEnumerable(Of NamedValue(Of Double))
        Dim fitness As GAFFitness = DirectCast(GA.Fitness, GAFFitness)
        Dim model As New TypeInfo(fitness.Model)
        Dim task As Func(Of ParameterVector, TypeInfo, ODEsOut, String(), Dictionary(Of String, Double), Integer, Double, Double, Double) =
            AddressOf GetFitness
        Dim observation As ODEsOut = fitness.observation
        Return source.AsDistributed(Of NamedValue(Of Double))(
            task, model,
            observation,
            fitness.modelVariables,
            fitness.y0,
            observation.x.Length,
            observation.x(0),
            observation.x.Last)
    End Function
End Module
