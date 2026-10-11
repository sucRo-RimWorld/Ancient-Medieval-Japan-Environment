# Step 3 湿地4種の地形適性・野生発生モデル調査（未承認）

**状態：検討資料／ゲームへの適用なし。** 2026-10-11。所有：AMJ Environment。
正式な植生・資材・CCTOの承認範囲は `Docs/NativeVegetationStep3Design-ja.md` §3.1–3.3.1 および `Docs/Research/WetlandPlantMaterialsProfiles-ja.md` を優先する。配分数値は引き続き `Docs/Research/WetlandPlantDistributionDraft.json` の `proposal_not_approved` のまま。本記録は許可・採用・自然発生PASSではない。

## 現行ゲームの観測（作者提供ログ抜粋）

- 開発専用 `AMJE_Test_Yoshi/Suge/Hannoki/Mizugoke` は全て `fertilityMin=0.05`、`completelyIgnoreFertility=False`、`WildTerrainTagsCount=0`。いずれも本番PlantDefではない。検証APIは `CanEverPlantAt`、`checkMapTemperature=false`。
- 温帯湿地・冷涼湿原では全4種とも、Mud・WaterShallow、冷涼湿原のMarshで採用0/96。冷涼湿原の2026-10-11 08:24ログはこれらの**全拒否96/96を肥沃度0.00に帰属**した。MarshyTerrainは96/96、SoilRichは冷涼湿原81/81、Soilは冷涼湿原95/96（残る1件の理由未判定）。温帯湿地にMarshはなく、両マップにAMJ_ThinSoilもない。
- 以前の基礎分布実測は温帯湿地62,500セル中MarshyTerrain 13,336、冷涼湿原10,886。これらの地形はサンプル肥沃度1.00であり、**湿原の貧栄養泥炭面と等価ではない**。植物ごとの実生定着・自然発生・広がりや複数シードの分布は未測定。
- 根拠となる行番号・サンプル件数・重み保持の監査は `WetlandPlantDistributionDraft.json` の `generated_map_observations`、`terrain_eligibility_observations`、`terrain_rejection_diagnostics` に保存。提示ログは全体 `run-tests.bat` PASSの代用にはしない。

## 生態資料からの区別

- 環境省の湿原分類は、流水・地下水の影響が強い低層湿原、スゲ類が見られる中間湿原、降水依存・貧栄養でミズゴケ類が主になる高層湿原を区別する。ひとつの `ColdBog` と単一 `MarshyTerrain` はこのすべてを意味しない。
  - https://www.biodic.go.jp/reports/2-3/a040.html
  - https://kushirodata-center.env.go.jp/wetland/wetland_article1_2.html
- ヨシ・スゲ類は低層～中間湿原の植物群落候補。スゲは多様な種を含むため「一律に深水面で育つ」という扱いはできない。
  - https://www.biodic.go.jp/reports/2-3/a040.html
- ハンノキは湿地・沼沢地の湿生林を形成し、地下水位の高い場所に自生する。ただし水面一面を高木で覆うモデルは不要。
  - https://www.ktr.mlit.go.jp/arajo/arajo00047.html
  - https://www1.ous.ac.jp/garden/hada/plantsdic/angiospermae/dicotyledoneae/choripetalae/betulaceae/hannoki/hannoki.htm
- ミズゴケは冷涼・酸性・貧栄養の泥炭湿原に関係する。地表の浅い水域すべてをミズゴケへ置換するのは不適切。
  - https://www.biodic.go.jp/moni1000/findings/reports/pdf/FY2009-2017_Inland_Waters_Survey.pdf

## 植物別の**非採用候補**（数値未設定）

| 植物 | 最初に検討する生育場 | 避ける表現と留保 |
|---|---|---|
| ヨシ（葦） | 温帯湿地の水際／低層湿原、ぬかるみや岸辺。冷涼湿原も低層型なら候補 | 単なる一般土壌全域、深い開放水域への一律出現は不可。浅水への進出は別実験 |
| スゲ（菅） | 低層～中間湿原の湿った草地・周縁。両湿地の候補 | すべてのスゲを深水草本と同一視しない |
| ハンノキ（榛の木） | 湿地林・河岸の比較的安定した地表。ぬかるみと岸辺を中心に木本重みの置換候補 | 浅水／泥面に全域伐採木が立つ、既存Willow/Birch/Mapleを全撤去する、などを避ける |
| ミズゴケ（水苔） | `ColdBog`の低木密度・水位・貧栄養環境を勘案した限定区域 | 現行`MarshyTerrain`肥沃度1.00が高層湿原の泥炭面を表すと断定しない。TemperateSwamp追加は現行草案では対象外 |

