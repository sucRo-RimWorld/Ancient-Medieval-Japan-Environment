#!/usr/bin/env bash
# Build native AMJE production and dedicated Quicktests against an owned
# RimWorld Linux 1.6 game installation; no Steam/user configuration changes.
set -euo pipefail

if (( $# != 4 )); then
  echo "Usage: $0 GAME_ROOT HARMONY_1_6_DLL QUICKSTARTS_1_6_DLL OUTPUT_ROOT" >&2
  exit 2
fi

root="$(cd "$(dirname "$0")/.." && pwd)"
game="$1"
harmony="$2"
quickstarts="$3"
out="$4"
managed="$game/RimWorldLinux_Data/Managed"

if [[ ! -f "$game/Version.txt" ]] || ! grep -Eq '1\.6\.' "$game/Version.txt"; then
  echo "[ERROR] Expected a licensed RimWorld 1.6 Linux game root" >&2
  exit 2
fi
command -v mcs >/dev/null 2>&1 || { echo "[ERROR] Mono compiler (mcs) missing" >&2; exit 2; }

for ref in "$managed/Assembly-CSharp.dll" "$managed/UnityEngine.dll" \
    "$managed/UnityEngine.CoreModule.dll" "$managed/UnityEngine.IMGUIModule.dll" \
    "$managed/Unity.Mathematics.dll" "$managed/Unity.Collections.dll" \
    "$managed/netstandard.dll" "$harmony" "$quickstarts"; do
  [[ -f "$ref" ]] || { echo "[ERROR] Missing assembly: $ref" >&2; exit 2; }
done

mkdir -p "$out/Assemblies" "$out/Quicktests/Assemblies"
production="$out/Assemblies/AncientMedievalJapanEnvironment.dll"
testdll="$out/Quicktests/Assemblies/AncientMedievalJapanEnvironment.Quicktests.dll"
sources=("$root"/Source/AncientMedievalJapanEnvironment/*.cs)

mcs -target:library -langversion:latest -optimize+ -out:"$production" \
  -r:"$managed/Assembly-CSharp.dll" -r:"$managed/UnityEngine.CoreModule.dll" \
  -r:"$managed/Unity.Mathematics.dll" -r:"$managed/Unity.Collections.dll" \
  -r:"$managed/netstandard.dll" -r:"$harmony" \
  "$root"/Source/AncientMedievalJapanEnvironment/*.cs

mcs -target:library -langversion:latest -optimize+ -out:"$testdll" \
  -r:"$managed/Assembly-CSharp.dll" -r:"$managed/UnityEngine.CoreModule.dll" \
  -r:"$managed/UnityEngine.dll" -r:"$managed/UnityEngine.IMGUIModule.dll" \
  -r:"$managed/Unity.Mathematics.dll" -r:"$managed/Unity.Collections.dll" \
  -r:"$managed/netstandard.dll" -r:"$harmony" -r:"$quickstarts" \
  -r:"$production" \
  "$root/Tests/Quickstarts/EnvironmentBiomeTerrainQuickstarts.cs"

sha256sum "$production" "$testdll"
echo "[OK] AMJE Linux 1.6 production and native Quicktests compiled."
