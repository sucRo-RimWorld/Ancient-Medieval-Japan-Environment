# AMJ Environment Agent Instructions

## Start here

1. Read this file and `main:Docs/Coordination.md`; locate the latest relevant owner/status/evidence, including later corrections. Historical entries are not current approval.
2. Read Project [AGENTS.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/AGENTS.md) and [Docs/SharedRules.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/SharedRules.md): apply its stop conditions, then open only the task-relevant canonical procedures.
3. Read the local specification and affected source/tests below. Shared rules are owned by Project; this file owns only local scope and routing. Missing access or conflicting authority blocks the dependent action, not unrelated safe work.

New features cannot enter implementation before the Project [existing-Mod audit gate](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/ExistingModAudit.md#implementation-entry-gate) covers VE and non-VE alternatives and records why independent implementation is needed. Existing approved behavior is not redesigned by this rule audit.

## Local task routes and stops

- Design/implementation: `Docs/Design.md`. Retain the scoped XML-first preference below.
- Plant/tree art: before resuming, selecting another target, handing off or claiming completion, read `Docs/GoldenPaths/PlantVisualCoverage.md` and run `python Tests/validate_plant_visual_coverage.py`. Resolve/report pending current-plant states first. Whole-plant/all-state completion requires `--require-complete`; normal-state approval is insufficient. Existing-tree retextures remain author-deferred.
- Color review: fixed actual local time 12:00, Clear weather, paused map, matching zoom and season/shader intensity for comparisons; record actual conditions. Follow the same coverage document. Dawn/dusk/night/weather screenshots cannot establish source colors.
- Art procedures: `Docs/GoldenPaths/README.md` and the routing below. Accepted masters and fixed regions retain Project protection.
- Release: `Docs/GoldenPaths/WorkshopPublication.md`, `Docs/GoldenPaths/ReleaseCandidate.md`, `Docs/ReleaseReadiness.md`. Pinned clean source, exact selected upload root and external manifest, actual downloaded source/DLL and four-profile cutting gate are required to clear distributed-release HOLD. Never upload a dirty development root or infer uploaded bytes from Git main. Steam remains author-manual.
- Public copy: `Docs/WorkshopDescription.md`, `Docs/2GamePresentation.md`, and Project's current public-description contract.

## Implementation preference

Prefer RimWorld Def/XML/Patch operations when they are sufficient. Add C# only for world-generation or runtime behavior that cannot be expressed cleanly and compatibly through Defs/XML.

## PowerShell change safety

PowerShell changes must not be committed directly to `main`.

- Stage any `.ps1` edit on a temporary branch.
- Keep `.github/workflows/powershell-syntax.yml` as the repository-level parser gate.
- Open a pull request and require the PowerShell syntax check to pass before merging the branch to `main`.
- `run-tests.bat` must execute `Scripts/Validate-PowerShellSyntax.ps1` before build/static validation so local test runs fail fast on parser errors.
- Avoid large string-replacement edits to `Validate-Environment.ps1`; when changing a focused block, re-read the edited range and the end of file before merge to detect truncation or duplicated tails.

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
