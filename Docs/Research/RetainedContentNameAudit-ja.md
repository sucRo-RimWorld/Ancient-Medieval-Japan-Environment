# 残存Vanilla／Medieval Overhaul環境コンテンツの表示名監査（2026-10-10）

**状態：実ソースとの対応監査済み。新規表示名の追加採用・XML変更なし。**  
名称変更の共通原則は [Docs/Design.md](../Design.md) §11.5.5。既に改称したVanilla樹木6種は [VanillaPlantStep2DescriptionReview-ja.md](../VanillaPlantStep2DescriptionReview-ja.md)、Vanilla湿地2種は [VanillaWetlandBiomeAudit-ja.md](../VanillaWetlandBiomeAudit-ja.md) が正本。この記録は未解決候補の**改称可否**と実ソース照合の補助監査であり、重複する別の最終仕様ではない。

## 1. 判定基準と影響範囲

- **実在の種／地域／形態と対応する表示名だけ**を採用する。「日本に似た役割の動物がいる」だけでは `Bear_Grizzly` をツキノワグマ、`Wolf_Timber` をニホンオオカミと呼ばない。
- 既存`ThingDef`、`PawnKindDef`等の表示名PatchはAMJEが生成したBiome内だけでなく**そのDefを参照する他のBiome・シナリオ・Mod**にも波及する。局所的な出現用途とグローバルな命名変更は区別する。
- 汎用Plantを特定種へ見せかける改称、実際の外観を伴わない動物の種別偽装、独立Modの責務をEnvironmentへ取り込む変更は禁止。改称のためだけに新Defを複製しない。
- Environmentが採用する自然植生・地形の名称と説明が対象。MOの設備・研究・作物・食材の一括名称変更は他のownerと競合しないよう別判断。MO固有のファンタジー樹種を実在日本種へ勝手に同定しない。
- 表示名の承認・静的CI PASS・**ロード済みゲームでの日英表示テスト**は別の証拠。今回の監査で新しいnative test PASSは主張しない。

## 2. 既存のgeneric植物7種 — 種への読み替えなし

Environment正本 `Defs/BiomeDefs/AMJ_Biomes.xml` と `Patches/VanillaWetlandVegetation.xml` に残る、特定種へ同定していない植物群。

| DefName | 現行の役割 | 今回の名称判定 |
| --- | --- | --- |
| `Plant_Grass` | 汎用の草本 | 特定の日本の草に改称しない |
| `Plant_TallGrass` | 背の高い汎用草本 | アシ・スゲ・ススキと同一視しない。これらは別のEnvironment種候補 |
| `Plant_Brambles` | 汎用の茂み | 日本固有のイバラ／特定属と断定しない |
| `Plant_Bush` | 汎用低木 | 特定の日本種と断定しない |
| `Plant_Moss` | 汎用コケ類 | 追加予定のミズゴケとは別。ミズゴケへ改称しない |
| `Plant_Berry` | Vanillaのgeneric果実低木とベリー供給 | 日本の特定果実低木に同定しない。独立Wild Food Foragingとの資源所有も維持 |
| `Plant_HealrootWild` | 維持が確定したVanilla薬草供給 | 日本の固有薬草へ改称しない。薬系Modの分離方針・MedicineHerbal供給を維持 |

**結論：新しいspecies-specific表示名Patchは7種とも不要。** これはVanillaの日本語汎用名を削除するという決定ではなく、追加の不確かな名称上書きを行わないという判断。実際の日本語UI訳の字句を全部承認したことも意味しない。既存の汎用訳に明白な誤訳が後日発見された場合は、種への同定ではない狭い用語修正を別途検討する。

## 3. AMJEの野生動物9 Def — functional proxyと生物種は別

現在の `Defs/BiomeDefs/AMJ_Biomes.xml` と `Patches/VanillaWetlandEcology.xml` の `wildAnimals` の固有キーを照合。`Docs/Design.md` §14の**機能的なVanilla代理**という公式境界を維持する。

| 採用PawnKindDef | 現行の用途・制約 | 改称判定 |
| --- | --- | --- |
| `Hare`, `Snowhare` | ウサギ類の機能的代理 | 特定日本種への改称は保留 |
| `Squirrel`, `Rat` | 汎用小型哺乳類代理 | 種レベル改称なし |
| `Deer` | 日本のシカの生態ニッチ | `ニホンジカ`への直接改称は外観・種定義・他Biomeへの影響が未確認 |
| `WildBoar` | 日本のイノシシ相当の機能的代理 | Vanillaの種名を一括再定義しない |
| `Fox_Red` | キツネ相当の機能的代理 | Vanilla種／亜種の相違を隠す固有名への改称は保留 |
| `Wolf_Timber` | オオカミ類の機能的代理 | **ニホンオオカミと同定しない** |
| `Bear_Grizzly` | 大型クマの機能的代理 | **ツキノワグマ／ヒグマと同定しない**。外観・生物種の不一致が特に大きい |

