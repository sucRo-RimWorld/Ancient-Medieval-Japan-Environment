# AMJ Environment Coordination

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
**Status:** STEAM DISTRIBUTION HOLD; WRONG DOWNLOADED loadFolders.xml; NO PICKLE GAME RUN YET

Author's `test-run.bat` blocked on the actual installed Steam Workshop `3814638060` before launching RimWorld. The existing approved `AMJE-Final-20261008-214525/Manifest.json` has 32 files; installed Workshop has the same 32 paths, **30 SHA256-identical**, and two differing metadata files: `About/PublishedFileId.txt` and `loadFolders.xml`. The approved payload's canonical `loadFolders.xml` has only `<v1.6><li>/</li></v1.6>`. The downloaded payload's exact semantic XML (author test log) includes an **additional executable conditional entry** `<li IfModActive="rimworks.quickstarts">DevQuickstarts</li>`. The tracked **developer** root `main:loadFolders.xml` contains that same conditional entry; the source candidate builder `Scripts/Build-WorkshopPayload.py` explicitly replaces it with canonical root-only `LOAD`. This is a meaningful runtime path difference, NOT harmless formatting; the fail-closed Pickle preflight correctly rejected it. It is **consistent with publishing the developer Mod folder through YADA's filtering rather than selecting the validated extracted candidate** (YADA filters directories but does not edit retained XML contents), although the actual uploader selection path is not independently recorded, so do not call the selection proven.

**Action and stop:** Stop rerunning Pickle/RimTest or weakening `loadFolders.xml` verification: no runtime suite can validate 32/32 candidate provenance while the downloaded copy differs. The author must next select the **verified extracted 32-file candidate**, not the developer checkout, as the source for a manual Workshop correction. The already approved 32-file candidate does not need gameplay rebuild; its prepublication four-profile Quickstarts and 40-report independent audit remain PASS. The author owns manual Steam/YADA publication; it has **not** been performed in this workstream. Afterwards require a **new actual Steam download**, byte/provenance comparison, then Pickle loaded-game tests and full downloaded-root map/native-cutting checks separately.

**Framework source audit performed:** `RimWorks/Rimworld-Pickle` official `Docs/getting-started.md`, `Docs/authoring.md`, `Docs/autorun.md`, `Docs/running.md`, `Docs/reports.md`, `RimWorks/pickle-template/README.md`, and `ilyvion/rimtest-redux/README.md` were read. Pickle's Gherkin, isolated test Steps in `Pickle/Assemblies`, `-pickle-run`, independent report/exit-code and per-mod-set conventions match the present workflow at a design level. RimTest Redux is a unit-test framework requiring test-only assemblies; it is not a substitute for Steam artifact provenance or Pickle's live loading. **Real Windows Pickle C# compilation/runtime is not yet validated.** Avoid further speculative harness rewrites until the release-source blocker is resolved. The official RimWorld `loadFolders.xml` semantics document confirms `IfModActive` loads an additional path when the target Mod is active.

### ENV-STEAM-2-METADATA-VARIANCES-20261008 — author test-run.bat preflight diagnosis

**Owner:** Environment Workshop distribution / Pickle E2E
**Status:** STRICT 32/32 BYTE MANIFEST FAIL CONFIRMED; METADATA SEMANTIC AUDIT MERGED; STEAM PICKLE RUNTIME NOT YET PASSED; RELEASE HOLD

Author ran `test-run.bat` against actual installed Workshop `3814638060`. The tool compared `%TEMP%/AMJE-Final-20261008-214525/Manifest.json` with the installed root and stopped **before game start**: `missing=[]`, `extra=[]`, `changed=['About/PublishedFileId.txt', 'loadFolders.xml']`. Therefore 30 of 32 files were SHA256-identical to the approved candidate. Exact 32/32-byte provenance is **not established**. The actual contents of the two differing files were not supplied; do not assert that their meaning is unchanged before running the semantic gate.

PR #39 merged as `c6f674a2237dcc739299d59408f4a9c16271a6d8` (CI `Workshop payload filtering` and `Plant visual coverage ledger` both PASS on final PR head). Its `Scripts/Run-WorkshopPickle.py` now recognizes only two **explicit, narrowly scoped** metadata format deviations: `PublishedFileId.txt` must decode to exact numeric Workshop ID `3814638060`, and `loadFolders.xml` must parse into an identical root-only RimWorld 1.6 load-folder tree. Missing/extra files, any other SHA256 mismatch, noncanonical approved manifest ID/XML, changed XML structure/load paths, or wrong ID remain hard failures. On passing metadata semantics, the production subscriber validator runs on actual downloaded content with only equivalent metadata normalized **in memory**; the installed Mod, manifest and release candidate are never modified. Pickle report records `manifest_audit.byte_identical=false` and the differing files; it **does not clear** `steam_release_cleared=false`. The preexisting strict `Build-WorkshopPayload.py verify` and the full rendered/native cutting distribution release gate remain unchanged.

Next human action: synchronize local Environment checkout and run **only `test-run.bat`** with RimWorld normally closed. If semantic gate rejects either metadata file, its automatic diagnostic is the next evidence; do not weaken the acceptance gate or ask the author for manual file-by-file comparisons. If Pickle runs and passes, record its 20 loaded-Def checks separately; 9-map/native-cutting actual Steam runtime and exact provenance remain outstanding. No new Steam upload authorized by this record.

### ENV-WORKSHOP-BATCH-ENTRY-20261008 — author-facing test-run.bat

**Owner:** Environment testing / publication
**Status:** ONE-COMMAND BATCH ENTRY MERGED; STATIC CI TWO PASS; WINDOWS PICKLE E2E STILL UNVERIFIED; DISTRIBUTED RELEASE HOLD

Author explicitly requested the same batch-first test practice as other AMJ workstreams: `test-run.bat` must be the user-facing entry instead of assembling `py`, timestamps, Manifest or report paths. PR #38 squash-merged as `62ac577f7ce030b2dafc8ec59db8ed1e4bf417e4`. The root-level `test-run.bat` internally launches `Scripts/Run-WorkshopPickle.py`, which finds Workshop3814638060, chooses only an **exact byte-matching** `%TEMP%/AMJE-Final-*/Manifest.json`, makes a fresh timestamped output, executes real RimWorks Pickle for the four loaded-Def smoke profiles and automatically prints failures and available report/log excerpts. Missing or mismatching Steam manifest and an already-running RimWorld are blockers, not runtime PASS. The existing `run-tests.bat` remains the development static/rendered test entry; `test-run.bat` covers subscribed Workshop Pickle verification.

Regression tests cover the batch's zero-flag interface, fail-closed manifest selection and absent-result diagnostics; GitHub `Workshop payload filtering` and `Plant visual coverage ledger` passed on PR head. No game code/assets/published Workshop bytes or local config were altered by this change. This is **test infrastructure only**: Windows C# compilation and 20 Pickle runtime scenarios have not yet been author-executed, and Pickle alone does not validate actual native CutPlant jobs, nine rendered maps or complete Steam publication. Preserve earlier valid 32-file candidate evidence; Steam runtime release HOLD remains.

### ENV-WORKSHOP-PICKLE-E2E-20261008 — downloaded source and loaded-Def audit

**Owner:** Environment runtime testing / publication  
**Status:** PICKLE HARNESS MERGED; STATIC CI PASS; WINDOWS RUNTIME NOT YET EXECUTED; DISTRIBUTION HOLD

PR #37 was squash-merged as `1967bd4626f3169f3d0f13c2eb09789033dcbd1d`. This adds
`Scripts/Run-WorkshopPickle.py`, test-only
`Tests/Pickle/EnvironmentWorkshopSteps.cs`, Gherkin feature file and the
`Docs/GoldenPaths/WorkshopPickleTests.md` runnable procedure. It uses
**RimWorks Pickle** for five downloaded-Workshop loaded-source/plant Def/
biome-exclusion/representative-commonality/Vanilla+MO harvest-*Def*
scenarios across four isolated profiles. Active MO/CCTO membership, real
Steam root/assembly, Unity ERROR observer, exact external manifest,
untouched original Workshop bytes and normal RimWorld config are guards.
GitHub `Workshop payload filtering` and `Plant visual coverage ledger`
CI both passed on the PR head before merge. No production runtime code,
XML, images, Workshop upload or candidate bytes were changed.

**Limit:** Pickle test assemblies have not yet been compiled in the author's
Windows RimWorld environment and the new Pickle E2E scenarios have not yet
run there. The prior `Steam-Runtime-20261008-233148` missing result directory
does not establish a gameplay failure; the older Quickstarts runner can
fail its preflight before creating that directory (e.g. if RimWorld is
still open), but its exact cause was not captured. Do not falsely claim
success or silently replace the existing 9-map+native-cutting rendered
runtime gate with five loaded-Def Pickle checks. Author's previously
verified **32-file candidate** and the 40-report saved-log audit stay
valid; actual subscribed-root runtime evidence is still required.

### ENV-STEAM-CHANGE-NOTES-20261008 — author confirmation

**Owner:** Environment release/publication
**Status:** STEAM CHANGE NOTES VISIBLE (author-confirmed); SUBSCRIBER PAYLOAD IDENTITY/RUNTIME STILL HOLD

The author confirmed on 2026-10-08 JST that the prepared update history appeared in Steam Workshop Change Notes for the current Environment update. This provides **author-reported live Steam UI evidence** that the Add Changenote publication pathway produced the intended type of update entry on this occasion; unlike earlier publication history, it was not only auto-generated boilerplate. The text was prepared in `About/Changelog.txt` using matching `0.1.0` About/Manifest/version metadata. Exact Steam entry wording and the Steam log were not independently supplied here. Steam-side description and gallery were not separately confirmed.

