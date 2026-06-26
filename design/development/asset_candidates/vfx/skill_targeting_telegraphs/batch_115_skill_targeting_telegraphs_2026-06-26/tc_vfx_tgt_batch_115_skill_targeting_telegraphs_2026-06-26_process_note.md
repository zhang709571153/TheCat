# Batch115 Skill Targeting Telegraphs Process Note

Batch: `batch_115_skill_targeting_telegraphs_2026-06-26`

Family: `vfx`

Source truth: `Qr1 UI/style truth revision 816; Qr1 P0 combat readability and battle HUD context; Batch81 skill slot frame context; Batch87 battle HUD preflight context; Batch105 combat feedback context; no IAd character-body claim; no HDo/FoW9 map archive claim`.

Process: built-in Codex `imagegen` source generation, workspace copy, local magenta chroma-key alpha removal, deterministic 3x3 projection split, contact sheet generation, and 128px/64px target-background readability board generation.

Image generation note: the user requested imagegen skill / image2. This pass used the built-in Codex imagegen tool, not an API key. The built-in tool does not expose a model selector in this environment, so the output is recorded as built-in imagegen rather than model-locked image2 provenance.

Source files:

- Generated source copy: `source/tc_vfx_tgt_batch115_chromakey_source_v001.png`
- Names file: `source/tc_vfx_tgt_batch115_names.txt`
- Alpha sheet: `alpha/tc_vfx_tgt_batch115_alpha_sheet_v001.png`
- Manifest: `tgt_batch115_semantic_manifest.csv`
- Contact sheet: `tgt_batch115_semantic_contact_sheet_v001.png`
- Readability board: `tgt_batch115_128px_64px_skill_targeting_readability_board_v001.png`

Hashes:

- Source sheet SHA-256: `d0066fdaf49ec8563f323a7db83e594e2ea6fa1825ed2e539ab4dab443b78657`
- Alpha sheet SHA-256: `f300a916a9da958e183e84d9bc4958633a016a7b6034345330c62df9287f32bf`
- Contact sheet SHA-256: `ffdf9ed52c1b3b11b720bfc59b071fcfb788c0ccb3349474d9ce37c708a7baa0`
- Readability board SHA-256: `f09825c75768639dffd608d0245a2794504b391c25fca3d26a0f07b1751ecc25`

Semantic order:

1. `skill_target_valid_ring`
2. `skill_target_invalid_cross`
3. `skill_aoe_circle_field`
4. `skill_aoe_cone_field`
5. `skill_line_target_strip`
6. `skill_chain_link_path`
7. `skill_shield_zone_floor`
8. `skill_heal_zone_floor`
9. `skill_summon_slot_rune`

Chroma-key and cut results:

- Key removal used auto-sampled magenta (`#f409f5`) with soft matte, despill, and `edge-contract 1`.
- Transparent pixels: `1149182/1572516`.
- Partially transparent pixels: `211108/1572516`.
- Initial split attempt used the full scaffold batch id in sprite filenames and hit Windows path-length failure after the first sprite. The partial output was removed, and the final split uses shortened sprite filenames with prefix `tgt` and batch id `batch115`.
- Final split wrote 9 transparent semantic sprites under `semantic_sprites/`.
- Every manifest row reports `alpha_extrema` `(0, 255)`.

Scope and safety:

- This batch is static skill targeting telegraph/map overlay art only.
- No character body, face, portrait, costume, animation frame, framesheet, enemy body, map background, or runtime replacement was generated.
- No IAd character source approval is claimed.
- No HDo/FoW9 map archive coverage is claimed.
- No runtime import was performed.
- No candidate file was copied into `Assets/`.

Review state:

- Visual/source-boundary review: Hypatia `PASS_WITH_P2`.
- 128px/64px readability review: Curie `PASS_WITH_P2`.
- Production QA review: Beauvoir `PASS_WITH_P2`.
- Final review CSV: 5 `candidate_keep`, 4 `candidate_conditional`, 0 rejected.
- Final review plain summary: 5 candidate_keep, 4 candidate_conditional, 0 rejected.

Runtime gate:

Batch115 remains `candidate_only_pending_unity_review`. Required gates before runtime promotion: import settings, target runtime path decision, skill-cast battle screenshot, live target-background readability, binding proof, no recursive candidate import, and clean Console.
