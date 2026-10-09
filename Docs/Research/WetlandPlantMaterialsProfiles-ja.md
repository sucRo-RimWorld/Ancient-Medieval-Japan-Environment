# Step 3 湿地4種の一次採取と素材利用（3プロファイル設計）

**日付:** 2026-10-09
**Owner:** AMJ Environment（自然植物・自然発生・一次採取）。加工品・装備・建物は利用Modが所有。
**状態:** 作者指示の3プロファイル（Vanilla / MO / 他AMJ）を踏まえた**実装前設計案**。下記の抽象素材・収量は作者が未承認。新PlantDef、採取ThingDef、Recipe、ゲーム内画像、Steamは変更していない。

## 前提と改訂対象

- **4種ともEnvironment自体が日本の自然植生として提供**する。他の植生ModやOdysseyを前提にせず、他Modの同等植物によってAMJEの4種を省略しない。
- 従来のStep 3にあった「植物だけ／新採取物は本工程に入れない」は、**一次採取の有用性を3構成で再検討する**という2026-10-09の作者指示に限って見直す。一次採取の採用・収量確定は別であり、精製・編組・屋根・医薬・農業レシピまでEnvironmentに移管しない。
- 生息環境・景観・持続的な木材供給を優先。自然発生の草本を刈るだけで安価な無限建材・食料にしない。未熟植物／成熟植物のHarvestとCutPlantの実挙動、季節・雪・植生再生をテストする。
- Vanillaの`Hay`（干し草）をヨシ／スゲの茎葉の**便宜的なゲーム素材**に流用する案は、実物が牧草や稲藁と同一という意味ではない。*家畜飼料として消費される点は歴史／バランス上のリスク*であり、最終採用前に検証する。出典なしに「中世日本で一般にヨシを飼料にした」とは主張しない。

## ① Vanilla + Environment（最小依存・新規アイテム抑制）

| Environmentの植物 | 一次採取候補 | 暫定値（採用前のテスト基準） | ゲーム内の用途と判断 |
|---|---|---|---|
| `AMJ_Plant_Yoshi`（ヨシ） | Vanilla `Hay` | 成熟株あたり **2** | ゲーム内の既存の干し草として使用可能。ただし葦・茅を厳密に表す専用資材ではなく家畜飼料化の副作用あり。採取済み藁を新ThingDefとして増殖させない。 |
| `AMJ_Plant_Suge`（スゲ） | Vanilla `Hay` | 成熟株あたり **1** | 収穫量をヨシより少なくする。すげ笠・蓑の素材としての識別情報はHayでは残らない。こちらも飼料化が不自然なら採用しない。 |
| `AMJ_Tree_Hannoki`（ハンノキ） | Vanilla `WoodLog` | 成熟木あたり **24** | 既存木材・薪・工作に利用。別の「ハンノキ材」ThingDefや樹皮・染料は追加しない。体格・植林・伐採労力と木材バランスを実測する。 |
| `AMJ_Plant_Mizugoke`（ミズゴケ） | **一次採取なし** | **0** | 泥炭地の景観・植生として維持。古代・中世日本での広い実用を裏付ける史料がない段階で、薬・肥料・燃料・布・吸収材を発明しない。 |

**採取ルール:** 作物としての手動播種を草本3種へ無条件解放しない。採取対象の成熟条件・WorkGiver実挙動・失敗時の収量／破壊を確認。自然再生を単純な農業・家畜給餌・薪供給の抜け道にしない。各植物の`harvestedThingDef`／`harvestYield`は正式な画像・日本語本文・採用値が揃ってから、新PlantDefと同じ実装PRで検証する。

## ② Medieval Overhaul + Environment（任意互換）

**ソースの固定:** 作者提供 `3219596926.zip` の `1.6/` XML を静的確認。MOなし基盤へ `DankPyon_*` を無条件参照しない。

| 採取元 | MOの既存経路 | 禁止・未検証事項 |
|---|---|---|
| ヨシ／スゲ → Vanilla `Hay` | MO `Patches/Core/Add_HarvestYield_Grass.xml` が既存の `Plant_Grass` にHay 1、`Plant_TallGrass` にHay 2を付与している。MO `Defs/ProcessorDef/ProcessDef_DryingRacks.xml` の `DankPyon_StrawProcess` が**Hay→`DankPyon_Straw`**を処理する（乾燥棚、2.5日、表示された定義では効率1.0）。`DankPyon_Straw` は `Defs/ThingDefs_Items/Items_Resource_Ingredients.xml` 定義のMO既存資材。 | ヨシ・スゲから`DankPyon_Straw`を**直接追加ドロップしない**（Hay→乾燥処理と二重化）。ヨシ／スゲの「茎葉」と「穀藁」は現実には違う。MO内部の汎用変換を利用する機能上の抽象化としてのみ扱い、専用茅葺き技術と誤表示しない。MO乾燥棚での実処理は未試験。 |
| ハンノキ → Vanilla `WoodLog` | `Patches/ToggleOptions/MOSetting_WoodChain.xml` の条件設定が有効なら、植物の`harvestedThingDef=WoodLog`を`DankPyon_RawWood`へ**広域置換**する。MO内の原木加工へ接続できる可能性がある。 | **MO導入だけで常にRawWoodになると主張しない**。木材チェーン設定のON/OFF、AMJEのDef読込・パッチ順、伐採後実ドロップを検証する。既存MOの資材をAMJEで重複定義しない。 |
| ミズゴケ | 対応する汎用加工を新設しない | 既存MOに対応素材・Recipeがあると確認できていないため、「MO時は医療用水苔を採取」等を作らない。 |

