@echo off
setlocal EnableExtensions

set "RIMWORLD_DIR=%~1"
if not defined RIMWORLD_DIR set "RIMWORLD_DIR=D:\SteamLibrary\steamapps\common\RimWorld"

set "BIOME=%~2"
if not defined BIOME set "BIOME=CoolTemperate"

set "TEXTURE_DEBUG_TARGET="
if /I "%BIOME%"=="WarmTemperate" set "TEXTURE_DEBUG_TARGET=AMJWarmTemperateTerrainQuickstart"
if /I "%BIOME%"=="CoolTemperate" set "TEXTURE_DEBUG_TARGET=AMJCoolTemperateTerrainQuickstart"
if /I "%BIOME%"=="Subalpine" set "TEXTURE_DEBUG_TARGET=AMJSubalpineTerrainQuickstart"
if /I "%BIOME%"=="Alpine" set "TEXTURE_DEBUG_TARGET=AMJAlpineTerrainQuickstart"

if not defined TEXTURE_DEBUG_TARGET (
    echo [ERROR] Unknown texture-debug biome selector: %BIOME%
    echo         Expected one of: WarmTemperate, CoolTemperate, Subalpine, Alpine
    exit /b 2
)

set "ROOT=%~dp0"
set "RIMWORLD_EXE=%RIMWORLD_DIR%\RimWorldWin64.exe"
set "RESULT_ROOT=%ROOT%TestResults\TextureDebug"
set "TEST_SAVEDATA=%RESULT_ROOT%\SaveData"
set "LOG=%RESULT_ROOT%\%TEXTURE_DEBUG_TARGET%.log"
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
echo Running focused %BIOME% texture-debug Quickstart
echo ============================================================
echo Target: %TEXTURE_DEBUG_TARGET%
echo This mode intentionally omits Quickstarts verification/report flags.
echo RimWorld will stay open until you close it manually.
echo The isolated profile has Dev Mode enabled.
echo Use the biome where the target plant naturally occurs.
echo Log:
echo   %LOG%
echo.

set "RIMWORLD_QUICKSTART=%TEXTURE_DEBUG_TARGET%"

"%RIMWORLD_EXE%" ^
    -savedatafolder="%TEST_SAVEDATA%" ^
    -logFile "%LOG%" ^
    -quickstart=%TEXTURE_DEBUG_TARGET%

set "RESULT=%ERRORLEVEL%"
echo.
echo RimWorld closed with code %RESULT%.
echo Texture-debug log:
echo   %LOG%
exit /b %RESULT%
