# Terrestrial biome replacement and Alpine access gate

The root-surface processor excludes twelve named Vanilla terrestrial biome
candidates, not their Defs. Water and third-party candidates retain normal worker
scoring. AMJ workers accept wet land. Alpine access caps Impassable at 25% of
AMJ Alpine tiles, preserving the highest peaks and retaining elevation/climate.
Existing saves/worlds are not regenerated.

Use `Scripts/run-biome-replacement-gates.cmd` through the existing private-desktop
runner `Tests/Release/IsolatedDesktopRunner.cs`. Compile the runner to
`TestResults/BiomeReplacement/IsolatedDesktopRunner.exe` with the .NET Framework
C# compiler; create that output directory first. Pass the absolute gate CMD path
as argument 1 and the repository root as argument 2. Never switch to its desktop.
This maintains rendering without displaying the game on the normal desktop.

The CMD builds the implementation and developer Quickstarts, creates isolated
save-data profiles, runs six vegetation/river/coast scenarios, then a focused MO
profile. Normal ModsConfig/Prefs and saves are not changed.
It sets `RIMWORLD_AMJE_WORLD_BIOME_AUDIT=1` to require a 30% world containing Alpine
and wetland candidates. An ordinary 5% test world can contain neither and is not
sufficient coverage. The Quicktest assembly must reference the production DLL.

Required assertions:

- no Vanilla terrestrial biomes in generated land;
- saturated wetland has a positive AMJ worker score at every climate boundary;
- wet candidates resolve to allowed land biomes;
- no more than 25% of AMJ Alpine land remains Impassable;
- Vanilla Defs remain loaded and water candidates remain allowed;
- MO Dark Forest actually appears in the MO profile.

Reports/logs are under `TestResults/BiomeReplacement`. The existing runner fails
on failed assertions, startup errors, incomplete live capture, truncated logs,
or repository-origin runtime ERROR entries. Never accept only scenario counts.
Build/static checks remain separate; Workshop source/size validation is not
Steam publication. Other biome mods beyond MO need their own runtime profile to
prove actual distribution.
