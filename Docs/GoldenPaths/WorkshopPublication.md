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
containing the same six harvest fields as current main. Current main has since
changed plant labels/descriptions; its checkout XML hash is
`6d66a15cffbdd970dc985083fc2bef62a45766203e6d1c3e521370c1e915041a`.
After removing only label/description nodes, the two parsed Def trees are equal.
Do not confuse matching cutting behavior with byte-identical entire XML.
The fix was present inside the uploaded development tree but outside the root
loaded by the game.
The retained local `TestResults/integrate_approved_descriptions.py` and
`TestResults/prepare_beta.py` both explicitly set their root to
`Path(__file__).resolve().parent / 'SourceSync'`; the former proves/tests the
six Haimatsu harvest-field additions there. Those procedures updated the
publication PR worktree, not the top-level development Mod. Root About still
says Alpha while SourceSync/main says Beta. This accounts for both mismatches
without attributing them to a Steam download failure or XML patch collision.

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
Reproduce the same-path and actual local YADA-rule comparison with
`python Scripts/Audit-WorkshopUploadSource.py --workshop INSTALLED_ROOT
--development DEVELOPMENT_ROOT --output FRESH_EVIDENCE_JSON`.
Original runtime evidence remains in the location listed by WorkshopRuntimeTests.

## Steam Change Notes via Add Changenote

AMJE keeps Steam Workshop change-note text in Git rather than composing it
manually in Steam.

- `About/Manifest.xml` is the publication-version source used by Add Changenote.
- `About/About.xml` `<modVersion>` must match the Manifest version.
- `About/Changelog.txt` contains versioned Workshop change-note blocks. The
  current block must begin with exactly the current Manifest version.
- `About/Manifest.xml` and `About/Changelog.txt` are intentional subscriber
  files. Do not add either basename to `.rimignore`; YADA's temporary upload
  copy must retain both so Add Changenote can read them.
- During the author's manual RimWorld Workshop upload, both **YADA** and
  **Add Changenote** must be enabled. YADA remains responsible for the filtered
  upload root; Add Changenote intercepts RimWorld's auto-generated change note
  and replaces it with the current changelog block.
- Before opening the upload confirmation, update the version and changelog in
  Git, build from the pinned clean commit, and require the payload validators to
  pass. If either publisher helper is disabled, or if the matching changelog
  block is missing, do not claim that the intended Steam Change Note was
  published.
- After Steam installs the new manifest, confirm the Workshop Change Notes page
  shows the intended current-version block as part of the normal post-upload
  verification. Repository preparation alone is not Steam publication proof.

The first tracked AMJE publication version is `0.1.0`. Earlier Workshop
uploads were not assigned retroactive versions.

## Reusable sequence

1. Read main AGENTS/Coordination and WorkshopRuntimeTests/PlantHarvestTests.
   Start from a fresh main clone outside immediate Mod enumeration. Never reset
   a dirty author checkout, copy its whole directory, or upload a nested worktree.
2. Pin the full expected main SHA. Confirm `About/About.xml` modVersion,
   `About/Manifest.xml` version and the leading version in the current
   `About/Changelog.txt` block all match. Run build/static/art validators as
   required. Run `python Tests/test_workshop_payload.py` (negative publication
   fixtures).
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
5. Immediately before the author's manual upload, enable YADA and Add Changenote,
   then verify the exact selected root:
   `python Scripts/Build-WorkshopPayload.py verify SELECTED_ROOT Manifest.json`.
   Keep manifest outside the upload root. Use the extracted runtime root, with
   original packageId/PublishedFileId, as the author's selected upload source.
   The subscriber ZIP excludes README, documentation and .rimignore itself;
   filtering follows the shared Core WorkshopPackaging contract. Run
   `python Tests/validate_workshop_payload.py --payload SELECTED_ROOT
   --expected-assembly AncientMedievalJapanEnvironment.dll` too.
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

The independent Unity capture Mod loads immediately after Vanilla Core; the
Quickstarts-dependent source/regression observer loads after Quickstarts and
the selected production Mod. Combining both into an early-loaded DLL caused
ReflectionTypeLoadException during the first candidate run. Keep their assembly
dependencies separate and retain every per-process Unity error log; overwriting
one shared error file across six games would lose earlier startup errors.

If the installed Quickstarts throws `Collection was modified` inside its
`LogCapture.CountErrors/Arm` during startup, retain that failed directory and
its independent Unity ERROR. Do not suppress the exception, edit Workshop
dependencies, or count a partially completed profile as PASS. A focused rerun
uses `--profiles MO CCTO MO-CCTO` (or another explicit subset) with a fresh
output root and the unchanged payload. Its summary explicitly says it is not
by itself a complete four-profile gate. Combine only individually completed,
validated profiles from the identical payload; preserve the failed attempt.
`Scripts/Combine-WorkshopPayloadResults.py --payload ROOT --manifest MANIFEST
--results RESULT_DIRECTORY... --output FRESH_REPORT_JSON` rechecks all saved
reports/full logs, exact retired runtime bytes, real Workshop dependency roots,
Direct3D and independent error captures before accepting the four-profile union.
Use `--steam` only with the actual installed Workshop root. Candidate union
success explicitly does not clear the distributed-release HOLD.

