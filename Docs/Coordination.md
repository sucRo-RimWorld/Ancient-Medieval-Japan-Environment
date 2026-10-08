# AMJ Environment Coordination

### TEST-ENV-UNIFIED-20261008 — unify parallel AMJE test and wetland development tracks

**Requested by:** author (2026-10-08 JST)  
**Owner:** Environment / runtime testing and wetland development — one shared handoff  
**Status:** IN PROGRESS — unified workflow and source of truth; latest-main runtime PASS pending

The author requested integration of the separate test chat with the current wetland/vegetation development chat. These now use the **same authoritative main** and `run-tests.bat` as the standard local automated entry; no duplicate branch, competing test scope, or user-mediated relay is required.

**Merged documentation source:** PR #25 / squash merge `0af0590957d0416b77c285e3548c40dacaedab47` consolidates the regional sowing/alpine regression, wetland Phase 5 patch and ecology gates in `Docs/GoldenPaths/RenderedRuntimeTests.md` and updates `Docs/GoldenPaths/PlantSowingTests.md`. This change is documentation-only; Workshop payload CI passed and no new RimWorld runtime result was produced.

**Reconciled latest state:**
- Earlier warm-temperate isolated run failed 1/74 plus a ColdBog Cypress-related pre-launch PatchOperation ERROR. PR #23 (`a2ea49aecf69c8975ca26e4b84b755ba488005ca`) corrected whole-pool wetland patching and biome-local sowing comparison. No new runtime PASS claimed.
- Parallel Alpine test chat subsequently recorded 69/70 PASS; the sole failed assertion conflated natural `AMJ_Shrub_Haimatsu` (`plant.IsTree`) with the empty Alpine grow-zone tree menu. Current main already separates `ExpectedWildTreeLikePlants` from `ExpectedSowableTrees`; the native menu/Haimatsu exclusion assertions had passed. Keep `ENV-TREE-SOWING-RUNTIME-002` OPEN for a fresh matrix.
- PR #24 (`3e946ad24f4e5d93ced19478219b73f865befa54`) changed retained-wetland wildlife, diseases, climate weather and wild pack-animal pools, and added both loaded-Def wetland ecology/terrain Quickstart assertions. Static CI passed; full post-change game/runtime still pending.
- Standard `run-tests.bat` covers static/build then the eight base Quickstarts (four AMJ biomes, two wetlands, River/Coast) on an isolated rendering-enabled Windows desktop; optional CCTO and Grains/Core profiles are installed-presence-dependent. **This runner does not currently exercise the full Vanilla/MO/CCTO/MO+CCTO four-profile matrix**, because isolated MO startup is disabled in `run-runtime-tests.bat`. The four-profile/release gate must not be marked PASS merely because the standard gate passes.
- Future latest-main acceptance requires complete eight-scenario reports with zero pre-launch/runtime ERROR and non-truncated capture, optional profiles identified explicitly, world wetland-share sanity, and separate four-profile coverage when possible. Wetland descriptions remain unapproved Japanese-first drafts; bilingual content synchronization and Step 1 closure are still pending. `ENV-RETEX-012` stays BLOCKED.

**Canonical ongoing test handoff:** `Docs/GoldenPaths/RenderedRuntimeTests.md` (unified matrix/status), `Docs/GoldenPaths/PlantSowingTests.md` (tree contract), `Docs/VanillaWetlandBiomeAudit-ja.md` (wetland ecology and approval conditions). Refer future test work to this item together with `ENV-TREE-SOWING-RUNTIME-002` and `ENV-WETLAND-BIOME-001`, rather than maintaining separate chat-specific statuses.

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
**Status:** OPEN — audit DONE; implementation + Step 1 completion gate required

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

**Next smallest unit:** implement the wetland wildlife/disease/weather/description corrections, then run the Step 1 completion gate.

**Test entrypoint infrastructure (2026-10-08):** PR #19 / squash merge `d2b4d261c16481c748544661499b287b039a50d3` makes `run-tests.bat` the canonical AMJE automated gate. It now runs the static/build/source validation first, then launches the normal runtime Quickstart matrix on a rendering-enabled non-visible Windows desktop. The default runtime matrix already includes `AMJTemperateSwampVegetationQuickstart` and `AMJColdBogVegetationQuickstart`, so the forthcoming Step 1 wetland assertions belong in those scenarios and will be exercised automatically by `run-tests.bat`. This tooling merge does **not** claim the new wetland wildlife/disease/weather/terrain completion assertions are implemented or passing yet.


**Static runner handoff regression fixed (2026-10-08):** PR #21 / squash merge `2926162147b83d3574ee849777b1349df77c849a` resolved the stale `Validate-Environment.ps1` requirement that MO texture audit commands appear directly inside `run-tests.bat`. The validator now verifies that `run-tests.bat` delegates to `run-static-tests.bat`, where the MO audit actually runs, and checks that `run-runtime-tests.bat` uses `--skip-static` without recursion. `Tests/test_run_tests_entrypoint.py` covers the regression. The post-failure marker groups were also reviewed against current source. PR CI passed PowerShell syntax, Workshop payload, and plant visual coverage checks. This is a static harness correction only; no fresh Windows RimWorld runtime PASS or wetland Step 1 completion is claimed.


**2026-10-08 uploaded runtime failure correction:** PR #23 / squash merge `a2ea49aecf69c8975ca26e4b84b755ba488005ca` fixes two failures observed in the AMJWarmTemperateTerrainQuickstart report (73 of 74 assertions passed; one reported preLaunch ERROR). The failed ColdBog Cypress-specific XML PatchOperationReplace was replaced with complete approved wetland `wildPlants` pools for both inherited Vanilla wetlands, preserving Phase 5 total/woody commonality. The tree-sowing Quickstart now checks the base `map.Biome.wildPlants` instead of map-wide `WildPlantSpawner.AllWildPlants`, which can include coast/river mutator plants. The separate actual growing-zone menu assertion remains in place. `Tests/test_tree_sowing_contract.py` now checks the wetland patch's approved pools and runtime wiring; `Docs/GoldenPaths/PlantSowingTests.md` records the semantics. PR CI passed Regional tree sowing contract, Workshop payload filtering and Plant visual coverage ledger. No new installed-Windows RimWorld runtime result is available yet, and wetland wildlife/disease/weather/description work remains OPEN; do not close Step 1 on the basis of this merge.



**2026-10-08 ecology implementation:** PR #24 / squash merge `3e946ad24f4e5d93ced19478219b73f865befa54` adds `Patches/VanillaWetlandEcology.xml` for both retained Vanilla wetlands: Japan-oriented wildlife proxies, adjusted humid weather/snow bands, historical-world-facing gameplay disease mixes (excluding mechanites), separate disease MTB values, and removal of inherited foreign wild pack animals. Original wetland workers, terrain patch makers and Phase 5 wild plants remain unchanged. The `Tests/Quickstarts/EnvironmentBiomeTerrainQuickstarts.cs` wetland scenarios now require loaded Def/terrain assertions; `Tests/test_wetland_ecology_contract.py` statically guards all ten replaced fields. The full numeric provisional balance and **unapproved Japanese-first description drafts** are recorded in `Docs/VanillaWetlandBiomeAudit-ja.md` and `Docs/Design.md`. PR CI passed Regional tree sowing contract (including the ecology regression), Workshop payload, and Plant visual coverage. **OPEN:** new English/Japanese runtime descriptions require approval of the Japanese drafts before translation; installed RimWorld runtime and world wetland-share sanity remain unverified. Do not mark vegetation roadmap Step 1 DONE or unblock ENV-RETEX-012 on CI results alone.


### ENV-RETEX-012 — existing-tree retention audit and AMJE description rewrite

**Requested by:** author (2026-10-08 JST)  
**Owner:** Environment vegetation / localization / art  
**Status:** BLOCKED — wait for wetland Biome bundle correction + Step 1 completion test

Before any broad Vanilla / Medieval Overhaul tree retexture pass, re-audit the trees and ground vegetation AMJE currently leaves in place. The previous rationale that human-created pine woodland / grassland / secondary forest justifies retaining Vanilla vegetation is rejected: those historical vegetation forms must themselves be represented by species and vegetation appropriate to ancient/medieval Japan. Do not assume an existing PlantDef should remain merely because it is already present. For each candidate, decide whether it belongs in AMJE's target region, pre-Edo scope, vegetation bands and landscape role; remove/replace/non-adopt targets that are unnecessary or inappropriate.

Only trees retained after that audit proceed to the later art pass. Their inherited Vanilla/MO descriptions must also be rewritten into the established AMJE plant-description format, Japanese-first, under the shared historical-description rules before English synchronization. The audit explicitly includes correcting culturally or historically mismatched inherited wording; the Vanilla bamboo wording that describes bamboo as not beautiful is a named review target.

Durable policy is recorded in `Docs/Design.md` sections **11.5.5–11.5.8**. The required order is **retention audit -> distribution/ownership decision -> Japanese description audit/rewrite -> author/content approval -> retexture**. Do not begin the retained-Vanilla art pass before description review. After the retained Vanilla set is complete, the roadmap proceeds to missing Japanese vegetation/medicinal plants and only then to final Wild Healroot removal.

The 2026-10-08 wetland whole-Biome audit blocks this item temporarily. Do not start retained-Vanilla description review until `TemperateSwamp` / `ColdBog` wildlife, disease, weather and description bundles are corrected and the Step 1 completion test passes.

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

### DOC-WORKSHOP-003 — linked named mods and post-Beta retexture roadmap

**Requested by:** author (2026-10-06 JST)  
**Owner:** Documentation/release  
**Status:** DONE

README remains the detailed public source and now has direct links for Ancient & Medieval Japan Core, CCTO, Medieval Overhaul, and Harmony plus an explicit Planned follow-up section for staged Vanilla / Medieval Overhaul tree retextures.

Japanese Workshop copy was updated first, then English synchronized. Named external/related mods now receive direct links without repeating long URLs on every occurrence: published mods use Steam Workshop links; AMJ Core currently links to its GitHub repository because no public Workshop item is recorded. The development-status heading now explicitly includes future plans, with the existing post-Beta Vanilla / Medieval Overhaul tree-retexture plan retained as the summary.

Current maintained description sizes remain below Steam's 8,000-byte limit: Japanese 7,715 bytes; English 7,838 bytes.

Steam publication itself remains author-manual and is not claimed by these repository changes.


### DOC-WORKSHOP-002 — Biome rationale and representative-tree section

**Requested by:** author (2026-10-06 JST)  
**Owner:** Documentation/release  
**Status:** DONE — Japanese Workshop summary approved and English synchronized

README contains the detailed public explanation of why AMJE uses four broad vegetation/climate bands and why each band receives one AMJE-owned structural representative: Warm-temperate / Sudajii, Cool-temperate / Japanese beech, Subalpine / Shirabiso, Alpine / Haimatsu. It also includes the four AMJE in-game plant graphics and longer ecological/historical summaries.

The approved Japanese Workshop source is the concise summary of that README content. Japanese and English Workshop sources now explain the same biome/plant rationale, embed the four AMJE in-game production PNGs via raw GitHub URLs, and keep the plant notes shorter than README/in-game descriptions. These images are the actual Mod graphics, not real-world tree photographs.


### LOC-ENV-001 — Kanji / alias opening pass for current AMJE descriptions

**Requested by:** author (2026-10-06 JST)  
**Owner:** Environment localization  
**Status:** DONE — expanded Japanese descriptions approved and English synchronized

The four structural-plant descriptions now use the AMJ educational structure: name/aliases → Japanese distribution/ecological context → supported ancient/medieval role or landscape context → modern difference/use where supportable. Japanese text uses literal `\n\n` paragraph breaks between logical sections for RimWorld readability, and the English Def defaults now translate that approved Japanese content with aligned paragraph structure.

The descriptions deliberately distinguish direct evidence from landscape context. Sudajii uses Early-Jomon Castanopsis fruit-use evidence; Japanese beech uses Jomon vegetation evidence plus late-Heian/medieval Fagus-genus turned-wood evidence; Shirabiso and Haimatsu do not fabricate specific medieval resource uses, and instead connect their verified Mt. Ontake vegetation roles with independently attested later-medieval mountain worship.

Shared durable policy: Core `Docs/HistoricalDescriptionGuidelines.md` commit `9859b6d4a9d6a5a7f4829ae067c5c79318ab57ee`.

### ENV-ART-RULES-011 — shared AMJ art-rule consolidation

**Requested by:** author (2026-10-06 JST; AMJ-wide rule cleanup)  
**Owner:** Environment art / documentation  
**Status:** DONE — follow-up audit and art-rule CI green

Environment art documentation now inherits the Core `Docs/ArtStyle.md` project-wide invariants instead of carrying a second full shared prompt/rule set.

- `AGENTS.md` is a routing layer only for art work.
- `Docs/ArtDirection.md` owns Environment-specific vegetation/terrain/world rules and accepted species baselines. Its restrained soft-gradient allowance is explicitly recorded as a tree/plant class difference from the flatter Core crop/item budget.
- `Docs/GoldenPaths/RetextureGeneration.md` owns preflight/reference loading only; the duplicated frozen generation prompt was removed.
- `Docs/GoldenPaths/TextureAssetPipeline.md` owns installation/runtime validation and only points to the shared fixed-template policy when actually needed.

