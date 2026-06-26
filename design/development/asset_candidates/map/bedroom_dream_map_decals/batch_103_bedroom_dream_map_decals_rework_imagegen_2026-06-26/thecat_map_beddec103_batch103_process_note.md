# Batch 103 Bedroom Dream Map Decal Rework Process Note

Date: 2026-06-26

Process: built-in Codex `imagegen` source generation, workspace source copy, local magenta chroma-key alpha removal with the imagegen helper, deterministic 2x2 projection split with shortened filenames for Windows/Unity path safety, contact sheet generation, 64px bedroom-map readability board, and candidate review CSV.

Source truth: `Qr1 UI/style authority revision 816`; Qr1 P0 bedroom dream map requirement; Batch102 agent review reject/rework findings for items 04, 05, 08, and 09.

Prompt goal: rework the four Batch102 rejected concepts as static map/UI environment sprites only: a floor-hugging pillow barricade, a soft toy-block obstacle marker, a nightmare puddle decal, and a thin bed aura floor glow. The batch is restricted to map decals/soft obstacle markers: no characters, no cat bodies, no portraits, no enemy bodies, no framesheets, no animation frames, no text, and no runtime replacement.

Boundary note: Batch103 does not use or claim IAd character body approval or HDo/FoW9 map archive coverage; it is a static Qr1/Batch102 bedroom dream map decal rework candidate only.

Source and alpha:

- Built-in imagegen source copied from `C:\Users\PC\.codex\generated_images\019efc23-3cc2-7f23-9beb-cb16d118a13f\ig_01c74782702cf2db016a3d5451c14c819bb94fc993ea05b214.png`.
- Workspace source: `source/thecat_map_bedroom_dream_map_decals_rework_batch103_chromakey_source_v001.png`
- Alpha sheet: `source/thecat_map_bedroom_dream_map_decals_rework_batch103_alpha_sheet_v001.png`
- Chroma-key helper sampled key color `#fb02ee`; transparent pixels `1246931/1572516`; partially transparent pixels `210393/1572516`.

Cutting and rework notes:

- The first split attempt used longer semantic names and hit Windows path-length risk after writing one sprite. That partial sprite was moved to `superseded/failed_longname_pillow_barricade_batch103_candidate_v001.png`.
- The accepted split uses shorter semantic filenames from `source/thecat_map_beddec_rework_batch103_names.txt`.
- Manifest rows: 4.
- Final review decisions: 1 `candidate_keep`, 3 `candidate_conditional`, 0 `reject_rework`.

No runtime import was performed. Batch103 stays candidate-only until Unity import settings, bedroom map screenshots, accepted decal contrast, soft obstacle scale proof, aura warm-floor contrast, explicit target paths, binding proof, and clean Console evidence pass.
