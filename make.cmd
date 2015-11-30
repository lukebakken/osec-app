@echo off
setlocal

.nuget\NuGet.exe restore .nuget\packages.config -PackagesDirectory packages -ConfigFile .nuget\NuGet.Config -NonInteractive -Verbosity detailed
.nuget\NuGet.exe restore src\Test\packages.config -PackagesDirectory packages -ConfigFile .nuget\NuGet.Config -NonInteractive -Verbosity detailed

reg.exe query "HKLM\SOFTWARE\Microsoft\MSBuild\ToolsVersions\4.0" /v MSBuildToolsPath > nul 2>&1
if ERRORLEVEL 1 goto MissingMSBuildRegistry

for /f "skip=2 tokens=2,*" %%A in ('reg.exe query "HKLM\SOFTWARE\Microsoft\MSBuild\ToolsVersions\4.0" /v MSBuildToolsPath') do SET MSBUILDDIR=%%B

IF NOT EXIST %MSBUILDDIR%nul goto MissingMSBuildToolsPath
IF NOT EXIST %MSBUILDDIR%msbuild.exe goto MissingMSBuildExe

"%MSBUILDDIR%msbuild.exe" OSecApp.sln

goto :exit

:MissingMSBuildRegistry
echo Cannot obtain path to MSBuild tools from registry
goto :exit

:MissingMSBuildToolsPath
echo The MSBuild tools path from the registry '%MSBUILDDIR%' does not exist
goto :exit

:MissingMSBuildExe
echo The MSBuild executable could not be found at '%MSBUILDDIR%'
goto :exit

:exit
exit /b %ERRORLEVEL%