Source-of-truth commits:
- AGENTS routing: `82497e955bf5aeaa26d6c656724e912ec4045242`;
- generation entry cleanup: `d9450de315c66ac5ace71b032bf5106d82bee08f`;
- ArtDirection inheritance/exception clarification: `c8ef22a14769b3e8f708ae0da908d39040fca469`;
- texture pipeline deduplication: `69d8833629c88d79a91e41e517d7e04c94e31969`.

**Follow-up audit (2026-10-06 JST):** Retexture generation now assembles the current Core+Environment rules instead of using a frozen prompt, does not insert a mandatory pre-generation approval loop, and reviews candidates against the current request plus any already-approved target design. The Golden Path index was synchronized, accepted Sudajii/beech reference blobs are regression-locked, and stale completed visual-review statuses were reconciled. `.github/workflows/art-rule-structure.yml` runs the dedicated documentation/routing guard; run `37442081809` passed.


### ENV-001 — Initial Japan-style world generation

**Requested by:** Environment/design  
**Owner:** Environment/worldgen  
**Status:** DONE

Establish the first playable world-generation baseline for a Japan-like environment:
- bounded Japan-oriented temperature range;
- Japan-like elevation and hilliness distribution;
- increased small/medium river frequency;
- no naturally generated Large/Huge rivers;
- more coastline through more indented coasts, bays, peninsulas and islands rather than simply reducing land area.

The authoritative Alpha targets are in `Docs/Design.md`.

River XML and the first C# world-generation prototype are now implemented in source:
- Creek 30,000 / 0.70 and River 75,000 / 0.80;
- Large/Huge natural spawning disabled while their Defs remain available;
- Vanilla Terrain generation is now Harmony-postfixed: Vanilla runs normally first, then Environment applies Japan-oriented elevation/coastline, hilliness, annual-temperature and rainfall transforms and reselects the biome;
- seasonal amplitude uses the Environment 8–16 C curve on the root surface;
- separate daily random variation is reduced to 3/7 of Vanilla on the root surface after CCTO climate calibration;
- Alpha diagnostics automatically log terrain/coast/hilliness and river statistics;
- Harmony is the sole technical dependency;
- `run-tests.bat` builds the DLL and validates the installed RimWorld 1.6 source-Def assumptions.

**Local build finding:** first build attempt failed at `WorldGenStep_AMJEnvironmentTerrain.cs` because `PlanetLayer.GetTileCenter()` exposes RimWorld 1.6 API metadata using `Unity.Mathematics.float3/int3`, while the legacy build script referenced only Assembly-CSharp / UnityEngine.CoreModule / netstandard / Harmony. This is a build-reference issue, not a worldgen logic failure. `build.bat` and the SDK project now reference both `Unity.Mathematics.dll` and `Unity.Collections.dll` from RimWorld's Managed directory. Fix commits: `c122e9c065c1dce49d7c2bc8413eecbea805c5d8`, `3646a352a9b6a78802db4bad61cd2c7f627df73b`.

**Runtime smoke finding:** the first game launch failed while loading the patched `WorldGenerator.xml`: RimWorld could not resolve `AncientMedievalJapan.Environment.WorldGenStep_AMJEnvironmentTerrain` from the XML `Class` attribute. The custom-worker XML replacement has therefore been removed. Terrain processing now uses a Harmony postfix on Vanilla `WorldGenStep_Terrain.GenerateFresh`, preserving the same required ordering (Vanilla terrain first, Environment transform second, later world-gen steps afterward) without custom Def-time type resolution. Fix commits: `41aa8a30e0b2565e169c82383f755e1005b5b16c`, `68653067cae51bc0fb71cd75b7e73bb8b2339dd6`. Design/validator updates: `23088d177cd8d6b0787edbac799b5a4a62c4ea03`, `2df3ca4a99200520dd58354d95ee8bb78b898303`.

The previous build/static PASS predates this integration change and is no longer the current gate result.

**Next action:** pull the latest main and rerun `run-tests.bat "D:\SteamLibrary\steamapps\common\RimWorld"`. If it passes, relaunch RimWorld with Harmony + Environment, enable Dev Mode, and generate the first test world. Use the `[AMJ Environment] Terrain summary` and `[AMJ Environment] River summary` log lines to compare the generated result against the Alpha targets. Climate validation must then use actual temperature samples against the CCTO/AMJ 10/8/5/0 C growth thresholds and -1/-4/-8 C cold-death reference thresholds, not annual mean alone.

**First successful world-generation smoke:** PASS for runtime execution, but balance requires another iteration.

Observed summary:
- tiles: 119,904;
- land: 54,546;
- coastal land: 4,167 / **7.6%**;
- annual-mean range: **4.0..20.0 C**;
- transformed elevation range: **-300..2034 m**;
- Hilliness: Flat **27.1%**, Small **36.9%**, Large **22.0%**, Mountainous **10.2%**, Impassable **3.7%**;
- river tiles: 5,181 / **4.3%** of all tiles;
- river edges: 5,000;
- Creek: 2,582; River: 2,418;
- **no LargeRiver/HugeRiver generated**, matching the intended river-size rule.

Interpretation:
- river size filtering is working and the 4.3% river-tile share is suitable for continued testing;
- Small Hills are substantially over target while Mountainous terrain is substantially under target;
- maximum elevation 2034 m is too low to exercise the intended rare >3000 m highlands;
- because elevation is too low, the generated annual-mean minimum only reached 4 C and did not exercise the intended cold/highland range;
- coastline cannot yet be judged against the 1.5x target because the Vanilla same-seed baseline was not logged.

Second-pass implementation:
- Hilliness now uses a spatial ruggedness ranking aimed at the 25/20/25/25/5 Alpha distribution instead of incremental Vanilla-category promotion;
- Large/Mountainous/Impassable tiles receive progressively stronger elevation uplift, with rare high peaks capped at 3800 m;
- diagnostics now log the Vanilla same-seed coastal baseline before Environment modification and report land-only temperature/elevation plus >=1500/2500/3000 m shares.
Implementation commits: `c87fe52cc5451ae8ca3059fc90a4f0b971ec486a`, `a2256bd84cc4dd55a5ceb8944c7d09978f5f575f`.

**Next action:** pull/rebuild, generate another Dev Mode world, and capture all three AMJ Environment diagnostic lines. Compare coastline against the same-seed Vanilla baseline and verify the revised Hilliness/highland/temperature distribution before changing river values.

**Smoke-test environment note:** a later Player.log showed only the legacy two-line diagnostic format (`Terrain summary` + `River summary`) with no startup marker and no `Vanilla terrain baseline`. The observed values were land 63,643, coastal share 5.9%, annual mean -6.3..20.0 C, elevation -300..2875 m, Hilliness 21.5/31.9/29.5/13.0/4.1, and river share 4.6%. Because the log format itself predates the second-pass diagnostics, this run used an older built DLL and must not be used to evaluate the current second-pass terrain balance.

Alpha diagnostics were therefore made independent of Dev Mode and an assembly/Harmony startup marker was added. Commits: `a82f55cb78db53f3ea66acd34ff2e7dd8b529047`, `78ab298a8088f3623db3b333b2f3f5b53a093d3f`.

**Current local verification gate:** pull latest `main`, rebuild with `run-tests.bat`, fully restart RimWorld, then confirm `[AMJ Environment] Assembly loaded; Harmony patches applied.` plus the three current world-generation diagnostic lines before evaluating balance.

**Current second-pass smoke:** runtime and balance targets are aligned on the first valid current-DLL seed.

Observed:
- Vanilla: land 61,209; coastal land 3,084; coastal share **5.0%**;
- Environment: land 61,674; coastal land 4,655; coastal share **7.5%**;
- same-seed coastline multiplier: **1.50x**;
- land count delta: **+0.76%**, indicating the coastline target was achieved primarily by increased coastal complexity rather than by flooding land;
- land annual mean: **-8.0..20.0 C**, avg **11.8 C**;
- land elevation: **0..3366 m**;
- highlands: >=1500 m **7.0%**, >=2500 m **0.4%**, >=3000 m present but rounded to 0.0% in the old diagnostic precision;
- Hilliness: **25.0 / 20.0 / 25.0 / 25.0 / 5.0%**, exactly matching the current Alpha distribution target;
- river tiles: 6,511 / 119,904 = **5.4% of all tiles**; Creek 3,387, River 2,895; no Large/Huge.

Interpretation:
- coastline, Hilliness, annual-mean range, and rare highland generation all meet the current single-seed Alpha intent;
- river size filtering remains correct;
- this is not yet a final lock because the design requires several seeds and the previous river percentage used all world tiles rather than land tiles.

Diagnostics were expanded before multi-seed validation:
- same-seed coast multiplier and land-count delta are now printed directly;
- >=2500 m and >=3000 m highlands include counts and finer percentages;
- land rainfall and biome distribution are logged;
- river diagnostics now include river-bearing **land-tile share** in addition to all-tile share.
Commit: `81c7729be58ed1b8b2af5cd69a3eb598c74b2cdc`.

**Next action:** rebuild, then generate at least two additional seeds at the same planet coverage and compare the expanded Environment diagnostics. If the terrain/coast/climate distributions remain stable, lock the Alpha world-generation baseline and move to CCTO hourly/daily climate sampling.

**Multi-seed gate result:** PASS. Two additional current-DLL seeds produced coastline multipliers **1.43x** and **1.50x**, land annual means **-7.9..20.0 C** and **-8.0..20.0 C**, exact 25/20/25/25/5 Hilliness, maximum elevations **3800 m** and **3141 m**, rainfall **832..2750** / **832..2917**, and river-bearing land shares **8.8%** / **7.9%**. Across the three current seeds the coastline multiplier averages approximately **1.48x** and land annual-mean averages remain tightly grouped at **11.5–12.0 C**. No Large/Huge rivers were produced.

Exact biome percentages are not locked because the validation mod list includes non-Vanilla biome Defs (for example `DankPyon_DarkForest`), making the histogram mod-list dependent. The terrain/climate envelope itself is accepted.

The current terrain/coast/elevation/rainfall/river Alpha baseline is now locked in `Docs/Design.md`. Further tuning requires a gameplay or compatibility finding rather than single-seed numerical preference.

**Result / references:** initial design `9f5fd58c77b8e9ae5bad00851189d0127a122925`; CCTO calibration `e93da687fcd543f6d3ec94d5398fc604c0559749`; river patch `40d2b6b5615ea26ac6d91ee10f2433e3bd474829` + compatibility hardening `8aee69c23726a08f72b101ccd22a1e2065846364`; terrain prototype `8080b41f144fbacbf31411e11b7853860cd5703b`; climate hooks `16d870ee74a8e259de8233bfc84148ae48cfcf4d` / `b8b84723adf254b3f1bbed7a75cce5223a16222e`; build/static gate `aa87755d6099e89fda36de40acf358fd9bfebb68`, `a1da1cf981d22239f1833765835106804636814c`, `3d5fb2e52058f7d518b56e98620de3ee2af92fc4`; diagnostics `3403742e6c57d89c611cb94f338fff2ff15ef647`.


### ENV-002 — CCTO runtime climate calibration

**Requested by:** Environment/design  
**Owner:** Environment/climate  
**Status:** DONE

Validate the locked world-generation baseline against actual RimWorld 1.6 outdoor temperatures used by CCTO rather than annual mean alone.

Use representative southern warm lowland, central temperate lowland, northern cool lowland, and highland tiles. Sample one in-game year through the public `TileTemperaturesComp.OutdoorTemperatureAt(PlanetTile, absTick)` API so the measurement includes the Environment seasonal-amplitude hook, scaled daily random variation, and Vanilla sun-cycle component.

Collect time below growth thresholds **10 / 8 / 5 / 0 C** and cold-death thresholds **-1 / -4 / -8 C**, plus min/max actual temperature and isolated lethal-cold events.

Automatic climate-calibration diagnostics are implemented in `ClimateCalibrationDiagnostics.cs`. Fresh world finalization selects warm/temperate/cool lowland plus highland representatives and samples `TileTemperaturesComp.OutdoorTemperatureAt` hourly for one 60-day year, including threshold-hours and lethal-event duration statistics. Implementation commit: `9d2e70dde20dfa709fb7bfe5d6ff992bc24df884`.

**First CCTO hourly calibration:** the warm, cool-lowland and highland signals matched intent, but the representative 14 C temperate lowland reached **-8.4 C** and spent **2 h below Barley's -8 C death threshold**. CCTO's cold-death path runs from the plant long-tick check and kills immediately on actual ambient temperature strictly below the threshold, so the brief event is gameplay-significant rather than ignorable. The representative also spent 136 h below -1 C and 70 h below -4 C, which correctly preserves Rice and millet winter risk. Warm lowland had no time below -1 C; cool lowland and highland had 195 h / 338 h below -8 C respectively.

**Calibration change:** keep the seasonal-amplitude curve and Vanilla ±7 C sun-cycle unchanged, but reduce the separate daily random variation from ~±4 C to **~±3 C (3/7 Vanilla)**. This targets the isolated central-lowland Barley failure without weakening northern/highland winter lethality. Implementation commit: `2df9de31cabc3cae5aab52b66f4ae7624bba6529`; validator lock: `2187c482e5d4df676f19ff19a2e2fde4ecb29765`.

**Build/static gate:** PASS after the climate-calibration diagnostics were added. The earlier CS0016 failure was a local DLL file lock from RimWorld still having `AncientMedievalJapanEnvironment.dll` mapped; closing RimWorld and rerunning the gate succeeded. This was not a code compilation failure.

