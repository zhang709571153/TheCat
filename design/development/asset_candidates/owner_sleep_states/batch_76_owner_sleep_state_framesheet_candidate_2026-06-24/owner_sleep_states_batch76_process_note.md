# Owner Sleep States Batch 76 Process Note

Process: built-in image_gen generation, workspace source copy, local chroma-key alpha removal with the imagegen helper, deterministic padded frame splitting, contact sheet creation, manifest generation, and candidate review.

Generation prompt summary:

- 4-row by 6-column sprite sheet for the owner sleep-state overlay.
- Row 1: deep sleep idle.
- Row 2: half dream / half awake.
- Row 3: nearly awake warning.
- Row 4: wake / failure transition.
- Flat `#00ff00` chroma-key background, no cats, no text, no starter-cat motifs.
- V002 revision request added owner/pillow/blanket overlay framing, cell-safety margins, alarm/light vibration, dream cracks, and consciousness-orb return cues.

Frame normalization:

- Each raw 256x256 sheet cell is trimmed by 20px to remove neighbor-cell slivers, scaled to 216x216, centered on a transparent 256x256 canvas, then cleared along a 28px outer edge guard.
- This preserves row/column timing while adding review margins around all candidate frame outputs.

Chroma-key result:

- Key color sampled by helper: `#03f902`.
- Transparent pixels: 790092 / 1572864.
- Partially transparent pixels: 34929 / 1572864.

Manifest rows: 24.

No Unity import was performed.
