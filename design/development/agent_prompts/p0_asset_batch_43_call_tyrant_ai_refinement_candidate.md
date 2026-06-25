# P0 Asset Batch 43 - Call Tyrant AI Refinement Candidate

Date: 2026-06-15

## Task Scope

Use Codex built-in image generation to produce one Call Tyrant Boss AI
refinement candidate from the source concept reference, then standardize it
into project candidate files. This is a candidate-only batch. Do not import the
result into Unity and do not replace runtime Boss sprites, warning VFX,
framesheets, prefabs, scenes, or manifest counts.

This is a one-enemy-at-a-time Call Tyrant task. Do not produce Black Mud, Cold
Light, starter-cat, UI, or additional enemy bitmap candidates in this batch.

## Required Design Sources

- `design/姊﹀鏀厤鑰呮牳蹇冪帺娉?docs/00_overview/p0_minimum_design.md`
- `design/姊﹀鏀厤鑰呮牳蹇冪帺娉?docs/02_combat_and_systems/core_numeric_system_v1.md`
- `design/姊﹀鏀厤鑰呮牳蹇冪帺娉?docs/04_art_production/p0_digital_asset_inventory.md`
- `design/姊﹀鏀厤鑰呮牳蹇冪帺娉?assets/enemies/en06_call_tyrant/concept/call_tyrant_concept.png`
- `design/姊﹀鏀厤鑰呮牳蹇冪帺娉?assets/enemies/en06_call_tyrant/animation/call_tyrant_animation.png`
- `design/development/asset_candidates/enemies/p0_core/batch_38_p0_enemy_source_reference_pack_2026-06-15/thecat_enemy_p0_core_batch38_source_reference_review_sheet.png`
- `design/development/asset_candidates/enemies/p0_core/batch_38_p0_enemy_source_reference_pack_2026-06-15/call_tyrant/thecat_enemy_call_tyrant_batch38_combat_sprite_reference_512_512x512_candidate_v001.png`

## Project Files To Read

- `design/development/asset_review/p0_codex_to_unity_asset_pipeline_2026-06-14.md`
- `design/development/P0_ART_DIRECTION_AND_ASSET_PIPELINE.md`
- `design/development/prompts/p0_boss_assets.md`
- `design/development/tools/build_p0_enemy_source_reference_pack.py`
- `design/development/tools/validate_p0_enemy_source_reference_pack.ps1`
- `Assets/TheCat/Scripts/Runtime/Tools/P0HardReferenceSourceLocks.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0AssetManifestCatalog.cs`
- `Assets/TheCat/Scripts/Runtime/Data/Catalogs/P0VisualAssetCatalog.cs`

## Expected Output

- One raw project copy of the built-in generated image under
  `design/development/asset_candidates/enemies/call_tyrant/batch_43_call_tyrant_ai_refinement_candidate_2026-06-15`.
- One standardized `1024x1024` Call Tyrant combat candidate.
- One `512x512` preview candidate.
- One manifest, prompt record, review note, summary, and review sheet.
- All outputs stay outside `Assets`.
- Formal import remains blocked until active-enemy Play Mode screenshot review
  passes.

## Output Policy

- Candidate files must stay under
  `design/development/asset_candidates/enemies/call_tyrant`.
- Keep all candidate PNGs outside `Assets`.
- Do not create Unity `.meta` files for candidate outputs.
- Do not modify `Assets/TheCat/Art/Enemies/Sprites`.
- Do not modify source concept, source animation, or source-lock hashes.
- Do not modify prefabs, scenes, runtime visual bindings, runtime manifests, or
  formal import state.

## Call Tyrant Lock Rules

Call Tyrant must preserve giant phone shell, red call-eye signal, purple tie,
black mud body and base, app projectile language, summon portal/minion
vibration feel, Boss-scale silhouette, cracked glass screen, and phone-call
nightmare identity.

Reject human office boss body, generic smartphone icon mascot, cute robot
styling, clean ordinary phone, brand logos, readable text, keyboard, laptop,
alarm/lamp/toy motifs, black mud removal, missing purple tie, missing red call
eyes, missing cracked phone shell, or missing app projectile language.

## Built-In Image Generation Prompt

```text
Use case: stylized-concept
Asset type: game Boss combat sprite candidate for TheCat P0 enemy Call Tyrant
Primary request: Create one polished full-body combat sprite candidate for Call Tyrant, strictly preserving the provided reference image identity.
Input images: the visible Call Tyrant concept image is the hard visual authority.
Subject: corrupted ringing-phone nightmare Boss, giant cracked black smartphone shell as the main body, two red glowing phone-call icon eyes on the screen, black mud dripping from the phone edges and body, glossy black mud base pooling around the bottom, purple necktie wrapped across the phone body with a tied knot and hanging tie tail, black mud arms or tendrils, tiny phone minions emerging from the mud, app projectile language suggested by colored square app icons orbiting or being thrown near one side, summon and throw pressure read.
Style/medium: hand-painted dream-fantasy 2D game Boss sprite, readable combat silhouette for a top/angled camera, soft ink linework, painterly cracked glass, glossy mud, and cloth texture, consistent with the provided reference.
Composition/framing: centered single Boss enemy, square 1024x1024, three-quarter front view, full body including mud base, phone shell, arms/tendrils, purple tie, and small thrown app projectiles, generous padding, neutral warm parchment background, no UI frame.
Color palette: lock to black/dark graphite phone shell, glossy black mud, saturated red phone-call eyes, deep purple tie, small cyan/red/green/yellow app projectile accents, neutral parchment ground.
Constraints: preserve giant phone shell, red call-eye signal, purple tie, black mud body and base, app projectile language, summon portal/minion vibration feel, Boss-scale silhouette, cracked glass screen, and phone-call nightmare identity. Keep it clearly the same Boss as the reference, not a redesign.
Avoid: human office boss body, generic smartphone icon mascot, cute robot, clean ordinary phone, brand logos, readable text, keyboard, laptop, alarm/lamp/toy motifs, black mud removed, missing purple tie, missing red call eyes, missing cracked phone shell, missing app projectile language, gore, realistic horror anatomy, watermark, UI frame, heavy scene background.
```

## Validation

```powershell
& 'C:\Users\PC\.cache\codex-runtimes\codex-primary-runtime\dependencies\python\python.exe' design/development/tools/build_call_tyrant_ai_refinement_candidate.py <generated_image_path>
powershell -NoProfile -ExecutionPolicy Bypass -File design/development/tools/validate_call_tyrant_ai_refinement_candidate.ps1
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.Runtime.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' .\TheCat.EditModeTests.csproj /t:Build /p:Configuration=Debug /p:UseSharedCompilation=false /m:1 /nr:false /nologo
git diff --check
```

Unity validation remains pending until MCP/editor execution is available:

1. Refresh AssetDatabase only after a separate import batch exists.
2. Capture `09-active-enemy-call-tyrant.png`.
3. Compare runtime Boss scale, silhouette, summon readability, throw warning
   readability, app projectile read, and hitbox readability against Batch 38
   and this Batch 43 review sheet.
4. Check Unity Console, Sprite import settings, prefab references, scene
   bindings, and Boss warning VFX separation before approving any runtime
   installation.
