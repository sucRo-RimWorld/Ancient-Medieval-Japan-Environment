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

**2026-10-10 follow-up, runtime test blocker:** On the author's local
`run-tests.bat`, `Scripts/Validate-Environment.ps1` still required the retired
`<li IfModActive="rimworks.quickstarts">DevQuickstarts</li>` loader entry,
contradicting the already-correct production root-only `loadFolders.xml`.
The validator is corrected to parse XML and require only one `v1.6/li` entry
with `/` and no attributes, preserving fail-closed detection of extra folders.
`Tests/test_run_tests_entrypoint.py` now also guards against the obsolete
validator expectation. Do not reintroduce `DevQuickstarts` in the production
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

### ENV-RETAINED-TREE-LABELS-20261010 — historically appropriate Japanese names

**Owner:** Environment vegetation/localization and Vanilla tree regression  
**Status:** SIX VANILLA TREE + TWO WETLAND LABEL CHANGES ON MAIN; GENERIC/WILDLIFE/MO 1.6 SOURCE AUDIT DONE (NO ADDITIONAL RENAME); NATIVE FOUR-PROFILE TEST UNVERIFIED

2026-10-10 author correction: Japanese pre-Edo flora naming includes **display labels, not merely descriptions**. Formal owner `Docs/Design.md` §11.5.5, six-Def mapping and existing approved two-paragraph texts `Docs/VanillaPlantStep2DescriptionReview-ja.md`. English `Patches/VanillaTreeDescriptions.xml` and Japanese `Languages/Japanese/DefInjected/ThingDef/AMJ_WildPlants.xml` add labels without changing DefNames, forest distribution, sowing, wood cutting, ingredients or art. The six existing Quickstart description assertions also check corresponding EN/JA loaded labels without changing expected assertion counts. The retained oak is a group-level Japanese deciduous Nara proxy; a narrower specific Mizunara identity is a separate evidence/visual/approved-text decision. **2026-10-10 next naming slice:** the two retained Vanilla wetland BiomeDefs receive Japanese-context display names, `TemperateSwamp` 温帯湿地 / temperate wetland and `ColdBog` 冷涼湿原 / cool wetland. The same `Patches/VanillaWetlandDescriptions.xml` and Japanese BiomeDef DefInjected own label changes; descriptions stay author-approved and unchanged. Wetland canonical mapping: `Docs/VanillaWetlandBiomeAudit-ja.md`. `defName`, BiomeWorker, world and map terrain, weather, diseases and wildlife stay untouched. Existing wetland Quickstart description assertions now check bilingual labels too, without increasing counts. This name change does not certify the earlier 2026-10-08 description-only runtime PASS as a new PASS. Animal renaming and other inherited Vanilla/MO flora remain separate evidence/side-effect review, particularly because reused global ThingDefs affect contexts outside Environment's biome pools. Fresh in-game bilingual label loading and four-profile validation remain outstanding.

**2026-10-10 evidence-based closeout for the next naming candidates:** `Docs/Research/RetainedContentNameAudit-ja.md` audits all 7 retained generic Vanilla plants, 9 wildlife PawnKindDef proxy names across Environment's six biomes, and actual MO 1.6 tree/farm/dark-forest source in the supplied Workshop archive (archive SHA256 recorded in the research). No additional specific-species relabel is justified: keep generic flora generic, do not relabel the globally shared Bear_Grizzly/Wolf_Timber/Deer etc. as Japanese endemic species, preserve Great Oak/Iter/Fir/Willow as MO-owned fantasy identities. AMJE naming is now **bounded by actual Def identity** and any future species-level rename requires separate visual/biogeographic/ownership checks; no new XML labels/test cases added in this audit. Follow up only on already-unverified bilingual loaded-game and four-profile gates.

### ENV-MO-WETLAND-WILDPLANTS-PATCH-INTERACTION-20261010 — preserve MO herbs

**Owner:** Environment optional Medieval Overhaul coexistence  
**Status:** AUTHOR DECISION IMPLEMENTED AND MERGED (`e4723b0db2d51ae0d1a61d5b4e9ea351175fd1e1`); PR + MAIN STATIC CI PASS; PICKLE LOADED-DEF ASSERTIONS ADDED; NATIVE RUNTIME/SPAWN UNTESTED

