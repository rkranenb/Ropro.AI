@echo off
setlocal

set SCRIPT_DIR=%~dp0
set PROJECT=%SCRIPT_DIR%src\Ropro.AI\Ropro.AI.csproj

echo Packing Ropro.AI...
dotnet pack "%PROJECT%" -c Release
if errorlevel 1 exit /b %errorlevel%

echo.
echo Done. Package output:
dir /b "%SCRIPT_DIR%src\Ropro.AI\bin\Release\*.nupkg"
