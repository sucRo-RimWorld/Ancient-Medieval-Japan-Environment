# ハンノキ自然季節遷移 — RimWorld 1.6・1.7実ゲーム自動テスト（2026-10-17）

## 判定と対象

**自然な紅葉係数・落葉・再展葉の標準ゲーム機構を検証済み。**
[Project Actions #39016607046](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39016607046) は本物の
Linux RimWorld **1.6.4871 rev600** を隔離されたセルフホストランナーで起動し、
`AMJHannokiSeasonalSampleQuickstart` の **73/73 PASS、実際のUnity ERROR 0、
exit 0、タイムアウトなし**。原本ログ・Quickstart JSON・source SHA 証跡は
[実行artifact](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39016607046) に保存した。

- Environment production source: `4dacbcf0fbe3399ee44795f3097b8cbe498d0ae3`。
- 同一59ファイル配布候補 ZIP SHA256: `4b7707284f1b5a9d404962d21869e5914ea70aeb842951d0d45c395678e4b7c3`。
- 実際にロードした production DLL SHA256: `29c41ebf8998db993ac496d7d5800b9ae39fec613490dee8da6a721cad702996`。
- Project側の専用オンデマンドテストコード: PR [#597](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/597) / `97ebc33a5385e433638946fec83a4c8ecd61db7a`、同一プロファイルのfixture修正 PR [#600](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/600) / `06b54b63869b610ef8fef8c72dc2116cc8a835e2`。

## 実際に検証した前提と判定

ゲームが自然生成した **冷涼林（AMJ_CoolTemperateForest）** の最寒・定住可能
タイル（年平均約8.01℃）に隔離マップを生成した。実際の屋外ブナ10株の位置へ
**テスト専用に生産版 `AMJ_Tree_Hannoki` を若い成熟株として配置**し、
個体ごとにゲームが保持する `LeaflessTemperatureThresh` のうち
落葉が最も観察しやすい個体を1本選んだ。この操作は通常のハンノキの
自然分布を検証するものではなく、ハンノキの自然分布・収穫は先行する
独立の湿原／伐採テストで検証している。

植物の `MakeLeafless`、落葉閾値、気温、`PlantFallColors` の強制上書きは
行わない。年間60日相当を毎時のゲーム内日付進行、
気温キャッシュの標準更新、`Plant.TickLong` で加速サンプリングした。
これは**世界全体の60日分の通常Tickや長期セーブ運用の再現ではない**。

| 観察対象 | 実値・判定 |
| --- | --- |
| 季節の標準紅葉係数 | `minFall=0`、`maxFall=1` |
| 同一ハンノキの生葉→落葉 | `leafless=True`、`Hannoki_A`→`Hannoki_Leafless_A` |
| 同一ハンノキの再展葉 | `recovered=True`、`Hannoki_Leafless_A`→`Hannoki_A` |
| 追跡中のマテリアル | `validTextures=True` |
| 観察最低気温 | −13.5862℃ |
| 標準個体の落葉閾値 | −10.27121℃ |
| native ERROR／exit | 0件／0 |
| Quickstart検証 | 73成功、0失敗 |

実際の原本ログでは落葉した木は冬の `leafless=True texture=Hannoki_Leafless_A`、
57日目に `leafless=False texture=Hannoki_A` へ戻った。

## 先行失敗（改ざん・PASS扱いしない）

[初回 Actions #39016256642](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39016256642) は
**FAIL（59検証中10失敗、Unity ERROR 1、exit 1）**。
冷涼湿原（ColdBog）を対象にした初期fixtureが、自然な湿原がないシードで
暖かい地形へ湿原Biomeを強制適用し、寒さが−7.53℃までしか下がらず
個体閾値−10.01℃に届かなかった。さらに流用した共通シナリオの
湿原に非対応な検証条件などが失敗した。**不適切なfixture条件の訂正**であり
本番の気候／落葉挙動／絵柄は変更していない。元のFAILを保持したまま
修正後の単発実行を別番号で記録している。

## 受け入れの限界と次の工程

この結果は、既に作者採用済みの六つの**静的画像状態**とは独立した
**自然季節遷移のゲーム機構の検証**である。ハンノキの季節機構について
`Docs/PlantVisualCoverage.json` の `transition` を
`automated_native_calendar_sampling` として verified にする根拠となる。
長期間の通常プレイ、毎日全ワールドの完全Tick、
Steamからダウンロードした配布実物やWorkshop非公開更新を証明するものではない。
下記の独立1.7実ゲームテストが1.7の季節機構のみを補完する。

ハンノキの画像バイナリ・生産XML・バランス・元ゲーム・ユーザーのセーブは変更していない。

## Linux RimWorld 1.7 — 独立実ゲーム検証（2026-10-17）

**[Project Actions #39018400784](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39018400784) は PASS。**
本物の Linux **RimWorld 1.7.4977 rev1248** を元のインストールから隔離コピーし、
正式 Environment source `88f054abd75f7955f6597d53ffc2bc22f1556a87` の
C# 8ファイルを未改変のまま **1.7 UnityStubs / .NET 10 に対しテスト専用ビルド**。
このビルドをロードして実マップを生成し、ハンノキ個体の季節変化を観察した。
**配布用の共有 net472 DLLを実際にLinux 1.7でテストした結果ではない。**

### テスト仕様と結果

- **自然生成した** `AMJ_CoolTemperateForest` の最寒定住可能な地点（年平均 **8.004106℃**）を使用。
  ランダムな年間気候を人為的に寒くせず、適用Biomeを変更しない。
- テスト専用マップに、正式な `AMJ_Tree_Hannoki` Defで新規生成した
  **成熟個体12本を配置**。内部の落葉閾値が最も高い個体を1本追跡。
  自然スポーン率のテストとは異なる。
- 年間60日を実ゲームの時間を毎時進める加速サンプリングと
  `WorldComponentTick`／`RetrieveCachedData`／`CheckCache`／
  `MapTemperature.TemperatureUpdate`／`Plant.TickLong` で検証。
  **全マップを60日間の通常Tickで進めるものではない。**
  `MakeLeafless`、気温値、閾値、紅葉係数を強制的に操作しない。
- 実測 **`fallMin=0`／`fallMax=1`**。
  最低気温 **−13.49252℃**、追跡木の標準落葉閾値 **−11.37465℃**。
- **同一個体で落葉・再展葉を計4回観察**。原本ログの切替点：
  step957/day45/−11.52738℃ → 落葉、
  step1031/day48/−9.251407℃ → 葉あり、
  step1076/day50/−11.58789℃ → 落葉、
  step1198/day55/−9.555622℃ → 葉あり。
  木の内部 `LeaflessNow` 変化を検証した。
- 本物のPickle **1/1 PASS**、failed0、skipped0、flaky0、
  Unity ERROR0、FMOD ERROR0、ゲーム exit0、timeoutなし。
  実際のランナーが原本 `launcher.log`、`summary.json`、
  `result.json`、ビルドログをAction artifactに保存。
- テストのtest-only実装は Project PR [#604](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/604) から始まり、
  最終的に **Project main `7f123d6d279cb67bb9f4db1a656223cf668b3f50`** に入った
  1.7気温キャッシュ再評価を含む。テスト専用の温度更新のみ変更し、
  Environmentの生産XML・テクスチャ・植物コードには変更なし。

### FAIL履歴と検証境界

最初のゲームまで到達した
[Actions #39018171464](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39018171464) は
**厳密にはFAIL**（Pickle0/1、Unity ERROR2、ゲームexit1）。
`fallMin=0`／`fallMax=1` は確認したが、
温度キャッシュを適切に再評価しないテスト用実装では
観測最低気温が **+2.306426℃** に留まり、落葉閾値 **−11.37465℃** に到達しなかった。
テスト専用fixtureのキャッシュ処理を1.6で実績のある方法に修正したのであり、
気候の本番実装・落葉閾値・紅葉色を変更してFAILをPASSに置き換えたわけではない。
それ以前のビルドエラーもゲーム起動前のtest-only fixture欠陥として保持。

**1.7で証明したのは植物の季節係数・落葉フラグ・再展葉と実マップ上の実行である。**
1.6と異なり、今回の1.7シナリオはマテリアルの
`Hannoki_A / Hannoki_Leafless_A` 実画面切替、
画像の美観、雪オーバーレイを検証していない。
また Linux1.7の共有配布DLL、Vanilla+MO+CCTOの全構成、
Steamからダウンロードした物理パッケージ、実セーブ長期進行を保証しない。

既存1.6の同一59ファイル配布候補に対する季節／グラフィックの証拠は
本書冒頭に残し、二つのゲームバージョンの独立したテスト結果を混同しない。