MOのStrawはDryingRack `Hay` 入力経路がある一方、[Grains設計のStraw規則](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/blob/main/Docs/Design.md)では**穀物脱穀時のMO Straw副産物だけ**をGrainsが所有する。Environmentでの野草Hay収穫／MO既存乾燥加工はそれと別経路であり、Grainsの脱穀Recipeを変更しない。MO×Grains併用時はMO資材の二重生産源・需給を別検証する。

## ③ 他のAMJとの連携（将来Modは全て任意・所有Modが消費側を実装）

| 連携するAMJ役割 | 対象素材・用途 | 責任境界 |
|---|---|---|
| 将来の **Architecture / Building Materials** | ヨシの茎：茅・草葺き屋根材。スゲ：菅笠に限らず敷物／蓑等の草材。 | 茅・菅素材を利用する建築部材、屋根、仕上げRecipeは将来の建築／生活素材Mod所有。Environmentは自然植物と採取資源の必要部分だけ所有。未実装ModをEnvironmentの成立条件にしない。 |
| 将来の **Fiber / Living Goods** | スゲ葉を笠／蓑へ、必要ならヨシも敷物や編組へ利用。 | 「菅」由来を要求するRecipeに、どの草からでも出る`Hay`を無条件投入可能にしない。将来、**出所が必要なら別の原料ThingDef／条件付き追加採取の所有者と供給経路を先に合意**する。既存セーブ・二重ドロップ・独立導入時の素材確保を併せて設計。Environmentに織機・縫製台・帽子完成品を追加しない。 |
| **Grains（既存）／Rice Cultivation（将来）** | 穀物由来の稲藁・麦藁と、ヨシ・スゲの刈草を区別。 | GrainsのVanilla時「新Straw ThingDefなし」、MO時「脱穀副産物として既存`DankPyon_Straw`を使用」を変更しない。野草・葦採取で穀物の副産物を自動生成しない。Rice側の稲藁生産も別所有。 |
| **Waterworks** | 湿地・河川の自然植生との共存。 | 採取物で用水路工事を前提化しない。通常の植生除去で採取物が出るか、工事時の`CutPlant`が素材を二重生産しないかだけ将来の任意互換検査。 |
| **Medicine** | ミズゴケを医薬材料にするかは保留。 | 古代～中世日本での用途と医療ゲームループの裏付けが必要。現時点で`MedicineHerbal`やHealrootへの変換なし。 |
| **MO Japanization** | MO資材を使う実際の日本化研究・設備が確定した場合に接続。 | MOの乾燥棚・原木経路を日本の茅葺き・菅笠に自動読み替えない。設備・研究改造はJapanizationまたは用途所有Modの責務。 |

**重要な将来課題：** `Hay`のスタックに植物の出所は保存されない。スゲ専用の菅笠・ヨシ専用の葦葺きを正確に表す必要が生じたら、`Hay`消費Recipeを追加するだけでは不足する。二重採取を避けた原料ルート（新しい専用原料、植物固有のJobや製品化経路、既存データ移行）を別途正式設計する。それまでは「将来AMJで笠や茅葺きが製造できる」とは表記しない。

## 実装・検証条件

1. **史実と利用目的:** ヨシの茅葺きは[文化庁「ふるさと文化財の森」](https://www.bunka.go.jp/koho_hodo_oshirase/hodohappyo/94187001.html)で裏付け。スゲの古代の笠・蓑への利用は[國學院大學『万葉神事語辞典』](https://jmapps.ne.jp/kokugakuin/det.html?data_id=32059)で裏付ける。[文化庁の越中福岡の菅笠](https://kunishitei.bunka.go.jp/heritage/detail/302/00000855)は近世に発達した地域技術を含み、AMJで江戸期そのものの専用産業を持ち込まない。古代・中世の**湿地植物採取量**の史実数値は未確定。ミズゴケの中世用途は未確認。
2. **既存Mod先行監査の拡張:** 既存の4種植生の判定JSON（`Docs/Research/WetlandPlantImplementationDecision.json`）は**野生PlantDefの必要性**を許可する記録であり、採取・製造チェーンを無条件に許可するものではない。新資材・新レシピが必要になった場合はProjectのVE／非VE既存Mod調査と独立した必要性判定を追加する。
3. **Vanilla検証:** ヨシ／スゲの収穫・刈り取りで`Hay`がどの成長段階に出るか、現存Haygrassとの収量対比、飼料価値／野草無限生産／自動採取Jobを実測する。ハンノキは伐採後`WoodLog`実ドロップを確認する。ミズゴケから副産物が出ないことを確認する。
4. **MO検証:** MO木材チェーンON/OFF、`Hay→DankPyon_Straw`乾燥棚、MO×GrainsでのStraw需給、MO提供の草／木の収量変化を独立プロファイルで確認。XMLの存在だけで実際の採取・加工成功としない。
5. **他AMJ:** 加工側Modが未作成の段階で仮Recipeや仮`ThingDef`をEnvironment本体へ出荷しない。将来の素材の出所識別・独立導入・二重ドロップ・移行セーブは、その機能の所有Modと公式任意互換パッチで確定する。
6. **既存の承認ゲート:** 4種の日本語説明・全必要状態の画像は未承認。採取や利用法の本文追記・英訳・画像制作・採用数値のXML反映は別途承認とテストを経て実施する。野生・栽培Healroot／`MedicineHerbal`を維持し、現行湿地の植生commonalityと木本配分を無断変更しない。

**判定状態:** 3プロファイル設計は提出済み。`Hay`の流用・2/1/24という採取収量、他AMJの具体的な加工内容は**まだ未採用**。新規のHarvestやXML/Def/Textureの実装PASS、MO/AMJ互換PASS、作者の日本語説明承認を主張しない。
