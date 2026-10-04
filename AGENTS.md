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

## 自動テスト優先方針（AMJ共通）

AMJおよび関連Modでは、RimTest Redux・Pickleを積極的に用いた自動テストを優先し、人間による手動テストを最小限にする。

- ロジック・計算・設定検証などはRimTest Redux、ロード後のDef・実際のゲーム内挙動・統合回帰などはPickleを中心に、適した自動テストで確認する。
- 新機能・不具合修正では、再現可能な確認を可能な限り自動化し、リリース前の回帰確認も自動テストへ寄せる。既存のビルド・XML・静的検証は併用する。
- 手動テストは、画像の見た目、UIの読みやすさ、操作感・遊び心地など、人間の目視・操作が必要な項目に限定する。自動で確認済みの数値や挙動を毎回手動で再確認させない。
- 自動化が未整備の項目は、未検証範囲と自動化する対象を明示する。静的検証の成功を実行時テストの成功として扱わない。
- RimWorldを起動する自動テストでは、既存の実行時ERROR検出方針を必ず適用する。シナリオが全件成功しても対象Mod由来のERRORがあれば全体を失敗とする。

## Automated runtime-error gate

For any automated test that launches RimWorld, a passing scenario/test count is not sufficient by itself.

The test harness must capture an isolated runtime log and fail the overall test run if the repository-owned mod emits any ERROR-level entry. Do this even when all Pickle/RimTest scenarios otherwise pass. Warnings remain non-fatal unless a repository-specific test explicitly promotes them.

Any new RimWorld runtime-test harness added to this repository must include this mod-origin ERROR gate from the start. Static-only validation does not fabricate a runtime-log result; add the gate when runtime automation is introduced.

## Historical description audit (AMJ common)

Follow the shared policy in Ancient-Medieval-Japan-Core `Docs/HistoricalDescriptionGuidelines.md` whenever AMJE uses, retextures, selects, patches, or localizes Vanilla / Medieval Overhaul items, plants, animals, or comparable content.

Inherited Vanilla/MO descriptions must be audited from the perspective of ancient/medieval Japan and rewritten when they are anachronistic, culturally mismatched, misleading, overly modern, or otherwise unsuitable. AMJE-authored descriptions should include supported historical facts and, where supportable, a meaningful difference from modern Japan, modern use, or modern distribution.

Historical description text is Japanese-first: draft and review Japanese first, obtain author approval, then translate only the approved Japanese text into English. Preserve research/source rationale in durable documentation.

## Public mod descriptions

Use the CCTO-based shared [mod description guidelines](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Core/blob/main/Docs/ModDescriptionGuidelines.md) when writing or updating public descriptions. Include save compatibility in every mod description, stating addition/removal conditions accurately for the mod's implementation. Keep README, Workshop English/Japanese BBCode, and About.xml consistent; refine the shared baseline as presentation improves.
