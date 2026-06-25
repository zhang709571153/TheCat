# P0 Asset Memory / TODO Ledger

Project: `D:\Unity Workspace\TheCat`
Updated: 2026-06-25 12:02 +08:00

This ledger tracks asset-production state without implying Unity acceptance.
`[x]` means the Codex-side packet or check exists. `[ ]` means runtime evidence
or a formal install decision is still pending.

## Current Asset Memory

| Lane | Latest batch | Status | Done | Still pending |
| --- | --- | --- | --- | --- |
| Starter-cat body art | 49/50/51, 70/71/74 | blocked_by_unity_validation | [x] strict candidate evidence repaired under the 11-capture screenshot names; [x] source reference plates; [x] Saiban debug reference atlas; [x] source audit packet; [x] active-cat screenshots registered 3/3 as `05-active-cat-saiban.png`, `06-active-cat-nephthys.png`, and `07-active-cat-suzune.png` | [ ] colored-turnaround comparison approval; [ ] explicit formal import approval notes; [ ] formal runtime sprite install |
| Core enemy bodies | 40/42/44/46 | blocked_by_unity_validation | [x] candidate cutouts; [x] dashboard/review evidence | [ ] active-enemy screenshots; [ ] prefab binding; [ ] Console/import checks |
| Bedroom interactables | 54 | candidate_complete_pending_unity_review | [x] bed/litter/feeder candidate pack; [x] manifest/review notes | [ ] runtime scale screenshots; [ ] Sprite import and binding decision |
| Cat-room map elements | 91 | semantic_candidate_complete_pending_unity_review | [x] built-in Codex imagegen chroma-key source sheet; [x] alpha sheet; [x] 12 auto-cut independent PNG candidates; [x] contact sheet; [x] manifest; [x] process note; [x] independent visual review PASS_WITH_P2; [x] production QA PASS_WITH_P1; [x] v004 hard-alpha dream-portal replacement accepted; [x] 12 semantic sprites; [x] semantic manifest; [x] semantic contact sheet; [x] final semantic pack review PASS_WITH_P2 | [ ] Unity import/settings/binding proof; [ ] portal scale/pivot review; [ ] color/scale check and possible curation for interaction rings |
| Cat-room interaction states | 92 | semantic_candidate_complete_pending_unity_review | [x] built-in Codex imagegen chroma-key source sheet; [x] alpha sheet; [x] 12 projection-cut semantic sprites; [x] final contact sheet v003; [x] semantic manifest with v001/v002/v003 cooldown traceability; [x] final review CSV; [x] visual review PASS_WITH_P1; [x] production QA PASS_WITH_P1; [x] cooldown v003 P1 fix reviewed PASS_WITH_P2; [x] process note; [x] agent review note | [ ] Unity import/settings/binding proof; [ ] ring semantic curation against ready/selected/target states; [ ] small-scale tests for sleep/feed/litter/attention markers; [ ] locator reclassification decision |
| Cat-room map decals/trim | 93 | semantic_candidate_complete_pending_unity_review | [x] built-in Codex imagegen chroma-key source sheet; [x] alpha sheet; [x] 12 projection-cut semantic sprites; [x] semantic contact sheet; [x] semantic manifest with review decision, pivot, sorting, and collider fields; [x] final review CSV; [x] visual review PASS_WITH_P2; [x] production QA PASS_WITH_P2; [x] process note; [x] agent review note | [ ] Unity import/settings/binding proof; [ ] shallow/deep/target cat-room floor screenshots; [ ] light/decal sorting-layer plan; [ ] reclassify 04 as mat/pad and 10 as doorway-only threshold |
| Cat-room dream portal animation | 94 | candidate_complete_pending_unity_loop_review | [x] built-in Codex imagegen 12-frame source sheet; [x] stable Batch91-derived portal base; [x] v004 12-frame transparent 512x512 sequence; [x] manifest with standard import fields; [x] final review CSV; [x] contact sheet v004; [x] loop preview GIF v004; [x] visual review PASS_WITH_P2 after P1 drift fix; [x] production QA PASS_WITH_P2 after residue cleanup; [x] process note; [x] agent review note | [ ] Unity portal loop import/binding proof; [ ] runtime loop jitter review; [ ] cat-room scale/pivot/sorting screenshots; [ ] no-collider reuse of static dream entrance hitbox |
| Bedroom interaction feedback | 67 | candidate_complete_pending_unity_review | [x] bed/litter/feeder affordance candidate pack; [x] manifest/review notes | [ ] interaction timing screenshots; [ ] Console/import checks |
| Owner sleep-state animation | 76 | candidate_complete_pending_unity_review | [x] v002 imagegen source copied; [x] chroma alpha sheet; [x] 24 padded 256x256 frames; [x] manifest/review/process notes; [x] three-agent review integrated; [x] hardened validator passed | [ ] Unity slicing/pivot/scale; [ ] overlay-vs-bed-layer decision; [ ] sleep-state timing; [ ] battle screenshots; [ ] Console check |
| Owner sleep status icons | 77 | candidate_complete_pending_unity_review | [x] symbolic non-cat HUD/settlement icon candidate pack; [x] 12 icons across 4 states x 3 sizes; [x] manifest/review/process notes; [x] validator passed; [x] no Unity `.meta` files | [ ] HUD/settlement screenshots; [ ] 64px/32px readability; [ ] dark/light HUD and cooldown-overlay checks; [ ] Sprite import settings; [ ] scene/prefab binding; [ ] Console check |
| Settings controls | 78 | candidate_complete_pending_unity_review | [x] non-cat settings slider/switch/checkbox candidate pack; [x] 6 transparent controls at exact target sizes; [x] manifest/review/process notes; [x] three-agent review integrated; [x] validator passed; [x] no Unity `.meta` files | [ ] settings-screen screenshots; [ ] slider drag / knob alignment; [ ] switch value-fill behavior; [ ] switch on/off color-blind contrast and accessibility; [ ] dark/light panel readability; [ ] click/pointer target scale; [ ] Sprite import settings; [ ] scene/prefab binding; [ ] Console check |
| System icons | 79 | candidate_complete_pending_unity_review | [x] non-cat system icon candidate pack; [x] 30 icons across 10 meanings x 3 sizes; [x] manifest/review/process notes; [x] three-agent review integrated; [x] validator passed; [x] no Unity `.meta` files | [ ] UI screenshots; [ ] 64px/32px readability; [ ] mute vs sound clarity; [ ] warning meaning without text; [ ] dark/light panel readability; [ ] Sprite import settings; [ ] scene/prefab binding; [ ] Console check |
| Starter skill VFX | 55/61 | installed_pending_unity_validation | [x] candidate VFX pack; [x] installed symbolic VFX assets | [ ] skill-cast timing screenshots; [ ] Console/import/binding checks |
| Starter skill icon motifs | 80 | recommended_candidate_pending_hud_overlay_review | [x] chroma-key source copied; [x] alpha sheets; [x] 18 motifs x 4 sizes; [x] 2 v002 revised motifs x 4 sizes; [x] 18 v003 lightframe motifs x 4 sizes; [x] recommended mixed set; [x] Battle HUD frame/cooldown overlay test; [x] manifest/review/process notes; [x] HUD readability/comparison boards; [x] validator passed; [x] UI/style and character reviews integrated | [ ] pipeline QA agent timed out/closed; [ ] integrate HUD overlay review agents; [ ] Unity import decision |
| Skill slot frames | 81 | candidate_complete_pending_unity_review | [x] square/round ready/cooldown/disabled/selected frame candidates; [x] 256/128/64 sizes; [x] Batch 80 recommended icon-fit board; [x] focused visual/QA review integrated; [x] cooldown digit mockups `1/12/99`; [x] digit readability review integrated; [x] v002_light square-only lighter badge/corner variant; [x] v002_light icon-fit and digit boards; [x] local actual-scale HUD mockup pack; [x] local actual-scale HUD review integrated; [x] v002_light independent review integrated; [x] v002_light preferred import-test candidate recorded in notes; [x] manifest/review/process notes; [x] validators passed | [ ] selected-vs-ready Unity HUD screenshot; [ ] `99` cooldown Unity actual-HUD proof; [ ] Unity import settings; [ ] Console/binding proof; [ ] formal Unity import decision |
| UI common components | inventory + 82 | candidate_complete_pending_unity_review | [x] 17-row common component inventory; [x] 8 installed pending Unity evidence rows; [x] 9 candidate-only/import-test rows; [x] 0 missing design-needed rows after Batch 82; [x] Batch 82 derivative textless common UI state candidates generated as 25 sprites; [x] validator passed; [x] inventory and Batch 82 review agents integrated; [x] segmented-control and list-row P1 visual findings fixed; [x] cwd-independent matrix and all-evidence path validation fixes | [ ] Batch 82 screen-priority review in actual entry/loading/cat-room/skill-selection/settings surfaces; [ ] disabled/list-row contrast proof; [ ] modal padding/divider proof; [ ] focus/danger state screenshot proof; [ ] Unity import settings; [ ] click-target/readability screenshots; [ ] scene/prefab binding; [ ] Console check |
| Loading/start screen | 83 | queued_for_unity_validation_pending_screenshots | [x] Batch 83 local preflight generated; [x] 4 transparent candidate sprites; [x] 4 local loading/start mockups across 1920x1080, 1365x768, 1280x720, 1024x768; [x] visual/style review PASS_WITH_P2; [x] production QA P1 fixed and follow-up PASS; [x] validator passed; [x] matrix integrated; [x] P0 asset production queue/checklist updated as candidate-complete pending Unity review | [ ] Unity-rendered loading/start screenshots; [ ] spinner non-character interpretation proof; [ ] Unity-rendered text/state replacement for placeholders; [ ] 1024x768 crowding check; [ ] import settings; [ ] binding proof; [ ] Console check |
| Result/settlement screen | 84 | queued_for_unity_validation_pending_screenshots | [x] Batch 84 local preflight generated; [x] 7 transparent candidate sprites; [x] 4 local result/settlement mockups across 1920x1080, 1365x768, 1024x768; [x] visual/style review PASS_WITH_P2 after failure-stamp fix; [x] production QA PASS after sheet hash lock; [x] validator passed; [x] matrix integrated; [x] P0 asset production queue/checklist updated as candidate-complete pending Unity review | [ ] Unity-rendered victory/defeat/settlement screenshots; [ ] Unity-rendered Chinese text and number replacement; [ ] 1024x768 run-failed crowding check; [ ] import settings; [ ] binding proof; [ ] Console check |
| Settings/pause screen | 85 | queued_for_unity_validation_pending_screenshots | [x] Batch 85 local preflight generated; [x] 6 transparent candidate sprites; [x] 4 local settings/pause mockups across 1920x1080, 1365x768, 1280x720, 1024x768; [x] visual/style review PASS_WITH_P2 with key-hint watch item; [x] production QA PASS; [x] validator passed; [x] matrix integrated; [x] P0 asset production queue/checklist updated as candidate-complete pending Unity review | [ ] Unity-rendered settings/pause screenshots; [ ] Unity-rendered Chinese text/value replacement; [ ] 1024x768 key-hint-vs-back semantics check; [ ] tab/close/back/key-hint/slider/switch/checkbox click-target proof; [ ] import settings; [ ] binding proof; [ ] Console check |
| Dream route screen | 86 | queued_for_unity_validation_pending_screenshots | [x] Batch 86 local preflight generated; [x] 6 transparent dream-route sprites; [x] 4 local dream-entry/route-map mockups across 1920x1080, 1365x768, 1280x720, 1024x768; [x] visual/style review PASS_WITH_P2 with compact density and boss-gate scale watches; [x] production QA PASS_WITH_P2; [x] manifest provenance P2 fixed with row-level note/process/prompt hashes; [x] validator passed; [x] matrix integrated; [x] P0 asset production queue/checklist updated as candidate-complete pending Unity review | [ ] Unity-rendered dream-entry/route-map screenshots; [ ] Unity-rendered Chinese text/route labels/reward replacement; [ ] node/path semantics proof; [ ] 1024x768 lower-half route-card crowding check; [ ] boss gate scale proof; [ ] route-card click-target proof; [ ] import settings; [ ] binding proof; [ ] Console check |
| Character select screen | 88 | queued_for_unity_validation_pending_screenshots | [x] Batch 88 local preflight generated; [x] 6 transparent character-select sprites; [x] 4 local character-select mockups across 1920x1080, 1365x768, 1280x720, 1024x768; [x] visual/style review PASS_WITH_P2 with low-height density watch; [x] production QA PASS_WITH_P2; [x] provenance P2 fixed in process note and validator; [x] validator passed; [x] matrix integrated; [x] P0 asset production queue/checklist updated as candidate-complete pending Unity review | [ ] Unity-rendered character-select screenshots; [ ] source-lock HUD avatar consistency proof; [ ] Unity-rendered Chinese names/roles/descriptions/start labels; [ ] 1280x720 and 1024x768 crowding proof; [ ] three-card and start-action click-target proof; [ ] import settings; [ ] binding proof; [ ] Console check |
| Skill selection screen | 89 | queued_for_unity_validation_pending_screenshots | [x] Batch 89 local preflight generated; [x] 8 transparent skill-selection sprites; [x] 4 local skill-selection mockups across 1920x1080, 1365x768, 1280x720, 1024x768; [x] visual/style review PASS_WITH_P2 with density and state-semantics watches; [x] production QA PASS; [x] validator passed; [x] matrix integrated; [x] P0 asset production queue/checklist updated as candidate-complete pending Unity review | [ ] Unity-rendered skill-selection screenshots; [ ] selected/ready/disabled/locked state proof; [ ] cooldown/low-resource/no-target semantics; [ ] Chinese text/number replacement; [ ] click-target proof; [ ] import settings; [ ] binding proof; [ ] Console check |
| Cat room screen | 90 | runtime_surface_captured_pending_candidate_import_review | [x] Batch 90 local preflight generated; [x] 6 transparent cat-room sprites; [x] 4 local cat-room mockups across 1920x1080, 1365x768, 1280x720, 1024x768; [x] visual/style review PASS_WITH_P2 with dream-entry and disabled-state semantics watches; [x] production QA PASS; [x] validator passed; [x] matrix integrated; [x] P0 asset production queue/checklist updated as candidate-complete pending Unity review; [x] Play Mode 11-capture smoke verifies the existing cat-room runtime surface and writes `02-cat-room.png` | [ ] Batch 90 sprite import/settings/binding proof; [ ] four-resolution Unity-rendered Batch 90 parity screenshots; [ ] bed/feeder/litter/dream entrance interaction proof; [ ] hover/disabled/range states; [ ] click-target proof; [ ] Console check |
| Battle HUD screen | 87 | queued_for_unity_validation_pending_screenshots | [x] Batch 87 local preflight generated; [x] 6 transparent battle HUD sprites; [x] 4 local battle HUD mockups across 1920x1080, 1365x768, 1280x720, 1024x768; [x] production QA PASS; [x] initial visual P1 found and fixed; [x] follow-up visual review PASS with four-gauge dense layout; [x] validator passed; [x] matrix integrated; [x] P0 asset production queue/checklist updated as candidate-complete pending Unity review | [ ] Unity-rendered battle HUD screenshots; [ ] Unity-rendered Chinese labels/tooltips/text/number replacement; [ ] 1024x768 four-gauge proof; [ ] skill ready/selected/cooldown/disabled/low-resource state readability; [ ] enemy spawn/telegraph occlusion proof; [ ] pause/speed/restart click-target proof; [ ] import settings; [ ] binding proof; [ ] Console check |
| Skill HUD feedback | 57/60 | installed_pending_unity_validation | [x] candidate HUD/VFX pack; [x] installed non-cat HUD feedback assets | [ ] battle-HUD screenshots; [ ] timing/readability review |
| Runtime controls | 62/63 | candidate_complete_pending_unity_review | [x] pause/resume/speed/restart icon and panel candidates | [ ] HUD scale screenshots; [ ] shortcut readability checks |
| Secondary enemy warnings | 64 | candidate_complete_pending_unity_review | [x] future enemy warning candidate pack | [ ] enemy-prefab gate; [ ] warning screenshots |
| Route-map readability | 65/86 | candidate_complete_pending_unity_review | [x] current/selected/path/Boss route accents; [x] Batch 86 screen-level dream-route preflight composes route panels, nodes, paths, choice cards, and boss gate without importing into `Assets` | [ ] route-map screenshots; [ ] selected/current/path readability acceptance; [ ] 1024x768 route-card density proof; [ ] Boss gate scale proof |
| Chinese UI scale evidence | 75 | local_preflight_complete_pending_unity_screenshots | [x] 20-row capture matrix; [x] templates; [x] 20 local scale mockups; [x] local visual/QA review integrated; [x] code-backed packet checks | [ ] 5 surfaces x 4 resolutions Unity screenshots; [ ] Console notes |

