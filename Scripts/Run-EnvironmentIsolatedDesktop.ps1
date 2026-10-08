param(
    [string]$RimWorldRoot = 'D:\SteamLibrary\steamapps\common\RimWorld',
    [int]$TimeoutSeconds = 1800
)

$ErrorActionPreference = 'Stop'
$env:PSModulePath = "$env:SystemRoot\System32\WindowsPowerShell\v1.0\Modules"

if ($RimWorldRoot -match '["%\r\n]') {
    throw 'RimWorldRoot contains characters that are unsafe for the batch launcher.'
}

$rimWorldExe = Join-Path $RimWorldRoot 'RimWorldWin64.exe'
if (-not (Test-Path -LiteralPath $rimWorldExe)) {
    throw "RimWorld executable was not found: $rimWorldExe"
}

$runner = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\run-runtime-tests.bat'))
if (-not (Test-Path -LiteralPath $runner)) {
    throw "Environment runtime runner was not found: $runner"
}
$quicktestManager = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot 'Manage-EnvironmentQuicktestFixture.py'))
if (-not (Test-Path -LiteralPath $quicktestManager)) {
    throw "Environment Quicktests fixture manager was not found: $quicktestManager"
}

$testResults = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\TestResults'))
New-Item -ItemType Directory -Force -Path $testResults | Out-Null
$log = Join-Path $testResults 'EnvironmentIsolatedRuntime.log'

Add-Type -TypeDefinition @'
using System;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;

public static class AmjeDesktop
{
    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
    public struct Startup
    {
        public int cb;
        public string reserved;
        public string desktop;
        public string title;
        public int x;
        public int y;
        public int cx;
        public int cy;
        public int xchars;
        public int ychars;
        public int fill;
        public int flags;
        public short show;
        public short reservedBytes;
        public IntPtr reservedPtr;
        public IntPtr input;
        public IntPtr output;
        public IntPtr error;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ProcessInfo
    {
        public IntPtr process;
        public IntPtr thread;
        public uint pid;
        public uint tid;
    }

    [DllImport("user32.dll", CharSet=CharSet.Unicode, SetLastError=true)]
    public static extern IntPtr CreateDesktop(
        string name,
        IntPtr device,
        IntPtr mode,
        uint flags,
        uint access,
        IntPtr security);

    [DllImport("user32.dll")]
    public static extern bool CloseDesktop(IntPtr desktop);

    [DllImport("kernel32.dll", CharSet=CharSet.Unicode, SetLastError=true)]
    public static extern bool CreateProcess(
        string app,
        StringBuilder command,
        IntPtr psa,
        IntPtr tsa,
        bool inherit,
        uint flags,
        IntPtr env,
        string cwd,
        ref Startup startup,
        out ProcessInfo process);

    [DllImport("kernel32.dll")]
    public static extern uint WaitForSingleObject(IntPtr handle, uint millis);

    [DllImport("kernel32.dll")]
    public static extern bool GetExitCodeProcess(IntPtr process, out uint code);

    [DllImport("kernel32.dll")]
    public static extern bool CloseHandle(IntPtr handle);

    public delegate bool WindowCallback(IntPtr window, IntPtr parameter);

    [DllImport("user32.dll", SetLastError=true)]
    public static extern bool EnumDesktopWindows(
        IntPtr desktop,
        WindowCallback callback,
        IntPtr parameter);

    [DllImport("user32.dll")]
    public static extern uint GetWindowThreadProcessId(IntPtr window, out uint pid);

