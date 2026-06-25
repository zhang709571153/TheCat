# P0 Status Compact Icons

Batch: `p0_asset_batch_14_status_compact_icons`

## Scope

Generate 32x32 compact HUD icons for the five P0 status tags:

- Sleep Stable
- Slow
- Knockback
- Mark
- Shield

## Source Rule

Use only the accepted 64x64 status icon assets as sources:

- `Assets/TheCat/Art/UI/Icons/thecat_ui_status_sleep_stable_64_v001.png`
- `Assets/TheCat/Art/UI/Icons/thecat_ui_status_slow_64_v001.png`
- `Assets/TheCat/Art/UI/Icons/thecat_ui_status_knockback_64_v001.png`
- `Assets/TheCat/Art/UI/Icons/thecat_ui_status_mark_64_v001.png`
- `Assets/TheCat/Art/UI/Icons/thecat_ui_status_shield_64_v001.png`

Do not use starter cat sprites, cat turnarounds, or generated cat candidates.

## Output

Write generated PNG files to `Assets/TheCat/Art/UI/Icons`:

- `thecat_ui_status_sleep_stable_32_v001.png`
- `thecat_ui_status_slow_32_v001.png`
- `thecat_ui_status_knockback_32_v001.png`
- `thecat_ui_status_mark_32_v001.png`
- `thecat_ui_status_shield_32_v001.png`

Each output must:

- be exactly `32x32`
- keep transparent background
- preserve the source icon silhouette and palette
- include `.png.meta` with `TheCatP0ImportSettings:v1`
- include `.png.meta` userData marker `batch:p0_asset_batch_14_status_compact_icons`

## Acceptance

- The compact icons are derived from accepted 64px status assets.
- The compact icons are registered in `P0_ASSET_MANIFEST.csv`.
- Runtime bindings expose the compact icons for status HUD review.
- The runtime contact sheet includes all compact status bindings.
- No cat body derivative or starter cat file is read or written.