## Batch 76 Checklist

| Item | State | Evidence |
| --- | --- | --- |
| Local Feishu-derived inventory gap identified | [x] | `design/梦境支配者核心玩法/docs/04_art_production/p0_digital_asset_inventory.md` |
| Non-cat lane selected | [x] | Owner sleep-state animation, no starter-cat source-lock risk |
| Imagegen v001 generated | [x] | Historical full-bed source retained in the Batch 76 folder |
| Review found v001 slice/prop collision risks | [x] | Three-agent review notes integrated into the candidate review |
| Imagegen v002 overlay source generated | [x] | `thecat_owner_sleep_states_batch76_chromakey_source_1536x1024_v002.png` |
| Chroma-key alpha sheet generated | [x] | `thecat_owner_sleep_states_batch76_alpha_sheet_1536x1024_candidate_v002.png` |
| 24 padded frames generated | [x] | `frames/thecat_owner_sleep_state_*_f*_256_candidate_v001.png` |
| Manifest, review sheet, contact sheet written | [x] | Batch 76 candidate directory |
| Validator hardened | [x] | Checks manifest identity, hashes, dimensions, alpha margins, path safety, and `.meta` absence |
| Validator passed | [x] | `Owner sleep state Batch 76 validation passed` |
| Unity import accepted | [ ] | Requires backlog item 190 |

