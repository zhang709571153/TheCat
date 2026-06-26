# Batch116 Character Roster Status Chips Agent Review

Decision: `candidate_complete_pending_unity_review`

Do not import Batch116 into runtime folders yet. This pack is symbolic UI only and is not character body art, portrait art, paw/animal silhouette art, costume art, framesheet art, animation art, source-lock approval art, or runtime replacement art.

## Inputs

- Candidate folder: `design/development/asset_candidates/ui/character_roster_status_chips/batch_116_character_roster_status_chips_2026-06-26`
- Asset table: `thecat_batch_116_character_roster_status_chips_2026-06-26_asset_table.csv`
- Manifest: `tc_ui_roster_batch116_semantic_manifest.csv`
- Contact sheet: `tc_ui_roster_batch116_semantic_contact_sheet_v001.png`
- Readability board: `tc_ui_roster_batch116_96px_64px_character_roster_readability_board_v001.png`
- Final review CSV: `thecat_batch_116_character_roster_status_chips_2026-06-26_final_review.csv`
- Process note: `thecat_batch_116_character_roster_status_chips_2026-06-26_process_note.md`

## Review Verdicts

| Review lane | Agent | Verdict | Notes |
| --- | --- | --- | --- |
| Visual/source boundary | Peirce / `019f00f3-e0d3-77c2-81ef-f914796d8113` | `PASS_WITH_P2` | Candidate-only symbolic UI. Matches Batch113/114 ornate storybook badge language and remains compatible with Batch95/96. No character body, portrait, face, paw, costume, silhouette, framesheet, animation, baked text, watermark, or source-claim overreach. |
| 96px/64px readability | Boyle / `019f00f3-f4ef-71e1-8bb7-9fc272ba09ef` | `PASS_WITH_P2` | No P1/FAIL blocker. P2 semantic risks remain for ready-vs-selected, locked-vs-source-locked, new-vs-scene-affinity, and Batch113/114 team-slot/corner-tab collision. |
| Production QA | Mendel / `019f00f4-08f6-7f73-9a88-53386030952c` | `PASS_WITH_P2` | 9 manifest rows, 9 final-review rows, matching hashes/dimensions, alpha `(0, 255)`, transparent corners and outer edges, 24px minimum padding, no `.meta`, no `Assets/` leak. P2: tiny fully opaque near-magenta edge specks and path length watch at 240 chars. |

## Final Candidate Decisions

| Semantic | Decision | Gate note |
| --- | --- | --- |
| `character_roster_ready_chip` | `candidate_keep` | Strong ready read at 64px; prove ready-vs-selected in live roster layout. |
| `character_roster_locked_chip` | `candidate_keep` | Plain lock is readable; prove locked-vs-source-locked distinction. |
| `character_roster_new_chip` | `candidate_keep` | Sparkle marker is distinct from scene-affinity on the local board. |
| `character_roster_source_locked_chip` | `candidate_conditional` | Lock dominates at 64px; needs label, tooltip, or placement proof against plain locked. |
| `character_roster_selected_ribbon` | `candidate_conditional` | Checkmark/ribbon can collide with ready state and Batch114 corner language; use only with anchored selected context. |
| `character_roster_role_slot_chip` | `candidate_keep` | Diamond role-slot silhouette is distinct; needs roster context for meaning. |
| `character_roster_scene_affinity_chip` | `candidate_conditional` | Moon/path motif overlaps Batch113 affinity and Batch96 dream-route language; needs roster-specific proof. |
| `character_roster_team_slot_chip` | `candidate_conditional` | Linked-gem motif overlaps Batch113/114 team-ready semantics; needs non-duplication proof. |
| `character_roster_review_needed_chip` | `candidate_keep` | Exclamation plus magnifier reads as review-needed, not danger/error. |

Final review counts: 5 `candidate_keep`, 4 `candidate_conditional`, 0 `reject_rework`, 0 `pending_review`.

## Required Unity Gates

- Import only into a dedicated preflight UI path, never recursively from the candidate folder.
- Verify Sprite import settings for all 9: 2D/UI Sprite, alpha enabled, no mipmaps, UI-appropriate compression/filtering.
- Bind each semantic chip in character roster or character-select UI without using it as character body, portrait, costume, source-lock approval art, or animation.
- Capture character roster/card UI screenshots at `1920x1080`, `1365x768`, `1280x720`, and `1024x768`.
- Capture dark-card and warm-card backgrounds with chips at actual `96px` and `64px` placements.
- Prove ready-vs-selected-vs-team-slot distinction on the same screen.
- Prove locked-vs-source-locked distinction on the same screen.
- Prove new-vs-scene-affinity distinction on the same screen.
- Prove collision safety with Batch88 card frames and Batch113/114 badges/tabs active together.
- Use center pivot, UI canvas ordering, and parent-owned UI hit areas. Do not auto-create colliders.
- Require clean Console evidence and explicit human approval before formal runtime import.

## Safety

- Source truth is Qr1 UI/style revision 816 plus local character source-lock boundary only.
- IAd live access remains blocked; this pack makes no IAd live approval claim.
- No HDo/FoW9 map archive coverage is claimed.
- No runtime import was performed.