**Final ~±3 C re-run:** PASS. Warm lowland stayed above -1 C; temperate lowland stayed above -8 C while still spending substantial winter time below -1 C and -4 C; cool lowland and highland remained well below -8 C for long periods. The intended Rice/Awa/Hie/Barley regional separation is therefore preserved.

**Result:** ENV-002 complete. Further climate tuning is deferred unless gameplay testing finds a concrete mismatch.


### ENV-003 — Japan vegetation bands and biome structure

**Requested by:** Environment/design  
**Owner:** Environment/biomes  
**Status:** DONE

Move from the accepted climate/worldgen baseline into Japan-oriented biome and wild-vegetation structure.

Decision:
- use four coarse natural vegetation bands: WarmTemperate >=15 C, CoolTemperate 8..15 C, Subalpine 0..8 C, Alpine <0 C;
- treat swampiness >=0.5 as a wetland candidate overlay;
- do not repurpose Vanilla TropicalRainforest/TemperateForest/BorealForest/Tundra Defs because their animals, diseases, weather and compatibility semantics would become misleading;
- create AMJ-owned BiomeDefs after validating band shares;
- ReGrowth 2 is reference only; do not copy restricted code/assets.

A preview diagnostic is implemented as `[AMJ Environment] Japan vegetation-band preview`. Commit: `4a2bf50600fbaec0a7cb5fafec02119e594c7fef`.

**Vegetation-band preview:** PASS. First representative world: WarmTemperate **27.5%**, CoolTemperate **51.4%**, Subalpine **19.5%**, Alpine **1.7%**, swampiness>=0.5 **0.7%**. This matches the intended shape; the locked worldgen climate does not need retuning.

**Implemented Alpha biome pass:**
- custom workers for the four accepted climate bands, excluding swampiness>=0.5;
- AMJ-owned BiomeDefs `AMJ_WarmTemperateForest`, `AMJ_CoolTemperateForest`, `AMJ_SubalpineForest`, `AMJ_AlpineZone`;
- temporary Vanilla world textures, existing Vanilla wild plants, and minimal Vanilla wildlife placeholders only;
- no final art yet.
Implementation commits: `c84eaf9549c21c644b3da21eed4d5558c7f8ce4c`, `3e973f5f5b5c4a984e35d1b44fd08de158fbfa5e`. Static validation commit: `14fa045d6d3e8aa794673a92389bde2469cc76ae`.

**Runtime biome distribution:** PASS for coexistence. With Medieval Overhaul active, the first measured world produced AMJ WarmTemperate **27.0%**, CoolTemperate **43.7%**, Subalpine **7.2%**, Alpine **0.3%**, MO Dark Forest **16.3%**, Temperate Swamp **2.0%**, plus small residual Vanilla shares. The lower AMJ Subalpine/Alpine shares are partly explained by specialized MO biome competition rather than a climate-band generation failure.

**Compatibility decision:** the four AMJ biomes are baseline vegetation bands, not exclusive replacements. **MO is a standard AMJ coexistence target**, not merely another optional biome mod: Environment stays technically loadable without MO, but normal AMJ play is expected to include MO. MO-owned specialized natural biomes such as Dark Forest should therefore remain available through normal BiomeWorker competition where their climate rules fit. A brief attempted score increase that would have forced AMJ ownership over MO Dark Forest was reverted. Coexistence implementation commit: `3e5dedf94743baaea7248f0073882cde0a7ddf9f`; validator restore: `7ff5ad1d637f8e5b9e4f6af790466359b1331065`.

**Natural-soil prerequisite:** complete. ENV-004 is locked for Alpha, so ENV-003 has moved into Japan-specific wild vegetation.

**Japan-specific structural vegetation implementation:** added a deliberately small four-PlantDef set using temporary Vanilla graphics:
- `AMJ_Tree_Shii` — warm-temperate evergreen broadleaf dominant;
- `AMJ_Tree_Beech` — cool-temperate deciduous broadleaf dominant;
- `AMJ_Tree_Shirabiso` — subalpine evergreen conifer dominant;
- `AMJ_Shrub_Haimatsu` — alpine dwarf-pine scrub dominant.

Existing Vanilla oak/maple/birch/pine/bamboo remain at lower commonality as secondary/placeholder components rather than being removed. No new food/medicine/fiber item types were added; the trees yield ordinary wood, while haimatsu is non-timber low scrub. Final graphics remain deferred.

Implementation commits:
- PlantDefs: `00619f365ffbd4e14d88f3956a5af19cbe1ed60f`;
- Japanese localization: `f71899c7afdf3bd89456f175f0a1e5f651076652`;
- biome commonality: `858009e67afd6607652559cdf698d72bf1875fc3`;
- static validator: `582478b9a6f13ab4fc71e546cdaf600bad6c997c`;
- design source of truth: `258fd93af61570d9ae0082ef494993969deabf3d`.

**Local static-gate finding:** the first vegetation validation run failed before XML semantics because Windows PowerShell 5.1 decoded the BOM-less Japanese localization file with the local ANSI code page. The repository file itself is valid UTF-8, but the validator's plain `Get-Content` produced mojibake and consumed bytes around closing tags, making the decoded text appear malformed. The validator now reads the Japanese localization explicitly with `-Encoding UTF8`. Fix commit: `f400a230f366f96d46387630aa844d4d9ae9e090`.

**Local build/static rerun:** PASS after the UTF-8 validator fix. The Japan-specific vegetation PlantDefs, biome wiring, Japanese localization, normal Environment DLL, developer Quicktest DLL, and static validation gate all completed successfully.

**Runtime validation automation:** the four manual vegetation Quicktests have been replaced by a one-command isolated runtime gate. Each fixed-biome Quickstart now implements `Verify()` assertions for biome identity, target structural-plant generation, target dominance over secondary trees, and Alpine safety limits (Haimatsu <=5% of cells; full-size Pine+Birch <=1%). The suite prepares an isolated Core+Harmony+RimLogging+Quickstarts+Environment profile, launches the four biomes sequentially, writes JSON/log output, exits automatically, and fails on Environment-origin ERROR entries.

Implementation:
- Quickstart vegetation assertions: `afd57825808691b48b6ea5d1ac0247954103573d`;
- isolated runtime profile: `65ac0924de6f2edb664b9fc129adffe587a2fa35`;
- mod-origin runtime ERROR gate: `7c251c6301d9373cffb989d2e12abf74e4a3113e`;
- four-run PowerShell runner: `9b59e34892919edfa8711fcddac9e64c21c1a197`;
- one-command batch entry point: `8621ca8e5bf0bc4d828aa98b50409b019fdb07e2`.

**First automated runtime run:** FAIL, and the gate correctly exposed two independent issues before vegetation assertions could run.

1. With the isolated profile intentionally excluding Medieval Overhaul, `Patches/Compatibility/MedievalOverhaul.xml` still attempted its Dark Forest `PatchOperationReplace`. The top-level operation-level `MayRequire` did not skip the patch in this structure. The compatibility patch is now wrapped in `PatchOperationSequence` with `MayRequire="DankPyon.Medieval.Overhaul"` on the child replace operation, so Environment can load cleanly without MO. Fix: `67ae970dd8bb2dc2dd9466594816470d8ed59ad5`.

2. The fixed 5% world had no usable WarmTemperate tile and no worker-positive proxy, so the test stopped with `No usable world tile found for biome test AMJ_WarmTemperateForest`. Fixed-biome developer tests now have a final generic synthetic-climate fallback: select the nearest unoccupied land tile, set only that test tile to the target band's representative annual temperature (17.5 / 11.5 / 4 / -4 C), ensure rainfall >=800 and swampiness <0.5, then assign the requested biome. This is test-only and does not alter normal world generation. Fix: `195cc8dd5ba12ad6f42e468dfd43c8923400cb19`.

Static validator synchronized with both fixes.

**Runtime report audit correction:** the later exported Quickstarts reports showed that the nominal four-biome PASS was a false positive at the harness level. Every standalone report had `passed=true`, `failed=0`, and all vegetation assertions passed, but also `preLaunchErrors=2`. The corresponding startup log errors were AMJE-owned XML inheritance failures:

- Shirabiso: inherited `visualSizeRange` text `1.5~2.0` was merged with child `<min>1.25</min><max>2.7</max>`;
- Haimatsu: inherited `visualSizeRange` text `0.7~1.1` was merged with child `<min>0.45</min><max>0.75</max>`.

The structural distribution measurements themselves were otherwise healthy: Warm Shii 3.21%, Cool Beech 3.21%, Subalpine Shirabiso 1.29%, Alpine Haimatsu 0.69%, with Alpine full-size Pine+Birch only 0.02%. These values do not indicate balance retuning.

Fixes:
- both custom FloatRange overrides now use `Inherit="False"`: `fe6f641d671af41979b36dfc8649d17fb37f4b5e`;
- the runner now fails when `preLaunchErrors > 0`: `aa6b7098d825f0e7de67803d1f4053ad909d7cbe`;
- static regression checks cover both fixes: `5a8d195529318b7802c95a02423246c40cb06c57`.

**Final clean rerun:** PASS. All four standalone reports returned `passed=true`, `failed=0`, `preLaunchErrors=0`, `captureLive=true`, and `logTruncated=false`; no report log contained a structured `Level: ERROR` entry. Current target shares were Shii **3.14%**, Beech **3.22%**, Shirabiso **1.29%**, and Haimatsu **0.68%**. Alpine full-size Pine+Birch was **0.03%**, well below the 1% safety limit.

**Result:** ENV-003 is complete for the Alpha structural vegetation stage. The composition/commonality values remain accepted without retuning. Final art remains deferred to the visual-art pass.


**Public wildlife-description sync (2026-10-08):** PR #17 / squash merge `88f87fd8997f8cd2476be49db5cdd8171c57e17b` synchronized README, Workshop JA/EN, 2game, About and changelog so the player-facing scope now explicitly includes Japan-oriented wildlife distribution. Wording clarifies that AMJE curates Vanilla animal pools as functional gameplay proxies rather than claiming literal Japan-specific species additions. No wildlife balance/implementation values changed in this PR.

### ENV-005 — Optional CCTO integration for AMJE plants

**Requested by:** Environment/design  
**Owner:** Environment/compatibility  
**Status:** DONE

Ownership decision:
- AMJE standalone is intentionally a complete "medieval Japan-like environment" experience without CCTO;
- CCTO is an optional realism layer: combining it with AMJE adds stricter species-specific cold-growth/death/dormancy behavior rather than unlocking basic Environment functionality;
- AMJE owns cold-tolerance compatibility for AMJE-owned plants;
- CCTO remains unaware of AMJE and receives no AMJE-specific balance/code;
- AMJE remains fully usable without CCTO;
- AMJE conditionally uses CCTO's public XML-facing `ColdToleranceExtension` only when CCTO is active.

Two-layer temperature model:
- standalone AMJE explicitly keeps the Vanilla-style `minGrowthTemperature=0 C` baseline on Sudajii, Beech, Shirabiso, and Haimatsu;
- with CCTO active, AMJE's own optional compatibility patch changes the four targets to:
  - Sudajii: min growth 8 C, fixed death -8 C;
  - Beech: min growth 5 C, cold dormancy;
  - Shirabiso: min growth 0 C, fixed death -35 C;
  - Haimatsu: min growth 0 C, fixed death -35 C.

Implementation:
- explicit standalone baseline: `89cf48d65786547b6a7f3fb8dd2027313ef07adf`;
- AMJE-owned optional CCTO patch: `ac99339fc70f290b39ca06b6702a0b4a2e9d1faf`;
- optional load order, not dependency: `f1967a832005bef342aa87999b35bf3fe9dbddbd`;
- runtime loaded-Def assertions: `13772aec27dbccef01889ae59dfa6a66a8b462b1`;
- isolated optional-CCTO profile support: `d69428bb7bfe2ae9952d9b4920fb265abdb030e4`;
- focused compatibility runtime mode: `a7bbae4f6c3bad52fa6c1e63b9ff0ff6b540bed6`;
- one-command optional CCTO sub-gate: `81ba86a3cf7917fb663084ae2f151eae60d7a61e`;
- standalone loaded-Def isolation assertions: `574077b196d26c50922ae9fc56cac641e2400eb8`;
- optional-sub-gate exit-code handling: `43ecf5f52666bb3641fc1ddc581b50450199742e`;
- static validator: `719a91373d4b38a105c3ec7582ca588301b92ccd`;
- design source of truth: `cbf246511026b45ee687c402b8a7d75510623522`;
- product/balance positioning (AMJE standalone complete, CCTO optional realism layer): `5a104a8f09ac084fbd61aebedbfe82b02a5ba57e`;
- development tooling docs: `8ea3386080de63218e23021d2fa4f20cbc65dda3`.

**Verification status:** exported standalone and AMJE+CCTO reports confirm that all loaded-Def CCTO assertions themselves passed, including the 0/5/8 C growth values, dormancy/death behavior, and exactly one CCTO extension per AMJE target. However, the same two AMJE `visualSizeRange` startup XML errors appeared in the CCTO report as `preLaunchErrors=2`. Therefore ENV-005 is **not** accepted yet; the compatibility assertions are good, but the corrected package must rerun with `preLaunchErrors=0`.

The report-audit fixes are shared with ENV-003: `fe6f641d671af41979b36dfc8649d17fb37f4b5e`, `aa6b7098d825f0e7de67803d1f4053ad909d7cbe`, `5a8d195529318b7802c95a02423246c40cb06c57`.

