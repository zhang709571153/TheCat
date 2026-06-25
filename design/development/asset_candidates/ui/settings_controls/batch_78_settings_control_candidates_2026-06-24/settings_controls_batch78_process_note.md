# Settings Controls Batch 78 Process Note

Process: built-in image_gen generation, workspace source copy, local chroma-key alpha removal with the imagegen helper, deterministic horizontal alpha segmentation, exact-size transparent control normalization, contact sheet creation, manifest generation, and candidate review.

Generation prompt summary:

- Six settings UI controls in one horizontal row.
- Control order: slider track, slider knob, switch off, switch on, checkbox unchecked, checkbox checked.
- Flat `#00ff00` chroma-key background, no cats, no text, no starter-cat motifs.
- UI language: dreamglass navy panels, moon-blue rim light, lavender glow, restrained fish-gold active accents, muted gray-blue inactive accents.

Chroma-key result:

- Transparent pixels: 1371347 / 1572519.
- Partially transparent pixels: 4115 / 1572519.
- Alpha sheet size: 1983x793.

Detected control segments:

- `slider_track` -> `design/development/asset_candidates/ui/settings_controls/batch_78_settings_control_candidates_2026-06-24/controls/thecat_ui_settings_slider_track_384x64_candidate_v001.png` (384x64)
- `slider_knob` -> `design/development/asset_candidates/ui/settings_controls/batch_78_settings_control_candidates_2026-06-24/controls/thecat_ui_settings_slider_knob_96x96_candidate_v001.png` (96x96)
- `switch_off` -> `design/development/asset_candidates/ui/settings_controls/batch_78_settings_control_candidates_2026-06-24/controls/thecat_ui_settings_switch_off_192x96_candidate_v001.png` (192x96)
- `switch_on` -> `design/development/asset_candidates/ui/settings_controls/batch_78_settings_control_candidates_2026-06-24/controls/thecat_ui_settings_switch_on_192x96_candidate_v001.png` (192x96)
- `checkbox_unchecked` -> `design/development/asset_candidates/ui/settings_controls/batch_78_settings_control_candidates_2026-06-24/controls/thecat_ui_settings_checkbox_unchecked_96x96_candidate_v001.png` (96x96)
- `checkbox_checked` -> `design/development/asset_candidates/ui/settings_controls/batch_78_settings_control_candidates_2026-06-24/controls/thecat_ui_settings_checkbox_checked_96x96_candidate_v001.png` (96x96)

Manifest rows: 6.

No Unity import was performed.

Known validation limits from independent review:

- Candidate PNGs, source image, and alpha sheet are hash-checked by the validator.
- Contact sheet, review sheet, review note, and process note are existence-checked but not hash-checked.
- Builder segmentation is deterministic and sorted left-to-right, but segment coordinates are not persisted in the manifest.
