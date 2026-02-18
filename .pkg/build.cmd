@echo off

SET drive=%~d0
SET R_HOME=%drive%/GCModeller\src\R-sharp\App\net10.0
SET pkg=./Darwinism.zip

%R_HOME%/Rscript.exe --build /src ../ /save %pkg% --skip-src-build
%R_HOME%/R#.exe --install.packages %pkg%

pause