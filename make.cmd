@echo off
setlocal
.nuget\NuGet.exe restore .nuget\packages.config -PackagesDirectory packages -ConfigFile .nuget\NuGet.Config -NonInteractive -Verbosity detailed
.nuget\NuGet.exe restore src\Test\packages.config -PackagesDirectory packages -ConfigFile .nuget\NuGet.Config -NonInteractive -Verbosity detailed
exit /b %ERRORLEVEL%
