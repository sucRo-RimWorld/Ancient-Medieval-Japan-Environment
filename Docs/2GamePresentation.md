# RimWorld Mod データベース（2game）掲載方針 — AMJE

共通正本はProjectの[ModDescriptionGuidelines.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/ModDescriptionGuidelines.md)。形式の基準はCCTOの[2GamePresentation.md](https://github.com/sucRo-RimWorld/RimWorld-Crop-Cold-Tolerance-Overhaul/blob/main/Docs/2GamePresentation.md)と[日本語説明](https://github.com/sucRo-RimWorld/RimWorld-Crop-Cold-Tolerance-Overhaul/blob/main/Docs/2GameDescription-ja.txt)。

貼り付け用の管理元は `Docs/2GameDescription-ja.txt`。READMEを詳細な公開内容の正本とし、Workshopよりさらに短く要約する。新しい機能・互換性の主張を追加しない。

常体・1文1情報・短い箇条書きを使う。見出しは `▼ `（半角スペース付き）とし、短い要約 → 特徴 → バランス方針 → 対応範囲 → 対応・互換性 → セーブ互換性 → 今後の予定、の順に揃える。BBCodeやMarkdownの装飾は使わない。

Harmony・CCTO・Medieval Overhaulは2game詳細ページへ直接リンクする。AMJ Coreは公開2gameページのIDが管理資料にないためGitHubへリンクする。AMJE自身のGitHubリンクは末尾に1回記載する。

AMJEの2game掲載ページIDは本リポジトリでは未記録。IDを推測せず、掲載確認後に記録する。実際のサイト投稿・更新とGitHub上の説明準備は別の状態として管理する。

冒頭でバニラの地形・植生・バイオーム構成を日本列島向けに置き換え・再構成することを明示する。既存の世界生成やバニラの河川・海岸生成を活用することと、生成結果そのものを変更することを区別する。副次植生については「バニラだから残す」と書かず、現在の採用可否監査と、不適切な植物を除外・置換している実態を反映する。特殊バイオームの共存は保持する。

## 表記・情報選別ルール

- 日本語の一般語は日本語で書く。`Vanilla` は `バニラ`、`Biome` は `バイオーム`、`WorldGen` は `世界生成`、`runtime` は `実行時`を基本とする。
- 英語を残すのは正式なMod名、固有名詞、略称、URL、識別に必要な正式名称を基本とする。`Thin Soil` のような公式英語名は、日本語名の補助として括弧内に併記してよい。
- 2game本文では内部実装用語を避け、利用者から見える挙動で説明する。河川・海岸については `River / Coast mutator` ではなく「バニラの河川・海岸生成と互換」と書く。
- 「追加した要素」と「既存要素の分布・挙動を再構成した部分」を混同しない。痩せた土壌は新規地形として明記し、礫地等は自然土壌バランスの変更として説明する。
- 制作手法や画像の出自はアピールポイントにしない。「独自画像」「自作グラフィック」「AI生成」等ではなく、プレイヤーに影響する機能だけを記載する。
- 2gameはWorkshopよりさらに短い導入判断用説明とし、内部実装・制作工程・詳細な研究説明は載せない。

## 更新時の確認

READMEの機能・対応範囲・追加と削除のセーブ条件・現在進行中の植生監査・予定を確認し、日本語説明へ必要な差分だけ反映する。6見出しの順序、常体、関連MODの直接リンク、AMJE自身のGitHubリンク、完了済み除外と今後の予定を混同していないことを確認する。Workshop日英とAbout.xmlも内容の整合を確認する。
