# 湿地植物の採取・選択Def 技術監査（2026-10-09）

**対象:** 作者承認済みのスゲとミズゴケの**Def挙動だけ**。配分・各種収穫量・日本語本文・画像・レシピの作者承認とは別。
**状態:** 静的ソース監査＋実装前自動テスト準備。**PlantDef未実装・実ロード／操作テスト未実施**。

## MO 1.6.2.2の固定実ソースによる確認

作者提供 `3219596926.zip` のSHA256：`6ed379d7c400db43b3af2a9a7fd6203f7f7e99ca1a670d36b0f73fd1adccc9e3`。
`1.6/` XMLを実際に展開・解析し、XPath、`ProcessDef`、条件設定を確認した。ゲームを起動してロード済みDefを調べたものではない。

- `Patches/Core/Add_HarvestYield_Grass.xml`: `Plant_Grass`に`harvestedThingDef=Hay`／`harvestYield=1`、`Plant_TallGrass`に`Hay`／`2`、`Plant_Bush`に`WoodLog`／`2`を追加する。**AMJ_Plant_Sugeにはこのパッチは命中しない。** よってスゲ自身のPlantDefからVanilla`Hay`を採る設定が必要。MOの草の収量1/2をスゲの承認済み収量と見なさない。
- `Defs/ProcessorDef/ProcessDef_DryingRacks.xml`の`DankPyon_StrawProcess`: `ingredientFilter/thingDefs/li=Hay`、`thingDef=DankPyon_Straw`、`processDays=2.5`、`efficiency=1.0`。スゲはHayを落とせば既存経路の**投入素材条件に一致する見込み**。乾燥棚の実作業・温度／環境制約・必要設備・ロード後処理は未検証。
- `Patches/ToggleOptions/MOSetting_WoodChain.xml`: `woodChain`有効時に`Defs/ThingDef/plant/harvestedThingDef[text()="WoodLog"]`を`DankPyon_RawWood`へ置換するPatchOperationが存在。ただしAMJEとMOのパッチ適用順やトグル状態は未確認。ハンノキへの最終効果も未試験。

## 2026-10-12 再監査：本番Defで確認するMO接続条件

作者提供のMO 1.6 XMLを再読し、**新たな実験用Defや追加レシピを作る必要はない**ことを確認した。下記は実装経路の静的確認であり、ロード後の成功判定ではない。

- スゲ：`Patches/Core/Add_HarvestYield_Grass.xml` は `Plant_Grass`、`Plant_TallGrass`、`Plant_Bush` だけを対象とする。スゲの `AMJ_Plant_Suge` には命中しないため、採用済み `Hay` 出力はスゲ自身の本番Defに定義する。収量は未承認のため、ここでは決定しない。MO側 `DankPyon_StrawProcess` の `ingredientFilter/thingDefs/li` は `Hay` で、スゲ専用の加工Patchは追加しない。
- ハンノキ：`Patches/ToggleOptions/MOSetting_WoodChain.xml` の `woodChain` 有効側は `Defs/ThingDef/plant/harvestedThingDef[text()="WoodLog"]` を `DankPyon_RawWood` に置換する。XPathは**明示的なplant子要素**が一致する場合の操作であり、継承された値が同じタイミングで見えるか、AMJE新規Defが当該MO Patchより先に処理されるか、このファイルだけからは保証できない。ハンノキの採取物自体は未承認。後の本番Def検証では WoodChain ON/OFF のロード後 `harvestedThingDef` と実際のCutPlant出力を区別して検査し、不一致を見てから最小限の条件付き互換Patchを検討する。先回りした無条件 `DankPyon_*` 参照は追加しない。
- ミズゴケ：MO専用の原料や収穫変換は不要。既存の承認済み「選択・CutPlant除去・ドロップなし」を本番Defの実ロードとJobで確認する。

この再監査で未承認の収量・出現率・植物画像・生育日数を確定しない。追加の使い捨て自然発生計測を再開せず、本番Defでの4構成実動作検証へ接続する。

## スゲ — PlantDef実装形状の確定部分

- DefNameは`AMJ_Plant_Suge`、ベース候補は`ParentName=PlantBase`（**継承後の実ロード確認が必要**）。
- 選択可能。野生植物としての基本表示と`harvestedThingDef=Hay`を基本構成に記載。**Vanilla既存の収穫対象植物（Haygrass等）が持つ`harvestTag=Standard`を候補Defに明示し、通常の収穫Jobで有効かロード済みDef／実機で確認する。** MOが存在しなくても`DankPyon_*`参照なしでロードする。
- `harvestYield`は**必須となるが数値は未承認**。自動テストの仮値`1`は例示であり採用値ではない。
- 野生の新植物を自動で栽培可能にしない：`sowTags`や農業研究設定を無断追加せず、最終の継承後`sowTags`・栽培メニューを確認する。
- MO時もスゲから`DankPyon_Straw`を直接落とさず、`Hay`一種類だけを採取する。MO乾燥棚の加工工程と二重採取を混同しない。

## ミズゴケ — 選択・除去のみの確定部分

- DefNameは`AMJ_Plant_Mizugoke`、ベース候補は`ParentName=PlantBase`（実ロードで親Defの継承値を確認する）。
- `selectable=true`を明示して選択・情報確認を可能にする。
- `plant/harvestedThingDef`、`harvestYield`、`harvestTag`など**採取物に関する項目を追加しない**。「`harvestYield=0`」だけでは収穫コマンド非表示の保証に使えない。
- `sowTags`・研究による播種拡張をしない。切れない不壊オブジェクトにしない。通常の`CutPlant`作業による除去を想定するが、`CutPlant`で実際にドロップがゼロかは実動作で確認する。
- `harvestedThingDef`などが親Defや他Modによって導入されないか、ロード済みDefを確認する。

## 自動テストと受け入れ判定

`Tests/test_wetland_material_plant_contract.py` は、**実際の収穫機能やUIの成功を証明しない**静的形状の回帰テスト。

- **スゲ:** Vanilla`Hay`の取得先、正の`harvestYield`が将来Defに必要、**`harvestTag=Standard`明示**、MO専用アイテムへの置換や新規直接副産物なしを検査する。タグを付けただけで収穫可能と主張せず、本番の収穫Jobを実機テストする。
- **ミズゴケ:** 選択可能、収穫先なし、播種設定なし、除去を妨げる明示的設定なしを検査する。
- 未実装中は、モックDefに対する正例と誤設定の変更テストを実行する。**実PlantDefの検査は存在しない間SKIP**と明示する。新Defが追加されれば、同じ検査がそのDefに対して働く。基本画像・本文・配分や既存セーブのゲートはこれとは別。
- 実ロード後は**Vanilla / MO / CCTO / MO+CCTO**で、成熟前後の選択／命令（Harvest/Cut）／切り倒し後のドロップ／栽培リスト、MO乾燥棚でのStraw生成、既存セーブの読み戻しを実地でチェック。数値・画像・日本語説明が作者承認されるまで、本番Defに着手しない。

**近年のVanilla XMLと古い逆コンパイル参考資料の注意:** 公開ミラーのPlant XML／decompileは1.6最新版そのものと確認できないため、親Defの最終継承、収穫UI、CutPlantのドロップを現時点で検証済みとしない。MOについてだけ作者提供の固定1.6アーカイブ実XMLを証拠とする。
