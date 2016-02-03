﻿Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Security
Imports System.Security.AccessControl
Imports System.Text
Imports System.Threading
Imports Microsoft.VisualBasic.ComputingServices.FileSystem.Protocols
Imports Microsoft.VisualBasic.FileIO
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Protocol
Imports Microsoft.VisualBasic.Serialization
Imports Microsoft.Win32.SafeHandles

Namespace FileSystem.IO

    ''' <summary>
    ''' Provides a System.IO.Stream for a file, supporting both synchronous and asynchronous
    ''' read and write operations.To browse the .NET Framework source code for this type,
    ''' see the Reference Source.
    ''' </summary>
    <ComVisible(True)> Public Class FileStream
        Inherits BaseStream
        '
        ' Summary:
        '     
        '
        ' Parameters:
        '   path:
        '     A relative or absolute path for the file that the current FileStream object will
        '     encapsulate.
        '
        '   mode:
        '     A constant that determines how to open or create the file.
        '
        ' Exceptions:
        '   T:System.ArgumentException:
        '     path is an empty string (""), contains only white space, or contains one or more
        '     invalid characters. -or-path refers to a non-file device, such as "con:", "com1:",
        '     "lpt1:", etc. in an NTFS environment.
        '
        '   T:System.NotSupportedException:
        '     path refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in a
        '     non-NTFS environment.
        '
        '   T:System.ArgumentNullException:
        '     path is null.
        '
        '   T:System.Security.SecurityException:
        '     The caller does not have the required permission.
        '
        '   T:System.IO.FileNotFoundException:
        '     The file cannot be found, such as when mode is FileMode.Truncate or FileMode.Open,
        '     and the file specified by path does not exist. The file must already exist in
        '     these modes.
        '
        '   T:System.IO.IOException:
        '     An I/O error, such as specifying FileMode.CreateNew when the file specified by
        '     path already exists, occurred.-or-The stream has been closed.
        '
        '   T:System.IO.DirectoryNotFoundException:
        '     The specified path is invalid, such as being on an unmapped drive.
        '
        '   T:System.IO.PathTooLongException:
        '     The specified path, file name, or both exceed the system-defined maximum length.
        '     For example, on Windows-based platforms, paths must be less than 248 characters,
        '     and file names must be less than 260 characters.
        '
        '   T:System.ArgumentOutOfRangeException:
        '     mode contains an invalid value.
        ''' <summary>
        ''' Initializes a new instance of the System.IO.FileStream class with the specified
        ''' path and creation mode.
        ''' </summary>
        ''' <param name="path">远程机器上面的文件</param>
        ''' <param name="mode"></param>
        <SecuritySafeCritical> Public Sub New(path As String, mode As FileMode, remote As FileSystem)
            Call MyBase.New(remote)
            Name = path
            FileHandle = remote.OpenFileHandle(path, mode)
        End Sub

        Public ReadOnly Property FileHandle As FileHandle

        ''
        '' Summary:
        ''     Initializes a new instance of the System.IO.FileStream class for the specified
        ''     file handle, with the specified read/write permission.
        ''
        '' Parameters:
        ''   handle:
        ''     A file handle for the file that the current FileStream object will encapsulate.
        ''
        ''   access:
        ''     A constant that sets the System.IO.FileStream.CanRead and System.IO.FileStream.CanWrite
        ''     properties of the FileStream object.
        ''
        '' Exceptions:
        ''   T:System.ArgumentException:
        ''     access is not a field of System.IO.FileAccess.
        ''
        ''   T:System.Security.SecurityException:
        ''     The caller does not have the required permission.
        ''
        ''   T:System.IO.IOException:
        ''     An I/O error, such as a disk error, occurred.-or-The stream has been closed.
        ''
        ''   T:System.UnauthorizedAccessException:
        ''     The access requested is not permitted by the operating system for the specified
        ''     file handle, such as when access is Write or ReadWrite and the file handle is
        ''     set for read-only access.
        '<SecuritySafeCritical> Public Sub New(handle As SafeFileHandle, access As FileAccess)

        'End Sub
        ''
        '' Summary:
        ''     Initializes a new instance of the System.IO.FileStream class for the specified
        ''     file handle, with the specified read/write permission.
        ''
        '' Parameters:
        ''   handle:
        ''     A file handle for the file that the current FileStream object will encapsulate.
        ''
        ''   access:
        ''     A constant that sets the System.IO.FileStream.CanRead and System.IO.FileStream.CanWrite
        ''     properties of the FileStream object.
        ''
        '' Exceptions:
        ''   T:System.ArgumentException:
        ''     access is not a field of System.IO.FileAccess.
        ''
        ''   T:System.Security.SecurityException:
        ''     The caller does not have the required permission.
        ''
        ''   T:System.IO.IOException:
        ''     An I/O error, such as a disk error, occurred.-or-The stream has been closed.
        ''
        ''   T:System.UnauthorizedAccessException:
        ''     The access requested is not permitted by the operating system for the specified
        ''     file handle, such as when access is Write or ReadWrite and the file handle is
        ''     set for read-only access.
        '<Obsolete("This constructor has been deprecated.  Please use new FileStream(SafeFileHandle handle, FileAccess access) instead.  http://go.microsoft.com/fwlink/?linkid=14202")>
        'Public Sub New(handle As IntPtr, access As FileAccess)

        'End Sub
        ''
        '' Summary:
        ''     Initializes a new instance of the System.IO.FileStream class with the specified
        ''     path, creation mode, and read/write permission.
        ''
        '' Parameters:
        ''   path:
        ''     A relative or absolute path for the file that the current FileStream object will
        ''     encapsulate.
        ''
        ''   mode:
        ''     A constant that determines how to open or create the file.
        ''
        ''   access:
        ''     A constant that determines how the file can be accessed by the FileStream object.
        ''     This also determines the values returned by the System.IO.FileStream.CanRead
        ''     and System.IO.FileStream.CanWrite properties of the FileStream object. System.IO.FileStream.CanSeek
        ''     is true if path specifies a disk file.
        ''
        '' Exceptions:
        ''   T:System.ArgumentNullException:
        ''     path is null.
        ''
        ''   T:System.ArgumentException:
        ''     path is an empty string (""), contains only white space, or contains one or more
        ''     invalid characters. -or-path refers to a non-file device, such as "con:", "com1:",
        ''     "lpt1:", etc. in an NTFS environment.
        ''
        ''   T:System.NotSupportedException:
        ''     path refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in a
        ''     non-NTFS environment.
        ''
        ''   T:System.IO.FileNotFoundException:
        ''     The file cannot be found, such as when mode is FileMode.Truncate or FileMode.Open,
        ''     and the file specified by path does not exist. The file must already exist in
        ''     these modes.
        ''
        ''   T:System.IO.IOException:
        ''     An I/O error, such as specifying FileMode.CreateNew when the file specified by
        ''     path already exists, occurred. -or-The stream has been closed.
        ''
        ''   T:System.Security.SecurityException:
        ''     The caller does not have the required permission.
        ''
        ''   T:System.IO.DirectoryNotFoundException:
        ''     The specified path is invalid, such as being on an unmapped drive.
        ''
        ''   T:System.UnauthorizedAccessException:
        ''     The access requested is not permitted by the operating system for the specified
        ''     path, such as when access is Write or ReadWrite and the file or directory is
        ''     set for read-only access.
        ''
        ''   T:System.IO.PathTooLongException:
        ''     The specified path, file name, or both exceed the system-defined maximum length.
        ''     For example, on Windows-based platforms, paths must be less than 248 characters,
        ''     and file names must be less than 260 characters.
        ''
        ''   T:System.ArgumentOutOfRangeException:
        ''     mode contains an invalid value.
        '<SecuritySafeCritical>
        'Public Sub New(path As String, mode As FileMode, access As FileAccess)

        'End Sub
        ''
        '' Summary:
        ''     Initializes a new instance of the System.IO.FileStream class for the specified
        ''     file handle, with the specified read/write permission, and buffer size.
        ''
        '' Parameters:
        ''   handle:
        ''     A file handle for the file that the current FileStream object will encapsulate.
        ''
        ''   access:
        ''     A System.IO.FileAccess constant that sets the System.IO.FileStream.CanRead and
        ''     System.IO.FileStream.CanWrite properties of the FileStream object.
        ''
        ''   bufferSize:
        ''     A positive System.Int32 value greater than 0 indicating the buffer size. The
        ''     default buffer size is 4096.
        ''
        '' Exceptions:
        ''   T:System.ArgumentException:
        ''     The handle parameter is an invalid handle.-or-The handle parameter is a synchronous
        ''     handle and it was used asynchronously.
        ''
        ''   T:System.ArgumentOutOfRangeException:
        ''     The bufferSize parameter is negative.
        ''
        ''   T:System.IO.IOException:
        ''     An I/O error, such as a disk error, occurred.-or-The stream has been closed.
        ''
        ''   T:System.Security.SecurityException:
        ''     The caller does not have the required permission.
        ''
        ''   T:System.UnauthorizedAccessException:
        ''     The access requested is not permitted by the operating system for the specified
        ''     file handle, such as when access is Write or ReadWrite and the file handle is
        ''     set for read-only access.
        '<SecuritySafeCritical>
        'Public Sub New(handle As SafeFileHandle, access As FileAccess, bufferSize As Integer)

        'End Sub
        ''
        '' Summary:
        ''     Initializes a new instance of the System.IO.FileStream class for the specified
        ''     file handle, with the specified read/write permission and FileStream instance
        ''     ownership.
        ''
        '' Parameters:
        ''   handle:
        ''     A file handle for the file that the current FileStream object will encapsulate.
        ''
        ''   access:
        ''     A constant that sets the System.IO.FileStream.CanRead and System.IO.FileStream.CanWrite
        ''     properties of the FileStream object.
        ''
        ''   ownsHandle:
        ''     true if the file handle will be owned by this FileStream instance; otherwise,
        ''     false.
        ''
        '' Exceptions:
        ''   T:System.ArgumentException:
        ''     access is not a field of System.IO.FileAccess.
        ''
        ''   T:System.Security.SecurityException:
        ''     The caller does not have the required permission.
        ''
        ''   T:System.IO.IOException:
        ''     An I/O error, such as a disk error, occurred.-or-The stream has been closed.
        ''
        ''   T:System.UnauthorizedAccessException:
        ''     The access requested is not permitted by the operating system for the specified
        ''     file handle, such as when access is Write or ReadWrite and the file handle is
        ''     set for read-only access.
        '<Obsolete("This constructor has been deprecated.  Please use new FileStream(SafeFileHandle handle, FileAccess access) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed.  http://go.microsoft.com/fwlink/?linkid=14202")>
        'Public Sub New(handle As IntPtr, access As FileAccess, ownsHandle As Boolean)

        'End Sub
        ''
        '' Summary:
        ''     Initializes a new instance of the System.IO.FileStream class with the specified
        ''     path, creation mode, read/write permission, and sharing permission.
        ''
        '' Parameters:
        ''   path:
        ''     A relative or absolute path for the file that the current FileStream object will
        ''     encapsulate.
        ''
        ''   mode:
        ''     A constant that determines how to open or create the file.
        ''
        ''   access:
        ''     A constant that determines how the file can be accessed by the FileStream object.
        ''     This also determines the values returned by the System.IO.FileStream.CanRead
        ''     and System.IO.FileStream.CanWrite properties of the FileStream object. System.IO.FileStream.CanSeek
        ''     is true if path specifies a disk file.
        ''
        ''   share:
        ''     A constant that determines how the file will be shared by processes.
        ''
        '' Exceptions:
        ''   T:System.ArgumentNullException:
        ''     path is null.
        ''
        ''   T:System.ArgumentException:
        ''     path is an empty string (""), contains only white space, or contains one or more
        ''     invalid characters. -or-path refers to a non-file device, such as "con:", "com1:",
        ''     "lpt1:", etc. in an NTFS environment.
        ''
        ''   T:System.NotSupportedException:
        ''     path refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in a
        ''     non-NTFS environment.
        ''
        ''   T:System.IO.FileNotFoundException:
        ''     The file cannot be found, such as when mode is FileMode.Truncate or FileMode.Open,
        ''     and the file specified by path does not exist. The file must already exist in
        ''     these modes.
        ''
        ''   T:System.IO.IOException:
        ''     An I/O error, such as specifying FileMode.CreateNew when the file specified by
        ''     path already exists, occurred. -or-The system is running Windows 98 or Windows
        ''     98 Second Edition and share is set to FileShare.Delete.-or-The stream has been
        ''     closed.
        ''
        ''   T:System.Security.SecurityException:
        ''     The caller does not have the required permission.
        ''
        ''   T:System.IO.DirectoryNotFoundException:
        ''     The specified path is invalid, such as being on an unmapped drive.
        ''
        ''   T:System.UnauthorizedAccessException:
        ''     The access requested is not permitted by the operating system for the specified
        ''     path, such as when access is Write or ReadWrite and the file or directory is
        ''     set for read-only access.
        ''
        ''   T:System.IO.PathTooLongException:
        ''     The specified path, file name, or both exceed the system-defined maximum length.
        ''     For example, on Windows-based platforms, paths must be less than 248 characters,
        ''     and file names must be less than 260 characters.
        ''
        ''   T:System.ArgumentOutOfRangeException:
        ''     mode contains an invalid value.
        '<SecuritySafeCritical>
        'Public Sub New(path As String, mode As FileMode, access As FileAccess, share As FileShare)

        'End Sub
        ''
        '' Summary:
        ''     Initializes a new instance of the System.IO.FileStream class for the specified
        ''     file handle, with the specified read/write permission, buffer size, and synchronous
        ''     or asynchronous state.
        ''
        '' Parameters:
        ''   handle:
        ''     A file handle for the file that this FileStream object will encapsulate.
        ''
        ''   access:
        ''     A constant that sets the System.IO.FileStream.CanRead and System.IO.FileStream.CanWrite
        ''     properties of the FileStream object.
        ''
        ''   bufferSize:
        ''     A positive System.Int32 value greater than 0 indicating the buffer size. The
        ''     default buffer size is 4096.
        ''
        ''   isAsync:
        ''     true if the handle was opened asynchronously (that is, in overlapped I/O mode);
        ''     otherwise, false.
        ''
        '' Exceptions:
        ''   T:System.ArgumentException:
        ''     The handle parameter is an invalid handle.-or-The handle parameter is a synchronous
        ''     handle and it was used asynchronously.
        ''
        ''   T:System.ArgumentOutOfRangeException:
        ''     The bufferSize parameter is negative.
        ''
        ''   T:System.IO.IOException:
        ''     An I/O error, such as a disk error, occurred.-or-The stream has been closed.
        ''
        ''   T:System.Security.SecurityException:
        ''     The caller does not have the required permission.
        ''
        ''   T:System.UnauthorizedAccessException:
        ''     The access requested is not permitted by the operating system for the specified
        ''     file handle, such as when access is Write or ReadWrite and the file handle is
        ''     set for read-only access.
        '<SecuritySafeCritical>
        'Public Sub New(handle As SafeFileHandle, access As FileAccess, bufferSize As Integer, isAsync As Boolean)

        'End Sub
        ''
        '' Summary:
        ''     Initializes a new instance of the System.IO.FileStream class for the specified
        ''     file handle, with the specified read/write permission, FileStream instance ownership,
        ''     and buffer size.
        ''
        '' Parameters:
        ''   handle:
        ''     A file handle for the file that this FileStream object will encapsulate.
        ''
        ''   access:
        ''     A constant that sets the System.IO.FileStream.CanRead and System.IO.FileStream.CanWrite
        ''     properties of the FileStream object.
        ''
        ''   ownsHandle:
        ''     true if the file handle will be owned by this FileStream instance; otherwise,
        ''     false.
        ''
        ''   bufferSize:
        ''     A positive System.Int32 value greater than 0 indicating the buffer size. The
        ''     default buffer size is 4096.
        ''
        '' Exceptions:
        ''   T:System.ArgumentOutOfRangeException:
        ''     bufferSize is negative.
        ''
        ''   T:System.IO.IOException:
        ''     An I/O error, such as a disk error, occurred.-or-The stream has been closed.
        ''
        ''   T:System.Security.SecurityException:
        ''     The caller does not have the required permission.
        ''
        ''   T:System.UnauthorizedAccessException:
        ''     The access requested is not permitted by the operating system for the specified
        ''     file handle, such as when access is Write or ReadWrite and the file handle is
        ''     set for read-only access.
        '<Obsolete("This constructor has been deprecated.  Please use new FileStream(SafeFileHandle handle, FileAccess access, int bufferSize) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed.  http://go.microsoft.com/fwlink/?linkid=14202")>
        'Public Sub New(handle As IntPtr, access As FileAccess, ownsHandle As Boolean, bufferSize As Integer)

        'End Sub
        ''
        '' Summary:
        ''     Initializes a new instance of the System.IO.FileStream class for the specified
        ''     file handle, with the specified read/write permission, FileStream instance ownership,
        ''     buffer size, and synchronous or asynchronous state.
        ''
        '' Parameters:
        ''   handle:
        ''     A file handle for the file that this FileStream object will encapsulate.
        ''
        ''   access:
        ''     A constant that sets the System.IO.FileStream.CanRead and System.IO.FileStream.CanWrite
        ''     properties of the FileStream object.
        ''
        ''   ownsHandle:
        ''     true if the file handle will be owned by this FileStream instance; otherwise,
        ''     false.
        ''
        ''   bufferSize:
        ''     A positive System.Int32 value greater than 0 indicating the buffer size. The
        ''     default buffer size is 4096.
        ''
        ''   isAsync:
        ''     true if the handle was opened asynchronously (that is, in overlapped I/O mode);
        ''     otherwise, false.
        ''
        '' Exceptions:
        ''   T:System.ArgumentOutOfRangeException:
        ''     access is less than FileAccess.Read or greater than FileAccess.ReadWrite or bufferSize
        ''     is less than or equal to 0.
        ''
        ''   T:System.ArgumentException:
        ''     The handle is invalid.
        ''
        ''   T:System.IO.IOException:
        ''     An I/O error, such as a disk error, occurred.-or-The stream has been closed.
        ''
        ''   T:System.Security.SecurityException:
        ''     The caller does not have the required permission.
        ''
        ''   T:System.UnauthorizedAccessException:
        ''     The access requested is not permitted by the operating system for the specified
        ''     file handle, such as when access is Write or ReadWrite and the file handle is
        ''     set for read-only access.
        '<Obsolete("This constructor has been deprecated.  Please use new FileStream(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed.  http://go.microsoft.com/fwlink/?linkid=14202")> <SecuritySafeCritical>
        'Public Sub New(handle As IntPtr, access As FileAccess, ownsHandle As Boolean, bufferSize As Integer, isAsync As Boolean)

        'End Sub
        ''
        '' Summary:
        ''     Initializes a new instance of the System.IO.FileStream class with the specified
        ''     path, creation mode, read/write and sharing permission, and buffer size.
        ''
        '' Parameters:
        ''   path:
        ''     A relative or absolute path for the file that the current FileStream object will
        ''     encapsulate.
        ''
        ''   mode:
        ''     A constant that determines how to open or create the file.
        ''
        ''   access:
        ''     A constant that determines how the file can be accessed by the FileStream object.
        ''     This also determines the values returned by the System.IO.FileStream.CanRead
        ''     and System.IO.FileStream.CanWrite properties of the FileStream object. System.IO.FileStream.CanSeek
        ''     is true if path specifies a disk file.
        ''
        ''   share:
        ''     A constant that determines how the file will be shared by processes.
        ''
        ''   bufferSize:
        ''     A positive System.Int32 value greater than 0 indicating the buffer size. The
        ''     default buffer size is 4096.
        ''
        '' Exceptions:
        ''   T:System.ArgumentNullException:
        ''     path is null.
        ''
        ''   T:System.ArgumentException:
        ''     path is an empty string (""), contains only white space, or contains one or more
        ''     invalid characters. -or-path refers to a non-file device, such as "con:", "com1:",
        ''     "lpt1:", etc. in an NTFS environment.
        ''
        ''   T:System.NotSupportedException:
        ''     path refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in a
        ''     non-NTFS environment.
        ''
        ''   T:System.ArgumentOutOfRangeException:
        ''     bufferSize is negative or zero.-or- mode, access, or share contain an invalid
        ''     value.
        ''
        ''   T:System.IO.FileNotFoundException:
        ''     The file cannot be found, such as when mode is FileMode.Truncate or FileMode.Open,
        ''     and the file specified by path does not exist. The file must already exist in
        ''     these modes.
        ''
        ''   T:System.IO.IOException:
        ''     An I/O error, such as specifying FileMode.CreateNew when the file specified by
        ''     path already exists, occurred. -or-The system is running Windows 98 or Windows
        ''     98 Second Edition and share is set to FileShare.Delete.-or-The stream has been
        ''     closed.
        ''
        ''   T:System.Security.SecurityException:
        ''     The caller does not have the required permission.
        ''
        ''   T:System.IO.DirectoryNotFoundException:
        ''     The specified path is invalid, such as being on an unmapped drive.
        ''
        ''   T:System.UnauthorizedAccessException:
        ''     The access requested is not permitted by the operating system for the specified
        ''     path, such as when access is Write or ReadWrite and the file or directory is
        ''     set for read-only access.
        ''
        ''   T:System.IO.PathTooLongException:
        ''     The specified path, file name, or both exceed the system-defined maximum length.
        ''     For example, on Windows-based platforms, paths must be less than 248 characters,
        ''     and file names must be less than 260 characters.
        '<SecuritySafeCritical>
        'Public Sub New(path As String, mode As FileMode, access As FileAccess, share As FileShare, bufferSize As Integer)

        'End Sub
        ''
        '' Summary:
        ''     Initializes a new instance of the System.IO.FileStream class with the specified
        ''     path, creation mode, read/write and sharing permission, buffer size, and synchronous
        ''     or asynchronous state.
        ''
        '' Parameters:
        ''   path:
        ''     A relative or absolute path for the file that the current FileStream object will
        ''     encapsulate.
        ''
        ''   mode:
        ''     A constant that determines how to open or create the file.
        ''
        ''   access:
        ''     A constant that determines how the file can be accessed by the FileStream object.
        ''     This also determines the values returned by the System.IO.FileStream.CanRead
        ''     and System.IO.FileStream.CanWrite properties of the FileStream object. System.IO.FileStream.CanSeek
        ''     is true if path specifies a disk file.
        ''
        ''   share:
        ''     A constant that determines how the file will be shared by processes.
        ''
        ''   bufferSize:
        ''     A positive System.Int32 value greater than 0 indicating the buffer size. The
        ''     default buffer size is 4096..
        ''
        ''   useAsync:
        ''     Specifies whether to use asynchronous I/O or synchronous I/O. However, note that
        ''     the underlying operating system might not support asynchronous I/O, so when specifying
        ''     true, the handle might be opened synchronously depending on the platform. When
        ''     opened asynchronously, the System.IO.FileStream.BeginRead(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)
        ''     and System.IO.FileStream.BeginWrite(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)
        ''     methods perform better on large reads or writes, but they might be much slower
        ''     for small reads or writes. If the application is designed to take advantage of
        ''     asynchronous I/O, set the useAsync parameter to true. Using asynchronous I/O
        ''     correctly can speed up applications by as much as a factor of 10, but using it
        ''     without redesigning the application for asynchronous I/O can decrease performance
        ''     by as much as a factor of 10.
        ''
        '' Exceptions:
        ''   T:System.ArgumentNullException:
        ''     path is null.
        ''
        ''   T:System.ArgumentException:
        ''     path is an empty string (""), contains only white space, or contains one or more
        ''     invalid characters. -or-path refers to a non-file device, such as "con:", "com1:",
        ''     "lpt1:", etc. in an NTFS environment.
        ''
        ''   T:System.NotSupportedException:
        ''     path refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in a
        ''     non-NTFS environment.
        ''
        ''   T:System.ArgumentOutOfRangeException:
        ''     bufferSize is negative or zero.-or- mode, access, or share contain an invalid
        ''     value.
        ''
        ''   T:System.IO.FileNotFoundException:
        ''     The file cannot be found, such as when mode is FileMode.Truncate or FileMode.Open,
        ''     and the file specified by path does not exist. The file must already exist in
        ''     these modes.
        ''
        ''   T:System.IO.IOException:
        ''     An I/O error, such as specifying FileMode.CreateNew when the file specified by
        ''     path already exists, occurred.-or- The system is running Windows 98 or Windows
        ''     98 Second Edition and share is set to FileShare.Delete.-or-The stream has been
        ''     closed.
        ''
        ''   T:System.Security.SecurityException:
        ''     The caller does not have the required permission.
        ''
        ''   T:System.IO.DirectoryNotFoundException:
        ''     The specified path is invalid, such as being on an unmapped drive.
        ''
        ''   T:System.UnauthorizedAccessException:
        ''     The access requested is not permitted by the operating system for the specified
        ''     path, such as when access is Write or ReadWrite and the file or directory is
        ''     set for read-only access.
        ''
        ''   T:System.IO.PathTooLongException:
        ''     The specified path, file name, or both exceed the system-defined maximum length.
        ''     For example, on Windows-based platforms, paths must be less than 248 characters,
        ''     and file names must be less than 260 characters.
        '<SecuritySafeCritical>
        'Public Sub New(path As String, mode As FileMode, access As FileAccess, share As FileShare, bufferSize As Integer, useAsync As Boolean)

        'End Sub
        ''
        '' Summary:
        ''     Initializes a new instance of the System.IO.FileStream class with the specified
        ''     path, creation mode, read/write and sharing permission, the access other FileStreams
        ''     can have to the same file, the buffer size, and additional file options.
        ''
        '' Parameters:
        ''   path:
        ''     A relative or absolute path for the file that the current FileStream object will
        ''     encapsulate.
        ''
        ''   mode:
        ''     A constant that determines how to open or create the file.
        ''
        ''   access:
        ''     A constant that determines how the file can be accessed by the FileStream object.
        ''     This also determines the values returned by the System.IO.FileStream.CanRead
        ''     and System.IO.FileStream.CanWrite properties of the FileStream object. System.IO.FileStream.CanSeek
        ''     is true if path specifies a disk file.
        ''
        ''   share:
        ''     A constant that determines how the file will be shared by processes.
        ''
        ''   bufferSize:
        ''     A positive System.Int32 value greater than 0 indicating the buffer size. The
        ''     default buffer size is 4096.
        ''
        ''   options:
        ''     A value that specifies additional file options.
        ''
        '' Exceptions:
        ''   T:System.ArgumentNullException:
        ''     path is null.
        ''
        ''   T:System.ArgumentException:
        ''     path is an empty string (""), contains only white space, or contains one or more
        ''     invalid characters. -or-path refers to a non-file device, such as "con:", "com1:",
        ''     "lpt1:", etc. in an NTFS environment.
        ''
        ''   T:System.NotSupportedException:
        ''     path refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in a
        ''     non-NTFS environment.
        ''
        ''   T:System.ArgumentOutOfRangeException:
        ''     bufferSize is negative or zero.-or- mode, access, or share contain an invalid
        ''     value.
        ''
        ''   T:System.IO.FileNotFoundException:
        ''     The file cannot be found, such as when mode is FileMode.Truncate or FileMode.Open,
        ''     and the file specified by path does not exist. The file must already exist in
        ''     these modes.
        ''
        ''   T:System.IO.IOException:
        ''     An I/O error, such as specifying FileMode.CreateNew when the file specified by
        ''     path already exists, occurred.-or-The stream has been closed.
        ''
        ''   T:System.Security.SecurityException:
        ''     The caller does not have the required permission.
        ''
        ''   T:System.IO.DirectoryNotFoundException:
        ''     The specified path is invalid, such as being on an unmapped drive.
        ''
        ''   T:System.UnauthorizedAccessException:
        ''     The access requested is not permitted by the operating system for the specified
        ''     path, such as when access is Write or ReadWrite and the file or directory is
        ''     set for read-only access. -or-System.IO.FileOptions.Encrypted is specified for
        ''     options, but file encryption is not supported on the current platform.
        ''
        ''   T:System.IO.PathTooLongException:
        ''     The specified path, file name, or both exceed the system-defined maximum length.
        ''     For example, on Windows-based platforms, paths must be less than 248 characters,
        ''     and file names must be less than 260 characters.
        '<SecuritySafeCritical>
        'Public Sub New(path As String, mode As FileMode, access As FileAccess, share As FileShare, bufferSize As Integer, options As FileOptions)

        'End Sub
        ''
        '' Summary:
        ''     Initializes a new instance of the System.IO.FileStream class with the specified
        ''     path, creation mode, access rights and sharing permission, the buffer size, and
        ''     additional file options.
        ''
        '' Parameters:
        ''   path:
        ''     A relative or absolute path for the file that the current System.IO.FileStream
        ''     object will encapsulate.
        ''
        ''   mode:
        ''     A constant that determines how to open or create the file.
        ''
        ''   rights:
        ''     A constant that determines the access rights to use when creating access and
        ''     audit rules for the file.
        ''
        ''   share:
        ''     A constant that determines how the file will be shared by processes.
        ''
        ''   bufferSize:
        ''     A positive System.Int32 value greater than 0 indicating the buffer size. The
        ''     default buffer size is 4096.
        ''
        ''   options:
        ''     A constant that specifies additional file options.
        ''
        '' Exceptions:
        ''   T:System.ArgumentNullException:
        ''     path is null.
        ''
        ''   T:System.ArgumentException:
        ''     path is an empty string (""), contains only white space, or contains one or more
        ''     invalid characters. -or-path refers to a non-file device, such as "con:", "com1:",
        ''     "lpt1:", etc. in an NTFS environment.
        ''
        ''   T:System.NotSupportedException:
        ''     path refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in a
        ''     non-NTFS environment.
        ''
        ''   T:System.ArgumentOutOfRangeException:
        ''     bufferSize is negative or zero.-or- mode, access, or share contain an invalid
        ''     value.
        ''
        ''   T:System.IO.FileNotFoundException:
        ''     The file cannot be found, such as when mode is FileMode.Truncate or FileMode.Open,
        ''     and the file specified by path does not exist. The file must already exist in
        ''     these modes.
        ''
        ''   T:System.PlatformNotSupportedException:
        ''     The current operating system is not Windows NT or later.
        ''
        ''   T:System.IO.IOException:
        ''     An I/O error, such as specifying FileMode.CreateNew when the file specified by
        ''     path already exists, occurred. -or-The stream has been closed.
        ''
        ''   T:System.Security.SecurityException:
        ''     The caller does not have the required permission.
        ''
        ''   T:System.IO.DirectoryNotFoundException:
        ''     The specified path is invalid, such as being on an unmapped drive.
        ''
        ''   T:System.UnauthorizedAccessException:
        ''     The access requested is not permitted by the operating system for the specified
        ''     path, such as when access is Write or ReadWrite and the file or directory is
        ''     set for read-only access. -or-System.IO.FileOptions.Encrypted is specified for
        ''     options, but file encryption is not supported on the current platform.
        ''
        ''   T:System.IO.PathTooLongException:
        ''     The specified path, file name, or both exceed the system-defined maximum length.
        ''     For example, on Windows-based platforms, paths must be less than 248 characters,
        ''     and file names must be less than 260 characters.
        '<SecuritySafeCritical>
        'Public Sub New(path As String, mode As FileMode, rights As FileSystemRights, share As FileShare, bufferSize As Integer, options As FileOptions)

        'End Sub
        ''
        '' Summary:
        ''     Initializes a new instance of the System.IO.FileStream class with the specified
        ''     path, creation mode, access rights and sharing permission, the buffer size, additional
        ''     file options, access control and audit security.
        ''
        '' Parameters:
        ''   path:
        ''     A relative or absolute path for the file that the current System.IO.FileStream
        ''     object will encapsulate.
        ''
        ''   mode:
        ''     A constant that determines how to open or create the file.
        ''
        ''   rights:
        ''     A constant that determines the access rights to use when creating access and
        ''     audit rules for the file.
        ''
        ''   share:
        ''     A constant that determines how the file will be shared by processes.
        ''
        ''   bufferSize:
        ''     A positive System.Int32 value greater than 0 indicating the buffer size. The
        ''     default buffer size is 4096.
        ''
        ''   options:
        ''     A constant that specifies additional file options.
        ''
        ''   fileSecurity:
        ''     A constant that determines the access control and audit security for the file.
        ''
        '' Exceptions:
        ''   T:System.ArgumentNullException:
        ''     path is null.
        ''
        ''   T:System.ArgumentException:
        ''     path is an empty string (""), contains only white space, or contains one or more
        ''     invalid characters. -or-path refers to a non-file device, such as "con:", "com1:",
        ''     "lpt1:", etc. in an NTFS environment.
        ''
        ''   T:System.NotSupportedException:
        ''     path refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in a
        ''     non-NTFS environment.
        ''
        ''   T:System.ArgumentOutOfRangeException:
        ''     bufferSize is negative or zero.-or- mode, access, or share contain an invalid
        ''     value.
        ''
        ''   T:System.IO.FileNotFoundException:
        ''     The file cannot be found, such as when mode is FileMode.Truncate or FileMode.Open,
        ''     and the file specified by path does not exist. The file must already exist in
        ''     these modes.
        ''
        ''   T:System.IO.IOException:
        ''     An I/O error, such as specifying FileMode.CreateNew when the file specified by
        ''     path already exists, occurred. -or-The stream has been closed.
        ''
        ''   T:System.Security.SecurityException:
        ''     The caller does not have the required permission.
        ''
        ''   T:System.IO.DirectoryNotFoundException:
        ''     The specified path is invalid, such as being on an unmapped drive.
        ''
        ''   T:System.UnauthorizedAccessException:
        ''     The access requested is not permitted by the operating system for the specified
        ''     path, such as when access is Write or ReadWrite and the file or directory is
        ''     set for read-only access. -or-System.IO.FileOptions.Encrypted is specified for
        ''     options, but file encryption is not supported on the current platform.
        ''
        ''   T:System.IO.PathTooLongException:
        ''     The specified path, file name, or both exceed the system-defined maximum length.
        ''     For example, on Windows-based platforms, paths must be less than 248 characters,
        ''     and file names must be less than 260 characters.
        ''
        ''   T:System.PlatformNotSupportedException:
        ''     The current operating system is not Windows NT or later.
        '<SecuritySafeCritical>
        'Public Sub New(path As String, mode As FileMode, rights As FileSystemRights, share As FileShare, bufferSize As Integer, options As FileOptions, fileSecurity As FileSecurity)

        'End Sub

        '
        ' Summary:
        '     Gets a value indicating whether the current stream supports reading.
        '
        ' Returns:
        '     true if the stream supports reading; false if the stream is closed or was opened
        '     with write-only access.

        Public Overrides ReadOnly Property CanRead As Boolean
            Get

            End Get
        End Property

        '
        ' Summary:
        '     Gets a value indicating whether the current stream supports seeking.
        '
        ' Returns:
        '     true if the stream supports seeking; false if the stream is closed or if the
        '     FileStream was constructed from an operating-system handle such as a pipe or
        '     output to the console.
        Public Overrides ReadOnly Property CanSeek As Boolean
        '
        ' Summary:
        '     Gets a value indicating whether the current stream supports writing.
        '
        ' Returns:
        '     true if the stream supports writing; false if the stream is closed or was opened
        '     with read-only access.
        Public Overrides ReadOnly Property CanWrite As Boolean
        '
        ' Summary:
        '     Gets the operating system file handle for the file that the current FileStream
        '     object encapsulates.
        '
        ' Returns:
        '     The operating system file handle for the file encapsulated by this FileStream
        '     object, or -1 if the FileStream has been closed.
        '
        ' Exceptions:
        '   T:System.Security.SecurityException:
        '     The caller does not have the required permission.
        <Obsolete("This property has been deprecated.  Please use FileStream's SafeFileHandle property instead.  http://go.microsoft.com/fwlink/?linkid=14202")>
        Public Overridable ReadOnly Property Handle As IntPtr
        '
        ' Summary:
        '     Gets a value indicating whether the FileStream was opened asynchronously or synchronously.
        '
        ' Returns:
        '     true if the FileStream was opened asynchronously; otherwise, false.
        Public Overridable ReadOnly Property IsAsync As Boolean
        '
        ' Summary:
        '     Gets the length in bytes of the stream.
        '
        ' Returns:
        '     A long value representing the length of the stream in bytes.
        '
        ' Exceptions:
        '   T:System.NotSupportedException:
        '     System.IO.FileStream.CanSeek for this stream is false.
        '
        '   T:System.IO.IOException:
        '     An I/O error, such as the file being closed, occurred.
        Public Overrides ReadOnly Property Length As Long

        ''' <summary>
        ''' Gets the name of the FileStream that was passed to the constructor.
        ''' </summary>
        ''' <returns>A string that is the name of the FileStream.</returns>
        Public ReadOnly Property Name As String
        '
        ' Summary:
        '     Gets or sets the current position of this stream.
        '
        ' Returns:
        '     The current position of this stream.
        '
        ' Exceptions:
        '   T:System.NotSupportedException:
        '     The stream does not support seeking.
        '
        '   T:System.IO.IOException:
        '     An I/O error occurred. - or -The position was set to a very large value beyond
        '     the end of the stream in Windows 98 or earlier.
        '
        '   T:System.ArgumentOutOfRangeException:
        '     Attempted to set the position to a negative value.
        '
        '   T:System.IO.EndOfStreamException:
        '     Attempted seeking past the end of a stream that does not support this.
        Public Overrides Property Position As Long
        '
        ' Summary:
        '     Gets a Microsoft.Win32.SafeHandles.SafeFileHandle object that represents the
        '     operating system file handle for the file that the current System.IO.FileStream
        '     object encapsulates.
        '
        ' Returns:
        '     An object that represents the operating system file handle for the file that
        '     the current System.IO.FileStream object encapsulates.
        Public Overridable ReadOnly Property SafeFileHandle As SafeFileHandle

        '
        ' Summary:
        '     Ends an asynchronous write operation and blocks until the I/O operation is complete.
        '     (Consider using System.IO.FileStream.WriteAsync(System.Byte[],System.Int32,System.Int32,System.Threading.CancellationToken)
        '     instead; see the Remarks section.)
        '
        ' Parameters:
        '   asyncResult:
        '     The pending asynchronous I/O request.
        '
        ' Exceptions:
        '   T:System.ArgumentNullException:
        '     asyncResult is null.
        '
        '   T:System.ArgumentException:
        '     This System.IAsyncResult object was not created by calling System.IO.Stream.BeginWrite(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)
        '     on this class.
        '
        '   T:System.InvalidOperationException:
        '     System.IO.FileStream.EndWrite(System.IAsyncResult) is called multiple times.
        '
        '   T:System.IO.IOException:
        '     The stream is closed or an internal error has occurred.
        <SecuritySafeCritical>
        Public Overrides Sub EndWrite(asyncResult As IAsyncResult)

        End Sub
        '
        ' Summary:
        '     Clears buffers for this stream and causes any buffered data to be written to
        '     the file.
        '
        ' Exceptions:
        '   T:System.IO.IOException:
        '     An I/O error occurred.
        '
        '   T:System.ObjectDisposedException:
        '     The stream is closed.
        Public Overrides Sub Flush()

        End Sub
        '
        ' Summary:
        '     Clears buffers for this stream and causes any buffered data to be written to
        '     the file, and also clears all intermediate file buffers.
        '
        ' Parameters:
        '   flushToDisk:
        '     true to flush all intermediate file buffers; otherwise, false.
        <SecuritySafeCritical>
        Public Overloads Sub Flush(flushToDisk As Boolean)

        End Sub
        '
        ' Summary:
        '     Prevents other processes from reading from or writing to the System.IO.FileStream.
        '
        ' Parameters:
        '   position:
        '     The beginning of the range to lock. The value of this parameter must be equal
        '     to or greater than zero (0).
        '
        '   length:
        '     The range to be locked.
        '
        ' Exceptions:
        '   T:System.ArgumentOutOfRangeException:
        '     position or length is negative.
        '
        '   T:System.ObjectDisposedException:
        '     The file is closed.
        '
        '   T:System.IO.IOException:
        '     The process cannot access the file because another process has locked a portion
        '     of the file.
        <SecuritySafeCritical>
        Public Overridable Sub Lock(position As Long, length As Long)

        End Sub
        '
        ' Summary:
        '     Applies access control list (ACL) entries described by a System.Security.AccessControl.FileSecurity
        '     object to the file described by the current System.IO.FileStream object.
        '
        ' Parameters:
        '   fileSecurity:
        '     An object that describes an ACL entry to apply to the current file.
        '
        ' Exceptions:
        '   T:System.ObjectDisposedException:
        '     The file is closed.
        '
        '   T:System.ArgumentNullException:
        '     The fileSecurity parameter is null.
        '
        '   T:System.SystemException:
        '     The file could not be found or modified.
        '
        '   T:System.UnauthorizedAccessException:
        '     The current process does not have access to open the file.
        <SecuritySafeCritical>
        Public Sub SetAccessControl(fileSecurity As FileSecurity)

        End Sub
        '
        ' Summary:
        '     Sets the length of this stream to the given value.
        '
        ' Parameters:
        '   value:
        '     The new length of the stream.
        '
        ' Exceptions:
        '   T:System.IO.IOException:
        '     An I/O error has occurred.
        '
        '   T:System.NotSupportedException:
        '     The stream does not support both writing and seeking.
        '
        '   T:System.ArgumentOutOfRangeException:
        '     Attempted to set the value parameter to less than 0.
        <SecuritySafeCritical>
        Public Overrides Sub SetLength(value As Long)

        End Sub
        '
        ' Summary:
        '     Allows access by other processes to all or part of a file that was previously
        '     locked.
        '
        ' Parameters:
        '   position:
        '     The beginning of the range to unlock.
        '
        '   length:
        '     The range to be unlocked.
        '
        ' Exceptions:
        '   T:System.ArgumentOutOfRangeException:
        '     position or length is negative.
        <SecuritySafeCritical>
        Public Overridable Sub Unlock(position As Long, length As Long)

        End Sub

        ' Exceptions:
        '   T:System.ArgumentNullException:
        '     array is null.
        '
        '   T:System.ArgumentException:
        '     offset and count describe an invalid range in array.
        '
        '   T:System.ArgumentOutOfRangeException:
        '     offset or count is negative.
        '
        '   T:System.IO.IOException:
        '     An I/O error occurred. - or -Another thread may have caused an unexpected change
        '     in the position of the operating system's file handle.
        '
        '   T:System.ObjectDisposedException:
        '     The stream is closed.
        '
        '   T:System.NotSupportedException:
        '     The current stream instance does not support writing.
        ''' <summary>
        ''' Writes a block of bytes to the file stream.
        ''' </summary>
        ''' <param name="array">The buffer containing data to write to the stream.</param>
        ''' <param name="offset">The zero-based byte offset in array from which to begin copying bytes to the
        ''' stream.</param>
        ''' <param name="count">The maximum number of bytes to write.</param>
        <SecuritySafeCritical>
        Public Overrides Sub Write(array() As Byte, offset As Integer, count As Integer)
            Dim args As WriteStream = New WriteStream With {
                .Handle = FileHandle,
                .buffer = array,
                .length = count,
                .offset = offset
            }
            Dim req As New RequestStream(ProtocolEntry, FileSystemAPI.WriteBuffer, args)
            Dim invoke As New AsynInvoke(FileSystem.Portal)
            Call invoke.SendMessage(req)
        End Sub

        '
        ' Summary:
        '     Writes a byte to the current position in the file stream.
        '
        ' Parameters:
        '   value:
        '     A byte to write to the stream.
        '
        ' Exceptions:
        '   T:System.ObjectDisposedException:
        '     The stream is closed.
        '
        '   T:System.NotSupportedException:
        '     The stream does not support writing.
        <SecuritySafeCritical>
        Public Overrides Sub WriteByte(value As Byte)

        End Sub
        '
        ' Summary:
        '     Releases the unmanaged resources used by the System.IO.FileStream and optionally
        '     releases the managed resources.
        '
        ' Parameters:
        '   disposing:
        '     true to release both managed and unmanaged resources; false to release only unmanaged
        '     resources.
        <SecuritySafeCritical>
        Protected Overrides Sub Dispose(disposing As Boolean)

        End Sub
        '
        ' Summary:
        '     Ensures that resources are freed and other cleanup operations are performed when
        '     the garbage collector reclaims the FileStream.
        <SecuritySafeCritical>
        Protected Overrides Sub Finalize()

        End Sub

        '
        ' Summary:
        '     Begins an asynchronous read operation. (Consider using System.IO.FileStream.ReadAsync(System.Byte[],System.Int32,System.Int32,System.Threading.CancellationToken)
        '     instead; see the Remarks section.)
        '
        ' Parameters:
        '   array:
        '     The buffer to read data into.
        '
        '   offset:
        '     The byte offset in array at which to begin reading.
        '
        '   numBytes:
        '     The maximum number of bytes to read.
        '
        '   userCallback:
        '     The method to be called when the asynchronous read operation is completed.
        '
        '   stateObject:
        '     A user-provided object that distinguishes this particular asynchronous read request
        '     from other requests.
        '
        ' Returns:
        '     An object that references the asynchronous read.
        '
        ' Exceptions:
        '   T:System.ArgumentException:
        '     The array length minus offset is less than numBytes.
        '
        '   T:System.ArgumentNullException:
        '     array is null.
        '
        '   T:System.ArgumentOutOfRangeException:
        '     offset or numBytes is negative.
        '
        '   T:System.IO.IOException:
        '     An asynchronous read was attempted past the end of the file.
        <SecuritySafeCritical>
        Public Overrides Function BeginRead(array() As Byte, offset As Integer, numBytes As Integer, userCallback As AsyncCallback, stateObject As Object) As IAsyncResult

        End Function
        '
        ' Summary:
        '     Begins an asynchronous write operation. (Consider using System.IO.FileStream.WriteAsync(System.Byte[],System.Int32,System.Int32,System.Threading.CancellationToken)
        '     instead; see the Remarks section.)
        '
        ' Parameters:
        '   array:
        '     The buffer containing data to write to the current stream.
        '
        '   offset:
        '     The zero-based byte offset in array at which to begin copying bytes to the current
        '     stream.
        '
        '   numBytes:
        '     The maximum number of bytes to write.
        '
        '   userCallback:
        '     The method to be called when the asynchronous write operation is completed.
        '
        '   stateObject:
        '     A user-provided object that distinguishes this particular asynchronous write
        '     request from other requests.
        '
        ' Returns:
        '     An object that references the asynchronous write.
        '
        ' Exceptions:
        '   T:System.ArgumentException:
        '     array length minus offset is less than numBytes.
        '
        '   T:System.ArgumentNullException:
        '     array is null.
        '
        '   T:System.ArgumentOutOfRangeException:
        '     offset or numBytes is negative.
        '
        '   T:System.NotSupportedException:
        '     The stream does not support writing.
        '
        '   T:System.ObjectDisposedException:
        '     The stream is closed.
        '
        '   T:System.IO.IOException:
        '     An I/O error occurred.
        <SecuritySafeCritical>
        Public Overrides Function BeginWrite(array() As Byte, offset As Integer, numBytes As Integer, userCallback As AsyncCallback, stateObject As Object) As IAsyncResult

        End Function
        '
        ' Summary:
        '     Waits for the pending asynchronous read operation to complete. (Consider using
        '     System.IO.FileStream.ReadAsync(System.Byte[],System.Int32,System.Int32,System.Threading.CancellationToken)
        '     instead; see the Remarks section.)
        '
        ' Parameters:
        '   asyncResult:
        '     The reference to the pending asynchronous request to wait for.
        '
        ' Returns:
        '     The number of bytes read from the stream, between 0 and the number of bytes you
        '     requested. Streams only return 0 at the end of the stream, otherwise, they should
        '     block until at least 1 byte is available.
        '
        ' Exceptions:
        '   T:System.ArgumentNullException:
        '     asyncResult is null.
        '
        '   T:System.ArgumentException:
        '     This System.IAsyncResult object was not created by calling System.IO.FileStream.BeginRead(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)
        '     on this class.
        '
        '   T:System.InvalidOperationException:
        '     System.IO.FileStream.EndRead(System.IAsyncResult) is called multiple times.
        '
        '   T:System.IO.IOException:
        '     The stream is closed or an internal error has occurred.
        <SecuritySafeCritical>
        Public Overrides Function EndRead(asyncResult As IAsyncResult) As Integer

        End Function
        '
        ' Summary:
        '     Asynchronously clears all buffers for this stream, causes any buffered data to
        '     be written to the underlying device, and monitors cancellation requests.
        '
        ' Parameters:
        '   cancellationToken:
        '     The token to monitor for cancellation requests.
        '
        ' Returns:
        '     A task that represents the asynchronous flush operation.
        '
        ' Exceptions:
        '   T:System.ObjectDisposedException:
        '     The stream has been disposed.
        <ComVisible(False)> <SecuritySafeCritical>
        Public Overrides Function FlushAsync(cancellationToken As CancellationToken) As Task

        End Function
        '
        ' Summary:
        '     Gets a System.Security.AccessControl.FileSecurity object that encapsulates the
        '     access control list (ACL) entries for the file described by the current System.IO.FileStream
        '     object.
        '
        ' Returns:
        '     An object that encapsulates the access control settings for the file described
        '     by the current System.IO.FileStream object.
        '
        ' Exceptions:
        '   T:System.ObjectDisposedException:
        '     The file is closed.
        '
        '   T:System.IO.IOException:
        '     An I/O error occurred while opening the file.
        '
        '   T:System.SystemException:
        '     The file could not be found.
        '
        '   T:System.UnauthorizedAccessException:
        '     This operation is not supported on the current platform.-or- The caller does
        '     not have the required permission.
        <SecuritySafeCritical>
        Public Function GetAccessControl() As FileSecurity

        End Function

        ' Exceptions:
        '   T:System.ArgumentNullException:
        '     array is null.
        '
        '   T:System.ArgumentOutOfRangeException:
        '     offset or count is negative.
        '
        '   T:System.NotSupportedException:
        '     The stream does not support reading.
        '
        '   T:System.IO.IOException:
        '     An I/O error occurred.
        '
        '   T:System.ArgumentException:
        '     offset and count describe an invalid range in array.
        '
        '   T:System.ObjectDisposedException:
        '     Methods were called after the stream was closed.
        ''' <summary>
        ''' Reads a block of bytes from the stream and writes the data in a given buffer.
        ''' </summary>
        ''' <param name="array">
        ''' When this method returns, contains the specified byte array with the values between
        ''' offset and (offset + count - 1) replaced by the bytes read from the current source.
        ''' </param>
        ''' <param name="offset">The byte offset in array at which the read bytes will be placed.</param>
        ''' <param name="count">The maximum number of bytes to read.</param>
        ''' <returns>The total number of bytes read into the buffer. This might be less than the number
        ''' of bytes requested if that number of bytes are not currently available, or zero
        ''' if the end of the stream is reached.</returns>
        <SecuritySafeCritical>
        Public Overrides Function Read(array() As Byte, offset As Integer, count As Integer) As Integer
            Dim args As ReadBuffer = New ReadBuffer(Me.FileHandle) With {
                .length = count,
                .offset = offset
            }
            Dim req As RequestStream =
                New RequestStream(ProtocolEntry, FileSystemAPI.ReadBuffer, args.GetJson)
            Dim invoke As New AsynInvoke(FileSystem.Portal)
            Dim rep As RequestStream = invoke.SendMessage(req)
            Call System.Array.ConstrainedCopy(rep.ChunkBuffer, Scan0, array, offset, count)
            Return rep.ChunkBuffer.Length
        End Function
        '
        ' Summary:
        '     Asynchronously reads a sequence of bytes from the current stream, advances the
        '     position within the stream by the number of bytes read, and monitors cancellation
        '     requests.
        '
        ' Parameters:
        '   buffer:
        '     The buffer to write the data into.
        '
        '   offset:
        '     The byte offset in buffer at which to begin writing data from the stream.
        '
        '   count:
        '     The maximum number of bytes to read.
        '
        '   cancellationToken:
        '     The token to monitor for cancellation requests.
        '
        ' Returns:
        '     A task that represents the asynchronous read operation. The value of the TResult
        '     parameter contains the total number of bytes read into the buffer. The result
        '     value can be less than the number of bytes requested if the number of bytes currently
        '     available is less than the requested number, or it can be 0 (zero) if the end
        '     of the stream has been reached.
        '
        ' Exceptions:
        '   T:System.ArgumentNullException:
        '     buffer is null.
        '
        '   T:System.ArgumentOutOfRangeException:
        '     offset or count is negative.
        '
        '   T:System.ArgumentException:
        '     The sum of offset and count is larger than the buffer length.
        '
        '   T:System.NotSupportedException:
        '     The stream does not support reading.
        '
        '   T:System.ObjectDisposedException:
        '     The stream has been disposed.
        '
        '   T:System.InvalidOperationException:
        '     The stream is currently in use by a previous read operation.
        <ComVisible(False)> <SecuritySafeCritical>
        Public Overrides Function ReadAsync(buffer() As Byte, offset As Integer, count As Integer, cancellationToken As CancellationToken) As Task(Of Integer)

        End Function
        '
        ' Summary:
        '     Reads a byte from the file and advances the read position one byte.
        '
        ' Returns:
        '     The byte, cast to an System.Int32, or -1 if the end of the stream has been reached.
        '
        ' Exceptions:
        '   T:System.NotSupportedException:
        '     The current stream does not support reading.
        '
        '   T:System.ObjectDisposedException:
        '     The current stream is closed.
        <SecuritySafeCritical>
        Public Overrides Function ReadByte() As Integer

        End Function
        '
        ' Summary:
        '     Sets the current position of this stream to the given value.
        '
        ' Parameters:
        '   offset:
        '     The point relative to origin from which to begin seeking.
        '
        '   origin:
        '     Specifies the beginning, the end, or the current position as a reference point
        '     for offset, using a value of type System.IO.SeekOrigin.
        '
        ' Returns:
        '     The new position in the stream.
        '
        ' Exceptions:
        '   T:System.IO.IOException:
        '     An I/O error occurred.
        '
        '   T:System.NotSupportedException:
        '     The stream does not support seeking, such as if the FileStream is constructed
        '     from a pipe or console output.
        '
        '   T:System.ArgumentException:
        '     Seeking is attempted before the beginning of the stream.
        '
        '   T:System.ObjectDisposedException:
        '     Methods were called after the stream was closed.
        <SecuritySafeCritical>
        Public Overrides Function Seek(offset As Long, origin As SeekOrigin) As Long

        End Function
        '
        ' Summary:
        '     Asynchronously writes a sequence of bytes to the current stream, advances the
        '     current position within this stream by the number of bytes written, and monitors
        '     cancellation requests.
        '
        ' Parameters:
        '   buffer:
        '     The buffer to write data from.
        '
        '   offset:
        '     The zero-based byte offset in buffer from which to begin copying bytes to the
        '     stream.
        '
        '   count:
        '     The maximum number of bytes to write.
        '
        '   cancellationToken:
        '     The token to monitor for cancellation requests.
        '
        ' Returns:
        '     A task that represents the asynchronous write operation.
        '
        ' Exceptions:
        '   T:System.ArgumentNullException:
        '     buffer is null.
        '
        '   T:System.ArgumentOutOfRangeException:
        '     offset or count is negative.
        '
        '   T:System.ArgumentException:
        '     The sum of offset and count is larger than the buffer length.
        '
        '   T:System.NotSupportedException:
        '     The stream does not support writing.
        '
        '   T:System.ObjectDisposedException:
        '     The stream has been disposed.
        '
        '   T:System.InvalidOperationException:
        '     The stream is currently in use by a previous write operation.
        <ComVisible(False)> <SecuritySafeCritical>
        Public Overrides Function WriteAsync(buffer() As Byte, offset As Integer, count As Integer, cancellationToken As CancellationToken) As Task

        End Function
    End Class
End Namespace