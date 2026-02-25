@echo off
setlocal enabledelayedexpansion

set SCRIPT_DIR=%~dp0
set KEY_FILE=%SCRIPT_DIR%nuget-api-key.txt
set NUPKG_DIR=%SCRIPT_DIR%src\Ropro.AI\bin\Release

rem Pack first
call "%SCRIPT_DIR%pack.cmd"
if errorlevel 1 exit /b %errorlevel%

rem Read API key
if not exist "%KEY_FILE%" (
  echo Error: API key file not found: %KEY_FILE%
  echo Create the file and paste your NuGet API key into it.
  exit /b 1
)

set /p API_KEY=<"%KEY_FILE%"

if "%API_KEY%"=="" (
  echo Error: API key file is empty.
  exit /b 1
)
if "%API_KEY%"=="REPLACE_WITH_YOUR_NUGET_API_KEY" (
  echo Error: Please put your actual NuGet API key in %KEY_FILE%
  exit /b 1
)

rem Log which key is being used (masked)
set KEY_FULL=%API_KEY%
call :strlen KEY_LEN KEY_FULL
echo Using API key: %API_KEY:~0,4%...%API_KEY:~-4% (length: !KEY_LEN!)
echo Key file: %KEY_FILE%

rem Find the latest .nupkg
set NUPKG=
for %%f in ("%NUPKG_DIR%\*.nupkg") do set NUPKG=%%f

if "%NUPKG%"=="" (
  echo Error: No .nupkg file found in %NUPKG_DIR%
  exit /b 1
)

rem Publish to local feed
set LOCAL_FEED=%USERPROFILE%\Packages
if not exist "%LOCAL_FEED%" mkdir "%LOCAL_FEED%"
echo.
echo Publishing %NUPKG% to local feed (%LOCAL_FEED%)...
dotnet nuget push "%NUPKG%" --source "%LOCAL_FEED%" --skip-duplicate
if errorlevel 1 exit /b %errorlevel%

rem Publish to nuget.org
echo.
echo Publishing %NUPKG% to nuget.org...
dotnet nuget push "%NUPKG%" --api-key %API_KEY% --source https://api.nuget.org/v3/index.json --skip-duplicate
if errorlevel 1 exit /b %errorlevel%

echo.
echo Published successfully (local + nuget.org).
exit /b 0

:strlen <resultVar> <stringVar>
setlocal enabledelayedexpansion
set "s=!%~2!"
set len=0
for %%i in (4096 2048 1024 512 256 128 64 32 16 8 4 2 1) do (
  if not "!s:~%%i!"=="" (
    set /a len+=%%i
    set "s=!s:~%%i!"
  )
)
endlocal & set "%~1=%len%"
exit /b 0
