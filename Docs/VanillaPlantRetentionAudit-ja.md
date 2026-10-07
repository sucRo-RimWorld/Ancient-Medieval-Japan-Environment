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

## Phase 3 — 亜高山帯の汎用Pineとシラビソ密度補正

対象: `Plant_TreePine`

判定: **亜高山帯から削除**

理由:
- 本州の亜高山帯自然林はシラビソ・オオシラビソ・コメツガ等の常緑針葉樹が主要構造で、AMJEではその代表を `AMJ_Tree_Shirabiso` が所有する。
- Vanillaの汎用Pineはアカマツ系の二次林表現として冷温帯では残存余地があるが、亜高山帯の主要高木として重ねる必要はない。
- Pine 0.90を削るだけでは樹木比率が落ちるため、その0.90をシラビソへ移し2.60→3.50とする。

樹木 commonality:
- 変更前: 4.00（Shirabiso 2.60 + Pine 0.90 + Birch 0.50）
- 変更後: **4.00**（Shirabiso 3.50 + Birch 0.50）
- 総wildPlants commonality 16.14も不変。

実装:
- `AMJ_SubalpineForest/wildPlants` から `Plant_TreePine` を除外。
- `AMJ_Tree_Shirabiso` を3.50へ増加。
- `Plant_TreeBirch` はダケカンバ相当の副次高木として暫定残存。
- 亜高山帯QuickstartでPine自然生成数0を回帰条件にする。

## Phase 4 — 高山帯の汎用高木・花の除外

対象:
- `Plant_TreePine`
- `Plant_TreeBirch`
- `Plant_Dandelion`
- `Plant_Astragalus`

判定: **高山帯から削除**

理由:
- `AMJ_AlpineZone` は森林限界より上を表すため、通常の高木Pine/Birchを低率でも常設する必要はない。高山帯の木本構造はハイマツが担う。
- Vanilla Dandelionは種を特定しない汎用タンポポで、現代日本で広く見られるセイヨウタンポポは20世紀初頭の移入記録がある。AMJEの高山植生をこのDefで代表させる積極的理由はない。
- Vanilla Astragalusも「alpine climates」の汎用属表現に留まり、日本の対象時代・対象高山植生をこのDefで代表させる根拠が弱い。必要なら日本固有・在来の高山草本を別途追加する。
- 高山帯では普通の高木を補充しないこと自体が設計目的。ただし植生候補総量と木本候補比率が削除だけで落ちないよう再配分する。

commonality再配分:
- Pine 0.02 + Birch 0.02 → Haimatsuへ移し1.30→**1.34**。
- Dandelion 0.20 + Astragalus 0.20 → Grass +0.20 / Moss +0.20。
- 総wildPlants commonalityは**7.31のまま不変**。
- 木本 commonalityもHaimatsu込みで**1.34のまま不変**。ただし通常高木は0となり、森林限界を明確化する。

実装:
- 上記4 Vanilla Defを `AMJ_AlpineZone/wildPlants` から除外。
- `AMJ_Shrub_Haimatsu` を1.34、Grassを2.20、Mossを3.20へ補正。
- 高山帯Quickstartで4 Defの自然生成数0を回帰条件にする。

## 未処理候補

以下は後続監査候補。複数種をまとめて監査・実装してよいが、削除・置換のコミットは履歴を追いやすい論理単位に分ける。ここに列挙されているだけでは削除決定ではない。

- `Plant_HealrootWild` — 削除単独ではなく、日本の野生薬草への置換と同時に扱う