これらは生態に基づく比較対象であり、`fertilityMin`、`WildTerrainTags`、`terrainBlacklist`、`completelyIgnoreFertility`、`wildPlantUseDistanceToShore` のいずれも本番値として承認しない。バイオーム全体の `wildPlants` 重みは河岸・水縁・低層/高層湿原の地形区分を直接表現しない。

## 次に検証するロード済み地形タグ（設計への先行反映は禁止）

既存の湿地Quickstartsへ、実際にロードされた地形Defの
`tags`、`IsWater`、`fertility` を一マップ一行で報告する処理を追加した。
`[AMJ Environment Wetland Loaded Terrain Contract]` で検索できる。
この記録は温帯湿地と冷涼湿原に関する静的地形情報の**観測**であり、
既存植物の野生配置やヨシの浅水発生を証明しない。

`WildTerrainTags` は該当地形にタグが付いて初めて使える。
ただし水辺タグが共有されるなら、ヨシだけを浅水に許可する設計は
`fertilityMin` の回避と、意図しない深水・海水・Mud等の排除を
**セットで**評価する必要がある。独自タグの追加が必要になる場合は、
既存植物や他Modの地形条件への副作用を先に調べ、承認無しに
共通地形Defを編集しない。

旧公開Core XMLの`Water`タグ等の構造は年代が古い場合があり、
1.6現行との完全同一性を仮定しない。今回のロード済みログを
優先する。既に分かっている冷涼湿原の肥沃度0拒否を繰り返し
確認するだけの別テストは追加しない。

## ロード済み地形タグの実測確定事項（2026-10-11、非承認）

作者提示の `[AMJ Environment Wetland Loaded Terrain Contract]` ログ2件より、実際の両湿地マップでロードされた地形の分類が一致した。生態・自然発生への適用可否は未承認。全値・行番号・地形セル数は `Docs/Research/WetlandPlantDistributionDraft.json` の `loaded_terrain_contract_observations` に記録する。これはログ抜粋であり、最新ブランチの `run-tests.bat` 総合PASSを意味しない。

| 地形 | IsWater | fertility | 実際のタグ | 両湿地のセル存在 |
|---|---|---:|---|---|
| `Mud` | false | 0.00 | `Mud` | あり |
| `MarshyTerrain` | false | 1.00 | `Soil` | あり |
| `Marsh` | true | 0.00 | `Water`, `WaterMarshy` | ColdBogのみ |
| `WaterShallow` | true | 0.00 | `Water`, `WaterFreshShallow`, `WaterFreshShallowStill` | あり |
| `Soil` | false | 1.00 | `Soil` | あり |
| `SoilRich` | false | 1.40 | `Soil` | あり |
| `AMJ_ThinSoil` | false | 0.50 | `Soil` | 両マップとも0セル（Def自体はロード済み） |

**機構上の解釈と限界：**

- `WaterFreshShallowStill` は今回の7地形中では `WaterShallow` を `Marsh`・`Mud` から区別できるタグ。ただし世界全地形で唯一という証拠はない。浅水用候補タグがあることと、実際にヨシが定着・自然発生できることは別。
- 既存4仮Defの `fertilityMin=0.05` と `completelyIgnoreFertility=false` では肥沃度0の `WaterShallow` に生えない。タグだけ追加しても解決しない。`completelyIgnoreFertility` の無条件利用は野生植物スポーナーの密度・候補集合へ影響し得るため、本番への採用を保留する。
- `MarshyTerrain`・通常 `Soil`・`SoilRich`・`AMJ_ThinSoil` はすべて `Soil` タグ。**`WildTerrainTags=Soil` だけで湿地土壌のヨシ／スゲ／ハンノキや、貧栄養ミズゴケ地を限定することはできない。** 現状のColdBogとTemperateSwampのバイオーム単位の分布を使う簡略化案と、環境別の地形/タグを新設する精密案とは別途比較が必要。
- 特に `Marsh` の `WaterMarshy` と `WaterShallow` の `WaterFreshShallowStill` は異なる。湿原＝浅水とみなし、双方をヨシに一律許可しない。
- 新しい地形タグ／TerrainDef編集、C#制御や個体数比、種別の原料・収量・画像は**今回決めない**。既存MO・バニラ植物、ヒールルート、河川・地表生成の回帰を維持する。

