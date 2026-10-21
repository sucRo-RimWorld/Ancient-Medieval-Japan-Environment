# Translation maintenance / 翻訳者向け更新情報

This page tracks **upstream strings that community translation add-ons may need to update**. It is not a claim that a third-party translation has been checked, incorporated, or kept compatible with every AMJ Environment release.

AMJ Environment's upstream language support is English and Japanese. Community translations remain independently maintained; the main Mod does not depend on them. The upstream package ID is `sucro.ancientmedievaljapan.environment`.

## Where to obtain the text / 翻訳元

- **Approved Japanese wording:** [ThingDef localization](../Languages/Japanese/DefInjected/ThingDef/AMJ_WildPlants.xml), [BiomeDef localization](../Languages/Japanese/DefInjected/BiomeDef/AMJ_Biomes.xml) and [TerrainDef localization](../Languages/Japanese/DefInjected/TerrainDef/AMJ_NaturalTerrains.xml).
- **English labels and descriptions:** [wetland herbs](../Defs/ThingDefs_Plants/AMJ_WetlandHerbs.xml), [other AMJ plants](../Defs/ThingDefs_Plants/AMJ_WildPlants.xml), [biomes](../Defs/BiomeDefs/AMJ_Biomes.xml), and [terrain](../Defs/TerrainDefs/AMJ_NaturalTerrains.xml).
- **Vanilla definitions amended by Environment:** [Patches](../Patches/), including wetland and tree descriptions. These may require translation even when no new AMJ Def is introduced.
- **Release-level summary:** [About/Changelog.txt](../About/Changelog.txt). Read the source XML to identify actual display-string changes; a balance or graphics change does not necessarily add translation keys.

The `defName` and the `<DefName>.label` / `<DefName>.description` keys are identifiers, not words to translate. Preserve XML structure and the existing escaped `\n` paragraph separators.

## 0.2.0 — new strings compared with 0.1.1

The 0.2.0 wetland-plant addition introduces **four ThingDefs**, each with a new `.label` and `.description` localization entry in `Languages/Japanese/DefInjected/ThingDef/AMJ_WildPlants.xml`:

| Key prefix (both `.label` and `.description`) | Japanese name | English source name |
| --- | --- | --- |
| `AMJ_Plant_Yoshi` | アシ（ヨシ） | reed (ashi) |
| `AMJ_Plant_Suge` | スゲ | sedge (suge) |
| `AMJ_Tree_Hannoki` | ハンノキ | Alder (hannoki) |
| `AMJ_Plant_Mizugoke` | ミズゴケ | sphagnum moss (mizugoke) |

Corresponding English source texts live in `Defs/ThingDefs_Plants/AMJ_WetlandHerbs.xml` (Yoshi, Suge, Mizugoke) and `Defs/ThingDefs_Plants/AMJ_WildPlants.xml` (Hannoki). Use the full current descriptions rather than translating the short release-note summaries.

The same 0.2.0 changelog reports a **Thin Soil Growing Zone bug fix**, artwork updates and harvest behavior. Those are not additional ThingDef translation keys. No new harvested item name is added for reeds, sedges, alder or sphagnum: their outputs use existing Hay/WoodLog, or no output.

## Following future updates / 今後の更新の追跡

1. Check the version and latest changes in [About/Changelog.txt](../About/Changelog.txt).
2. Compare this release's `Languages/Japanese/DefInjected/`, English `Defs/`, and description-related `Patches/` with the previous version. Track **added, renamed, removed, and rewritten** `label`/`description` entries, including overrides of Vanilla Defs.
3. Keep translated XML keys identical to their upstream identifiers; retain supported `\n` escapes and verify the file as UTF-8 XML. For a separate RimWorld translation add-on, place the language-specific DefInjected files in the structure recognized by RimWorld for that language.
4. Validate the translated strings in-game under that locale. The upstream changelog does not establish a third-party add-on's compatibility.

For each future version that changes visible text, upstream maintainers should add a short **version-specific key delta** to this page alongside the source text changes. Do not change existing approved Japanese historical descriptions simply to ease translation; proposed source corrections undergo their own review.

## Independent community translation / コミュニティ翻訳

- [Chinese translation add-on (Steam Workshop ID 3823797121)](https://steamcommunity.com/sharedfiles/filedetails/?id=3823797121) — externally maintained and not bundled with AMJ Environment. Its present translation coverage and compatibility have **not been independently verified here**.

**日本語概要：** 翻訳者は上記の日本語 `DefInjected`、英語 `Defs`、Vanilla定義を変更する `Patches`、更新履歴を照合してください。0.2.0で新たに翻訳が必要になる主な表示文言は湿地植物4種の名前と説明です。外部翻訳Modの更新状況はAMJ本体では保証しません。
