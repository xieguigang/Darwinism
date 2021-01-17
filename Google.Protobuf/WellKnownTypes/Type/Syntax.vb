Imports Microsoft.VisualBasic.Language
Imports pbc = Google.Protobuf.Collections
Imports pbr = Google.Protobuf.Reflection

Namespace Google.Protobuf.WellKnownTypes

#Region "Enums"
    ''' <summary>
    '''  The syntax in which a protocol buffer element is defined.
    ''' </summary>
    Public Enum Syntax
        ''' <summary>
        '''  Syntax `proto2`.
        ''' </summary>
        <pbr.OriginalName("SYNTAX_PROTO2")>
        Proto2 = 0
        ''' <summary>
        '''  Syntax `proto3`.
        ''' </summary>
        <pbr.OriginalName("SYNTAX_PROTO3")>
        Proto3 = 1
    End Enum

#End Region
End Namespace