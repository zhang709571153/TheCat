# Batch 80 Starter Skill Icon Motifs Process Note

Date: 2026-06-25
Lane: `starter_cat_skill_icons`
Status: `candidate_complete_pending_visual_review`

## Scope

Batch 80 creates symbolic, textless skill icon motif candidates for the three
P0 starter cats. This is intentionally not body art and not a runtime import.

Output:

- 1 chroma-key source sheet.
- 1 alpha source sheet.
- 18 icon motifs.
- 4 sizes per motif: 256, 128, 64, 32.
- 72 individual transparent PNG candidates.
- 2 v002 replacement motifs after independent review.
- 8 v002 replacement PNG candidates.
- 18 v003 lightframe motifs.
- 72 v003 lightframe PNG candidates.
- 18 recommended final-candidate motifs copied into a unified candidate set.
- 72 recommended final-candidate PNGs.
- 18 ready-frame HUD test composites at 128 px.
- 18 cooldown-overlay HUD test composites at 64 px.
- 1 contact sheet.
- 1 CSV manifest.

## Source Truth

| Area | Authority |
| --- | --- |
| UI style | `Qr1XdXd6KosnjMxjgW7cS89kn9c`, live outline revision `816`, plus local synced P0 design docs. |
| Character motifs | Local source-lock basis from `IAdkdcpciobUTXxa7dBcRx7Bngf`; live fetch is blocked by Feishu `3380004`. |
| Character bodies | Not in scope. No starter-cat body, portrait, or runtime sprite is generated or imported. |

## Generation Path

1. Used built-in `image_gen` because this shell has no `OPENAI_API_KEY`; strict
   `gpt-image-2` CLI generation is blocked until the key is set.
2. Prompt requested a 3 x 6 chroma-key skill icon sheet, no text, no cat bodies,
   no humans, no watermark.
3. Copied the generated source from Codex generated-images storage into this
   candidate folder.
4. Ran the imagegen skill `remove_chroma_key.py` helper with `#00ff00`.
5. Ran `design/development/tools/split_batch80_starter_skill_icons.py` to
   split the alpha sheet into 72 independent transparent PNGs.
6. Independent review rejected two cells: Saiban battle flag rally and Suzune
   team heal ice enchant.
7. Generated a focused v002 two-cell sheet and split it with
   `design/development/tools/split_batch80_revision_icons.py`.
8. Generated a full v003 lightframe sheet to test whether a lighter HUD frame
   solves the 32px readability risk, then split it with
   `design/development/tools/split_batch80_lightframe_icons.py`.
9. Integrated two focused review agents and built a recommended mixed set with
   `design/development/tools/build_batch80_recommended_set.py`.
10. Built Battle HUD frame/cooldown overlay composites with
    `design/development/tools/build_batch80_battle_hud_overlay_test.py`, using
    the actual installed `thecat_ui_skill_ready_frame_512_v001.png` and
    `thecat_ui_skill_cooldown_overlay_512_v001.png`.

## Candidate Boundary

These files must stay in `design/development/asset_candidates/...` until they
pass visual review, import settings review, runtime HUD readability, and Console
checks. No Unity `.meta` files should exist in this batch folder.

## v002 Revisions

| Asset | v001 issue | v002 correction |
| --- | --- | --- |
| `thecat_ui_skill_saiban_battle_flag_rally_*` | Crescent flag and crowd silhouettes created cultural/identity drift. | Torn red knightly pennant with shield, sword, crown, and knotwork; no crescent and no people. |
| `thecat_ui_skill_suzune_team_heal_ice_enchant_*` | Humanoid silhouettes and medical-cross reading. | Bell pulse, paw sigils, talismans, snowflake ring; no people and no medical cross. |

## v003 Lightframe Variant

`v003_lightframe` is a full 18-icon alternate set for HUD readability testing.
It is not an automatic replacement for the accepted v001/v002 candidate set.
The comparison board is:

`icons/thecat_ui_starter_skill_icon_motifs_batch80_lightframe_comparison_board_v001.png`

## Recommended Set

The recommended set selects 4 v003 lightframe icons and keeps 14 current
v001/v002 icons. It is stored under:

`recommended/`

This is still candidate-only and must not be imported into Unity without the
runtime HUD/cooldown/import gates.

## Battle HUD Overlay Test

The HUD overlay test is stored under:

`hud_overlay_test/`

It is evidence for readability only. It does not prove Unity import, scene
binding, prefab use, input timing, or Console cleanliness.
