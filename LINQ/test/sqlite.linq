imports "Sqlite3"

FROM x as table("GenePathways")
IN "E:\GCModeller\src\runtime\Darwinism\LINQ\test\xcc.db"
WHERE x.pid = 11
ORDER BY x.gid