**2026-10-10 author instruction: Environment must not touch plants added by MO.** Source: MO supplied archive `3219596926.zip` 1.6 `1.6/Patches/Core/Add_Plants_To_Biomes.xml` adds Mindwort/Poppy/Fleawort/FlyAgaric to both retained Vanilla wetlands. Environment formerly replaced both complete `wildPlants` nodes, removing MO entries whenever its patch applied later. The owning `Patches/VanillaWetlandVegetation.xml` now **conditionally removes only exact named Vanilla entries and re-adds only the accepted Vanilla baseline weights**; MO/other-Mod-added independent entries remain untouched whether they are patched before or after AMJE. Missing ColdBog/Cypress no longer makes an unguarded removal operation fail. The 7.30/8.22 totals now explicitly measure only Environment-owned Vanilla baseline plants; optional MO additions have their own weights. Formal source: `Docs/Design.md` and `Docs/VanillaWetlandBiomeAudit-ja.md`. Static contracts and the existing MO-conditional wetland Quickstarts verify the intent; **actual Environment+MO loaded Defs, natural spawn, four-profile regression and save compatibility remain OPEN until executed**. Do not revise MO herbs' DefNames, properties, spawn weights or ownership. **MO 1.6 source rechecked directly on 2026-10-10:** all four named herbs are added to both wetlands with `0.05` each, archive sha256 `6ed379d7c400db43b3af2a9a7fd6203f7f7e99ca1a670d36b0f73fd1adccc9e3`. Reuse the existing development Pickle `Replacement native plants retain tuned development commonality` scenario for these eight **loaded** values under MO and MO-CCTO, and the already-existing two wetland Quickstarts; do not add a duplicate scenario or extra RimTest case. Runtime path and remaining evidence are described in `Docs/GoldenPaths/RenderedRuntimeTests.md`. This source/CI coverage does not establish native MO spawn, ERROR-zero or four-profile PASS.


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

**2026-10-10 plume revision follow-up:** Author requested transparent game-ready regeneration and conditionally considered the asymmetric image if its left/right direction is preserved. A simplified right-facing five-plume candidate was regenerated; 256×256 RGBA export `AMJE_Susuki_Mature_RightFacing_Candidate_256.png`, SHA-256 `0c2980bab0050270ad6ffe4ec0dffc5cc745118321e2c9c589fef70ed1dfe67e`, remains unaccepted and is not a production asset. Straight branch-tip alignment still needs review/correction. Formal direction requirement and pinned `Plant.Print` source finding are in `Docs/ArtDirection.md`: normal plants randomize flip directly, so XML `allowFlip=false` alone is insufficient. Scoped Susuki/Ashi rendering remains unimplemented/untested under the shared two-species direction contract in `Docs/ArtDirection.md`; preserve other plants and the earlier accepted master.

### ENV-STEP3-NATIVE-VEGETATION-20261008 — four Environment-owned wetland plants

**Owner:** Environment natural vegetation; future Medicine Mod separately owns medicinal plants and remedies
**2026-10-10 CCTO/winter testing handoff:** Author approved the optional low-temperature model for the four future wetland PlantDefs: Yoshi standalone/CCTO growth 0/5°C + dormancy; Suge 0/0°C + dormancy; Hannoki 0/5°C + dormancy; Mizugoke 0/0°C + death below −35°C. Formal source is `Docs/NativeVegetationStep3Design-ja.md` §3.3.1 and `Docs/Design.md` §9.6. All future CCTO/Vanilla/MO profiles and winter/harvest/native rendering checks must use the existing non-visible, isolated `run-tests.bat` / framework launchers, **not** visible direct runtime debug execution; render with Direct3D, not `-nographics`. This is design/test routing only: source art, production Defs, loaded four-profile/native tests, snow behavior, old-save validation remain OPEN; earlier harvest yield/commonality drafts are not silently approved. An existing four-profile development Pickle plant-ownership step is now extended to verify loaded 0/5°C values and exactly one correctly configured CCTO extension per wetland species **when its real PlantDef exists**; absent species are not claimed as PASS. This changes test-only C# and does not add a scenario or new production Def. No CCTO repository change is requested.

