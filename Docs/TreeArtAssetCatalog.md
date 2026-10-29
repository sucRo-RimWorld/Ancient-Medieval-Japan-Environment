# AMJ Environment — 樹木状態アセット再利用索引（2026-10-30）

## この索引の役割

過去の**採用画像そのもの、制作に使える原画、実装済み素材、積雪制作スクリプト、採用根拠**を1か所から追えるようにする。画像バイナリを重複保管したり、この一覧を新しい採用判断・ゲーム仕様の正本にしたりしない。

- **正式な画風・状態別の採否:** [ArtDirection.md](ArtDirection.md)、[PlantVisualCoverage.json](PlantVisualCoverage.json)。**制作開始手順:** [RetextureGeneration.md](GoldenPaths/RetextureGeneration.md)。**本番配置:** [TextureAssetPipeline.md](GoldenPaths/TextureAssetPipeline.md)。
- 以下の「main」は調査時点の main に現物がある意味。「PR」は**未マージの作業ブランチ**に現物がある意味。次回利用時はPRの現在のheadと採否を再確認する。
- **採用された見た目／原画を保有／本番に配置済み／ゲームで表示確認済み**は別。別樹種の画像を形状の代用品として再生成しない。

## 1. 落葉画像の直接参照資産

### main：本番画像と採用記録のある樹木

| 樹種 | 葉ありの基準 | 落葉の基準 | 他の状態と確認根拠 |
| --- | --- | --- | --- |
| **ブナ** | [葉あり本番PNG](../Textures/Things/Plant/AMJ/Beech/Beech_A.png) | [落葉本番PNG](../Textures/Things/Plant/AMJ/Beech_Leafless/Beech_Leafless_A.png) | [落葉積雪PNG](../Textures/Things/Plant/AMJ/Beech_Leafless_Snow/Beech_Leafless_Snow_A.png)、[積雪受入記録](ValidationEvidence/BeechLeaflessSnowSourceAcceptance.md)・[修正監査](ValidationEvidence/BeechLeaflessSnowRepairAudit.md)。落葉の形状方針は[ArtDirection.md](ArtDirection.md)「Japanese beech — leafless」。 |
| **ハンノキ** | [葉あり本番PNG](../Textures/Things/Plant/AMJ/Hannoki/Hannoki_A.png)・[原画](../Art/Sources/Things/Plant/AMJ/Hannoki/Hannoki_Mature_Source.png) | [落葉本番PNG](../Textures/Things/Plant/AMJ/Hannoki_Leafless/Hannoki_Leafless_A.png)・[落葉原画](../Art/Sources/Things/Plant/AMJ/Hannoki_Leafless/Hannoki_Leafless_Source.png) | [落葉積雪オーバーレイ](../Textures/Things/Plant/AMJ/Hannoki_Leafless_Snow/Hannoki_Leafless_Snow_A.png)、[実画面の採用記録](ValidationEvidence/HannokiVisualAcceptance-20261015.md)・[自然な季節遷移の検証](ValidationEvidence/HannokiSeasonalNative-20261017.md)。 |

**現物識別（Git blob SHA-1）:** ブナ葉あり `7face00d1128333b5fa6e1ab419d3424bc21b6dd`／落葉 `c9fdc9b87e6b559ab2a2ff9788b6d015593b8406`。ハンノキ葉あり原画 `5975d43775a4836c05cf40ed1465dd9d3e08b843`／落葉原画 `4d3e9f268d81370380e8eb090d5954a09403e3ce`。採用対象の上書き・置換ではなく、次回作業前の照合用。

ブナ・ハンノキの**落葉状態は採用済み**だが、「その落葉画像を生成したモデルの全プロンプト／操作履歴・途中マスク」は再現可能な制作手順として保存されていない。**これらの画像を見本として利用できること**と、**当時と同じ生成操作を再実行できること**は区別する。ハンノキについては過去の説明に残る主枝の相違を「完全一致」と読み替えない。

