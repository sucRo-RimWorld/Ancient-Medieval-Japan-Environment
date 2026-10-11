# Step 3 湿地4種の地形適性・野生発生モデル調査（未承認）

**状態：検討資料／ゲームへの適用なし。** 2026-10-11。所有：AMJ Environment。
正式な植生・資材・CCTOの承認範囲は `Docs/NativeVegetationStep3Design-ja.md` §3.1–3.3.1 および `Docs/Research/WetlandPlantMaterialsProfiles-ja.md` を優先する。配分数値は引き続き `Docs/Research/WetlandPlantDistributionDraft.json` の `proposal_not_approved` のまま。本記録は本番採用・本番PlantDef自然発生PASSではない。仮Defのみの観測結果を含む。

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

既存の湿地Quickstartsへ一時的な読み取り専用の観測処理を追加し、
実際にロードされた地形Defの`tags`、`IsWater`、`fertility`を
`[AMJ Environment Wetland Loaded Terrain Contract]` として採取した。
観測値を研究JSONに保存し、一時観測コードは調査完了後に除去した。
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

## ヨシ浅水自然発生：隔離実機の観測結果（2026-10-11）

**結果：テスト専用ヨシは両湿地で通常のマップ生成後に浅水上で確認できた。**
仮Defは `AMJE_Test_YoshiWaterSpawn`。本番ヨシでも正式な分布採用でもなく、隔離 `AMJE.EnvironmentQuicktests` に限る。試験HEAD `b6a4ebde103f8b11b6c8c4527b19d59882e353f1`、PR #98。作者から提示された各Quickstartの **INFOログ抜粋2本**が根拠。ログを出す時点はマップ生成後、既存CutPlantプローブによるセル改変の前である。

| マップ | 浅水上の仮ヨシ | 他地形 | 背の高い草 | コケ | 総植物数 |
|---|---:|---:|---:|---:|---:|
| TemperateSwamp | **1,026** | **0** | 17,805 | 0 | 37,767 |
| ColdBog | **789** | **0** | 10,917 | 8,287 | 26,656 |

実験に限り、`wildTerrainTags=WaterFreshShallowStill`、`completelyIgnoreFertility=true`、`wildPlantUseDistanceToShore=true`、`wildOrder=1`、`fertilityMin=0.05`、各Biomeへ **0.25を追加**した。既存植物の重みは削っていない。別実行で得た浅水セル数3,672 / 3,542を仮の分母とすれば約 **27.94% / 22.28%** に相当する。ただし両ログには今回の地形セル数がなく、厳密な同一実行の被覆率ではない。この追加重みが高密度となることを示唆しても、本番の適量を決める根拠ではない。

元のベースライン観測でのTallGrassは17,977 / 11,054、ColdBogのMossは8,488。今回のTallGrassは17,805 / 10,917、MossはColdBog 8,287。ただし**別実行の観測値の差は、ヨシの追加による因果的減少と判定しない**。比較用の同一設定・独立乱数管理された対照実験も長期再生試験もない。ゲーム版・実行プロファイル・完全なテスト集計および当該ローカルHEADは、このログ抜粋だけでは確定できない。

技術的に実現可能なことと、生態・密度・原料・画像・古いセーブの承認は区別する。特に `0.25` の常設、全地形のゼロ肥沃度無視、公式の浅水専用ヨシDef追加は行わない。本番への採用前に、ヨシを陸側・水側へどう配分するか、他植物が実際にどれだけ影響を受けるか、採取量や景観に見合う密度を判断する必要がある。

観測のデータ正本：`Docs/Research/WetlandPlantDistributionDraft.json` 内 `water_yoshi_natural_spawn_observations`。完了した一時試験のDef・Quickstarts計測・ステージングはPR #98で除去し、通常の自動回帰テストには増やさない。元の試験コード・条件はPR #98のコミット `b6a4ebde103f8b11b6c8c4527b19d59882e353f1` に残る。

## 4種の水際・陸側の役割分担と限定実験の結果（2026-10-11、未承認）

### 地形の観測と設計上の分離可能性

両湿地の実測済み `TerrainDef` タグによれば、`WaterShallow` は
`WaterFreshShallowStill`（肥沃度0）、`Marsh` は `WaterMarshy`
（肥沃度0）、`Mud` は `Mud`（肥沃度0）で、互いを区別できる。
`MarshyTerrain`（肥沃度1.00）、通常 `Soil`（1.00）、
`SoilRich`（1.40）、`AMJ_ThinSoil`（0.50）は全て
`Soil` タグである。**湿地における `Soil` タグだけでは、
河岸の乾湿境界や泥炭面を切り分けられない。**
温帯湿地には `Marsh` が存在しない実測があり、ColdBogでは
477セルだった。これは別サンプルの値であり恒常的な地形比率ではない。

