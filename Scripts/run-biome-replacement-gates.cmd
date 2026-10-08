@echo off
setlocal
set "ROOT=%~dp0.."
set "PSModulePath=%SystemRoot%\System32\WindowsPowerShell\v1.0\Modules"
set "RIMWORLD_DIR=D:\SteamLibrary\steamapps\common\RimWorld"
set "RESULT=%ROOT%\TestResults\BiomeReplacement"
set "RIMWORLD_AMJE_WORLD_BIOME_AUDIT=1"
call "%ROOT%\build.bat" "%RIMWORLD_DIR%" > "%RESULT%\suite.log" 2>&1
if errorlevel 1 exit /b 1
powershell -NoProfile -ExecutionPolicy Bypass -File "%ROOT%\Scripts\Prepare-EnvironmentRuntimeTestSaveData.ps1" -OutputRoot "%RESULT%\SaveData" >> "%RESULT%\suite.log" 2>&1
if errorlevel 1 exit /b 1
powershell -NoProfile -ExecutionPolicy Bypass -File "%ROOT%\Scripts\Run-EnvironmentVegetationQuickstarts.ps1" -ExePath "%RIMWORLD_DIR%\RimWorldWin64.exe" -SaveDataFolder "%RESULT%\SaveData" -ResultDir "%RESULT%\Reports" -TimeoutSeconds 240 >> "%RESULT%\suite.log" 2>&1
if errorlevel 1 exit /b 1
powershell -NoProfile -ExecutionPolicy Bypass -File "%ROOT%\Scripts\Prepare-EnvironmentRuntimeTestSaveData.ps1" -OutputRoot "%RESULT%\SaveData-MO" -IncludeMedievalOverhaul >> "%RESULT%\suite.log" 2>&1
if errorlevel 1 exit /b 1
powershell -NoProfile -ExecutionPolicy Bypass -File "%ROOT%\Scripts\Run-EnvironmentVegetationQuickstarts.ps1" -ExePath "%RIMWORLD_DIR%\RimWorldWin64.exe" -SaveDataFolder "%RESULT%\SaveData-MO" -ResultDir "%RESULT%\Reports-MO" -TimeoutSeconds 240 -TreeTextureAuditOnly >> "%RESULT%\suite.log" 2>&1
exit /b %ERRORLEVEL%
