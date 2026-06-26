# Batch 99 Dream Route Choice Badges Agent Review

Scope: `design/development/asset_candidates/ui/dream_route_choice_badges/batch_99_dream_route_choice_badges_imagegen_2026-06-25`

Verdict: PASS_WITH_P2

Current gate: candidate_only_pending_unity_route_choice_card_review

## Review Inputs

- Source image: `source/thecat_ui_dream_route_choice_badges_batch99_chromakey_source_v001.png`
- Alpha sheet: `source/thecat_ui_dream_route_choice_badges_batch99_alpha_sheet_v001.png`
- Manifest: `thecat_ui_choice_badge_batch99_semantic_manifest.csv`
- Contact sheet: `thecat_ui_choice_badge_batch99_semantic_contact_sheet_v001.png`
- 64px/card readability board: `thecat_ui_choice_badge_batch99_64px_card_readability_board_v001.png`
- Final review CSV: `thecat_ui_choice_badge_batch99_final_review.csv`
- Process note: `thecat_ui_choice_badge_batch99_process_note.md`

## Visual/style

Verdict: PASS_WITH_P2

Batch99 matches the Qr1/Batch86 dreamglass route-card language: navy glass, cyan edge light, gold bevels, route-card overlay semantics, and no baked text.

Accepted clean keep:

- `dream_route_choice_selected_corner`
- `dream_route_choice_recommended_ribbon`
- `dream_route_choice_reward_spark`
- `dream_route_choice_rest_moon`
- `dream_route_choice_unknown_veil`

Accepted with P2 watch:

- `dream_route_choice_locked_chain`: readable, but needs final card-width scale proof.
- `dream_route_choice_danger_corner`: semantics work, but pink/magenta accent must stay controlled against cyan/gold dreamglass palette.
- `dream_route_choice_event_mist`: strong mood, but teal mist can obscure card content or drift from badge clarity.
- `dream_route_choice_boss_pressure`: good boss-pressure read, but dense silhouette and magenta dominance need Unity-card proof.
- `dream_route_choice_selected_corner`: verify final click-target/card-edge margin in runtime overlay placement.

## Source-lock

Verdict: PASS_WITH_P2

Source boundary is clean. Batch99 uses only `Qr1 UI/style authority revision 816; Batch86 dream-route preflight`. No character or animation content is present: no character bodies, faces, portraits, framesheets, recolors, replacements, or animation claims.

P2 fixed:

- Asset-table `asset_id` values initially omitted the shorter `choice_badge` prefix used by the manifest and sprite filenames. The asset table was updated so every `asset_id` matches the manifest.

## Production QA

Verdict: PASS_WITH_P2

Verified:

- Workspace source PNG exists.
- Alpha sheet exists with alpha extrema `(0, 255)`.
- `semantic_sprites/` contains exactly 9 PNGs.
- Manifest has exactly 9 rows, and rows match current file existence, dimensions, alpha extrema, SHA-256, and `status=ok`.
- `final_review.csv` exists with 9 semantic rows: 5 `candidate_keep`, 4 `candidate_conditional`.
- Contact sheet and 64px/card readability board exist.
- No Unity `.meta` files are in the candidate package.
- No matching Batch99 files were written under `Assets/`.
- Candidate gate remains pending: route-choice card screenshot, import settings, binding proof, and clean Console.

P2 watch:

- Max absolute path length is close to the legacy Windows limit. Avoid deeper folders and longer revision names for this batch.

## Gate Notes

Do not import this batch into Unity yet. Runtime acceptance still requires Unity route-choice card screenshots, import settings, binding proof, and clean Console evidence.
