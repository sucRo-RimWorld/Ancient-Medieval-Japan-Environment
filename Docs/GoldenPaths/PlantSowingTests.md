# Regional tree sowing regression

Contract: `Docs/Design.md` — Regional tree sowing contract. Natural vegetation
retention decisions in `Docs/VanillaPlantRetentionAudit-ja.md` also own whether
an ordinary tree remains available for growing-zone sowing in an AMJE biome.

## Fast preflight

Run `python Tests/test_tree_sowing_contract.py` before pushing. The scoped
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

## Existing-save plant and fruit-tree update E2E (planned, not implemented)

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
gate. Current native growing-zone loaded-Def tests **do not** verify a planted
pawn Job, real fruit harvest, save migration or plant natural spawning on an
already generated map. None of the above E2E has been run or accepted yet.
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
