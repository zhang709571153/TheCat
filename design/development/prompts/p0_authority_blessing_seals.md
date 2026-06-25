# P0 Authority Blessing Seal Assets

Scope: deterministic non-cat UI seals for the three P0 authority blessings used
by the roguelite route reward system.

## Source References

- `thecat_ui_choice_authority_blessing_icon_128_v001`
- `thecat_ui_routecard_blessing_frame_512x256_v001`
- `thecat_style_status_icons_5x64_v001`

## Generated Assets

- `thecat_ui_blessing_oath_bedline_seal_128_v001`
  - Path: `Assets/TheCat/Art/UI/Icons/thecat_ui_blessing_oath_bedline_seal_128_v001.png`
  - Purpose: specific route reward icon for `authority_oath_bedline`.
- `thecat_ui_blessing_dominion_sandglass_seal_128_v001`
  - Path: `Assets/TheCat/Art/UI/Icons/thecat_ui_blessing_dominion_sandglass_seal_128_v001.png`
  - Purpose: specific route reward icon for `authority_dominion_sandglass`.
- `thecat_ui_blessing_rhythm_lullaby_seal_128_v001`
  - Path: `Assets/TheCat/Art/UI/Icons/thecat_ui_blessing_rhythm_lullaby_seal_128_v001.png`
  - Purpose: specific route reward icon for `authority_rhythm_lullaby`.

## Symbol Rules

- Oath Bedline: shield, bedline, silver-blue and fish-gold accents.
- Moon-Sand Dominion: sandglass, crescent, eye shape, lapis and sand-gold accents.
- Lullaby Rhythm: crescent, bell, sleep wave, shrine-red and moon-cyan accents.
- Keep the silhouettes readable at `32x32`, while authored at `128x128`.

## Cat Consistency Constraints

- Do not depict starter cat bodies, heads, markings, costumes, props, or poses.
- Do not sample from or modify starter cat turnarounds, candidate sheets, or
  locked runtime sprites.
- These are abstract UI seals tied to blessing ids; formal cat art remains
  governed by the colored three-view turnaround source locks.

## Runtime Integration

- `P0VisualAssetCatalog.GetAuthorityBlessingSeal(blessingId)` must resolve all
  three P0 authority blessings.
- `P0VisualAssetCatalog.GetRouteChoiceIcon(RouteRewardChoice)` must use these
  seals for gain and upgrade blessing choices.
- Runtime bindings:
  - `blessing_detail.oath_bedline`
  - `blessing_detail.dominion_sandglass`
  - `blessing_detail.rhythm_lullaby`

## Rebuild Command

```powershell
& .\design\development\tools\build_authority_blessing_seals.ps1
& .\design\development\tools\build_runtime_visual_contact_sheet.ps1
```
