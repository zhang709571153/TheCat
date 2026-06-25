# P0 Codex-to-Unity Asset Pipeline

Date: 2026-06-14

## Decision

P0 asset production can happen primarily in Codex, then be installed into Unity after review. Unity is the verification and integration environment, not the only place where bitmap assets may be produced.

## Production Lanes

1. Source-locked derivative lane
   - Use existing design sources under `design/梦境支配者核心玩法/assets`.
   - Produce deterministic crops, swatches, contact sheets, review sheets, and reference candidates.
   - Keep outputs under `design/development/asset_candidates`.
   - Use this lane first for Saiban, Nephthys, Suzune, and any identity-sensitive character asset.

2. Codex image-generation lane
   - Use Codex image generation for missing concept, sprite, UI, and VFX candidates.
   - For starter cats, generate one cat at a time only after the source-locked reference pack exists.
   - Treat generated images as candidates, not approved runtime assets.
   - Reject any candidate that drifts from the colored three-view turnaround.

3. Unity install lane
   - Copy only approved assets into `Assets/TheCat/...`.
   - Add Unity `.meta` import settings only after candidate review passes.
   - Refresh AssetDatabase, check Console, verify Sprite settings, and capture Play Mode screenshots.
   - Runtime binding, prefab, and scene connection checks happen here.

## Starter Cat Hard Rules

- Saiban, Nephthys, and Suzune must remain locked to their colored three-view turnarounds.
- Candidate files stay under `design/development/asset_candidates/starter_cats/<cat_id>/<batch_slug>/`.
- No starter-cat candidate may be imported into `Assets` until active-cat Play Mode screenshots are captured and reviewed.
- Formal import remains blocked until the active screenshot matches front, side, back, palette, markings, costume, prop, and civilization-symbol anchors.

## Batch 29 Status

- First source-locked derivative production batch: Saiban only.
- Generator:
  `design/development/tools/build_saiban_strict_turnaround_derivatives.py`
- Validator:
  `design/development/tools/validate_saiban_strict_turnaround_derivatives.ps1`
- Manifest:
  `design/development/asset_candidates/starter_cats/batch_29_strict_turnaround_derivatives_2026-06-14/saiban_batch29_strict_turnaround_manifest.csv`
- Review sheet:
  `design/development/asset_candidates/starter_cats/saiban/batch_29_strict_turnaround_derivatives_2026-06-14/thecat_cat_saiban_batch29_strict_turnaround_review_sheet.png`
- Review note:
  `design/development/asset_candidates/starter_cats/saiban/batch_29_strict_turnaround_derivatives_2026-06-14/saiban_batch29_strict_turnaround_candidate_review.md`

## Next Installation Gate

Do not install Batch 29 into Unity yet. The next gate is to capture `05-active-cat-saiban.png`, compare the current runtime presentation against the colored three-view turnaround and Batch 29 review sheet, then decide whether to produce an AI refinement candidate or install a reviewed source-derived placeholder.

## Batch 30 Status

- First Codex built-in image-generation refinement candidate: Saiban only.
- Builder:
  `design/development/tools/build_saiban_ai_refinement_candidate.py`
- Validator:
  `design/development/tools/validate_saiban_ai_refinement_candidate.ps1`
- Manifest:
  `design/development/asset_candidates/starter_cats/batch_30_ai_refinement_candidate_2026-06-14/saiban_batch30_ai_refinement_manifest.csv`
- Candidate:
  `design/development/asset_candidates/starter_cats/saiban/batch_30_ai_refinement_candidate_2026-06-14/thecat_cat_saiban_ai_refinement_combat_1024_candidate_v001.png`
- Review sheet:
  `design/development/asset_candidates/starter_cats/saiban/batch_30_ai_refinement_candidate_2026-06-14/thecat_cat_saiban_batch30_ai_refinement_review_sheet.png`

Batch 30 remains candidate-only. It is a front-view AI refinement candidate that preserves Saiban identity well enough for review, but it does not satisfy side/back validation by itself. Side and back identity remain locked to the colored turnaround and Batch 29 references until Unity active-cat screenshot review is available.

## Batch 31 Status

- First deterministic transparent/cutout preparation batch: Saiban only.
- Builder:
  `design/development/tools/build_saiban_cutout_candidate.py`
