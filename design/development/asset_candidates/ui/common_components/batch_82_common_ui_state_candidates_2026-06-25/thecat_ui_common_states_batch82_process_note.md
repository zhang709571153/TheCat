# Batch 82 Process Note

Generation method: deterministic derivative sprites from existing local UI art.

Source assets:
- `Assets/TheCat/Art/UI/Frames/thecat_ui_button_primary_384x96_v001.png`
- `Assets/TheCat/Art/UI/Frames/thecat_ui_panel_dreamglass_512x256_v001.png`
- `Assets/TheCat/Art/UI/Frames/thecat_ui_routecard_shop_frame_512x256_v001.png`

Why no image2 in this pass:
- Strict CLI `gpt-image-2` generation requires `OPENAI_API_KEY`, which is not set in this shell.
- Built-in `image_gen` does not expose a model selector, so using it here would not satisfy the explicit image2 provenance requirement.
- These missing rows are stateful UI frames that can be safely derived from the existing style assets without adding new art-language drift.

Output:
- 25 transparent PNG candidate sprites.
- Manifest: `design/development/asset_candidates/ui/common_components/batch_82_common_ui_state_candidates_2026-06-25/thecat_ui_common_states_batch82_manifest.csv`
- Contact sheet: `design/development/asset_candidates/ui/common_components/batch_82_common_ui_state_candidates_2026-06-25/thecat_ui_common_states_batch82_contact_sheet_v001.png`
- Review sheet: `design/development/asset_candidates/ui/common_components/batch_82_common_ui_state_candidates_2026-06-25/thecat_ui_common_states_batch82_review_sheet_v001.png`

Unity gate:
- Candidate-only until screen-level UI priority, import settings, click-target/readability screenshots, binding proof, and Console checks pass.
