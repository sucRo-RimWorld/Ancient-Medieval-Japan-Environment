# AMJ Environment — isolated Linux 1.6 current-main candidate build

**Date:** 2026-10-15 JST  
**Scope:** source build, exact payload, real 1.6 Vanilla nine-map Quickstarts, four-provider natural-world biome checks and four-provider native cutting checks.
**NOT** a complete four-provider strict ERROR0 full-map matrix, Steam subscriber, private publication, save/season or preview-visual approval.

## Authoritative evidence

- Environment source: `4dacbcf0fbe3399ee44795f3097b8cbe498d0ae3` (clean fixed `main`).
- Project host source: [PR #260](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/260), merged as `bcb46071ed1eb6eefaabee0b50590898309f8913`.
- Fixed native-host command: `/amj environment-candidate-16` in private Project Issue #39.
- [Actual workflow run #38681874743](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38681874743) **SUCCESS**. Private isolated host game reference `1.6.4871 rev598`; C# production build uses actual local managed references and verified Harmony.
- [Artifact #11948751150](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38681874743/artifacts/11948751150), retained by GitHub Actions for seven days: `AncientMedievalJapanEnvironment.zip`, exact extracted payload directory, external `Manifest.json`, `result.json`, `static-tests.log`, `build-output.log`.

## Results

| Gate | Observed |
| --- | --- |
| Environment static package / release negative tests | PASS |
| Fresh pinned source net472 production DLL build | PASS |
| Exact extracted payload validation | PASS, 59 subscriber files |
| Canonical subscriber root loader, missing state checks | PASS |
| ZIP CRC, file-by-file byte identity and external manifest | PASS |
| Native RimWorld 1.6 Vanilla original nine map Quickstarts | **PASS 466/466, 9/9 scenarios, ERROR 0** |
| Four-provider unforced natural-world biome assertions | **24/24 each; scoped functional PASS x4**, Vanilla/CCTO ERROR0, MO/MO+CCTO each FMOD2 strict FAIL |
| Five-tree actual pawn cutting | **17/17 each in Vanilla/CCTO**, MO/MO+CCTO output correct but FMOD2 strict FAIL |
| Old-save migration/season or stable full play loop | **NOT RUN**, not required for forward-only biome correction; separate stable criteria remain |
| Downloaded Steam Workshop `3814638060` | **NOT UPDATED OR VERIFIED** |
| Preview image author acceptance | **NOT VERIFIED** |

Production assembly SHA-256:
`29c41ebf8998db993ac496d7d5800b9ae39fec613490dee8da6a721cad702996`

Generated ZIP SHA-256:
`4b7707284f1b5a9d404962d21869e5914ea70aeb842951d0d45c395678e4b7c3`

The selected source-tracked artwork was `About/preview.png`, repackaged as canonical `About/Preview.png` to avoid case-insensitive duplicate filenames. The PNG is **not yet asserted** to match separately accepted Workshop preview bytes. Neither replacing image pixels nor visual author sign-off occurred.

## Boundaries and next acceptance

The host's existing pinned `AMJ_Environment` test Mod is **older source** and was not modified. This candidate was constructed in separate temporary source/build space and is archived as an artifact only. Earlier Windows 4-profile candidate tests and Linux older-source Quickstarts **do not** establish the new 59-file current-main release's native performance. Complete same-candidate native MO/CCTO/MO+CCTO map/real-cutting matrix with ERROR=0, plus requisite season/save/visual checks. Next private-only publication (if authorized and supported) must validate the exact physical upload source, then the actually downloaded bytes and replay separate runtime gates. Do not infer Workshop update from Git or from this build.

The 2026-10-09 manual/YADA text in `Docs/GoldenPaths/WorkshopPublication.md` needs reconciliation with the newer author direction to publish through GitHub Actions; no private publisher implementation/credentials were asserted by this build.

## 2026-10-15 native Vanilla rendered Quickstarts — confirmed

The Project [PR #274](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/274) was merged as `cd957c65139f7a2e178cd69b5a668304bf933c20` and introduced a guarded exact-candidate Quickstart harness. Both tests independently fetched the above **same 59-file candidate artifact**, checked its external manifest, ZIP SHA-256 and DLL SHA-256, built a disposable **full copy of the licensed 1.6 game** inside the isolated host's scratch root, and replaced Environment **only in that disposable game**, never the original staged Mod. Original tests from the pinned author Environment commit were compiled against native managed game libraries and executed under Xvfb with full rendering. The original host game reports `Version.txt 1.6.4871 rev598`; the actual launched RimWorld/Unity log reports `1.6.4871 rev600`; preserve both observed strings rather than concealing the distinction.

| Tested module | Native result |
| --- | ---: |
| WarmTemperateTerrain | 80/80 |
| CoolTemperateTerrain | 73/73 |
| SubalpineTerrain | 70/70 |
| AlpineTerrain | 70/70 |
| TemperateSwampVegetation | 70/70 |
| ColdBogVegetation | 73/73 |
| WorldWetlandDistribution (unforced generated world) | 24/24 |
| RiverMapHandoff | 3/3 |
| CoastMapHandoff | 3/3 |
| **Total** | **466/466** |

- [First four Quickstarts, run #38686699729](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38686699729): **PASS**, four real game launches, 293/293, each exit 0 and native ERROR 0, complete live capture, no timeout. [Artifact #11949943033](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38686699729/artifacts/11949943033).
- [Remaining five Quickstarts, run #38686772083](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38686772083): **PASS**, five real game launches, 173/173, each exit 0 and native ERROR 0, complete live capture, no timeout. [Artifact #11950188346](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38686772083/artifacts/11950188346).

**Acceptance boundary:** This is the **Vanilla profile only**, scoped to nine rendered map generation Quickstarts including natural-world biome exclusion. It is not the full four-profile ReleaseCandidate/WorkshopPayloadTests gate, no ten-scenario native cutting scenario, no loaded subscriber Steam path, no human visual approval and no save/season acceptance. Preserve HOLD. Project coordination and source hosting are outside this owning evidence document.

## 2026-10-15 original native CutPlant checks across four real provider profiles

This work **reuses** the same source/DLL and 59-file archive described above,
without editing Environment production XML or the user's game/saves. Fixed
Project Issue #39 commands were implemented in
[Project PR #283](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/283),
merged as `3ce4dc1e8264f65b32a469a82735fb305f4fe0ae`.
The original Quickstart's production native CutPlant probe spawns a real pawn
and each of five actual production AMJE plants (no fake harvest Def). Each
plant is cut to completion and its only resulting resource/count is checked.
Both tests run within a disposable full game copy, preserving old host
owner-staged Mods, Steam and saves.

| Plant | Expected output, at crop yield factor 1 | Base loaded resource |
| --- | ---: | --- |
| `AMJ_Tree_Shii` (スダジイ) | 42 | WoodLog or MO-selected DankPyon_RawWood |
| `AMJ_Tree_Beech` (ブナ) | 40 | same |
| `AMJ_Tree_Shirabiso` (シラビソ) | 30 | same |
| `AMJ_Shrub_Haimatsu` (ハイマツ) | 8 | same |
| `AMJ_Tree_Hannoki` (ハンノキ) | 24 | same |

The original cutting-only Quickstart emits **17** checks per profile, not 11.
The first [base run #38689111207](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38689111207)
had correct five tree/resource/quantity observations, actual game exit 0 and
Unity ERROR0 but **workflow FAIL** because the wrapper incorrectly expected 11.
Project [PR #285](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/285),
main `51e6c96d597cb023e642c6ea05ec9c6672845edf`, corrected the wrapper
to the owner-approved 17 assertions. Do not re-label the original failure PASS.

| Actual independent run | Native five trees | Assertions / Unity ERROR | Overall |
| --- | --- | --- | --- |
| [Vanilla+CCTO #38689421756](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38689421756) | Vanilla five WoodLog counts correct; CCTO five correct | **17/17 each, ERROR 0 each, game exit0** | **PASS/PASS** |
| [MO #38689626203](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38689626203) | Five original RawWood counts correct | 17 reported, exit0, **2 Unity FMOD unsupported audio-format ERRORs** | **FAIL strict ERROR0** |
| [MO+CCTO initial #38689992750](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38689992750) | No Quickstart executed | Early exit139, no assertions/live capture | **FAIL; preserved** |
| [MO+CCTO focused retry #38690112481](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38690112481) | Five original RawWood counts correct | 17 reported, exit0, **2 Unity FMOD unsupported audio-format ERRORs** | **FAIL strict ERROR0** |

The focused single MO+CCTO command avoids rerunning already failed MO.
[Project PR #287](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/287)
merged as `d9f7d9cbe54bd11b465e991cdd148be7502058fc`.
No upstream sounds or logs were changed/suppressed. The identified FMOD
startup errors also occur in other real-MO owner tests; audio remediation
belongs to the Project shared native/FMOD workstream. The author has explicitly
classified this exact independently reproduced external error pair as non-blocking
for Environment's private functional-release decision under `Docs/ReleaseReadiness.md`;
it still strictly fails the original global ERROR0 suite. No actual full four-profile
ERROR0 release PASS is claimed. Remaining release gates:
accepted preview, appropriate season/state checks,
source-specific Workshop upload/download and subscriber runtime verification.

**Legacy save scope:** This biome-selection correction is forward-looking
world generation; it does not rewrite existing planet tiles. An exhaustive
old-world seasonal conversion/migration run would test an unimplemented
promise. No destructive Def renames/deletions were part of this pinned
candidate. Extensive old-save compatibility tests are not required for
this specific change; a narrowly scoped load smoke is warranted only if
future changes remove/rename referenced Defs. The separate future stable
new-save save/reload continuity requirement is not hereby proven.

## 2026-10-15 exact 59-file candidate — four real loaded-provider natural-world checks

This is the **same pinned implementation source, ZIP and production DLL** as the
above Vanilla nine-map and native cutting tests. The original Environment
`AMJWorldWetlandDistributionQuickstart` is run on the **unforced 30%-coverage
world** in real RimWorld Linux 1.6, using actual installed providers, with no
replacement of the isolated host game and no Steam upload.

Each profile checks the *independent* 24-label inventory:
7 natural world / wetland conditions (land >=2000, all assigned,
wetland candidates and naturally selected wetland exist, all wetland tiles
are valid candidates, wetland share <=20%);
all 10 prohibited base Vanilla biome identities are absent on **all**
surface tiles, including SeaIce; all 4 AMJ climate bands are present;
TemperateSwamp/ColdBog remain naturally placeable; and the MO DarkForest
coexists when loaded. Both wetland subtypes **need not simultaneously occur**
in the selected seed. Original native reports had 24 assertion PASS and
0 failed per profile; native game exit0 and complete live capture.

| Profile | Exact candidate native report | Unity errors | Gate classification |
| --- | --- | ---: | --- |
| Vanilla | [earlier full 9 Quickstarts #38686772083](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38686772083), unforced natural world **24/24** | 0 | strict ERROR0 + scoped functional **PASS** |
| CCTO | [#38693096172](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38693096172), **24/24** | 0 | strict ERROR0 + scoped functional **PASS** |
| MO | [#38693109757](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38693109757), **24/24** | 2 exact known FMOD | scoped functional **PASS**; strict ERROR0 **FAIL** |
| MO+CCTO | [#38693122496](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38693122496), **24/24** | 2 exact known FMOD | scoped functional **PASS**; strict ERROR0 **FAIL** |

The initial CCTO [#38692321756](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38692321756),
MO [#38692329465](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38692329465)
and MO+CCTO [#38692339487](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38692339487)
reports each contained all **24 original assertion PASS, exit0**, but a
separate wrapper incorrectly read `version_seen` only from launcher stdout
rather than RimWorld's actual Unity `-logFile`, giving a wrapper false
FAIL. Project [PR #297](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/297)
merged as `2b0033e6be81b90cb238b68617690839db4922c8`;
the corrected observer reads native Unity and retains independent launcher
supplemental errors. It has eight offline regression guards for new ERRORs,
wrong profiles, early crash, failed original assertions and missing version.
**None of the first failures has been relabelled PASS.**

The two exact audio messages retained in each later MO run are
`Cannot create FMOD::Sound instance for clip ""` /
`FMOD error: Unsupported file or audio format.`, already observed in
controlled tests without Environment. Raw Unity ERROR count is **2**, never
reinterpreted as zero. The governing owner exception is narrowly limited
to the **Environment-only private functional release gate**.
No sound fixes, test assertion removals or producer-asset edits were made.

**Release status:** Environment four-provider biome replacement **functional
PASS**, original global ERROR0 four-profile strict **FAIL** due known external
audio. The actual GitHub Actions private Steam publisher/verified owner state,
accepted preview/source match, downloaded subscriber provenance and
post-update smoke remain OPEN. No Workshop bytes or visibility were changed.
