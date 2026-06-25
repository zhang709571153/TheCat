# System Icons Batch 79 Candidate Review

Decision: candidate review only; do not import into Unity.

This batch fills the P0 UI inventory gap for general system icons: settings, sound, mute, back, close, pause, continue, retry, lock, and warning. It is a non-cat symbolic UI icon packet and does not modify runtime prefabs, scenes, or `Assets` files.

## Outputs

- Chroma source: `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/thecat_ui_system_icons_batch79_chromakey_source_v001.png`
- Alpha sheet: `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/thecat_ui_system_icons_batch79_alpha_sheet_v001.png`
- 128px icons: `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_128`
- 64px icons: `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_64`
- 32px icons: `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_32`
- Contact sheet: `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/thecat_ui_system_icons_batch79_contact_sheet_v001.png`
- Review sheet: `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/thecat_ui_system_icons_batch79_review_sheet_v001.png`
- Manifest: `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/system_icons_batch79_manifest.csv`
- Process note: `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/system_icons_batch79_process_note.md`
- Agent prompt: `design/development/agent_prompts/p0_asset_batch_79_system_icon_candidates.md`

## Visual Decision

- Pass: ten system icons are present: settings, sound, mute, back, close, pause, continue, retry, lock, and warning.
- Pass: no cat body, fur markings, paws, tails, starter-cat costume motifs, colored-turnaround crops, text, letters, numbers, or watermarks are present.
- Pass: icons are symbolic UI assets rather than character art.
- Pass: the icon language is consistent with Batch 78 settings controls and the dreamglass UI direction.
- Watch: `mute` keeps sound arcs behind the slash and must be checked at 32px to avoid reading as sound-on.
- Watch: `warning` avoids text/exclamation marks and must be checked in context to ensure it still reads as a warning state.

## Independent Review Findings

- P0: three independent review lanes found no visual/source-lock, tooling, or tracking blocker for candidate-complete status.
- P1: `mute` remains readable at 32px, but because sound arcs remain behind the slash, final UI review must check it against `sound` to avoid a sound-on read.
- P1: `warning` is readable as a triangle, but without an exclamation/text cue it can read as a dream sigil; warning-state recognition must be confirmed in context.
- P1: the validator hashes candidate PNGs plus source/alpha sheets, but only existence-checks the contact sheet, review sheet, review note, and process note.
- P1: the builder uses deterministic 2x5 grid splitting, but source cell coordinates are not preserved in the manifest or independently rechecked by the validator.
- P1: the validator does not token-check process-note content.
- Tracking: keep Batch 79 as `candidate_complete_pending_unity_review`.

## Unity Gate

- Import is blocked until Unity validates Sprite import settings, 64px/32px readability, dark/light panel readability, scene/prefab binding, and Console status.
- Candidate files stay outside `Assets` and must not receive Unity `.meta` files.

## Manifest Rows

- `settings` `128px` -> `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_128/thecat_ui_system_icon_settings_128_candidate_v001.png`
- `settings` `64px` -> `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_64/thecat_ui_system_icon_settings_64_candidate_v001.png`
- `settings` `32px` -> `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_32/thecat_ui_system_icon_settings_32_candidate_v001.png`
- `sound` `128px` -> `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_128/thecat_ui_system_icon_sound_128_candidate_v001.png`
- `sound` `64px` -> `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_64/thecat_ui_system_icon_sound_64_candidate_v001.png`
- `sound` `32px` -> `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_32/thecat_ui_system_icon_sound_32_candidate_v001.png`
- `mute` `128px` -> `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_128/thecat_ui_system_icon_mute_128_candidate_v001.png`
- `mute` `64px` -> `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_64/thecat_ui_system_icon_mute_64_candidate_v001.png`
- `mute` `32px` -> `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_32/thecat_ui_system_icon_mute_32_candidate_v001.png`
- `back` `128px` -> `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_128/thecat_ui_system_icon_back_128_candidate_v001.png`
- `back` `64px` -> `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_64/thecat_ui_system_icon_back_64_candidate_v001.png`
- `back` `32px` -> `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_32/thecat_ui_system_icon_back_32_candidate_v001.png`
- `close` `128px` -> `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_128/thecat_ui_system_icon_close_128_candidate_v001.png`
- `close` `64px` -> `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_64/thecat_ui_system_icon_close_64_candidate_v001.png`
- `close` `32px` -> `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_32/thecat_ui_system_icon_close_32_candidate_v001.png`
- `pause` `128px` -> `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_128/thecat_ui_system_icon_pause_128_candidate_v001.png`
- `pause` `64px` -> `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_64/thecat_ui_system_icon_pause_64_candidate_v001.png`
- `pause` `32px` -> `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_32/thecat_ui_system_icon_pause_32_candidate_v001.png`
- `continue` `128px` -> `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_128/thecat_ui_system_icon_continue_128_candidate_v001.png`
- `continue` `64px` -> `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_64/thecat_ui_system_icon_continue_64_candidate_v001.png`
- `continue` `32px` -> `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_32/thecat_ui_system_icon_continue_32_candidate_v001.png`
- `retry` `128px` -> `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_128/thecat_ui_system_icon_retry_128_candidate_v001.png`
- `retry` `64px` -> `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_64/thecat_ui_system_icon_retry_64_candidate_v001.png`
- `retry` `32px` -> `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_32/thecat_ui_system_icon_retry_32_candidate_v001.png`
- `lock` `128px` -> `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_128/thecat_ui_system_icon_lock_128_candidate_v001.png`
- `lock` `64px` -> `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_64/thecat_ui_system_icon_lock_64_candidate_v001.png`
- `lock` `32px` -> `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_32/thecat_ui_system_icon_lock_32_candidate_v001.png`
- `warning` `128px` -> `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_128/thecat_ui_system_icon_warning_128_candidate_v001.png`
- `warning` `64px` -> `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_64/thecat_ui_system_icon_warning_64_candidate_v001.png`
- `warning` `32px` -> `design/development/asset_candidates/ui/system_icons/batch_79_system_icon_candidates_2026-06-24/icons_32/thecat_ui_system_icon_warning_32_candidate_v001.png`
