# P0 Asset Batch 90 - Cat Room Preflight

Review and validate the candidate-only cat room screen packet before any Unity import.

Scope:

- Candidate directory: `design/development/asset_candidates/ui/cat_room/batch_90_cat_room_preflight_2026-06-25`
- Intended import root after approval only: `Assets/TheCat/Art/UI/CatRoom`
- Source truth: Qr1 UI/style; use existing BedroomDream background and props, Batch 67 bedroom interaction affordances, Batch 82 common UI states, and core sleep/hunger/poop icons.
- Character truth boundary: do not create, crop, recolor, or replace cat body art, portraits, poses, costumes, or framesheets.

Review gates:

- Confirm manifest, contact sheet, review sheet, process note, and internal agent prompt exist and hash correctly.
- Confirm all PNGs remain under `design/development/asset_candidates` and no Unity `.meta` files are present.
- Confirm bed, feeder, litter, dream entrance, hover, disabled, blocked, and range states are visually distinct.
- Confirm no baked Chinese text; Unity-rendered interaction labels, status values, prompts, and dream entrance labels remain required.
- Confirm 1024x768 density, prop scale, and bed/feeder/litter/dream entrance click-target space are acceptable for Unity screenshot validation.

Return `PASS`, `PASS_WITH_P2`, or `FAIL` with exact file paths and concrete findings.
