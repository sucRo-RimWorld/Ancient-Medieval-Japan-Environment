# AMJ Environment — Final Art Direction

## 2026-11-01 シラカバ／ダケカンバ葉あり積雪オーバーレイ — 作者「採用で」承認

- **採用範囲:** 2026-11-01に実際に提示・比較した2樹種の積雪合成プレビューを作者が「採用で」と明示。採用対象はその合成画像に使用した**雪だけの256×256 RGBA透過オーバーレイ**。合成画像を本番の樹木画像に置き換えたり、採用済み通常樹木を再描画しない。
- **シラカバ:** 原候補 `Art/Candidates/BirchLeafySnowOverlay_20261101/Shirakaba_SnowOnly_Candidate.png`、Git Blob `43c89b797c2a620466ab0b9bd65e5e1013d150ec`。同じバイト列を `Art/Sources/Things/Plant/AMJ/Shirakaba_Snow/Shirakaba_Snow_A.png` と `Textures/Things/Plant/AMJ/Shirakaba_Snow/Shirakaba_Snow_A.png` に保存。本番DDS同名 `.dds`（9 MIP/非圧縮BGRA、上下反転）を追加。雪有効画素6,618。
- **ダケカンバ:** 原候補 `Art/Candidates/BirchLeafySnowOverlay_20261101/Dakekanba_SnowOnly_Candidate.png`、Git Blob `a459335df81a748c23bddf034d7fec029f922780`。同じバイト列を `Art/Sources/Things/Plant/AMJ/Dakekanba_Snow/Dakekanba_Snow_A.png` と `Textures/Things/Plant/AMJ/Dakekanba_Snow/Dakekanba_Snow_A.png` に保存。本番DDS同名 `.dds`（9 MIP/非圧縮BGRA、上下反転）を追加。雪有効画素5,066。
- **Def接続:** `AMJ_Tree_Shirakaba/snowOverlayGraphicPath=Things/Plant/AMJ/Shirakaba_Snow`、`AMJ_Tree_Dakekanba/snowOverlayGraphicPath=Things/Plant/AMJ/Dakekanba_Snow`。両樹種の `leaflessSnowOverlayGraphicPath=Things/Plant/AMJ/LeaflessNoSnow` を保持。生息域・伐採量・通常／落葉原本は不変。
- **ゲーム受入の境界:** ここでの採用は実RimWorld撮影の受入ではない。ロードされたDDSの方向、正午／晴天／雪深1の葉あり雪の視認性、裸枝の無雪、季節切替、自然発生・植林、セーブ互換は後続の実機検証対象。 `Docs/PlantVisualCoverage.json` の両樹種 `snow` 状態は **pending（source art accepted）** を維持。
- 合成画像と抽出手順は `Docs/ValidationEvidence/BirchLeafySnowOverlayCandidate-20261101.md` を参照。元の全木雪画像は参照素材として保護し、雪専用overlayの代替にはしない。


## 2026-11-01 シラカバ・ダケカンバ落葉画像の枝分かれ削減版 — 作者採用

**現行の採用原画:** 作者が直接アップロードした **1774×887 RGBA透過の2本組PNG**（左シラカバ、右ダケカンバ）を、描画し直さず左右分離。本人が「ではこの画像を分離してそれぞれの落葉画像として」と指示したため、以前の2026-10-30採用落葉画像に優先する。2本組の**原バイトそのもの**を `Art/Sources/Things/Plant/AMJ/Birch_Leafless_Pair/Birch_Leafless_Pair_Approved_20261101.png` に保存（SHA-256 `19d7479dfb0205c7c9429007971e9f906c9f21efb244c2c694d39aab94a43d11`）。原画像は改変しない。

| 項目 | シラカバ | ダケカンバ |
|---|---|---|
| 原画像内の採用対象の実画素外接矩形（alpha>=8） | x=253..645, y=22..835 | x=1113..1528, y=22..834 |
| 無加工で切り出した大判画像 | 409×830 RGBA、SHA-256 `22870eb264843c4cf3b9e494cf414e9857223a64028b5cd69e142f110f272bad` | 432×829 RGBA、SHA-256 `ef14a9c60dee8f34f09dac549f425f6e4fc17fb1ed383c97aa1b56252ee033e2` |
| 分離した新しい落葉原本 | `Art/Sources/Things/Plant/AMJ/Shirakaba_Leafless/Shirakaba_Leafless_Source.png` | `Art/Sources/Things/Plant/AMJ/Dakekanba_Leafless/Dakekanba_Leafless_Source.png` |
| 256px派生画像 | `Art/Candidates/Shirakaba_Leafless/Shirakaba_Leafless_256_Review.png`（SHA-256 `da293abfc3cc90bbfa015e19ceb49ba6ddb4a91fe65535ebc9e5e952d8d81485`） | `Art/Candidates/Dakekanba_Leafless/Dakekanba_Leafless_256_Review.png`（SHA-256 `91942dc1d6563d26bad48bd26cce7bb1c73088c18fc3a014310e156acf85da04`）|
| 旧版の原本・256px（履歴保持） | `Art/Archive/Things/Plant/AMJ/Shirakaba_Leafless/` | `Art/Archive/Things/Plant/AMJ/Dakekanba_Leafless/` |

**導出方法・監査:** RGBAの連結画面を左右で分け、有効alphaの最大の木本連結成分を基準に各8px余白を持つ長方形を切り出した。描画内容の枝/幹の色・形・輪郭は加工せず、256px用の派生では**縦横を同じ倍率でLanczos縮小**してRGBA透明256×256へ配置。旧採用256pxの根元に中心を合わせた（シラカバ130px→130px、ダケカンバ138px→139px）。alpha16以上の新外接矩形はシラカバ(x73..189,y13..253)、ダケカンバ(x78..201,y12..252)。新旧比で樹冠幅が狭いのは作者が選んだ新画像の形状差であり、見かけの均等拡大や横方向の不自然な引き伸ばしはしていない。RimWorld縮小画面でも枝数を減らした樹形として判読できる。

**所有境界:** 画像1状態の原画採用・PNG分離のみが完了。葉あり原本、葉あり積雪、落葉時の枝積雪なし、植生分布/DefName/植物系C#は変えない。両樹種の本番ThingDefとTextures接続・DDS書き出し・実RimWorld 1.6/1.7での樹形確認は、該当する樹種実装工程で行う。旧原本および過去の画像受入記録は履歴として残すが、現在の落葉採用画像ではない。


## 2026-11-01 ハンノキ葉あり積雪：作者採用・現行正本

- 作者は修正候補の実ゲーム比較提示後に「ｙ」と返答し、ハンノキの**葉あり積雪修正版**を採用した。以前の「積雪してない」は旧画像に対する指摘であり、新画像には適用しない。
- 採用原本：`Art/Sources/Things/Plant/AMJ/Hannoki_Snow/Hannoki_Snow_A.png`。SHA-256 `5549f3a99c268211af103f4bf6036e61f36ff8034d01ddd45b00498b61724250`。本番`Textures/Things/Plant/AMJ/Hannoki_Snow/Hannoki_Snow_A.png`と**同一バイト列**。対応DDSは256px/9 mip/BC3、SHA-256 `7c99559a18c6f4d48740bca8ef9211da25489f5870bc58a5016b4c5ea35a42bc`。
- 旧版原画（雪2,477px）は`Art/Archive/Things/Plant/AMJ/Hannoki_Snow/Hannoki_Snow_A_20261015.png`へ**原バイトを変えず保存**。従前の受入・不具合指摘を履歴として保持。現在採用した雪オーバーレイは6,771pxであり、採用済みハンノキ本体／葉／幹／輪郭を変更していない。
- [実RimWorld 1.6.4871、正午・晴れ・一時停止／雪深0と1の同画面比較](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/41270559182)で、葉あり樹冠の雪が視認でき、落葉時には枝雪なし、地面積雪あり、Unity ERROR0を確認。元スクリーンショットはActions Artifact、画像比較は`Docs/ValidationEvidence/Images/HannokiLeafySnow_BeforeAfterNative_20261101.png`。静止画は実際の1年間の季節推移を意味しない。
- 本採用は`AMJ_Tree_Hannoki/snow`に限定し、`Docs/PlantVisualCoverage.json`でacceptedにする。後続の「候補」「未承認」「pending」という記載は**本採用より前の工程履歴**。落葉時の枝積雪は従来の透明オーバーレイ運用を継続。Workshop配布は別承認。


