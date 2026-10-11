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

**次の必要証拠（無意味なテスト追加禁止）：** 既存の4構成隔離Quickstarts/Pickleを再利用し、RimWorld 1.6の実際のロード済み地形タグ・肥沃度・候補Def・自然発生適性を調べる。ゼロ肥沃度水面対応を採る必要がある場合のみ、使い捨てDefで種別／地形別の限定実験を行い、対象外の植物密度への副作用を比較する。画像完成後の本番PlantDef・分布比・古いセーブとの互換性は別ゲート。

**現時点の結論：** 特にハンノキとミズゴケを水面全域の植物にする根拠はない。現行草案の配分値を承認・適用せず、ゲーム内で識別可能な湿地の下位環境と対応制限を確認してから値を決める。
