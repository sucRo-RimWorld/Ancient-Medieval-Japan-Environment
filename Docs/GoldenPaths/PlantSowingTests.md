# Regional tree sowing regression

Contract: `Docs/Design.md` — Regional tree sowing contract. Natural vegetation
retention decisions in `Docs/VanillaPlantRetentionAudit-ja.md` also own whether
an ordinary tree remains available for growing-zone sowing in an AMJE biome.

## Fast preflight

Run `python Tests/Production/test_tree_sowing_contract.py` before pushing. The scoped
`Regional tree sowing contract` workflow runs the same gate on main/PR changes.
It locks all four XML wild woody pools, positive commonality, AMJE parent
inheritance, the absence of unreviewed sowing overrides, and the distinct
natural woody/sowable runtime sets. Mutation cases reject reintroduced Poplar,
zero-commonality Sudajii, missing/zero Haimatsu, regional-gate bypass and
accidental Haimatsu tree inheritance. This is static
coverage, not a substitute for XML inheritance/patch resolution in the game.

## Loaded-Def/native growing-zone gate

1. Use `build.bat` with installed RimWorld 1.6, Harmony and Quickstarts. Confirm
   `DevQuickstarts/Assemblies/AncientMedievalJapanEnvironment.Quicktests.dll`
   was built; a skipped Quickstarts build is not a successful runtime test.
2. Use the canonical `run-tests.bat` entry point documented in
   [RenderedRuntimeTests.md](RenderedRuntimeTests.md). Its base isolated
   rendered suite contains **eight** Quickstarts: four AMJE forests,
   TemperateSwamp, ColdBog, River and Coast. Installed CCTO and Grains/Core
   integrations are optional profiles. The separate four-profile
   Vanilla/MO/CCTO/MO+CCTO release matrix is not currently automated
   by this standard runner because the MO runtime profile is disabled.
3. All four fixed-biome scenarios call `AddTreeSowingAssertions` automatically.
   It checks loaded ordinary-tree Ground/TreeSowing/regional contracts, the
   exact base-Biome natural woody set, no ordinary-tree menu options before research,
   the separate approved menu set after research, and Haimatsu exclusion. It uses
   the two native menu filters on an unregistered, unpolluted growing-zone
   probe. No planted pawn job or completed tree growth is claimed by this test.
4. The test temporarily changes research progress in the disposable test game,
   restores the entire progress dictionary in `finally`, and never registers
   the zone or changes normal settings/saves. It does not call research
   completion hooks, unlock Mod content globally, or open UI menus.
5. Require successful scenarios, zero pre-launch/runtime ERROR, full live-log
   capture and no truncation via the existing runner/error gate. Preserve
   `[AMJ Environment TreeSowing]` expected/actual wild-woody and sowable sets with the reports.

The special harvest-only early return intentionally continues to run just the
native cutting contract; use the normal vegetation matrix for sowing coverage.
Profiles with unrelated third-party tree/distribution changes require a
deliberately reviewed compatibility contract instead of silently widening the
approved sets. The base-Biome tree check uses `map.Biome.wildPlants` rather than map-wide `WildPlantSpawner.AllWildPlants`, which may include tile-mutator or supplemental-biome plants. The exact native growing-zone menu check still rejects unexpected sowable trees.
In RimWorld 1.6, `AMJ_Shrub_Haimatsu` appears in the Alpine base-Biome
`wildPlants` with `plant.IsTree=true`, although it inherits `BushBase` and is
not sowable. Therefore Alpine expects wild woody `{AMJ_Shrub_Haimatsu}`
but grow-zone sowable `{}`. Never compare those different sets directly.

## Birch sow-Job and sampled-growth focus (PR #310; generic native PASS)

The source-owned `AMJShirakabaSowingJobQuickstart` and
`AMJDakekanbaSowingJobQuickstart` exercise each new species in its own
native AMJE biome. They choose the warmest eligible world tile of that biome,
register a temporary one-cell growing zone, unlock TreeSowing only in the
disposable test game, and ask Vanilla `WorkGiver_GrowerSow.JobOnCell` for a
real pawn `Sow` Job. The Job must place the requested species, mark it sown,
and complete its native sow work. A further hourly game-calendar + real
`Plant.TickLong` probe must observe growth of **that same sown individual**.
The zone, pawn, plant and research progress are disposed/restored in `finally`.
Soil/obstacles around the probe are deliberately prepared for a reproducible
sowing test; this is not a claim that an untouched map cell is always plantable.

