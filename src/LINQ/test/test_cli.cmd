@echo off

SET LINQ="../../build/LINQ.exe"
SET sqlite_db="./xcc.db"
SET query="./sqlite_cli.db"

%LINQ% %query% --db %sqlite_db% --table "GenePathways"