## Batch 77 Checklist

| Item | State | Evidence |
| --- | --- | --- |
| Local Feishu-derived UI inventory gap identified | [x] | `design/梦境支配者核心玩法/docs/04_art_production/p0_digital_asset_inventory.md` |
| Non-cat symbolic UI lane selected | [x] | Owner sleep-state HUD/settlement status icons |
| Imagegen source generated | [x] | `thecat_ui_owner_sleep_status_icons_batch77_chromakey_source_v001.png` |
| Chroma-key alpha sheet generated | [x] | `thecat_ui_owner_sleep_status_icons_batch77_alpha_sheet_v001.png` |
| Four 256px icons generated | [x] | `icons_256/thecat_ui_owner_sleep_status_*_256_candidate_v001.png` |
| Four 64px icons generated | [x] | `icons_64/thecat_ui_owner_sleep_status_*_64_candidate_v001.png` |
| Four 32px icons generated | [x] | `icons_32/thecat_ui_owner_sleep_status_*_32_candidate_v001.png` |
| Manifest, review sheet, contact sheet written | [x] | Batch 77 candidate directory |
| Validator passed | [x] | `Owner sleep status icon Batch 77 validation passed` |
| Unity import accepted | [ ] | Requires backlog item 191 |

## Batch 78 Checklist

| Item | State | Evidence |
| --- | --- | --- |
| Local Feishu-derived UI inventory gap identified | [x] | `design/梦境支配者核心玩法/docs/04_art_production/p0_digital_asset_inventory.md` |
| Non-cat settings UI lane selected | [x] | Settings sliders, switches, and checkboxes |
| Imagegen source generated | [x] | `thecat_ui_settings_controls_batch78_chromakey_source_v001.png` |
| Chroma-key alpha sheet generated | [x] | `thecat_ui_settings_controls_batch78_alpha_sheet_v001.png` |
| Six exact-size controls generated | [x] | `controls/thecat_ui_settings_*_candidate_v001.png` |
| Manifest, review sheet, contact sheet written | [x] | Batch 78 candidate directory |
| Three-agent review integrated | [x] | No P0 blockers; P1 runtime/tooling watch items carried forward |
| Validator passed | [x] | `Settings control Batch 78 validation passed` |
| Unity import accepted | [ ] | Requires backlog item 192 |

