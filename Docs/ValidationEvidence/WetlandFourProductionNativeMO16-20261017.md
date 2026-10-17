# Wetland four plants — genuine MO and MO+CCTO RimWorld 1.6 acceptance (2026-10-17)

**Status: targeted native gameplay evidence, not strict ERROR0 nor publish acceptance.** Both source-pinned Project Actions below launched actual isolated Linux RimWorld 1.6 with real MO/VEF/SYR and Environment, with optional CCTO. All observed Unity prelaunch errors were exactly the previously independently isolated MO/SYR blank-clip FMOD error, two in each case; they remain **strict ERROR0 FAIL**, never globally PASS.

## First full 1.6 MO pair

[Project Actions #39038178612](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39038178612), exact gameplay source `ea22a4fcb0454630d85eb48c9e88604cedc97497`, fresh 60-file candidate, every native case launched and original results/logs kept.

| Profile | Native wetland map | Original functional assertions | Game exit | Unity prelaunch FMOD errors | Strict result |
|---|---|---:|---:|---:|---|
| MO | TemperateSwamp | **83/84 FAIL** (Yoshi real CutPlant removed mature plant but yielded Hay0) | 1 | 2 | FAIL |
| MO | ColdBog | 75/75 PASS | 0 | 2 | FAIL |
| MO+CCTO | TemperateSwamp | 84/84 PASS | 0 | 2 | FAIL |
| MO+CCTO | ColdBog | 75/75 PASS | 0 | 2 | FAIL |

All four cases had the same exact FMOD `Cannot create FMOD::Sound instance for clip ""` with unsupported format and no other distinct Unity ERROR. Original live wetland spawn/weight/graphics, production Defs, native tree output and real herb CutPlant semantics were checked. Except the one Yoshi stochastic Hay0 the remaining functional assertions passed. The previous disposable test-only herb PlantDefs had harvest failure disabled; production wild Yoshi/Suge deliberately retain normal failable harvest.

Source archive SHA256 `ff9fbf5375531c9dba2d767298687a99bbd0ff107ffe989d386616c2a7890d35`, DLL SHA256 `858a1d933e60c4da3a0295b97f9ad696b5ccf8bed136ed6cd15732d75191e4d5`.

## Focused one-map follow-up after test-only bounded retries

[Project Actions #39039584212](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39039584212), owner main `451faf80409730f3ef023ac0c454f9c44bb82b71` [PR #196](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/pull/196). **MO TemperateSwamp 68/68 real gameplay assertions PASS, native exit0; strict ERROR0 still FAIL due exactly two known external FMOD events**.

- Production Yoshi real native CutPlant removed mature specimen, Hay2 on first independent skill20 attempt.
- Production Suge real native CutPlant removed the first mature specimen but got Hay0; second independently spawned mature specimen produced Hay1. The failable game behavior is unchanged; observer merely permits up to three real tries and records the stochastic failures, never creates Hay itself.
- Same provider and two-baseline 1.6 map definitions loaded; no new Unity error types.
- ZIP SHA256 `41c004ccb554b5a91aded4bf728b1a49ac9a654a00cdb28cf9653c1c2e68880e`, DLL SHA256 `f884e725627cb3aa744abcbe4508a04eaf51c2a0764a6cfb95c8d7268f523c74`.

## Exact-payload and acceptance boundaries

**The three runs do not share byte-identical production DLLs/ZIPs, including the earlier [Vanilla/CCTO native 1.6 PASS](WetlandFourProductionNative16-20261017.md) despite matching 60-file inventories.** The payload manifests show unchanged non-DLL file digests, but compilation produced different DLL bytes. Thus functional coverage across the identical author-approved Defs and gameplay sources must not be mislabeled a **single exact candidate** four-profile publication PASS. A publishing candidate must still prove the same exact staged payload and its download/installed equivalence, with the separately authorized known-FMOD functional exception applied only after case evidence satisfies the owner policy.

New 1.7 actual wetland behavior, seasonal persistence and new-save load/reload continuity remain separate OPEN gates. No version bump, Steam update or subscriber validation from these runs.
