# character_team_slot_markers Process Note

Batch: `batch_118_character_team_slot_markers_2026-06-26`
Family: `ui`
Source truth: `Qr1 UI/style truth revision 816; IAd local source-lock boundary; Batch95/113/116 symbolic character UI context; symbolic-only no character body art; no portrait/framesheet/identity replacement claim`

Process: candidate-only asset production with built-in Codex `imagegen` skill, workspace source copy, local magenta chroma-key alpha removal, 3x3 projection split, contact sheet, target-size readability proof, and review variants.

Generation note: the active built-in `imagegen` path does not expose a model selector in this environment, so this batch records the user-requested imagegen path but does not claim a model-locked `image2` API run.

Prompt intent: create nine symbolic character team-slot UI markers for front line, rear support, healer, shield guard, burst skill, utility control, synergy pair, reserve bench, and empty locked slot. The prompt explicitly forbade character bodies, faces, portraits, paws, cats, baked text, letters, numbers, watermarks, shadows on the key background, merged cells, and identity replacement claims.

Chroma-key and cut results:

- Source sheet: `source/thecat_ui_team_slot_batch118_chromakey_source_v001.png`
- Alpha sheet: `alpha/thecat_ui_team_slot_batch118_alpha_sheet_v001.png`
- Key color: `#ff00ff`
- Transparent pixels: `1045830/1572516`
- Partially transparent pixels: `17332/1572516`
- Cut method: `3 rows x 3 columns projection split`, `pad=24`
- Projection row bands: `(77,407)`, `(467,788)`, `(848,1169)`
- Projection column bands: `(86,390)`, `(472,780)`, `(856,1163)`
- Active semantic sprites: `9`
- Active review variant PNGs: `45`
- Active review variant manifests: `9`
- Superseded note: the first long-path review variant set was archived to `superseded/long_path_variants_v001/` with `archive_map.csv`; active variants were rebuilt with short prefixes `b118_01_front` through `b118_09_lock`.
- Max active variant path length after short-prefix rebuild: `201`

Focused front-line v002 rework:

- Maxwell visual/source-boundary review found dirty/key-source contamination in the upper-left navy field of `front_line_slot_marker` v001 and a shield-language collision with `shield_guard_slot_marker`.
- New source: `source/thecat_ui_team_slot_front_line_slot_marker_v002_chromakey_source.png`
- New alpha: `alpha/thecat_ui_team_slot_front_line_slot_marker_v002_alpha.png`
- Key color: `#ff00ff`
- Transparent pixels: `1064736/1572516`
- Partially transparent pixels: `5089/1572516`
- Active sprite: `semantic_sprites/thecat_ui_team_slot_front_line_slot_marker_batch118_candidate_v002.png`
- Active sprite SHA256: `5ef00deb5b006d56b2b924906a340ffccf37c1250449f1881df6e8fb070ced79`
- v001 sprite was moved to `superseded/front_line_v001/`.
- Active contact sheet was rebuilt as `thecat_ui_team_slot_batch118_semantic_contact_sheet_v002.png`.
- Active readability board was rebuilt as `thecat_ui_team_slot_batch118_96px_64px_dark_warm_readability_board_v002.png`.

Review evidence:

- Manifest: `thecat_ui_team_slot_batch118_semantic_manifest.csv`
- Contact sheet: `thecat_ui_team_slot_batch118_semantic_contact_sheet_v002.png`
- Readability board: `thecat_ui_team_slot_batch118_96px_64px_dark_warm_readability_board_v002.png`
- Final review CSV: `thecat_batch_118_character_team_slot_markers_2026-06-26_final_review.csv`
- Agent review note: `BATCH118_CHARACTER_TEAM_SLOT_MARKERS_AGENT_REVIEW_2026-06-26.md`

Initial self-check:

- All 9 active sprites have alpha extrema `(0, 255)`.
- All 9 active sprites have transparent corners.
- No character body, face, portrait, paw, or baked text is present in the active contact sheet.
- Independent initial reviews returned `PASS_WITH_P2` for visual/source-boundary, target-size readability, and production QA.
- Integrated final decision is `3 candidate_keep` and `6 candidate_conditional`.
- Focused delta reviews confirmed the front-line v002 rework closed the dirty-source issue and no P1 remains.
- `reserve_bench_marker` is readable but softer at 64px on warm backgrounds; keep as a review watch item unless runtime screenshots prove it cleanly.
- No runtime import was performed.

Checklist:

- [x] Asset table written before generation.
- [x] Raw source art copied into `source/`.
- [x] Background removed locally and alpha candidates stored outside runtime folders.
- [x] Manifest/contact sheet or review variants produced.
- [x] Visual/style review completed.
- [x] Production QA review completed.
- [ ] Runtime import remains blocked until import settings, binding proof, screenshots, and clean console evidence pass.

Asset table: `design/development/asset_candidates/ui/character_team_slot_markers/batch_118_character_team_slot_markers_2026-06-26/thecat_batch_118_character_team_slot_markers_2026-06-26_asset_table.csv`
Final review CSV: `design/development/asset_candidates/ui/character_team_slot_markers/batch_118_character_team_slot_markers_2026-06-26/thecat_batch_118_character_team_slot_markers_2026-06-26_final_review.csv`

No runtime import was performed.
