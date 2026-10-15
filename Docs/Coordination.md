# AMJ Environment Coordination

Current OPEN / IN PROGRESS / BLOCKED handoffs are retained below. Formal designs and procedures remain in `Docs/Design.md`, `Docs/GoldenPaths/`, and production source/tests. Historical DONE/superseded details have been condensed; the pre-compaction Git history plus canonical owner docs retain original evidence. This is a status queue, not a second specification.

## Immediate execution priority

**2026-10-15 latest build-only checkpoint:** Latest owner main
`4dacbcf0fbe3399ee44795f3097b8cbe498d0ae3` has an **actual native-host
source build and 59-file exact subscriber payload/ZIP static PASS** in
[Project Actions #38681874743](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38681874743).
This was executed via Project PR #260/main `bcb46071ed1eb6eefaabee0b50590898309f8913`,
without modifying old staged game Mods. Exact DLL and archive hashes plus
unresolved preview, native four-profile, save/season and Steam gates are in
`Docs/ValidationEvidence/EnvironmentCandidateLinux16-20261015.md`.
**2026-10-15 subsequent native candidate evidence:** exact source/ZIP/DLL validated under disposable full-copy Linux 1.6 Vanilla profile. [Base four native maps #38686699729](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38686699729) **293/293 PASS**, [remaining five maps #38686772083](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38686772083) **173/173 PASS**: **9/9, 466/466, native ERROR0 and each game exit0**. Project PR #274 was merged as `cd957c65139f7a2e178cd69b5a668304bf933c20`; original pinned host Mod unchanged. Exact evidence in `Docs/ValidationEvidence/EnvironmentCandidateLinux16-20261015.md`. **MO/CCTO/MO+CCTO, same-candidate real cutting, save/season, preview approval and actual Steam subscriber remain OPEN;**\n
neither older source-scoped PASS nor current build-only PASS clears release HOLD.
The newer author GitHub-Actions publication direction supersedes YADA-only
operational assumptions; reconcile publication implementation separately
before a live private Steam upload.


### ENV-BIOME-REPLACEMENT-20261012 — inappropriate Vanilla biomes

**Owner:** Environment world generation / existing native test gates
**Status:** SOURCE REPAIR IMPLEMENTED; WINDOWS BUILD / FOUR-PROFILE NATURAL-WORLD / CANDIDATE MAP-CUTTING / LOADED RIMTEST PASS; HARNESS CORRECTIONS AUTHORIZED FOR PR INTEGRATION; STEAM CORRECTION OPEN

The author reports TropicalRainforest/TropicalSwamp in the installed Workshop
build and explicitly made this repair the highest execution priority. This
supersedes the earlier instruction to continue wetland Step 3 first: pause
wetland species/art and other feature expansion until the biome repair has
fresh natural-world evidence. Project priority/roadmap/handoff are reflected
in Project main `9156770a7f857db033e847f08c952ddc75166d26` (PR #19).

Canonical corrected scope/inventory: `Docs/Design.md` §9.2.1. The production
root-surface selector excludes ten original Vanilla identities before Worker
scoring, retains the two audited wetlands and water, and leaves MO scoring /
Def ownership intact. Existing saved worlds are not rewritten. The former
residual-Vanilla acceptance is revoked; old distribution/fixed-map PASS cannot
certify this repair.

The existing unforced WorldWetlandDistribution Quickstart now has 24
assertions including each excluded biome == 0 over ALL surface tiles, four
AMJ bands, actual wetlands, natural-generation availability of both adopted
wetland Defs and DarkForest when MO is loaded. Each seed need not contain
both wetland types; native Workers still select the subtype. The
existing ten-test RimTest replaces its threshold-only test with 128 real
loaded-Worker production-selector combinations plus ocean handling. The
existing four-profile hidden payload runner accepts `--world-biomes-only` for
this same natural-world scenario and reports scoped biome evidence separately
from full map/native-cutting release acceptance. Procedure:
`Docs/GoldenPaths/RenderedRuntimeTests.md`; release remains gated by
`Docs/ReleaseReadiness.md` and publication provenance.

2026-10-12 Windows closeout: built the source and corrected test observer
against the installed RimWorld DLL. The exact 32-file candidate from
`50c1c9286e3f48260dc96cbfd828aa4967a10801` remains unchanged. Inspected native
natural-world reports pass 24/24 in Vanilla, MO, CCTO and MO+CCTO with complete
live capture, no prelaunch/runtime ERROR, verified loaded source, Direct3D
rendering and preserved normal ModsConfig/Prefs. Evidence roots:
`C:/Users/sucRo/AppData/Local/Temp/AMJE-WetlandContract-20261012` (Vanilla)
and `AMJE-WetlandContract-Remaining-20261012` (MO/CCTO/MO+CCTO).
The initial MO startup-race attempt remains recorded as failed; only the
subsequent fresh ERROR-zero MO run is accepted. These are scoped natural-world
results across two runs, not a full release/cutting PASS. ColdBog count zero
on this seed is not a defect: the diagnostic found all 903 wetland candidates
at 13.12–19.97 C and native TemperateSwamp scores higher on every candidate.
Both wetland Defs remain naturally available; existing fixed-biome ColdBog
regressions remain required. No climate, wetland candidate, native Worker,
vegetation or production DLL change was made for this test correction.
Static publication/ecology/tree/visual checks pass; unimplemented plant
acceptance remains SKIP. The independent full candidate audit now passes:
Vanilla426/15, MO449/15, CCTO474/15, MO+CCTO497/15 map/native-cutting
assertions, ten reports and zero runtime ERROR per profile. Exact 32-file
payload and normal config are preserved. Canonical audit:
`C:/Users/sucRo/AppData/Local/Temp/AMJE-FullRelease-Remaining-20261012/FourProfileAudit.json`;
raw results at `AMJE-FullRelease-Fixed-20261012` and
`AMJE-FullRelease-Remaining-20261012`. The first full run mistakenly included
unapproved development plant probes without their disposable Defs; release
staging now excludes only those probe calls, leaving developer probes and
all production assertions intact. The original MO runner later rejected its
complete 10 native reports against stale non-MO counts; the independent
auditor rechecks every actual report/source/render/error/preservation gate
using the corrected MO count table and records MO as independently recovered.
Original failed outcomes remain preserved; they are not relabelled PASS.
Loaded RimTest10/10 also passes, including 128 real Worker/production-selector
conditions and ocean handling, at `AMJE-CandidateRimTest-20261012`.
Its production assembly path and candidate bytes were checked through an
About-only temporary candidate; the normal development installation was not
replaced. The signed research evidence retains canonical LF bytes via a
single-path Git attribute; its approved SHA256/content are unchanged.
The author authorized the local harness/doc corrections for topic-branch
commit/push, PR, exact-head CI verification and protected main integration
on 2026-10-12. GitHub integration evidence is separate from the candidate tests. No new-main, full `run-tests.bat`, stable/save-season or
Steam subscriber PASS is claimed. Steam upload remains author-manual; actual
downloaded source/DLL and distributed four-profile correction remain OPEN.

### ENV-HANNOKI-LEAFLESS-SNOW-20261013 — production source and Windows native verification

**Owner:** Environment / wetland plant art and native tests
**Status:** WINDOWS 1.6 FOUR-PROFILE MAP / NATIVE CUTTING / RIMTEST / PICKLE PASS; SEVEN VISUAL STATES / SEASON / OLD SAVE / MO WOOD-CHAIN OFF / STEAM OPEN; LINUX MIGRATION DEFERRED

The author accepted the exact Hannoki source/state artwork and numerical
comparison through PR #145 (`e91f58194b525456ee006e35d751c755b0c376b8`).
The production-native assertions were integrated through PR #147
(`fa4fc045fc8ccbc6252d4b3c0bd8ca9f2b500985`).
Approved artwork/bytes and approval limits remain in `Docs/ArtDirection.md`;
confirmed values and wood-pool transfers remain in
`Docs/NativeVegetationStep3Design-ja.md` §3.1.2 and production XML.
Shared beech fall conversion remains a native visual candidate, not author
color acceptance. Leafless branch-shape/state acceptance remains provisional.

2026-10-13 latest author instruction: **do not migrate to Linux yet; execute
available tests**. This explicitly authorizes the available Windows test run.
It does not authorize shared-host provisioning or renewed per-chat Linux
installation/builds. The planned persistent-host architecture remains owned by
Project `Docs/SharedNativeTestHost.md`; deployment/dispatch selection is OPEN.

A fresh isolated Windows 1.6.4871 candidate from
`094d81f1fc9f32d621f7aaa3dbe97416997fa84b` passed all four existing profiles:
Vanilla440/17, MO463/17, CCTO488/17, MO+CCTO511/17 map/five-tree native-cutting
assertions; ten complete ERROR-free reports per profile. Independent re-audit,
exact source/provider/Direct3D/error captures and payload/normal config
preservation PASS. The same candidate DLL passed RimTest10/10 and serial
Pickle5/5 in each profile. Both real wetlands have positive natural Hannoki,
native yield24, correct WoodLog/MO RawWood and CCTO0/5 C+one dormancy extension.
Canonical tested premises, source/DLL/archive hashes, raw Windows evidence and
limitations: `Docs/ValidationEvidence/HannokiWindowsNative-20261013.md`.
Procedure and reusable observer corrections:
`Docs/GoldenPaths/RenderedRuntimeTests.md`. No production/gameplay/art edit.

Earlier Chat-side Linux evidence is retained, not upgraded: the supplied owned
1.6 game booted, source/Quicktests built reproducibly from pinned main, and
TemperateSwamp Vanilla/CCTO strict70/70 passed; MO/MO+CCTO72/72 native
assertions had two FMOD prelaunch ERRORs and therefore strict FAIL.
Windows did not reproduce those errors; the Linux cause remains unresolved.
The permanent source-build/bootstrap recipe from PR #155
(`4dc73bcf4eb608c5959c2d9a3e96ffb605bbb4c3`) remains
`Scripts/Build-ChatLinux.sh` and
`Docs/GoldenPaths/ChatLinuxPublicModBootstrap.md`.
Superseded Chat dependency/provisioning correction chains and their exact
hashes are retained in main Git history before this queue compaction.

**Remaining:** Hannoki's seven pending human visual states, actual seasonal
transition and snow depths, immature/icon, CCTO winter recovery, final Hannoki
sow-menu behavior, additional seeds, MO wood-chain OFF, real legacy save
load/re-save/add/remove, and actual Steam upload/download verification.
The installed Workshop is still the old source. Candidate test PASS does not
clear art/save/distributed-release HOLD or claim one-command standard-suite/
Core-Grains/developer-probe completion.

## Publication and distribution

### ENV-BEECH-FALL-CALIBRATION-20261013 — autumn value/saturation source mismatch

**Owner:** Environment plant art / native seasonal observer
**Status:** AUTHOR-REVIEWED ADOPTION CANDIDATE IMPLEMENTED; FILE-LOADED COLOR / NATIVE SEASON EVIDENCE PASS; HANNOKI PRODUCTION / DISTRIBUTION OPEN

The author accepted proceeding with the right-hand source-value/saturation
calibration on 2026-10-13 JST. Canonical palette/source values and preserved
Hannoki master: `Docs/ArtDirection.md`; provenance/test premises and limitations:
`Docs/ValidationEvidence/BeechAutumnCalibration-20261013.md`; procedure:
`Docs/GoldenPaths/PlantVisualCoverage.md`. Normal/leafless/snow source pixels,
draw metadata, gameplay and the live player's config remain unchanged.
The preferred DDS is decoded-RGBA-identical to the accepted PNG; final loaded
Def/LUT GPU readback and noon/Clear screenshots pass separately from the
prototype review. Seasonal evidence is retained in the named validation record;
the first cold-resistant-individual sampling failure is preserved, with only
the observer's native individual selection corrected. No seed/climate/Def
change is made for that failure.

Hannoki's provided master and paired 256px image are authorized for separate
source transfer by the author's 2026-10-13 instruction 「画像も更新」. Exact paths,
dimensions and SHA-256 are recorded in `Docs/ArtDirection.md`; this bounded
transfer preserves both local PNG byte sequences and does not modify Defs.
It was not part of the beech calibration PR. Shared LUT color conversion
was observed only in a disposable comparison Def. Its production Def, actual
state family, draw size/yield and natural seasonal behavior remain OPEN. PR136's
earlier documentation is still independent; current source uses explicit
TreeBase seasonal parameters, not a DeciduousTreeBase inheritance claim.
GitHub integration and Steam/old-save/four-profile release results are separate;
this item does not claim an upload or distributed correction.

### ENV-LOADFOLDERS-SOURCE-REPAIR-20261009 — root-only source contract and isolated Quicktests

**Owner:** Environment publication provenance / developer runtime-test plumbing
**Status:** SOURCE REPAIR IMPLEMENTED; STATIC/CI/RUNTIME EVIDENCE SEPARATE; STEAM DISTRIBUTION HOLD REMAINS

The tracked production `loadFolders.xml` is now byte-identical to the approved
root-only subscriber contract; the former executable
`IfModActive="rimworks.quickstarts"` → `DevQuickstarts` entry is removed.
`Scripts/Build-WorkshopPayload.py` now requires the tracked loader bytes to
match its root-only `LOAD` contract and fails closed on drift instead of
silently replacing a mismatching source loader during staging.

Developer fixed-biome Quickstarts remain test-only. `build.bat` still compiles
their DLL under excluded `DevQuickstarts`, while `run-runtime-tests.bat`
stages that DLL as a standalone temporary Mod
(`sucro.ancientmedievaljapan.environment.quicktests`), adds it only to each
isolated test ModsConfig after the production Environment Mod, and removes the
owned fixture on exit. The fixture manager refuses to overwrite a pre-existing
same-named directory and refuses deletion unless its ownership marker,
packageId and DLL are intact. The outer isolated-desktop launcher also attempts
the same guarded cleanup after abnormal runner termination. Normal user
ModsConfig/Prefs are not edited by this path.

**2026-10-10 follow-up, runtime test blocker:** On the author's local
`run-tests.bat`, `Scripts/Validate-Environment.ps1` still required the retired
`<li IfModActive="rimworks.quickstarts">DevQuickstarts</li>` loader entry,
contradicting the already-correct production root-only `loadFolders.xml`.
The validator is corrected to parse XML and require only one `v1.6/li` entry
with `/` and no attributes, preserving fail-closed detection of extra folders.
Executable payload rejection tests cover invalid production load paths.
Do not reintroduce `DevQuickstarts` in the production
loader or use visible runtime execution; rerun the same isolated `run-tests.bat`
on the Windows machine after the corrected source is present. No latest native
RimWorld test result is claimed by this source-only fix.

This repair changes publication/test plumbing, not Environment gameplay Defs,
production C# behavior or art. The root-only loader bytes are the same bytes
already present in the previously validated 32-file candidate. It reduces the
specific risk that selecting the development root through YADA leaks
`DevQuickstarts` load semantics, but **does not authorize the development root
as an upload source** and does not replace the exact selected-root manifest
gate. No new RimWorld runtime PASS or Steam upload/download is claimed here.
Actual Workshop correction remains author-manual using the verified extracted
candidate, followed by a fresh Steam download and distribution/runtime checks.

### ENV-STEAM-DEVQUICKSTARTS-LOADFOLDERS-20261009 — published root mismatch confirmed

**Owner:** Environment Workshop publication and downloaded-Pickle runtime
**Status:** BLOCKED UNTIL NEXT AUTHOR-MANUAL WORKSHOP RELEASE; WRONG DOWNLOADED loadFolders.xml; STEAM DISTRIBUTION HOLD

Author's `test-run.bat` blocked on the actual installed Steam Workshop `3814638060` before launching RimWorld. The existing approved `AMJE-Final-20261008-214525/Manifest.json` has 32 files; installed Workshop has the same 32 paths, **30 SHA256-identical**, and two differing metadata files: `About/PublishedFileId.txt` and `loadFolders.xml`. The approved payload's canonical `loadFolders.xml` has only `<v1.6><li>/</li></v1.6>`. The downloaded payload's exact semantic XML (author test log) includes an **additional executable conditional entry** `<li IfModActive="rimworks.quickstarts">DevQuickstarts</li>`. The tracked **developer** root `main:loadFolders.xml` contains that same conditional entry; the source candidate builder `Scripts/Build-WorkshopPayload.py` explicitly replaces it with canonical root-only `LOAD`. This is a meaningful runtime path difference, NOT harmless formatting; the fail-closed Pickle preflight correctly rejected it. It is **consistent with publishing the developer Mod folder through YADA's filtering rather than selecting the validated extracted candidate** (YADA filters directories but does not edit retained XML contents), although the actual uploader selection path is not independently recorded, so do not call the selection proven.

**Latest author decision (2026-10-09):** Correct this discrepancy **together with the next regular author-manual Workshop update**, not by an immediate corrective re-upload. The previously verified 32-file candidate is evidence for its pinned source only; before the next upload, pin the then-current clean source, build/verify its exact candidate and external manifest, and select that physically verified extracted root (never the development checkout). Preserve Steam provenance/runtime HOLD and the existing fail-closed downloaded-Pickle gate; do not rerun downloaded-Pickle against the known mismatched old installation. After the next author upload and fresh Steam download, verify downloaded file/source/DLL identity, then run Pickle and the separate four-profile rendered/native cutting gate. This decision did not perform an upload.

**Framework source audit performed:** `RimWorks/Rimworld-Pickle` official `Docs/getting-started.md`, `Docs/authoring.md`, `Docs/autorun.md`, `Docs/running.md`, `Docs/reports.md`, `RimWorks/pickle-template/README.md`, and `ilyvion/rimtest-redux/README.md` were read. Pickle's Gherkin, isolated test Steps in `Pickle/Assemblies`, `-pickle-run`, independent report/exit-code and per-mod-set conventions match the present workflow at a design level. RimTest Redux is a unit-test framework requiring test-only assemblies; it is not a substitute for Steam artifact provenance or Pickle's live loading. **Real Windows Pickle C# compilation/runtime is not yet validated.** Avoid further speculative harness rewrites until the release-source blocker is resolved. The official RimWorld `loadFolders.xml` semantics document confirms `IfModActive` loads an additional path when the target Mod is active.


### ENV-WORKSHOP-BILINGUAL-CLOSEOUT-20261008 — single-field English/Japanese Workshop copy

**Owner:** Environment public copy and release  
**Status:** GITHUB COPY PREPARED; STEAM POSTING AUTHOR-MANUAL; OCTOBER 9 DESCRIPTION REORGANIZATION MERGED

The owner source `Docs/SteamWorkshopDescription.txt` now contains one complete English-then-Japanese description followed by one common plant-image gallery with four distinct images and EN/JA captions; `Docs/SteamWorkshopDescription-ja.txt` is its maintained Japanese draft, not a second Steam field. The full combined body fits the 8,000-byte budget and omits license/AI/donation statements. `Docs/WorkshopDescription.md` defines the paste workflow. `Tests/validate_workshop_description.py` is integrated into the Workshop payload CI to prevent regressions. About title, packageId, gameplay code/defs, image assets and Steam live posting are unchanged by this description correction. Confirm CI separately; final Steam posting remains author-managed.

Current Workshop organization follows the author-final 2game grouping (climate/weather, rivers/coasts, biomes, soil, vegetation, wildlife, wetland disease, descriptions): `087fb8e7b6a965d793cab07a58fc85dc4a310808`. Live Steam posting remains unverified.

### ENV-PICKLE-TERMINAL-RESULT-GATE-20261009 — terminal success guard implemented

**Status:** SOURCE/CI DONE; WINDOWS RUNTIME NOT YET VERIFIED

PR #46 squash-merged as `35a16e889847cbf11ea15bd1ac7c088f8bb5202b`. Both development Pickle and downloaded-Workshop Pickle runners require `summary.json.exitReason == "passed"` alongside complete passed scenario counts. Static regression checks and both triggered GitHub workflows passed on PR head and main. No author-side Windows/Pickle runtime PASS is claimed. This does not clear actual Steam source mismatch or rendered/native cutting release HOLD.

### Current release evidence boundary

**Status:** CANDIDATE FOUR-PROFILE PASS; ACTUAL DOWNLOADED STEAM CONTENT AND COMBINED GAMEPLAY LOOP NOT CLEARED

The old pinned 32-file candidate has author-reported four-profile map/native-cutting success, audited saved reports, and preserved payload/config; that result is not a PASS for future changed source, newly built candidate, or subscriber bytes. Newly added RimTest/Pickle harnesses still require author-side Windows execution. Stable seasonal/save/combined gameplay criteria remain OPEN in `Docs/ReleaseReadiness.md`. The publication procedure is `Docs/GoldenPaths/WorkshopPublication.md`.

## Vegetation, ecology, artwork and runtime follow-ups

### ENV-RETAINED-TREE-LABELS-20261010 — historically appropriate Japanese names

**Owner:** Environment vegetation/localization and Vanilla tree regression  
**Status:** SIX VANILLA TREE + TWO WETLAND LABEL CHANGES ON MAIN; GENERIC/WILDLIFE/MO 1.6 SOURCE AUDIT DONE (NO ADDITIONAL RENAME); NATIVE FOUR-PROFILE TEST UNVERIFIED

2026-10-10 author correction: Japanese pre-Edo flora naming includes **display labels, not merely descriptions**. Formal owner `Docs/Design.md` §11.5.5, six-Def mapping and existing approved two-paragraph texts `Docs/VanillaPlantStep2DescriptionReview-ja.md`. English `Patches/VanillaTreeDescriptions.xml` and Japanese `Languages/Japanese/DefInjected/ThingDef/AMJ_WildPlants.xml` add labels without changing DefNames, forest distribution, sowing, wood cutting, ingredients or art. The six existing Quickstart description assertions also check corresponding EN/JA loaded labels without changing expected assertion counts. The retained oak is a group-level Japanese deciduous Nara proxy; a narrower specific Mizunara identity is a separate evidence/visual/approved-text decision. **2026-10-10 next naming slice:** the two retained Vanilla wetland BiomeDefs receive Japanese-context display names, `TemperateSwamp` 温帯湿地 / temperate wetland and `ColdBog` 冷涼湿原 / cool wetland. The same `Patches/VanillaWetlandDescriptions.xml` and Japanese BiomeDef DefInjected own label changes; descriptions stay author-approved and unchanged. Wetland canonical mapping: `Docs/VanillaWetlandBiomeAudit-ja.md`. `defName`, BiomeWorker, world and map terrain, weather, diseases and wildlife stay untouched. Existing wetland Quickstart description assertions now check bilingual labels too, without increasing counts. This name change does not certify the earlier 2026-10-08 description-only runtime PASS as a new PASS. Animal renaming and other inherited Vanilla/MO flora remain separate evidence/side-effect review, particularly because reused global ThingDefs affect contexts outside Environment's biome pools. Fresh in-game bilingual label loading and four-profile validation remain outstanding.

**2026-10-10 evidence-based closeout for the next naming candidates:** `Docs/Research/RetainedContentNameAudit-ja.md` audits all 7 retained generic Vanilla plants, 9 wildlife PawnKindDef proxy names across Environment's six biomes, and actual MO 1.6 tree/farm/dark-forest source in the supplied Workshop archive (archive SHA256 recorded in the research). No additional specific-species relabel is justified: keep generic flora generic, do not relabel the globally shared Bear_Grizzly/Wolf_Timber/Deer etc. as Japanese endemic species, preserve Great Oak/Iter/Fir/Willow as MO-owned fantasy identities. AMJE naming is now **bounded by actual Def identity** and any future species-level rename requires separate visual/biogeographic/ownership checks; no new XML labels/test cases added in this audit. Follow up only on already-unverified bilingual loaded-game and four-profile gates.

### ENV-MO-WETLAND-WILDPLANTS-PATCH-INTERACTION-20261010 — preserve MO herbs

**Owner:** Environment optional Medieval Overhaul coexistence  
**Status:** MO PLANTS PRESERVED; DEVELOPMENT PICKLE FOUR-PROFILE 20/20 PASS; MO-LOADED WETLAND NATIVE-SPAWN QUICKSTARTS BOTH PASS (ATTACHED 18/18 RUNTIME CONSOLE LOG); RELEASE/CUTTING STILL OPEN

**2026-10-10 author instruction: Environment must not touch plants added by MO.** Source: MO supplied archive `3219596926.zip` 1.6 `1.6/Patches/Core/Add_Plants_To_Biomes.xml` adds Mindwort/Poppy/Fleawort/FlyAgaric to both retained Vanilla wetlands. Environment formerly replaced both complete `wildPlants` nodes, removing MO entries whenever its patch applied later. The owning `Patches/VanillaWetlandVegetation.xml` now **conditionally removes only exact named Vanilla entries and re-adds only the accepted Vanilla baseline weights**; MO/other-Mod-added independent entries remain untouched whether they are patched before or after AMJE. Missing ColdBog/Cypress no longer makes an unguarded removal operation fail. The 7.30/8.22 totals now explicitly measure only Environment-owned Vanilla baseline plants; optional MO additions have their own weights. Formal source: `Docs/Design.md` and `Docs/VanillaWetlandBiomeAudit-ja.md`. Static contracts and the existing MO-conditional wetland Quickstarts verify the intent; **actual Environment+MO loaded Defs, natural spawn, four-profile regression and save compatibility remain OPEN until executed**. Do not revise MO herbs' DefNames, properties, spawn weights or ownership. **MO 1.6 source rechecked directly on 2026-10-10:** all four named herbs are added to both wetlands with `0.05` each, archive sha256 `6ed379d7c400db43b3af2a9a7fd6203f7f7e99ca1a670d36b0f73fd1adccc9e3`. Reuse the existing development Pickle `Replacement native plants retain tuned development commonality` scenario for these eight **loaded** values under MO and MO-CCTO, and the already-existing two wetland Quickstarts; do not add a duplicate scenario or extra RimTest case. Runtime path and remaining evidence are described in `Docs/GoldenPaths/RenderedRuntimeTests.md`. This source/CI coverage does not establish native MO spawn, ERROR-zero or four-profile PASS.


**2026-10-10 MO wetland native-spawn gate COMPLETE for the tested generated maps:** User-attached `EnvironmentIsolatedRuntime(4).log` (773 lines) records **18/18 non-visible Quickstarts PASS, all 18 owned-ERROR checks PASS**, including **both new MO-loaded wetland scenarios** within the actual Grains/Core+MO profile (9 base + 1 CCTO + 8 MO+Grains/Core). In current source the existing MO-specific wetland assertions require each of Mindwort, Poppy, Fleawort and FlyAgaric to retain `BiomeDef.CommonalityOfPlant = 0.05`, and independently require **at least one of the four plants already present on each generated wetland map** using `map.listerThings.ThingsOfDef`. There is no forced MO herb spawn in the test. As both wetland scenarios completed, those bounded loaded-Def and aggregate-spawn assertions passed; **the console log does not contain exact per-species plant counts** (they are emitted to individual per-scenario game logs) and therefore does not establish all four species on both maps, long-term spawner behavior or MO-only standalone rendered profile. No individual JSON, Unity Player.log or physical provider hash was attached. The author previously supplied Pickle 20/20 and RimTest 10/10 separately. Repeating the same Quickstarts is not required for this evidence; distinct native four-profile release/cutting, save and subscriber checks remain OPEN.

### ENV-SUGI-FOREST-20261010 — priority-A cedar candidate restored

**Owner:** AMJ Environment
**Status:** QUEUED AFTER ALL FOUR WETLAND SPECIES COMPLETE; NATURAL CEDAR FIRST / REGIONAL LATE-MEDIEVAL PLANTATION CONTEXT APPROVED; NO LIVE PLANTDEF OR NUMERIC APPROVAL

The 2026-10-07 additional-vegetation grading explicitly gave **Sugi (Cryptomeria japonica) A** alongside Susuki A. Sugi was intended as a cool-temperate-dominant, partly warm-temperate forestry/woodland species, but was omitted when Environment Step 3 narrowed around four wetland natives. Restored in `Docs/NativeVegetationStep3Design-ja.md` §3.5 and `Docs/Design.md` without changing existing implementations. Historical natural cedar habitat is not automatically equivalent to post-medieval/modern planted forest distributions. New need/VE+non-VE audit, author-approved Japanese text and artwork, distribution / parent / tree sowing / optional MO wood / yield tests remain OPEN. The four-species wetland decision JSON does NOT cover Sugi. **2026-10-10 author decision:** finish the four Environment wetland plants (production Defs, author-accepted texts and graphics, actual suitable spawn and functional/profile/save tests) **before** implementing either Sugi or Susuki. For Sugi, implement historical **natural cedar woodland first**, while discussing late-medieval planting and wood utilization only as regionally attested history; do not generate modern nationwide cedar plantations. Planting availability, forestry work, harvest yields and biome weights are still undecided. The Sugi/Susuki order within the later phase is not specified. No release or gameplay change.

Grade audit: the 2026-10-07 explicitly A-rated plants were **Sugi and Susuki**; Susuki is already covered in its own item, while Yoshi was A-minus, Kuri B-plus, Hinoki B. Later chestnut/fruit gathering ownership and wetland scope decisions remain unchanged.

### ENV-SUSUKI-GRASSLAND-20261010 — priority-A grassland species restored

**Owner:** AMJ Environment; future architecture/fiber resource consumers separately.
**Status:** EARLIER SUSUKI SOURCE ACCEPTANCE RETAINED; PLUME REVISION CANDIDATE / RIGHT-FACING CONDITION UNDER REVIEW; IMPLEMENTATION QUEUED AFTER FOUR WETLAND PLANTS; OTHER GATES OPEN

The author's 2026-10-07 review marked Susuki (Miscanthus sinensis) priority A to represent open warm/cool temperate secondary grasslands. It was not carried into the Environment's canonical Step3 design or current gameplay. `Docs/NativeVegetationStep3Design-ja.md` §3.4 and `Docs/Design.md` now retain this as a **separate grassland candidate**, not a fifth approved plant in the wetland four-species gate. Existing-Mod comparisons (VE/non-VE), Project decision record, historical Japanese prose, original artwork, biome placement, harvest material and save/runtime tests are all pending. Do not assume Suge's approved Hay/MO Straw contract applies to Susuki. **2026-10-10 source-art naming correction:** The author selected the most recent grass sprite, then explicitly corrected the plant identity: **"この画像はススキ。その後にアシを作れ"**. Mark this as accepted *Susuki mature source art*, **never Ashi**. Original user-attached PNG is `三本の穂が揺れるススキの草株.png` (1254×1254 RGBA, SHA-256 `d2b5bbca0f4112bacf4426800c92c985d5f52266c21e08602d822f8ff67d340f`). Durable species/art acceptance scope and planned source path are in `Docs/ArtDirection.md`. Binary upload to GitHub is NOT completed; no playable texture, PlantDef, other state acceptance, or runtime test. Next artwork target is a distinct **Ashi (`AMJ_Plant_Yoshi`)** candidate. **2026-10-10 sequencing decision:** Susuki follows completion of the 4 wetland plants, in the same subsequent phase as Sugi; no order between these two priority-A plants was decided. No production assets or Steam files changed.

**Reed display-name follow-up:** For the already planned wetland plant `AMJ_Plant_Yoshi`, Japanese author-review text now uses 「葦（アシ、ヨシとも呼ぶ）」. The botanical standard name is Yoshi, historically Ashi is older; no national frequency claim. Internal defName remains unchanged, no English translation or gameplay edit.

**Susuki/Ashi direction and art review:** Susuki's right-facing revision remains unaccepted; candidate hash and the native Plant.Print finding are retained in `Docs/ArtDirection.md`. On 2026-10-12 the author accepted the Ashi mature in-game screenshots with 「採用とする」: accepted Yoshi_A, drawSize=1.5, visualSizeRange=1~1, growth=1, fixed horizontal direction. Twelve specimens loaded correctly at local noon/Clear/paused; isolated rendered ERROR gate PASS. Canonical scope/hashes: `Docs/ArtDirection.md`; screenshots, test Def/source and logs: `TestResults/AshiReview-20261012/Review.md` and `CloseRun/`. This is test-fixture mature visual acceptance. Immature source art is provisionally accepted separately in ArtDirection; its loaded appearance remains unverified. Production PlantDef/rendering, icon/snow, ecology and harvesting remain open; do not mark whole-plant completion.

### ENV-STEP3-NATIVE-VEGETATION-20261008 — four Environment-owned wetland plants

**Owner:** Environment natural vegetation / isolated runtime testing.
**Status (2026-10-12):** FOUR JAPANESE DESCRIPTIONS AUTHOR-APPROVED; PR #102 CLOSED UNMERGED AS UNNECESSARY EXPERIMENT; DIRECT PRODUCTION SPECIFICATION/ART ACCEPTANCE NEXT; PLANTDEFS/BALANCE/ART NOT YET APPROVED; SUGI/SUSUKI AFTER FOUR WETLAND SPECIES.

**Sources of truth:** `Docs/NativeVegetationStep3Design-ja.md` §§3–4.1 (accepted Japanese texts, prepared English, CCTO and order); `Docs/Research/WetlandPlantTerrainSuitabilityReview-ja.md` and `WetlandPlantDistributionDraft.json` (unapproved habitat/weight candidates and test evidence); `Docs/Research/WetlandPlantMaterialsProfiles-ja.md`, `Docs/Research/WetlandPlantMaterialDefAudit-ja.md` (materials and native CutPlant); `Docs/Research/WetlandPlantImplementationDecision.json` (VE/non-VE/AMJ prior-art and ownership).

**Accepted unchanged:** Environment itself provides Yoshi/Ashi, Suge, Hannoki, Mizugoke without mandatory MO/Odyssey; approved Japanese labels/descriptions; Suge harvests generic Vanilla Hay and optional MO Hay→Straw, without a species-specific raw item; Mizugoke selectable, CutPlant-removable, zero-drop, non-harvestable, non-sowable. Cold settings Vanilla/CCTO: Yoshi 0/5°C+dormancy, Suge 0/0°C+dormancy, Hannoki 0/5°C+dormancy, Mizugoke 0/0°C and death below −35°C. Retain MO herbs, Healroot, adopted Vanilla trees, baseline total/woody `7.30/3.00` and `8.22/1.80` (excluding optional other-Mod wildPlants). **Suge mature basic harvestYield=1 Vanilla Hay is author-approved (2026-10-12; PR #117); PR #118 locks this value in the existing static contract. Suge distribution commonality TemperateSwamp=0.80 and ColdBog=1.10 is ALSO author-approved (2026-10-12; the user's '続けて' was approval of the immediately preceding question, subsequently clarified explicitly).** Yoshi Hay/output, Hannoki wood/output, Yoshi=2 and Hannoki=24 yields, other species commonalities and source artwork remain **unapproved**. Images last; do not add production Defs/CCTO refs/textures with placeholders. Sugi/Susuki remain later work.

**Evidence closed:** PR #98 tested temporary shallow-only Yoshi; PR #99 (`d116749`) archived dual-habitat logs showing 1,063/3,672 and 775/3,542 shallow cells with only 28/46,585 and 11/40,475 terrestrial occurrences in sampled Soil-tagged classes; removed experimental code. PR #100 (`5df91b7`) recorded those constraints in design. PR #101 (`4dcf8a6`) corrected the existing-Mod audit's obsolete whole-node wetland vegetation replacement statement and refreshed its pinned evidence hash. These results **do not** approve population levels or complete four-profile runtime/cutting, overwintering, old-save or subscriber gates.

**2026-10-12 author decision / current work:** PR #102 was **closed without merging**, after the author judged the extra Suge/Hannoki/Mizugoke disposable native-spawn sampling unnecessary. Its CI 4/4 does not establish runtime behavior; no Windows experimental run is required and no test-only Def/Patch/Quickstart additions from that branch enter main. Existing terrain-tag/fertility results and PR #99 Yoshi observations remain available in formal research. Resume the direct, species-by-species production implementation path in `Docs/NativeVegetationStep3Design-ja.md` §3.1.1; validate actual production natural spawn and jobs after the required real art/undecided numerical author acceptance. No placeholder textures or unapproved final commonalities/yields. Keep MO herbs, baseline pool totals, CCTO settings and save compatibility intact.

**Remaining:** Yoshi/Suge/Mizugoke unfinished species imagery and production, Yoshi output and remaining biome weights; Hannoki timber 24 and 0.70/0.45 approved and production PR opened. Native four-profile/old-save/winter/harvest and Hannoki autumn/leafless appearance remain open. The closed experiment is not a prerequisite. Steam verification was reported complete by the author in chat; no independently inspected new downloaded payload evidence is asserted here.

### ENV-SAVE-FRIENDLY-VEGETATION-20261008 — staged additions / existing-save gate

**Requested by:** author (2026-10-08 JST)  
**Owner:** Environment / world generation, vegetation and runtime testing  
**Status:** POLICY CONFIRMED; migration E2E NOT IMPLEMENTED / NOT RUN

Author approved prioritizing changes that require new-game world/map
generation first; postponing new fruit trees, harvestable woody plants,
traditional herbs and purely visual retextures where possible. Subsequent
plant additions should target existing saves **already using AMJE** without
demanding a new game for each species. This is not a claim that installing
AMJE into a save without AMJE, removing it, or regenerating existing biome
world geography is safe.

Formal contract now belongs to `Docs/Design.md` (Save-compatible vegetation
development order); proposed reproducible validation belongs to
`Docs/GoldenPaths/PlantSowingTests.md`. At the first new plant/fruit harvest
implementation, add an old-version-save -> new-version-load -> existing-map
natural spread -> real fruit/nut harvest -> research-gated real sow Job ->
save/reload regression, with complete logs and owned ERROR gate, and separate
standalone/MO/CCTO/MO+CCTO profiles. Avoid treating existing loaded-Def tree
menu tests as proof of existing-save compatibility. Retextures remain deferred
until the author reopens art work. This is design/handoff only; no gameplay
implementation, installed-game test PASS or Workshop publication is claimed.

### PRIORITY-FOREST-FOODS-20261008 — native fruit/forest-food feature queue

**2026-10-14 author boundary correction:** The old forest-food queue incorrectly made Environment the owner of **new food-purpose natural species**. The formal [Environment design](Design.md) now leaves existing Shii/Beech scenery/forestry and general environmental trees here while new food-purpose trees, greens, fungi and tubers, their source art and Environment-biome insertion patches are Foraging-owned. No Environment-owned PlantDef or gameplay was changed. This overrides the superseded ownership statement within this older priority entry.

**Owner:** Environment for **existing scenic vegetation and BiomeDefs**; [Wild Food Foraging](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Wild-Food-Foraging) for **new edible wild species/food content, graphics, biome placement patches, harvesting**  
**Status:** PROJECT EXECUTION ORDER CONFIRMED; fruit/forest-food feature work NOT IMPLEMENTED

2026-10-08 author priority: complete the current Environment vegetation/runtime
closeout, then when selecting the next **major new forest-food expansion**, place
Japan-appropriate native fruit trees / edible tree nuts directly **after
Ironmaking and ahead of further Waterworks/Rice feature development**. This
reorders new-feature execution only; do not interrupt current Step 3 medicinal /
wetland vegetation work or imply that all future species are now approved.

Authority: Project `Docs/ImplementationPriorities.md` and `Docs/Roadmap.md`;
Environment's accepted old-save-friendly design remains `Docs/Design.md`,
with migration test plan in `Docs/GoldenPaths/PlantSowingTests.md`.
Environment owns its **already implemented landscape PlantDefs** (including Shii/Beech), their original timber/scenic appearance and native distribution. **Foraging owns any newly added edible-purpose PlantDefs** (trees, greens, fungi, wild tubers), graphics, food resources and **Foraging-side `wildPlants` patches** into Environment Biomes. The
**2026-10-10 author dependency decision:** Wild Food Foraging now **requires** Environment (`sucro.ancientmedievaljapan.environment`) as the provider of Sudajii and other Japan-native vegetation; Environment has **no** reverse dependency. Environment defines the native Biomes and its baseline scenic species pools; Foraging adds **its own edible plants** conditionally to those pools without requiring Environment to ship food species. No forced Sudajii everywhere, no new tree/food Defs or yield approvals. Dedicated Wild Food Foraging owns edible resources **and** harvesting gameplay and will add its required `modDependencies` metadata when its playable About is created. The Shii mechanism source exists but is inactive; **new edible species selection, actual gameplay/old-save/native test and release remain open**. Preservation owns downstream fruit processing. Verify cross-owner harvesting with the existing-AMJE-save→new-build natural-spawn/real-harvest/sow/save-load E2E before claiming compatibility. Do not represent this handoff as
a completed fruit feature or runtime PASS.

### TEST-ENV-UNIFIED-20261008 — unified AMJE native test and wetland handoff

**Owner:** Environment / existing native test suite
**Status:** LOCAL STANDARD ENTRY PASS ON 1.7; INDEPENDENT RELEASE/CUTTING, OLD SAVES AND STEAM SUBSCRIBER GATES OPEN

The current `run-tests.bat` passed static validation, all 18 hidden rendered
Quickstarts, RimTest 10/10 and the four development Pickle profiles at 5/5 each
on 1.7.4976 rev1043. Native captures and report completeness passed; no runtime
ERROR. Exact evidence and tooling scope are in ENV-RIMWORLD-17-20261013 and
`Docs/GoldenPaths/RimWorld17Tests.md`.

This normal suite includes fixed-biome, natural-world, CCTO-focused and installed
MO + Core/Grains scenarios. It is not the independent four-profile subscriber
release/cutting contract, old-save E2E, or actual Steam-download verification.
Those remain OPEN, along with any specifically required unforced per-species
sampling not covered by the existing assertions. Older author-reported and
console-only result chains are preserved in Git history and the owning
`Docs/GoldenPaths/RenderedRuntimeTests.md`; they do not substitute for a fresh
revision-specific gate. Do not reopen unrelated feature/art work to clear tests.
### ENV-TREE-SOWING-001 — Regional growing-zone tree regression (2026-10-08 JST)

**Requested by:** author
**Owner:** Environment / vegetation testing
**Status:** IMPLEMENTATION / STATIC DONE; FRESH RUNTIME MATRIX PENDING

Extends ENV-PLANT-AUDIT-001 to natural distribution and ordinary tree-sowing
options together. `Docs/Design.md` owns the responsibility boundary and exact
four-biome sets; `Docs/VanillaPlantRetentionAudit-ja.md` links the joint audit.
Warm Shii/Maple/Bamboo, Cool Beech/Oak/Maple/Birch/Pine, Subalpine
Shirabiso/Birch, Alpine no ordinary trees. Haimatsu stays unsowable.

Existing fixed-biome Quickstarts now verify loaded Ground/TreeSowing/regional
conditions, exact wild-tree sets, research-locked empty tree options and
research-unlocked exact regional options using both native growing-zone menu
filters. The unregistered clean-cell probe preserves zones; research progress
is restored in `finally`. No production Def/C#/art change was needed.

Local preflight PASS: six static contract/mutation tests, full C# syntax parsing,
workflow YAML, existing art routing, strict plant visual coverage/texture/snow,
Workshop payload tests and publication-tool compilation. The scoped new CI
runs the fast contract gate; existing runtime runner automatically includes the
new assertions. No installed game/reference assemblies are available in this
editing environment, so no fresh game build/runtime PASS is claimed.

Testing handoff: run the normal non-visible four-profile vegetation matrix
(Vanilla/MO/CCTO/MO+CCTO), not harvest-only or one-biome focused runs. Require
complete capture and existing pre-launch/runtime ERROR gates. Record results
under this item and `Docs/GoldenPaths/PlantSowingTests.md`. Existing historical
map/cutting PASS counts do not satisfy this new gate. No user relay required.

### ENV-RETEX-012 — existing-tree retention audit and AMJE description rewrite

**Requested by:** author (2026-10-08 JST)  
**Owner:** Environment vegetation / localization / art  
**Status:** DESCRIPTION/RETENTION AUDIT UNBLOCKED — wetland Step 1 author-confirmed PASS; actual retained-tree retexture remains deferred

Before any broad Vanilla / Medieval Overhaul tree retexture pass, re-audit the trees and ground vegetation AMJE currently leaves in place. The previous rationale that human-created pine woodland / grassland / secondary forest justifies retaining Vanilla vegetation is rejected: those historical vegetation forms must themselves be represented by species and vegetation appropriate to ancient/medieval Japan. Do not assume an existing PlantDef should remain merely because it is already present. For each candidate, decide whether it belongs in AMJE's target region, pre-Edo scope, vegetation bands and landscape role; remove/replace/non-adopt targets that are unnecessary or inappropriate.

Only trees retained after that audit proceed to the later art pass. Their inherited Vanilla/MO descriptions must also be rewritten into the established AMJE plant-description format, Japanese-first, under the shared historical-description rules before English synchronization. The audit explicitly includes correcting culturally or historically mismatched inherited wording; the Vanilla bamboo wording that describes bamboo as not beautiful is a named review target.

Durable policy is recorded in `Docs/Design.md` sections **11.5.5–11.5.8**. The required order is **retention audit -> distribution/ownership decision -> Japanese description audit/rewrite -> author/content approval -> retexture**. Do not begin the retained-Vanilla art pass before description review. After the retained Vanilla set is complete, the roadmap proceeds to missing Japanese vegetation/medicinal plants and only then to final Wild Healroot removal.

The retained wetland wildlife, disease, weather and description bundles are now implemented; updated Step 1 runtime tests were reported PASS by the author on 2026-10-08. The **retention and Japanese-first description review may now start**, with individual descriptions still subject to author approval. Do not initiate existing-tree retexture: it remains separately deferred under `AGENTS.md` and pending current-plant visual Golden Path tasks. The four-profile matrix and detailed runtime log audit are separate unresolved checks.

### ENV-010 — existing tree retextures deferred; current plant art accepted

**Owner:** Environment vegetation / art
**Status:** CURRENT FOUR STRUCTURAL PLANT VISUALS ACCEPTED; VANILLA/MO RETEXTURES AUTHOR-DEFERRED

The four authored species and all required current visual states are accepted in `Docs/PlantVisualCoverage.json`, `Docs/GoldenPaths/PlantVisualCoverage.md`, and `Docs/ReleaseReadiness.md`. Earlier Sudajii question-mark graphics were traced to malformed PNG chunk boundaries and repaired (`0421396e6184fdc12933d07db0bf88990b43ab9d`); recursive PNG validation was added. Earlier diagnostic attempts are historical in Git. Proposed existing Vanilla/MO tree retextures remain explicitly deferred and require current actual assets and human visual acceptance under the Art Golden Paths; do not reopen them implicitly. Newly added wetland plant art has its own separate approval gate.

### ENV-010 text/localization rule — retextured plants

**Requested by:** author (2026-10-04 JST)  
**Owner:** art/localization  
**Status:** IN PROGRESS

All plants whose artwork is replaced by AMJE now also require an AMJE-authored description. This includes AMJE-owned structural plants, Vanilla tree Defs retextured by AMJE, and MO tree Defs conditionally retextured by AMJE. Labels remain unchanged unless a separate naming issue is identified; this requirement is specifically about replacing unsuitable/inconsistent descriptions.

Authoring flow is fixed: draft Japanese first, author reviews/approves the Japanese wording, then translate only the approved Japanese text into English. English should remain semantically aligned with the approved Japanese source rather than becoming an independent rewrite.

Canonical policy is recorded in `Docs/ArtDirection.md`: `4881b88a2efcbbc560429026329b2e1a34ee4a0c`.

### ENV-010 historical description scope expansion

**Requested by:** author (2026-10-04 JST)  
**Owner:** art/localization  
**Status:** IN PROGRESS

The prior ENV-010 rule requiring rewritten descriptions for AMJE-retextured plants is now explicitly part of the broader AMJ historical-description policy. AMJE must audit inherited Vanilla/MO descriptions for AMJ-facing items/plants/animals it uses, patches, retextures, selects, or localizes, not only the specific tree sprites being replaced.

Descriptions should contain supported historical facts and meaningful modern differences where supportable, with Japanese drafted and author-approved before English translation. Retexture work still carries a mandatory description review as a minimum art-pass requirement.

**Result / references:** shared policy in Core `Docs/HistoricalDescriptionGuidelines.md` commit `ca17b37eb3cca5266d1f62a2d73f527a503d76e5`; AMJE AGENTS `0e1b74173eca79dde09dffa2287fc5f72a583c27`; ArtDirection alignment `3b8b7c753b79951b315c2ffe31822416a31595c8`.
### ENV-RIMWORLD-17-20261013 — supported metadata and standard runner

**Owner:** Environment runtime/testing
**Status:** LOCAL FULL STANDARD ENTRY PASS; PR INTEGRATION / STEAM UPDATE PENDING

1.7.4976 rev1043 `run-tests.bat` passed static validation, all 18 actual
rendered Quickstart invocations (9 base, 1 CCTO, 8 installed MO + Core/Grains
integration), RimTest 10/10 including loaded production provider, and all four
development Pickle profiles at 5/5 each. Every invoked gate had complete
reports/captures and zero runtime ERROR. Evidence:
`TestResults/Standard17-Runner.log`, `TestResults/EnvironmentIsolatedRuntime.log`,
`TestResults/VegetationRuntime/`, and the runner's timestamped AMJE-RimTest /
AMJE-Development-Pickle temporary result directories.

`Docs/GoldenPaths/RimWorld17Tests.md` owns native executable/reference/log
selection and the serial log/PID collision guard. The production DLL is
preserved and loadFolders remains root-only for 1.6 and 1.7. This port changes
metadata/tooling, not gameplay or accepted art. Concurrent Hannoki image edits
are excluded. It does not clear unrelated old-save, species/art or actual
Steam-distribution HOLDs. CI/main and author-manual Steam update remain separate.
