# scene_selection_tokens Process Note

Batch: `batch_107_scene_selection_tokens_2026-06-26`
Family: `ui`
Source truth: `Qr1 UI/style truth revision 816; static scene-selection UI token lane; no IAd character-body claim; no HDo map-archive claim`

Process: built-in Codex `imagegen`, workspace source copy, local chroma-key alpha removal, projection-based 3x3 sprite splitting, contact/readability boards, and independent review. No runtime import was performed.

## Produced Files

- Built-in imagegen source: `C:\Users\PC\.codex\generated_images\019efc23-3cc2-7f23-9beb-cb16d118a13f\ig_0e14e82ceacb0a6f016a3d6d0d719c8198b17a982199203386.png`
- Workspace source copy: `design/development/asset_candidates/ui/scene_selection_tokens/batch_107_scene_selection_tokens_2026-06-26/source/tc_ui_scene_batch107_chromakey_source_v001.png`
- Semantic names file: `design/development/asset_candidates/ui/scene_selection_tokens/batch_107_scene_selection_tokens_2026-06-26/source/tc_ui_scene_batch107_names.txt`
- Alpha sheet: `design/development/asset_candidates/ui/scene_selection_tokens/batch_107_scene_selection_tokens_2026-06-26/alpha/tc_ui_scene_batch107_alpha_sheet_v001.png`
- Semantic sprites: `design/development/asset_candidates/ui/scene_selection_tokens/batch_107_scene_selection_tokens_2026-06-26/semantic_sprites/*.png`
- Manifest: `design/development/asset_candidates/ui/scene_selection_tokens/batch_107_scene_selection_tokens_2026-06-26/tc_ui_scene_batch107_semantic_manifest.csv`
- Contact sheet: `design/development/asset_candidates/ui/scene_selection_tokens/batch_107_scene_selection_tokens_2026-06-26/tc_ui_scene_batch107_semantic_contact_sheet_v001.png`
- 64px readability board: `design/development/asset_candidates/ui/scene_selection_tokens/batch_107_scene_selection_tokens_2026-06-26/tc_ui_scene_batch107_64px_scene_selection_readability_board_v001.png`
- Asset table: `design/development/asset_candidates/ui/scene_selection_tokens/batch_107_scene_selection_tokens_2026-06-26/tc_ui_scene_batch_107_scene_selection_tokens_2026-06-26_asset_table.csv`
- Final review CSV: `design/development/asset_candidates/ui/scene_selection_tokens/batch_107_scene_selection_tokens_2026-06-26/tc_ui_scene_batch_107_scene_selection_tokens_2026-06-26_final_review.csv`

## Chroma-Key And Cut Results

- Source size: `1254x1254`, RGB.
- Chroma-key helper: `C:\Users\PC\.codex\skills\.system\imagegen\scripts\remove_chroma_key.py`
- Helper args: `--auto-key border --soft-matte --transparent-threshold 12 --opaque-threshold 220 --despill`
- Sampled key color: `#f603ec`
- Transparent pixels: `640588/1572516`
- Partially transparent pixels: `21081/1572516`
- Cut method: projection-based split from alpha sheet via `sprite_batch_tools.py split-sheet`.
- Projection row bands: `(30, 427)`, `(431, 829)`, `(832, 1230)`.
- Projection column bands: `(113, 407)`, `(478, 772)`, `(845, 1139)`.
- Result: 9 semantic transparent PNG sprites with full alpha extrema `(0, 255)`.

## Semantic Order

1. `bedroom_scene_card_token`
2. `cat_room_scene_card_token`
3. `dream_route_scene_card_token`
4. `battle_scene_card_token`
5. `reward_scene_card_token`
6. `shop_event_scene_card_token`
7. `settings_scene_card_token`
8. `locked_scene_card_token`
9. `unknown_scene_card_token`

## Local Safety

- No runtime import was performed.
- No candidate file was copied into `Assets/`.
- No Unity `.meta` files exist in this batch directory.
- No character body, portrait, paw, animation frame, replacement sprite, or source-locked character derivative was produced.
- No HDo/FoW9 map archive coverage is claimed.
- All assets remain candidate-only pending Unity import settings, binding proof, scene-selection screenshots, target-background readability, and clean Console evidence.

## Current Review State

- Visual/UI style review: `PASS_WITH_P2`; all nine accepted as visual candidates; P2 checks remain for actual-layout unknown readability, cat-room/shop/reward distinction, and teal-background contrast.
- Source-lock boundary review: `PASS_WITH_P2`; `dream_route_scene_card_token` is accepted only as an abstract route/progression UI symbol, and `unknown_scene_card_token` is accepted only as an abstract veiled unknown-state token.
- Production QA review: initial `PASS_WITH_P1` because process note, final review decisions, and review-paperwork integration were incomplete at review time; those paperwork P1 items were closed by this process note, completed final review CSV, mirrored review note, and validator pass.
- Final review counts: `candidate_keep=5`, `candidate_conditional=4`, `reject_rework=0`, `pending_review=0`.

Final gate remains `candidate_only_pending_unity_review`.
