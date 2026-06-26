# Batch 102 Bedroom Dream Map Decals Process Note

Date: 2026-06-25

Process: built-in Codex `imagegen` source sheet, workspace copy, local magenta chroma-key alpha removal, projection split into semantic sprites, row-8 drift cleanup, v003 hard-key salvage for the blanket trim only, contact-sheet regeneration, and 64px bedroom-map readability board generation.

Source truth: `Qr1 UI/style authority revision 816`; Qr1 P0 bedroom dream map requirement; Batch54/67 bedroom interaction context; Batch101 bedroom battle marker context.

Prompt goal: produce a static no-animation 3x3 sprite sheet of bedroom dream map decals and small soft obstacle/map elements. The batch is restricted to map/UI environment sprites: no characters, no cat bodies, no portraits, no enemy bodies, no framesheets, no animation frames, no text, and no runtime replacement.

Source and alpha files:

- Chroma-key source: `source/thecat_map_bedroom_dream_map_decals_batch102_chromakey_source_v001.png`
- Alpha sheet: `source/thecat_map_bedroom_dream_map_decals_batch102_alpha_sheet_v001.png`
- Conservative alpha attempt: `source/thecat_map_bedroom_dream_map_decals_batch102_alpha_sheet_v002_conservative.png`
- Hard-key salvage attempt: `source/thecat_map_bedroom_dream_map_decals_batch102_alpha_sheet_v003_hardkey.png`
- Semantic order: `source/thecat_map_beddec_batch102_names.txt`
- Asset table: `thecat_map_bedroom_dream_map_decals_batch102_asset_table.csv`
- Manifest: `thecat_map_beddec_batch102_semantic_manifest.csv`
- Contact sheet: `thecat_map_beddec_batch102_semantic_contact_sheet_v001.png`
- 64px board: `thecat_map_beddec_batch102_64px_bedroom_map_readability_board_v001.png`
- Final review CSV: `thecat_map_beddec_batch102_final_review.csv`

Chroma-key and cut results:

- Key color sampled by helper: `#fa02f9`.
- Transparent pixels: `1112328/1572516`.
- Partially transparent pixels: `131048/1572516`.
- Cut method: alpha projection bands with 3 rows x 3 columns and 24px padding.
- Projection bands: rows `[(81, 400), (488, 764), (824, 1209)]`; columns `[(0, 418), (418, 836), (836, 1254)]`.
- Manual cleanup: `bedroom_nightmare_puddle_decal` contained a thin right-edge drift fragment from the adjacent bed-aura cell; removed the isolated component and regenerated the manifest hash plus contact sheet.
- Salvage: `bedroom_blanket_boundary_trim` was upgraded to `candidate_v002` from the hard-key sheet because the initial soft matte damaged purple trim detail less than the prop-like cells but still benefited from a cleaner source.
- Review classification: visual review rejected `bedroom_pillow_barricade_decal`, `bedroom_toy_block_soft_obstacle`, `bedroom_nightmare_puddle_decal`, and `bedroom_bed_aura_floor_glow` for rework before import; they remain archived in the manifest for traceability only.

Rows: 9 semantic sprites.

No Unity import was performed. No files were copied into `Assets/`, and no Unity `.meta` files should exist under this candidate package.

Current gate: `partial_candidate_only_pending_unity_bedroom_map_decal_review_with_rework_required`.

Remaining Unity gates: import settings, target paths, scene/prefab binding proof, bedroom map screenshots, floor contrast, soft-obstacle collider ownership, sorting-layer proof, and clean Console.
