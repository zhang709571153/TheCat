# Batch97 Unlock Skill Icons Process Note

Project: `D:\Unity Workspace\TheCat`  
Batch: `batch97`  
Date: 2026-06-25 +08  
Gate: `candidate_only_pending_unity_review`

## Scope

Batch97 is a static, no-animation UI skill icon batch for unlockable characters. It covers Kagemaru, Cotton/Mianhua, and Yuheng symbolic skill motifs.

This batch intentionally contains no character body, no face, no portrait, no runtime replacement, no framesheet, and no animation.

## Source Truth

| Source | Use |
| --- | --- |
| `Qr1XdXd6KosnjMxjgW7cS89kn9c` | UI/style authority and P0 visual boundary. |
| `design/梦境支配者核心玩法/markdown/04 - 游戏角色设定.md` | Local fallback for IAd character design because live IAd access is still ACL-blocked. Used only for symbolic skill motifs in sections 6.4, 6.5, and 6.6. |
| Batch80 starter skill icon motifs | Local precedent for symbolic skill icons staying candidate-only pending HUD/Unity evidence. |

## Asset Rows

See `thecat_ui_unlock_skill_batch97_asset_table.csv`.

The nine planned cells are:

1. `kagemaru_arc_slash`
2. `kagemaru_light_lure`
3. `kagemaru_phantom_hunt`
4. `cotton_star_mark`
5. `cotton_dream_web`
6. `cotton_star_carpet_ult`
7. `yuheng_causality_coin`
8. `yuheng_bagua_array`
9. `yuheng_yinyang_burst`

## Generation

Mode: built-in Codex `imagegen`; no API key; no CLI fallback.

Prompt boundary:

- 3 columns x 3 rows.
- Pure `#ff00ff` chroma-key background.
- HUD skill icon composition, not scene cards.
- No baked text, labels, watermark, or numbers.
- No cat bodies, faces, portraits, full character silhouettes, character frames, or animation.
- Symbols and UI-ready icons only.

Source file:

`source/thecat_ui_unlock_skill_batch97_chromakey_source_v001.png`

Default generated image retained under:

`C:\Users\PC\.codex\generated_images\019efc23-3cc2-7f23-9beb-cb16d118a13f\ig_07baa5a0e6706d89016a3d2e802dcc8199a5b790fa9484a474.png`

## Background Removal

Tool:

`C:\Users\PC\.codex\skills\.system\imagegen\scripts\remove_chroma_key.py`

Command shape:

```powershell
python remove_chroma_key.py --input <source> --out <alpha> --auto-key border --soft-matte --transparent-threshold 12 --opaque-threshold 220 --despill --edge-contract 1
```

Output:

`source/thecat_ui_unlock_skill_batch97_alpha_sheet_v001.png`

Reported key color: `#f802f9`.

## Split

Tool:

`C:\Users\PC\.codex\skills\asset-sprite-production\scripts\sprite_batch_tools.py`

Command:

```powershell
python sprite_batch_tools.py split-sheet --sheet <alpha> --out-dir <batch> --batch-id batch97 --prefix thecat_ui_unlock_skill --names source/thecat_ui_unlock_skill_batch97_names.txt --rows 3 --cols 3 --pad 24 --root <repo>
```

Projection bands:

- Rows: `(56,415)`, `(451,808)`, `(845,1202)`
- Columns: `(55,409)`, `(448,803)`, `(842,1196)`

## Outputs

| Artifact | Path |
| --- | --- |
| Source sheet | `source/thecat_ui_unlock_skill_batch97_chromakey_source_v001.png` |
| Alpha sheet | `source/thecat_ui_unlock_skill_batch97_alpha_sheet_v001.png` |
| Semantic sprites | `semantic_sprites/*.png` |
| Manifest | `thecat_ui_unlock_skill_batch97_semantic_manifest.csv` |
| Contact sheet | `thecat_ui_unlock_skill_batch97_semantic_contact_sheet_v001.png` |
| 64px readability board | `thecat_ui_unlock_skill_batch97_64px_readability_board_v001.png` |
| Final review CSV | `thecat_ui_unlock_skill_batch97_final_review.csv` |
| Asset table | `thecat_ui_unlock_skill_batch97_asset_table.csv` |

## Local QA

Validation summary:

```json
{
  "manifest_rows": 9,
  "review_rows": 9,
  "issues": [],
  "meta_files": [],
  "nonascii_paths": [],
  "source_exists": true,
  "alpha_exists": true,
  "contact_exists": true,
  "semantic_png_count": 9,
  "max_path_length": 221
}
```

Local QA passed for:

- alpha channel and transparent pixels;
- clean outer edges;
- no opaque magenta residue;
- no Unity `.meta` files;
- ASCII-safe file paths;
- 9 planned sprites present;
- local 64px light/dark skill-slot readability board generated for early review.

## Current Gate

`candidate_only_pending_unity_review`

Required before runtime promotion:

- independent visual/style review;
- independent source-lock compliance review;
- independent production QA review;
- Unity import settings;
- skill-slot/HUD binding proof;
- target-size screenshots with ready/cooldown/disabled overlays;
- Console check;
- explicit import policy and target runtime paths.
