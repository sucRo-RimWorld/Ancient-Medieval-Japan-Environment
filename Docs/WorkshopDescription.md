# Steam Workshop Presentation — Ancient & Medieval Japan - Environment（中世日本 - 環境）

`README.md` is the detailed public-content source of truth. Workshop follows the author's finalized 2game organization: order the features by function (climate/weather → rivers/coasts → biomes → soil → vegetation → wildlife → wetland disease → descriptions), combine overlapping bullets, and preserve coverage of **all implemented player-visible change categories** even when one bullet covers several. Keep additional Workshop-only details such as wetland terrain retention and plant cutting yield when needed for complete coverage. Do not repeat a 'balance' section that merely duplicates features or compatibility. Structure the body as features → scope → compatibility → save compatibility → future candidates. The four unimplemented wetland-plant candidates are child bullets of one parent marked `（未実装）`; deferred retexturing is another marked parent. Retain the literal Japanese suffix in both language blocks. Distinguish candidates from approved work and never repeat withdrawn plans. No new feature claim may be introduced beyond README.

## Authoring order

Workshop descriptions are written **Japanese first**.

1. Update README with the detailed public content first, using Design as the internal factual/rationale source.
2. Draft and polish a concise Japanese Workshop summary from that README.
3. Treat the finalized Japanese section of the combined Workshop text as the wording/content source for English translation.
4. Translate that Japanese text into English without adding or removing substantive claims.
5. Keep both Workshop language versions synchronized with each other and substantively contained within README.

Source files:
- **Single paste-ready bilingual Workshop description:** `Docs/SteamWorkshopDescription.txt` (English body → one `[hr][/hr]` separator → Japanese body → **second `[hr][/hr]` separator** → shared gallery with bilingual captions). Paste this complete file **only in Steam's English description field**; do not duplicate it in the Japanese description field.
- Maintain Japanese wording in the Japanese section of `Docs/SteamWorkshopDescription.txt`; no separate Japanese-only text file is needed.

## Publication responsibility

Repository work ends with preparing and synchronizing the README, Japanese/English Workshop BBCode, and referenced Mod graphics. **The author performs the actual Steam Workshop publication/update manually**, including pasting the description, setting images, and confirming the live page. Do not treat a GitHub description update as a Steam-side update until the author confirms publication.

## Title

- Canonical English/Japanese display title: `Ancient & Medieval Japan - Environment（中世日本 - 環境）`

## Release stage

RimWorld 1.6 / 1.7 (unstable) — Beta

## Next feature release — wetland vegetation, target 0.2.0 (author correction 2026-10-17)

The author corrected the version intent: the **four new wetland plants** (Ashi/Yoshi, Suge, Hannoki and Mizugoke) constitute a substantial **feature update targeting `0.2.0` from currently tracked `0.1.1`**, **not `1.0.0`**. “Major update” referred to the size of the content, not semantic-version major release. The current **0.1.1** About/Manifest/Changelog metadata is *not* advanced in anticipation of unfinished content. Version number and Beta/stable maturity are independent release decisions. When actually releasing, synchronize `About/About.xml`, `About/Manifest.xml` and `About/Changelog.txt` at the same version, then prepare accurate bilingual change notes; do not publish or imply a Workshop update merely because this policy exists.

For the 0.2.0 Workshop copy:

