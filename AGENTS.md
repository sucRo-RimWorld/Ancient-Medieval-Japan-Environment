# AMJ Environment Agent Instructions

This repository is part of the **Ancient & Medieval Japan (AMJ)** project.

Before starting work in this repository:

1. Read this file.
2. Read the authoritative coordination log at `main:Docs/Coordination.md`.
3. Check for OPEN / IN PROGRESS items owned by the current workstream before starting new work.
4. For plant/tree image-generation or retexture work, enter through `Docs/GoldenPaths/RetextureGeneration.md` before any generation call. Read its current main version, load and actually view the accepted reference PNGs, and carry its complete shared prompt into the generator.

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

Historical description text is Japanese-first: draft and review Japanese first, obtain author approval, then translate only the approved Japanese text into English. Preserve research/source rationale in durable documentation.

## Public mod descriptions

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
## Retexture visual-style rule

Tree/plant retextures must follow the canonical shared visual-style rules in `Docs/ArtDirection.md` under **Retexture visual-style rules**, then add only species-specific differences.

Do not design each asset from scratch. Reuse the shared AMJE baseline for:
- strong simplification/deformation rather than botanical realism;
- thick dark outline and restrained low-to-medium saturation;
- limited color steps with only subtle gradient variation;
- transparent background with no decorative ground base;
- species differentiation through silhouette/structure, not color alone;
- leafy/leafless continuity for deciduous trees;
- normal-game-zoom readability over fine detail.

Use accepted Sudajii and Japanese beech assets as the current evergreen-broadleaf and deciduous-broadleaf reference baselines. Production transfer/integration must separately follow `Docs/GoldenPaths/TextureAssetPipeline.md`.

## New-chat image-generation continuity

For every new chat or resumed tree/plant art task, follow `Docs/GoldenPaths/RetextureGeneration.md`. Read the shared and species rules, actually view and attach the accepted baseline images, recover the precise approval stage from main Coordination, and present the species composition/design before generation unless that exact proposal already has author approval. Preserve the full baseline prompt when adding species-specific details; do not regenerate the style from memory. Rule-saving/new-chat requests do not approve a pending image proposal. Record durable approvals in ArtDirection and current progress in main Coordination before handoff.

## Pixel-exact reused components (AMJ shared policy)

Follow the Core source of truth [FixedImageTemplates.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Core/blob/main/Docs/GoldenPaths/FixedImageTemplates.md). Same style does not mean identical parts: species-specific silhouettes remain distinct. Where a trunk, container, title or other component is reused, register an approved lossless master and binary editable mask with SHA-256 hashes in the owning repository, generate only variable material, and composite deterministically. Final decoded protected RGBA pixel differences must be zero; reference-image editing/visual similarity cannot replace this check. Use Core `Scripts/Art/fixed_template.py` for compositing/validation, then retain Environment's PNG integrity/install gates. Read the master/manifest and actually view the approved reference in every new chat. No shared-pixel guarantee may be claimed before the family template and output pass this gate. Existing proposal approval stages are unchanged.
