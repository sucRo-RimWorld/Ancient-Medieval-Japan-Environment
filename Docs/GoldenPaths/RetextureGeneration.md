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

## 同一樹木の季節差分：基準1枚固定 → 参照して派生（2026-10-31 作者指示）

**対象:** 同じ個体・同じ樹種を、緑葉／開花／落葉／葉あり積雪／落葉積雪などの複数画像状態で再現する作業。**全状態を基準画像なしで一括新規生成しない。** この手順は樹形の不一致を減らす制作方針であり、画像生成だけで画素一致が保証されるという実証ではない。

1. **最初に基準樹形を1枚採用する。** 採用済みの通常緑葉版、または作者が採用した開花版など、**実際の原画PNGを開く**。その画像の幹・根元・主枝・全高・全幅・幹中心・樹冠の外形を基準として確定する。別の樹種の画像や未採用の再生成結果を基準として代入しない。作者の承認を待たずに採用済み原画を上書きしない。
2. **残りの状態は基準画像を必ず参照して派生する。** 例：緑葉基準から、花と赤褐色の若葉を同時に持つヤマザクラ開花版、落葉版、葉あり積雪版、落葉積雪版を作る。幹・主枝・根元・配置・描画寸法・輪郭は維持し、変更を必要な葉・花・雪のみに限定する。開花時の花・若葉や、葉のない状態に必要な細枝は、対象種の固有要件を守りつつ既存の主枝から自然につなぐ。
3. **状態をまとめて指示しても、完成物は状態ごとに独立させる。** 参照元を共用して複数状態を同時に依頼してよいのは、ツールが**各状態を独立した単木PNGとして出力できる場合だけ**。複数状態のコラージュ・比較表・スプライトシート・ラベル付き画像を最終テクスチャとして生成しない。ツールが単一画像しか出せない、比較シートへ逸脱する場合は**1状態ずつ**基準画像付きで編集する。比較表は承認監査用に別途合成してよいが、採用する画像そのものにはしない。
4. **各状態を個別に監査する。** 採用済み基準画像と同じ正方形キャンバスに置き、根元下端・幹中心・全高・主枝の接続・樹冠の広がりを重ねて比べる。外形は葉ありと落葉で当然異なるため、樹冠を画素一致させること自体を合格条件にしない。画像自体のRGBA透過（偽の市松背景不可）、縁の切れ、太外枠、葉／枝の細かさ、256pxと64pxでの可読性、既存採用ブナ等との画風も確認する。**ぼかして細部を隠すのではなく、くっきりした大きな葉塊・少ない色面へ簡略化する。**
5. **差分だけ修正し、既存の採用画像は不変とする。** ある状態が不合格でも全状態を作り直さない。生成で幹・主枝が変わった場合は「固定した」と報告せず、元画像の保護領域を実画素固定して編集するか、再度基準画像に沿った候補に戻す。状態ごとの作者承認・不採用・妥協採用および不一致箇所を分離して記録する。
6. **採用後に初めて保存・実装へ進む。** 状態ごとの採用PNG原本を `Art/Sources/` に原バイト保持し、決定的な256px透過派生を `Textures/` に置く。実際の `PlantDef` / season-graphic / snow rendering の接続、静的PNG検証、実ゲーム表示は別ゲートとする。**画像外観の承認 ≠ 季節遷移や本番ロードの成功**。詳しい保存・導入手順は `TextureAssetPipeline.md` を参照する。

**積雪の扱い:** 過去の作業では雪専用透過レイヤの作成に、枝から浮く・裏側に見える・ずれる・細い白線になる失敗が繰り返された。これを無条件の成功方式として勧めない。まず**採用済み葉あり／落葉原画を基準とした、雪を含む単木全体の視覚候補**を作ることは認める。一方、RimWorld の `snowOverlayGraphicPath` / `leaflessSnowOverlayGraphicPath` は**雪部分だけのオーバーレイ**を必要とするため、全木画像をそのパスへそのまま指定しない。オーバーレイを利用する場合に限り、後段の「雪だけを別レイヤ化」および `PlantSnowOverlayPlan.md` を適用する。全木画像を直接切り替える別方式は描画・積雪条件・互換性を設計／実機確認してから採用する。**画像制作方式とゲームへの接続方式は混同しない。**

**適用履歴・限界:** 2026-10-31のヤマザクラ制作では、独立生成で主枝・根元の不一致、比較シートの誤生成、ぼかし簡略化、積雪レイヤの不自然さが発生した。作者は採用緑葉版を樹形基準とし、状態の派生を希望している。既に妥協採用された落葉・積雪原画などの**実際の採用範囲を変更・撤回せず**、次の新規状態制作からこの手順を適用する。

