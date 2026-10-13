# Hannoki / Environment Windows native verification — 2026-10-13 JST

The author requested native tests on the available Windows machine while
deferring Linux migration. No persistent Linux host was provisioned, no
Chat-local Linux build was resumed, and no Steam publication was performed.

## Tested source and preservation

- Source: `094d81f1fc9f32d621f7aaa3dbe97416997fa84b`, cloned into a dedicated
  Windows test checkout. The ordinary installed Environment's tracked source
  remained on clean `b08f60c7eab69b32f3570b494f71115d341dc940`.
- Native game: installed Windows RimWorld 1.6.4871; `Version.txt` rev590,
  actual Unity log rev591.
- Production DLL SHA-256:
  `eef5e8d9b617dd6cbb925b4cfcaf2490fa01f00228ba8f8db7049ce985355e59`.
- Exact candidate: 51 files; archive SHA-256
  `4a5f96a2fc75d6ee1bef1b32243923209b1908a9976d8ca80b277abf4cac9ae3`.
- Builder rebuilt the production DLL from the clean pinned source. Existing
  author-approved Workshop preview bytes were reused.
- Each temporary candidate changed only About identity to the established
  `sucro.ancientmedievaljapan.environment.releasevalidation` package. Root/DLL
  provenance was independently checked. All candidate runtime files remained
  identical, and the original payload and normal ModsConfig/Prefs hashes were
  preserved. All owned fixtures were retired after each gate.
- Native execution used private Windows desktops with Direct3D 11 rendering;
  no `-nographics`, normal desktop switch, normal save edit or Workshop edit.
- A later read-only inventory found eight new untracked DDS files in the
  ordinary installation's Hannoki/Suge/Yoshi texture folders, timestamped
  2026-10-13 22:22:21 JST. Their creating process was not established; these
  files were preserved and excluded from this tested candidate. Do not infer
  whole-install byte preservation from the verified tracked-source and normal
  config preservation gates.

## Actual test items and premises

| Item | Assumed specification and observed test |
|---|---|
| Nine map scenarios | Four AMJ climate forests, TemperateSwamp, ColdBog, natural-world wetland distribution, river and coast; each generated independently with the existing fixed seed/scenario premises |
| Hannoki loaded values | Production tree, growDays 20, harvestWork 800, harvestYield 24, Wood tag; exact loaded owner and graphics, independent of the disposable yield=1 probe |
| Wetland commonality / occurrence | TemperateSwamp 0.70, ColdBog 0.45; strictly positive naturally generated plants counted before test spawning |
| Native cutting | Actual colonist CutPlant, Plants skill 0, cropYieldFactor 1 restored afterwards; tree removed, exactly 24 actual loaded wood items, no unexpected output |
| Resource profile | Vanilla/CCTO WoodLog; MO/MO+CCTO DankPyon_RawWood with the currently loaded MO wood-chain setting |
| Cold configuration | Without CCTO: min growth 0 C / no CCTO extension. With CCTO: 5 C / exactly one coldDormancy extension. This checks loaded settings, not full winter recovery |
| Graphics | Normal, leafless, leafy snow and leafless snow resolve non-BadTex. This is loaded-content evidence, not seasonal visual acceptance |
| Natural world | All ten rejected Vanilla biome identities zero across all surface tiles; AMJ bands and native wetland availability retained. Each seed need not contain both wetland subtypes |
| Regional sowing | Existing forest scenarios check the exact research-locked/unlocked regional tree options and Haimatsu exclusion. This does not approve final Hannoki sow-menu behavior |
| Five-tree cutting audit | Shii 42, Beech 40, Shirabiso 30, Haimatsu 8, Hannoki 24; loaded resource plus real native output, and six retained description checks: 17 assertions |
| RimTest | Ten loaded-assembly tests: temperature bands, biome eligibility, 128 real production-selector/Worker combinations plus ocean, temperature/elevation bounds and mountain ordering |
| Pickle | Five existing scenarios per profile: exact source/DLL, unique structural-plant ownership plus implemented wetland cold settings, vegetation exclusions, tuned commonalities/MO herb preservation, loaded wood contracts |
| Strict acceptance | Expected scenario identities/counts, every assertion true, no timeout, preLaunchErrors 0, logErrors 0, captureLive true, logTruncated false, independent Unity capture ready and ERROR-free; exact source/provider/profile/render checks |