**First post-change local build finding:** the normal Environment DLL built, but the developer Quicktest DLL failed because RimWorld 1.6 exposes `GetActiveModWithIdentifier` on `Verse.ModLister`, not `Verse.LoadedModManager`. The CCTO-active probe now uses `ModLister.GetActiveModWithIdentifier(..., true)`. Fix: `6e2ad338228fc380342e444e81a79eb45bd385a6`; validator lock: `57233f5ef00aef3af82f00503a39c36ba30ae170`.

**Final clean AMJE+CCTO rerun:** PASS. The focused compatibility report returned **29/29 assertions passed**, `preLaunchErrors=0`, complete live log capture, and no structured `Level: ERROR` entry. Loaded Def checks confirmed the intended two-layer values for all four AMJE plants and exactly one CCTO extension per target.

**Result:** ENV-005 is complete. AMJE remains fully functional standalone with its Vanilla-style temperature baseline, while CCTO remains an optional higher-realism layer owned through AMJE's conditional compatibility patch.


### ENV-006 — Regional weather baseline

**Requested by:** Environment/design  
**Owner:** Environment/weather  
**Status:** DONE

Alpha policy:
- reuse the eight Vanilla WeatherDefs rather than introducing AMJ-specific weather types;
- let Vanilla rainfall-factor curves and temperature ranges provide the first layer of regional/seasonal differentiation;
- keep Warm/Cool liquid precipitation and fog stronger than Subalpine/Alpine;
- progressively increase snow commonality toward Subalpine/Alpine;
- make dry thunderstorms rare in the humid-Japan baseline while retaining rainy thunderstorms as the more common severe-storm proxy;
- do not add calendar-specific Baiu/Akisame/typhoon weighting yet;
- do not fake Sea-of-Japan-side vs Pacific-side winter exposure from vegetation-band names alone.

Implementation:
- dry-thunderstorm commonality reduced to 0.1 in Warm/Cool/Subalpine and 0.05 in Alpine: `d695f3cc74ec50effd75c65ae8955c3d6976a603`;
- fixed-biome Quickstarts now assert all eight loaded weather commonalities and rainy > dry thunderstorm: `098e9b55f968b0eb17f4bb98a9cf17d153ec5895`;
- static validator locks the accepted table: `dbf80ab37c56da734646aba770e9d43e48096dca`;
- design source of truth and JMA research basis: `4561fdb8cee7ead6c9cf042adea78bd4f26244fa`.

**Final automated runtime result:** PASS. WarmTemperate, CoolTemperate, Subalpine, and Alpine all returned `passed=true`, `failed=0`, `preLaunchErrors=0`, and `logErrors=0`. Every loaded weather commonality matched the accepted Alpha table and rainy thunderstorms remained more common than dry thunderstorms. The optional AMJE+CCTO run also passed **39/39** assertions with `preLaunchErrors=0` and `logErrors=0`, confirming that the optional realism layer does not alter Environment weather.

The remaining startup WARN messages are generic RimWorld metadata warnings about dependencies lacking `downloadUrl` / `steamWorkshopUrl`; they are not Environment runtime errors and are outside this weather gate.

**Result:** ENV-006 complete. Calendar-specific Baiu/Akisame/typhoon weighting and Sea-of-Japan/Pacific-side winter exposure remain deferred unless a later gameplay finding justifies the added system complexity.


### ENV-007 — Seasonal scenery baseline

**Requested by:** Environment/design  
**Owner:** Environment/seasonal scenery  
**Status:** DONE

Alpha decision:
- do not add a custom seasonal-scene controller;
- reuse Vanilla snow accumulation/rendering from SnowGentle/SnowHard;
- use Japanese beech as the deciduous structural representative through `DeciduousTreeBase`, inherited fall shader behavior, and its leafless graphic;
- keep Sudajii, Shirabiso and Haimatsu evergreen;
- defer final AMJE-specific plant/environment artwork until the final-art pass.

Implementation/test coverage:
- loaded seasonal-scene runtime assertions: `fcca9ce0c3b33fa61ab2b89dce4d9ece685fc784`;
- static validation of deciduous/evergreen profile and test markers: `213aa5a9404e4bb327a4493ed8f8bfc9ddfac525`;
- design source of truth: `b3aa742f9b8c4a4c7479546def9fbc5db81b26d1`.

**Final automated runtime result:** PASS. WarmTemperate and CoolTemperate passed **31/31** assertions, Subalpine and Alpine passed **29/29**, and the focused AMJE+CCTO run passed **43/43**. Every report returned `preLaunchErrors=0`, `logErrors=0`, `captureLive=true`, and `logTruncated=false`. The new seasonal checks confirmed the loaded beech leafless graphic, inherited Vanilla fall-shader behavior, evergreen status of Sudajii/Shirabiso/Haimatsu, and positive SnowGentle/SnowHard snow rates.

One existing Environment warning may appear in the small fixed-world Quickstarts when climate calibration cannot find a Highland representative tile. It is a non-fatal diagnostic from the calibration sampler, not a seasonal-scene failure and not an ERROR-level event.

**Result:** ENV-007 complete. No custom seasonal controller is needed for Alpha. Final texture/visual quality remains deferred to the final-art pass.


### ENV-008 — River/coast world-to-map handoff

**Requested by:** Environment/design  
**Owner:** Environment/world-to-map integration  
**Status:** DONE

Decision:
- AMJE continues to own river/coast distribution at world level only;
- do not add a custom local-map river/coast generator;
- rely on RimWorld 1.6 `WorldGenStep_Mutators` and Vanilla `River` / `Coast` tile mutators;
- validate that Environment-generated river/coastal world tiles still produce actual local River/Ocean terrain;
- keep waterfalls deferred.

Implementation/test coverage:
- dedicated River and Coast handoff Quickstarts: `b6189c8c5b0e172020dd8cdc225e6415d755f180`;
- standalone runtime runner now includes both handoff tests: `71fd964f8b194ce3ee3feee94fdc1311998f7636`;
- static validator covers both tests/runner entries: `6680f276ab1cf5dd16026a4e775fde0ad3278d32`;
- design source of truth: `2816449f2c90d27b8f117d47e2e991b1d13ba232`.

**Final local runtime result:** PASS, reported by the author after running the combined Environment runtime gate with the new River and Coast handoff Quickstarts enabled. The run therefore satisfies the current gate as implemented, including the River/Ocean terrain assertions and the existing pre-launch/runtime ERROR checks. Detailed assertion counts and water-cell counts were not supplied for this run and are not recorded as fixed evidence.

**Result:** ENV-008 complete. AMJE retains world-level ownership only; Vanilla River/Coast mutators remain the accepted local-map generation path. Waterfalls remain deferred.


### ENV-009 — Functional Vanilla wildlife proxy baseline

**Requested by:** Environment/design audit  
**Owner:** Environment/biomes  
**Status:** DONE

Responsibility:
- Environment does not add Japan-specific animal Defs merely for regional flavor;
- AMJ-owned BiomeDefs still require functional wildlife pools, so Alpha uses curated Vanilla PawnKindDefs as gameplay proxies;
- proxy names are not literal historical-species claims.

Implemented cleanup:
- removed Raccoon, Elk, Ibex, Fox_Arctic and Lynx from AMJ biome pools;
- replaced the Alpine Elk/Ibex/ArcticFox/Lynx placeholder mix with Deer/Fox_Red while retaining existing generic hare/wolf proxies;
- runtime assertions verify retired placeholders have zero commonality and accepted proxies remain positive through the public `BiomeDef.CommonalityOfAnimal` API.

Implementation/test history:
- wildlife pool cleanup: `bc949740fad30df8d7cf68169c3c87318de09139`;
- original runtime proxy assertions: `9cccbc3a80eee3e3ebc7a9550fde01d3093ee11a`;
- public-API runtime fix: `9d8d4c5ce95753e0e4d0c145dffb9d374aab30c6`;
- original static/localization coverage: `1d5ad32d47c8da48af9e8ecae8a9c3a37b9c85aa`;
- static validation now locks the full accepted positive proxy sets as well as the retired list: `bef56f78096fff07eb8c6a80bf53feba48e480b4`;
- design source of truth and research/proxy interpretation: `6a12906195ba153efc98c67c17f7ddd5fa125894`.

**Runtime status:** PASS by inheritance from the combined Environment runtime gate reported by the author during ENV-008 verification. The wildlife assertions were already present in the Quicktest source before the River/Coast handoff tests were added, so that successful combined run exercised them. No separate manual wildlife smoke is required.

**Result:** the Alpha wildlife layer is a functional Vanilla proxy baseline only. Future Japan-specific animals, textures or animal-resource loops require a separate owning feature/mod and should not silently expand Environment's scope.


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


### ENV-004 — Low-fertility natural terrain

**Requested by:** Environment/design  
**Owner:** Environment/terrain  
**Status:** DONE

Add meaningful naturally poor growable ground after the initial biome structure, without duplicating Vanilla Gravel or MO agricultural improvements.

Implemented Alpha pass:
- one new `AMJ_ThinSoil` TerrainDef at fertility **0.50**;
- reuse Vanilla `Gravel` at **0.70**, `Soil` at **1.00**, and `SoilRich` at **1.40** rather than adding a redundant second poor-soil tier;
- biome-specific `terrainsByFertility` thresholds make poor/stony ground progressively more common from warm/cool forest toward subalpine/alpine;
- targeted MO Dark Forest compatibility preserves the MO biome while applying the same natural-soil ladder;
- player-created MO `DankPyon_PlowedSoil` remains untouched;
- fresh-map diagnostics report terrain shares automatically.

Implementation: `8f0324b1476ce9f7ffd082f74114d8bd6bda19ee` (TerrainDef), `dc9dec1e5c5936261fc825b0dba05a818d1c878b` (AMJ biome distribution), `0869bd7d9c4f3fdabef3b811050ccf9be1a9ee07` (MO compatibility), `0e88f29467b3237bc160733eb799b164c710e654` (map diagnostics).

**Build finding:** the first ENV-004 local build failed because the legacy .NET Framework compiler used by `build.bat` does not support the C# `nameof` expression in `MapTerrainDiagnostics.cs`. This is a compiler-syntax compatibility issue, not a map-terrain logic failure. The Harmony attribute now uses the equivalent string literal `"GenerateMap"`, and the static validator was updated to match. Fix commits: `81852f75fc9b98c8215843a8760bc4eaa3fdbb68`, `6ef7af956751965479497b1a32c4e8d007b40ad5`.

**Fixed-biome test tooling:** the previous random QuickTest is unsuitable for cross-biome terrain comparison because its landing biome is random. Environment now ships five developer-only RimWorks Quickstarts (WarmTemperate, CoolTemperate, Subalpine, Alpine, MO Dark Forest) behind `IfModActive="rimworks.quickstarts"`. They share a fixed world seed, generate 250x250 maps, and prefer Flat settlement tiles before hillier fallbacks so terrain-threshold comparisons are less noisy. Source: `498cfced568ea9b55947baa273029339dab92b05`; conditional load folder: `4c58adcda39fa76ca4cad8c414b9961bf76127a5`; conditional build support: `f7aeff8d72282edf38bd5f4c2ba4fc850da1fc44`; optional load order: `3f5da75fdf2f97935d4248a3a3dff04f96ae6950`; validator: `a65965ee9ff2d9bdc68535695fae3f8764140aa0`.

**Load-diagnosis note:** the developer Quicktest DLL can build successfully yet still fail to appear in the Quickstarts picker if RimWorld never loads the conditional `DevQuickstarts` folder. The Quicktest assembly now emits `[AMJ Environment Quicktest] Developer quicktest assembly loaded.` from a static constructor so runtime loading can be verified directly. Diagnostic commit: `7316c9a7e8ee3b4a3c209e5da8ce8e467763dbb8`.

**Runtime diagnosis after missing Quicktest load marker:** the conditional XML syntax matches RimWorld 1.6 source, so do not change it blindly. First runtime result was `quickstartsActive=False` and Environment loaded only its root folder, even though the user is using Workshop item 3793646067. This points to either a package-id mismatch between the installed Workshop build and the expected `rimworks.quickstarts`, or the Workshop item not being present in RimWorld's active-mod list. The startup diagnostic now also enumerates any active mod whose display name or package ID contains `Quickstart`, with its player-facing package ID. Implementation: `c40d9c72f93a41b67efde0b25904ca412afe294e`.

**Quickstarts diagnosis resolved:** Workshop item 3793646067 had not actually been enabled; the visible button was RimWorld's Vanilla Dev QuickTest button. Once Quickstarts is active, the fixed-biome test assembly can be used as designed.

**Runtime terrain samples:** WarmTemperate reported Thin/Gravel/Soil/Rich/Other = 11.3/35.5/27.4/3.2/22.5%. CoolTemperate reported 16.3/37.8/29.8/4.1/12.0%. Subalpine reported 16.9/16.5/4.8/0.8/61.0%. Among only the four fertility-ladder terrains, Subalpine is approximately 43.3% Thin, 42.3% Gravel, 12.4% Soil, 2.0% Rich, so the intended strong fertility degradation is present; the high Other share still needs interpretation before threshold tuning.

