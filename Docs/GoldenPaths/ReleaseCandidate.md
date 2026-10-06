# Release candidate packaging

Use `Scripts/Build-ReleaseCandidate.py` after the current build/static checks and
`Tests/validate_plant_visual_coverage.py --self-test --require-complete` pass.
Before publication, run the [four-species harvest resource gate](PlantHarvestTests.md)
against the current source. Its result is separate from ZIP startup/map checks.
The candidate contains About, the production assembly, Defs, translations,
patches, textures, LICENSE, README and the English/Japanese Workshop copy.
The distribution loadFolders.xml loads only the root; developer Quickstarts,
art candidates, scripts and runtime test profiles are excluded.

The builder verifies archive CRC, exact source bytes, all four plant/state paths
and the retained Haimatsu UI icon path, and saves per-file SHA256 in a manifest.
The candidate is based on the local working tree; building it does not commit,
push or publish anything. A validated archive is not a clean installed runtime
PASS. Record that separate check before calling the candidate release-ready.

Keep staging below a container without About/About.xml so RimWorld does not
enumerate it as another installed mod with the same PackageId. Do not put a
publication checkout directly in Mods even when its directory name is hidden.

## Verified exact-package runtime sequence

Retain Tests/Release/ReleaseAudit.cs as an independent observer mod, compiled against installed Assembly-CSharp/UnityEngine.CoreModule/netstandard. Extract the ZIP byte-for-byte and verify every manifest hash. With no existing RimWorld process, temporarily move only source About into a retained backup; this permits unchanged original PackageId testing without selecting the development copy. Use an isolated save-data profile with Harmony/Core/RimLogging/candidate/observer; Quickstarts absent. The observer validates actual candidate root, normal/UI/snow/leafless graphics and Haimatsu UI/map scales at startup. Require its PASS marker, the Environment ERROR gate and no global structured ERROR/duplicate entries. Stop only the owned process, restore source About with hash equality, archive both fixtures below TestResults, and revalidate every payload hash. On interruption, restore source About before resuming ordinary work. This is a standalone startup/data-load smoke, not a map-generation or long-play test. Verified evidence: TestResults/ReleaseRuntime/Report.json.

## Exact-package map matrix

Reuse Scripts/Run-EnvironmentVegetationQuickstarts.ps1 with a new result root and isolated base/CCTO profiles. Keep the release ZIP unchanged and root-only; load the compiled DevQuickstarts test DLL from a separate audit observer Mod instead. The matrix observer verifies actual loaded candidate root and all four species images/UI/snow. Tests/Release/IsolatedDesktopRunner.cs retains the Core desktop launcher bindings and runs the driver batch on a non-visible desktop with rendering enabled; timeout kills only its owned process tree. Run six base scenarios followed by CctoCompatibilityOnly. Require successful JSON counts, zero pre-launch errors, complete capture/no truncation, observer PASS in every log, mod-origin/global error gates, unchanged payload hashes, restored source About and retired fixtures. Verified 2026-10-06: 58/57/54/52/3/3 base assertions and70 CCTO, all passed; TestResults/ReleaseMatrix/Report.json. Full MO runtime and long-play remain outside this matrix.
