Imports Microsoft.VisualBasic.Language
Imports pbc = Google.Protobuf.Collections
Imports pbr = Google.Protobuf.Reflection

Namespace Google.Protobuf.WellKnownTypes
#Region "Enums"
    ''' <summary>
    '''  `NullValue` is a singleton enumeration to represent the null value for the
    '''  `Value` type union.
    '''
    '''   The JSON representation for `NullValue` is JSON `null`.
    ''' </summary>
    Public Enum NullValue
        ''' <summary>
        '''  Null value.
        ''' </summary>
        <pbr.OriginalName("NULL_VALUE")>
        NullValue = 0
    End Enum

#End Region
End Namespace