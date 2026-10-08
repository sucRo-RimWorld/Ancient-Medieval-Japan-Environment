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
set "UNITY_MATH=%MANAGED%\Unity.Mathematics.dll"
set "UNITY_COLLECTIONS=%MANAGED%\Unity.Collections.dll"
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

if not exist "%UNITY_MATH%" (
    echo [ERROR] Unity.Mathematics.dll was not found:
    echo         %UNITY_MATH%
    exit /b 1
)

if not exist "%UNITY_COLLECTIONS%" (
    echo [ERROR] Unity.Collections.dll was not found:
    echo         %UNITY_COLLECTIONS%
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
    /reference:"%UNITY_MATH%" ^
    /reference:"%UNITY_COLLECTIONS%" ^
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

rem Developer-only fixed-biome Quickstarts. This assembly is loaded only when
rem rimworks.quickstarts is active through loadFolders.xml.
set "QUICKSTART_ROOT=%STEAMAPPS%\workshop\content\294100\3793646067"
set "QUICKSTART_DLL="

for %%P in (
    "%QUICKSTART_ROOT%\1.6\Assemblies\Quickstarts.dll"
    "%QUICKSTART_ROOT%\Current\Assemblies\Quickstarts.dll"
    "%QUICKSTART_ROOT%\Assemblies\Quickstarts.dll"
) do (
    if not defined QUICKSTART_DLL if exist "%%~P" set "QUICKSTART_DLL=%%~fP"
)

if not defined QUICKSTART_DLL if exist "%QUICKSTART_ROOT%" (
    for /r "%QUICKSTART_ROOT%" %%F in (Quickstarts.dll) do (
        if not defined QUICKSTART_DLL set "QUICKSTART_DLL=%%~fF"
    )
)

if defined QUICKSTART_DLL (
    set "QUICKTEST_SOURCE=%ROOT%Tests\Quickstarts\EnvironmentBiomeTerrainQuickstarts.cs"
    set "QUICKTEST_OUTPUT_DIR=%ROOT%DevQuickstarts\Assemblies"
    set "QUICKTEST_OUTPUT_DLL=!QUICKTEST_OUTPUT_DIR!\AncientMedievalJapanEnvironment.Quicktests.dll"

    if not exist "!QUICKTEST_SOURCE!" (
        echo [ERROR] Environment Quickstart source was not found:
        echo         !QUICKTEST_SOURCE!
        exit /b 1
    )

    if not exist "!QUICKTEST_OUTPUT_DIR!" mkdir "!QUICKTEST_OUTPUT_DIR!"

    echo.
    echo Quickstarts: !QUICKSTART_DLL!
    echo Quicktests : !QUICKTEST_OUTPUT_DLL!
    echo.

    "%CSC%" /nologo /target:library /optimize+ /out:"!QUICKTEST_OUTPUT_DLL!" ^
        /reference:"%ASSEMBLY_CSHARP%" ^
        /reference:"%UNITY_CORE%" ^
        /reference:"%MANAGED%\UnityEngine.IMGUIModule.dll" ^
        /reference:"%UNITY_MATH%" ^
        /reference:"%UNITY_COLLECTIONS%" ^
        /reference:"%NETSTANDARD%" ^
        /reference:"%HARMONY_DLL%" ^
        /reference:"!QUICKSTART_DLL!" ^
        "!QUICKTEST_SOURCE!"

    if errorlevel 1 (
        echo.
        echo [ERROR] Environment Quickstart build failed.
        exit /b 1
    )

    echo.
    echo [OK] Fixed-biome Quickstarts built:
    echo      !QUICKTEST_OUTPUT_DLL!
) else (
    echo.
    echo [INFO] Quickstarts ^(Workshop 3793646067^) was not found.
    echo        Skipping developer-only fixed-biome Quickstart assembly.
)

exit /b 0