## Independently inspected results

| Profile | Map assertions | Five-tree cutting | Reports | Runtime ERROR |
|---|---:|---:|---:|---:|
| Vanilla | 440/440 | 17/17 | 10 | 0 |
| MO | 463/463 | 17/17 | 10 | 0 |
| CCTO | 488/488 | 17/17 | 10 | 0 |
| MO+CCTO | 511/511 | 17/17 | 10 | 0 |

RimTest: **10/10**, exact candidate DLL provider and ERROR gate PASS.
Pickle: **5/5 in each of four serial profiles**, full isolated logs and ERROR
gate PASS. The saved full-map/cutting results were independently re-audited
with `Combine-WorkshopPayloadResults.py`; payload/config preservation PASS.
Windows did not reproduce the two historical Linux MO FMOD errors; their
Linux cause and strict failure remain unresolved and are not relabelled PASS.

| Profile | TemperateSwamp natural Hannoki | ColdBog natural Hannoki | Cut in each wetland |
|---|---:|---:|---|
| Vanilla | 2,855 | 1,236 | WoodLog 24 |
| MO | 2,727 | 1,255 | RawWood 24 |
| CCTO | 2,856 | 1,240 | WoodLog 24 |
| MO+CCTO | 2,738 | 1,381 | RawWood 24 |

These counts describe the existing scenario maps, not representative regional
frequency estimates or an additional-seed survey.

## Harness corrections and reproducibility

The first candidate attempt stopped before game launch because the staged
PowerShell runner lacked its new `RimWorldRuntime.ps1` dependency. Its failed
outcome and preservation evidence remain retained. The fresh corrected run
used unchanged candidate bytes. No production Def, gameplay or image changed.

The existing release runner/stager now includes the shared runtime helper,
retains per-profile console diagnostics, validates production or About-only
candidate ownership explicitly, and expects the seven added production
Hannoki wetland assertions and all five native cutting outputs. The independent
auditor uses the same five-tree contract. The existing development Pickle
runner also validates the supplied root's declared identity; test steps accept
only production or the existing candidate identity and still require exact
root/DLL/Def provenance. Negative checks reject unrelated/missing identities,
incomplete reports and ERROR-bearing results.

Procedure: [RenderedRuntimeTests](../GoldenPaths/RenderedRuntimeTests.md).
Local canonical raw evidence root:
`C:/Users/sucRo/.codex/.chatgpt-projects/g-p-6abf6039d3d08191b279b8154951a150/EnvironmentNativeTest-20261013/TestResults/`:

- `NativeCandidate/Manifest.json` and exact extracted candidate;
- `FourProfileNative/`: original failed attempt;
- `FourProfileNative-Retry/`: all 40 JSON/full logs, independent Unity captures,
  Before/Preservation/Summary and retired fixtures;
- `FourProfileAudit.json`: independent strict re-audit;
- `NativeRimTest/`, `RimTestCandidateRetired/`;
- `NativePickle/`, `PickleCandidateRetired/`.

The temporary local adapter used for the initial execution is retained under
TestResults; its candidate/ownership/count/dependency corrections are now in
the existing canonical scripts. It is not a new production runner.

## Still unverified / not claimed

Hannoki's seven human visual states remain pending. Actual seasonal timing,
snow depths 0/0.5/1 and overlay alignment, immature/icon readability, CCTO
dormancy recovery, final Hannoki sow-menu behavior, additional seeds, MO
wood-chain OFF, real old-save load/re-save/add/remove and Steam downloaded
payload acceptance are not established here. This run is Windows 1.6 only;
the separate historical 1.7 evidence retains its own revision and scope.
The standard `run-tests.bat` was not run as one command against this new
candidate; optional Core/Grains integration and disposable developer wetland
probes are not claimed from the subscriber observer matrix. Full release/art/
save readiness and Steam correction remain HOLD.
