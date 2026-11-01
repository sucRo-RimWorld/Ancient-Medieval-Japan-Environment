# Ancient & Medieval Japan - Environment — Design

**2026-11-01 ハンノキ葉あり積雪の作者採用（現行):** 地面だけ雪で樹冠が黄緑色のままになる旧 `Hannoki_Snow_A.png` の積雪不足を修正。ハンノキの葉あり雪専用オーバーレイを白色雪塊2,477→6,771画素へ増量し、既存の枝・葉・樹冠シルエットは一切変更しない。採用PNG（SHA-256 `5549f3a99c268211af103f4bf6036e61f36ff8034d01ddd45b00498b61724250`）・DDS（SHA-256 `7c99559a18c6f4d48740bca8ef9211da25489f5870bc58a5016b4c5ea35a42bc`）の原本と本番を同期。RimWorld 1.6.4871実ゲーム静止画で正午・晴天・積雪深1.0の葉あり積雪表示と落葉時の枝積雪なしを確認し、作者「ｙ」で採用。旧版原画は`Art/Archive`に保存。DefName・季節処理・木材量・自然分布・落葉時透明画像は不変。実プレイでの年間季節サイクルや1.7はこの受入範囲外。

## 2026-11-01 作者決定：AMJE落葉樹の「落葉積雪」を廃止

**現行仕様:** 落葉時の枝・幹への雪オーバーレイを表示しない。地面の積雪、樹木の落葉状態、葉あり樹木への通常積雪は継続する。既存のブナ（`AMJ_Tree_Beech`）・ハンノキ（`AMJ_Tree_Hannoki`）に即時適用し、今後実装するシラカバ・ダケカンバ・ヤマザクラ・ウメ・クリ・カキ等の**Environment所有の落葉樹にも同じ描画方針**を適用する。バニラ／他Mod所有の落葉樹を無断で変更しない。

**実装:** 既存の `leaflessGraphicPath` と `snowOverlayGraphicPath` は維持し、`leaflessSnowOverlayGraphicPath` だけを共通の**完全透明** `Things/Plant/AMJ/LeaflessNoSnow`（256px PNGとDDS、すべてα=0）に接続する。単純にXML要素を削除して葉あり用積雪へフォールバックすることを避ける。旧落葉積雪の採用画像・マスク・制作履歴は `Art/Sources`／`Art/Templates` とGit履歴に保持し、本番Defから参照しない。通常積雪の画像・Def、幹・根元・落葉の採用画像、季節の切替ロジック、木材量・分布・植林・セーブ用DefNameは変更しない。

**受入境界:** ユーザーの「落葉積雪廃止で」は仕様の承認。実装の確認はロード済みのRimWorldで落葉時に**枝の雪なし・地面の雪あり**、葉ありでは従来の積雪あり、ERRORなしをチェックする。過去の落葉積雪画像に対する作者受入は履歴であり現行の仕様ではない。未実装の将来植物や未検証の季節切替を実装済みとみなさない。

## 2026-10-30 作者決定：季節性アップデートの独立範囲

今回の植生追加アップデートは、**古代・中世日本の春・秋・冬の景観変化をゲーム上で明確にすること**を目的とし、次の**新規5種類のみ**を対象とする。

| 対象 | 主な季節表現 |
|---|---|
| ヤマザクラ | 春の開花・新緑・秋の紅葉・冬の落葉 |
| ウメ | 冬末～早春の開花・落葉 |
| クリ | 秋の結実・落葉 |
| カキ | 秋の果実・紅葉・落葉 |
| ススキ | 秋の穂・冬の枯れ姿 |

- この5種類の**新規追加・季節状態表現**を一つの実装単位とする。具体的な自然発生重み、成長・結実・収穫数値、PlantDef/画像の完成および実ゲーム受入をこの範囲決定だけで承認済みとみなさない。
- **梅雨の追加実装は保留（2026-10-30作者判断）。** 今回の季節性アップデートの構想段階で梅雨による季節表現の拡充も検討したが、プレイ上のデメリットが目立ち、見合う利点がないとの作者評価により採用しなかった。今回の新規5植物と同じ更新範囲には入れず、将来計画でも実装保留とする。これは既存の0.3.0開発ソースに含まれる暫定的な梅雨・秋雨の天候選択重みや日照補正を削除・無効化する決定ではなく、それらの既存仕様・バランスHOLDは §4.1 のまま維持する。再検討には具体的なプレイ上の利点と不利益の比較が必要。
- **既存樹木のリテクスチャ**（ヤナギ・マツ・タケ）は別工程とし、今回の5種類の完了条件に含めない。
- **スギ・ヒノキ**は今回の季節性アップデートに含めない。将来の森林植生拡充で検討する対象として保持し、採否そのものを撤回・確定しない。
- 既存の湿地植生、山野採集、既存Def/セーブ互換等の正式契約を変更しない。2026-10-10のスギ優先度Aや当時のStep 3作業順は**今回の季節性アップデートへのスギの自動編入根拠にならない**。
- 本節は今回の作業範囲について従来の一般的な植生追加優先順位より優先する。個別植物の正式仕様・採用画像・テスト根拠は、それぞれの正本で確認する。



**Target:** RimWorld 1.6  
**Repository:** `sucRo-RimWorld/Ancient-Medieval-Japan-Environment`  
**Role:** standalone sister mod for Japan-oriented climate, terrain, rivers, biomes, vegetation, weather, seasonal scenery **and regionally constrained wild food foraging** (latter is accepted future scope, not shipped functionality).

## 1. Responsibility boundary

Environment is independent from AMJ Core.

**Standalone Japan-environment contract (2026-10-09 author decision):** Environment must provide its ancient-to-medieval Japanese natural environment on the baseline RimWorld/Harmony installation **without requiring other vegetation Mods, Medieval Overhaul or optional DLC**. Its required landscapes and vegetation remain Environment-owned **even if an existing Mod already supplies equivalent species or functions**. VE, non-VE, MO and DLC comparisons inform **optional compatibility only** (duplicate spawns, natural distribution, patch load order and licenses), not removal of Environment species or migration of that scope to another Mod. The shared VE/non-VE implementation-entry audit and formal evidence record still apply, as do accepted-art, historical-description, save and runtime gates. This does not expand Environment into crops, medicinal treatments, processing or harvesting subsystems owned by other AMJ Mods.

Environment owns:
- world-map climate;
- temperature distribution and seasonal range;
- elevation and hilliness distribution;
- coastline shape and coastal-tile frequency;
- world rivers and their size/frequency;
- biome definitions and placement;
- **Environment-owned landscape vegetation composition**, including the existing Shii/Beech and all new native non-food and **wild-edible** scenery, its original tree forestry and regional distribution;
- regional tree distribution, ordinary native-tree sowing contracts, and natural wild-edible resources (the twelve approved foods, food Things, ecological spawn, gathering and minimum safe-preparation rules; see [Foraging Integration](ForagingIntegrationDesign-ja.md));
- weather/environmental regional differences;
- seasonal scenery where it affects the environment.

Environment does not own:
- cultivated field-food/crop gameplay owned by Grains/Rice/Crop Growth Balance; long-term food preservation, brewing, broader cooking chains and unrelated hunting/fishing/coastal gathering. The *minimum processing required to avoid treating unsafe raw wild food as edible* is within Environment's wild-food acquisition contract (not a wholesale transfer of cooking/preservation systems);
- cultivated crop Defs;
- crop yield, growDays, fertility sensitivity or sowing skill;
- rice paddies or agricultural production buildings;
- crop processing;
- cooking;
- animals merely for regional flavor;
- unrelated farming automation.

### Save-compatible vegetation development order (approved 2026-10-08)

AMJEでは、作者の既存セーブで繰り返しニューゲームが必要になる変更を減らす。**ワールド／Biomeの配置・地形・海岸・河川など、生成済みワールドやマップへ遡及しない変更を優先して固める**。独自Biomeの追加や全体の再配置は既存ワールドへの完全反映を前提にせず、意図した環境を評価する場合はニューゲームを必要とする変更として扱う。既存マップ上の初期植生は、新規定義／自然発生プールを変更しても即時に再生成されない。

その後に追加できる**Environment所有の日本在来景観樹木・野生食料植物**および既存植物のリテクスチャは、**AMJE導入済みの既存セーブへ後から適用できる設計を優先する**。2026-10-17作者決定により、別Mod案のWild Food ForagingはEnvironment大型更新へ統合する。新規果樹・山菜・菌類・自然薯等のPlantDef/食材/画像/自然分布/採集処理はEnvironmentが所有する。既存のスダジイ・ブナはDefを維持し、スダジイの種実採集を同一Mod内の独立動作で付与する。詳細は[山野採集統合設計](ForagingIntegrationDesign-ja.md)。

- **Environment所有の新規景観樹木・食用自然植物**は同じBiomeの `wildPlants` と必要な新規 `ThingDef` を使用し、分布・成長・採集をEnvironment内で整合させる。旧Foragingから独立した重複Defや、既存スダジイを食材専用に置き換えるDefを作らない。植生の自然発生には時間・気候・地形・密度などの条件があり、即時出現や古い植生の一括置換を保証しない。
- 通常植林できる樹木は上記の **Regional tree sowing contract**（地域内の自然分布、Vanilla樹木栽培研究、`mustBeWildToSow`等）を維持する。全樹木・全果樹を一律に植林可能にする方針ではない。
- 既存Defの画像だけを差し替えるリテクスチャは、DefName・アイテム挙動・保存データ構造を変えずに途中導入可能な形を原則とする。ゲームロジック・保存形式を変える場合は「画像のみ」と見なさず個別監査する。
- **AMJE未導入セーブへの初回導入**と、**AMJE導入済みセーブへの植物追加アップデート**は別の互換性問題である。前者は未検証で、ワールドを再生成しない。AMJEそのものの途中削除を安全と宣言しない。
- この方針は設計目標であり、将来の新規植物がすべてセーブ互換と実証済みという意味ではない。特に自然発生・実際の収穫・植林Jobが成立するかはリリース前の既存セーブ移行E2Eで確認し、未検証項目を明示する。

**回帰ゲート:** Environment所有の新規植生・木材等の更新時には、AMJE旧版で作成したセーブを新版でロードする前後比較を自動化する。統合する新規食用種・採集処理はEnvironmentの最終生産ソースに対して検証し、従来のForaging診断実験を本番PASSとして流用しない。ロード済みDef／Biomeプール、既存マップの保持、経時の自然発生、成熟後の果実・木の実の取得、研究前後の植林候補と実際の植林、セーブ・再ロード、所有ERRORゼロを確認する。静的なDef／メニュー判定だけで「既存セーブへ途中導入可能」と判定しない。基本／MO／CCTO／MO+CCTOの互換プロファイルを区別し、ゲーム起動テストは既存の非対話ランナー・ログゲートを使う。テスト仕様・実装状況は `Docs/GoldenPaths/PlantSowingTests.md` に置く。**現時点で移行E2Eは未実装・未通過**。

### カバノキ類の2樹種化（2026-10-30 作者確定、2026-11-01 実装PR #310）

**目的:** 冷温帯林の白いシラカバと亜高山帯の灰白～淡褐色のダケカンバを、別のThingDef・名前・画像として表示し、植生帯の景観を区別する。単なるVanilla Birchの改名・全Biome共通リテクスチャではない。

| 項目 | シラカバ | ダケカンバ |
| --- | --- | --- |
| 新規DefName | `AMJ_Tree_Shirakaba` | `AMJ_Tree_Dakekanba` |
| 日本語名 | シラカバ | ダケカンバ |
| 英語名 | Japanese white birch | Erman's birch |
| 親Def | `DeciduousTreeBase` | `DeciduousTreeBase` |
| AMJE自然分布 | `AMJ_CoolTemperateForest` | `AMJ_SubalpineForest` |
| 初期置換候補のwildPlants重み | 現行Birchの0.60を移管 | 現行Birchの0.50を移管 |
| 植林 | `TreeSowing` 後、自然分布Biome内 | 同左 |
| 季節 | 葉あり・落葉・秋色・葉あり積雪（落葉時の枝積雪なし） | 同左 |

**2026-10-30 作者訂正（ColdBogと旧Birchの役割）:** 旧 `Plant_TreeBirch` を残す目的は **既存セーブのDef参照互換だけ**であり、現在・将来のAMJE自然分布や植林候補として残す理由にはしない。`AMJ_CoolTemperateForest` と `AMJ_SubalpineForest` の旧Birchは上記の新種に置換する。**`ColdBog` でも旧Birch 0.45を自然発生枠として保持しない。** ColdBogの樺の木枠は**シラカバをほぼ主体、ダケカンバを少数**とする（2026-10-30 作者選択）。旧Birchの重み合計 `0.45` を維持する暫定的な配分案はシラカバ `0.40`／ダケカンバ `0.05`（約89%対11%）。これは自然発生枠内の比率案であり、実ゲームでの成木本数比の保証ではない。地域／標高別の湿原分布を持たない現行Biomeの近似とし、この数値自体の独立バランス受入は未完了。本番 `Patches/VanillaWetlandVegetation.xml` の切替は新種Def・画像接続と同じ実装単位で行う。正式実装時にはColdBogと該当する地域植林候補から旧Birchを外し、他Mod由来の植物を保護する。Vanilla `Plant_TreeBirch` Defそのものは削除・改名せず、旧セーブに残る個体・他Mod所有Biomeの参照は破壊しない。既存マップの樹木が即時置換されるとは主張しない。

**データ値決定の境界:** 木材の種類・収穫量・伐採作業量・成長日数・美観・生育温度・枯死条件は、新しい樹種差を恣意的に設けない。実装時に現行Vanilla `Plant_TreeBirch` の読み込まれる値を確認し、差異を持たせる必要がないものは同等の挙動を維持する。現行重み0.60／0.50は**木本総量を維持する実装候補**として明記するが、自然発生本数・植林適地・ゲームバランスが検証済みの最終数値とはしない。新しい係数を導入する場合は独立したバランス判断が必要。

**2026-11-01 元ネイティブログの定量確認：** [BirchNative16の元ログ分析](ValidationEvidence/BirchNative16-20261101.md) により、同一固定seedのVanilla/CCTO初期マップでは冷温帯林にシラカバ854/856本、亜高山帯にダケカンバ155/158本、ColdBogにシラカバ1,141/1,084本とダケカンバ169/155本が自然生成された。ColdBog樺の木の実際の内訳はシラカバ87.1%/87.5%で、作者指定の「ほぼシラカバ」と定性的に一致。ただし**二つの環境は同じseed**であり、多seed・年間動態・植林Jobs/旧セーブへの増殖・経済的影響は検証していない。重み `0.40/0.05` は独立バランス最終受入未了の暫定値。元のQuickstart結果JSONはRimWorld **1.6.4871 rev600** と記録しており、rev598と記した旧記述はこのマトリクスには適用しない。両樹種の通常葉あり原画は2026-10-30採用済みで、台帳の `normal` に原画受入として反映。実機の無雪通常葉あり・未熟・UI・紅葉・季節推移は受入済みとしない。

**2026-11-01 RimWorld 1.6実機・検証根拠（PR #310）：** [樺2樹種の実機証拠](ValidationEvidence/BirchNative16-20261101.md) に原スクリーンショット（元画像のSHA-256固定）と画像切り出し、Vanilla/CCTO 10+10=20の実ゲームQuickstart PASS／Unity ERROR0／正確なソースコミット `56bfd6915935420c6e87872f58041e2d0e60e0ae` を記録。シラカバ・ダケカンバ双方の葉あり積雪・落葉時枝雪なしをRimWorld 1.6.4871 rev598の正午Clear一時停止フレームで実見した。撮影Actionの**ジョブ結論はFAIL**（prefix付きUnityログをsummary抽出できないテスト集計不具合）、実際の6本ログ/PNG/Unity ERROR0とは区別し、Project PR #1171で集計修正済み。**作者の「画像問題なし」による葉あり積雪・落葉の実機静止外観受入は完了**。一方、年間切替、MO系、既存セーブ移行、ColdBogの独立数値受入は未完了。両樹種のPlantVisualCoverageは画像受入範囲ごとに分離し、`normal`（作者承認原画）・`snow`・`leafless` がaccepted、その他4状態はpending。なおWorkshop説明／ギャラリーの既知FAILは別リリースゲートであり今回の機能テストではNOT RUN。

**2026-11-01 本番実装候補（PR #310）：** 新規 `AMJ_Tree_Shirakaba` / `AMJ_Tree_Dakekanba`、採用済み葉あり／枝分かれ削減後の落葉PNG、対応する256px RGBA無圧縮DDS（9 mip、画像上下向きは実機未検証）、自然分布・植林期待値・日英表示を追加。Vanilla Birchの20成長日／伐採800／木材27を引き継ぎ、旧 `Plant_TreeBirch` Defは既存セーブ参照のため残す。Vanilla Birchの紅葉キーとLUTを暫定接続し、Nutrition=2／食性を明示して旧Birchの樹木資源・被食性を維持する。CCTO併用時は追加数値を設定せず休眠を有効にする（実ゲーム確認待ち）。**2026-11-01 作者「採用で」により葉あり積雪の雪専用透過オーバーレイを2樹種とも正式採用し、`Art/Sources` の原本・本番PNG/DDS・各Defの `snowOverlayGraphicPath` に接続済み（PR #310）。** 元の全木積雪画像ではなく、採用された雪のみのPNGを正確に複製。後続のRimWorld 1.6実ゲーム画像（正午12時/Clear/一時停止）についても作者が「画像問題なし」と承認したため、**葉あり積雪と落葉静止状態の実機外観は受入済み**。自然な季節切替・未熟株・アイコン・紅葉・旧セーブ移行・1.7は未検証。落葉時は既存 `LeaflessNoSnow` を使用。ColdBogの0.40/0.05は暫定配分・独立バランス受入待ち。PlantVisualCoverageの2樹種×7状態のうち **normal/snow/leafless がそれぞれaccepted**、他4状態はpending（normalは原画受入であり無雪完成形の実機撮影とは区別）。自然季節推移・実植林・旧セーブ移行などの全機能受入や配布完了は主張しない。

**2026-11-01 落葉画像更新（現行採用）：** 両樹種の落葉画像は作者が1枚の二本組画像として直接再添付した枝数削減版を採用し、左右に分離した。元の二本組原バイトは `Art/Sources/Things/Plant/AMJ/Birch_Leafless_Pair/Birch_Leafless_Pair_Approved_20261101.png`、個別の大判分離原本と256×256 RGBA派生は `Art/Sources/Things/Plant/AMJ/{Shirakaba,Dakekanba}_Leafless/` および `Art/Candidates/{Shirakaba,Dakekanba}_Leafless/` に保存した。2026-10-30旧落葉原画・派生画像は `Art/Archive` へ保持し、現在の落葉外観への承認より優先しない。葉あり採用画像は不変。**原画採用時点では本番Def実装やDDS描画が未完成。後続のPR #310でPNG/DDSとDefを追加し、2026-11-01の実RimWorld 1.6比較画像に対して作者が「画像問題なし」と承認した。受入範囲は葉あり積雪と落葉の静止状態に限定する。**

**画像・描画:** PR #293で承認した各原画を保持する。新種専用葉あり・落葉画像と季節色シェーダーを接続し、葉あり積雪画像が樹木全体である場合はそのまま `snowOverlayGraphicPath` に指定せず、**葉あり用の雪だけの透過オーバーレイを本体と同じ画面位置・寸法で派生生成**して接続する。落葉状態には枝積雪を追加せず、上述の完全透明グラフィックを指定する。未承認の画像状態を仮画像や別樹種画像で代用して配布しない。紅葉・積雪・未熟・UI・実際の状態切替は別途本番Defで検証する。

**既存契約との同期:** 実装時は `Defs/ThingDefs_Plants/` の両新規Def、`Defs/BiomeDefs/AMJ_Biomes.xml` の該当2エントリ、下記植林候補表・植林テスト期待値、英日DefInjected、状態画像および既存セーブのロード・残存・自然発生・植林・再保存を一貫して変更・確認する。ColdBogの旧Birch参照もシラカバ主体・ダケカンバ少数の新樹種配分へ置換する（新Def実装と同時に本番パッチを変更する）。本節の2026-10-30記述は当時の確定設計である。現在の本番コードへの実装候補は上記PR #310を参照し、実ゲーム受入とは区別する。

### Regional tree sowing contract

**Environmentが所有する既存景観樹木・新規景観樹木・新規野生食用樹木**の地域分布と、それに連動する通常の植林可能樹種はEnvironmentの責務とする。新規食用クリ・オニグルミ系・カヤも既存樹木のTreeSowing研究と自然分布地域限定の植林条件を踏襲し、承認前の栽培範囲の全国拡張や既存候補への暗黙の変更をしない。作物栽培の所有とは分け、AMJE樹木もVanillaの樹木栽培研究後に、その樹種が自然分布する地域で植林できるものとする。

`TreeBase` / `DeciduousTreeBase` 由来の `sowTags=Ground`、`sowResearchPrerequisites=TreeSowing`、`mustBeWildToSow=true` を保持する。AMJE管理Biomeでは、自然植生から除外した通常樹木を植林候補にも残さない。Vanilla Def自体の削除や、他Mod所有Biomeでの植林制限は行わない。

