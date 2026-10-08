# Vanilla湿地Biome残存監査

監査日: 2026-10-08 JST

対象:
- `TemperateSwamp`
- `ColdBog`

目的:
AMJEの4基準植生帯から除外している `swampiness >= 0.5` の土地で、Vanilla湿地Biomeを残すこと自体が古代～中世日本環境として妥当かを、植物だけでなくBiome全体の責務として再監査する。

## 結論

**湿地という環境区分は残す。Vanillaの `TemperateSwamp` / `ColdBog` DefNameと基本的な湿地生成ロジックも、互換性を優先して採用してよい。**

ただし、現状のように `wildPlants` だけを日本向けにPatchした状態を最終形とはしない。

AMJE有効時は両Biomeを「採用するVanilla湿地Biome」として扱い、以下をAMJE側で監査・補正する。

1. 植生
2. 野生動物
3. 病気
4. 天候
5. 説明文
6. 必要ならworld texture / 表示
7. pack animal等、Biomeに束ねられた残存設定

地形生成とBiomeWorkerは、監査で問題がない限りVanilla再利用を優先する。新しいAMJE湿地BiomeDefを作るのは、既存Defでは日本の湿地構造を表現できないことが判明した場合だけ再検討する。

## Vanilla 1.6側の構造

### TemperateSwamp

Vanillaの基本値:
- `animalDensity = 4.3`
- `plantDensity = 0.80`
- `movementDifficulty = 4`
- `diseaseMtbDays = 40`
- 地形: Soil / Rich Soilを基礎に、MarshyTerrain / Mud / WaterShallowをPatch生成
- 天候: Clear 18 / Fog 1 / Rain 2 / DryThunderstorm 1 / RainyThunderstorm 1 / FoggyRain 1 / SnowGentle 4 / SnowHard 4

Vanilla worker:
- 水上タイルは不可
- 年平均気温 -10℃未満は不可
- rainfall 600未満は不可
- swampiness 0.5未満は不可
- それ以外では気温・降水量・swampinessが高いほどscoreが上がる

**判定: 湿潤な低地湿地を作る技術基盤としては利用可能。**

MarshyTerrain / Mud / shallow waterを混在させる地形構造は、日本の低地湿原・沼沢地を表現する基盤として妥当。Workerも「十分な湿潤性を持つ暖～温帯寄りの湿地」を選ぶため、AMJE世界での特殊湿地overlayとして大きな矛盾はない。

### ColdBog

Vanillaの基本値:
- `animalDensity = 3.3`
- `plantDensity = 0.60`
- `movementDifficulty = 4`
- `diseaseMtbDays = 45`
- 地形: Soil / Rich Soilを基礎に、MarshyTerrain / Mud / WaterShallow / MossyTerrain / MarshをPatch生成
- 天候: TemperateSwampと同じく Clear 18 / Fog 1 / Rain 2 / DryThunderstorm 1 / RainyThunderstorm 1 / FoggyRain 1 / SnowGentle 4 / SnowHard 4

Vanilla worker:
- 水上タイルは不可
- 年平均気温 -10℃未満は不可
- swampiness 0.5未満は不可
- それ以外では低温かつswampinessが高いほどscoreが上がる

**判定: 冷涼な湿原・泥炭湿地を作る技術基盤としては利用可能。**

日本の冷涼湿原にはヨシ・スゲを主体とする低層湿原、ハンノキ林、ミズゴケを主体とする高層湿原が共存する。Vanilla ColdBogの浅水・泥・苔・marshを混在させる構造は、個々の植物Defを日本向けに補正する前提なら再利用価値が高い。

## 日本の湿地との対応

環境省の湿地資料では、日本の湿地は亜寒帯から亜熱帯まで広がり、bog / fen、湖沼、河川、沿岸湿地など多様な形を取る。

代表例として釧路湿原では、
- ヨシ・スゲ類を主体とする低層湿原
- ハンノキ主体の湿地林
- ミズゴケを主体とする高層湿原
が併存する。

したがって「温帯湿地」「冷涼なbog/fen」というBiome類型自体をAMJEから排除する理由はない。むしろ日本列島の自然環境を表現するために湿地overlayは必要である。

参考:
- https://www.env.go.jp/en/nature/npr/wetland/present.html
- https://www.env.go.jp/en/nature/npr/ramsar/ramsar.html
- https://www.env.go.jp/nature/nationalparks/list/kushiro-shitsugen/feature/
- https://www.env.go.jp/nature/important_wetland/wetland/w027.html

