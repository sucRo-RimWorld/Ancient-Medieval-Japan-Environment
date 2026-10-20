# AMJ Environment Coordination

### ENV-020-EXISTING-WORKSHOP-UPDATE-20261020 — author accepts public release visibility; actual upload HOLD

**Supersedes the earlier private-only Workshop blocker:** User explicitly accepts that updating the **existing** Workshop item ID `3814638060` may make Environment 0.2.0 changes publicly visible. Do **not** create a second private staging item or change the existing item's visibility; retain the original item and identity. In unmerged Environment [release PR #217](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/pull/217), source head `5686aba364e73125a8d9a8802987a2092a15384b`, `Publishing/SteamWorkshop.json` is now `visibility_policy=preserve_public`, and `Tools/SteamPublisher/readiness.py` + release tests match. Owner [Actions #39366776095](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/actions/runs/39366776095) PASS. Project's central [PR #820](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/820) merged main `4da4e79b8ffca2e613304e176de21ccdb36cdc49`, registry pins exact same owner source and `preserve_public`, with author comment gate **`/approve-steam-existing-update`** rather than the misleading prior `/approve-steam-private`. The SteamCMD existing-update VDF **omits visibility** and preserves current setting; this is not a Steam visibility change request. Project offline 60-file subscriber ZIP [Actions #39366927791](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39366927791) PASS.

**WSL state safely aligned, not authenticated:** One-time Project [Issue #821](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/issues/821), now closed, dispatched actual [Actions #39367044012](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39367044012) SUCCESS: isolated Environment 0700/0600 state pinned correct Workshop ID and safely migrated its **unverified** previous `private_only` policy to `preserve_public`, leaving `identity_verified=false`; CCTO/LWM unaffected, no SteamCMD/login/upload/Steamworks operations. Central `AMJ_STEAMCMD_BIN`, `AMJ_STEAM_ACCOUNT`, `AMJ_STEAM_STATE_DIR`, `AMJ_CENTRAL_PUBLISHER_READY` still missing. The exact publisher identity and Windows Steamworks host/session remain unverified. Project registry `publication_enabled=false`, Environment PR #217 **not merged**. User's current approval concerns public-visibility *policy*, not actual upload or auth bypass. **Steam 0.2.0 HOLD** pending host proof and separate release/publish authorization.

### ENV-CENTRAL-WSL-PUBLISHER-STATE-PASS-20261020 — private host sentinel PASS; existing Steam item privacy HOLD

**Latest verified Project host evidence supersedes old QUEUED/absent-state notes:** Project [PR #815](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/815) main `436df9c034af0b2dbb889e9d99837f44b696ebe6` added safe host-only state preparation; exact author [private Project Issue #816](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/issues/816) (now closed) triggered real [Project Actions #39364648488](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39364648488) **SUCCESS** on the restricted existing WSL Linux SteamCMD publisher runner. Owner's new Project-central `~/.private/amj-central-publisher/states/Ancient-Medieval-Japan-Environment.json` exists (directories 0700, state file 0600), original Workshop ID 3814638060 matched, but `identity_verified=false` intentionally: no authenticated Steam owner/visibility verification occurred. CCTO and LWM host states/service/credentials unchanged; both had SteamCMD/account **variables present** (values not read or displayed). **All** Environment `AMJ_STEAMCMD_BIN`, `AMJ_STEAM_ACCOUNT`, `AMJ_STEAM_STATE_DIR`, and withheld `AMJ_CENTRAL_PUBLISHER_READY` variables were **MISSING** on the runner; Steam upload, Windows tags and publication approval jobs skipped. No extra Linux runner test required for already observed PASS.

**Visibility safety blocker:** Existing item 3814638060 is indexed publicly (<https://rimworld.2game.info/detail.php?id=3814638060>), but third-party indexing is not authenticated proof of *current* Steam Workshop state. Private-only central publisher **omits** VDF `visibility` for an *existing* update, therefore cannot implicitly privatize a preexisting public item. Project [PR #818](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/818), merged main `7b7d57d5030461add66792f03005921bd1a50f83`, requires **both** host `identity_verified=true` and `visibility_verified="private"` before any existing-item `private_only` upload, rejecting missing or known public visibility. Do not fabricate either flag or set a credential or convert a public Workshop item without explicit author choice and independent real ownership/visibility proof. The Project registry `publication_enabled=false` and host `AMJ_CENTRAL_PUBLISHER_READY` missing remain intact. Current Environment 0.2.0 [PR #217](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/pull/217) unmerged. **No actual Steam mutation or downloaded subscriber check; publication HOLD**. Any separate private staging item, change to existing public item visibility, or public release requires a distinct author decision.

### ENV-CENTRAL-LINUX-PUBLISHER-ROUTE-20261020 — GitHub permissions PROVEN; host secrets/state absent, Steam HOLD

**Latest external Project evidence:** The author completed selected-workflow/group access for private Project in org `amj-steam-private` (ID 4); private Project [PR #812](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/812) main `8737ba870bbc807dc9ccb00da10ac2c4873a82ce` added a strictly author-only, one-time, read-only runner reachability path to the **same** allowed `steam-central.yml@refs/heads/main`. Genuine [Project Actions #39316208231](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39316208231) ran on a Linux self-hosted `steamcmd-private-publisher` runner, **SUCCESS** with `CENTRAL_RUNNER_ROUTE_PASS`; preflight, author-approval, Windows, Steam upload and tag jobs **skipped**. The temporary author Issue #813 was closed. Read-only log reported exactly **`AMJ_STEAMCMD_BIN`, `AMJ_STEAM_ACCOUNT`, `AMJ_STEAM_STATE_DIR`, `AMJ_CENTRAL_PUBLISHER_READY` all MISSING**; no values, paths, credentials or Steam commands printed/executed. Project `main:Docs/Coordination.md` tracks formal evidence in [PR #814](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/814), main `e4a583d9dd48af72bd6a64bbf2d7ab439efc04cc`.

**Result / next action:** Do not ask the author to redo runner-group access: the private Project → group4 Linux publisher route **is verified**. Separately provision the first three `AMJ_*` host-only variables/account/state after validating Environment's genuine existing Steam Workshop item `3814638060`; `AMJ_CENTRAL_PUBLISHER_READY` intentionally stays absent until security/state approvals are satisfied. Independent Windows `steamworks-tag-publisher` runner/session **not verified**. Central registry `mods.environment.publication_enabled=false`, Environment 0.2.0 [PR #217](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/pull/217) unmerged, no real update or downloaded subscriber acceptance: **Steam HOLD**. No repeated RimWorld 1.6 or Linux runner check needed on unchanged implementation.

### ENV-CENTRAL-STEAM-APPROVAL-20261020 — author GitHub comment gate integrated; Steam HOLD

**Current latest central publishing gate:** GitHub Free/Pro/Team cannot rely on `Required reviewers` for a **private** Project repository. The private [Project central publisher PR #810](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/810) was merged as `f3a68ee34937d9b79e014387c3381384a70fc4be` and now runs privileged preflight **only after** two separately recorded author GitHub actions: create the author issue `[publish:amj-central:environment]` with exact current Project main SHA and later comment **`/approve-steam-private`** on that same Issue. A newly opened Issue alone never triggers it; the issue author/comment author IDs, timestamp, exact source and current main are checked and approval revalidated after nonprivileged preflight. Formal Project policy: `Docs/WorkshopPackaging.md`, `Docs/GoldenPaths/GitHubWorkshopPublishing.md`. Environment release candidate [PR #217](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/pull/217) separately adopted that method in its formal `Docs/GoldenPaths/WorkshopPublication.md`, `Docs/ReleaseReadiness.md`; current candidate HEAD **`da90c53075d84a5fd23c1c3a4725cf21f39cf130`**, unmerged. Project central registry was updated to that exact owner candidate in [Project PR #811](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/811), merged Project main `5f6da9fece39d041e3f2bccdac970a7490cd5599`; genuine compiled 1.6 DLL source, 60-file subscriber inventory and publication package are unchanged; Project offline full source CI [#39311820817](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39311820817) **PASS**.

**Still not activated:** `Publishing/ModRegistry.json` explicitly keeps `publication_enabled=false`, Project-only privileged jobs also require host-only `AMJ_CENTRAL_PUBLISHER_READY=YES`; no release Issue/comment created, no Steam update or subscriber installation. Org restricted runner group `amj-steam-private` (ID4) needs **private Project** repository access and one exact selected Project workflow `sucRo-RimWorld/Ancient-Medieval-Japan-Project/.github/workflows/steam-central.yml@refs/heads/main`, alongside unmigrated CCTO/LWM allowlisted workflows. Runner/admin settings and separately authenticated Linux/Windows hosts cannot be inferred from CI. Preserve existing Environment game/manual PASS evidence and original native raw FAILs. Older messages calling a private Project `Required reviewers` gate mandatory are superseded; **Environment 0.2.0 Steam HOLD**.

### ENV-CENTRAL-PUBLISHER-MIGRATION-20261020 — private Project workflow source integrated; release PR HOLD

**Author decision (2026-10-20):** Stop adding new owner public-repository workflows to GitHub org runner-group Workflow access. Instead use **one** private `sucRo-RimWorld/Ancient-Medieval-Japan-Project` main-pinned workflow `.github/workflows/steam-central.yml@refs/heads/main`. Project canonical source and shared procedure are merged in [Project PR #807](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/807) commit `b0f48566bcb6d6fdb8caee18b24d9705a6ac918f`; exact Environment owner source pin and **genuine 60-file offline integration PASS** in [Project PR #808](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/808) main `d20c9b23ce7e63d235bdc8dfc90004186d4c4dbb`, CI [#39308072324](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39308072324). **Project registry Environment source pin** `3a609c731e726993e631edd831edc1d9e5897567` (unmerged Environment 0.2.0 [PR #217](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/pull/217)); original 1.6 DLL `b2ddbf7888322ba9b207c20296dba31175b846c05492902b900ec225da9fcffe` SHA-256, original tested runtime files and 0.2.0 release copy remain intact. Source PR #217 **retired** owner-specific `.github/workflows/steam-approved.yml` and `.github/workflows/steam-readiness.yml`, adjusted its offline test and formal publication source. These deletions are staged **only in PR #217**; current Environment main still contains its old historical upload workflow, and it must **not** be dispatched as a production route. Main itself remains current published 0.1.1 until separately authorized release promotion.

**New security contract:** Only the private Project repo + exact Project central workflow should be permitted in group `amj-steam-private` (plus the old CCTO/LWM workflows only until their separate migrations). A new Mod requires Project registry review, **not** a new group Workflow access entry. Project protected `steam-workshop-private` actual required reviewer and author approval, separate Linux/Windows privileged host registration and host-only per-Mod Steam identities/state, existing item `3814638060` ownership/private visibility and actual downloaded-subscriber verification are **not yet proven**. Central workflow additionally fails closed unless host-only `AMJ_CENTRAL_PUBLISHER_READY=YES` is independently provisioned. No GitHub author issue was created; no live SteamCMD upload, Steamworks tags, item visibility change or 0.2.0 PR merge occurred. 0.2.0 **HOLD**. Existing original native-run FAIL and precise known MO/SYR FMOD exception remain historically recorded; author's RimWorld 1.6 Thin Soil/Nice Plant Menu PASS is unchanged. Do not rerun native acceptance for this process migration.

### ENV-020-PUBLISHER-NATIVE-TRANSPORT-20261020 — offline PASS in PR #217; real publisher HOLD

**Staged, not live/main:** Unmerged [release PR #217](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/pull/217) replaces private Project artifact token transport with source-audited, immutable Git commit `5dac3140230402b3b2aa96cbc117b98a23745dde` containing the exact 1.6 production DLL (blob `b8e3766d5ba1dde28e1bbb9e746ffcb7e7d390a8`; SHA-256 `b2ddbf7888322ba9b207c20296dba31175b846c05492902b900ec225da9fcffe`). Original Project run [#39247372678](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39247372678) genuinely built the DLL, original full artifact/manifest/provenance verified before owner [test-branch commit Actions #39258628883](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/actions/runs/39258628883) staged precisely those bytes. Owner preflight independently verifies exact commit + blob + DLL hash + source commit, the complete 58 immutable runtime source files, release-only metadata differences, original Project run/archive digest and preview. Project artifact ZIP itself remains private. **Offline 0.2.0 60-file subscriber candidate build and importer PASS** in [Environment static CI #39276858703](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/actions/runs/39276858703); targeted negative tests for wrong commit/blob/DLL [CI #39277200471](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/actions/runs/39277200471) PASS. Steam/game processes and privileged publisher hosts were not invoked. This removes the need for a newly provisioned `AMJ_PROJECT_ARTIFACT_TOKEN` **only after PR #217's new pipeline becomes main**; the existing `main` pipeline still uses the old missing token and must not be dispatched as readiness or publisher until superseded/authorized. Original native FAIL and strict MO/SYR FMOD errors remain recorded. Formal staged transport: PR #217 `Docs/GoldenPaths/WorkshopPublication.md`, `Docs/ReleaseReadiness.md` and source workflows/importer/config.

**True outstanding authorization:** GitHub `steam-workshop-private` requires an actual author `sucRo0629` approval record and independently verified reviewer settings; separate Environment-specific Linux SteamCMD and Windows Steamworks runner allowlisting/account/authenticated host state is not proven. Identity/private visibility of existing Workshop item 3814638060 and exact distributed subscriber installation must be verified. No Steam upload/tag/visibility change, release-PR merge or subscriber test occurred. Keep **publication HOLD**; this status does not authorize bypassing an author approval or initiating [publish:...] issues.

### ENV-020-PUBLISHER-READINESS-20261019 — latest candidate/native source fixed; GitHub publisher HOLD

**Current owner source/native provenance:** Source `fe6861ac490bbd43ec73dce8efc1c2a5e5140479` includes Thin Soil growing-zone patch. Genuine Project [native 1.6 build #39247372678](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39247372678) created the new genuine 60-file candidate and compiled DLL (`b2ddbf7888322ba9b207c20296dba31175b846c05492902b900ec225da9fcffe` SHA-256); its original native run was **FAIL** (Vanilla warm SIGSEGV, MO/SYR FMOD), preserved as recorded. Supplemental **byte-identical** [Vanilla warm/cold rerun #39250184421](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39250184421) passed 82/82 + 73/73, clean exits/ERROR0, completing scoped eight-profile-case **628/628 functional assertions** on this source. Separate genuine 1.7 [Waylith #39249306881](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39249306881) passed its **focused** test, not 1.7 complete zoning acceptance. Old `5050e572...` artifact/DLL must not be reused. The unmerged release-candidate [PR #217](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/pull/217) pins the new 1.6 native ZIP/DLL/preview and bilingual 0.2.0 metadata; **not main, not uploaded**.

**Steam readiness still BLOCKED:** Original author-only read-only [Environment Actions #39243327351](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/actions/runs/39243327351) proved exact owner+main guard PASS but **Project artifact reader `AMJ_PROJECT_ARTIFACT_TOKEN` absent**, **no recorded protected-environment author approval**, and independent Linux/Windows Steam hosts **not reached**. These are GitHub publishing administration prerequisites; no upload, Workshop tag/visibility modification, or subscriber-download acceptance exists. The historical strict MO/SYR **FMOD ERROR0 FAIL stays visible**, while the author-approved *identical known FMOD baseline* is **non-blocking only for Environment-scoped private functional release**, never for novel error/crash. No release PR merge or Steam upload authorized by this status. Formal publication source `Docs/GoldenPaths/WorkshopPublication.md`, owner readiness `Docs/ReleaseReadiness.md`, and updated release PR evidence remain authoritative. Do not repeat source build or functional matrix merely to reconfirm a resolved scope.

### ENV-THINSOIL-GROWING-ZONE-20261019 — source MERGED; 1.6 author manual gameplay PASS

**Implemented source:** [Environment PR #218](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/pull/218), main `8ebe4d5dcbffabcd7e68198d6fd9a2a8bb1393ae`, `Source/AncientMedievalJapanEnvironment/ThinSoilGrowingZonePatch.cs`. No change to fertility 0.50, other terrain placement rules or Grains crop fertility minima. **Playable test branch:** [`test/environment-020-playable-20261019`](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/tree/test/environment-020-playable-20261019), commit `5dac3140230402b3b2aa96cbc117b98a23745dde`, genuine native DLL staged by [GitHub Actions #39258628883](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/actions/runs/39258628883).

**2026-10-19 author observation:** Tested actual **RimWorld 1.6** with the agreed compact three-point manual checklist: growing-zone paint on Thin Soil, compatible/incompatible crop sowing (including 0.70 rice/wheat), and rejection on unsuitable/occupied land. The author reported **“問題なし”**; separately confirmed the optional **Nice Plant Menu** presented a **fertility-deficiency warning**. This is **manual author-reported PASS, no independently collected screenshot/log**, not an automatic native assertion and not proof of 1.7 zoning, every edge cell or a full harvest lifecycle. **No repeated human verification requested** for the same 1.6 scope. Formal accepted feature behavior/evidence: `Docs/Design.md §10.3a`.

### ENV-020-SEASON-SAVE-NATIVE16-PASS-20261019 — scoped accepted, release HOLD

**Latest:** Project [Actions #39194825692](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39194825692), exact Environment source `03c743376a34ef9256acfed147da74adf5e75c51`, real Linux 1.6 Vanilla ColdBog: **26/26 seasonal/graphics checks PASS**, saved-game file 12,594,839 bytes, second native process reloaded ColdBog and Yoshi/Suge/Mizugoke/Hannoki, raw ERROR0 and both game exits 0. Original harness failures remain historical FAIL; test-only root setup fixed via genuine Play scene load. Canonical evidence: `Docs/ValidationEvidence/WetlandSeasonSaveNative16-20261019.md` and `Docs/ReleaseReadiness.md`. This accepts scoped calendar sampling + new-save reload, **not full 60-day simulation, 1.7 save, or final exact four-profile Steam subscriber delivery**. Independent private publisher, subscriber checks and final release copy/version are still OPEN.

### ENV-WETLAND-020-NATIVE17-20261017 — actual scoped 1.7 PASS

**Observed native:** [Project Actions #39041272179](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39041272179), exact Environment gameplay source `ea22a4fcb0454630d85eb48c9e88604cedc97497`; real Linux RimWorld 1.7.4977 Waylith native **Pickle 1/1 PASS, native ERROR0, FMOD0, exit0**. Actual loaded Yoshi/Suge/Mizugoke owner Defs, wetland biome commonality, nonfood and wild-only, and real CutPlant **Hay2 / Hay1 / no-drop** confirmed. Formal evidence: `Docs/ValidationEvidence/WetlandFourProductionNative17-20261017.md`. **This is a 50×50 normal-map test, not 1.7 wild wetland natural spawning or full season/save, all provider combinations or Steam release.** Existing 1.6 Vanilla/CCTO and MO functional evidence remains; original MO strict FMOD errors unchanged. 0.2.0 gallery/description/version, author-isolated private upload and subscriber byte verification still OPEN. Do not rerun accepted 1.7 smoke without changed production behavior.


### ENV-WORLDMAP-UNPAUSE-20261017 — author-corrected active mountain mapping handoff

**Owner:** WorldMap, **Environment interface:** stable loaded forest/subalpine/alpine BiomeDefs and ecological treeline/species data. **Latest author instruction:** WorldMap development must **not be paused until the reported issues are corrected**. The previous 'wait until Environment forest work is all complete' restriction is superseded for the four user-observed defects (offshore faction settlement sites, very scarce subalpine/alpine terrain, missing inhabitable mountain land and impassable summit exceptions for Fuji/Tateyama/Hakusan). WorldMap [#2](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-WorldMap/pull/2) changes base relocation and mountain passability, [#3](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-WorldMap/pull/3) improves physical input z8 GlobalMap DEM→z9 GSI DEM10B **without inventing a biome quota**, and [#4](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-WorldMap/pull/4) adds one-shot native acceptance of site/hill/peak status and measured mountain band counts. **No new highland distribution is yet certified**; WorldMap may require further terrain/treeline iteration. Do not demand Environment's unrelated wetland 0.2.0/full forest-food completion before WorldMap fixes. Preserve Environment's own Vanilla/MO procedural-world BiomeDef definitions/independent source and its existing vegetation release sequence. Any accepted new regional forest/treeline boundary must be formally recorded in Environment `Docs/Design.md`/production source if it changes Environment rather than remaining a WorldMap-only tile assignment. No Environment production change or native test by this handoff.

### ENV-WORLDMAP-HIGHLANDS-20261018 — latest author-owned design transfer

**Owner:** WorldMap for its own fixed-Japan intermediate-temperate forest BiomeDef, optional north/south regional forest subdivisions and map tile allocations. **Environment owner:** its existing four standard biome definitions, natural-world generation, common tree/plant/foraging ThingDefs and graphics. **Status: OWNERSHIP DECIDED / WORLDMAP IMPLEMENTATION NOT YET COMPLETE / NO ENVIRONMENT GAMEPLAY CHANGE.** This supersedes the prior request that Environment define the intermediate-temperate forest before WorldMap could assign it. WorldMap highland biome redistribution is deferred until **WorldMap** adds the intermediate forest and validates its regional ecology; 10°C/2°C trial thresholds remain unapproved. No new Environment Def, worker, planting or natural-world change is authorized. Confirmed owner design: WorldMap `Docs/Design.md` and Environment `Docs/Design.md`; any need for genuinely new shared species/PlantDefs is a separate Environment-owned specification. Preserve the prior coastline and released Environment behavior.

### ENV-WILD-FOOD-MERGE-20261017 — author-selected owner consolidation

**Owner:** Environment. **Status:** DESIGN MIGRATION / PRODUCTION OPEN. 2026-10-17 author approved folding the existing *planned, not playable* Wild Food Foraging 12-resource scope into a later Environment forest/wild-food update. Detailed single-owner source: `Docs/ForagingIntegrationDesign-ja.md`; central owner map `Docs/Design.md`. Existing wild Food Foraging research/tests remain historical only; **do not** build/release or require separate WFF, or copy old diagnostic fixtures as shipping content. Keep existing Shii tree/timber IDs and tree-sowing behavior; preserve all twelve adopted food resources, safety, native-gathering, region/host and no-hunting/preservation boundaries. All-AMJ play was always the intended food-balance target; do not claim the merge alone creates a new Grains balance risk. Wetland four-species 0.2.0 goes first; intermediate forest and food sourcing are later and not released by this decision. Project Architecture/Roadmap and former WFF owner must be synchronized by their own repo PRs; do not ask author to relay. No game test triggered.

## Immediate execution priority

**2026-10-15 — biome replacement candidate functional CLOSEOUT (source `4dacbcf0fbe3399ee44795f3097b8cbe498d0ae3`):** isolated licensed Linux 1.6 genuine production DLL + exact **59-file** subscriber package STATIC/ZIP PASS, [Project Actions #38681874743](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38681874743). Same-source Vanilla nine map/wetland/world/river/coast native **466/466 PASS, ERROR0**. Original five-tree actual pawn CutPlant: Vanilla/CCTO **17/17 each and ERROR0**, MO/MO+CCTO five RawWood yields correct but strict ERROR0 FAIL from independent upstream audio error.

**Unforced world biome functional 4/4 COMPLETE** (all original 24 assertions per profile, exit0): Vanilla [#38686772083](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38686772083), CCTO [#38693096172](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38693096172), MO [#38693109757](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38693109757), MO+CCTO [#38693122496](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38693122496). CCTO/Vanilla strict ERROR0; real MO and MO+CCTO raw Unity ERROR2 **exact previously independently reproduced** blank-clip FMOD, global strict FAIL. **Author's Environment-only private release classification treats unchanged external FMOD as non-blocking**, if assertions/exit/source pass with no new errors. Exact premises/raw failures/hashes: `Docs/ValidationEvidence/EnvironmentCandidateLinux16-20261015.md`; governing contract `Docs/ReleaseReadiness.md`. Earlier harness source/version false FAIL and first MO+CCTO exit139 are retained and not relabelled PASS. Original host game, source assets, Steam and saves unchanged.

**Immediate remaining priority:** existing author-owned GitHub Actions private-only Workshop uploader, verified isolated publisher credentials and original item `3814638060` state; review current tracked preview/source identity, stage **exact candidate** (not development root), then compare **actual downloaded** Workshop bytes/loader/DLL and run downloaded native smoke. No working Environment publisher/authenticated state has been demonstrated; do **not** borrow CCTO-for-VPE Steam auth/runner, do not revert to YADA and do not claim upload. Separate stable play-loop/season/new-save criteria remain; comprehensive legacy-world migration is not required for unchanged Def identities.

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

### ENV-STEAM-DEVQUICKSTARTS-LOADFOLDERS-20261009 — published root mismatch confirmed

**Owner:** Environment Workshop publication and downloaded-Pickle runtime
**Status:** BLOCKED UNTIL NEXT AUTHOR-MANUAL WORKSHOP RELEASE; WRONG DOWNLOADED loadFolders.xml; STEAM DISTRIBUTION HOLD

Author's `test-run.bat` blocked on the actual installed Steam Workshop `3814638060` before launching RimWorld. The existing approved `AMJE-Final-20261008-214525/Manifest.json` has 32 files; installed Workshop has the same 32 paths, **30 SHA256-identical**, and two differing metadata files: `About/PublishedFileId.txt` and `loadFolders.xml`. The approved payload's canonical `loadFolders.xml` has only `<v1.6><li>/</li></v1.6>`. The downloaded payload's exact semantic XML (author test log) includes an **additional executable conditional entry** `<li IfModActive="rimworks.quickstarts">DevQuickstarts</li>`. The tracked **developer** root `main:loadFolders.xml` contains that same conditional entry; the source candidate builder `Scripts/Build-WorkshopPayload.py` explicitly replaces it with canonical root-only `LOAD`. This is a meaningful runtime path difference, NOT harmless formatting; the fail-closed Pickle preflight correctly rejected it. It is **consistent with publishing the developer Mod folder through YADA's filtering rather than selecting the validated extracted candidate** (YADA filters directories but does not edit retained XML contents), although the actual uploader selection path is not independently recorded, so do not call the selection proven.

**Latest author decision (2026-10-09):** Correct this discrepancy **together with the next regular author-manual Workshop update**, not by an immediate corrective re-upload. The previously verified 32-file candidate is evidence for its pinned source only; before the next upload, pin the then-current clean source, build/verify its exact candidate and external manifest, and select that physically verified extracted root (never the development checkout). Preserve Steam provenance/runtime HOLD and the existing fail-closed downloaded-Pickle gate; do not rerun downloaded-Pickle against the known mismatched old installation. After the next author upload and fresh Steam download, verify downloaded file/source/DLL identity, then run Pickle and the separate four-profile rendered/native cutting gate. This decision did not perform an upload.

**Framework source audit performed:** `RimWorks/Rimworld-Pickle` official `Docs/getting-started.md`, `Docs/authoring.md`, `Docs/autorun.md`, `Docs/running.md`, `Docs/reports.md`, `RimWorks/pickle-template/README.md`, and `ilyvion/rimtest-redux/README.md` were read. Pickle's Gherkin, isolated test Steps in `Pickle/Assemblies`, `-pickle-run`, independent report/exit-code and per-mod-set conventions match the present workflow at a design level. RimTest Redux is a unit-test framework requiring test-only assemblies; it is not a substitute for Steam artifact provenance or Pickle's live loading. **Real Windows Pickle C# compilation/runtime is not yet validated.** Avoid further speculative harness rewrites until the release-source blocker is resolved. The official RimWorld `loadFolders.xml` semantics document confirms `IfModActive` loads an additional path when the target Mod is active.


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
sampling not covered by the existing assertions. The distinct Linux Waylith
1.7 pinned-source **single-map loaded-Def smoke** on 2026-10-15 passed real
Pickle 1/1, ERROR0, exit0 using an isolated .NET10 source build and genuine
Waylith UnityStubs; see `Docs/GoldenPaths/RimWorld17Tests.md` and
[Project Actions #38748267869](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38748267869).
It does **not** certify the separately shipped shared net472 production DLL
or full 1.7 Environment world/profile acceptance. Older author-reported and
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