| AMJE Biome | 樹木栽培研究後の通常植林候補 |
|---|---|
| `AMJ_WarmTemperateForest` | `AMJ_Tree_Shii`, `Plant_TreeMaple`, `Plant_TreeBamboo` |
| `AMJ_CoolTemperateForest` | `AMJ_Tree_Beech`, `Plant_TreeOak`, `Plant_TreeMaple`, `AMJ_Tree_Shirakaba`, `Plant_TreePine` |
| `AMJ_SubalpineForest` | `AMJ_Tree_Shirabiso`, `AMJ_Tree_Dakekanba` |
| `AMJ_AlpineZone` | 通常樹木なし |

`AMJ_Shrub_Haimatsu` は自然低木・伐採資源として残し、植林候補には含めない。自然生成される木本と農業ゾーンから植林可能な通常樹木は、関連するが**同一集合ではない**。RimWorld 1.6ではハイマツが `plant.IsTree` として数えられるため、高山帯の自然木本期待集合は `{AMJ_Shrub_Haimatsu}`、通常植林候補は空集合とする。実行時Quickstartsは両者を別々に検証し、研究のロック／解除とハイマツの植林不可も確認する。候補変更時はBiome、上表、静的契約テスト、ロード後の両期待値を同じ変更で同期する。

回帰手順と静的／実行時検証の区別は [PlantSowingTests](GoldenPaths/PlantSowingTests.md) を参照。実際のメニュー判定はGround適合だけでなく `PlantUtility.ValidPlantTypesForGrowers` と `Command_SetPlantToGrow.IsPlantAvailable` の両段階を使い、ロード後の継承・研究・野生分布条件を検証する。

AMJ Core remains responsible for the minimum Terrain/Defs required for its agriculture to function. Environment may alter the *distribution/context* of land, but does not become a prerequisite for Core agriculture.

Environment remains technically standalone: AMJ Core, CCTO and Medieval Overhaul are not hard dependencies.

**2026-10-23 author correction:** AMJ is independent and MO is an **optional compatibility target**, not the content or design authority. An MO-owned biome/plant has no automatic right to remain in an ancient-to-medieval Japan world. Select content for Japanese period/ecological fit and gameplay need, then provide targeted MO integration without preserving unsuitable content. Specifically **MO `DankPyon_DarkForest` is excluded from natural generation**, while retaining the Def for references and preexisting saves. The earlier generic MO biome-retention policy is superseded.

**Harmony is the sole current technical dependency.** It is used for the post-Vanilla terrain transformation and for temperature-runtime hooks that RimWorld 1.6 does not expose cleanly through Def/XML.

### Public replacement scope

AMJE replaces and reconfigures the generated Vanilla terrain, vegetation, and baseline biome composition for a Japanese-archipelago-inspired environment. This describes the generated result, not wholesale deletion of Vanilla Defs or replacement of the engine pipeline. `EnvironmentTerrainProcessor.Apply` transforms root-surface elevation/coastlines, hilliness, climate and biome selection after Vanilla terrain generation. The four AMJE biome workers take precedence over broad Vanilla workers on eligible land, while wetlands and stronger compatible specialized workers can remain. The current Beta biome mixes still reuse some Vanilla vegetation provisionally alongside AMJE representative plants, but Vanilla vegetation has no default retention right: each reused plant must be re-audited and removed/replaced unless its Japanese distribution, period fit, ecological role, and presentation justify retention. Natural soil distribution is rebalanced without replacing every TerrainDef. Local River / Coast mutators and Vanilla weather/snow/deciduous systems remain the implementation foundation. Public summaries must state both the changed environment and these reuse/coexistence boundaries.

## 2. Design goal

The target is not a literal GIS reconstruction of Japan and not one specific historical year.

The world should produce a **pre-Edo Japan-like environmental gameplay profile**:
- temperate to cool, humid climate;
- meaningful hot summers and cold winters;
- little true tropical or polar land;
- limited flat land;
- abundant hills and mountains;
- short, relatively frequent small-to-medium rivers;
- no naturally generated continental-scale rivers;
- long, indented coastlines with bays, peninsulas and islands;
- environmental differences that matter to agriculture, preservation and resource use.

Historical fidelity is subordinate to gameplay. Values are abstractions intended to reproduce the environmental decisions of living in the Japanese archipelago.

## 3. Alpha world-generation targets

The values below are **Alpha baselines**. They are authoritative starting values, not final balance. Runtime world-generation statistics and playtesting may adjust them.

### 3.1 Temperature

#### Annual mean temperature

Target generated land range:

- minimum annual mean: **-8 C**
- maximum annual mean: **20 C**

Typical lowland bands:

- southern warm lowland: **17–20 C**
- central temperate lowland: **12–16 C**
- northern cool lowland: **5–9 C**

High elevations may fall below the lowland bands through elevation cooling, but land annual means remain clamped to the global Alpha range.

#### Elevation cooling

Initial target lapse rate:

- approximately **6.0–6.5 C per 1000 m**

This replaces Vanilla's much broader planet-scale climate behavior with a Japan-oriented range.

#### Seasonal variation target

Seasonal amplitude should remain large enough for both summer heat and winter cold:

- warm south: approximately **±8 C**
- central region: approximately **±11–13 C**
- cool north: approximately **±15–16 C**

Exact curve values are implementation details and may be adjusted after generated-world sampling.

#### Daily temperature variation

RimWorld's actual outdoor temperature includes seasonal shift, a fixed day/night sun-cycle component, and a separate daily random variation. Because CCTO evaluates the plant's actual ambient temperature, a short cold dip can cross a plant's lethal threshold even when the seasonal mean is much warmer.

Alpha policy:

- keep the Vanilla day/night sun-cycle amplitude unchanged initially;
- reduce the separate daily random temperature variation from Vanilla's approximately **±7 C** maximum scale to approximately **±3 C**;
- if playtesting still causes excessive one-night crop deaths, adjust this random component before weakening the crop-specific CCTO thresholds.

This keeps weather variability while avoiding a continental-style random swing overwhelming the Japan-oriented seasonal climate.

### 3.2 Elevation

Target elevation domain:

- ocean floor generation floor: approximately **-300 m**
- land maximum: approximately **3800 m**

Land distribution should be strongly weighted below 1500 m.

Conceptual elevation bands:

| Elevation | Intended role |
|---|---|
| 0–200 m | coastal plain, floodplain, basin lowland |
| 200–600 m | foothills and low uplands |
| 600–1500 m | common mountain land |
| 1500–2500 m | high mountains |
| 2500–3800 m | rare alpine/high mountain tiles |

Values above 3000 m should be uncommon.

Current Alpha implementation applies additional relief only to Large Hills / Mountainous / Impassable tiles after coastline/land classification, so lowlands remain low while rare highland peaks can approach the upper range.

### 3.3 Hilliness

Initial land-tile distribution target:

| Hilliness | Target share |
|---|---:|
| Flat | 25% |
| Small Hills | 20% |
| Large Hills | 25% |
| Mountainous | 25% |
| Impassable | 5% |

This is a gameplay abstraction of a mountainous archipelago. "Mountainous country" is represented across Small Hills, Large Hills and Mountainous rather than making most settlements Mountainous/Impassable.

The target should be evaluated statistically over multiple generated worlds rather than requiring exact percentages per seed.

Current Alpha implementation uses a spatial ruggedness score (terrain noise plus elevation influence), ranks generated land tiles by that score, and maps the resulting bands toward the 25/20/25/25/5 target. This replaces the first prototype's incremental promotion rules, which overproduced Small Hills and underproduced Mountainous terrain.

### 3.4 Rivers

Japan-oriented river design:

- increase the number of river-bearing tiles;
- favor **Creek** and **River**;
- do not naturally generate **LargeRiver** or **HugeRiver**;
- retain relatively narrow map-scale rivers;
- let high rainfall and mountainous terrain create a dense network rather than one or two continental rivers.

Initial RiverDef Alpha values:

| RiverDef | spawnFlowThreshold | spawnChance | widthOnMap |
|---|---:|---:|---:|
| Creek | **30,000** | **0.70** | 4 |
| River | **75,000** | **0.80** | 6 |
| LargeRiver | **disabled for natural spawn** | — | unchanged for compatibility |
| HugeRiver | **disabled for natural spawn** | — | unchanged for compatibility |

Implementation rule:
- set LargeRiver/HugeRiver natural spawn threshold to a non-spawning value while leaving the Defs present for compatibility;
- naturally spawned rivers are therefore Creek or River only;
- do not delete the larger RiverDefs.

These values must be re-evaluated together with the Environment rainfall model because RimWorld river flow accumulates rainfall.

### 3.5 Coastline and islands

Environment should increase the proportion of **land tiles adjacent to ocean** without merely reducing total land area.

Target form:
- more bays;
- more peninsulas;
- more medium/small islands;
- more irregular coastlines;
- fewer very large uninterrupted continental interiors.

Initial statistical target:

- at the same planet coverage, **coastal land tiles ≈ 1.5× Vanilla baseline**.

This is a comparison target, not an exact per-seed guarantee.

Total land fraction should remain playable. The coastline goal must be achieved primarily by increasing coastal complexity rather than flooding large amounts of land.

## 4. Rainfall baseline

Japan-oriented generation should strongly reduce dry continental interiors.

Initial broad target for most settled land:

- approximately **800–3000 mm/year** equivalent RimWorld rainfall values.

Extremely dry biomes should become uncommon in the natural Environment profile.

Rainfall design must be tuned together with river flow because RimWorld 1.6 river generation accumulates tile rainfall.

Detailed regional rainfall/monsoon/snow-country differences are a later detailed-design section and should not be hard-coded before generated-world sampling.

**0.3.0 annual-rainfall source audit (2026-10-22):** [Rainfall distribution audit](Research/RainfallDistributionAudit-0.3.0.md) confirms the existing root-surface world-generation transform uniformly applies `clamp(800 + originalRainfall × 0.55, 800, 3000)` before biome selection; no biome-specific rainfall target is implemented. The four AMJ forest/altitude biome workers require `>=800mm`, which cannot distinguish land after this transform if input rainfall is nonnegative. Regional rain-side differentiation is WorldMap-owned. **Do not assert biome rainfall has been rebalanced in 0.3.0**: distribution and river comparison evidence remain missing, so retain numeric constants and document the gap instead of introducing new arbitrary biome rainfall values.

### 4.1 Provisional Tsuyu/Akisame seasonal weather selection (2026-10-21)

**Ownership:** Environment's existing Japan-oriented root-surface biomes, including its four owned forests/altitude bands and the retained Vanilla `TemperateSwamp` / `ColdBog`. This is a first playable weather-frequency slice, not a final historical quantitative calibration or a new humidity statistic. Japan Sea/Pacific side or WorldMap projection corrections remain WorldMap future work. Non-AMJ biomes and non-root layers retain Vanilla/third-party weather.

**Decision and evidence:** [Seasonal Weather prior-art audit](Research/SeasonalWeatherExistingModAudit-2026-10-21.md) and [VE/non-VE implementation decision](Research/SeasonalWeatherImplementationDecision.json). Seasonal Weather (Continued) replaces shared biome commonality lists at broad season changes; Environment instead chooses a small `WeatherDecider.CurrentWeatherCommonality` Harmony postfix so it only changes positive per-selection weights for known Vanilla `WeatherDef` names, with no shared Def mutation, new weather/event Def, external content dependency or Dynamic Atmosphere graphic override.

| Local 60-day year | Approximate Japanese phenomenon | Provisional weather multipliers |
|---|---|---|
| Days 16–22 inclusive (7 days) | Tsuyu / early-summer rainy period, compressed June to July | `Rain`, `FoggyRain` x1.65; `RainyThunderstorm` x1.08; `Fog` x1.15; `Clear` x0.75 |
| Days 30–38 inclusive (9 days) | Akisame / early-autumn rain, compressed September to October | `Rain`, `FoggyRain` x1.40; `RainyThunderstorm` x1.12; `Fog` x1.15; `Clear` x0.85 |
| Days 23–29 inclusive (7 days) | Midsummer clear weather after Tsuyu | `Clear` x1.25 only; all other weather weights unchanged |
| Days 39–44 inclusive (6 days) | Autumn clear weather after Akisame | `Clear` x1.25 only; all other weather weights unchanged |
| All other days | Baseline weather | No change |

