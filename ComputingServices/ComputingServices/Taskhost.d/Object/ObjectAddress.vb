Imports Microsoft.VisualBasic.Serialization

Namespace TaskHost

    ''' <summary>
    ''' Object reference address on the remote
    ''' </summary>
    Public Class ObjectAddress

        Public Property TypeId As String
            Get
                Return _typeId
            End Get
            Set(value As String)
                _typeId = value
                Call __initHashCode()
            End Set
        End Property

        ''' <summary>
        ''' <see cref="Object.GetHashCode"/> from local
        ''' </summary>
        ''' <returns></returns>
        Public Property HashCode As String
            Get
                Return _hashCode
            End Get
            Set(value As String)
                _hashCode = value
                Call __initHashCode()
            End Set
        End Property

        Dim _typeId As String
        Dim _hashCode As String
        Dim __md5 As String
        Dim __hash As Long

        Private Sub __initHashCode()
            __md5 = SecurityString.GetMd5Hash(_typeId & _hashCode)
            __hash = SecurityString.ToLong(__md5)
        End Sub

        Public Function ReferenceAddress() As Long
            Return __hash
        End Function

        ''' <summary>
        ''' Object is equals on the remote and local
        ''' </summary>
        ''' <param name="addr"></param>
        ''' <returns></returns>
        Public Overloads Function ReferenceEquals(addr As ObjectAddress) As Boolean
            Return __hash = addr.__hash
        End Function

        Public Overloads Function ReferenceEquals(obj As Object) As Boolean
            Dim addr As ObjectAddress = ObjectAddress.AddressOf(obj)
            Return __hash = addr.__hash
        End Function

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function

        Public Shared Function [AddressOf](x As Object) As ObjectAddress
            Return New ObjectAddress With {
                .HashCode = x.GetHashCode,
                .TypeId = x.GetType.FullName
            }
        End Function

        Public Shared Operator =(a As ObjectAddress, b As ObjectAddress) As Boolean
            Return a.__hash = b.__hash
        End Operator

        Public Shared Operator <>(a As ObjectAddress, b As ObjectAddress) As Boolean
            Return a.__hash <> b.__hash
        End Operator

        Public Shared Operator =(a As ObjectAddress, b As Object) As Boolean
            Return a.__hash = [AddressOf](b).__hash
        End Operator

        Public Shared Operator <>(a As ObjectAddress, b As Object) As Boolean
            Return a.__hash <> [AddressOf](b).__hash
        End Operator

        Public Shared Operator =(a As Object, b As ObjectAddress) As Boolean
            Return [AddressOf](a).__hash = b.__hash
        End Operator

        Public Shared Operator <>(a As Object, b As ObjectAddress) As Boolean
            Return [AddressOf](a).__hash <> b.__hash
        End Operator
    End Class
End Namespace