## RimWorld自然発生の実装上の留保

公開されているRimWorld逆コンパイルコード（現在のインストール版1.6と同一の保証なし）では、`PlantUtility.CanEverPlantAt` に肥沃度、地形タグ、占有物等の条件があり、`WildPlantSpawner.CheckSpawnWildPlantAt` はそれに加えて候補プール、密度、周辺植物、選択重み等を判定する。
- https://github.com/Chillu1/RimWorldDecompiled/blob/master/RimWorld/PlantUtility.cs
- https://github.com/Chillu1/RimWorldDecompiled/blob/master/RimWorld/WildPlantSpawner.cs

`completelyIgnoreFertility` を用いれば一部ゼロ肥沃度での候補適性が変わりうるが、`WildPlantSpawner`側のゼロ肥沃度セル候補／密度条件にも作用する。**4種に一律ON、または水だけでなくMud/Marsh全域まで許可する無制限実装はしない。** `wildPlantUseDistanceToShore` は参照コードでは主に水面セルの岸からの距離に対応し、陸上のぬかるみ分布を自動的に水縁集中へ変えるとは限らない。

## RimWorld 1.6.9438 ソース監査：XMLのみでできる範囲（検討用）

**検証元:** `Chillu1/RimWorldDecompiled` の 2026-05-20 公開コミット
`2d508035082e7cb0c8e29e230d26bda6e546928f`（表記 `v1.6.9438.38202`）。
[PlantUtility.cs](https://github.com/Chillu1/RimWorldDecompiled/blob/2d508035082e7cb0c8e29e230d26bda6e546928f/RimWorld/PlantUtility.cs)、
[PlantProperties.cs](https://github.com/Chillu1/RimWorldDecompiled/blob/2d508035082e7cb0c8e29e230d26bda6e546928f/RimWorld/PlantProperties.cs)、
[WildPlantSpawner.cs](https://github.com/Chillu1/RimWorldDecompiled/blob/2d508035082e7cb0c8e29e230d26bda6e546928f/RimWorld/WildPlantSpawner.cs)、
[GenStep_Plants.cs](https://github.com/Chillu1/RimWorldDecompiled/blob/2d508035082e7cb0c8e29e230d26bda6e546928f/RimWorld/GenStep_Plants.cs)。
作者実機のビルド番号をこのコミットと照合した証拠はなく、実際の自然発生を通過した証拠でもない。

1. `PlantUtility.CanEverPlantAt` は通常、`fertility < fertilityMin` を拒否し、`completelyIgnoreFertility` はこの**個々の植物の肥沃度ゲート**を迂回する。`WildTerrainTags` は地形タグとの**1個以上の重複**を要求し、全タグのAND条件ではない。タグ指定だけで水上生育は解放しない。
2. `WildPlantSpawner.CheckSpawnWildPlantAt` は**植生候補全体**に肥沃度0を拒否するゲートを持ち、当該バイオームのロード済み野生植物に `completelyIgnoreFertility=true` を持つ候補がいない限り、`fertilityMin=0` だけの植物は肥沃度0セルで自然発生できない。候補植物がいると、`HaveAnyPlantsWhichIgnoreFertility` を通じ、密度計算も変わる。
3. `GetBaseDesiredPlantsCountAt` ではゼロ肥沃度の**水域**セルへ暫定基礎値`0.1`が用いられる一方、非水域のMud（肥沃度0）にはこの水域用の補正がない。したがって、ゼロ肥沃度無視の対象を広げてもMud全面に通常水域と同数が生えるとは言えない。生成の最終量はBiome密度・競合・重み・ローカル飽和等に左右される。
4. `wildPlantUseDistanceToShore=true` は野生植物選定の追加確率ゲートであり、`GetWaterPlantDistanceToShoreWeight` は**水域のセル**で岸からの距離に応じて重みを減らす。陸側`MarshyTerrain`へ水際距離制限を与える機能ではない。
5. `GenStep_Plants` が同じ`WildPlantSpawner.CheckSpawnWildPlantAt`を生成時に利用するため、特殊な新規マップ生成C#を追加する前に、上述のXML設定＋既存Biomeの`wildPlants`だけで成立するかを限定実験できる。ただし`CanEverPlantAt`単独PASSは`GenStep_Plants`での自然発生PASSではない。

### 実装候補の比較（全て未承認）

| 選択肢 | XML/生成への影響 | 必ず残る問題 |
|---|---|---|
| **ヨシA：湿地土壌だけ** | `fertilityMin>0`を保ち、両湿地の現在の土壌上で生育。水面向け設定を使わない最小変更 | `Soil`タグだけでは通常土壌と`MarshyTerrain`を区別できず、実際の浅水に自生しない |
| **ヨシB：淡水浅水だけ** | `wildTerrainTags=WaterFreshShallowStill`＋`completelyIgnoreFertility=true`＋任意で`wildPlantUseDistanceToShore=true`を**仮Defに限り検証**する候補。最終commonalityは別 | ゼロ肥沃度スポーナー密度への副作用、植物タグの全地形での衝突、岸からの距離、初期生成と長期再生を測定する必要。これだけでは陸側の葦群落ができない |
| **スゲ・ハンノキ：湿地土壌** | 一般土壌をある程度許す簡略案、または限定的に`MarshyTerrain`を識別する専用タグを**調査** | 同じ`Soil`タグから湿地土壌だけを選ぶことはできない。既存の河岸・林縁木本との生存比率も不明 |
| **ミズゴケA：ColdBog＋既存土壌** | `wildPlants`をColdBogのみとする最小案、初期は普通の`Soil`全域に出うる | 「貧栄養泥炭地だけ」という生態表現を満たさない。肥沃度1.00の`MarshyTerrain`を泥炭とみなせない |
| **ミズゴケB：ぬかるみ向け独自タグ** | `MarshyTerrain`のタグにAMJE固有の**追加タグ**を与え、将来のミズゴケ側`WildTerrainTags`で選択するXML中心案 | 地形Def追加タグは全Biomeの同地形に適用され、他Modとのタグ衝突・追加適性を監査する必要。ColdBogのぬかるみが高層湿原を正確に表すわけではない |
| **ミズゴケC：泥炭面専用TerrainDef** | ColdBog生成に限定した別TerrainDef／TerrainPatchの設計から始める、精密な将来案 | ワールド／新規マップ生成・既存セーブ・見た目・他Mod/Waterworks地形との互換を大きく再評価する必要。いま採用しない |

**仕様上の注意:** `wildTerrainTags`に`Soil`と`WaterFreshShallowStill`を同時に並べても、タグはOR条件であるため「水際に限った土壌」にはならない。逆に、`terrainBlacklist`で現行の土壌Defだけを除外する案は、新規Mod地形が追加された際に抜け道になる。必要な環境分離がないまま既存TerrainDefの共通タグを変更・削除しない。

**今回の作業範囲:** 正式な植生重み・最低肥沃度・タグ・ゼロ肥沃度無視・水域植物設置・地形生成を確定しない。既存の4構成と湿地Quickstartsを増やさず、ヨシBの初期生成・再生を本当に採用すると判断した時だけ、使い捨てDefで`GenStep_Plants`を含む限定検証を行う。作者承認済みの採取・CCTO温度値はこの調査で変更しない。

**次の必要証拠（無意味なテスト追加禁止）：** 既存の4構成隔離Quickstarts/Pickleを再利用し、RimWorld 1.6の実際のロード済み地形タグ・肥沃度・候補Def・自然発生適性を調べる。ゼロ肥沃度水面対応を採る必要がある場合のみ、使い捨てDefで種別／地形別の限定実験を行い、対象外の植物密度への副作用を比較する。画像完成後の本番PlantDef・分布比・古いセーブとの互換性は別ゲート。

**現時点の結論：** 特にハンノキとミズゴケを水面全域の植物にする根拠はない。現行草案の配分値を承認・適用せず、ゲーム内で識別可能な湿地の下位環境と対応制限を確認してから値を決める。
