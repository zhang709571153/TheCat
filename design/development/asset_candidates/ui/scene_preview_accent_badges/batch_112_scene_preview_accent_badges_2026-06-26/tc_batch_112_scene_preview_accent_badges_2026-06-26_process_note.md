# scene_preview_accent_badges Process Note

Batch: `batch_112_scene_preview_accent_badges_2026-06-26`
Family: `ui`
Source truth: `Qr1 UI/style truth revision 816; Batch107 scene-selection token context; Batch111 scene-preview backplate context; no IAd character-body claim; no HDo/FoW9 map-archive claim`

Process: built-in Codex imagegen source, workspace copy, local magenta chroma-key alpha removal, deterministic 3x3 projection split, target-size readability board, independent visual/readability/production QA review.

Prompt or derivation goal: produce nine static scene-preview accent badges for bedroom, cat room, dream route, battle, reward, shop/event, settings, locked, and unknown states. These are candidate-only symbolic UI overlays for scene-selection review. They are not character identity art, not animation frames, not map archive truth, and not runtime replacements.

Source and alpha results:

- Source copied into workspace: `source/tc_ui_scene_accent_batch112_chromakey_source_v001.png`.
- v001 alpha: `alpha/tc_ui_scene_accent_batch112_alpha_sheet_v001.png`; generated with `--despill`; rejected after visual review found light/glow color damage in cat-room, reward, shop/event, and unknown candidates. The rejected alpha/contact are preserved under `superseded/rejected_alpha/`.
- v002 alpha: `alpha/tc_ui_scene_accent_batch112_alpha_sheet_v002_no_despill.png`; generated without `--despill`; accepted as the current sprite source because lantern, reward spotlight, and galaxy glow preserve the source color.
- Chroma key: `#ef05d7`.
- v002 transparent pixels: `830940/1572516`.
- v002 partially transparent pixels: `387370/1572516`.
- Cut method: 3x3 projection split with 24px padding.
- Row bands: `(73, 400)`, `(454, 781)`, `(836, 1162)`.
- Column bands: `(84, 396)`, `(469, 783)`, `(856, 1169)`.

Current artifacts:

- [x] Asset table written before generation.
- [x] Raw source art copied into `source/`.
- [x] Background removed locally and alpha candidates stored outside runtime folders.
- [x] Manifest/contact sheet produced: `tc_ui_scene_accent_batch112_semantic_manifest.csv`, `tc_ui_scene_accent_batch112_semantic_contact_sheet_v001.png`.
- [x] Target-size readability board produced: `tc_ui_scene_accent_batch112_96px_64px_scene_preview_readability_board_v001.png`.
- [x] Visual/style review completed; initial P1 alpha-damage finding fixed by v002 no-despill recut.
- [x] Production QA review completed; file, alpha, hash, padding, and no-runtime-write checks passed after asset-id alignment.
- [x] Runtime import remains blocked until import settings, binding proof, screenshots, and clean console evidence pass.

Decision summary:

- Integrated review verdict: `PASS_WITH_P2`.
- `candidate_keep`: `dream_route_accent_badge`, `reward_accent_badge`, `settings_accent_badge`, `locked_accent_badge`, `unknown_accent_badge`.
- `candidate_conditional`: `bedroom_accent_badge`, `cat_room_accent_badge`, `battle_accent_badge`, `shop_event_accent_badge`.
- Conditional reason: several candidates read as mini scene cards or have dense/dark details at 64px. Actual scene-selection placement over Batch111 backplates is required before Unity import.

No runtime import was performed. No candidate file was copied into `Assets/`, and no Unity `.meta` files were created in this candidate package.
No candidate file was copied into Assets/.
