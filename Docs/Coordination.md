# AMJ Environment Coordination

Current OPEN / IN PROGRESS / BLOCKED handoffs are retained below. Formal designs and procedures remain in `Docs/Design.md`, `Docs/GoldenPaths/`, and production source/tests. Historical DONE/superseded details have been condensed; the pre-compaction Git history plus canonical owner docs retain original evidence. This is a status queue, not a second specification.

## Publication and distribution

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

### ENV-WORKSHOP-CANDIDATE-FOUR-PROFILE-PASS-20261008 — current 32-file candidate

**Owner:** Environment Workshop release  
**Status:** CANDIDATE RUNTIME PASS (author-supplied structured summary); STEAM PUBLICATION/DOWNLOAD NOT VERIFIED; RELEASE HOLD

The author ran the corrected 9-map + 1-cutting release suite in all four profiles against the **same 32-file candidate** built from source commit `1d46727102f53243099927b59881091d448dba61`; its archive SHA256 is `a6df959f18a1a72153ba5dfbc196806b646194e452fec2e8f8c71d312635b0f6`. The test-only harness was corrected through merge `f39914d5bbd02edb90f7c91fe5faaf6e645d86fd`. Author-supplied `Summary.json`: `passed=true`, `full_four_profile_gate=true`; Vanilla409 map/15 cutting, MO409/15, CCTO457/15, MO+CCTO457/15; ten reports and zero runtime errors per profile. Author-supplied `Preservation.json`: `payload_unchanged=true`, `normal_config_unchanged=true`. The old expected-count and omitted-wetland/Unity-report-count failures were in the test harness; the production payload was **not rebuilt**.

Author's preserved runtime evidence is at `C:/Users/sucRo/AppData/Local/Temp/AMJE-Final-20261008-214525/Runtime-Final-20261008-224132`. The author then executed `Build-WorkshopPayload.py verify`, returning `PASS exact payload: 32 files; source 1d46727102f53243099927b59881091d448dba61`, and `Combine-WorkshopPayloadResults.py` on those saved results, returning `PASS four-profile source/render/cutting/error/preservation gate`. The supplied `FourProfileAudit.json` has `passed=true`, matching pinned source and archive hashes, lists 40 checked scenario reports (ten per profile) and reports `steam_release_cleared=false`. This independent **local tool-based** re-audit checks detailed logs, Unity captures, selected source/DLL/Def roots, genuine dependencies, Direct3D, enabled profiles, four native outputs and original payload/config preservation; the individual raw logs were not separately uploaded to this conversation. The candidate fixture changed only About name/packageId for testing; original upload identity remains intact. The actual Mod folder selected in the game publisher still needs exact-root/subscriber verification before manual upload.

**Steam remains author-manual.** Enable YADA and Add Changenote and select the validated 32-file candidate, never the dirty developer tree. A real downloaded Workshop3814638060 manifest/hash/source/DLL and four-profile runtime check are still required before claiming distributed-release PASS. Stable combined-loop/save-season criteria remain OPEN. Historical 26/28-file candidate records below are not an authorization to upload those obsolete candidates.

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

### ENV-STEP3-NATIVE-VEGETATION-20261008 — four Environment-owned wetland plants

**Owner:** Environment natural vegetation; future Medicine Mod separately owns medicinal plants and remedies
**Status:** FOUR PLANTS IN ENVIRONMENT SCOPE; FORMAL VE/NON-VE ENTRY DECISION RECORDED (CI GATE SEPARATE); NEW PLANTDEFS BLOCKED ON AUTHOR-APPROVED JAPANESE TEXT / SOURCE ART

