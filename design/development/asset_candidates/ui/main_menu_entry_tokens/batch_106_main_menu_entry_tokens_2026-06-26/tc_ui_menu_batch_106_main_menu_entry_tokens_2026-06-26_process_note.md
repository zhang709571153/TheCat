# Batch106 Main Menu Entry Tokens Process Note

Batch: `batch_106_main_menu_entry_tokens_2026-06-26`
Family: `ui`
Source truth: `Qr1 UI/style truth revision 816; ui_main_menu lane`
Risk: `safe_symbolic_non_character`
Target use: `candidate_only_main_menu_ui_tokens`

Process: built-in Codex `imagegen` source sheet, workspace source copy, local chroma-key alpha removal, projection-based 3x3 sheet splitting, deterministic contact sheet, and 64px readability board generation.

Generated source:

- Original built-in imagegen output: `C:/Users/PC/.codex/generated_images/019efc23-3cc2-7f23-9beb-cb16d118a13f/ig_01d97fc23cc16f68016a3d64ff8c6c819baabc526238fd6eff.png`
- Workspace source copy: `design/development/asset_candidates/ui/main_menu_entry_tokens/batch_106_main_menu_entry_tokens_2026-06-26/source/tc_ui_menu_batch106_chromakey_source_v001.png`
- Names file: `design/development/asset_candidates/ui/main_menu_entry_tokens/batch_106_main_menu_entry_tokens_2026-06-26/source/tc_ui_menu_batch106_names.txt`

Chroma-key and cut results:

- Source size: `1254x1254`, RGB.
- Chroma key helper output: `alpha/tc_ui_menu_batch106_alpha_sheet_v001.png`.
- Key color sampled by helper: `#f106ec`.
- Transparent pixels: `890113/1572516`.
- Partially transparent pixels: `71867/1572516`.
- Projection row bands: `(48,406)`, `(463,801)`, `(844,1163)`.
- Projection column bands: `(77,410)`, `(468,789)`, `(850,1191)`.
- Manifest rows: `9`.
- Semantic sprites: `9`.
- Contact sheet: `tc_ui_menu_batch106_semantic_contact_sheet_v001.png`.
- 64px readability board: `tc_ui_menu_batch106_64px_main_menu_readability_board_v001.png`.

Review integration:

- Visual/style review agent `019effdb-f546-7dd0-ac1b-7009b50cd3b9` / Cicero: `PASS_WITH_P2`.
- Source-lock/boundary review agent `019effdc-3854-7c33-a3ca-f3216d103332` / Fermat: `PASS_WITH_P2`.
- Production QA review agent `019effdc-8782-7c50-bf12-98d65d85247f` / Zeno: `PASS_WITH_P2`.
- Final review CSV decision counts: `candidate_keep=5`, `candidate_conditional=4`, `reject_rework=0`, `pending_review=0`.

Candidate decision summary:

- `candidate_keep`: `start_dream_gate_token`, `continue_run_moon_token`, `dream_route_map_token`, `collection_book_token`, `notice_bell_token`.
- `candidate_conditional`: `cat_room_home_token`, `settings_cog_token`, `exit_door_token`, `disabled_lock_token`.
- `reject_rework`: none.

Checklist:

- [x] Asset table written before generation.
- [x] Raw source art copied into `source/`.
- [x] Background removed locally and alpha candidates stored outside runtime folders.
- [x] Manifest/contact sheet and 64px readability proof produced.
- [x] Visual/style review completed.
- [x] Source-lock/boundary review completed.
- [x] Production QA review completed.
- [x] Final review CSV decisions integrated.
- [ ] Runtime import remains blocked until import settings, binding proof, main-menu screenshots, target-background readability, formal approval, and clean console evidence pass.

No character body, portrait, paw, animation frame, framesheet, formal character replacement, or IAd character-source approval is claimed by this package.

No runtime import was performed.
