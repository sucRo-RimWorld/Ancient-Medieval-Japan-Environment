@echo off
setlocal EnableExtensions

set "RIMWORLD_DIR=%~1"
if not defined RIMWORLD_DIR set "RIMWORLD_DIR=D:\SteamLibrary\steamapps\common\RimWorld"

set "SKIP_STATIC="
if /I "%~2"=="--skip-static" set "SKIP_STATIC=1"
set "ROOT=%~dp0"

if not defined SKIP_STATIC (
    call "%ROOT%run-static-tests.bat" "%RIMWORLD_DIR%"
    if errorlevel 1 exit /b 1
)

where py >nul 2>&1
if errorlevel 1 (
    echo [ERROR] Python launcher py.exe was not found.
    exit /b 2
)

if not exist "%RIMWORLD_DIR%\RimWorldWin64.exe" (
    echo [ERROR] RimWorld executable was not found:
    echo         %RIMWORLD_DIR%\RimWorldWin64.exe
    exit /b 2
)

echo.
echo Running Environment logic/calculation tests through RimTest Redux...
py -3 "%ROOT%Scripts\Run-EnvironmentRimTest.py" --game "%RIMWORLD_DIR%" --root "%ROOT%"
if errorlevel 1 exit /b 1

echo.
echo Running Environment loaded-Def integration matrix through RimWorks Pickle...
py -3 "%ROOT%Scripts\Run-DevelopmentPickle.py" --game "%RIMWORLD_DIR%" --root "%ROOT%"
if errorlevel 1 exit /b 1

echo.
echo [OK] AMJ Environment RimTest + Pickle development automation passed
exit /b 0
