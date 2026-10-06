# Plant visual coverage gate

`Docs/PlantVisualCoverage.json` is the authoritative per-state review ledger.
Coordination is a summary, not a substitute for this ledger. Source-art approval,
PNG/static validation, loaded-graphic validation and human appearance acceptance
are different evidence types. Never use one to satisfy another.

Before resuming plant art, choosing the next target, reporting completion or
handing off, run `python Tests/validate_plant_visual_coverage.py` and read every
pending row. Before claiming that all four AMJE plants are complete, run it with
`--require-complete`; pending rows make that command fail. Existing-tree
retextures remain deferred by the author's instruction. Finish pending current
plant states before proposing terrain or unrelated art.

Every plant needs normal mature appearance, immature appearance, UI icon and
snow-covered appearance review. Deciduous beech additionally needs leafless,
autumn color and actual leafy/leafless seasonal-transition review. Evergreen
leafless/seasonal-transition rows are explicitly N/A with the evergreen reason.
Keep the accepted normal sprites intact. Use the game's snow rendering with
species-aligned snow overlay assets where required. Ground snow alone does not
verify snow on foliage/branches; resolve the actual snow graphic path first.
Absence of a required overlay is an implementation gap, not a passed review.

Accepted rows require an explicit author statement, date, evidence location and
the reviewed production fingerprint. Fingerprints retain image hashes and the owning plant definition including size,
but exclude UI-only fields outside icon review and snow paths outside snow review.
Changes invalidate only affected states; unrelated UI/snow additions do not require
repeating accepted normal/growth reviews. Other definition and image changes remain conservative.
The validator rejects missing plants/states, unsupported N/A, missing evidence,
stale fingerprints and complete claims with pending rows. A successful ledger
validation means the records are consistent; it does not mean pending reviews
have passed. Its default output always lists the remaining work.

Normal-map approval without explicit snow, icon or immature review only satisfies
normal mature appearance. Loaded leafless graphics do not prove a visible
leafless tree or a successful seasonal transition. Automated count/BadTex/ERROR
checks accompany human review but do not replace visual judgment.

For focused game checks, allow only one RimWorld process. Check process inventory
before launch, wait for the owned test to exit before another launch, and leave
an unrelated running game untouched. Use isolated profiles and retained logs.
Log the selected plant's count, position and growth where possible.

After an author accepts a state, update only the corresponding ledger rows,
ArtDirection and main Coordination. Report completed states and remaining states
explicitly. Do not label the whole plant or ENV-010 DONE from one accepted state.

## Controlled color review

Color/palette review baseline is **local map time 12:00, Clear weather, paused**.
Set local solar time rather than assuming global/absolute noon is local noon.
Let the previous weather finish fading, then verify actual weather and local time.
Freeze time/weather throughout the comparison. Use the same map/terrain, zoom,
season, plant growth and seasonal shader intensity for before/after images.
Log actual conditions and retain the screenshot/log evidence. Record
`color_conditions` on a color-review ledger entry: `local_hour: 12`,
`weather: Clear`, `paused: true`, `weather_transition_complete: true`,
plus `season`, `shader_intensity`, `zoom`, `evidence`, and `verified: true`.
Use `review_kind: color` when accepting a palette review; autumn acceptance
always requires these controls. They must be measured, not merely requested.

Never infer source/palette errors solely from uncontrolled time/weather images.
Do not overwrite a palette to compensate for dusk, night, rain or fog lighting.
After controlled baseline acceptance, extra-condition checks may assess dusk,
rain and snow separately with recorded conditions; these do not replace baseline
color review. Older normal-appearance approvals remain historical evidence,
not retroactively controlled-color approvals. The pending beech autumn review
must be rerun under the fixed baseline before approving or changing its color.

## Beech leafless focused setup
After building, invoke AMJCoolTemperateTerrainQuickstart in an isolated profile with RIMWORLD_AMJE_BEECH_REVIEW=leafless. The helper selects a naturally generated mature beech, calls Plant.MakeLeafless(Cold,false), logs before/after state and resolved texture, checks for missing state/material and focuses the camera. Apply the runtime ERROR gate. Keep leafless human approval pending until the author reviews it; this forced-state setup does not satisfy the seasonal-transition row.

