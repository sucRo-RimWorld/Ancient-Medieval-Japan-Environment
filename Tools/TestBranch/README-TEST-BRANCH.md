# AMJ Environment 0.2.0 — playable test branch (NOT a release)

**Branch:** `test/environment-020-playable-20261019`

This branch starts from the unmerged 0.2.0 release metadata candidate PR #217. Its
runtime assets are the Environment release source plus the **genuine 1.6 net472
DLL** compiled from gameplay source `fe6861ac490bbd43ec73dce8efc1c2a5e5140479`.
The DLL's SHA-256 MUST be
`b2ddbf7888322ba9b207c20296dba31175b846c05492902b900ec225da9fcffe`.

**Check the DLL exists before testing:** `Assemblies/AncientMedievalJapanEnvironment.dll`.
If it does not, do **not** assume that pulling the branch installed the C# patches.
It means the branch materialization did not finish; the expected original native
file can be obtained from Project Actions run #39247372678 and verified using
`python Tools/TestBranch/import_original_dll.py --artifact <downloaded-artifact-zip> --install`.
No Steam API, Workshop publisher credentials or player Mods changes are made by this importer
other than adding the single pinned DLL to the current checkout.

### First-time checkout (Windows)

Clone **inside the RimWorld Mods directory** to a single directory that contains
this branch's `About/` at its root:

```powershell
git clone --branch test/environment-020-playable-20261019 --single-branch https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment.git AncientMedievalJapanEnvironment-Test020
```

If you already have a clean checkout dedicated to this test, switch once:

```powershell
git fetch origin test/environment-020-playable-20261019
git switch test/environment-020-playable-20261019
git pull --ff-only origin test/environment-020-playable-20261019
```

**Do not switch or overwrite your working-development checkout with uncommitted
changes.** Keep this as a separate test directory. Disable all other installed
versions of the *same* packageId, especially Steam Workshop item `3814638060`;
never enable both copies at the same time. Enable **Harmony** first, then AMJ
Environment; optionally enable AMJ Grains to validate poor-soil sowing.

### What is and is not tested

- In original RimWorld **1.6**, check a newly generated TemperateSwamp and ColdBog:
  live Yoshi/Suge/Hannoki/Mizugoke population, real harvest and textures.
- On **Thin Soil (50% fertility)**, paint a Vanilla Growing Zone in an otherwise
  unoccupied, unfogged, unroofed cell. Confirm Sand/water/occupied cells remain
  unavailable. With Grains loaded, confirm a 0.50-minimum crop can actually be
  selected and sown; rice and wheat requiring 0.70 cannot.
- The original game tests recorded 628/628 four-profile wetland *functional*
  assertions; the prior multi-profile run still has strict ERROR0 FAIL from eight
  upstream MO/SYR prelaunch FMOD errors. The Thin Soil zoning interaction is
  **not** yet native-accepted. 1.7 had a distinct source rebuild and a focused
  PASS; the **DLL stored in this branch was built for 1.6**, so compatibility
  with 1.7 from this particular compiled file is not verified.
- **Test only.** Do not merge this binary/test branch into `main` or the
  release PR, do not upload to Steam and do not treat it as a published release.
