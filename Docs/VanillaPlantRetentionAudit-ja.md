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

## Phase 2 — 暖温帯林のOakと樹木量補正

対象: `Plant_TreeOak`

判定: **暖温帯林から削除**

理由:
- 日本の暖温帯自然林には常緑カシ類も含まれるが、Vanillaの `Plant_TreeOak` は温帯林用の汎用落葉広葉樹として扱われており、AMJE暖温帯林の照葉樹構造を表す役としては不適切。
- 暖温帯林の主構造はAMJE所有のスダジイへ寄せ、カシ類を将来明示する必要が生じた場合は日本向けの種・表示として別途設計する。
- Oak削除だけで樹木候補比率を落とさない。先に削除済みのPoplar 0.15と今回のOak 0.40を合計した0.55をスダジイへ戻し、スダジイ commonality を2.00→2.55とする。

樹木 commonality:
- Poplar削除前: 3.30
- Phase 1後: 3.15
- Phase 2後: **3.30**
- 総wildPlants commonalityもPoplar削除前の13.42へ戻るため、Vanilla樹木整理だけを理由に暖温帯林の樹木量を減らさない。

実装:
- `AMJ_WarmTemperateForest/wildPlants` から `Plant_TreeOak` を除外。
- `AMJ_Tree_Shii` を2.55へ増加。
- 暖温帯QuickstartでOak / Poplarの自然生成数0を回帰条件にする。

## 未処理候補

以下は後続監査候補。複数種をまとめて監査・実装してよいが、削除・置換のコミットは履歴を追いやすい論理単位に分ける。ここに列挙されているだけでは削除決定ではない。

- 亜高山帯の `Plant_TreePine`
- 高山帯の `Plant_Dandelion`
- 高山帯の `Plant_Astragalus`
- 高山帯の `Plant_TreePine`
- 高山帯の `Plant_TreeBirch`
- `Plant_HealrootWild` — 削除単独ではなく、日本の野生薬草への置換と同時に扱う

