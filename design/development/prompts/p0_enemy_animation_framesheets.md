# P0 Enemy Animation Framesheets Prompt

## Scope

Produce deterministic, source-locked enemy animation frame sheets for the P0
battle-world enemies:

- `thecat_enemy_blackmud_move_framesheet_4x256_v001`
- `thecat_enemy_coldlight_cast_framesheet_4x256_v001`
- `thecat_enemy_calltyrant_bosspattern_framesheet_4x256_v001`

## Source Authority

- Black Mud Nightmare:
  `design/梦境支配者核心玩法/assets/enemies/en01_black_mud_nightmare/animation/black_mud_nightmare_animation.png`
- Cold Light Shadow:
  `design/梦境支配者核心玩法/assets/enemies/en04_cold_light_shadow/animation/cold_light_shadow_animation.png`
- Call Tyrant:
  `design/梦境支配者核心玩法/assets/enemies/en06_call_tyrant/animation/call_tyrant_animation.png`

Each output must preserve the source silhouette, color language, red enemy
readability points, black mud material, and action pose language. Do not invent
new enemy anatomy or mix enemy identities.

## Production Rule

Use source-image crop/extraction for this batch instead of freeform redraw.
Clean only the light source-sheet background and neighboring frame fragments.
The output is a 4-frame horizontal `1024x256` transparent PNG framesheet where
each 256px cell contains one readable source-derived frame.

## Cat Safety

This batch is strictly non-cat. It must not touch cat body, costume, face, fur,
turnaround, candidate derivative, HUD avatar, or skill icon assets.
