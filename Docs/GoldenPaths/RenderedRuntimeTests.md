# Rendered runtime tests on an isolated Windows desktop

Use the Core-owned reusable launcher and documented procedure:

[Core IntegratedRuntimeTesting.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Core/blob/main/Docs/IntegratedRuntimeTesting.md)

From Core: `powershell -NoProfile -ExecutionPolicy Bypass -File Scripts/IntegratedRuntimeDesktop/Run-AMJ-IsolatedDesktop.ps1`.

It runs the existing Core and Environment entry points serially on a newly
created, non-visible Windows desktop, with Direct3D rendering enabled. It does
not switch the active desktop or use `-nographics`. This is desktop isolation,
not Xvfb or a separate virtual monitor. Normal Steam access and a process-local
Windows PowerShell module path are necessary; see the Core failure-prevention
notes. No Environment PowerShell script is changed by this workflow.

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
