# Step 3：日本の不足植生（薬系をProjectへ移管）

**2026-10-08最新作者決定：Environmentはヒールルートと既存薬草供給を維持する。薬用植物・葛根・生薬・加工薬は独立薬系Modへ分離。研究・承認文正本：[Project薬系企画](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/MedicinalPlantsAndRemedies.md)。本書の旧薬草候補・Harvest案・Healroot撤去条件は失効。湿地植生の設計・承認ゲートは維持する。**

## 1. 所有範囲・先行工程

- 所有者は **AMJ Environment**。対象は古代～中世日本に対応する自然植生と野生植物の分布、表示、野生採集を含む場合の基本生態。江戸期の技術・文化を実装範囲としない。
- 正本のロードマップは `Docs/Design.md` 11.5.7、既存種残存とHealroot除外条件は `Docs/VanillaPlantRetentionAudit-ja.md`、湿地の現行配分は `Patches/VanillaWetlandVegetation.xml`、4独自Biomeの現行配分は `Defs/BiomeDefs/AMJ_Biomes.xml`。
- Step 1 湿地生態・Step 2 既存Vanilla樹木説明は作者報告のゲートを経て完了。**既存樹木リテクスチャは作者の指示どおり保留**。新規植物の表示状態確認は別の新規アート工程。
- 栽培／新規食材／屋根葺材／加工設備／採集Job等の大きなゲームプレイループは、Environmentだけで勝手に追加しない。新素材やレシピは、実装時点の所有Mod（将来の採集・建築・加工等）へ渡し、Environmentがその完成を先取りして説明しない。
- 日本語説明を**先に作者承認**してから英訳する。以下の候補文を英訳・Defに配置してはならない。

## 2. 現在の不足と先行Mod監査

| 現行の代理 | 足りない日本の植生 | 意図しない同一視 |
|---|---|---|
| `Plant_TallGrass` | ヨシ、湿地のスゲ類 | TallGrass をヨシ・スゲという特定種／属と断定しない |
| `Plant_Moss` | 泥炭性のミズゴケ類 | 通常のコケを高層湿原のミズゴケと同一視しない |
| `Plant_TreeWillow`・`Plant_TreeBirch`・`Plant_TreeMaple` | 低層湿原のハンノキ林 | Birch は Alnus ではない。河岸・縁辺林と湿原中心部を混同しない |

**VE最優先の機能重複監査（暫定）：** Vanilla Plants Expanded（Workshop 2134308522）は公開説明上、果樹・作物の追加と農業選択肢を主軸とする。既存農作物・雑草等の発想は参考にする一方、日本の湿地のヨシ／スゲ／ハンノキ／ミズゴケ自然群落をそれだけで満たすとは確認できない。**前提化しない／実装参考・任意互換候補**。MOおよび同Modのロード済みDefとの名称・画像・Harvest重複は、実装PRで実Defを確認する。既存Modの主要内容を大幅削除して前提化しない。

資料：環境省・釧路湿原国立公園はヨシ・スゲ湿原、ハンノキ林とミズゴケ高層湿原を区別。環境省「赤名湿原」はハンノキ・カサスゲを記載。文化庁「ふるさと文化財の森」はヨシを伝統的な草葺き材に含める。これら**現代植生資料から古代～中世の各地の割合・作業技術をそのまま外挿しない**。

### Odyssey DLC重複・非所持プロファイルの扱い（2026-10-09、先行調査）

