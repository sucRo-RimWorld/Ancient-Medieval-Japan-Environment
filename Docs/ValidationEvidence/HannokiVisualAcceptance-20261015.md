# ハンノキ実機表示画像の作者採用 — 2026-10-15

## 採用と未検証の境界

作者は、実機撮影画像とその切り抜き・拡大版を確認した上で
「**まあ採用で**」「**どうせ後で実ゲーム確認するし**」と明示した。
拡大するだけの提示方法については見づらいという否定的評価を示している。
これは**現行画像を変更・再生成せずに採用する意思表示**であり、
画像の見せ方が適切だったという評価、自然な季節遷移の実証、
Workshop配布完了やゲーム内での今後の微修正放棄を意味しない。

`Docs/PlantVisualCoverage.json` のハンノキは、以下の
**六つの静止した実機外観**（normal / immature / icon / autumn /
leafless / snow）を author-accepted と記録する。
**transition**（自然な紅葉・落葉・回復の時間的推移）は今回の
強制状態撮影では確認できないため、実プレイ確認を待つ `pending` とする。
よって現時点で `complete: false`、全状態 `--require-complete`
PASS は主張しない。画像の採用自体は完了扱いとし、
この一件を理由に再生成やさらなる拡大画像を要求しない。

## 本番ソースと実機撮影

- 対象Mod: `sucro.ancientmedievaljapan.environment`。
- 対象の実装ソース: `4dacbcf0fbe3399ee44795f3097b8cbe498d0ae3`。
- 対象配布候補: 59ファイル ZIP SHA-256
  `4b7707284f1b5a9d404962d21869e5914ea70aeb842951d0d45c395678e4b7c3`。
  本番DLL SHA-256
  `29c41ebf8998db993ac496d7d5800b9ae39fec613490dee8da6a721cad702996`。
- [実撮影GitHub Actions #38701213882](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38701213882)：
  5場面とも実RimWorld Linux 1.6.4871 rev600、正常終了、
  元Unity ERROR 0、1280×720、正午・Clear・一時停止、
  カメラzoom=18のレビュー用Quickstart。
- 撮影画像の固定Gitコミット:
  [Project review-only commit `ea7e480a8191f774c31fdb386fc5e2e7c7641b6a`](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/tree/ea7e480a8191f774c31fdb386fc5e2e7c7641b6a/ReviewScreenshots/Hannoki/38701213882)。
  [撮影summary](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/ea7e480a8191f774c31fdb386fc5e2e7c7641b6a/ReviewScreenshots/Hannoki/38701213882/capture-summary.json)
  と [test-only instrumentation](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/ea7e480a8191f774c31fdb386fc5e2e7c7641b6a/ReviewScreenshots/Hannoki/38701213882/instrumentation.json)
  を保存している。
- 本番配布ファイル、画像の原バイト、通常のプレイ環境、
  セーブデータ、Steam Workshopのアップロードは変更していない。
  ハンノキを4行に並べるテスト用Quickstartコードのみ
  一時コピー内で差し替えた。自然生成分布・実際の季節経過の再現ではない。

| 採用画像状態 | 実画面の条件・観察対象 | 証跡スクリーンショット | PNG SHA-256 |
| --- | --- | --- | --- |
| normal | 成熟株／葉あり、成長段階の右列 | [growth.png](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/ea7e480a8191f774c31fdb386fc5e2e7c7641b6a/ReviewScreenshots/Hannoki/38701213882/growth.png) | `0c43a4424ec0eed5558aa7296e77085056107e55a5d9cfeb623eb45f8d81b47f` |
| immature | 同じ通常画像の成長率10/50/100%表示（独立した幼木テクスチャではない） | [growth.png](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/ea7e480a8191f774c31fdb386fc5e2e7c7641b6a/ReviewScreenshots/Hannoki/38701213882/growth.png) | 同上 |
| icon | 実Defの32/64px UIアイコン | [icon.png](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/ea7e480a8191f774c31fdb386fc5e2e7c7641b6a/ReviewScreenshots/Hannoki/38701213882/icon.png) | `a699d6c88c81f7eea66fb723602066136e8ad36d7df78918720bafae60d07671` |
| autumn | ハンノキの本番葉／共用fall LUTを強度1に設定した秋色 | [autumn.png](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/ea7e480a8191f774c31fdb386fc5e2e7c7641b6a/ReviewScreenshots/Hannoki/38701213882/autumn.png) | `374fa13cf57367cacba4e4437b2a5d2089e441d8b0cf100e491c6be1403683b7` |
| leafless | テスト一時fixtureでPlant.MakeLeafless(Cold,false)を適用 | [leafless.png](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/ea7e480a8191f774c31fdb386fc5e2e7c7641b6a/ReviewScreenshots/Hannoki/38701213882/leafless.png) | `50630cf194b6cb73344bd0f6c059546c50a49109a40c8eb9813ffc9350158f42` |
| snow | 有葉／落葉、雪深0・0.5・1.0の本番スノーオーバーレイ | [snow.png](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/ea7e480a8191f774c31fdb386fc5e2e7c7641b6a/ReviewScreenshots/Hannoki/38701213882/snow.png) | `c3090210f5da0bc6b0b6646e4b5d95b72a48a696a4693fcf548564b115a4b1a8` |

## 今後の扱い

- 自然な紅葉・落葉・回復の発生時期・連続性は、今回の撮影では
  未実証。実ゲームで確認するため台帳の `transition` のみ保留する。
- 表示画像そのものを作り直す計画はない。作者が通常プレイで
  気づいた問題があれば、その時点で限定的に再検討する。
- 仮想撮影・画像拡大の視認性に限界があったことを残し、
  「画像が大きければレビュー精度が上がる」とは推定しない。
- `Docs/PlantVisualCoverage.json` の採用指紋は、
  配布候補の本番 `AMJ_WildPlants.xml` とすべての該当PNGから計算した。
  同じ手順で検算したブナ既存指紋が台帳と完全一致することを確認。
