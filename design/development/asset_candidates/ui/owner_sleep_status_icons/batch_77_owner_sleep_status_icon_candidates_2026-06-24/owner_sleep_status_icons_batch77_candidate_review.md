# Owner Sleep Status Icons Batch 77 Candidate Review

Decision: candidate review only; do not import into Unity.

This batch fills the P0 UI inventory gap for four owner sleep-state HUD/settlement status icons. The icons synchronize with the Batch 76 owner sleep-state animation packet.

## Outputs

- Chroma source: `design/development/asset_candidates/ui/owner_sleep_status_icons/batch_77_owner_sleep_status_icon_candidates_2026-06-24/thecat_ui_owner_sleep_status_icons_batch77_chromakey_source_v001.png`
- Alpha sheet: `design/development/asset_candidates/ui/owner_sleep_status_icons/batch_77_owner_sleep_status_icon_candidates_2026-06-24/thecat_ui_owner_sleep_status_icons_batch77_alpha_sheet_v001.png`
- 256px icons: `design/development/asset_candidates/ui/owner_sleep_status_icons/batch_77_owner_sleep_status_icon_candidates_2026-06-24/icons_256`
- 64px icons: `design/development/asset_candidates/ui/owner_sleep_status_icons/batch_77_owner_sleep_status_icon_candidates_2026-06-24/icons_64`
- 32px icons: `design/development/asset_candidates/ui/owner_sleep_status_icons/batch_77_owner_sleep_status_icon_candidates_2026-06-24/icons_32`
- Contact sheet: `design/development/asset_candidates/ui/owner_sleep_status_icons/batch_77_owner_sleep_status_icon_candidates_2026-06-24/thecat_ui_owner_sleep_status_icons_batch77_contact_sheet_v001.png`
- Review sheet: `design/development/asset_candidates/ui/owner_sleep_status_icons/batch_77_owner_sleep_status_icon_candidates_2026-06-24/thecat_ui_owner_sleep_status_icons_batch77_review_sheet_v001.png`
- Manifest: `design/development/asset_candidates/ui/owner_sleep_status_icons/batch_77_owner_sleep_status_icon_candidates_2026-06-24/owner_sleep_status_icons_batch77_manifest.csv`
- Process note: `design/development/asset_candidates/ui/owner_sleep_status_icons/batch_77_owner_sleep_status_icon_candidates_2026-06-24/owner_sleep_status_icons_batch77_process_note.md`
- Agent prompt: `design/development/agent_prompts/p0_asset_batch_77_owner_sleep_status_icon_candidates.md`

## Visual Decision

- Pass: four states are present: deep sleep, half awake, near awake, wake failure.
- Pass: no cat body, fur markings, paws, tails, starter-cat costumes, colored-turnaround crops, text, letters, or numbers are present.
- Pass: icons are symbolic UI assets rather than owner portraits or character art.
- Pass: the state progression escalates from calm blue sleep to amber warning and purple wake failure.
- Watch: `wake_failure` may read like a purple eye/mark sigil at 32px; Unity HUD review must confirm it does not collide with existing Mark icon language.
- Watch: `half_awake` is visually intense for a first warning and should be compared against the subtler Batch 76 half-awake timing before import approval.

## Unity Gate

- Import is blocked until Unity validates Sprite import settings, 64px and 32px readability, dark/light HUD backgrounds, cooldown overlays, scene/prefab binding, and Console status.
- Candidate files stay outside `Assets` and must not receive Unity `.meta` files.

## Manifest Rows

- `deep_sleep` `256px` -> `design/development/asset_candidates/ui/owner_sleep_status_icons/batch_77_owner_sleep_status_icon_candidates_2026-06-24/icons_256/thecat_ui_owner_sleep_status_deep_sleep_256_candidate_v001.png`
- `deep_sleep` `64px` -> `design/development/asset_candidates/ui/owner_sleep_status_icons/batch_77_owner_sleep_status_icon_candidates_2026-06-24/icons_64/thecat_ui_owner_sleep_status_deep_sleep_64_candidate_v001.png`
- `deep_sleep` `32px` -> `design/development/asset_candidates/ui/owner_sleep_status_icons/batch_77_owner_sleep_status_icon_candidates_2026-06-24/icons_32/thecat_ui_owner_sleep_status_deep_sleep_32_candidate_v001.png`
- `half_awake` `256px` -> `design/development/asset_candidates/ui/owner_sleep_status_icons/batch_77_owner_sleep_status_icon_candidates_2026-06-24/icons_256/thecat_ui_owner_sleep_status_half_awake_256_candidate_v001.png`
- `half_awake` `64px` -> `design/development/asset_candidates/ui/owner_sleep_status_icons/batch_77_owner_sleep_status_icon_candidates_2026-06-24/icons_64/thecat_ui_owner_sleep_status_half_awake_64_candidate_v001.png`
- `half_awake` `32px` -> `design/development/asset_candidates/ui/owner_sleep_status_icons/batch_77_owner_sleep_status_icon_candidates_2026-06-24/icons_32/thecat_ui_owner_sleep_status_half_awake_32_candidate_v001.png`
- `near_awake` `256px` -> `design/development/asset_candidates/ui/owner_sleep_status_icons/batch_77_owner_sleep_status_icon_candidates_2026-06-24/icons_256/thecat_ui_owner_sleep_status_near_awake_256_candidate_v001.png`
- `near_awake` `64px` -> `design/development/asset_candidates/ui/owner_sleep_status_icons/batch_77_owner_sleep_status_icon_candidates_2026-06-24/icons_64/thecat_ui_owner_sleep_status_near_awake_64_candidate_v001.png`
- `near_awake` `32px` -> `design/development/asset_candidates/ui/owner_sleep_status_icons/batch_77_owner_sleep_status_icon_candidates_2026-06-24/icons_32/thecat_ui_owner_sleep_status_near_awake_32_candidate_v001.png`
- `wake_failure` `256px` -> `design/development/asset_candidates/ui/owner_sleep_status_icons/batch_77_owner_sleep_status_icon_candidates_2026-06-24/icons_256/thecat_ui_owner_sleep_status_wake_failure_256_candidate_v001.png`
- `wake_failure` `64px` -> `design/development/asset_candidates/ui/owner_sleep_status_icons/batch_77_owner_sleep_status_icon_candidates_2026-06-24/icons_64/thecat_ui_owner_sleep_status_wake_failure_64_candidate_v001.png`
- `wake_failure` `32px` -> `design/development/asset_candidates/ui/owner_sleep_status_icons/batch_77_owner_sleep_status_icon_candidates_2026-06-24/icons_32/thecat_ui_owner_sleep_status_wake_failure_32_candidate_v001.png`
