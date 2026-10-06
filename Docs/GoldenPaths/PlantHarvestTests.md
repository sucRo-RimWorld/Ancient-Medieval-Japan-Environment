# Four-species harvest resource regression

Build the current source, then run `Scripts/Run-PlantHarvestTests.ps1` from
Windows PowerShell with Steam running and no existing RimWorld process.

The runner stages current runtime folders and developer Quickstarts under a
unique test PackageId, preserving normal saves/configuration. It runs one
focused Quickstart per isolated profile on a non-visible Windows desktop with
rendering enabled: Vanilla and actual installed Medieval Overhaul plus its
required dependencies. MO's default enabled wood chain must yield
DankPyon_RawWood; standalone must yield WoodLog. A disabled MO wood-chain
configuration is not covered by this two-profile gate.

Each run verifies four final loaded harvest Defs and base yields, spawns healthy
mature plants with an adjacent colonist, adds native cutting designations and
starts native CutPlant jobs. Targeted pather/driver ticks advance the real
cutting toil; expected output is never directly spawned. Destruction and exact
nearby output are required, rejecting the other wood resource.

The isolated scenario temporarily fixes crop-yield difficulty to 1 and Plants
skill to 0 (no bonus above100%). Restore the difficulty in finally. Calling only
DriverTick stalls work in 1.6: the native work toil uses DriverTickInterval.
Tick both paths; preserve this regression lesson. Hash the staged payload before
and after execution, retaining PayloadManifest.json beside the reports.

Normalize every isolated active PackageId to lowercase. ModsConfig.IsActive
lowercases the lookup but a hand-written mixed-case active list can load an
assembly while its ModMetaData.Active is false. This caused MO's settings
constructor/Utility initialization to fail. The shared profile preparer now
writes lowercase IDs. Do not patch MO to hide this harness error.

The harvest runner adds an independent Unity Error/Exception/Assert observer
alongside RimLogging (required by Quickstarts). Require its CAPTURE_READY marker,
zero captured errors and zero structured errors in the main runtime log;
these gates cannot be waived by a 9/9 report.
Disable resetModsConfigOnCrash only in the isolated Prefs to keep failures from
silently switching to Core-only. Normal prefs are unchanged. Use -Mode MO or
-Mode Vanilla for focused diagnosis; the default Both verifies both profiles.

This verifies the cutting-job output path, not autonomous work selection,
hauling, distant pathfinding or village survival. Those remain separate tests.

Both reports must have 9 passing assertions, zero failures/pre-launch errors,
complete capture/no truncation and four output log rows. Owned and global
runtime ERROR gates remain mandatory. Failures retain evidence. The fixture is
retired below the unique timestamped result directory in finally.

Haimatsu preserves its BushBase silhouette and now yields existing wood: base8,
compared with Shii42/Beech40/Shirabiso30. No new resource Def or forced MO
override is introduced; MO's own replacement patch determines the resource.
