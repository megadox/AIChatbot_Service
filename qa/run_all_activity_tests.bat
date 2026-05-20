@echo off
setlocal

pushd "%~dp0\.."
dotnet run --no-restore --project src\Tools.RetrievalSmokeTest\Tools.RetrievalSmokeTest.csproj -- --all-activities
set EXITCODE=%ERRORLEVEL%
popd

exit /b %EXITCODE%
