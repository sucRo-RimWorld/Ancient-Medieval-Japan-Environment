# AMJE Retexture Generation — New-Chat Start Procedure

## Scope and authority

Use this procedure for AMJE plant/tree sprites, including AMJE-owned plants and
Vanilla/MO tree retextures. It is the generation entry point, before
[TextureAssetPipeline.md](TextureAssetPipeline.md).

The shared visual source of truth is Ancient-Medieval-Japan-Project `Docs/ArtStyle.md`; Environment-specific tree/plant rules and accepted species baselines are owned by [ArtDirection.md](../ArtDirection.md). This procedure owns only the generation preflight and handoff, not a second copy of either style guide.

Workshop covers, logos, terrain textures and world-biome images are different
asset classes. Read their owning specifications; never substitute these tree
rules for a cover's shared-left/addon-right layout. If the requested class is
unclear, resolve the class before generating.

## New-chat preflight — required before an image-generation call

1. Read repository `AGENTS.md` and authoritative `main:Docs/Coordination.md`
   first. Read the current main versions, not a remembered chat summary.
2. Read Ancient-Medieval-Japan-Project `Docs/ArtStyle.md`, this procedure, `Docs/ArtDirection.md` (Environment/category/species additions), and `Docs/GoldenPaths/TextureAssetPipeline.md`.
3. Resolve the latest OPEN/IN PROGRESS art handoff and distinguish:
   proposed design / author-approved design / generated candidate /
   approved source / installed asset / accepted in-game appearance.
   A request to save rules or to start a new chat is not approval of a pending
   species design.
4. Fetch and actually view the accepted reference PNGs below. Do not merely read
   filenames, rely on an old conversation image, or claim to have inspected
   pixels that were not opened.
5. Reuse the existing approved category/style and any already-approved target design. If no target-specific proposal is already approved, derive the narrowest reasonable composition from the current request and the owning style rules; do not add a mandatory extra approval round unless the user explicitly asks for proposal/review-first work.
6. Build the generation request from the current Ancient-Medieval-Japan-Project `Docs/ArtStyle.md` invariants plus the applicable Environment/category/species rules in `ArtDirection.md`. Attach the actual viewed reference images. Do not maintain a second frozen style prompt in this procedure.
7. Generate one candidate for the requested target. Do not restart the style design or produce unrequested alternatives.

When fetching/viewing a required reference is blocked, report that limitation
and resolve the missing reference before generation. Do not silently substitute
an unrelated source. The user must not be used to relay information between
repository workstreams.

## Accepted reference assets

Fetch these paths from the owning repository's current main. The listed blobs
identify the accepted baseline at this procedure's introduction; a later
documented author-approved replacement supersedes them.

| Category / reference | Repository path | Accepted Git blob |
| --- | --- | --- |
| Evergreen broadleaf / Sudajii | `Textures/Things/Plant/AMJ/Shii/Shii_A.png` | `62d795d9717a3ad4d61827bfb470cd7b3cfc7000` |
| Deciduous broadleaf / leafy beech | `Textures/Things/Plant/AMJ/Beech/Beech_A.png` | `7face00d1128333b5fa6e1ab419d3424bc21b6dd` |
| Deciduous structure / leafless beech | `Textures/Things/Plant/AMJ/Beech_Leafless/Beech_Leafless_A.png` | `c9fdc9b87e6b559ab2a2ff9788b6d015593b8406` |

For Shirabiso and Haimatsu, Sudajii and leafy beech supply outline, simplification,
palette restraint and shading weight. Their broadleaf silhouettes are **not**
templates for conifer form. Use the tall-conifer or low/dwarf-conifer category
in ArtDirection for structure.

For an image tool accepting local references, pass the actual local paths in
its reference-image argument. If references are conversation images, include
the smallest recent-image set that contains every required reference. Do not
claim that written paths alone attach pixels to a generator.

## Generation request assembly

The generation request must carry the **current** applicable rules from:
- Ancient-Medieval-Japan-Project `Docs/ArtStyle.md` for AMJ-wide invariants;
- `Docs/ArtDirection.md` for Environment tree/plant class rules and the target species;
- the actual accepted reference images opened during preflight.

