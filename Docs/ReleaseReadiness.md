# Current four-plant release readiness — 2026-10-06 JST

# 2026-10-15 current-main Linux 1.6 candidate — build and Vanilla native PASS


## Author-approved release classification — external MO/SYR FMOD (2026-10-15)

Environment does **not** add or alter sounds, sound Defs or FMOD assets.
The exact repeated `Cannot create FMOD::Sound instance for clip ""`
unsupported-format errors in the genuine MO/SYR provider stack occur in
control tests *without* Environment. They are owned by the Project native/FMOD
workstream, not an Environment crop/vegetation/world-generation defect.

For an **Environment-only, private Workshop candidate** decision, an unchanged,
independently established pair of these external FMOD errors in MO/MO+CCTO
may be **classified as non-blocking for this Mod's functional release**
only when all of these conditions are demonstrated on the **same exact
candidate**:

1. Actual game process exit 0, non-truncated/complete native observer
   reports, source/ZIP/DLL identity, and original assertions for the feature
   being tested all pass; no timeout, early crash, missing report or hidden
   failed assertion.
2. Every recorded Unity ERROR in the scoped MO run is exactly the established
   blank-clip unsupported-format FMOD message; no changed error text,
   unexpected third-party error, new runtime error or other startup fault.
   Retain **every** raw Unity ERROR and count. Do not suppress logs, patch
   MO/SYR sounds or relabel such a run global **ERROR0 PASS**.
3. Vanilla and CCTO remain ERROR0; MO and MO+CCTO keep their independent
   actual functional/biome/harvest evidence, with both **scoped functional**
   and **strict ERROR0** statuses recorded separately.
4. This exception does **not** waive verified physical Workshop upload
   source, current reviewed preview, downloaded subscriber byte identity,
   new Environment-caused exceptions or user-private visibility requirements.
   Any new Def-breaking change gets its own targeted compatibility check.

Thus the original global strict four-profile zero-ERROR suite continues to
report **FAIL** while known MO/SYR FMOD persists, but this specific external
audio issue alone no longer keeps an otherwise complete **Environment-only**
private candidate on HOLD. The Project FMOD owner still investigates it.
This is **not** a project-wide error waiver or a statement of compatibility
with untested combinations. Stable new-save play-loop/season/save-reload
criteria remain separately tracked, not silently satisfied.


