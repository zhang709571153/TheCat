# Batch 86 Dream Route Agent Review

Date: 2026-06-25

Candidate packet: `design/development/asset_candidates/ui/dream_route/batch_86_dream_route_preflight_2026-06-25`

## Verdict

`PASS_WITH_P2`

Batch 86 may enter candidate-only Unity screenshot validation. It is not Unity accepted and must not be promoted into `Assets` until the runtime gates below pass.

## Produced Assets

- 6 transparent dream-route sprites:
  - `route_map_panel_frame`
  - `route_layer_header_frame`
  - `route_node_socket_frame`
  - `route_choice_card_slot`
  - `route_path_ribbon_frame`
  - `route_boss_gate_frame`
- 4 local mockups:
  - `dream_route_1920x1080`
  - `route_branch_1365x768`
  - `route_boss_pressure_1280x720`
  - `route_compact_1024x768`

## Visual Review

Result: `PASS_WITH_P2`

- No P0/P1 blockers.
- The packet aligns with Qr1-derived dreamglass UI: navy glass body, cyan outer strokes, thin gold accents, and Batch 65 route-state language.
- Current, selected, available, locked, and Boss-pressure states are broadly distinguishable.
- New sprites and mockups contain no baked Chinese text. The English title logo is an existing source asset, not a new text bake.
- No starter-cat body art, character turnaround crop, Unity `.meta`, or `Assets` import was found.
- P2 watch: the compact 1024x768 mockup has tight lower-half density around bottom cards, central locked/current overlays, and path connectors. Unity text and click-target proof are required.
- P2 watch: the boss-pressure mockup reads clearly, but the magenta gate frame is visually dominant and must be checked against final card text and boss icon scale.

## Production QA Review

Result: `PASS_WITH_P2`, then local provenance fix applied.

- Batch86 validator passed with 10 manifest rows.
- The manifest covers 6 sprites plus 4 local mockups.
- Candidate, contact sheet, review sheet, source asset, and source hash chains are present.
- The candidate directory has 12 PNGs including contact/review sheets, 0 Unity `.meta` files, and no Batch86 art under `Assets`.
- Provenance is honest: `source_model` is `deterministic_local_derivative_from_route_assets_not_image2`.
- The process note records that `OPENAI_API_KEY` is not set, so strict `gpt-image-2` CLI generation is unavailable in this shell.
- P2 fixed after review: the manifest now records row-level hashes for review note, process note, and agent prompt, and the validator enforces these fields.

## Required Unity Gates

- Dream-entry and route-map screenshots at target resolutions, including 1024x768.
- Unity-rendered Chinese title, route labels, node labels, card labels, and route rewards.
- Node/path semantics for current, selected, available, locked, and Boss-pressure states.
- Route-choice card click targets and lower-half layout density proof.
- Boss gate scale against final boss icon and route-card text.
- Sprite import settings, scene/prefab binding proof, and clean Console.

## Decision

Keep Batch86 candidate-only. Add it to the P0 asset production queue and Unity validation checklist as `CandidatePackCompletePendingUnityReview`, with the P2 watches carried into runtime gates.
