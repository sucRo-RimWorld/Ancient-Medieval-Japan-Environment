# Vanilla植物残存監査

AMJEのVanilla植物は、「Vanillaに存在するから残す」のではなく、古代〜中世日本の対象植生帯に残す積極的理由がある場合だけ採用する。

## 段階的削除

### Phase 1 — 暖温帯林のPoplar

対象: `Plant_TreePoplar`

判定: **削除**

理由:
- AMJEの暖温帯林はスダジイを中心とする日本の照葉樹林を基準とする。
- 日本在来のPopulus属そのものを否定するものではないが、現在の汎用Vanilla Poplarをこの帯の副構造樹木として置く積極的理由がない。
- 河畔林や北方のPopulus系植生が将来必要になった場合は、適切な日本種・分布条件を別途設計する。

実装:
- `AMJ_WarmTemperateForest/wildPlants` から `Plant_TreePoplar` を除外。
- 暖温帯Quickstartで自然生成数0を回帰条件にする。
- 他のVanilla植物はこのPhaseでは変更しない。

## 未処理候補

以下は後続監査候補。複数種をまとめて監査・実装してよいが、削除・置換のコミットは履歴を追いやすい論理単位に分ける。ここに列挙されているだけでは削除決定ではない。

- 暖温帯の `Plant_TreeOak`
- 亜高山帯の `Plant_TreePine`
- 高山帯の `Plant_Dandelion`
- 高山帯の `Plant_Astragalus`
- 高山帯の `Plant_TreePine`
- 高山帯の `Plant_TreeBirch`
- `Plant_HealrootWild` — 削除単独ではなく、日本の野生薬草への置換と同時に扱う

