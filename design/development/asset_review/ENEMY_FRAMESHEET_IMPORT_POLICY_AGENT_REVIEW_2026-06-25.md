# Enemy Framesheet Import Policy Agent Review

Packet date: 2026-06-25

Scope: independent review of the enemy framesheet split-sprite packet produced from the three existing P0 enemy animation framesheets.

## Reviewed Files

- `design/development/asset_review/ENEMY_FRAMESHEET_SLICED_SPRITES_2026-06-25.png`
- `design/development/asset_review/ENEMY_FRAMESHEET_SLICED_SPRITES_2026-06-25.csv`
- `design/development/asset_review/ENEMY_FRAMESHEET_IMPORT_POLICY_2026-06-25.md`
- `design/development/tools/build_enemy_framesheet_sliced_sprites.ps1`
- `design/development/tools/validate_enemy_framesheet_import_policy.ps1`
- `Assets/TheCat/Art/Enemies/Frames/*.png.meta`
- `Assets/TheCat/Art/Enemies/Frames/Sliced/*.png.meta`

## Visual Review

Reviewer: Pascal

Verdict: PASS with TODO caveats.

- Source consistency passed. The contact sheet keeps three coherent lanes: Black Mud crawler, Cold Light lamp-shadow caster, and Call Tyrant phone boss.
- Four useful frames per lane passed. The manifest has 12 split sprites, four per enemy lane, all 256x256 and marked `split_sprite_ready_pending_unity_runtime`.
- Transparency and centering passed for import testing. All split sprites have transparent margins; X-centering is stable enough for Unity import tests.
- Starter-cat and character replacement leakage passed. No starter-cat body, starter-cat framesheet, or character replacement art appears.

Visual TODOs:

- Keep Unity runtime acceptance open: import refresh, active enemy animation screenshot, prefab/catalog binding proof, and Console check.
- Validate pivot, playback, and hitbox intent for Call Tyrant frame 4 because it reads more like attack/VFX payoff than a standing boss-body frame.
- Verify no visible color drift in Unity source-lock review; alpha masks match, but visible RGB was not pixel-identical to source-sheet crops.

## Production QA Review

Reviewer: Ohm

Verdict: PASS after reproducibility fix.

- Source and split import settings passed. Source metas are Sprite/alpha; all 12 split metas are Sprite/alpha/max 256.
- Validator scope passed. The validator checks dimensions, alpha-capable sprites, transparent corners, meta tokens, CSV hashes, policy boundary wording, and character/starter path leakage.
- Matrix wording passed. The validation matrix says `pass_local_validator` and keeps runtime gates explicit; it does not claim Unity runtime acceptance.
- No starter-cat lane touched. Outputs stay under `Assets/TheCat/Art/Enemies/Frames/Sliced/`.
- Initial reproducibility risk was fixed: generated policy and matrix outputs now use fixed packet dates rather than wall-clock `Get-Date` strings. A same-input rerun hash check passed for five generated output files.

## Acceptance Boundary

This packet closes local import/slicing package readiness for the three P0 enemy framesheets. It does not close Unity acceptance.

Required next evidence:

- Unity asset import refresh.
- Active enemy animation screenshots for Black Mud, Cold Light, and Call Tyrant.
- Prefab/catalog binding proof.
- Console check.
- Pivot/playback/hitbox validation for Call Tyrant frame 4.
- Visual color-drift check in the active enemy screenshots.
