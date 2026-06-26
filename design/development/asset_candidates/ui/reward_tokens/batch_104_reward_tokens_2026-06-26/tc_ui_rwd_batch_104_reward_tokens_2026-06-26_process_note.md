# Batch104 Reward/Event/Blessing Tokens Process Note

Batch: `batch_104_reward_tokens_2026-06-26`

Family: `ui`

Source truth: `Qr1 UI/style truth revision 816; rewards_events_blessings lane`.

Process: built-in Codex `imagegen` source sheet, workspace copy, local magenta chroma-key alpha removal, projection-based 3x3 sprite cutting, semantic manifest/contact sheet generation, and local 64px readability board generation.

## Source And Safety

- Source image copied from built-in imagegen default output into `source/tc_ui_rwd_batch104_chromakey_source_v001.png`.
- Local alpha sheet: `alpha/tc_ui_rwd_batch104_alpha_sheet_v001.png`.
- Chroma key: auto-detected border key `#f603f4`, generated from requested flat `#ff00ff` key.
- Transparent pixels: `930236/1572516`.
- Partially transparent pixels: `15579/1572516`.
- No runtime import was performed.
- No character body, portrait, animation frame, framesheet, enemy body, Egypt map archive, HDo/FoW9 folder coverage, or IAd character approval is claimed.

## Semantic Order

1. `reward_chest_token`
2. `shop_token`
3. `event_question_token`
4. `partner_invite_token`
5. `blessing_sun_token`
6. `curse_risk_token`
7. `currency_fish_token`
8. `reroll_dice_token`
9. `claim_confirm_token`

## Cut Results

- Cut method: alpha projection bands on the local alpha sheet.
- Row bands: `(66,407)`, `(461,810)`, `(854,1186)`.
- Column bands: `(68,395)`, `(468,792)`, `(846,1183)`.
- Rows: 9 semantic sprites.
- Manifest: `tc_ui_rwd_batch104_semantic_manifest.csv`.
- Contact sheet: `tc_ui_rwd_batch104_semantic_contact_sheet_v001.png`.
- 64px readability board: `tc_ui_rwd_batch104_64px_reward_event_readability_board_v001.png`.

## Review Notes

- Initial visual self-check: 9 cells separated, consistent warm UI-token family, no character/body/portrait content.
- `event_question_token` contains a clear question-mark symbol and `claim_confirm_token` contains a check mark. These are UI symbols, not labels, but remain review watch items for the no-baked-text policy.
- Independent visual/style, source-lock/boundary, and production QA reviews were dispatched on 2026-06-26.

## Checklist

- [x] Asset table written before generation.
- [x] Raw source art copied into `source/`.
- [x] Background removed locally and alpha candidates stored outside runtime folders.
- [x] Manifest/contact sheet produced.
- [x] 64px readability board produced.
- [x] Independent visual/style review integrated.
- [x] Independent source-lock/boundary review integrated.
- [x] Independent production QA review integrated.
- [x] Root-level final review note mirrored into `design/development/asset_review/`.
- [x] Batch104 validator added to the local non-cat/symbolic matrix.
- [ ] Runtime import remains blocked until import settings, binding proof, reward/event screen screenshots, and clean console evidence pass.

Asset table: `tc_ui_rwd_batch_104_reward_tokens_2026-06-26_asset_table.csv`.

Final review CSV: `tc_ui_rwd_batch_104_reward_tokens_2026-06-26_final_review.csv`.
