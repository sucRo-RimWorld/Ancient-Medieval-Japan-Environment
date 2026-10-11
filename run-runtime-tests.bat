@echo off
setlocal EnableExtensions EnableDelayedExpansion

set "RIMWORLD_DIR=%~1"
if not defined RIMWORLD_DIR set "RIMWORLD_DIR=D:\SteamLibrary\steamapps\common\RimWorld"

set "SKIP_STATIC="
if /I "%~2"=="--skip-static" set "SKIP_STATIC=1"

set "ROOT=%~dp0"
set "RIMWORLD_EXE=%RIMWORLD_DIR%\RimWorldWin64.exe"
set "RESULT_ROOT=%ROOT%TestResults\VegetationRuntime"
set "TEST_SAVEDATA=%RESULT_ROOT%\SaveData"
set "REPORT_DIR=%RESULT_ROOT%\Reports"
set "CCTO_SAVEDATA=%RESULT_ROOT%\SaveData-CCTO"
set "CCTO_REPORT_DIR=%RESULT_ROOT%\Reports-CCTO"
set "MO_SAVEDATA=%RESULT_ROOT%\SaveData-MO"
set "MO_REPORT_DIR=%RESULT_ROOT%\Reports-MO"
set "CORE_SAVEDATA=%RESULT_ROOT%\SaveData-Core"
set "CORE_REPORT_DIR=%RESULT_ROOT%\Reports-Core"
set "QUICKTEST_DLL=%ROOT%DevQuickstarts\Assemblies\AncientMedievalJapanEnvironment.Quicktests.dll"
set "QUICKTEST_MANAGER=%ROOT%Scripts\Manage-EnvironmentQuicktestFixture.py"
set "WETLAND_PROBE_DEFS=%ROOT%Tests\Quickstarts\Fixtures\WetlandBehaviorProbeDefs.xml"
set "QUICKTEST_STAGED="

if not defined SKIP_STATIC (
    call "%ROOT%run-static-tests.bat" "%RIMWORLD_DIR%"
    if errorlevel 1 exit /b 1
)

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

if not exist "%QUICKTEST_MANAGER%" (
    echo [ERROR] Quicktests fixture manager was not found:
    echo         %QUICKTEST_MANAGER%
    exit /b 2
)

where py >nul 2>&1
if errorlevel 1 (
    echo [ERROR] Python launcher py.exe was not found.
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
echo Staging standalone Environment Quicktests test Mod...
py -3 "%QUICKTEST_MANAGER%" stage --game "%RIMWORLD_DIR%" --dll "%QUICKTEST_DLL%" --defs "%WETLAND_PROBE_DEFS%"
if errorlevel 1 exit /b 2
set "QUICKTEST_STAGED=1"
set "RESULT=0"

echo.
echo Preparing isolated RimWorld runtime-test profile...
powershell -NoProfile -ExecutionPolicy Bypass -File "%ROOT%Scripts\Prepare-EnvironmentRuntimeTestSaveData.ps1" ^
    -OutputRoot "%TEST_SAVEDATA%"
if errorlevel 1 (
    set "RESULT=2"
    goto :report
)
py -3 "%QUICKTEST_MANAGER%" activate --game "%RIMWORLD_DIR%" --config "%TEST_SAVEDATA%\Config\ModsConfig.xml"
if errorlevel 1 (
    set "RESULT=2"
    goto :report
)

echo.
echo Running Environment vegetation, wetland, and river/coast Quickstarts automatically...
echo The normal RimWorld mod list and Prefs.xml are not modified.
if defined AMJE_ISOLATED_RUNTIME (
    echo RimWorld renders on an isolated, non-visible Windows desktop.
) else (
    echo [INFO] Direct run-runtime-tests.bat execution may show RimWorld windows.
    echo        Use run-tests.bat for the standard non-visible full gate.
)
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
    py -3 "%QUICKTEST_MANAGER%" activate --game "%RIMWORLD_DIR%" --config "%CCTO_SAVEDATA%\Config\ModsConfig.xml"
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
    if not "!RESULT!"=="0" goto :report
) else (
    echo.
    echo [INFO] CCTO is not installed locally; skipping optional AMJE + CCTO runtime compatibility check.
)

set "CORE_INSTALLED="
if exist "%RIMWORLD_DIR%\Mods\AncientMedievalJapanCore\About\About.xml" set "CORE_INSTALLED=1"
if exist "%RIMWORLD_DIR%\Mods\Ancient-Medieval-Japan-Core\About\About.xml" set "CORE_INSTALLED=1"
if exist "%RIMWORLD_DIR%\Mods\AncientMedievalJapanGrains\About\About.xml" set "CORE_INSTALLED=1"
if exist "%RIMWORLD_DIR%\Mods\Ancient-Medieval-Japan-Grains\About\About.xml" set "CORE_INSTALLED=1"

if defined CORE_INSTALLED (
    echo.
    echo Preparing isolated AMJ Grains/Core + Environment gameplay-integration profile...
    powershell -NoProfile -ExecutionPolicy Bypass -File "%ROOT%Scripts\Prepare-EnvironmentRuntimeTestSaveData.ps1" ^
        -OutputRoot "%CORE_SAVEDATA%" ^
        -IncludeCore
    if errorlevel 1 (
        set "RESULT=2"
        goto :report
    )
    py -3 "%QUICKTEST_MANAGER%" activate --game "%RIMWORLD_DIR%" --config "%CORE_SAVEDATA%\Config\ModsConfig.xml"
    if errorlevel 1 (
        set "RESULT=2"
        goto :report
    )

    echo.
    echo Running AMJ Grains/Core + Environment gameplay-contract Quickstarts...
    powershell -NoProfile -ExecutionPolicy Bypass -File "%ROOT%Scripts\Run-EnvironmentVegetationQuickstarts.ps1" ^
        -ExePath "%RIMWORLD_EXE%" ^
        -SaveDataFolder "%CORE_SAVEDATA%" ^
        -ResultDir "%CORE_REPORT_DIR%" ^
        -TimeoutSeconds 420 ^
        -CoreIntegrationOnly

    set "RESULT=!ERRORLEVEL!"
    if not "!RESULT!"=="0" goto :report
) else (
    echo.
    echo [INFO] AMJ Grains/Core is not installed in the local RimWorld Mods folder; skipping optional integration runtime check.
)

echo.
echo [INFO] Medieval Overhaul tree-path validation is handled by the static gate.
echo        The isolated MO runtime profile is disabled because MO startup does not
echo        reach Quickstarts reliably in this minimal profile.

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
if defined CORE_INSTALLED (
    echo Grains/Core + Environment gameplay reports:
    echo   %CORE_REPORT_DIR%
)

if defined QUICKTEST_STAGED (
    echo.
    echo Removing standalone Environment Quicktests test Mod...
    py -3 "%QUICKTEST_MANAGER%" cleanup --game "%RIMWORLD_DIR%"
    if errorlevel 1 (
        echo [ERROR] Quicktests fixture cleanup failed; inspect the owned fixture before retrying.
        if "%RESULT%"=="0" set "RESULT=2"
    )
)
exit /b %RESULT%
