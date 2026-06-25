# P0 UI Shell Assets

Scope: deterministic non-cat UI shell assets for the current P0 playable
prototype. This batch exists to move main menu, settlement, and shared UI
surfaces away from pure graybox presentation while keeping starter cat art
locked to the colored three-view turnarounds.

## Source References

- `thecat_style_bedroomdream_anchor_1920x1080_v001`
- `thecat_style_status_icons_5x64_v001`

## Generated Assets

- `thecat_ui_mainmenu_dreamentry_bg_1920x1080_v001`
  - Path: `Assets/TheCat/Art/UI/Backgrounds/thecat_ui_mainmenu_dreamentry_bg_1920x1080_v001.png`
  - Size: `1920x1080`
  - Purpose: main menu dream-entry background.
- `thecat_ui_title_logo_512x256_v001`
  - Path: `Assets/TheCat/Art/UI/Frames/thecat_ui_title_logo_512x256_v001.png`
  - Size: `512x256`
  - Purpose: title logo frame for menu/loading surfaces.
- `thecat_ui_panel_dreamglass_512x256_v001`
  - Path: `Assets/TheCat/Art/UI/Frames/thecat_ui_panel_dreamglass_512x256_v001.png`
  - Size: `512x256`
  - Purpose: shared dreamglass panel frame.
- `thecat_ui_button_primary_384x96_v001`
  - Path: `Assets/TheCat/Art/UI/Frames/thecat_ui_button_primary_384x96_v001.png`
  - Size: `384x96`
  - Purpose: shared primary action button frame.
- `thecat_ui_reward_fishtreat_icon_128_v001`
  - Path: `Assets/TheCat/Art/UI/Icons/thecat_ui_reward_fishtreat_icon_128_v001.png`
  - Size: `128x128`
  - Purpose: settlement/shop fish treat reward icon.
- `thecat_ui_reward_dreamshard_icon_128_v001`
  - Path: `Assets/TheCat/Art/UI/Icons/thecat_ui_reward_dreamshard_icon_128_v001.png`
  - Size: `128x128`
  - Purpose: settlement/shop dream shard reward icon.

## Style Rules

- Palette must stay close to Bedroom Dream night blues, moon cyan, soft purple,
  and fish-gold action accents.
- UI frames should be readable over both dark and mixed battle/menu surfaces.
- Icons must remain clear at `64x64` even when authored at `128x128`.
- These assets do not define or replace any cat body, costume, markings, or
  facial traits.

## Consistency Gates

- Do not modify starter cat runtime sprites.
- Do not import starter cat derivative candidates from
  `design/development/asset_candidates/starter_cats`.
- Any future cat-facing UI portrait work must reference the formal
  source-lock packet and colored three-view turnaround review notes.
- All files must be listed in `P0AssetManifestCatalog`,
  `P0AssetGenerationBatchCatalog`, `P0VisualAssetCatalog`, and
  `design/development/P0_ASSET_MANIFEST.csv`.
- Runtime-facing assets must appear in `P0RuntimeVisualBindingCoverage` and the
  runtime contact sheet.

## Rebuild Command

```powershell
& .\design\development\tools\build_ui_shell_assets.ps1
& .\design\development\tools\build_runtime_visual_contact_sheet.ps1
```