## Batch 79 Checklist

| Item | State | Evidence |
| --- | --- | --- |
| Local Feishu-derived UI inventory gap identified | [x] | `design/梦境支配者核心玩法/docs/04_art_production/p0_digital_asset_inventory.md` |
| Non-cat system UI lane selected | [x] | Settings, sound, mute, back, close, pause, continue, retry, lock, warning |
| Imagegen source generated | [x] | `thecat_ui_system_icons_batch79_chromakey_source_v001.png` |
| Chroma-key alpha sheet generated | [x] | `thecat_ui_system_icons_batch79_alpha_sheet_v001.png` |
| Thirty size-variant icons generated | [x] | `icons_128`, `icons_64`, and `icons_32` |
| Manifest, review sheet, contact sheet written | [x] | Batch 79 candidate directory |
| Three-agent review integrated | [x] | No P0 blockers; P1 semantic/tooling watch items carried forward |
| Validator passed | [x] | `System icon Batch 79 validation passed` |
| Unity import accepted | [ ] | Requires backlog item 193 |

## Next TODO

| Priority | Task | Status |
| --- | --- | --- |
| P1 | Run Unity backlog item 190 after Unity MCP/editor approval is available. | [ ] |
| P1 | Run Unity backlog item 191 after Unity MCP/editor approval is available. | [ ] |
| P1 | Run Unity backlog item 192 after Unity MCP/editor approval is available. | [ ] |
| P1 | Run Unity backlog item 193 after Unity MCP/editor approval is available. | [ ] |
| P1 | Decide overlay-vs-bed-layer presentation for owner sleep-state frames in a real battle scene. | [ ] |
| P1 | Keep starter-cat body replacements blocked until active-cat screenshots are explicitly approved against the colored three-view turnarounds. | [ ] |
| P1 | Run the Batch 75 Chinese UI scale screenshot matrix before final UI visual acceptance. | [ ] |
| P1 | Run Batch 82 common UI state candidates through actual-screen priority, contrast, click-target, and Unity import review before promoting or expanding them. | [ ] |
| P1 | Run Batch 83 loading/start preflight through Unity screenshots, spinner interpretation, placeholder text/state, and 1024x768 crowding checks. | [ ] |
| P1 | Run Batch 84 result/settlement preflight through Unity victory/defeat/settlement screenshots, text replacement, and 1024x768 run-failed crowding checks. | [ ] |
| P1 | Run Batch 85 settings/pause preflight through Unity settings/pause screenshots, text replacement, click-target checks, and 1024x768 key-hint semantics. | [ ] |
| P1 | Run Batch 86 dream-route preflight through Unity dream-entry/route-map screenshots, text replacement, node/path semantics, click-target checks, boss gate scale, and 1024x768 route-card density. | [ ] |
| P1 | Run Batch 88 character-select preflight through Unity character-select screenshots, source-lock avatar consistency, text replacement, low-height density, card/start click targets, import settings, binding, and Console. | [ ] |
| P1 | Run Batch 87 battle HUD preflight through Unity battle screenshots, four-gauge proof, text/number replacement, skill-state readability, enemy telegraph occlusion, click-target checks, import settings, binding, and Console. | [ ] |
| P1 | Build a screen-level `ui_skill_selection_preflight` packet; do not count character select or skill slot components as this screen. | [x] |
| P1 | Build a screen-level `ui_cat_room_preflight` packet using textless interaction prompts and existing cat-room prop affordances; do not generate new cat body art. | [x] |
| P1 | Keep Batch 90 candidate import/binding checks open; `02-cat-room.png` proves runtime surface reachability only. | [ ] |
| P2 | If Batch 76 fails runtime scale review, regenerate a smaller overlay source with more internal spacing before import. | [ ] |

## 2026-06-25 Systematic Source / Agent Review Addendum

