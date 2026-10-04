# AMJ Environment — Texture Asset Golden Path

## Purpose

This is the **known-good, repeatable path** for installing AMJE production textures after a generated or edited image has been visually approved.

It exists because the Sudajii and Japanese beech work demonstrated that a visually correct source image can still fail in RimWorld if the binary transfer, Def switch, test target, or validation order changes between assets.

The rule is therefore: **do not improvise a new transfer/install path for each texture. Reuse this pipeline.**

## Generation prerequisite

For tree/plant sprites, begin with [RetextureGeneration.md](RetextureGeneration.md). It preserves the current visual rules and actually viewed/attached reference images across chat changes, and requires an approved target design before generation. This document begins at the subsequent production-source approval/install stage.

## Golden Path

### 1. Approve the source image before touching the Def

For plant sprites, the current ENV-010 baseline is:

- standalone sprite, not a comparison sheet;
- transparent background;
- 256×256 unless the owning art specification explicitly says otherwise;
- no baked terrain, UI, text, or decorative frame;
- visual direction already approved by the author.

Do not point a Def at a new AMJE texture path before the production PNG exists.

### 2. Validate and install the exact PNG bytes

Use the repository helper:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\Scripts\Install-TextureAsset.ps1 `
  -SourcePath "C:\path\to\approved\Beech_A.png" `
  -DestinationRelativePath "Textures\Things\Plant\AMJ\Beech\Beech_A.png" `
  -Force
```

The helper validates **before and after the copy**:

- PNG signature;
- required IHDR / IDAT / IEND chunks;
- every PNG chunk CRC;
- IDAT zlib/DEFLATE payload, Adler-32, exact decoded scanline sizes, and legal PNG filters;
- exact dimensions;
- alpha/tRNS transparency support by default;
- SHA-256 equality between source, staging copy, and installed destination.

It copies the source bytes directly. Do not manually reconstruct image bytes, hand-edit base64, or re-encode an approved image merely to move it into the repository.

For validation without installing:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File .\Scripts\Install-TextureAsset.ps1 `
  -SourcePath "C:\path\to\approved\Beech_A.png" `
  -ValidateOnly
```

Use `-AllowOpaque` only for an asset type whose canonical art specification explicitly permits an opaque PNG.

### 3. Change the owning Def/path only after binary validation succeeds

After the production PNG has passed the helper:

1. point the Def or compatibility patch at the AMJE-owned texture path;
2. remove the old placeholder reference;
3. extend static regression coverage when the new path/state creates a new invariant.

This ordering prevents a temporary missing/corrupt asset path from being committed as the active game definition.

### 4. Run the automated gate

Run:

```powershell
.\run-tests.bat "D:\SteamLibrary\steamapps\common\RimWorld"
```

The gate must remain responsible for deterministic checks such as:

- PowerShell syntax;
- build/static validation;
- all repository PNG structure/CRC checks;
- decoded PNG payload validation (a valid chunk CRC alone cannot prove image data is intact);
- required texture files;
- expected Def paths;
- retired placeholder references.

A static PASS is not an in-game visual PASS.

### 5. Open the biome where the target plant naturally occurs

Use:

```powershell
.\run-texture-debug.bat "D:\SteamLibrary\steamapps\common\RimWorld" <Biome>
```

Accepted biome selectors:

| Selector | Quickstart | Primary AMJE art target |
| --- | --- | --- |
| `WarmTemperate` | `AMJWarmTemperateTerrainQuickstart` | Sudajii |
| `CoolTemperate` | `AMJCoolTemperateTerrainQuickstart` | Japanese beech |
| `Subalpine` | `AMJSubalpineTerrainQuickstart` | Shirabiso |
| `Alpine` | `AMJAlpineTerrainQuickstart` | Haimatsu |

The runner defaults to `CoolTemperate` only when no selector is supplied.

Do not use a biome where the target does not naturally spawn and then compensate by Dev Tool spawning unless the natural-spawn path itself is what is being debugged.

### 6. Perform only the human checks that require human judgment

At normal game zoom, inspect:

- silhouette/species distinction;
- scale;
- outline weight;
- palette/saturation against terrain and neighboring vegetation;
- transparency/ground contact;
- UI icon/readability;
- leafy/leafless continuity for deciduous trees.

If the image is structurally valid but RimWorld still shows a red question mark, treat that as a runtime loading defect and inspect the actual selected Thing/texture path before regenerating art.

### 7. Lock the success

After the author confirms the in-game result:

- record acceptance in `Docs/ArtDirection.md`;
- add/keep automated regression coverage for every failure class discovered;
- record the result/handoff in `main:Docs/Coordination.md`;
- do not overwrite an accepted production sprite casually. A replacement starts again at step 1.

## Known failure classes this Golden Path prevents

The pipeline specifically guards against failures already encountered during ENV-010:

- malformed PNG binary transfer despite a visually correct source image;
- valid chunk boundaries but invalid IDAT CRC;
- recalculated chunk CRC hiding corrupt compressed image data (the leafy beech failure);
- Def switched before the final binary was safely installed;
- debug runner opened a biome where the target plant did not naturally occur;
- validator hardcoded the previous debug biome;
- PowerShell validation edits introduced parser corruption.

## Decoded-image regression gate

`Scripts/PngImageData.ps1` supplies the shared .NET payload validator to both
`Install-TextureAsset.ps1` and `Validate-Environment.ps1`. It validates concatenated
IDAT data, the zlib header/checksum, scanline byte counts and filters, including
Adam7 pass sizes. It runs on Windows PowerShell 5.1 and PowerShell 7 without
an additional image-processing dependency. Existing PNG chunk CRC validation
remains required; neither layer substitutes for the other.

`Tests/Test-PngImageData.ps1` checks every production PNG and rejects a generated
fixture whose chunk CRC is correct but zlib checksum is corrupt. The GitHub
PowerShell gate runs it on PNG changes as well as PowerShell changes.

Do not repair only the stored CRC to make a failing image pass. Restore from
validated source bytes and require both the structural and decoded-image gates.

## Completion criterion

A texture task is not complete merely because the source image was approved.

For AMJE production art, completion requires:

**approved source → exact-byte install → automated gate PASS → correct natural-biome runtime review → author acceptance → regression/documentation lock.**
