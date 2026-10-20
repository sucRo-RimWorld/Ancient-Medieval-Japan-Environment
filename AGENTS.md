# AMJ Environment Agent Instructions

## Start here

1. Read this file and `main:Docs/Coordination.md`; locate the latest relevant owner/status/evidence, including later corrections. Historical entries are not current approval.
2. Read Project [AGENTS.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/AGENTS.md) and [Docs/SharedRules.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/SharedRules.md): apply its stop conditions, then open only the task-relevant canonical procedures.
3. Read the local specification and affected source/tests below. Shared rules are owned by Project; this file owns only local scope and routing. Missing access or conflicting authority blocks the dependent action, not unrelated safe work.

New features cannot enter implementation before the Project [existing-Mod audit gate](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/ExistingModAudit.md#implementation-entry-gate) covers VE and non-VE alternatives and records why independent implementation is needed. Existing approved behavior is not redesigned by this rule audit.

## Local task routes and stops

- Design/implementation: `Docs/Design.md`. Retain the scoped XML-first preference below.
- **Fixed-map forest owner exception (author decision 2026-10-18):** AMJ WorldMap (not Environment) owns the proposed intermediate-temperate **BiomeDef** and fixed-Japan regional biome subdivisions. Environment retains its four broadly generated BiomeDefs and all common Japanese plant/foraging ThingDefs, graphics and behavior; WorldMap can reference those without duplicating them. Do not add new intermediate biome workers/placement to Environment's random-world generation from this decision alone. Formal boundary: `Docs/Design.md` and WorldMap `Docs/Design.md`.
- **Owner boundary (author-confirmed 2026-10-17, superseding WFF split):** Environment now owns **both Japan's landscape vegetation and its natural wild edible plants, nuts, mountain greens, mushrooms and wild roots**, including new native ThingDefs, food outputs, graphics, region-specific natural spawn and their bounded gather/preparation behavior. Source of truth: [Foraging Integration](Docs/ForagingIntegrationDesign-ja.md) and [Design](Docs/Design.md). Existing Shii/Beech PlantDef identities, forestry/wood and research remain intact; neither a separate WFF runtime dependency nor copied WFF PlantDefs may be introduced. The former [Wild Food Foraging repository](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Wild-Food-Foraging) is retained only as pre-merge research/diagnostic history, not a second active owner. Keep cultivation/Grains, preservation, fishing, hunting and other systems outside Environment.
- Plant/tree art: follow `Docs/GoldenPaths/PlantVisualCoverage.md` and the test-specific guard in `Tests/AGENTS.md` when validating plant visual coverage. Existing-tree retextures remain author-deferred.
- Color review: fixed actual local time 12:00, Clear weather, paused map, matching zoom and season/shader intensity for comparisons; record actual conditions. Follow the same coverage document. Dawn/dusk/night/weather screenshots cannot establish source colors.
- Art procedures: `Docs/GoldenPaths/README.md` and the routing below. Accepted masters and fixed regions retain Project protection.
- Accepted image binary transfer: `Art/AGENTS.md` (scope: accepted `Art/Sources/` masters and paired `Textures/` derivatives); use the established Base64 → temporary Library → Git Blob → topic-branch PR route, not a documentation-only substitute.
- Release (2026-10-20 author decision): Steam Workshop upload uses **RimWorld YADA + Add Changenote** again, not GitHub Actions/SteamCMD. Use only the sealed, verified release-root staging folder and existing Workshop item `3814638060`, preserving existing visibility. GitHub Actions may produce/validate a candidate but must not publish. Do not upload an unmerged 0.2.0 candidate without separate final publication authorization. The live Workshop upload and subscriber download remain unverified. Earlier Actions-only release instructions below are superseded by `Docs/GoldenPaths/WorkshopPublication.md`.
- Release: `Docs/GoldenPaths/WorkshopPublication.md`, `Docs/GoldenPaths/ReleaseCandidate.md`, `Docs/ReleaseReadiness.md`. Pinned clean source, exact selected upload root and external manifest, actual downloaded source/DLL and tested four-profile Environment behavior are required to clear distributed-release HOLD. The owner readiness contract permits only the independently reproduced, exact MO/SYR blank-clip FMOD baseline as a separately labeled **non-blocking external issue** for an Environment-only private release; raw ERROR counts and the strict global ERROR0 FAIL must remain intact, and new errors or crash are never waived. Never upload a dirty development root or infer uploaded bytes from Git main. Author-selected GitHub Actions private-only publication supersedes historical manual YADA; no live upload may occur without verified owner-isolated authenticated publisher state.
- Public copy: `Docs/WorkshopDescription.md`, `Docs/2GamePresentation.md`, and Project's current public-description contract.

## Implementation preference

Prefer RimWorld Def/XML/Patch operations when they are sufficient. Add C# only for world-generation or runtime behavior that cannot be expressed cleanly and compatibly through Defs/XML.

## PowerShell change safety

For edits under `Scripts/`, follow `Scripts/AGENTS.md`. For root-level test entry points such as `run-tests.bat`, keep its PowerShell syntax preflight intact. Changes still require the Project PR-only protection gate.

## Art / retexture routing

Do not duplicate detailed art rules in AGENTS.

For Environment art, use:
1. Ancient-Medieval-Japan-Project `Docs/ArtStyle.md` — AMJ-wide visual invariants and the Medieval Overhaul-oriented in-game art baseline;
2. `Docs/ArtDirection.md` — Environment-specific tree/plant/terrain/world style and accepted species baselines;
3. `Docs/GoldenPaths/RetextureGeneration.md` — generation entry/preflight only;
4. `Docs/GoldenPaths/TextureAssetPipeline.md` — installation/export/validation;
5. Ancient-Medieval-Japan-Project `Docs/GoldenPaths/FixedImageTemplates.md` only when a visible component is intentionally reused pixel-exactly.

Environment-specific rules may define controlled class differences, such as restrained internal gradient variation for tree sprites, but they must remain compatible with the shared AMJ invariants unless the owning style specification explicitly records an exception.

For Vanilla/MO retexture ownership and texPath behavior, continue to follow the shared Ancient-Medieval-Japan-Project `Docs/RetextureImplementationGuidelines.md`.
