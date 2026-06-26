# character_scene_affinity_badges Process Note

Batch: `batch_113_character_scene_affinity_badges_2026-06-26`
Family: `ui`
Source truth: `Qr1 UI/style truth revision 816; Batch95 role-scene token context; Batch96 unlock role-scene token context; Batch97 unlock skill icon context; IAd live ACL blocked, local symbolic source-lock rules only; no body/face/portrait/animation claim`

Process: built-in Codex imagegen source, workspace copy, local magenta chroma-key alpha removal, deterministic 3x3 projection split, target-size readability board, independent visual/source-boundary, readability, and production QA review.

Generation note: the user requested the built-in `imagegen` skill and `image2`. The active built-in image tool was used and does not expose a model selector in this environment, so this batch is recorded as built-in Codex imagegen output rather than model-locked CLI `image2`.

Prompt or derivation goal: produce nine static character-scene affinity badges for symbolic UI review. These badges express scene-role relationships only. They are not character bodies, portraits, costumes, animation frames, formal character replacements, map archive truth, or runtime imports.

Source and alpha results:

- Source copied into workspace: `source/thecat_ui_character_scene_affinity_badges_batch113_chromakey_source_v001.png`.
- Generated candidates review sheet: `reviews/batch113_generated_candidates_contact_sheet.png`.
- Selected source: candidate 01 from the built-in imagegen output folder, copied without deleting the original generated image.
- Alpha sheet: `source/thecat_ui_character_scene_affinity_badges_batch113_alpha_sheet_v001.png`; generated without `--despill` to preserve gold, cream, and glow detail.
- Chroma key: `#ea06d5`.
- Transparent pixels: `948183/1572516`.
- Partially transparent pixels: `196724/1572516`.
- Cut method: 3x3 projection split with 24px padding.
- Row bands: `(56, 415)`, `(458, 799)`, `(842, 1187)`.
- Column bands: `(76, 389)`, `(463, 784)`, `(842, 1171)`.

Current artifacts:

- [x] Asset table written before generation.
- [x] Raw source art copied into `source/`.
- [x] Background removed locally and alpha candidates stored outside runtime folders.
- [x] Manifest/contact sheet produced: `thecat_ui_char_scene_aff_batch113_semantic_manifest.csv`, `thecat_ui_char_scene_aff_batch113_semantic_contact_sheet_v001.png`.
- [x] Target-size readability board produced: `thecat_ui_char_scene_aff_batch113_96px_64px_readability_board_v001.png`.
- [x] Visual/source-boundary review completed: `PASS_WITH_P2`.
- [x] Target-size readability review completed: `PASS_WITH_P2`.
- [x] Production QA review completed after closing review-state and process-note P1 items.
- [x] Runtime import remains blocked until import settings, binding proof, screenshots, and clean console evidence pass.

Decision summary:

- Integrated review verdict: `PASS_WITH_P2`.
- `candidate_keep`: `saiban_bedroom_guard_badge`, `kagemaru_shadow_entry_badge`, `yuheng_reward_shop_badge`, `locked_role_scene_badge`, `team_ready_scene_badge`.
- `candidate_conditional`: `nephthys_dream_route_badge`, `suzune_cat_room_support_badge`, `cotton_safe_room_badge`, `unknown_role_scene_badge`.
- Conditional reasons: `nephthys_dream_route_badge` has thin route detail at 64px; `suzune_cat_room_support_badge` is busy at 64px; `cotton_safe_room_badge` has low contrast on warm beige backgrounds; `unknown_role_scene_badge` must be described as a veil/unknown-scene token and not as costume or character art.

No runtime import was performed. No candidate file was copied into `Assets/`, and no Unity `.meta` files were created in this candidate package.