    public static string WindowProcesses(IntPtr desktop)
    {
        var ids = new System.Collections.Generic.HashSet<uint>();
        WindowCallback callback = delegate(IntPtr window, IntPtr parameter)
        {
            uint pid;
            GetWindowThreadProcessId(window, out pid);
            ids.Add(pid);
            return true;
        };

        EnumDesktopWindows(desktop, callback, IntPtr.Zero);

        var names = new System.Collections.Generic.List<string>();
        foreach (uint pid in ids)
        {
            try
            {
                var process = System.Diagnostics.Process.GetProcessById((int)pid);
                names.Add(process.ProcessName + ":" + pid);
            }
            catch (ArgumentException)
            {
            }
        }

        return String.Join(",", names.ToArray());
    }
}
'@

$desktopName = 'AMJE_Automated_' + [Guid]::NewGuid().ToString('N')
$desktop = [AmjeDesktop]::CreateDesktop(
    $desktopName,
    [IntPtr]::Zero,
    [IntPtr]::Zero,
    0,
    0x01FF,
    [IntPtr]::Zero)

if ($desktop -eq [IntPtr]::Zero) {
    throw (New-Object ComponentModel.Win32Exception)
}

$info = New-Object AmjeDesktop+ProcessInfo
$started = $false
$finished = $false

try {
    $startup = New-Object AmjeDesktop+Startup
    $startup.cb = [Runtime.InteropServices.Marshal]::SizeOf($startup)
    $startup.desktop = 'WinSta0\' + $desktopName

    $env:AMJE_ISOLATED_RUNTIME = '1'

    $command = New-Object Text.StringBuilder
    [void]$command.Append(
        '"' + $env:ComSpec +
        '" /d /s /c ""' + $runner +
        '" "' + $RimWorldRoot +
        '" --skip-static > "' + $log + '" 2>&1"')

    $started = [AmjeDesktop]::CreateProcess(
        $env:ComSpec,
        $command,
        [IntPtr]::Zero,
        [IntPtr]::Zero,
        $false,
        0x08000000,
        [IntPtr]::Zero,
        (Split-Path -Parent $runner),
        [ref]$startup,
        [ref]$info)

    if (-not $started) {
        throw (New-Object ComponentModel.Win32Exception)
    }

    Write-Output (
        "[START] Desktop=$($startup.desktop); runner=$($info.pid); " +
        "rendering enabled; desktop never switched.")

    $seen = New-Object 'System.Collections.Generic.HashSet[string]'
    $timer = [Diagnostics.Stopwatch]::StartNew()

    while ($timer.Elapsed.TotalSeconds -lt $TimeoutSeconds) {
        $state = [AmjeDesktop]::WaitForSingleObject($info.process, 5000)
        if ($state -eq 0) {
            $finished = $true
            break
        }
        if ($state -ne 258) {
            throw "Wait failed: $state"
        }

        $windows = [AmjeDesktop]::WindowProcesses($desktop)
        if ($windows -and $seen.Add($windows)) {
            Write-Output "[WINDOWS] $desktopName : $windows"
        }
    }

    if (-not $finished) {
        throw ("Environment runtime suite exceeded " + $TimeoutSeconds + "s. See " + $log)
    }

    [uint32]$result = 0
    if (-not [AmjeDesktop]::GetExitCodeProcess($info.process, [ref]$result)) {
        throw (New-Object ComponentModel.Win32Exception)
    }

    Write-Output "[EXIT] $result; log=$log"
    exit $result
}
finally {
    Remove-Item Env:AMJE_ISOLATED_RUNTIME -ErrorAction SilentlyContinue

    if ($started -and -not $finished) {
        & taskkill /PID $info.pid /T /F | Out-Null
    }

    try {
        & py -3 $quicktestManager cleanup --game $RimWorldRoot
        if ($LASTEXITCODE -ne 0) {
            Write-Warning "Environment Quicktests fixture cleanup returned exit code $LASTEXITCODE."
        }
    }
    catch {
        Write-Warning "Environment Quicktests fixture cleanup could not run: $($_.Exception.Message)"
    }

    if ($info.thread -ne [IntPtr]::Zero) {
        [void][AmjeDesktop]::CloseHandle($info.thread)
    }
    if ($info.process -ne [IntPtr]::Zero) {
        [void][AmjeDesktop]::CloseHandle($info.process)
    }
    [void][AmjeDesktop]::CloseDesktop($desktop)
}