**Def error found during fixed-biome runs:** `AMJ_ThinSoil makes terrain filth and also accepts it.` Cause: AMJ_ThinSoil declared `generatedFilth=Filth_Dirt` without inheriting Vanilla `NaturalTerrainBase`, leaving `filthAcceptanceMask` at TerrainDef's default `Any`. Fix: inherit `NaturalTerrainBase` and align with Vanilla soil semantics using `categoryType=Soil` and the `Soil` tag. Implementation: `7646617cec9272d1589c308fdd65abda4c3e7c07`; validator: `f626fbc21dee496be824a36bf3005a0a438307bc`.

**Fixed-biome runtime follow-up:** Dark Forest reported Thin/Gravel/Soil/Rich/Other = 8.9/14.0/6.4/18.1/52.6%. Its high Rich and Other shares differ sharply from the AMJ forest biomes and should be interpreted with MO's own terrain patch makers before changing Environment thresholds.

The first Alpine Quicktest attempt exposed two developer-tooling errors rather than terrain-balance failures:
- climate calibration still ran from `World.FinalizeInit`, where RimWorks Quickstarts has generated the world but `gameStartAbsTick` is not yet initialized; `TileTemperaturesComp` therefore logged the TicksAbs error;
- the Alpine selector required a normally valid settlement tile, but the generated rare Alpine band had none.

Fixes:
- climate calibration now runs from `Game.InitNewGame` after the absolute start tick exists: `535338a7186c937d0c465f8b40b1719af9c2e827`;
- fixed-biome Quicktests retain normal settlement preference but may use an unoccupied non-settleable target-biome tile, including Impassable only as the final developer-only fallback: `09eb629ab0bdd84bb72856dcf6d7c17e00223f83`;
- map terrain diagnostics now log the five largest Defs making up `Other`: `d14bfb197bad14a8ae4350a58b02e16521e9fa0f`.

**Second Alpine retry finding:** the fixed 5% seed contained **zero natural `AMJ_AlpineZone` tiles**, so even the non-settlement fallback could not select one. This is compatible with normal biome competition: a target worker can be eligible while another biome (notably MO Dark Forest in overlapping cold/wet climates) wins the final `PrimaryBiome`.

The fixed-biome Quicktest now distinguishes natural-world placement from terrain-threshold testing. It first prefers an exact normal settlement tile, then any exact unoccupied target-biome tile. If the target biome is absent, it selects a tile on which the target biome worker scores positively and temporarily changes only that tile's `PrimaryBiome` for the developer test. Alpine additionally falls back to the coldest suitable land tile if the 5% world has no worker-positive Alpine proxy. The selected tile logs `forcedBiome`, `originalBiome`, and `targetBiomeScore`. This does not modify normal worldgen or settlement behavior. Implementation commits: `819445d5fb2e26e52f932017ec4153bf30c47a7e`, syntax follow-up `0d4e6e4b9f591d2338ace576d3376ae8442fcdd2`.

The separate startup error `No textures found at path Things/Item/Resource/PlantFoodRaw/RawLentils` is not emitted by Environment and is independent of the Alpine selector failure; do not treat it as an Environment terrain-balance result.

**Local build finding after synthetic-biome fallback:** the normal Environment DLL built, but the developer Quicktest DLL failed with CS0266 because `Find.WorldGrid[PlanetTile]` exposes the base `Tile` type while the fallback assigned it directly to `SurfaceTile`. All Quicktest world-grid reads that require `SurfaceTile` now use an explicit `as SurfaceTile` cast with null handling. Fix commit: `4dda399fa08ee5ceb28d826bc048f16bebd018dd`.

**Local build/static rerun:** PASS after `4dda399fa08ee5ceb28d826bc048f16bebd018dd`; both the normal Environment DLL and the developer fixed-biome Quicktest DLL compiled, and the static validation gate completed successfully.

**Alpine fixed-biome terrain result:** map generation succeeded. Terrain share was ThinSoil **61.6%**, Gravel **24.4%**, Soil **2.6%**, RichSoil **0.0%**, Other **11.4%**. Within only the four fertility-ladder terrains, this is approximately Thin **69.6%**, Gravel **27.5%**, Soil **3.0%**, Rich **0%**, so the Alpine threshold design is producing the intended severe fertility degradation. `OtherTop` was dominated by rough rock/natural wall terrain: `DankPyon_NaturalWall_Clay_Rough` 8.5% of all cells and `Slate_Rough` 2.0%; together they account for about 92% of Alpine's Other category. The remaining listed Other entries were individually <=0.2%. No fertility-threshold change is indicated by this Alpine sample.

**Dark Forest expanded terrain result:** ThinSoil **8.9%**, Gravel **14.0%**, Soil **6.4%**, RichSoil **18.1%**, Other **52.6%**. `OtherTop` is `MossyTerrain` **23.5%**, `Marble_Rough` **19.8%**, `Sandstone_Rough` **6.8%**, `Mud` **1.2%**, and `WaterShallow` **0.8%**. The supplied MO 1.6 Dark Forest Def confirms that MO itself has a terrain patch maker with MossyTerrain at 0.00-0.32, SoilRich at 0.32-0.80, Mud at 0.80-0.93, shallow water at 0.93-1.06, and deep water above that. Environment's compatibility patch replaces only `terrainsByFertility` and intentionally leaves this MO patch maker untouched. Therefore the large RichSoil/MossyTerrain shares are expected MO biome identity, not a failure of Environment's soil thresholds. No Dark Forest threshold change is indicated by this result.

**Final Subalpine expanded result:** ThinSoil **16.9%**, Gravel **16.5%**, Soil **4.8%**, RichSoil **0.8%**, Other **61.0%**. `OtherTop` is `Limestone_Rough` **34.2%**, `Slate_Rough` **26.1%**, `PackedDirt` **0.4%**, `DankPyon_Floor_Versailles_Sandstone` **0.2%**, and `AncientTile` **0.1%**. Limestone + Slate alone account for **60.3% of all cells** and about **98.9% of the Other category**, so the large Other share is overwhelmingly natural rough rock rather than a failure of the fertility ladder.

**Result:** ENV-004 accepted and locked for Alpha with no threshold retuning. Warm/Cool retain substantial ordinary soil; Subalpine shifts strongly toward Thin+Gravel among growable fertility terrains; Alpine is overwhelmingly poor/stony; and MO Dark Forest retains its own special terrain-patch identity. The accepted values and runtime evidence are now recorded in `Docs/Design.md`. Future changes require a concrete gameplay or compatibility finding.


**Core integration audit (2026-10-04):** AMJ Core has reconciled its older duplicate terrain/Hilliness ownership text with Environment. Core now treats fertility **0.50** as the shared low-fertility integration point: Soba/Kibi/Awa/Hie/Barley remain sowable, MO Wheat at fertilityMin 0.70 does not, and the intended growth-factor order is Soba > Kibi > Awa > Hie > Barley. Core does **not** request a 0.40 Environment terrain; Soba's 0.40 fertilityMin remains a crop property/compatibility floor. No Environment implementation change is requested. Core design reconciliation: `c841b66547f51af79743a72d3db20d38b7345816`; Core regression coverage: `201bbe90aa92a0d2d55ac9d1d16c92bdf6f28c11`.

### TEST-001 — AMJ-wide runtime ERROR gate policy

**Requested by:** project-wide automated-test policy  
**Owner:** testing/tooling  
**Status:** DONE

AMJ automated tests that launch RimWorld must capture an isolated runtime log and fail on ERROR-level entries attributed to the repository-owned mod, even when scenario counts otherwise pass.

Environment's `run-tests.bat` remains a build + static-validation gate only. A separate `run-runtime-tests.bat` now launches four fixed-biome vegetation Quickstarts against an isolated test profile, writes separate runtime logs/reports, and fails on Environment-origin ERROR entries in addition to Quickstart assertion failures. The user's normal mod list and preferences are not modified.

Policy commits: `2a86387ebf1bfe5d3de3fbf09de93800cace0e74`, `7148ff5df9cdc2078e55c4233bf4b60e1e112c70`.


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


### TEST-POLICY-002 — RimTest Redux / Pickle automation-first policy

**Requested by:** author (2026-10-04 JST)  
**Owner:** Testing/tooling  
**Status:** DONE (policy documentation)

The shared project policy now prioritizes RimTest Redux / Pickle automated testing and minimizes human manual tests. Durable instructions are in `AGENTS.md` and `Docs/DevelopmentTools.md`. Reproducible logic, loaded Defs, runtime behavior and release regressions should be automated; manual testing is reserved for appearance, readability and play/interaction feel. Build/static checks remain complementary, and runtime suites retain the mandatory mod-origin ERROR gate.

This documentation update does not claim new runtime coverage or a new test PASS. Existing implementation/test history remains unchanged. Environment's runtime harness is still absent; runtime/numeric checks listed in DevelopmentTools are explicitly identified as automation targets.

**Next action:** apply this policy to subsequent feature, fix and release work; record unautomated coverage explicitly and move reproducible checks into the automated gate.

**Result / references:** AGENTS policy commit `32d067ab2057ba032ccb21bdc371f3b2a6770d74`; development workflow commit `8a6106bc4aab214d3da77f75207c76d6e5bdb816`.

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

### ENV-010 Sudajii final visual direction

**Requested by:** author (2026-10-04 JST)  
**Owner:** art/localization  
**Status:** DONE — accepted production appearance recorded in ArtDirection

The Sudajii in-game comparison was reviewed against actual AMJE terrain and the current AMJ crop-art baseline. The author selected the strongest simplified/deformed **C-direction** and then approved a lower-saturation refinement for production.

Locked production direction:
- remove the baked green ground/grass ring entirely;
- use a thicker near-black outer outline;
- simplify foliage into larger/chunkier masses with fewer internal color clusters;
- keep the approved hue relationships but reduce foliage saturation, especially the brightest yellow-green highlights;
- reduce trunk orange saturation while preserving value contrast;
- keep transparent background and no decorative ground base;
- target the same simplified visual weight as the current AMJ crop sprites rather than the earlier more detailed Sudajii pass.

Canonical art-direction update: `0815e0257deaa4b39fd41fe5231426a5215f5306`.

The standalone Sudajii sprite was then generated in the locked C-direction and approved by the author. The first GitHub binary write for that final sprite was malformed and was correctly rejected by the new static PNG-structure gate with `Final Sudajii PNG is structurally invalid`; that failed write must not be treated as a valid production asset.

The exact validated 256x256 source PNG was then transferred without manual base64 transcription and now replaces `Textures/Things/Plant/AMJ/Shii/Shii_A.png`. GitHub blob SHA is `62d795d9717a3ad4d61827bfb470cd7b3cfc7000`; corrective commit: `81a2c60af98553610c87d6606b0eba71d05b2a6a`. This file is the approved C-direction asset: transparent background, no ground/grass ring, muted olive/forest foliage, restrained trunk saturation, and the accepted simplified silhouette.

The author completed the focused WarmTemperate visual check and accepted the final Sudajii sprite in game. The rendered tree now sits naturally against the terrain, the green ground ring is gone, and the muted C-style silhouette/outline treatment is approved. Canonical art-direction acceptance: `ee61dba6614e1344a96284b669f8983cee16af99`.

The author confirmed the normal Environment runtime gate passes with the accepted Sudajii asset in place. Sudajii is therefore complete for ENV-010 visual implementation and regression coverage.

The author approved the revised Japanese beech pair after rejecting the first pass as too similar to Sudajii. The accepted pair has a visibly open, horizontally spreading crown, exposed branch structure, pale grey-beige bark, lower-saturation lighter foliage, and a matching broad leafless branching silhouette.

Implementation:
- leafy sprite: `Textures/Things/Plant/AMJ/Beech/Beech_A.png`;
- leafless sprite: `Textures/Things/Plant/AMJ/Beech_Leafless/Beech_Leafless_A.png`;
- `AMJ_Tree_Beech` now points to those AMJE-owned paths instead of Vanilla `TreeMaple` placeholders;
- integration commit: `8d0eeabf808ec4cbe37a78745675e9aa39561466`;
- static validation now locks both files, both AMJE paths, PNG structure, and absence of the former TreeMaple placeholders: `af0fb7608c3fa1d0f3f3ec29877b9ba3637a66bd`;
- canonical art-direction record: `b21471b8e1bb7de1d328c9fb5055c12993463863`.

The first binary transfer of the approved beech sprites was malformed and was correctly rejected by the AMJE PNG-structure gate (`Beech_A.png`). The Def/path integration itself was correct; only the PNG payloads needed replacement.

Corrective binary transfer:
- leafy source was re-read directly from the validated 256x256 PNG and committed without manual reconstruction; current Git blob SHA `f6866dc5faa5ad4293f8bd1e72cc43b6668265f9`;
- leafless source was transferred the same way; current Git blob SHA `c9fdc9b87e6b559ab2a2ff9788b6d015593b8406`;
- corrective commit: `b7027fc930d77a9b079ace5b5f9d663ade2f236c`.

The author reran `run-tests.bat` after the corrected binary transfer and confirmed it passes. Static PNG structure, Def path, and placeholder-regression validation are therefore clean for both Japanese beech states.

The focused check exposed two separate issues:

1. The debug runner was still opening WarmTemperate, where Japanese beech does not naturally populate. The author had to spawn a beech manually. The focused runner now targets `AMJCoolTemperateTerrainQuickstart`, where `AMJ_Tree_Beech` is the intended natural structural tree: `da47b7b9e911bb2f38136b7ba884c99d54997be9`.
2. The manually spawned leafy beech rendered as the red question mark. Binary inspection showed that the PNG's chunk boundaries were valid, so the old static structure gate passed, but the `IDAT` CRC was wrong. The leafless PNG CRCs were already valid. The leafy PNG was repaired in place by recalculating only the stored IDAT CRC; current leafy blob SHA `c0928ef133866cefb006dade3240238412190d39`, corrective commit `1900bf107143aa025ed9713c408b8847425f8712`.

