# character_roster_status_chips Process Note

Batch: `batch_116_character_roster_status_chips_2026-06-26`
Family: `ui`
Source truth: `Qr1 UI/style truth revision 816; IAd local character source-lock boundary; Batch88/95/96/113/114 context; symbolic UI only no character body or portrait`

Process: built-in Codex imagegen on a flat magenta chroma-key background, workspace source copy, local chroma-key alpha removal, deterministic 3x3 sheet split, manifest/contact sheet generation, and 96px/64px readability board generation.

Prompt goal: static textless character roster status chips for a character-select/unlock UI. This is symbolic UI only. It must not be treated as character body art, portraits, costumes, paws, animal silhouettes, animation frames, map background art, or runtime replacement.

Source and cut results:

- Built-in imagegen source copied to `source/tc_ui_roster_batch116_chromakey_source_v001.png`.
- Source SHA-256: `44701d127bc1516a89b63bcccb9f81fbdc1acdf7c7919edff354dcfa9eba447b`.
- Chroma-key helper output: `alpha/tc_ui_roster_batch116_alpha_sheet_v001.png`.
- Alpha SHA-256: `44e27bf8ab29ab634f0dd48fdf4c285271b7984be90f02dc6d82495ad5c09d10`.
- Key color: `#f803e2`.
- Transparent pixels: `1054347/1572516`.
- Partially transparent pixels: `279379/1572516`.
- Split method: 3x3 grid, pad 24, projection bands from `sprite_batch_tools.py`.
- Row bands: `(95,392)`, `(464,791)`, `(854,1147)`.
- Column bands: `(60,415)`, `(437,813)`, `(847,1206)`.
- Manifest: `tc_ui_roster_batch116_semantic_manifest.csv`.
- Contact sheet SHA-256: `e212acfb5329bf30a7b32a51d09854825d42fa7c33e3daf8dbddb651c1a67723`.
- Readability board: `tc_ui_roster_batch116_96px_64px_character_roster_readability_board_v001.png`.
- Readability board SHA-256: `41519d7037b5663137024293126326aa9dc89004892dd69a66ec3b22b2e7b62e`.
- Rows: 9 semantic sprites.
- Review verdicts: visual/source-boundary `PASS_WITH_P2`, 96px/64px readability `PASS_WITH_P2`, production QA `PASS_WITH_P2`.
- Final review CSV: 5 `candidate_keep`, 4 `candidate_conditional`, 0 `reject_rework`, 0 `pending_review`.
- Final review plain summary: 5 candidate_keep, 4 candidate_conditional, 0 reject_rework, 0 pending_review.
- Built-in Codex imagegen was used through the imagegen skill. The built-in tool does not expose a model selector, so this process note does not claim a model-locked image2/API generation path.

Checklist:

- [x] Asset table written before generation.
- [x] Raw source art copied into `source/`.
- [x] Background removed locally and alpha candidates stored outside runtime folders.
- [x] Manifest/contact sheet or review variants produced.
- [x] Visual/style review completed.
- [x] Target-size readability review completed.
- [x] Production QA review completed.
- [ ] Runtime import remains blocked until import settings, binding proof, screenshots, and clean console evidence pass.

Asset table: `design/development/asset_candidates/ui/character_roster_status_chips/batch_116_character_roster_status_chips_2026-06-26/thecat_batch_116_character_roster_status_chips_2026-06-26_asset_table.csv`
Final review CSV: `design/development/asset_candidates/ui/character_roster_status_chips/batch_116_character_roster_status_chips_2026-06-26/thecat_batch_116_character_roster_status_chips_2026-06-26_final_review.csv`

No runtime import was performed.