- Validator:
  `design/development/tools/validate_saiban_cutout_candidate.ps1`
- Manifest:
  `design/development/asset_candidates/starter_cats/batch_31_cutout_candidate_2026-06-14/saiban_batch31_cutout_manifest.csv`
- Cutout candidate:
  `design/development/asset_candidates/starter_cats/saiban/batch_31_cutout_candidate_2026-06-14/thecat_cat_saiban_cutout_alpha_1024_candidate_v001.png`
- Review sheet:
  `design/development/asset_candidates/starter_cats/saiban/batch_31_cutout_candidate_2026-06-14/thecat_cat_saiban_batch31_cutout_review_sheet.png`

Batch 31 remains candidate-only. It prepares Batch 30 for Unity sprite review by adding alpha evidence, checkerboard review, dark-field review, and mask review. It does not approve import; formal import remains blocked until active-cat Play Mode screenshot review passes.

## Batch 32 Status

- First source-locked derivative production batch for Nephthys.
- Builder:
  `design/development/tools/build_nephthys_strict_turnaround_derivatives.py`
- Validator:
  `design/development/tools/validate_nephthys_strict_turnaround_derivatives.ps1`
- Manifest:
  `design/development/asset_candidates/starter_cats/batch_32_nephthys_strict_turnaround_derivatives_2026-06-14/nephthys_batch32_strict_turnaround_manifest.csv`
- Review sheet:
  `design/development/asset_candidates/starter_cats/nephthys/batch_32_nephthys_strict_turnaround_derivatives_2026-06-14/thecat_cat_nephthys_batch32_strict_turnaround_review_sheet.png`
- Review note:
  `design/development/asset_candidates/starter_cats/nephthys/batch_32_nephthys_strict_turnaround_derivatives_2026-06-14/nephthys_batch32_strict_turnaround_candidate_review.md`

Batch 32 remains candidate-only. It extracts front, side, back, combat, HUD, motif, and palette references directly from the locked Nephthys colored three-view turnaround. It does not approve import; formal import remains blocked until active-cat Play Mode screenshot review passes.

## Batch 33 Status

- First source-locked derivative production batch for Suzune.
- Builder:
  `design/development/tools/build_suzune_strict_turnaround_derivatives.py`
- Validator:
  `design/development/tools/validate_suzune_strict_turnaround_derivatives.ps1`
- Manifest:
  `design/development/asset_candidates/starter_cats/batch_33_suzune_strict_turnaround_derivatives_2026-06-14/suzune_batch33_strict_turnaround_manifest.csv`
- Review sheet:
  `design/development/asset_candidates/starter_cats/suzune/batch_33_suzune_strict_turnaround_derivatives_2026-06-14/thecat_cat_suzune_batch33_strict_turnaround_review_sheet.png`
- Review note:
  `design/development/asset_candidates/starter_cats/suzune/batch_33_suzune_strict_turnaround_derivatives_2026-06-14/suzune_batch33_strict_turnaround_candidate_review.md`

Batch 33 remains candidate-only. It extracts front, side, back, combat, HUD, bell-wand motif, and palette references directly from the locked Suzune colored three-view turnaround. It does not approve import; formal import remains blocked until active-cat Play Mode screenshot review passes.

## Batch 34 Status

- First Codex built-in image-generation refinement candidate for Suzune.
- Builder:
  `design/development/tools/build_suzune_ai_refinement_candidate.py`
- Validator:
  `design/development/tools/validate_suzune_ai_refinement_candidate.ps1`
- Manifest:
  `design/development/asset_candidates/starter_cats/batch_34_suzune_ai_refinement_candidate_2026-06-14/suzune_batch34_ai_refinement_manifest.csv`
- Candidate:
  `design/development/asset_candidates/starter_cats/suzune/batch_34_suzune_ai_refinement_candidate_2026-06-14/thecat_cat_suzune_ai_refinement_combat_1024_candidate_v001.png`
- Review sheet:
  `design/development/asset_candidates/starter_cats/suzune/batch_34_suzune_ai_refinement_candidate_2026-06-14/thecat_cat_suzune_batch34_ai_refinement_review_sheet.png`

