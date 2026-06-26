# Batch106 Main Menu Entry Tokens Agent Review

Verdict: `PASS_WITH_P2`

Gate: `candidate_complete_pending_unity_review`

Batch106 is a candidate-only static symbolic main-menu UI token pack under `Qr1 UI/style truth revision 816`. It produces 9 transparent sprites for main-menu entry and utility affordances. It does not claim IAd character-source approval, character body art, portraits, paws, animation frames, formal replacements, runtime import, or Unity acceptance.

## Review Inputs

- Candidate folder: `design/development/asset_candidates/ui/main_menu_entry_tokens/batch_106_main_menu_entry_tokens_2026-06-26/`
- Source copy: `source/tc_ui_menu_batch106_chromakey_source_v001.png`
- Alpha sheet: `alpha/tc_ui_menu_batch106_alpha_sheet_v001.png`
- Manifest: `tc_ui_menu_batch106_semantic_manifest.csv`
- Contact sheet: `tc_ui_menu_batch106_semantic_contact_sheet_v001.png`
- 64px readability board: `tc_ui_menu_batch106_64px_main_menu_readability_board_v001.png`
- Final review CSV: `tc_ui_menu_batch_106_main_menu_entry_tokens_2026-06-26_final_review.csv`
- Process note: `tc_ui_menu_batch_106_main_menu_entry_tokens_2026-06-26_process_note.md`

## Agent Verdicts

| Review lane | Agent | Verdict | Notes |
| --- | --- | --- | --- |
| Visual/style | Cicero `019effdb-f546-7dd0-ac1b-7009b50cd3b9` | `PASS_WITH_P2` | Qr1-style gold bevels, teal dreamglass, cream outer stroke, soft storybook lighting, textless symbols, and 64px readability pass. Watch `cat_room_home_token` and `exit_door_token` in actual main-menu context. |
| Source-lock/boundary | Fermat `019effdc-3854-7c33-a3ca-f3216d103332` | `PASS_WITH_P2` | Static symbolic `ui_main_menu` token boundary is preserved. No IAd claim, no character bodies, no portraits, no paws, no frames, no replacements. Watch `cat_room_home_token`, `notice_bell_token`, `settings_cog_token`, and `disabled_lock_token` semantics in UI context. |
| Production QA | Zeno `019effdc-8782-7c50-bf12-98d65d85247f` | `PASS_WITH_P2` | Image/package QA passes: source, alpha, 9 sprites, manifest hashes, contact sheet, 64px board, alpha padding, no `.meta`, and no runtime `Assets` promotion. Paperwork P2 is closed by this integrated note, final review CSV, and process-note update. |

## Final Decisions

| Semantic name | Decision | Follow-up |
| --- | --- | --- |
| `start_dream_gate_token` | `candidate_keep` | Unity import settings, binding proof, main-menu screenshots, target-background readability, clean Console. |
| `continue_run_moon_token` | `candidate_keep` | Prove continue/resume meaning in the main-menu layout. |
| `cat_room_home_token` | `candidate_conditional` | Must read as cat-room/home hub, not generic sleep/bedroom entry or character approval. |
| `dream_route_map_token` | `candidate_keep` | Prove route/map meaning in the main-menu layout. |
| `collection_book_token` | `candidate_keep` | Prove collection/log meaning in the main-menu layout. |
| `settings_cog_token` | `candidate_conditional` | Confirm it does not collide with other system icons and clearly means settings. |
| `exit_door_token` | `candidate_conditional` | Confirm it does not read as another dream-entry portal beside `start_dream_gate_token`. |
| `notice_bell_token` | `candidate_keep` | Sparkle is decorative; do not use it to imply reward/favorite unless UI copy supports that meaning. |
| `disabled_lock_token` | `candidate_conditional` | Confirm locked/disabled state in actual layout and avoid settings/lock visual collision. |

Decision counts: `candidate_keep=5`, `candidate_conditional=4`, `reject_rework=0`, `pending_review=0`.

## Production QA Evidence

- Source copied into workspace candidate folder.
- Alpha sheet is present and locally derived from chroma-key source.
- Manifest has 9 rows and 9 matching semantic PNGs.
- Contact sheet and 64px readability board exist.
- Each sprite has alpha extrema `(0, 255)`.
- Candidate directory contains no Unity `.meta` files.
- No Batch106 files were promoted into `Assets/` or `Resources/`.
- Import notes remain `ppu=100`, `pivot=center`, sorting note `UI canvas menu token slot`, collider policy `none_ui_click_area_owned_by_parent`.

## Remaining Runtime Gates

- [ ] Unity import settings.
- [ ] Binding proof.
- [ ] Main-menu placement screenshots at actual UI scale.
- [ ] Target-background readability proof.
- [ ] Settings/lock and start/exit semantic disambiguation in context.
- [ ] Clean Console proof.
- [ ] Formal runtime-promotion approval.

No runtime import was performed.
