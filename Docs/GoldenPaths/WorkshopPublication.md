# Pinned Workshop publication

The author performs Steam upload manually. A GitHub commit, locally built
candidate, or passing developer suite is not evidence of updated distribution.

## 2026-10-07 publication-path audit

The installed manifest8630945342812668549 contains1113 files. Every one has
the same SHA256 as the same relative path under the dirty production development
root `Mods/AncientMedievalJapanEnvironment` (HEAD552ce124, many local edits).
Root plant XML SHA256 is
`8cf2c9864b8b5a06196126a30928e0ac6e933d4c8a9d319782327f534c4828a4`;
it lacks Haimatsu harvest fields. Shipped
`TestResults/SourceSync/Defs/ThingDefs_Plants/AMJ_WildPlants.xml` has SHA256
`537b5b99f0b005c14dfb8f014e03c3ce5438ad4014f0d3a1012a797b43a0c323`,
equal to the fixed current-main XML. The fix was present inside the uploaded
development tree but outside the root loaded by the game.

Steam `workshop_log.txt` records successful uploads on2026-10-06 at23:01:36
(manifest8889000939694660295) and23:26:10 (manifest8630945342812668549),
and the second upload's preview source under the development root's About.
These are Steam log timestamps, not converted from the execution host clock.
The fix merge d38e285 was committed at22:00:57+09:00 before both uploads.

The installed YADA2971543841 source patches ModMetaData.PrepareForWorkshopUpload
and GetWorkshopUploadDirectory: it copies the selected RootDir through Scanner
to a name-plus-random temporary directory, then returns that directory only
after successful copying. Scanner inherits basename exclusions recursively;
it neither fetches Git main nor promotes nested XML into the selected root.
The actual local .rimignore contains YADA's generic defaults and excludes
neither Art nor TestResults. All installed files pass those current rules;
37 Art files and988 TestResults files shipped. Main's Art-only exclusion was
also insufficient for all developer output, and was not the local rule used.

Git still registers `_AMJ_PublishStaging/Environment` at76cd4d4, but its path
does not exist. This stale registration is not proof of upload from that path.
No retained YADA temporary upload directory proves which uploader branch ran;
the exact file identity and preview path establish the development-root content
regardless of whether YADA or Vanilla performed the final copy. Do not claim
YADA silently removed harvest fields, a failed Steam upload, or an uploaded
old staging checkout. The confirmed root cause is publication of unsynchronized
development-root content instead of the fixed formal-main payload, combined
with no provenance/exact-payload gate and ineffective development exclusions.

Full read-only comparison is retained locally in
`Mods/AncientMedievalJapanEnvironment/TestResults/WorkshopReleaseRepair/Audit.json`.
Original runtime evidence remains in the location listed by WorkshopRuntimeTests.

## Reusable sequence

1. Read main AGENTS/Coordination and WorkshopRuntimeTests/PlantHarvestTests.
   Start from a fresh main clone outside immediate Mod enumeration. Never reset
   a dirty author checkout, copy its whole directory, or upload a nested worktree.
2. Pin the full expected main SHA. Run build/static/art validators as required.
   Run `python Tests/test_workshop_payload.py` (negative publication fixtures).
3. Run `python Scripts/Build-WorkshopPayload.py build --expected-commit FULL_SHA
   --output FRESH_EXTERNAL_RESULT_DIRECTORY --preview APPROVED_EXISTING_PNG`.
   The preview's SHA256 is recorded as a separate author-approved input because
   the current main does not track it. The builder rejects tracked edits,
   untracked runtime assets and wrong HEAD, builds the production DLL afresh,
   uses only tracked runtime inputs, preserves Workshop/package identity,
   validates Haimatsu8/WoodLog/Wood and plant paths, and emits a root-only ZIP,
   extracted root and per-file Manifest.json. It does not publish.
4. Run `python Scripts/Run-WorkshopPayloadTests.py --payload EXTRACTED_ROOT
   --manifest Manifest.json --output FRESH_RESULT_DIRECTORY` as the normal user
   in Steam's session. It uses the existing hidden rendered desktop and current
   independent regression observers. Require six map checks plus native cutting
   for all four profiles, complete logs, zero errors and unchanged source payload.
   Candidate tests use a unique fixture ID in About only; all production runtime
   files remain exact. This harness identity difference must be reported.
5. Immediately before the author's manual upload, verify the exact selected root:
   `python Scripts/Build-WorkshopPayload.py verify SELECTED_ROOT Manifest.json`.
   Keep manifest outside the upload root. Use the extracted runtime root, with
   original packageId/PublishedFileId, as the author's selected upload source.
   If a copy is made for Mod discovery, verify that copy too. Never overlay it
   into the development or installed Workshop root. Retire duplicate temporary
   discovery copies after author upload. YADA rules are defensive, not provenance.
6. After author upload, wait for Steam to download a changed installed/latest
   manifest. Record Steam log/ACF and all file hashes; compare the downloaded
   runtime root to the candidate manifest. Do not write into Workshop to fake
   a download. Run Run-WorkshopPayloadTests against that root; the runner keeps
   actual `_steam` identity and the source/DLL observer must prove it loaded.
7. Record candidate and actual-distribution results separately in main
   Coordination. Only a real downloaded-payload PASS clears release HOLD.
   Preserve full logs and configs before retiring temporary observers.

The source observer defaults to the actual Workshop path; an explicit expected
root supports candidate validation without asserting that a candidate is Steam.
Existing game behavior is unchanged: the accepted main Haimatsu fix is reused.
