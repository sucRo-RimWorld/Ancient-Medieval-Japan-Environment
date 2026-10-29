# Step 3：日本の不足植生（薬系をProjectへ移管）

## 2026-10-30 季節性アップデートの対象確定（現行作業範囲）

今回の季節性を主目的とする追加作業は、**ヤマザクラ・ウメ・クリ・カキ・ススキの5種類**に限定する。ヤマザクラの春の花、ウメの冬末～早春の花、クリとカキの秋の実り、ススキの秋の穂・冬の枯れ姿を中心に、季節ごとの景観変化を実装する。

**別工程:** Vanilla既存樹木のヤナギ・マツ・タケのリテクスチャ。今回の新規5植物に算入しない。

**今回の対象外:** スギ・ヒノキ。森林景観拡充の将来候補として保持し、採用可否をここで新たに決定しない。後述の2026-10-10「スギ優先度A」は当時の一般的な追加候補評価であり、今回の5種類の対象範囲を拡張しない。

本節は**対象・目的の確定**であり、5種類の本番Def、数値、画像、季節変化機構、自然分布、実ゲーム検証が完成したという意味ではない。個別仕様と既存の保護対象・互換ゲートは維持する。



## 2026-10-17 作者確定（これ以前の未承認草案記述を上書き）

ヨシ：温帯湿地1.00／冷涼湿原0.60、収穫Vanilla Hay 2。ミズゴケ：冷涼湿原1.20、収穫なし／刈り取り除去・無ドロップ／非栽培。従来承認のスゲ0.80／1.10・Hay 1、ハンノキ0.70／0.45・WoodLog 24は維持する。TallGrassは温帯湿地1.40・冷涼湿原1.70、ColdBog Mossは1.40とし、植生総量7.30／8.22、木本3.00／1.80、Berry/Healrootと任意MO薬草は維持。

本番ソースは `Defs/ThingDefs_Plants/AMJ_WetlandHerbs.xml`、`Patches/VanillaWetlandVegetation.xml`、`Patches/Compatibility/CCTO.xml`。ヨシの水／陸共通Defは過去の隔離実験で自然発生した浅水 `WaterFreshShallowStill` と `Soil` のOR方式を採用。淡水浅水への集中（以前の仮重み0.05で97–99%）は既知の未検証リスクであり、今回の本番重み1.00／0.60で岸辺の陸上群落が再現されたとは主張しない。ミズゴケはVanillaのMoss画像を利用し独立PlantDefに限定分布する。スゲとヨシの growDays=3／harvestWork=60、ミズゴケの growDays=4 は近いVanilla植物に揃えた実装パラメータであり、作者承認の独立数値ではない。実ゲームテストは4種の本番実装後に一括して行う。旧節に残る「未承認」は上記作者確定以前の歴史的記録。


**2026-10-08最新作者決定：Environmentはヒールルートと既存薬草供給を維持する。薬用植物・葛根・生薬・加工薬は独立薬系Modへ分離。研究・承認文正本：[Project薬系企画](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/MedicinalPlantsAndRemedies.md)。本書の旧薬草候補・Harvest案・Healroot撤去条件は失効。湿地植生の設計・承認ゲートは維持する。**

## 1. 所有範囲・先行工程

- 所有者は **AMJ Environment**。対象は古代～中世日本に対応する自然植生と野生植物の分布、表示、野生採集を含む場合の基本生態。江戸期の技術・文化を実装範囲としない。
- **作者確定方針（2026-10-09）：Environment一つで中世日本の自然環境を再現する。** ヨシ・スゲ・ハンノキ・ミズゴケは既存Modに類似植物が存在してもEnvironment自身で実装する。外部植生Mod／Odyssey DLCを必須にしない。他Modは二重出現・分布・Patch競合を扱う**任意互換候補のみ**。調査・日本語説明・画像・テストの開始条件は維持する。
- 正本のロードマップは `Docs/Design.md` 11.5.7、既存種残存とHealroot除外条件は `Docs/VanillaPlantRetentionAudit-ja.md`、湿地の現行配分は `Patches/VanillaWetlandVegetation.xml`、4独自Biomeの現行配分は `Defs/BiomeDefs/AMJ_Biomes.xml`。
- Step 1 湿地生態・Step 2 既存Vanilla樹木説明は作者報告のゲートを経て完了。**既存樹木リテクスチャは作者の指示どおり保留**。新規植物の表示状態確認は別の新規アート工程。
- **2026-10-09一次採取の検討範囲拡張：** 植物を追加するだけでなく、素材を採れるようにするかをVanilla／MO／他AMJの3構成で比較する。提案元資料は`Docs/Research/WetlandPlantMaterialsProfiles-ja.md`。植物と一次採取はEnvironmentが担う。**2026-10-13にハンノキWoodLog 24、10-12にスゲHay 1を個別承認済み**。ヨシの採取品・収量など残る種の値は未承認。屋根葺材・笠・蓑の製品化、加工設備、別分野のRecipe／Job等の大きなループは、Environmentで勝手に実装せず、用途側Modの任意連携とする。未実装機能の存在を説明文で先取りしない。
- **2026-10-10に4種の日本語説明本文を作者承認済み**。確定日本語§4を正本に、英訳§4.1を意味対応させる。各種について画像と未決数値の承認を経るまで、本番Def・DefInjectedへの配置は禁止。**ハンノキは2026-10-13の承認を受け実装中**、他種のゲートは不変。

## 2. 現在の不足と先行Mod監査

| 現行の代理 | 足りない日本の植生 | 意図しない同一視 |
|---|---|---|
| `Plant_TallGrass` | ヨシ、湿地のスゲ類 | TallGrass をヨシ・スゲという特定種／属と断定しない |
| `Plant_Moss` | 泥炭性のミズゴケ類 | 通常のコケを高層湿原のミズゴケと同一視しない |
| `Plant_TreeWillow`・`Plant_TreeBirch`・`Plant_TreeMaple` | 低層湿原のハンノキ林 | Birch は Alnus ではない。河岸・縁辺林と湿原中心部を混同しない |

**正式な実装必要性・所有権の判定記録（2026-10-09）：** `Docs/Research/WetlandPlantImplementationDecision.json` に、4種をEnvironmentが提供する独立実装として、VE・非VE・Vanilla/MO・AMJの証拠SHA256・比較・任意互換候補を記録した。これは**実装するかどうかの判断**に限った`ready`であり、新規PlantDef・草案説明・画像の作者承認や実機互換を意味しない。固定証拠ハッシュ・独自実装の結論・4対象種の回帰は `Tests/Production/test_wetland_implementation_decision.py` で検査する。

