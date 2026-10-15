# AMJ Environment — isolated Linux 1.6 current-main candidate build

**Date:** 2026-10-15 JST  
**Scope:** source build, source-static packaging audit, extracted candidate / ZIP identity.
**NOT a native game, subscriber, Steam upload, visual approval or four-profile acceptance.**

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
| Native loaded RimWorld / four profiles / save / season | **NOT RUN** |
| Downloaded Steam Workshop `3814638060` | **NOT UPDATED OR VERIFIED** |
| Preview image author acceptance | **NOT VERIFIED** |

Production assembly SHA-256:
`29c41ebf8998db993ac496d7d5800b9ae39fec613490dee8da6a721cad702996`

Generated ZIP SHA-256:
`4b7707284f1b5a9d404962d21869e5914ea70aeb842951d0d45c395678e4b7c3`

The selected source-tracked artwork was `About/preview.png`, repackaged as canonical `About/Preview.png` to avoid case-insensitive duplicate filenames. The PNG is **not yet asserted** to match separately accepted Workshop preview bytes. Neither replacing image pixels nor visual author sign-off occurred.

## Boundaries and next acceptance

The host's existing pinned `AMJ_Environment` test Mod is **older source** and was not modified. This candidate was constructed in separate temporary source/build space and is archived as an artifact only. Earlier Windows 4-profile candidate tests and Linux older-source Quickstarts **do not** establish the new 59-file current-main release's native performance. Run a same-candidate, isolated native Vanilla/MO/CCTO/MO+CCTO map/real-cutting matrix with ERROR=0, plus requisite season/save/visual checks. Next private-only publication (if authorized and supported) must validate the exact physical upload source, then the actually downloaded bytes and replay separate runtime gates. Do not infer Workshop update from Git or from this build.

The 2026-10-09 manual/YADA text in `Docs/GoldenPaths/WorkshopPublication.md` needs reconciliation with the newer author direction to publish through GitHub Actions; no private publisher implementation/credentials were asserted by this build.
