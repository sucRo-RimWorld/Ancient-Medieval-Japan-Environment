# AMJE Golden Paths

A **Golden Path** is the repository's known-good, repeatable procedure for work that has already reached a verified successful result.

## Completion rule

When a task moves from failing/unknown to a verified PASS, do not immediately move on.

Before the task is considered complete, perform a Golden Path capture check:

1. identify the exact sequence that produced the successful result;
2. preserve that sequence in repository documentation when it is reusable;
3. automate every deterministic step that can reasonably be automated;
4. add regression checks for the failure classes discovered during the work;
5. leave only genuinely subjective checks—visual quality, feel, readability, artistic judgment—as manual steps;
6. link the procedure from the owning design/tooling documentation;
7. update the Golden Path whenever the successful workflow changes.

A one-off trivial text edit with no reusable procedure can be marked N/A. A workflow involving debugging, generated assets, binary transfer, build/test sequencing, runtime setup, release steps, or repeated manual commands is **not** N/A.

"Worked once" is not a stable completion state. The goal is **worked once → documented → automated where deterministic → regression-locked**.

## Current Golden Paths

- [Pinned Workshop Publication](WorkshopPublication.md) — proven upload-source audit, immutable runtime payload, exact selected-root verification and author-manual Steam handoff.

- [Actual Workshop Runtime Gate](WorkshopRuntimeTests.md) — source/DLL proof, four real profiles and current harvest regressions without replacing the distributed payload.

- [Plant Harvest Resource Tests](PlantHarvestTests.md) — native cutting outputs in Vanilla and actual MO.

- [Regional Tree Sowing Tests](PlantSowingTests.md) — static tree-pool contract and native growing-zone eligibility before/after research; fresh runtime matrix pending.

- [Rendered Runtime Tests](RenderedRuntimeTests.md) — `run-tests.bat` standard gate: static validation followed by rendering-enabled Quickstarts on an independent non-visible Windows desktop.

- [Plant Visual Coverage Gate](PlantVisualCoverage.md) — per-state approval/evidence and scoped invalidation.

- [Release Candidate](ReleaseCandidate.md) — exact runtime-only ZIP and standalone/map/CCTO validation.


- [Texture Asset Golden Path](TextureAssetPipeline.md) — production PNG validation/install, Def-switch order, automated gate, and correct-biome runtime review for ENV-010 art.

- [Retexture Generation / New-Chat Start Procedure](RetextureGeneration.md) — generation preflight, actual approved references, current Project+Environment style-rule assembly, and candidate review before the production texture pipeline.