**実XMLでのVE／非VE重複監査（2026-10-09）：** `Docs/Research/WetlandPlantExistingModAudit-ja.md` にVE／More Plants、ReGrowth 2、Biomes! Prehistoric、MO／Odysseyの固定1.6ソースと比較・互換性の未解決点を記録した。特にスゲ・ミズゴケには既存のBiomes! Prehistoric PlantDefがあるが、**Environmentの独自実装を省略する理由にはならない**。形式的判定JSONは作成済み。**Projectの正式Python CLIを2026-10-09に同一内容で実行し、構造／証拠バイト照合PASS**を取得した。対象Environmentコミット・Project PRとActionsログを `Docs/ValidationEvidence/WetlandEntryProjectValidator-20261009.md` に記録した。**日本語説明本文は2026-10-10に作者承認済み**。必要な実画像・他の未確定仕様が残るため、新PlantDefの実装着手はHOLD。

**VE最優先の機能重複監査（暫定）：** Vanilla Plants Expanded（Workshop 2134308522）は公開説明上、果樹・作物の追加と農業選択肢を主軸とする。既存農作物・雑草等の発想は参考にする一方、日本の湿地のヨシ／スゲ／ハンノキ／ミズゴケ自然群落をそれだけで満たすとは確認できない。**前提化しない／実装参考・任意互換候補**。MOおよび同Modのロード済みDefとの名称・画像・Harvest重複は、実装PRで実Defを確認する。既存Modの主要内容を大幅削除して前提化しない。

資料：環境省・釧路湿原国立公園はヨシ・スゲ湿原、ハンノキ林とミズゴケ高層湿原を区別。環境省「赤名湿原」はハンノキ・カサスゲを記載。文化庁「ふるさと文化財の森」はヨシを伝統的な草葺き材に含める。これら**現代植生資料から古代～中世の各地の割合・作業技術をそのまま外挿しない**。

### Odyssey DLC重複・非所持プロファイルの扱い（2026-10-09、先行調査）

- RimWorld Wikiの公開情報で、**Reeds（ヨシに景観・水辺機能の重複がある）**と**Bulrush（ガマ型の水生草本）**はOdyssey DLC専用と確認できる。Reedsは泥・湿地・浅い淡水、Bulrushは湿地・浅い静水に現れると記載されている。**Bulrushはスゲ類と同一種ではない**。公開情報での類似は、AMJが予定する日本のヨシ・スゲ・ハンノキ・ミズゴケをすべて代替できる根拠にはならない。
- 提供されたMedieval Overhaul 1.6ソースアーカイブの `1.6/Defs/Biomes/Biomes_DarkForest.xml` には、`Plant_Reeds`（commonality 1）と`Plant_Bulrush`（0.2）がどちらも `MayRequire="Ludeon.RimWorld.Odyssey"` 付きで定義されている。**DLCなしではロードされない条件付きエントリであり、MO単独でReeds/Bulrushが利用できる根拠にはならない。** これは手元アーカイブのXML静的調査のみで、MO＋Odyssey実機テストではない。
- 作者の検証環境にOdyssey DLCがなく、当面はDLC有効状態でのロード済みDef・生育地形・出現分布・テクスチャ・パッチ競合を実機検証できない。**DLCを必須化しない**。DLC未導入のVanilla基準（任意MO/CCTO共存を含む）を主たる実装・回帰ゲートとする。DLC専用Def・画像・参照を通常の配布ファイルへ無条件に追加しない。
- 将来Odyssey互換に着手する際は、実際の`Plants_Water.xml`等の版固定したDef、`wildPlants`再配分経路、Odysseyの水域植物生成経路、同種景観の重複量を確認する。特にAMJEの`Patches/VanillaWetlandVegetation.xml`は湿地の`wildPlants`全体を置換するため、Odyssey由来エントリを保持しているとは**未確認**。DLC向け調整は`Ludeon.RimWorld.Odyssey`条件の任意パッチとして独立させ、実機証拠がない間は互換済み・PASSと表示しない。
- 上記は公開情報を使った**部分的な重複・任意互換監査**であり、互換済みを証明するものではない。**4種のEnvironment実装範囲は作者判断で確定しており、他Modの有無では省略しない。** VE／非VEの実XML・コード、ライセンスと依存の比較は実施済みで、Project `Docs/Research/ExistingModAudit.md` の形式的決定JSONも記録済み。ProjectのPython CLIによる正式な構造／根拠照合は固定Environmentコミット `41db6dc` に対してPASS（証跡は `Docs/ValidationEvidence/WetlandEntryProjectValidator-20261009.md`）。**当時の保留は後日の種別承認で更新される**。2026-10-13にハンノキの収量・分布・画像接続を承認し本番XMLへ適用したが、自然発生・描画・伐採の実機結果は未検証。他種の未決値には承認を拡張しない。**日本語説明本文は2026-10-10に作者承認済み、英訳も§4.1に作成済み**。必要な承認済み実画像・動作確認が揃うまで本番Def配置・出荷はしない。