Batch 34 remains candidate-only. It is a front-view AI refinement candidate that preserves Suzune identity well enough for review, but it does not satisfy side/back validation by itself. Side and back identity remain locked to the colored turnaround and Batch 33 references until Unity active-cat screenshot review is available.

## Batch 35 Status

- First deterministic transparent/cutout preparation batch for Suzune.
- Builder:
  `design/development/tools/build_suzune_cutout_candidate.py`
- Validator:
  `design/development/tools/validate_suzune_cutout_candidate.ps1`
- Manifest:
  `design/development/asset_candidates/starter_cats/batch_35_suzune_cutout_candidate_2026-06-15/suzune_batch35_cutout_manifest.csv`
- Cutout candidate:
  `design/development/asset_candidates/starter_cats/suzune/batch_35_suzune_cutout_candidate_2026-06-15/thecat_cat_suzune_cutout_alpha_1024_candidate_v001.png`
- Review sheet:
  `design/development/asset_candidates/starter_cats/suzune/batch_35_suzune_cutout_candidate_2026-06-15/thecat_cat_suzune_batch35_cutout_review_sheet.png`

Batch 35 remains candidate-only. It prepares Batch 34 for Unity sprite review by adding alpha evidence, checkerboard review, dark-field review, and mask review. It does not approve import; formal import remains blocked until active-cat Play Mode screenshot review passes.

## Batch 36 Status

- First Codex built-in image-generation refinement candidate for Nephthys.
- Builder:
  `design/development/tools/build_nephthys_ai_refinement_candidate.py`
- Validator:
  `design/development/tools/validate_nephthys_ai_refinement_candidate.ps1`
- Manifest:
  `design/development/asset_candidates/starter_cats/batch_36_nephthys_ai_refinement_candidate_2026-06-15/nephthys_batch36_ai_refinement_manifest.csv`
- Candidate:
  `design/development/asset_candidates/starter_cats/nephthys/batch_36_nephthys_ai_refinement_candidate_2026-06-15/thecat_cat_nephthys_ai_refinement_combat_1024_candidate_v001.png`
- Review sheet:
  `design/development/asset_candidates/starter_cats/nephthys/batch_36_nephthys_ai_refinement_candidate_2026-06-15/thecat_cat_nephthys_batch36_ai_refinement_review_sheet.png`

Batch 36 remains candidate-only. It is a front-view AI refinement candidate that preserves Nephthys identity well enough for review, but it does not satisfy side/back validation by itself. Side and back identity remain locked to the colored turnaround and Batch 32 references until Unity active-cat screenshot review is available.

## Batch 37 Status

- First deterministic transparent/cutout preparation batch for Nephthys.
- Builder:
  `design/development/tools/build_nephthys_cutout_candidate.py`
- Validator:
  `design/development/tools/validate_nephthys_cutout_candidate.ps1`
- Manifest:
  `design/development/asset_candidates/starter_cats/batch_37_nephthys_cutout_candidate_2026-06-15/nephthys_batch37_cutout_manifest.csv`
- Cutout candidate:
  `design/development/asset_candidates/starter_cats/nephthys/batch_37_nephthys_cutout_candidate_2026-06-15/thecat_cat_nephthys_cutout_alpha_1024_candidate_v001.png`
- Review sheet:
  `design/development/asset_candidates/starter_cats/nephthys/batch_37_nephthys_cutout_candidate_2026-06-15/thecat_cat_nephthys_batch37_cutout_review_sheet.png`

Batch 37 remains candidate-only. It prepares Batch 36 for Unity sprite review by adding alpha evidence, checkerboard review, dark-field review, and mask review. It does not approve import; formal import remains blocked until active-cat Play Mode screenshot review passes.

## Batch 38 Status

- First deterministic source-reference preparation batch for P0 core enemies.
- Builder:
  `design/development/tools/build_p0_enemy_source_reference_pack.py`
- Validator:
  `design/development/tools/validate_p0_enemy_source_reference_pack.ps1`
- Manifest:
  `design/development/asset_candidates/enemies/batch_38_p0_enemy_source_reference_pack_2026-06-15/p0_enemy_batch38_source_reference_manifest.csv`
- Review sheet:
  `design/development/asset_candidates/enemies/p0_core/batch_38_p0_enemy_source_reference_pack_2026-06-15/thecat_enemy_p0_core_batch38_source_reference_review_sheet.png`
