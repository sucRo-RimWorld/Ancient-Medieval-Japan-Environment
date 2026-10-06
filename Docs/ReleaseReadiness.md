# Current four-plant release readiness — 2026-10-06 JST

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
