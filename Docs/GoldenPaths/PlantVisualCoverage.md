# Plant visual coverage gate

`Docs/PlantVisualCoverage.json` is the authoritative per-state review ledger.
Coordination is a summary, not a substitute for this ledger. Source-art approval,
PNG/static validation, loaded-graphic validation and human appearance acceptance
are different evidence types. Never use one to satisfy another.

Before resuming plant art, choosing the next target, reporting completion or
handing off, run `python Tests/validate_plant_visual_coverage.py` and read every
pending row. Before claiming that all five implemented AMJE plants are complete, run it with
`--require-complete`; pending rows make that command fail. Ordinary PR/CI validation
runs without `--require-complete`: pending image reviews are expected during
incremental development and cannot by themselves block merging an unrelated or
partially completed change. Release/whole-plant completion still requires the
explicit strict check; missing records, stale approvals and invalid evidence
continue to fail routine CI. Existing-tree
retextures remain deferred by the author's instruction. Finish pending current
plant states before proposing terrain or unrelated art.

Every plant needs normal mature appearance, immature appearance, UI icon and
snow-state review. For AMJE deciduous plants the leafy state retains snow artwork,
whereas the leafless state intentionally renders **no branch snow** via the
transparent shared `Things/Plant/AMJ/LeaflessNoSnow` graphic (author decision
2026-11-01). Check ground snow separately from tree snow and verify no fallback
to a leafy snow canopy. Deciduous beech additionally needs leafless,
autumn color and actual leafy/leafless seasonal-transition review. Evergreen
leafless/seasonal-transition rows are explicitly N/A with the evergreen reason.
Keep the accepted normal sprites intact. Use the game's snow rendering with
species-aligned snow overlay assets where required. Ground snow alone does not
verify snow on foliage/branches; resolve the actual snow graphic path first.
Absence of a required overlay is an implementation gap, not a passed review.

Accepted rows require an explicit author statement, date, evidence location and
the reviewed production fingerprint. Fingerprints retain image hashes and the owning visual/gameplay definition including size,
but exclude player-facing label/description text, UI-only fields outside icon review, and snow paths outside snow review.
Localization or historical-description edits therefore do not force a repeated visual review. Changes to visual state, size,
owned graphics, or other guarded definition fields still invalidate only the affected states.
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

**2026-11-01 Hannoki leafy-snow correction:** Native #41267149034 checked `SnowOverlayGraphic` as registered but did not prove a visible snowy crown. The right leafy Hannoki remained yellow-green. The author's direct screenshot objection supersedes the assistant's previous incorrect PASS. Need distinguish source overlay sparseness from DDS orientation/render issues and verify a corrected visual result. This does not reopen the snowless leafless decision or change the existing accepted leafy base sprite.

**2026-11-01 latest native acceptance:** [real 1.6 screenshot and logs](Docs/ValidationEvidence/LeaflessSnowRemovedNative16-20261101.md) verify Beech/Hannoki snow-free bare branches over snowy ground, while Beech leafy snow remains visible. **Correction: Hannoki leafy snow is NOT visually discernible in the same frame; the author rejected the previous PASS. Keep Hannoki/snow pending until visually corrected and revalidated.** The author's selected Beech leafless art and 2026-11-01 corrected DDS orientation were separately verified. The 2026-10-13 native Beech cold-transition mechanics are carried forward only because the underlying seasonal logic and temperature fields are unchanged; new screenshots demonstrate loaded endpoint art, not a repeated dynamic full-year simulation. Old branch-snow art is now archival and not an outstanding production task.