**2026-10-09 author decision:** Environment must reconstruct the ancient-to-medieval Japanese natural environment **by itself**, with no required other vegetation Mod or optional DLC. Yoshi, Suge, Hannoki and Mizugoke remain Environment implementation targets even if equivalent plants exist in other Mods. VE, Biomes! Prehistoric, ReGrowth 2, MO and Odyssey are **optional compatibility** targets only; prior-art review is used to prevent duplicate spawns, unexpected pool changes and loaded patch conflicts, **not to omit AMJE plants**. Canonical sources: `Docs/Design.md`, `Docs/NativeVegetationStep3Design-ja.md`, `Docs/Research/WetlandPlantExistingModAudit-ja.md` (prior-art PR #49).

**2026-10-09 decision-file progress:** `Docs/Research/WetlandPlantImplementationDecision.json` records `ready` for the scoped independent-implementation necessity, backed by the pinned `Docs/Research/WetlandPlantExistingModAudit-ja.md` SHA256 and a repo-owned negative-mutation/static check in `Tests/test_wetland_implementation_decision.py`. This does NOT approve historical Japanese text or visual assets, and does NOT constitute a runtime compatibility PASS. Project's exact cross-repository validator has not been executed in this chat; keep that separate from the repo CI gate.

**2026-10-09 historical-text evidence:** `Docs/Research/WetlandPlantHistoricalEvidence-ja.md` records the present-day low/high marsh ecology, cited ancient Suge/Hannoki context and Yoshi thatch evidence, with four revised Japanese author-review drafts. Neither these drafts nor the earlier text in `Docs/NativeVegetationStep3Design-ja.md` is author-approved; no English translation, localization or production plant content is authorized by source research alone.

**Still gated:** Author-approved Japanese descriptions and original plant assets, per-species XML/static/visual/native runtime and save-update tests. First bounded implementation remains Yoshi once these conditions are met. Four species are within agreed scope, not implemented or released. No new Steam/Windows runtime evidence is claimed.

**2026-10-09 weight-transfer draft:** `Docs/Research/WetlandPlantDistributionDraft.json` proposes a numerically conserved four-plant redistribution for TemperateSwamp and ColdBog (total/woody unchanged), with source-pool drift and mutation checks in `Tests/test_wetland_distribution_draft.py`. Draft only: no approved weights, no live vegetation/asset updates or runtime/Steam evidence. The eventual terrain suitability, per-biome spawn proportions and save migration require real tests and author balance review.

**Unchanged responsibilities:** Retain wild/cultivated Healroot and MedicineHerbal supply. Medicinal Yomogi/Kuzu and remedies belong to the future Medicine Mod, not Environment; Vanilla/MO tree retextures remain deferred. Wetland current total/woody commonality: 7.30/3.00 and 8.22/1.80. Old handoff language suggesting later Healroot removal, Yomogi Environment implementation or skipping a plant because of an equivalent Mod is superseded.

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

**Owner:** Environment native vegetation; Project owns future Gathering consumer  
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
Environment owns PlantDefs, appearance and regional natural distribution. The
Project-level Gathering candidate remains responsible for edible-nut and
forest-food **harvesting gameplay**; Preservation owns downstream fruit
processing where appropriate. Establish actual consumer ownership and run an
old-AMJE-save to new-AMJE-build natural-spawn/real-harvest/sow/save-load E2E
before declaring midway-save compatibility. Do not represent this handoff as
a completed fruit feature or runtime PASS.

### TEST-ENV-UNIFIED-20261008 — unify parallel AMJE test and wetland development tracks

**Requested by:** author (2026-10-08 JST)  
**Owner:** Environment / runtime testing and wetland development — one shared handoff  
**Status:** IN PROGRESS — older per-map/runtime evidence distinct from later 32-file candidate four-profile PASS; newly changed main/Steam distributor runtime and independent world-share data OPEN

The author requested integration of the separate test chat with the current wetland/vegetation development chat. These now use the **same authoritative main** and `run-tests.bat` as the standard local automated entry; no duplicate branch, competing test scope, or user-mediated relay is required. PR #27 added the ninth natural-world wetland distribution scenario; the author reports that test PASSED. Earlier eight-scenario counts below remain historical evidence, not fresh logs for this ninth scenario.

**Merged documentation source:** PR #25 / squash merge `0af0590957d0416b77c285e3548c40dacaedab47` consolidates the regional sowing/alpine regression, wetland Phase 5 patch and ecology gates in `Docs/GoldenPaths/RenderedRuntimeTests.md` and updates `Docs/GoldenPaths/PlantSowingTests.md`. This change is documentation-only; Workshop payload CI passed and no new RimWorld runtime result was produced.

**Reconciled latest state:**
- Earlier warm-temperate isolated run failed 1/74 plus a ColdBog Cypress-related pre-launch PatchOperation ERROR. PR #23 (`a2ea49aecf69c8975ca26e4b84b755ba488005ca`) corrected whole-pool wetland patching and biome-local sowing comparison. No new runtime PASS claimed.
- Parallel Alpine test chat subsequently recorded 69/70 PASS; the sole failed assertion conflated natural `AMJ_Shrub_Haimatsu` (`plant.IsTree`) with the empty Alpine grow-zone tree menu. Current main already separates `ExpectedWildTreeLikePlants` from `ExpectedSowableTrees`; the native menu/Haimatsu exclusion assertions had passed. Keep `ENV-TREE-SOWING-RUNTIME-002` OPEN for a fresh matrix.
- PR #24 (`3e946ad24f4e5d93ced19478219b73f865befa54`) changed retained-wetland wildlife, diseases, climate weather and wild pack-animal pools, and added both loaded-Def wetland ecology/terrain Quickstart assertions. Static CI passed; full post-change game/runtime still pending.
- Standard `run-tests.bat` covers static/build then the eight base Quickstarts (four AMJ biomes, two wetlands, River/Coast) on an isolated rendering-enabled Windows desktop; optional CCTO and Grains/Core profiles are installed-presence-dependent. **This runner does not currently exercise the full Vanilla/MO/CCTO/MO+CCTO four-profile matrix**, because isolated MO startup is disabled in `run-runtime-tests.bat`. The four-profile/release gate must not be marked PASS merely because the standard gate passes.
- Future latest-main acceptance requires complete eight-scenario reports with zero pre-launch/runtime ERROR and non-truncated capture, optional profiles identified explicitly, world wetland-share sanity, and separate four-profile coverage when possible. Wetland descriptions remain unapproved Japanese-first drafts; bilingual content synchronization and Step 1 closure are still pending. `ENV-RETEX-012` stays BLOCKED.

**Canonical ongoing test handoff:** `Docs/GoldenPaths/RenderedRuntimeTests.md` (unified matrix/status), `Docs/GoldenPaths/PlantSowingTests.md` (tree contract), `Docs/VanillaWetlandBiomeAudit-ja.md` (wetland ecology and approval conditions). Refer future test work to this item together with `ENV-TREE-SOWING-RUNTIME-002` and `ENV-WETLAND-BIOME-001`, rather than maintaining separate chat-specific statuses.

**2026-10-08 new warm-forest runtime log (single-scenario evidence):**
Author-supplied `AMJWarmTemperateTerrainQuickstart(4).log` confirms
`AMJWarmTemperateTerrainQuickstart` in the standalone AMJE profile:
**74/74 in-log assertions PASS, 0 FAIL, exit code 0, no `[ERROR]` tag**.
The three approved naturally woody trees match the unlocked native
growing-zone choices; graphics audits show zero BadTex. This supersedes
the earlier 73/74 failure *for the warm scenario*, not for the complete
eight-scenario matrix. Only the `.log` was provided, not matching
Quickstart `.json` or outer runner summary, so complete live-capture,
truncation and pre-launch gates cannot be independently certified.
The tiny generated world (3,787 total / 1,262 land tiles) had **zero**
natural WarmTemperate tiles and `swampiness>=0.5=0`; warm map
verification used a forced biome proxy. This does not prove or disprove
normal world prevalence, so the world wetland-share gate remains OPEN.
Durable evidence context and remaining gates: `Docs/GoldenPaths/RenderedRuntimeTests.md`.


**2026-10-08 complete outer-runner log and Highland diagnostic fix:** The author's `EnvironmentIsolatedRuntime(2).log` records the eight standalone AMJE Quickstarts **8/8 PASS** (four forest, TemperateSwamp, ColdBog, River, Coast), with the runner reporting zero owned ERROR for each. Focused AMJE+CCTO warm loaded-Def check also **PASS**. The optional Grains/Core+MO profile loaded the warm map but its `RequireClimateGradient` log gate stopped on `Climate calibration line was not found for Highland`; no full integration PASS was produced. Root cause: `ClimateCalibrationDiagnostics` excluded impassable tiles when searching for climate sample points, leaving none in tiny test worlds dominated by impassable highland. PR #26 / squash merge `ec6c2bbe9f4d6498e0f1abaf4f088b84fee5a91e` adds a diagnostic-only, Highland-only fallback to measure impassable mountain tiles after playable candidate search fails. Strict four-tier `<8C`/`<0C` gradient checks and owned runtime ERROR gates remain intact; no gameplay/world generation/Defs changed. Regression `Tests/test_run_tests_entrypoint.py` and workflow trigger updated; PR CI 3/3 PASS. **Post-fix installed-RimWorld rerun remains pending** for the Grains/MO profile and overall run. Also pending: independent world wetland-share sanity and author-approved bilingual wetland descriptions; wetland Step 1 and ENV-RETEX-012 remain OPEN/BLOCKED respectively. Detailed evidence: `Docs/GoldenPaths/RenderedRuntimeTests.md`.


**2026-10-08 author correction — post-fix test PASS:** The author explicitly clarified that `パス` means **the rerun PASSED**, not that the run should be skipped. Treat the Highland diagnostic correction's **Grains/Core + MO + AMJE integration retest as author-reported PASS**. The previous statement that this retest was skipped is incorrect and superseded. The new post-fix full runner log/JSON was **not attached in this message**, so the evidence remains **author-confirmed, not independently log-audited**; do not fabricate assertion counts, capture completeness or per-scenario details. The earlier `EnvironmentIsolatedRuntime(2).log` independently confirms base 8/8 and CCTO PASS, and its historical pre-fix Highland failure remains a separate earlier result. World-level wetland distribution and wetland descriptions are still open, as is the distinct four-profile release matrix unless independently executed.

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

### ENV-STEP2-PLANT-DESCRIPTION-20261008 — retained tree description follow-up

**Status:** STEP 2 AUTHOR-CONFIRMED CLOSED; OTHER FIVE HISTORICAL DRAFTS NOT AUTOMATICALLY APPROVED

Bamboo's bilingual historical description/loaded check were merged under PR #29 (`8ee572e4a17e1a894472445db7e28755a6308aed`). Other draft Japanese tree descriptions have not received author approval and must not be translated or published as accepted text without review. Canonical text/plant distribution inventory is `Docs/VanillaPlantStep2DescriptionReview-ja.md`; retexturing stays deferred. Wetland Phase 1 was separately author-confirmed; exact wild wetland world-share calibration and independently inspected latest runtime logs remain OPEN.

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

