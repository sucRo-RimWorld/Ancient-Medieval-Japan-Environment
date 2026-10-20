# Current four-plant release readiness — 2026-10-06 JST

## 2026-10-20 — existing Workshop 3814638060: public update accepted

Author explicitly accepts the risk that **0.2.0 changes become public**
when updating the existing Workshop item `3814638060`. Keep the item
and its current visibility unchanged; **do not create a second private
item** or forcibly switch visibility. The release candidate's
`Publishing/SteamWorkshop.json` uses `preserve_public`,
rather than superseded `private_only`. Project registry and
central approval command `/approve-steam-existing-update`
must match this policy. Owner/Steam account identity, Linux/Windows
host settings and the final explicitly authorized Steam update remain
unverified and blocked. Existing 1.6 native DLL, accepted 60-file
subscriber contents and gameplay acceptance are unchanged. Neither
the release PR nor Steam update is authorized merely by this decision.

## 2026-10-20 — publisher method moved to the private Project; Steam HOLD

The author selected **centralization before uploading**. Private
`sucRo-RimWorld/Ancient-Medieval-Japan-Project` now owns the single
`main:.github/workflows/steam-central.yml` workflow, separately recorded
GitHub author Issue approval comment and trusted SteamCMD/Steamworks
orchestration; the Environment owner retains only exact native/release
content, verified source and its package assembly code. Per-Mod
`steam-approved.yml` and `steam-readiness.yml` are **retired in this
unmerged Environment candidate**, not on Environment main until promotion.
The fixed source and originally successful [offline release CI
#39277376438](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/actions/runs/39277376438)
are existing evidence, not Steam publication or central publisher E2E proof.

The Project registry needs the new **final Environment release candidate
SHA** when this branch's cleanup is complete (metadata change only; same
tested DLL/source). Do not launch a central Project author publish issue
before that pin is consistent. Independent remaining actual blockers are
the separately posted real author `/approve-steam-private` approval comment
on the exact-main Project release Issue, plus group4 permission to
**Project private repo and its one exact main workflow**,
Linux/Windows host accounts and item state, verified private Workshop
ownership and real post-upload subscriber acceptance. Existing author
RimWorld 1.6 Thin Soil manual PASS, initial native-run errors and MO/SYR
FMOD baseline remain independently recorded. **0.2.0 publication HOLD.**

## 2026-10-20 — immutable public native transport: offline package verification PASS, Steam HOLD

