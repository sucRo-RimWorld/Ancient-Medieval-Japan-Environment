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

where python >nul 2>&1
if errorlevel 1 (
    echo [ERROR] Python executable python.exe was not found.
    exit /b 2
)

set "RIMWORLD_EXE=%RIMWORLD_DIR%\RimWorldWin64.exe"
if exist "%RIMWORLD_DIR%\UnityStubs.dll" set "RIMWORLD_EXE=%RIMWORLD_DIR%\RimWorld.exe"
if not exist "%RIMWORLD_EXE%" (
    echo [ERROR] RimWorld executable was not found:
    echo         %RIMWORLD_EXE%
    exit /b 2
)

echo.
echo Checking framework-test Python syntax and wiring...
python -m py_compile "%ROOT%Scripts\EnvironmentFrameworkTestCommon.py" "%ROOT%Scripts\Run-EnvironmentRimTest.py" "%ROOT%Scripts\Run-DevelopmentPickle.py"
if errorlevel 1 exit /b 1
python "%ROOT%Tests\Runtime\test_framework_automation.py"
if errorlevel 1 exit /b 1

echo.
echo Running Environment logic/calculation tests through RimTest Redux...
python "%ROOT%Scripts\Run-EnvironmentRimTest.py" --game "%RIMWORLD_DIR%"
if errorlevel 1 exit /b 1

echo.
echo Running Environment loaded-Def integration matrix through RimWorks Pickle...
set "PICKLE_WORKERS=%AMJE_PICKLE_WORKERS%"
if not defined PICKLE_WORKERS set "PICKLE_WORKERS=1"
if not "%PICKLE_WORKERS%"=="1" if not "%PICKLE_WORKERS%"=="2" if not "%PICKLE_WORKERS%"=="4" (
    echo [ERROR] AMJE_PICKLE_WORKERS must be 1, 2 or 4.
    exit /b 2
)
python "%ROOT%Scripts\Run-DevelopmentPickle.py" --game "%RIMWORLD_DIR%" --workers %PICKLE_WORKERS%
if errorlevel 1 exit /b 1

echo.
echo [OK] AMJ Environment RimTest + Pickle development automation passed
exit /b 0