| Item | State | Evidence |
| --- | --- | --- |
| UI/style source truth rechecked | [x] | `Qr1XdXd6KosnjMxjgW7cS89kn9c` live outline and section 9 fetch passed as `personal` user `章航宁` at 08:03 +08:00, revision `816`. |
| Gameplay source live access | [ ] | `MDrSdEoaToB5cnxZgrOcAE34nof` returned Feishu API `3380004` no permission under refreshed valid `personal` token at 08:03 +08:00; local synced markdown remains the working copy. |
| Character source live access | [ ] | `IAdkdcpciobUTXxa7dBcRx7Bngf` returned Feishu API `3380004` no permission under both user and bot identities at 08:03 +08:00; local source-lock packets remain the working authority. |
| Additional doc access | [ ] | `IZpFdIwtboEzzrx4ZFlcZLD2npe` and `HDoWdDNR3oZ6uhxuMzPcT8qCn5f` returned `3380004` no permission under refreshed valid `personal` token at 08:03 +08:00. |
| Drive folder access | [ ] | `FoW9fKYcDllwJjdTxGHcu4pbnab` inspect succeeds, but read-only `drive +status --quick` returned Drive API `1061004 forbidden` at 08:03 +08:00. |
| Feishu access recheck | [x] | `FEISHU_SOURCE_ACCESS_RECHECK_2026-06-25.md` now records valid `personal` user token, readable Qr1 revision `816`, remaining `3380004` doc ACL blockers, and folder `1061004` list blocker. |
| Unity MCP local recheck | [x] | Local Unity AI Assistant package, relay, Codex config, and approved registry records exist; this thread has no callable Unity MCP tools exposed, so runtime screenshots/Console remain pending. |
| UI/style review agent | [x] | Found 84 UI PNGs, strong installed/candidate coverage, incomplete runtime screenshots, and 5 reward-detail badge metas still at Default Texture. |
| Character review agent | [x] | Reconfirmed starter-cat body lock; P0-adjacent cats need source-lock packets before body art/import; symbolic candidates only. |
| Scene/enemy/VFX review agent | [x] | Reconfirmed bedroom/enemy/VFX file presence but scene/prefab binding gap; enemy framesheets needed import/slicing policy. |
| Enemy framesheet visual review agent | [x] | Passed source consistency, four frames per lane, transparent margins, and no starter-cat leakage; left Unity pivot/playback/hitbox and color-drift TODOs. |
| Enemy framesheet production QA agent | [x] | Passed source/split Sprite metas, validator scope, matrix boundary, and no starter-cat lane touch after fixed packet-date reproducibility. |
| Reward detail badge import fix | [x] | Five `Assets/TheCat/Art/UI/Badges/*reward_detail*.png.meta` files changed to Sprite texture type with alpha transparency. |
| UI common component inventory | [x] | `P0_UI_COMMON_COMPONENT_INVENTORY_2026-06-25.md` records 17 rows: 8 installed pending Unity evidence, 9 candidate-only/import-test, 0 missing design-needed after Batch 82. |
| UI common component review agents | [x] | `UI_COMMON_COMPONENT_INVENTORY_AGENT_REVIEW_2026-06-25.md` records visual/style PASS and production QA PASS after cwd-independent matrix and all-evidence path validation fixes. |
| Batch 82 common UI state candidates | [x] | `BATCH82_COMMON_UI_STATE_AGENT_REVIEW_2026-06-25.md` records 25 derivative textless candidate sprites, initial visual P1 findings, fixes, follow-up PASS, and remaining Unity gates. |
| Batch 83 loading/start preflight | [x] | `BATCH83_LOADING_START_AGENT_REVIEW_2026-06-25.md` records 8 local preflight rows, visual PASS_WITH_P2, production QA P1 fix, follow-up PASS, and remaining Unity gates. |
| Batch 83 Unity queue coverage | [x] | `P0AssetProductionQueueCatalog` records `p0_asset_queue_loading_start_preflight_candidates`; `P0_ASSET_UNITY_VALIDATION_CHECKLIST.md` includes Batch 83 coverage. |
| Batch 84 result/settlement preflight | [x] | `BATCH84_RESULT_SETTLEMENT_AGENT_REVIEW_2026-06-25.md` records 11 local preflight rows, visual PASS_WITH_P2 after failure-stamp fix, production QA PASS after sheet hash lock, and remaining Unity gates. |
| Batch 84 Unity queue coverage | [x] | `P0AssetProductionQueueCatalog` records `p0_asset_queue_result_settlement_preflight_candidates`; Batch 84 coverage is retained in the current Unity checklist. |
| Batch 85 settings/pause preflight | [x] | `BATCH85_SETTINGS_PAUSE_AGENT_REVIEW_2026-06-25.md` records 10 local preflight rows, visual PASS_WITH_P2 with 1024x768 key-hint semantics watch, production QA PASS, and remaining Unity gates. |
| Batch 85 Unity queue coverage | [x] | `P0AssetProductionQueueCatalog` records `p0_asset_queue_settings_pause_preflight_candidates`; the latest `P0_ASSET_UNITY_VALIDATION_CHECKLIST.md` keeps Batch 85 coverage inside the 19-item / 14-candidate queue. |
| Batch 86 dream-route preflight | [x] | `BATCH86_DREAM_ROUTE_AGENT_REVIEW_2026-06-25.md` records 10 local preflight rows, visual PASS_WITH_P2 with compact density and boss-gate watches, production QA PASS_WITH_P2, manifest provenance hash fix, and remaining Unity gates. |
| Batch 86 Unity queue coverage | [x] | `P0AssetProductionQueueCatalog` records `p0_asset_queue_dream_route_preflight_candidates`; Batch 86 remains covered in the current 19-item / 14-candidate Unity checklist. |
| Batch 87 battle HUD preflight | [x] | `BATCH87_BATTLE_HUD_AGENT_REVIEW_2026-06-25.md` records 10 local preflight rows, production QA PASS, initial visual P1 for missing 1024x768 fourth gauge, builder fix, follow-up visual PASS, and remaining Unity gates. |
| Batch 87 Unity queue coverage | [x] | `P0AssetProductionQueueCatalog` records `p0_asset_queue_battle_hud_preflight_candidates`; `P0_ASSET_UNITY_VALIDATION_CHECKLIST.md` now shows 19 queue items, 14 candidate-review items, and Batch 87 coverage. |
| Batch 88 character-select preflight | [x] | `BATCH88_CHARACTER_SELECT_AGENT_REVIEW_2026-06-25.md` records 10 local preflight rows, visual PASS_WITH_P2 for low-height density, production QA PASS_WITH_P2, provenance P2 fix, validator pass, and remaining Unity gates. |
| Batch 88 Unity queue coverage | [x] | `P0AssetProductionQueueCatalog` records `p0_asset_queue_character_select_preflight_candidates`; `P0_ASSET_UNITY_VALIDATION_CHECKLIST.md` now shows 19 queue items, 14 candidate-review items, and Batch 88 coverage. |
| Batch 89 skill-selection preflight | [x] | `BATCH89_SKILL_SELECTION_AGENT_REVIEW_2026-06-25.md` records 12 local preflight rows, visual PASS_WITH_P2 with density and state-semantics watches, production QA PASS, validator pass, and remaining Unity gates. |
| Batch 89 Unity queue coverage | [x] | `P0AssetProductionQueueCatalog` records `p0_asset_queue_skill_selection_preflight_candidates`; `P0_ASSET_UNITY_VALIDATION_CHECKLIST.md` now shows 19 queue items, 14 candidate-review items, and Batch 89 coverage. |
| Batch 90 cat-room preflight | [x] | `BATCH90_CAT_ROOM_AGENT_REVIEW_2026-06-25.md` records 10 local preflight rows, visual PASS_WITH_P2 with dream-entry and disabled-state semantics watches, production QA PASS, validator pass, and remaining Unity gates. |
| Batch 90 Unity queue coverage | [x] | `P0AssetProductionQueueCatalog` records `p0_asset_queue_cat_room_preflight_candidates`; `P0_ASSET_UNITY_VALIDATION_CHECKLIST.md` now shows 19 queue items, 14 candidate-review items, and Batch 90 coverage. |
| Total production design and required asset table | [x] | `P0_SYSTEMATIC_ART_PRODUCTION_TOTAL_DESIGN_2026-06-25.md` records the current source status, required asset table, production order, imagegen policy, archive rules, review agents, and TODO state. |
| Play Mode 11-capture evidence | [x] | `P0_PLAYMODE_ACCEPTANCE_SMOKE_REPORT.md` records 11/11 screenshot evidence, including `02-cat-room.png`, active-cat screenshots `05` through `07`, route flow 10/10, and defeat flow. |
| Cat-room runtime surface evidence | [x] | `02-cat-room.png` and the Play Mode smoke verify the existing `P0CatRoom` surface. This is not Batch 90 sprite import, binding, or interaction-state acceptance. |
| Unity offline acceptance current report | [x] | `P0_OFFLINE_ACCEPTANCE_REPORT.md` currently reports 6/6 gates passed, 0 failures, starter-cat strict candidates ready 3/3, formal import state `Blocked`, and active screenshots 3/3. |
| Unity batchmode offline attempt | [x] | `UNITY_BATCHMODE_OFFLINE_ATTEMPT_2026-06-25.md` records the stale 07:44 report, the framesheet Sprite/v2-marker fixes, the starter-cat blocked-with-screenshots gate fix, and the later zero-failure Unity batchmode pass. |
| Enemy framesheet import gate code fix | [x] | `P0AssetImportSettingsValidator` and `P0AssetMetaImportSettingsReadiness` now treat `framesheet` assets as Sprite Single, matching `ENEMY_FRAMESHEET_IMPORT_POLICY_2026-06-25.md`. |
| Starter-cat formal gate code fix | [x] | `P0StarterCatFormalImportReadiness` now keeps blocked review notes valid and blocked even when active screenshots exist; screenshots alone still do not approve import. |

