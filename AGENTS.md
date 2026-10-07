# AMJ Environment Agent Instructions

This repository is part of the **Ancient & Medieval Japan (AMJ)** project.

Before starting work in this repository:

1. Read this file.
2. Read the authoritative coordination log at `main:Docs/Coordination.md`.
3. Check for OPEN / IN PROGRESS items owned by the current workstream before starting new work.
4. For plant/tree art, follow the art routing section below.
5. Before resuming plant art, selecting a next target, handing off or claiming completion, read `Docs/GoldenPaths/PlantVisualCoverage.md` and run `Tests/validate_plant_visual_coverage.py`. Resolve/report its pending states first. A whole-plant/all-state completion claim requires `--require-complete` to pass; normal appearance approval alone is never full completion. Existing-tree retextures are deferred by the author; finish pending current-plant states before proposing unrelated art.
6. Color/palette comparisons must use fixed local time 12:00, Clear weather and a paused map, with the same zoom and season/shader intensity for before/after comparisons. Verify and record actual conditions; requested settings alone are insufficient. Do not judge source colors from uncontrolled dawn/dusk, night or weather screenshots. Follow the color-review controls in `Docs/GoldenPaths/PlantVisualCoverage.md`.

## VE-first overlap audit

Before designing a new substantial AMJ feature or proposing a separate Mod, audit the Vanilla Expanded (VE) family first for functional overlap and prior art, using `sucRo-RimWorld/Ancient-Medieval-Japan-Project/Docs/Research/ExistingModAudit.md` as the canonical criteria. VE is a comparison priority, not the AMJ design baseline or an automatic dependency: evaluate historical/cultural fit, dependency footprint, unrelated attached content, retention ratio and reuse value before choosing use-as-is, optional compatibility, patch/retexture, prior-art-only, or AMJ implementation.

## Unowned idea staging

When a new AMJ idea may become a separate Mod but does not yet have an owning repository, **record its durable concept, research and roadmap state in `sucRo-RimWorld/Ancient-Medieval-Japan-Project`**. Do not let this runtime repository become the evolving design home merely because the idea was discovered here. Keep only a concise compatibility or ownership-boundary pointer when relevant. Once a dedicated owner repository exists, migrate confirmed design there.

## Context reconstruction / source hierarchy

For every new chat or agent session working on AMJ or a related mod, rebuild context from repository sources instead of treating accumulated chat history as the primary source of truth:

1. Read this repository's `AGENTS.md` first.
2. Read the authoritative `main:Docs/Coordination.md`.
3. Read the relevant authoritative design, code, XML/Defs/Patches, localization, Golden Path, or other repository-owned source-of-truth files for the task.
4. Use prior chat history or memory only as supplementary context. If it conflicts with repository sources, the repository sources win.

Store information according to this hierarchy:

- Confirmed specifications, design decisions, accepted values, and implementation facts -> the appropriate formal repository source of truth.
- Cross-chat / cross-agent / cross-workstream handoff, current status, blockers, and requests -> `main:Docs/Coordination.md`.
- Permanent operating rules that should govern future work -> `AGENTS.md`.
- Do not leave a durable decision only in chat history or only in `Docs/Coordination.md`.

## Unowned AMJ idea staging

When work in this repository discovers an AMJ idea that may become a separate Mod but does not yet have an owning repository, **do not develop its evolving design here**. Record the concept, research and roadmap state in `sucRo-RimWorld/Ancient-Medieval-Japan-Project` until the author creates/selects an owner repository. Keep only a concise compatibility or ownership-boundary pointer here when it materially affects this repository.

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

## 非対話ランタイムテスト方針（AMJ共通）

人間の目視判断を必要としない自動テストでは、RimWorldの可視ウィンドウをユーザーのデスクトップへ出さないことを標準とする。詳細な共通正本は Ancient-Medieval-Japan-Core `Docs/DevelopmentGoldenPathGuidelines.md` の **Non-interactive runtime-test rule**。

- Pickle / RimTest Redux / Quickstarts / 統合回帰 / runtime ERROR gate / map・気候・土壌サンプリング等は、原則として非対話・非表示で実行する。
- 描画・Texture Atlas・`Graphic.Draw`・BadTex等を検証する場合は、描画そのものを無効化しない。仮想／オフスクリーン／非表示の表示先など、プラットフォームに適した隔離実行で実描画経路を維持する。
- 描画経路がテスト対象なら `-nographics` 等で迂回しない。
- 可視実行は、最終的なテクスチャ見た目、通常ズーム視認性、UI、操作感、遊び心地など、人間の判断が必要な確認だけに限定する。
- 通常の自動ランナーと、visual / interactive / debug用途の可視ランナーを分離する。
- 既存ランナーが可視ウィンドウを出す場合は移行対象とし、次回の重要変更時または定期回帰ゲート化前に非対話実行経路を追加する。挙動忠実度を保てない場合のみ、理由を文書化した例外を認める。