The static PNG validator now checks CRCs for every PNG chunk in addition to signature/chunk boundaries, preventing this class of false PASS: `25ccb50389f3b4dd6497cd78ad84849bfe0388cd`.

A follow-up static-validation failure exposed another recurring maintenance class: the validator still hardcoded the old WarmTemperate focused-debug target even though the runner had correctly moved to CoolTemperate. This was a stale self-test, not a new texture or Quickstart defect.

The focused-debug validator has now been made target-agnostic. Instead of requiring WarmTemperate and forbidding every other biome, it now:
- accepts any of the four known AMJE terrain Quickstarts;
- requires exactly one `RIMWORLD_QUICKSTART` assignment and exactly one `-quickstart=` invocation;
- requires those two targets to match;
- continues to forbid report/verify flags so the focused window remains open for manual inspection.

This removes the need to rewrite the static validator every time the focused texture-debug biome changes. Commit: `a7f7cbb7db10170cd9cdd5803abdc0b64ee60362`.

A parser failure then exposed that commit `a7f7cbb7db10170cd9cdd5803abdc0b64ee60362` had accidentally truncated the replacement block inside `Validate-Environment.ps1`: the regex string for `RIMWORLD_QUICKSTART` was left unterminated and the remainder of the intended validation block was missing. The many later errors involving method calls and ampersands were cascading parser errors from that single unterminated string.

The complete target-agnostic texture-debug validation block has now been restored: `9d80bfdbf0c7fe68ec32c50b90473f40663fc8b5`.

Because this class of error has recurred during PowerShell edits, repository-level prevention was added as well: `.github/workflows/powershell-syntax.yml` parses every `.ps1` with PowerShell's own AST parser on pushes and pull requests that touch PowerShell files. This is intended to catch malformed scripts on GitHub before the author discovers them during a local run. CI commit: `e3cf52307b9933dad57aab1640923c927de8dc23`.

A second parser failure (`UnexpectedToken ')'` near line 1163) showed that `Validate-Environment.ps1` still contained a duplicated trailing validator block after its intended `exit 0`. The extra tail began with a stray `)` and repeated earlier texture-debug/runtime validation content. This was the remaining source of the parser failure.

The validator has now been structurally repaired so it has a single terminal `exit 0` and no duplicated tail. Prevention was strengthened at two levels:
- local: new `Scripts/Validate-PowerShellSyntax.ps1` parses every repository `.ps1` via PowerShell AST, and `run-tests.bat` executes this preflight before build/static validation;
- repository process: `AGENTS.md` now requires any future `.ps1` edit to be staged on a temporary branch and merged only after the PowerShell syntax PR check passes. Large direct string-replacement edits to `Validate-Environment.ps1` are explicitly discouraged, with edited-range/end-of-file re-read required before merge.

This repair was deliberately staged on PR #1 rather than written directly to main. The `PowerShell syntax validation` workflow completed successfully on the PR head, and the PR was then squash-merged to main. Merge commit: `5d0a6a3167275d7323da76b1d4b45c692f168d66`.

The first local run of the new syntax preflight exposed a Windows command-line quoting defect rather than a parser defect: `run-tests.bat` passed `%~dp0` as `-RepoRoot "%~dp0"`; because `%~dp0` ends with a backslash, PowerShell received a path with a stray trailing quote and `Get-ChildItem` failed with `Path contains invalid characters`.

The prevention design has been simplified so this argument cannot fail:
- `Scripts/Validate-PowerShellSyntax.ps1` no longer accepts `RepoRoot`; it derives the repository root only from its own `$PSScriptRoot`;
- `run-tests.bat` invokes the preflight with no path argument;
- the GitHub workflow now executes that exact repository preflight script instead of maintaining a second inline parser implementation.

This was staged through PR #2. The actual repository preflight completed successfully in GitHub Actions before merge. Squash merge commit: `4bc33424ed008706aea6c616b7b7956e781004a7`.

The author reran `run-tests.bat` after the self-rooting preflight fix and confirmed it passes. The PowerShell syntax guard, build, AMJE static validation, and MO static tree-texture audit are therefore clean in the current main state.

**Next action:** resume the focused Japanese beech visual check with `run-texture-debug.bat`. The runner currently targets CoolTemperate so `AMJ_Tree_Beech` should appear naturally. Inspect the leafy sprite first; if accepted, check the leafless seasonal state next.

### ENV-010 Golden Path — production texture pipeline

**Requested by:** author / Environment art  
**Owner:** Environment/art + tooling  
**Status:** DONE

The successful Sudajii/Japanese-beech lessons are now captured as a reusable production-texture Golden Path. The Environment repository already contains the concrete implementation in `Docs/GoldenPaths/TextureAssetPipeline.md` and `Scripts/Install-TextureAsset.ps1`, with exact-byte SHA-256 copy verification, PNG signature/chunk/CRC/dimension/transparency validation, Def-switch ordering, repository static gates, and correct-biome focused runtime review. `run-texture-debug.bat` now accepts a biome selector instead of requiring a code edit for each target. The implementation and CI smoke coverage were merged in `8a2307e45bf669a154a1236297695be3f2a03cc2`; the corresponding GitHub PowerShell/Golden-Path workflow run completed successfully.

The AMJ-wide Golden Path closeout rule is additionally referenced from Environment `AGENTS.md` in `6c6d6a2aae3a80c3650f30049982fe8a36f01ea7`, pointing to Core `Docs/DevelopmentGoldenPathGuidelines.md`.

**Next action:** resume Japanese beech visual validation using the repository Golden Path rather than ad-hoc binary transfer/debug-runner edits.
### ENV-010 visual-style baseline — retexture rules

**Requested by:** author / Environment art  
**Owner:** Environment/art  
**Status:** DONE

The shared visual language for AMJE tree/plant retextures is now canonical in `Docs/ArtDirection.md` under **Retexture visual-style rules**. The rule set locks strong simplification/deformation, thick dark outlines, restrained low-to-medium saturation, limited color steps, transparent/no-ground-base sprites, structural rather than recolor-only species differentiation, deciduous leafy/leafless continuity, and normal-game-zoom readability.

Category baselines are also fixed for evergreen broadleaf, deciduous broadleaf, tall conifer, and low/dwarf conifer work. Accepted Sudajii and Japanese beech art are the current reference baselines for evergreen-broadleaf and deciduous-broadleaf retextures. Future species should declare their category and structural differences from those baselines before source-art generation.

Canonical art-direction commit: `447b55e082b5e08e0c083e37f5f81dc92e458651`. Agent enforcement/reference commit: `a970a4bb12df937a7f08a9240e00b30248e63c2d`.

Production binary transfer/integration remains governed separately by `Docs/GoldenPaths/TextureAssetPipeline.md`.

**Next action:** continue Japanese beech visual validation, then apply the fixed visual rules + existing texture Golden Path to Shirabiso, Haimatsu, and subsequent Vanilla/MO tree retextures.


### ENV-010 leafy-beech payload recovery and decoded-PNG regression gate (2026-10-05 JST)

**Requested by:** continuation of Environment texture work  
**Owner:** Environment/art + tooling  
**Status:** DONE — binary recovery and focused appearance review completed; later acceptance entry is authoritative

The focused preflight found that the current leafy `Beech_A.png` blob `c0928ef133866cefb006dade3240238412190d39` passed chunk CRC validation but failed zlib decompression and normal Pillow image decoding. The earlier CRC-only correction therefore did not resolve the image payload corruption; prior static PASS must not be treated as proof that this image was decodable.

Recovery used the pre-CRC-rewrite payload's original stored IDAT CRC and its zlib Adler-32 as independent checks. Exactly one two-byte correction satisfied both checks and restored the expected 65,792 decoded scanline bytes for a 256x256, 8-bit indexed PNG. Offsets relative to the original file: 11783 (26 -> 30) and 12611 (188 -> 124). The recovered image also passed all PNG chunk CRCs, legal scanline filters, Pillow loading, and visual comparison with the accepted leafy source. The approved original pair was resolved from `libfile_d663b627e21c81919d33a20cc804444f`; no art was regenerated.

Current leafy production blob: `7face00d1128333b5fa6e1ab419d3424bc21b6dd`. SHA-256: `88274eaac76fc6fb8165c5d155a3008b0384004dbe65b38f2cce6badf6c1e072`. Sudajii and leafless beech also passed independent chunk/decoded-image checks; they were not changed.

Reusable prevention is implemented in `Scripts/PngImageData.ps1`, called by both the exact-copy installer and the static PNG gate. It validates zlib/DEFLATE, Adler-32, decoded scanline sizes/filters, and Adam7 pass sizes without a Python dependency. `Tests/Test-PngImageData.ps1` validates every production PNG and rejects a CRC-valid / zlib-corrupt fixture. The workflow now runs explicit Ubuntu PowerShell 7 and Windows PowerShell 5.1 jobs and triggers for production PNG changes. The transient `matrix.shell` workflow mistake (run 37218721890) was fixed on the PR before merging; PR run 37218817889 passed both jobs, including syntax, production PNG decoding, the corruption regression and exact-copy smoke.

**Result / references:** PR #4 squash-merged after the successful checks; implementation/PNG/Golden Path commit `1a24e11755c24c2109b7ceebd9628601c62f1f12`. Durable procedure is in `Docs/GoldenPaths/TextureAssetPipeline.md`.

**Next action:** pull main and run `run-texture-debug.bat "D:\\SteamLibrary\\steamapps\\common\\RimWorld" CoolTemperate` for leafy/leafless beech appearance and seasonal continuity. This environment did not contain RimWorld, so no new runtime/build or in-game visual PASS is claimed. After author acceptance, lock the ArtDirection visual result and continue to Shirabiso, then Haimatsu using the existing Golden Path.


### ENV-010 Japanese beech focused appearance accepted (2026-10-05 JST)

**Requested by:** author confirmation after focused texture debug  
**Owner:** Environment/art  
**Status:** DONE (reported focused appearance review)

After the repaired leafy-beech PNG and decoded-image regression gate were merged in `1a24e11755c24c2109b7ceebd9628601c62f1f12`, the author replied `問題なし` to the CoolTemperate focused-debug check. Record this as acceptance of the current in-game beech appearance. Canonical acceptance is reflected in `Docs/ArtDirection.md`.

This confirmation does not supply a new automated runtime report or separately describe forced leafless/seasonal-switch coverage. Preserve that distinction; do not fabricate a runtime suite PASS from the visual confirmation. The accepted assets and existing exact-byte/decoded-image regression guards remain unchanged.

**Next action:** prepare the Shirabiso source-art proposal using the tall-conifer category and accepted Sudajii/beech style baselines. Proposed structure: a compact upright conifer with broad, irregular layered needle masses, limited muted grey/blue-green color steps, subtle gradients, a visible short grey-brown trunk, thick dark outline, transparent background and no ground base/snow. Present the proposal before generation; this is not yet an author-approved new asset or a Def-path change. Haimatsu follows Shirabiso.


### ENV-010 new-chat generation continuity (2026-10-05 JST)

**Requested by:** author, before Shirabiso generation  
**Owner:** Environment/art + documentation  
**Status:** DONE (repository instructions / reusable generation entry)

The generation entry is now `Docs/GoldenPaths/RetextureGeneration.md`, required from AGENTS and linked from ArtDirection, the Golden Path index and TextureAssetPipeline. It carries current-source reading, actual reference viewing/attachment, precise work-stage recovery, current Core+Environment style-rule assembly, and candidate review into every new/resumed tree-art task. Visual specifications remain owned by ArtDirection; current progress remains only on main Coordination.

No image was generated and no production asset or Def was changed for this request. Shirabiso's previously presented design remains awaiting author approval. The top Active tree-art handoff is the current restart position. Existing beech/Sudajii visual results and PNG regression gates are preserved.

### ART-TEMPLATE-001 — AMJ shared pixel-exact components

**Status:** DONE (policy binding); family registration required before derivatives

Core policy/tooling is published in commit `7ce9af2ce4b3cf3bce1efda89ca1b199ab4efc36`: `Docs/GoldenPaths/FixedImageTemplates.md` and `Scripts/Art/fixed_template.py`. These Environment instructions bind to that shared policy. Reused visible components require hashed masters/masks and zero protected RGBA pixel differences. Distinct species keep species-specific structure. No asset/Def was changed; no generation or runtime test was performed. Shirabiso approval remains pending. Register a template before making a derivative that declares shared fixed parts.

### ENV-010 Shirabiso source approval and integration (2026-10-05 JST)

**Status:** DONE — source/integration and later focused appearance acceptance completed

The author approved the standalone generated Shirabiso. The approved source was resized once to 256x256 RGBA using Lanczos; the resulting PNG passed chunk CRC, zlib decompression, decoded byte count, Pillow loading, dimension/alpha and exact-copy checks before the Def path was changed. Production SHA-256: `aaef0a8db34426e028fdebe4a69efd3aadbc17ed98875e1f74c1bc2da4147d3d`. Existing Sudajii/beech files are unchanged. The source/design acceptance is canonical in ArtDirection.