Do not copy a long frozen prompt into this file. That previously created a second style source that could drift from the project-wide rules. The request should contain only the shared invariants plus the approved target-specific silhouette/structure differences needed for the current asset.

If the target-specific request would contradict Project ArtStyle or ArtDirection, stop and resolve the specification first instead of compensating with ad-hoc prompt text.

## Mandatory audit after every generated image

Every individual output (including revisions and retries) must be visually compared with the **actual current author-accepted reference images** from the same asset class before it is presented as a viable candidate or followed by another generation. Do not rely only on a prompt, an old chat description or the target image itself.

1. Open the generated output and actual accepted reference PNGs; use the accepted deciduous tree reference for a new deciduous tree and any additional accepted wetland/plant image relevant to the target.
2. Compare outline thickness, internal line weight, simplified silhouette and major color masses, palette/color count, gradients/shading, leaf and branch detail density, proportions and overall readability at RimWorld sprite size; additionally inspect species-specific shape and PNG transparency/background.
3. State the audit result explicitly for **each generated output**, noting concrete mismatches against accepted art and whether the image is candidate-ready, requires correction or must be rejected. A mismatch cannot be marked adopted merely because the broad style prompt was followed.
4. If corrections are possible, carry the accepted baseline into the next revision and repeat the same inspection after that output. Never silently skip the audit between iterations.
5. If accepted reference pixels cannot be fetched and viewed, identify that blocker rather than claiming the comparison passed.

This is a generation QA requirement, not an automatic author acceptance or approval to install sprites.

## Candidate review and production handoff

Before presenting a generated candidate, compare it against the current user request, any already-approved target design, and the viewed references:

- category/silhouette and species distinction;
- strong simplification, large masses and thick outline;
- restrained palette, limited color steps and subtle shading;
- transparent background, complete edges and no ground/scenery/snow;
- deciduous state continuity when applicable.

If a constraint failed, name the discrepancy and correct it using the same
baseline. Do not label a conflicting candidate as ready for integration.
Source-art approval and in-game appearance acceptance are separate stages.

After source approval, use `TextureAssetPipeline.md` for the exact production
PNG bytes, structure/CRC/decoded-image validation, hash-preserving install,
Def-switch ordering and the matching natural-biome runtime check. Do not
rebuild base64 by hand, regenerate merely to transfer an image, or repair
only a stored CRC to hide corrupt image data.

Record durable accepted design/appearance in `Docs/ArtDirection.md`.
Record current stage, remaining coverage and next action only in
`main:Docs/Coordination.md`. Before leaving the task or moving to another chat,
ensure those records describe the current state and identify the actual
reference/output paths; do not depend on chat-only approvals or attachments.

## Portable restart instruction

A new chat can start with:

> AMJ Environmentの樹木リテクスチャを継続してください。
> mainのAGENTS.mdとDocs/Coordination.mdを最初に確認し、
> Docs/GoldenPaths/RetextureGeneration.mdの開始手順から再開してください。

Repository:
`sucRo-RimWorld/Ancient-Medieval-Japan-Environment`.

For any explicitly reused visible component, follow the Ancient-Medieval-Japan-Project `Docs/GoldenPaths/FixedImageTemplates.md`; ordinary same-style tree sprites do not require fixed-pixel templates.

## Snow-only derivative routing

For aligned snow overlays, follow [PlantSnowOverlayPlan.md](../PlantSnowOverlayPlan.md) in addition to this preflight. Inspect the accepted snow reference as well as the immutable base master. The no-snow rule applies to normal base sprites, not the requested separate overlay. Fixed-pixel success does not establish outline, internal-line, support or massing quality. Local painting and deterministic compositing must pass the same visual audit as generated candidates. Follow Core FixedImageTemplates authorization rules before choosing any new image generation.


## 葉あり樹木の積雪：樹冠輪郭を先に合わせる（2026-10-30 シラカバ事例）

