# Batch 80 Starter Skill Icon Motifs Review

Status: `candidate_complete_pending_visual_review`

## Pass Criteria

| Check | State | Notes |
| --- | --- | --- |
| No cat body or portrait generation | [x] | Icons are symbolic only. |
| Textless output | [x] | No readable Chinese, Latin letters, numbers, or watermark. |
| Transparent cutouts | [x] | Chroma-key source was alpha-matted and split into individual PNGs. |
| Multi-size output | [x] | 256, 128, 64, and 32 px variants exist for all 18 motifs. |
| Candidate boundary | [x] | Files are under `design/development/asset_candidates/...`; no Unity `.meta` files should be present. |
| Source-lock safety | [x] | Motifs use skill/authority symbolism only; no runtime body replacement. |

## Visual Risks To Review

| Asset | Risk | Required decision |
| --- | --- | --- |
| `thecat_ui_skill_saiban_battle_flag_rally_*` | The red flag uses a crescent-like mark and may read less Celtic/medieval than Saiban's source identity. | Accept as moon-banner symbolism or regenerate with a plainer torn red battle flag and shield/sword cue. |
| `thecat_ui_skill_suzune_team_heal_ice_enchant_*` | Includes small human-like silhouettes despite the no-human prompt. | Regenerate if silhouettes read as people rather than abstract party markers. |
| All 32 px variants | Fine detail may collapse at HUD size. | Validate at 32 and 64 px over dark and light HUD panels. |
| All icons | Round premium frame style may be visually heavier than existing skill HUD frame assets. | Review in Battle HUD before import. |

## Independent Review Result

| Reviewer | Verdict | Required action |
| --- | --- | --- |
| UI/style | `revise_then_candidate_hold` | Keep candidate-only; reject Saiban battle flag v001 and Suzune team-heal v001; test 64/32 px and lighter HUD frame treatment before import. |
| Character consistency | `accept_16_regenerate_2` | Accept all Nephthys icons, five Saiban icons, and five Suzune icons for source-consistency candidates; regenerate the two rejected cells. |

## v002 Local Review

| Asset | State | Notes |
| --- | --- | --- |
| `thecat_ui_skill_saiban_battle_flag_rally_*_candidate_v002` | [x] revise accepted for candidate hold | Removes crescent flag and human/crowd silhouette. Keeps red pennant, shield, sword, crown, and medieval knotwork cues. |
| `thecat_ui_skill_suzune_team_heal_ice_enchant_*_candidate_v002` | [x] revise accepted for candidate hold | Removes humanoid silhouettes and medical cross. Uses bell, paw sigils, talismans, snowflake ring, and moonlit heal field. |

## 64/32 px HUD Board

| Item | State | Notes |
| --- | --- | --- |
| Accepted set board | [x] | `icons/thecat_ui_starter_skill_icon_motifs_batch80_hud_readability_board_v001.png` uses 16 retained v001 icons plus 2 v002 replacements. |
| Readability finding | [ ] | 64px is generally readable; 32px exposes a heavy-frame risk, especially on sand and bell motifs. Needs UI/HUD owner decision before import. |
| Lightframe comparison board | [x] | `icons/thecat_ui_starter_skill_icon_motifs_batch80_lightframe_comparison_board_v001.png` compares accepted v001/v002 with v003 lightframe variants. |
| Lightframe finding | [ ] | v003 improves subject scale and 32px readability but remains a round framed icon family. Needs visual review before replacing or promoting any accepted candidate. |

## v003 Lightframe Review Integration

| Skill | Decision | Source |
| --- | --- | --- |
| Saiban shield barrier | switch to v003 lightframe | UI/HUD review: cleaner 32px shield read. |
| Saiban battle flag rally | switch to v003 lightframe | UI/HUD and character review: avoids crescent/crowd drift and reads cleaner than v002. |
| Nephthys quicksand trap spiral | switch to v003 lightframe | UI/HUD review: cleaner 32px swirl. |
| Suzune team heal ice enchant | switch to v003 lightframe | UI/HUD and character review: avoids humanoid/medical drift. |
| Other 14 skills | keep current v001/v002 | UI/HUD review: current silhouettes preserve stronger skill/civilization reads. |

Recommended set:

`recommended/thecat_ui_starter_skill_icon_motifs_batch80_recommended_manifest.csv`

## Battle HUD Overlay Test

| Item | State | Notes |
| --- | --- | --- |
| Ready frame board | [x] | `hud_overlay_test/thecat_ui_starter_skill_icon_motifs_batch80_battle_hud_overlay_board_v001.png` uses the actual installed `thecat_ui_skill_ready_frame_512_v001.png`. |
| Cooldown overlay | [x] | The board and per-icon `cooldown_64` composites use the actual installed `thecat_ui_skill_cooldown_overlay_512_v001.png`. |
| Initial local read | [ ] | Recommended icons remain visible, but the horizontal skill-ready frame competes with round icons and the 64px cooldown state is crowded. Needs focused review before Unity import. |

## Recommended Next Step

Use the recommended set as the next Unity import-candidate basis, but keep it
outside `Assets` until Battle HUD skill button/cooldown overlay tests pass.
Keep the rejected v001 cells and non-selected variants in history, but do not
import them.
