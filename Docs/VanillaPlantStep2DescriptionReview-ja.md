# Vanilla植物 Step 2：残存・日本語説明の再監査

**2026-10-08 — 竹の日本語説明のみ作者承認済み（英訳・実装候補）。ほか5樹木は日本語草案・未承認。植生分布・画像・ゲームプレイは未変更。**

対象はAMJEが現在採用するVanilla植物であり、Vanilla植物全体を日本向けと見なすものではない。自然生成は`Defs/BiomeDefs/AMJ_Biomes.xml`および`Patches/VanillaWetlandVegetation.xml`を正本とする。下記数値は`wildPlants`のcommonalityで、史実の構成比や植林の可否を示さない。

## 現在のVanilla樹木6種の配置

| PlantDef | 暖温帯 | 冷温帯 | 亜高山 | 高山 | 温帯湿地 | 冷涼湿原 |
|---|---:|---:|---:|---:|---:|---:|
| `Plant_TreeBamboo` | 0.50 | — | — | — | — | — |
| `Plant_TreeMaple` | 0.25 | 0.80 | — | — | 1.00 | 0.60 |
| `Plant_TreeOak` | — | 0.60 | — | — | — | — |
| `Plant_TreeBirch` | — | 0.60 | 0.50 | — | — | 0.60 |
| `Plant_TreePine` | — | 0.35 | — | — | — | — |
| `Plant_TreeWillow` | — | — | — | — | 2.00 | 0.60 |

4独自Biomeの植林候補は`Docs/Design.md`および`Docs/GoldenPaths/PlantSowingTests.md`の契約を維持する。湿地の植林可否は自然配置表だけでは判定できない。AMJE独自のスダジイ・ブナ・シラビソ・ハイマツは、この既存Vanilla樹木監査の対象外。Poplar・Cypressなど採用外樹木を「Vanillaにあるから」と復活させない。

## 残るgeneric植物7種（暫定分類）

- `Plant_Grass`：汎用の草本。特定の古代・中世日本の草種とは断定しない。
- `Plant_TallGrass`：背の高い草本。ヨシやスゲとの同一視を避ける。
- `Plant_Brambles`：genericな茂み。日本固有の低木種には固定しない。
- `Plant_Bush`：genericな低木。種の主張はしない。
- `Plant_Moss`：コケ類。湿原ミズゴケと同一視しない。
- `Plant_Berry`：ゲーム内の採集用generic低木。日本の具体的な果実種は未特定。
- `Plant_HealrootWild`：**将来的に除外する暫定植物**。日本向け薬草の採集経路が完成する前に削除しない。

汎用植物は現段階で新しい日本種へ無条件に読み替えない。日本固有の湿地植物・薬草追加はロードマップStep 3以降で検討する。

## 日本語説明（竹のみ承認済み／他5件は未承認草案）

既存ラベルは変更しない。竹は作者が2026-10-08に下記2段落を承認済み。ほか5樹木の日本語草案は引き続き承認待ちであり英訳・ゲーム内説明は変更しない。歴史的な利用を記した場合も、新しい生産レシピの実装を意味しない。

### Plant_TreeBamboo — 竹（タケ）

**日本語：作者承認済み（2026-10-08）、以下の2段落を固定。**

> 竹（タケ）は、日本の温暖で湿潤な土地に群生する、木のような姿のイネ科植物。古代・中世に存在したマダケやハチクなどの竹類を、ここではまとめて表す。
>
> 竹は軽くしなやかで、古くからかご・日用品や建築・農漁業の資材に使われてきた。現在広く見られるモウソウチクは江戸時代の渡来とする記録があり、ここで想定する竹類とは区別する。

**English（日本語確定後の対応翻訳）**

> Bamboo (take) is a member of the grass family that grows in stands in Japan's warm, humid regions, despite its tree-like appearance. Here it represents bamboo types such as madake and hachiku that existed in ancient and medieval Japan.
>
> Lightweight and flexible, bamboo has long been used for baskets, everyday objects, and materials for building, farming, and fishing. Moso bamboo, common in Japan today, is recorded as having arrived during the Edo period and is distinguished here from these earlier bamboo types.