The optional Project-manual `test_focus=birch-sowing` adapter is proposed in
Project PR #1183, for original RimWorld **1.6 Vanilla** only, exact owner PR
HEAD and same-run candidate. A genuinely executed PASS requires **all four**
native labels per species (growing season, real Sow Job, same-plant sampled
growth, cleanup) plus the existing strict ERROR0 and build/source checks.
These **original species-specific owner Quickstarts and the unmerged
Project PR #1183 adapter** have not separately run or been native-compiled.
Do not reactivate that special-purpose route merely because it exists.

**Real generic replacement tested 2026-11-03:** the Project's *existing*
source-pinned, owner-case-driven manual dispatcher now supports
`plant-sowing16`; the same non-species-specific `AMJPlantSowingQuickstart`
ran against the PR #310 candidate at
`0cf3564a4661408eb1ff5f3ad1fab1fb36ec6e3b`.
[Shirakaba Actions #41576018463](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/41576018463)
and [Dakekanba Actions #41576354854](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/41576354854)
each returned **4/4 native assertions PASS**, with original RimWorld
1.6.4871 rev600, exit0, Unity ERROR0, exact-case `def_name` and biome,
genuine `WorkGiver_GrowerSow.JobOnCell` / Pawn Sow Job placing the
target tree at native initial Growth `0.0001`, and sampled native
`Plant.TickLong` growth of that same tree. Disposable research, pawn, zone
and planted tree are cleaned up after the case. Formal original evidence
is in [BirchNative16-20261101](../ValidationEvidence/BirchNative16-20261101.md).
The earlier preflight/display/initial-seedling false-negative runs remain
FAIL and are explained in that file.

**Not established by this narrow PASS:** continuous fully-ticked growing
season or 20-day maturity, planted-to-cut Job/wood yield, legacy save
migration, natural repopulation in an already-loaded map, MO/CCTO/1.7,
author visual acceptance or release readiness. Previously verified
separate loaded-menu and cutting evidence remains unchanged.

## Birch actual old-version save -> candidate -> updated-save reload (PR #310; real native PASS)

The **existing** Project manual `plant-legacy-save16` suite selects
owner `case_id=shirakaba` from the exact SHA's
`Tests/PlantNativeAcceptance.json`, with no Birch-specific Project workflow
or automatic game trigger. Source-owned
`AMJBirchLegacySaveQuickstart` asserts the original ColdBog legacy
`Plant_TreeBirch` and both new natural species; Project supplies the reusable
real **three-process old-version → candidate → second reload** lifecycle.
Its pinned old Environment source SHA is
`b6ca638df7016a82c42606f0c73fb2470ce5f1b3`; the old product is
**actually built**, not copied from a current Mod or fabricated save XML.
The older, unmerged Project PR #1183 proposed a redundant
`test_focus=birch-save`; it is **not** the approved manual test entry.

1. Generate a populated **real ColdBog map** with naturally occurring old
   Birch under old production. Require no new Birch Defs, save a real `.rws`,
   preserve legacy tree count, a representative Thing ID and its SHA-256.
2. Replace only the *disposable* game's production Mod with the exact
   same-run PR package, load that **same old save** through entry→Play,
   require unchanged old Birch count/ID and ColdBog new `wildPlants`
   commonalities 0.40/0.05, with old Birch removed **only from natural spawn
   eligibility**, not deleted as a ThingDef.
3. Save the upgraded map to a second real `.rws`, restart the original game,
   reload the updated save, require the same old individuals and new
   loaded wild eligibility. Require zero Unity ERROR, exits0, correct game
   version, no timeout, exact phase markers, preserved raw logs and both
   original/updated save hashes.

**Actual PASS 2026-11-03:** [Project Actions #41591244191](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/41591244191)
executed all 3 phases under **RimWorld 1.6.4871 rev600 Vanilla** against exact
owner source `f03bc91b2e752a626a99302946ac33d8e41e6923`, Project engine
`29a9b3330322f196f97247f1d87419134a56aac3`, raw native artifact
**13210631825**. Actual old ColdBog map contained **1,226**
`Plant_TreeBirch`; representative Thing ID **8135** survived both
update and second saved-file reload. Candidate new native pool:
`AMJ_Tree_Shirakaba=0.40`, `AMJ_Tree_Dakekanba=0.05`,
old natural Birch entry absent. All 3 original-game processes exit0,
Unity ERROR0, baseline Quickstart 3/3 PASS. Real original and updated
`.rws` files are retained as artifacts with SHA-256.
[Formal per-phase log and digest evidence](../ValidationEvidence/BirchNative16-20261101.md).

**Scope limit:** proves old naturally occurring tree persistence plus the
*loaded* new spawner pool over a real update and two reloads, **not** actual
natural new-tree appearance after elapsed wild-spawner ticks on that already
populated map. The distinct mature-tree native CutPlant → 27 WoodLog
case was previously verified under real 1.6 Vanilla/CCTO but is **not**
a fully continuous sow→grow-to-maturity→cut chain.
MO/CCTO/1.7 were not run for this particular old-save suite. No user save
or Workshop distribution was changed.

## Existing-save plant and fruit-tree update E2E (remaining full acceptance)


Decision source: `Docs/Design.md` — Save-compatible vegetation development order.
This gate is for **updating an existing save with AMJE already installed**, not
for installing AMJE for the first time, removing AMJE, or retroactively
rebuilding the world map.

When the first new wild/fruit tree or harvestable herb is implemented:

1. Create a disposable old-version AMJE save with a generated and populated
   colony map; retain immutable baseline save/artifacts for a before/after pair.
2. Update to the new AMJE revision and load **that same save**. Assert normal
   load and preserved map/placed plants; assert the added PlantDef, products and
   applicable base-Biome `wildPlants` commonality are present in the loaded Defs.
3. Use a repeatable time/weather/terrain/density setup to verify **new natural
   growth on the existing map**, without confusing forced test spawning with
   natural spawner eligibility. Report time and sampling limits; do not require
   immediate retroactive repopulation of all existing forest cells.
4. Verify age/maturity and the **actual harvest yield** of the added fruit/nut
   or medicinal resource, including repeatable harvest rules if applicable.
5. For a deliberately sowable tree, verify regional/TreeSowing research gates,
   a real sowing Job on a valid growing-zone cell, subsequent growth, and
   product harvesting. Preserve non-sowable species' exclusion.
6. Save again, reload, check the added plants and products persist, and require
   zero AMJE-owned pre-launch/runtime ERRORs with complete capture.
7. Run separately where applicable for AMJE standalone, MO, CCTO and MO+CCTO;
   record unavailable integration profiles explicitly rather than claiming
   full matrix PASS.

Use non-visible rendering-enabled Quickstarts/Pickle or RimTest Redux as
appropriate, extending the existing `run-tests.bat` path and runtime ERROR
gate. Earlier growing-zone loaded-Def tests alone do **not** prove an actual
pawn Sow Job; the separate **2026-11-03 generic `plant-sowing16` native PASS**
now proves actual planting and same-tree **sampled** growth for Shirakaba
and Dakekanba in original 1.6 Vanilla. It does **not** prove real
maturity/wood yield, pre-feature old save migration, newly naturally
spawned Birch on an existing map or fruit harvest. The original Birch pre-update/upgrade/reload suite **has now passed**
its specifically bounded real original 1.6 Vanilla E2E, as documented above;
the **complete existing-save natural regrowth, product harvest, MO/CCTO/1.7
matrix** is still unverified.
Until its results exist, mid-save updates are a design target, not a
verified public compatibility promise.

## Evidence boundary

2026-10-08 JST: native sowing regressions added; Python contract/mutation tests
and C# syntax preflight run in the editing environment. Installed game/reference
assemblies are unavailable here, so a fresh game build and four-profile runtime
result remain pending. Earlier map/cutting PASS counts predate these assertions
and do not prove this new contract passed.

Latest author runtime evidence (2026-10-08 JST): warm/cool/subalpine passed;
Alpine failed 1 of 70 assertions under the old conflated-set test; 69 passed,
including Haimatsu generation and exclusion from sowing, with no captured runtime
ERROR. The corrected test must be rerun with the latest-main isolated
eight-scenario vegetation matrix, including both Vanilla wetlands and
River/Coast. The follow-on wetland ecology loaded-Def assertions added in
PR #24 also need a fresh game run; the old Alpine result does not validate
those new checks. A separate four-profile MO/CCTO release test is still
pending. No new full-matrix runtime PASS is claimed.
