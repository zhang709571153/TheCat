# character_scene_status_corner_tabs Process Note

Batch: `batch_114_character_scene_status_corner_tabs_2026-06-26`
Family: `ui`
Source truth: `Qr1 UI/style truth revision 816; Batch95/96 role-scene token context; Batch107 scene-selection token context; Batch111/112 scene-preview context; Batch113 character-scene affinity badge context; IAd live ACL blocked, local symbolic source-lock rules only; no body/face/portrait/animation claim`

Process: built-in Codex imagegen skill, workspace source copy, local chroma-key alpha removal, deterministic 3x3 projection split, target-size readability board, independent visual/source-boundary/readability/production QA review, and candidate-only gate.

Model note: the user requested image2 through the Codex imagegen skill. The built-in imagegen tool was used as required, but this environment does not expose a model selector; this pack is therefore recorded as built-in imagegen output and is not claimed as model-locked.

Source and alpha:

- Generated source retained at default Codex path: `C:\Users\PC\.codex\generated_images\019efc23-3cc2-7f23-9beb-cb16d118a13f\ig_0cac6dd6462957c9016a3d9e7bcda8819a8fe6ac6ddd8e6a08.png`.
- Workspace source copy: `source/thecat_ui_character_scene_status_corner_tabs_batch114_chromakey_source_v001.png`.
- v001 alpha sheet: `source/thecat_ui_character_scene_status_corner_tabs_batch114_alpha_sheet_v001.png`; key `#ed04e2`; transparent pixels `1155457/1572516`; partially transparent pixels `15512/1572516`; superseded because production QA found thin semi-transparent magenta fringe.
- v002 alpha sheet: `source/thecat_ui_character_scene_status_corner_tabs_batch114_alpha_sheet_v002.png`; key `#ed04e2`; `--despill --edge-contract 1`; transparent pixels `1164824/1572516`; partially transparent pixels `18500/1572516`; accepted.
- Formal v002 edge check: semi-transparent magenta-like pixels reduced from `5845` in v001 split sprites to `0` in the formal v002 split sprites.

Cut method:

- Projection split from v002 alpha sheet.
- Row bands: `(77,373)`, `(489,769)`, `(883,1167)`.
- Column bands: `(80,343)`, `(499,741)`, `(887,1109)`.
- Pad: `24`.
- Sprites: 9 transparent `semantic_sprites/*batch114_candidate_v001.png` files. Candidate version remains v001; alpha generation version is v002.

Checklist:

- [x] Asset table written before generation.
- [x] Raw source art copied into `source/`.
- [x] Background removed locally and alpha candidates stored outside runtime folders.
- [x] Manifest/contact sheet produced: `thecat_ui_char_scene_tab_batch114_semantic_manifest.csv`; `thecat_ui_char_scene_tab_batch114_semantic_contact_sheet_v001.png`.
- [x] Target-size readability board produced: `thecat_ui_char_scene_tab_batch114_96px_64px_readability_board_v001.png`.
- [x] Visual/source-boundary review completed: `PASS_WITH_P2`.
- [x] Target-size readability review completed: `PASS_WITH_P2`.
- [x] Production QA review completed with P1 fringe finding; P1 fixed by v002 despill/edge-contract recut and zero-magenta-edge check.
- [x] Production QA recheck completed: `PASS`; no remaining local production blocker before `candidate_complete_pending_unity_review`.
- [ ] Runtime import remains blocked until import settings, binding proof, character-scene UI screenshots, and clean console evidence pass.

Asset table: `design/development/asset_candidates/ui/character_scene_status_corner_tabs/batch_114_character_scene_status_corner_tabs_2026-06-26/thecat_batch_114_character_scene_status_corner_tabs_2026-06-26_asset_table.csv`
Manifest: `design/development/asset_candidates/ui/character_scene_status_corner_tabs/batch_114_character_scene_status_corner_tabs_2026-06-26/thecat_ui_char_scene_tab_batch114_semantic_manifest.csv`
Contact sheet: `design/development/asset_candidates/ui/character_scene_status_corner_tabs/batch_114_character_scene_status_corner_tabs_2026-06-26/thecat_ui_char_scene_tab_batch114_semantic_contact_sheet_v001.png`
Readability board: `design/development/asset_candidates/ui/character_scene_status_corner_tabs/batch_114_character_scene_status_corner_tabs_2026-06-26/thecat_ui_char_scene_tab_batch114_96px_64px_readability_board_v001.png`
Final review CSV: `design/development/asset_candidates/ui/character_scene_status_corner_tabs/batch_114_character_scene_status_corner_tabs_2026-06-26/thecat_batch_114_character_scene_status_corner_tabs_2026-06-26_final_review.csv`

No runtime import was performed.
