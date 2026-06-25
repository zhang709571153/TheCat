# Bedroom Interactables Batch 54 Candidate Review

Decision: candidate review only; do not import into Unity.

Batch 54 produces Codex-side candidates for the P0 bed, litter box, and feeder interactables. The batch improves review options without changing runtime visual bindings or manifest catalog counts.

## Output Summary

- Manifest: `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/bedroom_interactables_batch54_manifest.csv`
- Review sheet: `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/thecat_props_bedroom_interactables_batch54_review_sheet.png`
- Process note: `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/bedroom_interactables_batch54_process_note.md`
- Agent prompt: `design/development/agent_prompts/p0_asset_batch_54_bedroom_interactable_candidates.md`

## Visual Review

- Bed: independent protected-bed sprite, no room crop, strong navy blanket, gold stars, crescent, wooden posts, pillow, and readable rug base.
- Litter box: v002 selected because v001 had a visible green chroma-key glow; v002 keeps the blue plastic box, tan clean litter, and paw emblem with cleaner edges.
- Feeder: high-readability pink-lavender automatic feeder with visible kibble, transparent tank, paw emblem, and moon/star accents.
- Watch item: all three AI candidates are more polished and close-up than the current source-extracted sprites, so game-scale comparison is required before install.
- Watch item: bed includes a rug base and may need runtime scale reduction if it overlaps pathing or HUD screenshots.

## Safety

- The built-in image_gen outputs were copied into the workspace candidate folder.
- Chroma-key removal was done locally with the imagegen skill helper.
- Normalized alpha candidates are 1024x1024; review variants are 512x512.
- No candidate file was copied into `Assets`.
- No Unity `.meta` files were created.
- Formal Unity import remains blocked pending Sprite import settings, scene/prefab binding, Console checks, runtime scale, and active battle-world screenshots.

## Manifest Rows

- `bed` `chromakey_source` -> `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/thecat_prop_bed_batch54_interactable_chromakey_source_v001.png`
- `bed` `alpha_candidate_1024` -> `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/thecat_prop_bed_batch54_interactable_alpha_1024_candidate_v001.png`
- `bed` `alpha_preview_512` -> `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/thecat_prop_bed_batch54_interactable_alpha_512_preview_v001.png`
- `bed` `checkerboard_review_512` -> `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/thecat_prop_bed_batch54_interactable_checkerboard_512_review_v001.png`
- `bed` `darkfield_review_512` -> `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/thecat_prop_bed_batch54_interactable_darkfield_512_review_v001.png`
- `bed` `warmfield_review_512` -> `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/thecat_prop_bed_batch54_interactable_warmfield_512_review_v001.png`
- `bed` `alpha_mask_review_512` -> `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/thecat_prop_bed_batch54_interactable_alpha_mask_512_review_v001.png`
- `litter_box` `chromakey_source` -> `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/thecat_prop_litterbox_batch54_interactable_chromakey_source_v002.png`
- `litter_box` `alpha_candidate_1024` -> `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/thecat_prop_litterbox_batch54_interactable_alpha_1024_candidate_v002.png`
- `litter_box` `alpha_preview_512` -> `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/thecat_prop_litterbox_batch54_interactable_alpha_512_preview_v002.png`
- `litter_box` `checkerboard_review_512` -> `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/thecat_prop_litterbox_batch54_interactable_checkerboard_512_review_v002.png`
- `litter_box` `darkfield_review_512` -> `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/thecat_prop_litterbox_batch54_interactable_darkfield_512_review_v002.png`
- `litter_box` `warmfield_review_512` -> `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/thecat_prop_litterbox_batch54_interactable_warmfield_512_review_v002.png`
- `litter_box` `alpha_mask_review_512` -> `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/thecat_prop_litterbox_batch54_interactable_alpha_mask_512_review_v002.png`
- `feeder` `chromakey_source` -> `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/thecat_prop_feeder_batch54_interactable_chromakey_source_v001.png`
- `feeder` `alpha_candidate_1024` -> `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/thecat_prop_feeder_batch54_interactable_alpha_1024_candidate_v001.png`
- `feeder` `alpha_preview_512` -> `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/thecat_prop_feeder_batch54_interactable_alpha_512_preview_v001.png`
- `feeder` `checkerboard_review_512` -> `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/thecat_prop_feeder_batch54_interactable_checkerboard_512_review_v001.png`
- `feeder` `darkfield_review_512` -> `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/thecat_prop_feeder_batch54_interactable_darkfield_512_review_v001.png`
- `feeder` `warmfield_review_512` -> `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/thecat_prop_feeder_batch54_interactable_warmfield_512_review_v001.png`
- `feeder` `alpha_mask_review_512` -> `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/thecat_prop_feeder_batch54_interactable_alpha_mask_512_review_v001.png`