| 植物（正式仕様は未承認の部分を残す） | まず表現したい場所 | 現行のXMLのみでできること | 危険な近道・重複 |
|---|---|---|---|
| **ヨシ** | 水際の淡水浅水と低層湿原の陸側 | `WaterFreshShallowStill` と `Soil` のOR指定による**同一Defの水陸両生**は検証候補。浅水だけの自然発生はPR #98で既に実測 | `Soil` は普通の土も含む。岸からの距離を使う標準設定は陸上を水際だけに限定しない |
| **スゲ** | 湿った草地・ぬかるみと中間湿原の周縁 | `Soil` 上で生育、TallGrassの一部重みを段階的に移す案 | 一般土壌とぬかるみを混同して大量発生させること。深水生育を一律解放すること |
| **ハンノキ** | 浅い冠水の周辺、沼沢林・湿地林の立地 | 非水域の土壌と木本重み内で試す案。Willow/Maple/Birchは保護 | ゼロ肥沃度の水面全域で木本が成立する扱い／木本総量の単純増加 |
| **ミズゴケ** | ColdBogの低木密度・貧栄養泥炭相当域 | **ColdBogのみに配置**するBiome単位の候補が最小。ただし`Soil`制約だけでは泥炭を再現しない | 温帯湿地も含め`MarshyTerrain`を高層湿原と同一視すること。水面へ肥沃度無視を一律適用すること |

### ヨシの1つの本番Defで水陸共存させる案：比較の重要性

`PlantUtility.CanEverPlantAt` の `wildTerrainTags` は複数タグの
OR判定である。`Soil` と `WaterFreshShallowStill` の両方を許す
**単一ヨシDef**は技術的には土地と浅水に配置しうるが、
`BiomeDef.wildPlants` はこの1Defへ**1つのcommonality**しか与えない。
水側と陸側それぞれに別の重みを付ける機構ではない。
一方、浅水で競合する植物がヨシだけなら、commonalityを低くしても
密度が単純比例して下がらない可能性がある。
`wildPlantUseDistanceToShore` は水域側だけへの確率制限であり、
`Soil` 上のヨシを河岸へ引き寄せる仕組みではない。

- **案A：同一Def、水陸をOR許可。** 植物1種・Harvest/保存/翻訳/描画
  の単一性は維持できる。しかし湿地以外も含む`Soil`に広く分布し、
  浅水と陸側の重みを独立に制御できない。今回、水陸両域への自然発生を確認。ただし発生位置を個別に制御できない。
- **案B：同じ植物を別Defの陸側型／水側型に分割。** 各型で
  `commonality` と地形適性を区別できるが、ThingDefが2件となり、
  収穫・名札・選択UI・画像・セーブ・将来CCTOの設定重複が生じる。
  現行の「湿地ヨシ1種」という設計との齟齬もあるため、**現段階では非推奨・未採用**（作者の決定ではない）。
- **案C：湿地土壌を局所的に認識する別の生成条件。** 精度は改善しうるが、
  TerrainDef／PatchMaker／生成C#の追加、既存セーブやMO・Waterworksへ
  波及するため、XML-onlyを使い切るまでは取りかからない。

**試験PR #99：水陸両用ヨシ仮Defの実測（2026-10-11、観測のみ）**。

作者提示の両湿地Quickstart INFOログ抜粋による。対象は
`AMJE_Test_YoshiDualHabitatSpawn`、試験ブランチHEAD想定
`bc6d50a116ecb727563e93d02ac4c0423a696346`。ローカル`git rev-parse HEAD`・全体`run-tests.bat`
終了コード・完全なERRORゲートはこの2行だけでは照合できない。
**仮Defがゲーム内の通常マップ生成によって発生した事実だけを認定し、
テストスイート全体のPASSとは扱わない。**

仮Defは`wildTerrainTags=[WaterFreshShallowStill,Soil]`（OR）、
`completelyIgnoreFertility=true`、
`wildPlantUseDistanceToShore=true`、`wildOrder=1`、
`fertilityMin=0.05`。既存植生の重みは変えず、
`commonality=0.05`を両湿地へ**追加**した。

