# Art Asset Golden Path

This is the AMJ Environment Golden Path for plant/tree/terrain/world texture work.

It was formalized after the Sudajii and Japanese beech work exposed repeatable failure modes in binary transfer, Def switching, PNG validation, biome selection, and debug-runner maintenance.

## Scope

Use this procedure whenever AMJE:
- adds a new PNG asset;
- replaces a Vanilla or Medieval Overhaul texture;
- changes an AMJE plant/tree leafy or leafless state;
- changes terrain/world-biome artwork.

This procedure supplements `Docs/ArtDirection.md`. ArtDirection owns the visual specification; this document owns the repeatable implementation/test path.

## Golden Path

### 1. Approve the source art before integration

Human review decides:
- silhouette;
- color/saturation;
- outline weight;
- visual distinction from related assets;
- leafy/leafless consistency where applicable.

Do not change Def paths while the art is still being iterated.

### 2. Produce the final standalone asset

For plant/tree production art:
- use a standalone transparent PNG;
- normalize to the intended production size before repository integration;
- do not include comparison-sheet backgrounds, labels, or decorative ground rings unless explicitly part of the final design.

Preserve exact final bytes after approval.

### 3. Transfer exact bytes only

Binary asset transfer must preserve the approved file byte-for-byte.

Forbidden:
- manually transcribing or reconstructing base64;
- copying only part of a generated payload;
- re-encoding through an unrelated text/editing path without revalidation.

If the tooling cannot transfer the exact source bytes reliably, stop and use a different transfer path rather than committing an approximate reconstruction.

### 4. Commit the PNG before switching its Def path

The repository must never point at a texture that does not yet exist.

Order:
1. add the PNG;
2. verify its repository path;
3. run the PNG integrity/static gate;
4. only then change the owning Def/Patch to the AMJE path.

A combined commit is acceptable only when the same automated gate validates the final tree before it is considered complete.

### 5. Run the automated gate

Run:

```powershell
.\run-tests.bat "D:\SteamLibrary\steamapps\common\RimWorld"
```

The gate must remain responsible for:
- PowerShell syntax preflight;
- PNG signature/chunk-boundary/CRC validation;
- required production PNG presence;
- Def/Patch path ownership;
- retired placeholder path checks;
- other repository static regressions.

A PNG is not considered valid merely because its signature and chunk lengths parse. Chunk CRC validation is required.

### 6. Use the correct natural biome for visual inspection

Do not spawn the target manually merely because the focused runner opened the wrong biome.

Use:

```powershell
.\run-texture-debug.bat "D:\SteamLibrary\steamapps\common\RimWorld" <Warm|Cool|Subalpine|Alpine>
```

Examples:

```powershell
.\run-texture-debug.bat "D:\SteamLibrary\steamapps\common\RimWorld" Cool
.\run-texture-debug.bat "D:\SteamLibrary\steamapps\common\RimWorld" Subalpine
```

Current intended mappings:
- `Warm` → `AMJWarmTemperateTerrainQuickstart`
- `Cool` → `AMJCoolTemperateTerrainQuickstart`
- `Subalpine` → `AMJSubalpineTerrainQuickstart`
- `Alpine` → `AMJAlpineTerrainQuickstart`

If no second argument is supplied, the runner defaults to `Cool`.

Choose the biome where the target appears naturally. This keeps the inspection representative and avoids confusing texture failures with spawn/biome-selection mistakes.

### 7. Human in-game acceptance

Inspect only items that automation cannot judge reliably:
- scale at normal gameplay zoom;
- silhouette readability;
- outline weight;
- palette against surrounding terrain;
- UI icon;
- leafy/leafless seasonal continuity;
- tiling seams for terrain;
- world-map distinction for biome textures.

If a red question mark appears, treat it first as an asset/path/integrity failure. Do not approve the art until the actual production texture is visible.

### 8. Lock the accepted result

After in-game acceptance:
- record the accepted visual direction in `Docs/ArtDirection.md`;
- keep static/runtime coverage for the production path;
- do not overwrite an accepted production texture casually;
- record only status/handoff in `Docs/Coordination.md`.

For a new class of recurring failure, extend this Golden Path and its automated guards before moving to the next similar asset.

## Known failure modes now guarded

- malformed PNG chunk structure;
- valid-looking PNG with incorrect chunk CRC;
- Def path switched before production asset exists;
- placeholder Vanilla path left behind;
- focused runner hardcoded to the previous target biome;
- PowerShell parser breakage in validation tooling.

## Completion

An AMJE production asset is complete only when:
1. source art is accepted;
2. exact production PNG is in the repository;
3. automated static/integrity checks pass;
4. the owning Def/Patch uses the intended path;
5. the correct natural-biome debug run shows the actual asset;
6. human visual acceptance is complete;
7. any newly discovered reusable lesson has been added to this Golden Path or its automation.