This environment lacks PowerShell and RimWorld: no local PowerShell installer, build, full repository static suite or runtime PASS is claimed. GitHub CI runs the existing PowerShell PNG/payload gates and texture installer smoke. A dedicated Python regression locks the Shirabiso Def/path, production hash, dimensions and transparency. Existing Japanese/English descriptions were reviewed; no text change is introduced in this art-only integration, and the broader historical-description audit remains open.

**Next action:** pull main, run `run-tests.bat`, then `run-texture-debug.bat "D:\SteamLibrary\steamapps\common\RimWorld" Subalpine`. Review natural Shirabiso at normal zoom; record appearance acceptance separately from source approval. Then prepare Haimatsu proposal.

### ENV-010 Shirabiso focused appearance accepted (2026-10-05 JST)

**Status:** DONE (reported visual review)

After integration `e4ca2108f22aaacbead783240c59aab5759f8290` and the Subalpine focused-check instruction, the author replied `OK`. Record reported focused appearance acceptance in ArtDirection. No new runtime report or explicit full automated-suite PASS was supplied.

**Next proposal (not approved):** Haimatsu as a low, wind-shaped creeping pine shrub, clearly wider than tall, several asymmetric spreading needle masses, muted grey/blue-green palette, short partly hidden grey-brown woody branches, thick dark outlines, transparent/no ground/snow. Preserve accepted shared style; no upright miniature-tree silhouette. Present this before generation and await approval.

### ENV-010 Haimatsu source approval and integration (2026-10-05 JST)

**Status:** IN PROGRESS (source approved / installed; runtime appearance pending)

The approved generated source was resized once with Lanczos to 256x256 RGBA. Chunk CRC, zlib decompression, decoded scanline/filter checks, Pillow decoding, dimensions, transparency, exact-copy equality and Def-path regression passed. Production SHA-256: `44d22e74670bdfe081ef98c8a327700620f3d285e4a86b6dc52625c57daef551`. Canonical approval is in ArtDirection. Existing assets, plant mechanics and descriptions are unchanged. The reusable TextureAssetPipeline remains the production procedure; a Haimatsu regression joins the existing CI gates.

Execution is in a cloud Linux workspace despite the author using the desktop app. PowerShell and RimWorld are unavailable here; no local build/runtime/visual PASS is claimed.

**Next action:** verify CI, then run `run-tests.bat` and `run-texture-debug.bat "D:\SteamLibrary\steamapps\common\RimWorld" Alpine` in a Windows local execution task. Author review at normal zoom is still required.

### POLICY-001 — AMJ各Modのリテクスチャ所有方針

**Requested by:** author (2026-10-05 JST)  
**Owner:** Environment art / cross-mod ownership  
**Status:** DONE — durable design updated

AMJ共通方針に合わせ、Environmentは自分の景観責務に属するVanilla / MO等の前提資産まで画風統一を所有することを正式化した。

- ENV-010のVanilla/MO樹木リテクスチャはEnvironment本体の正式責務であり、別Retexture Modへ移管しない。
- 同一前提資産を他AMJ Modと競合上書きしない。1資産1所有Modを原則とする。
- Alpha中のplaceholder / final-art deferは作業順の都合であり、所有責務の移管を意味しない。
- Japan OnlyはEnvironmentのリテクスチャを担当しない。
- 横断資産で自然なownerを決められない場合だけ、将来の共通Retexture Modを再検討する。

Durable design source: `Docs/Design.md §11.5 AMJ共通リテクスチャ方針 — Environment所有範囲`, commit `5c836d3907577197396a465dbd88a982f601890b`.

**Next action:** ENV-010は既存Golden Path / ArtDirectionをそのまま使い、Environment-owned Vanilla/MO tree retexturesを継続する。

### TEST-002 — Core + Environment gameplay-contract automation

**Requested by:** author (2026-10-05 JST)  
**Owner:** Environment runtime integration / Core agriculture contract  
**Status:** DONE — runtime/integration gate completed; no release rerun required unless later changes invalidate it

Core + Environmentの独自ゲームプレイ差を、固定バイオームQuickstart上で機械評価する統合ゲートを追加した。

自動評価:
- 4バイオーム実マップのThin Soil / Gravel / Soil / Rich Soilセル数;
- Core Stage A作物のロード確認;
- Soba / BarleyがThin Soilを利用でき、Wheatは利用できないこと;
- 実マップ土壌分布でSoba/Kibi/Awa/Hie/Barleyの平均肥沃度成長倍率が分化すること;
- Coreの成長最低温度差が維持されること;
- Environment年間気候診断の <8C / <0C 時間がWarmLowland→Temperate→Cool→Highlandで寒冷化すること;
- Environment / Core由来のruntime ERRORゼロ。

`run-runtime-tests.bat` はローカルCoreを検出した場合、MO依存を含む隔離Core+Environmentプロファイルを自動生成し4固定バイオームを追加実行する。

PowerShell syntax workflowは関連スクリプト変更までGreen。Quicktest C#コンパイルと実RimWorld統合実行はこのクラウド環境では未実行。

設計精査中、Environmentの旧気候表に残っていたAwa -4C / Hie -4Cを、Core正本のAwa -3C / Hie -2Cへ同期した。

Durable design source: `Docs/Design.md §8.3 Core + Environment gameplay-contract runtime gate`, commits `14b09c8c05e4ee060446f4ab7d42db9ba94fca2f`, `6a3420b71b6a7fc60b16cbdd4bed73f759a33d2e`.

**Result:** author confirmed the Environment runtime/integration test work is complete. Preserve the existing automated gate for future regressions; do not rerun it solely because publication work resumed.

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

### WORK-001 — Retexture implementation precedent audit

**Requested by:** author (2026-10-05 JST)  
**Owner:** ENV-010 art compatibility / implementation  
**Status:** DONE — audit reflected in Environment design and Golden Path

The earlier handoff bundled runtime testing that the author has confirmed is already finished. No runtime rerun belongs to this item.

The cross-project prior-art audit covered VTE / VTE Variations, ReGrowth 2, current MO 1.6.2.2, Clean Textures, Van's retextures, Misc. Training Medieval Retexture, Primitive/Adaptive Primitive Storage, Better Looking Plants, and another current 1.6 plant/mushroom replacement pack.

Environment decision:
- Vanilla/MO tree retextures point to AMJE-owned unique texPaths through explicit patches;
- optional MO path replacements remain guarded compatibility work;
- a tree Def is complete only after all loaded visible graphic states are audited, not merely its mature/leafless baseline;
- same-path broad packs normally cease to conflict once AMJE points the Def to a unique path;
- ReGrowth/VTE-style explicit-path competitors require final-loaded-path ownership checks where they touch the same state;
- no C# or variation framework for ordinary static tree retextures.

Durable sources:
- shared Core `Docs/RetextureImplementationGuidelines.md` — `e61a6379886d0f8a5ada8edb75a94943572f23ea`;
- Environment `Docs/Design.md` — `132544092a7552d86f85547c627c757e0412249e`;
- Environment `Docs/ArtDirection.md` — `b42781ffbab01a70cce52349d858647c3aa18fd7`;
- Environment texture Golden Path — `1656615b6cb47176d8a0cf3dd4fb07e54739e845`.

**Next action:** continue ENV-010 art in its existing priority order. Before integrating each first Vanilla/MO target, enumerate the target's current loaded graphic states and create the matching AMJE target/state regression entries.

### DEV-TOOLS-001 — DevKit / Rim Control を開発専用補助ツールとして採用

**Requested by:** author (2026-10-06 JST)  
**Owner:** Testing/tooling  
**Status:** DONE (policy documentation)

AMJ共通の対話型開発補助ツールとして、DevKit — Better Dev Mode Menu (Workshop `3814373104`) と Rim Control (Workshop `3774299554`) を採用した。

DevKitはDef検索・スポーン・Debug Action等を用いた目視確認の準備短縮、Rim Controlはゲーム中の数値・visual・placement等の一時変更によるプロトタイピングに使用する。どちらも自動テストの代替や出荷依存にはしない。

Rim Controlで得た採用値はXML / C# / Def / 正式設計書へ正本化し、上書きを無効にしてから静的検証・RimTest Redux・Pickle・runtime gateで再検証する。正式テスト、自動テスト、リリースゲート、通常プロファイルの正式確認では両ツールを無効化する。

**Result / references:** durable rule in `Docs/DevelopmentTools.md`. Documentation-only change; no new runtime PASS is claimed.


### ENV-010 — Leafless beech snow rejection audit and repair (2026-10-06 JST)

**Owner:** Environment/art
**Status:** DONE — revision 5 accepted in native appearance and PR #5 merged

Author rejected `Art/Templates/Beech_Leafless-Snow-v1/exact-composite.png` because snow read as thin branch-following lines. The immediate PR repair was deterministic per-edge capsule painting (`853d7f5` -> `62434d2`), which created many tiny pellets and weak rims; area-only regression and output-derived mask did not enforce the visual spec. An unspecified earlier chat-generated image/prompt was not recoverable and is not attributed to an unverified cause.

PR candidate now uses twelve supported asymmetric outlined caps, accepted Haimatsu snow color planes, unchanged leafless master and a predeclared unapproved `v2-review` mask contract. No ImageGen call or native game test was performed. Core protected RGBA gate = 0 differences; PNG CRC/decode, exact overlay/composite, mass/outline/support checks and rejection fixtures pass. Accepted base masters, leafy snow and other species snow are byte-identical. State ledger has one pending snow row; strict completion still fails intentionally. No source approval, merge, release or installed-game change is claimed.

Durable spec/audit/regression changes are on draft PR #5, pushed head `4a1122c27bec8fb768a163c34c41f3950c63def9`; repair commit `b075162` and merge of the concurrent work-push/PR CI distinction are retained. See PR `Docs/PlantSnowOverlayPlan.md`, current-main-aligned `RetextureGeneration.md`, `ArtDirection.md` rejection supersession, and `Docs/ValidationEvidence/BeechLeaflessSnowRepairAudit.md`.

**Closeout:** superseded by the later revision-5 source/native acceptance below. No further current-plant snow review remains before Beta publication. Steam publication remains the author's manual step.

### ENV-010 — Revision 2 rejected; branch-contact revision 3 pending

Author feedback: `あまりに厳しい生成結果 / 枝が考慮されてない`. Revision 2 is rejected and must not serve as a style/production reference. Its independent horizontal caps and interior wood-proximity test did not establish actual branch contact.

PR #5 now contains `c119755c51eb5d73c1ae8ea9822eb22d6477012a`: eight snow masses whose bottom boundary traces actual upper branch pixels inside narrow declared corridors. Trunk/branch master is unchanged; snow thickness tapers along each branch. All eight bottom boundaries have full contact at zero/one-pixel AA distance; a partial-support regression now fails. Core protected RGBA difference=0; PNG and ledger checks pass. This is geometric/static evidence only. Template is `v3-review`, no filled exemplar; leafless snow remains pending and no game/native visual acceptance is claimed. Durable correction is in PR snow plan, ArtDirection and repair audit.

### ENV-010 — Snow looked behind branches; foreground revision 4 pending

Author: `雪が下のレイヤになってるように見える`. Actual composite order was master -> snow, but revision 3 stopped the cap at the branch silhouette, leaving original wood contour/face visually in front. PR #5 now has `501fc89d06aea530f7bf426802ce9cdd6b5eeaa3`: the same eight corridors and same mask/master with an opaque snow lip covering the original upper wood face, plus blue-grey front thickness. Upper branch outline/adjacent face occlusion, exact composition, reversed-order negative fixture, support/massing, PNG and ledger checks pass. No visual acceptance or game/native test is claimed; template `v4-review` and leafless snow remain REVIEW/pending. Earlier revision-3 acceptance-like geometry statements do not approve this image.

### ENV-010 — More whole-tree snow requested; revision 5 pending

Author requested `枝だから雪がつもりにくいのはそうだが、もうすこし全体的に積もらせたい`. PR #5 head `e37b4121c2511a4532ac356ccb3611b6ae3f27b9` adds seven upper/outer/lower ledges to the existing eight: fifteen supporting corridors / fourteen connected masses, solid snow 2108 -> 3628 pixels (~1.72x). Foreground occlusion, tapered branch-aligned thickness and exposed trunk/twig structure are retained. New allowed corridors are declared before painting under unapproved `v5-review`; accepted masters and other variants are unchanged. Core protected RGBA=0, support/occlusion/mass/layer-order, PNG and state ledger gates pass. No ImageGen, native/game test or visual acceptance is claimed. Leafless snow remains REVIEW/pending and PR remains draft.

### ENV-010 — Revision 5 adopted as compromise; source frozen

Author: `これで妥協する` (2026-10-06 JST), referring to the presented revision-5 PNG. Record source-art acceptance as compromise. PR #5 commit `9a7af9295724012292d542ef892392f7e2535d0e` records the statement/date/source hash, `v5` / `source_approved`, and prevents the builder from changing accepted source bytes. Images, master and mask remain identical to the adopted revision. Source SHA-256: `24a10f6a45ed200170a5dabf49cabd2bacecfa3bfaaa1679300e64204b933126`.

Native game appearance of these exact bytes was not shown in this chat, so it remains separately pending in the coverage ledger. Do not treat this as a new runtime PASS or merge/release authorization. Durable evidence is PR `Docs/ValidationEvidence/BeechLeaflessSnowSourceAcceptance.md` and ArtDirection. Remaining action is native appearance review when continuing release validation; do not regenerate this source or ask again to approve the same presented image.

