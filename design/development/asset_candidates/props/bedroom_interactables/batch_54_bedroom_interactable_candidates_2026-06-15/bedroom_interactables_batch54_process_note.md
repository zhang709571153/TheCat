# Bedroom Interactables Batch 54 Process Note

Process: built-in image_gen prop generation, workspace copy, local chroma-key alpha removal, 1024 normalization, deterministic review-pack generation.

Prompt goal: keep the Bedroom Dream source identity for bed, litter box, and feeder while producing cleaner isolated interactable candidates.

Chroma-key helper results:

- Bed v001: key color auto-sampled near `#0ee414`; initial helper output reported 981151 transparent pixels and 3360 partially transparent pixels out of 1572516.
- Litter box v001: rejected for visible green glow after chroma-key removal.
- Litter box v002: key color `#ff00ff`; helper output reported 958973 transparent pixels and 30467 partially transparent pixels out of 1572516.
- Feeder v001: key color auto-sampled near `#12f411`; helper output reported 963452 transparent pixels and 3027 partially transparent pixels out of 1572516.

Rows: 21

No Unity import was performed.
