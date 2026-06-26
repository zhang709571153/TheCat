# Batch111 Scene Preview Backplates Agent Review

Verdict: `PASS_WITH_P2`

Gate: `candidate_complete_pending_unity_review`

Scope: `design/development/asset_candidates/ui/scene_preview_backplates/batch_111_scene_preview_backplates_2026-06-26`

Source truth: Qr1 UI/style truth revision 816, Batch107 scene-selection token context, and Batch110 bedroom map overlay context. This review makes no IAd character-body claim and no HDo/FoW9 map-archive claim.

## Inputs

- Asset table: `design/development/asset_candidates/ui/scene_preview_backplates/batch_111_scene_preview_backplates_2026-06-26/tc_ui_scene_prev_batch_111_scene_preview_backplates_2026-06-26_asset_table.csv`
- Source sheet v002: `design/development/asset_candidates/ui/scene_preview_backplates/batch_111_scene_preview_backplates_2026-06-26/source/tc_ui_scene_prev_batch111_chromakey_source_v002.png`
- Alpha sheet: `design/development/asset_candidates/ui/scene_preview_backplates/batch_111_scene_preview_backplates_2026-06-26/alpha/tc_ui_scene_prev_batch111_alpha_sheet_v002.png`
- Manifest: `design/development/asset_candidates/ui/scene_preview_backplates/batch_111_scene_preview_backplates_2026-06-26/tc_ui_scene_prev_batch111_semantic_manifest.csv`
- Contact sheet: `design/development/asset_candidates/ui/scene_preview_backplates/batch_111_scene_preview_backplates_2026-06-26/tc_ui_scene_prev_batch111_semantic_contact_sheet_v001.png`
- 192x108 readability board: `design/development/asset_candidates/ui/scene_preview_backplates/batch_111_scene_preview_backplates_2026-06-26/tc_ui_scene_prev_batch111_192x108_scene_preview_readability_board_v001.png`
- Review variants: `design/development/asset_candidates/ui/scene_preview_backplates/batch_111_scene_preview_backplates_2026-06-26/reviews/variants/`
- Final review CSV: `design/development/asset_candidates/ui/scene_preview_backplates/batch_111_scene_preview_backplates_2026-06-26/tc_ui_scene_prev_batch_111_scene_preview_backplates_2026-06-26_final_review.csv`

## Agent Findings

- Noether visual/style review: `PASS_WITH_P2`. The 9 cards share the Qr1-style gold bevel, navy dreamglass, cream/warm lighting, and storybook ornament system. The 192x108 board remains readable over checker, night, and warm backgrounds. No baked text, watermark, character body, or animation content was found.
- Bernoulli source-lock review: `PASS_WITH_P2`. v001 rejected source is preserved because of paw markers. v002 accepted source avoids cat body, portrait, cat face, paw/print marker, animation frame, Egypt archive claim, and formal replacement claim. `cat_room_preview_backplate` and `dream_route_preview_backplate` are accepted only as symbolic preview cards.
- Faraday production QA: initial `PASS_WITH_P1`. The PNG payload passed mechanical QA with 9 semantic sprites, 45 review PNGs, 9 variant manifests, matching manifest hashes, transparent corners, no `.meta` files, and no runtime `Assets/` writes. The P1 gate-record gap was fixed by this review note, the process note, and final review CSV. The P2 absolute-path variant manifest issue was fixed by normalizing paths to workspace-relative form.

## Accepted Candidates

Decision: `candidate_keep` for all 9 semantic sprites.

- `bedroom_preview_backplate`
- `cat_room_preview_backplate`
- `dream_route_preview_backplate`
- `battle_preview_backplate`
- `reward_preview_backplate`
- `shop_event_preview_backplate`
- `settings_preview_backplate`
- `locked_preview_backplate`
- `unknown_preview_backplate`

## Conditional Notes

- `cat_room_preview_backplate` is a symbolic UI card only. It is not cat-room layout truth and not character identity evidence.
- `dream_route_preview_backplate` is a symbolic UI card only. It is not route-map truth and not Egypt archive evidence.
- Verify `cat_room`, `reward`, and `shop_event` together in the actual scene-selection card layout.
- Verify `unknown` versus `dream_route` distinction in the actual scene-selection card layout.

## Rejected Or Superseded

- v001 rejected: `design/development/asset_candidates/ui/scene_preview_backplates/batch_111_scene_preview_backplates_2026-06-26/superseded/rejected_source/tc_ui_scene_prev_batch111_rejected_source_v001_paw_markers.png`
- Reason: paw markers created source-lock and character-identity ambiguity.
- v002 accepted: `design/development/asset_candidates/ui/scene_preview_backplates/batch_111_scene_preview_backplates_2026-06-26/source/tc_ui_scene_prev_batch111_chromakey_source_v002.png`

## Runtime Gate

No runtime import was performed.

Still required before Unity acceptance:

- Unity import settings.
- Binding proof in the scene-selection UI.
- Scene-selection screenshots at target card size.
- Card-content overlay padding proof.
- Cat-room/shop/reward distinction proof.
- Unknown-vs-dream-route distinction proof.
- Clean Console evidence.

Current state: `candidate_complete_pending_unity_review`.