1. First update README to reflect the *completed* four-species PlantDefs, approved descriptions/graphics and real gameplay. Move those species from "Future candidates（未実装）" to implemented features only after they are production-complete, and keep later Sugi/Susuki and deferred retextures separately marked as future work.
2. Add **one actual, approved and reachable image per wetland species**, with a short **English / Japanese bilingual caption for each**, to the **existing last gallery** `[h2]Representative Plants / 各環境帯の代表植物[/h2]` in `Docs/SteamWorkshopDescription.txt`. Append them **after the current four structural-plant entries** within that *same* gallery; do not create a new gallery, duplicate images in the separate English/Japanese body blocks, remove older plant images, or use placeholder/broken URLs. For Mizugoke, which temporarily shares Vanilla `Plant_Moss` art, use a real verified public image (e.g. an accepted in-game screenshot), never invent an AMJE production PNG URL. Explicitly disclose the temporary image reuse **only in the Workshop caption**, not in the in-game plant description or other public surfaces.
3. Summarize the newly implemented four wetland plants in both language feature lists and their wetland subsection, and remove the corresponding obsolete planned-items parent/children. Edit the Japanese section of `Docs/SteamWorkshopDescription.txt` first, then synchronize English, 2game and About only from verified finished gameplay. Keep the bilingual combined BBCode below Steam's 8,000-byte limit and pass `Tests/validate_workshop_description.py`.
4. The image/description captions should use the existing concise educational style. Draft content grounded in the already author-approved Japanese descriptions at `Docs/NativeVegetationStep3Design-ja.md` §4 (English alignment in §4.1), without claiming new usable crafting recipes:
   - **Ashi / Yoshi — 葦（アシ、ヨシ）:** Reedbeds along rivers and shallow wetlands; reeds have long supplied thatching material. / 川辺や水の多い低地に群生する草本。古くから茅葺き屋根の材料に利用されました。
   - **Suge — 菅（スゲ）:** Sedges of wet grassland and fens; their leaves were used for traditional hats and rain capes. / 湿地や低層湿原に生える草本。葉は古くから笠や蓑の材料に利用されました。
   - **Hannoki — 榛の木（ハンノキ）:** A deciduous tree of wet river lowlands; alder-genus wetland woods existed in prehistoric Japan. / 河川沿いの湿った低地に生える落葉高木。古い時代から日本の湿地林を形づくってきました。
   - **Mizugoke — 水苔（ミズゴケ類）:** Sphagnum of cool bogs, helping form peat as dead plant matter accumulates. **Its artwork temporarily reuses Vanilla moss graphics.** / 冷涼な湿原に広がるコケ類。枯れた部分の堆積を通して泥炭形成に関わります。**画像は暫定的にバニラの苔と同じものを使用しています。**

Until all publication prerequisites are real and verifiable, the existing paste-ready Workshop description remains a description of the present release: **do not pre-advertise unimplemented species or use unapproved images**. The Steam Workshop item remains private unless the author makes a separate visibility decision.

## Public wording rules

- Japanese Workshop copy uses established Japanese terms for general concepts. Prefer `バニラ`, `バイオーム`, `世界生成`, and `実行時` instead of mixing `Vanilla`, `Biome`, `WorldGen`, or `runtime` into Japanese prose.
- Keep English in Japanese copy primarily for official Mod names, proper names, abbreviations, identifiers, and useful official-name parentheticals such as `痩せた土壌（Thin Soil）`.
- Describe player-visible changes and information needed to decide whether to install the Mod. Do not promote image provenance, custom/AI artwork, or internal implementation technique as a feature.
- Representative-plant images may illustrate what the Mod adds, but wording such as “custom graphics” or “AMJE-authored artwork” is not a selling point.
- Avoid engine/internal terms such as `WorldGen`, `TileMutatorDef`, and `River / Coast mutator` in public copy unless a technical compatibility explanation truly needs them. Prefer user-facing wording such as “既存の世界生成を活用” and “バニラの河川・海岸生成と互換”.
- Clearly distinguish added content from reused or redistributed content: AMJE adds four biomes, four representative plants, and Thin Soil; river/coast/weather systems are reused or reconfigured rather than presented as newly added systems.
- Keep the hierarchy README (detailed) → Workshop (installation-focused summary) → 2game (shorter summary) → About.xml (brief overview). Do not add substantive claims only in a shorter surface.

## Short description

**Japanese source**

