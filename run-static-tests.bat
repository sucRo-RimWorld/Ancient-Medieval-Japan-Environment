@echo off
setlocal EnableExtensions
set "PSModulePath=%SystemRoot%\System32\WindowsPowerShell\v1.0\Modules"

set "RIMWORLD_DIR=%~1"
if not defined RIMWORLD_DIR set "RIMWORLD_DIR=D:\SteamLibrary\steamapps\common\RimWorld"

echo.
echo Checking PowerShell syntax...
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0Scripts\Validate-PowerShellSyntax.ps1"
if errorlevel 1 exit /b 1

echo.
echo Checking texture Golden Path exact-copy helper...
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0Scripts\Install-TextureAsset.ps1" ^
    -SourcePath "%~dp0Textures\Things\Plant\AMJ\Shii\Shii_A.png" ^
    -DestinationRelativePath "TestResults\GoldenPath\Shii_A.png" ^
    -Force
if errorlevel 1 exit /b 1
if exist "%~dp0TestResults\GoldenPath\Shii_A.png" del /Q "%~dp0TestResults\GoldenPath\Shii_A.png"
if exist "%~dp0TestResults\GoldenPath" rmdir "%~dp0TestResults\GoldenPath"

if exist "%RIMWORLD_DIR%\UnityStubs.dll" (
    powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0Scripts\Build-17Tests.ps1" -GameRoot "%RIMWORLD_DIR%"
) else (
    call "%~dp0build.bat" "%RIMWORLD_DIR%"
)
if errorlevel 1 exit /b 1

echo.
echo Validating AMJ Environment against installed RimWorld Core source Defs...
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0Scripts\Validate-Environment.ps1" -RimWorldDir "%RIMWORLD_DIR%"
if errorlevel 1 exit /b 1

echo.
echo Auditing installed Medieval Overhaul tree texture references...
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0Scripts\Validate-MedievalOverhaulTreeTextures.ps1" -RimWorldDir "%RIMWORLD_DIR%"
if errorlevel 1 exit /b 1

echo.
echo [OK] AMJ Environment build + static validation passed
exit /b 0
