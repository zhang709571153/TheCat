# Batch121 Cat-Room Locator Socket Markers Process Note

Batch: `batch_121_cat_room_locator_socket_markers_2026-06-26`
Family: `map`
Current gate: `candidate_complete_pending_unity_review`

## Source Truth

- UI/style: `Qr1XdXd6KosnjMxjgW7cS89kn9c`, recorded locally as revision `816`.
- Local context: Batch91 cat-room elements, Batch92 cat-room interaction states where `home_room_locator_badge` was reclassified as a map-marker candidate, and Batch93 cat-room decals.
- Boundaries: no IAd live approval claim, no HDo/FoW9 archive claim, no character body, face, portrait, paw-print, costume, framesheet, animation, or runtime replacement.

## Process

1. Scaffolded the candidate pack before image generation.
2. Used built-in Codex `imagegen` per the user's package/subscription path, not an API-key workflow. The current built-in tool does not expose an explicit `image2` selector, so this note records the user-requested imagegen/image2 intent without claiming model-lock evidence.
3. Generated a 3x3 source sheet on a flat magenta chroma-key background.
4. Copied the selected built-in output into `source/`.
5. Removed the key locally with the installed imagegen chroma-key helper.
6. Split the alpha sheet into 9 transparent semantic sprites using projection bands.
7. Generated a contact sheet, 96px/64px dark/warm readability board, and 45 review variant PNGs.

## Prompt Summary

Generate nine isolated symbolic cat-room floor locator/socket markers in a 3x3 sheet: bed care, feeder meal, litter clean, dream portal, rest ready, low hunger alert, cleanup needed, return to room, and locked interaction. Style: Qr1-aligned deep navy dreamglass, warm gold bevel trim, teal/rose accents. Constraints: no text, no numbers, no watermark, no full cat body, no cat face, no paw prints, no character portraits, no animation frames, no large furniture props, flat uniform `#ff00ff` background.

## Chroma-Key And Split Results

- Source sheet: `source/thecat_map_catroom_socket_batch121_source_sheet_v001.png`
  - SHA-256: `DB8B0ACE08596E06DD4E24C14F0F5DECBE5B30B155DDCE6FE40AF85434B4294F`
- Alpha sheet: `alpha/thecat_map_catroom_socket_batch121_alpha_sheet_v001.png`
  - Key color: `#f803f8`
  - Transparent pixels: `903871/1572516`
  - Partially transparent pixels: `36059/1572516`
  - SHA-256: `545247F5669E3E0422A624C560CD2E8E7B5567BD7B3080B1BD117B993038EFF8`
- Projection row bands: `(71, 407)`, `(456, 800)`, `(844, 1171)`
- Projection column bands: `(56, 389)`, `(453, 786)`, `(853, 1186)`

## Output Artifacts

- Asset table: `thecat_batch_121_cat_room_locator_socket_markers_2026-06-26_asset_table.csv`
- Manifest: `thecat_map_catroom_socket_batch121_semantic_manifest.csv`
- Contact sheet: `thecat_map_catroom_socket_batch121_semantic_contact_sheet_v001.png`
  - SHA-256: `2DCEAB68FA1EAB3BCC9E9C574BE0BE8D82B2613BD92100E562BCEE18BA37C168`
- Readability board: `thecat_map_catroom_socket_batch121_96px_64px_cat_room_readability_board_v001.png`
  - SHA-256: `57C79CAE5D566CBA1F862744470E06D863D19CF30430A18460577F265376F5B8`
- Semantic sprites: `semantic_sprites/*.png`, 9 files.
- Review variants: `reviews/variants/`, 45 PNGs.
- Final review CSV: `thecat_batch_121_cat_room_locator_socket_markers_2026-06-26_final_review.csv`

## Local Visual Read

- `candidate_keep`: `dream_portal_locator_socket`, `return_to_room_floor_socket`, `locked_interaction_floor_socket`.
- `candidate_conditional`: `bed_care_locator_socket`, `feeder_meal_locator_socket`, `litter_clean_locator_socket`, `rest_ready_floor_socket`, `low_hunger_alert_floor_socket`, `cleanup_needed_alert_floor_socket`.
- Main watch item: several markers read as prop-emblem badges instead of pure floor sockets; Unity cat-room screenshots must prove marker placement and semantic separation.

## Review Outcomes

- Visual/source-boundary review: `PASS_WITH_P2`.
- Target-size readability review: `PASS_WITH_P2`.
- Local production validation: dedicated Batch121 validator passed; generic candidate-pack validator passed.
- Independent production QA review: `PASS_WITH_P2`; no P1; manifest/variant hashes and dimensions align; no `.meta` files or runtime `Assets` leak; max active path length is 241 characters and should stay on the watch list for future nested prompts.

## Safety

- No generated file was copied into `Assets/`.
- No Unity `.meta` file was created by this candidate production pass.
- No runtime import was performed.
- Runtime promotion remains blocked pending cat-room socket screenshots, import settings, binding proof, 96px/64px live contrast, marker-vs-prop proof, Batch91/92/93 semantic collision proof, no recursive candidate import, explicit human approval, and clean Console.
