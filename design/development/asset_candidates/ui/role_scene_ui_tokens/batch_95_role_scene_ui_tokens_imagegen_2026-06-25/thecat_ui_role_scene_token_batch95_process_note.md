# Batch95 Role / Scene UI Tokens Process Note

Date: 2026-06-25

## Scope

Batch95 is a static, candidate-only UI sprite pack for the updated goal direction: no animation and no character body or framesheet production. The pack contains 9 transparent sprites:

- 3 symbolic starter role badges for Saiban, Nephthys, and Suzune.
- 3 static scene tokens for bedroom dream, cat-room hub, and dream route.
- 3 empty card state frames for ready, selected, and locked UI states.

## Source Truth

- UI/style authority: Feishu `Qr1XdXd6KosnjMxjgW7cS89kn9c`, recorded locally as source-ready revision 816.
- Character authority: Feishu `IAdkdcpciobUTXxa7dBcRx7Bngf`, live ACL-blocked but local synced character design and source-lock packets are available.
- Character source-lock rule: no body, face, portrait, framesheet, recolor, runtime replacement, or formal import in this batch.

## Generation

- Tool path: Codex built-in `imagegen` skill, not CLI/API key fallback.
- Prompt type: 3x3 static sprite sheet on flat `#ff00ff` chroma-key background.
- Source file: `source/thecat_ui_role_scene_tokens_batch95_chromakey_source_v001.png`
- Alpha file: `source/thecat_ui_role_scene_tokens_batch95_alpha_sheet_v001.png`
- Cutting method: alpha projection split, 3 rows x 3 columns, 24 px padding.

## Generated Artifacts

- Asset table: `thecat_ui_role_scene_token_batch95_asset_table.csv`
- Manifest: `thecat_ui_role_scene_token_batch95_semantic_manifest.csv`
- Contact sheet: `thecat_ui_role_scene_token_batch95_semantic_contact_sheet_v001.png`
- Current sprites: `semantic_sprites/*.png`
- Superseded: `superseded/bom_split_v001/` keeps the initial split where a UTF-8 BOM polluted the first Saiban name. Current import-facing names are ASCII-clean.

## Local Validation

Current local validation passed:

- Manifest rows: 9.
- Missing files: 0.
- SHA-256 mismatches: 0.
- Non-ASCII import-facing ids/paths: 0.
- Non-transparent outer-edge pixels: 0.
- Opaque magenta-like pixels: 0.
- Unity `.meta` files: 0.

## Gate

State: `candidate_complete_pending_unity_review`.

Integrated review:

- Character source-lock compliance: `PASS`.
- Visual/style review: `PASS_WITH_P2`.
- Production QA: `PASS_WITH_P2`.

Runtime promotion still requires Unity import settings, actual UI screenshot checks, 64px readability proof, binding proof, and clean Console evidence. Import from `semantic_sprites/` only; do not recursively import `superseded/`.