**Historical 2026-10-17 Hannoki completion scope (superseded only for leafless snow by 2026-11-01 decision):** the author accepted the six
static native appearance states on 2026-10-15 (normal, immature scaling,
UI, forced autumn, forced leafless, leafy/leafless snow). The forced autumn
and leafless screenshots alone did not prove temporal mechanics. A separate
isolated real RimWorld 1.6 native run now proves the same test-placed production
Hannoki's original calendar-driven fall-factor change, natural leaf loss,
leaf recovery and non-BadTex materials without forcing color, temperature or
leaflessness: [Actions #39016607046](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39016607046).
Retain the initial failed fixture evidence and the accepted static fingerprints.
The `transition` entry is now `automated_native_calendar_sampling` / verified
with its own source fingerprint; seven-of-seven **defined visual coverage
states** are now satisfied, without demanding new art or author review of
already accepted sprites. This accelerated hourly observer does not validate
a full-world season-long game simulation, real wetland spawn or distributed
Workshop binaries. Read the precise independent evidence in
`Docs/ValidationEvidence/HannokiSeasonalNative-20261017.md` and original
static acceptance in `Docs/ValidationEvidence/HannokiVisualAcceptance-20261015.md`.


## Controlled color review

### Autumn LUT and source-color calibration

The native plant shader shifts HSV by destination-minus-source; it does not
replace every leaf pixel with the lookup color. A source key darker and more
saturated than the real foliage leaves that brightness/saturation difference
in the autumn output. Diagnose source HSV, destination HSV and rendered output
separately before darkening accepted normal artwork. Keep the key hue/range
unchanged when only value/saturation calibration is authorized. Shader override
and synthetic numeric readback are investigative evidence, not a replacement
for testing the final loaded production Def and file-based LUT.

Plant.Print can use an atlas replacement material. A prototype changing only
the original Graphic material can produce an unchanged rendered comparison.
Verify actual output differences and use the real atlas material for disposable
probes; final validation must load the accepted PNG/DDS through the normal
content loader, without destination/source material overrides.

For this horizontal 256x256 opaque LUT, preserve the exact accepted PNG under
`Art/Sources/Other`, run `python Scripts/Art/export_fall_gradient.py`, then
`python Tests/validate_fall_gradient.py --self-test`. The uncompressed RGBA DDS
is supported by the installed RimWorld 1.6 loader and preserves every decoded
pixel. Never leave a stale sibling DDS when revising the PNG. Validate actual
loaded texture pixels/source parameters and retain noon/Clear screenshots,
complete logs and the runtime ERROR gate. Preserve earlier non-autumn art
acceptance when source art, size and state paths are unchanged and native
fall=0 remains identical; record technical revalidation rather than inventing
a fresh human approval. Seasonal mechanics still require their scoped native
evidence when the ledger fingerprint changes.

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

Native individual leafless thresholds vary from growth-minimum minus18 to
minus10C. The observer selects the highest threshold among existing healthy
mature outdoor beech, using age only as a tie-break, so its state demonstration
does not depend on an arbitrary first/youngest cold-resistant individual.
Never move the climate, change a tree's threshold or force a state to satisfy
the same-tree leafless/recovery assertions. Log the selected native threshold
and sampled minimum temperature; retain any failed earlier attempt separately.

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

For beech snow comparisons set RIMWORLD_AMJE_SNOW_REVIEW=1 and RIMWORLD_AMJE_BEECH_SNOW_REVIEW=1. AMJPlantGrowthReviewQuickstart shows leafy beech north row and native cold-leafless beech second row, each at depths 0/0.5/1. **Current behavior:** leafy Beech/Hannoki snow overlays remain as approved; the leafless-specific path resolves to fully transparent `LeaflessNoSnow` without fallback. Verify a real native screenshot shows a bare tree without branch snow on snow-covered ground; check the resolved graphic and `Tests/validate_beech_snow.py`. The superseded former `Beech_Leafless_Snow` production texture, mask and old appearance screenshots are archival only. Loaded graphics and static transparent-alpha evidence do not replace checking actual rendered behavior.

For actual UI icon review set RIMWORLD_AMJE_ICON_REVIEW=1 with AMJPlantGrowthReviewQuickstart and leave snow-review variables unset. Dedicated test window draws Widgets.DefIcon at 32/64 pixels using each live ThingDef; logs texture names/BadTex validity. Close the window to inspect north-to-south Shii/Beech/Shirabiso/Haimatsu and west-to-east growth 10/50/100%. The window exists only in DevQuickstarts; source images/production behavior unchanged. Runtime validity is separate from author appearance acceptance.

For Haimatsu native info-card review use RIMWORLD_AMJE_HAIMATSU_INFO_REVIEW=1 and unset icon/snow-review flags. The growth quickstart opens Dialog_InfoCard for an actual spawned Haimatsu. Assert live GenUI.IconDrawScale=1, drawSize=2.60 and visualSizeRange=0.45..0.75. Explicit uiIconPath reuses the accepted sprite and prevents GenUI from applying map drawSize to UI icons; do not compensate by changing map size.

Automatic seasonal transition mechanics may use verified status with automated_native_calendar_sampling evidence, current fingerprint and explicit scope; do not require repeated human approval for mechanics already verified automatically. This differs from accepted visual appearance. Preserve earlier leafless approval when the approved leafless image/size is unchanged and only leafy fall palette/snow/UI settings changed; inspect prior_acceptance before requesting another check.

## GitHub Actions routing and notification noise

The coverage workflow runs for pushes to main and for pull requests targeting
main. Feature-branch pushes do not also run a duplicate coverage job. Relevant
paths are the ledger, ArtDirection/snow plan, retained validation evidence,
Art templates/sources, textures, plant Defs, Tests, art scripts and the workflow
itself. Unrelated documentation and main Coordination updates do not trigger it.
Use workflow_dispatch for an explicit diagnostic run.

A draft PR validates inventory/evidence consistency and all existing production
texture/snow guards. Pending human reviews remain visible but alone are not a
failure while the PR is a draft. A ready PR still requires --require-complete;
ready_for_review is an explicit trigger so converting a draft cannot bypass the
completion gate. Any inventory, evidence, fingerprint or texture error fails
both draft and ready checks. This is not a replacement for author approval.

Concurrency retains the newest run per PR/ref and cancels superseded active
runs. Before publishing workflow edits, parse the YAML and check main-only push
routing, ready_for_review/converted_to_draft triggers, relevant path coverage,
and the two consistency/completion command conditions. Then confirm the actual
GitHub Actions run, including production texture/snow validation, succeeds.
Repair a failing gate before stacking unrelated changes on the same branch.
