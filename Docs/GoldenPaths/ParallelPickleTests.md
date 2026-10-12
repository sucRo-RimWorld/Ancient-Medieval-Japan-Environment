# Opt-in parallel development Pickle tests

Use the existing development Pickle runner with `--workers 2` for a first two-game comparison:

```powershell
py -3 Scripts/Run-DevelopmentPickle.py --game "D:\SteamLibrary\steamapps\common\RimWorld" --workers 2
```

The default `--workers 1` retains sequential profile execution. `--workers 4` allows four profiles in flight, subject to available memory and stable startup. This is an optional development loaded-Def test, **not** a replacement for `run-tests.bat`, native rendered Quickstarts, candidate release tests, or downloaded Workshop tests.

The runner checks that no RimWorld process is already running, stages its shared test-only fixtures and support binaries **once**, and then runs the Vanilla, MO, CCTO, and MO-CCTO profiles in separate isolated desktops. Profile paths are distinct: `SaveData-<profile>`, `<profile>-Player.log`, `Pickle-<profile>`, `Unity-<profile>` and `<profile>-Runner.log`. It validates each profile's complete scenarios, native log and Unity ERROR capture before claiming overall PASS. It waits for all worker futures before moving shared fixtures to `Retired`; a failed profile makes the overall command fail.

Initial acceptance: compare `--workers 1` and `--workers 2` on the same built source, with no other game running and enough RAM for two RimWorld processes. Ensure all four profile reports exist, all report expected source/profile, zero runtime errors, and no normal ModsConfig/Prefs changes. Only then trial four workers. An ordinary windowed two-process launch establishes multi-instance capability, not automated parallel Pickle PASS.

Do not run this runner concurrently with another AMJE test harness sharing the same fixture names, and do not run source/build or Mod updates while it is active. Review the per-profile logs rather than interpreting an exit code alone.
