# Steam Workshop Presentation — Ancient & Medieval Japan - Environment（中世日本 - 環境）

`README.md` is the detailed public-content source of truth. The Workshop description is a concise summary of that README: it should select and compress the information players need to understand, evaluate, and install the mod, but it must not introduce substantive features, design rationale, or compatibility claims that are absent from the README.

## Authoring order

Workshop descriptions are written **Japanese first**.

1. Update README with the detailed public content first, using Design as the internal factual/rationale source.
2. Draft and polish a concise Japanese Workshop summary from that README.
3. Treat the finalized Japanese Workshop text as the wording/content source for Workshop localization.
4. Translate that Japanese text into English without adding or removing substantive claims.
5. Keep both Workshop language versions synchronized with each other and substantively contained within README.

Source files:
- **Single paste-ready bilingual Workshop description:** `Docs/SteamWorkshopDescription.txt` (English body → one `[hr][/hr]` separator → Japanese body → **second `[hr][/hr]` separator** → shared gallery with bilingual captions). Paste this complete file **only in Steam's English description field**; do not duplicate it in the Japanese description field.
- Japanese wording source: `Docs/SteamWorkshopDescription-ja.txt`. Its text must match the Japanese section in the combined file; it is not an additional Steam upload.

## Publication responsibility

Repository work ends with preparing and synchronizing the README, Japanese/English Workshop BBCode, and referenced Mod graphics. **The author performs the actual Steam Workshop publication/update manually**, including pasting the description, setting images, and confirming the live page. Do not treat a GitHub description update as a Steam-side update until the author confirms publication.

## Title

- Canonical English/Japanese display title: `Ancient & Medieval Japan - Environment（中世日本 - 環境）`

## Release stage

RimWorld 1.6 — Beta

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
- replacement/reconfiguration of Vanilla terrain, vegetation and biome composition for Japan, stated in the opening;
- distinguish the changed generated environment from reuse of RimWorld's existing world-generation and river/coast systems;
- why the four AMJ biomes are simplified vegetation/climate bands rather than prefectural or exclusive biome replacements;
- one representative structural plant for each band: Sudajii / Japanese beech / Shirabiso / Haimatsu, with concise bilingual captions;
- images for the four representative plants may be shown to identify the added plants;
- standalone use without AMJ Core;
- optional CCTO realism layer;
- Medieval Overhaul coexistence;
- save-compatibility limitations;
- Beta status, the active Vanilla / Medieval Overhaul vegetation-retention audit, and the rule that only retained plants proceed to staged retexturing.

For the representative-plant section, keep the Workshop summaries shorter than the in-game historical descriptions. The Workshop should explain the vegetation-band design and provide a short educational note for each plant; the in-game descriptions retain the fuller kanji/alias/historical detail.

The four current Workshop images reference production PNGs directly through `raw.githubusercontent.com`. Include **each image exactly once** in the shared image gallery, not one copy per language. Place the English and Japanese labels and captions together for each image. If a production path changes, update the Workshop image URL at the same time.

When the Workshop body names another mod, provide a direct link at least at its first or dependency-list mention. Prefer its Steam Workshop page when published; use the owning GitHub repository for an AMJ mod that has no public Workshop item yet. Keep the **combined English + Japanese + image gallery** below Steam's 8,000-byte limit rather than repeating the same long URL on every occurrence. Omit license, AI-production descriptions and donation requests from the Steam copy; preserve legally required repository/distribution notices.

The development-status section must state that Vanilla / Medieval Overhaul vegetation is already being audited for retention, with unsuitable plants removed or replaced before visual work. It should then summarize staged retexturing for the retained targets, covering the visible state family used by each plant. README remains the detailed source for the scope and technical compatibility policy.

Keep detailed world-generation numbers, full test results, exact plant cold-tolerance values, implementation details, and research rationale in README / Design rather than the Workshop body.

Do not add manually maintained build-version numbers or test-count summaries to the Workshop description.
