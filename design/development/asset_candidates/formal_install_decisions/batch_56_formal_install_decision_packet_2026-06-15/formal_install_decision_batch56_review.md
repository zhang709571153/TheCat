# Formal Install Decision Batch 56 Review

Decision: blocked pending Unity evidence. No candidate is approved for install.

This packet keeps the Codex candidate pipeline honest after Batch 54 and Batch 55: candidates can be produced outside Unity, but installation still requires Unity Console, screenshot, Sprite import, runtime scale, HUD/world readability, and scene/prefab binding evidence.

## Unity MCP Status

- Local setup check found Unity `6000.4.10f1`, `com.unity.ai.assistant` `2.12.0-pre.1`, relay `C:/Users/PC/.unity/relay/relay_win.exe`, and Codex config entries for Unity MCP.
- Current Codex tool discovery did not expose Unity MCP editor tools, so Console, AssetDatabase, Play Mode, screenshot, and scene/prefab checks are deferred.
- Existing connection registry includes approved records and older capacity/plan-limit records; current validation still requires live Unity MCP tool calls.

## Decisions

### starter_cat_saiban

- Decision: `blocked_pending_unity_evidence`
- Install allowed: `false`
- Related batches: `batch_49_saiban_low_drift_refinement_2026-06-15`
- Candidate manifest: `design/development/asset_candidates/starter_cats/batch_49_saiban_low_drift_refinement_2026-06-15/saiban_batch49_low_drift_refinement_manifest.csv`
- Candidate review: `design/development/asset_candidates/starter_cats/saiban/batch_49_saiban_low_drift_refinement_2026-06-15/saiban_batch49_low_drift_refinement_review.md`
- Unity import root: `Assets/TheCat/Art/Characters/Sprites`
- Runtime binding scope: `cat.combat.saiban`
- Required Unity evidence: `05-active-cat-saiban.png;Unity Console no new errors;colored-turnaround comparison approval`
- Blocking reason: Saiban active-cat Play Mode screenshot is missing; formal starter-cat import gate is still blocked
- Next action: capture Saiban active-cat screenshot and compare against locked colored three-view turnaround before choosing a replacement
- Rollback note: do not touch current locked Saiban Unity sprite until the formal import gate approves it

### starter_cat_nephthys

- Decision: `blocked_pending_unity_evidence`
- Install allowed: `false`
- Related batches: `batch_50_nephthys_strict_ai_generation_candidate_2026-06-15`
- Candidate manifest: `design/development/asset_candidates/starter_cats/batch_50_nephthys_strict_ai_generation_candidate_2026-06-15/nephthys_batch50_strict_ai_generation_manifest.csv`
- Candidate review: `design/development/asset_candidates/starter_cats/nephthys/batch_50_nephthys_strict_ai_generation_candidate_2026-06-15/nephthys_batch50_strict_ai_generation_candidate_review.md`
- Unity import root: `Assets/TheCat/Art/Characters/Sprites`
- Runtime binding scope: `cat.combat.nephthys`
- Required Unity evidence: `06-active-cat-nephthys.png;Unity Console no new errors;colored-turnaround comparison approval`
- Blocking reason: Nephthys active-cat Play Mode screenshot is missing; formal starter-cat import gate is still blocked
- Next action: capture Nephthys active-cat screenshot and compare against locked colored three-view turnaround before choosing a replacement
- Rollback note: do not touch current locked Nephthys Unity sprite until the formal import gate approves it

### starter_cat_suzune

- Decision: `blocked_pending_unity_evidence`
- Install allowed: `false`
- Related batches: `batch_51_suzune_strict_ai_generation_candidate_2026-06-15`
- Candidate manifest: `design/development/asset_candidates/starter_cats/batch_51_suzune_strict_ai_generation_candidate_2026-06-15/suzune_batch51_strict_ai_generation_manifest.csv`
- Candidate review: `design/development/asset_candidates/starter_cats/suzune/batch_51_suzune_strict_ai_generation_candidate_2026-06-15/suzune_batch51_strict_ai_generation_candidate_review.md`
- Unity import root: `Assets/TheCat/Art/Characters/Sprites`
- Runtime binding scope: `cat.combat.suzune`
- Required Unity evidence: `07-active-cat-suzune.png;Unity Console no new errors;colored-turnaround comparison approval`
- Blocking reason: Suzune active-cat Play Mode screenshot is missing; formal starter-cat import gate is still blocked
- Next action: capture Suzune active-cat screenshot and compare against locked colored three-view turnaround before choosing a replacement
- Rollback note: do not touch current locked Suzune Unity sprite until the formal import gate approves it

### enemy_black_mud_nightmare

- Decision: `blocked_pending_unity_evidence`
- Install allowed: `false`
- Related batches: `batch_40_black_mud_cutout_candidate_2026-06-15`
- Candidate manifest: `design/development/asset_candidates/enemies/batch_40_black_mud_cutout_candidate_2026-06-15/black_mud_batch40_cutout_manifest.csv`
- Candidate review: `design/development/asset_candidates/enemies/black_mud_nightmare/batch_40_black_mud_cutout_candidate_2026-06-15/black_mud_batch40_cutout_candidate_review.md`
- Unity import root: `Assets/TheCat/Art/Enemies/Sprites`
- Runtime binding scope: `enemy.black_mud_nightmare`
- Required Unity evidence: `07-active-enemy-black-mud.png;Unity Console no new errors;bed-pressure scale review;prefab/scene binding review`
- Blocking reason: Black Mud active-enemy screenshot and prefab/scene binding evidence are missing
- Next action: capture Black Mud runtime screenshot and verify bed-pressure scale before choosing a replacement
- Rollback note: keep current enemy runtime bindings unchanged until active-enemy review approves the candidate

