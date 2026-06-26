# Batch96 Unlock Role / Scene UI Tokens Process Note

Project: `D:\Unity Workspace\TheCat`  
Batch: `batch96`  
Date: 2026-06-25 +08  
Gate: `candidate_only_pending_unity_review`

## Scope

Batch96 is a static, no-animation UI sprite batch for unlockable role/scene tokens. It extends Batch95 with three non-starter character-symbol lanes and related scene/card-state UI sprites.

This batch intentionally contains no character body, no face, no portrait, no runtime replacement, no framesheet, and no animation.

## Source Truth

| Source | Use |
| --- | --- |
| `Qr1XdXd6KosnjMxjgW7cS89kn9c` | UI/style authority and P0 visual boundary. |
| `design/梦境支配者核心玩法/markdown/04 - 游戏角色设定.md` | Local fallback for IAd character design because live IAd access is still ACL-blocked. Used only for symbolic motifs in sections 6.4, 6.5, and 6.6. |
| Batch95 role/scene UI token review | Card-frame state watch items and candidate-only import boundary. |

## Asset Rows

See `thecat_ui_unlock_role_scene_token_batch96_asset_table.csv`.

The nine planned cells are:

1. `kagemaru_badge`
2. `cotton_badge`
3. `yuheng_badge`
4. `siam_scene`
5. `star_carpet_scene`
6. `bagua_shrine_scene`
7. `ready_frame`
8. `locked_frame`
9. `preview_frame`

## Generation

Mode: built-in Codex `imagegen`; no API key; no CLI fallback.

Prompt boundary:

- 3 columns x 3 rows.
- Pure `#ff00ff` chroma-key background.
- No baked text, labels, watermark, or numbers.
- No cat bodies, faces, portraits, full character silhouettes, character frames, or animation.
- Symbols and UI-ready tokens only.

Source file:

`source/thecat_ui_unlock_role_scene_tokens_batch96_chromakey_source_v001.png`

Default generated image retained under:

`C:\Users\PC\.codex\generated_images\019efc23-3cc2-7f23-9beb-cb16d118a13f\ig_0602edb24ce5b356016a3d2a4b47f8819b9bd6fb65e123414c.png`

## Background Removal

Tool:

`C:\Users\PC\.codex\skills\.system\imagegen\scripts\remove_chroma_key.py`

Command shape:

```powershell
python remove_chroma_key.py --input <source> --out <alpha> --auto-key border --soft-matte --transparent-threshold 12 --opaque-threshold 220 --despill --edge-contract 1
```

Output:

`source/thecat_ui_unlock_role_scene_tokens_batch96_alpha_sheet_v001.png`

Reported key color: `#f703f1`.

## Split

Tool:

`C:\Users\PC\.codex\skills\asset-sprite-production\scripts\sprite_batch_tools.py`

Final command used a shortened file prefix to avoid Windows path-length failures:

```powershell
python sprite_batch_tools.py split-sheet --sheet <alpha> --out-dir <batch> --batch-id batch96 --prefix thecat_ui_unlock_token --names source/thecat_ui_unlock_role_scene_tokens_batch96_names.txt --rows 3 --cols 3 --pad 24 --root <repo>
```

First split attempt failed because the original long semantic filenames exceeded the safe Windows path envelope. The source/alpha sheet were valid; names were shortened and the split was rerun successfully.

Projection bands:

- Rows: `(46,397)`, `(441,802)`, `(820,1213)`
- Columns: `(51,389)`, `(433,808)`, `(842,1197)`

## Outputs

| Artifact | Path |
| --- | --- |
| Source sheet | `source/thecat_ui_unlock_role_scene_tokens_batch96_chromakey_source_v001.png` |
| Alpha sheet | `source/thecat_ui_unlock_role_scene_tokens_batch96_alpha_sheet_v001.png` |
| Semantic sprites | `semantic_sprites/*.png` |
| Manifest | `thecat_ui_unlock_token_batch96_semantic_manifest.csv` |
| Contact sheet | `thecat_ui_unlock_token_batch96_semantic_contact_sheet_v001.png` |
| 64px readability board | `thecat_ui_unlock_token_batch96_64px_readability_board_v001.png` |
| Final review CSV | `thecat_ui_unlock_token_batch96_final_review.csv` |
| Asset table | `thecat_ui_unlock_role_scene_token_batch96_asset_table.csv` |

## Local QA

Validation summary:

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

Local QA passed for:

- alpha channel and transparent pixels;
- clean outer edges;
- no opaque magenta residue;
- no Unity `.meta` files;
- ASCII-safe file paths;
- 9 planned sprites present.
- local 64px light/dark-background readability board generated for early review.

## Current Gate

`candidate_only_pending_unity_review`

Required before runtime promotion:

- independent visual/style review;
- independent source-lock compliance review;
- independent production QA review;
- Unity import settings;
- binding proof;
- target-size screenshots, especially 64px readability for `locked_frame` and the three scene tokens;
- Console check;
- explicit no-recursive-import handling for candidate folders.
