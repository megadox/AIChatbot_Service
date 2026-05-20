@echo off
setlocal

pushd "%~dp0\.."
dotnet run --no-restore --project src\Tools.RetrievalSmokeTest\Tools.RetrievalSmokeTest.csproj -- --cases qa\user_test_cases.json
set EXITCODE=%ERRORLEVEL%
popd

exit /b %EXITCODE%
