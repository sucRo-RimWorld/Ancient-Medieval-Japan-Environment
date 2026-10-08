# Vanilla植物残存監査

AMJEのVanilla植物は、「Vanillaに存在するから残す」のではなく、古代〜中世日本の対象植生帯に残す積極的理由がある場合だけ採用する。

## 段階的削除

### Phase 1 — 暖温帯林のPoplar

対象: `Plant_TreePoplar`

判定: **削除**

理由:
- AMJEの暖温帯林はスダジイを中心とする日本の照葉樹林を基準とする。
- 日本在来のPopulus属そのものを否定するものではないが、現在の汎用Vanilla Poplarをこの帯の副構造樹木として置く積極的理由がない。
- 河畔林や北方のPopulus系植生が将来必要になった場合は、適切な日本種・分布条件を別途設計する。

実装:
- `AMJ_WarmTemperateForest/wildPlants` から `Plant_TreePoplar` を除外。
- 暖温帯Quickstartで自然生成数0を回帰条件にする。
- 他のVanilla植物はこのPhaseでは変更しない。

## Phase 2 — 暖温帯林のOakと樹木量補正

対象: `Plant_TreeOak`

判定: **暖温帯林から削除**

理由:
- 日本の暖温帯自然林には常緑カシ類も含まれるが、Vanillaの `Plant_TreeOak` は温帯林用の汎用落葉広葉樹として扱われており、AMJE暖温帯林の照葉樹構造を表す役としては不適切。
- 暖温帯林の主構造はAMJE所有のスダジイへ寄せ、カシ類を将来明示する必要が生じた場合は日本向けの種・表示として別途設計する。
- Oak削除だけで樹木候補比率を落とさない。先に削除済みのPoplar 0.15と今回のOak 0.40を合計した0.55をスダジイへ戻し、スダジイ commonality を2.00→2.55とする。

樹木 commonality:
- Poplar削除前: 3.30
- Phase 1後: 3.15
- Phase 2後: **3.30**
- 総wildPlants commonalityもPoplar削除前の13.42へ戻るため、Vanilla樹木整理だけを理由に暖温帯林の樹木量を減らさない。

実装:
- `AMJ_WarmTemperateForest/wildPlants` から `Plant_TreeOak` を除外。
- `AMJ_Tree_Shii` を2.55へ増加。
- 暖温帯QuickstartでOak / Poplarの自然生成数0を回帰条件にする。

## Phase 3 — 亜高山帯の汎用Pineとシラビソ密度補正

対象: `Plant_TreePine`

判定: **亜高山帯から削除**

理由:
- 本州の亜高山帯自然林はシラビソ・オオシラビソ・コメツガ等の常緑針葉樹が主要構造で、AMJEではその代表を `AMJ_Tree_Shirabiso` が所有する。
- Vanillaの汎用Pineはアカマツ系の二次林表現として冷温帯では残存余地があるが、亜高山帯の主要高木として重ねる必要はない。
- Pine 0.90を削るだけでは樹木比率が落ちるため、その0.90をシラビソへ移し2.60→3.50とする。

樹木 commonality:
- 変更前: 4.00（Shirabiso 2.60 + Pine 0.90 + Birch 0.50）
- 変更後: **4.00**（Shirabiso 3.50 + Birch 0.50）
- 総wildPlants commonality 16.14も不変。

実装:
- `AMJ_SubalpineForest/wildPlants` から `Plant_TreePine` を除外。
- `AMJ_Tree_Shirabiso` を3.50へ増加。
- `Plant_TreeBirch` はダケカンバ相当の副次高木として暫定残存。
- 亜高山帯QuickstartでPine自然生成数0を回帰条件にする。

## Phase 4 — 高山帯の汎用高木・花の除外

対象:
- `Plant_TreePine`
- `Plant_TreeBirch`
- `Plant_Dandelion`
- `Plant_Astragalus`

判定: **高山帯から削除**

理由:
- `AMJ_AlpineZone` は森林限界より上を表すため、通常の高木Pine/Birchを低率でも常設する必要はない。高山帯の木本構造はハイマツが担う。
- Vanilla Dandelionは種を特定しない汎用タンポポで、現代日本で広く見られるセイヨウタンポポは20世紀初頭の移入記録がある。AMJEの高山植生をこのDefで代表させる積極的理由はない。
- Vanilla Astragalusも「alpine climates」の汎用属表現に留まり、日本の対象時代・対象高山植生をこのDefで代表させる根拠が弱い。必要なら日本固有・在来の高山草本を別途追加する。
- 高山帯では普通の高木を補充しないこと自体が設計目的。ただし植生候補総量と木本候補比率が削除だけで落ちないよう再配分する。

commonality再配分:
- Pine 0.02 + Birch 0.02 → Haimatsuへ移し1.30→**1.34**。
- Dandelion 0.20 + Astragalus 0.20 → Grass +0.20 / Moss +0.20。
- 総wildPlants commonalityは**7.31のまま不変**。
- 木本 commonalityもHaimatsu込みで**1.34のまま不変**。ただし通常高木は0となり、森林限界を明確化する。

実装:
- 上記4 Vanilla Defを `AMJ_AlpineZone/wildPlants` から除外。
- `AMJ_Shrub_Haimatsu` を1.34、Grassを2.20、Mossを3.20へ補正。
- 高山帯Quickstartで4 Defの自然生成数0を回帰条件にする。

## Phase 5 — Vanilla湿地Biomeの植生流入監査

AMJEの4植生帯は通常陸地を置き換える一方、`swampiness >= 0.5` の湿地ではVanilla `TemperateSwamp` / `ColdBog` が残る。したがってAMJE独自Biomeだけを整理しても、湿地経由で不適切なVanilla植物が再流入する。

