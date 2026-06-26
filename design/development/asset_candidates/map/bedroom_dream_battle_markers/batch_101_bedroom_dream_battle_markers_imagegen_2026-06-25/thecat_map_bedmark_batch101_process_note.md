# Batch101 Bedroom Dream Battle Markers Process Note

Date: 2026-06-25

Process: built-in Codex imagegen source sheet, workspace copy, local magenta chroma-key removal, projection-band split into nine semantic sprites, contact sheet and 64px target readability board generation.

Source truth: `Qr1 UI/style authority revision 816; Qr1 P0 bedroom dream map requirement; Batch54/67 bedroom interaction context`.

Prompt goal: produce static, textless, candidate-only bedroom dream battle map/UI marker sprites for bed defense, enemy entry direction markers, safe-zone boundary, path direction, blocker, and spawn warning. No character or animation content was requested or generated.

Generated source:

- Built-in Codex imagegen output: `C:\Users\PC\.codex\generated_images\019efc23-3cc2-7f23-9beb-cb16d118a13f\ig_09c9c051f545dab1016a3d45b8bd64819ab7bf284aa86c2da4.png`
- Workspace source copy: `design/development/asset_candidates/map/bedroom_dream_battle_markers/batch_101_bedroom_dream_battle_markers_imagegen_2026-06-25/source/thecat_map_bedroom_battle_markers_batch101_chromakey_source_v001.png`

Chroma-key and cut results:

- Magenta key auto-sampled as `#f603f2`.
- Transparent pixels: `1044151/1572516`.
- Partially transparent pixels: `36005/1572516`.
- Alpha sheet: `source/thecat_map_bedroom_battle_markers_batch101_alpha_sheet_v001.png`.
- Split method: `asset-sprite-production/scripts/sprite_batch_tools.py split-sheet` using 3 rows, 3 columns, projection bands, 24 px padding, alpha threshold 12.
- Projection row bands: `(41, 411)`, `(456, 788)`, `(829, 1188)`.
- Projection column bands: `(57, 397)`, `(429, 798)`, `(849, 1219)`.

Artifacts:

- Asset table: `thecat_map_bedroom_battle_markers_batch101_asset_table.csv`.
- Manifest: `thecat_map_bedmark_batch101_semantic_manifest.csv`.
- Contact sheet: `thecat_map_bedmark_batch101_semantic_contact_sheet_v001.png`.
- 64px readability board: `thecat_map_bedmark_batch101_64px_bedroom_map_readability_board_v001.png`.
- Final review CSV: `thecat_map_bedmark_batch101_final_review.csv`.

Local review summary:

- All nine sprites have alpha extrema `(0, 255)` and `ok` manifest status.
- Four enemy entry markers read as directional variants at contact-sheet size and remain distinguishable at 64px.
- `bedroom_safe_zone_boundary_ring` and `bedroom_path_arrow_marker` are candidate-conditional until actual bedroom-battle screenshot contrast proves the thin blue lines remain visible.
- `bedroom_spawn_warning_marker` is strong at 64px but needs red warning contrast proof against live battle/map backgrounds.
- `bedroom_obstacle_blocker_marker` may read as a soft cushion/pad; runtime collision meaning should come from scene data, not automatic collider inference.

Gate: `candidate_only_pending_unity_bedroom_battle_marker_review`.

No Unity import was performed. No files were written to `Assets/`. No character bodies, portraits, framesheets, enemy bodies, or animation frames were generated.
