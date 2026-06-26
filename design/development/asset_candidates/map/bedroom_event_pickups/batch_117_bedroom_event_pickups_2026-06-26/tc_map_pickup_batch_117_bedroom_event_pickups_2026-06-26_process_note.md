# bedroom_event_pickups Process Note

Batch: `batch_117_bedroom_event_pickups_2026-06-26`
Family: `map`
Source truth: `Qr1 UI/style truth revision 816; Qr1 P0 bedroom dream map boundary; Batch101/108/109 bedroom-map context; Batch104 reward-token context; no IAd character-body claim; no HDo/FoW9 map-archive claim`

Process: built-in Codex imagegen via the `imagegen` skill, workspace source copy, local chroma-key alpha removal, 3x3 projection split, v002 single-asset rework for `story_page_clue_pickup`, review variants, contact sheets, and 96px/64px readability boards. The imagegen skill was used through the built-in Codex path. The built-in imagegen tool path is the user's requested Codex subscription path and does not require `OPENAI_API_KEY`; the current built-in tool does not expose a model selector, so this batch is recorded as built-in imagegen rather than a model-locked CLI/API run.

Prompt goal: static bedroom dream-map event pickup objects that complement Batch101 battle markers, Batch108 obstacle props, Batch109 entry/path props, Batch110 map overlay controls, and Batch104 reward/event token language without becoming character body art, animation, Egypt map archive coverage, or runtime replacement art.

## Source And Alpha Evidence

Initial 3x3 sheet:

- Built-in imagegen default output copied from `C:\Users\PC\.codex\generated_images\019efc23-3cc2-7f23-9beb-cb16d118a13f\ig_045a0e06c3d24823016a3db4f94888819aaa623d40cae62516.png`.
- Workspace source: `source/tc_map_pickup_batch117_chromakey_source_v001.png`.
- Source SHA256: `9628332a487826392c31e1ad5105fb44c8b93971b2fac65eeb39d70cc0ffd9cd`.
- Chroma-key helper: `C:\Users\PC\.codex\skills\.system\imagegen\scripts\remove_chroma_key.py`.
- Key color: `#fb02f9`.
- Transparent pixels: `1197158/1572516`.
- Partially transparent pixels: `72149/1572516`.
- Alpha sheet: `alpha/tc_map_pickup_batch117_alpha_sheet_v001.png`.
- Alpha sheet SHA256: `c5d5b6ed4daf965bd6485b9ce57d92853de52dba317b42d2d17d572dfa0cc869`.

Story page v002 rework:

- Reason: independent 64px readability review found a P1 on v001 because the page lost contrast on warm backgrounds and the dangling ornament read closer to key/ticket semantics.
- Workspace source: `source/tc_map_pickup_story_page_clue_pickup_v002_chromakey_source.png`.
- Source SHA256: `5812be2ee628e8b6530579237fbcc1271dde7dd89d28c09fffd94166f5185781`.
- Alpha: `alpha/tc_map_pickup_story_page_clue_pickup_v002_alpha.png`.
- Alpha SHA256: `2c0561b66bd4a2d8663d018c67d9c3a7edc83047621a3c59c419e966b6022de4`.
- Chroma-key result: key `#fb02f9`; transparent pixels `1021008/1572516`; partially transparent pixels `4051/1572516`.
- Active sprite: `semantic_sprites/tc_map_pickup_story_page_clue_pickup_batch117_candidate_v002.png`.
- Active sprite size: `942x1028`; SHA256 `b282bc6f732474463c98e38dc4a993a5c03b8ff31a9ffaaf4b5c34171e2f05b5`.
- Derivation: `single_v002_bbox_x198-1092_y114-1094_pad24`.
- Superseded v001 sprite and variants were moved to `superseded/story_page_v001/`.

## Split And Review Pack

