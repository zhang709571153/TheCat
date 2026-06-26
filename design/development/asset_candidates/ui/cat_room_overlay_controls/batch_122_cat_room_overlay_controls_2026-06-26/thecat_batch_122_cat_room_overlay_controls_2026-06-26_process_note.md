# cat_room_overlay_controls Process Note

Batch: `batch_122_cat_room_overlay_controls_2026-06-26`
Family: `ui`
Source truth: `Qr1 UI/style truth revision 816; Batch90 cat-room UI; Batch91/92/93 cat-room context; Batch121 locator/socket context; no IAd live approval claim; no HDo/FoW9 archive claim`

Gate: `candidate_complete_pending_unity_review`

Process: built-in Codex `imagegen` via the `imagegen` skill, workspace source copy, local chroma-key alpha removal, deterministic 3x3 semantic split, target-size readability board, and review variant generation. The user asked for imagegen/image2 intent; this Codex environment exposes the built-in image generation tool rather than an explicit selectable model field, so this note records the requested `imagegen/image2` intent without claiming a visible model-lock.

Prompt or derivation goal: create nine static textless cat-room overlay-control sprites in Qr1-style dreamglass UI language, using deep navy, warm gold, teal glow, rose accent, generous padding, no text, no watermark, no cats, no bodies, no portraits, no paw motifs, no animation, and a flat magenta chroma-key background.

Source and alpha:

- Source sheet: `design/development/asset_candidates/ui/cat_room_overlay_controls/batch_122_cat_room_overlay_controls_2026-06-26/source/thecat_ui_catroom_overlay_batch122_source_sheet_v001.png`
- Source SHA256: `2E73CA882461ABE1D764B731E01872362A30FA74E727E222243B448AE8F35081`
- Alpha sheet: `design/development/asset_candidates/ui/cat_room_overlay_controls/batch_122_cat_room_overlay_controls_2026-06-26/alpha/thecat_ui_catroom_overlay_batch122_alpha_sheet_v001.png`
- Alpha SHA256: `4285FEA7764A055DFBFFFC0147A6DB2C378920686F075F00107A4FEFD8573258`
- Key: magenta border key around `#ed04f3` to `#f405f4`.
- Alpha stats: 815987 transparent pixels, 61803 partially transparent pixels, 694726 opaque pixels, 1572516 total pixels.
- Cut method: fixed 3x3 reading-order cell split with alpha-bbox crop and 10px padding.

Outputs:

- Names file: `batch122_names.txt`
- Semantic sprites: 9 PNGs under `semantic_sprites/`.
- Manifest: `thecat_ui_catroom_overlay_batch122_semantic_manifest.csv`
- Contact sheet: `thecat_ui_catroom_overlay_batch122_semantic_contact_sheet_v001.png`, SHA256 `BEC9ADB400F0F81760D360EBD24E0A1C8E28847979D342F7ADD2ED4D57B3BD02`
- 96px/64px readability board: `thecat_ui_catroom_overlay_batch122_96px_64px_readability_board_v001.png`, SHA256 `307C710E45FC3D4C43FB3A7A184CB70BDC806BCA2527347C61278585E0AEEF58`
- Review variants: 45 review variant PNGs and 9 variant manifests under `reviews/variants/`.

Local visual read:

- `candidate_keep`: `needs_attention_filter_toggle`, `interaction_range_overlay_toggle`, `comfort_zone_overlay_toggle`, `return_to_default_view_control`.
- `candidate_conditional`: `bed_zone_focus_control`, `feeder_zone_focus_control`, `litter_zone_focus_control`, `dream_gate_focus_control`, `resource_need_filter_toggle`.
- Main watch item: first four controls contain object/zone motifs and must prove they read as overlay controls rather than props, Batch121 locator/socket markers, or existing cat-room state tokens.

Checklist:

- [x] Asset table written before generation.
- [x] Raw source art copied into `source/`.
- [x] Background removed locally and alpha candidates stored outside runtime folders.
- [x] Manifest/contact sheet or review variants produced.
- [x] Visual/style review completed: `PASS_WITH_P2`.
- [x] Target-size readability review completed: `PASS_WITH_P2`.
- [x] Production QA review completed: `PASS_WITH_P2`.
- [ ] Runtime import remains blocked until import settings, binding proof, screenshots, and clean console evidence pass.

Asset table: `design/development/asset_candidates/ui/cat_room_overlay_controls/batch_122_cat_room_overlay_controls_2026-06-26/thecat_batch_122_cat_room_overlay_controls_2026-06-26_asset_table.csv`
Final review CSV: `design/development/asset_candidates/ui/cat_room_overlay_controls/batch_122_cat_room_overlay_controls_2026-06-26/thecat_batch_122_cat_room_overlay_controls_2026-06-26_final_review.csv`
Agent review note: `design/development/asset_candidates/ui/cat_room_overlay_controls/batch_122_cat_room_overlay_controls_2026-06-26/BATCH122_CAT_ROOM_OVERLAY_CONTROLS_AGENT_REVIEW_2026-06-26.md`

No runtime import was performed.
