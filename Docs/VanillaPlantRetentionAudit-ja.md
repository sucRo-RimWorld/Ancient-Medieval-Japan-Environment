# Vanilla植物残存監査 — 2026-10-08

## 目的

AMJEは「Vanillaの植生へ日本要素を追加するMod」ではなく、古代〜中世日本を想定した自然環境へ植生構成を置き換え・再構成するModである。

したがって、Vanilla植物は「元からあるから残す」のではなく、次の条件を満たす場合だけAMJEのBiomeに残す。

1. 日本列島に在来または対象時代までに定着している植物として無理なく解釈できる。
2. 配置するAMJE植生帯の生態に合う。
3. AMJE独自植物ですでに役割が置き換わっていない。
4. Vanillaの名称・外観・説明をそのまま使っても別地域・別時代の植物に強く見えない、または後続のAMJE説明／リテクスチャで同一Defの範囲内に補正できる。
5. 「ゲーム上あると便利」だけでは残存理由にしない。

根拠が弱いものは一旦分布から外し、必要なら日本種のAMJE専用Defとして後から追加する。

## 初回監査結果

| Vanilla Def | 監査前のAMJE帯 | 判定 | 現在の扱い | 理由 |
|---|---|---|---|---|
| `Plant_Grass` | 全帯 | 残す | 継続 | 種を固定しない地表草本として扱え、日本の在来イネ科・カヤツリグサ科等の草地を表現する汎用役割がある。 |
| `Plant_TallGrass` | 暖温帯・冷温帯 | 残す | 継続 | 種を固定しない高茎草本として日本の草地・林縁に無理なく使える。 |
| `Plant_Brambles` | 暖温帯・冷温帯・亜高山 | 残す | 継続 | Rubus類を含む林縁性の棘性低木という汎用表現として利用可能。 |
| `Plant_Moss` | 冷温帯・亜高山・高山 | 残す | 継続 | 苔類の汎用地表表現であり、特定の外来種を指さない。 |
| `Plant_Bush` | 全帯 | 残す | 継続 | 種を固定しない低木枠として利用可能。後続の外観監査対象。 |
| `Plant_Berry` | 全帯 | 残す | 継続 | 種を固定しない野生果実低木として利用可能。RawBerriesは種同定ではなくゲーム上の収穫物カテゴリとして扱う。 |
| `Plant_TreeBamboo` | 暖温帯 | 残す | 暖温帯のみ | 日本には在来のSasa類を含む竹・笹類が存在する。Vanillaの「美しくない」という説明は文化的に不適切なため別途AMJE形式へ書き直す。 |
| `Plant_TreeMaple` | 暖温帯・冷温帯 | 残す | 継続 | 日本在来Acer類が多く、暖温帯〜冷温帯の森林に対応可能。説明・外観は日本種として成立するか後続監査する。 |
| `Plant_TreeOak` | 暖温帯・冷温帯 | 一部残す | **暖温帯から除外、冷温帯のみ** | 日本にはQuercus serrata等の在来ナラ類があるため冷温帯の落葉広葉樹としては使える。一方、暖温帯のAMJE主構造はスダジイを中心とする照葉樹林であり、Vanillaの典型的な落葉性oakを副構造として残す積極的理由が弱い。 |
| `Plant_TreePoplar` | 暖温帯 | 外す | **AMJE全帯から除外** | 日本在来のドロノキ等は存在するが、北方・攪乱地等の文脈が強く、現在の暖温帯配置は不適切。必要なら将来、河畔・北方植生として別途設計する。 |
| `Plant_TreePine` | 冷温帯・亜高山・高山 | 一部残す | **冷温帯のみ** | アカマツ等の日本在来Pinusは温帯に適合する。一方、亜高山帯はAMJE独自のシラビソ、高山帯はハイマツが構造植物を担当するため、汎用Vanilla pineを重ねる理由がない。 |
| `Plant_TreeBirch` | 冷温帯・亜高山・高山 | 一部残す | **冷温帯・亜高山のみ** | ダケカンバ等、日本在来Betulaは冷温帯〜亜高山で妥当。AMJ_AlpineZoneは森林限界より上を表すため、通常高木のVanilla birchは除外する。 |
| `Plant_Dandelion` | 高山 | 外す | **AMJE全帯から除外** | 日本在来タンポポ類は存在するが、Vanilla Defは種を区別しない。セイヨウタンポポは日本への最初の記録が1904年で対象時代外。高山帯の代表草本としてVanillaの曖昧なDefを残す必要はなく、必要なら日本在来種を専用Defで追加する。 |
| `Plant_Astragalus` | 高山 | 外す | **AMJE全帯から除外** | Vanillaは「alpine climatesの多年草」という汎用設定で、日本の高山植生として特定の根拠がない。日本で広く知られるレンゲ（Astragalus sinicus）は外来で、記録も近世以降。必要なら日本在来の高山草本を専用Defとして設計する。 |
| `Plant_HealrootWild` | 全帯 | 外す | **AMJE全帯から除外** | RimWorld固有の架空薬用植物であり、古代〜中世日本の自然植生を再構成するAMJEのwild plantとして残す理由がない。薬草ゲームプレイが必要なら日本の薬用植物を別途設計する。 |

