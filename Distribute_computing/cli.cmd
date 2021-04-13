@echo off

SET build="../build/Taskhost.d.exe"

%build% /CLI.dev ---echo > ./HPC_cluster/Taskhost.vb

pause