**実装:** `Patches/VanillaTreeDescriptions.xml` で `Plant_TreeBamboo.description` の英語だけを置換し、`Languages/Japanese/DefInjected/ThingDef/AMJ_WildPlants.xml` の `Plant_TreeBamboo.description` に承認済み日本語を同期する。原文と英訳の段落境界はXMLのリテラル `\\n\\n` で保持する。Vanillaラベル・伐採資源・分布・植林・PlantDef・画像は変更しない。暖温帯のQuickstartにロード済み日英本文の一致検証を追加し、**2026-10-08に作者提供の `EnvironmentIsolatedRuntime.log` 末尾で、更新後のEnvironmentランタイムゲートPASSが確認された。** 暖温帯シナリオ個別のJSON・詳細アサーション一覧は未添付のため、竹説明文アサーション1件のPASSをログから独立集計したとまでは主張しない。

**留保：** 種同定・現行画像は要確認。Vanillaの「not beautiful」という主観的評価は不採用。新たな竹加工レシピは未実装。

### Plant_TreeMaple — 楓（カエデ）

> 楓（カエデ）は、日本の落葉広葉樹林にも生育するカエデ属の樹木をまとめた呼び名。多くは季節によって葉の色を変える。
>
> 縄文時代の遺跡周辺にもカエデ属を含む森林が確認されており、古くから日本列島の森林を構成していた。この植物は特定のカエデ一種を再現するものではない。

**留保：** 温暖帯・両湿地での採用量は種同定や生態との整合を引き続き監査。

### Plant_TreeOak — 楢（ナラ）

> 楢（ナラ）は、コナラなど日本に分布する落葉性のナラ類を表す樹木。ブナなどとともに落葉広葉樹林の一部をなす。
>
> 日本列島では古くからナラ類を含む森林が広がり、地域ごとに人の火の利用や森林利用の影響を受けてきた。ここでは落葉ナラ類の代理とし、常緑カシ類まで同一視するものではない。

**留保：** 暖温帯の常緑カシや未実装のドングリ採集を含意しない。

### Plant_TreeBirch — 樺（カバ）

> 樺（カバ。カバノキ類）は、冷涼な山地にも分布する落葉高木の仲間。亜高山帯にはダケカンバなどが、針葉樹林に接する斜面にも見られる。
>
> この植物は冷温帯のカバノキ類と、亜高山帯のダケカンバに相当する樹木をまとめて表す。実際の種類は地域や標高によって異なる。

**留保：** ColdBog内の実際の湿地構成種としての妥当性は追加監査。樹皮などの歴史的用途は未確認。

### Plant_TreePine — 松（マツ）

> 松（マツ）は、日本の山地や痩せた土地にも育つ針葉樹の仲間。ここではアカマツなどのマツ類を代表する樹木として扱う。
>
> 古代から中世にかけて、地域によっては伐採や燃料利用に伴ってマツ類が増加したことが花粉分析からうかがえる。全国の森林が一律にマツ林だったわけではない。

**留保：** 二次林の成立と自然Biome分布は区別。京都盆地の事例を全国に一般化しない。

### Plant_TreeWillow — 柳（ヤナギ）

> 柳（ヤナギ）は、川沿いや湿った地面に育つヤナギ類を表す樹木。日本各地の川原では、氾濫や流水の影響を受ける場所にヤナギ林が見られる。
>
> 河川沿いの湿地林は、集落が接する低地景観を理解するうえでも重要な自然環境である。この植物は特定のヤナギ一種を表すものではない。

**留保：** 現代の河川改修後の分布を古代・中世の植生と直接同一視しない。

## 湿地に残る樹木の追加照合（2026-10-08、分布変更は未決定）

**監査対象:** `TemperateSwamp` の Willow 2.00 / Maple 1.00、および `ColdBog` の Willow / Birch / Maple 各 0.60。これは現在の `Patches/VanillaWetlandVegetation.xml` の数値であり、湿原中心部に各樹木が均一に生育するという史実上の主張ではない。Biome 全体の `wildPlants` は、河岸・微高地・林縁などの立地差を種別に指定する表ではない。