## 植生帯ごとの現在のVanilla残存植物

### 暖温帯林

- Plant_Grass
- Plant_TallGrass
- Plant_Brambles
- Plant_Bush
- Plant_TreeMaple
- Plant_TreeBamboo
- Plant_Berry

構造植物の中心はAMJ_Tree_Shii。

### 冷温帯林

- Plant_Grass
- Plant_TallGrass
- Plant_Brambles
- Plant_Moss
- Plant_Bush
- Plant_TreeOak
- Plant_TreeMaple
- Plant_TreeBirch
- Plant_TreePine
- Plant_Berry

構造植物の中心はAMJ_Tree_Beech。

### 亜高山帯林

- Plant_Grass
- Plant_Moss
- Plant_Brambles
- Plant_Bush
- Plant_TreeBirch
- Plant_Berry

構造植物の中心はAMJ_Tree_Shirabiso。

### 高山帯

- Plant_Grass
- Plant_Moss
- Plant_Bush
- Plant_Berry

構造植物の中心はAMJ_Shrub_Haimatsu。通常高木は置かない。

## 後続監査

今回の監査は「分布に残すか」の初回ゲートであり、残したVanilla Defを無条件に最終採用したことを意味しない。

残した植物は次の順で追加監査する。

1. Vanillaの実画像・全graphic stateを確認し、日本の対応種として不自然でないか判定。
2. 日本語名称・説明をAMJE Historical Description形式へ書き直す。
3. 同一Defの範囲で日本向けに補正可能ならAMJE所有リテクスチャを作る。
4. 種同一性が変わるほどの補正が必要ならVanilla Defを残さず、日本種の専用Defへ置換する。
5. MO併用時の植物も同じ基準で別途監査する。

## 参考資料

- Kew Plants of the World Online, Quercus serrata: https://powo.science.kew.org/taxon/urn:lsid:ipni.org:names:60444831-2
- Kew Plants of the World Online, Acer palmatum: https://powo.science.kew.org/taxon/urn:lsid:ipni.org:names:927504-1
- Kew Plants of the World Online, Betula ermanii var. ermanii: https://powo.science.kew.org/taxon/urn:lsid:ipni.org:names:77167714-1
- Kew Plants of the World Online, Pinus densiflora: https://powo.science.kew.org/taxon/urn:lsid:ipni.org:names:262894-1
- Hokkaido University, Populus maximowiczii: https://hosho.ees.hokudai.ac.jp/tsuyu/top/plt/willow/populus/max.html
- Kew Plants of the World Online, Sasa nipponica: https://powo.science.kew.org/taxon/urn:lsid:ipni.org:names:420410-1
- J-STAGE, Japanese native Taraxacum taxonomy review: https://www.jstage.jst.go.jp/article/cytologia/86/2/86_D-21-00023/_html/-char/ja
- 国立環境研究所 侵入生物DB, Taraxacum officinale: https://www.nies.go.jp/biodiversity/invasive/DB/detail/80640e.html
- 国立環境研究所 侵入生物DB, Astragalus sinicus: https://www.nies.go.jp/biodiversity/invasive/DB/detail/80930e.html

