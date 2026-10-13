# Chat-side Linux RimWorld public Mod bootstrap (2026-10-13)

## Verified scope

This is a repeatable **dependency acquisition / actual game boot** procedure,
NOT an accepted Hannoki or four-profile native-test result. The author
supplied a legitimate Linux RimWorld 1.6.4871 game archive. The Chat
container cannot directly resolve public hosts via DNS, but a temporary
GitHub Actions runner can download public Mod distributions and the GitHub
connector can return its short-lived build artifact as a real container file.
No Steam username/password, Steam Guard or access to the user's installed
Windows RimWorld is required.

### Verified transfer routes

1. For public GitHub Releases, resolve the exact version, asset name and
   published `sha256:` digest. On a **temporary topic branch only**, have a
   scoped `push` workflow with `permissions: contents: read` download the
   exact HTTPS release file using `curl --fail --location`, verify
   `sha256sum -c`, and `unzip -t`. Upload the verified binary using
   `actions/upload-artifact@v4` with **one-day retention**.
2. Use GitHub Actions workflow-run artifacts to obtain its real
   `download_workflow_artifact` file reference. Verify that exact file
   exists in the Chat container, extract the outer CI artifact and then
   verify the nested original Mod ZIP again. An Actions test report ZIP
   is **not** a Mod binary; inspect its contents.
3. For Mod repositories that track the runtime Mod layout but have no
   binary Release asset (VEF), download an immutable commit-SHA
   GitHub archive with the same temporary runner and verify it includes
   `About/About.xml`, the game's version-specific compiled Assemblies
   and the expected root Package ID. Record the immutable commit and
   downloaded archive checksum.
4. For a **public Steam Workshop item**, a GitHub Actions Linux runner
   can install Valve's unmodified SteamCMD (plus 32-bit runtime dependency)
   and run
   `steamcmd.sh +login anonymous +workshop_download_item 294100 ITEM_ID +quit`.
   For each success, require a real
   `steamapps/workshop/content/294100/ITEM_ID/About/About.xml` and
   `Success. Downloaded item ITEM_ID` evidence. Archive exactly that
   received directory with item ID, observed package ID and tar SHA-256.
   Upload it as a temporary GitHub Actions artifact and extract in the
   isolated Chat game workspace. Do **not** attempt user credentials,
   Workshop ownership bypass, or non-public items; if anonymous access is
   denied, that item remains blocked.
5. Install only inside the isolated Chat workspace `Mods/`. Use
   `About/About.xml` to validate `packageId`, `supportedVersions`
   and `modDependencies`. Link to the isolated game's `Game/Mods`;
   preserve the original archives, installation hashes and per-profile
   `ModsConfig.xml`. Never touch the author's live Steam cache,
   local Mods, configured save or Workshop upload.
6. Verify live RimWorld 1.6 under Xvfb/Mesa from the **real user-provided
   Unity game executable** and record full `Player.log`, bounded startup,
   optional GUI evidence and every `[ERROR]`. A timeout-ended smoke
   process is **not** a passed Quickstart. Never infer generated maps,
   ColdBog natural spawn, harvest or seasonal color from a DLL loading.

### 2026-10-13 acquisition evidence

| Mod | Exact public distribution | Loaded package ID |
|---|---|---|
| Harmony | `pardeike/HarmonyRimWorld` release `v3.0.0.0/HarmonyMod.zip`; published SHA-256 `2e3ae18a2d7b4f42a516023aac742f97874c00ef265f875267135aea1a834437` | `brrainz.harmony` |
| RimWorks Quickstarts | `RimWorks/Rimworld-Quickstarts` release `v4.1.0/Quickstarts-4.1.0.zip`; SHA-256 `3e0abe1511e3bfff18c3639efdcb6e9d68d4fc05272e6de9fec12791ee8b0ce9` | `rimworks.quickstarts` |
| RimLogging | `RimWorks/rimworld-logging-framework` release `v3.1.1/RimLogging-3.1.1.zip`; SHA-256 `46547f9564a6576e9f86619e7ec0cf232d7c8c9c683dc497d77575811312ec2e` | `rimworks.rimlogging` |
| Vanilla Expanded Framework | `Vanilla-Expanded/VanillaExpandedFramework` immutable commit `058d42ce56d5b5c744bd9568db763fb20efe2d34` (official `080926` tag). Source snapshot ZIP SHA-256 `6ebf932d8f8add2ef224a9baf2d822362f342df91170dc69d9d5fc056bfc96a3`. Contains `1.6/Assemblies/VEF.dll`. | `oskarpotocki.vanillafactionsexpanded.core` |
| Processor Framework (Continued) | Public Workshop `3210544395`, anonymous SteamCMD, actual 3,110,398-byte download | `syrchalis.processor.framework` |
| AMJ Environment (published build) | Public Workshop `3814638060`, anonymous SteamCMD | `sucro.ancientmedievaljapan.environment` |
| CCTO (published build) | Public Workshop `3812412548`, anonymous SteamCMD | `sucro.cropcoldtoleranceoverhaul` |

The user-uploaded MO 1.6.2.2 and MO Japanese translation were already
present and are **not** counted among the seven downloaded public Mods.
Quickstarts 4.1.0 specifically requires RimLogging, and its live patch
backend used the above Harmony. The AMJE disposable Quicktests
package ID `sucro.ancientmedievaljapan.environment.quicktests` is
**not** `rimworks.quickstarts`: both are required for actual custom
Hannoki native Quickstarts, but only the latter is publicly distributed.

### Observed native-game results and outstanding blockers

The original isolated Vanilla Xvfb main-menu screenshot confirmed real
RimWorld 1.6.4871 under Mesa llvmpipe. Subsequent bounded ~95-second
live startup probes with the public Mod payloads observed:

| Profile | Verified dynamic behavior | Native test status |
|---|---|---|
| Harmony + RimLogging + Quickstarts | RimLogging initialized; Quickstarts patched via Harmony; zero `[ERROR]` | Framework startup only |
| AMJE + CCTO + frameworks | AMJE production DLL loaded, Harmony patches installed; zero `[ERROR]` | Loaded-build smoke only |
| MO + VEF + Processor + AMJE + CCTO + frameworks | AMJE assembly loaded, Quickstarts initialized; **2 Unity `[ERROR]` events** about FMOD sound creation with unsupported/empty clip | Strict ERROR-free startup **FAIL**; investigation open |

- The above two new profiles exited after the deliberate 95-second
  limit (return code 124). That is not a natural game Quickstart PASS.
  Steam API warnings and other audio-device initialization warnings are
  separately identifiable from the two Unity `[ERROR]` FMOD events.
- The **Workshop AMJE `AMJ_WildPlants.xml` does not contain**
  `AMJ_Tree_Hannoki`, even though the current GitHub main does. The
  public Workshop smoke therefore **cannot** validate the newer
  Hannoki production or its author-approved source/master sprites.
- Compiled AMJE-specific `Quicktests.dll` and a pinned latest-source
  AMJE runtime DLL are not supplied by these public payloads; source
  runtime-build/test remains outstanding, as do real two-biome natural
  generation, 24 WoodLog cut evidence, 0/5℃ CCTO winter behavior,
  0/0.5/1 snow overlay, rendered autumn appearance and old saves.
- Containers and one-day transfer artifacts are temporary. Retain
  commands, immutable version/commit identifiers, SHA checks and test
  premises, **not** a claim that a later Chat session still owns the
  installed files.
- For the full Windows native acceptance suite, the owning validated
  entry point remains `run-tests.bat` with isolated desktop and the
  exact current candidate payload; published Workshop results do not
  substitute for that suite.