- Review note:
  `design/development/asset_candidates/enemies/p0_core/batch_38_p0_enemy_source_reference_pack_2026-06-15/p0_enemy_batch38_source_reference_candidate_review.md`

Batch 38 remains candidate-only. It extracts concept, animation, combat-crop,
warning-motif, and palette references from the locked Black Mud Nightmare, Cold
Light Shadow, and Call Tyrant design assets. It does not approve import; formal
enemy import remains blocked until active-enemy Play Mode screenshot review,
Console checks, AssetDatabase refresh, Sprite import settings, and runtime
binding checks pass.

## Batch 39 Status

- First Codex built-in image-generation refinement candidate for Black Mud
  Nightmare.
- Builder:
  `design/development/tools/build_black_mud_ai_refinement_candidate.py`
- Validator:
  `design/development/tools/validate_black_mud_ai_refinement_candidate.ps1`
- Manifest:
  `design/development/asset_candidates/enemies/batch_39_black_mud_ai_refinement_candidate_2026-06-15/black_mud_batch39_ai_refinement_manifest.csv`
- Candidate:
  `design/development/asset_candidates/enemies/black_mud_nightmare/batch_39_black_mud_ai_refinement_candidate_2026-06-15/thecat_enemy_black_mud_nightmare_ai_refinement_combat_1024_candidate_v001.png`
- Review sheet:
  `design/development/asset_candidates/enemies/black_mud_nightmare/batch_39_black_mud_ai_refinement_candidate_2026-06-15/thecat_enemy_black_mud_batch39_ai_refinement_review_sheet.png`

Batch 39 remains candidate-only. It is a single-view AI refinement candidate
that preserves the Black Mud Nightmare source concept identity well enough for
review, but it does not satisfy animation, alpha, hitbox, or runtime-scale
validation by itself. Source concept and animation identity remain locked to
`black_mud_concept`, `black_mud_animation`, and the Batch 38 reference pack
until Unity active-enemy screenshot review is available.

## Batch 40 Status

- First deterministic transparent/cutout preparation batch for Black Mud
  Nightmare.
- Builder:
  `design/development/tools/build_black_mud_cutout_candidate.py`
- Validator:
  `design/development/tools/validate_black_mud_cutout_candidate.ps1`
- Manifest:
  `design/development/asset_candidates/enemies/batch_40_black_mud_cutout_candidate_2026-06-15/black_mud_batch40_cutout_manifest.csv`
- Cutout candidate:
  `design/development/asset_candidates/enemies/black_mud_nightmare/batch_40_black_mud_cutout_candidate_2026-06-15/thecat_enemy_black_mud_nightmare_cutout_alpha_1024_candidate_v001.png`
- Review sheet:
  `design/development/asset_candidates/enemies/black_mud_nightmare/batch_40_black_mud_cutout_candidate_2026-06-15/thecat_enemy_black_mud_batch40_cutout_review_sheet.png`

Batch 40 remains candidate-only. It prepares Batch 39 for Unity sprite review
by adding alpha evidence, checkerboard review, dark-field review, and mask
review. It does not approve import; formal import remains blocked until
active-enemy Play Mode screenshot review, Console checks, AssetDatabase refresh,
Sprite import settings, runtime scale, hitbox readability, and prefab/scene
binding checks pass.

## Batch 41 Status

- First Codex built-in image-generation refinement candidate for Cold Light
  Shadow.
- Builder:
  `design/development/tools/build_cold_light_ai_refinement_candidate.py`
- Validator:
  `design/development/tools/validate_cold_light_ai_refinement_candidate.ps1`
- Manifest:
  `design/development/asset_candidates/enemies/batch_41_cold_light_ai_refinement_candidate_2026-06-15/cold_light_batch41_ai_refinement_manifest.csv`
- Candidate:
  `design/development/asset_candidates/enemies/cold_light_shadow/batch_41_cold_light_ai_refinement_candidate_2026-06-15/thecat_enemy_cold_light_shadow_ai_refinement_combat_1024_candidate_v001.png`
- Review sheet:
  `design/development/asset_candidates/enemies/cold_light_shadow/batch_41_cold_light_ai_refinement_candidate_2026-06-15/thecat_enemy_cold_light_batch41_ai_refinement_review_sheet.png`

