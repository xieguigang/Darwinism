Namespace Google.Protobuf

    ''' <summary>
    ''' Generic interface for a Protocol Buffers message,
    ''' where the type parameter is expected to be the same type as
    ''' the implementation class.
    ''' </summary>
    ''' <typeparam name="T">The message type.</typeparam>
    Public Interface IMessageType(Of T As IMessageType(Of T))
        Inherits IMessage, IEquatable(Of T), IDeepCloneable(Of T)
        ''' <summary>
        ''' Merges the given message into this one.
        ''' </summary>
        ''' <remarks>See the user guide for precise merge semantics.</remarks>
        ''' <param name="message">The message to merge with this one. Must not be null.</param>
        Overloads Sub MergeFrom(message As T)
    End Interface
End Namespace