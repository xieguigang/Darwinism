#Region "Microsoft.VisualBasic::fe442a9d9a44ad71f2ed2a2001036924, src\computing\Parallel\IpcParallel\Protocols.vb"

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

    '   Total Lines: 29
    '    Code Lines: 8 (27.59%)
    ' Comment Lines: 21 (72.41%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 0 (0.00%)
    '     File Size: 669 B


    ' Enum Protocols
    ' 
    '     GetArgumentByIndex, GetArgumentNumber, GetTask, PostError, PostResult
    '     PostStart
    ' 
    '  
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

''' <summary>
''' the IPC parallel protocols
''' </summary>
Public Enum Protocols
    ''' <summary>
    ''' get task method information
    ''' </summary>
    GetTask
    ''' <summary>
    ''' get total number of parameter input
    ''' </summary>
    GetArgumentNumber
    ''' <summary>
    ''' get parameter value by index
    ''' </summary>
    GetArgumentByIndex
    ''' <summary>
    ''' notify task is start
    ''' </summary>
    PostStart
    ''' <summary>
    ''' send result value to master node
    ''' </summary>
    PostResult
    ''' <summary>
    ''' send error message to master node
    ''' </summary>
    PostError
End Enum
