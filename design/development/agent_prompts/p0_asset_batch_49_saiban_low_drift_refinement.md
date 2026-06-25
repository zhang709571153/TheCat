# P0 Asset Batch 49 - Saiban Low-Drift Refinement

## Mission

Produce and package a second Saiban-only Codex built-in image-generation
candidate that reduces the Batch 48 ornament drift while preserving Saiban's
source-locked identity.

This is a candidate review batch. Do not import into Unity. Do not replace the
current runtime sprite. The output is only useful if it is closer to the locked
colored three-view turnaround than Batch 48.

## Required Inputs

- `design/梦境支配者核心玩法/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png`
- `design/development/asset_candidates/starter_cats/saiban/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15/thecat_cat_saiban_batch47_strict_generation_spec_v001.json`
- `design/development/asset_candidates/starter_cats/saiban/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15/thecat_cat_saiban_batch47_generation_prompt_v001.md`
- `design/development/asset_candidates/starter_cats/saiban/batch_48_saiban_ai_generation_pilot_2026-06-15/thecat_cat_saiban_batch48_ai_generation_alpha_1024_candidate_v001.png`
- `Assets/TheCat/Art/Characters/Sprites/thecat_cat_saiban_combat_sprite_512_v001.png`

## Built-In Image Prompt Intent

Use built-in image_gen, not CLI fallback. Use the colored three-view turnaround
as the strict source identity reference. Use Batch 48 only as a cautionary
example of excessive helmet and armor ornamentation.

Generate a single Saiban combat sprite on a flat `#00ff00` chroma-key
background.

Must keep:

- non-human silver-gray tabby cat body
- short legs, compact paws, cat head, muzzle, ears, whiskers, green eyes, and
  striped tail
- low-profile silver helmet that does not cover most of the ears
- simple silver shoulder plates and armor
- modest warm gold trim
- muted deep red cape with ragged holes
- one round shield with sun/star face
- one short sword
- one small blue gem at the chest

Reject:

- human or humanoid body
- human hands/fingers
- long legs
- oversized helmet
- ornate crown-like helmet
- excessive filigree
- excessive gold decoration
- huge sword
- huge shoulder pads
- missing shield, sword, red cape, striped tail, or blue gem
- generic mascot redesign

## Expected Outputs

- `design/development/tools/build_saiban_low_drift_refinement.py`
- `design/development/tools/validate_saiban_low_drift_refinement.ps1`
- `design/development/asset_candidates/starter_cats/batch_49_saiban_low_drift_refinement_2026-06-15/saiban_batch49_low_drift_refinement_manifest.csv`
- candidate/review PNGs and review notes under
  `design/development/asset_candidates/starter_cats/saiban/batch_49_saiban_low_drift_refinement_2026-06-15`

## Forbidden Changes

- Do not import into Unity.
- Do not copy candidate files into `Assets`.
- Do not create Unity `.meta` files.
- Do not modify runtime catalogs.
- Do not alter the locked source turnaround.
- Do not approve the candidate without active-cat screenshot review.

## Acceptance Standards

- The generated source is copied into the workspace and normalized to
  `1024x1024`.
- The alpha candidate has transparent corners and no obvious chroma-key field.
- The review sheet compares the locked source, Batch 47 spec, current Unity
  sprite, Batch 48 pilot, Batch 49 refinement, checkerboard, dark-field,
  warm-field, and alpha mask.
- The review note states that Batch 49 reduces helmet/armor ornament drift
  compared with Batch 48, while remaining blocked for Unity import.
- The manifest keeps recommendation `candidate_review_only_do_not_import`.

## Validation

Run:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_saiban_low_drift_refinement.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_saiban_ai_generation_pilot.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_starter_cat_strict_generation_spec_pack.ps1
```

When Unity MCP/editor access is available, capture
`05-active-cat-saiban.png` and compare the refinement against the colored
turnaround, Batch 47 spec card, Batch 48 pilot, and Batch 49 review sheet before
any formal import discussion.
