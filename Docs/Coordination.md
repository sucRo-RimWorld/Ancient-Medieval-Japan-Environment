# AMJ Environment Coordination

This file is the authoritative coordination surface for **Ancient & Medieval Japan: Environment**.

## Working rule

1. Read `AGENTS.md`.
2. Read this file from `main`.
3. Check OPEN / IN PROGRESS items before starting new work.
4. Put durable decisions into `Docs/Design.md` and implementation files.
5. Use this file only for handoff, status, blockers, and cross-repository coordination.

## Source of truth

Primary design source:

`Docs/Design.md`

## Status vocabulary

- **OPEN** — needs work
- **IN PROGRESS** — being investigated or implemented
- **BLOCKED** — waiting on a prerequisite
- **DONE** — completed and reflected in the proper source of truth
- **ARCHIVED** — retained for history only

## Current coordination items

### ENV-001 — Initial Japan-style world generation

**Requested by:** Environment/design  
**Owner:** Environment/worldgen  
**Status:** IN PROGRESS

Establish the first playable world-generation baseline for a Japan-like environment:
- bounded Japan-oriented temperature range;
- Japan-like elevation and hilliness distribution;
- increased small/medium river frequency;
- no naturally generated Large/Huge rivers;
- more coastline through more indented coasts, bays, peninsulas and islands rather than simply reducing land area.

The authoritative Alpha targets are in `Docs/Design.md`.

River XML and the first C# world-generation prototype are now implemented in source:
- Creek 30,000 / 0.70 and River 75,000 / 0.80;
- Large/Huge natural spawning disabled while their Defs remain available;
- Vanilla Terrain world-gen worker replaced by an Environment subclass that runs Vanilla first, then applies Japan-oriented elevation/coastline, hilliness, annual-temperature and rainfall transforms and reselects the biome;
- seasonal amplitude uses the Environment 8–16 C curve on the root surface;
- separate daily random variation is reduced to 4/7 of Vanilla on the root surface;
- Dev Mode diagnostics automatically log terrain/coast/hilliness and river statistics;
- Harmony is the sole technical dependency;
- `run-tests.bat` builds the DLL and validates the installed RimWorld 1.6 source-Def assumptions.

**Local build finding:** first build attempt failed at `WorldGenStep_AMJEnvironmentTerrain.cs` because `PlanetLayer.GetTileCenter()` exposes RimWorld 1.6 API metadata using `Unity.Mathematics.float3/int3`, while the legacy build script referenced only Assembly-CSharp / UnityEngine.CoreModule / netstandard / Harmony. This is a build-reference issue, not a worldgen logic failure. `build.bat` and the SDK project now reference both `Unity.Mathematics.dll` and `Unity.Collections.dll` from RimWorld's Managed directory. Fix commits: `c122e9c065c1dce49d7c2bc8413eecbea805c5d8`, `3646a352a9b6a78802db4bad61cd2c7f627df73b`.

**Next action:** run `run-tests.bat "D:\SteamLibrary\steamapps\common\RimWorld"`. If the build/static gate passes, launch RimWorld with Harmony + Environment, generate the first test world in Dev Mode, and use the two AMJ Environment summary log lines to tune the Alpha targets. Climate validation must then use actual temperature samples against the CCTO/AMJ 10/8/5/0 C growth thresholds and -1/-4/-8 C cold-death reference thresholds, not annual mean alone.

**Result / references:** initial design `9f5fd58c77b8e9ae5bad00851189d0127a122925`; CCTO calibration `e93da687fcd543f6d3ec94d5398fc604c0559749`; river patch `40d2b6b5615ea26ac6d91ee10f2433e3bd474829` + compatibility hardening `8aee69c23726a08f72b101ccd22a1e2065846364`; terrain prototype `8080b41f144fbacbf31411e11b7853860cd5703b`; climate hooks `16d870ee74a8e259de8233bfc84148ae48cfcf4d` / `b8b84723adf254b3f1bbed7a75cce5223a16222e`; build/static gate `aa87755d6099e89fda36de40acf358fd9bfebb68`, `a1da1cf981d22239f1833765835106804636814c`, `3d5fb2e52058f7d518b56e98620de3ee2af92fc4`; diagnostics `3403742e6c57d89c611cb94f338fff2ff15ef647`.
