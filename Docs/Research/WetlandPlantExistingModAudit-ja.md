# Step 3 湿地植生 — 既存Mod重複監査（2026-10-09）

**所有:** AMJ Environment。対象：ヨシ・スゲ・ハンノキ・ミズゴケの4候補。薬草・加工薬・山野採集は対象外。
**状態:** 1.6のVE／非VE／MO実XMLとOdyssey公開資料を比較済み。**作者確定方針（2026-10-09）：4種ともAMJEの実装対象、既存Modは任意互換候補のみ。** 新PlantDefは未実装。正式な判定JSON（`Docs/Research/WetlandPlantImplementationDecision.json`）は記録済みで、証拠SHA256と4環境種の構造照合を実施済み。Project指定のPython CLIは2026-10-09の版固定資料に対してPASS済み。日本語説明4件は2026-10-10に作者承認済み。画像・本番PlantDef・配分・実機互換は未完了。以下の2026-10-12追補を含む資料の再実行結果は別途区別する。

## 目的と判断基準

**既存Mod監査の役割（2026-10-09作者確定）：** Environment単体で中世日本の自然環境を成立させるため、同等種が既存Modに存在してもAMJE自身の必要な植生実装を省略しない。他Modは任意併用時の二重出現、分布、Patchロード順、依存・ライセンスを検討する**互換候補だけ**とする。外部Defsや画像を無断転載せず、自身の植生として実装する。Projectの形式的な既存Mod比較・判断記録、日本語本文・画像承認、実機検証の各ゲートは維持する。

Odyssey DLCなしの基本構成で、古代～中世日本の河岸・低層湿原・冷涼湿原の自然植生を表す。両湿地の`wildPlants`は、現在は`Patches/VanillaWetlandVegetation.xml`で**列挙されたVanilla由来の項目のみ条件付き削除＋基準値再追加**している（2026-10-10修正）。全ノード置換は廃止済み。総commonality 7.30／8.22、木本3.00／1.80は**AMJE管理のVanilla基準項目のみ**の値であり、MO等が追加した植物を含む最終合計を固定しない。新種はTallGrass／Moss／Willow等の代理から配分を移し、総量と森林構造を維持する。新採取アイテム・Job・資材加工・医療機能は追加しない。

検索語：RimWorld 1.6 wetland reeds/sedge/sphagnum/alder/bog moss, swamp plants, ヨシ・スゲ・ハンノキ・ミズゴケ、Vanilla Plants Expanded、ReGrowth 2、Biomes! Prehistoric。公開説明と実ソースを区別する。調査時点のコードを下記コミットに固定する。

## Vanilla・MO・VE・非VE比較

