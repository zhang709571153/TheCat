# Batch109 Bedroom Entry Path Props Agent Review

Decision: `candidate_complete_pending_unity_review`; do not import into runtime folders.

## Scope

- Candidate folder: `design/development/asset_candidates/map/bedroom_entry_path_props/batch_109_bedroom_entry_path_props_2026-06-26/`
- Asset table: `design/development/asset_candidates/map/bedroom_entry_path_props/batch_109_bedroom_entry_path_props_2026-06-26/thecat_map_path_batch_109_bedroom_entry_path_props_2026-06-26_asset_table.csv`
- Manifest: `design/development/asset_candidates/map/bedroom_entry_path_props/batch_109_bedroom_entry_path_props_2026-06-26/thecat_map_path_batch109_semantic_manifest.csv`
- Contact sheet: `design/development/asset_candidates/map/bedroom_entry_path_props/batch_109_bedroom_entry_path_props_2026-06-26/thecat_map_path_batch109_semantic_contact_sheet_v001.png`
- 64px readability board: `design/development/asset_candidates/map/bedroom_entry_path_props/batch_109_bedroom_entry_path_props_2026-06-26/thecat_map_path_batch109_64px_bedroom_entry_path_readability_board_v001.png`
- Process note: `design/development/asset_candidates/map/bedroom_entry_path_props/batch_109_bedroom_entry_path_props_2026-06-26/thecat_map_path_batch_109_bedroom_entry_path_props_2026-06-26_process_note.md`

## Integrated Verdict

`PASS_WITH_P2` for local candidate production.

This is not Unity acceptance. Batch109 remains candidate-only pending import settings, binding proof, bedroom map screenshots, scale/sorting/collider proof, no recursive candidate import, and clean Console evidence.

## Agent Results

### Visual / Style Review

Verdict: `PASS_WITH_P2`.

The nine props read as a coherent cozy moonlit bedroom dream-map family. Palette, soft beveling, moon/star motifs, fabric/toy materials, and static prop framing fit the Qr1-style boundary. No character-body drift, animation implication, Egypt/archive motifs, baked text, or source-locked identity claims were found.

Accepted at 64px:

- `north_doorway_dream_arch_prop`
- `side_window_moonbeam_marker`
- `floor_path_ribbon_curve`
- `sock_trail_path_marker`
- `toy_train_path_blocker`

Conditional:

- `blanket_curtain_entry_prop`: cloth is on-theme, but gray folds can soften on dark moon-shadow backgrounds.
- `alarm_clock_hazard_prop`: readable, but front-facing polish makes it the most UI-icon-like prop.
- `nightlight_safe_glow_marker`: bright star is readable but can read as a reward/safe UI token and dominate the map.
- `wall_crack_entry_marker`: readable, but higher-contrast and portal-like; use only where entry-crack semantics are intentional.

### Source-Lock / Boundary Review

Verdict: `PASS`.

The batch stays within `Qr1 UI/style truth revision 816` and the Qr1 P0 bedroom dream map boundary. It contains static bedroom/dream-map props only and does not approve character bodies, portraits, paws/faces, framesheets, animation, Egypt/desert/pyramid motifs, or HDo/FoW9 archive-derived claims.

Exact gate wording: `source_lock_boundary_review_complete; candidate batch remains candidate_only_bedroom_entry_path_props pending production_qa, unity_import_settings, binding_proof, bedroom_map_screenshots, and clean_console. No character-body/portrait/paw/face/framesheet content approved. No HDo/FoW9 Egypt/desert/pyramid/archive-derived claim approved. Animation remains paused.`

### Production QA Review

Verdict: `PASS_WITH_P2`.

Production QA found no P1 blockers. All 9 semantic sprites exist, match manifest hashes/dimensions, have alpha, clean transparent corners, zero outer-edge alpha, and no visible green/magenta key residue. No `.meta` files were found, and no Batch109 references were found under runtime project surfaces.

P2 cleanup items identified and closed in this integrated note:

- Final review CSV no longer says `pending_review`; it records 5 `candidate_keep` and 4 `candidate_conditional` rows.
- Process note no longer uses placeholder production wording; completed local steps are checked.
- Final review `asset_id` values now match the semantic manifest for automated joins.
- The integrated review note is mirrored at the batch root, under `reviews/`, and under `design/development/asset_review/` so both generic and batch-specific validators can find it.

## Final Decisions

| Semantic sprite | Decision | Gate note |
| --- | --- | --- |
| `north_doorway_dream_arch_prop` | `candidate_keep` | Readable doorway; verify scale below landmark dominance. |
| `side_window_moonbeam_marker` | `candidate_keep` | Readable moon-window path cue; verify glow contrast. |
| `blanket_curtain_entry_prop` | `candidate_conditional` | Needs dark-background cloth contrast proof. |
| `floor_path_ribbon_curve` | `candidate_keep` | Keep decal/overlay role; verify against prior path language. |
| `sock_trail_path_marker` | `candidate_keep` | Verify warm-floor contrast and path ordering at runtime scale. |
| `alarm_clock_hazard_prop` | `candidate_conditional` | Needs hazard semantic proof because it can read UI-icon-like. |
| `nightlight_safe_glow_marker` | `candidate_conditional` | Needs safe-marker semantic and brightness proof. |
| `toy_train_path_blocker` | `candidate_keep` | Verify collider/scale against Batch108 obstacles. |
| `wall_crack_entry_marker` | `candidate_conditional` | Needs entry-crack semantic proof; avoid enemy-portal confusion. |

## Runtime Gates Still Required

- Unity import settings and generated `.meta` in an approved runtime folder only after formal approval.
- Binding proof on the bedroom entry/path map.
- Bedroom map screenshots at intended scale on warm floor and dark moon-shadow placements.
- Scale proof against Batch101/102/103/108 markers, decals, and props.
- Sorting/layer proof for path/decal/prop roles.
- Collider policy confirmation: decals stay no-collider or scene-owned trigger only; blockers use scene-owned manual collider only.
- Semantic proof for alarm clock hazard, nightlight safe marker, and wall-crack entry marker.
- Clean Unity Console evidence.

## Safety

- Raw generated/source image was copied into the workspace candidate folder.
- Chroma-key removal was done locally.
- Independent sprites and review variants remain under `design/development/asset_candidates/...`.
- No candidate file was copied into runtime folders.
- No Unity `.meta` files were created in this candidate folder.
- No runtime import was performed.
