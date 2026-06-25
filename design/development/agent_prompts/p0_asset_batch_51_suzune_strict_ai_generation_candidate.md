# P0 Asset Batch 51 - Suzune Strict AI Generation Candidate

## Task Scope

Generate and package one Codex-side Suzune candidate for review only. The
candidate must use the colored three-view turnaround and Batch 47 strict
generation spec as the source lock. Do not import into Unity.

## Required Inputs

- Source turnaround:
  `design/梦境支配者核心玩法/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png`
- Batch 47 JSON spec:
  `design/development/asset_candidates/starter_cats/suzune/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15/thecat_cat_suzune_batch47_strict_generation_spec_v001.json`
- Batch 47 spec card:
  `design/development/asset_candidates/starter_cats/suzune/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15/thecat_cat_suzune_batch47_strict_generation_spec_card_v001.png`
- Prior cutout baseline:
  `design/development/asset_candidates/starter_cats/suzune/batch_35_suzune_cutout_candidate_2026-06-15/thecat_cat_suzune_cutout_alpha_1024_candidate_v001.png`
- Current Unity sprite:
  `Assets/TheCat/Art/Characters/Sprites/thecat_cat_suzune_combat_sprite_512_v001.png`

## Built-In Image Prompt

Use built-in image_gen, not CLI fallback. The colored three-view turnaround must be the
primary reference. Produce a single clean combat sprite on a perfectly flat
solid #00ff00 chroma-key background.

Positive prompt:

faithful P0 battle sprite for Suzune / Sleep Shrine Healer, a non-human calico
cat with bell ornaments, shrine outfit, and a small healer wand or branch;
compact cat body, short paws, round feline face, triangular ears, whiskers,
blue eyes, and calico orange-black-white tail; warm white and vermilion shrine
healer outfit with red cords, gold bell ornaments, moon-blue snowflake/talisman
accents, clustered golden bells, and blue tear-drop charms; hand-painted dream
defense roguelite game sprite, soft ink outline, compact readable silhouette,
source-locked to the colored three-view turnaround.

Negative prompt:

human, humanoid, shrine maiden girl, human priestess, long human arms, long
human legs, hands, fingers, generic white healer kitten, ordinary costume on a
person, dog, mascot redesign, missing calico markings, missing bells, missing
wand or branch healer prop, palette drift, extra characters, text, logo,
watermark.

Chroma-key constraints:

The background must be one uniform #00ff00 color with no shadows, gradients,
texture, floor plane, contact shadow, reflection, watermark, or text. Do not
use #00ff00 anywhere in the subject.

## Expected Outputs

- Workspace chroma-key source under:
  `design/development/asset_candidates/starter_cats/suzune/batch_51_suzune_strict_ai_generation_candidate_2026-06-15`
- Transparent alpha candidate made with the imagegen chroma-key helper.
- Review composites for checkerboard, dark field, warm field, and alpha mask.
- Manifest:
  `design/development/asset_candidates/starter_cats/batch_51_suzune_strict_ai_generation_candidate_2026-06-15/suzune_batch51_strict_ai_generation_manifest.csv`
- Review sheet:
  `design/development/asset_candidates/starter_cats/suzune/batch_51_suzune_strict_ai_generation_candidate_2026-06-15/thecat_cat_suzune_batch51_strict_ai_generation_review_sheet.png`
- Review note and process note in the same candidate folder.

## Forbidden Changes

- Do not modify `Assets`.
- Do not create Unity `.meta` files.
- Do not replace the current Unity sprite.
- Do not update runtime manifest counts.
- Do not promote Batch 51 over Batch 35 without active-cat screenshot review.

## Acceptance Criteria

- Candidate keeps the non-human cat body.
- Candidate keeps calico orange, black, and white markings from the turnaround.
- Candidate keeps the warm white and vermilion shrine outfit.
- Candidate keeps moon-blue accents, bell ornaments, and wand or branch healer
  prop.
- Candidate avoids human shrine maiden body drift.
- Candidate is copied from the built-in image_gen output into the workspace.
- Chroma-key alpha candidate is `1024x1024` and has transparent corners.
- Review sheet compares source turnaround, Batch 47 spec card, current Unity
  sprite, Batch 35 baseline, Batch 51 source, Batch 51 alpha, and alpha review
  composites.
- Manifest has 7 rows and recommendation
  `candidate_review_only_do_not_import`.
- Formal Unity import remains blocked until `07-active-cat-suzune.png`,
  Console, AssetDatabase refresh, Sprite import settings, runtime scale, HUD
  readability, and prefab/scene binding pass.

## Validation

- Run:
  `powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_suzune_strict_ai_generation_candidate.ps1`
- Run linked validators for Batch 45, Batch 46, Batch 47, Batch 48, Batch 49,
  and Batch 50 before considering this asset pipeline synchronized.
- Run Runtime/EditMode MSBuild and `git diff --check`.
