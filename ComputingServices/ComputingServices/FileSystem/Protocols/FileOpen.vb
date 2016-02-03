Imports System.IO
Imports Microsoft.VisualBasic.Net.Protocol
Imports Microsoft.VisualBasic.Net.Protocol.Reflection
Imports Microsoft.VisualBasic.Serialization

Namespace FileSystem.Protocols

    ''' <summary>
    ''' Initializes a new instance of the System.IO.FileStream class with the specified
    ''' path and creation mode.
    ''' </summary>
    Public Class FileOpen : Inherits FileHandle

        ''' <summary>
        ''' Specifies how the operating system should open a file.
        ''' </summary>
        ''' <returns></returns>
        Public Property Mode As Integer

        Public Overrides Function ToString() As String
            Return $"[{DirectCast(Mode, FileMode).ToString }] " & FileName.ToFileURL
        End Function

        ''' <summary>
        ''' Initializes a new instance of the System.IO.FileStream class with the specified
        ''' path and creation mode.
        ''' </summary>
        Public Function OpenHandle() As FileStream
            Dim mode As FileMode = DirectCast(Me.Mode, FileMode)
            Return New FileStream(FileName, mode)
        End Function
    End Class

    ''' <summary>
    ''' 在远端服务器上面的文件句柄对象
    ''' </summary>
    Public Class FileHandle
        Public Property FileName As String
        Public Property HashCode As Integer

        Sub New()
        End Sub

        Sub New(handle As FileHandle)
            Me.FileName = handle.FileName
            Me.HashCode = handle.HashCode
        End Sub

        Public Overrides Function ToString() As String
            Return Handle
        End Function

        ''' <summary>
        ''' 远程机器上面唯一标示的文件句柄值
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Handle As String
            Get
                Return $"{HashCode}+{FileName.ToFileURL}"
            End Get
        End Property
    End Class

    Public Class ReadBuffer : Inherits FileHandle
        Implements IReadWriteBuffer

        Public Property offset As Integer Implements IReadWriteBuffer.offset
        ''' <summary>
        ''' 输入的参数的长度
        ''' </summary>
        ''' <returns></returns>
        Public Property length As Integer Implements IReadWriteBuffer.length

        Sub New()
        End Sub

        Sub New(handle As FileHandle)
            Call MyBase.New(handle)
        End Sub

        Public Function CreateBuffer() As Byte()
            Return CreateBuffer(Me)
        End Function

        Public Shared Function CreateBuffer(op As IReadWriteBuffer) As Byte()
            Dim buffer As Byte() = New Byte(op.length + op.offset - 1) {}
            Return buffer
        End Function
    End Class

    Public Interface IReadWriteBuffer
        Property offset As Integer
        Property length As Integer
    End Interface

    Public Class WriteStream : Inherits RawStream
        Implements IReadWriteBuffer

        Public Property Handle As FileHandle
        Public Property length As Integer Implements IReadWriteBuffer.length
        Public Property offset As Integer Implements IReadWriteBuffer.offset
        Public Property buffer As Byte()

        Sub New()
        End Sub

        Sub New(raw As Byte())
            Dim buf As Byte() = New Byte(INT32 - 1) {}
            Dim p As Integer = Scan0
            Dim handleLen As Integer
            Dim bufferLen As Integer

            Call Array.ConstrainedCopy(raw, p.Move(INT32), buf, Scan0, INT32) : length = BitConverter.ToInt32(buf, Scan0)
            Call Array.ConstrainedCopy(raw, p.Move(INT32), buf, Scan0, INT32) : offset = BitConverter.ToInt32(buf, Scan0)
            Call Array.ConstrainedCopy(raw, p.Move(INT32), buf, Scan0, INT32) : handleLen = BitConverter.ToInt32(buf, Scan0)
            Call Array.ConstrainedCopy(raw, p.Move(INT32), buf, Scan0, INT32) : bufferLen = BitConverter.ToInt32(buf, Scan0)

            buf = New Byte(handleLen - 1) {}
            Call Array.ConstrainedCopy(raw, p.Move(handleLen), buf, Scan0, handleLen)
            Dim json As String = System.Text.Encoding.UTF8.GetString(buf)
            Handle = json.LoadObject(Of FileHandle)
            buffer = New Byte(bufferLen - 1) {}
            Call Array.ConstrainedCopy(raw, p, buffer, Scan0, bufferLen)
        End Sub

        Public Overrides Function Serialize() As Byte()
            Dim handle As Byte() = System.Text.Encoding.UTF8.GetBytes(Me.Handle.GetJson)
            Dim length As Byte() = BitConverter.GetBytes(Me.length)
            Dim offset As Byte() = BitConverter.GetBytes(Me.offset)
            Dim chunkBuffer As Byte() = New Byte(INT32 +  ' length
                                                 INT32 +  ' offset
                                                 INT32 +  ' handle length
                                                 INT32 +  ' buffer length
                                                 handle.Length +
                                                 buffer.Length - 1) {}
            Dim p As Integer = Scan0
            Dim handleLen As Byte() = BitConverter.GetBytes(handle.Length)
            Dim bufferLen As Byte() = BitConverter.GetBytes(buffer.Length)

            Call Array.ConstrainedCopy(length, Scan0, chunkBuffer, p.Move(INT32), INT32)
            Call Array.ConstrainedCopy(offset, Scan0, chunkBuffer, p.Move(INT32), INT32)
            Call Array.ConstrainedCopy(handleLen, Scan0, chunkBuffer, p.Move(INT32), INT32)
            Call Array.ConstrainedCopy(bufferLen, Scan0, chunkBuffer, p.Move(INT32), INT32)
            Call Array.ConstrainedCopy(handle, Scan0, chunkBuffer, p.Move(handle.Length), handle.Length)
            Call Array.ConstrainedCopy(buffer, Scan0, chunkBuffer, p, buffer.Length)

            Return chunkBuffer
        End Function

        Public Function CreateBuffer() As Byte()
            Return ReadBuffer.CreateBuffer(Me)
        End Function
    End Class
End Namespace