**Recompiled x0.90 Environment on actual 1.7 (2026-10-23):** [Project manual Waylith Actions #39808526633](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39808526633), [artifact #12439495796](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39808526633/artifacts/12439495796), exact implemented source **`de6d6821911326e4cc9c0f7823db62b11730b583`**. Genuine isolated RimWorld **1.7.4977 rev1248**, 12 production C# files compiled against original 1.7 engine, real 50×50 map and Harmony/Quickstarts/Pickle original-engine loaded scenario **1/1 PASS, ERROR0, exit0, FMOD ERROR0**, runtime reflection confirmed **compiled RainGlowFactor=0.90** and `WeatherWorker.CurSkyTarget` + rain selection Harmony target methods resolved; marker `AMJ_ENV_RAIN090_NATIVE17 constant=True productSourcePinned=True` from real native Pickle. **Narrow acceptance only**: default 50×50 test map is not asserted to be one of six AMJE Japanese biomes; this does **not** measure native 1.7 Rain sky light, actual crop/solar under rain, full-year weather, save/load, other weather co-load, or a published 1.7 DLL. Original 1.6 loaded specific-biome Rain/Potato/Solar ratio remains independently proven above. No additional generic 1.7 runner should be built solely for these existing 1/1 checks.

**Dynamic Atmosphere compatibility scope, user-selected reference:** [Steam Workshop 3808191148](https://steamcommunity.com/sharedfiles/filedetails/?id=3808191148) currently documents 1.6 visual-weather/cloud map drawing as the normal mechanism, with **hurricane as an actual game condition** and lightning positions altered, and that its normal sky/cloud shadow graphics are largely cosmetic. Environment deliberately changes **gameplay sunlight** via postfixed Vanilla weather-worker target glow on supported biomes and weather selection odds without replacing custom cloud textures or triggering additional weather. Thus the two mods' *stated functional responsibilities* do not inherently demand a new compatibility patch. **NOT CO-LOAD TESTED:** the actual Dynamic Atmosphere DLL and shader bundle were not acquired/started together with Environment; Harmony method overlap/patch priority, user graphics options, sky/render interplay and hurricanes on AMJE biomes cannot be claimed PASS. **Version correction from current official Workshop metadata (2026-10-23):** [Dynamic Atmosphere Steam page](https://steamcommunity.com/workshop/filedetails/?id=3808191148&l=indonesian) now explicitly lists both **1.6 and 1.7** tags and was updated **2026-10-17**, superseding the old body copy still saying 'built for 1.6' and the earlier 1.6-only search snapshot. This supports current **declared 1.7 availability**, not compatibility proof with Environment. The author noted on **2026-10-18** that they had not tested Linux Vulkan renderer behavior; combined 1.7 Linux graphics/performance and any gameplay-light interaction remain **untested**. Require an actual source/binary co-load before calling DA/AMJE compatibility PASS. A real-provider co-load is a release/compatibility check only when the required binary is available, not a reason to add a proxy Mod or alter current normal-rain factor.

**Annual crop-output gate:** The previously measured original 1.6 same-weather model estimates rain-light effects but compares to **hypothetical continuous Clear**, not Vanilla's natural weather, and uses a single selected site/seed and the light component of one potato crop. **No annual harvest mass, crop maturity, year-round growing temperatures, rain impact on other crops or comparison to Vanilla actual annual production is measured.** Consequently source x0.90 is an accepted **player-difficulty compromise only** and **must not** be described as confirmed crop-yield parity; do not add a water/fertilizer simulator just to fit the absent harvest figure. Additional long-running simulations are unnecessary unless they can change a concrete release/gameplay decision.

**2026-10-28 scoped original-game seasonal Plant.GrowthRate comparison:** [Project Actions #40631111289](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/40631111289), current Environment source `dbf420f80a5f707cfd5e33646a41f281c683b984`, genuinely loaded 1.6 Vanilla + Environment, 2/2 original native checks PASS, Unity ERROR0, exit0. One unforced warm-forest 36.66°N site, three RNG-seeded native weather-event-duration-stepped 60-day calendars, actual unroofed Potato and Rice `Plant.GrowthRate` sampled under **identical weather/time/temperature** with Environment's extra weather-glow attenuation ON versus a test-only native cancellation. Annual ratios for both crops: **0.9131 / 0.9067 / 0.9154**, average **0.9117** (roughly **8.8% lower time-weighted growth-rate potential** due to the additional weather-glow factors on this site). Midsummer and Tsuyu had stronger potential losses; `Clear ×1.25` does not guarantee recovery. **Not full-game 60-day plant ticking, observed crop harvest, 0.2.0-vs-0.3.0 changing weather frequencies, MO+Agriculture, a justified excessive-difficulty threshold, or release acceptance.** The separate 0.3 Workshop combined-description length check remains independently FAIL (8,000-byte limit); native-only research candidate was explicitly non-release-eligible. Exact assumptions, per-phase ratios and limits: [SeasonalCropRateNative16-20261028](ValidationEvidence/SeasonalCropRateNative16-20261028.md). No gameplay coefficient changed and full food-production balance remains OPEN.

**Final source-owned x0.90 implementation native acceptance (2026-10-23):** [Project Actions #39806207946](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39806207946), [artifact #12438416647](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39806207946/artifacts/12438416647), source-pinned **Environment main `de6d6821911326e4cc9c0f7823db62b11730b583`** with **actual compiled x0.90 production patch**, genuine RimWorld **1.6.4871 rev600 Vanilla**, ColdBog surface. Existing season/save native Quickstart **29/29 PASS, Unity ERROR0, exit0** and genuine second-game-process save reload **PASS, Unity ERROR0, exit0**; no native test matrix expansion. At matched daytime original potato + solar panel: Clear sky **0.911**, Rain sky **0.820**; native `Plant_Potato.GrowthRateFactor_Light` **0.818 Clear → 0.632 Rain** (≈**22.7% lower light component**), real solar `DesiredPowerOutput` **1548.5W → 1393.6W** (≈**10% less requested**). Actual weather-worker sky factor against Clear: Rain **0.90**, FoggyRain **0.90**, RainyThunderstorm **0.70**, Fog **0.90** (all native glow ratios PASS). Ten fixed calendar boundary probes verify rain/Clear weight periods day **15/16/22/23/29/30/38/39/44/45** without forced weather. This verifies the new **source-built gameplay mechanisms only**, **not** final 60-day harvest/yield parity, real multi-seed rainfall duration, solar grid electricity, 1.7 loaded sky factors or third-party weather compatibility. **The subscriber/tracked DLL and Workshop item remain unchanged.** This source run supersedes earlier source-pinned x0.80 expectations for current production behavior, without retroactively rewriting those historical measurements.

**Current 0.3.0 rain daylight gameplay setting (author-approved 2026-10-23):** Ordinary **Rain and FoggyRain x0.90** in the six supported AMJE surface biomes, updated from previous trial x0.80. This is an explicitly chosen **gameplay-balance compromise** to preserve Japanese rainy-season weather while avoiding a disproportionately strong vanilla crop-light penalty. It does **not** assert historical irradiation calibration or Vanilla-equivalent harvests. **RainyThunderstorm x0.70, Fog x0.90 and weather-selection weights/windows remain unchanged and provisional.** No direct plant growth patch, water management, moisture system or forced sunny day is added. Native matched-weather source comparison [Project #39798567237](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39798567237) gives modeled potato light component 84.72% at x0.80 versus 91.31% at x0.90 of the same-hour hypothetical all-Clear sky (a 43.1% reduction in that *modeled* deficit, **not** a measured crop-yield increase). A natural Japan-like biome was sampled for one RNG seed; year-round actual field production and other crops remain unverified. Historical x0.80 test results below refer to their original pinned builds, not the newly adopted code. This source change still requires a matching test acceptance and separately authorized DLL/Workshop release.

**Calendar adjustment (2026-10-22):** In a 60-day year (about 6.08 real calendar days per game day), the Japan Meteorological Agency's **Kanto-Koshin 1991–2020 normal** Tsuyu onset around June 7 and retreat around July 19 imply about **7 game-days**, not the previous 9. In the approximate Jugust alignment, days 16–22 represent June–July, and the midsummer clear window begins on day 23. Japan-wide regional and year-to-year variability is real: these are generic Japanese-biome windows, **not fixed Tokyo weather**. JMA calls Akisame broadly late-August–October and notes regional variability, but gives no fixed statistical opening/closing dates comparable to Tsuyu; retain provisional days 30–38 pending representative weather frequency measurements rather than shortening arbitrarily. Restrict autumn clear to days 39–44 because day 45 begins Decembary; a global day-45/46 Clear bonus contradicted the accepted winter regional-variation rule. Official sources: [JMA historical Tsuyu Kanto-Koshin](https://www.data.jma.go.jp/cpd/baiu/kako_baiu09.html), [JMA seasonal terminology, Akisame](https://www.jma.go.jp/jma/kishou/know/yougo_hp/kisetsu.html). The documented earlier nine-day native event observations remain historically correct **for the previous window only**, and should not be represented as empirical validation of these new windows.


- **Paired rain-light attenuation comparison (2026-10-22):** [Original RimWorld 1.6 run #39798567237](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39798567237) measured x0.80 current Rain/FoggyRain versus **test-only** x0.90 at identical original-game sky/weather moments, source pinned `f95d25563ece1f43b4ca4f8909c5fecae632d90e`, natural warm forest +36.66N, 44 event-duration-stepped events, 2/2 native PASS, ERROR0 exit0. For this seed, the independently derived potato *light-response factor model* relative to hypothetical always-Clear is **84.72% at x0.80** and **91.31% at x0.90** (light deficit 15.28%→8.69%, 43.1% decrease); this is **not Vanilla weather, actual elapsed plant growth or annual harvest**. The earlier weather-replay comparison [#39793818457](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39793818457) is a distinct FAILED comparison; the final test used no replay. **Historical observation status at that source pin:** x0.80 was unchanged and x0.90 had not yet been approved. **Superseded for current 0.3.0 source** by the author-approved x0.90 gameplay choice above; full crop-yield acceptance remains OPEN. Exact raw evidence, sky ratios by season and model caveats: [Tsuyu/Akisame calibration](Research/TsuyuAkisameClimateCalibration-2026-10-21.md). No need to add water/watering simulation for this proposed benefit.
- **Current 7-day Tsuyu native evidence (2026-10-22):** [Project genuine 1.6 run #39790847279](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39790847279), source-pinned `f95d25563ece1f43b4ca4f8909c5fecae632d90e`, natural warm forest **36.66°N**, 2/2 original Quickstart PASS, ERROR0/exit0, *one event-duration-stepped 60-day weather sample*, not a full game-tick simulation. Observed rain-time share **Tsuyu7 61.1%, Midsummer7 61.9%, Akisame9 20.0%, Autumn6 36.6%, Other31 25.5%**; seasonal sky glow/clear ratios 0.871/0.879/0.958/0.904/0.956. Potato light-factor **threshold-model** (not live crop) estimates ~**-15.3%** integrated against a hypothetical all-Clear weather at this site's sampled light schedule. **The midsummer Clear x1.25 period remained as wet as Tsuyu in this single seeded observation**, so merely shortening the rainy-season window has not established seasonal sunshine recovery or acceptable yields. The observed long weather event transitions/seed effects need discrimination before another numeric change. Research details and caveats: [Tsuyu/Akisame climate calibration](Research/TsuyuAkisameClimateCalibration-2026-10-21.md). Existing source multipliers and no-weather-forcing policy unchanged; not published.
- **0.3.0 original RimWorld 1.7 build/load acceptance (2026-10-22):** [Project run #39773104147](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39773104147) pinned Environment owner `0c73f88c1a0039205c740910701fbc3dc6bfa5dc`, compiled all 12 owner production C# files against original **1.7.4977 rev1248**, loaded on a genuine 50x50 map with Harmony/Quickstarts/Pickle: **Pickle 1/1 PASS, Unity ERROR0, FMOD ERROR0, exit0, not timed out**; `RainWeatherDaylightPatch` and `SeasonalRainWeatherPatch` symbols/targets resolved and owner bootstrap confirmed. This is a **source-built 1.7 smoke** only, not 1.6-distributed binary acceptance, full 1.7 world generation, real seasonal weather event frequencies, crop/solar output, or DA compatibility. Native output [artifact #12423207779](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39773104147/artifacts/12423207779). No publication.
- **0.3.0 native acceptance (2026-10-22):** [Project run #39765122226](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39765122226) built the production DLL from exact Environment source `0c73f88c1a0039205c740910701fbc3dc6bfa5dc` and ran original RimWorld 1.6.4871 rev600 Vanilla `AMJWorldWetlandDistributionQuickstart`: **24/24 PASS, Unity ERROR0, process exit0**, original native full world-generation load. This establishes **build/load/map-generation compatibility**, not the observed number of sunny days, plant growth/yield effect, solar power output, full 60-day climate simulation, Dynamic Atmosphere co-load, or RimWorld 1.7 runtime behavior. DLL is a test artifact only; Steam unchanged.
- **Midsummer and autumn clear-weather x1.25 are preliminary gameplay weights, not seasonal rainfall or growing-season compensation targets.** They increase the *relative probability* of an already eligible `Clear` candidate without reducing any other weather's base weight directly. Rain can still occur. They do not guarantee annual sunlight, crop yield or solar energy preservation. Winter remains unmodified because Japanese winter sunshine strongly varies between Japan Sea and Pacific sides; regional winter specialization belongs with the WorldMap owner.
- If the currently running weather is wet (`rainRate > 0.1`), a subsequent `Rain` or `FoggyRain` candidate within either window receives a further **x1.15** bonus. This makes wet-to-wet transitions more likely without forcing weather, modifying duration or bypassing native repeatability restrictions. All numbers are **provisional gameplay parameters**, not meteorological measurements or author-final balance.
- Map-local `GenDate.DayOfYear` determines the window. On tiles south of the engine's equator, shift the day phase by 30 game-days so the rainy interval follows the local warm season. This is intentionally **not a fix for the optional WorldMap's inflated virtual latitude**.
- The postfix returns unchanged if native commonality is zero/negative, no matching existing `WeatherDef`, nonselected biome, invalid/non-root tile, or outside the four seasonal windows. Other mods' custom weather identities and all AMJ weather base lists are left untouched. Existing temperature, rainfall, drought prevention, tutorial and fire behavior stays native.
- **Annual tile rainfall (800–3000mm), biome selection and river layout remain unchanged.** This only redistributes in-game *weather choice tendency* for selected parts of the 60-day year. It does **not** enforce conservation of simulated annual precipitation, guarantee continuous rain or claim calibrated cloud/rain amounts.
- **Rain/cloud darkness (provisional playable implementation, 2026-10-21):** Vanilla `WeatherWorker.CurSkyTarget` ordinarily sets gameplay `SkyTarget.glow` directly from celestial sunlight regardless of the weather's sky color, so ordinary rain/fog tint alone does **not** reduce `Plant.GrowthRateFactor_Light` (via `GlowGrid.GameGlowAt`) or `CompPowerPlantSolar.DesiredPowerOutput` (via `SkyManager.CurSkyGlow`). `RainWeatherDaylightPatch` now decreases the **weather worker's resulting glow before Vanilla blends new and previous weather** on AMJE's six existing owner biomes; weather/sky color and cloud/shader visuals remain native/other-Mod-owned. Current factors: `Rain`/`FoggyRain` **x0.90** (**adopted gameplay balance; not meteorological calibration**), `RainyThunderstorm` **x0.70** (provisional), `Fog` **x0.90** (provisional), `Clear` and all other weather **x1.00**. They apply to rainy weather **all year**, so the increased frequency during Tsuyu/Akisame organically produces a larger accumulated light deficit, without directly changing plant growth or another Mod's crops. Transitions use native sky blending; roofs and artificial light keep native `GlowGrid` behavior; real game conditions that darken the sky remain separate. The CPU path is constant-time per weather-worker sky sample (no tick-wide plant scan, map-wide tile scan or allocation). The normal-rain x0.90 setting uses original-game same-weather exposure as a **gameplay compromise**, not measured Japanese irradiance; thunderstorm and fog remain uncalibrated, while annual yields and full Vanilla parity remain untested.
- **Compatibility boundary for light:** Dynamic Atmosphere visual cloud shadows and jet streams are independently drawn; the Environment hook intentionally changes **gameplay sunlight** while leaving its cloud/shader/precipitation effects alone. Do not claim cloud-shadow-level gameplay irradiance or complete compatibility until a real joint load. If a third-party Mod adjusts `WeatherWorker.CurSkyTarget`, check actual patch order before judging sunlight. No separate plant or solar patches; weather attenuation must never be counted a second time as a global growth debuff. Annual rainfall, rivers and WorldMap projection untouched.
- **Later natural-site evidence (2026-10-21):** A 100%-coverage original 1.6 Vanilla generated world yielded a **naturally generated warm-temperate forest at +36.66°N, 15.82°C and 1560.4mm annual precipitation**, without altering tile climate/biome. [Project run #39596824609](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39596824609) is **1/1 PASS, Unity ERROR0**. One seed, native weather-event duration stepping (not full-year map ticks), observed 9-day Tsuyu **89.8/216.3 wet hours, sky/clear glow 0.891**, 9-day Akisame **43.1/215.3 hours, 0.958**, and remaining 42 days **342.8/1008.3 hours, 0.931**. Thus Tsuyu wet share exceeded ordinary days but Akisame did not in this one sample. **The representative-site gap is resolved for this observation only; seasonal weights, glow factors and crop yields remain quantitatively unvalidated**. The raw source/candidate, exact limits and cleanup decision are in [the calibration evidence](Research/TsuyuAkisameClimateCalibration-2026-10-21.md). Neither production numbers, WorldMap nor Workshop changed.
- **Historical/quantitative calibration HOLD (2026-10-21):** See [the Tokyo JMA 1991–2020 normal comparison, native potato sensitivity and elapsed-weather acceptance criteria](Research/TsuyuAkisameClimateCalibration-2026-10-21.md). JMA mid/late-June whole-period global irradiance (`14.3`, `13.2 MJ/m²/day`) is lower than late-May (`17.8 MJ/m²/day`) but **these are all-weather period means**; individual Rain Def glow x0.80 cannot be accepted/rejected by equating it to the JMA period ratio, and sunshine hours do not substitute for irradiation. Existing single-point rain factor 0.80 caused the verified potato light-growth component to fall about 45.5%; neither that nor the static 0.90/0.95 sensitivity cases measure whole-season yield. **Historical status at the 2026-10-21 source pin:** factors were unchanged and provisional; this statement is superseded for Rain/FoggyRain by the current x0.90 decision above. The 240 native fixed-state chooser draws are **not rain hours/days or persistent wet sequences**. A native observed weather-duration/elapsed sun sample on representative Japan-like biome (not only 50.8° ColdBog), in consistent units, is the gate before author-final numeric tuning. Changes must retain original annual tile rain/river outputs and avoid additional plant-specific growth patches or forced weather events. No runtime/Steam action authorized by this research record.
- **Japan-warm-forest follow-up (2026-10-21):** One-off AMJ WarmTemperate forest native 1.6 event-duration-stepped three-seed run [#39584097363](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39584097363) **PASS 1/1, Unity ERROR0, exit0** found 9-day Tsuyu wet hours **106.9 / 75.3 / 55.0**, Akisame **43.1 / 34.9 / 115.0**; sun/clear ratios Tsuyu **0.879 / 0.936 / 0.944**, Akisame **0.958 / 0.970 / 0.895**. **Not Japan-latitude verification:** chosen map tile +43.45°N, forced warm biome, annual rain1235mm and forced temp17.5°C; Kanto climate is materially different. Correct JMA Tokyo 1991–2020 annual norm **1598.2mm and 15.8°C** ([JMA](https://www.data.jma.go.jp/stats/etrn/view/nml_sfc_ym.php?block_no=47662&prec_no=44)). The next two geographical test-only runs [#39584824027](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39584824027) and [#39585527377](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39585527377) **FAILED** the expected 32–38°N land-tile premise in the generated 5%-coverage world; **no Tokyo-climate observation was executed**. No weather runtime error (raw Unity ERROR0 on both). **Stop repeated 5%-world tile probes; these cannot justify changing production numerical weather or daylight values.** Detailed evidence, nonrepresentativeness and valid limitations in [climate calibration research](Research/TsuyuAkisameClimateCalibration-2026-10-21.md). Temporary warm observation class and manual route are to be removed; normal terrain and ColdBog Quickstarts remain unchanged.
- **Bounded duration-stepped native observation and numerical decision HOLD (2026-10-21):** On one real RimWorld 1.6 ColdBog map at ~50.8°N, real engine weather choice/duration were used for **three 60-calendar-day, event-boundary-stepped models** (not 60 days of normal map ticks). [Project Actions #39581267737](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39581267737) passed **30/30, original ERROR0, and genuine save reload PASS**; formal per-seed 9-day Tsuyu wet hours were **79.1, 0.0, 35.0**, 9-day Akisame wet hours **88.1, 26.1, 94.3**, and daytime sky/clear ratios Tsuyu **0.895/0.988/0.965**, Akisame **0.919/0.977/0.905**. The three-seed results are small, the map is not representative of Japan's temperate regions, two-hour sample midpoint binning has ±~2h daily artifacts, and average rain hours are not observed JMA ≥1mm days. **Even x1.65 Tsuyu commonality can yield a fully dry nine-day episode in this model, so weights/persistence must not be equated with actual wet spells.** Full evidence and caveats in [Tsuyu/Akisame calibration research](Research/TsuyuAkisameClimateCalibration-2026-10-21.md). **No production rain/sky multipliers have been changed; do not add forced rain events or accept final values from this run.** The temporary event sampler was deleted from the persistent Quickstart after the one-off observation; existing weather light/plant/solar native assertions are preserved. Owner tracked subscriber DLL and Workshop still untouched.
- **New daylight source native 1.6 PASS, actual measured effects (2026-10-21):** Latest owner main [PR #243](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/pull/243), production implementation `cb288d90ba0d7a416df2b82d348bd65376c16923`; owner test-only repair [PR #244](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/pull/244) main `a29bf39703c5d9e753fbc937190f41a3d4f02641`. Project latest exact-source pin [PR #904](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/904), main `fa8f79ca2c0c686f4c79f278a771ce1731fb4ec4`; genuine isolated [1.6 Actions #39573958330](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39573958330) native ColdBog **28/28 assertions PASS**, error0, original exit0 and independent save reload PASS/error0/exit0. Measured actual native `WeatherWorker.CurSkyTarget` (same map and daytime): Clear `0.757`; Rain `0.606` x0.80; FoggyRain `0.606` x0.80; RainyThunderstorm `0.530` x0.70; Fog `0.681` x0.90. Exact rain-weight date samples remain 12/18/26/33/45 and 33/77/46/62/46 rainy choices per 240; logged Harmony postfix marker PASS. **Only glow ratio observed, not actual crop biomass/plant growth rates or live panel output.** Candidate DLL SHA-256 `daccdc05f1a65f562e4bdb14d8f758e420ffcdab4673b064546753027f79c498`, archive SHA-256 `9e7ce01060e34675b6bde55d94683352534094686241ebcb708d82a14d9b09fd`; test artifact remains independent of unchanged Git-tracked subscriber DLL. Source `a29bf39703c5d9e753fbc937190f41a3d4f02641` differs from first daylight production source only in quicktest-only `internal const` references. First [Actions #39573403274](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39573403274) built the new production assembly but failed the independently compiled Quickstart CS0117 due attempting to access production `internal` fields, **not** a game/native FAIL; kept historically failed, source test fix PR #244.
- **Latest 1.6 real crop and solar consumer acceptance (2026-10-21; later than the weather-worker-only check above):** Existing single ColdBog original-game scenario was extended in owner [PR #246](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/pull/246), merged `0f42f48e58ab379ddb65e47c4d719f184cbd25d1`, and test-API-only repair [PR #247](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/pull/247), merged `7b934dbc61a0874b2bf4ae405d28f74a2a186e89`. Project exact test source [PR #907](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/907), main `8b70019fa9a31286c38358f9fd95271d711b8be8`. Original-game [Actions #39576155780](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39576155780) with **RimWorld 1.6.4871 rev600 Vanilla+Environment, ColdBog** passed **29/29 Quickstart assertions, raw ERROR0, exit0 and independent genuine save reload PASS/ERROR0/exit0**. On the same unroofed map and game time, the test spawned an actual Vanilla `Plant_Potato` and `SolarGenerator`, called the loaded `SkyManager.SkyManagerUpdate` with Clear then Rain, and measured native potato `GrowthRateFactor_Light` plus real `CompPowerPlantSolar.DesiredPowerOutput`; after sampling it restored original weather/ticks and removed both temporary objects. Recorded actual values: Clear `CurSkyGlow=0.911`, potato light-growth factor `0.818`, desired solar power `1548.5W`; Rain `CurSkyGlow=0.729`, potato light-growth factor `0.446`, desired solar power `1238.8W`. **Thus 20% less weather sky glow causes ~45.5% lower *light component* of potato growth rate at this sampled daytime, while solar requested output drops 20%.** Actual unmodified potato Def light thresholds `growMinGlow=0.510`, `growOptimalGlow=1.000`. This is materially nonlinear gameplay and requires **explicit quantitative balance review before the provisional rain attenuation factors can be finalized**. It is *not* actual final crop growth per elapsed day, harvest quantity, or grid-connected delivered solar power; the one sampled ColdBog latitude and time must not be generalized to Japan-wide monthly sunshine data. Exact candidate archive SHA-256 `0a9b1c89c78c0f52e6ea1b48bf8730962657f9d7bec76981e9c5fd548e143dcd`; compiled production DLL SHA-256 `07519c9f979fd8a7904bcf44deb4b9e72fc5a8419b17c1121038ca9285f6a549`, retained in Actions artifact and **not installed in the owner main Assemblies directory**. The first test-only [Actions #39575700800](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39575700800) legitimately **FAILED before game startup**, because its separate 1.6 test DLL referenced an old public-decompile `GlowGrid.GameGlowAt` API no longer exposed by the actual RimWorld 1.6 assembly; owner PR #247 repaired this using the real `SkyManager.CurSkyGlow` while preserving live potato and solar reads. Do not count this first failure as a native game failure or retroactively mark its run PASS. No Environment production source, weather weights or published subscriber DLL changed in #246/#247.
- **New daylight source native 1.7 bounded PASS (2026-10-21):** Existing [1.7 Actions #39573408110](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39573408110), Project [PR #902](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/902), same production source `cb288d90ba0d7a416df2b82d348bd65376c16923`, genuine RimWorld 1.7.4977 rev1248; isolated native build of 12 production C# files PASS, new `RainWeatherDaylightPatch` and `WeatherWorker.CurSkyTarget` resolved on real loaded game, Pickle 1/1, ERROR0, exit0. It does **not** perform actual 1.7 glow-ratio test because the generic quickstart biome is not one of AMJE's weather-selected biomes. The test-only owner PR #244 cannot invalidate the unchanged production source's 1.7 smoke.
- **Verified scoped native 1.6 result (2026-10-21):** Project manual [Actions #39547041452](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39547041452) built and executed owner source `16caee72fcaaaafd48be1f1db2a7e43869a5edc4` with actual game 1.6.4871, produced subscriber candidate **60 files**, production DLL SHA-256 `4aa4be6e6ce2da1629e79898f81d7fbb877b314a3ec42e074d8bc9622c195675`, and executed the existing Vanilla ColdBog weather/calendar/save Quickstart **27/27 assertions PASS, actual ERROR0, native exit0**, followed by genuine save reload **PASS, ERROR0, exit0**. The loaded Harmony postfix identity and exact Rain/Clear multipliers across five samples were verified in actual 1.6 game code, with original ticks and random state restored.
- **Fixed-seed selector observations (not observed weather-day frequencies):** Native `WeatherDecider.ChooseNextWeather` invoked **240 times** at each calendar day in the one Northern Hemisphere ColdBog map (native latitude ~50.8°): day12 **33/240** wet choices, day18 Tsuyu **77/240**, day26 **46/240**, day33 Akisame **62/240**, day45 **46/240**. The absolute `Rain` chooser commonality is **2.331** outside the windows, **3.847** on Tsuyu day18 (x1.65), **3.264** on Akisame day33 (x1.40). These are weighted chooser draws at fixed weather state/seed, **not** 60 elapsed game-days, rainfall mm, a southern-hemisphere native run, or weather-duration observations.
- **Genuine 1.7 source-build and map-load acceptance (2026-10-21):** [Project Actions #39548357371](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39548357371), manual Project source [PR #900](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/900) main `760e2b024003c74894fd671b5b617c19fe9d5ee3`. Owner source `851f588b2a15846690ac155f6e043e250d0a5763`; real Waylith RimWorld 1.7.4977 rev1248 loaded isolated net10-built Environment DLL SHA-256 `e28e10ad632d79edf391495845204c80bbabcadad8ef303a89b899e2f6ee2ec1`. Pickle 1/1, original ERROR0/FMOD0, exit0; code verifies a real 50x50 map, four owned BiomeDefs plus thin soil, reflection-resolved `SeasonalRainWeatherPatch` type and 1.7 `WeatherDecider.CurrentWeatherCommonality` method, with one logged `AMJ_WAYLITH17_RAIN_HOOK_RESOLVED`. **This does not invoke or numerically verify 1.7's patched weather weights, or test gameplay precipitation/time-series, saves or Dynamic Atmosphere.** Original protected 1.7 game unchanged.
- **Genuine 1.6 MO+CCTO co-load, weather checks pass but strict gate FAIL (2026-10-21):** Existing native ColdBog Quickstart with profile `MO+CCTO` manually dispatched via Project [PR #901](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/901), main `5f6566e17037ca63bdf7b7b2c22f96ec8cf2b4d2`; actual [Actions #39548603977](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39548603977). Owner source `16caee72fcaaaafd48be1f1db2a7e43869a5edc4` compiled from genuine 1.6 references, DLL SHA-256 `eefb84da14cb2bf1744a6d7e40860fddd1fd08873efe369d092617ffc38500cb`. Loaded MO and CCTO together: 27/27 original Quickstart assertions PASS (failed0), Harmony weather postfix present; same five selected dates and weighted rain selection counts as Vanilla (day12 33/240, day18 77/240, day26 46/240, day33 62/240, day45 46/240) and exact direct weights PASS. Save file generated and native process exit0. **Strict native result FAIL: prelaunch ERROR2** exactly `Cannot create FMOD::Sound instance for clip "" (FMOD error: Unsupported file or audio format.)` twice; no in-scenario log errors. This matches the known original MO/SYR blank-audio baseline, not evidence of new Environment weather failure, but it is **not** waived in the original test gate. Since first phase was not strict-PASS, second-process save reload was intentionally NOT RUN. Does not establish CCTO-alone, 1.7 MO, ordinary play, or full four-profile PASS.
- **Remaining release HOLD and limits:** Initial [#39546718249](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39546718249) packaging-only test failure remains historically FAIL and was fixed in [owner PR #240](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/pull/240). Current main-tracked `Assemblies/AncientMedievalJapanEnvironment.dll` is still the older binary; isolated compiled candidate artifacts **do not** mean installed subscriber or Workshop/YADA upload. Actual 1.7 rain commonalities; Southern Hemisphere phase; warmer/mountain AMJE biome weather; standalone CCTO; optional Dynamic Atmosphere renderer/co-load/lightning; 60-day wet weather duration statistics; and long-term crop growth/yield and **grid-delivered** solar output remain **unverified** (one sampled potato light-growth factor and unconnected solar desired power are now verified in 1.6). Keep calendar windows/weights provisional and do not publish this as a finished climate compatibility matrix.

## 5. Implementation boundary: XML vs C#

### XML/Def/Patch work

Prefer XML where RimWorld exposes stable Def data.

First XML-owned target:
- `RiverDef` spawn thresholds/chances.

BiomeDefs, wild plants and compatible weather/Def changes should also remain XML where possible.

### Minimal C# world-generation layer

RimWorld 1.6 hard-codes major parts of world terrain generation in `WorldGenStep_Terrain`, including:
- elevation noise and range;
- latitude-based average temperature curve;
- elevation temperature reduction;
- hilliness assignment;
- coastline-forming elevation noise.

Therefore the following Environment responsibilities cannot be implemented robustly as pure XML:
- Japan-oriented temperature envelope;
- Japan-oriented elevation distribution;
- target hilliness distribution;
- increased coastline complexity.

Implementation architecture:
- keep the Vanilla `Terrain` `WorldGenStepDef` unchanged;
- Harmony-postfix `WorldGenStep_Terrain.GenerateFresh`;
- after Vanilla terrain generation completes, Environment applies its elevation/coastline, hilliness, annual-temperature and rainfall transforms;
- Environment then re-runs natural biome selection using the adjusted tile values;
- all changes are limited to the root surface layer;
- later Vanilla world-gen steps, including rivers, roads and factions, consume the adjusted terrain normally;
- do **not** reference an Environment custom `WorldGenStep` class from XML. This avoids Def-load-time type-resolution failure and reduces conflict surface with Vanilla Def loading.

Runtime temperature hooks:
- Harmony overrides the root-surface seasonal shift to an **±8 C → ±16 C** south-to-north curve;
- Harmony scales the separate root-surface daily random variation from the Vanilla ~±7 C scale to **~±3 C**;
- the normal day/night sun-cycle component remains Vanilla for the Alpha baseline.

Use the smallest world-generation code layer that can own these calculations without unrelated runtime systems.

Do not add C# to systems that can be expressed cleanly by Def/Patch XML.

## 6. Waterfalls

True vertical waterfalls are not an Alpha requirement.

RimWorld rivers are fundamentally two-dimensional map features and do not provide a native vertical river-drop model.

A later optional feature may generate a **pseudo-waterfall landform** when conditions such as:
- River present;
- Large Hills or Mountainous;
- suitable rock/terrain layout

are met.

Such a feature may combine map generation, rock walls, water terrain, visual effects and sound. It is explicitly deferred until the core Environment world-generation loop is stable.

## 7. Compatibility principles

- **Medieval Overhaul (MO): optional integration only.** Environment does not require MO, and AMJ is not obligated to retain MO-owned content. Suitable MO mechanics and natural plants can be integrated only on their merits; historical/ecological inconsistency takes precedence over keeping MO data untouched. **`DankPyon_DarkForest` is excluded from new normal-world biome generation** by conditional XML, keeping its Def and its existing saved-world references intact. AMJ-owned biomes remain the Japanese natural-world baseline.
- AMJ Core: optional technical integration. Environment conditions may influence the context in which AMJ crops are chosen, but crop balance remains owned by Core.
- CCTO: optional technical integration. CCTO remains responsible for crop cold-death semantics. Environment does not duplicate or override those crop thresholds; it uses the published CCTO/AMJ values as climate-calibration reference points.
- ReGrowth 2: implementation/reference target, not a dependency.
- **Dynamic Atmosphere (Astryl; Workshop 3808191148): author-selected official optional compatibility target (2026-10-21), never a prerequisite.** The existing AMJE BiomeDefs specify Vanilla weather names under `baseWeatherCommonalities` and the world-generation layer supplies tile temperature/rainfall; AMJE currently ships no custom `WeatherDef` or cloud/sky/precipitation-render replacement. Keep Vanilla weather identity and AMJE's owned climate values unchanged as the default integration surface. Dynamic Atmosphere independently renders cloud circulation and weather based partly on world terrain/rainfall/temperature and current in-game conditions; do not assume its local cloud precipitation imagery or jet streams reproduce a future Japanese rainy-season front automatically. Do not patch the other Mod's shaders, forcibly change its settings, or take ownership of its hurricane game conditions or modified colony lightning.
- **Dynamic Atmosphere implementation rule:** No dedicated XML/C# compatibility patch is justified by the current AMJE weather Defs alone. When AMJE's separately designed rainy-season/autumn-rain weather selection is implemented, keep its actual gameplay weather selection/duration independent of optional rendering and do not equate cloud-shadow visuals with the plant-growth light factor. Only if AMJE later introduces an AMJE-specific weather/event Def requiring a distinct look, evaluate the externally advertised `AtmosphereProfileDef` XML interface against the exact installed Dynamic Atmosphere version and add a conditional integration without making it mandatory. Avoid a second weather controller that overrides Dynamic Atmosphere or an unsupported cross-Mod API call.
- **Verification boundary (2026-10-21):** Workshop 3808191148 advertises 1.6/1.7 (latest observed Oct 17 update) and XML weather-appearance extensibility, but the exact distributed assembly/Def fields and joint AMJE runtime behavior have **not** been examined. Hence this is an approved compatibility objective and static non-overlap assessment, **not** a demonstrated 1.6/1.7 PASS. Before making a public compatibility claim, perform one consolidated actual rendered-game check on supported versions after a concrete implementation or collision exists: Vanilla-weather identity, wet and dry intervals, Dynamic Atmosphere cloud/precipitation display, lightning/hurricane ownership, new-save + existing-save behavior, and errors/performance. Do not trigger native tests merely for this documentation decision. WorldMap-specific Japanese-region climate and heavy snow remain future concepts, not Environment dependencies.
- other biome/worldgen mods: compatibility should favor explicit targeted patches rather than broad destructive replacement where possible.

## 8. Alpha validation

World-generation changes are accepted based on generated-world sampling, not only XML/static checks.

Automated validation is layered by responsibility:
- static/XML/build checks remain the first gate;
- RimTest Redux covers deterministic Environment logic/calculation boundaries in a separate test-only assembly;
- RimWorks Pickle covers loaded-source and loaded-Def integration in Vanilla, MO, CCTO and MO+CCTO development profiles;
- rendering-enabled Quickstarts remain authoritative for generated-world/map, terrain/rendering and native-job regressions;
- the downloaded-Workshop Pickle gate remains separate because publication provenance cannot be established from the development root.

The reusable development procedure is `Docs/GoldenPaths/FrameworkRuntimeTests.md`. Neither RimTest Redux nor Pickle is a production dependency of Environment.

The 2026-10-11 test audit removed checks that merely searched implementation
source for names/log strings, froze document wording or historical completion
states, or pinned production PNG hashes independently of the artwork ledger.
Executable fixture/payload tests, XML contracts, PNG decoding and registered
fixed-master/mask/composite checks remain. Native loaded-game checks retain
the responsibilities described above.

For several seeds at common planet coverage values, collect at minimum:
- land annual mean temperature min/max and distribution;
- elevation min/max and distribution;
- Hilliness percentages;
- rainfall distribution;
- Creek/River/Large/Huge counts;
- percentage/count of river-bearing land tiles;
- coastal land-tile count;
- coastal land-tile ratio relative to Vanilla using the same coverage;
- biome distribution.

### 8.1 Alpha world-generation baseline acceptance

The second-pass world-generation baseline is accepted for the current Alpha after three current-DLL seeds at the same common planet coverage.

Observed ranges across the three seeds:
- same-seed coastal-land multiplier vs Vanilla: **1.43x–1.50x** (mean approximately **1.48x**);
- land-count change vs same-seed Vanilla: approximately **-0.6% to +4.4%**;
- land annual-mean temperature: approximately **-8 C to 20 C** on every seed, with whole-land averages **11.5–12.0 C**;
- land maximum elevation: **3141–3800 m**;
- land at or above 1500 m: **7.0–12.1%**;
- land at or above 2500 m: **0.4–1.73%**;
- land at or above 3000 m: rare, from approximately **0.03% to 0.55%** where fine diagnostics were available;
- Hilliness: **25 / 20 / 25 / 25 / 5%** by construction on every current seed;
- land rainfall in the expanded-diagnostic seeds: **832–2917**, with land averages **1223–1416**;
- river-bearing land-tile share in the expanded-diagnostic seeds: **7.9–8.8%**;
- generated natural river sizes: **Creek and River only**; no LargeRiver/HugeRiver.

The coastline target is therefore being met primarily through increased coastal complexity rather than large-scale land loss.

Biome histograms from validation runs are **not** treated as standalone Environment baseline values when additional biome-providing mods are active. The Environment acceptance criterion is the climate/terrain envelope and coherent biome selection; exact biome percentages remain mod-list dependent.

This locks the current terrain/coast/elevation/rainfall/river Alpha baseline. Further changes require a demonstrated gameplay or compatibility problem rather than additional tuning toward one seed.

### 8.2 CCTO / AMJ crop-climate calibration

Environment climate balance must be checked against the authoritative CCTO thresholds used by AMJ rather than against annual mean temperature alone.

Primary calibration crops:

| Crop | Active-growth minimum | Cold-death threshold | Intended climate signal |
|---|---:|---:|---|
| Rice | 10 C | -1 C | warm-season crop; winter growth stops broadly and frost is dangerous |
| Foxtail millet / Awa | 8 C | -3 C | somewhat more cold-tolerant than rice, but not a winter crop |
| Japanese barnyard millet / Hie | 5 C | -2 C | longer cool-season growth window than Awa/rice, but not especially frost-hardy |
| Barley | 0 C | -8 C | the main cold-tolerant annual reference |

These values are owned by CCTO/AMJ, not by Environment.

Generated-climate acceptance intent:

- **southern warm lowlands:** rice and other warm crops have a long season; winter nights may stop growth and occasionally threaten frost-sensitive crops, while barley is normally safe;
- **central temperate lowlands:** rice and millets stop growing for winter and can be killed by frost if left standing; barley normally remains the safer cold-season annual, though the colder inland edge may threaten it;
- **northern cool lowlands / highlands:** ordinary annual crops cannot remain safely exposed through winter; even barley can face lethal cold, while dormancy-capable perennial crops gain a clear role.

Validation must sample actual hourly/daily outdoor temperatures, not only the tile annual mean.

Current Alpha diagnostic implementation:
- after fresh world creation, select four non-impassable representative root-surface tiles: warm lowland, temperate lowland, cool lowland and highland;
- prefer Northern Hemisphere tiles for consistent seasonal orientation, with all-surface fallback if a representative cannot be found there;
- lowland representatives are selected below 300 m near annual means 18.5 C / 14 C / 7 C;
- the highland representative is selected between 1500–3200 m near 2 C annual mean;
- sample the public RimWorld `TileTemperaturesComp.OutdoorTemperatureAt(PlanetTile, absTick)` API once per in-game hour across all 60 days of one year (1440 samples per representative);
- because this uses the game API after Harmony initialization, samples include the Environment seasonal-amplitude hook, reduced daily random variation, and Vanilla sun-cycle component rather than a separate approximation.

For each representative, collect:
- actual yearly min/max;
- hours and equivalent days below 10 C, 8 C, 5 C and 0 C;
- hours/days below -1 C, -4 C and -8 C;
- event count, count of short events (<=6 h), and maximum continuous duration for each lethal threshold.

If a region's gameplay does not match the intended signal above, adjust Environment climate variation before changing the established CCTO crop thresholds.

First hourly calibration result with the ~±4 C random scale:
- warm lowland (annual 18.5 C): actual -0.6..37.6 C; no time below the Rice -1 C death threshold;
- temperate lowland (annual 14.0 C): actual -8.4..35.9 C; 136 h below -1 C, 70 h below -4 C, and **2 h below -8 C**;
- cool lowland (annual 7.0 C): actual -17.1..32.7 C; 195 h below -8 C;
- highland (annual 2.0 C, 1643 m): actual -21.1..25.4 C; 338 h below -8 C.

CCTO checks cold-death through the plant's normal long-tick path and kills immediately when actual ambient temperature is strictly below the configured threshold. Therefore the temperate representative's 2 h below -8 C is sufficient to threaten Barley even though the event is brief. That conflicts with the intended signal that Barley should normally remain the safer central-lowland annual.

Calibration decision:
- keep the current seasonal-amplitude curve and Vanilla ±7 C sun-cycle component;
- reduce only the separate daily random component from ~±4 C to **~±3 C** (3/7 of Vanilla);
- the re-run passed, so the ~±3 C random scale is accepted as the Alpha CCTO climate baseline.

This is the smallest change that removes a one-night random-spike failure mode in central lowlands while preserving clearly lethal Barley conditions in cool lowlands and highlands.

The gameplay-validation split is automation-first:

**Automated regression must confirm whenever the condition is mechanically observable:**
- usable target-biome/map generation succeeds and the runtime gate completes without owned ERROR entries;
- accepted Hilliness, river/coast, soil-fertility and vegetation distributions remain inside their locked ranges;
- no natural Large/Huge rivers are produced;
- the representative annual climate samples preserve a warm-lowland -> temperate -> cool -> highland gradient, including increasing hours below crop-relevant 8 C and 0 C thresholds;
- when AMJ Core is installed, the four fixed-biome Quickstarts load the Stage A crop Defs and prove that the generated natural fertility ladder creates real crop-choice differences: Soba/Barley can use Thin Soil, Wheat cannot, and fertility sensitivity produces distinct average growth factors;
- crop/climate integration remains differentiated rather than collapsing to one effective growing window or one universally equivalent soil response.

**Manual playtesting is reserved for conditions that are not adequately reducible to a pass/fail metric:**
- whether Flat land feels frustratingly rare despite meeting the numerical distribution target;
- whether the visible river frequency feels intrusive despite staying within the accepted map/world metrics;
- whether the automated crop-choice differences produce interesting decisions rather than busywork;
- visual coherence, readability and overall seasonal atmosphere.

The rule is therefore: do not use a manual gameplay smoke test merely to reconfirm a condition that can be asserted from generated maps, loaded Defs, annual temperature samples or deterministic runtime output. Manual review begins after those automated gates pass.


### 8.3 Core + Environment gameplay-contract runtime gate

When a local AMJ Core installation is available, `run-runtime-tests.bat` creates an additional isolated **Core + Environment** profile and runs the four fixed-biome Quickstarts with real Core crop Defs loaded alongside Environment.

The integration gate is not a subjective playtest. It automatically checks:
- Core Stage A crop Def availability under the real combined mod stack;
- presence of the Environment natural fertility ladder on each generated map;
- actual Thin Soil coverage creating a sowability gap between low-fertility-tolerant crops and Wheat;
- map-weighted fertility growth factors remaining different across Soba, Kibi, Awa, Hie and Barley;
- Core cold-growth thresholds remaining differentiated;
- representative annual climate diagnostics retaining the accepted warm-to-cold gradient;
- no Environment- or Core-origin runtime ERROR entries.

The log records the generated terrain-cell counts, sowable-cell counts and average fertility factors so later balance changes can be compared without relying on screenshots or memory.

Future automation should extend this same contract to season-length / expected-harvest opportunity and long-run resource throughput when those can be measured robustly from the actual RimWorld APIs. Such metrics should be added only when they model the real game closely enough to avoid replacing one subjective judgment with a misleading synthetic score.

## 9. Biome and vegetation architecture

### 9.1 Natural vegetation model

The Environment biome model should follow Japan's broad natural forest zonation rather than Vanilla's continental labels.

Research basis:
- Ministry of the Environment, "Vegetation of Japan": https://www.biodic.go.jp/reports/2-2/aa029.html
- the broad natural sequence is evergreen broad-leaved forest -> deciduous broad-leaved forest -> evergreen coniferous forest from warmer to colder conditions, with a similar vertical transition as elevation increases;
- the same source identifies warm-temperate evergreen broadleaf forest, cool-temperate deciduous broadleaf forest, and subarctic/subalpine evergreen conifer forest as the major broad zones.

For Alpha gameplay, use four coarse climate bands:
- **Warm-temperate forest:** annual mean >= **15 C**;
- **Cool-temperate forest:** annual mean >= **8 C** and < 15 C;
- **Subalpine forest:** annual mean >= **0 C** and < 8 C;
- **Alpine zone:** annual mean < **0 C**.

These thresholds are gameplay approximations, not literal botanical boundaries. The existing Environment temperature/elevation model already makes the same sequence occur both northward and upward.

**2026-10-18 WorldMap biome ownership decision:** The planned *intermediate-temperate forest* (`中温帯林 / 中間温帯林`) will be an **optional Japan-fixed-world-only `BiomeDef` owned by AMJ WorldMap**, not an additional Environment procedural-world biome. Its eventual world-tile range, vegetation pool and other regional north/south variants belong to WorldMap. Environment continues to own the four broad standard biome definitions here, their ordinary random-world selection, and the shared Japanese tree/plant/food `ThingDef` plus their textures/behavior. WorldMap may reference those loaded PlantDefs in its own biome's `wildPlants`, but must not copy or rewrite them. The intermediate-def content, climate boundaries, natural-spawn semantics and compatibility are **not yet implemented** and this decision does not authorize changing the thresholds above. WorldMap's regional Subalpine/Alpine redistribution is deferred until its own intermediate-temperate biome is introduced. See WorldMap `Docs/Design.md` for the owning design.

Wetlands are treated as a moisture/topography overlay on those climate bands rather than as a separate latitude zone. Initial diagnostic candidate threshold: **swampiness >= 0.5**.

**2026-10-12 natural-world validation correction:** Retain both `TemperateSwamp` and `ColdBog` as available natural wetland types, but do not require both to occur on every seed. The existing native worker competition selects the subtype from the generated tile conditions. The reference seed's candidate tiles can all favor TemperateSwamp; ColdBog count zero alone is not a generation defect. Require actual natural wetlands, candidate containment, complete biome assignment and the provisional minority ceiling. Keep the separate fixed-biome ColdBog vegetation/terrain/native-job regression. Do not alter the locked climate, wetland candidate distribution or native worker scores merely to satisfy a per-seed subtype count.

### 9.2 Biome Def strategy

Do **not** repurpose Vanilla `TropicalRainforest`, `TemperateForest`, `BorealForest` or `Tundra` into AMJ-specific biomes.

Reason:
- those Defs also own animal pools, diseases, terrain generation, weather, forage behavior and compatibility targets;
- changing their meaning would make unrelated patches and content inherit the wrong semantic assumptions;
- for example, scoring Vanilla `TropicalRainforest` as a Japanese warm-temperate forest would still retain tropical disease/animal/content assumptions unless most of the Def were replaced.

Instead:
- add AMJ-owned BiomeDefs for the accepted vegetation zones;
- use temporary Vanilla world textures during Alpha where needed; final biome artwork remains deferred until Environment mechanics and distribution are stable;
- start each biome from the nearest Vanilla gameplay profile, then replace vegetation/weather/terrain details deliberately rather than by hidden inheritance;
- keep exact animal additions outside Environment unless required for biome functionality;
- allow third-party naturally generated biomes to remain compatible through normal worker scoring rather than globally suppressing all non-AMJ BiomeDefs;
- use the four AMJ biomes to **replace the broad Vanilla land-biome bundles**. Retain the audited Vanilla wetlands and water; specialized biomes from Medieval Overhaul and other compatible mods may replace part of a matching AMJ climate band when their own workers score higher. This compatibility allowance does not permit replaced Vanilla biomes to remain.

### 9.2.1 Vanilla surface-biome replacement contract (2026-10-12 correction)

The author made this foundational defect the highest execution priority, ahead of wetland plant additions and artwork. The former acceptance of residual Vanilla tropical/temperate biomes was a design error, not approved behavior. On a newly generated root-surface world, exclude the following original Vanilla biome bundles **before worker scoring**, then select the appropriate AMJ/retained/compatible biome using the already accepted temperature, rainfall and wetland conditions.

| Vanilla surface biome | New-world decision | Replacement / reason |
|---|---|---|
| `TropicalRainforest` | Exclude | Japanese warm-temperate AMJ band; do not inherit tropical flora/fauna/diseases |
| `TropicalSwamp` | Exclude | Retained temperate/cool wetlands selected by their native workers |
| `TemperateForest` | Replace | AMJ warm/cool bands own the Japan-oriented ecological bundles |
| `BorealForest` | Replace | AMJ subalpine and adjacent bands follow the accepted annual-temperature model |
| `Tundra` | Replace | AMJ alpine and adjacent bands; cold land does not retain the original tundra bundle |
| `AridShrubland` | Exclude | Outside the accepted humid Japan-oriented world model |
| `Desert` | Exclude | Outside that world model; ordinary sandy terrain is a separate matter |
| `ExtremeDesert` | Exclude | Outside that world model |
| `IceSheet` | Exclude | AMJ alpine land represents cold mountains, not a continental ice sheet |
| `SeaIce` | Exclude | Preserve ordinary water rather than a polar sea-ice biome |
| `TemperateSwamp` | Retain | Audited 温帯湿地 bundle; native Worker and terrain unchanged |
| `ColdBog` | Retain | Audited 冷涼湿原 bundle; native Worker and terrain unchanged |
| `Ocean` | Retain | Native water/coast handling |
| `Lake` | Retain | Native water/lake handling; this repair does not force natural lake generation |

Production owner: `EnvironmentTerrainProcessor.SelectBiome` in `Source/AncientMedievalJapanEnvironment/WorldGenStep_AMJEnvironmentTerrain.cs`. The filter operates only in the existing root-surface regeneration pass. It does not delete/relabel original BiomeDefs, change global `generatesNaturally`, raise AMJ scores to suppress MO, alter retained wetland Workers, or migrate saved worlds. Existing worlds and maps keep their saved biomes; test the correction on **fresh natural worlds**. DLC/other planet layers retain their own selection, and this base-surface repair does not certify their content.

The root cause was candidate admission plus relying only on score competition: at annual 20°C and rainfall 3000, Vanilla `TropicalRainforest` scores about 42.55 while AMJ warm-temperate scores 37.375; at swampiness 0.5 or more, all four AMJ band workers deliberately return 0 and Vanilla `TropicalSwamp` can win. Keeping AMJ score 38 preserves MO coexistence; excluding inappropriate candidate identities fixes both paths. Native Worker/SurfaceTile reference: `Chillu1/RimWorldDecompiled` `RimWorld/BiomeWorker_TropicalRainforest.cs`, `BiomeWorker_TropicalSwamp.cs`, `BiomeWorker_TemperateSwamp.cs`, `BiomeWorker_ColdBog.cs`, `RimWorld.Planet/SurfaceTile.cs` (GetScore/WaterCovered inspected). The supplied MO 1.6 archive retains `DankPyon_DarkForest` with its own Worker; no MO Def/Worker or herb weights change.

Acceptance uses the **existing** `AMJWorldWetlandDistributionQuickstart`, without forced tile-biome replacement: scan all surface tiles including water, require each excluded biome count to be zero, all four AMJ bands and actual wetlands to exist, both adopted wetland types to remain available for natural generation, no missing biome, and, when real MO is loaded, `DankPyon_DarkForest` BiomeDef remains defined but has `generatesNaturally=false` with **zero naturally generated Dark Forest tiles**. Wetland subtype occurrence remains seed-dependent; the separate fixed-biome scenarios cover each retained subtype's map behavior. Keep the original wetland/settlement/error/capture checks. Run the same natural-world gate in Vanilla / MO / CCTO / MO+CCTO using the existing hidden four-profile payload runner's `--world-biomes-only` option; focused success is scoped biome evidence, not full release/cutting PASS. The existing RimTest selector check samples 128 loaded-worker temperature/rainfall/swampiness combinations and an ocean tile. Historical fixed-map/world-share PASS, one profile or static syntax checks cannot close this repair. Four-profile runtime and actual Workshop correction remain unverified until new same-source evidence is obtained.

### 9.3 ReGrowth 2 reference policy

ReGrowth 2 remains an implementation/reference target, not a dependency.

Useful reference patterns:
- biome-specific wild-plant lists;
- rainfall-sensitive weather commonality;
- lightweight biome workers for optional regional sub-biomes;
- visual/seasonal atmosphere added without replacing unrelated core systems.

Do not copy ReGrowth source code, textures, sounds or other restricted assets. Implement AMJ behavior independently and use compatibility patches only when needed.

### 9.4 Tall Grass: Hidden Danger reference policy

Tall Grass: Hidden Danger is a **prior-art/reference candidate** for dynamic vegetation and ground-use feedback. It is not a dependency and should not be copied wholesale.

Reference-worthy patterns:
- vegetation state changing in response to repeated pawn traffic;
- trampled vegetation and emergent footpath-like ground traces;
- recovery/regrowth after traffic pressure is removed;
- using local vegetation change to make settlement surroundings reflect actual human activity.

Scope limits for AMJ Environment:
- prioritize ecological/visual feedback around settlements and traveled ground;
- do **not** treat Tall Grass's combat concealment model, including very high/100% cover-style behavior, as an AMJ target;
- any future implementation must be independently designed and should remain lightweight enough for large colonies and long-running saves;
- avoid duplicating another mod's full terrain or plant system when a small AMJ-owned mechanic or compatibility layer would achieve the intended environmental effect.

### 9.4 Validation before BiomeDef implementation

Before creating the four AMJ BiomeDefs, log the distribution of the proposed climate bands over generated land:
- WarmTemperate >=15 C;
- CoolTemperate 8..15 C;
- Subalpine 0..8 C;
- Alpine <0 C;
- swampiness >=0.5 candidates.

The target is not equal shares. Expected shape:
- cool-temperate should be the largest or one of the largest settled bands;
- warm-temperate should be common but concentrated toward warmer lowlands;
- subalpine should be meaningful but clearly smaller;
- alpine should be rare;
- wetland candidates should remain a minority.

If this shape is stable, proceed to AMJ BiomeDefs and vegetation composition. Do not tune the locked world-generation climate merely to force biome percentages.

Observed preview on the first validation seed:
- WarmTemperate: **27.5%**;
- CoolTemperate: **51.4%**;
- Subalpine: **19.5%**;
- Alpine: **1.7%**;
- swampiness >=0.5 candidates: **0.7%**.

This matches the intended shape closely enough that no climate/worldgen retuning is required.

### 9.5 Alpha BiomeDef implementation

The first AMJ-owned biome pass is implemented with four BiomeDefs:
- `AMJ_WarmTemperateForest`;
- `AMJ_CoolTemperateForest`;
- `AMJ_SubalpineForest`;
- `AMJ_AlpineZone`.

Biome workers:
- use the accepted annual-mean temperature bands;
- require rainfall >=800;
- exclude swampiness >=0.5 so existing wetland biomes continue to handle the initial 0.7% wetland minority;
- return an ordinary score centered near 38 among admitted candidates, allowing stronger specialized workers from MO or other compatible mods to coexist; excluded Vanilla identities cannot win regardless of their score.

Alpha content policy:
- world-map textures temporarily reuse nearest Vanilla biome textures;
- wild plants use existing Vanilla Defs only;
- wildlife pools are temporary Vanilla-backed functional placeholders so generated maps are not empty, not a historical fauna specification;
- no final textures, new plant graphics, regional weather effects, or Japan-specific wild-plant Defs are part of this pass;
- all placeholder content must be revisited after runtime distribution/map-generation validation.

The next validation gate is runtime: apply §9.2.1's zero-excluded-biome acceptance criteria alongside the four AMJ bands, retained wetlands and specialized-mod coexistence. Do not accept world shares alone as replacement success.

First runtime distribution with Medieval Overhaul active:
- AMJ WarmTemperate: **27.0%**;
- AMJ CoolTemperate: **43.7%**;
- AMJ Subalpine: **7.2%**;
- AMJ Alpine: **0.3%**;
- MO Dark Forest: **16.3%**;
- Temperate Swamp: **2.0%**;
- remaining Vanilla tropical/temperate biomes: small residual shares (**historical defect; fails the corrected replacement contract**).

MO Dark Forest coexistence remains valid: its own score rules may replace part of the suitable AMJ cool/subalpine climate space. Exact AMJ percentages are mod-list dependent and are not balance targets. The residual Vanilla tropical/temperate biomes were incorrectly accepted and do **not** satisfy the corrected contract; this historical distribution is not evidence for the repaired source.

**2026-10-23 作者指示 — MO薬草4種は現状維持・監査保留：** `DankPyon_Plant_MindwortWild`／`DankPyon_Plant_PoppyWild`／`DankPyon_Plant_FleawortWild`／`DankPyon_Plant_FlyAgaricWild` はMO独自の薬品・加工レシピに使われるため、今回の歴史適合性選別・MO不要コンテンツ除外の対象に**含めない**。既存の植物Def、自然分布の割当、採集による素材供給、薬品・レシピ接続、AMJEの4植生帯への任意互換配置を**変更しない**。種別の採否・置換・全面削除の監査は当面開始せず、薬品体系を別途見直す必要が生じた時点で素材供給から完成薬品まで一体で検討する。新たなMO必須依存は設定しない。これは保留・現状維持の確定指示であり、4種の日本史適合性を個別承認した意味ではない。

**2026-10-11 MO野生薬草の独自Biome互換修正：** MO 1.6.2.2の`Patches/Core/Add_Plants_To_Biomes.xml`は野生Mindwort／Poppy／Fleawort／FlyAgaricを`TemperateForest`（各0.05）、`BorealForest`（各0.16）、`Tundra`（各0.05）に追加する。AMJEの4植生帯がそれらVanillaの世界生成枠を置き換えると、MOの元Def・元Biome設定が残っていても実際に薬草が生えるマップの比率が低下する。このため、MOが有効な場合に限り、AMJE所有の`AMJ_WarmTemperateForest`／`AMJ_CoolTemperateForest`（各0.05）、`AMJ_SubalpineForest`（各0.16）、`AMJ_AlpineZone`（各0.05）の各`wildPlants`へ、MO側の4種を欠損分だけ追加する。MO所有の植物ThingDef、元Vanilla5Biome（森林3・湿地2）の値、MO所有DarkForestは変更しない。既に追加されている場合は重複させず上書きもしない。これは既存MOの生息場所をAMJE新Biomeへ継承する**任意互換**であり、AMJE単独の植物・薬草の追加機能ではない。XML/static、実ロード済みPickle、森林実マップQuickstartsで個別に検証し、新規実機結果を得るまではPASS扱いにしない。極寒・積雪の高山帯は実生数ゼロを直ちに配分欠落と判断せず、ロード済み出現設定と実生を区別して記録する。


### 9.6 Japan-specific structural wild vegetation

After the biome-placement and natural-soil gates passed, Environment moves from Vanilla-only vegetation placeholders to a deliberately small Japan-specific structural set.

Research basis:
- Ministry of the Environment vegetation classification identifies warm-temperate forests with shii/oak evergreen broadleaf communities such as Sudajii and tabu forests, cool-temperate forests with beech/mizunara communities, subalpine forests with evergreen conifers such as shirabiso/oshirabiso/kometsuga, and alpine vegetation with haimatsu scrub and alpine grassland;
- historical-vegetation research also shows that human activity from the Yayoi period through the medieval period increased pine, grassland and secondary-forest signatures in many settled regions. AMJ should therefore not render every lowland forest as an untouched climax forest.

Sources:
- https://www.biodic.go.jp/reports/3-4/hyo/c090.html
- https://www.biodic.go.jp/reports/2-2/aa029.html
- https://www.ffpri.go.jp/labs/prdb/sirabiso.html
- https://www.biodic.go.jp/reports2/5th/vgt_en/5_vgt_en.pdf
- https://www.jstage.jst.go.jp/article/jaqua/advpub/0/advpub_62.2211/_article/-char/en

Alpha structural set:

| Climate band | AMJ plant | Role |
|---|---|---|
| Warm-temperate | `AMJ_Tree_Shii` / Sudajii | evergreen broadleaf canopy dominant |
| Cool-temperate | `AMJ_Tree_Beech` / Japanese beech | deciduous broadleaf canopy dominant |
| Subalpine | `AMJ_Tree_Shirabiso` / shirabiso fir | evergreen conifer canopy dominant |
| Alpine | `AMJ_Shrub_Haimatsu` / haimatsu dwarf pine | low alpine scrub dominant above treeline |

Scope rules:
- the four AMJE structural plants remain the initial minimum custom set, but generic grass, moss, brambles, bushes, and secondary trees are **not** permanently delegated to Vanilla by default;
- current reuse of Vanilla oak/maple/birch/pine/bamboo and other filler vegetation is a Beta implementation shortcut, not a historical-design justification. Before the post-Beta vegetation/art pass, audit every reused plant for Japanese distribution, pre-Edo period fit, ecological/landscape role, and whether its Def/description/graphic can represent the intended Japanese vegetation without distortion;
- only plants that pass that audit remain in AMJE biome pools. Others are removed, replaced by a more suitable AMJE plant, or left outside AMJE-controlled biome vegetation;
- add no new food, medicine, fiber or other harvested item types in this pass. Gatherable-resource design belongs to the relevant AMJ resource/gathering systems and should be coordinated separately;
- 2026-10-06 wood-yield update: Haimatsu provides a small amount of existing wood (base yield 8), while retaining its low BushBase form. The other base yields are Shii42/Beech40/Shirabiso30. Vanilla yields WoodLog; MO's enabled wood-chain patch substitutes DankPyon_RawWood. This adds no new resource Def. Verify actual cutting outputs with `Docs/GoldenPaths/PlantHarvestTests.md`.
- the three AMJ trees yield ordinary Vanilla wood only;
- haimatsu is a low shrub rather than a normal timber tree, preserving alpine timber scarcity;
- all four new plants use Vanilla graphics as temporary placeholders. Final plant artwork remains deferred until vegetation distribution and gameplay are accepted.

Initial wild-plant commonality:
- Warm-temperate: Shii **2.0**, with Oak 0.4, Poplar 0.15, Maple 0.25 and Bamboo 0.5 retained as secondary components;
- Cool-temperate: Beech **1.8**, with Oak 0.6, Maple 0.8, Birch 0.6 and Pine 0.35 retained;
- Subalpine: Shirabiso **2.6**, Pine 0.9 and Birch 0.5;
- Alpine: Haimatsu **1.3**, with full-size Pine/Birch reduced to 0.02 each.

Automated runtime map validation is the acceptance gate for this Alpha vegetation set. The four fixed-biome Quickstarts run against an isolated profile and verify target-biome identity, target structural-plant generation, dominance over configured secondary tree species, Alpine Haimatsu coverage <=5% of all cells, Alpine full-size Pine+Birch coverage <=1% of all cells, complete live log capture, no Environment-origin live ERROR entries, and **zero Quickstarts pre-launch ERROR entries**.

A later report audit found that an earlier nominal Quickstarts PASS was incomplete: all vegetation assertions passed, but every report contained `preLaunchErrors=2`. The corresponding startup log entries were AMJE-owned XML inheritance errors on Shirabiso and Haimatsu `visualSizeRange`: the inherited parent FloatRange text and child min/max nodes were being merged into one invalid node. The two child ranges now explicitly use `Inherit="False"`, and the runtime runner now fails on any nonzero `preLaunchErrors` in the isolated profile.

A clean rerun after those fixes passed in full. All four standalone biome reports have `passed=true`, `failed=0`, `preLaunchErrors=0`, complete live log capture, and no ERROR-level runtime entries. The current measured structural shares are Warm Shii **3.14%**, Cool Beech **3.22%**, Subalpine Shirabiso **1.29%**, and Alpine Haimatsu **0.68%**; Alpine full-size Pine+Birch remains only **0.03%**. The Alpha structural vegetation composition/commonality values are therefore accepted without retuning. Final plant artwork remains deferred to the later visual-art pass.

#### Optional CCTO integration ownership

Cold-tolerance compatibility for AMJE-owned plants is owned by **AMJE**, not by the CCTO repository.

Product/balance positioning:
- **AMJE standalone** must be a complete and enjoyable "medieval Japan-like environment" mod for players who want the geography, climate, terrain, biomes and vegetation without adopting CCTO's stricter crop/plant cold simulation;
- **AMJE + CCTO** is the higher-realism configuration. CCTO adds species-specific minimum-growth temperatures plus cold death/dormancy behavior, making seasonal and regional plant survival more demanding;
- CCTO is therefore an optional realism layer, not a missing piece required to make AMJE function.

Dependency direction:
- AMJE does **not** require CCTO;
- CCTO does **not** need to know that AMJE exists;
- when CCTO is absent, AMJE plants use ordinary RimWorld plant behavior;
- when CCTO is present, AMJE conditionally patches only its own PlantDefs to use CCTO's public `ColdToleranceExtension`.

This keeps the two mods loosely coupled and allows either mod to be updated or used independently.

Temperature layers:

| Plant | AMJE without CCTO: min growth | AMJE + CCTO: min growth | CCTO cold response |
|---|---:|---:|---|
| Sudajii / `AMJ_Tree_Shii` | 0°C | 8°C | death below -8°C |
| Japanese beech / `AMJ_Tree_Beech` | 0°C | 5°C | cold dormancy |
| Shirabiso / `AMJ_Tree_Shirabiso` | 0°C | 0°C | death below -35°C |
| Haimatsu / `AMJ_Shrub_Haimatsu` | 0°C | 0°C | death below -35°C |

The standalone 0°C values are explicit even though RimWorld's `PlantProperties.minGrowthTemperature` default is also 0°C. This makes the Vanilla-style baseline a deliberate AMJE value rather than an accidental inherited default.

**2026-10-10 accepted Step 3 wetland CCTO design (not yet implemented):** The next four native wetland species follow the same optional, Environment-owned CCTO extension pattern. Exact approved values are:

| Planned species | Without CCTO: minimum growth | With CCTO: minimum growth | With CCTO: cold response |
|---|---:|---:|---|
| `AMJ_Plant_Yoshi` | 0°C | 5°C | cold dormancy |
| `AMJ_Plant_Suge` | 0°C | 0°C | cold dormancy |
| `AMJ_Tree_Hannoki` | 0°C | 5°C | cold dormancy |
| `AMJ_Plant_Mizugoke` | 0°C | 0°C | death below −35°C |

This is an approved **game-balance/compatibility specification**, not a statement of measured botanical lethal temperatures. Do not create CCTO patch references before the corresponding author-approved production PlantDefs and art exist; the current four implemented trees are unchanged. Scope the additions to AMJE-owned DefNames only. The canonical wetland-specific behavior, winter Harvest/CutPlant conflict and non-visible four-profile test requirements are in `Docs/NativeVegetationStep3Design-ja.md` §3.3.1. The existing `run-tests.bat` and framework runners keep Direct3D rendering active on a non-visible isolated Windows desktop; running the lower-level visible/debug Quickstarts directly is not the acceptance path. Fresh loaded-native/survival/snow tests are still OPEN.

The CCTO values are Alpha gameplay values on CCTO's existing scale, not claims of exact physiological lethal temperatures. Beech follows the same deciduous dormancy model as CCTO Oak/Birch/Maple; Shirabiso follows the CCTO Pine/Great Fir subalpine-conifer model; Haimatsu uses the same strong evergreen frost tolerance without inventing a new lower threshold solely for its alpine label. Sudajii uses a warmer 8°C growth threshold and a conservative -8°C lethal threshold appropriate to the warm-temperate role.

Implementation is isolated in `Patches/Compatibility/CCTO.xml` with `MayRequire="sucro.cropcoldtoleranceoverhaul"`. AMJE's base PlantDefs never contain a CCTO class reference.

Automated validation must cover both configurations:
1. AMJE alone: all four loaded PlantDefs remain at the explicit 0°C baseline and no CCTO dependency is required;
2. AMJE + CCTO: the four loaded PlantDefs expose the table values above and exactly one `ColdToleranceExtension` each.

The normal four-biome runtime gate remains CCTO-free. If CCTO is installed locally, the one-command runtime gate additionally creates a second isolated AMJE+CCTO profile and runs one focused loaded-Def compatibility Quickstart.



## 10. Natural soil fertility

### 10.1 Responsibility and purpose

Environment owns **naturally generated soil quality**. Core/MO own player-created agricultural improvements such as paddies, plowed soil and other cultivated terrain.

The goal is not to make farming uniformly worse. Low-fertility terrain should create meaningful land-use and crop-selection differences:
- fertile lowlands remain valuable;
- ordinary soil remains the common baseline;
- stony/poor ground makes low-fertility-tolerant crops more useful;
- highland regions become progressively harder to cultivate without making them completely sterile.

### 10.2 Existing terrain reuse

Do not duplicate Vanilla/MO terrain roles unnecessarily.

Relevant existing values:
- `SoilRich`: fertility **1.40**;
- `Soil`: fertility **1.00**;
- `Gravel` / stony soil: fertility **0.70**;
- `Sand`: fertility **0.10** and no `GrowSoil` affordance;
- MO `DankPyon_PlowedSoil`: fertility **1.25**, a player-created agricultural improvement and therefore outside Environment's natural-terrain responsibility.

Because Gravel already provides a 0.70 low-fertility step, Alpha adds only **one** additional natural growable terrain rather than creating multiple nearly identical poor-soil Defs.

### 10.3 AMJ Thin Soil

`AMJ_ThinSoil`:
- fertility: **0.50**;
- growable (`GrowSoil`);
- normal natural-ground building affordances retained so fertility is the main gameplay distinction rather than construction prohibition;
- slightly higher path cost than ordinary Soil/Gravel;
- placeholder appearance reuses/tints a Vanilla natural texture; final terrain art is deferred.

Do not add a separate 0.40 "very poor soil" during Alpha. The current useful ladder is:

`Thin Soil 0.50 -> Gravel 0.70 -> Soil 1.00 -> Rich Soil 1.40`.

Add another tier only if playtesting demonstrates a distinct gameplay role.

### 10.3a Thin Soil growing-zone designation (2026-10-19 defect correction)

**Player-reported issue:** Although `AMJ_ThinSoil` is defined with `GrowSoil` and fertility 0.50, Vanilla growing-zone drawing can reject the tile before a player has selected an AMJ crop. This defeats the accepted purpose of the poor-soil cultivation tier. Terrain affordance and an individual plant's `fertilityMin` are not sufficient proof that the **zone designator itself** accepts the tile.

**Source correction:** `Source/AncientMedievalJapanEnvironment/ThinSoilGrowingZonePatch.cs` uses a narrowly scoped Harmony postfix on `Designator_ZoneAdd_Growing.CanDesignateCell`: preserve any Vanilla/other-Mod acceptance, and additionally permit only an otherwise free, unroofed, unfogged, buildable `AMJ_ThinSoil` cell with effective fertility at least 0.50. Existing zones, buildings and other terrains retain their ordinary restrictions. No patch to Vanilla potato fertility, the global fertility threshold, non-AMJ terrains or unrelated designators.

**Owner:** Environment owns this terrain and its zoning compatibility. Grains continues to own each crop's own `fertilityMin`, temperature and growDays. Zone placement does **not** override a plant's sow eligibility: Thin Soil accepts the AMJ crops with `fertilityMin<=0.50` while rice and wheat at 0.70 still cannot be sown there. No change to the accepted 0.50→0.70→1.00→1.40 fertility ladder.

**RimWorld 1.6 author acceptance (2026-10-19, manual PASS):** The author pulled the 0.2.0 test-only branch `test/environment-020-playable-20261019` (verified 1.6 native DLL, branch commit `5dac3140230402b3b2aa96cbc117b98a23745dde`) and reported **no problems** with the agreed three-point game checklist: paint a Growing Zone on natural Thin Soil (fertility 0.50), confirm compatible Grains crops can be sown while higher-fertility rice/wheat cannot, and ensure rejected terrain/occupied-cell placement remains restricted. The author separately observed that the optional **Nice Plant Menu** displayed its **insufficient-fertility warning** as expected. This is **author-reported in-game observation**, not a screenshot/log-backed native automation assertion; avoid inventing per-cell or harvest evidence beyond that report. **1.7 Thin Soil zoning**, long-term growth/harvest and a Steam-downloaded subscriber build remain unverified. The previously approved 0.50→0.70→1.00→1.40 terrain ladder and crop-specific fertility minima remain unchanged.

### 10.3.1 Advanced Cultivation - Continued (conditional optional compatibility)

**Author decision (2026-10-13):** consider an **optional compatibility patch only if the third-party Mod gains substantial subscriber adoption**. This is not a present implementation order or a fixed numeric subscriber threshold. Do not make Advanced Cultivation an Environment/AMJ dependency, alter the accepted natural-soil fertility ladder, or start compatibility production just for this candidate.

- External Workshop: https://steamcommunity.com/sharedfiles/filedetails/?id=3623069749 (Meme Goddess; RimWorld 1.6 listed, no confirmed 1.7 support as of this check). Workshop snapshot checked 2026-10-13: **829 current subscribers**, 6,071 unique visitors, 291 collections. These are changeable popularity indicators, not an approved adoption cutoff.
- The Workshop description says its tilling raises fertility by +30% until harvest, adds compost/fertilizers/cultivated soils, and changes Vanilla Soil/Rich Soil/Gravel etc. fertility. It explicitly warns that soil added by other Mods lacks a tillable variant. Therefore **Environment `AMJ_ThinSoil` (base fertility 0.50)** is the main potential integration gap. The upstream 1.6 XML and checked-in C# tilling source have now been **partially audited** (see DBH comparison below); the current compiled assemblies, final patch/load order, and loaded combined runtime behavior have **not been verified**.
- When popularity justifies reopening, first refresh subscriptions, supported game versions and exact upstream code. Check whether Thin Soil can be tilled, whether tilling/reversion retains its source terrain and approved base fertility, whether supplied fertilizers can coexist with AMJ natural terrain, and whether the resulting soil/terrain transformations persist correctly through harvest, save/load and seasons. Avoid silently changing third-party balance or regenerating saved terrain.
- **Grains is a separate crop-balance owner:** only then test how increased fertility affects Soba/Barley/other low-fertility crops versus Wheat's current sowability gap, and reconcile with Grains rather than changing its crop Defs from Environment. Test standalone Environment and applicable Environment+Grains/MO profiles before claiming compatibility. No new patch, cross-owner work order, save guarantee or CI/runtime PASS is claimed by this note.

#### DBH irrigation / compost / fertilizer pre-audit (2026-10-13; source-only)

**Finding:** No identical `defName` or direct XML overwrite between Advanced Cultivation's 1.6 farm mechanics and Dubs Bad Hygiene (DBH) 1.6 irrigation/biosolids mechanics was found in the inspected files. **Do not infer tested coexistence from this.** Both can affect the fertility of the same terrain/cell, so the main risk is stacked growth/balance or an unexpected state transition, rather than duplicate compost machines or ingredient definitions.

- **Advanced Cultivation source:** `Patches/Vanilla_Overwrites_AC.xml` changes Vanilla Soil/SoilRich/Gravel to 0.70/1.00/0.40; `Defs/TerrainDefs/Terrain_AC.xml` adds separate tilled terrains with 1.00/1.30/0.70 fertility, three constructed fertilizer terrains with 1.00/1.30/1.70 fertility (tilled: 1.30/1.60/2.00), and `GrowSoil` / `Diggable` affordances. `Source/Tilling.cs` shows a harvest hook that switches tilled terrain back to its source terrain. AC's `AC_CompostBin`, `AC_RawCompost`, `AC_FermentedCompost`, `AC_Fertilizer` and their recipes are distinct from DBH's content. The upstream checked-in C# source is not proof of the shipped DLL's exact instructions. Source: https://github.com/MemeGoddess/Advanced_Cultivation.
- **DBH source:** `1.6/Defs/ThingDefs_Buildings/BuildingsF_Irrigation.xml` defines `IrrigationSprinkler` using `CompIrrigationSprinkler`, and `BiosolidsComposter` converting `FecalSludge` into `Biosolids`. `1.6/Defs/ModOptions/ModOptions.xml` exposes distinct irrigation, fertilizer and base-terrain fertility factors plus fertilizer lifespan. The original author describes sprinkler and biosolid bonuses as stackable with custom terrain and one another, but the exact 1.6 compiled cell-grid formula, caps, overlays and order of operations have not been inspected. Sources: https://github.com/Dubwise56/Dubs-Bad-Hygiene and https://github.com/Dubwise56/Dubs-Bad-Hygiene/wiki.
- **DBH for Medieval add-on, supplied reference Defs:** `ES_IrrigationCanal` / `ES_SluiceGate` use DBH plumbing/sprinkler components; an optional Processor Framework patch replaces `BiosolidsComposter` processing components. Neither inspected add-on patch targets `AC_*` terrain/item/composition Defs. This does **not** establish DLL/runtime compatibility; add-on version and current source identity need confirmation.
- **Environment/Grains impact:** Without any AMJ patch, `AMJ_ThinSoil` (0.50) lacks an AC `tillsTo` extension/paired tilled Def. AC also overrides the existing AMJ-designed Soil/Gravel/SoilRich fertility ladder, potentially weakening Grains' intended low-fertility crop differentiation even before DBH applies bonuses. Do not patch these accepted baseline values for the sake of an inactive subscriber-gated integration.

**Author-confirmed coordination requirement (2026-10-13):** If AMJ promotes Advanced Cultivation optional compatibility after the subscriber-adoption gate, **DBH / DBH for Medieval coexistence must be coordinated as part of that work, not deferred as an optional afterthought**. A standalone `AMJ_ThinSoil` tilling patch is insufficient for an AMJ compatibility claim. Establish the loaded effective-fertility model across AC terrain replacement/tilling, DBH irrigation-moisture and biosolid-fertilizer overlays (including DBH settings, stacking, caps, source-terrain changes, expiry and save/load). Review the existing Waterworks → DBH for Medieval canal/sluice supply adapter as a separate water-delivery path, not a new independent fertilizer system; verify its moisture outcome on AC soils without duplicating or altering DBH's existing consumer logic. **Environment owns natural terrain and any Thin Soil mapping; Waterworks owns only its optional canal supply bridge; Grains owns crop fertility thresholds/balance; exact AC–DBH patch ownership and required balance adjustments must be agreed between those owners before implementation.** Keep all three third-party Mods optional and independently usable; do not add hard dependencies, rewrite third-party features, or modify existing AMJ baseline soil/crop values without a separately accepted design decision. Waterworks' previously passed native DBH for Medieval integration does not certify AC coexistence.

**If popularity reopens this candidate**, make a narrow loaded-game matrix: (1) AC+DBH Soil/Gravel/fertilizer-terrain effective fertility with irrigation and biosolids each OFF/ON and both ON, at default/representative DBH slider values; (2) AC till → DBH treatment → harvest/reversion → expiry → save/reload, checking no stale effects/lost base terrain; (3) both separate compost manufacturing chains, no ingredient/Def collision; (4) optional DBH for Medieval canal/gate treatment on AC tilled and constructed soils; (5) Environment `AMJ_ThinSoil` and Grains crop-sowability/yields where relevant. Require game-loaded/error evidence and an author-approved balance decision before claiming coexistence or adding a patch. **No gameplay implementation or runtime PASS from this audit.**

### 10.4 Accepted Alpha biome distribution thresholds

RimWorld map generation creates a local fertility-noise field and resolves `BiomeDef.terrainsByFertility` against that value. Environment uses this existing XML mechanism; no custom map-generation C# is required for the terrain placement itself.

Accepted Alpha thresholds:

| Biome | Thin Soil | Gravel | Soil | Rich Soil |
|---|---:|---:|---:|---:|
| Warm-temperate | <0.30 | 0.30-0.50 | 0.50-0.87 | >=0.87 |
| Cool-temperate | <0.35 | 0.35-0.55 | 0.55-0.87 | >=0.87 |
| Subalpine | <0.50 | 0.50-0.70 | 0.70-0.92 | >=0.92 |
| Alpine | <0.65 | 0.65-0.90 | >=0.90 | none |

These are generation-noise thresholds, not terrain fertility values. They intentionally make lower-quality soil more common as the climate/elevation band becomes harsher.

### 10.5 MO compatibility without retaining unsuitable natural biomes (2026-10-23 revision)

**2026-10-23 MO wildlife leak correction:** Actual MO 1.6.2.2 `Patches/Core/Add_Animals_To_Biomes.xml` injects `DankPyon_Direwolf`, `DankPyon_Rox`, `DankPyon_DireBoar` and `DankPyon_NorthernBoar` into the ordinary Vanilla `TemperateForest`/`BorealForest`/`Tundra`/`ColdBog`/`IceSheet`/`SeaIce` populations, bypassing AMJE's hand-selected Japanese animal proxies (especially in the retained `ColdBog`). Environment now declares optional `loadAfter` for `DankPyon.Medieval.Overhaul`, and conditionally removes precisely those **wildlife commonality records** from those six Biomes after MO patches. It does **not** delete any MO PawnKindDefs, edit trader/incident/faction/raid/spawn sources or globally disable the creatures. `WildBoar` continues to represent the native Japanese wild-boar niche and is not removed. `TemperateSwamp` remains under AMJE's existing wildlife replacement; MO 1.6.2.2 does not add these four to that biome. Existing worlds retain their already-created maps/animals. This is a natural-world compatibility fix, not global MO fantasy removal. The current source is not yet game-tested.

`DankPyon_DarkForest` does **not** represent a required ancient-to-medieval Japanese ecosystem. MO 1.6.2.2's actual biome declares distinct fantasy trees/creatures, dark-forest special terrain and its own worker. Its mere presence in MO does not justify retaining it in AMJ's natural world.

- **New world:** `Patches/Compatibility/MedievalOverhaul.xml` conditionally sets `generatesNaturally=false` on `DankPyon_DarkForest`. This is the RimWorld 1.6 `WorldGenStep_Terrain` natural-biome selection gate. Expected result: **zero natural Dark Forest tiles**, even when MO is active. The four AMJ climate bands and accepted Vanilla wetlands remain the target distribution; no replacement biome is added here.
- **Old world/compatibility:** Keep the upstream `BiomeDef` (do not `PatchOperationRemove` it), its worker, animals, map terrain makers and existing legacy soil-ladder patch for already generated worlds, old saves and other MO references. These are compatibility protections, not approval to generate or advertise Dark Forest in AMJ.
- **Implementation boundary:** This suppresses new natural world selection only. It does **not** erase already saved Dark Forest tiles, prohibit a third-party forced biome, globally delete MO fantasy species, or certify every MO-owned creature/incident/merchant as suitable. Those must be audited at their own spawn/recipe/generation paths rather than misrepresenting this biome flag as global exclusion.
- **Legacy terrain profile (kept only for existing/forced Dark Forest maps):** Thin Soil <0.40; Gravel 0.40–0.60; Soil 0.60–0.90; Rich Soil >=0.90. No fertility values changed.
- **Test contract:** The existing natural-world MO Quickstart now requires Dark Forest count **0**, and (with MO present) the Def remains `implemented=true`, `generatesNaturally=false`. The previously required `DarkForest>0` assertion was wrong for the new period-fit policy and is superseded. Real 1.6/1.7 natural-world acceptance for this new patch remains **unverified** until a consolidated targeted run.

### 10.6 Runtime validation

During Alpha, fresh maps in the four AMJ biomes log:

`[AMJ Environment] Map terrain summary`

with shares for Thin Soil / Gravel / Soil / Rich Soil / other terrain.

Acceptance intent:
- Warm/Cool maps retain substantial ordinary Soil and some Rich Soil;
- Thin Soil is visible but not dominant in ordinary lowland forest;
- Subalpine shows a clear shift toward Thin Soil + Gravel;
- Alpine is dominated by poor/stony ground but still has limited growable patches;
- when MO is loaded, `DankPyon_DarkForest` contributes zero newly generated natural-world tiles while remaining a valid referenced Def;
- thresholds are tuned from map-generation results, not by changing terrain fertility values to force a desired percentage.

Fixed-biome Alpha validation results:

| Biome | Thin Soil | Gravel | Soil | Rich Soil | Other |
|---|---:|---:|---:|---:|---:|
| Warm-temperate | 11.3% | 35.5% | 27.4% | 3.2% | 22.5% |
| Cool-temperate | 16.3% | 37.8% | 29.8% | 4.1% | 12.0% |
| Subalpine | 16.9% | 16.5% | 4.8% | 0.8% | 61.0% |
| Alpine | 61.6% | 24.4% | 2.6% | 0.0% | 11.4% |
| MO Dark Forest *(legacy forced-map sample; no longer natural)* | 8.9% | 14.0% | 6.4% | 18.1% | 52.6% |

Interpretation:
- among the four fertility-ladder terrains only, Subalpine is approximately **43.3% Thin / 42.3% Gravel / 12.4% Soil / 2.0% Rich**;
- among the four fertility-ladder terrains only, Alpine is approximately **69.6% Thin / 27.5% Gravel / 3.0% Soil / 0% Rich**;
- Subalpine's large Other share is overwhelmingly natural rough rock: `Limestone_Rough` 34.2% of all cells and `Slate_Rough` 26.1%, together accounting for about 99% of its Other category;
- Alpine's Other share is likewise mostly rough rock/natural wall terrain, not unexpected fertile ground;
- The historical MO Dark Forest forced-map result only checks compatibility for an already selected/forced biome. It is **not** an argument for retaining Dark Forest as a naturally selected Japanese biome.

Result: the Alpha natural-soil thresholds above are accepted without numerical retuning. Further changes require a concrete gameplay or compatibility finding rather than preference from a single terrain percentage.

Existing maps are not retroactively rewritten; the natural-soil pass applies when generating new maps.

## 11. Regional weather baseline

### 11.1 Goal and scope

Environment owns regional weather differences, but Alpha should reuse RimWorld's existing weather system before adding custom weather types or seasonal C#.

Research basis:
- Japan has a humid climate with strong seasonal contrasts; early summer Baiu brings a cloudy/rainy period across most of the country;
- September rainfall is often increased by the autumn rain front and tropical cyclones;
- winter precipitation differs strongly by exposure: the Sea of Japan side and mountains receive frequent/heavy snow, while the Pacific side is often drier and sunnier.

References:
- https://www.data.jma.go.jp/cpd/longfcst/en/tourist.html
- https://www.data.jma.go.jp/cpd/longfcst/en/tourist_baiu.html
- https://www.data.jma.go.jp/cpd/longfcst/en/tourist_japan.html
- https://www.jma.go.jp/jma/kishou/know/kisetsu_riyou/tenkou/Average_Climate_Japan.html

These sources define the climatic shape, not literal RimWorld probabilities. Weather commonalities are gameplay abstractions.

### 11.2 Reuse Vanilla weather mechanics

Do not add AMJ-specific WeatherDefs during Alpha.

Use the existing eight Vanilla entries:
- Clear;
- Fog;
- Rain;
- DryThunderstorm;
- RainyThunderstorm;
- FoggyRain;
- SnowGentle;
- SnowHard.

RimWorld 1.6 already supplies two useful environmental filters:
- weather commonality is multiplied by each WeatherDef's `commonalityRainfallFactor`, so wetter AMJ world tiles naturally receive more rain/fog/snow where the Vanilla WeatherDef defines that curve;
- Rain / RainyThunderstorm / FoggyRain require non-freezing outdoor temperature, while SnowGentle / SnowHard require freezing outdoor temperature. The accepted AMJ seasonal temperature curve therefore converts part of winter precipitation from rain to snow without Environment adding another seasonal selector.

Vanilla does **not** natively apply month/season-specific commonality curves for Baiu, Akisame or typhoon season. Alpha will not add a Harmony patch solely for calendar-specific weather weighting. Add such a layer only if later gameplay shows that the temperature/rainfall-driven baseline fails to create meaningful seasonal weather.

Likewise, Alpha does not attempt a Sea-of-Japan-side versus Pacific-side winter precipitation split. The current four AMJ vegetation bands encode temperature/elevation, not mountain exposure or ocean-facing aspect; fabricating that distinction from the biome name would be misleading. A later coast/orography feature may provide a proper basis if the gameplay value justifies it.

### 11.3 Accepted Alpha base commonalities

| Biome | Clear | Fog | Rain | Dry thunderstorm | Rainy thunderstorm | Foggy rain | Gentle snow | Hard snow |
|---|---:|---:|---:|---:|---:|---:|---:|---:|
| Warm-temperate | 16 | 1.5 | 3 | **0.1** | 1.5 | 1.5 | 2 | 1 |
| Cool-temperate | 16 | 1.5 | 3 | **0.1** | 1.5 | 1.5 | 4 | 3 |
| Subalpine | 16 | 1 | 2 | **0.1** | 1 | 1 | 7 | 7 |
| Alpine | 16 | 1 | 1 | **0.05** | 0.5 | 0.5 | 12 | 12 |

Interpretation:
- warm/cool forest keeps the strongest liquid-precipitation and fog baseline;
- subalpine/alpine progressively shift commonality toward snow as temperature falls;
- dry thunderstorms remain possible for gameplay variety but are deliberately rare in the humid-Japan baseline;
- rainy thunderstorms are always more common than dry thunderstorms;
- higher AMJ tile rainfall further amplifies precipitation through Vanilla WeatherDef curves, so the table should not be interpreted as final raw percentages.

`RainyThunderstorm` is only a broad severe-rain/storm proxy. Alpha does not claim to simulate a literal typhoon.

### 11.4 Automated validation

The existing four fixed-biome Quickstarts also validate weather.

For each AMJ biome the runtime gate must confirm:
- exactly the eight accepted Vanilla weather entries are loaded in the isolated profile;
- all base commonality values equal the table above;
- rainy-thunderstorm commonality exceeds dry-thunderstorm commonality;
- no pre-launch ERROR and no Environment-origin runtime ERROR occurs.

The Quickstarts log `[AMJ Environment Weather]` with the loaded commonality table for each biome. This is a Def/runtime integrity gate, not a year-long stochastic weather-frequency test.

A statistical simulation should only be added if later weather changes introduce calendar-specific or dynamic weighting that cannot be proven from loaded Def values.

The first post-change automated runtime rerun passed cleanly in all four AMJ biomes: every standalone report returned `passed=true`, `failed=0`, `preLaunchErrors=0`, and `logErrors=0`. Loaded weather commonalities matched the table exactly in WarmTemperate, CoolTemperate, Subalpine, and Alpine. The optional AMJE+CCTO run also passed all **39/39** assertions with the same clean startup/runtime conditions, confirming that CCTO does not alter the weather baseline.

The Alpha regional weather baseline is therefore accepted.

## 11.5 AMJ共通リテクスチャ方針 — Environment所有範囲

AMJでいう**リテクスチャ**は、Environment独自Defの画像制作だけを指さない。**Environmentが自分の責務範囲で使用・再利用するVanilla / Medieval Overhaul等の前提Mod資産についても、AMJE追加資産と並べた際に画風・輪郭・色数・陰影・情報密度・解像感が統一されるよう、必要なテクスチャをEnvironment側から差し替えること**を含む。

目的は前提Mod全体を日本風へ変換することではなく、**Environmentを導入したとき、樹木・植物・地形・自然景観が一つのAMJアートセットとして成立すること**である。

### 11.5.1 所有原則

- **Environmentは、環境景観の責務に属する前提Mod資産の画風統一までを原則として所有する。**
- 対象には、Environmentが景観構成に採用するVanilla / MOの樹木・植物・地形・自然物等が含まれる。
- Coreの作物・食材・加工設備等、Core側の責務に属する前提資産はCoreが所有し、Environmentへ集約しない。
- **同一Def / 同一前提資産のテクスチャを複数AMJ Modが競合して上書きしない。AMJ内で1資産1所有Modを原則とする。**
- 所有先が曖昧な場合は、その資産の**主要なゲーム上・景観上の責務を持つMod**を正本とし、ロード順で勝たせる運用はしない。
- 前提資産をリテクスチャするという理由だけで、Retexture専用Modへ集約しない。複数AMJ Modから横断的に利用され自然な1所有Modを決められない場合、または独立外観パックとして単独導入価値が生じた場合だけ分離を再検討する。

### 11.5.2 実装方式と変更範囲

技術実装・既存リテクスチャMod監査・競合規則は、Projectの共通正本 [`Docs/RetextureImplementationGuidelines.md`](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/RetextureImplementationGuidelines.md) に従う。

Environmentでは特に以下を固定する。

- Environmentが所有するVanilla / MO樹木は、**AMJE固有texPathへ明示的にPatchする方式を標準**とし、元Modと同名のtexture pathを置いてロード順だけで勝たせる方式には依存しない。
- Vanilla樹木のPatchはEnvironment所有のretexture patchへ分離する。MO樹木はMO package IDでguardしたoptional compatibility/retexture patchへ分離する。
- `graphicData/texPath`だけでなく、loaded Defに存在する `leaflessGraphicPath` / `immatureGraphicPath` / `pollutedGraphicPath` / snow overlay系等の**実際に使用される全graphic state**を監査する。ArtDirection上の成木/落葉一覧は最低対象であり、総PNG数の固定値ではない。
- VTE / ReGrowth等が同じDef fieldを明示Patchする場合、AMJE所有対象では最終loaded pathがAMJE pathになることを保証する。同名ファイルだけを置くtexture packは、AMJEが固有pathへ切り替えた後は原則として直接競合しない。
- static retextureにC#やvariation frameworkを追加しない。動的表示が本当に必要な別機能だけを例外とする。

- 純粋なリテクスチャでは、対象Defの成長、分布、収量、耐寒性、カテゴリ、ゲームロジック等を変更しない。必要なPatchはgraphic / texPath等の表示差し替えに限定する。
- ゲームプレイ変更も必要な場合は、リテクスチャに便乗させずEnvironmentの通常仕様として別途設計する。
- Vanilla / MOのDefNameや外部参照は可能な限り維持し、前提Modとの互換性を壊さない。
- **全てのVanilla / MO環境画像を機械的に描き直すことは目的にしない。** AMJE追加資産と並べて明確に浮く、頻繁に表示される、地域景観の統一感へ大きく影響する資産を優先する。

### 11.5.3 時代・地域考証

- リテクスチャも通常のAMJ時代・地域考証対象とする。
- 画風統一だけなら元の植物・地形等の同一性を保つ。形態や種類の見せ方を日本向けに変更する場合は、AMJ対象時代・対象地域に存在する姿として妥当か確認する。
- 「日本らしい」という理由だけで時代外・地域外の意匠や植生へ変更しない。
- 元Defと別種・別物に見えるほどの変更が必要な場合は、単純リテクスチャではなく新Def・互換Patch・分布設計等を含めて責務を再検討する。

### 11.5.4 他Modとの境界と開発順

- **Japan OnlyはEnvironmentのリテクスチャを所有しない。** Japan OnlyはMO由来の西洋要素の除去・非表示化だけを担当する。
- 専用Retexture Modは標準構成では作らない。1資産1所有Modで整理できない横断案件が生じた場合だけ再検討する。
- 外部Modの配布テクスチャを複製・改変再配布するのではなく、Environmentが権利上問題のない独自テクスチャを所有し、Patchから参照する。
- 機能・分布検証が不安定なAlpha段階ではプレースホルダーを許容するが、**Environmentの最終アート工程には、AMJE独自植物だけでなく必要なVanilla / MO前提資産のリテクスチャも含める。**
- 既存のENV-010対象となっているVanilla/MO樹木リテクスチャは、このEnvironment所有原則に基づく正式な作業であり、別Retexture Modへ移管しない。

### 11.5.5 既存樹木の再精査と説明統一

既存Vanilla / Medieval Overhaul樹木については、**現在残しているという事実をそのまま採用理由にしない**。ポストBetaのリテクスチャ工程へ入る前に、まず各樹木をAMJEの植生として残す必要があるかを再監査する。

監査順序は以下とする。

1. AMJEの対象地域・対象時代・4植生帯・景観上の役割に照らし、その既存樹木を残す必要があるか判定する。
2. 不要・不適切・代替済みの樹木は、リテクスチャ対象へ自動的に残さず、分布からの除外・置換・非採用を検討する。
3. 残すと判断した樹木だけをEnvironment所有のリテクスチャ対象とする。
4. 残す既存樹木の**表示名と説明文の両方**を日本列島の古代～中世に対応する植生・樹種群へ整合させる。継承元Vanilla / MOの欧米的・汎用的な呼称を無条件で残さず、日本語名を先に確認し、英語名と説明文に反映する。承認済みの説明本文は名称変更を口実に改稿しない。

AMJE植物説明フォーマットは、現行の独自4植物と同様に、原則として**名称・別名 → 日本での分布／生態 → 古代～中世日本での利用・景観・文化的文脈 → 現代との差異や現代利用（裏付け可能な場合）**の順とする。直接史料が弱い場合は用途を捏造せず、景観・分布文脈に留める。

この説明監査には、文化的・歴史的文脈を欠くVanilla説明の補正も含む。特に竹を「美しくない」とするVanillaの文化的に偏った説明は修正対象とし、竹をAMJEに残すかの植生監査と分離せず同じ工程で扱う。

**2026-10-10作者方針変更：表示名も日本向けに修正する。** 表示名は `ThingDef.label`／日本語 `DefInjected .label`（Biome等は該当Defのlabel）で修正し、識別子 `defName`、収量、収穫品、植林、分布、画像、保存データを改名の都合で変更しない。特定種と断定できないVanilla代理樹木は、現行説明・生態・外観に矛盾しない和名の**樹種群の総称**を使う。固有種への限定（例：Oakをミズナラと断定）は、その地域・外観と承認済み説明の整合を別途確認する。残す樹木以外のVanilla/MO由来の植物・動物・Biomeも、実在物への対応を確認して名称を段階的に監査する。Generic植生は根拠なしに日本固有種へ改称せず、MOの幻想種も無根拠に日本在来種と呼ばない。

**2026-10-10実ソース追加監査：** 残るVanilla generic植物7種は種を限定しない名称のまま維持する。Environmentの野生動物9種は§14の**機能的代理**であり、`Bear_Grizzly`をツキノワグマ、`Wolf_Timber`をニホンオオカミ等と**ラベルだけで実在種に偽装しない**。MO 1.6のGreat Oak / Great Iter / Great Fir / Great WillowはMOの独自・幻想的種の同一性を維持し、Environmentは現時点では名称を上書きしない。これらの共用Def名はAMJEの地域Biome外にも影響する。使用した実ソース・対応Def・今回の未変更判断は [`Docs/Research/RetainedContentNameAudit-ja.md`](Research/RetainedContentNameAudit-ja.md) を参照。現在のスコープには新しい名称Patch・種レベル改名は含めない。

したがってポストBetaの既存樹木作業は、**retention audit（残すか） → distribution/ownership decision → Japanese label + approved description → retexture** の順を基本とし、「既存樹木をすべて残してから描き直す」ことは前提にしない。

### 11.5.6 Vanilla自然植物の残存ゲートと樹木量保全

Vanilla自然植物は、既存のBiomeDefに入っていること自体を採用理由にしない。AMJEの各植生帯へ残す場合は、少なくとも以下を満たすこと。

- 日本列島の対象植生帯に対応できる分布・生態上の理由がある。
- 古代～中世というAMJ対象時代に対して明白な時代外来種・未来植物ではない。
- Vanilla Defの名称・外観・季節挙動を、日本側の植物群・景観の代理として扱っても別物へのすり替えにならない。
- AMJE所有の代表植物と役割が重複する場合は、残す側に副次林・林床・遷移帯など明確な役割がある。

GenericなGrass / Moss / Bush / Brambles等は、特定の外来種を主張しない林床・草地フィラーとして残してよい。これに対し、樹木や固有名を持つ草本は個別監査する。

**樹木を削除するときは植生密度と木材供給を別問題として確認する。** `wildPlants` commonalityから木を単純削除すると、総 `plantDensity` が同じでも選択比率が草本へ移り、結果として森林の樹木量・木材量だけが落ち得る。そのため、森林帯で既存樹木を除外する場合は、削除理由と独立した理由がない限り、削除した樹木commonalityをその帯に妥当なAMJE所有樹木または残存樹木へ再配分し、監査前の木本比率を概ね維持する。

例外は高山帯の森林限界で、通常高木を削除した分を別の高木で埋め戻してはならない。この場合はハイマツ等の低木性木本へ必要分だけ再配分し、「木本植生は維持するが通常高木は生やさない」構造を優先する。

2026-10-08監査後の残存方針:
- 暖温帯: Maple / Bambooを残し、主構造はSudajiiへ集約。Vanilla Poplar / Oakは除外。
- 冷温帯: Oak / Maple / Birch / Pineを副次樹木として残す。Oakはナラ類、Birchはカバノキ類、Pineはアカマツ系二次林を表す汎用代理として扱い、後続の説明・外観監査対象とする。
- 亜高山帯: Birchをダケカンバ系の副次高木として残し、汎用Pineは除外してShirabisoへcommonalityを移す。
- 高山帯: 通常高木Pine / Birchと汎用Dandelion / Astragalusを除外し、Haimatsu・Grass・Mossへ再配分する。
- Berryは特定種を断定しない採集用低木、Grass / TallGrass / Brambles / Moss / Bushはgenericな林床・草地要素として当面残す。

Vanilla湿地Biomeも同じ残存ゲートの対象とする。AMJEの4植生帯は `swampiness < 0.5` の陸地を基準にし、湿潤地では `TemperateSwamp` / `ColdBog` が共存できるため、そこに残る `wildPlants` を監査しないとVanilla植生が湿地経由で再流入する。

2026-10-08湿地監査:
- `TemperateSwamp`: Chokevine 0.80をBrambles 0.80へ、Cypress 1.00を削除してWillow 1.00→2.00へ移す。総commonality **7.30**、木本commonality **3.00**を維持する。
- `ColdBog`: Chokevine 3.00をTallGrass +1.00 / Moss +2.00へ、Astragalus 0.10をMossへ、Cypress 0.60をBirch 0.60へ置換する。総commonality **8.22**、木本commonality **1.80**を維持する。
- **2026-10-10作者確定のMO薬草境界：** MOが追加する野生薬草（Mindwort／Poppy／Fleawort／FlyAgaric）の定義、湿地での出現設定、重みをEnvironmentは**削除・再定義・上書きしない**。過去の`Patches/VanillaWetlandVegetation.xml`の`wildPlants`全ノード置換方式は廃止し、**採用済みVanilla種名だけを対象とする条件付き除去＋Vanilla基準値の追加**に修正する。MO独自の`DankPyon_*`エントリはこの選択対象外とし、MOがEnvironmentより先にPatchしても後にPatchしても生き残る。Cypress等のエントリが最初から欠ける場合もエラーにしない。上記7.30／8.22と木本量は**AMJE管理のVanilla基準植生だけ**の値で、MOや他の任意Modが独立に追加する植物を含む最終合計ではない。MO薬草の採集・薬用効果・分類はEnvironmentの新機能にしない。ロード済みMO併用テストは別の必須確認対象。
- Willowは日本の湿地・河畔に対応可能な汎用ヤナギ類代理、Mapleは在来カエデ類代理として残す。ColdBogのBirchは冷温帯～亜高山帯で既に採用しているカバノキ類代理を流用する。
- Vanilla Cypressは湿地性のVanilla樹木であり、日本のヒノキを表す代理としては扱わない。
- `Plant_HealrootWild` は湿地を含む既存供給を維持する。薬系分離により撤去予定はない。

**2026-10-08作者確定：** Environmentでは野生・栽培ヒールルートと既存薬草供給を維持する。日本向け薬用植物と葛根採取・生薬・加工薬は独立薬系Modへ移管し、EnvironmentでのHealroot置換・最終撤去計画は撤回する。正本：[Project薬系企画](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/MedicinalPlantsAndRemedies.md)。

### 11.5.7 植生整備ロードマップ

Environmentの植生整備は、以下の順序を正本とする。

1. **不要なVanilla樹木・植生と、そこから再流入する不適切なBiome bundleを整理する。** 古代～中世日本の植生として残す根拠が弱いものを除外し、必要に応じて木本・総植生commonalityを妥当な日本側植物へ再配分する。AMJE-owned 4植生帯の主要植物監査は2026-10-08時点で完了したが、`TemperateSwamp` / `ColdBog` は植物だけでなく野生動物・病気・天候・説明を含むBiome全体の採用監査を完了してからStep 1を閉じる。
2. **残すVanilla/MO樹木・植物の日本向け表示名と説明文を監査してからリテクスチャする。** 日本語名・地域／樹種対応・Historical Description Guidelinesを確認し、英語ラベルも日本向けの意味に揃える。承認済み本文は維持し、未承認の本文変更は公表しない。実物に一致する範囲で `retention → Japanese label + description → retexture` とする。
3. **不足している日本の自然植生と野生食料を追加する。** 湿地・森林・草地の景観／生態基盤と、[採用済み12資源の山野採集](ForagingIntegrationDesign-ja.md)をEnvironmentが一体的に所有する（2026-10-17作者決定、従来のWFF別Mod方針を撤回）。新規食用樹木・山菜・野生根菜・菌類のDef/食材/画像/分布/採集/必要最小限の食用前処理はEnvironment内に実装する。薬材・治療は独立薬系、農業はGrains等、保存・発酵・複雑な料理は別の食料加工側、狩猟・漁業・海岸採集は対象外。既存の`AMJ_Tree_Shii`、`AMJ_Tree_Beech`、伐採木材、植林、自然分布のDefを重複・置換せず、スダジイへの種実採集を独立付加する。新規採集種/数量/画像/旧セーブ互換は未実装・未承認の部分を残す。
4. **既存の医療供給を維持する。** Wild Healrootの最終撤去工程は撤回。薬系Modの導入をEnvironmentの成立条件にしない。

植生・アート整備は既存医療供給を維持したまま進める。

**2026-10-12作者確定のミズゴケ表示方針：** `AMJ_Plant_Mizugoke` は独立Defとして湿原分布・低温条件を管理し、画像だけVanilla `Plant_Moss` と共用する。独自テクスチャは必須としない。Famine Foodとの食料利用は実Def・Recipe監査を前提とする任意互換であり、画像共用だけでは成立しない。非導入時は既存の非収穫・ゼロドロップ仕様を維持する。詳細は `Docs/NativeVegetationStep3Design-ja.md`。

**2026-10-17湿地4種の最終配分・収穫値確定（本番ソース正本と整合）：** ヨシ `AMJ_Plant_Yoshi` は TemperateSwamp 1.00／ColdBog 0.60、Hay2。ミズゴケ `AMJ_Plant_Mizugoke` は ColdBog 1.20、非収穫・非栽培・通常Cutのみでドロップなし。スゲ0.80／1.10・Hay1とハンノキ0.70／0.45・木材24の既存承認は維持。配分後TallGrassは両湿地1.40／1.70、ColdBog Mossは1.40、湿地植物合計7.30／8.22、木本3.00／1.80。ヨシは浅水＋Soilの既存自然発生実験設定を採用し、極端な浅水偏重を4構成実ゲーム受入で検査する。ミズゴケはVanilla Mossの既存画像のみ借用。CCTO併用時はヨシ5℃休眠、ミズゴケ0℃・-35℃枯死。実バージョン0.1.1からの更新・Steam公開・実ゲームPASSは別のゲート。

**2026-10-17作者訂正 — 湿地4種追加は次回0.2.0の機能追加アップデート：** 葦（アシ／ヨシ）・菅（スゲ）・ハンノキ・ミズゴケ類の4種を揃えた更新は、現行0.1.1から**0.2.0**へ進める。ここでいう大きな更新は機能内容の規模を示し、セマンティックバージョンのメジャー更新（1.0.0）を意味しない。これは版付けとWorkshopでの紹介方針の決定であり、4種の完成・配布・安定版判定を先取りしない。完成まで現行の版数を変更しない。配布準備時にAbout/Manifest/Changelogの同一版数へ更新し、Workshopの既存「Representative Plants / 各環境帯の代表植物」画像・説明セクションに湿地4種の実画像と日英併記の短文を追記する。別ギャラリーは作らず、既存4種の紹介を維持する。画像が無い種やゲーム本番に未実装の種は公開本文で実装済みと扱わない。具体的な本文・8,000-byte制限・日本語先行と英訳同期は `Docs/WorkshopDescription.md` を正本とする。

**2026-10-10作者確定のStep 3実装順：** 湿地4種（葦／アシ、菅／スゲ、榛の木／ハンノキ、水苔／ミズゴケ）の**実装と検証の完了を先行条件**とし、その**後に**優先度Aで記録復旧した**杉（スギ）と薄・芒（ススキ）の追加**へ進む。スギ／ススキ間の着手順は未指定で、湿地工程へ割り込ませない。ここでの「湿地4種の完了」は、4種の本番PlantDef・承認済み説明／画像が実際に導入され、意図した湿地で自然発生し、収穫・除去・非栽培等の確定仕様と既存Biome・4構成（Vanilla／MO／CCTO／MO+CCTO）の必要な回帰が通過した状態をいう。**資料・仮Defの作成、CIのみのPASS、候補画像の存在をもって完了とはしない。** Steamの公開・配信確認は別ゲートとし、開始条件に勝手に追加しない。

**スギの実装範囲：** まず古代・中世以前から存在する**日本在来の天然スギ林の分布**を森林植生として扱う。**中世後期の地域的な植林・木材利用**は歴史的な利用として説明・今後の任意の植林設計で区別し、人工林を自然林の分布に混ぜない。**近現代の全国的な人工スギ林の広がりは再現しない。** 本決定はスギの実装方針を確定するが、植林研究／メニューの解放条件、木材収量、commonality、追加Recipeは承認しない。


### 11.5.8 Vanilla湿地Biomeの採用監査

2026-10-08の再監査では、`TemperateSwamp` / `ColdBog` について「湿地だから残す」だけでは不十分と判定した。

**結論:** 湿地環境自体は日本列島の自然環境として必要であり、両Vanilla BiomeDefの**`defName`・基本Worker・mud / marsh / shallow water等の地形生成**はgenericな湿地基盤として再利用してよい。互換性上の利点も大きいため、現段階では新しいAMJE湿地BiomeDefへ分離しない。**表示用 `label` は2026-10-10作者方針に従い日本列島の類型名へ変更する**（`TemperateSwamp`＝温帯湿地／temperate wetland、`ColdBog`＝冷涼湿原／cool wetland）。名称・説明以外のバイオーム挙動は変更しない。名称表と対応範囲は `Docs/VanillaWetlandBiomeAudit-ja.md` を正本とする。

ただし、AMJEが両Biomeを採用する以上、`wildPlants` だけをPatchした状態を最終形とはしない。以下はEnvironmentの監査対象とする。

- **野生動物:** Vanilla湿地にはRaccoon / Ibex / Muffalo / Alpaca / Megasloth / Rhinoceros / Arctic Fox / Polar Bear / Lynx等、AMJEのJapan-oriented wildlife proxy policyと矛盾する候補が多数残る。湿地でも4基準Biomeと同じproxy原則へ揃える。
- **病気:** VanillaのFlu / Plague / Malaria / parasites / Mechanites等のbundleを、そのまま日本の湿地性疾患として採用しない。AMJの時代・Incident方針と整合させて再監査する。
- **天候:** Vanilla湿地のDryThunderstorm 1および暖冷湿地共通のSnow 4/4は、AMJEの湿潤気候・温度帯別weather baselineと不整合なので補正対象とする。
- **説明:** 「diseaseにchoked」「trees and vines」といったVanilla汎用過酷Biome説明を残さず、日本の湿原・沼沢地の生態と景観へ書き換える。
- **植生:** Phase 5のChokevine / Cypress / Astragalus除外とcommonality再配分は維持する。不足するヨシ・スゲ・ハンノキ・ミズゴケ等はStep 3で追加候補とする。
- **地形:** MarshyTerrain / Mud / WaterShallow / MossyTerrain / Marsh等による湿地構造は再利用候補として維持し、Step 1では新規地形追加を要求しない。

日本の湿地は低地のヨシ・スゲ湿原、ハンノキ林、ミズゴケ高層湿原等を含み、温暖側から冷涼側まで広く存在するため、TemperateSwamp / ColdBogというgenericな二類型自体には残存理由がある。正本の詳細監査は `Docs/VanillaWetlandBiomeAudit-ja.md` とする。

**Step 1の完了条件:** 両湿地のwildAnimals / diseases / weather / descriptionをAMJE方針へ揃え、loaded Defテストで不適切な植物・動物の再流入がないこと、湿地地形生成が維持されること、world上の湿地比率が不意に消失・急増していないことを確認する。これらの回帰ゲートについて2026-10-08に作者からPASS報告を受領したため、**基本Step 1を作者報告ベースで完了**とし、既存植物の残存判定・日本語説明監査へ進める。ただし実機ログによる独立監査や実際の湿地比率の較正、別構成のMO/CCTO統合検証は未完了と区別する。画像リテクスチャの保留は解除しない。

**2026-10-08暫定実装:** `Patches/VanillaWetlandEcology.xml` が両Vanilla湿地の `wildAnimals` / `baseWeatherCommonalities` / `diseaseMtbDays` / `diseases` / `allowedPackAnimals` をAMJE方針に沿って補正する。具体的な全Def・relative commonalityは `Docs/VanillaWetlandBiomeAudit-ja.md` の「湿地生態bundle実装」を正本とする。Vanillaの湿地地形とBiomeWorker、Phase 5植物構成はそのまま保持する。数値は史実の再現値ではなく実機確認待ちのゲームバランス案。説明文の日本語本文は2026-10-08に作者承認済み。英語版と日本語DefInjectedの置換を実装し、**当時は旧VanillaラベルとBiomeWorkerを維持していた**。**2026-10-10にラベルのみ新方針で変更し、BiomeWorkerと既存の説明本文は引き続き維持する。**ロード済み説明文の新たな回帰アサーションについて、2026-10-08に作者から実機テストPASSの報告を受領した。実機ログ・JSONは未添付のため、個別アサーション数、完全キャプチャ、起動前・実行時ERRORゼロを独立再監査したとまでは扱わない。これのみで全構成の統合試験完了や詳細な湿地出現率の確定を宣言しない。確定本文と英訳は `Docs/VanillaWetlandBiomeAudit-ja.md` を正本とする。

**自然分布回帰（2026-10-08）:** 湿地Biomeを強制できる既存Quickstartとは独立して、coverage 0.30の決定的seedの自然生成世界を読み取り専用で計数するテストを追加する。両湿地合算の存在、湿潤度0.5以上の候補の存在、候補外湿地・Biome未割当ゼロ、自然湿地マップへの入植、湿地の陸地比率20%以下を初期の粗い異常検出条件とする。20%は歴史的な適正比率ではなく、精密な目標比率は実測後に再評価する。作者から新しいテストのPASS報告を受けているが、実機ログ・JSONは未受領のため詳細なアサーション数、湿地比率、ERROR検証結果の独立監査は未完了。先行8/8の証拠とは区別する。詳細は `Docs/VanillaWetlandBiomeAudit-ja.md`。

### 11.5.9 Vanilla残存植物 Step 2監査

現行の6Biomeに含まれるVanilla樹木6種とgeneric植物7種の採用状態を再点検し、日本語説明と歴史資料を`Docs/VanillaPlantStep2DescriptionReview-ja.md` に記録した。竹は江戸期のモウソウチクへ一律に同定しない。マツ二次林の歴史的な存在を全自然林への分布根拠にしない。**6樹木すべての日本語本文を2026-10-08に作者承認済み**。既存のAMJE代表樹木4種は3段落構成、今回のVanilla樹木6種は2段落構成だが、いずれも名前・分布・歴史的な利用または景観・現代との差異（裏付け可能な場合）の共通順序を保持する。Birch/Willowは裏付けのない中世利用を加えず景観文脈に留める。改行はXMLリテラル `\\n\\n` に統一する。

竹の英語 `ThingDef.description` Patch・日本語DefInjected・ロード済み検証に加え、楓・楢・樺・松・柳の英訳／説明限定Patch／日本語DefInjected／Quickstartロード後アサーションをPR #30で実装・マージ済み（`bfb27420428dc75a060a317b3c22bdd4c5856fa8`）。**この2026-10-08のPR #30時点では** Vanillaのラベル、Biome・自然分布、植林、ゲームプレイ、画像は変更していなかった。**追加5樹木を含む最新版 `run-tests.bat` は2026-10-08に作者から通過報告あり**。これは作者報告ランタイムPASSであり、最新実行の個別JSON・完全ログ・所有ERRORゼロの独立監査はまだ行っていない。過去の竹テストPASSはそれ自体の旧版証拠として保持する。独立したVanilla/MO/CCTO/MO+CCTOの4構成統合試験は未完了であり、既存樹木のリテクスチャは引き続き保留する。

### 11.5.10 Step 3 不足する日本の自然植生

**2026-10-09一次採取の3構成設計：** `Docs/Research/WetlandPlantMaterialsProfiles-ja.md` にVanilla単体／Medieval Overhaul併用／将来の他AMJ連携の素材責務を記録。**作者確定：スゲはVanilla `Hay`、MOでは既存`Hay→DankPyon_Straw`へ統合し、スゲ専用原料を作らない。将来の菅笠・蓑・敷物も汎用干し草／藁でよい。ミズゴケは選択と情報表示・刈り取り除去を許可、収穫指示・素材ドロップ・栽培は不可。** ヨシのHay採用とヨシ2の収量は未承認。スゲのHay 1は2026-10-12、ハンノキのWoodLog24は2026-10-13にそれぞれ作者承認済み。MOの任意WoodChainと他AMJの製品・Recipeは未実装。2026-10-13にハンノキ本番Defと湿地配分のみ別工程で追加し、実機の伐採・互換性は未確認。

**2026-10-10本文承認：** 湿地4種（アシ／ヨシ、スゲ、ハンノキ、ミズゴケ）の日本語説明本文は、アシ優先とミズゴケ泥炭の修正文を含む最終候補を作者が「OK」と回答して**4件とも承認済み**。正式本文は`Docs/NativeVegetationStep3Design-ja.md` §4、意味を揃えた英訳は同§4.1。**当時は文章承認のみ**。2026-10-13にハンノキの画像4種接続・本番Def/DefInjected・配分・木材収量を追加したが、自然発生・季節表示・実収穫・4構成・Workshopは未完了。他の新規湿地3種にはこの承認を拡張しない。

**2026-10-10工程順の変更（作者指示「画像は最後」）：** 湿地4種では、既存の確定データと未承認草案を再利用し、画像制作より先にParentDef・採取/Cut・野生分布・低温処理・MO/CCTO互換の非画像部分を調査・準備する。RimTest Redux／Pickle／非表示Quickstartsの既存経路を優先し、重複する代替テストは新設しない。必要なら本番ロード対象外の隔離fixtureで先行検証するが、fixtureのPASSは本番PlantDefのPASSではない。画像制作・作者承認後に本番Def/patchと必要な描画状態を統合し、4構成・自然発生・操作・既存セーブの実機回帰を行う。画像未承認のPlantDefや仮texPathを本番配布に混入させず、草案の収量2/1/24・湿地commonalityを今回の工程変更だけで承認済みとみなさない。

**2026-10-13ハンノキ個別確定:** 他樹木とのゲーム内収量比較と日本の湿地林の生態比較を経て、
`AMJ_Tree_Hannoki` は `DeciduousTreeBase` を継承し、`growDays=20`、
`harvestedThingDef=WoodLog`／`harvestYield=24`／`harvestWork=800`／`Beauty=2` を採用。
温帯湿地 `0.70`（Willow `2.00→1.30`）、冷涼湿原 `0.45`（Willow/Birch/Maple各`0.60→0.45`）を
既存木本から移管し、総重み `7.30／8.22` と木本 `3.00／1.80` を維持。
最低生育温度はVanilla 0℃／CCTO 5℃＋休眠。
成熟・落葉・各積雪画像を既存の非再描画原本へ接続。成長縮小で未熟画像を兼用する。
紅葉キーは葉色サンプル、LUT共用は発色検証待ち。植林、実自然発生、伐採・木材チェーン、
季節遷移、既存セーブ、4構成・配布は**未検証**。
正式表と所有ソース：`Docs/NativeVegetationStep3Design-ja.md` §3.1.2。
これ以前の「ハンノキ24未承認／Def未実装」の旧記録は当時の状態を示す。

**2026-10-09作者決定：** ヨシ・スゲ・ハンノキ・ミズゴケの4種は、他Modに同名・類似の植物がある場合でも、**Environment自身の実装対象**とする。Environment一つで中世日本の自然環境を成立させるのが目的であり、重複監査は実装省略判定には使わない。既存Modは任意互換（同時導入時の二重出現、植物分布、Patch競合、ロード後Def）の調査対象のみとする。形式的な実装開始判定記録、日本語説明・画像の作者承認、実機テストは維持する。

**森林候補の復元（2026-10-10）：** **杉（スギ、*Cryptomeria japonica*）もススキと同じ追加植物の優先度A**。2026-10-07の「冷温帯中心・一部暖温帯、林業景観」の候補が正式設計とPlantDefに未記録だったため、`Docs/NativeVegetationStep3Design-ja.md` §3.5に復元。現代人工林と古代～中世の自然植生を同一視せず、独立実装必要性監査、史実本文・画像承認、植生帯分布・植林・採取物・MO互換の検証を別途行う。**スギは未実装・ゲームデータへの反映なし**。

**草地候補の復元（2026-10-10）：** **薄・芒（ススキ、*Miscanthus sinensis*）はEnvironmentの優先度Aの追加候補**。2026-10-07の検討で暖温帯・冷温帯の草地・二次草原向けに位置づけていたが、正式文書と本番Defへは移っていなかった。湿地4種と区別し、独立したVE／非VEの既存Mod・必要性監査、日本語本文・画像の作者承認、自然発生比率・収穫素材の設計を後続で行う。**ススキは未実装・数値と資材未承認**。正本は`Docs/NativeVegetationStep3Design-ja.md` §3.4。

湿地候補・説明草案・承認ゲートは `Docs/NativeVegetationStep3Design-ja.md` を正本とする。2026-10-08最新作者指示で薬系を分離したため、ヨモギ・葛の追加／葛根採取はEnvironment対象外。研究と承認済み説明は[Project薬系企画](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/MedicinalPlantsAndRemedies.md)へ移管した。Environmentは既存Healrootを維持し、旧Step 4撤去計画を実行しない。新PlantDef・画像参照は承認ゲートを通す。

**DLC非所持プロファイルと既存植物の重複（2026-10-09）：** Odyssey専用のReeds/Bulrushがヨシ等の水辺景観と部分的に重なることは公開資料で確認したが、作者の環境にはOdyssey DLCがなく、その実Def・水域配置・パッチ競合をロード済み実機では検証していない。**EnvironmentはOdysseyを必須依存にせず、DLCなしの湿地植生を成立させる。** Odyssey互換は独立した任意機能として保留し、配布可能・互換PASSと主張しない。**旧`wildPlants`全置換方式は2026-10-10に廃止し、Vanilla由来の個別項目だけを変更する方式へ修正した。** Odyssey固有の水域植物と実際にロードされたDefの重複・自然分布・互換性は依然として未検証として扱う。実装前のVE／非VE現行Mod調査、必要性の決定記録、日本語説明・画像承認ゲートは引き続き有効。詳細は`Docs/NativeVegetationStep3Design-ja.md`、既存Mod実XMLの重複監査は`Docs/Research/WetlandPlantExistingModAudit-ja.md`。

## 12. Seasonal scenery baseline

### 12.1 Alpha principle

Environment should not add a parallel seasonal-scenery system when RimWorld already provides the required visual state changes.

The Alpha seasonal-scene model is therefore:
- use Environment's accepted seasonal temperature curve and weather baseline;
- let Vanilla snow weather and SnowGrid provide winter ground/plant snow presentation;
- use one deciduous AMJE structural tree (Japanese beech) for autumn/leafless seasonal contrast;
- keep Sudajii, Shirabiso and Haimatsu evergreen so the four vegetation bands do not all change identically;
- defer final AMJE-owned plant textures, including required Vanilla/MO retextures, until the final-art pass while mechanics/distribution are still being stabilized; this is scheduling deferral only and does not transfer texture ownership to another Mod.

No new Harmony patch, GameCondition, WeatherDef or seasonal calendar controller is added for this stage.

### 12.2 Deciduous / evergreen structure

`AMJ_Tree_Beech` inherits `DeciduousTreeBase`. This preserves Vanilla's fall-color shader parameters and uses a leafless graphic when the plant enters Vanilla's leafless state.

The other three structural plants intentionally remain non-deciduous:
- `AMJ_Tree_Shii` — evergreen warm-temperate broadleaf;
- `AMJ_Tree_Shirabiso` — evergreen subalpine conifer;
- `AMJ_Shrub_Haimatsu` — evergreen alpine dwarf pine.

This creates the intended broad visual progression without inventing AMJE-specific seasonal logic.

### 12.3 Snow

AMJE uses Vanilla `SnowGentle` and `SnowHard`. Both have positive snow rates and feed Vanilla's existing snow accumulation/rendering system.

Because the accepted AMJE weather table already shifts colder bands toward more snow commonality, and the accepted climate model produces winter freezing conditions, snow scenery is treated as an emergent result of the existing temperature + weather systems rather than a separate Environment feature.

### 12.4 Validation

The fixed-biome runtime gate verifies loaded seasonal-scene prerequisites:
- Japanese beech has a loaded leafless graphic;
- Japanese beech retains the inherited `_FallBehaviorEnabled` shader parameter;
- Sudajii, Shirabiso and Haimatsu do not acquire a leafless graphic;
- Vanilla SnowGentle and SnowHard remain loaded with positive snow rates;
- no pre-launch or Environment-origin runtime ERROR occurs.

These checks validate integration/state wiring, not artistic quality. Final texture appearance remains a manual visual check at the final-art stage.

The first automated runtime rerun after adding these checks passed cleanly:
- WarmTemperate: **31/31** assertions;
- CoolTemperate: **31/31**;
- Subalpine: **29/29**;
- Alpine: **29/29**;
- AMJE+CCTO focused run: **43/43**.

All reports had `preLaunchErrors=0`, `logErrors=0`, complete live capture, and no truncated logs. The seasonal-scenery baseline is therefore accepted for Alpha without adding a custom seasonal controller.

## 13. World-to-map river and coast handoff

### 13.1 Ownership

Environment owns the **world-level** frequency and shape context of rivers/coasts:
- Creek / River spawn thresholds and chances;
- no natural Large/Huge rivers;
- increased coastal-tile frequency and coastline complexity.

Environment does **not** implement a second local-map river or coast generator.

RimWorld 1.6 already converts world information into map features through `WorldGenStep_Mutators`:
- a coastal world tile receives the Vanilla `Coast` tile mutator;
- a river-bearing world tile receives the Vanilla `River` mutator in the Core-only path;
- those workers generate ocean water, beach terrain, moving river water and the associated local water data.

AMJE therefore keeps this boundary:
1. alter world distribution;
2. preserve the Vanilla world-to-map handoff;
3. add custom local-water generation only if a later gameplay feature genuinely requires behavior Vanilla cannot express.

This is intentionally compatible with other systems that also understand Vanilla Coast/River mutators.

### 13.2 Alpha validation

Two deterministic developer Quickstarts validate the handoff:
- `AMJRiverMapHandoffQuickstart`;
- `AMJCoastMapHandoffQuickstart`.

Each test:
- selects an unoccupied land world tile carrying the corresponding Vanilla mutator;
- generates a normal 250x250 local map;
- verifies the mutator remains present on `map.TileInfo`;
- verifies at least one local terrain cell is tagged `River` or `Ocean`, respectively;
- remains under the standard isolated-runtime ERROR gate.

These tests do not lock an exact local water-cell percentage. Local map geometry is still Vanilla behavior; AMJE only needs to guarantee that its modified world rivers/coasts continue to reach that behavior.

The first local rerun of the combined Environment runtime gate after adding the River/Coast handoff Quickstarts was reported **PASS** by the author. This confirms the Alpha handoff path is accepted: AMJE world-level river/coast changes continue to reach Vanilla local-map generation without requiring a custom local-water generator. Exact assertion counts and water-cell counts were not recorded from that run, so they are intentionally not treated as design constants.

### 13.3 Waterfalls remain deferred

The existence of working Vanilla local rivers does not change the Waterfall decision in Section 6.

A true or pseudo-waterfall would require additional local-map logic beyond the standard River mutator and is not needed for the Alpha environment loop. It remains deferred until there is a concrete gameplay/visual reason to add it.

## 14. Functional wildlife proxy baseline

### 14.1 Responsibility boundary

Environment does not become a Japan-animal content mod.

The four AMJ-owned BiomeDefs still need functional `wildAnimals` pools so generated maps are not empty and ordinary RimWorld wildlife systems continue to work. Alpha therefore uses a **small curated set of Vanilla PawnKindDefs as gameplay proxies**, not as claims that the literal RimWorld species shown on screen are historically exact Japanese taxa.

Japan-specific animal Defs, retextures, subspecies, hunting products or animal-production loops belong in a dedicated content/resource feature or another owning mod if they later become worthwhile.

### 14.2 Research basis and proxy rule

Environment Ministry distribution material identifies native/widely distributed Japanese mammal groups including sika deer, wild boar, foxes, bears and hares. Raccoon, by contrast, is treated by Japan's invasive-species policy as an introduced invasive animal.

References:
- https://www.env.go.jp/press/files/jp/6252.html
- https://www.biodic.go.jp/kiso/atlas/pdf/3.mammals.pdf
- https://www.env.go.jp/nature/intro/2outline/iaslist.html

Alpha interpretation:
- `Deer` is a functional proxy for Japanese deer / sika-deer ecology;
- `WildBoar` is a functional proxy for Japanese wild boar;
- `Fox_Red` is a functional proxy for native Japanese fox populations;
- `Bear_Grizzly` is only a large-bear gameplay proxy and must not be read as claiming grizzly bears were literally distributed across medieval Japan;
- `Wolf_Timber` is a large-canid gameplay proxy for the historical Japanese-wolf niche, not a species-level reconstruction;
- `Hare` / `Snowhare`, `Squirrel` and `Rat` remain generic small-mammal functional proxies.

Do not add Vanilla `Monkey` merely to represent Japanese macaques: RimWorld's Monkey description/graphic semantics represent a generic curly-tailed tropical monkey and are visually misleading as a macaque stand-in. A future Japan-specific macaque requires its own appropriate Def/art or a compatible external animal mod.

### 14.3 Removed placeholders

The following earlier Vanilla placeholders are excluded from all AMJ biome pools:
- `Raccoon` — modern introduced/invasive species in Japan;
- `Elk` — use `Deer` as the gameplay proxy instead;
- `Ibex` — not an appropriate Japan-alpine proxy;
- `Fox_Arctic` — use `Fox_Red` as the Japan-like proxy;
- `Lynx` — no broad Japanese medieval counterpart suitable for these generic biome pools.

### 14.4 Accepted Alpha pools

| Biome | Vanilla wildlife proxies |
|---|---|
| Warm-temperate | Hare, Squirrel, Rat, Deer, WildBoar, Fox_Red, Wolf_Timber, Bear_Grizzly |
| Cool-temperate | Hare, Squirrel, Rat, Deer, WildBoar, Fox_Red, Wolf_Timber, Bear_Grizzly |
| Subalpine | Hare, Snowhare, Deer, WildBoar, Fox_Red, Wolf_Timber, Bear_Grizzly |
| Alpine | Hare, Snowhare, Deer, Fox_Red, Wolf_Timber |

The exact commonality numbers remain ordinary Alpha gameplay values in `AMJ_Biomes.xml`. They are not historical population-density estimates.

### 14.5 Automated validation

Static validation requires:
- none of the five retired placeholders appears in any AMJ biome;
- every accepted proxy for each biome is present with positive commonality.

The fixed-biome runtime Quickstarts repeat the same check through the loaded `BiomeDef.CommonalityOfAnimal` API. This catches Patch/Def interactions that static XML inspection cannot.

The combined Environment runtime gate executed after these assertions were introduced and was later reported PASS by the author during the ENV-008 verification cycle. Because the same Quickstart source already contained the wildlife assertions at that time, the Alpha proxy set is accepted without a separate manual wildlife smoke test.


### First-release artwork scope (author decision, 2026-10-05)
Finish AMJE's four existing custom plants and their required state graphics/display checks before release. Retextures of existing Vanilla/MO trees are deferred to post-release updates and are not first-release blockers. Verify icons, remaining state display and the actual distribution configuration before release. A visually disruptive existing-tree mismatch may be reconsidered narrowly with evidence; this does not authorize broad retexture work now.
