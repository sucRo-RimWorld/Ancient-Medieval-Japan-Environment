# Yamazakura actual RimWorld 1.6 appearance acceptance — 2026-11-02

## Scope and immutable test source

- **Owner feature:** [Environment PR #299](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Environment/pull/299), original game-tested production tree source `138d3328d8ab50cb0c50f0498ef39fb142d21671`.
- **Native run:** [Project Actions #41416988103](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/41416988103), original same-run Artifact `amje-pr16-native-41416988103` (ID `13128680421`). Production build and native jobs both SUCCESS. Original RimWorld Linux 1.6.4871 rev600 in isolated Vanilla profile, Unity ERROR0. The previous [run #41410634376](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/41410634376) independently verified 84/84 loaded/sampled blossom + snow contracts for that same owner source.
- **Frame baseline:** actual 1280×720 Xvfb native game screenshots, local time 12:00, `Clear`, weather transition complete, paused, camera rootSize 23. For each of four modes, six actual Yamazakura specimens in two rows: mature, soil/no snow; mature, snow depth 1.0; immature growth 0.30, soil/no snow. No other graphics or production assets are substituted.
- **Per-mode preserved PNG identities:**
  
  | Frame | Original artifact file | SHA-256 |
  | --- | --- | --- |
  | Normal foliage | `normal.png` | `8ac58fd4562c77ed16686b9ce44ff301e5a8e0eaaf3a190c8f28f0ff7beb591b` |
  | Blossom (spring day 7) | `bloom.png` | `ecc5e6e32a8025735286128766fabb87c53a12e7e9b22858f837173f0eff0213` |
  | Autumn | `autumn.png` | `363972a7dd18d3e2fd7aeb4d987fb9b0d748c33b45fb29b7c07d4ad74bc2189b` |
  | Leafless | `leafless.png` | `57e9228dc2b415e6120b5ec3d911e2dd8948f709d40db4718cb5c4fcc6ef9eb9` |

The frame PNG SHA-256 values were verified against the original native `matrix-result.json`. Corresponding per-mode `.unity.log` records six valid source `Plant.Graphic` and loaded non-BadTex materials; all four capture gates PASS, no Unity error. Ground snow was deliberately laid in 9×9 review cells, so rectangular white areas in the full-frame screenshot are the test fixture, **not** a Yamazakura texture boundary.

## Author decision and adopted images

The user was shown the rendered four-mode comparison including normal, blossom, autumn and leafless trees with snow/no-snow and immature control, plus separate native cold results; the immediate response was **「ｙ」 on 2026-11-02**, confirming the presented four real-game appearances. Accept **normal**, **immature (growth 0.30)**, **snow** (normal-leafy and blossoming full-tree snow plus bare branches without snow), **autumn**, and **leafless** at the exact production fingerprint under `Docs/PlantVisualCoverage.json`.

- **Normal:** standard mature green foliage `Yamazakura` after the bloom period; dark trunk/root silhouette unchanged from accepted source.
- **Blossom:** `Yamazakura_Bloom` at the production Spring 6–8 window, observed Spring day 7; with snow depth 1.0 the full-tree `Yamazakura_Bloom_SnowFull` takes precedence. The blossom is an additional image represented under the **transition** and **snow** ledger evidence; not a separate ledger row.
- **Autumn:** `autumn.png` uses production `Yamazakura` base with shared `AMJE_BeechFallGradient`, **test-forced autumn shader intensity 1.0**, Fall day 8 (log reports day=7 zero-based), not an unforced seasonal color-time trace.
- **Leafless:** `leafless.png` is a **test-forced endpoint** for visual acceptance; `Yamazakura_Leafless` renders on bare ground and on ground snow, with the fully transparent `LeaflessNoSnow` overlay. Author's earlier compromise on bare-branch geometry is retained, not silently redesigned.
- **Immature:** `Growth=0.30` remains green using the original mature source at reduced scale even during flowering/leafless mature cases; no independent immature texture is introduced.
- **No UI-icon approval:** these frames show map-drawn trees, not the `Widgets.DefIcon`, architect/selection icon or `Dialog_InfoCard`. Keep **icon=pending** until actual UI capture/author review.

## Separate same-tree natural transition evidence

Same original [native run #41416988103](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/41416988103), `cold/quickstart.json` has four specifically named assertions with `passed=true`, Unity ERROR0. The `cold/unity.log` directly records `sameTree=True naturallyLeafless=True recovered=True validTextures=True minTemp=-12.97 maxTemp=29.43`; leaf loss and subsequent recovery occurred at sampled hours 959 and 1200. It used a naturally selected cold cool-temperate tile, one genuine production `Plant`, `Plant.TickLong` and native cached temperatures. No `MakeLeafless` or shader override in this cold case.

The earlier [run #41410634376](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/41410634376) verified same-tree accelerated native **normal→bloom→normal** during the specified three-day spring window and snow routing. Accordingly the **transition=verified** ledger row reflects the conjunction of these bounded real-engine **accelerated calendar sampling** observations, **not** normal-speed fully ticked 60-day playthrough, unforced rendered autumn color timing, or an actual save upgrade.

## Remaining non-visual requirements

- Historical populated **pre-PR save** compatibility, including loading that save with this candidate, saving it again and reloading while retaining legacy plants, is **NOT_RUN**. A new generated Quickstart map is not a pre-existing save.
- Actual UI icon/Info Card appearance acceptance is **NOT_RUN**.
- This is owner-specific 1.6 Vanilla screenshot/cold evidence; it does not certify all four provider profiles, 1.7, Workshop subscriber bytes, or independently approve climate/forestry numerical balance. All previously accepted 4-profile harvest checks remain limited to their recorded scope.
- Image acceptance **does not** authorize PR #299 merge or Workshop publication without the other required owner gates.