## 現状の問題点

### 1. 野生動物 — **未監査で不適切**

現在のAMJE Patchは湿地の `wildAnimals` を変更していない。

Vanilla Coreの例では、TemperateSwampに Raccoon / Ibex / Muffalo / Alpaca / Boomalope / Boomrat / Megasloth / Rhinoceros / Cougar / Lynx / Warg 等が含まれ、ColdBogにも Elk / Caribou / Muffalo / Ibex / Raccoon / Turkey / Megasloth / Arctic Fox / Arctic Wolf / Cougar / Lynx / Warg / Polar Bear 等が含まれる。

これはAMJEの4独自Biomeで既に採用したJapan-oriented wildlife proxy方針と矛盾し、湿地経由で除外済みの動物が再流入する。

**要修正。**

湿地もEnvironmentのwildlife proxy policyに従い、既存Vanilla動物から日本の湿地・周辺森林に機能的に対応できるものだけを採用する。水鳥等を新規追加することはこの監査だけでは決めない。

### 2. 病気 — **Vanilla bundleをそのまま採用しない**

TemperateSwamp / ColdBogには、Flu / Plague / GutWorms / MuscleParasitesに加え、FibrousMechanites / SensoryMechanites等がBiome固有リストとして束ねられている。TemperateSwampにはMalariaも高commonalityで設定されている。

古代～中世日本にマラリア相当の「おこり」等が存在したこと自体は否定しないが、Vanillaの病気セットと比率をそのまま「日本の湿地性疾患」と解釈する根拠にはならない。また、MechanitesはAMJの歴史環境表現とは別問題である。

**要再設計。** 湿地だからVanilla disease listを丸ごと継承するのではなく、AMJE全体の時代・Incident方針と整合させる。

参考:
- https://www.jstor.org/content/oa_book_monograph/jj.23996202
- https://pmc.ncbi.nlm.nih.gov/articles/PMC3143877/

### 3. 天候 — **AMJE基準と不整合**

両Vanilla湿地は `DryThunderstorm = 1`、SnowGentle / SnowHard = 4 / 4 を共有する。

AMJEの基準Biomeは、湿潤な日本環境を反映してDryThunderstormを0.05～0.1程度へ抑え、雪は温度帯に応じて段階化している。Vanilla湿地だけDryThunderstormが高く、暖冷差を無視して同じ雪比率を使うのはAMJEの既存気候設計と不整合。

**要修正。**

### 4. 説明文 — **日本向け説明へ書き換える**

Vanilla TemperateSwampは「vegetation and diseaseにchoked」、ColdBogは「trees and vines」「disease is endemic」といったRimWorld汎用の過酷Biome説明で、日本の湿地植生・景観を説明していない。

残すVanilla植物と同じく、採用するBiomeの説明もAMJEの日本向け説明フォーマットへ変更する。

### 5. 植生 — **Phase 5は暫定的に有効だが最終ではない**

現行Phase 5によるChokevine / Cypress / Astragalus除外とcommonality再配分は、Vanilla植生の再流入防止として有効なので維持する。

ただし日本の湿地景観としては、ヨシ・スゲ・ハンノキ・ミズゴケ等の表現が不足している。これらは植生整備ロードマップStep 3「不足する古代～中世日本植生追加」で扱う。

Step 1では、現行generic植物を使った暫定構成でもよいが、Biome全体から明白に不適切なVanilla bundleを除去する。

## 採用方針

### 残す
- `TemperateSwamp` / `ColdBog` のDefName
- 基本的なwetland BiomeWorker / score思想
- movementDifficultyの「移動困難な湿地」というゲーム上の役割
- mud / marsh / shallow water / mossを使う地形生成
- 現行Phase 5で確定済みの植物除外と再配分

### AMJEで上書き・監査する
- `wildAnimals`
- `diseases` / `diseaseMtbDays`
- `baseWeatherCommonalities`
- description / localization
- 必要に応じてforage / pack animal / world texture等の残存field

### 後段へ送る
- ヨシ・スゲ・ハンノキ・ミズゴケ等の新規日本植生
- 日本固有の水鳥・両生類等の新規PawnKind
- 湿地専用の新規地形が本当に必要かの検討

## Step 1完了条件への影響

従来の「不要Vanilla植物を削除したのでStep 1完了」という扱いは撤回する。