- **Willow:** 日本の河川沿いで冠水しやすい岸辺や湿った河岸にヤナギ類の林が成立する。湿地・河畔を表すVanilla代理として残す積極的根拠がある。ただし高層湿原中心部の代表樹種にはしない。
- **Maple:** 日本の北方河川にはエゾイタヤ等を含む河畔の広葉樹林があり、湿地に隣接する沢沿い・比較的乾いた場所の代理としては扱える。ただし `ColdBog` の高層湿原や泥炭地全面をカエデ林と見なす根拠はない。採用量と出現位置の妥当性は実機地形サンプルとStep 3植生追加時に再評価する。
- **Birch:** シラカバ・ダケカンバ等は冷涼な湿原に隣接する林や孤立林の代理にはなるが、日本の低層湿原で重要な**ハンノキ（Alnus）**そのものではない。Vanilla Birchをハンノキの同種代理として説明したり、樹木commonality保全だけで湿地植生の完成と見なしたりしない。
- **不足種:** 低層湿原ではヨシ・スゲ・ハンノキ、高層湿原ではミズゴケ等が重要。これらは既定のStep 3追加候補であり、既存Vanilla樹木による完全な再現を主張しない。河岸、湿原縁辺、湿原中心部の差は後続の植生設計で扱う。

**暫定判断:** 上記は既存の「残す理由」の範囲・限界を明確化する調査記録であり、今の段階で植物の除外・commonality再配分・植林候補変更・説明文承認・リテクスチャ着手を決めるものではない。特に現行Wetlandの木本commonality保全（TemperateSwamp 3.00 / ColdBog 1.80）と、追加候補のAlnus等との両立を、将来の変更時に同時検討する。

追加参照（現在の植生から過去の全時期への直接外挿はしない）:
- 環境省・釧路湿原自然再生「湿原と植生概要」 https://kushirodata-center.env.go.jp/wetland/wetland_article2_9.html
- 環境省「赤名湿原」（ハンノキ・スゲ） https://www.env.go.jp/nature/important_wetland/wetland/w376.html
- 環境省・サロベツ周辺の林縁と湿原環境 https://hokkaido.env.go.jp/blog/rishiri/a-wakkanai/index_15.html
- 国土交通省・荒川のヤナギ林 https://www.ktr.mlit.go.jp/arajo/arajo00046.html
- 国土交通省・渚滑川の河畔林 https://www.mlit.go.jp/river/toukei_chousa/kasen/jiten/nihon_kawa/0101_shokotsu/0101_shokotsu_04.html

## 原典・参照資料

- 農林水産省：竹の種類と由来 https://www.maff.go.jp/j/pr/aff/2103/spe1_01.html
- 林野庁：主な竹の種類 https://www.rinya.maff.go.jp/j/tokuyou/take/syurui.html
- 文化庁：竹工芸 https://kunishitei.bunka.go.jp/heritage/detail/303/92
- 鈴木・能城（1997）：縄文時代の森林植生と木材利用 https://doi.org/10.4116/jaqua.36.329
- 佐々木ほか（2011）：京都盆地の里山林とマツ属花粉 https://doi.org/10.57466/chikyukankyo.16.2_115
- 林野庁：ダケカンバ群落 https://www.rinya.maff.go.jp/j/kokuyu_rinya/kakusyu_siryo/pdf/00240_2_h3_006.pdf
- 傳甫ほか（2008）：日本の河畔ヤナギ林の地域分布 https://doi.org/10.3825/ece.11.13

## 次の工程

1. **竹の日本語本文は承認済み。** 英語Patch・日本語DefInjected・静的テストと暖温帯Quickstart検証を実装済み。作者提供の実機ランタイムゲート末尾はPASS（`[OK] Environment runtime gate passed.`）。対応する個別JSON未取得のため、正確な竹アサーション結果の独立監査は保留する。
2. 残る5樹木の種代理と湿地配置について検討し、必要なら植生配分・植林設定を連動して再設計する。
3. **残り5樹木は日本語承認後だけ**英訳・Vanilla `ThingDef.description` のXML Patch・日本語DefInjected・ロード後テストを実装する。竹だけを先行実装する。
4. 現在の既存樹木画像のリテクスチャ保留を維持する。
