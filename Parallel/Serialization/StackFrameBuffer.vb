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