### enemy_cold_light_shadow

- Decision: `blocked_pending_unity_evidence`
- Install allowed: `false`
- Related batches: `batch_42_cold_light_cutout_candidate_2026-06-15`
- Candidate manifest: `design/development/asset_candidates/enemies/batch_42_cold_light_cutout_candidate_2026-06-15/cold_light_batch42_cutout_manifest.csv`
- Candidate review: `design/development/asset_candidates/enemies/cold_light_shadow/batch_42_cold_light_cutout_candidate_2026-06-15/cold_light_batch42_cutout_candidate_review.md`
- Unity import root: `Assets/TheCat/Art/Enemies/Sprites`
- Runtime binding scope: `enemy.cold_light_shadow`
- Required Unity evidence: `08-active-enemy-cold-light.png;Unity Console no new errors;ranged warning readability;prefab/scene binding review`
- Blocking reason: Cold Light active-enemy screenshot and warning readability evidence are missing
- Next action: capture Cold Light runtime screenshot and verify ranged warning readability before choosing a replacement
- Rollback note: keep current enemy runtime bindings unchanged until active-enemy review approves the candidate

### enemy_call_tyrant

- Decision: `blocked_pending_unity_evidence`
- Install allowed: `false`
- Related batches: `batch_44_call_tyrant_cutout_candidate_2026-06-15`
- Candidate manifest: `design/development/asset_candidates/enemies/batch_44_call_tyrant_cutout_candidate_2026-06-15/call_tyrant_batch44_cutout_manifest.csv`
- Candidate review: `design/development/asset_candidates/enemies/call_tyrant/batch_44_call_tyrant_cutout_candidate_2026-06-15/call_tyrant_batch44_cutout_candidate_review.md`
- Unity import root: `Assets/TheCat/Art/Enemies/Sprites`
- Runtime binding scope: `enemy.call_tyrant`
- Required Unity evidence: `09-active-boss-call-tyrant.png;Unity Console no new errors;boss summon/throw readability;prefab/scene binding review`
- Blocking reason: Call Tyrant active-boss screenshot and Boss behavior readability evidence are missing
- Next action: capture Call Tyrant runtime screenshot and verify summon/throw readability before choosing a replacement
- Rollback note: keep current Boss runtime bindings unchanged until active-boss review approves the candidate

### bedroom_interactables

- Decision: `blocked_pending_unity_evidence`
- Install allowed: `false`
- Related batches: `batch_54_bedroom_interactable_candidates_2026-06-15`
- Candidate manifest: `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/bedroom_interactables_batch54_manifest.csv`
- Candidate review: `design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/bedroom_interactables_batch54_candidate_review.md`
- Unity import root: `Assets/TheCat/Art/Scenes/BedroomDream`
- Runtime binding scope: `bed;interactable.litter_box;interactable.feeder`
- Required Unity evidence: `battle-world runtime screenshot;Unity Console no new errors;Sprite import settings;pathing readability;interaction feedback;scene binding review`
- Blocking reason: runtime scale, pathing readability, and scene/prefab binding evidence are missing
- Next action: run a Unity install preview scene and compare Batch 54 props against current BedroomDream sprites before any replacement
- Rollback note: leave current BedroomDream prop sprites unchanged unless runtime scale and interaction readability pass

### starter_skill_vfx

- Decision: `blocked_pending_unity_evidence`
- Install allowed: `false`
- Related batches: `batch_55_starter_skill_vfx_candidates_2026-06-15`
- Candidate manifest: `design/development/asset_candidates/vfx/starter_skills/batch_55_starter_skill_vfx_candidates_2026-06-15/starter_skill_vfx_batch55_manifest.csv`
- Candidate review: `design/development/asset_candidates/vfx/starter_skills/batch_55_starter_skill_vfx_candidates_2026-06-15/starter_skill_vfx_batch55_candidate_review.md`
- Unity import root: `Assets/TheCat/Art/VFX`
- Runtime binding scope: `skill.saiban;skill.nephthys;skill.suzune;status.shield;status.slow;status.mark;status.knockback;status.sleep_stable`
- Required Unity evidence: `skill timing screenshot set;Unity Console no new errors;Sprite import settings;runtime VFX scale;HUD/world readability;scene binding review`
- Blocking reason: runtime VFX scale, skill timing, and scene/prefab binding evidence are missing; Saiban paw emblem needs approval or simplification
- Next action: test VFX at combat scale and split full emblems into smaller per-skill sprites if readability is too busy
- Rollback note: leave current generic VFX bindings unchanged until a specific VFX install target is approved

## Safety

- No files were copied into `Assets`.
- No Unity `.meta` files were created.
- Runtime visual bindings and manifest catalogs remain unchanged.
- Batch 54 and Batch 55 are complete candidate packs, not install approvals.
