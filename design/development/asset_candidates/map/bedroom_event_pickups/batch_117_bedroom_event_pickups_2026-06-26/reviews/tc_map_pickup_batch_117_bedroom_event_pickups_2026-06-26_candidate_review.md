# Batch117 Bedroom Event Pickups Agent Review

Verdict: `PASS_WITH_P2`

Current gate: `candidate_complete_pending_unity_review`

Scope: Batch117 static bedroom event pickup sprites under `design/development/asset_candidates/map/bedroom_event_pickups/batch_117_bedroom_event_pickups_2026-06-26`.

## Inputs

- Asset table: `tc_map_pickup_batch_117_bedroom_event_pickups_2026-06-26_asset_table.csv`
- Manifest: `tc_map_pickup_batch117_semantic_manifest.csv`
- Active contact sheet: `tc_map_pickup_batch117_semantic_contact_sheet_v002.png`
- Active readability board: `tc_map_pickup_batch117_96px_64px_bedroom_event_pickup_readability_board_v002.png`
- Process note: `tc_map_pickup_batch_117_bedroom_event_pickups_2026-06-26_process_note.md`
- Final review CSV: `tc_map_pickup_batch_117_bedroom_event_pickups_2026-06-26_final_review.csv`

## Integrated Findings

- Visual/source-boundary review: `PASS_WITH_P2`. The pack fits the Qr1 bedroom-dream direction and preserves the source boundary: no character body, face, portrait, paw, costume, framesheet, animation, IAd live approval claim, HDo/FoW9 map archive coverage, or Egypt map coverage.
- Production QA review: `PASS_WITH_P2`. Current manifest paths resolve, hashes match, alpha extrema are `(0, 255)`, active sprites have transparent corners/outer edges, no `.meta` files were created, and no Batch117 files leaked into `Assets/`.
- Target-size readability review on v001: `PASS_WITH_P1` because `story_page_clue_pickup` was weak at 64px warm background.
- Focused v002 story-page review: `PASS_WITH_P2`. `story_page_clue_pickup` v002 fixes the P1 and no longer collides with `moon_ticket_pass_pickup`, `star_button_key_pickup`, or `reroll_dice_charm_pickup`; it still needs warm-room Unity proof before runtime acceptance.
- Delta production QA on v002: `PASS_WITH_P2`. v002 source, alpha, active sprite, contact sheet, readability board, and five review variants exist; v001 story page is superseded only.

## Candidate Decisions

`candidate_keep`:

- `fish_treat_pickup`
- `moon_ticket_pass_pickup`
- `blessing_lantern_pickup`
- `story_page_clue_pickup` v002
- `star_button_key_pickup`

`candidate_conditional`:

- `dream_thread_spool_pickup`: runtime must distinguish it from Batch108 yarn/obstacle language.
- `memory_shard_cluster_pickup`: runtime must prove pickup-vs-obstacle/VFX role and sorting against Batch108 crystal language.
- `soft_light_orb_pickup`: runtime must prove pickup-vs-blessing/VFX role and glow occlusion safety.
- `reroll_dice_charm_pickup`: runtime must align with or clearly separate from Batch104 reroll-token semantics.

Rejected: none.

## Required Runtime Gates

- Unity import settings for the active candidates, with `story_page_clue_pickup` pointing to `candidate_v002.png`.
- Binding proof for the intended bedroom-map pickup/item IDs.
- Bedroom map screenshots at 96px and 64px on dark and warm backgrounds.
- Focused ambiguity strip for moon ticket, story page, star key, and reroll dice.
- Batch101/108/109/104 conflict proof for markers, props, entry/path objects, and reward/event tokens.
- Scene-owned trigger-or-none collider policy proof.
- No recursive import from candidate or `superseded/` folders.
- Clean Console evidence.

No runtime import was performed.
