''' <summary>The open mode flags</summary>
Public Enum OpenMode As Integer
    ''' <summary>Set read-only access for nc_open()</summary>
    NC_NOWRITE = &H0
    ''' <summary>Set read-write access for nc_open()</summary>
    NC_WRITE = &H1
    ''' <summary>Use diskless file. Mode flag for nc_open() or nc_create()</summary>
    NC_DISKLESS = &H8
    ''' <summary>Share updates, limit caching. Use this in mode flags for both nc_create() and nc_open()</summary>
    NC_SHARE = &H800
    ''' <summary>Read from memory. Mode flag for nc_open() or nc_create()</summary>
    NC_INMEMORY = &H8000
End Enum