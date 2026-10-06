# Publication audit — 2026-10-06 JST

Author authorized publication only if the PR/branch/work-in-progress audit finds
no remaining issue. That condition is not yet met. No publication was performed.

## GitHub inventory verified this date

- PR #5 is the only open PR: art-visual-release-20261006, draft, mergeable,
  head 74f285bf344288767f6fa8ec204ceadbf4a358f6. Both visual-ledger and
  PowerShell workflows completed successfully; no review submissions exist.
- PRs #1–#4 are merged; their remaining fix/feature branches are historical,
  not evidence of ongoing feature implementation.
- docs/golden-path-art-assets has two unmerged commits, ahead 2 / behind 38
  relative to main 911fb41337fc23a29c2d1f27dc4e0b1291defce6. Changes are
  ArtAsset.md and the focused biome selector. Current TextureAssetPipeline and
  run-texture-debug already provide the corresponding procedure/selector;
  review for any unique requirements before retiring this branch. Do not merge
  its older runner over the newer one blindly.

## Open publication preparation

1. Integrate accepted art PR, retain other workstreams, update authoritative
   main handoff, then validate the final integrated distribution payload.
2. Beta positioning is prepared consistently in README, About and both Workshop
   texts. Validate-Environment.ps1 currently requires exact Alpha markers;
   that gate is updated on the temporary PR branch; syntax CI must pass again.
3. The four owned plant descriptions were approved by the author on 2026-10-06
   and integrated in Japanese and English. Sources/approval are retained in
   PlantDescriptionReview-ja.md. This closes that four-species slice, not the
   broader inherited Vanilla/MO plant/animal description audit.
4. About currently has only About.xml: no Workshop Preview.png is present.
   Finish publication presentation and verify actual upload method/destination.

The current four-plant visual ledger is complete. Existing Vanilla/MO tree
retextures are deferred to post-release updates. The combined AMJ basic-loop
gate is a stable-release condition, not a newly invented Beta blocker.
Existing exact-package 297 checks remain valid within their original scope;
they are not the final integrated source branch's runtime result.

Additional current-source harvest evidence: standalone and actual MO 9/9 each,
zero global/independent/owned errors; four native cutting outputs verified.
See Docs/ValidationEvidence/HarvestOutput.json. The shared test-profile writer
was corrected to lowercase PackageIds; mixed-case IDs previously made MO's
active metadata lookup fail. No MO or normal-user settings were modified.