Batch 41 remains candidate-only. It is a single-view AI refinement candidate
that preserves the Cold Light Shadow source concept identity well enough for
review, but it does not satisfy animation, alpha, beam-warning, hitbox, or
runtime-scale validation by itself. Source concept and animation identity remain
locked to `cold_light_concept`, `cold_light_animation`, and the Batch 38
reference pack until Unity active-enemy screenshot review is available.

## Batch 42 Status

- First deterministic transparent/cutout preparation batch for Cold Light
  Shadow.
- Builder:
  `design/development/tools/build_cold_light_cutout_candidate.py`
- Validator:
  `design/development/tools/validate_cold_light_cutout_candidate.ps1`
- Manifest:
  `design/development/asset_candidates/enemies/batch_42_cold_light_cutout_candidate_2026-06-15/cold_light_batch42_cutout_manifest.csv`
- Cutout candidate:
  `design/development/asset_candidates/enemies/cold_light_shadow/batch_42_cold_light_cutout_candidate_2026-06-15/thecat_enemy_cold_light_shadow_cutout_beam_alpha_1024_candidate_v001.png`
- Review sheet:
  `design/development/asset_candidates/enemies/cold_light_shadow/batch_42_cold_light_cutout_candidate_2026-06-15/thecat_enemy_cold_light_batch42_cutout_review_sheet.png`

Batch 42 remains candidate-only. It prepares Batch 41 for Unity sprite review
by adding beam-preserving alpha evidence, checkerboard review, dark-field
review, warm-HUD review, and mask review. It does not approve import; formal
import remains blocked until active-enemy Play Mode screenshot review, Console
checks, AssetDatabase refresh, Sprite import settings, runtime scale, hitbox
readability, beam-warning readability, and prefab/scene binding checks pass.

## Batch 43 Status

- First Codex built-in image-generation refinement candidate for Call Tyrant
  Boss.
- Builder:
  `design/development/tools/build_call_tyrant_ai_refinement_candidate.py`
- Validator:
  `design/development/tools/validate_call_tyrant_ai_refinement_candidate.ps1`
- Manifest:
  `design/development/asset_candidates/enemies/batch_43_call_tyrant_ai_refinement_candidate_2026-06-15/call_tyrant_batch43_ai_refinement_manifest.csv`
- Candidate:
  `design/development/asset_candidates/enemies/call_tyrant/batch_43_call_tyrant_ai_refinement_candidate_2026-06-15/thecat_enemy_call_tyrant_ai_refinement_combat_1024_candidate_v001.png`
- Review sheet:
  `design/development/asset_candidates/enemies/call_tyrant/batch_43_call_tyrant_ai_refinement_candidate_2026-06-15/thecat_enemy_call_tyrant_batch43_ai_refinement_review_sheet.png`

Batch 43 remains candidate-only. It is a single-view AI refinement candidate
that preserves the Call Tyrant source concept identity well enough for review,
but it does not satisfy animation, alpha, summon-warning, app-throw, hitbox, or
runtime-scale validation by itself. Source concept and animation identity remain
locked to `call_tyrant_concept`, `call_tyrant_animation`, and the Batch 38
reference pack until Unity active-enemy screenshot review is available.

## Batch 44 Status

- First deterministic transparent/cutout preparation batch for Call Tyrant
  Boss.
- Builder:
  `design/development/tools/build_call_tyrant_cutout_candidate.py`
- Validator:
  `design/development/tools/validate_call_tyrant_cutout_candidate.ps1`
- Manifest:
  `design/development/asset_candidates/enemies/batch_44_call_tyrant_cutout_candidate_2026-06-15/call_tyrant_batch44_cutout_manifest.csv`
- Cutout candidate:
  `design/development/asset_candidates/enemies/call_tyrant/batch_44_call_tyrant_cutout_candidate_2026-06-15/thecat_enemy_call_tyrant_cutout_boss_alpha_1024_candidate_v001.png`
- Review sheet:
  `design/development/asset_candidates/enemies/call_tyrant/batch_44_call_tyrant_cutout_candidate_2026-06-15/thecat_enemy_call_tyrant_batch44_cutout_review_sheet.png`

