# Wetland four species: genuine RimWorld 1.7 source-loaded acceptance (2026-10-17)

**Result: PASS for the scoped 1.7 plant behavior, NOT a full 0.2.0 release candidate.**

- Original real-game [Project Actions #39041272179](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39041272179) concluded success. Original artifact `amj-environment-native17-39041272179` (ID `12104755235`), with `result.json`, `summary.json`, original game launcher log and compiler logs.
- Exact immutable Environment gameplay source: `ea22a4fcb0454630d85eb48c9e88604cedc97497`; genuine Linux **RimWorld 1.7.4977 rev1248** with Waylith, Pickle, Quickstarts and Harmony, isolated test profile. Environment's nine original C# source files compiled for the actual 1.7 managed assembly / UnityStubs. Compiled native DLL SHA256 `20077f9c9c82394d7e3f7cbb145ccd9d7e4064e84af14ae89538aec86dff770b`.
- **Pickle 1/1 PASS**, no failure, skip or flakiness, game exit 0, timed_out false, native_error_count **0**, FMOD error count **0**, original game unmodified. Screenshot/manual color matching not claimed.
- The actual game on a normal 50×50 map confirmed owner definitions `AMJ_Plant_Yoshi`, `AMJ_Plant_Suge`, `AMJ_Plant_Mizugoke`; inspectable nonfood, wild-only traits and source-defined growth/hay yields; approved `TemperateSwamp`/ `ColdBog` plant commonality in the *loaded BiomeDefs*. Real Plants20 pawn CutPlant removed specimens, with **Yoshi Hay2, Suge Hay1, Mizugoke Hay0 and no other item**.
- Actual launcher marker: `AMJ_WETLAND17_RESULT passed=True source=production weights=6 yoshiHay=2 sugeHay=1 mizugokeHay=0 game=1.7`; Pickle says scenario passed. See archive logs for complete original context.

**Explicit untested scope:** the map was normal 50×50, so this result is **not** a natural 1.7 TemperateSwamp/ColdBog spawn-and-density pass, nor 1.7 MO/CCTO combination compatibility, seasonal cold/dormancy/death, existing save and reload continuity, full world-generation pass, subscriber byte identity or Steam update. These are separate gates, not absorbed by a one-scenario PASS.

The already-accepted 1.6 Vanilla+CCTO two-bog matrix is in [WetlandFourProductionNative16-20261017.md](WetlandFourProductionNative16-20261017.md). Actual 1.6 MO/MO+CCTO functional results and strict FMOD exception are in [WetlandFourProductionNativeMO16-20261017.md](WetlandFourProductionNativeMO16-20261017.md). **The original compiled 1.6/1.7 DLL and test payloads are not byte-identical Steam subscriber candidates.** Keep the 0.2.0 release/Workshop readiness HOLD.
