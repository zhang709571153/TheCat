# Batch 45 Starter Cat Source-Lock Audit Review

Decision: source-lock audit only; do not import into Unity yet.

This pack exists because starter cat assets must strictly match the colored three-view turnaround, not merely the general project style.

## Scope

- Covers Saiban, Nephthys, and Suzune only.
- Uses the locked colored three-view turnarounds as the hard authority.
- Compares each locked turnaround with the current Unity combat sprite and latest transparent cutout candidate.
- Keeps all Batch 45 outputs outside `Assets`.
- Creates no Unity `.meta` files.
- Formal import remains blocked until active-cat Play Mode screenshot comparison passes.

## Outputs

- Manifest: `design/development/asset_candidates/starter_cats/batch_45_starter_cat_source_lock_audit_pack_2026-06-15/starter_cat_batch45_source_lock_audit_manifest.csv`
- Review sheet: `design/development/asset_candidates/starter_cats/batch_45_starter_cat_source_lock_audit_pack_2026-06-15/thecat_starter_cat_batch45_source_lock_audit_review_sheet.png`
- Process note: `design/development/asset_candidates/starter_cats/batch_45_starter_cat_source_lock_audit_pack_2026-06-15/starter_cat_batch45_source_lock_audit_process_note.md`
- Agent prompt: `design/development/agent_prompts/p0_asset_batch_45_starter_cat_source_lock_audit_pack.md`

## Cat Rows

### Saiban / Sword Saint

- Source lock: `saiban_turnaround_colored`
- Source turnaround: `design/梦境支配者核心玩法/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png`
- Current Unity sprite: `Assets/TheCat/Art/Characters/Sprites/thecat_cat_saiban_combat_sprite_512_v001.png`
- Latest cutout preview: `design/development/asset_candidates/starter_cats/saiban/batch_31_cutout_candidate_2026-06-14/thecat_cat_saiban_cutout_alpha_512_preview_v001.png`
- Active screenshot gate: `05-active-cat-saiban.png`
- Audit card: `design/development/asset_candidates/starter_cats/saiban/batch_45_starter_cat_source_lock_audit_pack_2026-06-15/thecat_cat_saiban_batch45_source_lock_lineage_card_v001.png`

### Nephthys / Moon-Sand Agent

- Source lock: `nephthys_turnaround_colored`
- Source turnaround: `design/梦境支配者核心玩法/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png`
- Current Unity sprite: `Assets/TheCat/Art/Characters/Sprites/thecat_cat_nephthys_combat_sprite_512_v001.png`
- Latest cutout preview: `design/development/asset_candidates/starter_cats/nephthys/batch_37_nephthys_cutout_candidate_2026-06-15/thecat_cat_nephthys_cutout_alpha_512_preview_v001.png`
- Active screenshot gate: `06-active-cat-nephthys.png`
- Audit card: `design/development/asset_candidates/starter_cats/nephthys/batch_45_starter_cat_source_lock_audit_pack_2026-06-15/thecat_cat_nephthys_batch45_source_lock_lineage_card_v001.png`

### Suzune / Sleep Shrine Healer

- Source lock: `suzune_turnaround_colored`
- Source turnaround: `design/梦境支配者核心玩法/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png`
- Current Unity sprite: `Assets/TheCat/Art/Characters/Sprites/thecat_cat_suzune_combat_sprite_512_v001.png`
- Latest cutout preview: `design/development/asset_candidates/starter_cats/suzune/batch_35_suzune_cutout_candidate_2026-06-15/thecat_cat_suzune_cutout_alpha_512_preview_v001.png`
- Active screenshot gate: `07-active-cat-suzune.png`
- Audit card: `design/development/asset_candidates/starter_cats/suzune/batch_45_starter_cat_source_lock_audit_pack_2026-06-15/thecat_cat_suzune_batch45_source_lock_lineage_card_v001.png`

## Rejection Rules

- Reject any future starter-cat asset that drifts from the colored three-view turnaround.
- Reject any human body proportions, human costume pose, or generic cute mascot replacement.
- Reject missing side/back anchors even if the front view looks acceptable.
- Reject palette drift from the locked colored turnaround.
- Reject any candidate that cannot be compared against the active-cat Play Mode screenshot.
