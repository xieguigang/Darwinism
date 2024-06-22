#Region "Microsoft.VisualBasic::a73604e77c063a9a6bbcb32490912b6c, src\computing\Parallel\Serialization\StackFrameBuffer.vb"

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

    '   Total Lines: 31
    '    Code Lines: 24 (77.42%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 7 (22.58%)
    '     File Size: 1.42 KB


    '     Class StackFrameBuffer
    ' 
    '         Function: GetObjectSchema
    ' 
    '     Class StackMethodBuffer
    ' 
    '         Function: GetObjectSchema
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.ApplicationServices.Debugging.Diagnostics
Imports Microsoft.VisualBasic.Data.IO.MessagePack.Serialization

Namespace Serialization

    Public Class StackFrameBuffer : Inherits SchemaProvider(Of StackFrame)

        Protected Overrides Iterator Function GetObjectSchema() As IEnumerable(Of (obj As Type, schema As Dictionary(Of String, NilImplication)))
            Dim schema As New Dictionary(Of String, NilImplication) From {
                {NameOf(StackFrame.File), NilImplication.MemberDefault},
                {NameOf(StackFrame.Line), NilImplication.MemberDefault},
                {NameOf(StackFrame.Method), NilImplication.MemberDefault}
            }

            Yield (GetType(StackFrame), schema)
        End Function
    End Class

    Public Class StackMethodBuffer : Inherits SchemaProvider(Of Method)

        Protected Overrides Iterator Function GetObjectSchema() As IEnumerable(Of (obj As Type, schema As Dictionary(Of String, NilImplication)))
            Dim schema As New Dictionary(Of String, NilImplication) From {
                {NameOf(Method.Module), NilImplication.MemberDefault},
                {NameOf(Method.Method), NilImplication.MemberDefault},
                {NameOf(Method.Namespace), NilImplication.MemberDefault}
            }

            Yield (GetType(Method), schema)
        End Function
    End Class
End Namespace
