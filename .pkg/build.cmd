@echo off

SET drive=%~d0
SET R_HOME=%drive%/GCModeller\src\R-sharp\App\net6.0
SET pkg=./ggplot.zip

%R_HOME%/Rscript.exe --build /src ../ /save %pkg% --skip-src-build
%R_HOME%/R#.exe --install.packages %pkg%

pause