## Automated runtime-error gate

For any automated test that launches RimWorld, a passing scenario/test count is not sufficient by itself.

The test harness must capture an isolated runtime log and fail the overall test run if the repository-owned mod emits any ERROR-level entry. Do this even when all Pickle/RimTest scenarios otherwise pass. Warnings remain non-fatal unless a repository-specific test explicitly promotes them.

Any new RimWorld runtime-test harness added to this repository must include this mod-origin ERROR gate from the start. Static-only validation does not fabricate a runtime-log result; add the gate when runtime automation is introduced.

## Historical description audit (AMJ common)

Follow the shared policy in Ancient-Medieval-Japan-Core `Docs/HistoricalDescriptionGuidelines.md` whenever AMJE uses, retextures, selects, patches, or localizes Vanilla / Medieval Overhaul items, plants, animals, or comparable content.

Inherited Vanilla/MO descriptions must be audited from the perspective of ancient/medieval Japan and rewritten when they are anachronistic, culturally mismatched, misleading, overly modern, or otherwise unsuitable. AMJE-authored descriptions should include supported historical facts and, where supportable, a meaningful difference from modern Japan, modern use, or modern distribution.

Historical description text is Japanese-first: draft and review Japanese first, obtain author approval, then translate only the approved Japanese text into English. Apply the shared Core name-form rule: begin Japanese descriptions with an established kanji form when one exists, and include recognized aliases / alternate names or common alternate written forms at the opening; do not invent kanji or weakly sourced names. Preserve research/source rationale in durable documentation.

## Mod naming rule (AMJ common)

AMJ Core, Environment, CCTO and future related Mods must not use ASCII `:` or full-width `：` in Mod names. Use ` - ` when a separator is needed. Apply this to `About/About.xml` `<name>` and the corresponding Workshop title / formal README name; check it when creating, renaming or preparing a Mod for publication. YADA uses the display name for an upload staging directory, and an ASCII colon causes that step to fail on Windows. Display-name corrections must preserve `packageId` and existing Workshop IDs.

The shared source of truth is [Mod description guidelines — Mod名のコロン禁止](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Core/blob/main/Docs/ModDescriptionGuidelines.md). Colons in description prose, URLs and code syntax are outside this naming rule.

## Public mod descriptions

Public-description preparation and updates must also include the Japanese 2game summary in `Docs/2GameDescription-ja.txt` and its presentation policy in `Docs/2GamePresentation.md`. Follow the shared guideline's **2game向け説明（AMJ共通）** section and CCTO's six-section, plain-style template. Check README, Workshop English/Japanese, 2game Japanese and About.xml together; link named related mods and this mod's own GitHub repository. Record repository preparation separately from live-site publication.

Use the CCTO-based shared [mod description guidelines](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Core/blob/main/Docs/ModDescriptionGuidelines.md) when writing or updating public descriptions. Include save compatibility in every mod description, stating addition/removal conditions accurately for the mod's implementation. Keep README, Workshop English/Japanese BBCode, and About.xml consistent; refine the shared baseline as presentation improves.

## PowerShell change safety

PowerShell changes must not be committed directly to `main`.

- Stage any `.ps1` edit on a temporary branch.
- Keep `.github/workflows/powershell-syntax.yml` as the repository-level parser gate.
- Open a pull request and require the PowerShell syntax check to pass before merging the branch to `main`.
- `run-tests.bat` must execute `Scripts/Validate-PowerShellSyntax.ps1` before build/static validation so local test runs fail fast on parser errors.
- Avoid large string-replacement edits to `Validate-Environment.ps1`; when changing a focused block, re-read the edited range and the end of file before merge to detect truncation or duplicated tails.

## Golden Path capture rule (AMJ common)

When work reaches a verified successful state, perform a Golden Path capture before declaring the task complete.

