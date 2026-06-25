# Batch 47 Starter Cat Strict Generation Spec Review

Decision: strict generation spec only; do not import into Unity.

This pack exists because starter-cat images must match the locked colored three-view turnaround, not just the broader dream-cat art style.

## Scope

- Covers Saiban, Nephthys, and Suzune only.
- Produces machine-readable JSON generation specs, prompt files, and visual spec cards.
- Samples a source palette guard from each locked colored turnaround.
- Keeps all Batch 47 outputs outside `Assets`.
- Creates no Unity `.meta` files.
- Does not generate replacement art and does not approve any sprite import.
- Formal import remains blocked until active-cat Play Mode screenshot comparison passes.

## Outputs

- Manifest: `design/development/asset_candidates/starter_cats/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15/starter_cat_batch47_strict_generation_spec_manifest.csv`
- Review sheet: `design/development/asset_candidates/starter_cats/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15/thecat_starter_cat_batch47_strict_generation_spec_review_sheet.png`
- Process note: `design/development/asset_candidates/starter_cats/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15/starter_cat_batch47_strict_generation_spec_process_note.md`
- Agent prompt: `design/development/agent_prompts/p0_asset_batch_47_starter_cat_strict_generation_spec_pack.md`

## Rows

### Saiban / Sword Saint

- Source lock: `saiban_turnaround_colored`
- Palette guard: `#beb6af;#e5ded8;#898074;#713234;#f6f1e9;#372928;#61564b`
- Prompt file: `design/development/asset_candidates/starter_cats/saiban/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15/thecat_cat_saiban_batch47_generation_prompt_v001.md`
- JSON spec: `design/development/asset_candidates/starter_cats/saiban/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15/thecat_cat_saiban_batch47_strict_generation_spec_v001.json`
- Active screenshot: `05-active-cat-saiban.png`
- Recommendation: `strict_generation_spec_only_do_not_import`

### Nephthys / Moon-Sand Agent

- Source lock: `nephthys_turnaround_colored`
- Palette guard: `#374050;#2b2e34;#5c4a33;#99703f;#bb9769;#e1d9cb;#716045`
- Prompt file: `design/development/asset_candidates/starter_cats/nephthys/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15/thecat_cat_nephthys_batch47_generation_prompt_v001.md`
- JSON spec: `design/development/asset_candidates/starter_cats/nephthys/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15/thecat_cat_nephthys_batch47_strict_generation_spec_v001.json`
- Active screenshot: `06-active-cat-nephthys.png`
- Recommendation: `strict_generation_spec_only_do_not_import`

### Suzune / Sleep Shrine Healer

- Source lock: `suzune_turnaround_colored`
- Palette guard: `#cec4bb;#ebe4de;#f6f0ea;#8d7d72;#8c3e2c;#49352d;#c0926d`
- Prompt file: `design/development/asset_candidates/starter_cats/suzune/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15/thecat_cat_suzune_batch47_generation_prompt_v001.md`
- JSON spec: `design/development/asset_candidates/starter_cats/suzune/batch_47_starter_cat_strict_generation_spec_pack_2026-06-15/thecat_cat_suzune_batch47_strict_generation_spec_v001.json`
- Active screenshot: `07-active-cat-suzune.png`
- Recommendation: `strict_generation_spec_only_do_not_import`

## Blocking Items

- Any future image generation must use these specs plus the source turnaround as the primary reference.
- Any generated output must return through cutout, manifest, review sheet, and active-cat screenshot validation before Unity import.
- Reject human body proportions, generic mascot drift, missing required props, and palette drift from the source lock.
