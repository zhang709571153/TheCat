# Suzune Batch 05 Source-Locked Candidate Review

## Decision

- Recommendation: candidate review only; do not import into Unity yet.
- Reason: candidates are deterministic derivatives of the locked current sprite, but formal starter-cat import remains blocked pending active-cat screenshot comparison approval. Registered active-cat screenshots do not approve import by themselves.
- Import gate: compare the registered active-cat capture against the colored turnaround contact sheet and replace this block note with an explicit approval note before import.

## Evidence

- Source lock id: `suzune_turnaround_colored`
- Locked colored turnaround: `design/梦境支配者核心玩法/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png`
- Source SHA-256: `9b616470da7daa77ad27c70d2b0bf3b3f30649ce0b639f6550589b9e6fe700b3`
- Current Unity combat sprite: `Assets/TheCat/Art/Characters/Sprites/thecat_cat_suzune_combat_sprite_512_v001.png`
- Current sprite SHA-256: `246ddd74501b4e81482d03e329b27c898ddd61b580bc30abeeeadef3aa61eaae`
- Registered active-cat screenshot: `07-active-cat-suzune.png`
- Side-by-side review sheet: `design/development/asset_candidates/starter_cats/suzune/batch_05_source_locked_derivatives_2026-06-14/thecat_cat_suzune_batch05_source_locked_review_sheet.png`
- Turnaround conformance spec: `design/development/asset_review/p0_starter_cat_turnaround_conformance_spec_2026-06-14.md`

## Candidate PNGs

- `combat_sprite_refinement_512`: `design/development/asset_candidates/starter_cats/suzune/batch_05_source_locked_derivatives_2026-06-14/thecat_cat_suzune_combat_sprite_refinement_512_candidate_v001.png`
  - SHA-256: `8217c96f08e0827f53780c475d191b386d11574765e18549f415f640d78a428a`
- `front_animation_keyframe_512`: `design/development/asset_candidates/starter_cats/suzune/batch_05_source_locked_derivatives_2026-06-14/thecat_cat_suzune_front_animation_keyframe_512_idle_center_candidate_v001.png`
  - SHA-256: `8217c96f08e0827f53780c475d191b386d11574765e18549f415f640d78a428a`
- `hud_avatar_256`: `design/development/asset_candidates/starter_cats/suzune/batch_05_source_locked_derivatives_2026-06-14/thecat_cat_suzune_hud_avatar_256_candidate_v001.png`
  - SHA-256: `59f2b6745150dcb231cdb7f414f760219747324eb08ec4b74d7abe92ba1a449c`
- `skill_icon_motif_128`: `design/development/asset_candidates/starter_cats/suzune/batch_05_source_locked_derivatives_2026-06-14/thecat_cat_suzune_skill_icon_motif_128_candidate_v001.png`
  - SHA-256: `90afd8fbf91f6e1c9271ab8e30993a6f85537a5a0d91fb2c6927740fc3dd9ff8`

## Trait Coverage

- Preserved: calico markings from colored turnaround
- Preserved: shrine outfit on non-human cat body
- Preserved: bell ornaments
- Preserved: wand / branch healer silhouette
- Preserved: vermilion, warm white, and moon-blue healer palette

## Turnaround Conformance Checklist

- Review basis: `design/development/asset_review/p0_starter_cat_turnaround_conformance_spec_2026-06-14.md`
- Decision state: hold unless every front, side, back, palette, and prop/costume anchor below passes against the locked colored turnaround.

### Front-view anchors

- Required: front view calico orange, black, and white face patches with blue eyes
- Required: front view white shrine robe, vermilion skirt, sash, and central gold bell
- Required: front view bell wand/branch cluster with blue paper talismans

### Side-view anchors

- Required: side view calico head patches continue across ear and cheek
- Required: side view white sleeve with blue snowflake motif and red stitch trim
- Required: side view red ribbons, hanging bells, and bell wand remain readable

### Back-view anchors

- Required: back view orange and black calico head patches across both ears
- Required: back view large vermilion bow with gold bell over white robe
- Required: back view white sleeves show blue snowflake marks and calico tail

### Palette anchors

- Required: warm white fur and robe fabric
- Required: vermilion red skirt, sash, bow, and ribbons
- Required: gold bells with moon-blue talismans and sleep effects

### Prop/costume anchors

- Required: clustered kagura bell wand/branch
- Required: red-white flower hair ornament with hanging bells
- Required: paper talismans, blue snowflake charms, central bell, and back bow

### Prohibited drift

- Required: Reject generated-lineup or generic shrine-cat drift over the colored three-view turnaround.
- Required: Reject human shrine maiden proportions, human sleeves-as-arms pose, or human costume posture.
- Required: Reject palette drift away from calico patches, white robe, vermilion cloth, gold bells, and blue talismans.
- Required: Reject missing front, side, or back anchors including calico markings, bells, wand, snowflake sleeves, and back bow.


## Rejection Rules

- Reject if a later candidate drifts from the colored turnaround markings, costume, props, or silhouette.
- Reject if it introduces human proportions or human costume posture.
- Reject if the palette shifts away from the locked colored turnaround.
- Reject if any required cat-specific trait is missing.