### TemperateSwamp

**除外:** `Plant_Chokevine`, `Plant_TreeCypress`

**残す:** TallGrass / Bush / Willow / Maple / Berry / 一時的なWild Healroot

再配分:
- Chokevine 0.80 → Brambles 0.80
- Cypress 1.00 → Willowへ移し1.00→2.00
- 総commonality: **7.30 → 7.30**
- 木本commonality: **3.00 → 3.00**

Willowは湿地・河畔のヤナギ類代理として成立する。Vanilla Cypressは湿地性の樹木で、日本のヒノキ代理として扱わない。

### ColdBog

**除外:** `Plant_Chokevine`, `Plant_TreeCypress`, `Plant_Astragalus`

**残す:** TallGrass / Moss / Bush / Willow / Maple / Berry / 一時的なWild Healroot。Cypress分はBirchへ置換する。

再配分:
- Chokevine 3.00 → TallGrass +1.00 / Moss +2.00
- Astragalus 0.10 → Moss +0.10
- Cypress 0.60 → Birch 0.60
- 総commonality: **8.22 → 8.22**
- 木本commonality: **1.80 → 1.80**

Wild Healrootと既存薬草供給は維持する。2026-10-08薬系分離により、Yomogiへの置換・撤去計画は撤回。

## 現時点の残存判定

### Generic林床・低木

**残す:** `Plant_Grass`, `Plant_TallGrass`, `Plant_Brambles`, `Plant_Moss`, `Plant_Bush`

特定の外来種を主張しないgenericな林床・草地表現として利用する。日本の各植生帯にも草本・蘚苔類・低木層は存在し、これらを全てAMJE独自種へ置換する必要はない。

### Berry

**残す:** `Plant_Berry`

採集可能なgeneric berry shrubとして利用する。特定の海外種を史実上の日本種として断定しない。将来、日本固有の果実低木を追加する場合は役割重複を再監査する。

### Maple

**残す:** 暖温帯・冷温帯の `Plant_TreeMaple`

日本には多数の在来カエデ類があり、暖温帯から冷温帯の副次樹木としてgeneric Mapleを置くこと自体は成立する。後続でAMJE形式の説明・外観監査を行う。

### Bamboo

**残す:** 暖温帯の `Plant_TreeBamboo`

竹・ササ類は日本の植生・人間活動景観に重要であり、暖温帯の副次木本として残す。ただしVanilla説明の「not beautiful」という価値判断はAMJEでは不採用とし、説明書き換えを必須とする。種同定・外観が日本向け表現として不足する場合は後続でAMJE所有化を再検討する。

### Oak

**冷温帯のみ残す:** `Plant_TreeOak`

冷温帯ではナラ類を含む落葉広葉樹林・二次林の汎用代理として成立する。暖温帯では照葉樹構造をSudajiiへ寄せるため除外済み。

### Birch

**冷温帯・亜高山帯で残す:** `Plant_TreeBirch`

冷温帯のカバノキ類および亜高山帯のダケカンバ系副次高木の汎用代理として残す。高山帯では森林限界を越える通常高木になるため除外済み。

### Pine

**冷温帯のみ残す:** `Plant_TreePine`

アカマツ系の二次林・貧栄養立地の針葉樹を表す汎用代理として冷温帯で残す。亜高山帯ではShirabisoと役割が競合し、高山帯では通常高木が不適切なため除外済み。

## 通常植林候補との一体監査

通常樹木の地域分布と植林候補は一体で監査する。残存／除外判定は自然生成だけでなく、そのAMJE Biomeの樹木栽培研究後の候補にも適用する。正本の候補表は `Docs/Design.md` の Regional tree sowing contract、静的・ロード後回帰手順は `Docs/GoldenPaths/PlantSowingTests.md` を参照。ハイマツは植林候補に含めない。

## Step 2 既存Vanilla植物監査（2026-10-08）

現行6Biomeから樹木6種・汎用7種を確認し、`Docs/VanillaPlantStep2DescriptionReview-ja.md` に配置と日本語説明文を整理した。**竹を含む既存樹木6種の日本語説明は2026-10-08に作者承認済み**であり、英語Patch・日本語DefInjected・ロード済みQuickstartを6種へ拡張した。竹の旧ランタイムPASSと、今回追加した5種の実機ゲート作者報告PASS（PR #30、squash merge `bfb27420428dc75a060a317b3c22bdd4c5856fa8`）を区別する。今回の実行ログ・個別JSONの独立確認と4構成統合試験は未完了。湿地Step 1の作者報告PASSはStep 2着手の根拠であり、4構成の統合テストや実機ログ独立監査の完了を意味しない。画像制作は引き続き保留する。

## 次工程 — 説明文監査 → リテクスチャ

残存判定が済んだVanilla樹木・植物は、**まず日本語説明文をAMJE形式へ監査・修正し、内容承認後にリテクスチャする。** 特にBambooの文化的に不適切なVanilla説明はこの工程で修正する。説明文監査を飛ばして先にアートだけ差し替えない。

## 後続工程 — 不足する自然植生

Environmentは自然景観の不足植生を担当する。薬用植物・薬材・薬は独立薬系Modへ分離し、研究・承認文は[Project薬系企画](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/MedicinalPlantsAndRemedies.md)へ移管。

## Wild Healroot維持（2026-10-08最新作者決定）

野生・栽培Healrootおよび既存MedicineHerbal供給を維持する。旧最終撤去工程と代替薬草の供給ゲートは撤回。薬系Modを入れなくてもEnvironmentの医療供給を成立させる。
