# Seasonal appearance prior-art audit (2026-10-31)

Owner: AMJ Environment. Scope: date-based graphics for Yamazakura, Ume, Kuri, Kaki, Susuki without new game mechanics or mandatory external dependencies.

## Bounded implementation-entry comparison

- Vanilla RimWorld 1.6 / optional Medieval Overhaul: stock leafless/snow visuals and fall shaders, no XML calendar-defined species-specific blossoms, autumn fruit or grass flowerheads. Preserve normal Plant behavior and existing game balance; MO is not needed for drawing.
- Existing AMJE `AMJ_WildPlants.xml` and `PlantFacingPatches.cs`: base deciduous/snow variants and selected-plant orientation, but no timed bloom state. Preserve all currently implemented trees.
- VE: VEF `Plant_Blooming`, `BloomingPlantExtension`, `Plant_WinterBlooming` sources inspected 2026-10-31: https://github.com/Vanilla-Expanded/VanillaExpandedFramework/blob/main/Source/VEF/Plants/Plants/Plant_Blooming.cs and https://github.com/Vanilla-Expanded/VanillaExpandedFramework/blob/main/Source/VEF/Plants/Plants/Plant_WinterBlooming.cs . `Plant_Blooming.Graphic` excludes `LeaflessNow`; this blocks bare-branch Ume blossom if reused as-is. VPE Flowers adds weed/flower-care gameplay and extra content, not needed for four native trees: https://steamcommunity.com/sharedfiles/filedetails/?id=3748351639 .
- Non-VE: Cherry Blossom (2025, 1.5 catalog) adds petal-harvestable sakura, not verified for 1.6 five-species timing: https://catalogue.smods.ru/archives/351392 . Gauranlen - Cherry Blossom Colors (1.6) recolors Ideology trees rather than calendar-driven Yamazakura/Ume: https://rimworld.2game.info/detail.php?id=2552921263 . OverflowingFlowers (1.6) adds ground flowerbeds, not seasonal tree appearance: https://rimworld.2game.info/detail.php?id=3005103112 .

## Decision / limitations

Use a small AMJE-owned opt-in DefModExtension and Harmony rendering hook, allowing Ume blooming while leafless, preserving Vanilla leafless/snow graphics outside assigned windows and never hard-requiring VE/VEF, DLC, MO or CCTO. Reuse concepts but copy no external code/assets. Unlike the external alternatives, this keeps only the approved natural vegetation and has no extra costs, harvest outputs or progression effects. Current evidence is bounded: non-VE current binaries and MO loaded runtime not inspected, no blanket absence claim, 1.6/1.7 live graphics and save behavior untested. No unapproved image paths, Biome weights, dates, or production PlantDefs should ship.
