Imports Microsoft.VisualBasic.Data.IO.Xdr

Namespace Rpc.BindingProtocols
    ''' <summary>
    ''' A mapping of (program, version, protocol) to port number
    ''' http://tools.ietf.org/html/rfc1833#section-3.1
    ''' </summary>
    Public Structure mapping
        ''' <summary>
        ''' program number
        ''' </summary>
        <Order(0)>
        Public prog As UInteger
        ''' <summary>
        ''' version number
        ''' </summary>
        <Order(1)>
        Public vers As UInteger
        ''' <summary>
        ''' protocol
        ''' </summary>
        <Order(2)>
        Public prot As Protocol
        ''' <summary>
        ''' port
        ''' </summary>
        <Order(3)>
        Public port As UInteger

        ''' <summary>
        ''' Returns a <seecref="System.String"/> that represents the current <seecref="Rpc.BindingProtocols.mapping"/>.
        ''' </summary>
        ''' <returns>
        ''' A <seecref="System.String"/> that represents the current <seecref="Rpc.BindingProtocols.mapping"/>.
        ''' </returns>
        Public Overrides Function ToString() As String
            Return String.Format("port:{0} prog:{1} prot:{2} vers:{3}", port, prog, prot, vers)
        End Function
    End Structure
End Namespace
