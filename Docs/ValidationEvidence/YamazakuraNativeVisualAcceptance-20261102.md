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

- **Pre-feature populated save migration: PASS (bounded Vanilla 1.6).** [Project generic Actions #41571798116](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/41571798116) tested exact Environment head `52baec9bff68d81abbb94d3acbc280f39ff5b5d4`. Original native Artifact `amj-manual-native-41571798116` (ID `13202225867`), `manual-native-result.json`/`upgrade` logs independently confirm the real *staged older Environment product* had no Yamazakura Def, created a populated `.rws`, then the candidate loaded it, saved an upgraded `.rws` and reloaded it in a separate process. The old Shii ID `7350` and Beech ID `7351` survived both loads with growth >0.9, Yamazakura candidate Def loaded, strict Unity ERROR0 and exit0. Original save 2,235,235 bytes SHA-256 `cf190c2674bd4b039b44f8b7a5ef08fce6f42eab61d792d6e5a7a4b825125e51` remained unchanged; upgraded save 2,177,099 bytes SHA-256 `3569645d8aaaa81d53a95bbb55ab944926b0e4d6b72cac6b2e6e733d3a0d04dd`. This is **one real pre-feature staged-product save**, not an actual user's full historic save collection or all possible Workshop versions.
- **UI runtime capture: PASS; author acceptance recorded 2026-11-03 below.** Actual 32/64px `Widgets.DefIcon` and `Dialog_InfoCard` original 1.6 frames are preserved under [generic run #41573790164](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/41573790164), but the author has not approved these newly displayed images. See the dated detail below.
- This is owner-specific 1.6 Vanilla screenshot/cold evidence; it does not certify all four provider profiles, 1.7, Workshop subscriber bytes, or independently approve climate/forestry numerical balance. All previously accepted 4-profile harvest checks remain limited to their recorded scope.
- Image acceptance **does not** authorize PR #299 merge or Workshop publication without the other required owner gates.

## Generic shared-fixture failure and focused UI retest (2026-11-03)

The same generic native run [#41571798116](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/41571798116) is **overall FAIL**, despite its independently successful save migration. The original `visual/icon.unity.log` shows the mature snowy tree resolving to the approved `Things/Plant/AMJ/Yamazakura_SnowFull` **base** graphic, actual material valid. The shared test erroneously demanded the normal `Yamazakura` base plus a separate snow overlay, rejected the two snow-depth-1 specimens, and emitted `AMJ_PLANT_VISUAL_READY verified=False`. It produced **no icon.png or infocard.png**, so neither UI appearance nor author approval is established. No production sprite or tree behavior was changed.

Project [PR #1203](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/1203) fixed that **generic** test expectation in merged main commit `f661ed197a3d92057ec100a0dcad3f4ffcfe3254`: an actual full-tree snow graphic may appear as the Plant base graphic, or the expected snow path may appear in an overlay over a normal base. The configured texture suffixes, source SHA, no-snow normal path and non-BadTex check remain required. The fix itself is **not a native test result**.

For the **now-completed** single affected-case retest, `Tests/PlantNativeAcceptance.json` selected only `icon` and `infocard` with `run_cold=false` and `run_save=false`. The old-save result above is already established for the unchanged owner production source. The skipped check is **not** waived for future production/save-affecting changes: a new relevant source must re-register/run a corresponding old-save case when necessary. No repeat of accepted four screenshots, existing cold transition, or the already passing saved-world migration is authorized merely to repair the UI screenshot test.

## Focused genuine UI icon and InfoCard PASS — 2026-11-03

- **Source:** Environment PR #299 exact `1ed33b9d3376c48a294aeb00ebceace57d22001c`; no production sprite, XML, Def or C# change from prior actual rendered art/save source.
- **Engine:** Project [generic `plant-appearance16` Actions #41573790164](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/41573790164), project main after shared snow-base fix `f661ed197a3d92057ec100a0dcad3f4ffcfe3254` and offline contract repair `530fdc4df29d178cd266b6227eee4d7bb5abcce1`. Actual isolated RimWorld **1.6.4871 rev600, Vanilla only**.
- **Original same-run Artifact:** `amj-manual-native-41573790164` (ID `13202413436`), `visual/icon.png` SHA-256 `9875840599f7e2b8ee40e7a06a568f76861f66c1643a4b494eaae8e455b3f3b5`; `visual/infocard.png` SHA-256 `9848120a24b1c8098f851db75fa73b863f40cebe567117d6411d31a3d05f4172`. Both 1280×720 actual X11 framebuffer PNGs independently rehashed to match `manual-native-result.json`.
- **Actual verified scene:** `icon` uses `Widgets.DefIcon` at 32px and 64px in a test-only native window; `infocard` uses actual `Dialog_InfoCard` for the production `AMJ_Tree_Yamazakura`. Each uses six native tree instances and confirms owner-specified normal and full-tree snowy graphics under real render material; noon/Clear/paused camera rootSize23; valid icon texture; `ready_marker=true`, `six_live_tree_states=true`, `actual_framebuffer_png=true`, Unity `error_count=0`, `passed=true`.
- **Result:** Preflight, source-pinned package build and both native UI scenarios SUCCESS, `matrix_pass=true`, `native_status=PASS`. This **supersedes the UI evidence gap** and the earlier false-negative screenshot path failure of #41571798116, while preserving those earlier failed run classifications. No save or cold scenario was run in this retest (`upgrade=null`), because the earlier independent source-pinned save upgrade PASS and cold/4-state visual proofs were unchanged.

The actual screenshot images were presented to the author in chat. The following acceptance supersedes the previously pending icon approval while preserving the native proof, historical save scope and prior test-failure history.

## Author acceptance of genuine UI icon / InfoCard — 2026-11-03

- **Decision:** After receiving the actual game screenshots of the 32px and 64px `Widgets.DefIcon` and production `Dialog_InfoCard` from [Actions #41573790164](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/41573790164), the author responded **「OK」** in the immediately following chat on **2026-11-03**. This approves the displayed native UI/icon appearance of `AMJ_Tree_Yamazakura`. The original images and SHA-256 values are recorded in the previous section; no resynthesis or art modification is authorized.
- **Canonical acceptance:** In `Docs/PlantVisualCoverage.json`, `AMJ_Tree_Yamazakura/icon.status=accepted`, `date=2026-11-03`, with this evidence, the explicit author response and current unchanged production source/icon fingerprint. The other five visual states were accepted on 2026-11-02 and `transition=verified` on bounded native calendar evidence; thus `complete=true` denotes all **seven defined ledger states**, not every possible real-game condition.
- **Boundaries:** This approval is *only* the displayed UI appearance. Source-pinned pre-feature save migration PASS is separately documented above. Original native failures retain FAIL status; the successful focused retest supersedes only their resolved UI expectation. Full-speed 60-day gameplay, every historical player save, other provider/version combinations, numerical ecology and any Workshop upload remain separate and are not implied.
