@echo off
setlocal EnableExtensions EnableDelayedExpansion

set "RIMWORLD_DIR=%~1"
if not defined RIMWORLD_DIR set "RIMWORLD_DIR=D:\SteamLibrary\steamapps\common\RimWorld"

set "ROOT=%~dp0"
set "RIMWORLD_EXE=%RIMWORLD_DIR%\RimWorldWin64.exe"
set "RESULT_ROOT=%ROOT%TestResults\VegetationRuntime"
set "TEST_SAVEDATA=%RESULT_ROOT%\SaveData"
set "REPORT_DIR=%RESULT_ROOT%\Reports"
set "CCTO_SAVEDATA=%RESULT_ROOT%\SaveData-CCTO"
set "CCTO_REPORT_DIR=%RESULT_ROOT%\Reports-CCTO"
set "MO_SAVEDATA=%RESULT_ROOT%\SaveData-MO"
set "MO_REPORT_DIR=%RESULT_ROOT%\Reports-MO"
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

set "RESULT=!ERRORLEVEL!"
if not "%RESULT%"=="0" goto :report

set "CCTO_INSTALLED="
if exist "%RIMWORLD_DIR%\Mods\CropColdToleranceOverhaul\About\About.xml" set "CCTO_INSTALLED=1"
if exist "%RIMWORLD_DIR%\..\..\workshop\content\294100\3812412548\About\About.xml" set "CCTO_INSTALLED=1"

if defined CCTO_INSTALLED (
    echo.
    echo Preparing isolated AMJE + CCTO compatibility profile...
    powershell -NoProfile -ExecutionPolicy Bypass -File "%ROOT%Scripts\Prepare-EnvironmentRuntimeTestSaveData.ps1" ^
        -OutputRoot "%CCTO_SAVEDATA%" ^
        -IncludeCCTO
    if errorlevel 1 (
        set "RESULT=2"
        goto :report
    )

    echo.
    echo Running focused AMJE + CCTO loaded-Def compatibility check...
    powershell -NoProfile -ExecutionPolicy Bypass -File "%ROOT%Scripts\Run-EnvironmentVegetationQuickstarts.ps1" ^
        -ExePath "%RIMWORLD_EXE%" ^
        -SaveDataFolder "%CCTO_SAVEDATA%" ^
        -ResultDir "%CCTO_REPORT_DIR%" ^
        -TimeoutSeconds 180 ^
        -CctoCompatibilityOnly

    set "RESULT=!ERRORLEVEL!"
) else (
    echo.
    echo [INFO] CCTO is not installed locally; skipping optional AMJE + CCTO runtime compatibility check.
)

set "MO_READY="
if exist "%RIMWORLD_DIR%\..\..\workshop\content\294100\3219596926\About\About.xml" (
    if exist "%RIMWORLD_DIR%\..\..\workshop\content\294100\2023507013\About\About.xml" (
        if exist "%RIMWORLD_DIR%\..\..\workshop\content\294100\3210544395\About\About.xml" (
            set "MO_READY=1"
        )
    )
)

if defined MO_READY (
    echo.
    echo Preparing isolated AMJE + Medieval Overhaul tree-texture audit profile...
    powershell -NoProfile -ExecutionPolicy Bypass -File "%ROOT%Scripts\Prepare-EnvironmentRuntimeTestSaveData.ps1" ^
        -OutputRoot "%MO_SAVEDATA%" ^
        -IncludeMedievalOverhaul
    if errorlevel 1 (
        set "RESULT=2"
        goto :report
    )

    echo.
    echo Running focused AMJE + Medieval Overhaul tree graphic-state audit...
    powershell -NoProfile -ExecutionPolicy Bypass -File "%ROOT%Scripts\Run-EnvironmentVegetationQuickstarts.ps1" ^
        -ExePath "%RIMWORLD_EXE%" ^
        -SaveDataFolder "%MO_SAVEDATA%" ^
        -ResultDir "%MO_REPORT_DIR%" ^
        -TimeoutSeconds 180 ^
        -TreeTextureAuditOnly

    set "RESULT=!ERRORLEVEL!"
) else (
    echo.
    echo [INFO] Medieval Overhaul or one of its required framework mods is not installed in the expected Workshop paths.
    echo        Skipping optional AMJE + Medieval Overhaul tree-texture audit.
)

:report
echo.
if "%RESULT%"=="0" (
    echo [OK] Environment runtime gate passed.
) else if "%RESULT%"=="124" (
    echo [ERROR] A RimWorld Quickstart exceeded the outer timeout.
) else (
    echo [FAIL] Environment runtime gate failed.
)

echo Base reports:
echo   %REPORT_DIR%
if defined CCTO_INSTALLED (
    echo CCTO compatibility reports:
    echo   %CCTO_REPORT_DIR%
)
if defined MO_READY (
    echo Medieval Overhaul tree-texture audit reports:
    echo   %MO_REPORT_DIR%
)
exit /b %RESULT%
