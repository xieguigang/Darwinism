Imports "Sqlite3"

FROM x as table(?"--table")
IN ?"--db"
WHERE x.pid = 11
ORDER BY gid
TAKE 25