**Step 1の完了テスト前に、少なくとも以下を実施する。**
- TemperateSwamp / ColdBogのwildAnimalsをJapan-oriented proxy setへ置換
- Vanilla湿地の病気・天候bundleをAMJE方針へ監査・補正
- 両Biome説明を日本向けに修正
- loaded Def Quickstartで、除外動物・除外植物が湿地経由で再流入しないことを確認
- 地形生成（mud / marsh / shallow water等）が維持されることを確認
- world generationで湿地比率が不意に消滅・急増していないことを確認

このゲートが通ってから、植生ロードマップStep 2「説明文監査 → リテクスチャ」へ進む。

## 2026-10-08 湿地生態bundle実装（RimWorld 1.6・実機検証待ち）

### 実装境界

`Patches/VanillaWetlandEcology.xml` は、両Vanilla湿地の `wildAnimals`、`baseWeatherCommonalities`、`diseaseMtbDays`、`diseases`、`allowedPackAnimals` をfield単位で置換する。

`Patches/VanillaWetlandVegetation.xml` に記録されたPhase 5植生、Vanilla BiomeWorker、湿地地形生成、animalDensity/plantDensity、forage、移動困難度、world textureは変更しない。病気の選定と重み付けは史実の有病率を主張するものではなく、古代～中世日本の環境表現を目的とした**暫定的なゲーム内バランス**である。

### 野生動物（Vanilla PawnKind代理）

| PawnKind | TemperateSwamp | ColdBog |
|---|---:|---:|
| Hare | 0.80 | 0.80 |
| Snowhare | — | 0.50 |
| Squirrel | 0.60 | 0.70 |
| Rat | 1.00 | 1.00 |
| Deer | 0.70 | 0.50 |
| WildBoar | 0.80 | 0.50 |
| Fox_Red | 0.06 | 0.05 |
| Wolf_Timber | 0.03 | 0.05 |
| Bear_Grizzly | 0.04 | 0.04 |

海外固有のVanilla大型動物、Raccoon、Ibex、Lynx、Arctic Fox/Polar Bear、機械的/架空のBoom系やWargは採用しない。荷役動物リストは空にしてVanillaのMuffalo/Alpacaを野生の湿地動物として再流入させない。新しい湿地固有の水鳥等は引き続き将来の追加対象とする。

### 天候

| WeatherDef | TemperateSwamp | ColdBog |
|---|---:|---:|
| Clear | 16 | 16 |
| Fog | 2 | 2 |
| Rain | 3 | 3 |
| DryThunderstorm | 0.10 | 0.05 |
| RainyThunderstorm | 1.5 | 1 |
| FoggyRain | 1.5 | 1.5 |
| SnowGentle | 2 | 7 |
| SnowHard | 1 | 7 |

湿潤環境を表現するため乾いた雷雨の相対重みを小さくし、温暖な湿地と冷涼な湿地の降雪重みを分離する。これはWeatherDefの生成抽選用commonalityであり、実際の降水/積雪日数を意味しない。

### 病気（ゲーム内incidentの抽象化）

| diseaseInc | TemperateSwamp | ColdBog |
|---|---:|---:|
| Disease_Flu | 100 | 100 |
| Disease_Plague | 45 | 35 |
| Disease_Malaria | 35 | — |
| Disease_GutWorms | 65 | 40 |
| Disease_MuscleParasites | 40 | 30 |
| Disease_AnimalFlu | 90 | 100 |
| Disease_AnimalPlague | 40 | 35 |
| diseaseMtbDays | **50日** | **60日** |

`Disease_FibrousMechanites` と `Disease_SensoryMechanites` は採用しない。マラリアは暖湿地のみで重みを下げて維持し、冷涼湿地には置かない。病気ごとのcommonalityはincident候補内の相対重みであって、古代～中世日本の疫学的頻度ではない。将来World Rulesが担当する全球的な時代外Incident制御とは責務を分ける。

### 確定説明文（日本語承認済み・2026-10-08）

ユーザー承認の日本語を正本とし、文意と範囲を変えずに英語へ翻訳した。既存のVanilla `label` は変更しない。英語本文は `Patches/VanillaWetlandDescriptions.xml` で置換し、日本語本文は `Languages/Japanese/DefInjected/BiomeDef/AMJ_Biomes.xml` で上書きする。病気の過度な強調や未実装の植物・機能は追加しない。

**TemperateSwamp（温帯湿地）**

日本語（承認済み）:

> 日本列島の温暖で雨の多い低地や河川沿いに広がる湿地。背の高い草やヤナギ類の木立が入り混じり、泥土と浅い水面が広がる。湿った地盤は通行や建築に制約を与える。

English:

