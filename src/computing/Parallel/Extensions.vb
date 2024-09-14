#Region "Microsoft.VisualBasic::35ae46e4582028553144a611da9e9665, src\computing\Parallel\Extensions.vb"

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

    '   Total Lines: 24
    '    Code Lines: 12 (50.00%)
    ' Comment Lines: 8 (33.33%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 4 (16.67%)
    '     File Size: 720 B


    ' Module Extensions
    ' 
    '     Properties: Verbose
    ' 
    '     Sub: RegisterDiagnoseBuffer, SetVerbose
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Data.IO.MessagePack

<HideModuleName>
Public Module Extensions

    ''' <summary>
    ''' print the verbose debug echo?
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Verbose As Boolean = False

    ''' <summary>
    ''' set global verbose option
    ''' </summary>
    ''' <param name="v"></param>
    Public Sub SetVerbose(v As Boolean)
        _Verbose = v
    End Sub

    Public Sub RegisterDiagnoseBuffer()
        Call MsgPackSerializer.DefaultContext.RegisterSerializer(New Serialization.StackFrameBuffer)
        Call MsgPackSerializer.DefaultContext.RegisterSerializer(New Serialization.StackMethodBuffer)
    End Sub
End Module
