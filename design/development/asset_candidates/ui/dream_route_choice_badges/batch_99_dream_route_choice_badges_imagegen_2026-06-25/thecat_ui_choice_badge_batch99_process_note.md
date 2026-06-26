# Batch 99 Dream Route Choice Badge Process Note

Date: 2026-06-25

Status: candidate_only_pending_unity_route_choice_card_review

## Scope

Batch 99 produces nine transparent UI candidate sprites for dream-route choice-card overlays and badges. This is a UI-only symbolic batch. No character bodies, portraits, animation frames, replacements, or character-source claims are included.

## Source Truth

- Qr1 UI/style authority revision 816.
- Batch86 dream-route preflight as local route-card context.
- Character design source, live-blocked Feishu docs, and starter-cat source-lock packets were not used for this batch.

## Method

- Generation: built-in Codex imagegen on a flat magenta chroma-key background.
- Workspace source: `source/thecat_ui_dream_route_choice_badges_batch99_chromakey_source_v001.png`.
- Alpha removal: imagegen `remove_chroma_key.py` helper with auto-key border sampling, soft matte, despill, and edge contract.
- Alpha sheet: `source/thecat_ui_dream_route_choice_badges_batch99_alpha_sheet_v001.png`.
- Key color sampled by helper: `#fa03fa`.
- Cutting method: `sprite_batch_tools.py split-sheet`, 3 rows x 3 columns, alpha projection bands, 24 px padding.
- Naming fix: initial long prefix exceeded safe Windows path length during split. The final sprite prefix is `thecat_ui_choice_badge` to keep path length under the legacy 260-character boundary.

## Output

- Asset table: `thecat_ui_dream_route_choice_badges_batch99_asset_table.csv`.
- Manifest: `thecat_ui_choice_badge_batch99_semantic_manifest.csv`.
- Contact sheet: `thecat_ui_choice_badge_batch99_semantic_contact_sheet_v001.png`.
- 64px/card readability board: `thecat_ui_choice_badge_batch99_64px_card_readability_board_v001.png`.
- Final review CSV: `thecat_ui_choice_badge_batch99_final_review.csv`.
- Semantic sprites: 9 transparent PNGs under `semantic_sprites/`.

## Review Summary

- Visual/style: PASS_WITH_P2. Five clean keep assets and four conditional P2 watch assets.
- Source-lock: PASS_WITH_P2. Source boundary is clean; an asset-table/manifest id traceability P2 was fixed by aligning asset IDs.
- Production QA: PASS_WITH_P2. Hashes, dimensions, alpha extrema, file counts, no `.meta`, and no `Assets/` writes passed. Path length remains a P2 watch.

## Gate

No Unity import was performed.

Remaining gates:

- Unity route-choice card screenshot proof.
- Sprite import settings.
- Binding proof.
- Clean Console.
- Final scale and overlay placement proof for locked, danger, event, boss, and selected-corner watch items.