| 候補 | 調査した実ソース・重複の程度 | 判断 |
|---|---|---|
| Vanilla／Odyssey | DLCなしでは汎用TallGrass・Moss等を利用。Odysseyの[Reeds](https://rimworldwiki.com/wiki/Reeds)・[Bulrush](https://rimworldwiki.com/wiki/Bulrush)は**DLC専用**。Bulrushはスゲそのものではない。 | 通常構成の必須依存にできない。DLC有効時の生成は未検証。 |
| Medieval Overhaul | 作者提供`3219596926.zip`（SHA256 `6ed379d7c400db43b3af2a9a7fd6203f7f7e99ca1a670d36b0f73fd1adccc9e3`）の`1.6/Defs/Biomes/Biomes_DarkForest.xml`には`Plant_Reeds` 1・`Plant_Bulrush` 0.2が存在するが、**ともにOdyssey `MayRequire`条件付き**。提供MO 1.6のXML内に対象4種の独立PlantDefは見つからなかった。 | MO単独をヨシ／スゲの代替としない。 |
| VE — Vanilla Plants Expanded | [固定1.6植物Defs](https://github.com/Vanilla-Expanded/VanillaPlantsExpanded/tree/1d64cbe34a98585a29e87678dc37ea4c144f3999/1.6/Defs/ThingDefs_Plants)。栽培作物・果樹の定義を確認したが今回の野生湿地4種はない。 | 農業／果樹の拡張であり、Environment自然植生の必須依存にはしない。 |
| VE — More Plants | [固定1.6植物Defs](https://github.com/Vanilla-Expanded/VanillaPlantsExpanded-MorePlants/tree/9caf7c02c07dbf1fda2ecfcc203d71e6e67cd6f7/1.6/Defs/ThingDefs_Plants)。Watercress・Lotus・WaterChestnut等は**栽培植物**。対象4種とは別。 | 水辺の植物というだけで機能を同一視しない。 |
| 非VE — ReGrowth 2 | [固定1.6湿地Defs](https://github.com/Helixien/ReGrowth2/blob/0c6e3c4770f6d9e8acd8d6e74dc468e4f55f2aab/1.6/Defs/ThingDefs_Plants/Plants_Wild_Swamp.xml)。CreepStern／Dervish／SwampPod／SychiCap／WhiteWillowを追加。WhiteWillowはAlnusではない。[Biomeパッチ](https://github.com/Helixien/ReGrowth2/blob/0c6e3c4770f6d9e8acd8d6e74dc468e4f55f2aab/1.6/Patches/Core/Core-Biomes.xml)のReeds・BulrushはOdyssey条件付き。旧ReGrowth: Swampは非推奨。 | 湿地の先例・任意互換候補。対象4種の直接的な置換ではない。 |
| 非VE — Biomes! Prehistoric | [固定1.6植物Defs](https://github.com/biomes-team/BiomesPrehistoric/tree/5d9b69e1e8e41b49a25b1737963111d064993b4e/1.6/Defs/Plants)に**`BMT_Sedge`、`BMT_RedSphagnum`、`BMT_BogMoss`**が実在。Biomes! Coreの`Biomes_PlantControl`でMuddy／Boggy／Soil等の適性を設定。[Vanilla湿地パッチ](https://github.com/biomes-team/BiomesPrehistoric/blob/5d9b69e1e8e41b49a25b1737963111d064993b4e/1.6/Patches/BiomesPrehistoric_Vanilla_Plants.xml)はColdBogへBogMoss等を`PatchOperationAdd`で追加。恐竜と多数の先史植物を含む。[ライセンス](https://github.com/biomes-team/BiomesPrehistoric/blob/5d9b69e1e8e41b49a25b1737963111d064993b4e/LICENSE.md)はCC BY-NC-SA 4.0。 | **スゲ・ミズゴケは実装重複。** 全Modの必須化は過大。無断移植せず先行研究・任意互換として評価。 |
| 他の非VE補助 | [Cyanobot's Plant Gen Patches](https://steamcommunity.com/workshop/filedetails/discussion/3218426325/664963107066718505/)はOdyssey・Biomes!・ReGrowth等の植物追加に合わせる配分互換の先例。PlantDef自体を提供する証拠ではない。 | 任意パッチ設計の参考。 |

### 互換性上の未解決点

1. **2026-10-12追補：** 旧全ノード置換は2026-10-10に廃止済み。現行`Patches/VanillaWetlandVegetation.xml`は列挙したVanilla`Plant_*`だけを対象にするため、`BMT_*`のような**別名の追加植物は、Patch順にかかわらずこの操作では削除されない**。同名Vanilla項目はAMJEの基準値へ再設定する契約であり、Biomes!等が独自に加える項目の分だけロード後プール合計が増える。**Biomes! Prehistoric併用の実機ロード済みDef・自然発生・機能干渉は未検証**。このソース上の保持仕様だけで包括互換PASSを主張しない。MOの4薬草は各湿地0.05で追加され、この保護方式については別途、既存MO併用Quickstartsでロード値・4薬草合計から少なくとも1株の自然発生・ERRORゲートが検証済み（`Docs/GoldenPaths/RenderedRuntimeTests.md`および`Docs/Coordination.md`のENV-MO-WETLAND項目参照）。全4種が各マップで発生した証拠やBiomes!互換ではない。
2. Reeds＝ヨシ、Bulrush＝スゲ、WhiteWillow＝ハンノキとは断定しない。重複する景観・生態機能と厳密な種同定は別。
3. Biomes!固有地形タグ／ライセンス／幅広い先史コンテンツを含むModを、わずかな植物のために基本構成へ依存追加しない。XML・PNGは流用しない。
4. MedicineHerbal供給は維持する。野生・栽培Healrootは削除しない。

### 4種の確定実装範囲と任意互換

- **ヨシ:** Environmentが河岸・低層湿原の植生として実装する。OdysseyのReedsはDLC有効時の任意互換・二重出現調整候補。
- **スゲ:** Environmentが湿地草本として実装する。Biomes! Prehistoricの`BMT_Sedge`は任意互換の分布・Patch競合対象であり、AMJE独自Defを省略しない。
- **ハンノキ:** Environmentが湿地林木本として実装する。Vanilla／ReGrowthのWillow・Birchとの共存、木本総量・植林・落葉・伐採・旧セーブを確認する。
- **ミズゴケ:** Environmentが冷涼泥炭湿原の植生として実装する。Biomes! PrehistoricのSphagnum／BogMossは任意互換時の重複・分布調整対象。

**現在のゲート：** Project `Docs/Research/ExistingModAudit.md` に沿う独自実装の必要性・VE／非VE比較・正式判定JSONは記録済みで、証拠ハッシュの一致と同等構造照合を確認した（**2026-10-09版のProject指定Python CLI PASSは`Docs/ValidationEvidence/WetlandEntryProjectValidator-20261009.md`に保存**）。4種の日本語説明は2026-10-10に作者承認済み。本番PlantDefは**画像の作者承認、未決定の採取・分布条件**のためHOLD。採取収量とヨシ等の未決仕様は別途確定が必要。実マップ・4構成／既存セーブ検証は実装受け入れ時の別ゲートであり、この調査だけでPASSにはしない。Odysseyは作者未所持。Biomes!／VE／ReGrowthとの実機相互ロードは未確認。全Workshopの網羅調査をしたとは主張しない。
