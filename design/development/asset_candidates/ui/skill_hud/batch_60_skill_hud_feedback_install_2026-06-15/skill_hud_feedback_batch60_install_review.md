# P0 Batch 60 - Skill HUD Feedback Install Review

## Decision

Installed six non-cat Skill HUD feedback assets from Batch 57 candidates into Unity.
This batch contains no cat bodies, no cat portraits, and no starter-cat turnaround crops.

## Runtime Scope

- Ready, cooldown, no-target, and low-hunger skill cards now resolve state feedback assets.
- Target-resolved skill cards can surface the auto-target reticle.
- Interaction controls can surface the interaction-range ripple.

## Rows

- `thecat_ui_skill_ready_frame_512_v001` -> `Assets/TheCat/Art/UI/Icons/thecat_ui_skill_ready_frame_512_v001.png` binding `skill_hud.ready_frame` sha256 `54ad28cc2e7a77d90c82acb3f8d0d76096449178add9a021b64c82cf877c8d80`
- `thecat_ui_skill_cooldown_overlay_512_v001` -> `Assets/TheCat/Art/UI/Icons/thecat_ui_skill_cooldown_overlay_512_v001.png` binding `skill_hud.cooldown_overlay` sha256 `e6ca824839bd01d7e68f149dbb59352ca25a449c51f597018231c0f75a25b52c`
- `thecat_ui_skill_no_target_marker_512_v001` -> `Assets/TheCat/Art/UI/Icons/thecat_ui_skill_no_target_marker_512_v001.png` binding `skill_hud.no_target_marker` sha256 `3d3d776c2ef001dc0971d071fe8e91bd6d3ab98224456ca5b9216ff4f5470982`
- `thecat_ui_skill_hunger_cost_chip_512_v001` -> `Assets/TheCat/Art/UI/Icons/thecat_ui_skill_hunger_cost_chip_512_v001.png` binding `skill_hud.hunger_cost_chip` sha256 `62bbff7835df9b3e2cd4b2618423d3c968f0b310e76d67692db2ef7e58928107`
- `thecat_ui_auto_target_reticle_512_v001` -> `Assets/TheCat/Art/UI/Icons/thecat_ui_auto_target_reticle_512_v001.png` binding `skill_hud.auto_target_reticle` sha256 `bf91bb26272c06ea318a772779116929142736ba3f9768ed6a00a3e94dc89085`
- `thecat_ui_interaction_range_ripple_512_v001` -> `Assets/TheCat/Art/UI/Icons/thecat_ui_interaction_range_ripple_512_v001.png` binding `battle_hud.interaction_range_ripple` sha256 `b0f24caeb37643b14ac71d22112ac779ac422940179accd8d303eda12b4cfbed`

## Pending Unity Checks

- Refresh AssetDatabase and inspect Sprite import settings.
- Run a battle HUD screenshot pass for skill-state readability at runtime scale.
- Confirm Console remains clean after HUD icons are resolved in play mode.