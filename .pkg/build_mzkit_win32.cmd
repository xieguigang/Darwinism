@echo off

SET drive=%~d0
SET mzkit_renv=%drive%/mzkit/dist/bin
SET Rscript="%mzkit_renv%/Rscript.exe"
SET REnv="%mzkit_renv%/R#.exe"

SET mzkit_pkg=%drive%/mzkit/src/mzkit/setup/ggplot.zip

%Rscript% --build /src ../ /save %mzkit_pkg%
%REnv% --install.packages %mzkit_pkg%

pause