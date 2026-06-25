# P0 Asset Batch 48 - Saiban AI Generation Pilot

## Mission

Produce the first Saiban-only Codex built-in image generation pilot candidate
using the Batch 47 strict generation spec and the locked colored three-view
turnaround as the source identity authority.

This is a candidate review batch. Do not import into Unity. Do not replace the
current runtime sprite. Do not treat the pilot as approved even if it looks
promising.

## Required Inputs

- `design/梦境支配者核心玩法/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png`
- `design/development/asset_candidates/starter_cats/saiban/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15/thecat_cat_saiban_batch47_strict_generation_spec_v001.json`
- `design/development/asset_candidates/starter_cats/saiban/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15/thecat_cat_saiban_batch47_generation_prompt_v001.md`
- `design/development/asset_candidates/starter_cats/saiban/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15/thecat_cat_saiban_batch47_strict_generation_spec_card_v001.png`
- `Assets/TheCat/Art/Characters/Sprites/thecat_cat_saiban_combat_sprite_512_v001.png`

## Expected Outputs

- `design/development/tools/build_saiban_ai_generation_pilot.py`
- `design/development/tools/validate_saiban_ai_generation_pilot.ps1`
- `design/development/asset_candidates/starter_cats/batch_48_saiban_ai_generation_pilot_2026-06-15/saiban_batch48_ai_generation_pilot_manifest.csv`
- `design/development/asset_candidates/starter_cats/saiban/batch_48_saiban_ai_generation_pilot_2026-06-15/thecat_cat_saiban_batch48_ai_generation_chromakey_source_v001.png`
- `design/development/asset_candidates/starter_cats/saiban/batch_48_saiban_ai_generation_pilot_2026-06-15/thecat_cat_saiban_batch48_ai_generation_alpha_1024_candidate_v001.png`
- `design/development/asset_candidates/starter_cats/saiban/batch_48_saiban_ai_generation_pilot_2026-06-15/thecat_cat_saiban_batch48_ai_generation_alpha_512_preview_v001.png`
- `design/development/asset_candidates/starter_cats/saiban/batch_48_saiban_ai_generation_pilot_2026-06-15/thecat_cat_saiban_batch48_ai_generation_checkerboard_512_review_v001.png`
- `design/development/asset_candidates/starter_cats/saiban/batch_48_saiban_ai_generation_pilot_2026-06-15/thecat_cat_saiban_batch48_ai_generation_darkfield_512_review_v001.png`
- `design/development/asset_candidates/starter_cats/saiban/batch_48_saiban_ai_generation_pilot_2026-06-15/thecat_cat_saiban_batch48_ai_generation_warmfield_512_review_v001.png`
- `design/development/asset_candidates/starter_cats/saiban/batch_48_saiban_ai_generation_pilot_2026-06-15/thecat_cat_saiban_batch48_ai_generation_alpha_mask_512_review_v001.png`
- `design/development/asset_candidates/starter_cats/saiban/batch_48_saiban_ai_generation_pilot_2026-06-15/thecat_cat_saiban_batch48_ai_generation_pilot_review_sheet.png`
- `design/development/asset_candidates/starter_cats/saiban/batch_48_saiban_ai_generation_pilot_2026-06-15/saiban_batch48_ai_generation_pilot_review.md`
- `design/development/asset_candidates/starter_cats/saiban/batch_48_saiban_ai_generation_pilot_2026-06-15/saiban_batch48_ai_generation_pilot_process_note.md`

## Built-In Image Prompt

Use built-in image_gen, not CLI fallback. Use the displayed colored three-view
turnaround as the strict source identity reference. Generate a single Saiban
combat sprite on a flat `#00ff00` chroma-key background.

Must keep:

- non-human silver-gray tabby cat body
- short legs, compact paws, cat head, muzzle, ears, whiskers, and striped tail
- round shield
- short sword
- red cape
- silver armor and helmet
- gold trim and blue gem accent

Reject:

- human or humanoid body
- human hands/fingers
- long human legs
- missing shield, sword, red cape, helmet, striped tail, or blue gem
- generic armored mascot drift
- palette drift from Batch 47
- background shadows, gradients, texture, text, watermark, or non-flat color

## Forbidden Changes

- Do not import into Unity.
- Do not copy candidate files into `Assets`.
- Do not create Unity `.meta` files.
- Do not modify runtime catalogs.
- Do not alter the locked source turnaround.
- Do not approve the candidate without active-cat screenshot review.

## Acceptance Standards

- The generated source is copied into the workspace, while the original
  built-in output remains in the Codex generated image folder.
- The alpha candidate has transparent corners and no obvious chroma-key field.
- The review sheet compares the locked source, Batch 47 spec, current Unity
  sprite, generated chroma source, alpha candidate, checkerboard, dark-field,
  warm-field, and alpha mask.
- The review note explicitly names both pass and watch items.
- The manifest has 7 rows and keeps recommendation
  `candidate_review_only_do_not_import`.
- Formal Unity import remains blocked.

## Validation

Run:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_saiban_ai_generation_pilot.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_starter_cat_strict_generation_spec_pack.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_starter_cat_source_lock_audit_pack.ps1
```

When Unity MCP/editor access is available, capture
`05-active-cat-saiban.png` and compare the pilot against the colored turnaround,
Batch 47 spec card, current Unity sprite, and Batch 48 review sheet before any
formal import discussion.
