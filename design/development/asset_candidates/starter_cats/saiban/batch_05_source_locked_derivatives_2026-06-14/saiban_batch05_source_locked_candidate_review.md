# Saiban Batch 05 Source-Locked Candidate Review

## Decision

- Recommendation: candidate review only; do not import into Unity yet.
- Reason: candidates are deterministic derivatives of the locked current sprite, but formal starter-cat import remains blocked pending active-cat screenshot comparison approval. Registered active-cat screenshots do not approve import by themselves.
- Import gate: compare the registered active-cat capture against the colored turnaround contact sheet and replace this block note with an explicit approval note before import.

## Evidence

- Source lock id: `saiban_turnaround_colored`
- Locked colored turnaround: `design/梦境支配者核心玩法/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png`
- Source SHA-256: `156a7fcb4ac3e9a75bf54788f12b7f18a43b6eaff3c14607ea689af612403dc1`
- Current Unity combat sprite: `Assets/TheCat/Art/Characters/Sprites/thecat_cat_saiban_combat_sprite_512_v001.png`
- Current sprite SHA-256: `fe13afc3758f19f66fd87debc56943a19b946ac78a44eab22d9ee1b146cc106b`
- Registered active-cat screenshot: `05-active-cat-saiban.png`
- Side-by-side review sheet: `design/development/asset_candidates/starter_cats/saiban/batch_05_source_locked_derivatives_2026-06-14/thecat_cat_saiban_batch05_source_locked_review_sheet.png`
- Turnaround conformance spec: `design/development/asset_review/p0_starter_cat_turnaround_conformance_spec_2026-06-14.md`

## Candidate PNGs

- `combat_sprite_refinement_512`: `design/development/asset_candidates/starter_cats/saiban/batch_05_source_locked_derivatives_2026-06-14/thecat_cat_saiban_combat_sprite_refinement_512_candidate_v001.png`
  - SHA-256: `dee5b8922436a7b551c8ff6e9e7221fd84fd65e71c6e2aa4cbc7e51604125a94`
- `front_animation_keyframe_512`: `design/development/asset_candidates/starter_cats/saiban/batch_05_source_locked_derivatives_2026-06-14/thecat_cat_saiban_front_animation_keyframe_512_idle_center_candidate_v001.png`
  - SHA-256: `dee5b8922436a7b551c8ff6e9e7221fd84fd65e71c6e2aa4cbc7e51604125a94`
- `hud_avatar_256`: `design/development/asset_candidates/starter_cats/saiban/batch_05_source_locked_derivatives_2026-06-14/thecat_cat_saiban_hud_avatar_256_candidate_v001.png`
  - SHA-256: `5d5a6668c4554dd2e920cf26461d8d8bb21fb3304e411635b71aa8a62b323093`
- `skill_icon_motif_128`: `design/development/asset_candidates/starter_cats/saiban/batch_05_source_locked_derivatives_2026-06-14/thecat_cat_saiban_skill_icon_motif_128_candidate_v001.png`
  - SHA-256: `f22b2fa57ca7e07c5bce3a336234ffe76892c89d7b72c6a520753bd6b54b98c2`

## Trait Coverage

- Preserved: silver-blue armored non-human cat proportions
- Preserved: front-view tabby face markings
- Preserved: oath shield silhouette
- Preserved: sword silhouette
- Preserved: cape and helm read from colored turnaround

## Turnaround Conformance Checklist

- Review basis: `design/development/asset_review/p0_starter_cat_turnaround_conformance_spec_2026-06-14.md`
- Decision state: hold unless every front, side, back, palette, and prop/costume anchor below passes against the locked colored turnaround.

### Front-view anchors

- Required: front view silver-gray tabby face stripes and pale green eyes
- Required: front view red torn cape collar over silver-gold armor
- Required: front view round sun shield on the left side and single sword on the right side

### Side-view anchors

- Required: side view compact non-human cat muzzle and upright ears
- Required: side view red cape trails behind armor with striped tail visible
- Required: side view shield disk and angled sword silhouette remain readable

### Back-view anchors

- Required: back view gray tabby head stripes and rounded cat head
- Required: back view torn red cape covers armor with dark holes along the lower edge
- Required: back view striped tail sits below the cape with sword silhouette at the side

### Palette anchors

- Required: silver-gray fur and tabby markings
- Required: deep red cape fabric
- Required: silver armor, gold trim, blue gems, and warm sun shield face

### Prop/costume anchors

- Required: round oath sun shield
- Required: single sword and silver-gold armor plates
- Required: red torn cape, helm, and striped tail silhouette

### Prohibited drift

- Required: Reject generated-lineup or generic knight-cat drift over the colored three-view turnaround.
- Required: Reject human knight torso, long human legs, or biped costume posture.
- Required: Reject palette drift away from silver-gray fur, red cape, silver armor, gold trim, and blue gems.
- Required: Reject missing front, side, or back anchors including shield, sword, cape, tabby face, and striped tail.


## Rejection Rules

- Reject if a later candidate drifts from the colored turnaround markings, costume, props, or silhouette.
- Reject if it introduces human proportions or human costume posture.
- Reject if the palette shifts away from the locked colored turnaround.
- Reject if any required cat-specific trait is missing.
