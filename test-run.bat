@echo off
setlocal EnableExtensions

rem Standard one-command Steam Workshop Pickle test entry.
rem Python runs internally; no user-provided flags, manifests or output paths.
set "ROOT=%~dp0"
set "RIMWORLD_DIR=%~1"
if not defined RIMWORLD_DIR set "RIMWORLD_DIR=D:\SteamLibrary\steamapps\common\RimWorld"

if not exist "%RIMWORLD_DIR%\RimWorldWin64.exe" (
    echo [ERROR] RimWorld executable not found: "%RIMWORLD_DIR%\RimWorldWin64.exe"
    exit /b 2
)
where py >nul 2>&1
if errorlevel 1 (
    echo [ERROR] Python launcher py.exe not found.
    exit /b 2
)

echo [AMJE] Testing Steam Workshop 3814638060 using RimWorks Pickle...
echo [AMJE] Exact matching manifest and fresh results are selected automatically.
echo [AMJE] Normal game configuration and subscribed Mod remain unchanged.
echo.
py -3 "%ROOT%Scripts\Run-WorkshopPickle.py" --game "%RIMWORLD_DIR%"
set "RESULT=%ERRORLEVEL%"
if not "%RESULT%"=="0" (
    echo.
    echo [FAIL] AMJE Pickle stopped with exit code %RESULT%.
    echo [INFO] The failure reason was printed above; manual report lookup is unnecessary.
    exit /b %RESULT%
)

echo.
echo [OK] Steam Pickle loaded-Def validation passed.
echo [INFO] Rendered world/terrain and real cutting-job release checks remain separate.
exit /b 0
