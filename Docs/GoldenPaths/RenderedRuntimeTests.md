# Rendered runtime tests on an isolated Windows desktop

## Standard AMJE entry point

Use:

```bat
run-tests.bat
```

Optionally pass a RimWorld installation root as the first argument.

```bat
run-tests.bat "D:\SteamLibrary\steamapps\common\RimWorld"
```

`run-tests.bat` is the canonical AMJE automated gate. It runs:

1. `run-static-tests.bat` for PowerShell syntax, build, installed-source Def validation, texture-copy smoke, and the installed Medieval Overhaul tree-reference audit;
2. `Scripts/Run-EnvironmentIsolatedDesktop.ps1`;
3. the runtime suite on a newly created, non-visible Windows desktop with normal Direct3D rendering;
4. the default Environment Quickstart matrix, including the four AMJE climate biomes, `TemperateSwamp`, `ColdBog`, and River/Coast handoff;
5. optional installed CCTO and Core/Grains integration profiles already owned by `run-runtime-tests.bat`.

The isolated launcher does not switch the active desktop and does not use
`-nographics`. Rendering therefore remains active while automated RimWorld
windows stay off the user's visible desktop. Runtime reports retain the existing
pre-launch/runtime ERROR gate, live-log capture and truncation checks.

`run-static-tests.bat` is the lower-level static-only entry point.
`run-runtime-tests.bat` remains a lower-level runtime/debug entry point; when
run directly it may display RimWorld windows. The standard `run-tests.bat`
path invokes it with `--skip-static` from the isolated desktop after the static
gate has already passed, preventing recursion and duplicate static work.

The wiring is regression-locked by
`Tests/test_run_tests_entrypoint.py`. In particular, the test requires both
wetland Quickstarts to remain part of the default runtime suite.

## Combined AMJ launcher

For a combined Grains + Environment gate, the reusable launcher remains in the
Grains repository:

[Grains IntegratedRuntimeTesting.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/blob/main/Docs/IntegratedRuntimeTesting.md)

Environment no longer depends on that repository merely to obtain a non-visible
runtime path; its own standard `run-tests.bat` now provides the same isolation
property for AMJE-only validation.

## Historical verified result

2026-10-05 JST result: Environment build/static, PNG exact-copy, MO static
8-tree/15-reference audit, six base Quickstarts and the optional CCTO run PASS.
Base counts: 58/58, 57/57, 54/54, 52/52, River 3/3, Coast 3/3. CCTO 70/70.
Every report has zero pre-launch errors, complete live capture and no truncation;
runtime ERROR gates PASS. Reports remain in TestResults/VegetationRuntime.

This exercised the current local working tree, including prior uncommitted
plant-review changes. It does not mark pending human plant-state reviews
accepted, claim a full MO runtime run, or claim a combined production
Core+Environment profile. The MO gate remains static as intentionally designed.

Publication preserves newer GitHub-main gameplay-contract tests. The recorded
counts are historical local-snapshot evidence; the newer Core-profile runtime
gates remain pending and are not inferred to pass from those reports.

