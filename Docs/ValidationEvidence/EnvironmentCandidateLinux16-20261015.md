# AMJ Environment — isolated Linux 1.6 current-main candidate build

**Date:** 2026-10-15 JST  
**Scope:** source build, exact payload audit, and native Linux 1.6 Vanilla nine-map Quickstarts.
**NOT a native four-profile, subscriber, native-cutting, Steam upload, save/season or visual acceptance.**

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
| Native MO/CCTO/MO+CCTO, cutting, save and season | **NOT RUN on this candidate** |
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
