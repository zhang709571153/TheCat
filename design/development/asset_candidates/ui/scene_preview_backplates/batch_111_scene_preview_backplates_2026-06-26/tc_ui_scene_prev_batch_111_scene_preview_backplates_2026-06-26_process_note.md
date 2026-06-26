# Batch111 Scene Preview Backplates Process Note

Batch: `batch_111_scene_preview_backplates_2026-06-26`

Family: `ui`

Source truth: `Qr1 UI/style truth revision 816; Batch107 scene-selection token context; Batch110 bedroom map overlay context; safe scene-preview UI backplates only; no IAd character-body claim; no HDo/FoW9 map-archive claim`

Process: built-in Codex imagegen, workspace copy, local chroma-key alpha removal, projection split into independent transparent sprites, review variants, 192x108 readability board, independent agent review, and candidate-only gate recording.

## Source And Cut

- v001 rejected: `superseded/rejected_source/tc_ui_scene_prev_batch111_rejected_source_v001_paw_markers.png`
  - Reason: paw markers appeared in the cat-room preview source and created source-lock/identity ambiguity.
- v002 accepted: `source/tc_ui_scene_prev_batch111_chromakey_source_v002.png`
  - Reason: clean symbolic UI/scene-preview cards with no character body, cat face, paw/print marker, animation frame, or formal map/archive claim.
- Alpha sheet: `alpha/tc_ui_scene_prev_batch111_alpha_sheet_v002.png`
- Chroma key: auto-detected key color `#ef03e7`.
- Alpha removal result: transparent pixels `326018/1572864`; partial transparent pixels `17242/1572864`.
- Projection rows: `(16, 339)`, `(347, 662)`, `(672, 992)`.
- Projection columns: `(15, 491)`, `(521, 999)`, `(1032, 1510)`.

## Outputs

- Asset table: `tc_ui_scene_prev_batch_111_scene_preview_backplates_2026-06-26_asset_table.csv`
- Manifest: `tc_ui_scene_prev_batch111_semantic_manifest.csv`
- Contact sheet: `tc_ui_scene_prev_batch111_semantic_contact_sheet_v001.png`
- 192x108 readability board: `tc_ui_scene_prev_batch111_192x108_scene_preview_readability_board_v001.png`
- Final review CSV: `tc_ui_scene_prev_batch_111_scene_preview_backplates_2026-06-26_final_review.csv`
- Review variants: `reviews/variants/`

Rows: `9`

Semantic sprites: `9`

Review variant files: `54` total, including `45` PNGs and `9` variant manifests.

## Review Integration

- Noether visual/style review: `PASS_WITH_P2`.
  - All 9 cards share the Qr1-style gold bevel, navy dreamglass, cream/warm interior lighting, and storybook ornament system.
  - 192x108 previews are readable over checker, night, and warm backgrounds.
- Bernoulli source-lock review: `PASS_WITH_P2`.
  - v001 rejection is preserved because of paw markers.
  - v002 remains symbolic UI only and does not claim character identity, cat-room layout truth, route-map truth, Egypt archive coverage, or formal runtime replacement.
- Faraday production QA review: initial `PASS_WITH_P1`.
  - PNG payload passed mechanical QA.
  - P1 gate-record gaps were resolved by this note, the mirrored candidate review note, and final review CSV.
  - P2 absolute-path variant manifests were normalized to workspace-relative paths.

Integrated gate: `PASS_WITH_P2`, `candidate_complete_pending_unity_review`.

## Checklist

- [x] Asset table written before generation.
- [x] Raw source art copied into `source/`.
- [x] Rejected v001 preserved under `superseded/rejected_source/`.
- [x] Background removed locally and alpha candidates stored outside runtime folders.
- [x] Manifest/contact sheet and 192x108 readability board produced.
- [x] Review variants produced.
- [x] Visual/style review completed.
- [x] Source-lock boundary review completed.
- [x] Production QA review completed.
- [x] Final review CSV integrated with manifest asset ids.
- [ ] Runtime import remains blocked until import settings, binding proof, scene-selection screenshots, and clean console evidence pass.

Safety checks:

- `0 .meta` files in candidate package.
- 0 `.meta` files in candidate package.
- `0 runtime Assets/ writes`.
- 0 runtime `Assets/` writes.

No runtime import was performed.
