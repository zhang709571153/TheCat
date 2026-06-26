# Batch123 Main Menu Scene Backplates Process Note

Batch: `batch_123_main_menu_scene_backplates_2026-06-26`
Family: `ui`
Source truth: `Qr1XdXd6KosnjMxjgW7cS89kn9c revision 816; ui_main_menu and Batch106 context; no IAd character-body claim`

Process: built-in Codex `imagegen` source generation, workspace source copy, local magenta chroma-key alpha removal, deterministic 3x3 projection split, review-variant generation, and local candidate-pack validation. The current built-in image tool does not expose a selectable model parameter in this environment, so the requested imagegen/image2 constraint is recorded as intent, while the artifact is only claimed as built-in Codex imagegen output.

Prompt goal: produce nine static, textless, non-character main-menu scene/backplate UI sprites using Qr1-like dark dreamglass, warm gold bevels, teal glow, and small rose accents. The batch must not include character bodies, faces, portraits, paws, cat silhouettes, costumes, animation frames, enemies, or map-archive-specific landmarks.

Key source files:

- Source sheet: `source/thecat_ui_main_menu_scene_backplates_batch123_source_sheet_v001.png`
- Source sheet SHA256: `ABA5D1D2E02CFC1942180180EF882612E3B5A0C2223E16AADC259F397FC939BA`
- Alpha sheet: `alpha/thecat_ui_main_menu_scene_backplates_batch123_alpha_sheet_v001.png`
- Alpha sheet SHA256: `9D294426E0A54B05C3F3AAA3734D72EF3B621E998913ABAC7880D07F9F1246C5`
- Manifest: `tc_ui_menu_backplate_batch123_semantic_manifest.csv`
- Manifest SHA256: `39D38DE17C8201E41E8F31AA1748F550966A061969D01F32CF3936E9647DC83A`
- Contact sheet: `tc_ui_menu_backplate_batch123_semantic_contact_sheet_v001.png`
- Contact sheet SHA256: `B5BBA2145985355A7D664317586CE9B43D9BF390E574D97C5CD33E304694D8C4`
- Readability board: `tc_ui_menu_backplate_batch123_192x108_96x64_readability_board_v001.png`
- Readability board SHA256: `F8C9B02DC4CE6B68849876580C2775337411A61FBABDE699B529DD39C7E8AE77`

Chroma-key and cut results:

- Key color: `#f802f7`
- Transparent pixels: `967488/1572516`
- Partially transparent pixels: `23519/1572516`
- Split method: 3x3 projection-assisted grid split.
- Row bands: `(42,406)`, `(450,839)`, `(894,1200)`.
- Column bands: `(22,426)`, `(434,816)`, `(819,1226)`.
- Semantic sprites: 9 PNGs in `semantic_sprites/`.
- Review variants: 45 PNGs plus 9 review-variant manifests in `reviews/variants/`.

Checklist:

- [x] Asset table written before generation.
- [x] Raw source art copied into `source/`.
- [x] Background removed locally and alpha candidates stored outside runtime folders.
- [x] Manifest/contact sheet and target-size readability board produced.
- [x] Review variants produced.
- [x] Generic candidate-pack validator passed.
- [ ] Independent visual/source-boundary review completed.
- [ ] Independent target-size readability review completed.
- [ ] Independent production QA review completed.
- [ ] Runtime import remains blocked until import settings, binding proof, screenshots, and clean console evidence pass.

Local validation:

- Generic candidate-pack validator: passed with 9 manifest rows and 9 current PNG sprites after manifest path normalization to `design/development/...`.

No runtime import was performed.