**Status:** PHASE ONE — FOUR JAPANESE DESCRIPTIONS AUTHOR-APPROVED 2026-10-10; ENGLISH TRANSLATIONS PREPARED; LIVE PLANTDEFS STILL BLOCKED ON ACCEPTED SOURCE ART / UNSET FUNCTIONAL VALUES AND NATIVE TESTS; SUGI/SUSUKI QUEUED AFTER WETLAND FOUR
**2026-10-10 Ashi direction decision:** Author specified the same left/right-preserving rendering behavior for Ashi (`AMJ_Plant_Yoshi`) because its plume silhouette is also asymmetric. The two-species contract now lives in `Docs/ArtDirection.md`: preserve each species' own accepted source orientation across normal/snow meshes, with other species unaffected. Specification only; no new image acceptance, PlantDef, rendering code or runtime evidence. Ashi remains in the first wetland phase; Susuki remains after the four wetland species.

**2026-10-09 author decision:** Environment must reconstruct the ancient-to-medieval Japanese natural environment **by itself**, with no required other vegetation Mod or optional DLC. Yoshi, Suge, Hannoki and Mizugoke remain Environment implementation targets even if equivalent plants exist in other Mods. VE, Biomes! Prehistoric, ReGrowth 2, MO and Odyssey are **optional compatibility** targets only; prior-art review is used to prevent duplicate spawns, unexpected pool changes and loaded patch conflicts, **not to omit AMJE plants**. Canonical sources: `Docs/Design.md`, `Docs/NativeVegetationStep3Design-ja.md`, `Docs/Research/WetlandPlantExistingModAudit-ja.md` (prior-art PR #49).

**2026-10-09 decision-file progress:** `Docs/Research/WetlandPlantImplementationDecision.json` records `ready` for the scoped independent-implementation necessity, backed by the pinned `Docs/Research/WetlandPlantExistingModAudit-ja.md` SHA256 and a repo-owned negative-mutation/static check in `Tests/test_wetland_implementation_decision.py`. This does NOT approve historical Japanese text or visual assets, and does NOT constitute a runtime compatibility PASS. Current-main evidence SHA256 and all Project schema-equivalent structural assertions were independently rechecked on 2026-10-09 and matched. **The exact Project Python CLI has now executed and PASSed** on pinned Environment `41db6dc142478917e28d0b1510d3219399bca85a` using Project PR #7 (`3d73b8e6a1b53c555e65e125716cf944af874ca0`), GitHub Actions run `37947510648`; exact command and output in `Docs/ValidationEvidence/WetlandEntryProjectValidator-20261009.md`. Decision STRUCTURE/EVIDENCE only: editorial/art acceptance, production Defs and native/runtime profiles remain BLOCKED or unverified.

**2026-10-10 author Japanese prose approval:** The author replied **“OK”** to the specific four full Japanese descriptions in `Docs/NativeVegetationStep3Design-ja.md` §4, explicitly including historical first-name **Ashi (葦)** and the revised sphagnum peat explanation **枯れた部分が分解されにくい状態で堆積し、泥炭の形成に関わる**. This clears **Japanese prose author acceptance for all four species only**. Semantic-parity English translations are prepared in the same formal spec §4.1; actual DefInjected and PlantDef export remain gated by original art/visual state approval, harvest yields, distribution and native runtime. Research source and scope limitations stay in `Docs/Research/WetlandPlantHistoricalEvidence-ja.md`.

**2026-10-09 author approvals for plant materials/interaction:** In the three-profile `Docs/Research/WetlandPlantMaterialsProfiles-ja.md` design, Suge is definitively unified into Vanilla `Hay` and MO's existing `Hay→DankPyon_Straw` processing, with **no Suge-specific raw item**; future Suge hats/capes/mats may consume generic Hay/Straw and do not require provenance tracking. Mizugoke is selectable and inspectable, cuttable/removable without item drops, non-harvestable and non-sowable. These are approved **behavior/design choices, not implemented or game-tested**. Yoshi Hay use, Hannoki output amount, all proposed yields (2/1/24), future consumer recipes, MO functionality and artwork remain unapproved or unverified. This supersedes the earlier risk note that Suge provenance needs a separate raw ingredient. Project cross-stream consumer record must match. MO XML observations do not constitute runtime compatibility PASS.

**2026-10-09 Suge/Mizugoke static contract preparation:** `Docs/Research/WetlandPlantMaterialDefAudit-ja.md` records the author-supplied MO 1.6 archive XPath audit: MO adds Hay harvest to Vanilla Grass/TallGrass, **not** AMJ_Plant_Suge; AMJE's future Suge Def must explicitly supply Hay. MO already processes Hay to Straw. A new repo test `Tests/test_wetland_material_plant_contract.py` checks isolated XML fixtures and guards against direct Straw drops, Mizugoke harvest fields, accidental sowing and unselectable Moss. Until production PlantDefs exist, its actual-Def acceptance test is **SKIP**, not PASS. Owner approval of text, art, numeric yield and runtime remain necessary.

**2026-10-10 wetland source-reference preflight:** `Tests/test_wetland_production_refs.py` adds a no-false-PASS static source contract for the four planned wetland plants: detect an undefined Def referenced by live wetland pool, orphan owned Def, duplicate Def, and premature Mizugoke in TemperateSwamp; mock/negative cases run now, production acceptance SKIPs until real owned content exists. Author's earlier Mizugoke wording correction (replace “その遺体” with decomposing-dead-parts wording) is now reflected in the Step 3 author-review Japanese candidate; in-game text and graphics remain unapproved. Scope is static linking only, not real terrain suitability, loaded Defs or native jobs.

**2026-10-09 correctness follow-up:** The existing Mod audit's outdated claim that formal JSON does not exist was corrected, with the pinned evidence SHA256 refreshed in the formal JSON. The Suge shape fixture now explicitly guards `harvestTag=Standard`, consistent with Vanilla harvestable plant XML; actual loaded `harvestTag` and harvest Job remain a separate runtime gate, not a source-only success claim.

**2026-10-10 Japanese text review correction:** Existing Step3 §4 draft descriptions and `Docs/Research/WetlandPlantHistoricalEvidence-ja.md` were compared with primary official ecology/history sources and updated to a **single candidate set for author review**. The former Suge note saying no collection for hats/capes was inconsistent with approved generic Hay and has been retired. In-game draft texts now describe ecology and documented historical context, without claiming yet-unimplemented equipment/material recipes; all four descriptions remain unapproved, untranslated and absent from game files.

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

**Owner:** Environment native vegetation; [Wild Food Foraging](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Wild-Food-Foraging) edible-food harvesting consumer  
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
**2026-10-10 author dependency decision:** Wild Food Foraging now **requires** Environment (`sucro.ancientmedievaljapan.environment`) as the provider of Sudajii and other Japan-native vegetation; Environment has **no** reverse dependency, and regional species pools stay Environment-owned. No forced Sudajii everywhere, no new tree/food Defs or yield approvals. Dedicated Wild Food Foraging owns harvesting gameplay and will add its required `modDependencies` metadata when its playable About is created. Its prior-art implementation entry is still OPEN. Preservation owns downstream fruit processing. Verify cross-owner harvesting with the existing-AMJE-save→new-build natural-spawn/real-harvest/sow/save-load E2E before claiming compatibility. Do not represent this handoff as
a completed fruit feature or runtime PASS.

### TEST-ENV-UNIFIED-20261008 — unified AMJE native test and wetland handoff

**Owner:** Environment runtime testing and wetland development  
**Status:** OPEN — 2026-10-10 static/build PASS; current RimTest prelaunch blocker under repair; latest-main RimTest/Pickle/Quickstart and complete four-profile native gates NOT PASSED

**Current path:** `run-tests.bat` runs static/build, RimTest Redux, four-profile development Pickle, and then the non-visible rendered Quickstarts; `Docs/GoldenPaths/RenderedRuntimeTests.md` owns the matrix/evidence and `Docs/GoldenPaths/PlantSowingTests.md` owns sowing assertions. Do not equate the standard Quickstart runtime profiles with the separate full four-profile release gate; isolated MO map startup is not automatically enabled for every map profile.

**2026-10-10 author Windows run:** Static/build source Defs and MO texture audit PASS; Python framework wiring now **5 tests PASS**. The prior RimTest `--root "%ROOT%"` trailing-quote error was fixed in main via PR #80 (`d3dff876f924788509c3988b814ad365dd0a5da7`). RimTest still aborted **before launching RimWorld**, now with `Expected one 0Harmony.dll in .../workshop/content/294100/2009463077; found 3`. The original `find_dll` searched all game-version folders when no `1.6/Assemblies` existed. Harmony's official source `pardeike/HarmonyRimWorld:LoadFolders.xml` chooses `/` plus `Current` for `v1.6`, while the mod contains `1.4`, `1.5` and `Current` assembly variants. The framework helper now resolves version-active folders from the actual Mod loader and selects the effective 1.6 DLL, with a three-version fixture regression. **MO/CCTO loaded tests, hidden rendering and native spawn did not run in this log.** Fresh author-side rerun is required after merge; no runtime PASS inherited.

**2026-10-10 latest author-side rerun:** Python framework contract tests **6/6 PASS**. RimTest advanced through Harmony 1.6 DLL selection to its test-only C# compilation but stopped with `CS0103: nameof` at `Tests/RimTest/EnvironmentRimTests.cs:166` under Windows Framework64 v4.0.30319 `csc.exe`. This compiler predates the C# 6 `nameof` expression. The Harmony bridge now uses the same literal `"AfterResultsLogged"` method name (method signature and behavior unchanged), with an existing-contract regression for the old compiler. `CS1684 System.Span` output in this run is warning-level and not evidence of a separate compiler failure. The abort occurred **before actual RimWorld startup**; no RimTest result, Pickle or hidden Quickstart scenario passed in this run. Native rerun is pending. The disposable runner moves any temporary RimTest fixture out of the game Mod folder to its owned temporary `Retired` output on exit; no user save/normal ModsConfig change is inferred from this prelaunch failure.

**2026-10-10 latest author-side development Pickle log (Vanilla first profile):** Python framework checks **7/7 PASS**, RimTest Redux **10/10 PASS**, and the development Pickle Steps DLL compiles with warning-level CS1684. The first shown Pickle failure is **not a scenario count failure**: the strict `Player.log` owned-ERROR gate stops with `RimWorld Player.log contains ERROR`. The author-provided excerpt of `Vanilla-Player.log` line 976 reports `Mod AMJE development Pickle audit (temporary) did not load any content` for its temporary `AMJE.DevelopmentPickleAudit` Mod. The generated fixture has `Pickle/Assemblies` and `Pickle/Features` but no RimWorld-recognized normal asset; RimWorld `ModContentPack.AnyNonTranslationContentLoaded` checks loaded textures/audio/strings/root assemblies/patches/Defs and does not count Pickle-specific folders. The fixture now generates one **test-only `Strings/AMJEDevelopmentPickleAudit.txt`** under its owned temporary root before launching, leaving any gameplay Def and normal user configuration untouched. Contract regression checks marker creation. This resolves the **identified missing-content cause in source only**; other Player.log errors were not independently inspected. Do **not** suppress `[ERROR]`, change the Unity capture gate, or claim Pickle four-profile PASS until the author reruns the non-visible framework suite. The remaining Japanese-translation warning is not by itself evidence of this ERROR's cause.

**Prior evidence retained (not current-main PASS):** Author-supplied earlier `AMJWarmTemperateTerrainQuickstart(4).log` had 74/74 assertions with zero in-log error but no accompanying per-scenario JSON; its forced biome was not evidence of world prevalence. `EnvironmentIsolatedRuntime(2).log` showed eight base Quickstarts 8/8 with zero owned ERROR and focused CCTO PASS; historical Highland diagnostic and sowing failures were corrected in PR #23/26, see `Docs/GoldenPaths/RenderedRuntimeTests.md` and their Git history. Author explicitly reported the post-Highland-fix Grains/Core+MO retest **PASS**, without independent post-fix raw log/JSON; preserve it as author-reported only. The ninth natural-world wetland distribution scenario was author-reported PASS separately. Earlier 32-file pinned candidate four-profile results apply **only to that pinned source**, not the current development root, changed Mod, or downloaded Steam package.

**Remaining:** current-source non-visible RimTest/Pickle/Quickstart log + Unity capture/ERROR-zero review, correct MO herbs in both wetlands, new loaded bilingual names, natural wetland distribution, independent latest-source full four-profile gate and save/release checks. Keep `ENV-TREE-SOWING-RUNTIME-002` and `ENV-WETLAND-BIOME-001` OPEN where applicable; `ENV-RETEX-012` and Steam distribution HOLD remain independent. Do not claim gameplay or Workshop publication from static/CI results.

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

