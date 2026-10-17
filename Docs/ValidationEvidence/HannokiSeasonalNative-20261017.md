# ハンノキ自然季節遷移 — 実RimWorld 1.6自動テスト（2026-10-17）

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
長期間の通常プレイ、毎日全ワールドの完全Tick、1.7での同一試験、
Steamからダウンロードした配布実物やWorkshop非公開更新を証明するものではない。

ハンノキの画像バイナリ・生産XML・バランス・元ゲーム・ユーザーのセーブは変更していない。