- RimWorld Wikiの公開情報で、**Reeds（ヨシに景観・水辺機能の重複がある）**と**Bulrush（ガマ型の水生草本）**はOdyssey DLC専用と確認できる。Reedsは泥・湿地・浅い淡水、Bulrushは湿地・浅い静水に現れると記載されている。**Bulrushはスゲ類と同一種ではない**。公開情報での類似は、AMJが予定する日本のヨシ・スゲ・ハンノキ・ミズゴケをすべて代替できる根拠にはならない。
- 提供されたMedieval Overhaul 1.6ソースアーカイブの `1.6/Defs/Biomes/Biomes_DarkForest.xml` には、`Plant_Reeds`（commonality 1）と`Plant_Bulrush`（0.2）がどちらも `MayRequire="Ludeon.RimWorld.Odyssey"` 付きで定義されている。**DLCなしではロードされない条件付きエントリであり、MO単独でReeds/Bulrushが利用できる根拠にはならない。** これは手元アーカイブのXML静的調査のみで、MO＋Odyssey実機テストではない。
- 作者の検証環境にOdyssey DLCがなく、当面はDLC有効状態でのロード済みDef・生育地形・出現分布・テクスチャ・パッチ競合を実機検証できない。**DLCを必須化しない**。DLC未導入のVanilla基準（任意MO/CCTO共存を含む）を主たる実装・回帰ゲートとする。DLC専用Def・画像・参照を通常の配布ファイルへ無条件に追加しない。
- 将来Odyssey互換に着手する際は、実際の`Plants_Water.xml`等の版固定したDef、`wildPlants`再配分経路、Odysseyの水域植物生成経路、同種景観の重複量を確認する。特にAMJEの`Patches/VanillaWetlandVegetation.xml`は湿地の`wildPlants`全体を置換するため、Odyssey由来エントリを保持しているとは**未確認**。DLC向け調整は`Ludeon.RimWorld.Odyssey`条件の任意パッチとして独立させ、実機証拠がない間は互換済み・PASSと表示しない。
- 上記は公開情報を使った**部分的な重複監査**であり、独自植物の必要性を確定する新機能実装エントリ審査ではない。VEと非VEの現行Mod候補、実XML/コード・ライセンス・依存を別途比較し、Project `Docs/Research/ExistingModAudit.md` の決定記録ゲートを完了するまで、新PlantDef実装は保留する。承認済み日本語説明・実画像が揃うまで英訳や出荷も行わない。