公開参照：[Reeds](https://rimworldwiki.com/wiki/Reeds)、[Bulrush](https://rimworldwiki.com/wiki/Bulrush)、[Odyssey DLC公式発売案内](https://ludeon.com/blog/2025/07/the-rimworld-odyssey-expansion-is-out-now/)、[任意DLC条件 `MayRequire`](https://www.rimworldwiki.com/wiki/Modding_Tutorials/MayRequire)。

## 3. Step 3 候補・実装順（最終配分値は未決定）

**2026-10-10作者確定：** 実装は**湿地の4種（葦・菅・榛の木・水苔）を完了 → 杉・ススキを追加**の二段階とする。スギとススキは優先度Aでも、湿地4種を追い越して本番植物実装・配布候補へ入れない。両者の間の順序は未指定。湿地4種の「完了」は、実在する本番Def・承認済み日本語説明と必要画像・適切な自然発生分布・確定した採取と除去操作・既存生態とセーブの回帰・Vanilla／MO／CCTO／MO+CCTO各構成の必要テストを満たしたことを指す。**既存の静的テストPASSや設計確定だけでは完了と扱わない。** Workshop公開は別の判断。


| 順位 | PlantDef候補（名前は暫定） | 種・機能範囲 | 主な適用Biome候補 | 分布・互換の留保 |
|---|---|---|---|---|
| 3A-1 | `AMJ_Plant_Yoshi` | 葦（アシ。植物学上の標準和名はヨシ）、河岸・低層湿原の高草丈草本 | TemperateSwamp、ColdBogの低層湿原側 | `Plant_TallGrass`の一部を置換。全水域・高層泥炭地に一様に置かない |
| 3A-2 | `AMJ_Plant_Suge` | スゲ類（特定の一種に固定しない）、湿った草地 | TemperateSwamp、ColdBog | `Plant_TallGrass`の一部を置換。ヨシと見分けられる外観が必要 |
| 3A-3 | `AMJ_Tree_Hannoki` | ハンノキ（Alnus）、低層湿原の落葉湿地林 | TemperateSwamp、ColdBogの適切な湿地林 | 既存Willow/Maple/Birchの木本配分から移す。木本総量維持と研究後植林候補を同時監査 |
| 3B-1 | `AMJ_Plant_Mizugoke` | ミズゴケ類、冷涼な泥炭湿原の蘚苔類 | ColdBogの高層湿原相当 | `Plant_Moss`の一部を置換。通常の木立と均一混在するような無条件配置に注意 |

**2026-10-12 ミズゴケの画像・任意互換方針（作者承認）：** `AMJ_Plant_Mizugoke` は独立PlantDefのまま維持し、描画画像のみ Vanilla `Plant_Moss` と共用する。区別の中心は生育地・自然分布と低温特性であり、ミズゴケ専用の新規画像制作を必須としない。Famine Food系Modとの食料利用は任意互換として検討するが、現物の植物Def・収穫物・レシピを監査し、実際に存在する互換接点を確認してから条件付きパッチを実装する。画像共用だけでレシピ互換が成立するとは扱わない。Famine Food未導入時はミズゴケの既存確定仕様（非収穫・ドロップなし・伐採除去可・非栽培）を維持し、他Modの食料機能を無条件に追加しない。

**4種はすべてEnvironmentの実装対象だが、優先度は一括実装・出荷の承認ではない。** 3Aの最初の小さい実装単位はヨシ1種を想定し、専用テクスチャ・日本語本文承認・Def継承・地形適性・単独自然発生を確認してから拡張する。ヨシとスゲの違い、ハンノキの湿潤立地、ミズゴケの冷涼な泥炭地という区別を検証できなければ、機能を実装せず設計を再検討する。

**配置commonalityの次工程（2026-10-09、設計草案・未承認）：** `Docs/Research/WetlandPlantDistributionDraft.json` に、現行`Patches/VanillaWetlandVegetation.xml`からヨシ・スゲ・ハンノキ・ミズゴケへ重みを移す数値案を記録した。これは**ゲーム本体に読み込まれない設計用JSON**である。**2026-10-12にスゲの出現重み（温帯湿地0.80／冷涼湿原1.10）は作者承認済み。** ハンノキ重み（0.70／0.45）も2026-10-13に個別承認済み。他2種（ヨシ・ミズゴケ）の出現重みと移行案全体は未承認であり、一括確定扱いしない。温帯湿地7.30／木本3.00、冷涼湿原8.22／木本1.80、Berry・Healroot等の採集供給を保持する。この草案の数値を固定する専用テストは撤去済み。本番XMLの配分・木本総量・既存種保護は、実装時に本番Defを対象とする検査で確認する。実生育地形・密度・地域差のゲーム内確認を経てから、最終値を承認・採用する。

### 3.1 Biome構成比・木本総量の保持

現在の `TemperateSwamp` は wildPlants commonality 合計 **7.30**、木本合計 **3.00**、`ColdBog` はそれぞれ **8.22 / 1.80**。この数値は**配分契約の現状基準**であり、古代・中世の実際の面積比率ではない。

- まず元代理のcommonalityを減らして新候補へ移す。候補を単純追加して植物総量や木本量を増やさない。
- ヨシ・スゲは主としてTallGrass側の一部配分。ミズゴケはColdBogのMoss側。ハンノキは木本側。採用済みWillow/Maple/Birchを機械的に全削除せず、前Step 2の「河岸・微高地・林縁」の用途を考慮する。
- 本工程で4独自Biomeに薬用植物を追加しない。将来の新規自然植生を検討する際も、既存草本総量・林床構造と四帯の違いを維持する。
- `wildPlants`は「河岸／湿原中心／微高地」を個別指定する装置ではない。**植物自身の生育地形・水域・肥沃度／地形適性と実際のマップ生成を評価**し、追加先のBiomeを選ぶ。必要なら現行の二類型内で制御可能な範囲を先に検証し、無理に精密な生態分布を実装したと主張しない。
- 湿地の地形・Worker・天候・動物・疾病の完成済みStep 1を破壊しない。新PlantDefの植林タグ／研究前後の植林メニューは意図せず拡大させない。

### 3.1.1 湿地4種の生育場所の分担 — 2026-10-11隔離実測を受けた未承認評価

隔離Quickstartsの**実験専用**単一ヨシDef（浅水タグと`Soil`タグをORで許可し、
両Biomeの既存配分へ仮commonality 0.05を追加）は、
マップ生成時に両湿地で浅水と陸地の双方へ自然発生した。
しかし、浅水ではTemperateSwamp **1,063/3,672セル（28.95%）**、
ColdBog **775/3,542セル（21.88%）**なのに対し、
列挙した陸側3地形（MarshyTerrain・Soil・SoilRich）では
それぞれ**28/46,585セル（0.060%）**、
**11/40,475セル（0.027%）**にとどまった。
この地形別セル比は**当該マップ・仮Defの観測値**であり、
生態学的適正密度、長期再生、本番の配分承認ではない。
根拠のINFO行・除外地形・試験条件・旧試験との比較限界は
`Docs/Research/WetlandPlantTerrainSuitabilityReview-ja.md`および
`Docs/Research/WetlandPlantDistributionDraft.json`の
`dual_habitat_yoshi_natural_spawn_observations`を参照する。

**本番設計に残る制約：** 一つのPlantDefには両Biome内で
水面用・陸地用の独立した`commonality`を持たせられず、
`wildTerrainTags=Soil`は普通土・肥沃土とぬかるみを区別しない。
標準の`wildPlantUseDistanceToShore`も陸地の岸辺分布を作らない。
したがって「単一Defで水陸対応した」ことを
「岸辺の陸上ヨシ群落を再現した」と解釈しない。
草案JSONのヨシ重量（温帯湿地1.00／冷涼湿原0.60）は
**水域適性を有効化した場合の個体数予測値ではなく未承認**であり、
今回の0.05をその代替の本番値にも採用しない。

**配置責務の検討順（決定ではない）：** ヨシは淡水浅水・河岸の景観、
スゲは両湿地の非水域の湿生草地、
ハンノキは非水域の湿地林、
ミズゴケはColdBogの泥炭湿原相当域を第一候補として
相互の機能重複を監査する。ただし`Soil`タグだけでは
スゲ・ハンノキ・ミズゴケの湿地微地形を厳密には限定できない。
ヨシの陸側群落を省くか、専用の立地判定を検討するか、
ヨシDefを分割するかは**未決定**。画像・本番Def・
TerrainDef共通タグ・C#・生態配分を先行変更せず、
既存の種名／採取・CCTO承認／MO薬草保護／
wildPlants総量と木本総量を維持する。

**2026-10-17 スゲ本番実装：** 承認済み成熟／未熟画像、0℃、Hay 1、湿地分布（TemperateSwamp 0.80／ColdBog 1.10）を本番PlantDef・言語・Biome Patch・CCTO任意パッチへ組み込んだ。TallGrassから同量だけ移し、7.30／8.22と木本総量を維持する。成長日数・採取作業量等の未承認数値は新設せず、基底PlantBaseの挙動を利用する。これは**実装済み／実ゲーム未検証**であり、他3種を含む0.2.0の公開準備完了ではない。

**2026-10-12 実装工程の簡素化（作者指示）：** スゲ・ハンノキ・ミズゴケについて、実験専用PlantDefを追加して自然発生の詳細株数を先に測る工程は必須としない。未マージのPR #102は実機実験を行わず終了し、その仮Def・仮Patch・計測処理を本番に取り込まない。既存の地形タグ／肥沃度調査とヨシの自然発生観測を引き続き利用する。以後は、**種ごとに残る未承認事項と必要画像の実物確認・作者承認を満たした時点で、本番PlantDefとBiome配分を同じ実装単位で作成し、その本番Defの自然発生・適地・採取／除去・冬季・既存セーブ・4構成互換を検証する**。新たな仮Def実験を前提条件としない一方、未承認数値を「仮」のまま配布可能な本番値へ昇格させず、未承認画像の代用品も使わない。`Soil`タグで湿原内の微地形を厳密に区別できない制約とMO植物保護、野生植物・木本の基準総量は維持する。

### 3.1.2 ハンノキの本番数値（2026-10-13 作者承認）

作者は、既存AMJE樹木・Vanilla木材と現実の湿地植生との比較を経た
`ｙ` により次を採用した。本節がハンノキ値の正本である。
旧分布JSONに並ぶ他3種の草案と混同しない。

| 項目 | 採用値 | 本番正本 |
|---|---|---|
| 識別子・継承 | `AMJ_Tree_Hannoki`、`DeciduousTreeBase` | `Defs/ThingDefs_Plants/AMJ_WildPlants.xml` |
| 日本語表示名 | `ハンノキ`。漢字表記「榛の木」は既存の承認済み説明文冒頭に記載する | `Languages/Japanese/DefInjected/ThingDef/AMJ_WildPlants.xml` |
| 生育期間 | `growDays=20` | 同上。提案の18～20日から、カバノキ寄りの20日を選択 |
| 木材・採取 | `WoodLog` 24、`harvestWork=800`、`harvestTag=Wood` | 同上 |
| 美観 | `Beauty=2` | 同上 |
| 自然分布・温帯湿地 | `0.70`、ヤナギ `2.00→1.30` | `Patches/VanillaWetlandVegetation.xml` |
| 自然分布・冷涼湿原 | `0.45`、ヤナギ・カバノキ・カエデ `0.60→0.45` | 同上 |
| 木本重み・全植物重み | 温帯湿地 `3.00/7.30`、冷涼湿原 `1.80/8.22` を維持 | 同上 |
| Vanilla最低生育温度 | `0℃` | `AMJ_WildPlants.xml` |
| CCTO併用 | `5℃`・`coldDormancy=true`、CCTO無しで成立 | `Patches/Compatibility/CCTO.xml` |
| 画像 | 通常・落葉・通常積雪・落葉積雪の保存済み画像に接続。未熟専用画像なし | `ArtDirection.md` と本番Def |

- 本番用の秋色は既存 `PlantFallColors` 系の色変換を再利用する。
  葉の代表色から採取した試験キー `(0.423529412, 0.447058824, 0.262745098)` と
  ブナ用LUTの再利用は暫定接続であり、**ハンノキの最終発色・紅葉の採用ではない**。
  正午Clear停止・同条件のネイティブ画素を別検証し、不適切なら種別補正する。
- 既存 `DeciduousTreeBase` と共通の標準成長時縮小を使い、未熟専用PNGを追加しない。
  木材・生育地形・樹高と植林研究メニューの実際の継承結果はロード済みDef/実機で検証する。
- 木本を単純追加せず同じ重みを移管したため、列挙した生態的な分布比は
  **実際に生成された本数比ではない**。水面に生える野生木を増やさないこと、春秋冬の切替、
  Cut/Harvest、MO WoodChain ON/OFF、既存セーブ、4構成の誤参照・ERRORを検証する。
- `Docs/PlantVisualCoverage.json` に7状態を `pending` として登録する。
  静的CIは実機の美観・季節切替・自然発生・セーブ回帰の承認に代わらない。

### 3.2 湿地資材と3構成の接続（未承認）

**2026-10-16 Japanization一式統合による供給補足：** スゲの直接収穫 `Hay` 1は変更しない。Japanizationを入れずMOだけで遊ぶ場合の `Hay → Straw` はMO元の工程。**Japanization＋AMJ全構成**ではGrainsの穀物脱穀が`DankPyon_Straw`を供給し、Japanizationは乾燥ラックでのHay→Strawだけを停止する。Environmentは両系統で同じHay採取PlantDefを維持し、乾燥ProcessDefや穀物Straw Recipeを所有しない。実ゲーム統合検証は未完了。

`Docs/Research/WetlandPlantMaterialsProfiles-ja.md`を一次採取・利用の作業正本とする。**Def構造とMO 1.6実ソース確認：** `Docs/Research/WetlandPlantMaterialDefAudit-ja.md`、実装前回帰 `Tests/Production/test_wetland_material_plant_contract.py`。**ハンノキとスゲは実Defを実装**（スゲは2026-10-17本番XMLを追加、実ゲーム未検証）。ヨシ・ミズゴケの実Defは未実装であり、既存のテスト仮XMLを本番・画面動作の証明に流用しない。**作者確定（2026-10-09）：スゲの一次採取物はVanillaの`Hay`に統合し、MO併用時は既存乾燥棚の`Hay→DankPyon_Straw`経路を使う。専用スゲ原料は作らず、将来の菅笠・蓑・敷物も汎用Hay／MO Strawを利用してよい。** **2026-10-12作者承認：スゲの成熟株1株の基本収穫量は `Hay` 1（`harvestYield=1`）。** 他種の収量・収穫物の承認とは区別する。ヨシのHay採用、ヨシの収量と製品Recipeは未承認。ハンノキは2026-10-13に木材24（Vanilla WoodLog）を承認済み。ハンノキは`WoodLog`が候補であり、MO時のWoodChain設定も別検証。**ミズゴケはクリック選択・名前／説明／成長状態表示が可能で、収穫不可、通常の刈り取りで除去可能、アイテムドロップなし、栽培不可（作者承認済み）。** `selectable=true`相当の設定を維持し、収穫物のDefを持たせず、通常CutPlantでアイテムが出ないこと・播種メニュー非表示を実機確認する。新PlantDef・英訳・画像・実機互換は未実装／未検証。

**実装前のDef参照テスト（2026-10-10）：** `Tests/Production/test_wetland_production_refs.py`は本番`Patches/VanillaWetlandVegetation.xml`の2湿地を読み、追加予定4種のDef名について「自然発生プールで参照した種に対応する実PlantDefが1件存在する」「実PlantDefが存在する種は対象プールから参照される」「ミズゴケは現在の設計ではColdBog限定」を検査する。未実装中は形状・誤参照のモックテストを実行し、本番種がなければ本番参照検査だけをSKIPする。**ハンノキの0.70／0.45とスゲの0.80／1.10は種別承認・本番適用済み**。ヨシ・ミズゴケの残る分布草案をゲーム適用したり、ロード済みの適地・自然発生・画像・採取を検証済みと主張したりしない。

### 3.3 医療供給の維持

Environmentでは野生・栽培Healrootと既存MedicineHerbal供給を維持する。薬草の追加・置換・撤去・医療機能は本工程の対象外。薬系Modの独立設計へ移管する。

### 3.3.1 湿地4種のCCTO低温挙動 — 2026-10-10作者承認

EnvironmentはCCTOを必須依存とせず、4種の基礎PlantDefにはVanilla型の最低生育温度だけを置く。CCTO導入時のみEnvironment所有の`Patches/Compatibility/CCTO.xml`から`CropColdToleranceOverhaul.ColdToleranceExtension`を適用する。CCTO本体へAMJEのDef・数値を移さない。

| 植物Def | Environment単体：最低生育温度 | CCTO併用：最低生育温度 | CCTO低温応答 |
|---|---:|---:|---|
| `AMJ_Plant_Yoshi`（葦） | 0℃ | 5℃ | `coldDormancy=true` |
| `AMJ_Plant_Suge`（菅） | 0℃ | 0℃ | `coldDormancy=true` |
| `AMJ_Tree_Hannoki`（榛の木） | 0℃ | 5℃ | `coldDormancy=true` |
| `AMJ_Plant_Mizugoke`（水苔） | 0℃ | 0℃ | `coldDeathTemperature=-35` |

上表は2026-10-10作者が採用した**ゲームバランス値**であり、自然界の実測枯死温度を主張しない。CCTOの既存Grass/TallGrass（0℃・休眠）、Willow/Birch/Maple/Beech（5℃・休眠）、高耐寒常緑種（−35℃枯死）との役割比較による。アシは多年草の地下部越冬、ハンノキは冬季の落葉を意図する。ミズゴケは落葉休眠にせず、低温生存・積雪表示の実挙動を別に検証する。

**実装条件：** 画像を含む各種の本番PlantDefが承認・作成されるまでは、上表のDef参照・拡張を本番CCTO.xmlに先行追加しない。将来は対象種ごとに最低生育温度を条件付きで置換し、拡張を**ちょうど1個**付与する。既存のAMJE4樹種とCCTO所有Vanilla/MO植物の設定・Def・植物供給、既存MO薬草には触れない。CCTO無しの4種も単独で成立させる。

**冬季の検証条件：** 気温が最低生育温度より上下したときの成長停止・休眠・回復、ColdBogの冬季残存と再生、ミズゴケの−35℃境界、休眠中のHarvest可否・通常CutPlant時の除去とドロップ、春季の実際の再成長、積雪・落葉描画を区別して確認する。冬季アシ刈りはCCTO休眠中のHarvest停止と競合するため、通常Harvestで冬季も採取できるとは扱わない。積雪用画像を先に作るのでなく、**低温時に植物が残るか／どの画像が描画されるか**を非表示の実ゲーム試験で先に調べる。

**実行方法：** 4構成（Vanilla／MO／CCTO／MO+CCTO）のPickle/RimTestおよび必要な描画Quickstartsを、既存の`run-tests.bat`・`run-framework-tests.bat`経由で隔離・非表示実行する。Quickstartsは正常なDirect3D描画を保ち、`-nographics`は使わない。画面が見える可能性のある`run-runtime-tests.bat`直接実行は、この自動検証の入口に使わない。既存の通常プレイModsConfig/Prefsおよびセーブを変更せず、全構成のロード済みDef・自然発生・Cut/Harvest・セーブ再読込・エラー無しを記録する。画像や数値の実機PASSは未取得。ヨシ2など残る未承認値は確定させない。ハンノキ24・成長20日・伐採作業800・Beauty2は2026-10-13に、スゲの `harvestYield=1` は2026-10-12に個別作者承認済み。

### 3.4 草地植生の不足候補：薄・芒（ススキ）— 優先度A・湿地4種完了後に着手

**2026-10-07検討事項の復元（2026-10-10）：** ススキ（*Miscanthus sinensis*）は、暖温帯・冷温帯の開放的な草地、とくに採草・管理によって維持される二次草原の景観を補う、**Environment所有の優先度Aの追加候補**。湿地4種とは異なる生息環境を代表する別工程であり、既存の湿地4種専用判定JSONはススキを承認していない。以前の検討は存在したが正式設計・PlantDefに移管されていなかったため、本節を作る。

- **表示・分布：** 草地・林縁を候補とし、Warm/Cool植生帯の地表・木本比率を調べて配置する。既存のForest系Biomeへ一律追加すると森林内ばかりに生える可能性があるため、開放地を実際に表現できるか確認する。`wildPlants`を追加するだけで人為的な二次草原の形成・維持が再現できるとは見なさない。既存Grass/TallGrassと二重分布にしない。
- **PlantDef候補：** `AMJ_Plant_Susuki`（仮称）。日本独自のススキをgenericな`Plant_TallGrass`と無根拠に同一種扱いしない。**成熟株源画のみ2026-10-10に作者が視覚採用**（誤ってアシと呼んだのを後に「この画像はススキ」と訂正）。証跡は`Docs/ArtDirection.md`「2026-10-10採用源画」節、元画像SHA-256は`d2b5bbca0f4112bacf4426800c92c985d5f52266c21e08602d822f8ff67d340f`。**元PNGはGitHub未転送**で、成熟以外の独自画像、自然生育・収量・栽培可否・積雪時・生育温度・配置比率は未承認。
- **材料と歴史：** ススキは日本の茅葺き屋根材に含まれ、採草・茅場と結び付く。ただしVanilla`Hay`／MO`DankPyon_Straw`への収穫接続、専用茅材や屋根Recipeは**未決**。既に確定したスゲのHay採取仕様を自動で適用しない。
- **開始条件：** ProjectのVE／非VE／Vanilla／MOの既存Mod比較と独自実装必要性の判定、史実に基づく日本語説明文の作者承認、画像の各必要状態の作者承認、その後に独立した分布配分と実ロード・既存セーブ・収穫検証を実施する。湿地4種のテスト・アート承認や従来Biomeの木本総量を崩さない。
- **根拠：** [環境省「阿蘇の草原の種類」](https://www.env.go.jp/park/aso/effort/aso_2/index.html)、[環境省「環境白書」の二次草原](https://www.env.go.jp/policy/hakusyo/h07/9612.html)、[文化庁「ふるさと文化財の森」](https://www.bunka.go.jp/koho_hodo_oshirase/hodohappyo/94187001.html)。

**実装進捗（2026-10-10）：** 成熟株の源画案のみ作者採用として記録（リポジトリへの元画像バイナリ転送は未完了）。新PlantDef・植生XML・本番画像・ゲーム内翻訳・Steam配布物は未変更。湿地4種の承認・数量案をススキへ自動流用しない。画像採用によって湿地4種後という開始順は変えない。
### 3.5 森林の不足樹種：杉（スギ）— 優先度A・湿地4種完了後に着手

**2026-10-07に評価した優先度Aの復元（2026-10-10）：** 杉（スギ、*Cryptomeria japonica*）は、**ススキと並んでEnvironmentの追加植物優先度A**だった。2026-10-07の評価は「冷温帯中心・一部暖温帯、日本の林業景観の不足を補う追加樹種」。その後のStep 3資料が湿地4種中心となる過程で正式設計・本番Defに引き継がれず、記録漏れが生じた。**スギを実装しない、他Modへ移管するといった後続決定は確認されていない**。

- **責務（2026-10-10作者確定）：** Environment自身が日本在来の森林植生として追加する。**まず古代・中世以前からの天然スギ林を自然発生の対象**とする。**中世後期の植林と木材利用は地域的な歴史利用**として区別して扱う。自然スギ林の全域へ人工林を無条件に生成しない。現代のスギ人工林の全国的な分布・面積比も持ち込まない。既存Vanillaのマツ・ヒノキと安易に同一視しない。
- **Biomeと出現：** 2026-10-07の冷温帯中心・一部暖温帯は元の**検討方針**であり、採用commonality・湿度・標高・地形・木本比率は未決。現行のCool/Warm植生帯への配置は、日本の歴史的植生・生態を追加監査してから確定し、既存樹木の木本総量と林床・景観を不用意に変えない。
- **実装候補：** 独立した野生木の仮識別子`AMJ_Tree_Sugi`。PlantDef/Textures/DefInjectedをまだ作らない。自然林優先は承認されたが、ゲーム上で通常植林できるか、どの技術・研究段階で植林を許可するかは**未承認**。木材としてVanillaの既存資源を使うか、MO WoodChainとどう接続するか、収穫量・成長期間も未承認。
- **着手条件：** Project共通のVE・非VE・Vanilla/MO既存Mod比較と独自実装の要否を**スギの範囲で**判定。日本語の史実説明を作者承認後に英訳し、全必要画像状態の実原画・作者承認、通常の分布・植林・伐採・MO任意併用・既存セーブを検証する。既に完了した湿地4種の独立実装必要性判定を、スギの承認に転用しない。

**優先度Aの漏れ監査（2026-10-10）：** 2026-10-07に**Aと明示評価した追加植物はスギとススキの2候補**。ススキは本書§3.4に復元済み、スギは本節で復元する。当時のヨシはA−、クリB＋、ヒノキBであり、**Aと区別する**。後続でヨシは湿地4種に、実・木の実の採集はEnvironment外へ責務移管されたので、これらの優先度だけを根拠に現在の対象を勝手に変えない。

**状態：** 設計・優先順位のみ記録復旧。新規植物データ、画像、収量、分布、既存セーブ、Workshop配布物は変更なし。
## 4. 日本語説明 — 2026-10-10作者承認済み正本

**2026-10-10作者承認：** 史資料再点検・文章統一後、アシ優先の呼称と「枯れた部分が分解されにくい状態で堆積し、泥炭の形成に関わる」というミズゴケの修正文を含む**以下4種の日本語説明本文を作者が「OK」と回答し正式承認**。引用段落を日本語の確定正本とする。承認は**説明本文のみ**で、ゲーム内表示ラベル、採取量、収穫物の未決部分、植生比率、生育条件、テクスチャ、PlantDef、Steam掲載や配布を承認するものではない。英訳は許可されるが、本番Def/DefInjectedへの転記は画像を含む実装ゲートを通過後に行う。

説明は「漢字と一般名・別名 → 日本の生息環境 → 古代～中世との接点」を優先する。機能の実装有無や開発状態を本文に混ぜない。未証明の利用史／現代との比較は創作しない。研究上の留保と出典は `Docs/Research/WetlandPlantHistoricalEvidence-ja.md` に記録。英語版は§4.1の承認済み日本語本文に忠実な翻訳を採用する。実際のゲーム内説明では日英ともに2段落を `\\n\\n` で分割する。

### 葦（アシ、ヨシとも呼ぶ）— `AMJ_Plant_Yoshi`

> 葦（アシ、ヨシとも呼ぶ）は、河川や湖沼の岸辺、水の多い低地に群生する背の高い草本。周囲から流れ込む水に潤される低層湿原では、スゲ類とともに葦原をつくる。
>
> 葦は古くから屋根を葺く茅の材料に用いられた。茅にはススキや稲藁・麦藁なども含まれ、地域や建物によって材料が使い分けられてきた。

根拠：環境省「釧路湿原国立公園の特徴」（低層湿原の生態）、文化庁「ふるさと文化財の森」（ヨシほかの茅屋根材）、国立科学博物館の植物データベース（標準和名ヨシ・別名アシ）、国交省太田川水系植物資料（古称アシ）。歴史的な日本語表示は「葦（アシ）」を優先し、ヨシは別名として併記する。**全国の呼称頻度を断定しない。内部識別子`AMJ_Plant_Yoshi`は変更しない。ヨシ→Hayの採取素材は未承認**。本文は実装済みの屋根材Recipeを主張しない。

### 菅（スゲ、スガとも呼ぶ）— `AMJ_Plant_Suge`

> 菅（スゲ、スガとも呼ぶ）は、湿った草地や低層湿原に生えるスゲ類の総称。細長い葉を株状に茂らせ、葦原やハンノキ林の近くにも群落をつくる。
>
> 『万葉集』には菅や菅笠を詠んだ歌が残り、その葉は古くから笠や蓑の材料として用いられてきた。

根拠：國學院大學「万葉神事語辞典 すげ」、環境省湿原植生資料。ゲーム内の汎用`Hay`採取と基本収穫量 `Hay` 1 は**作者承認済み**だが、完成した菅笠／蓑のRecipeは**未承認・未実装**。植物固有の原料アイテムを増やさない。

### 榛の木（ハンノキ）— `AMJ_Tree_Hannoki`

> 榛の木（ハンノキ）は、川沿いや地下水位の高い湿地に生える落葉高木。湿った低地に林をつくり、アシやスゲの草地とともに湿地の景観を形づくる。
>
> 縄文時代の関東地方の谷にはハンノキ属などからなる湿地林が成立していたことが、遺跡から出土した自然木の調査で確かめられている。こうした林は、古くから日本の河川低地にみられた自然植生の一つである。

根拠：森林総合研究所「関東地方の遺跡出土材の識別による木材利用史の解明」、環境省湿原植生資料。縄文期の**ハンノキ属**に関する史料をハンノキ単一種の中世全国分布や木材製品の用途へ拡大しない。

### 水苔（ミズゴケ類）— `AMJ_Plant_Mizugoke`

> 水苔（ミズゴケ）は、湿原の地表を覆うように群落をつくるコケ類。冷涼で雨水に潤される高層湿原ではミズゴケ類が広がり、枯れた部分が分解されにくい状態で堆積し、泥炭の形成に関わる。
>
> 河川や地下水の影響を受けるアシ・スゲの低層湿原とは、水の供給や植生が異なる。日本の冷涼な地域に発達する泥炭湿原を特徴づける植物の一つである。

根拠：環境省「釧路湿原国立公園の特徴」「重要湿地・別寒辺牛湿原」、環境省自然環境保全基礎調査「特定植物群落調査」。**選択・刈り取り可能、収穫・栽培不可、ドロップなしは作者承認済み**。史実に裏付けのない古代～中世の医療・資材利用を本文へ追加しない。

## 4.1 English translations aligned with approved Japanese

**Prepared 2026-10-10 after Japanese author approval.** The following English descriptions translate the four Japanese paragraphs in §4; these are translation source text, **not a claim that game Defs, graphics, in-game labels, or translations have been installed or runtime-tested**. Do not add extra historical claims, product Recipes, or unapproved harvest behavior when using them. Preserve two paragraphs per species using literal `\\n\\n` within RimWorld XML/DefInjected.

### Reed (ashi; also called yoshi) — `AMJ_Plant_Yoshi`

> Reed (ashi, also called yoshi) is a tall herbaceous plant that grows in dense stands along rivers and lakes and in wet lowlands. In low-lying fens supplied with water from their surroundings, it forms reedbeds alongside sedges.
>
> Reeds have long been used as thatching material for roofs. Thatch also includes Japanese silver grass and rice or barley straw, and the materials used have varied by region and building.

### Sedge (suge; also called suga) — `AMJ_Plant_Suge`

> Sedge (suge, also called suga) is a collective name for sedges that grow in wet grasslands and fens. Their long, narrow leaves grow in dense clumps, forming stands near reedbeds and alder woods.
>
> The Man'yōshū includes poems about suge and hats made from it. Its leaves have long been used to make hats and traditional rain capes.

### Alder (hannoki) — `AMJ_Tree_Hannoki`

> Alder (hannoki) is a tall deciduous tree that grows along rivers and in wetlands with a high water table. It forms woods in damp lowlands, shaping wetland landscapes together with reeds and sedges.
>
> Studies of naturally occurring wood recovered from archaeological sites show that wetland forests containing trees of the alder genus existed in valleys of the Kantō region during the Jōmon period. Such forests were one of the forms of natural vegetation found in Japan's river lowlands long ago.

### Sphagnum moss (mizugoke) — `AMJ_Plant_Mizugoke`

> Sphagnum moss (mizugoke) is a group of mosses that forms mats over wetland ground. In cool, rain-fed raised bogs, sphagnum spreads, and its dead parts accumulate where they decompose slowly, contributing to the formation of peat.
>
> These bogs differ in their water supply and vegetation from reed and sedge fens fed by rivers or groundwater. Sphagnum is one of the characteristic plants of peat bogs in Japan's cooler regions.

## 5. 新規アート・技術設計の実装ゲート

**2026-10-10作者指示：画像制作は最後。** 本工程は確定済み日本語説明・低温仕様・採取操作、既存の収量/配置草案とテストを再利用し、先に非画像のDef継承/Job・分布・MO/CCTO条件付き動作を調査・検証する。実ゲームでの先行試験が必要なら、本番ロード対象外の隔離されたテスト用データだけを使い、正式Def・素材の完成と混同しない。作者承認を要する数値・素材は草案のまま保持する。最後に新規画像の制作・作者承認、本番PlantDef/画像/CCTO互換の統合、通常の非表示4構成・セーブ/自然発生/描画ゲートを通す。この順序変更は、未承認のPNG・仮texPath付きDefを配布する許可ではない。

- **DLC非導入を標準の成立条件とする。** 基本湿地プールはDLC固有の`Plant_Reeds`・`Plant_Bulrush`を参照せず、既存Healrootの全Biome供給を維持する。Odyssey同時導入時の重複・水域生育・`wildPlants`置換挙動は未試験なので、現時点ではOdyssey互換パッチを実装しない。

- 新PlantDefを生かすには対応する**実在・承認済みPNG**が先に必要。既存Vanilla/MOの樹木・雑草画像を、史実上不一致の日本種として無断流用しない。欠損texPathや仮画像でリリースしてはならない。
- 新規植物は既存 `Docs/GoldenPaths/RetextureGeneration.md` → `TextureAssetPipeline.md` → `Docs/PlantVisualCoverage.json` の手順へ追加する。**共通画風を既存の承認済み画像と現物比較**し、作者承認前のイメージを自動採用しない。
- ヨシ：細く長い葉と直立した稈・穂、スゲ：低めで葉が叢生、ハンノキ：落葉性の小～中型湿地樹冠、ミズゴケ：水際地面を覆う低いコケ状。**作画指示案であって完成画像ではない**。
- **画像状態の必要性は種ごとに決める。** 葦（ヨシ）と菅（スゲ）は穂／花穂がない未熟状態を、成熟状態とは別の承認済み画像で表現する。未熟画像の実際の切替条件とロード済み `immatureGraphicPath` は本番Def・RimWorld 1.6の描画で確認し、画像があるだけで自動的に使用されるとはみなさない。
- **ハンノキは未熟専用画像を作らない。** 作者が暫定採用した成熟画像を成長に応じて縮小する標準描画を使い、樹高と成長の実表示を検証する。試作した未熟画像は不採用とする。既存ブナ `AMJ_Tree_Beech` と同じ `DeciduousTreeBase` の落葉機構を基本とし、通常・落葉・積雪・落葉時積雪のグラフィック状態を分けて検証する。ブナの本番XML正本は `Defs/ThingDefs_Plants/AMJ_WildPlants.xml`。落葉にはハンノキ固有の `leaflessGraphicPath` 対応画像が必要で、ブナの落葉画像をハンノキの外観として直接流用しない。
- **ハンノキの紅葉は専用紅葉PNGや独自C#を原則追加しない。** ブナで稼働中の `graphicData/shaderParameters`（`_FallColorSource`、`_FallColorDestination`、`_FallTransitionRange`、`_FallBehaviorEnabled`）と標準 `PlantFallColors` の方式を採用する。ただしブナの色値／グラデーションPNGをハンノキの生物学的な色として無検証で固定しない。実画像に合う色変化を日中Clearの同条件で確認し、通常→紅葉→落葉の季節遷移を本番Defで確認する。
### ブナ・ハンノキ共通の紅葉品質改善（2026-10-12 作者指示）

- **作業対象はブナの既存紅葉改善とハンノキへの紅葉適用の両方。** ブナの現行紅葉は作者から「黄色に偏る」「通常画像の葉色が淡く、美しい紅葉にならない」と指摘され、既存の紅葉採用履歴を消さず、品質再評価の対象にする。既存の通常・落葉・積雪画像は承認済みの原本を保持し、比較前に上書きしない。
- **現物と作用範囲を分離して監査する。** ブナ通常画像 `Textures/Things/Plant/AMJ/Beech/Beech_A.png`、既存 `Textures/Other/AMJE_BeechFallGradient.png`（DDSも配布）、`Defs/ThingDefs_Plants/AMJ_WildPlants.xml` の `_FallColorSource` / `_FallColorDestination` / `_FallTransitionRange` / `_FallBehaviorEnabled` を正本として、通常葉色・変換の色域・ゲーム内最終発色を別々に評価する。現在のグラデーションが黄土色～黄褐色に寄ることは実画像確認済みだが、通常葉色だけが原因と断定しない。
- **ブナの目標色域：** 黄色単色への偏りを避け、黄・黄橙・橙・赤橙・褐色の表現幅を候補として比較する。参考写真の樹種、撮影条件、秋色の個体差を考慮し、森林全体が必ず均一な赤橙色になる仕様は固定しない。元画像の緑色を濃くする案は、通常季節の景観を損ねないことを比較してから採否を判断する。
- **ハンノキは同じ紅葉機構を使い、カラーマップは共用案を先に検証する。** 日本のハンノキの黄・黄橙・橙・条件による赤橙の参考例を評価し、ブナ用グラデーションを未検証でそのまま確定しない。共用で両種の自然な紅葉を再現できない場合のみハンノキ用グラデーションを分離する。専用紅葉C#、専用紅葉の通常PNGを先に追加しない。
- **検証順と合格条件：** (1) 既存グラデーションとブナ原画の色調・アルファ／彩度を現物で確認、(2) グラデーションのみ調整した案と必要なら通常葉色も調整した案を同一条件で比較、(3) ブナの通常時と紅葉時の双方で見た目が改善するか判定、(4) 共用案をハンノキ原画と同条件で比較、(5) 必要な場合のみ種別色設定／カラーマップ分離、(6) 両樹種とも自然な季節移行・落葉への切替と描画エラーなしを確認する。色評価は `Docs/GoldenPaths/PlantVisualCoverage.md` の正午・Clear・停止・同ズーム・同季節強度で行い、確認項目、比較画像、色値・読み込みパスを記録する。ブナの既存PASSを新たなハンノキの合格証拠として使わない。
- **承認範囲：** ここで確定するのは再評価作業・比較基準・既存方式の再使用であり、新カラーマップの具体的ピクセル、ブナ通常画像の再彩色、ハンノキ本番Def、紅葉実機合格を承認したことにはならない。変更が必要なら原画・派生物・定義の整合性を保ち、実際のゲーム内比較後に採用を判定する。

- **積雪は既存の描画方式を再利用する。** ブナの `snowOverlayGraphicPath` および `leaflessSnowOverlayGraphicPath` と同じ仕組みを使い、ハンノキの通常／落葉の樹形に合致した画像状態が必要かを実描画で確認する。春夏の通常画像だけで季節画像の完成を宣言しない。
- **ミズゴケは作者承認の画像共用方針を優先する。** 独立 `AMJ_Plant_Mizugoke` Defの外観はVanilla `Plant_Moss` 画像を共用し、未熟・成熟の専用画像は制作不要。分布・低温条件は独立。Famine Foodの利用は実Def／Recipeの監査後に条件付き互換とし、未導入時は非収穫・ゼロドロップを維持する。
- 植物ごとのUI表示・積雪表示を実際の読み込みパスで監査する。未完成の画像・本番Def接続・ゲーム内動作を、画像の候補生成や既存ブナのテスト結果から完了扱いしない。
- 第一実装PRは候補1種ごとにDef構造、地形適性、自然発生数・比率、画像読込BadTex検出、表示状態、植林候補の意図的非拡張、バニラ／MO任意共存を試験する。日本語承認後の英訳・DefInjectedの一致を検証する。
- 自動テストは `run-tests.bat` の非表示RimWorld描画経路で行い、所有ERRORゼロ・完全ログを要求。既存の暖温帯／冷温帯／亜高山／高山／湿地2種／河川／海岸／自然湿地世界サンプルの回帰を維持。別の Vanilla/MO/CCTO/MO+CCTO 4構成検証は別ゲートである。
- レンダリングが必要でも視覚評価のためユーザーを長時間の手動テスターにしない。種同定・絵柄・自然な密度と見え方など人間判断を要する最終確認のみ手動。

## 6. 参照・比較先（研究・採用の根拠）

- 環境省「釧路湿原国立公園の特徴」 https://www.env.go.jp/nature/nationalparks/list/kushiro-shitsugen/feature/
- 環境省「重要湿地・赤名湿原」 https://www.env.go.jp/nature/important_wetland/wetland/w376.html
- 環境省「重要湿地・別寒辺牛湿原」 https://www.env.go.jp/nature/important_wetland/wetland/w033.html
- 日本漢字能力検定協会・漢字ペディア「榛の木」 https://www.kanjipedia.jp/kotoba/0003658400
- 文化庁「ふるさと文化財の森」 https://www.bunka.go.jp/koho_hodo_oshirase/hodohappyo/94187001.html
- Steam Workshop「Vanilla Plants Expanded」 https://steamcommunity.com/sharedfiles/filedetails/?id=2134308522

## 7. 次回実装前に固定する条件

1. **日本語説明文4種は2026-10-10に作者承認済み**。§4.1の英訳との意味・改行対応を本番実装時に検査。各PlantDefの種代理の範囲・表示ラベル・未決の採取物と数値は実装前に別途審査し、薬用植物は薬系Modの責務。
2. 各新規植物の承認済み画像、状態差、描画・地形適性（野生生成が消滅しないこと）。
3. 二つの湿地と暖・冷温帯のcommonality移管案（元代理・新種・総量・木本量の増減を表にして確認）。
4. ハンノキの通常植林・研究前後メニューへの影響と、MOの植物／資源との重複監査。
5. 薬系の実装は行わず、既存Healroot供給を維持する。Odyssey非導入を標準構成とし、Odyssey互換を実機未検証と記録する。