## 2026-06-25 Master TODO

| Priority | Task | Status |
| --- | --- | --- |
| P0 | Use `design/development/asset_review/P0_SYSTEMATIC_ART_ASSET_MASTER_LEDGER_2026-06-25.md` as the current art-production control ledger. | [x] |
| P0 | Use `design/development/asset_review/P0_SYSTEMATIC_ART_ASSET_REQUIREMENTS_2026-06-25.csv` as the script-readable master asset requirement table. | [x] |
| P0 | Keep starter-cat body and framesheet generation blocked until registered active-cat screenshots receive explicit colored-turnaround comparison approval. | [x] |
| P0 | Validate current non-cat candidate packs before opening another imagegen batch. | [x] |
| P0 | Add explicit master rows or candidate plan for per-starter symbolic skill icons. | [x] |
| P0 | Integrate Batch 80 UI/style and character-consistency review agents. | [x] |
| P0 | Integrate Batch 80 production-pipeline QA agent. | [ ] blocked: agent timed out twice and was closed |
| P0 | Build Batch 80 64/32 px HUD readability board. | [x] |
| P0 | Generate Batch 80 lighter-frame comparison variant. | [x] |
| P0 | Review whether Batch 80 v003 lightframe should replace any accepted v001/v002 icons. | [x] |
| P0 | Build Batch 80 recommended mixed candidate set. | [x] |
| P0 | Generate Batch 80 Battle HUD skill frame/cooldown overlay test board. | [x] |
| P0 | Integrate Batch 80 Battle HUD overlay review agents. | [x] |
| P0 | Generate Batch 81 square/round skill slot frame candidates. | [x] |
| P0 | Integrate Batch 81 skill slot frame review agents. | [x] |
| P0 | Create square-only Batch 81 import-test plan with cooldown digits `1`, `12`, `99`. | [x] |
| P0 | Generate Batch 81 v002_light square-only slot variant and QA boards. | [x] |
| P0 | Integrate Batch 81 v002_light visual/production QA agents. | [x] |
| P0 | Build and validate Batch 81 v002_light local actual-scale HUD mockup pack. | [x] |
| P0 | Integrate Batch 81 v002_light local actual-scale HUD mockup review agents. | [x] |
| P0 | Build and validate Batch 75 local Chinese UI scale preflight mockup pack. | [x] |
| P0 | Integrate Batch 75 local Chinese UI scale preflight review agents. | [x] |
| P0 | Resolve enemy framesheet local import settings and slicing policy. | [x] |
| P0 | Build and validate UI common component inventory. | [x] |
| P0 | Integrate UI common component inventory review agents and fix script P1 findings. | [x] |
| P0 | Produce Batch 82 derivative common UI state candidate sprites for the prior uncovered component rows. | [x] |
| P0 | Integrate Batch 82 visual and production QA reviews and fix P1 findings. | [x] |
| P0 | Build and validate Batch 83 loading/start local preflight candidate packet. | [x] |
| P0 | Integrate Batch 83 visual and production QA reviews and fix P1 findings. | [x] |
| P0 | Add Batch 83 loading/start preflight to the P0 asset production queue and Unity validation checklist without promoting it into `Assets`. | [x] |
| P0 | Build and validate Batch 84 result/settlement local preflight candidate packet. | [x] |
| P0 | Integrate Batch 84 visual and production QA reviews and fix P2 findings. | [x] |
| P0 | Add Batch 84 result/settlement preflight to the P0 asset production queue and Unity validation checklist without promoting it into `Assets`. | [x] |
| P0 | Build and validate Batch 85 settings/pause local preflight candidate packet. | [x] |
| P0 | Integrate Batch 85 visual and production QA reviews and carry the P2 key-hint semantics watch into Unity gates. | [x] |
| P0 | Add Batch 85 settings/pause preflight to the P0 asset production queue and Unity validation checklist without promoting it into `Assets`. | [x] |
| P0 | Build and validate Batch 86 dream-route local preflight candidate packet. | [x] |
| P0 | Integrate Batch 86 visual and production QA reviews and carry compact density, boss-gate scale, and provenance-hash findings into gates. | [x] |
| P0 | Add Batch 86 dream-route preflight to the P0 asset production queue and Unity validation checklist without promoting it into `Assets`. | [x] |
| P0 | Build and validate Batch 87 battle HUD local preflight candidate packet. | [x] |
| P0 | Integrate Batch 87 battle HUD production QA and visual reviews, including the 1024x768 four-gauge P1 fix. | [x] |
| P0 | Add Batch 87 battle HUD preflight to the P0 asset production queue and Unity validation checklist without promoting it into `Assets`. | [x] |
| P0 | Build and validate Batch 88 character-select local preflight candidate packet. | [x] |
| P0 | Integrate Batch 88 character-select production QA and visual reviews, including the provenance P2 fix and low-height density watch. | [x] |
| P0 | Add Batch 88 character-select preflight to the P0 asset production queue and Unity validation checklist without promoting it into `Assets`. | [x] |
| P0 | Build and validate Batch 89 skill-selection local preflight candidate packet. | [x] |
| P0 | Integrate Batch 89 skill-selection production QA and visual reviews, including density and state-semantics watches. | [x] |
| P0 | Add Batch 89 skill-selection preflight to the P0 asset production queue and Unity validation checklist without promoting it into `Assets`. | [x] |
| P0 | Build and validate Batch 90 cat-room local preflight candidate packet. | [x] |
| P0 | Integrate Batch 90 cat-room production QA and visual reviews, including dream-entry and disabled-state semantics watches. | [x] |
| P0 | Add Batch 90 cat-room preflight to the P0 asset production queue and Unity validation checklist without promoting it into `Assets`. | [x] |
| P0 | Repair stale Unity offline gates for enemy framesheet Sprite policy and blocked-with-screenshots starter-cat formal import state. | [x] |
| P0 | Unity offline acceptance rerun produced a fresh zero-failure report after Unity batchmode/license execution stabilized. | [x] |
| P0 | Repair Batch 47/49/50/51 strict candidate validators, manifests, prompts, and evidence hashes after 11-capture screenshot renumbering. | [x] |
| P0 | Run Play Mode 11-capture smoke including cat-room surface capture `02-cat-room.png`. | [x] |
| P0 | Produce visible built-in imagegen Batch91 cat-room map-elements source sheet, alpha sheet, contact sheet, manifest, and 12 independent cutout PNGs. | [x] |
| P0 | Dispatch independent Batch91 visual/style and production QA review agents before semantic naming or Unity import. | [x] |
| P0 | Integrate Batch91 review results and create semantic review table for accepted `prop_01` through `prop_12`. | [x] |
| P0 | Regenerate or clean Batch91 dream entrance portal so `prop_04` can be replaced by an accepted portal sprite. | [x] |
| P0 | Build Batch91 12-sprite semantic contact sheet and semantic manifest for Unity import review. | [x] |
| P0 | Produce visible built-in imagegen Batch92 cat-room interaction-state source sheet, alpha sheet, semantic sprites, final contact sheet, manifest, and review CSV. | [x] |
| P0 | Dispatch independent Batch92 visual/style, production QA, and cooldown P1-fix review agents before Unity import review. | [x] |
| P0 | Integrate Batch92 review results and replace rejected cooldown v001 with accepted cooldown v003. | [x] |
| P0 | Run Unity import/settings/binding review for Batch91 cat-room map elements, including portal scale/pivot and interaction-ring curation. | [ ] |
| P0 | Run Unity import/settings/binding review for Batch92 cat-room interaction states, including cooldown v003, ring semantics, marker scale, and locator classification. | [ ] |
| P0 | Produce visible built-in imagegen Batch93 cat-room map decal/trim source sheet, alpha sheet, semantic sprites, contact sheet, manifest, process note, and final review CSV. | [x] |
| P0 | Dispatch independent Batch93 visual/style and production QA review agents and integrate PASS_WITH_P2 findings. | [x] |
| P0 | Run Unity import/settings/binding review for Batch93 cat-room map decals, including floor contrast, sorting layers, pivot/baseline choices, and no-collider policy. | [ ] |
| P0 | Produce Batch94 cat-room dream portal 12-frame animation sequence and close visual/production P1 findings with v004. | [x] |
| P0 | Run Unity import/settings/binding review for Batch94 dream portal animation, including loop jitter, scale/pivot, sorting, and static-hitbox reuse. | [ ] |
| P0 | Run Unity active enemy animation import/screenshots for split sprites, including Call Tyrant f04 pivot/playback/hitbox and color-drift checks. | [ ] |
| P0 | Run the Batch 75 Chinese UI scale screenshot matrix in Unity. | [ ] |
| P0 | Capture remaining per-batch Unity screen matrices and Console checks for loading/start, result/settlement, settings/pause, dream-route, skill-selection, battle HUD, and Batch 90 candidate states; cat-room runtime surface screenshot exists as `02-cat-room.png`. | [ ] |
| P1 | Request Feishu access to `MDr...`, `IAd...`, `IZp...`, `HDo...`, and folder `FoW9...`, then refresh this ledger. | [ ] |
| P1 | Upgrade `lark-cli` from `1.0.53` to `1.0.57` after current asset pass if approved. | [ ] |

