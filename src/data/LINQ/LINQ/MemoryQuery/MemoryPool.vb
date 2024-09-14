Imports System.Reflection
Imports Microsoft.VisualBasic.Emit.Delegates
Imports Microsoft.VisualBasic.Scripting

''' <summary>
''' index of the general clr object array.
''' </summary>
Public Class MemoryPool : Inherits MemoryIndex

    ReadOnly vector As DataObjectVector
    ''' <summary>
    ''' the raw input pool data
    ''' </summary>
    ReadOnly pool As Array

    Sub New(data As Array, Optional [property] As String = Nothing)
        vector = New DataObjectVector(data)

        If Not [property].StringEmpty() Then
            ' index via the nested data property
            ' not the raw vector
            ' the query returns result use the raw vector
            vector = New DataObjectVector(DirectCast(vector(name:=[property]), Array))
        End If
    End Sub

    Protected Overrides Function GetData(Of V)(field As String) As V()
        Dim prop As PropertyInfo = vector.GetProperty(field)

        If prop.PropertyType Is GetType(V) Then
            Return vector(field)
        Else
            Dim pull As Array = vector(field)
            Dim cast As V() = pull.CTypeDynamic(GetType(V))

            Return cast
        End If
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="filter"></param>
    ''' <returns>
    ''' this function will returns nothing if query filter has no result
    ''' </returns>
    Public Function Query(filter As IEnumerable(Of Query)) As Array()
        Dim index As Integer() = GetIndex(filter)

        If index.IsNullOrEmpty Then
            Return Nothing
        End If

        Return index _
            .Select(Function(i) pool(i)) _
            .ToArray
    End Function
End Class

''' <summary>
''' Index of the generic clr object
''' </summary>
Public Class MemoryPool(Of T) : Inherits MemoryIndex

    ReadOnly pool As T()
    ReadOnly vector As DataValue(Of T)

    Sub New(data As T())
        pool = data
        vector = New DataValue(Of T)(data)
    End Sub

    Protected Overrides Function GetData(Of V)(field As String) As V()
        Dim prop As PropertyInfo = vector.GetProperty(field)

        If prop.PropertyType Is GetType(V) Then
            Return vector(field)
        Else
            Dim pull As Array = vector(field)
            Dim cast As V() = pull.CTypeDynamic(GetType(V))

            Return cast
        End If
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="filter"></param>
    ''' <returns>
    ''' this function will returns nothing if query filter has no result
    ''' </returns>
    Public Function Query(filter As IEnumerable(Of Query)) As T()
        Dim index As Integer() = GetIndex(filter)

        If index.IsNullOrEmpty Then
            Return Nothing
        End If

        Return index _
            .Select(Function(i) pool(i)) _
            .ToArray
    End Function
End Class
