# P0 Asset Batch 02 - Gameplay Placeholders Agent Prompt

## Task Scope

Generate or extract P0 gameplay placeholder sprites and HUD icons after Batch 01
style anchors are accepted:

- Saiban, Nephthys, and Suzune combat sprites.
- Black Mud Nightmare and Cold Light Shadow combat sprites.
- Bedroom Dream battle background.
- Bed, litter box, and feeder interaction props.
- Owner sleep, cat HP, team poop, and team hunger HUD icons.

Use Batch 01 outputs as secondary mood references only. For cats, enemies, and
bedroom props, the listed design-source images are the hard source of truth.
Keep sprites practical for Unity graybox replacement: readable silhouette,
centered subject, generous padding, transparent background, and no baked labels.

## Required Reading

- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `design/development/P0_ASSET_MANIFEST.csv`
- `design/development/prompts/p0_gameplay_placeholders.md`
- `design/development/prompts/p0_style_anchors.md`
- `design/梦境支配者核心玩法/assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png`
- `design/梦境支配者核心玩法/assets/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png`
- `design/梦境支配者核心玩法/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png`
- `design/梦境支配者核心玩法/assets/enemies/en01_black_mud_nightmare/concept/black_mud_nightmare_concept.png`
- `design/梦境支配者核心玩法/assets/enemies/en01_black_mud_nightmare/animation/black_mud_nightmare_animation.png`
- `design/梦境支配者核心玩法/assets/enemies/en04_cold_light_shadow/concept/cold_light_shadow_concept.png`
- `design/梦境支配者核心玩法/assets/enemies/en04_cold_light_shadow/animation/cold_light_shadow_animation.png`
- `design/梦境支配者核心玩法/assets/levels/lv01_bedroom_dream/concept/bedroom_dream_map_concept.png`
- `design/梦境支配者核心玩法/assets/levels/lv01_bedroom_dream/sprites/bedroom_dream_foreground_sprites.png`
- `design/梦境支配者核心玩法/assets/levels/lv01_bedroom_dream/sprites/bedroom_dream_mid_background_sprites.png`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetGenerationBatchCatalog.cs`

## Expected Output

- Accepted PNG files under the exact manifest import paths in:
  - `Assets/TheCat/Art/Characters/Sprites/`
  - `Assets/TheCat/Art/Enemies/Sprites/`
  - `Assets/TheCat/Art/Scenes/BedroomDream/`
  - `Assets/TheCat/Art/UI/Icons/`
- For the battle background, preserve the Bedroom Dream map-concept bed /
  room layout and output the accepted runtime image as
  `Assets/TheCat/Art/Scenes/BedroomDream/thecat_bg_bedroomdream_battle_1920x1080_v001.png`.
- Manifest status changes only for accepted files present in the workspace.
- Development log entry with prompt summaries, paths, and consistency notes.
- Rejected outputs moved outside `Assets` to `design/development/rejected_assets/`
  with a short mismatch reason.

## Do Not Modify

- Runtime logic or existing C# systems.
- Batch 01 anchor files unless the main session explicitly approves a new
  version.
- Boss-specific assets reserved for Batch 03.
- Design-source reference files under `design/梦境支配者核心玩法/assets/`.

## Acceptance Criteria

- Starter cat sprites preserve the colored turnaround proportions, fur colors,
  eye colors, clothing, weapons, props, and civilization motifs strictly enough
  that the source character is unmistakable.
- Starter cat sprites remain cat characters, not human bodies with cat ears.
- Black Mud Nightmare preserves the source concept/animation silhouette,
  black-violet mud material, and hostile core/eye cue.
- Cold Light Shadow preserves the source concept/animation silhouette, cold cyan
  rectangular glow, and ranged-harasser read.
- Bed, litter box, and feeder preserve the source bedroom prop identity,
  proportions, and palette enough to fit the existing room sheets.
- Bedroom Dream battle background preserves the source map-concept room layout,
  bed location, interaction prop readability, and dream bedroom palette. It is
  not cat art and does not affect starter-cat formal import readiness.
- HUD icons are readable at 64x64 and harmonize with the status icon anchor.
- No asset is marked `generated` unless the file exists at its manifest path and
  passes the source-reference consistency check.

## Validation

Run offline compile, P0 code smoke, and import readiness. When Unity MCP/editor
is available, verify texture type/import settings for sprites and icons, inspect
Console, and capture a bedroom/HUD preview screenshot.