For autumn appearance, use the same isolated CoolTemperate Quickstart with RIMWORLD_AMJE_BEECH_REVIEW=autumn. The helper applies the installed game's PlantFallColors native debug override at full intensity (1), invokes its shader-global setup, keeps the selected tree leafy and logs live texture validity. The override exists only in the test process. Appearance review does not establish natural seasonal timing. Never change the production PNG to bake autumn colors.

### Automated noon/Clear setup
Set RIMWORLD_AMJE_COLOR_BASELINE=1 together with RIMWORLD_AMJE_BEECH_REVIEW=autumn. The developer Quickstart computes local ticks from longitude, moves within the current day to local noon, applies Clear with both current/previous weather and a completed transition, pauses and fixes camera rootSize=24. It logs the measured local hour, current weather, transition factor, pause, absolute tick/longitude, zoom, growth and fall intensity; a mismatch emits an ERROR. Check that log and the runtime ERROR gate before requesting color approval. Human changes to zoom/time/weather invalidate the comparison conditions and require recording a new baseline.

### Native seasonal sampling
AMJBeechSeasonalSampleQuickstart selects the coldest settlement-valid natural CoolTemperate tile in the generated world, then tracks one naturally generated mature outdoor beech across hourly calendar samples. It disables the fall override, refreshes native temperature caches and calls native Plant.TickLong. No temperature override or MakeLeafless call is used. Check its retained log with Tests/validate_beech_season_sample.py and the runtime ERROR gate. This is accelerated calendar/native-state sampling, not a full year of ordinary world simulation or human visual acceptance. Leafless behavior is temperature-dependent: a warm site can change autumn colors without crossing the tree's individual cold threshold, and foliage can recover when temperatures rise even before spring.

### Growth-stage comparison map
Build and launch AMJPlantGrowthReviewQuickstart using an isolated profile and retained log. This developer-only setup clears a small central soil plot and places four rows (north to south: Shii, Beech, Shirabiso, Haimatsu), with growth 0.1, 0.5, 1.0 from west to east. It validates all 12 live materials, fixes local noon/Clear/paused/zoom=18 and disables autumn tint for a normal-foliage comparison. Apply the runtime ERROR gate and verify the actual baseline log before human review. The youngest column may use inherited immature graphics; inspect the actual resolved texture rather than assuming a scaled mature image. No production Def/art or ordinary saves are edited. Human growth-stage acceptance remains a separate ledger update.

Set RIMWORLD_AMJE_SNOW_REVIEW=1 to reuse this layout for mature plants with snow depth 0, 0.5, 1.0 from west to east. Snow uses the native snow grid around each plant. Actual depths are logged; base-material validity does not by itself verify the visible snow overlay. Retain the runtime ERROR gate and human snow review.

Installed RimWorld 1.6 Plant.Print uses a strict snow depth >0.8 threshold for
plant overlays. Thus depth 0.5 shows ground snow but no plant snow; these columns
are below/above-threshold samples, not three degrees of foliage-snow coverage.
The overlay shares the base plane's X/Z center, square size, horizontal flip and
wind colors, with only a tiny altitude offset for layering. A misregistered cap
therefore needs asset alignment correction, not a world-position/drawSize workaround.

For installed Haimatsu snow, this mode explicitly validates the live
Plant.SnowOverlayGraphic material as well as the base material. Run
Tests/validate_haimatsu_snow.py to guard the separate overlay hash/transparent
256x256 PNG, base-master bytes and accepted drawSize. Coverage fingerprints include
owned snow-overlay PNGs. A loaded material is not proof of matching pixel alignment;
judge the native rendered overlap in game. Retain generated source and normalized
production source separately; use the exact-copy PNG installer after resizing.

After a snow alignment edit, inspect a direct alpha-composite of the actual
normalized production candidate over the unchanged master, not another generated
illustrative preview. The Haimatsu snow guard additionally rejects more than 1%
of visible snow pixels outside the master alpha silhouette and coverage outside
30..65% of the master. These species-specific guards preserve visible sides and
branches; they do not replace human judgment or apply automatically to other trees.