## Historical 28-file candidate (superseded by subscriber-only policy)

Immutable candidate source commit: `f404769a3c70d44a0471576ff70e95ba6ff04d08`.
Archive SHA256: `1e81e525da09680c3592c05d89a2cbd332127d9319ec72c982d959f3eb4d11d4`.
The payload has28 files, original packageId/Workshop ID, current accepted main
runtime content and author-approved preview. Subsequent tooling/docs-only
commits do not change this tested artifact; do not rebuild and substitute a
new DLL hash without revalidation.

| Profile | Six map/plant/terrain checks | Native cutting | Runtime ERROR |
| --- | --- | --- | --- |
| Vanilla Core + candidate |227/227|9/9|0|
| Core + real MO + candidate |227/227|9/9|0|
| Core + real CCTO + candidate |275/275|9/9|0|
| Core + real MO + CCTO + candidate |275/275|9/9|0|

All28 completed reports were rechecked with the saved-log combination gate:
exact profile membership and source/DLL/Def ownership, real dependency roots,
Direct3D11, no truncation/pre-launch/global ERROR, seven independent Unity logs
per profile, four native outputs42/40/30/8, and exact retired runtime bytes.
Only temporary fixture About identity differed; original candidate, normal
ModsConfig/Prefs and Workshop payload were preserved. Failed attempts remain
in CandidateRuntime-1/2; the corrected observer and unchanged candidate rerun
are recorded separately. This is a candidate PASS, not a Steam PASS.

Local artifact/evidence root:
`Mods/AncientMedievalJapanEnvironment/TestResults/WorkshopReleaseRepair`:
Candidate-f404769, CandidateRuntimeGate.json, UploadSourceAudit.json and
CandidateRuntime-1/2/3. Formal compact evidence and hashes are retained in
`Docs/ValidationEvidence/WorkshopPublicationAudit.json`,
`WorkshopCandidateRuntime.json` and `WorkshopCandidateManifest.json`.

The28-file candidate is historical runtime evidence and must not be uploaded:
README/.rimignore fail the newer shared subscriber contract. The final26-file
Candidate-a23a9eb uses sourcea23a9eb2b6cac3afd3b876860ae1654327a90370 and
archive15af3f24ced8988859db0b811ef58348b9b6a05ea77cbf93757176f023be4a99.
Its shared filtering and exact four-profile runtime gates passed separately at
SubscriberCandidateRuntime-1. Distributed approval remains HOLD on manifest8630945342812668549
until a real Steam download matches the intended payload and passes the actual
four-profile source/cutting gate. No Workshop file overwrite or upload occurred.

## Final subscriber-only candidate (2026-10-07 JST)

The26-file Candidate-a23a9eb is the author-upload handoff. Its original
packageId and Workshop3814638060 identity are preserved. Source commit
`a23a9eb2b6cac3afd3b876860ae1654327a90370` is retained in merged PR6;
production inputs are unchanged on subsequent main documentation commits.
Archive SHA256: `15af3f24ced8988859db0b811ef58348b9b6a05ea77cbf93757176f023be4a99`.

The actual26-file stage passed the shared subscriber validator with expected
production DLL. Its freshly built DLL and exact stage were then independently
rerun in all four profiles: Vanilla227/227, MO227/227, CCTO275/275,
MO+CCTO275/275; cutting9/9 each, Haimatsu output8. All28 reports were rechecked
with Combine-WorkshopPayloadResults: actual source/DLL/Def ownership, real
dependency roots, Direct3D11, complete capture, zero pre-launch/global/Unity
ERROR, exact retired bytes and normal config preservation. Only temporary
fixture About name/packageId changed; the publication artifact retains the
original identity. No failure occurred in this final26-file matrix.

Exact archive/manifest: TestResults/WorkshopReleaseRepair/Candidate-a23a9eb.
Full evidence: SubscriberCandidateRuntime-1 and SubscriberCandidateRuntimeGate.json.
Active formal evidence: Docs/ValidationEvidence/WorkshopCandidateManifest.json
and WorkshopCandidateRuntime.json. The28-file artifacts remain historical.
Final preservation proof verifies all1113 actual Workshop file hashes and
installed/latest manifest8630945342812668549 are unchanged.

**Publication decision:** author-manual upload may proceed with this verified
26-file root after immediately rechecking its manifest. Actual distributed
release remains HOLD; no Steam repair is claimed. Once Steam downloads a new
payload, audit that actual root and rerun all four profiles including cutting.
