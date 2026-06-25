# Batch95 Role / Scene UI Tokens Agent Review

Scope path: `design/development/asset_candidates/ui/role_scene_ui_tokens/batch_95_role_scene_ui_tokens_imagegen_2026-06-25`

## Verdict

Overall: `PASS_WITH_P2`

Batch95 is accepted as a static candidate-only UI sprite package. It must not be treated as character body art, portrait art, animation frames, framesheet source, or runtime-approved import material.

## Review Inputs

- Source sheet: `source/thecat_ui_role_scene_tokens_batch95_chromakey_source_v001.png`
- Alpha sheet: `source/thecat_ui_role_scene_tokens_batch95_alpha_sheet_v001.png`
- Manifest: `thecat_ui_role_scene_token_batch95_semantic_manifest.csv`
- Contact sheet: `thecat_ui_role_scene_token_batch95_semantic_contact_sheet_v001.png`
- Final review CSV: `thecat_ui_role_scene_token_batch95_final_review.csv`

## Independent Review Summary

| Lane | Verdict | Notes |
| --- | --- | --- |
| Character source-lock compliance | PASS | No cat body, face, portrait, character replacement, animated frame sequence, or source-lock-risk derivative found. The pack serves static character-symbol, scene-token, and UI-sprite needs. |
| Visual/style review | PASS_WITH_P2 | Palette and dream UI style are cohesive. Saiban and Nephthys motifs read well. Suzune badge and scene tokens need 64px readability watch. |
| Production QA | PASS_WITH_P2 | Current 9 semantic PNGs match manifest hashes and dimensions; alpha edges are clean; no `.meta` files; candidate-only gate is preserved. Superseded BOM split was cleaned after review. |

## Accepted Candidates

- `saiban_oath_sun_shield_role_badge`
- `nephthys_moonsand_obelisk_role_badge`
- `suzune_moon_bell_torii_role_badge`
- `bedroom_dream_scene_token`
- `cat_room_hub_scene_token`
- `dream_route_scene_token`
- `ready_card_frame`
- `selected_card_frame`
- `locked_card_frame`

## P2 Watch Items

- Test Suzune badge at true 64px; simplify or boost silhouette contrast if bell, tag, and torii cluster collapses.
- Test bedroom and cat-room scene tokens at true 64px; use larger card placement if they soften into texture.
- Test locked frame at true 64px; the lock emblem may need a larger overlay if used as a small icon.

## Unity Import Notes

- Import from `semantic_sprites/` only.
- Texture Type: `Sprite (2D and UI)`.
- Sprite Mode: `Single`.
- Alpha Source: input texture alpha; Alpha Is Transparency on; mipmaps off.
- Pivot: center.
- Colliders: none; use RectTransform or Button hit targets.
- Sorting: prefer Canvas hierarchy or sibling order. For SpriteRenderer tests, use UI/token sorting and keep selected overlays above ready/locked states.

## Gate

Current gate: `candidate_complete_pending_unity_review`.

Required before promotion: import settings, 64px readability screenshots, target UI binding proof, no recursive import from `superseded/`, and clean Console evidence.