## 2026-11-01 ハンノキ葉あり積雪の視認性修正候補（作者レビュー・実機受入待ち）
**実機による候補の機能的確認（2026-11-01）:** [Project Actions #41270559182](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/41270559182) で Environment PR #306 の候補SHA 6fecd5a25ca2279c79f94502bacda7ffcc2a9b9d をロード。RimWorld 1.6.4871／正午・晴れ・一時停止／積雪深1.0。修正前は黄緑色に見えたハンノキ葉あり樹冠に、修正後は白い雪が視認できる。落葉時の枝積雪なしも維持。Unity ERROR0、VisualCoverage台帳整合PASSだがハンノキ雪の作者レビューは引き続きpending。撮影2枚から同一範囲を6倍拡大した比較画像と条件は Docs/ValidationEvidence/HannokiLeafySnowVisibilityFixNative-20261101.md に記録。作者が候補を最終採用するまではマージしない。


作者の「修正」に基づく**候補**。作者の採用済み葉あり画像 `Textures/Things/Plant/AMJ/Hannoki/Hannoki_A.png` は原バイト不変（SHA-256 `6594360cca125386b5ff1033b7e376a23fc602be120e6c096b18244d6fedced1`）。元の `Hannoki_Snow_A.png` は積雪2,477有効画素しかなく、2026-11-01のRimWorld 1.6実機画面では積雪を識別できなかった。

- 比較した3つの本番DDS（ハンノキ葉あり本体、ハンノキ積雪、正常に見えるブナ積雪）はすべてPNGに対して**上下反転したDDS画素配置**だった。これはゲーム向けの既存方式と整合するので、**DDSの行配置を不具合原因と断定しない**。今回もDDS画素をPNG比で上下反転した形で用意した。
- 採用済みの雪片画素をそのまま保持し、現物の葉画像の緑色領域と元雪片の支持位置を使い、上下各3px／左右各5px以内で既存の雪塊を広げた。雪以外のピクセルや葉あり本体を変更せず、既存の雪3色のみ使用。積雪有効画素2,477→6,771。局所的な拡大を行った**修正候補**であり、勝手に採用済みとしない。
- **候補PNG:** `Art/Candidates/Hannoki_Snow_VisibilityFix/Hannoki_Snow_Candidate.png`、SHA-256 `5549f3a99c268211af103f4bf6036e61f36ff8034d01ddd45b00498b61724250`。本番試験用 `Textures/Things/Plant/AMJ/Hannoki_Snow/Hannoki_Snow_A.png` にはそのバイト列を置いた。
- **候補DDS:** 同名 `.dds`（256×256、9 MIP、DXT5/BC3、各MIPで反転配置）、SHA-256 `7c99559a18c6f4d48740bca8ef9211da25489f5870bc58a5016b4c5ea35a42bc`。DDS復号／上下反転PNGの平均絶対画素差0.40、アルファマスクIoU 1.0。元のBC7がDXT5に変更されるため、この差も実ゲームで確認する。
- **画像比較:** `Art/Candidates/Hannoki_Snow_VisibilityFix/ReviewComposite_256.png` および `ReviewComposite_64px_x8.png`。これらは採用済み本体＋候補雪片の**静的合成プレビューであり、ゲームスクリーンショットではない**。
- **保持:** 従来の積雪原画 `Art/Sources/Things/Plant/AMJ/Hannoki_Snow/Hannoki_Snow_A.png` は無変更。本体 `Hannoki_A.png`／落葉画像／落葉時透明オーバーレイ／季節Defは無変更。
- **受入ゲート:** (1) 凍結した実RimWorld 1.6の昼12時・晴れ・一時停止の通常倍率で葉ありハンノキの雪が視認できること、(2) 雪無し通常葉あり画像と幹・樹形が合致し、白い領域が浮かないこと、(3) 落葉時の枝積雪なしを維持、(4) 作者がこの**修正後**の見た目を採用と判断すること。これらが終わるまでは `Docs/PlantVisualCoverage.json` の `AMJ_Tree_Hannoki/snow=pending` を維持する。

## 2026-11-01 ハンノキ葉あり積雪の表示不備（最新の作者指摘）

作者が実ゲーム撮影を見て「いや、積雪してない」と訂正した。右列のハンノキの葉あり樹冠は黄緑色のままで、雪景色における積雪が視認できない。従来の **Hannoki leafy snow visual PASS は撤回**する。`Hannoki_Snow_A.png` は実在しDefも登録されているが、点在する白い小片がゲーム縮尺で十分な雪景色になっていない。DDS描画方向や重なりの問題はまだ排除していない。葉あり積雪画像・DDSのバイト列は作者の新しい採用画像がない限り変更しない。**落葉時の枝積雪廃止は引き続き正常**。検証元は `Docs/ValidationEvidence/LeaflessSnowRemovedNative16-20261101.md` とProject Actions #41267149034。台帳ではハンノキのsnow状態だけpendingとする。


## 2026-11-01 落葉積雪廃止の実ゲーム検証

作者決定どおり、落葉時は裸枝に雪なし、地表は積雪、葉ありの雪は従来維持。ブナ・ハンノキの落葉時の枝積雪なし、およびブナ葉あり積雪を隔離RimWorld 1.6実画面・Unity ERROR0で確認。**ハンノキ葉あり積雪は画面上で雪が視認できず、従来の表示PASSを撤回して再確認待ち**（[Actions #41267149034](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/41267149034)）。ブナ落葉の原画作者採用と修正済DDSの上下方向についても、既存の実ゲーム証拠を合わせ現行画像に対応。季節切替の旧実機検証は挙動不変の範囲のみ継承し、今回の静止画を新たな1年動作試験とは扱わない。参照: `Docs/ValidationEvidence/LeaflessSnowRemovedNative16-20261101.md`。


## 2026-11-01 落葉積雪の廃止（現行の優先仕様）

ユーザー指示「落葉積雪廃止で」により、AMJE所有の落葉樹では**裸枝への積雪描画を廃止**する。対象は本番のブナ・ハンノキ、および将来追加する落葉樹の描画方針。地面の積雪・葉あり積雪・通常落葉画像はそのまま。落葉時の `leaflessSnowOverlayGraphicPath` には透明共通画像 `Things/Plant/AMJ/LeaflessNoSnow` を使い、葉あり用の積雪へフォールバックさせない。256×256の透明PNG（SHA-256 `70a213096a8a3aa52d8b05ee5ff5f8d14305a01d51f9305d2555880a4a2b458c`）とDDS 9 mip DXT5（SHA-256 `b832ea2cc14a1da84be189c4bd5be36b1cc083d800151b24fc047cfd22aaebb5`）の全画素α=0。旧ブナ・ハンノキ落葉積雪の画像と以前の採用記録、シラカバ・ダケカンバで別途採用された**全木積雪原画そのもの**は消さず、制作史料として残すが、**現在以降の本番実装予定の落葉積雪グラフィックとしては使わない**。これより下の「落葉積雪の制作・保留・採用」の記載は、この決定より前の履歴であり、現行の制作必須条件ではない。対応する正本仕様は `Docs/Design.md`。

## 2026-11-01 ブナ落葉の枝数削減版 — 作者採用（画像原本／本番画像候補）

作者は現行のブナ落葉画像について枝先の過密・不自然な分岐を指摘し、枝先を減らした修正版の1254px透過原画を直接再添付して「これを採用する」と承認した。**この採用は新しい落葉画像の外観と原画に限定**し、落葉積雪・実ゲーム表示・季節遷移の再受入は含まない。

- **現行採用原画:** `Art/Sources/Things/Plant/AMJ/Beech_Leafless/Beech_Leafless_Source.png` — 1254×1254 RGBA、608835 bytes、SHA-256 `7e030c298239428c6f90ee0db3a37b563f40ac37c1f9b1d751927a4102cb21f1`。作者が最終的に再添付したPNGの**原バイトを変更せず**保存。
- **本番PNG:** `Textures/Things/Plant/AMJ/Beech_Leafless/Beech_Leafless_A.png` — 原画全体を座標比固定で256×256へLANCZOS縮小、RGBA/透明維持。SHA-256 `e85df0f7eec38c5af892ba5da79e5177708b2fe8ab0aaba1d351dd4bca678a2f`。
- **本番DDS（2026-11-01 ゲーム内反転不具合への修正候補）:** 同じパスの `Beech_Leafless_A.dds` — 256×256、9 mip levels、DXT5/BC3圧縮。従来のBC7から形式変更された。原画・本番PNGは根元が下だが、実ゲームスクリーンショット（Project Actions #41232462353）では落葉ブナだけ根元が上になる上下反転を確認。**PNGと原画は無変更のまま**、DDSの全MIPのBC3ブロック行／各ブロック内のα・RGBインデックスを上下反転させた。DDSをPillowで復号した画素は、修正前DDSを上下反転した画素とピクセル単位で完全一致（圧縮再実行なし）。修正後DDS SHA-256 `72a77167f8efef6a356d39ea65e0e9cf4887c6c5783e3a8317bf3e8a25d07461`、Git blob `740bade223ee58998def2ced0fc5ebf20e714cc2`。**実ゲーム内の向き修正確認（限定PASS）:** Projectの隔離RimWorld 1.6.4871実機、Actions [#41236249041](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/41236249041) で採用PNGと新DDSを含むPR #301のcommit `b7d0e48fc6c99381edc963418d4c1edf43622886` をロードし、正午・晴天・一時停止の落葉ブナの枝を上・根元を下に正常描画することを実スクリーンショットで確認。修正前 [#41232462353](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/41232462353) は逆向き。6株を配置した撮影セットのUnityエラーは0件。**この合格は上下向きに限る。** 葉あり積雪・落葉積雪の枝形状整合、原画像からの見た目、自然な季節切替／実プレイの最終受入は引き続き未完了。ハンノキの本番画像・DDS・Defは一切変更していない。
- **旧版の扱い:** 2026-10-04採用の `Art/Sources/Things/Plant/AMJ/Beech_Leafless/Beech_Leafless_A.png` は**旧版の履歴**として保持。本番の葉あり `Beech_A.png` とDefのID／描画パス／生態設定は不変。旧版の採用履歴は新原画への承認に優先しない。
- **積雪との整合:** 旧落葉状態の形に合わせて作成・承認された `Beech_Leafless_Snow_A.png` は本画像の枝に対し位置ずれする可能性がある。雪専用素材を**未承認のまま自動加工・置換しない**。既存積雪は旧版への承認であり、この修正版への雪の適合・再受入は未完了。
- **レビュー範囲:** 原画像のPNG CRC／RGBA、256px縮小版の構造・透明背景、およびDDS復号・MIPの静的確認。通常／64px表示と葉あり樹形の視覚比較はソース段階の検討。実ゲームの落葉・積雪・季節切替の受入は別ゲート。


## 2026-10-29 カバノキ類2樹種の画風方針（作者承認、状態別画像は制作中）

`Docs/Design.md` の「カバノキ類の2樹種化」に基づき、これまで1つの `Plant_TreeBirch` で代表させた樹木は、今後の画像制作・実装では**シラカバ**と**ダケカンバ**を区別する。

- **シラカバ（冷温帯林）:** 明るい白色系の幹を景観の主要な特徴とする。
- **ダケカンバ（亜高山帯）:** 灰白色～淡褐色の幹を特徴とし、シラカバと樹形・幹色で区別する。
- 両者ともAMJ共通の低密度の描写、大きな葉塊、明瞭な外枠・透明背景を守る。既存のブナ・スダジイと比較し、カエデ・ナラの採用済み画像を参考にするが、同じ樹形の単純な色替えにはしない。
- 葉あり・落葉等の必要状態は実際のロード済みDef確認後に対応する。季節による紅葉表現は後日の実ゲーム確認に回す。
- **状態別画像は制作中。** この2樹種化は既存 `Plant_TreeBirch` の樹木画像・表示名・ゲーム挙動を変更していない。状態別の採用状況は下記の個別記録で判定し、1枚の承認を全画像・本番Defの承認へ拡大しない。


## 2026-10-30 シラカバ葉あり画像の採用（原画・視覚のみ）

作者がシラカバの葉を増やした版を再生成し、枝につながらない葉塊を指摘した後の修正版を「まあ妥協するか」として採用した。ここでの採用は**葉あり通常画像1枚の外観**のみであり、積雪版・落葉版・季節発色・実ゲーム表示まで承認したものではない。

- **採用原画:** `Art/Sources/Things/Plant/AMJ/Shirakaba/Shirakaba_Leafy_Source.png`。作者承認直前の `白樺の木と豊かな緑の樹冠.png` の**PNG原バイトを保存**。1254×1254 RGBA、1,066,813 bytes、SHA-256 `a15db90bc8dba266b8ceff5edeec4a1e3d7b77a8ae2aa389ec18f58c7e2ad2c4`。
- **256px確認用:** `Art/Candidates/Shirakaba/Shirakaba_Leafy_256_Review.png`。256×256 RGBA、SHA-256 `1aad44a80942c36926d6a8e2d41b139a493584924693210943582b983de122a2`。原画像からLANCZOS縮小した別バイト列で、原画の置換ではない。
- 白い幹・黒い横模様・明るい軽い樹冠を維持する。葉量と枝の接続は作者が現行候補で妥協したため、ここでは再生成しない。
- **葉あり採用時点の未対応（履歴）:** シラカバ落葉と、最終葉あり画像の形に一致する葉あり積雪、必要な季節・積雪描画状態、本番 `Textures/` のDef接続と実ゲーム確認。落葉については後記の作者承認で更新された。過去のシラカバ積雪候補を採用済み画像と取り違えない。
- 本PRは**画像原本と256px確認素材の保管**に限定する。現在の汎用 `Plant_TreeBirch` やBiome/植林候補は変更していない。

## 2026-10-30 シラカバ落葉原画の採用（幹・根元の外形比較合格）

作者は採用済みシラカバ葉あり原画と、後に提示された落葉候補 `葉のない白樺の木_sprite.png` の重ね比較（`AMJE_Shirakaba_Leafy_Leafless_Comparison.png` および `AMJE_Shirakaba_Trunk_Silhouette_Overlap.png`）を確認し、**「合格」**と承認した。採用対象はこの落葉候補の**外観と元原画のPNGバイト列**であり、下位の画像処理による別の `AMJE_Shirakaba_Leafless_Simplify_11.png` は今回の採用画像ではない。

- **採用落葉原画:** `Art/Sources/Things/Plant/AMJ/Shirakaba_Leafless/Shirakaba_Leafless_Source.png`。1254×1254 RGBA、638,316 bytes、SHA-256 `1fcc03b8467b8ee79ac4a1895587ce1149eca7a0972fb49b076070e63ef58a26`。実際に採用した `葉のない白樺の木_sprite.png` のバイト列を無変更で保存。
- **256px確認用派生:** `Art/Candidates/Shirakaba_Leafless/Shirakaba_Leafless_256_Review.png`。256×256 RGBA、SHA-256 `61d227e221de5846f2a7a3cf0276514eef8fc507a0fd1ff3c272130f3f1cc01f`。原画の1254×1254の座標を保持したままLANCZOS縮小。原画とは独立した派生画像。
- **確認範囲:** 葉あり・落葉の1254px正方形画像を根元・幹中心軸で合わせて重ね、幹下部・根元の外形がほぼ維持されていることを確認。ユーザーがこの比較を合格とした。**樹皮色・模様と葉に隠れていた上部の枝はピクセル単位では一致しない**。全枝の一致や既存季節処理の動作を実証したものではない。
- **未対応:** 葉あり積雪の正式採用、落葉積雪、Def・`Textures/` の本番画像経路、ゲーム上のサイズ／季節遷移・既存セーブ確認。**落葉画像のみのソース採用**を、全描画状態や実機受入に拡張しない。既存の葉あり採用原画は変更しない。

## 2026-10-30 シラカバ落葉積雪の妥協採用（原画の外観のみ）

作者は2026-10-30の画像制作で積雪の不自然な白線・貼り付け感を複数回指摘した後、**直接再添付した1枚**に対して「これで妥協」と承認した。採用対象は**この添付画像そのもの**であり、同セッションの `AMJE_Shirakaba_LeaflessSnow_v8.png` や `AMJE_Shirakaba_LeaflessSnow_v8_SnowOnly.png` は、同一ファイル・同一画素ではないため採用画像へ読み替えない。以前の候補は不採用履歴として扱う。

- **採用原画（原バイト保持）:** `Art/Sources/Things/Plant/AMJ/Shirakaba_Leafless_Snow/Shirakaba_Leafless_Snow_Source.png`。作者が再添付した1254×1254 RGBA PNG、797,344 bytes、SHA-256 `044ad2fee8a5f0678c9f17cd8429973f246ec11b1e964c34943004e74926386e`（Git blob `d75851c692774f21931df1676f3df8007a87e659`）。
- **256px確認画像:** `Art/Candidates/Shirakaba_Leafless_Snow/Shirakaba_Leafless_Snow_256_Review.png`、1254px原画からLANCZOS縮小した256×256 RGBA、SHA-256 `459846fd68b9a0bbb995d3cb4a5dc633d4f0cb681739d669a035ba516f5acdad`。原画を上書きしない。
- **確認・採用の境界:** 添付画像は既存採用落葉原画 `Shirakaba_Leafless_Source.png` と**枝・木本の画素・輪郭が異なる**。二枚のRGBA変更は331,882画素、α16以上での輪郭IoUは全画面約0.742・下部y=930～1253で約0.944（等寸1254px同座標の静的比較）。これらの数値は外形差の説明であり、視覚採用の取消理由ではない。作者は**別の積雪した全木画像の見た目**を妥協承認したのであって、幹の下地固定や雪専用レイヤのピクセル一致を確認したわけではない。
- **未実装:** 雪だけの透明オーバーレイ化・採用済み通常落葉画像との完全座標整合、本番 `Textures/` への導入、`leaflessSnowOverlayGraphicPath` の設定と実ゲーム表示・季節遷移。**全木を含むPNGを雪だけを要求する`leaflessSnowOverlayGraphicPath`へ直接指定しない。** 将来の技術的調整時も採用原画を無断上書きせず、別の派生として扱う。
- 葉ありと通常落葉の採用原画、およびダケカンバの各採用画像は変更しない。

## 2026-10-30 ダケカンバ葉あり・落葉原画の取り込み（作者提示の採用画像）

作者が「採用はこれだったと思う」と直接提示した**ダケカンバの葉あり・通常落葉**の2枚を、そのまま葉なし積雪の参照元として保持する。以前ライブラリから取り出した別構図の `AMJE_Dakekanba_GreyBrown_Candidate_256.png` は今回の採用元ではない。2026-10-30「忘れず追加しろ」の指示で、これまでPRから漏れていた原画と256px確認用派生を同じPRへ追加した。

- **葉あり採用原画:** `Art/Sources/Things/Plant/AMJ/Dakekanba/Dakekanba_Leafy_Source.png`、作者提示の `黒背景に佇む stylized な緑の樹木(1).png` の**PNG生バイトを無変更**で保存。1024×1536 RGBA、1,574,575 bytes、SHA-256 `f0e3e17ed68b2d2b505c2cf4a9fdd436b51cc40c4dc2d4d9de35fa4764ed3a5d`。
- **通常落葉採用原画（2026-10-30更新）:** `Art/Sources/Things/Plant/AMJ/Dakekanba_Leafless/Dakekanba_Leafless_Source.png`。葉あり版と幹・根元の位置を比較し、既存樹木と同じ許容基準で作者が採用した**下方30px位置補正版** `落葉_下30px位置補正.png` のPNGバイト列を保存。1024×1536 RGBA、871,287 bytes、SHA-256 `676c62c6334ae96a9b740c44ca565d94b7535decdbfaa2de0aa6c15862a1de56`。以前の原画SHA `84ed9ea0...` は更新前の履歴であり現行採用物ではない。
- **256px確認用派生:** `Art/Candidates/Dakekanba/Dakekanba_Leafy_256_Review.png`（SHA-256 `8fdc5c4d394ec990b5ae962f7667b9e906817bfc8c226af524a30f2b51eaf164`）と `Art/Candidates/Dakekanba_Leafless/Dakekanba_Leafless_256_Review.png`（SHA-256 `729498a6610ba1fad90245fc7df9d55d75350d4ec1f71681df15ed13629352ca`）。各々256×256 RGBA、縦横比維持で最大256pxへLANCZOS縮小し、透明キャンバス中央に配置。元画像とは独立したバイト列であり、原画を置換しない。
- **状態の境界:** 通常の葉あり／落葉／落葉積雪の3状態は原画を個別に保持。落葉積雪は後述の採用原画と対応するが、**枝輪郭の厳密な同一座標・雪だけの独立オーバーレイ**は未完成。通常葉ありの積雪版は未採用。本番の `Textures/` ファイル、Defのグラフィック経路、季節処理と実機表示は今回変更していない。

## 2026-10-30 ダケカンバ落葉積雪画像の作者採用（原画・視覚のみ）

作者は会話で提示された葉あり／落葉の2状態を参照したのち、最後に生成された**落葉積雪の単木画像**を「葉っぱなし積雪は採用」と明示承認した。採用対象は画像 `雪をまとった冬の裸木スプライト.png` の**原画像そのもの**であり、それ以前の別樹形の積雪案、葉あり積雪版、シラカバ画像の採用には拡張しない。

- 採用原画: `Art/Sources/Things/Plant/AMJ/Dakekanba_Leafless_Snow/Dakekanba_Leafless_Snow_Source.png`。1254×1254 RGBA、713066 bytes、SHA-256 `5f37cbade023e75aa232523e1dfd5e8810ea4ce0fcad3f44dee609f029908b48`。原画像のバイト列を保持し、無断再生成／描き直ししない。
- 256px確認用縮小画像: `Art/Candidates/Dakekanba_Leafless_Snow/Dakekanba_Leafless_Snow_256_Review.png`。256×256 RGBA、SHA-256 `1a8ab5a6ebd257bfd75c836f4d59b19d39174e6404fdc8ed39cd9e3e9c7d7e57`。LANCZOS縮小による**別バイト列**。本番 `Textures/` への配置、画像読込・Def接続は未実施。
- この承認は**樹木全体を含む落葉積雪の見た目**に限定。現行仕様の `leaflessSnowOverlayGraphicPath` は雪だけを含む透過素材を想定するため、この樹木全体の画像をそのまま雪オーバーレイとして登録しない。落葉元画像へのピクセル一致、雪のみの抽出・位置合わせ、最終描画方法・256px表示と実ゲーム内の受入は未完了。
- 既存の採用済み葉あり／落葉原画は変更しない。他の状態の採用および `Plant_TreeBirch` の機能変更は今回の承認対象外。


## Current accepted production (2026-10-15 JST)

The four original AMJE species (Shii, Beech, Shirabiso, Haimatsu) retain
accepted normal, immature, UI and snow appearances. Beech leafless/autumn
appearance is accepted and its seasonal mechanics were verified by bounded
native calendar sampling. **Hannoki's six rendered static appearance states**
(normal, immature scaling, UI, autumn, leafless, snowy leafy/leafless) have
now also been **author-accepted** in the current production candidate.
**Hannoki's natural temperature-driven seasonal mechanics are now independently
verified** by the scoped real RimWorld 1.6 calendar sampling on 2026-10-17:
native fall intensity changes, the same test-placed production Hannoki loses
leaves and then regains them without a forced state. Its six static appearance
states remain author-accepted and the seventh `transition` is technically
verified; `Docs/PlantVisualCoverage.json` now records
`AMJ_Tree_Hannoki.complete: true`. This is not a 60-day full-world simulation,
natural wetland spawn test, 1.7 acceptance or Steam distribution proof.
Evidence: `Docs/ValidationEvidence/HannokiSeasonalNative-20261017.md`.
The author noted the supplied crops/zoom were hard to inspect but explicitly
chose to adopt the existing art anyway. Do not redraw/re-scale accepted source
to compensate for the inadequate review presentation.
See `Docs/ValidationEvidence/HannokiVisualAcceptance-20261015.md`.
Historical trial/review sections below describe **their earlier dates only**
and do not revoke the 2026-10-15 image acceptance.
Haimatsu retains drawSize2.60, visualSizeRange0.45..0.75 and an explicit
UI path for native UI scale1. Existing Vanilla/MO retextures remain deferred.


## 2026-10-15 ハンノキ実機画像の採用

作者が提示画像を見づらいと指摘した上で「まあ採用で」と回答したため、
**本番画像を変更せず現状採用**とする。
RimWorld 1.6の同一59ファイル配布候補を用いた実画面5セット
（通常・未熟、UI、強制紅葉、強制落葉、葉あり／落葉積雪）を対象とする。
画像や植物Defの再作成を要求しない。
`Docs/ValidationEvidence/HannokiVisualAcceptance-20261015.md` に
撮影経路・固定コミット・画像SHA-256・テストの前提を保存済み。

| 本番表示状態 | 採否 | 対象・限界 |
| --- | --- | --- |
| 通常・成熟 | 採用 | 通常本番PNG、昼12時・Clear・停止 |
| 未熟株 | 採用 | 同じPNGの成長率0.1/0.5/1.0による縮小 |
| UIアイコン | 採用 | 実DefIcon 32px / 64px |
| 紅葉 | 採用 | 本番LUT・色基準、秋色強度1の強制表示 |
| 落葉 | 採用 | 本番落葉パス、MakeLeaflessの強制表示 |
| 葉あり／落葉積雪 | 採用 | 両本番雪パス、積雪深0/0.5/1 |
| 自然な紅葉→落葉→回復 | **未検証** | 強制状態の静止画から推定しない。今後の実ゲーム確認 |

上記の6状態は `Docs/PlantVisualCoverage.json` の
個別 `accepted` 行で固定する。自然季節遷移は `pending` に留める。
作者が「どうせ後で実ゲーム確認するし」と明示した通り、
確認は通常プレイで行い、将来問題が発見された場合のみ対応する。
既存原画・採用済PNG・DDS・XMLの内容は変更しない。

## 2026-10-13 ブナ紅葉の基準色校正・カラーマップ採用候補

作者は「明度・彩度を校正した右側の発色を採用候補として進める」確認に `ｙ` と回答した。
対象は通常ブナPNGの再彩色ではなく、既存紅葉カラーマップと `_FallColorSource` の組み合わせ。
通常・落葉・積雪の承認済み原画、描画寸法、生態・収量は保持する。

- 基準RGB：`(0.496758088, 0.537254902, 0.323263543)`。従来の色相約71.35度と変換範囲 `(0.02,0.10)` は保持し、実葉色との明度・彩度の不整合を校正する。
- カラーマップ正本：`Art/Sources/Other/AMJE_BeechFallGradient.png`。SHA256 `1ccc50e0619cd7dfa40d6e874f27f9c3a6a78b4871cc5f0d5182098d218ce8f2`。本番PNGは同一バイト、DDSは全画素の復号RGBAが一致する別形式派生物。
- 現行RimWorldの植物シェーダーはHSVで `元画素 + t × (変換先 − 基準色)` を計算する。従来の基準色は原画の明るい葉より暗く高彩度だったため、カラーマップより明るく淡い紅葉になった。実GPU10件で照合し、通常強度0の比較領域も画素差0を確認した。
- 比較専用材質の校正は作者レビュー用。正式候補のロード済みDef・DDS画素・実発色は別に検証し、証跡は `Docs/ValidationEvidence/BeechAutumnCalibration-20261013.md` に保持する。これはSteam配布・4構成・旧セーブの完了証拠ではない。

ハンノキ提供原画と、それを含む比較画像はローカルに保持し、今回のブナ修正PRの公開物には含めない。
2026-10-13、作者の「画像も更新」により、別PRで現在のローカル原画と256px画像を公開ソースへ反映する。原画は `Art/Sources/Things/Plant/AMJ/Hannoki/Hannoki_Mature_Source.png`（1289×1221 RGBA、SHA-256 `a63145b884dc943113f29171b8cc7366a40d7183ec27cf289d8243bbd1273cf8`）、配置用画像は `Textures/Things/Plant/AMJ/Hannoki/Hannoki_A.png`（256×256 RGBA、SHA-256 `6594360cca125386b5ff1033b7e376a23fc602be120e6c096b18244d6fedced1`）。両方とも提供された現物のバイト列を保持し、再生成・再縮小しない。これは画像ファイルの反映指示であり、未熟・UI・落葉・積雪・実機での全状態受入や本番Defの完了を意味しない。
共用カラーマップの色変化は比較専用Defで確認したが、ハンノキ本番Def、描画寸法・収量、落葉・積雪画像、自然な季節移行の採用は別途必要。
ブナは現行XML上 `DeciduousTreeBase` に季節パラメータと落葉パスを明示している。ハンノキも同じ親Defと標準 `PlantFallColors` を用いる。旧記録の `TreeBase` 表記は現行ソースと不一致だったため訂正。

## 2026-10-13 ハンノキ積雪オーバーレイ暫定採用

作者の「一旦これで採用」は、通常画像から枝・樹冠を再生成した先行不採用案ではなく、
256pxの既存通常画像と位置を揃えて作成した**雪専用透過オーバーレイ**の
原画・静的見た目に限定した暫定採用である。現物の完全なPNGバイト列を凍結する。
雪が斑点状に見える箇所などの改善余地は既知だが、この採用を理由に自動再描画しない。

- 通常画像の参照: `Textures/Things/Plant/AMJ/Hannoki/Hannoki_A.png`、256×256 RGBA、
  SHA-256 `6594360cca125386b5ff1033b7e376a23fc602be120e6c096b18244d6fedced1`。
  原画 `Art/Sources/Things/Plant/AMJ/Hannoki/Hannoki_Mature_Source.png` は不変。
- 積雪の承認ソース: `Art/Sources/Things/Plant/AMJ/Hannoki_Snow/Hannoki_Snow_A.png`。
  本番配置用の同一バイト: `Textures/Things/Plant/AMJ/Hannoki_Snow/Hannoki_Snow_A.png`。
  両ファイルとも **256×256 RGBA、2,986 bytes**、
  SHA-256 `ecc2c0412e030eb19f3f45ab016aaed94a3b1e7b37f66d6db8375f573651df16`。
- 合成比較画像（承認時の照合証跡であり本番ソースではない）:
  `Hannoki_Snow_Composite_candidate.png`、256×256 RGBA、
  SHA-256 `eb6d5c718e67d2f64abc7b1a3fd33b12c760780de9bac58bbf16b76d09a3d5a0`。
  これは元画像の雪以外のRGBA値をそのまま残す重ね合わせ確認用。
- 静的確認: PNGシグネチャ、IHDR/IDAT/IEND、CRC、Pillow復号、RGBA、透過、
  端のクリッピング無し、雪オーバーレイが不透明な通常画像領域のみに存在すること、
  オーバーレイの透明画素範囲で合成結果のRGBAが通常画像と一致すること。
  雪の非透明画素数は2,477。元画像のバイト列は変更しない。
- **未完了:** 本番Hannoki PlantDefの`snowOverlayGraphicPath`接続、落葉時の
  `leaflessSnowOverlayGraphicPath`要否・状態画像、未熟/UI、
  正午・Clear・停止・雪深0/0.5/1.0の実ゲーム表示、自然な季節移行、
  全状態の最終受入、Workshop配布。静的・原画承認を実機のPASSと混同しない。

## 2026-10-13 ハンノキ落葉積雪オーバーレイ暫定採用

作者は落葉状態に対して前回の再描画積雪画像を不採用とし、元の落葉画像を固定した雪専用オーバーレイ方式を指示した。原画像・256px・64px比較と静的監査を提示後、2026-10-13の単独 `ｙ` で直前のレビュー候補を暫定採用・次工程続行と承認した。**承認対象は最後に提示した合成結果と、その実体である雪専用透過オーバーレイのみ**。細雪の64px視認性には既知の改善余地があり、作者の新たな変更指示なしに描き直さない。

- 位置合わせの基準原画: `Art/Sources/Things/Plant/AMJ/Hannoki_Leafless/Hannoki_Leafless_Source.png`。
  1254×1254 RGBA、758781 bytes、SHA-256 `dffb281e09a79f4ba6cb031237d658c6f2abcbb403029cfe3cbf7326b76ad428`。
  2026-10-13に「よさそう」と評された落葉候補の元の画像であり、通常有葉画像との枝分岐差は既知。**この採用は落葉状態自体の最終実機受入を意味しない**。
- 対応する落葉256pxレビュー基準: `Art/Sources/Things/Plant/AMJ/Hannoki_Leafless/Hannoki_Leafless_A.png` と
  `Textures/Things/Plant/AMJ/Hannoki_Leafless/Hannoki_Leafless_A.png`。両方とも256×256 RGBA、58279 bytes、SHA-256 `dbdd69bc98f21d294c2a083b12225de55392eefc48c03164b103b82176155003`。
  原画の再生成・上書きではなく、レビュー時に用いた派生物をそのまま保持する。
- **暫定採用された雪のみの透過原画**:
  `Art/Sources/Things/Plant/AMJ/Hannoki_Leafless_Snow/Hannoki_Leafless_Snow_A.png` と
  `Textures/Things/Plant/AMJ/Hannoki_Leafless_Snow/Hannoki_Leafless_Snow_A.png`。
  両方とも256×256 RGBA、8268 bytes、SHA-256 `41e7548dfc2f93839e1ff4c55d3ddbffffd0c6a8e6f09c45ba09cdd0f35ad640`。
- 静的合成レビュー `Hannoki_Leafless_Snow_Composite_review.png` は256×256 RGBA、
  SHA-256 `921830866f3f92824aaa30374b458b94f34d6651e13f203d7773c480ab818d85`。
  透明雪画素の範囲で元の落葉256pxとのRGBA差分0。
  Pillowの `Image.alpha_composite(leafless_256,snow_overlay)` と全画素一致。雪の非透明画素1917、そのうち枝シルエット外への微小なオーバーハング375画素・最大2px。キャンバス四辺に雪画素なし。
  PNG正常復号、RGBA、原画像の保持を確認。デザイン監査で細雪の64px視認性に改善余地あり。
- この段階は**画像バイナリ保管・原画レビュー承認のみ**。落葉原画の完全な季節連続性、本番 `PlantDef`、`leaflessGraphicPath` /
  `leaflessSnowOverlayGraphicPath` 接続、自然発生・雪深閾値のRimWorld実機表示、
  葉あり・落葉・積雪の全状態最終受入、Workshopへの同梱・配布は未完了。
  `Docs/PlantVisualCoverage.json` の現行対象は実装済み4種のみであり、未実装ハンノキを完了状態で追加しない。

## 2026-10-13 ハンノキ本番画像接続（ゲーム実機は未検証）

2026-10-13の作者による既存樹木・現実植生との数値比較承認に基づき、
`Defs/ThingDefs_Plants/AMJ_WildPlants.xml`に `AMJ_Tree_Hannoki` を実装し、
保存済みの既存画像に**再描画せず**次の4状態を接続した。
本節は上の採用時点の古い「本番Def未作成」という進捗記述を更新する。

| 描画状態 | 本番パス | 承認区分 |
|---|---|---|
| 成熟／成長縮小／標準UI | `Things/Plant/AMJ/Hannoki` | 成熟画像の暫定採用、ゲームUI・未熟状態は未検証 |
| 落葉 | `Things/Plant/AMJ/Hannoki_Leafless` | レビュー元画像保持。枝分岐の通常形との不一致は既知、最終実機承認待ち |
| 通常積雪 | `Things/Plant/AMJ/Hannoki_Snow` | 雪専用オーバーレイ原画暫定採用 |
| 落葉積雪 | `Things/Plant/AMJ/Hannoki_Leafless_Snow` | 雪専用オーバーレイ原画暫定採用。64px可読性に既知の弱点 |

- ハンノキの落葉機構は実際のブナ同様 `DeciduousTreeBase`。
  暖湿地の木本構成を増やさない出現数値・実材24・成長20日・収穫作業800は
  `Docs/NativeVegetationStep3Design-ja.md` §3.1.2 が正本。
- 秋色は既存 `PlantFallColors` 機構・ブナLUT `/Other/AMJE_BeechFallGradient` を
  **検証候補として再利用**。ハンノキ葉の256px画像で緑葉画素を抽出して
  得た代表的なRGB(108,114,67)/255 = `(0.423529412, 0.447058824, 0.262745098)`
  を `_FallColorSource` に設定した。既存原画像やブナカラーマップは変更しない。
  **このキーと共用LUTの最終的な紅葉発色は作者未確認**。
- `Docs/PlantVisualCoverage.json` は7状態 `pending`。
  本番Defの参照先・PNGハッシュ・CCTO XMLの静的監査と、
  実RimWorldでの落葉切替・雪深・秋色・UI・成長スケール・旧セーブを区別する。

## Scope

Environment intentionally deferred final art until the functional/balance Alpha loop was stable.

The final-art pass currently tracks **27 primary art targets / baseline states**, but this is **not an exact final PNG count**. The scope was expanded after reviewing the Vanilla and Medieval Overhaul trees that actually appear alongside AMJE vegetation. For any prerequisite tree Def, every additional loaded graphic state that can appear in normal play is part of the same retexture family and must be covered or explicitly documented as an exception.

### AMJE structural plants

1. `AMJ_Tree_Shii` — leafy Sudajii / Shii tree
2. `AMJ_Tree_Beech` — leafy Japanese beech
3. `AMJ_Tree_Beech` — leafless Japanese beech
4. `AMJ_Tree_Shirabiso` — leafy/evergreen Shirabiso fir
5. `AMJ_Shrub_Haimatsu` — evergreen Haimatsu dwarf pine

### Vanilla tree retextures used by AMJE / normal MO coexistence

`Plant_TreePoplar` is not an AMJE natural-vegetation retexture target: the accepted retained-vegetation audit excludes it from the AMJE biomes. Its Vanilla Def is not deleted or renamed; this only removes the unneeded art plan.

6. `Plant_TreeOak` — leafy
7. `Plant_TreeOak` — leafless
8. `Plant_TreeMaple` — leafy
9. `Plant_TreeMaple` — leafless
10. `Plant_TreeBirch` — leafy
11. `Plant_TreeBirch` — leafless
12. `Plant_TreeWillow` — leafy
13. `Plant_TreeWillow` — leafless
14. `Plant_TreePine` — evergreen
15. `Plant_TreeBamboo` — evergreen

These are the baseline mature/leafless targets used to track art direction. They intentionally affect the corresponding Vanilla tree Defs wherever they appear while AMJE is active. Creating duplicate AMJ-only tree Defs solely to change graphics would add unnecessary biome/content duplication and compatibility cost.

**State-family completion rule:** before any Vanilla tree Def is marked fully retextured, inspect its loaded RimWorld 1.6 `PlantProperties` and include every non-empty state that can be visible in normal play: base, leafless, immature, polluted, leafless-immature, snow-overlay, leafless-snow-overlay, and immature-snow-overlay as applicable. Do not invent a state that the Def does not use. This rule supersedes treating the numbered list above as the exact number of production PNGs.

### Medieval Overhaul conditional tree retextures

16. `DankPyon_GreatOak` — leafy
17. `DankPyon_GreatOak` — leafless
18. `DankPyon_GreatIter` — leafy
19. `DankPyon_GreatIter` — leafless
20. `DankPyon_GreatFir` — evergreen
21. `DankPyon_GreatWillow` — leafy
22. `DankPyon_GreatWillow` — leafless

These are **style harmonization only**. Great Oak / Great Iter / Great Fir / Great Willow remain Medieval Overhaul species/content with their original mechanics, scale, transparency class, harvest behavior, and fantasy identity. AMJE should only replace their graphic paths when MO is active.

### Natural terrain

23. `AMJ_ThinSoil` — seamless/tileable thin stony soil

### World-biome textures

24. `AMJ_WarmTemperateForest`
25. `AMJ_CoolTemperateForest`
26. `AMJ_SubalpineForest`
27. `AMJ_AlpineZone`

No new weather, snow, river, coast, or seasonal-effect art is required for the Alpha pass because those systems intentionally reuse Vanilla rendering.

## AMJ / Medieval Overhaul visual baseline

Environment inherits the AMJ-wide in-game art direction from `Ancient-Medieval-Japan-Project/Docs/ArtStyle.md`. The shared AMJ target is to **look at home beside Medieval Overhaul, not beside RimWorld Vanilla**. Medieval Overhaul is therefore the primary visual calibration reference for AMJ in-game sprites/textures; Vanilla is not the target style.

For AMJE specifically:
- AMJE-owned structural plants must remain compatible with that shared AMJ / Medieval Overhaul-oriented visual language;
- Vanilla vegetation retained by AMJE should be retextured toward the same AMJ baseline when its visual pass is performed, rather than pulling AMJE art back toward Vanilla;
- optional Medieval Overhaul retextures are style-harmonization work inside the same baseline, not a separate visual direction;
- while only part of the retained vegetation has been retextured, a visible contrast against untouched Vanilla art can be a temporary rollout mismatch. Judge whether AMJE art itself is on-style against the accepted AMJ/MO references before deciding to redraw it toward Vanilla.

Environment's tree/plant rules below intentionally allow controlled class differences such as restrained internal gradient variation, but they do not change this project-wide target.

## 2026-10-12 暫定採用：葦（アシ／ヨシ）未熟株の画像

作者は穂のない葉・若い茎中心の直近画像を「一旦OK」として暫定採用した。対象は監査後の原画 `中央に佇むスタイリッシュな草むら.png`（1254×1254 RGBA）そのもの。採用済み成熟株のマスターは変更しない。

- 原画：`Art/Sources/Things/Plant/AMJ/Yoshi/Yoshi_Immature_Source.png`。SHA-256 `6437c65dd4524f3c5d267406bd504a69e5862bd71df7cdaa24b9796fcfde89bd`。
- 256px派生：`Textures/Things/Plant/AMJ/Yoshi_Immature/Yoshi_Immature_A.png`。SHA-256 `ab6b8d36d535355f026d30ab3df08e33c45929f7829938a2b12bd21e929c90f6`。LANCZOS縮小のみ。
- 範囲：未熟株の画像デザインを暫定採用。輪郭の半透明・色ノイズ、実機描画とDef参照は未検証。未熟株の採用をPlantDef・冬季画像・ゲーム内テストの承認とみなさない。

## 2026-10-12 暫定採用：菅（スゲ）未熟株の画像

作者は穂がなく葉主体の直近のスゲ未熟株画像を「一旦OK」として暫定採用した。原画はチャット内の `若いスゲの草むらスプライト.png`（1254×1254 RGBA）であり、成熟株原画は変更しない。

- 原画：`Art/Sources/Things/Plant/AMJ/Suge/Suge_Immature_Source.png`。SHA-256 `960a77e05c98c28f9d1410af193cdaa555a6d930ef06218756a871fc65cf2eb7`。
- 256px派生：`Textures/Things/Plant/AMJ/Suge_Immature/Suge_Immature_A.png`。SHA-256 `7dcbac89d64ae43e4498344fe51ae4d37574451a2f4f2b1abd455d887dd5f35c`。LANCZOS縮小。
- 承認範囲：未熟株の画像のみ。輪郭品質、実機描画、PlantDef接続、積雪・UI画像、植生設定や収量は別途確認する。

## 2026-10-12採用：菅（スゲ）成熟株の画像

作者は直近のフラット化したスゲ成熟株画像を「一旦これで採用」として承認した。**この承認は成熟株の画像のみ**であり、未熟・UI・雪・実機描画・PlantDef実装を承認したものではない。承認元は比較表でも旧案でもなく、会話の最後に確認・監査された1254×1254 RGBA PNG。

- 承認原画：`Art/Sources/Things/Plant/AMJ/Suge/Suge_Mature_Source.png`、SHA-256 `fc115d7af72b4f5009f6aab2a2f50387970318dd13ddf9b45c4ff6bb8f3c4917`。
- 256px派生画像：`Textures/Things/Plant/AMJ/Suge/Suge_A.png`、SHA-256 `4e61c100ef5fb910d98c53aee4f8cbb15c3cd96cdac11d3f7bef635132790ede`。原画を変更せずLANCZOS縮小。
- 現状：輪郭の色ノイズ・半透明ピクセルは既知の未解決品質課題。作者の一旦採用は実機表示品質の合格を意味しない。本番Defは画像状態族と参照先を確認してから実装し、既存の植生配分や採取承認を変更しない。

## 2026-10-12採用：葦（アシ／ヨシ）成熟株の画像

作者は葦の成熟株の画風・形状を採用し、2026-10-12に「採用としてpush」と指示した。対象は**直近の独立した葦画像**（既存樹木をAIで再描画した比較表や、別の再生成画像・背景付きの切り出し画像ではない）。

- **原画像の現物：** 会話内で承認されたPNG（1254×1254 RGBA）。SHA-256 `0d65557b5f3646fb04e96be9997ba239c990a995f33a02eb65f84ee3fa8f2ea4`。
- **縮小派生画像：** 256×256 RGBA。SHA-256 `3604ab2b6c489870fdca1c0a0f660dd3beb5870c317dd581c65eca93ec020af2`。Pillow LANCZOSでの縮小のみ。元画像とは別のバイト列。
- **採用範囲：** 成熟株のデザインと元画像。さらに2026-10-12、作者が実機スクリーンショットを見て「採用とする」と回答し、確認用Defでの成熟株の通常表示を採用した。`drawSize=(1.5,1.5)`、`visualSizeRange=1~1`、成長度1、左右固定、`Things/Plant/AMJ/Yoshi`の採用画像が対象。昼12時・Clear・天候遷移完了・一時停止・春・秋色強度0・積雪0、zoom=18/9で撮影し、12株の画像読み込み／固定描画とERRORゼロを確認。証跡：`TestResults/AshiReview-20261012/Review.md`、`CloseRun/Ashi-Mature-Close.png`（SHA-256 `2f0b007c61a0a34c7b5cc6c066dc29e9980af57a4ad49153d128047dfe3b6730`）。確認用DefのSHA-256は`7cb5126c4b3845803bda63175ec5fd371114313ace34ab10c745b45d1b3c46ed`。未熟株の画像デザインは別節で暫定採用済みだが、未熟の実機表示・アイコン・積雪・自然分布・採取仕様・本番PlantDefは未承認／未実装。ススキとの識別性と既存AMJE画風を維持し、承認済み原画を再生成・描き直さない。本番Def導入時はこの採用設定と画像を照合して通常状態のfingerprintを登録する。現行`PlantVisualCoverage.json`は実装済み4樹種だけを対象とするため、未実装アシを完了行として追加しない。
- **保存先：** `Art/Sources/Things/Plant/AMJ/Yoshi/Yoshi_Mature_Source.png` に原画像の正確なバイト列を保存済み。256px派生画像を `Textures/Things/Plant/AMJ/Yoshi/Yoshi_A.png` に配置済み。Def採用前に正式なgraphic-state構成・最終texPathを確認する。
- **転送・導入状態：** 最新mainには両PNGの実体が保存され、上記SHA-256と一致する。確認用Defでの成熟株表示は前述の範囲で採用済み。本番PlantDefの接続・全状態の受入・配布完了を意味しない。

## 2026-10-10採用源画：薄・芒（ススキ）成熟株（実装予約）

**作者の2026-10-10の明示指示：** 「直前の画像を採用にする」。その後「**この画像はススキ。その後にアシを作れ**」と種名を訂正したため、採用元は**ススキ（*Miscanthus sinensis*）の成熟株の源画**とする。**アシ（ヨシ）用には流用しない。** 画像の制作段階でアシと誤認した旧チャット記録に優先し、この種名判定を正本とする。

- **承認元画像のファイル名（会話添付）：** `三本の穂が揺れるススキの草株.png`。
- **元画像の形式：** PNG / RGBA / 1254×1254 px / 936760 bytes。SHA-256 `d2b5bbca0f4112bacf4426800c92c985d5f52266c21e08602d822f8ff67d340f`。
- **256px派生レビューPNG：** `AMJE_Susuki_Mature_256_Review.png`、SHA-256 `bff51c894f345a6069da47f736b802b30e84e532aa7cbb1167ce6868864e1046`（縮小版は**元画と同一バイトではない**）。64pxでのゲーム視認性は本番テスト未了。
- **予定する源画所有パス：** `Art/Sources/Things/Plant/AMJ/Susuki/Susuki_Mature_Source.png`。**2026-10-10現在、GitHubには元PNGバイナリ未転送**。画像のSHAと採用履歴を先に登録したのみで、リポジトリへの画像取り込み、`Textures/`インストール、実Def参照、読み込みテストは未完了。実際の源画転送後にSHA-256完全一致を確認し、受け入れ証跡を追記する。
- **採用範囲：** 成熟株の視覚的源画のみ。未熟、UIアイコン、積雪時の各状態、ゲーム内の見え方や`Docs/PlantVisualCoverage.json`の本番fingerprintは未承認・未記録。元画像には形状の境界がキャンバスに達する箇所があるため、実装時のクロップ・余白と描画スケールを別途検証する。
- **工程順：** 草地植物ススキの**本番実装は湿地4種完了後**。独立実装要否の調査・日本語本文・出現率・収穫物などの未決仕様はススキ単独で審査する。アシの新画像制作は湿地4種の先行作業であり、**この採用元をアシ扱いしない**。

### 2026-10-10 穂形状の再検討と左右固定条件（候補段階）

作者は写真を見直し、穂の枝先を示した直線に沿って揃える修正、透過背景、単純縮小ではなくゲーム用への再生成を指示した。その後「左右の向きが変わらないようにできるならこの画像でもよさそうだが」と条件を付けた。新しい右向きの5穂候補は旧採用源画の上書き承認ではない。候補は少色・簡略化し、全体を収めた256×256 RGBA PNGとして書き出す。穂先の直線整列、見え方、未熟・UI・積雪の受入は未了。

**左右固定要件：** この非対称候補を採用する場合、ススキの穂を元画どおり右向きに保持する。アシにも同じ反転抑制仕様を適用する（下記「ススキ・アシの左右固定仕様」）。湿地4種の後というススキの本番実装順を維持する。

## Plant technical direction

Current plant Defs use `Graphic_Random`.

RimWorld's `Graphic_Collection` loads every texture found inside the configured folder path. A folder containing a single valid texture therefore remains compatible with `Graphic_Random`; additional variants can be added later without changing the Def class.

### ススキ・アシの左右固定仕様（2026-10-10作者指定・未実装）

対象はススキと葦（アシ、`AMJ_Plant_Yoshi`）に限定し、各種の採用元画像の左右の向きを保持してランダムな水平反転を抑制する。アシの非対称な穂も同じ扱いとする。通常・積雪の描画と複数株で方向を揃え、未熟・UIも各種の状態画像と整合させる。種ごとの画像・形状・受入は独立して維持し、既存4樹種・スゲ・ハンノキ・ミズゴケ・他Mod植物の反転挙動は変えない。

RimWorld 1.6.9438.38202の[`Plant.Print`](https://github.com/Chillu1/RimWorldDecompiled/blob/2d508035082e7cb0c8e29e230d26bda6e546928f/RimWorld/Plant.cs)は`bool flipUv = Rand.Bool`を直接生成してアトラスと通常・積雪の両平面へ渡し、`graphicData.allowFlip`を参照しない。`allowFlip=false`や`Graphic_Single`への切替だけでは成立しないため、対象2種に限定した描画処理が必要。通常・積雪・複数株と他種への非干渉を実ゲームで検証する。**本番の左右固定描画処理とPlantDefは未実装。アシ成熟株画像は保存済みで、2026-10-12の確認専用FixedPlantでは左右固定表示が採用された（上記証跡）。これは通常・積雪を含む本番2種の描画検証完了ではない。** 各種の既定の実装順・画像承認・分布・採取仕様のゲートを維持する。

Alpha art policy:
- begin with **one strong final texture per state** rather than producing many variants immediately;
- use the **adopted ENV-010 atlas style** as the visual baseline for AMJE, Vanilla retextures, and optional MO retextures;
- the adopted style is rounded and simplified rather than botanically literal, with smooth high-resolution edges rather than pixel-art;
- saturation is reduced relative to the first concept pass, while the approved hue relationships remain intact;
- use a restrained but not posterized palette, limited base/shadow/highlight planes with the restrained tree-volume variation defined below, and clear silhouette separation at RimWorld gameplay zoom;
- keep transparent backgrounds;
- no baked terrain, UI border, text, or decorative frame;
- use RimWorld-readable top-down/three-quarter plant silhouettes rather than botanical illustration plates;
- preserve clear empty transparent space around branch/needle edges to avoid a rectangular sprite appearance;
- match existing plant scale through `graphicData.drawSize` / `visualSizeRange` rather than baking excessive canvas padding;
- avoid photographic rendering; target the simplified game-sprite look of the approved AMJE tree references;
- do not bake dynamic snow into plant textures.

Production texture transfer, validation, Def-switch ordering, and focused runtime review must follow `Docs/GoldenPaths/TextureAssetPipeline.md`. Do not invent a per-asset binary-transfer path after the source art is approved.

## Generation entry and new-chat continuity

Before a plant/tree generation call, follow [RetextureGeneration.md](GoldenPaths/RetextureGeneration.md). That procedure reads the current Ancient-Medieval-Japan-Project `Docs/ArtStyle.md`, this document, the current coordination state, and the actual accepted reference images. Do not preserve a separate frozen style prompt in this repository.

## Retexture visual-style rules

Shirabiso snow trial (2026-10-05): accepted master unchanged; its own seven
foliage regions define the snow mask. White/blue-grey snow palette reused from
accepted Haimatsu, upper branch surfaces covered, trunk/sides/outer AA protected.
Current snow overlay SHA-256 b2778ae82c1756e3a48715adb80dc289ce7a96dc38bdb12401f33ee0daca2c56.
Registry Art/Templates/Shirabiso-Snow-v1/template.json remains REVIEW; no snow
visual acceptance inferred. Native snow display remains depth >0.8.

Haimatsu master-registered snow (2026-10-05): previous approximate snow alignment
was rejected by author (`まだずれてる。`). Current review overlay uses a binary
mask registered to the accepted master's own five foliage pads, with generated
snow material colors and the master's broad shade blocks. Current SHA-256:
f77fb8d8db633c5c3b7452927e61bbde152a531fbb351c47f0a721587eef6147.
Registry: Art/Templates/Haimatsu-Snow-v1/template.json (review, not active).
Protected master RGBA differences=0; native snow threshold and drawSize unchanged.
Final in-game appearance remains pending.

Haimatsu snow derivative trial (2026-10-05): author authorized proceeding from
the illustrative combined preview with `良さそうな気はするのでそれで進めて。`.
Separate snow-only overlay installed at Things/Plant/AMJ/Haimatsu_Snow, SHA-256
e17099fe45e6106bf0a3a1bd07c40352b8e8d1ec1ad679923d060b083305bdc0.
Normal master bytes/drawSize remain intact. Native snow overlay routing and
in-game appearance are reviewed separately; illustrative preview is not exact
compositing/pixel-registration evidence.

Snow registration correction (2026-10-05): author reported slightly offset snow
and instructed continuation. Snow-only image edited with the accepted Haimatsu
master as positional reference; cap coverage reduced to upper pad surfaces.
Current overlay SHA-256 e1037f0a6c1c264dd10949e01f604bd50df75966147957482a0587041a3d4f7f.
Actual normalized pixel composite retained at
Art/Candidates/Haimatsu-Snow/Haimatsu_Snow_ExactComposite.png. Master pixels/size
remain unchanged; final in-game snow appearance is still pending acceptance.

Accepted-form derivatives (author rule, 2026-10-05): after a tree's shape is
accepted, generate derived states without renewed pre-generation approval.
Retain its established form/scale/branch identity and shared style as appropriate
to the state. This authorization does not replace final in-game appearance review.

These rules are the canonical visual baseline for AMJE tree/plant retextures. Individual species may add stricter requirements, but they should not redefine the shared style from scratch.

### Shared style

AMJE tree/plant sprites inherit the project-wide invariants in Ancient-Medieval-Japan-Project `Docs/ArtStyle.md`: silhouette-first simplification, strong readable outlines, restrained palette/detail, no photographic or painterly surface treatment, and readability at gameplay scale.

Environment-specific additions/controlled differences are:

- use stronger structural deformation/simplification than botanical-plate realism;
- use a clearly readable near-black or very dark local-color outer outline;
- keep saturation low-to-medium against RimWorld terrain;
- use limited base/shadow/highlight steps; **restrained soft gradient variation is allowed for tree/plant volume**, which is an explicit class difference from the flatter Core crop/item budget;
- use smooth high-resolution edges and transparent backgrounds;
- do not bake in scenery, UI, labels, terrain tiles, turf/grass bases, unrelated ground clutter, or winter snow.

These additions narrow the shared AMJ style for Environment vegetation; they do not replace it.

### Species differentiation

Species must differ through structure, not only through recoloring.

Use a combination of:
- overall silhouette;
- crown width/height;
- crown density;
- branch visibility;
- leaf/needle mass size and arrangement;
- trunk color, thickness, and visible proportion;
- branching rhythm;
- upright versus spreading growth habit.

A recolor of an otherwise identical silhouette is not sufficient for a distinct tree species.

### Trunk and branch treatment

- Use trunk/branch color and shape as a species cue, but keep bark treatment simplified.
- Express bark mainly through **color blocks, value changes, trunk thickness, and branching structure**, not fine etched texture.
- Sparse/open crowns should expose more branch structure; dense evergreen crowns may hide more of it.
- Branch tips must remain thick/simple enough to survive RimWorld zoom reduction.

### Seasonal continuity

For deciduous trees:
- keep the normal leafy source in neutral healthy foliage rather than baking in autumn/red/orange seasonal color when Vanilla seasonal tinting already provides that behavior;
- leafy and leafless states must still read as the **same individual species and approximate tree form**;
- preserve corresponding trunk placement, crown width, overall height, and major branch identity between the two states;
- do not make the leafless state look like a separate species or a differently scaled tree.

Evergreen base textures must likewise avoid baked winter snow; snow/weather remain runtime systems.

### Ground contact

- End the sprite at the trunk/root/branch silhouette on transparency.
- Decorative green circles, turf pads, grass rings, and similar base markers are not part of the AMJE tree style.
- Ground contact should read through the actual plant form rather than an added platform.

### Category baselines

#### Evergreen broadleaf trees

- comparatively dense/heavy crown;
- broad foliage masses;
- stable visual weight;
- avoid evenly spaced round blobs or ornamental symmetry.

Sudajii is the current accepted baseline for this category.

#### Deciduous broadleaf trees

- generally more open than the evergreen-broadleaf baseline;
- allow visible gaps and readable branch structure;
- species identity should remain clear in both leafy and leafless states.

Japanese beech is the current accepted baseline for this category.

#### Tall conifers

- retain a clear vertical/conifer identity;
- use layered, bundled, or irregular needle masses rather than perfect bilateral symmetry;
- avoid an oversized decorative "Christmas tree" silhouette;
- remain wide/readable enough not to collapse into a thin line at gameplay zoom.

Shirabiso should use this category while remaining distinct from Vanilla Pine.

#### Low/dwarf conifers

- read immediately as low and spreading rather than as a scaled-down tall conifer;
- favor horizontal, creeping, wind-shaped, or matted growth;
- keep the silhouette clearly wider than tall when appropriate to the species;
- do not imply a normal timber-tree form when the gameplay/ecological role is shrub-like.

Haimatsu is the current accepted target for this category.

### In-game readability

Final human review is performed at normal gameplay zoom. Prioritize:
- silhouette/species distinction;
- scale relative to neighboring vegetation;
- outline weight;
- palette/saturation against actual terrain;
- branch/foliage readability;
- UI icon readability;
- leafy/leafless continuity where applicable.

Fine details that are visible only when the PNG is enlarged are not a reason to increase complexity.

### Prohibited / strongly discouraged directions

Unless a later asset-specific decision explicitly overrides them, avoid:
- botanical-plate realism;
- photographic rendering;
- pixel-art/dithered treatment;
- excessive gradients or many small color steps;
- recolor-only species differentiation;
- decorative ground bases;
- highly symmetrical ornamental tree shapes;
- background/scenery baked into the sprite;
- saturation high enough to detach the plant visually from the map.

### Baseline references

Use the currently accepted Sudajii and Japanese beech work as the first comparison point for future tree retextures:
- Sudajii establishes the evergreen-broadleaf baseline: dense chunky crown, muted palette, thick outline, no ground ring.
- Japanese beech establishes the deciduous-broadleaf baseline: more open/spreading crown, visible branches, pale grey-beige trunk, restrained lighter foliage, matching leafy/leafless identity.

Future tree work should first state its category and the specific structural differences from these accepted baselines before source-art generation begins.

### Sudajii

Visual role:
- dense warm-temperate evergreen broadleaf canopy;
- broad rounded crown;
- dark glossy foliage;
- visually heavier/darker than Japanese beech;
- trunk/branch structure should remain readable beneath the crown.

Accepted ENV-010 production treatment:
- use the strongest simplified/deformed **C-direction** from the in-game comparison;
- foliage masses are broad and chunky, with fewer internal clusters than the earlier Sudajii pass;
- use a clearly thicker, near-black outer outline so the tree reads at the same visual weight as the current AMJ crop sprites;
- remove the small green ground/grass ring entirely; the sprite must end at the trunk/root silhouette on transparent background;
- keep the hue relationships from the accepted comparison, but lower foliage saturation by roughly one visual step so the tree sits naturally against RimWorld's brown/grey terrain and existing muted vegetation;
- especially suppress the brightest yellow-green highlights and the orange saturation of the trunk while preserving value contrast;
- do not reduce contrast by thinning the outline or flattening all shading; simplification comes from larger shape masses and fewer color steps.
- **Accepted in-game on 2026-10-04:** the final 256x256 Sudajii sprite is approved for production after WarmTemperate visual review; scale, silhouette, muted palette, thick outline, transparent base, and removal of the green ground ring are all accepted.

Avoid:
- European oak silhouette;
- tropical palm appearance;
- highly symmetric ornamental tree shape;
- decorative turf, grass clumps, or a circular ground base baked into the sprite;
- high-saturation lime highlights that make the tree appear pasted on top of the terrain.

### Japanese beech — leafy

Visual role:
- broad cool-temperate deciduous canopy;
- lighter/open crown than Sudajii;
- smooth pale-grey trunk impression;
- compatible with Vanilla fall-color shader behavior.

The base art should use neutral healthy foliage rather than permanently orange/red autumn colors, because seasonal color is supplied dynamically.

Accepted source-art direction:
- visibly more open and horizontally spreading than Sudajii;
- smaller foliage masses with branch structure visible between them;
- smooth pale grey-beige trunk rather than warm orange-brown bark;
- low-saturation yellow-green/olive foliage, lighter than Sudajii;
- thick near-black outline and simplified AMJ crop/Sudajii visual weight remain shared;
- no baked ground/grass base;
- leafy source sprite approved by the author on 2026-10-04 and integrated at `Textures/Things/Plant/AMJ/Beech/Beech_A.png`;
- **Focused in-game review accepted on 2026-10-05:** after the decoded-PNG repair in `1a24e11755c24c2109b7ceebd9628601c62f1f12`, the author reported no issues with the CoolTemperate focused check. The current leafy beech is accepted for production appearance. The report did not separately identify a forced leafless/seasonal-switch check; keep that specific coverage distinct from the confirmed focused appearance review.

### Japanese beech — leafless

Visual role:
- same approximate trunk/crown proportions as the leafy beech;
- branching structure should align closely enough that the seasonal switch does not look like a different species/size;
- no foliage.

Accepted source-art direction:
- preserve the leafy sprite's pale grey-beige trunk identity and overall crown width;
- expose a broad, irregular branching fan rather than a narrow upright silhouette;
- keep branch tips simplified and thick enough to remain readable at game zoom;
- **Historical/superseded:** leafless source sprite approved by the author on 2026-10-04 and integrated at `Textures/Things/Plant/AMJ/Beech_Leafless/Beech_Leafless_A.png`; in-game seasonal-switch validation remains pending. This earlier sprite is replaced by the 2026-11-01 author-accepted version recorded above; past in-game evidence does not verify the new geometry.

### Shirabiso fir

**Source approved 2026-10-05:** author approved the generated standalone evergreen sprite. Compact asymmetric layered conifer crown, muted grey/blue-green needle masses, grey-brown exposed trunk, thick dark outline, transparent background without ground/snow. Production texture: `Textures/Things/Plant/AMJ/Shirabiso/Shirabiso_A.png` (256x256 RGBA). SHA-256: `aaef0a8db34426e028fdebe4a69efd3aadbc17ed98875e1f74c1bc2da4147d3d`. This is a distinct species silhouette; no shared fixed component is declared. **Focused in-game appearance accepted 2026-10-05:** the author replied `OK` after the Subalpine focused-check instruction. Record reported appearance acceptance; this does not supply a new automated runtime log or full-suite result.

Visual role:
- narrow subalpine evergreen conifer;
- layered but compact conical crown;
- colder/darker needle mass than ordinary broadleaf trees;
- visually distinct from Vanilla Pine rather than merely recoloring it.

Avoid:
- oversized Christmas-tree symmetry;
- extremely tall narrow sprite that becomes unreadable at game zoom.

### Haimatsu dwarf pine

**Current production accepted in game 2026-10-05:** author accepted the further-deformed five-mass Haimatsu with `採用で`. Production: `Textures/Things/Plant/AMJ/Haimatsu/Haimatsu_A.png` (256x256 RGBA), SHA-256 `a20cac361b08b19b0892d2dcdf88bf40257bf186d3661661941e090f19bf18b6`, drawSize=2.60. Thick dark outlines, coarse rounded foliage pads, simplified woody branches and restrained shading; transparent/no ground/snow. Focused Alpine appearance is accepted; dedicated winter/snow appearance review is not claimed. No shared fixed component is declared.

Visual role:
- low, spreading, wind-shaped alpine shrub;
- clearly wider than tall;
- creeping/matted pine structure rather than a miniature upright tree;
- no wood-harvest visual implication.

This is the most important silhouette distinction for keeping the alpine zone above the treeline.

## Retextured plant description policy

This section is the ENV-010 application of the AMJ-wide historical description policy in Ancient-Medieval-Japan-Project `Docs/HistoricalDescriptionGuidelines.md`.

Every plant whose in-game artwork is replaced by AMJE must also receive an AMJE-authored description so the visual pass does not retain text that conflicts with AMJE's tone, environmental role, or ancient/medieval Japanese historical context. This is a minimum requirement for the art scope; the shared AMJ policy is broader and also applies to Vanilla/MO items, plants, and animals that AMJ explicitly adopts, patches, selects, or localizes.

This applies to:
- AMJE-owned structural plants;
- Vanilla tree Defs retextured by AMJE;
- Medieval Overhaul tree Defs conditionally retextured by AMJE when MO is active.

Text changes are presentation/localization only. They must not imply mechanical changes that AMJE does not actually make, and MO-owned plants must retain their original gameplay/fantasy identity even when their wording is rewritten for consistency.

Authoring workflow:
1. write the Japanese description first;
2. have the author review and approve the Japanese wording;
3. only after Japanese approval, translate that approved text into English;
4. keep the English version semantically aligned with the approved Japanese source rather than independently rewriting it.

Labels should remain unchanged unless there is a separate concrete naming issue. The current requirement is to replace unsuitable or tonally inconsistent descriptions, not to rename every retextured plant.

Japanese opening-name rule (shared AMJ policy): when an established kanji form exists, put it at the start of the description; include recognized aliases / alternate names and common alternate written forms there as well. Do not invent kanji or weakly sourced names. Current structural-plant name evidence: Forestry and Forest Products Research Institute identifies ブナ as 山毛欅 with aliases シロブナ / ホンブナ; Forestry Agency material records 橅 / 椈 as alternate writings, スダジイ aliases イタジイ / ナガジイ, シラビソ alias シラベ, and ハイマツ as 這松. Dictionary references record シラビソ as 白檜曽. English text is translated from the approved Japanese opening rather than independently normalized.

Historical-description research basis for the current four-plant Japanese draft:
- **Sudajii:** FFPRI identifies it as a warm-temperate evergreen canopy tree, records the aliases イタジイ / ナガジイ, and lists timber, bark-tannin/dye and edible-seed uses. Vegetation-history research places Castanopsis / evergreen broadleaf forest in western Japan by the Early Jomon; the Nara National Research Institute cultural-property database records Early-Jomon use of Castanopsis fruits at the Ireibaru site in Okinawa.
- **Japanese beech:** FFPRI identifies ブナ as 山毛欅, aliases シロブナ / ホンブナ, and a principal cool-temperate deciduous tree. Vegetation-history research records Fagus in Jomon cool-temperate forests. A Nagano archaeological report records Fagus-genus wood among selectively used turned wooden vessels / lacquerware from the late Heian to medieval period. Forestry Agency material documents modern interior/furniture, mushroom-log and fuel uses.
- **Shirabiso:** University of Tokyo and FFPRI sources identify 白檜曽 / シラビソ（シラベ） as a major subalpine evergreen conifer. Current Forestry Agency / Environment Ministry descriptions of Mt. Ontake record Shirabiso-dominated subalpine forest, while scholarship on Ontake worship places organized medieval worship / ascent in the later medieval period. The draft therefore describes the forest as part of the mountain-religion landscape without claiming a specific timber use.
- **Haimatsu:** Forestry Agency and Ministry of the Environment sources identify 這松 as a representative alpine dwarf pine above the treeline. Botanical-history research treats its Japanese alpine distribution as a northern cold-climate lineage left at high elevation after postglacial warming. Current Mt. Ontake vegetation includes Haimatsu above the treeline; the historical sentence separately notes the mountain's later-medieval worship history rather than claiming direct medieval use of Haimatsu.

Key references:
- FFPRI Sudajii: https://www.ffpri.go.jp/kys/business/jumokuen/jumoku/zukan/sudajii.html
- FFPRI distribution maps: https://www.ffpri.go.jp/labs/prdb/sudazii.html , https://www.ffpri.go.jp/labs/prdb/buna.html , https://www.ffpri.go.jp/labs/prdb/sirabiso.html
- FFPRI beech name: https://www.ffpri.go.jp/fsm/business/jumokuen/06_ha/buna.html
- Early-Jomon Castanopsis use (Nabunken): https://heritagemap.nabunken.go.jp/statistic/64887-%E4%BC%8A%E7%A4%BC%E5%8E%9F%E9%81%BA%E8%B7%A1.html
- Jomon vegetation / wood use: https://doi.org/10.4116/jaqua.36.329
- Medieval turned-wood evidence (Nagano archaeological report): https://sitereports.nabunken.go.jp/files/attach/9/9472/7427_1_%E5%B1%8B%E4%BB%A3%E9%81%BA%E8%B7%A1%E7%BE%A4.pdf
- Forestry Agency beech modern use: https://www.rinya.maff.go.jp/kanto/joetu/invitation/invitation/shinetutrail.html
- Mt. Ontake vegetation: https://www.rinya.maff.go.jp/chubu/policy/business/conservation/hogorin/2-20.html
- Later-medieval Ontake worship: https://doi.org/10.57492/sangakushugen.42.0_5
- Haimatsu alpine ecology: https://www.rinya.maff.go.jp/tohoku/syo/huzisato/zukan/haimatu.html
- Alpine-flora history / Haimatsu: https://doi.org/10.18942/bunrui.KJ00004872189

## Thin Soil technical direction

`AMJ_ThinSoil` reuses Vanilla `Terrain/Surfaces/Gravel` without adding a separate texture. Its author-accepted tint is `(1.00, 0.93, 0.80)`: light, low-saturation warm grey-beige, intended to look drier and visibly different from unmodified grey Gravel instead of suggesting fertile dark soil. This is a Def color-only change; the established fertility 0.50, generated terrain rules, edge behavior, and save identity remain unchanged. **2026-10-17 author visual acceptance:** the author checked the changed terrain color in the actual game and reported "実ゲーム確認した OK". The acceptance applies to this tint with the existing Gravel texture. Capture conditions were not supplied; no independent native test, new standalone terrain texture, or Steam-distribution validation is implied.

Final texture should:
- tile seamlessly;
- read as shallow brown-grey soil with visible small stones;
- remain visually distinguishable from Vanilla Gravel and ordinary Soil;
- avoid looking like constructed flooring;
- remain low-contrast enough that pawns/plants/items stay readable on top;
- work with the existing `FadeRough` edge behavior.

The final fertility remains 0.50; art must communicate poor natural ground without looking completely barren.

## World-biome texture direction

World textures should distinguish the four AMJ bands at globe zoom without becoming saturated map icons.

### Warm-temperate forest
- deep evergreen green;
- slightly humid/dense visual impression;
- warmer green than Subalpine.

### Cool-temperate forest
- medium natural green;
- lighter/more neutral than Warm-temperate;
- must be visibly distinguishable from Warm-temperate because both currently reuse Vanilla TemperateForest.

### Subalpine forest
- dark cool blue-green / conifer impression;
- denser/cooler than Cool-temperate.

### Alpine zone
- muted grey-green/brown;
- sparse vegetation impression;
- should read as high-elevation land, not snow/ice biome.

Do not bake winter snow into world-biome textures; temperature/weather already control seasonal map behavior and the world texture represents biome identity rather than current-day weather.

## Proposed paths

AMJE plant folders:

- `Textures/Things/Plant/AMJ/Shii/`
- `Textures/Things/Plant/AMJ/Beech/`
- `Textures/Things/Plant/AMJ/Beech_Leafless/`
- `Textures/Things/Plant/AMJ/Shirabiso/`
- `Textures/Things/Plant/AMJ/Haimatsu/`

Vanilla retexture folders:
- `Textures/Things/Plant/AMJ/Retexture/TreeOak/`
- `Textures/Things/Plant/AMJ/Retexture/TreeOak_Leafless/`
- `Textures/Things/Plant/AMJ/Retexture/TreeMaple/`
- `Textures/Things/Plant/AMJ/Retexture/TreeMaple_Leafless/`
- `Textures/Things/Plant/AMJ/Retexture/TreeBirch/`
- `Textures/Things/Plant/AMJ/Retexture/TreeBirch_Leafless/`
- `Textures/Things/Plant/AMJ/Retexture/TreeWillow/`
- `Textures/Things/Plant/AMJ/Retexture/TreeWillow_Leafless/`
- `Textures/Things/Plant/AMJ/Retexture/TreePine/`
- `Textures/Things/Plant/AMJ/Retexture/TreeBamboo/`

For any additional loaded state discovered during integration, use the same AMJE-owned family with an explicit state suffix, e.g. `TreeOak_Immature`, `TreeWillow_Polluted`, `TreePine_Immature`, rather than falling back to the original/broad-pack art. The exact folders are created only for states that the loaded target Def actually uses.

Medieval Overhaul conditional retexture folders:
- `Textures/Things/Plant/AMJ/Retexture/MO/GreatOak/`
- `Textures/Things/Plant/AMJ/Retexture/MO/GreatOak_Leafless/`
- `Textures/Things/Plant/AMJ/Retexture/MO/GreatIter/`
- `Textures/Things/Plant/AMJ/Retexture/MO/GreatIter_Leafless/`
- `Textures/Things/Plant/AMJ/Retexture/MO/GreatFir/`
- `Textures/Things/Plant/AMJ/Retexture/MO/GreatWillow/`
- `Textures/Things/Plant/AMJ/Retexture/MO/GreatWillow_Leafless/`

Terrain:

- `Textures/Terrain/Surfaces/AMJ_ThinSoil.png`

World:

- `Textures/World/Biomes/AMJ_WarmTemperateForest.png`
- `Textures/World/Biomes/AMJ_CoolTemperateForest.png`
- `Textures/World/Biomes/AMJ_SubalpineForest.png`
- `Textures/World/Biomes/AMJ_AlpineZone.png`

The corresponding Def paths should only be switched after each actual asset exists, so the repository never points to missing textures.

Vanilla path replacements should live in an AMJE-owned retexture patch and point to AMJE-owned unique texPaths. Medieval Overhaul path replacements must be isolated in an optional compatibility/retexture patch guarded by MO's package ID. No MO source asset is copied or edited; AMJE supplies its own replacement textures and changes only the loaded graphic paths when both mods are active. Same-name texture shadowing is not the canonical AMJE mechanism. If ReGrowth, VTE, or another active Mod explicitly patches the same loaded Def field, AMJE's ownership contract requires the final loaded path for an AMJE-owned target to resolve to the AMJE path; add only the narrow load-order/compatibility rule needed for that real conflict.

## Acceptance

Automated/static checks can verify:
- expected paths exist in Def XML;
- every owned non-empty loaded graphic state resolves to the expected AMJE path;
- no placeholder Vanilla/MO or broad-pack path remains on an owned state once that state is converted;
- optional MO patches are absent/harmless when MO is inactive;
- runtime loads without missing-texture ERRORs or BadTex.

Human visual review is still required for:
- scale;
- silhouette readability;
- tiling seams;
- color/contrast;
- whether Warm/Cool/Subalpine/Alpine are distinguishable on the world map.

This is one of the few project stages where manual visual review is intentionally required.

## Adopted style baseline

The author accepted the final ENV-010 atlas direction after iterative comparison.

The accepted characteristics are:
- smoother/high-resolution rendering than the rejected pixel-like pass;
- stronger simplification/deformation than the initial more naturalistic concept;
- lower saturation than the initial concept, but without shifting the established warm/cool hue relationships;
- rounded, readable foliage masses and simplified branch/trunk shapes;
- enough internal shading to remain game-art rather than flat iconography;
- consistent treatment across AMJE, Vanilla retextures, Medieval Overhaul retextures, low vegetation, and world/terrain examples.

The atlas itself is a **style/reference board**, not a shippable sprite sheet. Production assets are generated or painted individually from this baseline so that transparency, crop, scale, and RimWorld path structure can be controlled per Def/state.

## Fixed reused components

When a future Environment asset intentionally reuses a visible component pixel-exactly, follow Ancient-Medieval-Japan-Project `Docs/GoldenPaths/FixedImageTemplates.md`. Same-style but structurally distinct species do not use fixed-pixel templates.

### Revision 5 native appearance accepted; leafless snow complete (2026-10-06 JST)

Author: `ブナOKなのでPRマージして`. This explicitly closes the separate native-appearance review for the already frozen revision-5 leafless-beech snow source. Keep exact composite SHA-256 `24a10f6a45ed200170a5dabf49cabd2bacecfa3bfaaa1679300e64204b933126`, installed snow overlay SHA-256 `3cc4da672499e8bb7f96847623793050ab136b766cd91713a7921eda66dc496b`, and leafless master SHA-256 `24f8664bdedd3ebd0dee58fe627439b3784b5ecc599aaf49d750832f315be112`. Template v5 is ACTIVE with the approved filled exemplar. The Beech snow ledger row is accepted for both leafy and leafless variants; all current AMJE structural-plant visual states are complete. This approval authorizes PR #5 merge but does not fabricate a new runtime-test PASS.

## 2026-10-30 ダケカンバ葉あり積雪画像の妥協採用（原画・視覚のみ）

作者は元の葉あり画像の粗い葉塊に近づけた再生成版 `a_clean_high_quality_stylized_illustration_on_a_d.png` を「妥協採用」とした。以前の細かな葉版は採用しない。画像の更なる再生成は現時点では不要だが、積雪量・葉形には改善余地がある。

- **採用原画:** `Art/Sources/Things/Plant/AMJ/Dakekanba_Leafy_Snow/Dakekanba_Leafy_Snow_Source.png`。1024×1536 RGBA、1,918,063 bytes、SHA-256 `734b539452feac76dfa46996ed09b8a391a535a51dd9d79d398fc1ec4360a3f1`。採用されたPNGの原バイトを保持する。
- **256px確認用派生:** `Art/Candidates/Dakekanba_Leafy_Snow/Dakekanba_Leafy_Snow_256_Review.png`。256×256 RGBA、SHA-256 `38ab287f36603468af153e034d637cf848695959ea478bb6a3905d5398c5d784`。LANCZOS比例縮小と透明余白による別ファイル。
- **受入範囲:** 外観について妥協採用。元の葉あり樹木とのピクセル一致、季節切替、積雪位置、透過縁の実ゲーム表示、Def/Texturesへの本番接続は未検証・未実施。本追加はレビュー用原本保存であり、Workshop配布ではない。
