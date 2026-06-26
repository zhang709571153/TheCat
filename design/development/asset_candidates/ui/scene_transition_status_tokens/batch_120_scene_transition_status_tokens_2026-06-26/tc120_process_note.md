# Scene Transition Status Tokens Process Note

Batch: `batch_120_scene_transition_status_tokens_2026-06-26`
Family: `ui`
Source truth: `Qr1 UI/style truth revision 816; scene-selection/dream-route UI context; no IAd live approval claim; no HDo/FoW9 archive claim`

Process: built-in Codex `imagegen` source sheet, workspace copy, local magenta chroma-key alpha removal, projection split into 9 independent transparent UI sprites, target-size readability proof, and review variants. The user requested the Codex imagegen/image2 path; the current built-in tool does not expose an exact model selector, so this note records the intent without claiming a model-locked output.

Prompt goal: create a static no-animation 3x3 UI scene-transition status token sheet for bedroom, cat room, dream route, battle warning, reward, shop/event, settings, locked, and unknown states. Prompt constraints explicitly banned text, watermark, character body, animal body, cat face, paw, costume, portrait, sequence frame, map screenshot, and shadows on the chroma-key background.

Source and alpha:

- Generated source copied from `C:\Users\PC\.codex\generated_images\019efc23-3cc2-7f23-9beb-cb16d118a13f\ig_02ea6a2819ecc02d016a3dd64a69a4819981c5287f5c63d700.png`.
- Workspace source: `design/development/asset_candidates/ui/scene_transition_status_tokens/batch_120_scene_transition_status_tokens_2026-06-26/source/thecat_ui_scene_xfer_batch120_source_sheet_v001.png`.
- Workspace alpha: `design/development/asset_candidates/ui/scene_transition_status_tokens/batch_120_scene_transition_status_tokens_2026-06-26/alpha/thecat_ui_scene_xfer_batch120_alpha_sheet_v001.png`.
- Auto key color: `#f602f7`.
- Transparent pixels: `953606/1572516`.
- Partially transparent pixels: `26852/1572516`.
- Source SHA256: `FE9F3923438E339FCA39DAF737E8B1A077B176CF8A16AD882A6AF41040551087`.
- Alpha SHA256: `8CB1324250D0EADFE02271FE7C558B6CA93DBD5B74C17F2067346C1B4681930C`.

Cut and review artifacts:

- Projection row bands: `(59,400)`, `(459,799)`, `(856,1178)`.
- Projection column bands: `(54,388)`, `(460,784)`, `(868,1198)`.
- Semantic sprites: `9`.
- Manifest: `design/development/asset_candidates/ui/scene_transition_status_tokens/batch_120_scene_transition_status_tokens_2026-06-26/thecat_ui_scene_xfer_batch120_semantic_manifest.csv`.
- Contact sheet: `design/development/asset_candidates/ui/scene_transition_status_tokens/batch_120_scene_transition_status_tokens_2026-06-26/thecat_ui_scene_xfer_batch120_semantic_contact_sheet_v001.png`.
- Contact sheet SHA256: `2836FA41EB0B19BF0E420D15D6439134BE244EEADB25A51548C1E53D498112F8`.
- Readability board: `design/development/asset_candidates/ui/scene_transition_status_tokens/batch_120_scene_transition_status_tokens_2026-06-26/thecat_ui_scene_xfer_batch120_96px_64px_scene_transition_readability_board_v001.png`.
- Readability board SHA256: `959A018848B8F6D992EC7CB6A3B9E74B177CB5D0447302382DE739D33AFE87F9`.
- Review variants: 45 PNGs plus 9 manifests under `reviews/variants/`.

Final candidate decisions:

- `candidate_keep`: dream_route_transition_ready_token, reward_transition_available_token, locked_transition_gate_token.
- `candidate_conditional`: bedroom_transition_ready_token, cat_room_transition_ready_token, battle_transition_warning_token, shop_event_transition_available_token, settings_transition_safe_token, unknown_transition_gate_token.

Checklist:

- [x] Asset table written before generation.
- [x] Raw source art copied into `source/`.
- [x] Background removed locally and alpha candidates stored outside runtime folders.
- [x] Manifest/contact sheet/review variants/readability board produced.
- [x] Independent visual/style review completed.
- [x] Independent target-size readability review completed.
- [ ] Independent production QA review completed.
- [ ] Runtime import remains blocked until import settings, binding proof, screenshots, human approval, and clean console evidence pass.

No runtime import was performed.