バニラの地形・植生・バイオーム構成を日本列島向けに置き換え・再構成する環境Mod。単体で中世日本風の自然環境を楽しめ、CCTO併用時は植物の寒冷耐性までよりリアリティ重視になる。

**English translation**

A standalone environment overhaul that replaces and reconfigures Vanilla terrain, vegetation, and biome composition for a Japanese-archipelago-inspired landscape. Works standalone, with optional CCTO integration for a stricter realism-focused plant cold model.

## Replacement-scope review

For public-copy updates, verify `Docs/Design.md` against `EnvironmentTerrainProcessor.Apply`, `JapanBiomeScoring`, biome plant/terrain Defs and the River / Coast handoff before synchronizing README -> Japanese Workshop -> English Workshop -> About / 2game. Replacement means the generated terrain and baseline biome/vegetation composition are reconfigured; it does not mean deleting all Vanilla Defs, replacing every specialized biome, or adding a separate local river/coast generator. Do not imply that secondary Vanilla plants are preserved by default: retained plants are provisional until their AMJE fit is confirmed, and unsuitable plants are removed or replaced as the audit proceeds. Preserve wetlands/MO coexistence, representative plants, optional integrations and save limitations. Check the **entire combined BBCode body** for balanced tags, exact Japanese source inclusion, English-before-Japanese ordering with separate horizontal rules at both the language and trailing gallery boundaries, unique image URLs and a total UTF-8 size below 8,000 bytes with both LF and CRLF. The repository validation is `python Tests/validate_workshop_description.py`; the Workshop payload CI runs it.

## Workshop content policy

Emphasize:
- replacement/reconfiguration of Vanilla terrain, vegetation and biome composition for medieval Japan, stated in the opening;
- distinguish the changed generated environment from reuse of RimWorld's existing world-generation and river/coast systems;
- why the four AMJ biomes are simplified vegetation/climate bands rather than prefectural or exclusive biome replacements;
- one representative structural plant for each band: Sudajii / Japanese beech / Shirabiso / Haimatsu, with concise bilingual captions;
- images for the four representative plants may be shown to identify the added plants;
- standalone use without AMJ Core;
- optional CCTO realism layer;
- Medieval Overhaul coexistence;
- save-compatibility limitations;
- completed wetland vegetation, wildlife, pack-animal, disease, weather and descriptions; retained-tree descriptions, sowing and cutting outcomes;
- unimplemented native wetland plant candidates grouped under a single `（未実装）` parent with four named child items, plus deferred Vanilla/MO retextures as a separate `（未実装）` parent;
- Beta status and continued Healroot supply, with medicinal plants outside Environment.

For the representative-plant section, keep the Workshop summaries shorter than the in-game historical descriptions. The Workshop should explain the vegetation-band design and provide a short educational note for each plant; the in-game descriptions retain the fuller kanji/alias/historical detail.

The four current Workshop images reference production PNGs directly through `raw.githubusercontent.com`. Include **each image exactly once** in the shared image gallery, not one copy per language. Place the English and Japanese labels and captions together for each image. If a production path changes, update the Workshop image URL at the same time.

When the Workshop body names another mod, provide a direct link at least at its first or dependency-list mention. Prefer its Steam Workshop page when published; use the owning GitHub repository for an AMJ mod that has no public Workshop item yet. Keep the **combined English + Japanese + image gallery** below Steam's 8,000-byte limit rather than repeating the same long URL on every occurrence. Omit license, AI-production descriptions and donation requests from the Steam copy; preserve legally required repository/distribution notices.

The future-candidates section must keep unfinished wetland plants and deferred Vanilla / Medieval Overhaul tree retextures visibly distinct from implemented features, without listing every plant as a separate promised development. README remains the detailed source for scope, prior vegetation audits and technical compatibility policy.

Keep detailed world-generation numbers, full test results, exact plant cold-tolerance values, implementation details, and research rationale in README / Design rather than the Workshop body.

Do not add manually maintained build-version numbers or test-count summaries to the Workshop description.
