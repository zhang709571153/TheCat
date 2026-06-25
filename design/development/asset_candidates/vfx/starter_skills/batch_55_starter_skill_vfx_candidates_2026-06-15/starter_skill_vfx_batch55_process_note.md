# Starter Skill VFX Batch 55 Process Note

Process: built-in image_gen output, workspace copy, local chroma-key alpha removal, deterministic review-pack generation.

Final prompts used a flat `#00ff00` chroma-key background and requested isolated symbolic VFX only: no cat body, no human body, no bed, no enemies, no text, and no watermark.

## Prompt Set

- Saiban: isolated oath bedline defense VFX with silver-blue shield barrier, round sun-shield sigil, knockback wave strokes, and small gold sword-light slash.
- Nephthys: isolated moon-sand control VFX with dark blue obelisk/pyramid sigil, quicksand field, sand-gold spiral grains, moon-teal slow rings, and royal eye mark.
- Suzune: isolated lullaby healing/sleep-stable VFX with moon-blue healing field, vermilion torii light silhouette, gold kagura bells, paper talismans, crescent moon notes, and outward push rings.

## Chroma-Key Removal

Tool:

```powershell
C:\Users\PC\.cache\codex-runtimes\codex-primary-runtime\dependencies\python\python.exe C:\Users\PC\.codex\skills\.system\imagegen\scripts\remove_chroma_key.py --auto-key border --soft-matte --transparent-threshold 18 --opaque-threshold 200 --edge-contract 1 --despill
```

Observed helper summaries:

- Saiban key `#0aee17`, transparent pixels `1130451/1572516`, partial pixels `87491/1572516`.
- Nephthys key `#04f016`, transparent pixels `999218/1572516`, partial pixels `100017/1572516`.
- Suzune key `#09f012`, transparent pixels `773159/1572516`, partial pixels `156096/1572516`.

Rows: 21

No Unity import was performed.