| 生育地形・同一実行の地形セル数 | TemperateSwamp | ColdBog |
|---|---:|---:|
| WaterShallow | **1,063 / 3,672** | **775 / 3,542** |
| MarshyTerrain | 8 / 13,336 | 2 / 10,886 |
| Soil | 7 / 14,780 | 4 / 18,009 |
| SoilRich | 13 / 18,469 | 5 / 11,580 |
| その他Soilタグ地形 | 0 | 0 |
| その他浅水タグ地形 | 0 | 0 |
| **許可タグ外** | **0** | **0** |
| **仮ヨシ総数（うち陸側）** | **1,091（28）** | **786（11）** |
| 浅水割合（仮ヨシ総数に占める） | **97.43%** | **98.60%** |
| 背の高い草（TallGrass） | 17,889 | 10,841 |
| コケ（Moss） | 0 | 8,305 |
| 全植物（NonStumpPlant） | 37,777 | 26,633 |

証拠：`TestResults/VegetationRuntime/Reports/AMJTemperateSwampVegetationQuickstart.log:1093`
（2026-10-11 14:17:59.897）および
`AMJColdBogVegetationQuickstart.log:1080`
（同日14:18:34.006）。
`[AMJ Environment Yoshi Dual Habitat Generated]` 行は
既存CutPlant手動プローブ**前**の自然生成数である。

**判定:** この構成で、淡水浅水と`Soil`タグを持つ陸地の
**双方への自然発生は成立**。一方で生成個体の約97〜99%が浅水に集中した。
`Soil`指定は岸辺だけでなく普通土・肥沃土にも及び、
`wildPlantUseDistanceToShore`は陸地を水際に制限しない。
これを本番バランスの承認とみなさない。

以前の水域限定`commonality=0.25`時の浅水数は
1,026 / 789で、今回の1,063 / 775と近い。
しかし許可地形と重みを**同時に変更**し、
別の非ペアマップで計測しているため、
`commonality`低下の効果・変化の原因・既存植物への影響を
**因果的に推定できない**。浅水候補が少ない場合には
相対競合・密度ゲートが支配する可能性がある。

**処理:** 仮Def・両湿地への仮Patch・一時Quickstart計測・
そのためだけの静的テストはPR #99の統合前に撤去し、
使い捨てコードの履歴はPRに残す。以降の常設シナリオは増やさない。
正式な`AMJ_Plant_Yoshi`の水陸配置、必要な局所性、
最終重み、収量・画像・4構成・セーブ・長期再生は別途検討し、
未承認の値を本番XMLへ流用しない。

### 固定する現行の生態・所有契約

- Yoshi/Sugeは既存TallGrass側、MizugokeはColdBogのMoss側、
  Hannokiは現行木本配分から移す候補。今の草案
  TemperateSwamp合計7.30/木本3.00、ColdBog合計8.22/木本1.80
  を破らないこと。別Mod追加の植物は削除しない。
- Suge→Vanilla Hay（MOでHay→Straw）、Mizugoke→選択可・
  非収穫・伐採除去のみ／無ドロップは作者承認済み。
  YoshiのHay/収量、HannokiのWoodLog量、全4種の分布重みは未承認。
- CCTO併用時の最低生育温度と休眠・枯死温度は
  `Docs/NativeVegetationStep3Design-ja.md` §3.3.1を優先し、
  地形分布の調査によって勝手に再決定しない。
- 新しい4種は**まだ本番PlantDefがない**。仮DefのCut/Spawn PASSを
  実際の本番画像・採集・自然再生・旧セーブ・4構成PASSへ転用しない。

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

**今回の作業範囲:** 正式な植生重み・最低肥沃度・タグ・ゼロ肥沃度無視・水域植物設置・地形生成を確定しない。既存の4構成と湿地Quickstartsのシナリオ数を増やさず、ヨシBの初期生成・再生を本当に採用すると判断した時だけ、使い捨てDefで`GenStep_Plants`を含む限定検証を行う。完了した一時観測を定常回帰へ追加しない。作者承認済みの採取・CCTO温度値はこの調査で変更しない。

**次の必要証拠（無意味なテスト追加禁止）：** 既存の4構成隔離Quickstarts/Pickleを再利用し、RimWorld 1.6の実際のロード済み地形タグ・肥沃度・候補Def・自然発生適性を調べる。ゼロ肥沃度水面対応を採る必要がある場合のみ、使い捨てDefで種別／地形別の限定実験を行い、対象外の植物密度への副作用を比較する。画像完成後の本番PlantDef・分布比・古いセーブとの互換性は別ゲート。

**現時点の結論：** 特にハンノキとミズゴケを水面全域の植物にする根拠はない。現行草案の配分値を承認・適用せず、ゲーム内で識別可能な湿地の下位環境と対応制限を確認してから値を決める。