**Actual build and exact payload validation PASS:** Project isolated runner
[Actions #38681874743](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38681874743)
compiled pinned Environment `4dacbcf0fbe3399ee44795f3097b8cbe498d0ae3`
against real Linux 1.6 managed assemblies, then validated a **59-file** payload
and ZIP. DLL SHA256 `29c41ebf8998db993ac496d7d5800b9ae39fec613490dee8da6a721cad702996`,
archive SHA256 `4b7707284f1b5a9d404962d21869e5914ea70aeb842951d0d45c395678e4b7c3`.
The canonical root-only `loadFolders.xml` and source/static checks PASS.
[Exact evidence and unfulfilled gates](ValidationEvidence/EnvironmentCandidateLinux16-20261015.md).

**Additional same-candidate Vanilla native PASS:** 9 original map Quickstarts, **466/466**, nine game exits 0, Unity ERROR 0, split over [base #38686699729](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38686699729) and [remaining #38686772083](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38686772083). Source/ZIP/DLL identity verified; see exact evidence linked above.\n\n**Subsequent exact-candidate actual cutting evidence (2026-10-15):** [Vanilla and CCTO five-tree native CutPlant #38689421756](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38689421756) both **17/17, ERROR0, PASS**, with source-matched WoodLog yields 42/40/30/8/24. MO and MO+CCTO five-tree RawWood yields also match, but [MO #38689626203](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38689626203) and [MO+CCTO #38690112481](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38690112481) each log two original-MO FMOD ERRORs, so **strict FAIL**. The earlier MO+CCTO attempt [#38689992750](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38689992750) exited 139 before testing and remains separate FAIL. [Detailed tested premises and evidence](ValidationEvidence/EnvironmentCandidateLinux16-20261015.md).

**Four-provider biome-replacement functional gate CLOSED:** Original unforced-world 24/24 assertions in each Vanilla, CCTO, MO, MO+CCTO profile against the **identical 59-file candidate**, with actual game exit0 and complete live captures. Strict ERROR0 succeeded in Vanilla/CCTO; MO/MO+CCTO each retain exactly two independently known blank-clip FMOD errors, so their strict global test remains FAIL but the owner-scoped functional release classification is PASS. [Verified run matrix and failed initial harness attempts](ValidationEvidence/EnvironmentCandidateLinux16-20261015.md).

**Still HOLD for actual private Workshop publication:** accepted preview/source match, necessary season/visual-state criteria, Environment-owned GitHub Actions publisher and authenticated private Steam state, exact physical selected upload source, downloaded subscriber bytes/identity and runtime smoke. **Known external MO FMOD2 alone is no longer an Environment-specific private-release blocker under the scoped contract above.**
Do not promote Vanilla-only native evidence into a complete four-profile functional 1.6/1.7 or Steam PASS.
The Linux test host's staged Environment game Mod is older, not this candidate.


## Highest-priority biome replacement repair — 2026-10-12 JST

The author reports tropical rainforest/swamp in the installed Workshop build.
Source candidate selection also admitted inappropriate Vanilla biomes, and the
former natural-world test did not reject them. This is a foundational defect;
old candidate/runtime PASS below does not certify biome replacement.
`Docs/Design.md` §9.2.1 owns the corrected full base-surface inventory and scope.
Source now filters ten replaced Vanilla identities before scoring and the
existing unforced world test has 24 assertions, including zero excluded tiles,
four AMJ bands, both retained wetlands and real MO coexistence. Build a fresh
production DLL/candidate and obtain the same-source Vanilla/MO/CCTO/MO+CCTO
natural-world functional evidence with native ERROR audit and the scoped external-FMOD classification above before clearing this repair. Native build/runtime
and corrected Steam upload/download are still OPEN. Prioritize this before
wetland species/art additions; Future upload must follow the author-approved GitHub Actions direction (not historical YADA), with owner-scoped private-only publisher credentials and exact downloaded-subscriber validation; no working Environment publisher has yet been demonstrated.

## Current candidate status (2026-10-08 JST)

A 32-file pinned candidate from `1d46727102f53243099927b59881091d448dba61`
passed the author's corrected full nine-map/one-cutting four-profile runtime
suite (Vanilla/MO 409+15, CCTO/MO+CCTO 457+15, zero runtime ERROR,
ten reports per profile, unchanged payload and normal config).
Test-only harness revision `f39914d5bbd02edb90f7c91fe5faaf6e645d86fd`
did not rebuild candidate bytes. This is **candidate runtime PASS** only. The author subsequently passed
exact 32-file `Build-WorkshopPayload.py verify` and the independent
`Combine-WorkshopPayloadResults.py` saved-log audit (40 reports).
The supplied `FourProfileAudit.json` has `passed=true` and
`steam_release_cleared=false`. Actual Steam upload, downloaded-payload
source provenance and runtime checks remain unverified, as do Stable
combined-loop/season/save criteria. Older evidence is historical.


This specific forward-only biome-generation repair does not require comprehensive **legacy save migration** testing: existing worlds are not rewritten, and this candidate does not remove or rename Defs. Check an old-save load smoke only if a future change may break referenced Def identity. This does not waive the separate new-save save/reload continuity in the stable combined-loop criterion.

Stable release additionally requires the AMJ combined basic-loop gate defined in
Core `Docs/ReleaseCriteria.md` (author decision, 2026-10-06 JST). Actual pawn work
with Core + Environment + real MO, recurring food production, a seasonal cycle
and save/reload continuity remain OPEN. The results below do not satisfy that
gate. Beta feedback is useful but its absence is not evidence of success.

The subsequent four-species cutting-resource gate passed in standalone and
actual MO profiles: 9/9 each, all runtime ERROR gates clean, exact outputs
42/40/30/8 of WoodLog or DankPyon_RawWood respectively at yield factor1.
Evidence: Docs/ValidationEvidence/HarvestOutput.json and native log excerpts.
The staged payload hashes were unchanged after execution; fixtures were retired.
This covers native cutting jobs and output, not autonomous work selection or
the combined AMJ survival loop. The original ZIP below predates these changes.

All four current AMJE plants have accepted normal/growth, UI and snow appearances.
Beech leafless/autumn approvals are retained; its automatic seasonal transition is
verified by bounded native calendar sampling, separately from human acceptance.
The strict visual coverage ledger passes with zero pending states.
Existing Vanilla/MO tree retextures are deferred to post-release updates.

The unchanged runtime-only candidate archive has SHA256
`d1ecd33a0c18cb2abe665685c9487a60bc257346000bb1faa3d528287109ce0d`.
Standalone boot/graphic-load smoke passed. Four biome maps, river/coast handoff
and optional CCTO checks passed: 58/57/54/52/3/3 and70 assertions, 297 total.
All seven reports had complete live capture, zero pre-launch/owned/global ERRORs,
no truncation and the correct extracted candidate root. Test-only Quickstarts
were loaded from a separate observer Mod; the ZIP excludes development code.
Normal saves/settings were preserved; temporary source metadata was restored.
See retained ReleaseRuntime/Report.json and ReleaseMatrix/Report.json.

These results cover the approved local release payload. This source PR also
preserves subsequent upstream testing/tooling additions; it is not a claim that
the integrated branch, full MO runtime or long-term gameplay has already passed
those same runtime gates. Build/static and Python checks are rerun on this branch.
No Workshop publication or GitHub release upload has been performed.

main:Docs/Coordination.md remains the authoritative handoff. Its current main
entry predates the local art closeout; update it on main when integrating this PR.
No branch-specific Coordination update is included.

Integration fix: the static validator now checks the current Owned AMJ runtime
ERROR marker introduced upstream, preserving the runtime gate itself unchanged.
The PowerShell change remains in this temporary PR branch and requires CI before
merge, per AGENTS.
