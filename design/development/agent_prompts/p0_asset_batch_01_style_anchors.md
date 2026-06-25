# P0 Asset Batch 01 - Style Anchors Agent Prompt

## Task Scope

Generate the four P0 style anchor images that define the visual baseline before
any gameplay sprite or icon work starts:

- `thecat_style_bedroomdream_anchor_1920x1080_v001`
- `thecat_style_startercats_lineup_2048_v001`
- `thecat_style_blackmud_concept_2048_v001`
- `thecat_style_status_icons_5x64_v001`

Use the built-in Codex image generation workflow first. Generated images are
project-bound, so every accepted output must be copied into the matching
`unity_import_path` from `design/development/P0_ASSET_MANIFEST.csv`.

## Required Reading

- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `design/development/P0_ASSET_MANIFEST.csv`
- `design/development/prompts/p0_style_anchors.md`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetGenerationBatchCatalog.cs`

## Expected Output

- Four accepted PNG files under `Assets/TheCat/Art/_GeneratedReferences/`.
- Manifest rows updated from `planned` to `generated` only for accepted files
  that actually exist at their `unity_import_path`.
- Development log entry with final prompt summary, output paths, rejected
  variants if any, and consistency notes.
- Unity validation backlog entry for editor-side texture import checks.

## Do Not Modify

- Runtime combat, roguelite route, enemy, skill, or UI logic.
- Existing generated assets; create a new version if replacement is needed.
- Design source documents outside the required asset manifest/log updates.

## Acceptance Criteria

- Cats are non-humanoid cats, not cat girls or mascots in human body form.
- Bedroom anchor reads as a hand-painted dream bedroom guard-bed scene.
- Starter lineup clearly separates Saiban, Nephthys, and Suzune by symbol,
  palette, and role while keeping them in one shared art direction.
- Black Mud Nightmare language is readable, non-gory, and consistent with
  small gameplay enemy silhouettes.
- Status icon anchor covers sleep/stable, slow, knockback, mark, and shield
  with simple HUD-readable shapes and no baked text.
- All accepted files match the manifest filename and path exactly.

## Validation

Run the offline C# compile checks and the P0 code smoke harness. Then run:

- `P0AssetImportReadiness.EvaluateP0Manifest()` through the code smoke summary.
- Unity MCP/editor validation when available: refresh project, inspect Console,
  verify imported texture dimensions, and capture a Project/preview screenshot.
