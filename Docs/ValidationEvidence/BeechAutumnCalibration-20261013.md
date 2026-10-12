# Beech autumn calibration — author review and loaded candidate

## Accepted scope

On 2026-10-13 JST, the author answered `ｙ` to proceeding with the right-hand
value-and-saturation-calibrated appearance as the adoption candidate. The same
normal beech PNG and candidate-v2 LUT were used in all three comparison panels.
The underlying accepted normal/leafless/snow masters and draw metadata are
unchanged. This acceptance is not a Steam upload or a complete Hannoki plant.

Source RGB is `(0.496758088,0.537254902,0.323263543)`; source hue and the existing
`(0.02,0.10)` transition range are preserved. LUT source/production PNG SHA256:
`1ccc50e0619cd7dfa40d6e874f27f9c3a6a78b4871cc5f0d5182098d218ce8f2`.
The source is archived under `Art/Sources/Other`; the preferred uncompressed
RGBA DDS decodes pixel-exactly to that PNG.

## Why the previous output was pale

The installed RimWorld `Custom/Plant cutout` pixel shader was extracted locally
and its D3D instructions inspected. Its HSV result is input plus the weighted
destination-minus-source difference. Current-source V=.4352/S≈.667 contrasted
with beech highlight167,166,88 (V≈.655/S≈.473), so the final image retained the
highlight's positive value and negative saturation offsets. The gradient's
194,109,51 sample rendered as244,147,112 for that highlight. Ten isolated native
GPU readbacks at intensity0/1 matched the derived math within1/255 per channel.
This is diagnostic shader evidence, not a substitute for file-loaded artwork.
No proprietary game shader binary is included in this repository.

## Actual comparison and preservation

Eight native screenshots measured local noon12, Clear, completed transition1,
paused, zoom24 and ticks15000. Cases: old/new source at fall0; current LUT old
source at fall1; candidate LUT old source, S-only, V-only and S+V at fall1;
S+V at fall0.5. Mature plants: three beech and three temporary Hannoki probes.
The original/corrected normal comparison region had0 changed pixels. Whole
screens differ in UI; a whole-screen identity claim is not made. Source-key
correction changes only autumn color conversion, not ordinary source pixels.

The subsequent installed-file candidate test did **not** call SetTexture or
SetVector to replace the fall destination/source. It loaded the changed XML and
DDS normally, logged source RGB0.496758074,0.537254930,0.323263556 (float32),
texture `AMJE_BeechFallGradient`, formatRGBA32, Trilinear, and source root
`D:/SteamLibrary/steamapps/common/RimWorld/Mods/AMJE.ColorReviewCandidate-20261013`.
GPU readback of all65536 loaded LUT pixels differed from the accepted PNG in0
pixels. Noon/Clear/paused/zoom24 normal and fall1 native screenshots completed.
The test used native intensity debug controls only, not production palette
material overrides.

Complete raw evidence is retained privately in the originating work session's
`output/AutumnReview/`: `SourceCalibrationEvidence/` holds the eight-case run,
and `ProductionLoadedEvidence/` the real file-loaded run. Both have complete
Player.log, independent Unity captureREADY/ERROR0, runner exit0, normal
ModsConfig/Prefs hash equality and cleanup of the owned fixtures. Public
`BeechAutumnCalibration-Review.log` and `-Loaded.log` contain scoped native
message excerpts only, without machine details or private runtime logs.
The public image is `BeechAutumnCalibration-BeechComparison.png`, cropped to
the beech row only. The provided Hannoki image and screenshots containing it
are retained locally and are not published by this PR.

## Seasonal scope

The existing native calendar sampling is rerun against the changed file-based
candidate; the retained result is `BeechAutumnCalibration-Season.log`. It
refreshes native temperature caches and calls native Plant.TickLong, without
temperature override or forced MakeLeafless. This is bounded hourly state
sampling, not a full ordinary-year simulation or existing-save proof.

The first selected native tree had threshold≈−17.99C while this map sample
reached only≈−13.59C: fall0..1 and textures passed, but leafless/recovery did not.
That failed report remains `FailedSeasonal1/`; it is not relabelled PASS.
The existing observer now selects the most cold-sensitive **existing natural
mature outdoor tree**, with age as tie-break. It does not change weather,
world seed, Defs, tree threshold or any gameplay setting; all four original
season assertions remain mandatory. The corrected result is recorded separately:
73/73 loaded assertions PASS, including native fall0..1, same-tree cold leafless
and recovery, and valid leafy/leafless materials. Minimum≈−13.59C crossed the
selected unchanged threshold≈−10.07C. Complete live report, independent Unity
ERROR0 and preserved normal config accompany the corrected run; the failed
earlier report remains.

## Remaining scope

Hannoki's provided master is retained exactly **locally**.
Its public binary transfer requires explicit authorization and remains OPEN.
Its color change was tested through a disposable beech-shaped definition only.
No Hannoki production Def, yield/drawSize, natural seasonal transition,
leafless/snow art or distribution is accepted by this result. Four-profile,
old-save and Workshop distribution gates remain separate. Prior non-autumn
beech author approvals are retained with explicit technical revalidation;
unchanged art is not described as newly human-reviewed.