Do not conflate successful Change Notes display with exact uploaded game files: the current 32-file candidate from `1d46727102f53243099927b59881091d448dba61` and archive `a6df959f18a1a72153ba5dfbc196806b646194e452fec2e8f8c71d312635b0f6` passed four-profile prepublication tests and a 40-report saved-log audit, but a **fresh actual Steam download inventory, candidate-manifest comparison, source/DLL proof and downloaded-root four-profile gate** have not yet been provided. Distributed release HOLD remains for these checks. No further upload or gameplay edits are authorized by this note.

### ENV-WORKSHOP-CANDIDATE-FOUR-PROFILE-PASS-20261008 — current 32-file candidate

**Owner:** Environment Workshop release  
**Status:** CANDIDATE RUNTIME PASS (author-supplied structured summary); STEAM PUBLICATION/DOWNLOAD NOT VERIFIED; RELEASE HOLD

The author ran the corrected 9-map + 1-cutting release suite in all four profiles against the **same 32-file candidate** built from source commit `1d46727102f53243099927b59881091d448dba61`; its archive SHA256 is `a6df959f18a1a72153ba5dfbc196806b646194e452fec2e8f8c71d312635b0f6`. The test-only harness was corrected through merge `f39914d5bbd02edb90f7c91fe5faaf6e645d86fd`. Author-supplied `Summary.json`: `passed=true`, `full_four_profile_gate=true`; Vanilla409 map/15 cutting, MO409/15, CCTO457/15, MO+CCTO457/15; ten reports and zero runtime errors per profile. Author-supplied `Preservation.json`: `payload_unchanged=true`, `normal_config_unchanged=true`. The old expected-count and omitted-wetland/Unity-report-count failures were in the test harness; the production payload was **not rebuilt**.

Author's preserved runtime evidence is at `C:/Users/sucRo/AppData/Local/Temp/AMJE-Final-20261008-214525/Runtime-Final-20261008-224132`. The author then executed `Build-WorkshopPayload.py verify`, returning `PASS exact payload: 32 files; source 1d46727102f53243099927b59881091d448dba61`, and `Combine-WorkshopPayloadResults.py` on those saved results, returning `PASS four-profile source/render/cutting/error/preservation gate`. The supplied `FourProfileAudit.json` has `passed=true`, matching pinned source and archive hashes, lists 40 checked scenario reports (ten per profile) and reports `steam_release_cleared=false`. This independent **local tool-based** re-audit checks detailed logs, Unity captures, selected source/DLL/Def roots, genuine dependencies, Direct3D, enabled profiles, four native outputs and original payload/config preservation; the individual raw logs were not separately uploaded to this conversation. The candidate fixture changed only About name/packageId for testing; original upload identity remains intact. The actual Mod folder selected in the game publisher still needs exact-root/subscriber verification before manual upload.

**Steam remains author-manual.** Enable YADA and Add Changenote and select the validated 32-file candidate, never the dirty developer tree. A real downloaded Workshop3814638060 manifest/hash/source/DLL and four-profile runtime check are still required before claiming distributed-release PASS. Stable combined-loop/save-season criteria remain OPEN. Historical 26/28-file candidate records below are not an authorization to upload those obsolete candidates.

### ENV-WORKSHOP-BILINGUAL-CLOSEOUT-20261008 — single-field English/Japanese Workshop copy

**Owner:** Environment public copy and release  
**Status:** GITHUB SOURCE PREPARED; STEAM POSTING NOT PERFORMED; AUTOMATED CI RESULT PENDING

The owner source `Docs/SteamWorkshopDescription.txt` now contains one complete English-then-Japanese description followed by one common plant-image gallery with four distinct images and EN/JA captions; `Docs/SteamWorkshopDescription-ja.txt` is its maintained Japanese draft, not a second Steam field. The full combined body fits the 8,000-byte budget and omits license/AI/donation statements. `Docs/WorkshopDescription.md` defines the paste workflow. `Tests/validate_workshop_description.py` is integrated into the Workshop payload CI to prevent regressions. About title, packageId, gameplay code/defs, image assets and Steam live posting are unchanged by this description correction. Confirm CI separately; final Steam posting remains author-managed.

### MEDICINE-SPLIT-20261008 — latest author decision supersedes herb workstream

**Status:** DONE — ownership/design records; medicine implementation not started.

Environment retains wild/cultivated Healroot and existing MedicineHerbal supply. No medicinal-plant replacement or final Healroot removal is planned. Yomogi/Kuzu plants, Kuzu-root harvest, raw drugs and remedies belong to the future standalone medicine Mod; Fiber keeps stem processing. Research and approved text moved to [Project medicine record](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/MedicinalPlantsAndRemedies.md). Design, retention audit and Step 3 updated. All older herb-owner/Healroot-removal entries below are historical and superseded. Public-copy reconciliation of old future-roadmap promises remains OPEN for the ongoing public-description workstream; no runtime/art/Steam change or new runtime PASS claimed. Golden Path N/A.

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

### ENV-KUZU-MEDICINAL-OWNER-20261008 — kudzu root / stem-fiber boundary

**Owner:** Environment / Step 3 traditional herbs  
**Status:** OWNERSHIP DONE; IMPLEMENTATION OPEN — no new PlantDef, yield, art or runtime result

Author assigned unowned kudzu → medicinal root to Environment with Yomogi, and stem → fiber to the future fiber Mod. Project's deferred-plant record had no root owner. Formal sources: `Docs/NativeVegetationStep3Design-ja.md` section 8 and `Docs/Design.md` 11.5.10. Root harvest and medicinal use are a required herbal slice; decorative-only addition is not completion. Other herbs remain candidates; Edo-centered Gen-no-shoko medicine history needs pre-Edo evidence before adoption. Existing Yoshi-first route, Japanese/art approval gates and Healroot supply safeguards remain.

Fiber has no repo yet; Project keeps its formal stem/fiber record and handoff. Avoid duplicate plants and fiber output overriding root supply. Description, art, harvest mechanism, historical source audit and six-Biome medical supply verification remain pending. This documentation change is not Step 3 runtime PASS or publication.

### ENV-STEP3-NATIVE-VEGETATION-20261008 — new native wetland plants and mugwort

**Owner:** Environment / native vegetation and medicine-supply boundary
**Status:** IN PROGRESS — design/prior-art groundwork merged, new species and art NOT IMPLEMENTED

2026-10-08: Step 2 tree-description work was author-confirmed PASS and closed, so Step 3's missing Japan-appropriate wetland vegetation and traditional herbs is now the active scoped workstream. [PR #32](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/pull/32) squash-merged as `def9a2d17d6b3030b52d675718fadda2dc6aacf3`, after successful Workshop-payload PR CI. Formal design source of truth: `Docs/NativeVegetationStep3Design-ja.md`, referenced by `Docs/Design.md` 11.5.10. It covers VE-first overlap, selected prospective reed/sedge/alder/sphagnum/mugwort owners, two wetland commonality totals 7.30/8.22 and woody totals 3.00/1.80, controlled redistribution of Vanilla proxies, per-biome habitat and tree-sowing checks, historical Japanese drafts, art asset gate, automated Quickstarts/error gate and no premature medicinal yield.

**Immediate implementation route:** Start with one Japanese reed (`AMJ_Plant_Yoshi`) on a separate PR only after approved Japanese description and approved source PNG exist; then handle other wetland species and Yomogi as independently auditable slices. The original candidates, commonality transfers, Japanese draft text and harvest mechanics are NOT yet author-approved or implemented. ENV-KUZU-MEDICINAL-OWNER-20261008 confirms author-assigned Kuzu-root ownership and scope, not detailed mechanics or assets. Do not translate unapproved drafts or create live Def references to missing textures. Existing four AMJE plant visual coverage rows remain complete; legacy-tree retextures are still deferred and new plant visual reviews are separate.

**Critical safeguard:** Wild `Plant_HealrootWild` remains in all six currently covered Biomes until separate Step 4 full harvest/supply verification. Yomogi proposed only for warm/cool temperate and cannot itself replace medicine collection in subalpine, alpine or wetlands. One successful standard `run-tests.bat` for Step 2 does not prove Step 3's new Defs or the separate Vanilla/MO/CCTO/MO+CCTO four-profile matrix. No Steam publication is claimed.

### ENV-WETLAND-NATURAL-DISTRIBUTION-002 — independent natural-world wetland gate (2026-10-08 JST)

