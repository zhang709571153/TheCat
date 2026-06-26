# Batch 100 Dream Theme Scene Token Process Note

Date: 2026-06-25

Status: candidate_only_pending_unity_theme_selection_review

## Scope

Batch 100 produces nine transparent UI candidate sprites for dream-theme or scene-selection tokens. This is a UI-only symbolic batch. No character bodies, portraits, animation frames, replacements, map backgrounds, or character-source claims are included.

## Source Truth

- Qr1 UI/style authority revision 816.
- Batch86 dream-route preflight as local dream-entry and route context.
- Qr1 confirms the P0 has bedroom and Egypt dream map requirements, but this batch only produces symbolic UI theme tokens.
- The HDo map art archive is still ACL-blocked and was not used. The Egypt token must not be treated as map-art archive coverage.
- `IAdkdcpciobUTXxa7dBcRx7Bngf` remains live ACL-blocked. Local character source-lock packets were not used; this batch contains no character body, portrait, framesheet, animation, or replacement claim.

## Method

- Generation: built-in Codex imagegen on a flat magenta chroma-key background.
- Selected generated source: `C:\Users\PC\.codex\generated_images\019efc23-3cc2-7f23-9beb-cb16d118a13f\ig_0e1f92dbf4a2f346016a3d40901ed0819a9efea004e837a965.png`.
- Workspace source: `source/thecat_ui_dream_theme_scene_tokens_batch100_chromakey_source_v001.png`.
- Alpha removal: imagegen `remove_chroma_key.py` helper with auto-key border sampling, soft matte, despill, and edge contract.
- Alpha sheet: `source/thecat_ui_dream_theme_scene_tokens_batch100_alpha_sheet_v001.png`.
- Key color sampled by helper: `#f702f6`.
- Cutting method: `sprite_batch_tools.py split-sheet`, 3 rows x 3 columns, alpha projection bands, 24 px padding.
- Path-length fix: the first split attempt used the full batch slug as the batch id and failed on a long Windows path. The final split uses short `batch100` file suffixes inside the full batch directory.

## Output

- Asset table: `thecat_ui_dream_theme_scene_tokens_batch100_asset_table.csv`.
- Manifest: `thecat_ui_theme_token_batch100_semantic_manifest.csv`.
- Contact sheet: `thecat_ui_theme_token_batch100_semantic_contact_sheet_v001.png`.
- 64px/theme-selection readability board: `thecat_ui_theme_token_batch100_64px_theme_selection_board_v001.png`.
- Final review CSV: `thecat_ui_theme_token_batch100_final_review.csv`.
- Semantic sprites: 9 transparent PNGs under `semantic_sprites/`.

## Current Local Observations

- All nine semantic sprites were cut in the intended reading order.
- Manifest rows preserve dimensions, alpha extrema, projection crops, and SHA-256 hashes.
- The 64px readability board shows clear silhouettes for all tokens on dark, warm, and framed theme-slot backgrounds.
- P2 watch items: the Egypt token is symbolic only and does not prove HDo archive coverage; the available glow ring has soft edges; the boss warning is dense at 64px; the unknown veil needs contrast verification on light cards.

## Gate

No Unity import was performed.

Remaining gates:

- Independent visual/style review.
- Independent source-lock review.
- Independent production QA review.
- Unity theme-selection screenshot proof.
- Sprite import settings.
- Binding proof.
- Clean Console.
- Explicit runtime import policy and target paths.