The >1% tolerance was superseded after the author still observed misregistration.
Current Haimatsu route: Scripts/Art/build_haimatsu_snow.py registers a binary
upper-foliage mask from the accepted master's five pads and reuses generated
snow material colors; only the variable snow layer is produced. Template registry
Art/Templates/Haimatsu-Snow-v1/template.json is REVIEW, not active/visually accepted.
Core fixed_template implementation is an unmodified fetched snapshot under
Tests/Tools, with its Git blob/source/hash recorded beside it. Use its protected
RGBA validation: branch/outer-edge/side differences must be zero. The snow guard
now requires zero pixels outside the registered mask and alpha silhouette and
checks the saved exact-composite against the actual installed overlay. A global
alpha-overlap percentage alone cannot establish registration of individual caps.

Shirabiso uses Scripts/Art/build_shirabiso_snow.py with its own accepted master,
seven registered foliage regions and upper-column caps; it reuses the accepted
Haimatsu snow material palette only, not its shape. Registry is
Art/Templates/Shirabiso-Snow-v1/template.json. Tests/validate_shirabiso_snow.py
checks original master hash, exact production layer/composite, zero snow outside
mask, protected RGBA differences=0 and rejects a one-pixel shift. Registry stays
REVIEW until human snow acceptance. Snow-review Quickstart validates both owned
Haimatsu and Shirabiso live overlays; its north-to-south third row is Shirabiso.

Shii snow follows Scripts/Art/build_shii_snow.py: foliage-pixel mask registered to five fixed crown anchors, curved upper caps, original branch/edge RGBA protected. Registry Art/Templates/Shii-Snow-v1/template.json remains REVIEW until acceptance. Tests/validate_shii_snow.py checks original master hash, actual installed layer/composite, mask confinement/protected pixels and a one-pixel shift regression. The first/northern comparison row is Shii; native overlay visibility remains snow depth >0.8. Do not substitute a generated full-tree preview for the retained exact composite.

Shii shape correction supersedes five geometric crown anchors: the author rejected their cutoffs. Current builder traces seven actual upper-lit foliage masses from master pixels. The validator rejects crown_anchors metadata returning and checks the actual installed mask/composite. Pixel registration alone did not guarantee natural-looking cap shapes; human snow-shape judgment remains mandatory.

Publication worktrees must sit below a container without About/About.xml, e.g. Mods/_AMJ_PublishStaging/Environment, rather than directly in Mods. Hidden directory names are still scanned by RimWorld and cause duplicate PackageIds. Use git worktree move to preserve registration.

For beech snow comparisons set RIMWORLD_AMJE_SNOW_REVIEW=1 and RIMWORLD_AMJE_BEECH_SNOW_REVIEW=1. AMJPlantGrowthReviewQuickstart shows leafy beech north row and native cold-leafless beech second row, each at depths 0/0.5/1. Native SnowOverlayGraphic selects leaflessSnowOverlayGraphicPath automatically. Verify logged leafless state and actual snowTexture before asking for appearance review. Run Tests/validate_beech_snow.py for source/registration checks.

For actual UI icon review set RIMWORLD_AMJE_ICON_REVIEW=1 with AMJPlantGrowthReviewQuickstart and leave snow-review variables unset. Dedicated test window draws Widgets.DefIcon at 32/64 pixels using each live ThingDef; logs texture names/BadTex validity. Close the window to inspect north-to-south Shii/Beech/Shirabiso/Haimatsu and west-to-east growth 10/50/100%. The window exists only in DevQuickstarts; source images/production behavior unchanged. Runtime validity is separate from author appearance acceptance.

For Haimatsu native info-card review use RIMWORLD_AMJE_HAIMATSU_INFO_REVIEW=1 and unset icon/snow-review flags. The growth quickstart opens Dialog_InfoCard for an actual spawned Haimatsu. Assert live GenUI.IconDrawScale=1, drawSize=2.60 and visualSizeRange=0.45..0.75. Explicit uiIconPath reuses the accepted sprite and prevents GenUI from applying map drawSize to UI icons; do not compensate by changing map size.

Automatic seasonal transition mechanics may use verified status with automated_native_calendar_sampling evidence, current fingerprint and explicit scope; do not require repeated human approval for mechanics already verified automatically. This differs from accepted visual appearance. Preserve earlier leafless approval when the approved leafless image/size is unchanged and only leafy fall palette/snow/UI settings changed; inspect prior_acceptance before requesting another check.
