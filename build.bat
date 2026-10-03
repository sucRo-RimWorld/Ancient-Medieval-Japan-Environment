@echo off
setlocal EnableExtensions EnableDelayedExpansion

set "RIMWORLD_DIR=%~1"
if not defined RIMWORLD_DIR set "RIMWORLD_DIR=D:\SteamLibrary\steamapps\common\RimWorld"

set "CSC=%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
if not exist "%CSC%" set "CSC=%WINDIR%\Microsoft.NET\Framework\v4.0.30319\csc.exe"

if not exist "%CSC%" (
    echo [ERROR] C# compiler was not found.
    exit /b 1
)

set "MANAGED=%RIMWORLD_DIR%\RimWorldWin64_Data\Managed"
set "ASSEMBLY_CSHARP=%MANAGED%\Assembly-CSharp.dll"
set "UNITY_CORE=%MANAGED%\UnityEngine.CoreModule.dll"
set "NETSTANDARD=%MANAGED%\netstandard.dll"

if not exist "%ASSEMBLY_CSHARP%" (
    echo [ERROR] Assembly-CSharp.dll was not found:
    echo         %ASSEMBLY_CSHARP%
    exit /b 1
)

if not exist "%UNITY_CORE%" (
    echo [ERROR] UnityEngine.CoreModule.dll was not found:
    echo         %UNITY_CORE%
    exit /b 1
)

if not exist "%NETSTANDARD%" (
    echo [ERROR] netstandard.dll was not found:
    echo         %NETSTANDARD%
    exit /b 1
)

set "STEAMAPPS=%RIMWORLD_DIR%\..\.."
set "HARMONY_ROOT=%STEAMAPPS%\workshop\content\294100\2009463077"
set "HARMONY_DLL="

for %%P in (
    "%HARMONY_ROOT%\Current\Assemblies\0Harmony.dll"
    "%HARMONY_ROOT%\1.6\Assemblies\0Harmony.dll"
    "%HARMONY_ROOT%\Assemblies\0Harmony.dll"
) do (
    if not defined HARMONY_DLL if exist "%%~P" set "HARMONY_DLL=%%~fP"
)

if not defined HARMONY_DLL if exist "%HARMONY_ROOT%" (
    for /r "%HARMONY_ROOT%" %%F in (0Harmony.dll) do (
        if not defined HARMONY_DLL set "HARMONY_DLL=%%~fF"
    )
)

if not defined HARMONY_DLL (
    echo [ERROR] 0Harmony.dll was not found under:
    echo         %HARMONY_ROOT%
    echo.
    echo Make sure Harmony ^(Steam Workshop 2009463077^) is installed.
    exit /b 1
)

set "ROOT=%~dp0"
set "SOURCE_DIR=%ROOT%Source\AncientMedievalJapanEnvironment"
set "OUTPUT_DIR=%ROOT%Assemblies"
set "OUTPUT_DLL=%OUTPUT_DIR%\AncientMedievalJapanEnvironment.dll"

if not exist "%OUTPUT_DIR%" mkdir "%OUTPUT_DIR%"

set "SOURCES="
for /r "%SOURCE_DIR%" %%F in (*.cs) do (
    set "SOURCES=!SOURCES! "%%~fF""
)

echo RimWorld : %RIMWORLD_DIR%
echo Harmony  : %HARMONY_DLL%
echo Compiler : %CSC%
echo Output   : %OUTPUT_DLL%
echo.

"%CSC%" /nologo /target:library /optimize+ /out:"%OUTPUT_DLL%" ^
    /reference:"%ASSEMBLY_CSHARP%" ^
    /reference:"%UNITY_CORE%" ^
    /reference:"%NETSTANDARD%" ^
    /reference:"%HARMONY_DLL%" ^
    !SOURCES!

if errorlevel 1 (
    echo.
    echo [ERROR] Build failed.
    exit /b 1
)

echo.
echo [OK] Build succeeded:
echo      %OUTPUT_DLL%
exit /b 0
