#Region "Microsoft.VisualBasic::fe1f9bab4890015c1e59337a422ea48c, LINQ\LINQ\Language\Tokens.vb"

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

    '     Enum Tokens
    ' 
    '         [Boolean], [Integer], [Operator], Close, Comma
    '         Comment, Interpolation, Invalid, keyword, Literal
    '         Number, Open, Reference, Symbol, Terminator
    ' 
    '  
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace Language

    ''' <summary>
    ''' 
    ''' </summary>
    Public Enum Tokens
        Invalid

        keyword

        Open
        Close

        [Operator]

        Symbol
        Reference

        ''' <summary>
        ''' the string interpolation
        ''' </summary>
        Interpolation
        ''' <summary>
        ''' the string literal
        ''' </summary>
        Literal
        CommandLineArgument

        Number
        [Integer]
        [Boolean]

        Comma
        ''' <summary>
        ''' 与VB语言类似的，是以换行作为语句结束
        ''' </summary>
        Terminator

        Comment
    End Enum
End Namespace