> A wetland found in the warm, rainy lowlands and along rivers of the Japanese archipelago. Tall grasses and stands of willow trees intermingle among muddy ground and shallow water. The damp ground restricts travel and construction.

**ColdBog（冷涼湿原）**

日本語（承認済み）:

> 冷涼な地域に広がる湿原。草本やコケ類に加え、ヤナギやカバノキ類の木立が点在する。水を多く含む地盤はぬかるみやすく、移動や建築が難しい。

English:

> A wetland found across cooler regions. Grasses and mosses grow alongside scattered stands of willow and birch trees. The waterlogged ground readily turns muddy, making travel and construction difficult.

**実装と検証:** 両英文の `/Defs/BiomeDef[defName="..."]/description` のみをPatchOperationReplaceし、日本語は同一DefNameの `.description` DefInjectedで置換する。BiomeのWorker、名称、湿地地形・動植物・天候・病気には影響させない。`Tests/test_wetland_ecology_contract.py` でXML/日英本文/変更対象を固定し、両湿地Quickstartのロード済み `BiomeDef.description` が日本語または英語の承認本文と一致することを回帰確認する。**実装変更後の実機テストは2026-10-08に作者報告PASS**。先行するPR #27の9シナリオPASSとは独立の報告として記録する。対応するログ・JSONは未添付のため、個別アサーション数・ログ完全性・起動前/実行時ERRORゼロを独立監査したものとは扱わない。

### 残る完了判定

- `Tests/test_wetland_ecology_contract.py` が静的Patch構造・内容を確認する。
- 両湿地Quickstartは、ロード済みwildAnimals/diseases/weather、pack animal、疾病MTB、Vanilla湿地地形生成を検証する。
- 非表示の実描画テストは `run-tests.bat` が標準入口。
- 従前の自然分布テスト（PR #27）と、今回追加したロード済み説明文の実機テスト（PR #28）はいずれも作者報告PASS。両方とも詳細ログ・JSONは未受領のため、実測値・ERROR検証結果の独立監査は未完了。
- 日本語本文承認・日英XML同期・ロード済み説明文Quickstartの作者報告PASSは揃った。実装済み動植物・疾病・天候Patch、地形生成、自然分布の回帰ゲートと照合し、**湿地の基礎Step 1を作者報告ベースで完了**とする。詳細ログ・JSONによる独立監査、湿地比率の本格的な数値較正、4構成の統合試験は別途未完了。4構成のMO/CCTO統合試験は別のリリースゲート。


**次工程:** 植生ロードマップStep 2の既存植物残存判定と日本語説明文監査への着手を許可する。既存樹木の画像制作そのものは作者指示により保留し、別途`Docs/GoldenPaths/PlantVisualCoverage.md` の未完了項目を解消する。未実装のヨシ・スゲ・ハンノキ・ミズゴケなどはStep 3で検討し、このStep 1に遡って必須化しない。

## 自然生成世界の湿地分布ゲート（2026-10-08、作者による実機PASS報告あり）

従来の湿地QuickstartはBiome強制設定を許すため、自然分布の存在を立証しない。別途 `AMJWorldWetlandDistributionQuickstart` を標準テストに加える。

- seedは `AMJ-Environment-Terrain-Alpha`、coverage `0.30`（通常の0.05より広い世界）。`PrimaryBiome`・`swampiness` の値は読み取り専用で集計する。
- 陸地、湿潤度0.5以上の候補、`TemperateSwamp` と `ColdBog` の自然生成数、両者合計の陸地比率、候補外の湿地、未割当Biomeをログ出力する。
- 初期回帰ゲート：陸地2000以上、湿地候補と自然生成湿地がともに非ゼロ、候補外湿地と未割当Biomeがゼロ、自然生成湿地で入植マップ生成ができること。
- 陸地20%以下という上限は**粗い異常検知用の暫定値**であり、日本列島の歴史的な湿地割合という主張ではない。実測後に目標上下限を別途検討する。
- 小さな既往テスト世界では湿地候補がゼロだったが、その事実を通常サイズ世界への証明に使わない。

標準テストは8本から9本になる。2026-10-08、作者からPR #27のテストがPASSした旨の報告を受領したため、**実機テストは作者報告PASS**として扱う。ログ/JSONの添付はなく、各アサーション数、実際の湿地候補数・出現率、完全キャプチャ、起動前・実行時ERRORゼロをこちらで独立再確認した記録はない。既存の8/8 PASSは別の歴史的証拠として保持する。湿地比率の適正値の承認や4構成の統合試験の完了は、この報告からは推定しない。