### 未マージPR：原画はあるが本番と混同しない

| 樹種 | 参照先・状態 | そのまま流用してはいけない部分 |
| --- | --- | --- |
| **ナラ** | [PR #292](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/pull/292)：`Art/Sources/Things/Plant/AMJ/Vanilla/Oak/Oak_LeafyAcceptedMaster.png`（最新採用葉あり）／`Art/Sources/Things/Plant/AMJ/Vanilla/Oak/Oak_AcceptedSheet.png`（落葉元を含む旧比較シート）／`Textures/Things/Plant/AMJ/Vanilla/Oak_Leafless/Oak_Leafless_A.png`（落葉単体化済み） | 最新の葉あり版と落葉版の幹・主枝の連続性は未再確認。旧シートの**葉あり**は現行採用版ではない。 |
| **カエデ** | [PR #292](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/pull/292)：`Art/Sources/Things/Plant/AMJ/Vanilla/Maple/Maple_AcceptedMaster.png`（葉あり） | 落葉画像は未採用。ナラの落葉や別のカエデ生成物で補わない。 |
| **ダケカンバ** | [PR #293](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/pull/293)：`Art/Sources/Things/Plant/AMJ/Dakekanba/Dakekanba_Leafy_Source.png`、`Art/Sources/Things/Plant/AMJ/Dakekanba_Leafless/Dakekanba_Leafless_Source.png`、`Art/Sources/Things/Plant/AMJ/Dakekanba_Leafless_Snow/Dakekanba_Leafless_Snow_Source.png`。256px確認画像は各々 `Art/Candidates/` 配下。 | 葉あり・落葉・落葉積雪は原画ごとの見た目が採用されたが、幹・枝座標と寸法の厳密な状態間一致は未確認。積雪原画は**樹木全体**で、雪のみのレイヤではない。 |
| **シラカバ** | [PR #293](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/pull/293)：`Art/Sources/Things/Plant/AMJ/Shirakaba/Shirakaba_Leafy_Source.png`（採用葉あり）／`Art/Sources/Things/Plant/AMJ/Shirakaba_Leafless/Shirakaba_Leafless_Source.png`（**採用落葉**）／各 `Art/Candidates/Shirakaba*/` に256pxレビュー素材 | 2026-10-30、葉あり版との幹・根元の重ね比較を作者が**「合格」**と承認。上部枝と樹皮模様はピクセル一致ではない。**積雪画像・本番Def接続は未承認・未実施**。 |

**未マージPRの識別（Git blob SHA-1）:** ナラ新葉あり `02fc609ac84be9585fb20ccd35d542ce11da07ad`／落葉256px `9eaca11a748b4e4cba237cc42c524e1a422a307a`、ダケカンバ葉あり `da30bc6a316b6bef0c9391388a43b153bdc2bd93`／落葉 `60582ef66778f7af01bde38084e724a9bbcfec9e`、シラカバ葉あり `d9fa93cdfbeac9bffa85c3ad143e5d8f6db03282`。これらは各PRの当時のheadにおける実在blobであり、mainの採用済み本番画像という意味ではない。

## 2. 制作工程として再利用できる資産

- **開始・画風照合:** [RetextureGeneration.md](GoldenPaths/RetextureGeneration.md) と [Project共通ArtStyle](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/ArtStyle.md)。必ず対象樹種の採用済み画像とブナの葉あり・落葉を**実際に開く**。
- **積雪レイヤの原本と検証法:** `Art/Templates/Beech_Leafless-Snow-v1/`（master / mask / overlay / composite / template.json）、[build_beech_snow.py](../Scripts/Art/build_beech_snow.py)、[validate_beech_snow.py](../Tests/validate_beech_snow.py)、[PlantSnowOverlayPlan.md](PlantSnowOverlayPlan.md)。これは**落葉積雪の工程**であり、葉ありから裸木を生成するスクリプトではない。
- **ほかの積雪制作の実装例:** `Scripts/Art/build_haimatsu_snow.py`・`build_shii_snow.py`・`build_shirabiso_snow.py` と各 `Art/Templates/*-Snow-v1/`。それぞれの**既存画像用**なので、別樹種へ座標やマスクを直接コピーしない。
- **本番設置・ロード済み検証:** [TextureAssetPipeline.md](GoldenPaths/TextureAssetPipeline.md)、[PlantVisualCoverage.md](GoldenPaths/PlantVisualCoverage.md)。画像受入と本番採用・実ゲーム表示を混同しない。
- **不存在を明示:** `Scripts/Art/` に採用済みブナ／ハンノキ等の裸木を同一構図で生成する**共通の leafless builder は存在しない**（2026-10-30 mainツリー）。チャットで生成しただけの候補・未保存パラメータを、再現可能な正本として扱わない。

## 3. 次の落葉画像制作に使う最小手順

1. **正しい原画を選択:** 対象樹種の現行採用**葉あり**原画（mainまたは明示された未マージPR）を取得し、ファイル実体・寸法・透明領域・SHAを確認。類似樹種や不採用候補で代用しない。
2. **幹・根元・主枝を保存:** 幹の色柄、根元の両端と最下端、幹中心軸、主枝の分岐位置を固定基準にする。葉に隠れる先端だけを補う必要がある。葉色の単純な自動抽出のみでは、隠れた枝の再建まで成功したことにはならない。
3. **枝先を簡略化:** 葉塊を除き、太い主枝から少数の二次枝を伸ばす。ブナのように細枝を無数に増やす必要はない。画像生成によって幹や根元ごと別形状になったら、**同一木の落葉版として不適合**とする。
4. **同寸法・重ね比較:** 葉ありと落葉を同じ256×256pxへ**同じ縮尺ルール**で配置。アルファ16以上の有効画素の外接矩形、全高・全幅・幹と根元位置・主枝の位置を個別に記録する。根元の最下端と幹中心軸を揃えた半透明重ね合わせと、256px・64pxの並列画像を確認する。**葉あり樹冠の外周と落葉枝先の全幅が異なるだけでは不合格にしない**。合否を決める前に実際の形と差の理由を見る。
5. **監査と採用の境界:** 実際の採用済み落葉参照（ブナ、ハンノキ、該当するPR素材）に対して太外枠・細部密度・樹種判別・透過・位置の差を記録する。初回生成・再生成ごとに監査し、候補／不採用／作者採用を明示する。独自の数値許容差を合意なしに「合格基準」として追加しない。
6. **採用後に保存:** 新たな作者採用があった**その画像実体**だけ `Art/Sources/` に原バイトで保持。別の256px派生、必要な積雪レイヤと本番 `Textures/`、Defの読み込み検証はそれぞれ別工程。採用済み画像を無断生成・上書きしない。

**シラカバ落葉の採用更新（2026-10-30）:** 途中の機械的な除葉候補と細枝過多の生成結果は**不採用の履歴**。後に新たに提示された `葉のない白樺の木_sprite.png` を葉あり原画と重ねて幹・根元のシルエットを確認し、作者が「合格」と承認した。採用原画はPR #293の `Art/Sources/Things/Plant/AMJ/Shirakaba_Leafless/Shirakaba_Leafless_Source.png`（Git blob `dd13a91b846708703f6e0789694052065a0fc8d4`）。完全な生成操作・途中のマスクが保存されたことを意味せず、画像そのものと重ね比較の成功結果のみを資産として扱う。

## 更新ルール

新たに**実際に採用・反映された**樹種が増えたときだけ、原画・256px派生・本番・ゲーム確認の存在範囲を更新する。既存採用画像のバイナリはこの索引の整理目的で移動・修正・削除しない。正式な仕様や採用判定の更新は各元ファイルへ反映し、ここには参照リンクのみ残す。
