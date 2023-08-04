Imports Microsoft.VisualBasic.MachineLearning.ComponentModel.StoreProcedure
Imports Microsoft.VisualBasic.Serialization

''' <summary>
''' An abstract model that running in client
''' </summary>
Public MustInherit Class MPP

    Sub New(ds As DataSet)

    End Sub

    ''' <summary>
    ''' get the best model from this client, and then sent back to the master node
    ''' </summary>
    ''' <returns></returns>
    Public MustOverride Function GetBestModel() As ISerializable


End Class
