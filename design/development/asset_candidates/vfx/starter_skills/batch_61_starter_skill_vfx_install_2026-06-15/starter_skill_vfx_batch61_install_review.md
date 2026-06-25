# P0 Batch 61 - Starter Skill VFX Install Review

## Decision

Installed three symbolic starter skill VFX assets from Batch 55 candidates into Unity.
This batch contains no cat bodies, no cat portraits, no new cat silhouettes, and no AI-generated cat-body replacement.

## Runtime Scope

- Saiban skill feedback routes to the bedline shield/sword/sun oath VFX.
- Nephthys skill feedback routes to the moon-sand obelisk/control VFX.
- Suzune skill feedback routes to the lullaby bell/torii/healing VFX.
- Generic hit, shield, sleep, litter, feeder, and mark VFX remain as fallbacks.

## Rows

- `thecat_vfx_saiban_bedline_skill_512_v001` -> `Assets/TheCat/Art/VFX/thecat_vfx_saiban_bedline_skill_512_v001.png` binding `skill_vfx.saiban_bedline` source lock `saiban_turnaround_colored` sha256 `d58f07f214e574a49cee57790cdc5377edf3341c537549d5f403db8cd0ae2932`
- `thecat_vfx_nephthys_moonsand_skill_512_v001` -> `Assets/TheCat/Art/VFX/thecat_vfx_nephthys_moonsand_skill_512_v001.png` binding `skill_vfx.nephthys_moonsand` source lock `nephthys_turnaround_colored` sha256 `6b613ee2ba2ab6f46fed2d67975b992d0b32a5fdabce7e1472f5188896c1d852`
- `thecat_vfx_suzune_lullaby_skill_512_v001` -> `Assets/TheCat/Art/VFX/thecat_vfx_suzune_lullaby_skill_512_v001.png` binding `skill_vfx.suzune_lullaby` source lock `suzune_turnaround_colored` sha256 `982f923d19ef87921ff94ff58e213c64d43201413eaa064e956d057c895bfbff`

## Cat Consistency Boundary

- The source locks are used as authority-symbol locks only.
- No starter-cat body art is imported from Batch 55.
- Any future cat-body replacement remains blocked by colored three-view turnaround comparison and active-cat Unity screenshots.

## Pending Unity Checks

- Refresh AssetDatabase and inspect Sprite import settings.
- Trigger one Saiban, Nephthys, and Suzune skill in Play Mode and capture feedback screenshots.
- Confirm VFX scale, timing, alpha edges, and Console cleanliness.