### ENV-010 — Leafless beech snow final acceptance and PR #5 merge

**Requested by:** author (2026-10-06 JST)  
**Owner:** Environment/art + release  
**Status:** DONE

After source compromise acceptance (`これで妥協する`), the author completed the separate native appearance review with `ブナOKなのでPRマージして` and explicitly authorized merge. Revision 5 remains frozen: exact composite SHA-256 `24a10f6a45ed200170a5dabf49cabd2bacecfa3bfaaa1679300e64204b933126`; installed leafless-snow overlay SHA-256 `3cc4da672499e8bb7f96847623793050ab136b766cd91713a7921eda66dc496b`; accepted leafless master SHA-256 `24f8664bdedd3ebd0dee58fe627439b3784b5ecc599aaf49d750832f315be112`.

PR #5 was synchronized with current main without reverting the newer README / Workshop / historical-description work. Before merge, the PR checks were all green: Plant visual coverage ledger, Art rule structure, and PowerShell syntax validation. The visual fingerprint gate was also corrected so label/description-only changes do not invalidate prior visual approvals; visual Def fields, state paths, size, and owned PNG hashes remain guarded.

PR #5 was squash-merged to main as `d38e28544693f8ac2d095143b64a37de68ab887a`. This completes current AMJE structural-plant art for the first public Beta. Existing Vanilla / Medieval Overhaul tree retextures remain explicitly post-publication work.


### CI-NOTIFICATION-001 — Repeated plant-coverage failure notifications

**Requested by:** author (2026-10-06 JST)
**Owner:** CI/tooling
**Status:** DONE

The earlier label/description fingerprint defect is fixed in merged PR #5.
Repeated notifications were amplified by feature-branch push + PR duplicate
execution, broad Docs triggers and expected incomplete draft-review failures.
Commit f4c86f890161296b36896349593b3ceeb150dc71 restricts pushes to main, scopes
inputs to actual ledger/art/evidence/Def/test dependencies, cancels superseded
runs per PR/ref and uses consistency mode on draft PRs. Ready PRs retain the
strict completion gate, including an explicit ready_for_review event. Existing
production texture/snow/error guards are unchanged. No account-level email
notification setting was changed; real validation failures still fail CI.

YAML/routing assertions passed locally. The actual main workflow run
https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/actions/runs/37469313074
passed inventory/selftests and all production texture/snow validators.
Reusable routing and troubleshooting procedure is in
Docs/GoldenPaths/PlantVisualCoverage.md. Coordination-only updates are excluded
from this workflow's triggers.

### POLICY-MOD-NAME-001 — Mod名のコロン禁止（2026-10-06 JST）

**Requested by:** author  
**Owner:** AMJ shared release / documentation  
**Status:** DONE

AMJ Core・Environment・CCTOおよび今後の関連Modの名称では、半角 `:`・全角 `：` を禁止し、必要な区切りには ` - ` を使用する。About.xmlのname、Workshopタイトル、README等の正式名称に適用する。表示名の修正ではpackageId・既存Workshop IDを維持する。

YADAがMod表示名を一時ディレクトリ名に使用し、Windowsで半角コロンによりアップロード前処理が停止した件の再発防止。共通正本はCore `Docs/ModDescriptionGuidelines.md`（commit `695bff2a32a57cdf817b13b255587e749399b417`）。このリポジトリのAGENTSにも規則を反映済み（commit `fb40c1695203650851911fdd0169b6a3d43f9f05`）。

確認時点でCore・Environment・CCTOのAbout.xmlのnameはいずれもコロンなし。今回の変更は文書・運用規則のみで、ゲーム実行時テストやSteam公開の成功を示すものではない。

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


### ENV-PLANT-AUDIT-001 — Vanilla自然植物の残存監査（2026-10-08 JST）

**Requested by:** author  
**Owner:** Environment / vegetation  
**Status:** DONE — Vanilla forest/wetland retention/removal pass complete; later vegetation stages handed off

Author corrected the previous assumption that existing Vanilla vegetation should
remain by default. AMJE now requires a positive ancient/medieval-Japan reason
for each reused Vanilla plant. Existing presence is not a retention reason.

The earlier broad draft PR #7 was closed without merge. The author clarified
that the audit/removal work itself does not need to proceed one species at a
time. Multiple clearly justified removals may be researched and implemented in
the same workstream; keep removal commits separated into reviewable logical
units so history remains easy to inspect and revert.

**Phase 1 DONE:** PR #8 / squash merge `565d71f16ce194af0398e1f431246cbe58a04b22`
- removes only `Plant_TreePoplar` from `AMJ_WarmTemperateForest`;
- leaves every other candidate unchanged;
- adds a Quickstart regression assertion that Poplar does not naturally
  generate in the target AMJE biome;
- records the decision in `Docs/VanillaPlantRetentionAudit-ja.md`.

**Phases 2–4 DONE:** PR #11 / squash merge `9f80a117ef21b04762b92a77997c7ea0ebd329b9`
- warm-temperate Vanilla Oak removed; Sudajii commonality 2.00→2.55, restoring pre-Poplar woody commonality 3.30 and total wild-plant commonality 13.42;
- subalpine generic Pine removed; Shirabiso 2.60→3.50, preserving woody commonality 4.00 and total 16.14;
- alpine generic Pine/Birch/Dandelion/Astragalus removed; Haimatsu/Grass/Moss rebalanced so total commonality remains 7.31 and woody commonality 1.34 while ordinary tall trees become zero above treeline;
- retained Vanilla vegetation is now explicitly classified in `Docs/VanillaPlantRetentionAudit-ja.md`: generic groundcover/Berry, Maple/Bamboo, cool-temperate Oak/Pine/Birch, and subalpine Birch remain with defined proxy roles;
- Quickstarts now guard every completed exclusion.

PR #11 regression gates passed after synchronizing the art-rule test with the already-completed Core→Grains repository rename. GitHub preflight/error-hygiene rules are now recorded in `AGENTS.md`: do deterministic validation before remote writes and use Actions as regression gates rather than exploratory debugging.

Existing PR CI passed before merge: Workshop payload filtering and Plant visual coverage ledger. The new warm-temperate runtime Quickstart exclusion assertion is committed for the next runtime matrix, but no fresh RimWorld runtime execution is claimed for this one-line distribution removal.

Later candidates may be audited and prepared together. Do not artificially
serialize the work by species. Keep each removal or tightly coupled replacement
in a separate reviewable commit where practical; a PR may contain multiple such
commits.

The author later fixed the broader vegetation roadmap in PR #16 / squash merge
`b7006ad91f49c1a0d2dca2aaea658f384922f4f0`:
1) remove unsuitable inherited vegetation;
2) audit/rewrite retained Vanilla descriptions first, then retexture;
3) add missing ancient-to-medieval Japanese vegetation, including traditional
medicinal plants such as yomogi;
4) remove `Plant_HealrootWild` only at the final cleanup stage after the
replacement vegetation and gathering balance are established.

Yomogi addition and Healroot removal are no longer required to be atomic in one
change. Cultivated `Plant_Healroot` remains outside this natural-vegetation
roadmap.

Public description sync is complete in squash merge
`75b74a6d3624701df2e427549bd6e7f7b0c1432d`: README, Japanese/English
Workshop copy, 2game copy, About.xml and their presentation policies now state
that the Vanilla-vegetation audit is already active, unsuitable plants are
removed/replaced, and warm-temperate Vanilla Poplar is the first completed
removal. Workshop BBCode is balanced and remains under the 8,000-byte limit in
both LF and CRLF forms. Actual Steam / 2game site publication is not claimed.



Public descriptions were synchronized again after Phases 2–4 in squash merge
`734db85070c55d5e88f04a4346648549138ef247`. README, Workshop JA/EN,
2game and About now record the broader removals, the commonality redistribution
that prevents unintended forest thinning, and Wild Healroot→yomogi as the main
remaining Vanilla-vegetation replacement. Workshop preflight passed before PR:
JA 7,759 bytes LF / 7,870 CRLF; EN 7,670 LF / 7,781 CRLF; BBCode balanced.
Actual Steam / 2game publication is still author-manual and is not claimed here.



**Phase 5 DONE:** PR #15 / squash merge `489fb9d6e933e07d4fd2108bad0564879409f757`
- audited retained Vanilla `TemperateSwamp` / `ColdBog` vegetation, which remained reachable because AMJE intentionally leaves swampy tiles outside its four baseline bands;
- TemperateSwamp removes Chokevine/Cypress, redistributing to Brambles/Willow while preserving total commonality 7.30 and woody commonality 3.00;
- ColdBog removes Chokevine/Cypress/Astragalus, redistributing to TallGrass/Moss/Birch while preserving total commonality 8.22 and woody commonality 1.80;
- runtime Quickstarts and exact loaded-commonality assertions were added for both wetlands; the normal vegetation runner now includes them;
- GitHub static gates all passed: PowerShell syntax, Workshop payload, plant visual coverage and regional tree-sowing contract.
- A fresh RimWorld runtime execution of the two new wetland Quickstarts has not been run from this chat/tool environment and is not claimed.

The plant-pool retention/removal pass is complete, but the broader vegetation-roadmap Step 1 is **not closed yet**. Whole-Biome audit PR #18 / squash merge `34fce92e5b6bf6da24e38355e475623ada664d66` found that retained Vanilla `TemperateSwamp` / `ColdBog` still reintroduce non-Japan wildlife and carry unreviewed Vanilla disease/weather/description bundles. Wild Healroot remains intentionally temporary. Step 1 completion now requires the wetland bundle corrections and their runtime/static gate before description/retexture work begins.

**2026-10-08 structural-plant static-gate repair:** PR #20 / squash merge `1cc69585dd08cff71786548848c753a7ca01c99a` corrected `Scripts/Validate-Environment.ps1` after the accepted Phase 2–4 rebalances. The old Shii 2.0 / Shirabiso 2.6 / Haimatsu 1.3 raw markers incorrectly rejected current XML values 2.55 / 3.5 / 1.34. The validator now checks all four structural plants in their own BiomeDefs with approved commonalities, and CI runs `Tests/test_structural_plant_commonality.py` to catch future drift. PR checks passed (PowerShell syntax, Workshop payload, Plant visual coverage). No production Def/XML balance or new RimWorld runtime test result changed; wetland Step 1 completion remains pending.

### COORD-PRIORITY-001 — Supersede stale Beta-first vegetation handoff (2026-10-08 JST)

**Requested by:** author
**Owner:** Environment coordination
**Status:** DONE — summary aligned with latest owner audit and runtime handoff

The release/tree-art summaries now use `ENV-PLANT-AUDIT-001` and `ENV-TREE-SOWING-001` rather than the older Beta-publication-before-audit sequence. Retention/removal is complete; fresh distribution/wetland/tree-sowing runtime evidence remains pending. Wild Healroot is intentionally temporary, not a pre-upload blocker. Earlier entries are retained as history.

Project `Docs/ImplementationPriorities.md` assigns Environment Reconstruction P0 and recommends current vegetation/runtime closeout first, before Living Norms v0.1 and Grains closeouts. No production change, new runtime PASS or live Workshop publication is claimed. Golden Path N/A for this status-only reconciliation; existing vegetation testing and design sources remain authoritative.


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

### MERGE-LATEST-20261008 — preserve latest wetland policy

**Requested by:** author (2026-10-08 JST)
**Status:** DONE — merge resolution follows latest main

Author selected the latest GitHub policy over the older local all-land replacement.
Production generation, biome textures, public descriptions and regression tests
match d2b4d261c16481c748544661499b287b039a50d3. The earlier local terrestrial
replacement/Alpine-cap changes are superseded, not current runtime evidence.
Historical downloaded-Workshop validation remains recorded separately; it does
not validate this latest development tree or close its pending wetland gate.

Merge validation: build and Quickstarts compile PASS; PowerShell syntax, payload
filter/regressions, tree-sowing contract, unified entrypoint, art routing and
visual ledger checks PASS (Python UTF-8 mode on Windows). The latest main static
suite still stops at its stale <AMJ_Tree_Shii>2.0</AMJ_Tree_Shii> marker while the
current audited Def uses 2.55. Validator and Def are unchanged from the selected
main; full static/runtime PASS is not claimed by this merge.


### ENV-TREE-SOWING-RUNTIME-002 — Alpine natural woody versus sowable set (2026-10-08 JST)

**Owner:** Environment / regional tree-sowing verification  
**Status:** IN PROGRESS — assertion corrected; full matrix rerun required

The author's isolated vegetation runtime report passed warm/cool/subalpine and failed Alpine 1/70 assertions. It compared the Alpine `map.Biome.wildPlants` tree-like set (including `AMJ_Shrub_Haimatsu`, since RimWorld 1.6 reports `plant.IsTree`) against the approved empty growing-zone sowing set. Native sowing menu and Haimatsu-non-sowable assertions passed, with zero captured runtime ERROR. Vegetation XML and production gameplay are unchanged.

Correction: the runtime/static tests separately expect Alpine natural woody `{AMJ_Shrub_Haimatsu}` and sowable trees `{}`; other three bands continue to use equal sets. Canonical rule and procedure: `Docs/Design.md` and `Docs/GoldenPaths/PlantSowingTests.md`. Rerun the full isolated vegetation matrix for wetland/river/coast coverage; no new full-runtime PASS or publication claimed.
