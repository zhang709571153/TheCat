# Batch105 Combat Feedback Tokens Process Note

Batch: `batch_105_combat_feedback_2026-06-26`

Family: `vfx`

Source truth: `Qr1 UI/style truth revision 816; VFX-02 combat feedback P0 boundary`.

Process: built-in Codex `imagegen` source sheet, workspace copy, local magenta chroma-key alpha removal, projection-based 3x3 sprite cutting, semantic manifest/contact sheet generation, and local 64px readability board generation.

## Source And Safety

- Source image copied from built-in imagegen default output into `source/tc_vfx_cfb_batch105_chromakey_source_v001.png`.
- Original built-in output retained at `C:\Users\PC\.codex\generated_images\019efc23-3cc2-7f23-9beb-cb16d118a13f\ig_082a90d4063c3c75016a3d605de8b881999c7ec4d46f7a8509.png`.
- Local alpha sheet: `alpha/tc_vfx_cfb_batch105_alpha_sheet_v001.png`.
- Chroma key: auto-detected border key `#f703f5`, generated from requested flat `#ff00ff` key.
- Transparent pixels: `1041576/1572516`.
- Partially transparent pixels: `103169/1572516`.
- No runtime import was performed.
- No character body, portrait, animation frame, framesheet, starter-cat motif derivative, enemy body, Egypt map archive, HDo/FoW9 folder coverage, or IAd character approval is claimed.

## Semantic Order

1. `normal_hit_spark_token`
2. `shield_flash_token`
3. `slow_freeze_token`
4. `aftershock_ring_token`
5. `bed_hit_crack_token`
6. `monster_death_puff_token`
7. `damage_number_chip_token`
8. `heal_plus_spark_token`
9. `debuff_burst_token`

## Cut Results

- Cut method: alpha projection bands on the local alpha sheet.
- Row bands: `(65,397)`, `(475,779)`, `(854,1166)`.
- Column bands: `(69,402)`, `(485,762)`, `(853,1183)`.
- Rows: 9 semantic sprites.
- Manifest: `tc_vfx_cfb_batch105_semantic_manifest.csv`.
- Contact sheet: `tc_vfx_cfb_batch105_semantic_contact_sheet_v001.png`.
- 64px readability board: `tc_vfx_cfb_batch105_64px_combat_feedback_readability_board_v001.png`.

## Review Notes

- Initial visual self-check: 9 cells separated, consistent warm dream-defense VFX/UI token family, no text, no digits, no character/body/portrait content.
- `bed_hit_crack_token` is deliberately a symbolic cracked dream-bed corner rather than a full bed sprite. It remains a runtime scale/source-context watch item.
- `damage_number_chip_token` is an empty number plaque with no baked number; runtime text/number rendering remains separate.
- `monster_death_puff_token` and `debuff_burst_token` need live dark-background contrast checks because both use dark smoke/mud language.
- Independent visual/style, source-lock/boundary, and production QA reviews were dispatched on 2026-06-26.

## Checklist

- [x] Asset table written before generation.
- [x] Raw source art copied into `source/`.
- [x] Background removed locally and alpha candidates stored outside runtime folders.
- [x] Manifest/contact sheet produced.
- [x] 64px readability board produced.
- [x] Independent visual/style review integrated.
- [x] Independent source-lock/boundary review integrated.
- [x] Independent production QA review integrated after P1 package-note/final-review fixes.
- [x] Root-level final review note mirrored into `design/development/asset_review/`.
- [x] Batch105 validator added to the local non-cat/symbolic matrix.
- [ ] Runtime import remains blocked until import settings, binding proof, combat-feedback screenshots, and clean console evidence pass.

Asset table: `tc_vfx_cfb_batch_105_combat_feedback_2026-06-26_asset_table.csv`.

Final review CSV: `tc_vfx_cfb_batch_105_combat_feedback_2026-06-26_final_review.csv`.
