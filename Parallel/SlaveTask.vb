Imports System.IO
Imports Microsoft.VisualBasic.MIME.application.json
Imports Microsoft.VisualBasic.MIME.application.json.BSON
Imports Microsoft.VisualBasic.Scripting.MetaData

Public Class SlaveTask

    Public Function RunTask(entry As [Delegate], ParamArray parameters As Object()) As Object
        Dim target As New IDelegate(entry)
        Dim result As Object = Nothing
        Dim host As New IPCSocket With {
            .handlePOSTResult = Sub(buf)
                                    result = BSONFormat.Load(buf).CreateObject(entry.Method.ReturnType)
                                End Sub,
            .nargs = parameters.Length,
            .handleGetArgument = Function(i)
                                     Dim type As Type = parameters(i).GetType
                                     Dim element = type.GetJsonElement(parameters(i), New JSONSerializerOptions)

                                     Return BSONFormat.GetBuffer(element)
                                 End Function
        }

        Call host.Run()

        Return result
    End Function

End Class
