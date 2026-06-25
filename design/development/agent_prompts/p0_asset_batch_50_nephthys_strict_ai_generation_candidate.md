# P0 Asset Batch 50 - Nephthys Strict AI Generation Candidate

## Task Scope

Generate and package one Codex-side Nephthys candidate for review only. The
candidate must use the colored three-view turnaround and Batch 47 strict
generation spec as the source lock. Do not import into Unity.

## Required Inputs

- Source turnaround:
  `design/梦境支配者核心玩法/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png`
- Batch 47 JSON spec:
  `design/development/asset_candidates/starter_cats/nephthys/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15/thecat_cat_nephthys_batch47_strict_generation_spec_v001.json`
- Batch 47 spec card:
  `design/development/asset_candidates/starter_cats/nephthys/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15/thecat_cat_nephthys_batch47_strict_generation_spec_card_v001.png`
- Prior cutout baseline:
  `design/development/asset_candidates/starter_cats/nephthys/batch_37_nephthys_cutout_candidate_2026-06-15/thecat_cat_nephthys_cutout_alpha_1024_candidate_v001.png`
- Current Unity sprite:
  `Assets/TheCat/Art/Characters/Sprites/thecat_cat_nephthys_combat_sprite_512_v001.png`

## Built-In Image Prompt

Use built-in image_gen, not CLI fallback. The colored three-view turnaround must be the
primary reference. Produce a single clean combat sprite on a perfectly flat
solid #00ff00 chroma-key background.

Positive prompt:

faithful P0 battle sprite for Nephthys / Moon-Sand Agent, a non-human
gold-brown tabby cat controlling dreams with a floating pyramid prop; compact
cat body, feline face, visible ears inside a deep navy hood, whiskers, compact
paws, short body, and striped tail; deep navy cloak with sand-gold trim and
dream-script symbols; crescent moon forehead ornament, blue tear gem, golden
eyes, blue gemstone chest ornament, winged gold collar, ankh emblem, and
floating pyramid or obelisk; hand-painted dream defense roguelite game sprite,
soft ink outline, compact readable silhouette, source-locked to the colored
three-view turnaround.

Negative prompt:

human, humanoid, Cleopatra, human priestess, long human arms, long human legs,
hands, fingers, ordinary Egyptian costume on a person, dog, mascot redesign,
missing hood, missing pyramid or obelisk, missing blue gems, palette drift,
extra characters, text, logo, watermark.

Chroma-key constraints:

The background must be one uniform #00ff00 color with no shadows, gradients,
texture, floor plane, contact shadow, reflection, watermark, or text. Do not
use #00ff00 anywhere in the subject.

## Expected Outputs

- Workspace chroma-key source under:
  `design/development/asset_candidates/starter_cats/nephthys/batch_50_nephthys_strict_ai_generation_candidate_2026-06-15`
- Transparent alpha candidate made with the imagegen chroma-key helper.
- Review composites for checkerboard, dark field, warm field, and alpha mask.
- Manifest:
  `design/development/asset_candidates/starter_cats/batch_50_nephthys_strict_ai_generation_candidate_2026-06-15/nephthys_batch50_strict_ai_generation_manifest.csv`
- Review sheet:
  `design/development/asset_candidates/starter_cats/nephthys/batch_50_nephthys_strict_ai_generation_candidate_2026-06-15/thecat_cat_nephthys_batch50_strict_ai_generation_review_sheet.png`
- Review note and process note in the same candidate folder.

## Forbidden Changes

- Do not modify `Assets`.
- Do not create Unity `.meta` files.
- Do not replace the current Unity sprite.
- Do not update runtime manifest counts.
- Do not promote Batch 50 over Batch 37 without active-cat screenshot review.

## Acceptance Criteria

- Candidate keeps the non-human cat body.
- Candidate keeps deep navy hood and cloak.
- Candidate keeps gold-brown tabby fur and mask-like face markings.
- Candidate keeps crescent moon ornament, blue tear gem, blue gems, ankh, winged
  collar, and floating pyramid or obelisk prop.
- Candidate avoids human Cleopatra/priestess body drift.
- Candidate is copied from the built-in image_gen output into the workspace.
- Chroma-key alpha candidate is `1024x1024` and has transparent corners.
- Review sheet compares source turnaround, Batch 47 spec card, current Unity
  sprite, Batch 37 baseline, Batch 50 source, Batch 50 alpha, and alpha review
  composites.
- Manifest has 7 rows and recommendation
  `candidate_review_only_do_not_import`.
- Formal Unity import remains blocked until `06-active-cat-nephthys.png`,
  Console, AssetDatabase refresh, Sprite import settings, runtime scale, HUD
  readability, and prefab/scene binding pass.

## Validation

- Run:
  `powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_nephthys_strict_ai_generation_candidate.ps1`
- Run linked validators for Batch 47, Batch 45, Batch 46, Batch 48, and Batch
  49 before considering this asset pipeline synchronized.
- Run Runtime/EditMode MSBuild and `git diff --check`.
