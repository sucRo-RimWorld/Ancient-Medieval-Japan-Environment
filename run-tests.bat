@echo off
setlocal EnableExtensions

set "RIMWORLD_DIR=%~1"
if not defined RIMWORLD_DIR set "RIMWORLD_DIR=D:\SteamLibrary\steamapps\common\RimWorld"

call "%~dp0build.bat" "%RIMWORLD_DIR%"
if errorlevel 1 exit /b 1

echo.
echo Validating AMJ Environment against installed RimWorld 1.6 source Defs...
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0Scripts\Validate-Environment.ps1" -RimWorldDir "%RIMWORLD_DIR%"
if errorlevel 1 exit /b 1

echo.
echo [OK] AMJ Environment build + static validation passed
exit /b 0
