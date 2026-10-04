@echo off
setlocal EnableExtensions

set "RIMWORLD_DIR=%~1"
if not defined RIMWORLD_DIR set "RIMWORLD_DIR=D:\SteamLibrary\steamapps\common\RimWorld"

set "ROOT=%~dp0"
set "RIMWORLD_EXE=%RIMWORLD_DIR%\RimWorldWin64.exe"
set "RESULT_ROOT=%ROOT%TestResults\VegetationRuntime"
set "TEST_SAVEDATA=%RESULT_ROOT%\SaveData"
set "REPORT_DIR=%RESULT_ROOT%\Reports"
set "QUICKTEST_DLL=%ROOT%DevQuickstarts\Assemblies\AncientMedievalJapanEnvironment.Quicktests.dll"

call "%ROOT%run-tests.bat" "%RIMWORLD_DIR%"
if errorlevel 1 exit /b 1

if not exist "%RIMWORLD_EXE%" (
    echo [ERROR] RimWorld executable was not found:
    echo         %RIMWORLD_EXE%
    exit /b 2
)

if not exist "%QUICKTEST_DLL%" (
    echo [ERROR] Fixed-biome Quicktest DLL was not built:
    echo         %QUICKTEST_DLL%
    echo.
    echo Make sure RimWorks Quickstarts is installed.
    exit /b 2
)

echo.
echo Resetting isolated vegetation runtime-test output...
if exist "%RESULT_ROOT%" rmdir /S /Q "%RESULT_ROOT%"
if errorlevel 1 (
    echo [ERROR] Failed to clear:
    echo         %RESULT_ROOT%
    exit /b 2
)

echo.
echo Preparing isolated RimWorld runtime-test profile...
powershell -NoProfile -ExecutionPolicy Bypass -File "%ROOT%Scripts\Prepare-EnvironmentRuntimeTestSaveData.ps1" ^
    -OutputRoot "%TEST_SAVEDATA%"
if errorlevel 1 exit /b 2

echo.
echo Running four fixed-biome vegetation Quickstarts automatically...
echo The normal RimWorld mod list and Prefs.xml are not modified.
echo RimWorld will open and exit once per biome.
echo.

powershell -NoProfile -ExecutionPolicy Bypass -File "%ROOT%Scripts\Run-EnvironmentVegetationQuickstarts.ps1" ^
    -ExePath "%RIMWORLD_EXE%" ^
    -SaveDataFolder "%TEST_SAVEDATA%" ^
    -ResultDir "%REPORT_DIR%" ^
    -TimeoutSeconds 180

set "RESULT=%ERRORLEVEL%"

echo.
if "%RESULT%"=="0" (
    echo [OK] Environment vegetation runtime gate passed.
) else if "%RESULT%"=="124" (
    echo [ERROR] A RimWorld Quickstart exceeded the outer timeout.
) else (
    echo [FAIL] Environment vegetation runtime gate failed.
)

echo Reports:
echo   %REPORT_DIR%
exit /b %RESULT%