Batch 44 remains candidate-only. It prepares Batch 43 for Unity sprite review
by adding alpha evidence, checkerboard review, dark-field review, warm-HUD
review, and mask review. It does not approve import; formal import remains
blocked until active-enemy Play Mode screenshot review, Console checks,
AssetDatabase refresh, Sprite import settings, runtime scale, hitbox
readability, summon readability, app-throw readability, and prefab/scene binding
checks pass.

## Batch 45 Status

- Deterministic starter-cat source-lock audit pack for Saiban, Nephthys, and
  Suzune.
- Builder:
  `design/development/tools/build_starter_cat_source_lock_audit_pack.py`
- Validator:
  `design/development/tools/validate_starter_cat_source_lock_audit_pack.ps1`
- Manifest:
  `design/development/asset_candidates/starter_cats/batch_45_starter_cat_source_lock_audit_pack_2026-06-15/starter_cat_batch45_source_lock_audit_manifest.csv`
- Review sheet:
  `design/development/asset_candidates/starter_cats/batch_45_starter_cat_source_lock_audit_pack_2026-06-15/thecat_starter_cat_batch45_source_lock_audit_review_sheet.png`

Batch 45 remains candidate-only. It generates no model art and installs no
runtime asset. It provides the strict comparison baseline for future starter-cat
generation or replacement: locked colored three-view turnaround, current Unity
sprite, latest transparent cutout candidate, required identity anchors, reject
rules, source-lock id, and active-cat screenshot gate. Formal import remains
blocked until active-cat Play Mode screenshot comparison, Console checks,
AssetDatabase refresh, Sprite import settings, runtime scale, HUD readability,
and prefab/scene binding checks pass.

## Batch 46 Status

- Deterministic P0 asset production dashboard for the first Unity installation
  queue.
- Builder:
  `design/development/tools/build_p0_asset_production_dashboard.py`
- Validator:
  `design/development/tools/validate_p0_asset_production_dashboard.ps1`
- Manifest:
  `design/development/asset_candidates/p0_asset_dashboard/batch_46_p0_asset_production_dashboard_2026-06-15/p0_asset_batch46_production_dashboard_manifest.csv`
- Review sheet:
  `design/development/asset_candidates/p0_asset_dashboard/batch_46_p0_asset_production_dashboard_2026-06-15/thecat_p0_asset_batch46_production_dashboard_review_sheet.png`

Batch 46 remains candidate-only. It consolidates the current P0 art production
queue into 6 dashboard rows: Saiban, Nephthys, Suzune, Black Mud Nightmare,
Cold Light Shadow, and Call Tyrant. Each row names source locks, current Unity
runtime art, latest candidate preview, proposed install target, active
screenshot, Unity validation gate, next action, blockers, and the
`dashboard_only_unity_validation_pending` recommendation.

Pipeline decision: Codex-side asset production is valid for model generation,
cleanup, cutout preparation, contact sheets, manifests, process notes, and
review packets. Unity remains the install-time and runtime validation gate for
AssetDatabase refresh, Sprite import settings, prefab/scene wiring, Console
checks, Play Mode screenshots, and feel/readability review.

Formal import remains blocked until the six dashboard screenshots are captured
and reviewed:

- `05-active-cat-saiban.png`
- `06-active-cat-nephthys.png`
- `07-active-cat-suzune.png`
- `07-active-enemy-black-mud.png`
- `08-active-enemy-cold-light.png`
- `09-active-enemy-call-tyrant.png`

## Batch 47 Status

- Deterministic strict generation spec pack for Saiban, Nephthys, and Suzune.
- Builder:
  `design/development/tools/build_starter_cat_strict_generation_spec_pack.py`
- Validator:
  `design/development/tools/validate_starter_cat_strict_generation_spec_pack.ps1`
- Manifest:
  `design/development/asset_candidates/starter_cats/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15/starter_cat_batch47_strict_generation_spec_manifest.csv`
- Review sheet:
  `design/development/asset_candidates/starter_cats/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15/thecat_starter_cat_batch47_strict_generation_spec_review_sheet.png`