The private Project Actions artifact reader secret is **no longer a prerequisite
on unmerged release PR #217**. The original pinned Project build artifact
`39247372678` (60 native subscriber files, `fe6861ac...` gameplay source,
DLL SHA-256 `b2ddbf7888322ba9b207c20296dba31175b846c05492902b900ec225da9fcffe`)
was previously downloaded and ZIP/manifest/DLL/preview-verified in
[owner test-branch Actions #39258628883](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/actions/runs/39258628883).
The exact DLL is now retrievable by immutable public test-branch **commit**
`5dac3140230402b3b2aa96cbc117b98a23745dde` and Git **blob**
`b8e3766d5ba1dde28e1bbb9e746ffcb7e7d390a8`; the SHA-256,
unchanged source and entire original 58 source files are independently
checked in the new publication preflight. Only the three release metadata
files and generated PublishedFileId can legitimately differ.

Full source-verified 0.2.0 subscriber package was built **offline** in
[Environment Actions #39276858703](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/actions/runs/39276858703)
without a Project read secret, game launch or Steam upload. The original
Project run FAIL and exact FMOD raw errors remain intact, with the user-
approved unchanged baseline non-blocking only for Environment private
functional acceptance. The author's 1.6 Thin Soil/Nice Plant Menu check
remains scoped manual PASS; no user retest needed for this same change.

**Still mandatory:** separate real GitHub author issue-comment approval
from `sucRo0629` (`/approve-steam-private` on the original author Issue),
the Environment-specific Linux/Windows Steam host accounts/state/runner
allowlist, privately owned existing item ID and visibility check, then a
separately authorized private Steam update and downloaded-subscriber smoke.
No evidence proves these gates yet. **PR #217 OPEN; no live upload.**
Old sections below identifying `AMJ_PROJECT_ARTIFACT_TOKEN` as blocking
reflect the superseded 2026-10-19 transport, not the current PR design.
See `Docs/GoldenPaths/WorkshopPublication.md` for the full operational
provenance/security contract.

## 2026-10-19 — author direct 1.6 Thin Soil gameplay acceptance: scoped PASS

The author pulled the **actual playable test branch** [`test/environment-020-playable-20261019`](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/tree/test/environment-020-playable-20261019) (commit `5dac3140230402b3b2aa96cbc117b98a23745dde`), which carries the genuine production **RimWorld 1.6** DLL SHA256 `b2ddbf7888322ba9b207c20296dba31175b846c05492902b900ec225da9fcffe`; the import action [#39258628883](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/actions/runs/39258628883) recorded hash verification and successful commit. The author checked the previously requested three in-game behaviors (Thin Soil 0.50 Growing Zone paint, compatible crop sowing versus 0.70 rice/wheat, and inappropriate/occupied placement rejection) and reported **“問題なし”**. Separately, the optional **Nice Plant Menu** correctly displayed an **insufficient-fertility warning**. Mark **author-reported manual RimWorld 1.6 scope PASS**; no screenshot or independent per-cell diagnostics were submitted. Do not infer a full growth/harvest lifecycle, RimWorld 1.7 zone interaction, or Steam subscriber delivery.

The original source [native Project #39247372678](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39247372678) remains **workflow FAIL** due to original Vanilla warm crash and MO/SYR FMOD. The same exact DLL's supplemental [Vanilla rerun #39250184421](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39250184421) passed warm **82/82**, cold **73/73**, clean exit and ERROR0; combined with the other original seven cases this establishes **628/628 functional assertions**, not global strict ERROR0. The previously approved *specific unchanged FMOD baseline* is non-blocking for Environment-only private functional acceptance; no new errors/crashes are waived. There is **no need to repeat** the completed manual Thin Soil 1.6 check solely to reconfirm it.

**Release still HOLD:** PR #217 is unmerged and Steam unchanged. Independent blockers: `AMJ_PROJECT_ARTIFACT_TOKEN` for private Project artifact transport, actual required author-approval record, Environment Linux/Windows publisher-host authorization, verified private Steam ownership/visibility and downloaded subscriber acceptance. The new native artifact expires 2026-10-26 UTC; a valid final pin must be used if the artifact has expired. Do not conflate test-branch source+D​​​​LL acceptance with a published Workshop release.

## 2026-10-19 — Thin Soil current-source native build / supplemental evidence; release HOLD

**Source:** Environment `fe6861ac490bbd43ec73dce8efc1c2a5e5140479`, including `ThinSoilGrowingZonePatch.cs`. Genuine Project [1.6 native build/run #39247372678](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39247372678) produced 60 subscriber files and a net472 production DLL with **0 build warnings/errors**. Original source candidate ZIP SHA-256 `86c433992e21366d026c468b8a7a73142abeb2c44c1f51f74afaafa8977341fe`; DLL SHA-256 `b2ddbf7888322ba9b207c20296dba31175b846c05492902b900ec225da9fcffe`; original Project artifact ID `12184871110`, name `amje-wetland16-39247372678`. The prior source pin `5050e572` and compiled DLL are superseded for prospective new publication, but remain historical evidence.

**Original native 1.6 result remains FAIL:** Vanilla warm crashed in Mono GC with SIGSEGV exit139 before Quickstart report; CCTO warm/cold 82/82 + 73/73, Vanilla cold 73/73, MO and MO+CCTO warm/cold 84/84 + 75/75 assertions all passed. All four MO-containing cases retain exactly two upstream MO/SYR blank-clip FMOD prelaunch Unity ERROR each (8 raw errors). Strict original action/error-zero FAIL is **not** waived. No new non-FMOD Unity ERRORs were seen in seven completed cases.

**Same exact source/archive/DLL supplemental native Vanilla result:** [Project Actions #39250184421](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39250184421) downloaded the immutable original `39247372678` artifact, verified source/ZIP/DLL hashes and ran original normal warm **82/82** and ColdBog native cutting **73/73**, each exit0, Unity ERROR0, untruncated evidence. An earlier [#39249695255](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39249695255) was harvest-only **66/66** due to a focused scenario selection confound; it is a distinct narrow PASS, not the missing normal warm evidence. Taken together, eight original 1.6 wetland cases across the four profiles have **628/628 gameplay assertions** on one byte-identical DLL with clean exits in the final corresponding cases, subject only to the explicitly recorded original FMOD failures. This **does not** change original run's FAIL, global strict ERROR0 FAIL or replace full final release map/zone tests.

**Current source's genuine RimWorld 1.7 evidence:** [Project Actions #39249306881](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39249306881) built the same source for real Waylith 1.7.4977, ran a 50×50 wetland map/cutting Pickle 1/1 PASS, Unity ERROR0, FMOD0, exit0. This is one focused 1.7 scenario, not broad world/zone or save acceptance.

**CRITICAL UNTESTED:** The new Thin Soil Growing Zone Harmony postfix is included in both actual compiled sources, but no loaded-game test has attempted to draw a growing zone on Thin Soil, reject covered/fogged/occupied cells or sow a Grains 0.50-minimum crop while rejecting 0.70 rice/wheat. That is still required for acceptance of the player-reported defect; compilation and unrelated wetland assertions do not prove functionality.

**Publisher still BLOCKED:** This revised 0.2.0 PR stages the new immutable original Project artifact pin and matching private-source download in both publisher and read-only check workflows; the Environment repository's `AMJ_PROJECT_ARTIFACT_TOKEN` is still absent, the author-approval reviewer configuration/record is unverified, and Environment's Linux/Windows publishing hosts, actual Workshop item ownership/private visibility, uploaded bytes and downloaded subscriber remain unverified. No Steam upload, publication or downloaded-subscriber test. Formal publication HOLD unchanged; artifact expiry 2026-10-26 UTC.

## 2026-10-19 — actual author-only read-only publisher gate: BLOCKED, release HOLD

Environment [PR #220](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/pull/220) installed a separate read-only publisher readiness path; [PR #222](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/pull/222) fixed its first SHA-guard syntax error. Exact trusted main at the second author-run: `3c8a47ceaed98d7d4e9250642f46bbcbcdea106e`. The real [Actions #39243327351](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/actions/runs/39243327351) proves:

- **Guard: PASS** for the exact author-created issue `#223` and main SHA.
- **Private Project token: BLOCKED.** Actual GitHub Actions output explicitly reports `BLOCKED: private Project artifact reader not configured`; the `AMJ_PROJECT_ARTIFACT_TOKEN` secret is absent or empty in this Environment workflow. No Actions cross-repository download was attempted. The earlier GitHub-connector download is NOT this proof.
- **Protected author approval: BLOCKED.** `steam-workshop-private` job ran but GitHub returned no recorded approval for `sucRo0629` on this run. Independently confirm the repository Environment **required-reviewer rule**, self-review setting and author approval UI; source-level approval guards alone do not establish configuration.
- **Linux and Windows hosts: SKIPPED** by the safety dependencies, so Environment's allowlist membership, separate Linux SteamCMD state/owner identity/account and Windows Steamworks session/receipt have **no runtime acceptance**. Previously documented shared Linux publisher group allowed only CCTO-for-VPE and LWM-s-Fuel-Filter-Fix; Environment access remains unverified.
- **Real publication workflow: SKIPPED** for this read-only issue. No SteamCMD update, new Workshop item, Steamworks tag change or visibility mutation occurred.

**Separate, decisive native binary provenance change:** latest main after [PR #218](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/pull/218) adds production `Source/AncientMedievalJapanEnvironment/ThinSoilGrowingZonePatch.cs` (51 new lines). GitHub compare `5050e57250c6913c2210be47723fee68d318e59e..3c8a47ceaed98d7d4e9250642f46bbcbcdea106e` detects this actual Source difference. Existing `Publishing/VerifiedAssembly.json` and Project run `39200796636` pin the **older** source; the current importer correctly rejects changed gameplay sources once the artifact is downloaded. The previously assembled 60-file local 0.2.0 ZIP is therefore **not** an exact-current-source DLL release candidate. Recompile against genuine game assemblies, create a new immutable native-tested source/DLL/ZIP pin and run bounded affected acceptance before attempting publication; do not waive Thin Soil functionality or reuse the previous DLL silently.

The [unmerged copy/version PR #217](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/pull/217) remains prepared only, not current main or Steam. **HOLD** until the token/reviewer/Environment runner ownership, newly pinned DLL and correct candidate, author-confirmed live item identity/visibility and actual downloaded subscriber check are independently established. Original 1.6 wetland 628/628 functional assertions and strict MO/SYR ERROR0 **FAIL** remain unmodified historical evidence.

## 2026-10-19 — source-pinned DLL publisher transport integrated, operational acceptance OPEN

Environment [PR #213](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/pull/213) integrates pinned original 1.6 native DLL artifact import into the author-only Steam Actions preflight; [publisher transport procedure](GoldenPaths/WorkshopPublication.md#2026-10-19-pinned-020-production-dll-transport-prepared-not-publisher-verified) specifies exact pin/source/fingerprint. **This is source implementation only**. No publisher workflow run, successful private-Project artifact-token authentication, final 0.2.0 stage, live Steam upload, or subscriber download is established. The pinned artifact is temporary and expires 2026-10-26 UTC. Steam readiness remains **HOLD** while these concrete dependent gates and final 0.2.0 version/copy promotion are incomplete. Preserve the separate original 1.6 628/628 functional PASS and strict MO FMOD ERROR0 FAIL below.

## 2026-10-19 — one pinned 1.6 wetland candidate, four profiles: function PASS, strict ERROR0 FAIL

[Project Actions #39200796636](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39200796636) finished **FAIL**, and that original workflow status must remain FAIL. The uploaded original artifact `amje-wetland16-39200796636` (ID `12164466198`) was inspected case by case, including all eight `quickstart.json`, `result.json` and full `unity.log` captures. The **same 60-file candidate** from Environment source `5050e57250c6913c2210be47723fee68d318e59e` was used throughout (ZIP SHA256 `fbe3bddc7c1cbf0502b43fdf09cc7710874cdda07caf76c3811ff1128a42c7c3`; production DLL SHA256 `3601246eb6429789a32ee25253740f1e587358cd6e09699d7e1bfbbfe4016520`).

- **Tested assumptions:** genuine Linux RimWorld 1.6.4871; normal TemperateSwamp and ColdBog maps; unchanged wetland four-plant source/weights, loaded graphics and real mature-plant harvest behavior; native cutting is included in ColdBog. Original Quickstarts: Vanilla **82/82 + 73/73**, CCTO **82/82 + 73/73**, MO **84/84 + 75/75**, MO+CCTO **84/84 + 75/75** = **628/628 game assertions**, zero assertion failures, eight clean process exits, complete uncropped captures, no runtime errors.
- **Strict result:** Vanilla/CCTO had zero Unity errors. Each of the four MO-containing cases had exactly **two prelaunch Unity ERRORs** with the identical independently established external MO/SYR message `Cannot create FMOD::Sound instance for clip "" (FMOD error: Unsupported file or audio format. )`; **8 genuine raw ERRORs total**. These were verified from the full original Unity logs, not inferred from a passing assertion count. **Both MO profile summaries and global strict ERROR0 stay FAIL**; no logs are suppressed.
- **Owner-scoped private-release functional classification:** the unchanged known external blank-clip FMOD-only exception in this Mod's existing release contract applies **to these eight scoped cases**. Wetland 1.6 functional acceptance is **PASS on this exact one candidate**, separately from strict ERROR0 FAIL. Do not rerun the same unchanged wetland matrix to relabel the workflow.
- **Not established:** this is only two wetland maps per profile. It is **not** the complete final release map/cutting suite, not the distinct pinned 1.6 season/save source `03c743376a34ef9256acfed147da74adf5e75c51`, not 1.7 matrix/old-save evidence, nor a Steam uploaded/downloaded subscriber test. Final 0.2.0 release, current-version copy, DLL provenance through publisher and privately authenticated upload remain **HOLD**.

## 2026-10-19 — wetland ColdBog calendar and native save/reload PASS (scoped)

[Project Actions #39194825692](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39194825692) tested exact Environment source `03c743376a34ef9256acfed147da74adf5e75c51` in real Linux RimWorld 1.6 Vanilla: **26/26 PASS, seasonal temperature -11.80 to 30.94 °C, graphics valid, actual 12,594,839-byte save, separate-process genuine reload with all four named wetland plants and ColdBog biome retained, raw ERROR0, both exits0**. [Full tested assumptions, hashes and limits](ValidationEvidence/WetlandSeasonSaveNative16-20261019.md). Earlier test-harness reload failures remain failed history; the scene transition was corrected in the test assembly, not the production Mod. Scoped season/calendar and save/reload acceptance is **PASS**, not a full continuous 60-day simulation or 1.7 old-save/season test. **Release HOLD remains:** final synchronized Workshop/2game BBCode and version 0.2.0 promotion, exact final single candidate four-profile gate, independently authenticated owner-isolated Steam private publisher, and actual downloaded subscriber native check.

## 2026-10-18 — 0.2.0 public copy prepared, release HOLD

Four wetland plants exist in development source. README now distinguishes it from Workshop 0.1.1. The author-accepted three new production PNGs (Ashi/Suge/Hannoki) and existing four images are referenced in the staged bilingual 0.2.0 draft, which stays NOT PASTE READY pending other release checks. By author decision on 2026-10-18, no separate Mizugoke gallery image is required while it reuses Vanilla Moss art: seven accepted images plus Mizugoke's bilingual text-only caption are sufficient. Exact final BBCode size/tags, 0.2.0 About/Manifest/Changelog, exact final candidate four-profile acceptance, owner-isolated private Steam upload and downloaded subscriber verification are still OPEN. The scoped 1.6 ColdBog calendar and new-save reload gate passed on 2026-10-19; see the newer evidence above. Current `Docs/SteamWorkshopDescription.txt` remains the correct live 0.1.1 text, unchanged by this draft.


## 2026-10-17 wetland four plants — native RimWorld 1.7 smoke PASS (scoped)

Actual [Project Actions #39041272179](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39041272179) tested immutable Environment gameplay source `ea22a4fcb0454630d85eb48c9e88604cedc97497` on genuine Linux **RimWorld 1.7.4977**. **Pickle 1/1 PASS, native ERROR0, FMOD ERROR0, game exit0;** actual Plants20 CutPlant gave Yoshi Hay2, Suge Hay1 and zero-drop Mizugoke. Owner PlantDefs, both biome weight sets and nonfood/wild-only inheritance were checked as loaded real 1.7 data. See `Docs/ValidationEvidence/WetlandFourProductionNative17-20261017.md` for precise marker/hashes/limitations. This does not verify 1.7 natural wetland spawning, winter/saves, all 1.7 provider combinations, exact release candidate or Steam subscriber upload/download. **0.2.0 private release HOLD unchanged.**


## New wetland 1.6 MO and MO+CCTO native checks — 2026-10-17

Project [MO/MO+CCTO both wetland scenarios #39038178612](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39038178612) actually ran 4 maps. Their loaded-game function counts were MO warm 83/84 (one genuine Yoshi Hay0 on a failable harvest), MO cold 75/75, MO+CCTO warm 84/84, MO+CCTO cold 75/75. All cases had exactly two previously independently established blank-clip MO/SYR FMOD prelaunch errors; **raw strict ERROR0 remains FAIL**. Test-only bounded independent natural harvest attempts fixed stochastic one-roll acceptance, and [focused MO warm #39039584212](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39039584212) passed **68/68** original loaded-game checks including real Yoshi Hay2 and Suge Hay1 after an independently recorded stochastic Hay0. Normal game harvest failure and approved base yields unchanged. See `Docs/ValidationEvidence/WetlandFourProductionNativeMO16-20261017.md`.

**Still not a full release PASS:** These independent 60-file builds do NOT share byte-identical ZIP/DLL digests; four-profile exact-candidate publication gate remains OPEN, as do genuine 1.7 wetland behavior, seasons/save, owner-isolated publisher and actual downloaded subscriber bytes. The published private-release MO/SYR error classification only applies within its exact stated bounds. No new Environment-origin runtime error was observed in the scoped MO logs.



## New wetland four-species 1.6 Vanilla/CCTO native PASS — 2026-10-17

The new Ashi/Yoshi, Suge, Hannoki and Mizugoke production source `ea22a4fcb0454630d85eb48c9e88604cedc97497` passes actual Linux RimWorld 1.6 Vanilla and CCTO both TemperateSwamp and ColdBog native tests: **310/310 assertions; 0 prelaunch errors, 0 runtime errors, game exits 0**. Base Yoshi Hay2/Suge Hay1 and actual CutPlant work; Mizugoke produces no drop. Detailed [same-source log evidence](ValidationEvidence/WetlandFourProductionNative16-20261017.md); [Project Actions #39034867997](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39034867997). This does **not** verify MO/MO+CCTO or 1.7 with the new species, four-profile release, seasonal/save behavior, user-accepted gallery images or Steam upload/subscriber bytes. Preserve old failed build/game attempts as failures and retain private-publication HOLD.


# 2026-10-15 current-main Linux 1.6 candidate — build and Vanilla native PASS


## Author-approved release classification — external MO/SYR FMOD (2026-10-15)

Environment does **not** add or alter sounds, sound Defs or FMOD assets.
The exact repeated `Cannot create FMOD::Sound instance for clip ""`
unsupported-format errors in the genuine MO/SYR provider stack occur in
control tests *without* Environment. They are owned by the Project native/FMOD
workstream, not an Environment crop/vegetation/world-generation defect.

For an **Environment-only, private Workshop candidate** decision, an unchanged,
independently established pair of these external FMOD errors in MO/MO+CCTO
may be **classified as non-blocking for this Mod's functional release**
only when all of these conditions are demonstrated on the **same exact
candidate**:

1. Actual game process exit 0, non-truncated/complete native observer
   reports, source/ZIP/DLL identity, and original assertions for the feature
   being tested all pass; no timeout, early crash, missing report or hidden
   failed assertion.
2. Every recorded Unity ERROR in the scoped MO run is exactly the established
   blank-clip unsupported-format FMOD message; no changed error text,
   unexpected third-party error, new runtime error or other startup fault.
   Retain **every** raw Unity ERROR and count. Do not suppress logs, patch
   MO/SYR sounds or relabel such a run global **ERROR0 PASS**.
3. Vanilla and CCTO remain ERROR0; MO and MO+CCTO keep their independent
   actual functional/biome/harvest evidence, with both **scoped functional**
   and **strict ERROR0** statuses recorded separately.
4. This exception does **not** waive verified physical Workshop upload
   source, current reviewed preview, downloaded subscriber byte identity,
   new Environment-caused exceptions or user-private visibility requirements.
   Any new Def-breaking change gets its own targeted compatibility check.

Thus the original global strict four-profile zero-ERROR suite continues to
report **FAIL** while known MO/SYR FMOD persists, but this specific external
audio issue alone no longer keeps an otherwise complete **Environment-only**
private candidate on HOLD. The Project FMOD owner still investigates it.
This is **not** a project-wide error waiver or a statement of compatibility
with untested combinations. Stable new-save play-loop/season/save-reload
criteria remain separately tracked, not silently satisfied.


**Actual build and exact payload validation PASS:** Project isolated runner
[Actions #38681874743](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38681874743)
compiled pinned Environment `4dacbcf0fbe3399ee44795f3097b8cbe498d0ae3`
against real Linux 1.6 managed assemblies, then validated a **59-file** payload
and ZIP. DLL SHA256 `29c41ebf8998db993ac496d7d5800b9ae39fec613490dee8da6a721cad702996`,
archive SHA256 `4b7707284f1b5a9d404962d21869e5914ea70aeb842951d0d45c395678e4b7c3`.
The canonical root-only `loadFolders.xml` and source/static checks PASS.
[Exact evidence and unfulfilled gates](ValidationEvidence/EnvironmentCandidateLinux16-20261015.md).

**Additional same-candidate Vanilla native PASS:** 9 original map Quickstarts, **466/466**, nine game exits 0, Unity ERROR 0, split over [base #38686699729](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38686699729) and [remaining #38686772083](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38686772083). Source/ZIP/DLL identity verified; see exact evidence linked above.\n\n**Subsequent exact-candidate actual cutting evidence (2026-10-15):** [Vanilla and CCTO five-tree native CutPlant #38689421756](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38689421756) both **17/17, ERROR0, PASS**, with source-matched WoodLog yields 42/40/30/8/24. MO and MO+CCTO five-tree RawWood yields also match, but [MO #38689626203](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38689626203) and [MO+CCTO #38690112481](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38690112481) each log two original-MO FMOD ERRORs, so **strict FAIL**. The earlier MO+CCTO attempt [#38689992750](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38689992750) exited 139 before testing and remains separate FAIL. [Detailed tested premises and evidence](ValidationEvidence/EnvironmentCandidateLinux16-20261015.md).

**Four-provider biome-replacement functional gate CLOSED:** Original unforced-world 24/24 assertions in each Vanilla, CCTO, MO, MO+CCTO profile against the **identical 59-file candidate**, with actual game exit0 and complete live captures. Strict ERROR0 succeeded in Vanilla/CCTO; MO/MO+CCTO each retain exactly two independently known blank-clip FMOD errors, so their strict global test remains FAIL but the owner-scoped functional release classification is PASS. [Verified run matrix and failed initial harness attempts](ValidationEvidence/EnvironmentCandidateLinux16-20261015.md).

**Hannoki author visual acceptance (2026-10-15):** six production static visual states (normal, immature, UI, forced autumn, forced leafless and leafy/leafless snow) have been author-accepted after five real Linux 1.6 screenshots with ERROR0, [native capture #38701213882](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38701213882). The reviewer criticized the small screenshots but explicitly adopted the current artwork without rework. The natural temperature-driven **Hannoki seasonal mechanics were additionally verified** on real Linux 1.6 with an isolated test-placed production specimen, 73/73 Quickstart, Unity ERROR0, native leafless→recovered and valid materials ([run #39016607046](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39016607046); [full scope](ValidationEvidence/HannokiSeasonalNative-20261017.md)). The earlier forced-static capture is not itself transition evidence. A separate genuine Linux 1.7.4977 native test also passed [#39018400784](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39018400784): same production Hannoki source compiled for the Waylith test host (net10/UnityStubs), actual cold forest, unforced fall factor 0..1, leafless/recovered=true, four native seasonal state transitions, Pickle1/1, Unity ERROR0 and exit0. Formal [1.6/1.7 Hannoki seasonal evidence](ValidationEvidence/HannokiSeasonalNative-20261017.md). This bounded accelerated annual sampling does **not** establish 60 days of full-world ticks, observed 1.7 texture/snow presentation, the shipping shared net472 DLL on Linux 1.7, all-provider 1.7 matrix or Steam subscriber PASS. Exact per-state SHA fingerprints and unmodified image sources: [Hannoki visual evidence](ValidationEvidence/HannokiVisualAcceptance-20261015.md) / `Docs/PlantVisualCoverage.json`.

**Still HOLD for actual private Workshop publication:** accepted Workshop preview/source match (separate from Hannoki in-game sprites), Environment-owned GitHub Actions publisher and authenticated private Steam state, exact physical selected upload source, downloaded subscriber bytes/identity and runtime smoke. **Known external MO FMOD2 alone is no longer an Environment-specific private-release blocker under the scoped contract above.**
Do not promote Vanilla-only native evidence into a complete four-profile functional 1.6/1.7 or Steam PASS.
The Linux test host's staged Environment game Mod is older, not this candidate.


## Highest-priority biome replacement repair — 2026-10-12 JST

The author reports tropical rainforest/swamp in the installed Workshop build.
Source candidate selection also admitted inappropriate Vanilla biomes, and the
former natural-world test did not reject them. This is a foundational defect;
old candidate/runtime PASS below does not certify biome replacement.
`Docs/Design.md` §9.2.1 owns the corrected full base-surface inventory and scope.
Source now filters ten replaced Vanilla identities before scoring and the
existing unforced world test has 24 assertions, including zero excluded tiles,
four AMJ bands, both retained wetlands and real MO coexistence. Build a fresh
production DLL/candidate and obtain the same-source Vanilla/MO/CCTO/MO+CCTO
natural-world functional evidence with native ERROR audit and the scoped external-FMOD classification above before clearing this repair. Native build/runtime
and corrected Steam upload/download are still OPEN. Prioritize this before
wetland species/art additions; Future upload must follow the author-approved GitHub Actions direction (not historical YADA), with owner-scoped private-only publisher credentials and exact downloaded-subscriber validation; no working Environment publisher has yet been demonstrated.

## Current candidate status (2026-10-08 JST)

A 32-file pinned candidate from `1d46727102f53243099927b59881091d448dba61`
passed the author's corrected full nine-map/one-cutting four-profile runtime
suite (Vanilla/MO 409+15, CCTO/MO+CCTO 457+15, zero runtime ERROR,
ten reports per profile, unchanged payload and normal config).
Test-only harness revision `f39914d5bbd02edb90f7c91fe5faaf6e645d86fd`
did not rebuild candidate bytes. This is **candidate runtime PASS** only. The author subsequently passed
exact 32-file `Build-WorkshopPayload.py verify` and the independent
`Combine-WorkshopPayloadResults.py` saved-log audit (40 reports).
The supplied `FourProfileAudit.json` has `passed=true` and
`steam_release_cleared=false`. Actual Steam upload, downloaded-payload
source provenance and runtime checks remain unverified, as do Stable
combined-loop/season/save criteria. Older evidence is historical.


This specific forward-only biome-generation repair does not require comprehensive **legacy save migration** testing: existing worlds are not rewritten, and this candidate does not remove or rename Defs. Check an old-save load smoke only if a future change may break referenced Def identity. This does not waive the separate new-save save/reload continuity in the stable combined-loop criterion.

Stable release additionally requires the AMJ combined basic-loop gate defined in
Core `Docs/ReleaseCriteria.md` (author decision, 2026-10-06 JST). Actual pawn work
with Core + Environment + real MO, recurring food production, a seasonal cycle
and save/reload continuity remain OPEN. The results below do not satisfy that
gate. Beta feedback is useful but its absence is not evidence of success.

The subsequent four-species cutting-resource gate passed in standalone and
actual MO profiles: 9/9 each, all runtime ERROR gates clean, exact outputs
42/40/30/8 of WoodLog or DankPyon_RawWood respectively at yield factor1.
Evidence: Docs/ValidationEvidence/HarvestOutput.json and native log excerpts.
The staged payload hashes were unchanged after execution; fixtures were retired.
This covers native cutting jobs and output, not autonomous work selection or
the combined AMJ survival loop. The original ZIP below predates these changes.

All four current AMJE plants have accepted normal/growth, UI and snow appearances.
Beech leafless/autumn approvals are retained; its automatic seasonal transition is
verified by bounded native calendar sampling, separately from human acceptance.
The strict visual coverage ledger passes with zero pending states.
Existing Vanilla/MO tree retextures are deferred to post-release updates.

The unchanged runtime-only candidate archive has SHA256
`d1ecd33a0c18cb2abe665685c9487a60bc257346000bb1faa3d528287109ce0d`.
Standalone boot/graphic-load smoke passed. Four biome maps, river/coast handoff
and optional CCTO checks passed: 58/57/54/52/3/3 and70 assertions, 297 total.
All seven reports had complete live capture, zero pre-launch/owned/global ERRORs,
no truncation and the correct extracted candidate root. Test-only Quickstarts
were loaded from a separate observer Mod; the ZIP excludes development code.
Normal saves/settings were preserved; temporary source metadata was restored.
See retained ReleaseRuntime/Report.json and ReleaseMatrix/Report.json.

These results cover the approved local release payload. This source PR also
preserves subsequent upstream testing/tooling additions; it is not a claim that
the integrated branch, full MO runtime or long-term gameplay has already passed
those same runtime gates. Build/static and Python checks are rerun on this branch.
No Workshop publication or GitHub release upload has been performed.

main:Docs/Coordination.md remains the authoritative handoff. Its current main
entry predates the local art closeout; update it on main when integrating this PR.
No branch-specific Coordination update is included.

Integration fix: the static validator now checks the current Owned AMJ runtime
ERROR marker introduced upstream, preserving the runtime gate itself unchanged.
The PowerShell change remains in this temporary PR branch and requires CI before
merge, per AGENTS.