**Owner:** Environment / world distribution test  
**Status:** DONE — [PR #27](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/pull/27) merged (`7264c5e101302a5a5d12d479af08c8666603a4a6`); new runtime gate author-reported PASS, detailed reports not attached

- PR #27 (original commit `ae2611ba2296fbbe38aa754351d9e4d86c6ccb57`, merged into main as `7264c5e101302a5a5d12d479af08c8666603a4a6`) adds `AMJWorldWetlandDistributionQuickstart` to the standalone standard suite, increasing base coverage from eight to nine scenarios. The existing eight remain unchanged.
- The test generates a 30%-coverage world with deterministic seed, **never forces a biome**, counts actual `TemperateSwamp` / `ColdBog` against `swampiness>=0.5` candidates, and verifies at least one naturally selected wetland settlement tile. A 20% land-share ceiling is a deliberately broad regression alarm, **not** an approved historical ecological target.
- All five PR checks (two PowerShell, tree-sowing contract, plant visual coverage, Workshop payload) returned **SUCCESS**. These are static/infrastructure results, not installed RimWorld runtime evidence.
- Follow-up PR #27 commit `76fa4435045414426f492dd47d7cfd9e6ceb3a0e` optimizes settlement-suitability probing across the 30%-coverage world, adds `candidateShare` and `wetlandOfCandidates` diagnostic fields, and gives only the ninth scenario a 420-second timeout. The original eight retain their default timeout. Updated branch CI and post-merge CI are **5/5 PASS**. The author reported the new test passed; its detailed runtime results were not attached.
- **Acceptance on 2026-10-08:** author explicitly reported that the new test passed; the PR was merged only after this report and successful static CI. This is **author-reported runtime PASS**, not independent validation of the nine `.json`/`.log` reports. Exact assertion counts, world wetland candidate/selected shares, live-capture completeness and the owned ERROR gate are not independently audited without those artifacts. Never infer a numerical wetland share or a calibrated historical frequency from the report. The distinct four-profile matrix remains open.
- Confirmed design/test intent belongs to `Docs/VanillaWetlandBiomeAudit-ja.md`, `Docs/Design.md` and `Docs/GoldenPaths/RenderedRuntimeTests.md` in that PR. `ENV-WETLAND-BIOME-001` baseline Step 1 is accepted on author-reported runtime PASS after PR #28; independently inspected runtime logs, precise wetland-share calibration and the four-profile matrix remain separate.

### TEST-ENV-UNIFIED-20261008 — unify parallel AMJE test and wetland development tracks

**Requested by:** author (2026-10-08 JST)  
**Owner:** Environment / runtime testing and wetland development — one shared handoff  
**Status:** IN PROGRESS — prior base 8/8 and CCTO runner PASS evidenced; newer 9-scenario, updated wetland-description test and Highland-fix Grains/Core integration author-reported PASS; detailed world-share numbers, independent updated runtime logs and four-profile matrix pending

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

## Release handoff — first public Beta

- **Current priority:** close the current vegetation/runtime work before the next author-manual Workshop update. `ENV-PLANT-AUDIT-001` supersedes the older Beta-publication-before-vegetation-audit sequence: the forest/wetland retention/removal pass is already complete; Wild Healroot remains intentionally temporary and is not a pre-upload blocker.
- **Runtime gate:** historical runtime results remain historical evidence. The later distribution/wetland assertions and `ENV-TREE-SOWING-001` require the fresh non-visible Vanilla/MO/CCTO/MO+CCTO vegetation matrix with complete capture and pre-launch/runtime ERROR gates; older PASS counts do not close this new gate.
- **Workshop presentation:** cover / preview artwork is already complete in a separate workstream but is not stored in this repository yet. Repository-side README / BBCode / localization can be prepared by agents, but the actual Steam Workshop publication/update is performed manually by the author. Do not mark Steam as updated without author confirmation.
- **Public copy:** README, About.xml, and Japanese/English Workshop source now use Beta release wording. The internal Design document's "Alpha" baselines remain historical/design-stage terminology and are not public release-stage labels.
- **Local publication staging:** the current publication workstream uses `D:\\SteamLibrary\\steamapps\\common\\RimWorld\\Mods\\_AMJ_PublishStaging`; local staging state may be newer than GitHub main and must not be reconstructed from GitHub alone.
- **Vegetation follow-up:** follow the latest `ENV-PLANT-AUDIT-001` roadmap: completed unsuitable-vegetation removal → retained-description audit/rewrite before retexture → missing historical vegetation additions → final Wild Healroot cleanup after replacements and gathering balance. Re-audit retention when new evidence warrants it; do not restart the completed pass merely because an older handoff called it post-publication work.

## Active tree-art handoff

- **Current stage:** PR #5 merged after final native leafless-beech snow approval. All current AMJE structural-plant visual states are accepted/complete.
- **Next target:** current vegetation/runtime closeout under `ENV-PLANT-AUDIT-001` and `ENV-TREE-SOWING-001`; retained Vanilla descriptions precede later retexture. The older Beta-first/retention-audit-later handoff is superseded. Accepted current AMJE art remains complete; this does not claim completion of future vegetation stages or new runtime checks.
- **Approval state:** leafless snow revision 5 source accepted with `これで妥協する`, then native appearance accepted with `ブナOKなのでPRマージして`; template v5 is ACTIVE and the plant visual coverage ledger has no pending AMJE plant state.
- **Mandatory restart entry:** `Docs/GoldenPaths/RetextureGeneration.md`; canonical visual rules in `Docs/ArtDirection.md`; production handling in `Docs/GoldenPaths/TextureAssetPipeline.md`.
- **Reference state:** accepted Sudajii / leafy beech / leafless beech remain unchanged. Shirabiso production path: `Textures/Things/Plant/AMJ/Shirabiso/Shirabiso_A.png`.

## Current coordination items

### ENV-WORKSHOP-CHANGELOG-001 — Git-managed Steam Change Notes

**Requested by:** author (2026-10-08 JST)  
**Owner:** Environment release/publication  
**Status:** DONE — repository workflow ready; next Steam publication remains author-manual

AMJE now tracks Workshop change-note text in Git. Merge
`312b20ce39b3e0c3bfb2f8d1b510eb70feb1c560` added
`About/Manifest.xml`, `About/Changelog.txt`, matching
`About.xml <modVersion>`, payload regression checks and the publication Golden
Path. The first tracked publication version is `0.1.0`; earlier Workshop
uploads are not retroactively versioned.

For the author's manual RimWorld upload, enable both YADA and Add Changenote.
The selected pinned payload must retain Manifest/Changelog and pass the Workshop
payload validators. Add Changenote then replaces RimWorld's auto-generated
change note with the current version block. Post-upload verification must still
confirm the actual Steam Change Notes page; repository preparation is not proof
that Steam was updated.

### ENV-WETLAND-BIOME-001 — retained Vanilla wetland whole-Biome audit and correction

**Requested by:** author (2026-10-08 JST)  
**Owner:** Environment / biome / vegetation / wildlife  
**Status:** BASELINE DONE (author-confirmed) — biome corrections, bilingual descriptions and Step 1 regression gates implemented; updated runtime PASS reported, independent log audit and four-profile release matrix separate

Whole-Biome audit completed in PR #18 / squash merge `34fce92e5b6bf6da24e38355e475623ada664d66`.

Decision:
- retain `TemperateSwamp` / `ColdBog` as generic compatible wetland DefNames rather than creating AMJE-only replacements;
- retain their basic wetland Worker/terrain-generation role unless implementation testing finds a concrete problem;
- current Phase 5 plant-pool cleanup remains valid but is not sufficient to close roadmap Step 1;
- patch wetland `wildAnimals` to the AMJE Japan-oriented proxy policy so excluded animals cannot re-enter through Vanilla wetlands;
- audit/patch wetland diseases, weather, and descriptions instead of inheriting the Vanilla bundle unchanged;
- missing Japanese wetland plants (reed/sedge/alder/sphagnum candidates) remain Step 3, not a prerequisite for this correction;
- after implementation, require loaded-Def/runtime coverage for excluded plants/animals, retained wetland terrain generation, and world wetland-share sanity before unblocking ENV-RETEX-012.

Durable sources: `Docs/VanillaWetlandBiomeAudit-ja.md`, `Docs/Design.md` section 11.5.8.

**2026-10-08 closeout:** wildlife/disease/weather correction merged in PR #24, natural-world test in PR #27 (`7264c5e101302a5a5d12d479af08c8666603a4a6`), bilingual descriptions and loaded-text tests in PR #28 (`84d077d08e353972e6b7eaedd9139e2e47e6065d`). The author reported both successive runtime gates PASS. This accepts the **initial wetland Step 1 gate** as author-confirmed, not independently log-audited; ecological share percentages were not supplied. Do not conflate with the separate four-profile release matrix. Remaining plant-retention / Japanese-description audit can proceed, while existing-tree retexture is still deferred.

**Test entrypoint infrastructure (2026-10-08):** PR #19 / squash merge `d2b4d261c16481c748544661499b287b039a50d3` makes `run-tests.bat` the canonical AMJE automated gate. It now runs the static/build/source validation first, then launches the normal runtime Quickstart matrix on a rendering-enabled non-visible Windows desktop. The default runtime matrix already includes `AMJTemperateSwampVegetationQuickstart` and `AMJColdBogVegetationQuickstart`, so the forthcoming Step 1 wetland assertions belong in those scenarios and will be exercised automatically by `run-tests.bat`. This tooling merge does **not** claim the new wetland wildlife/disease/weather/terrain completion assertions are implemented or passing yet.


**Static runner handoff regression fixed (2026-10-08):** PR #21 / squash merge `2926162147b83d3574ee849777b1349df77c849a` resolved the stale `Validate-Environment.ps1` requirement that MO texture audit commands appear directly inside `run-tests.bat`. The validator now verifies that `run-tests.bat` delegates to `run-static-tests.bat`, where the MO audit actually runs, and checks that `run-runtime-tests.bat` uses `--skip-static` without recursion. `Tests/test_run_tests_entrypoint.py` covers the regression. The post-failure marker groups were also reviewed against current source. PR CI passed PowerShell syntax, Workshop payload, and plant visual coverage checks. This is a static harness correction only; no fresh Windows RimWorld runtime PASS or wetland Step 1 completion is claimed.


**2026-10-08 uploaded runtime failure correction:** PR #23 / squash merge `a2ea49aecf69c8975ca26e4b84b755ba488005ca` fixes two failures observed in the AMJWarmTemperateTerrainQuickstart report (73 of 74 assertions passed; one reported preLaunch ERROR). The failed ColdBog Cypress-specific XML PatchOperationReplace was replaced with complete approved wetland `wildPlants` pools for both inherited Vanilla wetlands, preserving Phase 5 total/woody commonality. The tree-sowing Quickstart now checks the base `map.Biome.wildPlants` instead of map-wide `WildPlantSpawner.AllWildPlants`, which can include coast/river mutator plants. The separate actual growing-zone menu assertion remains in place. `Tests/test_tree_sowing_contract.py` now checks the wetland patch's approved pools and runtime wiring; `Docs/GoldenPaths/PlantSowingTests.md` records the semantics. PR CI passed Regional tree sowing contract, Workshop payload filtering and Plant visual coverage ledger. No new installed-Windows RimWorld runtime result is available yet, and wetland wildlife/disease/weather/description work remains OPEN; do not close Step 1 on the basis of this merge.



**2026-10-08 ecology implementation:** PR #24 / squash merge `3e946ad24f4e5d93ced19478219b73f865befa54` adds `Patches/VanillaWetlandEcology.xml` for both retained Vanilla wetlands: Japan-oriented wildlife proxies, adjusted humid weather/snow bands, historical-world-facing gameplay disease mixes (excluding mechanites), separate disease MTB values, and removal of inherited foreign wild pack animals. Original wetland workers, terrain patch makers and Phase 5 wild plants remain unchanged. The `Tests/Quickstarts/EnvironmentBiomeTerrainQuickstarts.cs` wetland scenarios now require loaded Def/terrain assertions; `Tests/test_wetland_ecology_contract.py` statically guards all ten replaced fields. The full numeric provisional balance and **unapproved Japanese-first description drafts** are recorded in `Docs/VanillaWetlandBiomeAudit-ja.md` and `Docs/Design.md`. PR CI passed Regional tree sowing contract (including the ecology regression), Workshop payload, and Plant visual coverage. **OPEN:** new English/Japanese runtime descriptions require approval of the Japanese drafts before translation; installed RimWorld runtime and world wetland-share sanity remain unverified. Do not mark vegetation roadmap Step 1 DONE or unblock ENV-RETEX-012 on CI results alone.

### ENV-STEP2-PLANT-DESCRIPTION-20261008 — retained Vanilla plant descriptions (2026-10-08)

**Owner:** Environment / plant retention, historical localization
**Status:** IN PROGRESS — [PR #29](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/pull/29) merged as `8ee572e4a17e1a894472445db7e28755a6308aed`; Bamboo bilingual description and loaded check implemented, updated runtime gate author-provided PASS; other five drafts unapproved

- After the author-confirmed wetland Step 1, Step 2 audits the **current exact Vanilla wildPlant inventory** across AMJE's four biomes and two retained Vanilla wetlands: six tree Defs (Bamboo/Maple/Oak/Birch/Pine/Willow) and seven generic ground/forage Defs (Grass/TallGrass/Brambles/Bush/Moss/Berry/Wild Healroot). AMJE-owned four structural plants are separate.
- Draft PR #29 commit `c3b94ec46e7bade291b1bc9da42bc893ae9a1306` adds `Docs/VanillaPlantStep2DescriptionReview-ja.md` with an XML-grounded distribution matrix, six **unapproved Japanese historical description drafts**, supporting citations and explicit proxy/region/era caveats. `Docs/Design.md` and `Docs/VanillaPlantRetentionAudit-ja.md` point to this as the formal text source. Do not merge descriptions into DefInjected or translate prior to author's Japanese approval.
- Priority is Bamboo's misleading Vanilla aesthetic judgment and exclusion of post-Edo Moso bamboo assumptions. Pine secondary forest vs natural biome, and Willow/Maple/Birch in wetlands, require species/distribution caution. Preserve original Vanilla labels and gameplay until separately approved.
- **2026-10-08 Bamboo approval and implementation:** the author approved the exact two-paragraph Japanese `Plant_TreeBamboo` text. PR #29 branch commit `805527a976f00107bbd67229ea4a5c70fe6cb450` adds `Patches/VanillaTreeDescriptions.xml` (English description-only), Japanese `ThingDef` DefInjected, the warm-temperate loaded EN/JA Quickstart assertion, `Tests/test_vanilla_bamboo_description.py` and workflow triggers. The **branch static CI completed 3/3 SUCCESS**; this does not prove installed-game runtime.
- The other five Japanese tree descriptions are still unapproved/untranslated/unpatched. No vanilla labels, plant stats, natural distribution, sowing rules or art changed. Existing Vanilla/MO tree retexture remains deferred; current plant visuals still have precedence under AGENTS/PlantVisualCoverage.
- **2026-10-08 acceptance and merge:** author provided the end of `TestResults/EnvironmentIsolatedRuntime.log` after the updated Bamboo test run. Excerpt shows `[OK] AMJCoastMapHandoffQuickstart passed.`, `[OK] No owned AMJ runtime ERROR entries were found.`, `[OK] AMJ Core + Environment gameplay-contract Quickstarts passed.`, `[OK] Environment runtime gate passed.` and separate base/CCTO/Core reports folders. This **confirms the supplied overall runtime-gate PASS**, not individually inspected `AMJWarmTemperateTerrainQuickstart.json` or source revision checksum; do not invent Bamboo assertion counts. PR #29 merged after 4/4 CI success. The full Vanilla/MO/CCTO/MO+CCTO release matrix remains independent and OPEN.
- **Progress-visibility follow-up:** isolated launcher logs all internal `[WAIT]` and `[OK]` to `TestResults/EnvironmentIsolatedRuntime.log`, while the outer terminal prints `[WINDOWS]` only if the window/process set changes, creating a misleading apparent hang. The current Golden Path in `Docs/GoldenPaths/RenderedRuntimeTests.md` documents `Get-Content .\\TestResults\\EnvironmentIsolatedRuntime.log -Tail 30 -Wait` for live inspection. Future harness improvement: safe, single-pass real-time progress mirroring while preserving isolated rendering, ERROR gate and reliable logs; not implemented in this PR.
- Runtime PASS from PRs #27/#28 is historical evidence, not a new Step 2 change validation. The separate four-profile matrix and exact wetland-share calibration remain open. Existing tree-sowing tests govern plantability; do not assert wetland sowability from wildPlants alone.

### ENV-RETEX-012 — existing-tree retention audit and AMJE description rewrite

**Requested by:** author (2026-10-08 JST)  
**Owner:** Environment vegetation / localization / art  
**Status:** DESCRIPTION/RETENTION AUDIT UNBLOCKED — wetland Step 1 author-confirmed PASS; actual retained-tree retexture remains deferred

Before any broad Vanilla / Medieval Overhaul tree retexture pass, re-audit the trees and ground vegetation AMJE currently leaves in place. The previous rationale that human-created pine woodland / grassland / secondary forest justifies retaining Vanilla vegetation is rejected: those historical vegetation forms must themselves be represented by species and vegetation appropriate to ancient/medieval Japan. Do not assume an existing PlantDef should remain merely because it is already present. For each candidate, decide whether it belongs in AMJE's target region, pre-Edo scope, vegetation bands and landscape role; remove/replace/non-adopt targets that are unnecessary or inappropriate.

Only trees retained after that audit proceed to the later art pass. Their inherited Vanilla/MO descriptions must also be rewritten into the established AMJE plant-description format, Japanese-first, under the shared historical-description rules before English synchronization. The audit explicitly includes correcting culturally or historically mismatched inherited wording; the Vanilla bamboo wording that describes bamboo as not beautiful is a named review target.

Durable policy is recorded in `Docs/Design.md` sections **11.5.5–11.5.8**. The required order is **retention audit -> distribution/ownership decision -> Japanese description audit/rewrite -> author/content approval -> retexture**. Do not begin the retained-Vanilla art pass before description review. After the retained Vanilla set is complete, the roadmap proceeds to missing Japanese vegetation/medicinal plants and only then to final Wild Healroot removal.

The retained wetland wildlife, disease, weather and description bundles are now implemented; updated Step 1 runtime tests were reported PASS by the author on 2026-10-08. The **retention and Japanese-first description review may now start**, with individual descriptions still subject to author approval. Do not initiate existing-tree retexture: it remains separately deferred under `AGENTS.md` and pending current-plant visual Golden Path tasks. The four-profile matrix and detailed runtime log audit are separate unresolved checks.

### DOC-PUBLICCOPY-005 — public description wording alignment

**Requested by:** author (2026-10-07 JST)  
**Owner:** Documentation/release  
**Status:** DONE — repository-side public descriptions and wording rules aligned; Steam/2game publication remains manual

README, Japanese/English Workshop sources, 2game Japanese source, and About.xml were aligned around the current AMJE feature set. Public copy now explicitly identifies **Thin Soil / 痩せた土壌 (50% fertility)** as an added terrain while separately describing the higher-elevation poor/stony soil distribution. Japanese public copy uses Japanese general terminology instead of mixed `Vanilla / Biome / WorldGen / runtime / mutator` wording, while official Mod names and useful proper names remain unchanged.

Implementation/art provenance is no longer presented as a feature: the previous “custom/AMJE-authored graphics” promotional wording was removed. Public river/coast wording now describes compatibility/reuse of RimWorld's existing systems instead of exposing the internal River / Coast mutator terminology.

Durable wording rules are recorded in AMJE `AGENTS.md`, `Docs/WorkshopDescription.md`, and `Docs/2GamePresentation.md`, with the AMJ-common source updated in Grains `Docs/ModDescriptionGuidelines.md`. Japanese Workshop BBCode remained balanced and below the 8,000-byte limit under CRLF accounting. Actual Steam Workshop and 2game page updates are not claimed.


**2026-10-08 static-validator regression repair:** PR #22 / squash merge `5d48d6f82ab6520904bea754f63ab74196da1c51` removes stale mandatory Japanese/English Workshop wording `Ancient & Medieval Japan Core is not required` from `Scripts/Validate-Environment.ps1`. Workshop copy intentionally omits that redundant claim; the validator now checks its actual standalone wording, while preserving CCTO optionality and README/About compatibility assertions. `Tests/test_workshop_standalone_copy.py` prevents reintroducing the obsolete marker and checks CRLF-safe 8,000-byte Workshop sizes. PR CI: PowerShell syntax, Plant visual coverage and Workshop payload PASS. Public copy itself and production values were not changed. No Windows RimWorld runtime PASS claimed.

### DOC-WORKSHOP-004 — AMJE GitHub repository link

**Requested by:** author (2026-10-06 JST)

**Owner:** Documentation/release

**Status:** DONE — repository-side description sources updated; Steam update remains author-manual

Linked the existing README / Design reference in both Japanese and English Workshop sources to AMJE's own GitHub repository, once per language. README's research/design section now identifies the same repository explicitly. The Workshop-as-README-summary and named-mod-link policies remain unchanged; existing related-mod links and substantive claims are preserved.

Validation: UTF-8 description sizes remain below 8,000 bytes even with Windows CRLF line endings; one AMJE repository BBCode link per language, balanced URL tags, unchanged existing link targets, and clean diff whitespace. Golden Path N/A: trivial documentation-link edit with no new reusable procedure. No runtime or Steam-side update is claimed.

### ENV-010 — Final Environment art pass

**Requested by:** Environment/release readiness  
**Owner:** Environment/art  
**Status:** IN PROGRESS

All functional Alpha Environment gates are now complete, so the project can enter the previously deferred final-art stage.

Expanded minimum final-art scope after Vanilla/MO audit:
- 5 AMJE plant-state images: Sudajii, leafy Japanese beech, leafless Japanese beech, Shirabiso, Haimatsu;
- 12 Vanilla tree-state retextures used by AMJE / normal MO coexistence: Oak, Maple, Poplar, Birch, Willow (leafy + leafless), Pine, Bamboo;
- 7 optional Medieval Overhaul tree-state retextures: Great Oak, Great Iter, Great Willow (leafy + leafless), Great Fir;
- 1 seamless Thin Soil texture;
- 4 world-biome textures: WarmTemperate, CoolTemperate, Subalpine, Alpine.

Total minimum: **29 image assets**.

The retexture scope is visual only. Vanilla Def mechanics are unchanged; MO Def mechanics/content identity are unchanged and MO graphic-path patches apply only when MO is active.

Art policy:
- start with one final texture per plant/state; the existing `Graphic_Random` implementation can load a one-texture folder and accept more variants later;
- preserve Vanilla seasonal systems rather than baking snow/autumn state into evergreen/base textures;
- only switch Def paths after the corresponding binary asset actually exists;
- final visual review is intentionally manual for silhouette, scale, tiling, color/contrast, and world-map distinction;
- automated validation must continue to catch missing-texture/runtime ERRORs after each asset is integrated.

Art direction source of truth: `Docs/ArtDirection.md`; initial direction `6c310e7efb1fb0ae2a03e87cda7d803bffd398a8`, expanded Vanilla/MO retexture scope `d973f30d70235c6060e68fcd8aa574964454c6c5`.

**Current state:** the ENV-010 atlas direction is the accepted style baseline. The first production asset, Sudajii, has now been extracted/reworked from that accepted direction into an AMJE-owned transparent PNG and integrated at `Textures/Things/Plant/AMJ/Shii/Shii_A.png`.

Integration commits:
- binary Sudajii asset: `94898bcd1dff5023a21b559fab01b990b2f0e832`;
- `AMJ_Tree_Shii` switched from the Vanilla TreeOak placeholder folder to `Things/Plant/AMJ/Shii`: `fad9938eb10e8cff2e8d8381c9622a7b505c5a6b`;
- static missing-texture / placeholder-path regression check: `74883e61a5761baca1dd6b751019390171e97e08`.

**Verification status:** the normal build/static gate passes after the final Sudajii asset switch and validator fixes. The latest isolated runtime reports generated AMJE vegetation successfully and did not show a missing-texture error, but the runtime suite previously verified only that plant Defs/instances existed, not that their resolved material textures were non-BadTex.

Static-gate follow-up commits:
- UTF-8 BOM fix for Windows PowerShell 5.1 validator parsing: `b4d873dfe2a36b5f73a3d82870352d2a57a7e19f`;
- wildlife XML commonality parse fix: `d8c523b31431388467f52c6b1728dc8b67f09489`.

BadTex runtime coverage added:
- plant graphic assertions now resolve the live `Graphic.MatSingle.mainTexture` for Sudajii, Japanese beech leafy/leafless, Shirabiso and Haimatsu and require a non-null, non-`BaseContent.BadTex`/ERRORTEX texture: `37c89ea3f862d5d27dba5b8fb4d9eaa681fe6f0c`;
- static validator locks those runtime assertions in place: `3056060f3188d0f17930c5bcb4bf81dc72104fd0`.

Follow-up after the BadTex gate passed locally: the user's normal-play `Player.log` did not emit any standard missing-texture/BadTex diagnostic for the visible question-mark tree, so the audit scope is being widened beyond AMJE-owned plants.

Tree-audit extension:
- all loaded `ThingDef` trees now have their base graphic plus configured leafless/immature/polluted/snow-overlay states resolved and checked for null/BadTex/ERRORTEX; `Graphic_Collection` variants are inspected individually: `bbcf4ae47066d828fa59dc0b1f84de84798823f2`;
- isolated runtime profiles can now include Medieval Overhaul with its required Vanilla Expanded Framework and Processor Framework dependencies: `f302316f7f14ecd3a73d9343d68d0e83a1216cfa`;
- focused tree-texture audit runner mode: `4e2c06b2071fd6d3e237ffed5bb3dfd56ee4369c`;
- `run-runtime-tests.bat` automatically runs the optional MO profile when MO and both required framework Workshop mods are installed: `7985593955576535ea4b9eb251facb1a057ddccc`, with CCTO failure preservation fix `8a16eb847904458b45835bb8919de9ca393c0326`;
- static validator locks the expanded audit harness in place: `11c69e2cec9c873d508e7d1d903f14d37aef86da`.

Expanded audit first-run result: the AMJE-only profile failed on `Plant_TreePine:polluted=Things/Plant/TreePine_Polluted`. This was a test-harness false positive, not evidence of the visible question-mark tree: RimWorld only loads `pollutedGraphic` when Biotech is active, while the isolated AMJE-only profile does not activate Biotech. The audit now checks the polluted tree state only when `ModsConfig.BiotechActive` is true: `16f5dbb583ed7ec9a98b90687d3508df61677a15`; the static validator locks this condition in place: `01d35282578bebf2bed52b5d859cc2e6a62d51d7`.

**Verification status:** previous AMJE-only/CCTO BadTex gates passed locally; the corrected all-tree audit proceeds into the optional Medieval Overhaul profile, but the first focused MO run exceeded the original 180-second outer timeout before producing a report. This is currently treated as a harness-time-budget issue rather than a tree-texture failure because no `BAD def=...` result was produced.

MO runtime-harness follow-up:
- the runner now prints a 15-second `[WAIT]` heartbeat while RimWorld is still alive, so slow startup is distinguishable from a frozen console: `1bbb382ce4702e82871b10bdcc2da1bf39ed099b`;
- the focused Medieval Overhaul profile timeout is extended from 180 to 360 seconds: `e22e76922e9de519ce621e5e7e8447c0b573e950`;
- static validation locks both the MO-specific 360-second timeout and heartbeat marker in place: `31e291906c3a421b0b65443d2e832c129da9bb9b`.

Second MO runtime attempt also exceeded the 360-second outer timeout. The previous MO timeout log stopped during very early mod loading, before Quickstarts/AMJE initialization, so repeatedly extending the timeout is not a useful default gate. The MO tree-link check is therefore moved to a deterministic static audit of the installed MO 1.6 Defs and texture files.

MO timeout pivot:
- new `Scripts/Validate-MedievalOverhaulTreeTextures.ps1` scans installed MO 1.6 `TreeBase` / `DeciduousTreeBase` Defs and checks base plus configured leafless/immature/polluted/snow-state texture paths against MO texture roots: `37cfa2cafe8dc88db597bd549d0ba9f7c7457363`, with PowerShell variable-delimiting fix `2fc70cee687356afcb585d81b8092727b7804936`;
- the static MO audit now runs from `run-tests.bat`: `aeefd28ded6fab2d26dd8d85b016183860764d30`;
- the hanging isolated MO runtime profile is removed from the default runtime gate; AMJE and CCTO runtime checks remain unchanged: `a7fe02a2e739874b0274dd14952dad06a931d954`;
- static validation locks the new audit path in place: `6d2fb435ee1cd2deefb5e5b8d97ffd038ae6630c`.

An offline audit of the supplied MO 1.6 source archive found all 15 tree graphic references from the MO tree Defs resolving to PNG assets, including the four Dark Forest great trees and the four fruit-tree base/immature states. This reduces the likelihood that the visible question-mark tree is a simple missing file in MO's own current tree Defs.

The revised static gate and normal runtime gate now both pass locally after removing the hanging MO minimal-runtime profile. This confirms AMJE-owned plant graphics, the normal AMJE/CCTO runtime path, and installed MO tree source texture references all pass the current automated coverage.

To minimize manual diagnosis of the still-visible question-mark object, a Dev Mode live-map diagnostic has been added. It scans the actual current map things under the user's normal mod list, resolves each live `thing.Graphic` material (plus plant snow overlays), and logs the exact `defName`, label, map position, graphic class/path and texture name for null/BadTex/ERRORTEX cases: `f3d26cc9142c16f857a26aa3d71deb9484b16686`. Static validation locks this diagnostic in place: `d2ebf5437121b2da8b564973e00675d4e16856b6`.

The user clarified that the red question-mark textures were visible during the automated Quickstarts themselves, including both the first WarmTemperate run and the final Alpine run. Video review shows many question-mark tiles/objects across the map rather than one isolated tree. Since the existing live-plant audit still passed, the diagnosis has been widened beyond plants.

Quickstart live-object audit extension:
- every fixed-biome Quickstart now scans live non-plant map Things with standard `graphicData`, resolves the actual `thing.Graphic.MatAt(...)`, and logs exact `defName`, label, category, position, runtime type, graphic class/path and texture for null/BadTex/ERRORTEX cases: `4fc048be9ad3cc8afa62609505d000df2f7bc970`;
- static validation locks the new `[AMJ Environment LiveThingTextureAudit] BAD` assertion path in place: `56bb08d616a2da200758bf64c8e86d656a53be26`.

The visible question marks still appear in WarmTemperate while both live-plant and live-non-plant Thing audits pass. Video review shows repeated question-mark decals distributed over open ground rather than attached to ordinary Things. RimWorld's `SectionLayer_TerrainScatter` renders `ScatterableDef` materials directly from each terrain's `scatterType`, outside `map.listerThings`; its generated scatter points use a five-cell minimum spacing, which matches the observed repeated ground pattern. AMJ Thin Soil currently uses `scatterType=Rocky`, so terrain scatter is now the leading candidate.

Terrain-scatter diagnostic extension:
- each fixed-biome Quickstart now collects the scatter types actually present on map terrain, checks every referenced loaded `ScatterableDef.mat.mainTexture` for null/BadTex/ERRORTEX, and logs exact defName/scatterType/texturePath on failure: `6de0ed9adcde6823d6f71d6e34d09744b695c035`;
- static validation locks the new `[AMJ Environment TerrainScatterTextureAudit] BAD` path in place: `b6b6d3db2e18270d99200175639c9852e725962a`.

The user reran WarmTemperate with visible red question marks still present, yet the plant, non-plant Thing, and terrain-scatter audits all passed. This rules out their source materials at the data-object level, but not the actual material handed to RimWorld's map section renderer.

Render-pipeline diagnostic extension:
- the developer Quicktest assembly now Harmony-patches `MapDrawLayer.GetSubMesh(Material)`, the common path used by static map section layers (terrain, terrain scatter, map-mesh Things, etc.), and emits `[AMJ Environment BadRenderMaterial]` as an ERROR whenever the actual render material is `BaseContent.BadMat` or uses `BaseContent.BadTex`/ERRORTEX: `a58053f875131a90f4f859723db4ce251015c0df`;
- Quicktest compilation now explicitly references Harmony: `b93f50402e91c4554fa70e506ced15181d2a2046`;
- first local build after this change failed because `Environment.GetEnvironmentVariable` was resolved against the enclosing `AncientMedievalJapan.Environment` namespace instead of `System.Environment`; fixed by fully qualifying the runtime API: `77968bd4d566530713f359a6a00ee341b3e308f1`;
- static validation locks the render-material interceptor, Harmony reference, and fully-qualified environment lookup in place: `8f924aa69154b1e0fde3b479ebfc75ca9ee87466`, `9b970172077dd784963251220fcedb510edb9249`.

This is intentionally lower-level than the previous Def/Thing/scatter checks. The first run with the interceptor did not find a bad submesh material; instead the diagnostic itself caused Unity log flooding by reading `Material.mainTexture` on built-in shader materials that legitimately do not expose `_MainTex` (for example SunShadowFade, EdgeShadow, masks, lighting overlays, and water-depth materials). Quickstarts therefore reported all 54 assertions passing while the overall report failed with 10,000 captured log errors.

Harness correction:
- render-material probing now first checks `material.HasProperty("_MainTex")`; materials without that shader property are skipped instead of invoking Unity's error-producing getter: `adff36a11f26f27937a0a7cde1b121363ab4d2e7`;
- static validation locks the safe `_MainTex` guard in place: `d80c81249e0e2df76cfd7ddcab49dfd1674369d1`.

The corrected submesh-material interceptor now runs cleanly: WarmTemperate still visibly shows red question-mark placeholders, but the full Environment runtime gate passes, including AMJE+CCTO. This means the question marks are not being exposed as BadTex on the final static `MapDrawLayer.GetSubMesh` materials. A likely remaining gap is atlas substitution: static sprite paths can pass a BadTex source material into `Graphic.TryGetTextureAtlasReplacementInfo` and then hand a valid atlas material to the section layer, masking the original BadTex from the previous interceptor. Realtime Thing/Mote-style draws also bypass map submesh generation.

Deeper render-path tracing:
- Quicktests now intercept `Graphic.TryGetTextureAtlasReplacementInfo` before atlas substitution and fail/log `[AMJ Environment PreAtlasBadTexture]` if the source material is BadTex;
- Quicktests also intercept `Graphic.Draw` and `Graphic.DrawFromDef` to catch realtime Thing/Mote-style BadTex draws with defName, label, position, runtime type and graphic path;
- implementation: `7ccd9b38c5c69580a961c672e086b3abb41ed4e5`;
- static validation locks all three new trace markers/assertions in place: `cf29a5d60da56539a9a76f197b0d4d7a726811a5`.

Latest WarmTemperate rerun triggered the new realtime tracer, but the three hits were all intentional pawn shadow graphics: Human, YorkshireTerrier, and Hare were reported as `Verse.Graphic_Shadow` with `BadTexture`. This is a harness false positive. Vanilla `Graphic_Shadow` inherits the base `Graphic.MatAt/MatSingle`, which return `BaseContent.BadMat`, but `Graphic_Shadow.DrawWorker` never renders that material; it renders `MatBases.SunShadowFade` directly. The visible question marks therefore remain unexplained by these three errors.

Harness correction:
- realtime tracing now skips `Graphic_Shadow` before probing `MatAt`, and the assertion explicitly refers to non-shadow BadTex materials: `42ee30a048316ecd00dd5f70b59aa1aa892b380c`;
- static validation locks the Graphic_Shadow false-positive guard in place: `b7692029f3751ac7e1a3c72a5210877fc3508f3c`.

The user confirmed the visible red question marks still appear after excluding the intentional `Graphic_Shadow` sentinel, while the runtime gate passes. The informational line about Medieval Overhaul is expected: this Quickstart profile does not load MO at runtime, and MO tree-path coverage remains static-only. It is not evidence that the visible placeholders come from MO.

Video/frame review shows many roughly pawn/cell-sized red-question-mark placeholders distributed across the generated map. The prior generic `Graphic.Draw` tracer does not cover RimWorld 1.6 pawn bodies: pawn rendering uses `PawnRenderNodeWorker.GetFinalizedMaterial` and the render-tree pipeline directly. This is now the next targeted gap rather than immediately blaming Flecks.

Pawn render-node diagnostic:
- Quicktests now Harmony-postfix `PawnRenderNodeWorker.GetFinalizedMaterial` and emit `[AMJ Environment PawnRenderBadMaterial]` with pawn def/kind, label, position, node class, primary graphic path, material/texture, and apparel/hediff/gene context if an actual pawn render node resolves BadTex;
- implementation: `ddbf164838e166098f188ec9c337a51bbfdc81fa`;
- static validation locks the new pawn-render trace and assertion in place: `b8946fc2408f88ac5c006df791a4b1e593ba659c`.

The user noted that the placeholder count is visually too high to make ordinary pawns a strong explanation, and that the first WarmTemperate Quickstart reproduces the issue every run. For iterative diagnosis, repeatedly launching the full six-scenario runtime suite is unnecessary; keep the full suite as the final regression gate, but use a focused WarmTemperate-only interactive runner during root-cause investigation.

Focused interactive texture-debug runner:
- new `run-texture-debug.bat` builds/static-validates, prepares the same isolated Dev Mode profile, launches only `AMJWarmTemperateTerrainQuickstart`, and intentionally omits Quickstarts verify/report flags so RimWorld stays open until the user closes it manually;
- this lets the user select/inspect a visible red-question-mark object directly while all current quicktest Harmony diagnostics remain active through `RIMWORLD_QUICKSTART`;
- implementation: `cebf28d972452009098532225d30641719b53efd`;
- static validation ensures the helper stays WarmTemperate-only and does not accidentally add auto-exit `-quickstartreport` / `-quickstartverify` flags: `977beab5ea5e1ff07a9f5c7ed3cc0d27c237096f`.

The full `run-runtime-tests.bat` remains unchanged and should still be used once the defect is fixed to prove all biome + river/coast + optional CCTO runtime coverage.

The focused interactive run resolved the object identity: the user selected one of the visible red question marks and the inspect panel identified it as **Sudajii / `AMJ_Tree_Shii`**. The large number of placeholders is consistent with WarmTemperate's high Sudajii wild-plant commonality; the pawn-render hypothesis is no longer relevant to this defect.

Binary inspection then found the direct cause in `Textures/Things/Plant/AMJ/Shii/Shii_A.png`: the PNG had valid IHDR and PLTE chunks but a malformed transition after the indexed-color transparency (`tRNS`) chunk. Three stray bytes occurred before the valid IDAT chunk, so standard PNG chunk traversal failed and RimWorld's texture loader fell back to the red `BadTexture` placeholder. The image's IDAT payload itself was intact.

Fix and regression coverage:
- repaired the existing accepted Sudajii PNG without replacing the artwork; the malformed bytes were removed, the `tRNS` CRC was corrected, and the repaired file now parses as IHDR/PLTE/tRNS/IDAT/IEND with valid boundaries: `0421396e6184fdc12933d07db0bf88990b43ab9d`;
- Quicktest BadTex detection now also recognizes the runtime texture name `BadTexture`, and Sudajii's resolved UI icon is explicitly asserted non-BadTex: `1eacad99783d5e0de5e0a0cd84183053e2c4b85e`, follow-up collection-path guard `69013cbaa5cb3c01fe7ded3167953fb3c3e7439b`;
- static validation now checks PNG chunk structure, first for Sudajii and then recursively for all AMJE PNG assets so future generated art cannot silently ship a malformed image: `2f0a868fe438b8f54fde3476b48923af887a1867`, `d615543c312b429e37c8a3fb33c0f53538ea0a7d`.

**Next action:** pull/build and rerun `run-texture-debug.bat`. Confirm that Sudajii now renders normally both on the map and in its inspect/UI icon. Once confirmed, run the normal runtime gate once as the regression check, then continue ENV-010 with the remaining planned tree retextures and Japanese-first description review.

### DOC-001 — Shared public-description format and save compatibility

**Requested by:** author / public-description policy (2026-10-04 JST)  
**Owner:** Documentation/release  
**Status:** DONE (public source files prepared; Steam page not yet published)

All AMJ-related mod descriptions must include save compatibility. CCTO is the evolving format baseline. Durable shared policy: [Docs/ModDescriptionGuidelines.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Core/blob/main/Docs/ModDescriptionGuidelines.md). Addition/removal safety must reflect each mod's actual implementation; custom content and world-generation mods do not inherit CCTO's safe-removal claim.

About.xml now states the development build's save-compatibility limits; AGENTS.md points to the shared policy for future README/Workshop preparation.

Public source preparation is now complete:
- detailed README source: `fa998e2e0f8a690f6b0add09146d3247dd3366f4`;
- Japanese Workshop source written first: `c2686e93378b14aceb0270c758e1bebb9ec9325a`;
- English translation synchronized from the Japanese source: `105c181b69c1ce402a6467b5e9478d2a057a2db0`;
- Workshop presentation/authoring policy: `a3c951739bcbf5624009740c6b1c0df7014ce334`;
- About.xml Alpha positioning and save-compatibility wording: `72f93c6d93b2a9b9fb8e47a95ff54688b3e5f14e`;
- public-description static validation: `06f7aa605d6d4519fa2ae643fa18055847fbe9a0`, marker correction `274b9cac3f4544fab1b2dc49c5a7ec45134a8679`.

Japanese Workshop source is **3692 UTF-8 bytes** and English is **3616 bytes**, both below the 8,000-byte policy limit.

**Result:** repository-side public copy is prepared. This does **not** claim that the Steam Workshop page itself has been created or updated. Final screenshots/art should be completed before public release presentation is treated as finished.

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

### ENV-010 Haimatsu source approval and integration (2026-10-05 JST)

**Status:** IN PROGRESS (source approved / installed; runtime appearance pending)

The approved generated source was resized once with Lanczos to 256x256 RGBA. Chunk CRC, zlib decompression, decoded scanline/filter checks, Pillow decoding, dimensions, transparency, exact-copy equality and Def-path regression passed. Production SHA-256: `44d22e74670bdfe081ef98c8a327700620f3d285e4a86b6dc52625c57daef551`. Canonical approval is in ArtDirection. Existing assets, plant mechanics and descriptions are unchanged. The reusable TextureAssetPipeline remains the production procedure; a Haimatsu regression joins the existing CI gates.

Execution is in a cloud Linux workspace despite the author using the desktop app. PowerShell and RimWorld are unavailable here; no local build/runtime/visual PASS is claimed.

**Next action:** verify CI, then run `run-tests.bat` and `run-texture-debug.bat "D:\SteamLibrary\steamapps\common\RimWorld" Alpine` in a Windows local execution task. Author review at normal zoom is still required.

### TEST-POLICY-003 — Non-interactive runtime tests

**Requested by:** author (2026-10-05 JST)  
**Owner:** Testing/tooling  
**Status:** DONE — project-wide policy recorded; runtime harness migration pending

AMJ共通方針として、人間の目視を必要としないQuickstarts / integration / runtime ERROR / map・気候・土壌テストは可視RimWorldウィンドウを出さず実行する。

BadTex、Texture Atlas、Graphic.Draw等の描画検証ではrenderingを無効化せず、仮想/オフスクリーン/非表示表示先で実描画経路を維持する。可視実行は最終texture review等、人間判断が必要な場合だけに限定する。

Shared durable source: Core `Docs/DevelopmentGoldenPathGuidelines.md`, commit `a81c0389fb1834a098a462291b3f4814abcffef3`. Repository instruction: `AGENTS.md`, commit `6e33a8c4789b3d22e1b8d98901e0427d06fe636b`.

**Next action:** desktop Work/local Windows toolingで`run-runtime-tests.bat`系の非対話実行経路を実装する。rendering-dependent BadTex coverageを失わず、通常自動ゲートがデスクトップへ表示されないことを確認する。visual/debug runnerは別に保持する。

### TEST-RENDERED-PUBLISH — Isolated rendered runtime tooling (2026-10-05 JST)

**Owner:** Testing/tooling
**Status:** DONE (tooling publication); latest-main runtime regression pending

The earlier local snapshot passed Core 7/7, Environment six base Quickstarts
(58/57/54/52/3/3 assertions) and CCTO 70/70, with eight clean runtime logs
and combined exit 0. Game windows were enumerated on an independent non-visible
WinSta0 desktop while Direct3D rendering stayed enabled. The first hidden run
exposed worker-thread texture/map assertions; Core now posts those two steps
through PickleDriver and checks Unity main-thread execution. New Village Python
text reads explicitly use UTF-8. The saved launcher rerun passed after a separate
Haimatsu-review game released its DLL lock. No unrelated process was terminated.

Only this task's tooling/docs are published. Original working folders and other
local art/Def/review changes are preserved. These changes were transplanted onto
latest GitHub main, retaining its newer Core eight-scenario suite and Environment
Core-profile tests. Those newer runtime gates were not executed in the recorded
run and remain pending. No production art/Def/config change is included.

**Next action:** run the saved isolated desktop launcher against updated local
repositories to exercise the newer suites; human visual acceptance stays separate.

**Procedure:** Core Docs/IntegratedRuntimeTesting.md and
Scripts/IntegratedRuntimeDesktop/Run-AMJ-IsolatedDesktop.ps1.

### DOC-2GAME-001 — Japanese 2game summary and shared formatting

**Requested by:** author (2026-10-06 JST)

**Owner:** Documentation/release

**Status:** DONE — repository source prepared; live 2game publication not claimed

Added Docs/2GameDescription-ja.txt and Docs/2GamePresentation.md using CCTO's short summary and six ▼ sections, plain Japanese and short bullets. Content is contained within README: standalone environment scope, four biome bands/plants, optional CCTO/MO, new-game recommendation, unverified existing-save addition, removal limitations and post-Beta tree retextures. Related published mods link to 2game; AMJ Core links to GitHub because no 2game page ID is recorded. AMJE's own GitHub appears once at the end. No AMJE listing ID is invented.

AGENTS routes 2game work to Core's shared ModDescriptionGuidelines.md, now explicitly covering the AMJ-wide template. Validation: ordered six headings, plain-style/format checks, expected direct links, README/Workshop/About consistency and diff whitespace. Reusable update checks are recorded in Docs/2GamePresentation.md; no new runtime result is needed for this text-only task.

### TEST-WORKSHOP-001 — Actual distributed four-profile runtime gate

**Requested by:** author (2026-10-06 JST; closeout 2026-10-07 JST)
**Owner:** Testing / release
**Status:** TESTING DONE; RELEASE HANDOFF OPEN — author-manual Workshop payload update required

Actual Workshop3814638060 was tested on a non-visible Windows desktop with
Direct3D enabled, using six existing map/world/texture/loaded-Def Quickstarts
per configuration. Vanilla+AMJE227/227, MO+AMJE227/227, CCTO+AMJE275/275,
MO+CCTO+AMJE275/275; all24 reports have zero runtime/pre-launch ERROR,
complete live capture and no truncation. An independent observer proves one
AMJE pack and the production DLL loaded from the actual Workshop directory.
MO and dependencies were real Workshop versions, not fixtures. MO DarkForest
final soil patch and AMJE Def ownership also passed supplemental checks.

Applying current-main's existing native-cutting regression to that unchanged
payload failed7/9 in all four profiles: Haimatsu has no harvested resource and
produces zero wood. Other three species cut correctly. Current main already
contains the base8 fix; its existing Vanilla/MO harvest runner was rebuilt and
rerun with9/9 each and zero Unity/structured ERROR. No new production XML or
DLL repair was needed. That main-derived fixture PASS is not a Workshop PASS.

**Release request:** manually publish the current accepted runtime payload,
then test the actual downloaded new manifest. Current Workshop final release
approval is HOLD on the missing Haimatsu cutting contract. Do not substitute
the local development Mod or overwrite the installed Workshop files.
This note is the release-workstream handoff; the user is not asked to relay it.

Inherited warnings were identified: Japanese Vanilla Def-injection36 plus a
FactionGreetingWarm argument mismatch, installed-mod metadata discovery
warnings, and absent Highland climate representative in the tiny test world.
No AMJE translation key error, startup exception, PatchOperation failure or
CCTO/MO conflict was observed. Long-running play/highland annual coverage is
outside this short gate.

All1112 original Workshop files and normal configuration hashes remained
unchanged. Steam externally added only About/preview.png during closeout:
manifest8889000939694660295 ->8630945342812668549; runtime bytes unchanged.
No Steam upload is claimed by this testing workstream.

Durable evidence/procedure: Docs/GoldenPaths/WorkshopRuntimeTests.md;
test-only source/Def observer and deterministic current-regression staging
helper are retained in Tests/Release and Scripts. Local full logs/manifests
are linked in that document. Production art/Defs/order/saves are preserved.

### PUB-WORKSHOP-002 — Upload-root provenance and release gate repair (2026-10-07 JST)

**Owner:** Release / testing
**Status:** IN PROGRESS — actual Steam update remains author-manual

**Closeout:** formal tooling and final26-file candidate are DONE; actual Steam
release remains author-manual/HOLD. See final PUB-WORKSHOP-002 result below.

Read main AGENTS, TEST-WORKSHOP-001 and WorkshopRuntimeTests before work. Dirty production checkout is preserved; work uses a separate main clone under TestResults/WorkshopReleaseRepair/Repository. Current Steam manifest remains 8630945342812668549.

Read-only evidence: all1113 installed Workshop file hashes equal the same relative paths in the dirty development root. Its root Haimatsu Def lacks harvest fields, whereas shipped TestResults/SourceSync/Defs contains the fixed Def. Steam workshop_log records successful content uploads at 23:01 and23:26 on2026-10-06 and the latter preview path under Mods/AncientMedievalJapanEnvironment/About. YADA copies the selected ModMetaData.RootDir through Scanner; the local .rimignore is generic and excludes neither Art nor TestResults. Git's _AMJ_PublishStaging worktree registration points to a missing path. This establishes wrong-root publication and missing payload/provenance gates; no claim that YADA loses XML fields or that an absent staging path was uploaded.

Next: formal immutable main-derived payload builder, exact file/provenance/harvest regression guards, durable Golden Path and candidate tests. No duplicate production behavior patch. Workshop source, normal settings and saves are not changed. Release HOLD until author publication and actual downloaded four-profile/cutting gate.

### POLICY-WORKSHOP-PAYLOAD-001 — Subscriber-only distribution (2026-10-07 JST)

**Requested by:** author
**Owner:** AMJ shared release / packaging
**Status:** DONE — repository policy/exclusions; actual Steam update remains separate

Core, Environment and CCTO now route subscriber-only Workshop packaging through
AGENTS and Core Docs/WorkshopPackaging.md. Root .rimignore excludes Art, Docs,
README, source, scripts/build tools, tests/fixtures/reports, VCS/editor metadata,
local overrides, archives and debug leftovers. Runtime assets, About identity,
loadFolders where used and required license/attribution remain. Development
originals stay in Git. YADA upstream Scanner.cs confirms inherited basename
rules; ineffective Patches/_LocalTest.xml is corrected to _LocalTest.xml.
Do not replace project filters with YADA's generic starter template.

Validation PASS: three tracked-file inventories; nested fixture/path-syntax,
accidental-runtime-exclusion and actual-payload leakage regressions; Core
archive/YADA equality and publisher adapter drift checks; corrected whole-Art
source-exclusion regression; Environment builder fixture retains production DLL
and root-only loader, and excludes README/Docs/Art/tests. Python/XML/workflow
syntax checks PASS. These prove packaging/static behavior, not new real-game
runtime or Steam publication success. Workshop filter CI is added with main-only
push and canceled superseded runs. Core's standard preparation gates source and
staged output; Environment's candidate builder gates the actual subscriber files.

**Release-workstream handoff:** PUB-WORKSHOP-002 stays with release/testing. Its\nimmutable/main-derived builder must exclude README/Workshop copy, retain external\nprovenance manifests and run Tests/validate_workshop_payload.py --payload STAGE\n--expected-assembly AncientMedievalJapanEnvironment.dll. The existing builder\nreceived only filtering/final-payload checks. Production XML/DLL/installed\nWorkshop bytes and existing provenance/harvest/runtime release HOLD are unchanged.\n

### PUB-WORKSHOP-002 — Formal repair merged; final subscriber candidate testing

**Status:** TOOLING DONE; FINAL CANDIDATE RUNTIME IN PROGRESS; STEAM RELEASE HOLD

PR6 merged at aaf798a452af0ad29f2138c8cdb4513be54a6f43 after exact-head PowerShell syntax, subscriber/publication regressions, plant coverage and art-rule CI passed. No production XML/C#/art change. Shared policy df986ad is retained: README/Docs/.rimignore are excluded; new builder follows the authoritative filter and audits actual staged output. The initial28-file candidate had all four map/cutting gates PASS but is superseded for publication by the subscriber-only policy.

Final26-file candidate: TestResults/WorkshopReleaseRepair/Candidate-a23a9eb; sourcea23a9eb2b6cac3afd3b876860ae1654327a90370, archive SHA25615af3f24ced8988859db0b811ef58348b9b6a05ea77cbf93757176f023be4a99. It preserves identity and current accepted runtime bytes, freshly built production DLL and approved preview. Shared final-payload filter PASS; exact four-profile non-visible Direct3D matrix is running in SubscriberCandidateRuntime-1. Do not upload the historical28-file candidate or development root.

Author-manual upload/download remains separate. No Workshop overwrite/upload, normal config/save change or actual-distribution repair is claimed. Final source/DLL/cutting rerun from newly downloaded Steam bytes is still required to clear HOLD.

### PUB-WORKSHOP-002 — Final subscriber candidate and root-cause closeout

**Status:** DONE (investigation/tooling/verified candidate); STEAM PUBLICATION HANDOFF OPEN

Root cause is evidenced by1113/1113 exact same-path hashes: successful Steam
uploads carried the unsynchronized dirty development root. Root Haimatsu has
no harvest fields, while fixed fields exist in the uploaded TestResults/SourceSync
copy; retained preparation scripts explicitly target SourceSync. Root generic
YADA exclusions leaked37 Art and988 TestResults files. No claim that the absent
_AMJ_PublishStaging checkout was uploaded or that YADA removed XML fields.
Current main and nested fixed XML differ only in later labels/descriptions;
their remaining parsed Def trees, including all six cutting fields, are equal.

PR6 merged at aaf798a452af0ad29f2138c8cdb4513be54a6f43 with exact-head green
PowerShell syntax, subscriber/publication regressions, strict plant coverage
and art-rule checks. Shared policy df986ad is retained. Changes are publishing
provenance, packaging, tests and procedure; production XML/C#/art are unchanged.
The historical28-file candidate was superseded, not uploaded.

Final verified26-file root: TestResults/WorkshopReleaseRepair/Candidate-a23a9eb/
AncientMedievalJapanEnvironment. Sourcea23a9eb2b6cac3afd3b876860ae1654327a90370;
ZIP SHA25615af3f24ced8988859db0b811ef58348b9b6a05ea77cbf93757176f023be4a99.
Actual stage filter PASS. Its freshly built DLL/26 files were rerun on non-visible
Direct3D desktops: Vanilla/MO/CCTO/MO+CCTO six-map counts227/227,227/227,
275/275,275/275 and native cutting9/9 each, outputs42/40/30/8. All28 reports
passed source/DLL/ownership, real dependency roots, full capture, zero global
and independent Unity ERROR, and exact-byte/preservation rechecks. Temporary
fixture About name/packageId differs only for test selection; original candidate
identity is retained. Final matrix had no failed scenario.

Full evidence: SubscriberCandidateRuntime-1, SubscriberCandidateRuntimeGate.json,
UploadSourceAudit.json, WorkshopFinalPreservation.json and appworkshop-final.acf.
Normal configs/saves and actual Workshop bytes are preserved; all1113 Workshop
files remain unchanged and installed/latest manifest is8630945342812668549.
Formal evidence/manifests and reusable automation are in WorkshopPublication.md
and Docs/ValidationEvidence/Workshop*.json.

**Author release handoff:** proceed manually using the exact verified26-file
root after selected-root manifest verification. Do not upload the dirty root or
historical28-file package. Actual distribution is NOT fixed/approved yet.
Only a real Steam download and four-profile/cutting rerun clear release HOLD.

### DOC-ENV-REPLACEMENT-001 — Vanilla replacement scope in public copy (2026-10-07 JST)

**Owner:** Documentation / release
**Status:** DONE — repository description sources; live Steam / 2game publication remains separate

Audited main AGENTS/Coordination, Design, terrain processor, biome scoring and River / Coast handoff. README, Japanese-first Workshop and English translation now lead with replacement/reconfiguration of Vanilla terrain, vegetation and biome composition for Japan. Existing WorldGen/mutator reuse, four baseline biomes, representative plants, Vanilla secondary vegetation, wetlands/MO coexistence and save limitations remain consistent. About and 2game summaries/policies are synchronized; formal public titles match the existing colon-free About name. Durable scope is in Design; repeatable review checks are in WorkshopDescription.md.

Validation: bilingual semantic review, LF/CRLF UTF-8 byte limits, BBCode balance/link/image preservation, About XML/identity and diff whitespace. No production C#/Defs/art changes or new runtime result. Author-manual Steam description update uses the two committed BBCode sources; no live-site publication is claimed.

### TEST-WORKSHOP-003 — Newly downloaded actual payload (2026-10-07 JST)

**Status:** RUNTIME DONE; EXACT CANDIDATE/PACKAGING MISMATCH OPEN

Manifest7945743700437607405 actual26-file Workshop root passed Vanilla/MO/CCTO/
MO+CCTO maps227/227,227/227,275/275,275/275 and cutting9/9 each. Haimatsu now
passes. Completed28 reports and independent Unity capture gates have zero ERROR;
full payload/normal-config hashes preserved. Initial CCTO attempt hit Quickstarts
LogCapture.CountErrors collection modification exception; fresh rerun passed.
Retained evidence: TestResults/WorkshopDownloaded-20261007-Run1 and Run2;
Run2/VerifiedSummary.json. Durable procedure/result: WorkshopRuntimeTests.md.
Downloaded DLL/loadFolders/PublishedFileId bytes and preview filename case differ
from approved candidate manifest. Runtime XML/textures match; conditional missing
DevQuickstarts loader remains. Runtime success does not prove exact candidate
publication or clear packaging/provenance HOLD. No production/Workshop edits or
upload performed; evidence/documentation local only.

### ENV-TREE-SOWING-RUNTIME-002 — Alpine natural woody versus sowable set (2026-10-08 JST)

**Owner:** Environment / regional tree-sowing verification  
**Status:** IN PROGRESS — assertion corrected; full matrix rerun required

The author's isolated vegetation runtime report passed warm/cool/subalpine and failed Alpine 1/70 assertions. It compared the Alpine `map.Biome.wildPlants` tree-like set (including `AMJ_Shrub_Haimatsu`, since RimWorld 1.6 reports `plant.IsTree`) against the approved empty growing-zone sowing set. Native sowing menu and Haimatsu-non-sowable assertions passed, with zero captured runtime ERROR. Vegetation XML and production gameplay are unchanged.

Correction: the runtime/static tests separately expect Alpine natural woody `{AMJ_Shrub_Haimatsu}` and sowable trees `{}`; other three bands continue to use equal sets. Canonical rule and procedure: `Docs/Design.md` and `Docs/GoldenPaths/PlantSowingTests.md`. Rerun the full isolated vegetation matrix for wetland/river/coast coverage; no new full-runtime PASS or publication claimed.

### RULE-AUDIT-20261008 — operating-rule consolidation

**Owner:** Project common rules; this repository retains its local specification and gates.
**Status:** SOURCE RESTRUCTURED; validation/publication evidence is recorded in Project `Docs/RuleAudit.md` and actual commit/CI results, not inferred here.

AGENTS now routes through Project `Docs/SharedRules.md` stop conditions and task procedures. New development requires VE and non-VE source/evidence comparison plus a justified implementation decision. Static/runtime/specification/distribution/publication remain separate states. Historical records below/above retain their original scope; this entry does not reopen paused work, change gameplay/dependencies/art/versions, or supersede owner runtime/release blockers. Main-only Coordination means one authoritative integrated log, not deleting branch snapshots. No Steam/2game update is claimed.
