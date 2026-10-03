# AMJ Environment Agent Instructions

This repository is part of the **Ancient & Medieval Japan (AMJ)** project.

Before starting work in this repository:

1. Read this file.
2. Read the authoritative coordination log at `main:Docs/Coordination.md`.
3. Check for OPEN / IN PROGRESS items owned by the current workstream before starting new work.

## Cross-chat / cross-agent coordination

Do not use the user as a messenger between chats, agents, repositories, or workstreams.

When another workstream or AMJ repository needs to be consulted, record the request and relevant context in the authoritative `main:Docs/Coordination.md` of the repository that owns the requested work.

## Source-of-truth rule

`Docs/Coordination.md` is only for handoff, status, blockers, and cross-workstream notes.

Durable decisions must also be reflected in the appropriate source of truth, such as:

- `Docs/Design.md`;
- XML/Defs/Patches;
- localization files;
- C# code where XML/Defs cannot robustly express the intended behavior.

Do not treat a coordination note as the final specification.

## Branch rule

The authoritative coordination log exists only on `main`.

Do not create branch-specific copies of `Docs/Coordination.md`.

## Consistency rule

When a design or implementation policy changes, check the existing design, implementation, related systems, compatibility patches, balance values, documentation, and tests for consistency before applying the change.

## Implementation preference

Prefer RimWorld Def/XML/Patch operations when they are sufficient. Add C# only for world-generation or runtime behavior that cannot be expressed cleanly and compatibly through Defs/XML.

## Reporting GitHub changes

Only report that a GitHub file was updated when the change was actually committed to GitHub. When reporting repository changes, include the actual commit SHA.
