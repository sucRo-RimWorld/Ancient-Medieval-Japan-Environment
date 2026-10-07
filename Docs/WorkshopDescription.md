# Steam Workshop Presentation — Ancient & Medieval Japan - Environment

`README.md` is the detailed public-content source of truth. The Workshop description is a concise summary of that README: it should select and compress the information players need to understand, evaluate, and install the mod, but it must not introduce substantive features, design rationale, or compatibility claims that are absent from the README.

## Authoring order

Workshop descriptions are written **Japanese first**.

1. Update README with the detailed public content first, using Design as the internal factual/rationale source.
2. Draft and polish a concise Japanese Workshop summary from that README.
3. Treat the finalized Japanese Workshop text as the wording/content source for Workshop localization.
4. Translate that Japanese text into English without adding or removing substantive claims.
5. Keep both Workshop language versions synchronized with each other and substantively contained within README.

Source files:
- Japanese: `Docs/SteamWorkshopDescription-ja.txt`
- English: `Docs/SteamWorkshopDescription.txt`

## Publication responsibility

Repository work ends with preparing and synchronizing the README, Japanese/English Workshop BBCode, and referenced Mod graphics. **The author performs the actual Steam Workshop publication/update manually**, including pasting the description, setting images, and confirming the live page. Do not treat a GitHub description update as a Steam-side update until the author confirms publication.

## Title

- English: `Ancient & Medieval Japan - Environment`
- Japanese working title: `古代・中世日本 - 環境`

## Release stage

RimWorld 1.6 — Beta

## Short description

**Japanese source**

バニラの地形・植生・バイオーム構成を日本列島向けに置き換え・再構成する環境Mod。単体で中世日本風の自然環境を楽しめ、CCTO併用時は植物の寒冷耐性までよりリアリティ重視になる。

**English translation**

A standalone environment overhaul that replaces and reconfigures Vanilla terrain, vegetation, and biome composition for a Japanese-archipelago-inspired landscape. Works standalone, with optional CCTO integration for a stricter realism-focused plant cold model.

## Replacement-scope review

For public-copy updates, verify `Docs/Design.md` against `EnvironmentTerrainProcessor.Apply`, `JapanBiomeScoring`, biome plant/terrain Defs and the River / Coast handoff before synchronizing README -> Japanese Workshop -> English Workshop -> About / 2game. Replacement means the generated terrain and baseline biome/vegetation composition are reconfigured; it does not mean deleting all Vanilla Defs, replacing every specialized biome, or adding a separate local river/coast generator. Preserve secondary Vanilla plants, wetlands/MO coexistence, representative plants, optional integrations and save limitations. Check both BBCode bodies for balanced tags and UTF-8 size below 8,000 bytes with LF and CRLF.

## Workshop content policy

Emphasize:
- replacement/reconfiguration of Vanilla terrain, vegetation and biome composition for Japan, stated in the opening;
- distinguish the changed generated environment from reuse of WorldGen and River / Coast mutators;
- why the four AMJ biomes are simplified vegetation/climate bands rather than prefectural or exclusive biome replacements;
- one representative structural plant for each band: Sudajii / Japanese beech / Shirabiso / Haimatsu, with the selection rationale;
- current production images for those four plants in the Workshop body;
- standalone use without AMJ Core;
- optional CCTO realism layer;
- Medieval Overhaul coexistence;
- save-compatibility limitations;
- Beta status and the completed AMJE structural-plant artwork/state variants, including snow overlays and beech leafless/autumn states; broader Vanilla / Medieval Overhaul tree retextures are post-publication follow-up.

For the representative-plant section, keep the Workshop summaries shorter than the in-game historical descriptions. The Workshop should explain the vegetation-band design and provide a short educational note for each plant; the in-game descriptions retain the fuller kanji/alias/historical detail.

The four current Workshop images reference the repository's production PNGs directly through `raw.githubusercontent.com`. If a production path changes, update the Workshop image URL at the same time.

When the Workshop body names another mod, provide a direct link at least at its first or dependency-list mention. Prefer its Steam Workshop page when published; use the owning GitHub repository for an AMJ mod that has no public Workshop item yet. Keep the description below Steam's 8,000-byte limit rather than repeating the same long URL on every occurrence.

The development-status section must also summarize planned post-Beta visual follow-up: staged Vanilla / Medieval Overhaul tree retextures covering the visible state family used by each target. README remains the detailed source for the scope and technical compatibility policy.

Keep detailed world-generation numbers, full test results, exact plant cold-tolerance values, implementation details, and research rationale in README / Design rather than the Workshop body.

Do not add manually maintained build-version numbers or test-count summaries to the Workshop description.
