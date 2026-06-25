# Nephthys Batch 05 Source-Locked Candidate Review

## Decision

- Recommendation: candidate review only; do not import into Unity yet.
- Reason: candidates are deterministic derivatives of the locked current sprite, but formal starter-cat import remains blocked pending active-cat screenshot comparison approval. Registered active-cat screenshots do not approve import by themselves.
- Import gate: compare the registered active-cat capture against the colored turnaround contact sheet and replace this block note with an explicit approval note before import.

## Evidence

- Source lock id: `nephthys_turnaround_colored`
- Locked colored turnaround: `design/梦境支配者核心玩法/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png`
- Source SHA-256: `37ad1532c8a981baaff67c05009ddc38482e9f00e71e6addfb2b321dad31de06`
- Current Unity combat sprite: `Assets/TheCat/Art/Characters/Sprites/thecat_cat_nephthys_combat_sprite_512_v001.png`
- Current sprite SHA-256: `6eabcb75078dd2bfe9c5f0ba5191af40376ad7f9ea545b12d75f39b8ffb45a20`
- Registered active-cat screenshot: `06-active-cat-nephthys.png`
- Side-by-side review sheet: `design/development/asset_candidates/starter_cats/nephthys/batch_05_source_locked_derivatives_2026-06-14/thecat_cat_nephthys_batch05_source_locked_review_sheet.png`
- Turnaround conformance spec: `design/development/asset_review/p0_starter_cat_turnaround_conformance_spec_2026-06-14.md`

## Candidate PNGs

- `combat_sprite_refinement_512`: `design/development/asset_candidates/starter_cats/nephthys/batch_05_source_locked_derivatives_2026-06-14/thecat_cat_nephthys_combat_sprite_refinement_512_candidate_v001.png`
  - SHA-256: `c068903b6816af6208cd3807a507e23942aa058fcbdda75f0cf9ad4c8f6738b9`
- `front_animation_keyframe_512`: `design/development/asset_candidates/starter_cats/nephthys/batch_05_source_locked_derivatives_2026-06-14/thecat_cat_nephthys_front_animation_keyframe_512_idle_center_candidate_v001.png`
  - SHA-256: `c068903b6816af6208cd3807a507e23942aa058fcbdda75f0cf9ad4c8f6738b9`
- `hud_avatar_256`: `design/development/asset_candidates/starter_cats/nephthys/batch_05_source_locked_derivatives_2026-06-14/thecat_cat_nephthys_hud_avatar_256_candidate_v001.png`
  - SHA-256: `5431221463e10f65fd82c66a24fc2521d6234d9d7aa52845cf1c52f66fbbf77a`
- `skill_icon_motif_128`: `design/development/asset_candidates/starter_cats/nephthys/batch_05_source_locked_derivatives_2026-06-14/thecat_cat_nephthys_skill_icon_motif_128_candidate_v001.png`
  - SHA-256: `be4374da017e2ad516166a0bdc5b32924ef145e380d85bddc5e6c7a09bfc2887`

## Trait Coverage

- Preserved: hooded non-human cat body
- Preserved: moon-sand Egyptian motif read
- Preserved: floating pyramid / obelisk prop silhouette
- Preserved: gold and blue palette from colored turnaround
- Preserved: dream-script controller identity

## Turnaround Conformance Checklist

- Review basis: `design/development/asset_review/p0_starter_cat_turnaround_conformance_spec_2026-06-14.md`
- Decision state: hold unless every front, side, back, palette, and prop/costume anchor below passes against the locked colored turnaround.

### Front-view anchors

- Required: front view gold-brown tabby face, large golden eyes, and dark blue hood
- Required: front view crescent hood ornament with blue tear gem and gold script border
- Required: front view floating pyramid over inverted obelisk prop beside raised paw

### Side-view anchors

- Required: side view hood volume wraps the cat head while ears stay visible
- Required: side view blue cloak layers and gold script trim sweep behind the compact body
- Required: side view floating pyramid/obelisk prop remains in front of the paw

### Back-view anchors

- Required: back view dark blue hood and cloak with centered vertical gold script strip
- Required: back view winged blue gem and ankh emblem on the shoulder mantle
- Required: back view split cloak exposes gold-brown striped tail

### Palette anchors

- Required: gold-brown tabby fur
- Required: deep navy cloak and hood
- Required: sand-gold trim with blue gems and cyan magic particles

### Prop/costume anchors

- Required: floating pyramid over inverted obelisk controller prop
- Required: crescent moon hood ornament and blue teardrop gem
- Required: gold script trim, ankh symbol, winged chest and back jewel

### Prohibited drift

- Required: Reject generated-lineup or generic Egyptian fantasy drift over the colored three-view turnaround.
- Required: Reject Cleopatra costume cliche, human body language, or human robe posture.
- Required: Reject palette drift away from gold-brown fur, deep navy cloth, sand-gold trim, and blue gems.
- Required: Reject missing front, side, or back anchors including hood, script trim, pyramid prop, ankh, and striped tail.


## Rejection Rules

- Reject if a later candidate drifts from the colored turnaround markings, costume, props, or silhouette.
- Reject if it introduces human proportions or human costume posture.
- Reject if the palette shifts away from the locked colored turnaround.
- Reject if any required cat-specific trait is missing.
