$ErrorActionPreference='Stop'
. (Join-Path $PSScriptRoot '..\Scripts\RimWorldRuntime.ps1')
$temp=Join-Path ([IO.Path]::GetTempPath()) ('runtime-log-'+[Guid]::NewGuid().ToString('N')+'.log')
try {
 [IO.File]::WriteAllText($temp,"Process: pid 123`n2026-10-13 01:02:03.100 INFO [Verse] ERROR count=0`n")
 Assert-RimWorldNativeLog $temp 123
 foreach($text in @("Process: pid 124`n","Process: pid 123`n2026-10-13 01:02:03.100 ERROR [Verse] failure`n",'')) {
  [IO.File]::WriteAllText($temp,$text)
  $rejected=$false
  try{Assert-RimWorldNativeLog $temp 123}catch{$rejected=$true}
  if(-not $rejected){throw 'Invalid native log was accepted'}
 }
 Write-Output '[OK] Native ERROR, empty and wrong-PID logs rejected; INFO accepted.'
} finally {Remove-Item -LiteralPath $temp -ErrorAction SilentlyContinue}
