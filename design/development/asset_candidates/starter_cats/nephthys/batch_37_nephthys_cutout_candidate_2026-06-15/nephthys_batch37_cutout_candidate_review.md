# Nephthys Batch 37 Cutout Candidate Review

Decision: candidate review only; do not import into Unity yet.

formal import remains blocked until active-cat Play Mode screenshot review passes.

## Source Authority

- Colored three-view turnaround: `design/梦境支配者核心玩法/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png`
- Batch 32 review sheet: `design/development/asset_candidates/starter_cats/nephthys/batch_32_nephthys_strict_turnaround_derivatives_2026-06-14/thecat_cat_nephthys_batch32_strict_turnaround_review_sheet.png`
- Batch 36 candidate review: `design/development/asset_candidates/starter_cats/nephthys/batch_36_nephthys_ai_refinement_candidate_2026-06-15/nephthys_batch36_ai_refinement_candidate_review.md`
- Source lock id: `nephthys_turnaround_colored`
- Active screenshot required before import: `06-active-cat-nephthys.png`

## Cutout Outputs

- Alpha 1024 candidate: `design/development/asset_candidates/starter_cats/nephthys/batch_37_nephthys_cutout_candidate_2026-06-15/thecat_cat_nephthys_cutout_alpha_1024_candidate_v001.png`
- Alpha 512 preview: `design/development/asset_candidates/starter_cats/nephthys/batch_37_nephthys_cutout_candidate_2026-06-15/thecat_cat_nephthys_cutout_alpha_512_preview_v001.png`
- Checkerboard review composite: `design/development/asset_candidates/starter_cats/nephthys/batch_37_nephthys_cutout_candidate_2026-06-15/thecat_cat_nephthys_cutout_checkerboard_512_review_v001.png`
- Alpha mask review: `design/development/asset_candidates/starter_cats/nephthys/batch_37_nephthys_cutout_candidate_2026-06-15/thecat_cat_nephthys_cutout_alpha_mask_512_review_v001.png`
- Review sheet: `design/development/asset_candidates/starter_cats/nephthys/batch_37_nephthys_cutout_candidate_2026-06-15/thecat_cat_nephthys_batch37_cutout_review_sheet.png`
- Process note: `design/development/asset_candidates/starter_cats/nephthys/batch_37_nephthys_cutout_candidate_2026-06-15/nephthys_batch37_cutout_process_note.md`

## Cutout Metrics

- Background RGB sampled from border: `242,237,229`
- Flood threshold: `48`
- Transparent pixels: `709878`
- Opaque pixels: `338698`
- Opaque coverage: `32.30%`

## Visual Review

- Pass: output has an alpha channel and transparent corners for Unity sprite review.
- Pass: core Nephthys identity remains inherited from Batch 36: non-human cat body, gold-brown tabby markings, golden eyes, deep navy hood and cloak, crescent ornament, blue tear gem, sand-gold script trim, blue gemstone chest ornament, winged gold collar, ankh emblem, floating pyramid/obelisk prop, and striped tail.
- Watch: local flood-fill transparency can leave edge residue or over-soften pale fur, cloak tips, pyramid particles, and tail edges; verify in Unity against dark and warm HUD fields.
- Watch: this is still a front-view candidate only; side and back anchors remain governed by the colored turnaround and Batch 32 source-derived references.

## Rejection Rules

- Reject if future cutout iterations clip ears, hood, crescent ornament, blue tear gem, face markings, floating pyramid prop, cyan particles, ankh, cloak panels, paws, or striped tail.
- Reject if future iterations introduce human body proportions, long legs, human hands, Cleopatra costume cliche, generic Egyptian fantasy drift, or generic controller costume.
- Reject if alpha edges show obvious parchment halos in Unity active-cat screenshot review.
- Reject if the candidate is imported into Unity before active-cat screenshot review.
