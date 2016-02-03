Imports Microsoft.VisualBasic.ComputingServices.ComponentModel
Imports Microsoft.VisualBasic.ComputingServices.FileSystem.Protocols
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Protocol
Imports Microsoft.VisualBasic.Net.Protocol.Reflection
Imports Microsoft.VisualBasic.Serialization
Imports Microsoft.VisualBasic.LINQ.Extensions
Imports System.IO

Namespace FileSystem

    <Protocol(GetType(FileSystemAPI))>
    Public Class FileSystemHost : Inherits IHostBase

        ''' <summary>
        ''' 远程服务器上面已经打开的文件句柄
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property OpenedHandles As Dictionary(Of String, FileStream) =
            New Dictionary(Of String, FileStream)

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="port"></param>
        Sub New(port As Integer)
            Dim protocols As New ProtocolHandler(Me)
            __host = New TcpSynchronizationServicesSocket(port)
            __host.Responsehandler = AddressOf protocols.HandleRequest
            Call Parallel.Run(AddressOf __host.Run)
        End Sub

        Public Overrides ReadOnly Property Portal As IPEndPoint
            Get
                Return New IPEndPoint(AsynInvoke.LocalIPAddress, __host.LocalPort)
            End Get
        End Property

        <Protocol(FileSystemAPI.ReadBuffer)>
        Private Function ReadBuffer(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream
            Dim handle = args.GetUTF8String.LoadObject(Of ReadBuffer)
            Dim uid As String = handle.Handle
            If OpenedHandles.ContainsKey(uid) Then
                Dim stream As FileStream = OpenedHandles(uid)
                Dim buffer As Byte() = handle.CreateBuffer
                Call stream.Read(buffer, handle.offset, handle.length)
                Dim value As RequestStream = RequestStream.CreatePackage(buffer)
                Return value
            Else
                Return New RequestStream(Scan0, HTTP_RFC.RFC_TOKEN_INVALID, $"File handle {uid} is not opened!")
            End If
        End Function

        '
        ' Summary:
        '     Gets or sets the current directory.
        '
        ' Returns:
        '     The current directory for file I/O operations.
        '
        ' Exceptions:
        '   T:System.IO.DirectoryNotFoundException:
        '     The path is not valid.
        '
        '   T:System.UnauthorizedAccessException:
        '     The user lacks necessary permissions.
        <Protocol(FileSystemAPI.CurrentDirectory)>
        Private Function CurrentDirectory(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream
            Dim DIR As String = args.GetUTF8String
            If String.IsNullOrEmpty(DIR) Then
                DIR = FileIO.FileSystem.CurrentDirectory    ' GET
                Return New RequestStream(DIR)
            Else
                Try
                    FileIO.FileSystem.CurrentDirectory = DIR    'SET
                Catch ex As Exception
                    Return New RequestStream(HTTP_RFC.RFC_OK, HTTP_RFC.RFC_INTERNAL_SERVER_ERROR, ex.ToString)
                End Try

                Return NetResponse.RFC_OK
            End If
        End Function
        '
        ' Summary:
        '     Returns a read-only collection of all available drive names.
        '
        ' Returns:
        '     A read-only collection of all available drives as System.IO.DriveInfo objects.

        <Protocol(FileSystemAPI.Drives)>
        Private Function Drives(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream
            Dim lst = FileIO.FileSystem.Drives.ToArray(Function(x) x.GetJson)
            Return New RequestStream(lst.GetJson)
        End Function

        '
        ' Summary:
        '     Copies the contents of a directory to another directory.
        '
        ' Parameters:
        '   sourceDirectoryName:
        '     The directory to be copied.
        '
        '   destinationDirectoryName:
        '     The location to which the directory contents should be copied.
        '
        ' Exceptions:
        '   T:System.ArgumentException:
        '     The new name specified for the directory contains a colon (:) or slash (\ or
        '     /).
        '
        '   T:System.ArgumentException:
        '     The path is not valid for one of the following reasons: it is a zero-length string;
        '     it contains only white space; it contains invalid characters; or it is a device
        '     path (starts with \\.\).
        '
        '   T:System.ArgumentNullException:
        '     destinationDirectoryName or sourceDirectoryName is Nothing or an empty string.
        '
        '   T:System.IO.DirectoryNotFoundException:
        '     The source directory does not exist.
        '
        '   T:System.IO.IOException:
        '     The source directory is a root directory
        '
        '   T:System.IO.IOException:
        '     The combined path points to an existing file.
        '
        '   T:System.IO.IOException:
        '     The source path and target path are the same.
        '
        '   T:System.InvalidOperationException:
        '     The operation is cyclic.
        '
        '   T:System.IO.PathTooLongException:
        '     The path exceeds the system-defined maximum length.
        '
        '   T:System.NotSupportedException:
        '     A folder name in the path contains a colon (:) or is in an invalid format.
        '
        '   T:System.Security.SecurityException:
        '     The user lacks necessary permissions to view the path.
        '
        '   T:System.UnauthorizedAccessException:
        '     A destination file exists but cannot be accessed.
        Private Function CopyDirectory(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream

        End Function

        '
        ' Summary:
        '     Copies a file to a new location.
        '
        ' Parameters:
        '   sourceFileName:
        '     The file to be copied.
        '
        '   destinationFileName:
        '     The location to which the file should be copied.
        '
        ' Exceptions:
        '   T:System.ArgumentException:
        '     The path is not valid for one of the following reasons: it is a zero-length string;
        '     it contains only white space; it contains invalid characters; or it is a device
        '     path (starts with \\.\).
        '
        '   T:System.ArgumentException:
        '     The system could not retrieve the absolute path.
        '
        '   T:System.ArgumentException:
        '     destinationFileName contains path information.
        '
        '   T:System.ArgumentNullException:
        '     destinationFileName or sourceFileName is Nothing or an empty string.
        '
        '   T:System.IO.FileNotFoundException:
        '     The source file is not valid or does not exist.
        '
        '   T:System.IO.IOException:
        '     The combined path points to an existing directory.
        '
        '   T:System.IO.IOException:
        '     The user does not have sufficient permissions to access the file.
        '
        '   T:System.IO.IOException:
        '     A file in the target directory with the same name is in use.
        '
        '   T:System.NotSupportedException:
        '     A file or directory name in the path contains a colon (:) or is in an invalid
        '     format.
        '
        '   T:System.IO.PathTooLongException:
        '     The path exceeds the system-defined maximum length.
        '
        '   T:System.UnauthorizedAccessException:
        '     The user does not have required permission.
        '
        '   T:System.Security.SecurityException:
        '     The user lacks necessary permissions to view the path.
        Private Function CopyFile(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream

        End Function

        '
        ' Summary:
        '     Creates a directory.
        '
        ' Parameters:
        '   directory:
        '     Name and location of the directory.
        '
        ' Exceptions:
        '   T:System.ArgumentException:
        '     The directory name is malformed. For example, it contains illegal characters
        '     or is only white space.
        '
        '   T:System.ArgumentNullException:
        '     directory is Nothing or an empty string.
        '
        '   T:System.IO.PathTooLongException:
        '     The directory name is too long.
        '
        '   T:System.NotSupportedException:
        '     The directory name is only a colon (:).
        '
        '   T:System.IO.IOException:
        '     The parent directory of the directory to be created is read-only
        '
        '   T:System.UnauthorizedAccessException:
        '     The user does not have permission to create the directory.
        Private Function CreateDirectory(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream

        End Function
        '
        ' Summary:
        '     Deletes a directory.
        '
        ' Parameters:
        '   directory:
        '     Directory to be deleted.
        '
        '   onDirectoryNotEmpty:
        '     Specifies what should be done when a directory that is to be deleted contains
        '     files or directories. Default is DeleteDirectoryOption.DeleteAllContents.
        '
        ' Exceptions:
        '   T:System.ArgumentException:
        '     The path is a zero-length string, is malformed, contains only white space, or
        '     contains invalid characters (including wildcard characters). The path is a device
        '     path (starts with \\.\).
        '
        '   T:System.ArgumentNullException:
        '     directory is Nothing or an empty string.
        '
        '   T:System.IO.DirectoryNotFoundException:
        '     The directory does not exist or is a file.
        '
        '   T:System.IO.IOException:
        '     The directory is not empty, and onDirectoryNotEmpty is set to ThrowIfDirectoryNonEmpty.
        '
        '   T:System.IO.IOException:
        '     The user does not have permission to delete the directory or subdirectory.
        '
        '   T:System.IO.IOException:
        '     A file in the directory or subdirectory is in use.
        '
        '   T:System.NotSupportedException:
        '     The directory name contains a colon (:).
        '
        '   T:System.IO.PathTooLongException:
        '     The path exceeds the system-defined maximum length.
        '
        '   T:System.Security.SecurityException:
        '     The user does not have required permissions.
        '
        '   T:System.OperationCanceledException:
        '     The user cancels the operation or the directory cannot be deleted.
        Private Function DeleteDirectory(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream

        End Function



        '
        ' Summary:
        '     Deletes a file.
        '
        ' Parameters:
        '   file:
        '     Name and path of the file to be deleted.
        '
        ' Exceptions:
        '   T:System.ArgumentException:
        '     The path is not valid for one of the following reasons: it is a zero-length string;
        '     it contains only white space; it contains invalid characters; it has a trailing
        '     slash where a file must be specified; or it is a device path (starts with \\.\).
        '
        '   T:System.ArgumentNullException:
        '     file is Nothing or an empty string.
        '
        '   T:System.IO.PathTooLongException:
        '     The path exceeds the system-defined maximum length.
        '
        '   T:System.NotSupportedException:
        '     A file or directory name in the path contains a colon (:) or is in an invalid
        '     format.
        '
        '   T:System.IO.IOException:
        '     The file is in use.
        '
        '   T:System.Security.SecurityException:
        '     The user lacks necessary permissions to view the path.
        '
        '   T:System.IO.FileNotFoundException:
        '     The file does not exist.
        '
        '   T:System.UnauthorizedAccessException:
        '     The user does not have permission to delete the file or the file is read-only.
        Private Function DeleteFile(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream

        End Function

        '
        ' Summary:
        '     Moves a directory from one location to another.
        '
        ' Parameters:
        '   sourceDirectoryName:
        '     Path of the directory to be moved.
        '
        '   destinationDirectoryName:
        '     Path of the directory to which the source directory is being moved.
        '
        ' Exceptions:
        '   T:System.ArgumentException:
        '     The path is not valid for one of the following reasons: it is a zero-length string;
        '     it contains only white space; it contains invalid characters; or it is a device
        '     path (starts with \\.\).
        '
        '   T:System.ArgumentNullException:
        '     sourceDirectoryName or destinationDirectoryName is Nothing or an empty string.
        '
        '   T:System.ArgumentNullException:
        '     sourceDirectoryName or destinationDirectoryName is Nothing or an empty string.
        '
        '   T:System.IO.DirectoryNotFoundException:
        '     The directory does not exist.
        '
        '   T:System.IO.IOException:
        '     The source is a root directory or The source path and the target path are the
        '     same.
        '
        '   T:System.IO.PathTooLongException:
        '     The path exceeds the system-defined maximum length.
        '
        '   T:System.InvalidOperationException:
        '     The operation is cyclic.
        '
        '   T:System.NotSupportedException:
        '     A file or directory name in the path contains a colon (:) or is in an invalid
        '     format.
        '
        '   T:System.Security.SecurityException:
        '     The user lacks necessary permissions to view the path.
        '
        '   T:System.UnauthorizedAccessException:
        '     The user does not have required permission.
        Private Function MoveDirectory(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream

        End Function

        '
        ' Summary:
        '     Moves a file to a new location.
        '
        ' Parameters:
        '   sourceFileName:
        '     Path of the file to be moved.
        '
        '   destinationFileName:
        '     Path of the directory into which the file should be moved.
        '
        ' Exceptions:
        '   T:System.ArgumentException:
        '     The path is not valid for one of the following reasons: it is a zero-length string;
        '     it contains only white space; it contains invalid characters; or it is a device
        '     path (starts with \\.\); it ends with a trailing slash.
        '
        '   T:System.ArgumentNullException:
        '     destinationFileName is Nothing or an empty string.
        '
        '   T:System.IO.FileNotFoundException:
        '     The source file is not valid or does not exist.
        '
        '   T:System.IO.IOException:
        '     The file is in use by another process, or an I/O error occurs.
        '
        '   T:System.IO.PathTooLongException:
        '     The path exceeds the system-defined maximum length.
        '
        '   T:System.NotSupportedException:
        '     A file or directory name in the path contains a colon (:) or is in an invalid
        '     format.
        '
        '   T:System.Security.SecurityException:
        '     The user lacks necessary permissions to view the path.
        Private Function MoveFile(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream


        End Function
        '
        ' Summary:
        '     Renames a directory.
        '
        ' Parameters:
        '   directory:
        '     Path and name of directory to be renamed.
        '
        '   newName:
        '     New name for directory.
        '
        ' Exceptions:
        '   T:System.ArgumentException:
        '     The path is not valid for one of the following reasons: it is a zero-length string;
        '     it contains only white space; it contains invalid characters; or it is a device
        '     path (starts with \\.\).
        '
        '   T:System.ArgumentException:
        '     newName contains path information.
        '
        '   T:System.ArgumentNullException:
        '     directory is Nothing.-or-newName is Nothing or an empty string.
        '
        '   T:System.IO.DirectoryNotFoundException:
        '     The directory does not exist.
        '
        '   T:System.IO.IOException:
        '     There is an existing file or directory with the name specified in newName.
        '
        '   T:System.IO.PathTooLongException:
        '     The path exceeds 248 characters.
        '
        '   T:System.NotSupportedException:
        '     A file or directory name in the path contains a colon (:) or is in an invalid
        '     format.
        '
        '   T:System.Security.SecurityException:
        '     The user lacks necessary permissions to view the path.
        '
        '   T:System.UnauthorizedAccessException:
        '     The user does not have required permission.
        Private Function RenameDirectory(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream

        End Function
        '
        ' Summary:
        '     Renames a file.
        '
        ' Parameters:
        '   file:
        '     File to be renamed.
        '
        '   newName:
        '     New name of file.
        '
        ' Exceptions:
        '   T:System.ArgumentException:
        '     The path is not valid for one of the following reasons: it is a zero-length string;
        '     it contains only white space; it contains invalid characters; or it is a device
        '     path (starts with \\.\).
        '
        '   T:System.ArgumentException:
        '     newName contains path information or ends with a backslash (\).
        '
        '   T:System.ArgumentNullException:
        '     file is Nothing.-or-newName is Nothing or an empty string.
        '
        '   T:System.IO.FileNotFoundException:
        '     The directory does not exist.
        '
        '   T:System.IO.IOException:
        '     There is an existing file or directory with the name specified in newName.
        '
        '   T:System.IO.PathTooLongException:
        '     The path exceeds the system-defined maximum length.
        '
        '   T:System.NotSupportedException:
        '     A file or directory name in the path contains a colon (:) or is in an invalid
        '     format.
        '
        '   T:System.Security.SecurityException:
        '     The user lacks necessary permissions to view the path.
        '
        '   T:System.UnauthorizedAccessException:
        '     The user does not have required permission.
        Private Function RenameFile(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream

        End Function
        '
        ' Summary:
        '     Writes data to a binary file.
        '
        ' Parameters:
        '   file:
        '     Path and name of the file to be written to.
        '
        '   data:
        '     Data to be written to the file.
        '
        '   append:
        '     True to append to the file contents; False to overwrite the file contents. Default
        '     is False.
        '
        ' Exceptions:
        '   T:System.ArgumentException:
        '     The path is not valid for one of the following reasons: it is a zero-length string;
        '     it contains only white space; it contains invalid characters; or it is a device
        '     path (starts with \\.\); it ends with a trailing slash.
        '
        '   T:System.ArgumentNullException:
        '     file is Nothing.
        '
        '   T:System.IO.FileNotFoundException:
        '     The file does not exist.
        '
        '   T:System.IO.IOException:
        '     The file is in use by another process, or an I/O error occurs.
        '
        '   T:System.IO.PathTooLongException:
        '     The path exceeds the system-defined maximum length.
        '
        '   T:System.NotSupportedException:
        '     A file or directory name in the path contains a colon (:) or is in an invalid
        '     format.
        '
        '   T:System.OutOfMemoryException:
        '     There is not enough memory to write the string to buffer.
        '
        '   T:System.Security.SecurityException:
        '     The user lacks necessary permissions to view the path.
        Private Function WriteAllBytes(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream

        End Function
        '
        ' Summary:
        '     Writes text to a file.
        '
        ' Parameters:
        '   file:
        '     File to be written to.
        '
        '   text:
        '     Text to be written to file.
        '
        '   append:
        '     True to append to the contents of the file; False to overwrite the contents of
        '     the file.
        '
        ' Exceptions:
        '   T:System.ArgumentException:
        '     The path is not valid for one of the following reasons: it is a zero-length string;
        '     it contains only white space; it contains invalid characters; or it is a device
        '     path (starts with \\.\); it ends with a trailing slash.
        '
        '   T:System.ArgumentNullException:
        '     file is Nothing.
        '
        '   T:System.IO.FileNotFoundException:
        '     The file does not exist.
        '
        '   T:System.IO.IOException:
        '     The file is in use by another process, or an I/O error occurs.
        '
        '   T:System.IO.PathTooLongException:
        '     The path exceeds the system-defined maximum length.
        '
        '   T:System.NotSupportedException:
        '     A file or directory name in the path contains a colon (:) or is in an invalid
        '     format.
        '
        '   T:System.OutOfMemoryException:
        '     There is not enough memory to write the string to buffer.
        '
        '   T:System.Security.SecurityException:
        '     The user lacks necessary permissions to view the path.
        Private Function WriteAllText(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream

        End Function
        '
        ' Summary:
        '     Combines two paths and returns a properly formatted path.
        '
        ' Parameters:
        '   baseDirectory:
        '     String. First path to be combined.
        '
        '   relativePath:
        '     String. Second path to be combined.
        '
        ' Returns:
        '     The combination of the specified paths.
        '
        ' Exceptions:
        '   T:System.ArgumentException:
        '     baseDirectory or relativePath are malformed paths.
        Private Function CombinePath(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream

        End Function
        '
        ' Summary:
        '     Returns True if the specified directory exists.
        '
        ' Parameters:
        '   directory:
        '     Path of the directory.
        '
        ' Returns:
        '     True if the directory exists; otherwise False.
        Private Function DirectoryExists(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream

        End Function
        '
        ' Summary:
        '     Returns True if the specified file exists.
        '
        ' Parameters:
        '   file:
        '     Name and path of the file.
        '
        ' Returns:
        '     Returns True if the file exists; otherwise this method returns False.
        '
        ' Exceptions:
        '   T:System.ArgumentException:
        '     The name of the file ends with a backslash (\).
        Private Function FileExists(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream

        End Function
        '
        ' Summary:
        '     Returns a read-only collection of strings representing the names of files containing
        '     the specified text.
        '
        ' Parameters:
        '   directory:
        '     The directory to be searched.
        '
        '   containsText:
        '     The search text.
        '
        '   ignoreCase:
        '     True if the search should be case-sensitive; otherwise False. Default is True.
        '
        '   searchType:
        '     Whether to include subfolders. Default is SearchOption.SearchTopLevelOnly.
        '
        ' Returns:
        '     Read-only collection of the names of files containing the specified text..
        '
        ' Exceptions:
        '   T:System.ArgumentException:
        '     The path is not valid for one of the following reasons: it is a zero-length string;
        '     it contains only white space; it contains invalid characters; or it is a device
        '     path (starts with \\.\).
        '
        '   T:System.ArgumentNullException:
        '     directory is Nothing or an empty string.
        '
        '   T:System.IO.DirectoryNotFoundException:
        '     The specified directory does not exist.
        '
        '   T:System.IO.IOException:
        '     The specified directory points to an existing file.
        '
        '   T:System.IO.PathTooLongException:
        '     The path exceeds the system-defined maximum length.
        '
        '   T:System.NotSupportedException:
        '     The specified directory path contains a colon (:) or is in an invalid format.
        '
        '   T:System.Security.SecurityException:
        '     The user lacks necessary permissions to view the path.
        '
        '   T:System.UnauthorizedAccessException:
        '     The user lacks necessary permissions.
        Private Function FindInFiles(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream

        End Function

        '
        ' Summary:
        '     Returns a collection of strings representing the path names of subdirectories
        '     within a directory.
        '
        ' Parameters:
        '   directory:
        '     Name and path of directory.
        '
        ' Returns:
        '     Read-only collection of the path names of subdirectories within the specified
        '     directory..
        '
        ' Exceptions:
        '   T:System.ArgumentException:
        '     The path is not valid for one of the following reasons: it is a zero-length string;
        '     it contains only white space; it contains invalid characters; or it is a device
        '     path (starts with \\.\).
        '
        '   T:System.ArgumentNullException:
        '     directory is Nothing or an empty string.
        '
        '   T:System.IO.DirectoryNotFoundException:
        '     The specified directory does not exist.
        '
        '   T:System.IO.IOException:
        '     The specified directory points to an existing file.
        '
        '   T:System.IO.PathTooLongException:
        '     The path exceeds the system-defined maximum length.
        '
        '   T:System.NotSupportedException:
        '     A file or directory name in the path contains a colon (:) or is in an invalid
        '     format.
        '
        '   T:System.Security.SecurityException:
        '     The user lacks necessary permissions to view the path.
        '
        '   T:System.UnauthorizedAccessException:
        '     The user lacks necessary permissions.
        Private Function GetDirectories(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream

        End Function

        '
        ' Summary:
        '     Returns a System.IO.DirectoryInfo object for the specified path.
        '
        ' Parameters:
        '   directory:
        '     String. Path of directory.
        '
        ' Returns:
        '     System.IO.DirectoryInfo object for the specified path.
        '
        ' Exceptions:
        '   T:System.ArgumentException:
        '     The path is not valid for one of the following reasons: it is a zero-length string;
        '     it contains only white space; it contains invalid characters; or it is a device
        '     path (starts with \\.\).
        '
        '   T:System.ArgumentNullException:
        '     directory is Nothing or an empty string.
        '
        '   T:System.IO.PathTooLongException:
        '     The path exceeds the system-defined maximum length.
        '
        '   T:System.NotSupportedException:
        '     The directory path contains a colon (:) or is in an invalid format.
        '
        '   T:System.Security.SecurityException:
        '     The user lacks necessary permissions to view the path
        Private Function GetDirectoryInfo(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream

        End Function
        '
        ' Summary:
        '     Returns a System.IO.DriveInfo object for the specified drive.
        '
        ' Parameters:
        '   drive:
        '     Drive to be examined.
        '
        ' Returns:
        '     System.IO.DriveInfo object for the specified drive.
        '
        ' Exceptions:
        '   T:System.ArgumentException:
        '     The path is not valid for one of the following reasons: it is a zero-length string;
        '     it contains only white space; it contains invalid characters; or it is a device
        '     path (starts with \\.\).
        '
        '   T:System.ArgumentNullException:
        '     drive is Nothing or an empty string.
        '
        '   T:System.IO.PathTooLongException:
        '     The path exceeds the system-defined maximum length.
        '
        '   T:System.Security.SecurityException:
        '     The user lacks necessary permissions to view the path
        Private Function GetDriveInfo(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream

        End Function
        '
        ' Summary:
        '     Returns a System.IO.FileInfo object for the specified file.
        '
        ' Parameters:
        '   file:
        '     Name and path of the file.
        '
        ' Returns:
        '     System.IO.FileInfo object for the specified file
        '
        ' Exceptions:
        '   T:System.ArgumentException:
        '     The path name is malformed. For example, it contains invalid characters or is
        '     only white space. The file name has a trailing slash mark.
        '
        '   T:System.ArgumentNullException:
        '     file is Nothing or an empty string.
        '
        '   T:System.NotSupportedException:
        '     The path contains a colon in the middle of the string.
        '
        '   T:System.IO.PathTooLongException:
        '     The path is too long.
        '
        '   T:System.Security.SecurityException:
        '     The user lacks necessary permissions.
        '
        '   T:System.UnauthorizedAccessException:
        '     The user lacks ACL (access control list) access to the file.
        Private Function GetFileInfo(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream

        End Function
        '
        ' Summary:
        '     Returns a read-only collection of strings representing the names of files within
        '     a directory.
        '
        ' Parameters:
        '   directory:
        '     Directory to be searched.
        '
        ' Returns:
        '     Read-only collection of file names from the specified directory.
        '
        ' Exceptions:
        '   T:System.ArgumentException:
        '     The path is not valid for one of the following reasons: it is a zero-length string;
        '     it contains only white space; it contains invalid characters; or it is a device
        '     path (starts with \\.\).
        '
        '   T:System.ArgumentNullException:
        '     directory is Nothing.
        '
        '   T:System.IO.DirectoryNotFoundException:
        '     The directory to search does not exist.
        '
        '   T:System.IO.IOException:
        '     directory points to an existing file.
        '
        '   T:System.IO.PathTooLongException:
        '     The path exceeds the system-defined maximum length.
        '
        '   T:System.NotSupportedException:
        '     A file or directory name in the path contains a colon (:) or is in an invalid
        '     format.
        '
        '   T:System.Security.SecurityException:
        '     The user lacks necessary permissions to view the path.
        '
        '   T:System.UnauthorizedAccessException:
        '     The user lacks necessary permissions.
        Private Function GetFiles(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream

        End Function



        '
        ' Summary:
        '     Parses the file name out of the path provided.
        '
        ' Parameters:
        '   path:
        '     Required. Path to be parsed. String.
        '
        ' Returns:
        '     The file name from the specified path.
        Private Function GetName(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream

        End Function
        '
        ' Summary:
        '     Returns the parent path of the provided path.
        '
        ' Parameters:
        '   path:
        '     Path to be examined.
        '
        ' Returns:
        '     The parent path of the provided path.
        '
        ' Exceptions:
        '   T:System.ArgumentException:
        '     The path is not valid for one of the following reasons: it is a zero-length string;
        '     it contains only white space; it contains invalid characters; or it is a device
        '     path (starts with \\.\).
        '
        '   T:System.ArgumentException:
        '     Path does not have a parent path because it is a root path.
        '
        '   T:System.ArgumentNullException:
        '     path is Nothing.
        '
        '   T:System.IO.PathTooLongException:
        '     The path exceeds the system-defined maximum length.
        '
        '   T:System.NotSupportedException:
        '     A file or directory name in the path contains a colon (:) or is in an invalid
        '     format.
        Private Function GetParentPath(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream

        End Function
        '
        ' Summary:
        '     Creates a uniquely named zero-byte temporary file on disk and returns the full
        '     path of that file.
        '
        ' Returns:
        '     String containing the full path of the temporary file.
        Private Function GetTempFileName(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream

        End Function

        <Protocol(FileSystemAPI.OpenHandle)>
        Private Function OpenFileHandle(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream
            Dim params As FileOpen = Serialization.LoadObject(Of FileOpen)(args.GetUTF8String)
            Dim stream As FileStream = params.OpenHandle
            Dim handle As New FileHandle With {
                .FileName = params.FileName,
                .HashCode = stream.GetHashCode
            }  ' 可能会出现重复的文件名，所以使用这个句柄对象来进行唯一标示
            Call OpenedHandles.Add(handle.ToString, stream)
            Return New RequestStream(handle.GetJson)
        End Function
        '
        ' Summary:
        '     Opens a System.IO.StreamReader object to read from a file.
        '
        ' Parameters:
        '   file:
        '     File to be read.
        '
        ' Returns:
        '     System.IO.StreamReader object to read from the file
        '
        ' Exceptions:
        '   T:System.ArgumentException:
        '     The file name ends with a backslash (\).
        '
        '   T:System.IO.FileNotFoundException:
        '     The specified file cannot be found.
        '
        '   T:System.Security.SecurityException:
        '     The user lacks necessary permissions to read from the file.
        Private Function OpenTextFileReader(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream

        End Function
        '
        ' Summary:
        '     Opens a System.IO.StreamWriter object to write to the specified file.
        '
        ' Parameters:
        '   file:
        '     File to be written to.
        '
        '   append:
        '     True to append to the contents of the file; False to overwrite the contents of
        '     the file. Default is False.
        '
        ' Returns:
        '     System.IO.StreamWriter object to write to the specified file.
        '
        ' Exceptions:
        '   T:System.ArgumentException:
        '     The file name ends with a trailing slash.
        Private Function OpenTextFileWriter(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream

        End Function
        '
        ' Summary:
        '     Returns the contents of a file as a byte array.
        '
        ' Parameters:
        '   file:
        '     File to be read.
        '
        ' Returns:
        '     Byte array containing the contents of the file.
        '
        ' Exceptions:
        '   T:System.ArgumentException:
        '     The path is not valid for one of the following reasons: it is a zero-length string;
        '     it contains only white space; it contains invalid characters; or it is a device
        '     path (starts with \\.\); it ends with a trailing slash.
        '
        '   T:System.ArgumentNullException:
        '     file is Nothing.
        '
        '   T:System.IO.FileNotFoundException:
        '     The file does not exist.
        '
        '   T:System.IO.IOException:
        '     The file is in use by another process, or an I/O error occurs.
        '
        '   T:System.IO.PathTooLongException:
        '     The path exceeds the system-defined maximum length.
        '
        '   T:System.NotSupportedException:
        '     A file or directory name in the path contains a colon (:) or is in an invalid
        '     format.
        '
        '   T:System.OutOfMemoryException:
        '     There is not enough memory to write the string to buffer.
        '
        '   T:System.Security.SecurityException:
        '     The user lacks necessary permissions to view the path.
        <Protocol(FileSystemAPI.ReadAllBytes)>
        Private Function ReadAllBytes(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream
            Dim file As String = args.GetUTF8String
            Dim byts As Byte() = FileIO.FileSystem.ReadAllBytes(file)
            Return New RequestStream(HTTP_RFC.RFC_OK, HTTP_RFC.RFC_OK, byts)
        End Function
    End Class
End Namespace