## 成功事例ログ：ヤマザクラ開花→開花積雪（2026-11-01、PR #299）

**記録の位置づけ：作者が外観を採用した1例。** 今後の季節画像で同じ方法を試すための「成功候補手順」であり、複数回の再現成功や、元画像の全ピクセル固定・RimWorld実機表示を証明した「最適手順」ではない。採用原本と実装状態は [ArtDirection](../ArtDirection.md) を正本とする。

### 実際にうまくいった条件と順序

1. **元画像を直接指定：** ユーザーが、既に採用したヤマザクラの「開花＋赤褐色の若葉」原画を画像として会話へ再添付した。元画像は Art/Sources/Things/Plant/AMJ/Yamazakura_Bloom/Yamazakura_Bloom_Source.png（1254×1254 RGBA、SHA-256 b2f5e1e880f7f8de0ebbcbe31ae67b860c453fd2fe4c3697ab1a0a1c5d769640）。似た樹種の画像、以前の不採用候補、名称だけの参照で代用しなかった。
2. **変更点を一つに絞る：** ユーザーの実際の指示は**「これに積雪させて」**。同じ幹・枝・花房・赤褐色若葉を残した**単木の全木積雪版**を生成する編集であり、落葉化・再構図・別樹形・雪だけの透過レイヤの生成を同時に要求しなかった。雪は主枝・葉・花房の**上側に厚みをもって載せ**、下側の花と若葉、幹を見せる仕上がりとなった。淡い花、赤褐色葉、白～薄青色の雪、太い輪郭線が共存した。
3. **元画像と比較：** 同一寸法で幹・根元・主枝、樹冠の左右と上端、開花・若葉の分布、雪の支持関係、輪郭・透過を確認した。生成された樹木全体の**完全な画素一致は未達／未保証**であり、視覚上の樹形連続性を評価したもの。RimWorldサイズ（256px/64px）の可読性は通常どおり別途確認し、未実施項目をPASSとはしない。
4. **原画の見た目を作者が採用：** 候補に対して「わりといいのでは？」、続いて**「これ採用で」**と判断。採用原画は Art/Sources/Things/Plant/AMJ/Yamazakura_Bloom_SnowFull/Yamazakura_Bloom_SnowFull_Source.png（1254×1254 RGBA、SHA-256 bd348475785d999ea4a0955b08c5d7e48e4bfb4f9d3e32943b50b7f47eb1e8e4、Git Blob 1891a3bc15399c9ac4738c35e6734539b9149729）。採用画像を別の再生成結果へ置換しなかった。
5. **採用後に実装へ移す：** 原画を原バイト保持し、別ファイルの256×256派生を Textures/Things/Plant/AMJ/Yamazakura_Bloom_SnowFull/Yamazakura_Bloom_SnowFull_A.png へ設置。[PR #299 コミット 88e741a](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/commit/88e741aa0f3adc8968d0b8c06bf3ce81414c2a86) で、開花期＋積雪深>0.8時の**全木画像切替**へ接続。元の開花画像を変更せず、Vanilla雪専用オーバーレイ経路には全木画像を指定しない。Git Blob/ツリー照合は実施、DLLビルド・DDS・ゲーム内切替は未検証。

### 以前の失敗例との相違（原因と断定しない）

- **基準画像の取り違え・別個体化：** 以前は採用済み開花樹形から逸脱した積雪画像が出た。今回は**その場で添付された実際の開花原画**を起点にした。
- **雪だけのレイヤ抽出：** 以前は雪が浮く、継ぎ目が見える、矩形に切れる等が発生。今回は**雪を含んだ全木の完成絵**として先に見た目を成立させ、別レイヤ化を要求しなかった。
- **複数状態同時生成：** 以前は別状態の混入や比較シート化があった。今回は**開花積雪1状態**に限定した。
- **原因の限界：** これは同一条件の比較実験ではない。短い指示、参照画像の直接添付、全木化、生成の偶然性のうち何が主因かは未確定。**この1回だけで方式を絶対化しない。**

### 後続の成功・失敗ログを記録する最小項目

次の樹木・季節画像でも、**樹種／採用原画の実ファイル・寸法・SHA／変更対象状態／実際の生成指示（言い換えない）／参照画像の渡し方／全木か雪専用レイヤか／生成画像の識別子・寸法・SHA／元画像との具体的差分／作者の採否／実機検証有無**を残す。未取得のモデルパラメータ・seedは推測で補完しない。失敗例も消さず、同じ症状の再発を確認できるようにする。**複数樹種・複数状態で実績が蓄積してから**標準手順を見直す。

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
