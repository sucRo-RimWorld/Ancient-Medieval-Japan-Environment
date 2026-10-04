@echo off
setlocal EnableExtensions

set "RIMWORLD_DIR=%~1"
if not defined RIMWORLD_DIR set "RIMWORLD_DIR=D:\SteamLibrary\steamapps\common\RimWorld"

echo.
echo Checking PowerShell syntax...
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0Scripts\Validate-PowerShellSyntax.ps1" -RepoRoot "%~dp0"
if errorlevel 1 exit /b 1

call "%~dp0build.bat" "%RIMWORLD_DIR%"
if errorlevel 1 exit /b 1

echo.
echo Validating AMJ Environment against installed RimWorld 1.6 source Defs...
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0Scripts\Validate-Environment.ps1" -RimWorldDir "%RIMWORLD_DIR%"
if errorlevel 1 exit /b 1

echo.
echo Auditing installed Medieval Overhaul tree texture references...
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0Scripts\Validate-MedievalOverhaulTreeTextures.ps1" -RimWorldDir "%RIMWORLD_DIR%"
if errorlevel 1 exit /b 1

echo.
echo [OK] AMJ Environment build + static validation passed
exit /b 0