## 2026-06-25 Non-Cat Validation Matrix Addendum

| Item | State | Evidence |
| --- | --- | --- |
| Matrix script | [x] | `design/development/tools/run_p0_noncat_candidate_validation_matrix.ps1` |
| Matrix report | [x] | `design/development/asset_review/P0_NONCAT_CANDIDATE_VALIDATION_MATRIX_2026-06-25.md` and `.csv` |
| Local validator pass count | [x] | 38 non-cat/symbolic validators passed, 0 failed after adding the Batch 89 skill-selection and Batch 90 cat-room preflight validators. |
| Enemy framesheet local import policy | [x] | `ENEMY_FRAMESHEET_IMPORT_POLICY_2026-06-25.md`, `ENEMY_FRAMESHEET_SLICED_SPRITES_2026-06-25.csv`, contact sheet, validator, and agent review are in place. |
| UI common component inventory | [x] | `P0_UI_COMMON_COMPONENT_INVENTORY_2026-06-25.md`, `.csv`, validator, and agent review are in place. |
| Batch 82 common UI state candidates | [x] | `thecat_ui_common_states_batch82_manifest.csv`, contact/review sheets, validator, and `BATCH82_COMMON_UI_STATE_AGENT_REVIEW_2026-06-25.md` are in place. |
| Batch 83 loading/start preflight | [x] | `thecat_ui_loading_start_batch83_manifest.csv`, contact/review sheets, validator, and `BATCH83_LOADING_START_AGENT_REVIEW_2026-06-25.md` are in place. |
| Batch 84 result/settlement preflight | [x] | `thecat_ui_result_settlement_batch84_manifest.csv`, contact/review sheets, validator, and `BATCH84_RESULT_SETTLEMENT_AGENT_REVIEW_2026-06-25.md` are in place. |
| Batch 85 settings/pause preflight | [x] | `thecat_ui_settings_pause_batch85_manifest.csv`, contact/review sheets, validator, and `BATCH85_SETTINGS_PAUSE_AGENT_REVIEW_2026-06-25.md` are in place. |
| Batch 86 dream-route preflight | [x] | `thecat_ui_dream_route_batch86_manifest.csv`, contact/review sheets, validator, and `BATCH86_DREAM_ROUTE_AGENT_REVIEW_2026-06-25.md` are in place. |
| Batch 87 battle HUD preflight | [x] | `thecat_ui_battle_hud_batch87_manifest.csv`, contact/review sheets, validator, and `BATCH87_BATTLE_HUD_AGENT_REVIEW_2026-06-25.md` are in place. |
| Batch 88 character-select preflight | [x] | `thecat_ui_character_select_batch88_manifest.csv`, contact/review sheets, validator, and `BATCH88_CHARACTER_SELECT_AGENT_REVIEW_2026-06-25.md` are in place. |
| Batch 89 skill-selection preflight | [x] | `thecat_ui_skill_selection_batch89_manifest.csv`, contact/review sheets, validator, and `BATCH89_SKILL_SELECTION_AGENT_REVIEW_2026-06-25.md` are in place. |
| Batch 90 cat-room preflight | [x] | `thecat_ui_cat_room_batch90_manifest.csv`, contact/review sheets, validator, and `BATCH90_CAT_ROOM_AGENT_REVIEW_2026-06-25.md` are in place. |
| Feishu live access recheck | [x] | `FEISHU_SOURCE_ACCESS_RECHECK_2026-06-25.md` confirms token validity and distinguishes ACL blockers from OAuth state. |
| Stale install validator count fixed | [x] | Batch 60 and Batch 61 install validators now check for `P0ManifestAssetCount =` plus lane tokens instead of stale `= 115`. |
| Unity offline gate fix | [x] | Runtime/editor import readiness, v2 import marker compatibility, and starter-cat formal import gate code were updated after `UNITY_BATCHMODE_OFFLINE_ATTEMPT_2026-06-25.md`; the current offline report passes 6/6 gates with 0 failures. |
| Runtime acceptance | [ ] | Play Mode smoke passes 11/11 screenshot evidence and captures `02-cat-room.png`, but per-lane import settings, Console checks, Batch90 candidate binding proof, active enemy animation review, and formal install decisions remain pending. |
| Independent matrix audit | [x] | Matrix scope is curated and non-exhaustive; do not read the current 38/38 pass as whole-project or Unity acceptance. |
| Next Unity evidence queue | [x] | `design/development/asset_review/P0_ASSET_UNITY_VALIDATION_CHECKLIST.md` includes Batch 83, Batch 84, Batch 85, Batch 86, Batch 87, Batch 88, Batch 89, and Batch 90; `design/development/asset_review/BATCH81_V002_LIGHT_UNITY_EVIDENCE_QUEUE.md` remains the skill-slot focused queue. |

