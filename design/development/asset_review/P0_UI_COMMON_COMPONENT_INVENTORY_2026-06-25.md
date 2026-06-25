# P0 UI Common Component Inventory

Packet date: 2026-06-25

Authority: Qr1XdXd6KosnjMxjgW7cS89kn9c live revision 816, especially section 9 P0 minimum art assets, plus current local asset manifests.

This packet inventories common UI component coverage before opening another UI image-generation batch. It deliberately separates installed assets, candidate-only packets, and screen-level runtime gates.

- Rows: 17
- Installed pending Unity evidence: 8
- Candidate-only or import-test rows: 9
- Missing design-needed rows: 0

| Component | Category | Current state | Installed | Candidate | Next gate | Notes |
| --- | --- | --- | --- | --- | --- | --- |
| primary_button_default | button | installed_pending_unity | yes | no | Runtime click-target and text-fit screenshots | Only the primary/default frame is installed; no state atlas yet. |
| button_state_atlas | button | candidate_complete_pending_unity_review | no | yes | Screen-priority review, then Unity click-target and text-fit screenshots | Batch 82 derivative textless button states cover the prior missing row; not image2 provenance. |
| dreamglass_panel | panel | installed_pending_unity | yes | no | Runtime panel scaling and nine-slice/padding proof | Single base panel exists; runtime should prove scale/padding before more variants. |
| modal_dialog_frame | dialog | candidate_complete_pending_unity_review | no | yes | Screen-priority review, then Unity modal scale/padding screenshots | Batch 82 derivative textless modal frames cover the prior missing row; not image2 provenance. |
| tabs_segmented_controls | tab | candidate_complete_pending_unity_review | no | yes | Screen-priority review, then Unity selected/unselected tab screenshots | Batch 82 derivative textless tabs and segmented controls cover the prior missing row; not image2 provenance. |
| route_reward_card_frames | card_frame | installed_pending_unity | yes | no | Route/card screenshot readability and selected/current/path states | Five route card frames are installed. |
| choice_content_cards | card | installed_pending_unity | yes | no | Result/route/reward UI screenshots and text overlay proof | Representative cards are installed for each current card family. |
| list_row_frame | list_row | candidate_complete_pending_unity_review | no | yes | Screen-priority review, then Unity list density and text-fit screenshots | Batch 82 derivative textless list rows cover the prior missing row; not image2 provenance. |
| system_icon_set | icon | candidate_complete_pending_unity_review | no | yes | 64px/32px semantics and import decision | Candidate-only; not installed under Assets. |
| settings_control_set | settings_control | candidate_complete_pending_unity_review | no | yes | Settings screen screenshot and drag/click behavior | Candidate-only controls exist. |
| runtime_control_panels | runtime_control | candidate_complete_pending_unity_review | no | yes | Pause/speed/restart HUD screenshot and click-target checks | Candidate-only; keep out of Assets until Unity evidence. |
| lock_warning_controls | icon | candidate_complete_pending_unity_review | no | yes | 32px/64px readability, color semantics, and screen contrast proof | Covered inside Batch 79 lock/warning system icons. |
| skill_slot_state_frames | skill_slot | candidate_complete_pending_unity_review | no | yes | Battle HUD import-test screenshots, selected-vs-ready, cooldown 99 proof | Candidate-only; v002_light is the preferred import-test candidate. |
| skill_hud_feedback_overlays | skill_hud | installed_pending_unity_validation | yes | no | Battle HUD timing/readability screenshots and binding proof | Installed symbolic skill HUD feedback exists. |
| core_gauge_bars | gauge | installed_pending_unity_validation | yes | no | Battle HUD screenshot and number/text overlay proof | Gauge frames/fills and icons are installed. |
| result_settlement_banners | banner | installed_pending_unity_validation | yes | no | Victory/defeat/settlement screenshots | Installed; still needs screen-level composition proof. |
| reward_detail_badges | badge | installed_pending_unity_validation | yes | no | Reward-detail UI screenshot and import scan | Installed; metas were fixed to Sprite/alpha in this pass. |

## Production Policy

- Do not promote or expand common UI component art until Batch 82 is reviewed against real screens.
- Batch 82 derivative candidates now cover the prior missing button state, modal/dialog, tab/segmented-control, and list-row rows.
- No missing design-needed rows remain in the common-component inventory, but screen-level entry/loading, cat-room prompt, and skill-selection detail coverage remains separate.
- Candidate-only system/settings/runtime/skill-slot rows must stay outside `Assets/` until Unity screenshots, click-target proof, import settings, and Console checks pass.
- Avoid baked Chinese text in generated UI sprites; keep text rendered by Unity.
- No starter-cat body, starter-cat frame, or character replacement asset is required or allowed by this packet.

## Validator

`design/development/tools/validate_ui_common_component_inventory.ps1`
