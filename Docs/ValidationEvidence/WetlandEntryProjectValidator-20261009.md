# Project共通Validatorによる湿地4種実装必要性監査（2026-10-09）

**判定:** **PASS（正式Python CLI実行済み）**。これはProjectの独自実装必要性・既存Mod監査の**構造／証拠バイト照合のみ**。新PlantDefの受け入れ、歴史説明文の作者承認、画像の作者承認、収量数値、実ロード／描画／採取、MOやDLC互換、既存セーブ適合の判定ではない。

| 証拠 | 実際に実行・照合したもの |
|---|---|
| Project正本 | `sucRo-RimWorld/Ancient-Medieval-Japan-Project` の `Scripts/validate_rule_contract.py` |
| Project実行経路 | [PR #7](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/7)、[workflow（Project）](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/.github/workflows/pinned-amje-wetland-decision.yml) |
| Project統合SHA | `3d73b8e6a1b53c555e65e125716cf944af874ca0` |
| Environment監査対象SHA | `41db6dc142478917e28d0b1510d3219399bca85a`（当該4種判定JSON／既存Mod監査を含む実コミット） |
| 実行コマンド | `python project/Scripts/validate_rule_contract.py --owner-root environment --decision environment/Docs/Research/WetlandPlantImplementationDecision.json` |
| 実行日時 | 2026-10-09 14:53 UTC（GitHub Actionsログ記録） |
| 確認したCI | [Project PRテスト run #37947510648](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/37947510648)（success）、統合後の同一ワークフロー（success） |
| CLI標準出力 | `Decision structure/evidence bytes PASS; substantive research and authorization still required.` |

**範囲固定:** この結果は上記Environment SHAに固定。Environmentの次の未承認PlantDef・画像・日本語説明・採取ロジックの追加が自動で承認されたことにはならない。今後、比較対象や独自実装の前提が実質的に変わった場合、正式判定JSONを更新し、同じProject実スクリプトを**変更後の正確なコミット**に対して再度実行する。

**監査資料とJSONのハッシュ正本は固定のまま維持:** `Docs/Research/WetlandPlantExistingModAudit-ja.md`と`Docs/Research/WetlandPlantImplementationDecision.json`は相互のSHA256を含むため、監査実行完了だけを理由に監査原本を書き換えず、判定実行証跡は本書と`Docs/NativeVegetationStep3Design-ja.md`へ保持する。