Batch 47 remains spec-only. It produces no model art and installs no runtime
asset. It converts the starter-cat colored three-view turnaround authority into
per-cat visual spec cards, JSON generation specs, prompt files, sampled palette
guards, must-keep anchors, reject rules, and active-cat screenshot gates.

Future starter-cat image generation must use Batch 47 plus the source
turnaround as input. A generated cat candidate should be rejected if it matches
the broad project style but violates the Batch 47 body rule, palette guard,
required props, source-lock identity, or active-cat screenshot gate.

Formal import remains blocked until generated candidates return through cutout
preparation, manifest updates, review sheets, Console checks, AssetDatabase
refresh, Sprite import settings, runtime scale/HUD readability review, prefab
or scene binding checks, and active-cat Play Mode screenshot comparison.

## Batch 48 Status

- First Saiban-only Codex built-in image-generation pilot candidate.
- Builder:
  `design/development/tools/build_saiban_ai_generation_pilot.py`
- Validator:
  `design/development/tools/validate_saiban_ai_generation_pilot.ps1`
- Manifest:
  `design/development/asset_candidates/starter_cats/batch_48_saiban_ai_generation_pilot_2026-06-15/saiban_batch48_ai_generation_pilot_manifest.csv`
- Review sheet:
  `design/development/asset_candidates/starter_cats/saiban/batch_48_saiban_ai_generation_pilot_2026-06-15/thecat_cat_saiban_batch48_ai_generation_pilot_review_sheet.png`

Batch 48 remains candidate-only. It proves that Codex-side image generation can
enter the project asset pipeline without immediately touching Unity runtime
assets: built-in generation, workspace copy, chroma-key alpha removal, review
sheet, manifest, process note, and validator.

Manual review summary:

- Pass: non-human cat body, shield, sword, red cape, silver armor, gold trim,
  blue gem, tabby face, and striped tail are present.
- Watch: helmet and armor are more ornate than the locked source.
- Watch: one front combat pose does not prove side/back identity anchors.
- Blocked: no Unity import before active-cat screenshot comparison, Console
  clean, AssetDatabase refresh, Sprite import settings, runtime scale,
  HUD readability, and prefab/scene binding.

Formal import remains blocked. Before expanding the same approach to Nephthys
and Suzune, either accept this ornamentation tradeoff as a viable game-read
direction or tighten the generation prompt to reduce helmet/armor drift from
the colored turnaround.

## Batch 49 Status

- Second Saiban-only Codex built-in image-generation refinement candidate.
- Builder:
  `design/development/tools/build_saiban_low_drift_refinement.py`
- Validator:
  `design/development/tools/validate_saiban_low_drift_refinement.ps1`
- Manifest:
  `design/development/asset_candidates/starter_cats/batch_49_saiban_low_drift_refinement_2026-06-15/saiban_batch49_low_drift_refinement_manifest.csv`
- Candidate:
  `design/development/asset_candidates/starter_cats/saiban/batch_49_saiban_low_drift_refinement_2026-06-15/thecat_cat_saiban_batch49_low_drift_alpha_1024_candidate_v001.png`
- Review sheet:
  `design/development/asset_candidates/starter_cats/saiban/batch_49_saiban_low_drift_refinement_2026-06-15/thecat_cat_saiban_batch49_low_drift_refinement_review_sheet.png`

Batch 49 remains candidate-only. It tightens the Batch 48 direction toward the
locked colored three-view turnaround: lower-profile helmet, exposed ears,
reduced armor ornamentation, and preserved shield, sword, red cape, blue gem,
tabby face, and striped tail.

Manual review summary:

- Pass: lower helmet and exposed ears improve source-lock consistency versus
  Batch 48.
- Pass: armor ornamentation is reduced while preserving Saiban's sword-saint
  readability.
- Watch: armor is still slightly more polished than the source turnaround.
- Watch: one front combat pose does not prove side/back identity anchors.
- Blocked: no Unity import before active-cat screenshot comparison, Console
  clean, AssetDatabase refresh, Sprite import settings, runtime scale,
  HUD readability, and prefab/scene binding.

Codex-side production remains the correct place for image generation, cleanup,
alpha preparation, review packets, manifests, and hash evidence. Unity remains
the install-time validator for runtime asset adoption. If the remaining polish
drift is accepted in active-cat review, Batch 49 should supersede Batch 48 as
the preferred Saiban install candidate.

