param(
 [Parameter(Mandatory=$true)][string]$GameRoot,
 [Parameter(Mandatory=$true)][string]$ArgumentBase64,
 [Parameter(Mandatory=$true)][string]$LogPath,
 [int]$TimeoutSeconds=300
)
$ErrorActionPreference='Stop'
. (Join-Path $PSScriptRoot 'RimWorldRuntime.ps1')
try {
 $arguments=[Text.Encoding]::UTF8.GetString([Convert]::FromBase64String($ArgumentBase64))
 Invoke-RimWorldIsolated $GameRoot $arguments $LogPath $TimeoutSeconds
} catch [TimeoutException] { Write-Output ('[ERROR] '+$_.Exception.Message); exit 124 } catch { Write-Output ('[ERROR] '+$_.Exception.Message); exit 1 }
