# Batch96 Unlock Role / Scene UI Tokens Agent Review

Project: `D:\Unity Workspace\TheCat`  
Batch path: `design/development/asset_candidates/ui/unlock_role_scene_tokens/batch_96_unlock_role_scene_tokens_imagegen_2026-06-25/`  
Date: 2026-06-25 +08  
Final gate: `candidate_complete_pending_unity_review`

## Scope

Batch96 is a static, no-animation unlockable role/scene UI token pack. It covers symbolic UI references for Kagemaru, Cotton/Mianhua, and Yuheng plus three related scene tokens and three unlock-card state frames.

The package contains no character body, face, portrait, recolor, runtime replacement, framesheet, or animation.

## Review Inputs

| Input | Path |
| --- | --- |
| Contact sheet | `design/development/asset_candidates/ui/unlock_role_scene_tokens/batch_96_unlock_role_scene_tokens_imagegen_2026-06-25/thecat_ui_unlock_token_batch96_semantic_contact_sheet_v001.png` |
| 64px readability board | `design/development/asset_candidates/ui/unlock_role_scene_tokens/batch_96_unlock_role_scene_tokens_imagegen_2026-06-25/thecat_ui_unlock_token_batch96_64px_readability_board_v001.png` |
| Source sheet | `design/development/asset_candidates/ui/unlock_role_scene_tokens/batch_96_unlock_role_scene_tokens_imagegen_2026-06-25/source/thecat_ui_unlock_role_scene_tokens_batch96_chromakey_source_v001.png` |
| Alpha sheet | `design/development/asset_candidates/ui/unlock_role_scene_tokens/batch_96_unlock_role_scene_tokens_imagegen_2026-06-25/source/thecat_ui_unlock_role_scene_tokens_batch96_alpha_sheet_v001.png` |
| Manifest | `design/development/asset_candidates/ui/unlock_role_scene_tokens/batch_96_unlock_role_scene_tokens_imagegen_2026-06-25/thecat_ui_unlock_token_batch96_semantic_manifest.csv` |
| Final review CSV | `design/development/asset_candidates/ui/unlock_role_scene_tokens/batch_96_unlock_role_scene_tokens_imagegen_2026-06-25/thecat_ui_unlock_token_batch96_final_review.csv` |
| Process note | `design/development/asset_candidates/ui/unlock_role_scene_tokens/batch_96_unlock_role_scene_tokens_imagegen_2026-06-25/thecat_ui_unlock_token_batch96_process_note.md` |
| Asset table | `design/development/asset_candidates/ui/unlock_role_scene_tokens/batch_96_unlock_role_scene_tokens_imagegen_2026-06-25/thecat_ui_unlock_role_scene_token_batch96_asset_table.csv` |

## Verdicts

| Review lane | Verdict | Notes |
| --- | --- | --- |
| Visual/style consistency | `PASS_WITH_P2` | All 9 accepted. Strong TheCat dream UI fit, textless, symbolic, coherent indigo/gold/teal/copper material language, no body/face/portrait/animation. |
| Source-lock / updated-goal compliance | `PASS` | No source-lock violations. Kagemaru, Cotton/Mianhua, and Yuheng motifs are symbolic references only. |
| Production package QA | `PASS_WITH_P2` | 9/9 sprites, manifest/review rows align, hashes/dimensions/alpha paths validate, no `.meta`, no runtime promotion. |

No P1 blocker remains.

## Accepted Candidates

| Semantic name | Decision | Watch |
| --- | --- | --- |
| `kagemaru_badge` | `candidate_keep` | None beyond target-size review. |
| `cotton_badge` | `candidate_keep` | None beyond target-size review. |
| `yuheng_badge` | `candidate_keep` | None beyond target-size review. |
| `siam_scene` | `candidate_keep` | P2: rich scene detail may compress at 64px; moon path should remain the intended read target. |
| `star_carpet_scene` | `candidate_keep` | P2: target-size scene token readability. |
| `bagua_shrine_scene` | `candidate_keep` | P2: rich scene detail may compress at 64px; bagua floor should remain the intended read target. |
| `ready_frame` | `candidate_keep` | P2: compare against `preview_frame`; ensure ready does not read as only an empty card. |
| `locked_frame` | `candidate_keep` | P2: lock/chains readability and dark veil weight on dark UI backgrounds. |
| `preview_frame` | `candidate_keep` | P2: preview moon-orb must remain distinct from ready state at target size. |

## Production QA Evidence

Local validation summary:

```json
{
  "manifest_rows": 9,
  "review_rows": 9,
  "issues": [],
  "meta_files": [],
  "nonascii_paths": [],
  "contact_exists": true,
  "source_exists": true,
  "alpha_exists": true,
  "semantic_png_count": 9,
  "max_path_length": 229
}
```

The first split attempt failed due to overly long Windows semantic filenames. The batch was repaired by shortening file-level names while keeping semantic notes in the asset table, then split successfully.

## Current Gate

`candidate_complete_pending_unity_review`

Do not promote or recursively import into runtime until:

- Unity import settings are explicit;
- target runtime paths/import policy are explicit;
- binding proof exists;
- target-size screenshots confirm readability, especially `locked_frame`, `siam_scene`, `star_carpet_scene`, and `bagua_shrine_scene`;
- `ready_frame` and `preview_frame` are visually distinct in the target UI;
- Console is clean;
- candidate folders and `superseded/` are excluded from runtime import unless explicitly approved.