**結論：今回の動物ラベルPatchはゼロ。** 既存Vanillaの種・遺伝・肉・革・外観・全世界出現などを一緒に監査しないまま、日本動物の種名を表示だけで差し替えない。Environmentは「日本産動物コンテンツMod」ではないという既存§14を変更しない。日本産固有動物の実装やリテクスチャが必要になった場合は所有Mod・既存Mod比較を含む別企画。

## 4. Medieval Overhaul 1.6実ソース — MO樹木の固有名は維持

**一次データ：** この会話に提供されたMO Workshop `3219596926.zip`、archive SHA256 `6ed379d7c400db43b3af2a9a7fd6203f7f7e99ca1a670d36b0f73fd1adccc9e3`。解凍して再配布せず、以下の**内部XMLテキスト**を確認。archiveの`LoadFolders.xml`では`v1.6`の`1.6`階層が指定される。これはこの提供ファイルの調査であり、プレイヤーが現在ロードしている最新版や最終loaded Defの保証ではない。

| MO 1.6内のソース | 確認結果・名称判断 |
| --- | --- |
| `1.6/Defs/ThingDefs_Plants/Plants_Wild_DarkForest.xml` | `DankPyon_GreatOak` = great oak、`DankPyon_GreatIter` = great Iter、`DankPyon_GreatFir` = great fir、`DankPyon_GreatWillow` = great Willow。固有／幻想的な樹種として**一律にナラ・シラビソ・ヤナギ等へ改称しない**。 |
| `1.6/Defs/Biomes/Biomes_DarkForest.xml` | `DankPyon_DarkForest` (Dark Forest)の自然植物に`DankPyon_GreatOak`、`DankPyon_GreatFir`とVanillaの`Plant_TreeWillow`・`Plant_TreeOak`・`Plant_TreeMaple`・`Plant_TreePoplar`が同居。**GreatIter／GreatWillowのPlantDef存在＝DarkForestでの自然出現とは限らない**。 |
| `1.6/Defs/ThingDefs_Plants/Plants_Cultivated_Farm.xml` | MO果樹`DankPyon_Tree_Apple`、`DankPyon_Tree_Lemon`、`DankPyon_Tree_Mulberry`、`DankPyon_Tree_GriffonBerry`を確認。自動的に中世日本の自然植生と判定せず、農業・食材の所有ModおよびWild Food Foragingとの重複を別監査。 |
| `1.6/Patches/Core/Add_Plants_To_Biomes.xml` | MO固有野生薬草（Mindwort/Poppy/Fleawort/FlyAgaric）をVanilla`TemperateSwamp`・`ColdBog`等へ追加するPatchあり。**AMJEの湿地wildPlants全置換Patchとの相互作用はロード順に依存し得る**。名称の問題ではなく別のMO互換実機テスト課題としてowner Coordinationに引き継ぐ。 |
| `Patches/Compatibility/MedievalOverhaul.xml` (Environment本体) | MO DarkForestで現在置換するのは**地表肥沃度地形の階層だけ**。MO樹木Defのラベル・種名をPatchしている証拠はない。 |

Environment `Docs/ArtDirection.md` の四大MO樹木対象は**将来の条件付き画風調整**であり、日本種への置換計画ではない。既存の画像延期ルールも維持する。MO DarkForestの`Plant_TreePoplar`はEnvironmentの4基準Biomeに採用しないが、MO上の存在をもってEnvironmentの新規種名同定に含めない。

## 5. 次のゲート／他ownerとの境界

1. **表示名についての今回の追加実装は不要**：6 Vanilla樹木と2 Vanilla湿地Biomeの既存改称のみ維持。ユーザー承認済み本文・植生分布・森林・気候の設計を再変更しない。
2. **MO湿地野生薬草の実機互換**：MOの野生薬草追加PatchとAMJEの湿地wildPlants全置換のロード後結果を、将来のEnvironment+MOプロファイルで検証する。名称問題として扱わず、別の互換課題として記録。源Mod側の草Defを勝手に二重作成しない。
3. **ロード済み名称**：既存6樹木・湿地2種の英日表示を、必要なVanilla/MO/CCTO/MO+CCTOプロファイルで検証する。static/CIだけでゲーム内テスト済みとしない。広範囲の別名追加テストは作らない。
4. **新しい種・獣**：古代～中世日本に適した新規個別種が必要になった場合だけ、実在・分布・史料／VEと非VEの既存Mod、元Defの外観・収穫・互換性、owner責務を比較して設計する。
