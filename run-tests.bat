@echo off
setlocal EnableExtensions

set "RIMWORLD_DIR=%~1"
if not defined RIMWORLD_DIR set "RIMWORLD_DIR=D:\SteamLibrary\steamapps\common\RimWorld"

call "%~dp0run-static-tests.bat" "%RIMWORLD_DIR%"
if errorlevel 1 exit /b 1

echo.
echo Running RimTest Redux + Pickle development framework suite...
call "%~dp0run-framework-tests.bat" "%RIMWORLD_DIR%" --skip-static
if errorlevel 1 exit /b 1

echo.
echo Running rendered runtime regression suite on an isolated Windows desktop...
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0Scripts\Run-EnvironmentIsolatedDesktop.ps1" ^
    -RimWorldRoot "%RIMWORLD_DIR%"
if errorlevel 1 exit /b 1

echo.
echo [OK] AMJ Environment full static + framework + isolated runtime validation passed
exit /b 0
