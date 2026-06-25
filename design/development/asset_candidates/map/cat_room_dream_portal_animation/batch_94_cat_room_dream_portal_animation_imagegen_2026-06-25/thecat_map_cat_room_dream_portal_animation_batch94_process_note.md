# Batch 94 Cat-Room Dream Portal Animation Process Note

## Scope

Candidate-only 12-frame idle loop for the cat-room dream entrance portal. This batch is map/VFX animation, not character body art. It extends the accepted Batch91 dream entrance portal concept with a loopable glow/mist/star pulse.

## Source Truth

- Batch91 accepted semantic sprite: `design/development/asset_candidates/map/cat_room_elements/batch_91_cat_room_map_elements_imagegen_2026-06-25/semantic_sprites/thecat_map_cat_room_dream_entrance_portal_batch91_candidate_v001.png`.
- UI/style source remains the local Qr1 working copy and existing Batch91/92/93 cat-room candidates.
- Character body/framesheet generation remains locked and untouched.

## Production

- Built-in Codex imagegen was used, not API-key CLI fallback.
- Prompt requested a 4x3 chroma-key source sheet on flat `#ff00ff`, no text, no watermark, no characters, no enemies.
- Source sheet: `D:\Unity Workspace\TheCat\design\development\asset_candidates\map\cat_room_dream_portal_animation\batch_94_cat_room_dream_portal_animation_imagegen_2026-06-25\source\thecat_map_cat_room_dream_portal_animation_batch94_chromakey_source_v001.png`.
- Alpha sheet: `D:\Unity Workspace\TheCat\design\development\asset_candidates\map\cat_room_dream_portal_animation\batch_94_cat_room_dream_portal_animation_imagegen_2026-06-25\source\thecat_map_cat_room_dream_portal_animation_batch94_alpha_sheet_v001.png`.
- Initial cut method: fixed 4x3 grid from 1448x1086 source, 362x362 cells, each composited into a uniform 512x512 transparent canvas.
- v001/v002 review result: both were superseded before Unity review. v002 still had frame-to-frame redraw drift and source-sheet line residue risk.
- v003 P1 fix: use the accepted Batch91 portal as a stable outer shell, then generate only procedural inner teal glow, mist bands, and star twinkles across 12 frames.
- v004 P2 cleanup: remove alpha<=2 key-color residue from v003 and move v001/v002/v003 frame PNGs to `superseded/`. The current `semantic_sprites/` directory contains only the 12 v004 PNG frames, and the current `sprites/` directory contains only the 12 v004 raw copies.
- Stable base: `D:\Unity Workspace\TheCat\design\development\asset_candidates\map\cat_room_dream_portal_animation\batch_94_cat_room_dream_portal_animation_imagegen_2026-06-25\source\thecat_map_cat_room_dream_portal_animation_batch94_stable_base_from_batch91_v003.png`.
- Interior mask: `D:\Unity Workspace\TheCat\design\development\asset_candidates\map\cat_room_dream_portal_animation\batch_94_cat_room_dream_portal_animation_imagegen_2026-06-25\source\thecat_map_cat_room_dream_portal_animation_batch94_interior_mask_v003.png`.
- Contact sheet: `D:\Unity Workspace\TheCat\design\development\asset_candidates\map\cat_room_dream_portal_animation\batch_94_cat_room_dream_portal_animation_imagegen_2026-06-25\thecat_map_cat_room_dream_portal_animation_batch94_semantic_contact_sheet_v004.png`.
- Loop preview GIF: `D:\Unity Workspace\TheCat\design\development\asset_candidates\map\cat_room_dream_portal_animation\batch_94_cat_room_dream_portal_animation_imagegen_2026-06-25\thecat_map_cat_room_dream_portal_animation_batch94_loop_preview_v004.gif`.
- Manifest: `D:\Unity Workspace\TheCat\design\development\asset_candidates\map\cat_room_dream_portal_animation\batch_94_cat_room_dream_portal_animation_imagegen_2026-06-25\thecat_map_cat_room_dream_portal_animation_batch94_semantic_manifest.csv`.
- Initial review CSV: `D:\Unity Workspace\TheCat\design\development\asset_candidates\map\cat_room_dream_portal_animation\batch_94_cat_room_dream_portal_animation_imagegen_2026-06-25\thecat_map_cat_room_dream_portal_animation_batch94_review.csv`.

## Gate State

`candidate_complete_pending_unity_loop_review`.

Required before runtime promotion:

- Independent visual/style review for portal consistency, no character leakage, no text/watermark, and frame-to-frame identity stability: complete, final result `PASS_WITH_P2`.
- Independent production QA for alpha edges, identical canvas size, pivot, loop jitter, sorting layer, import settings, and no duplicate collider policy: complete, final result `PASS_WITH_P2`.
- Unity cat-room screenshots proving scale, sorting, and loop readability on the dream entrance.
