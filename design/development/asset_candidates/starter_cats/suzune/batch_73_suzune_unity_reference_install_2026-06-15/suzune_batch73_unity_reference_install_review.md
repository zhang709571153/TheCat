# Batch 73 - Suzune Unity Reference Install Review

## Decision

Installed one source-derived Suzune front/side/back reference atlas into Unity as a debug reference asset.
This is not a runtime-bound combat sprite and does not replace the existing Suzune combat sprite.
Formal starter-cat body-art import remains blocked until active-cat Play Mode screenshots pass source-lock review.

## Installed Asset

- Asset id: `thecat_cat_suzune_turnaround_reference_atlas_2304x768_v001`
- Unity import path: `Assets/TheCat/Art/Characters/References/thecat_cat_suzune_turnaround_reference_atlas_2304x768_v001.png`
- Unity meta path: `Assets/TheCat/Art/Characters/References/thecat_cat_suzune_turnaround_reference_atlas_2304x768_v001.png.meta`
- Installed size: `2304x768`
- Source lock: `suzune_turnaround_colored`
- Recommendation: `installed_debug_reference_not_runtime_binding_pending_unity_visual_smoke`

## Source Evidence

- Colored turnaround: `design/梦境支配者核心玩法/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png`
- Locked combat sprite retained: `Assets/TheCat/Art/Characters/Sprites/thecat_cat_suzune_combat_sprite_512_v001.png`
- Front plate: `design/development/asset_candidates/starter_cats/batch_70_source_turnaround_reference_plates_2026-06-15/thecat_cat_suzune_turnaround_front_reference_plate_768_batch70_v001.png`
- Side plate: `design/development/asset_candidates/starter_cats/batch_70_source_turnaround_reference_plates_2026-06-15/thecat_cat_suzune_turnaround_side_reference_plate_768_batch70_v001.png`
- Back plate: `design/development/asset_candidates/starter_cats/batch_70_source_turnaround_reference_plates_2026-06-15/thecat_cat_suzune_turnaround_back_reference_plate_768_batch70_v001.png`

## Consistency Notes

- The atlas is a direct left-to-right concatenation of the Batch 70 Suzune reference plates.
- It preserves the source motif: calico sleep-shrine healer cat, bells, wand/branch silhouette, vermilion warm-white moon-blue palette.
- No AI-generated cat body candidate was imported.
- No existing combat sprite, HUD avatar, source turnaround, or Batch 70 plate was overwritten.
- Unity visual smoke remains pending: refresh AssetDatabase, inspect import settings, and capture active-cat screenshots.
