@echo off
setlocal EnableExtensions

set "RIMWORLD_DIR=%~1"
if not defined RIMWORLD_DIR set "RIMWORLD_DIR=D:\SteamLibrary\steamapps\common\RimWorld"

set "TEXTURE_TARGET=%~2"
if not defined TEXTURE_TARGET set "TEXTURE_TARGET=Cool"

set "TEXTURE_QUICKSTART="
if /I "%TEXTURE_TARGET%"=="Warm" set "TEXTURE_QUICKSTART=AMJWarmTemperateTerrainQuickstart"
if /I "%TEXTURE_TARGET%"=="Cool" set "TEXTURE_QUICKSTART=AMJCoolTemperateTerrainQuickstart"
if /I "%TEXTURE_TARGET%"=="Subalpine" set "TEXTURE_QUICKSTART=AMJSubalpineTerrainQuickstart"
if /I "%TEXTURE_TARGET%"=="Alpine" set "TEXTURE_QUICKSTART=AMJAlpineTerrainQuickstart"

if not defined TEXTURE_QUICKSTART (
    echo [ERROR] Unknown texture-debug biome alias: %TEXTURE_TARGET%
    echo         Expected one of: Warm, Cool, Subalpine, Alpine
    exit /b 2
)

set "ROOT=%~dp0"
set "RIMWORLD_EXE=%RIMWORLD_DIR%\RimWorldWin64.exe"
set "RESULT_ROOT=%ROOT%TestResults\TextureDebug"
set "TEST_SAVEDATA=%RESULT_ROOT%\SaveData"
set "LOG=%RESULT_ROOT%\%TEXTURE_QUICKSTART%.log"
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
    exit /b 2
)

if exist "%RESULT_ROOT%" rmdir /S /Q "%RESULT_ROOT%"
if errorlevel 1 (
    echo [ERROR] Failed to clear:
    echo         %RESULT_ROOT%
    exit /b 2
)

powershell -NoProfile -ExecutionPolicy Bypass -File "%ROOT%Scripts\Prepare-EnvironmentRuntimeTestSaveData.ps1" ^
    -OutputRoot "%TEST_SAVEDATA%"
if errorlevel 1 exit /b 2

echo.
echo ============================================================
echo Running focused %TEXTURE_TARGET% texture-debug Quickstart
echo ============================================================
echo Quickstart: %TEXTURE_QUICKSTART%
echo This mode intentionally omits Quickstarts verification/report flags.
echo RimWorld will stay open until you close it manually.
echo The isolated profile has Dev Mode enabled.
echo Log:
echo   %LOG%
echo.

set "RIMWORLD_QUICKSTART=%TEXTURE_QUICKSTART%"

"%RIMWORLD_EXE%" ^
    -savedatafolder="%TEST_SAVEDATA%" ^
    -logFile "%LOG%" ^
    -quickstart=%TEXTURE_QUICKSTART%

set "RESULT=%ERRORLEVEL%"
echo.
echo RimWorld closed with code %RESULT%.
echo Texture-debug log:
echo   %LOG%
exit /b %RESULT%