## Batch 50 Status

- Nephthys-only Codex built-in image-generation candidate created after the
  Batch 47 strict starter-cat generation spec.
- Builder:
  `design/development/tools/build_nephthys_strict_ai_generation_candidate.py`
- Validator:
  `design/development/tools/validate_nephthys_strict_ai_generation_candidate.ps1`
- Manifest:
  `design/development/asset_candidates/starter_cats/batch_50_nephthys_strict_ai_generation_candidate_2026-06-15/nephthys_batch50_strict_ai_generation_manifest.csv`
- Candidate:
  `design/development/asset_candidates/starter_cats/nephthys/batch_50_nephthys_strict_ai_generation_candidate_2026-06-15/thecat_cat_nephthys_batch50_strict_ai_alpha_1024_candidate_v001.png`
- Review sheet:
  `design/development/asset_candidates/starter_cats/nephthys/batch_50_nephthys_strict_ai_generation_candidate_2026-06-15/thecat_cat_nephthys_batch50_strict_ai_generation_review_sheet.png`

Batch 50 remains candidate-only. It preserves the Nephthys colored-turnaround
anchors: non-human hooded cat body, visible ears, crescent ornament, blue tear
gem, ankh, winged collar, striped tail, navy cloak, gold script trim, and
floating pyramid prop.

Manual review summary:

- Pass: source-lock identity is recognizable and avoids human
  Cleopatra/priestess drift.
- Pass: palette stays close to Batch 47 navy, sand-gold, brown tabby, pale
  cloth, and blue gem guard.
- Watch: Batch 50 is more close-up and hero-polished than Batch 37.
- Watch: one front combat pose does not prove side/back identity anchors.
- Blocked: no Unity import before active-cat screenshot comparison, Console
  clean, AssetDatabase refresh, Sprite import settings, runtime scale,
  HUD readability, and prefab/scene binding.

Batch 50 does not supersede Batch 37 yet. The next Nephthys install decision
must compare Batch 37 and Batch 50 in `06-active-cat-nephthys.png` before
choosing the preferred runtime candidate.

## Batch 51 Status

- Suzune-only Codex built-in image-generation candidate created after the Batch
  47 strict starter-cat generation spec.
- Builder:
  `design/development/tools/build_suzune_strict_ai_generation_candidate.py`
- Validator:
  `design/development/tools/validate_suzune_strict_ai_generation_candidate.ps1`
- Manifest:
  `design/development/asset_candidates/starter_cats/batch_51_suzune_strict_ai_generation_candidate_2026-06-15/suzune_batch51_strict_ai_generation_manifest.csv`
- Candidate:
  `design/development/asset_candidates/starter_cats/suzune/batch_51_suzune_strict_ai_generation_candidate_2026-06-15/thecat_cat_suzune_batch51_strict_ai_alpha_1024_candidate_v001.png`
- Review sheet:
  `design/development/asset_candidates/starter_cats/suzune/batch_51_suzune_strict_ai_generation_candidate_2026-06-15/thecat_cat_suzune_batch51_strict_ai_generation_review_sheet.png`

Batch 51 remains candidate-only. It preserves the Suzune colored-turnaround
anchors: non-human calico cat body, blue eyes, triangular ears, warm white and
vermilion shrine outfit, red cords, bell ornaments, moon-blue talismans,
tear-drop charms, bell wand, and calico tail.

Manual review summary:

- Pass: source-lock identity is recognizable and avoids human shrine maiden
  drift.
- Pass: palette stays close to Batch 47 warm white, vermilion, moon-blue, gold
  bell, and calico fur guard.
- Watch: Batch 51 is more close-up and hero-polished than Batch 35.
- Watch: wand strings, bells, talismans, and droplets create more alpha edge
  complexity than the older cutout.
- Watch: one front combat pose does not prove side/back identity anchors.
- Blocked: no Unity import before active-cat screenshot comparison, Console
  clean, AssetDatabase refresh, Sprite import settings, runtime scale,
  HUD readability, and prefab/scene binding.

Batch 51 does not supersede Batch 35 yet. The next Suzune install decision must
compare Batch 35 and Batch 51 in `07-active-cat-suzune.png` before choosing the
preferred runtime candidate.