- Preserve the known-good sequence in repository documentation whenever the procedure is reusable.
- Automate deterministic/repeatable steps instead of relying on memory or chat history.
- Add regression coverage for failure classes discovered during the work when practical.
- Keep manual verification only for checks that genuinely require human judgment.
- Reuse an existing Golden Path before inventing a new per-task workflow; update the Golden Path when the procedure changes.
- "Worked once" is not sufficient for recurring/debug-heavy work. The target state is: worked once -> documented -> automated where deterministic -> regression-locked.
- Trivial edits with no reusable procedure may be treated as Golden Path N/A.

AMJE's repository index is `Docs/GoldenPaths/README.md`. Production texture work must follow `Docs/GoldenPaths/TextureAssetPipeline.md`.

## Golden Path closeout rule

Follow the AMJ shared Golden Path policy in Ancient-Medieval-Japan-Core `Docs/DevelopmentGoldenPathGuidelines.md`.

After a non-trivial task succeeds, especially after debugging or failed attempts, do not move on with only the working implementation. Record the successful reusable procedure in the owning repository, automate deterministic/repetitive steps, and add regression guards for failure modes discovered during the work. For recurring work, completion includes the reusable documented/automated path, not only the one successful result.

`Docs/Coordination.md` remains status/handoff only; the procedure itself must live in durable repository documentation/scripts.
## Retexture technical implementation rule

For Vanilla / Medieval Overhaul / other prerequisite-asset retextures, also follow the shared Core source of truth `Docs/RetextureImplementationGuidelines.md`.

- Use AMJE-owned unique texPaths with explicit XML/Patch ownership by default; do not rely only on same-name texture shadowing.
- Treat each plant/tree target as the complete loaded graphic-state family. Audit mature/base, leafless, immature, polluted and snow-overlay states that actually exist.
- Keep pure retexture changes visual-only and preserve source rendering metadata unless the replacement technically requires a documented rendering adjustment.
- Guard optional MO patches and validate the final AMJE path against known explicit-path competitors such as ReGrowth/VTE where they touch the same field.

## Workshop distribution rule (AMJ common)

Workshop updates must also follow `Docs/GoldenPaths/WorkshopPublication.md`:
use a pinned clean source and verify the exact selected upload root against its
external manifest. Never upload a dirty development root or infer its bytes
from Git main. Only the actual downloaded source/DLL and four-profile cutting
gate clear distributed-release HOLD; Steam publication stays author-manual.

AMJ Core, Environment, CCTO and future related Mods must exclude **all files unnecessary for a Workshop subscriber** through the repository-root `.rimignore`. This includes Art masters/templates, design and development documentation (including README), source, tests/fixtures/reports, scripts/build tools, VCS/editor metadata, local overrides and debug/backup/archive files. Preserve runtime assets, About metadata, loadFolders.xml where used, and legally required licenses/attribution.

- `.rimignore` is the authoritative exclusion list. YADA uses inherited basename/wildcard rules, not Git-ignore path or negation syntax; exclude `_LocalTest.xml`, not `Patches/_LocalTest.xml`.
- Adding a file/folder includes deciding whether subscribers need it and updating exclusions when they do not. Preserve development/source material in Git; exclusion is not deletion.
- Every alternative publisher/archive/staging builder must produce the same subscriber-only payload. Keep adapters synchronized with `.rimignore`; do not maintain independent policy exceptions.
- Run `python Tests/validate_workshop_payload.py` before publication. Validate the final staging/installed package too; runtime-required DLLs and assets must actually be present. Repository filtering PASS alone is not build/runtime/Steam publication PASS.
- Shared procedure and payload contract: [Core Docs/WorkshopPackaging.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Core/blob/main/Docs/WorkshopPackaging.md).

## Art / retexture routing

Do not duplicate detailed art rules in AGENTS.

For Environment art, use:
1. Core `Docs/ArtStyle.md` — AMJ-wide visual invariants;
2. `Docs/ArtDirection.md` — Environment-specific tree/plant/terrain/world style and accepted species baselines;
3. `Docs/GoldenPaths/RetextureGeneration.md` — generation entry/preflight only;
4. `Docs/GoldenPaths/TextureAssetPipeline.md` — installation/export/validation;
5. Core `Docs/GoldenPaths/FixedImageTemplates.md` only when a visible component is intentionally reused pixel-exactly.

Environment-specific rules may define controlled class differences, such as restrained internal gradient variation for tree sprites, but they must remain compatible with the shared AMJ invariants unless the owning style specification explicitly records an exception.

For Vanilla/MO retexture ownership and texPath behavior, continue to follow the shared Core `Docs/RetextureImplementationGuidelines.md`.