- Split method for v001 sheet: `sprite_batch_tools.py split-sheet --rows 3 --cols 3 --pad 24`.
- Row bands: `(79,393)`, `(450,775)`, `(851,1152)`.
- Column bands: `(82,374)`, `(486,734)`, `(867,1143)`.
- Manifest: `tc_map_pickup_batch117_semantic_manifest.csv`; the active story-page row now points to `candidate_v002`.
- Active contact sheet: `tc_map_pickup_batch117_semantic_contact_sheet_v002.png`; SHA256 `8305fc8be2ea48dd2df1006fbfa23f22ede78fd527a8d045b955d7975dd72d54`.
- Active readability board: `tc_map_pickup_batch117_96px_64px_bedroom_event_pickup_readability_board_v002.png`; SHA256 `ce1121e3bdc1a8b8554e3d5885bd849e8318bbbe03f66c6367f071da957bd363`.
- Superseded contact/readability proof retained: `tc_map_pickup_batch117_semantic_contact_sheet_v001.png`; `tc_map_pickup_batch117_96px_64px_bedroom_event_pickup_readability_board_v001.png`.
- Active review variants: `45` PNGs and `9` manifests under `reviews/variants/`, including v002 story-page variants.

Rows: 9 semantic sprites:

- `fish_treat_pickup` `305x353`
- `dream_thread_spool_pickup` `296x335`
- `memory_shard_cluster_pickup` `267x351`
- `moon_ticket_pass_pickup` `340x298`
- `blessing_lantern_pickup` `232x373`
- `story_page_clue_pickup` `942x1028` v002
- `star_button_key_pickup` `259x344`
- `soft_light_orb_pickup` `289x335`
- `reroll_dice_charm_pickup` `321x295`

## Review Integration

- Visual/source-boundary review: `PASS_WITH_P2`; no character body/face/portrait/paw/costume/framesheet/animation content and no HDo/FoW9 archive claim.
- Production QA review on v001 package: `PASS_WITH_P2`; no `.meta`, no runtime `Assets` leak, hashes and alpha checks passed.
- Target-size readability review on v001: `PASS_WITH_P1`; `story_page_clue_pickup` required rework before unlabeled runtime use.
- Focused v002 story-page readability review: `PASS_WITH_P2`; v002 closes the P1 and has no blocking semantic collision with `moon_ticket_pass_pickup`, `star_button_key_pickup`, or `reroll_dice_charm_pickup`.
- Delta production QA on v002: `PASS_WITH_P2`; active manifest paths resolve, v002 variants exist, v001 is superseded only, and no runtime leakage was found.
- Final integrated review: `5 candidate_keep`, `4 candidate_conditional`, `0 pending_review`, `0 reject_rework`.

Final decisions:

- `candidate_keep`: `fish_treat_pickup`, `moon_ticket_pass_pickup`, `blessing_lantern_pickup`, `story_page_clue_pickup` v002, `star_button_key_pickup`.
- `candidate_conditional`: `dream_thread_spool_pickup`, `memory_shard_cluster_pickup`, `soft_light_orb_pickup`, `reroll_dice_charm_pickup`.

## Checklist

- [x] Asset table written before generation.
- [x] Raw source art copied into `source/`.
- [x] Background removed locally and alpha candidates stored outside runtime folders.
- [x] Manifest/contact sheet and review variants produced.
- [x] 96px/64px readability board produced.
- [x] Visual/style review completed.
- [x] Target-size readability review completed.
- [x] Production QA review completed.
- [x] v002 story-page P1 rework completed and independently reviewed.
- [ ] Runtime import remains blocked until import settings, binding proof, screenshots, and clean Console evidence pass.

Asset table: `design/development/asset_candidates/map/bedroom_event_pickups/batch_117_bedroom_event_pickups_2026-06-26/tc_map_pickup_batch_117_bedroom_event_pickups_2026-06-26_asset_table.csv`
Final review CSV: `design/development/asset_candidates/map/bedroom_event_pickups/batch_117_bedroom_event_pickups_2026-06-26/tc_map_pickup_batch_117_bedroom_event_pickups_2026-06-26_final_review.csv`

No runtime import was performed.
No Unity `.meta` files were intentionally created for this candidate folder.
Animation remains paused; this batch contains static map pickup/event objects only.
