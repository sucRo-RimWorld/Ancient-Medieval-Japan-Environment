# Regional tree sowing regression

Contract: `Docs/Design.md` — Regional tree sowing contract. Natural vegetation
retention decisions in `Docs/VanillaPlantRetentionAudit-ja.md` also own whether
an ordinary tree remains available for growing-zone sowing in an AMJE biome.

## Fast preflight

Run `python Tests/test_tree_sowing_contract.py` before pushing. The scoped
`Regional tree sowing contract` workflow runs the same gate on main/PR changes.
It locks all four XML tree pools, positive commonality, AMJE parent inheritance,
the absence of unreviewed sowing overrides, and alignment with runtime expected
sets. Mutation cases reject reintroduced Poplar, zero-commonality Sudajii,
regional-gate bypass and accidental Haimatsu tree inheritance. This is static
coverage, not a substitute for XML inheritance/patch resolution in the game.

## Loaded-Def/native growing-zone gate

1. Use `build.bat` with installed RimWorld 1.6, Harmony and Quickstarts. Confirm
   `DevQuickstarts/Assemblies/AncientMedievalJapanEnvironment.Quicktests.dll`
   was built; a skipped Quickstarts build is not a successful runtime test.
2. Prepare isolated configurations with the existing runtime preparation
   scripts. Run the normal six-scenario vegetation runner through the existing
   [non-visible rendered launcher](RenderedRuntimeTests.md). Use all four
   supported profiles: Vanilla+AMJE, MO+AMJE, CCTO+AMJE, MO+CCTO+AMJE.
3. All four fixed-biome scenarios call `AddTreeSowingAssertions` automatically.
   It checks loaded ordinary-tree Ground/TreeSowing/regional contracts, the
   exact base-Biome wild-tree set, no ordinary-tree menu options before research,
   the exact approved menu set after research, and Haimatsu exclusion. It uses
   the two native menu filters on an unregistered, unpolluted growing-zone
   probe. No planted pawn job or completed tree growth is claimed by this test.
4. The test temporarily changes research progress in the disposable test game,
   restores the entire progress dictionary in `finally`, and never registers
   the zone or changes normal settings/saves. It does not call research
   completion hooks, unlock Mod content globally, or open UI menus.
5. Require successful scenarios, zero pre-launch/runtime ERROR, full live-log
   capture and no truncation via the existing runner/error gate. Preserve
   `[AMJ Environment TreeSowing]` expected/actual sets with the reports.

The special harvest-only early return intentionally continues to run just the
native cutting contract; use the normal vegetation matrix for sowing coverage.
Profiles with unrelated third-party tree/distribution changes require a
deliberately reviewed compatibility contract instead of silently widening the
approved sets. The base-Biome tree check uses `map.Biome.wildPlants` rather than map-wide `WildPlantSpawner.AllWildPlants`, which may include tile-mutator or supplemental-biome plants. The exact native growing-zone menu check still rejects unexpected sowable trees.

## Evidence boundary

2026-10-08 JST: native sowing regressions added; Python contract/mutation tests
and C# syntax preflight run in the editing environment. Installed game/reference
assemblies are unavailable here, so a fresh game build and four-profile runtime
result remain pending. Earlier map/cutting PASS counts predate these assertions
and do not prove this new contract passed.
