# Wetland four plants — genuine native RimWorld 1.6 acceptance (2026-10-17)

**PASS:** [Project Actions #39034867997](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39034867997), artifact `amje-wetland16-39034867997` / ID `12102131507`.
Environment source SHA: `ea22a4fcb0454630d85eb48c9e88604cedc97497`. Fresh, source-verified **60-file** subscriber candidate built with actual Linux 1.6 production DLL.
ZIP SHA256: `08a10c42df2676072dfe1cd9e6e1020772202c9d7f70deec48efd303e77fe2a6`.
DLL SHA256: `9f35718fbc444a750aacc4d926c9075b5fbaee6882cf94430eee742a98cbd2ea`.

| Game profile | Actual native map | Assertions | Prelaunch errors | Unity errors | Exit | Result |
|---|---|---:|---:|---:|---:|---|
| RimWorld 1.6 Vanilla | TemperateSwamp | 82/82 | 0 | 0 | 0 | PASS |
| RimWorld 1.6 Vanilla | ColdBog | 73/73 | 0 | 0 | 0 | PASS |
| RimWorld 1.6 CCTO | TemperateSwamp | 82/82 | 0 | 0 | 0 | PASS |
| RimWorld 1.6 CCTO | ColdBog | 73/73 | 0 | 0 | 0 | PASS |

**310/310** exact current-source loaded-game assertions passed, all live Quickstart reports complete and untruncated. The older original Hannoki loaded wood-cut and natural-spawn assertions also passed.

## Actual native harvest and population evidence

- TemperateSwamp: Yoshi naturally spawned 5626 Vanilla / 5586 CCTO; of these 1052 / 1045 on shallow water. Suge naturally spawned 4327 / 4265 on other terrain. A real skilled pawn completed CutPlant, yielding Yoshi Hay2/Suge Hay1 Vanilla and Yoshi Hay3/Suge Hay1 CCTO. The CCTO Hay3 reflects normal pawn skill output adjustment, **not** a change to the author-approved base Yoshi yield 2.
- ColdBog: Yoshi 2148 Vanilla / 2166 CCTO naturally spawned, 748 / 774 on shallow water; Suge 3577 / 3606; Mizugoke 3994 / 3914. A real pawn completed native CutPlant on all three herbs, producing Yoshi Hay2 / Suge Hay1, and **no drop when removing Mizugoke** in both profiles.
- Production plant textures loaded without BadTex, approved loaded wildPlants commonality and wild-only status verified. This does not demonstrate precise proximity to land riverbanks, seasonal imagery, long-term saves, MO/MO+CCTO or 1.7 gameplay.

## Preserved failed precursor evidence

- [#39030166022](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39030166022): true native FAIL from Plants0 stochastic herb harvest (Hay0 some cuts); older temporary fixtures had disabled harvest failures.
- [#39034206328](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39034206328): actual cutting and all 82 assertions per warm map PASS with Plants20, but strict runtime FAIL with four `Nutrition == 0` / inherited `RawBad` prelaunch Def errors per profile.
- New approved production code adds `NeverForNutrition` to zero-nutrition Yoshi/Suge, without changing Hay output or harvest-failure mechanics; [#39034867997](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39034867997) confirms preLaunchErrors0.

**Release remains HOLD:** MO / MO+CCTO same-source 1.6, 1.7, required season/save continuity, Workshop bilingual copy/gallery, private owner-only uploader and downloaded Steam subscriber byte/runtime verification are separate unpassed gates. Existing known MO FMOD issue is classified under `Docs/ReleaseReadiness.md` only if exactly reproduced without new errors. No upload or 0.2.0 version bump was performed.
