# P0 Route Choice Icon Assets

Scope: deterministic non-cat UI icons for P0 roguelite reward choices on the
route map. These assets make shop, event, blessing, partner, and rest choices
readable without changing any starter cat art.

## Source References

- `thecat_style_status_icons_5x64_v001`
- `thecat_ui_panel_dreamglass_512x256_v001`
- `thecat_ui_reward_fishtreat_icon_128_v001`
- `thecat_ui_reward_dreamshard_icon_128_v001`

## Generated Assets

- `thecat_ui_choice_partner_recruit_icon_128_v001`
  - Path: `Assets/TheCat/Art/UI/Icons/thecat_ui_choice_partner_recruit_icon_128_v001.png`
  - Purpose: route choice icon for partner recruitment choices.
- `thecat_ui_choice_purchase_supply_icon_128_v001`
  - Path: `Assets/TheCat/Art/UI/Icons/thecat_ui_choice_purchase_supply_icon_128_v001.png`
  - Purpose: route choice icon for shop supply purchases.
- `thecat_ui_choice_authority_blessing_icon_128_v001`
  - Path: `Assets/TheCat/Art/UI/Icons/thecat_ui_choice_authority_blessing_icon_128_v001.png`
  - Purpose: route choice icon for gaining P0 authority blessings.
- `thecat_ui_choice_authority_upgrade_icon_128_v001`
  - Path: `Assets/TheCat/Art/UI/Icons/thecat_ui_choice_authority_upgrade_icon_128_v001.png`
  - Purpose: route choice icon for deepening owned authority blessings.
- `thecat_ui_choice_rest_supply_icon_128_v001`
  - Path: `Assets/TheCat/Art/UI/Icons/thecat_ui_choice_rest_supply_icon_128_v001.png`
  - Purpose: route choice icon for rest-nest recovery.
- `thecat_ui_choice_dreamevent_modifier_icon_128_v001`
  - Path: `Assets/TheCat/Art/UI/Icons/thecat_ui_choice_dreamevent_modifier_icon_128_v001.png`
  - Purpose: route choice icon for dream-event next-battle modifiers.

## Style Rules

- Use the existing route node and UI shell palette: dream blues, moon cyan,
  soft purple, fish-gold accents, and readable silhouette shapes.
- Icons must remain legible at `32x32` and `64x64` when authored at `128x128`.
- Keep icons symbolic and UI-facing; do not depict full cat bodies, starter-cat
  costumes, or turnaround-derived cat markings.
- Existing fish treat and dream shard reward icons remain the resource icons for
  `GainFishTreats`, `PurchaseFishTreats`, and `GainDreamShards`.

## Consistency Gates

- Do not modify starter cat runtime sprites or candidate directories.
- Do not weaken `saiban_turnaround_colored`, `nephthys_turnaround_colored`, or
  `suzune_turnaround_colored` source locks.
- All files must be listed in `P0AssetManifestCatalog`,
  `P0AssetGenerationBatchCatalog`, `P0VisualAssetCatalog`, and
  `design/development/P0_ASSET_MANIFEST.csv`.
- Runtime-facing assets must appear in `P0RuntimeVisualBindingCoverage`, the
  runtime contact sheet, and route-map reward choice surface coverage.

## Rebuild Command

```powershell
& .\design\development\tools\build_route_choice_icons.ps1
& .\design\development\tools\build_runtime_visual_contact_sheet.ps1
```
