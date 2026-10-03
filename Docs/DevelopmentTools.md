# Environment Development Tools

## Build

Harmony is required.

Default RimWorld path:

`D:\SteamLibrary\steamapps\common\RimWorld`

Run from the repository root:

`build.bat "D:\SteamLibrary\steamapps\common\RimWorld"`

The build script:
- locates RimWorld 1.6 `Assembly-CSharp.dll`;
- locates `UnityEngine.CoreModule.dll` and `netstandard.dll`;
- locates Harmony from Steam Workshop item 2009463077;
- compiles `Source/AncientMedievalJapanEnvironment/*.cs`;
- writes `Assemblies/AncientMedievalJapanEnvironment.dll`.

## First runtime smoke

After a successful build, start RimWorld with:
- Harmony;
- Ancient & Medieval Japan: Environment;
- no other world-generation/biome overhaul for the first smoke.

Create several worlds and check:

1. startup/world generation completes with no AMJ Environment error;
2. Large/Huge rivers do not naturally appear;
3. Creek/River tiles are visibly more common than Vanilla;
4. land annual mean temperatures stay within -8 C to 20 C;
5. highland temperatures fall with elevation;
6. Flat terrain is reduced while usable settlement sites remain common enough;
7. coastlines are visibly more indented rather than merely losing land area;
8. the world still contains coherent natural biomes after Environment adjustment.

## Climate/CCTO validation

For representative southern, central, northern and highland tiles, inspect actual outdoor temperature through the year.

Reference thresholds:
- growth: 10 / 8 / 5 / 0 C;
- cold death: -1 / -4 / -8 C.

The first balance goal is qualitative:
- southern lowlands: long warm season; barley normally safe;
- central lowlands: rice/millets stop in winter; barley generally safer;
- northern/highlands: normal annual crops cannot safely overwinter outdoors.

Do not retune CCTO crop thresholds to compensate for Environment world-generation errors. Adjust Environment climate first.