## Batch 80 Checklist

| Item | State | Evidence |
| --- | --- | --- |
| Safe non-body lane selected | [x] | Starter skill icon motifs; no cat bodies, portraits, or runtime replacements. |
| Built-in imagegen source generated | [x] | `source/thecat_ui_starter_skill_icon_motifs_batch80_chromakey_source_v001.png` |
| Chroma-key alpha sheet generated | [x] | `source/thecat_ui_starter_skill_icon_motifs_batch80_alpha_sheet_v001.png` |
| 72 v001 independent icons generated | [x] | `icons_256`, `icons_128`, `icons_64`, `icons_32` |
| Two rejected cells revised as v002 | [x] | `thecat_ui_skill_saiban_battle_flag_rally_*_candidate_v002.png`; `thecat_ui_skill_suzune_team_heal_ice_enchant_*_candidate_v002.png` |
| Full lightframe variant generated | [x] | `icons_lightframe_256`, `icons_lightframe_128`, `icons_lightframe_64`, `icons_lightframe_32` |
| Contact sheet generated | [x] | `icons/thecat_ui_starter_skill_icon_motifs_batch80_contact_sheet_v001.png` |
| Manifest and review docs written | [x] | Batch 80 candidate directory |
| Validator passed | [x] | `Starter skill icon motif Batch 80 validation passed` |
| Independent visual review dispatched | [x] | Agents: UI/style, character source consistency, production pipeline QA |
| UI/style and character visual review integrated | [x] | 16 v001 motifs retained as candidates; 2 v001 motifs rejected and regenerated as v002. |
| Production-pipeline review integrated | [ ] | Agent timed out twice and was closed; local validator and py_compile passed. |
| HUD readability board | [x] | `icons/thecat_ui_starter_skill_icon_motifs_batch80_hud_readability_board_v001.png` |
| Heavy-frame decision | [ ] | 32px board suggests possible frame-over-subject risk; needs UI owner/Unity HUD decision. |
| Lightframe comparison board | [x] | `icons/thecat_ui_starter_skill_icon_motifs_batch80_lightframe_comparison_board_v001.png` |
| Lightframe replacement decision | [x] | Switch 4 icons to v003 lightframe; keep 14 current v001/v002. |
| Recommended mixed set generated | [x] | `recommended/thecat_ui_starter_skill_icon_motifs_batch80_recommended_manifest.csv` |
| Battle HUD/cooldown overlay test | [x] | `hud_overlay_test/thecat_ui_starter_skill_icon_motifs_batch80_battle_hud_overlay_board_v001.png` |
| Battle HUD overlay review | [x] | Recommended set may enter Unity import testing only; horizontal frame blocks formal promotion. |
| Unity import accepted | [ ] | Candidate-only until review and runtime evidence pass |

## Batch 81 Checklist

| Item | State | Evidence |
| --- | --- | --- |
| Safe UI component lane selected | [x] | Skill slot frame candidates; no character/body art. |
| Built-in imagegen source generated | [x] | `source/thecat_ui_skill_slot_frames_batch81_chromakey_source_v001.png` |
| Chroma-key alpha sheet generated | [x] | `source/thecat_ui_skill_slot_frames_batch81_alpha_sheet_v001.png` |
| 24 slot frames generated | [x] | `frames_256`, `frames_128`, `frames_64` across square/round ready/cooldown/disabled/selected. |
| Batch 80 icon-fit board generated | [x] | `icon_fit_test/thecat_ui_skill_slot_frames_batch81_icon_fit_board_v001.png` |
| Manifest and review docs written | [x] | Batch 81 candidate directory |
| Validator passed | [x] | `Skill slot frame Batch 81 validation passed` |
| Independent visual review dispatched | [x] | Visual review and production QA integrated. |
| Square slot import-test direction accepted | [x] | Use square ready/cooldown/disabled/selected only for import testing; round is backup/reference. |
| Square import-test plan written | [x] | `design/development/asset_review/BATCH80_81_SQUARE_SKILL_SLOT_IMPORT_TEST_PLAN.md` |
| Cooldown digit mockup | [x] | `cooldown_digit_test/thecat_ui_skill_slot_frames_batch81_square_cooldown_digit_board_v001.png` |
| Cooldown digit readability review | [x] | Digits `1/12/99` are readable enough for import-test planning; `99` remains a watch item. |
| V002 light square source | [x] | `source/thecat_ui_skill_slot_frames_batch81_chromakey_source_v002_square_light.png`; `source/thecat_ui_skill_slot_frames_batch81_alpha_sheet_v002_square_light.png` |
| V002 light square frames | [x] | `frames/frames_square_v002_light_256`, `frames_square_v002_light_128`, `frames_square_v002_light_64`; 12 PNGs total. |
| V002 light QA boards | [x] | `icon_fit_test_v002_light/thecat_ui_skill_slot_frames_batch81_icon_fit_board_v002_light.png`; `cooldown_digit_test_v002_light/thecat_ui_skill_slot_frames_batch81_square_cooldown_digit_board_v002_light.png` |
| V002 light actual-scale HUD mockup | [x] | `actual_scale_hud_test_v002_light/` with 72 local 64 px composites and 6 HUD boards. |
| V002 light actual-scale HUD mockup review | [x] | Visual and engineering QA agents found no local P0 blocker; `selected` vs `ready` and cooldown `99` remain Unity HUD watch items. |
| V002 light independent review | [x] | Preferred over v001 for square import-test planning; watch selected-vs-ready clarity and cooldown `99`. |
| Unity import accepted | [ ] | Candidate-only until visual review and runtime evidence pass |