**目的:** 採用済みの葉あり樹木を基準に、枝・葉塊ごとの**上面に厚みのある雪**を載せ、樹冠の不規則な輪郭へ自然につなげる。雪の量を葉先の細い白線だけにしない。対象は積雪用の**視覚候補と、その後の雪専用オーバーレイ**であり、通常画像の改変ではない。

**今回改善した点と限界:** シラカバの採用葉あり原画（[PR #293](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/pull/293) の `Art/Sources/Things/Plant/AMJ/Shirakaba/Shirakaba_Leafy_Source.png`）を形の基準にし、2026-10-30の「雪化粧の白樺ツリー_sprite.png」候補は雪を葉塊上面に広く置いたことで、以前の薄い葉先だけの積雪や、矩形マスクで切れた積雪より**視覚的に改善**した。ただしこれは**樹木全体を含む画像生成結果**で、元画像のピクセル同一性は成立していない。画像生成のみでは元の幹・枝・葉色・輪郭を完全固定できない。作者による正式な積雪画像採用、雪単体レイヤへの分離、ゲームでの検証はまだ別段階である。「うまく見える」ことを「雪だけのレイヤ完成」と誤記しない。

### 再利用する短い作業順

1. **元画像を固定:** 最新の作者採用済み葉あり原画を実際に開き、原ファイルのバイト列・キャンバス寸法・RGBA/アルファ・SHA-256を保持する。新規生成画像や、過去の不採用積雪候補を元画像の代わりにしない。必要な参照樹種はこの文書の preflight と `Docs/ArtDirection.md` で確認する。
2. **見た目の見本を1枚作る:** 画像生成・編集が現在の依頼で許可されているときだけ、**採用画像そのものを画像参照**として渡す。指示は「同一の幹・主枝・葉塊配置・全体の高さと幅を保つ」「各葉塊の不規則な**上側の輪郭**に沿う、白・青灰の厚い雪の面」「葉の下半分・樹皮は見せる」「黒い太外枠とAMJの簡略化を維持」「単木、透明背景、発光・地面・矩形クリップなし」を中心に組む。積雪用の参照画像が別にある場合も、その**樹形を取り込まない**。
3. **見た目を比較監査:** 元画像と候補を**同一大きさで並べ**、樹冠上端・左右端、幹/枝接続、雪の接地、白い量感、輪郭線、透過縁を確認する。特に「雪が葉先のみ」「元の葉塊より内側に矩形で切れる」「宙に浮く」「葉や幹を描き直した」「光輪がある」は修正対象。256pxと64pxでも確認し、各生成ごとに既存採用画風との比較を省略しない。
4. **雪だけを別レイヤ化:** 視覚候補の雪の**位置・量感だけを参考**に、採用済み原画と**厳密に同サイズ・同座標**の透明RGBAレイヤを作る。承認済み原画に直接上書きしない。雪は元の葉塊上面へ不規則な曲線で接触させ、必要な部分だけ元の葉を前面から覆う。葉塊ごとの矩形クロップをそのまま雪の輪郭/マスクに使わない。画像生成された樹木全体から差分抽出した画像を、検証なしで「雪だけ」と呼ばない。
5. **合成とチェック:** `元の採用PNG + 雪専用透過PNG` を決定的に合成。雪レイヤ外で原画RGBAが一致すること、背景に矩形境界や発光がないこと、葉塊に積雪が支持されていることを確認する。雪だけの画像と合成プレビューを別々に提示し、作者承認後に [PlantSnowOverlayPlan.md](../PlantSnowOverlayPlan.md)、[TextureAssetPipeline.md](TextureAssetPipeline.md) の本番経路に進む。既存の承認済み雪画像を自動で置換しない。

**失敗を繰り返さないための境界:** 葉の色域だけを機械的に検出して雪を塗ると、葉先だけが白くなる／四角く切れることがある。逆に木全体を画像生成し直すと雪の形は良くても元の幹・枝・葉の画素が変わる。この2つを混同せず、**視覚設計は参照画像を使い、正確な素材化は元原画不変の別レイヤで行う**。
