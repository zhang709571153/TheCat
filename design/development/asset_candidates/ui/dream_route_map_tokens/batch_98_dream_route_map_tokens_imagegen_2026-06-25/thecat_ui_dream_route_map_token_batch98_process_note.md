# Batch 98 Dream Route Map Tokens Process Note

Date: 2026-06-25

Mode: built-in Codex `imagegen` plus local magenta chroma-key removal.

## Goal

Produce a narrow, candidate-only static UI/map token kit for the dream-route screen. This batch intentionally avoids character bodies, portraits, framesheets, recolors, runtime replacements, animation, baked text, and full scene paintings.

## Source Boundary

- UI/style authority: Feishu `Qr1XdXd6KosnjMxjgW7cS89kn9c`, revision `816`.
- Local route composition reference: Batch 86 dream-route preflight.
- Character design doc remains ACL-blocked live; this batch does not depend on character-body or portrait source truth.

## Produced Files

- `source/thecat_ui_dream_route_map_tokens_batch98_chromakey_source_v001.png`
- `source/thecat_ui_dream_route_map_tokens_batch98_alpha_sheet_v001.png`
- `source/thecat_ui_dream_route_map_tokens_batch98_names.txt`
- `semantic_sprites/*.png`
- `thecat_ui_dream_route_map_tokens_batch98_asset_table.csv`
- `thecat_ui_dream_route_map_token_batch98_semantic_manifest.csv`
- `thecat_ui_dream_route_map_token_batch98_semantic_contact_sheet_v001.png`
- `thecat_ui_dream_route_map_token_batch98_64px_readability_board_v001.png`
- `thecat_ui_dream_route_map_token_batch98_final_review.csv`

## Cutting Notes

The generated image was a square 3 by 3 sheet on a flat magenta key. Local chroma-key removal sampled the border as `#f903e9` and produced an alpha sheet with alpha extrema `(0, 255)`.

Projection bands from the splitter:

- Rows: `(81,396)`, `(478,762)`, `(859,1160)`.
- Columns: `(67,394)`, `(479,789)`, `(887,1183)`.

Each semantic sprite was cropped to alpha bounds and padded with 24 px transparent margins.

## Local Validation

- Manifest rows: 9.
- Final review rows: 9.
- Missing sprite paths: 0.
- Hash, dimension, and alpha-extrema mismatches: 0.
- Nontransparent outer edge pixels: 0.
- `.meta` files in candidate package: 0.
- `Assets/` promotion: none.
- Longest candidate-package file path observed: 252 characters.

## Review Outcome

- Visual/style review: `PASS_WITH_P2`.
- Source-lock review: final `PASS_WITH_P2`; initial provenance-wording P1 was fixed by narrowing the asset table to Qr1 plus Batch 86 only.
- Production QA review: `PASS_WITH_P2`.

Current gate: `candidate_only_pending_unity_import_settings_route_map_screenshot_console`.

Main P2 watches: boss gate density at 64 px, current-vs-selectable node distinction after Unity glow treatment, locked node/connector contrast on the live dark route background, teal event-mist color drift in future edits, and path length near legacy Windows/Unity limits.