公開参照：[Reeds](https://rimworldwiki.com/wiki/Reeds)、[Bulrush](https://rimworldwiki.com/wiki/Bulrush)、[Odyssey DLC公式発売案内](https://ludeon.com/blog/2025/07/the-rimworld-odyssey-expansion-is-out-now/)、[任意DLC条件 `MayRequire`](https://www.rimworldwiki.com/wiki/Modding_Tutorials/MayRequire)。

## 3. Step 3 候補・実装順（最終配分値は未決定）

| 順位 | PlantDef候補（名前は暫定） | 種・機能範囲 | 主な適用Biome候補 | 分布・互換の留保 |
|---|---|---|---|---|
| 3A-1 | `AMJ_Plant_Yoshi` | 葦・ヨシ（アシ）、河岸・低層湿原の高草丈草本 | TemperateSwamp、ColdBogの低層湿原側 | `Plant_TallGrass`の一部を置換。全水域・高層泥炭地に一様に置かない |
| 3A-2 | `AMJ_Plant_Suge` | スゲ類（特定の一種に固定しない）、湿った草地 | TemperateSwamp、ColdBog | `Plant_TallGrass`の一部を置換。ヨシと見分けられる外観が必要 |
| 3A-3 | `AMJ_Tree_Hannoki` | ハンノキ（Alnus）、低層湿原の落葉湿地林 | TemperateSwamp、ColdBogの適切な湿地林 | 既存Willow/Maple/Birchの木本配分から移す。木本総量維持と研究後植林候補を同時監査 |
| 3B-1 | `AMJ_Plant_Mizugoke` | ミズゴケ類、冷涼な泥炭湿原の蘚苔類 | ColdBogの高層湿原相当 | `Plant_Moss`の一部を置換。通常の木立と均一混在するような無条件配置に注意 |

**優先度は一括導入の承認ではない。** 3Aの最初の小さい実装単位はヨシ1種を想定し、専用テクスチャ・日本語本文承認・Def継承・地形適性・単独自然発生を確認してから拡張する。ヨシとスゲの違い、ハンノキの湿潤立地、ミズゴケの冷涼な泥炭地という区別を検証できなければ、機能を実装せず設計を再検討する。

### 3.1 Biome構成比・木本総量の保持

現在の `TemperateSwamp` は wildPlants commonality 合計 **7.30**、木本合計 **3.00**、`ColdBog` はそれぞれ **8.22 / 1.80**。この数値は**配分契約の現状基準**であり、古代・中世の実際の面積比率ではない。

- まず元代理のcommonalityを減らして新候補へ移す。候補を単純追加して植物総量や木本量を増やさない。
- ヨシ・スゲは主としてTallGrass側の一部配分。ミズゴケはColdBogのMoss側。ハンノキは木本側。採用済みWillow/Maple/Birchを機械的に全削除せず、前Step 2の「河岸・微高地・林縁」の用途を考慮する。
- 本工程で4独自Biomeに薬用植物を追加しない。将来の新規自然植生を検討する際も、既存草本総量・林床構造と四帯の違いを維持する。
- `wildPlants`は「河岸／湿原中心／微高地」を個別指定する装置ではない。**植物自身の生育地形・水域・肥沃度／地形適性と実際のマップ生成を評価**し、追加先のBiomeを選ぶ。必要なら現行の二類型内で制御可能な範囲を先に検証し、無理に精密な生態分布を実装したと主張しない。
- 湿地の地形・Worker・天候・動物・疾病の完成済みStep 1を破壊しない。新PlantDefの植林タグ／研究前後の植林メニューは意図せず拡大させない。

### 3.2 医療供給の維持

Environmentでは野生・栽培Healrootと既存MedicineHerbal供給を維持する。薬草の追加・置換・撤去・医療機能は本工程の対象外。薬系Modの独立設計へ移管する。

## 4. 日本語説明・レビュー用草案（湿地4種は未承認／英訳禁止）

名前・異名の読みや時代別の厳密な利用事例は、実装前に史資料と照合して作者承認を得る。以下の記述は**草案**であり、ゲーム内本文としては使用しない。

### 葦（ヨシ、アシとも呼ぶ）— `AMJ_Plant_Yoshi`

> 葦（ヨシ、アシとも呼ぶ）は、河川や湖沼の岸辺、湿った低地に群生する背の高い草本。水辺に広がるヨシ原は、日本の低層湿原を形づくる代表的な草地の一つである。
>
> ヨシを含む草は、古くから建物の草葺き屋根などに用いられてきた。ここでは水辺の自然植生を表し、屋根材の採取や加工ができることを意味しない。

### 菅（スゲ類）— `AMJ_Plant_Suge`

> 菅（スゲ）は、湿った草地や湿原などに多く生育する草本の仲間。日本各地の湿原には、スゲ類を主体とする草地が見られる。
>
> 低層湿原ではヨシ原やハンノキの林と隣り合って分布することがある。ここでは特定のスゲ一種ではなく、湿った土地に広がる草本群落をまとめて表す。

### 榛の木（ハンノキ）— `AMJ_Tree_Hannoki`

> 榛の木（ハンノキ）は、河川沿いや地下水位の高い湿地に生える落葉高木。水を多く含む低層湿原では、ヨシやスゲの草地と並んで湿地林をつくる。
>
> 日本の湿地林は、周囲の川筋や土砂、水位などによって広がり方が変わる。ここでは低層湿原の木立を代表させ、高層湿原の全面に生える樹木としては扱わない。

### 水苔（ミズゴケ類）— `AMJ_Plant_Mizugoke`

> 水苔（ミズゴケ）は、湿原の水を含んだ地表に群落をつくるコケ類。冷涼な地域では泥炭が発達した高層湿原を形づくる植物の一つとなる。
>
> 高層湿原は主として雨水によって潤され、河岸のヨシ・スゲ湿原やハンノキ林とは異なる景観をもつ。ここでは特定のミズゴケ一種を再現するものではない。

薬用植物の説明草案・承認済み本文はProject薬系企画へ移管済み。

## 5. 新規アート・技術設計の実装ゲート

- **DLC非導入を標準の成立条件とする。** 基本湿地プールはDLC固有の`Plant_Reeds`・`Plant_Bulrush`を参照せず、既存Healrootの全Biome供給を維持する。Odyssey同時導入時の重複・水域生育・`wildPlants`置換挙動は未試験なので、現時点ではOdyssey互換パッチを実装しない。

- 新PlantDefを生かすには対応する**実在・承認済みPNG**が先に必要。既存Vanilla/MOの樹木・雑草画像を、史実上不一致の日本種として無断流用しない。欠損texPathや仮画像でリリースしてはならない。
- 新規植物は既存 `Docs/GoldenPaths/RetextureGeneration.md` → `TextureAssetPipeline.md` → `Docs/PlantVisualCoverage.json` の手順へ追加する。**共通画風を既存の承認済み画像と現物比較**し、作者承認前のイメージを自動採用しない。
- ヨシ：細く長い葉と直立した稈・穂、スゲ：低めで葉が叢生、ハンノキ：落葉性の小～中型湿地樹冠、ミズゴケ：水際地面を覆う低いコケ状。**作画指示案であって完成画像ではない**。
- 画像状態は成熟／未熟／アイコン／積雪を基本とする。ハンノキは落葉／秋色／季節移行も別検証。新種の植生・描画・雪・成長のコストと現行アート完成状態を混同しない。
- 第一実装PRは候補1種ごとにDef構造、地形適性、自然発生数・比率、画像読込BadTex検出、表示状態、植林候補の意図的非拡張、バニラ／MO任意共存を試験する。日本語承認後の英訳・DefInjectedの一致を検証する。
- 自動テストは `run-tests.bat` の非表示RimWorld描画経路で行い、所有ERRORゼロ・完全ログを要求。既存の暖温帯／冷温帯／亜高山／高山／湿地2種／河川／海岸／自然湿地世界サンプルの回帰を維持。別の Vanilla/MO/CCTO/MO+CCTO 4構成検証は別ゲートである。
- レンダリングが必要でも視覚評価のためユーザーを長時間の手動テスターにしない。種同定・絵柄・自然な密度と見え方など人間判断を要する最終確認のみ手動。

## 6. 参照・比較先（研究・採用の根拠）

- 環境省「釧路湿原国立公園の特徴」 https://www.env.go.jp/nature/nationalparks/list/kushiro-shitsugen/feature/
- 環境省「重要湿地・赤名湿原」 https://www.env.go.jp/nature/important_wetland/wetland/w376.html
- 環境省「重要湿地・別寒辺牛湿原」 https://www.env.go.jp/nature/important_wetland/wetland/w033.html
- 日本漢字能力検定協会・漢字ペディア「榛の木」 https://www.kanjipedia.jp/kotoba/0003658400
- 文化庁「ふるさと文化財の森」 https://www.bunka.go.jp/koho_hodo_oshirase/hodohappyo/94187001.html
- Steam Workshop「Vanilla Plants Expanded」 https://steamcommunity.com/sharedfiles/filedetails/?id=2134308522

## 7. 次回実装前に固定する条件

1. ヨシ・スゲ・ハンノキ・ミズゴケの日本語説明文の個別承認と、各PlantDefの種代理の範囲。薬用植物は薬系Modの責務。
2. 各新規植物の承認済み画像、状態差、描画・地形適性（野生生成が消滅しないこと）。
3. 二つの湿地と暖・冷温帯のcommonality移管案（元代理・新種・総量・木本量の増減を表にして確認）。
4. ハンノキの通常植林・研究前後メニューへの影響と、MOの植物／資源との重複監査。
5. 薬系の実装は行わず、既存Healroot供給を維持する。Odyssey非導入を標準構成とし、Odyssey互換を実機未検証と記録する。

